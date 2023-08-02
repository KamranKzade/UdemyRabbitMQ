using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using UdemyRabbitMQWeb.ExcelCreate.Hubs;
using UdemyRabbitMQWeb.ExcelCreate.Models;

namespace UdemyRabbitMQWeb.ExcelCreate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<MyHub> _hubContext;

        public FilesController(AppDbContext context, IHubContext<MyHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int fileId)
        {
            if (file is not { Length: > 0 }) return BadRequest();


            var userFile = await _context.UserFiles.FirstOrDefaultAsync(x => x.Id == fileId);
            var filePath = userFile.FileName + Path.GetExtension(file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", filePath);


            using FileStream stream = new(path, FileMode.Create);
            await file.CopyToAsync(stream);

            userFile.CreatedDate = DateTime.Now;
            userFile.FilePath = filePath;
            userFile.FileStatus = FileStatus.Completed;
            await _context.SaveChangesAsync();


            // SignalR notification olusturulacak
            await _hubContext.Clients.User(userFile.UserId).SendAsync("CompletedFile");

            return Ok();
        }
    }
}
