using CoreDashboard.Models;
using CoreDashboard.Services.DataUploadingSevice;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace CoreDashboard.Controllers
{
    public class HomeController(ApplicationContext context) : Controller
    {
		private readonly ApplicationContext _context = context;

		public IActionResult Index()
        {
			IEnumerable<UploadedDb> data = _context.UploadedDbs.Include(c => c.Discipline);
			return View(data);
        }

		[HttpPost]
		public async Task<string> AddFile(string uploadingDbName, IFormFile uploadedFile)
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

			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			Encoding encoding = Encoding.GetEncoding("windows-1251");

			using (var reader = new StreamReader(filePath, encoding))
			{
				fileContent = await reader.ReadToEndAsync();
			}

			DataUploader dataUploader = new(_context);
			CancellationToken cancellationToken = new();
			string message = await dataUploader.ConvertTextToEducationalRecords(fileContent, uploadingDbName, cancellationToken);

			return message;
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
