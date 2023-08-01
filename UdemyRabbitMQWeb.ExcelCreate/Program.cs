using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UdemyRabbitMQWeb.ExcelCreate.Models;
using Microsoft.Extensions.DependencyInjection;


namespace UdemyRabbitMQWeb.ExcelCreate
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();


			using (var scope = host.Services.CreateScope())
			{
				var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

				appDbContext.Database.Migrate();

				if (!appDbContext.Users.Any())
				{
					userManager.CreateAsync(new IdentityUser() { UserName = "deneme", Email = "deneme@outlook.com" }, "Password12*").Wait();
					userManager.CreateAsync(new IdentityUser() { UserName = "deneme2", Email = "deneme2@outlook.com" }, "Password12*").Wait();
				}
			}
			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
