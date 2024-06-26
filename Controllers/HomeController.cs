using CoreDashboard.Models;
using CoreDashboard.Services.DataUploadingSevice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

namespace CoreDashboard.Controllers
{
	[Authorize]
    public class HomeController(ApplicationContext context) : Controller
    {
		private readonly ApplicationContext _context = context;

		public IActionResult Index()
        {
			TempData.TryGetValue("Notification", out object? obj);
			ViewBag.Notification = obj is null ? null : obj as string;
			

			IEnumerable<UploadedDb> data = _context.UploadedDbs
				.Include(c => c.Discipline)
				.Include(c => c.User);
			return View(data);
        }

		[HttpPost]
		public async Task<IActionResult> AddFile(string uploadingDbName, IFormFile uploadedFile)
		{
			if (uploadedFile == null || uploadedFile.Length == 0)
			{
				ViewBag.Notificaion = "���� �� ������ ��� ����";
				return View();
			}

			// ��������� ���������� �����
			var fileExtension = Path.GetExtension(uploadedFile.FileName);

			if (fileExtension is not ".csv")
			{
				ViewBag.Notificaion = "���������� ����� �� ��������������. ��������� ���� � ����������� csv.";
				return View();
			}

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

			var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            int userId = _context.Users.First(u => u.UserEmail == email).UserId;

			string message = await dataUploader.ConvertTextToEducationalRecords(fileContent, uploadingDbName, cancellationToken, userId);

			TempData.Add("Notification", message);

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => 
			View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
