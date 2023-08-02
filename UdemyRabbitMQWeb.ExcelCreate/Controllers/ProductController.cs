using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using UdemyRabbitMQWeb.ExcelCreate.Models;
using UdemyRabbitMQWeb.ExcelCreate.Services;


namespace UdemyRabbitMQWeb.ExcelCreate.Controllers
{
	public class ProductController : Controller
	{
		private readonly AppDbContext _appDbContext;
		private readonly RabbitMQPublisher _rabbitMQPublisher;
		private readonly UserManager<IdentityUser> _userManager;

		public ProductController(AppDbContext appDbContext, UserManager<IdentityUser> userManager, RabbitMQPublisher rabbitMQPublisher)
		{
			_appDbContext = appDbContext;
			_userManager = userManager;
			_rabbitMQPublisher = rabbitMQPublisher;
		}


		[Authorize]
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> CreateProductExcel()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);

			var fileName = $"product-excel-{Guid.NewGuid().ToString().Substring(1, 10)}";

			UserFile userfile = new()
			{
				UserId = user.Id,
				FileName = fileName,
				FileStatus = FileStatus.Creating
			};

			await _appDbContext.UserFiles.AddAsync(userfile);
			await _appDbContext.SaveChangesAsync();

			// RabbitMQ-e mesaj gonder
			_rabbitMQPublisher.Publish(new Shared.CreateExcelMessage()
			{
				FileId = userfile.Id,
			});
			TempData["StartCreatingExcel"] = true;

			return RedirectToAction(nameof(Files));
		}

		public async Task<IActionResult> Files()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);

			return View(await _appDbContext.UserFiles.Where(x => x.UserId == user.Id).OrderByDescending(x => x.Id).ToListAsync());
		}
	}
}
