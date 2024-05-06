using CoreDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CoreDashboard.Controllers
{
    public class HomeController(ApplicationContext context) : Controller
    {
		private readonly ApplicationContext _context = context;

		public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public async Task<string> AddFile(IFormFile uploadedFile)
		{
			if (uploadedFile == null || uploadedFile.Length == 0)
				return "File not selected or empty.";

			// Получение расширения файла
			var fileExtension = Path.GetExtension(uploadedFile.FileName);

			if (fileExtension is not ".csv")
				return "File extension is not supported";


			var filePath = Path.GetTempFileName();

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await uploadedFile.CopyToAsync(stream);
			}

			string fileContent;
			using (var reader = new StreamReader(filePath))
			{
				fileContent = await reader.ReadToEndAsync();
			}

			//await _context.AddRangeAsync(ConvertTextToEducationalRecords(fileContent));
			await _context.SaveChangesAsync();
			return fileContent;
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
