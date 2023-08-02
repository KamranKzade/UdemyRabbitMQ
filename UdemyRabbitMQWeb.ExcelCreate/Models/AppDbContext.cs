using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace UdemyRabbitMQWeb.ExcelCreate.Models
{
	public class AppDbContext : IdentityDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<UserFile> UserFiles { get; set; }

	}
}
