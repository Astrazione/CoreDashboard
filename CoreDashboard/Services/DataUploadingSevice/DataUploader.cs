using Microsoft.EntityFrameworkCore;
using CoreDashboard.Models;

namespace CoreDashboard.Services.DataUploadingSevice
{
	public class DataUploader(ApplicationContext context)
	{
		private readonly char[] _separators = ['\r', '\n'];
		private readonly char _splitChar = ';';
		private readonly ApplicationContext _context = context;

		public async Task<string> ConvertTextToEducationalRecords(string inputText, string uploadedDbName, ApplicationContext dbContext, CancellationToken cancellationToken)
		{
			string[] records = inputText.Split(_separators, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
			try
			{
				string[] firstDataRow = records[1].Split(_splitChar);
				string disciplineName = firstDataRow[0];

				if (await IsUploadedDbWithNameExistsAsync(uploadedDbName, cancellationToken))
					return "база данных с таким названием уже существует";

				Discipline discipline = await AddDisciplineIfNotExistsAsync(disciplineName, cancellationToken);

				await _context.UploadedDbs.AddAsync(
					new UploadedDb { 
						UploadedDbName = uploadedDbName,
						UploadDate = DateTime.Now,
						DisciplineId = discipline.DisciplineId,
						UserId = (await _context.Users.FirstAsync(cancellationToken)).UserId,
					},
					cancellationToken
				);

				foreach (var record in records)
				{
					var values = record.Split(_splitChar);

					var (studentName, groupName, pairThemeName, rating, studentEmail, directionName, teacher) =
						(values[2], values[3], values[4], values[9], values[10], values[11], values[12]);
					bool presence = false;
					var themeScore = 0;

				}

				await _context.SaveChangesAsync(cancellationToken);


			}
			catch { }

            /*for (int i = 1; i < 1000; i++)
			{
				var values = records[i].Split(_splitChar);

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
			}*/

			return "";
		}

		public async Task<bool> IsUploadedDbWithNameExistsAsync(string uploadedDbName, CancellationToken cancellationToken) =>
			await _context.UploadedDbs.AnyAsync(uploadedDb => uploadedDb.UploadedDbName == uploadedDbName, cancellationToken);

		public async Task<Discipline> AddDisciplineIfNotExistsAsync(string disciplineName, CancellationToken cancellationToken)
		{
			Discipline? discipline = await _context.Disciplines.FirstOrDefaultAsync(discipline => discipline.DisciplineName == disciplineName, cancellationToken);
			discipline ??= new Discipline { DisciplineName = disciplineName };

			await _context.Disciplines.AddAsync(discipline, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
			return discipline;
		}
	}
}
