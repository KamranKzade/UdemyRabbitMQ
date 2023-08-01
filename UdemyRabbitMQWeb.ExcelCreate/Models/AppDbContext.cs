using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace UdemyRabbitMQWeb.ExcelCreate.Models
{
	public class AppDbContext : IdentityDbContext
	{

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<UserFile> UserFiles { get; set; }

	}
}
