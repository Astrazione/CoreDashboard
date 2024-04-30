using CoreDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CoreDashboard.Controllers
{
    public class HomeController(ApplicationContext context) : Controller
    {
		private readonly ApplicationContext _context = context;
		private static readonly char[] separator = [ '\r', '\n' ];

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

			await _context.AddRangeAsync(ConvertTextToEducationalRecords(fileContent));
			await _context.SaveChangesAsync();
			return fileContent;
		}

		public static IEnumerable<EducationalRecord> ConvertTextToEducationalRecords(string inputText)
		{
			List<EducationalRecord> educationalRecords = [];

			var lines = inputText.Split(separator, StringSplitOptions.RemoveEmptyEntries);

			for (int i = 1; i < lines.Length; i++)
			{
				var values = lines[i].Split(';');

				educationalRecords.Add(new EducationalRecord
				{
					Student = values[0],
					DisciplineName = values[1],
					GroupName = values[2],
					TopicName = values[3],
					TopicScore = string.IsNullOrEmpty(values[4]) ? 0 : Convert.ToDecimal(values[4]),
					Presence = string.IsNullOrEmpty(values[5]) ? '-' : values[5][0],
					ControlPoint = string.IsNullOrEmpty(values[6]) ? 0 : Convert.ToDecimal(values[6]),
					TotalScore = Convert.ToDecimal(values[7]),
					Rating = values[8],
					StudyDirection = values[9],
					Teacher = values[10]
				});
			}

			return educationalRecords;
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
