using System;
using RabbitMQ.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using UdemyRabbitMQWeb.Watermark.Models;
using Microsoft.Extensions.Configuration;
using UdemyRabbitMQWeb.Watermark.Services;
using Microsoft.Extensions.DependencyInjection;
using UdemyRabbitMQWeb.Watermark.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var conn = builder.Configuration.GetConnectionString("RabbitMQ");
builder.Services.AddSingleton(sp => new ConnectionFactory()
{
	Uri = new Uri(conn),
	DispatchConsumersAsync = true
});
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddSingleton<RabbitMQClientService>();

// ImageWatermark -i projecte elave edirik
builder.Services.AddHostedService<ImageWatermarkProcessBackgroundService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseInMemoryDatabase(databaseName: "productDb");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();