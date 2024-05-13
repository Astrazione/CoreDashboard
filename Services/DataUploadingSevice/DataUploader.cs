using Microsoft.EntityFrameworkCore;
using CoreDashboard.Models;
using CoreDashboard.Models.Extras;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Diagnostics;

namespace CoreDashboard.Services.DataUploadingSevice
{
	public class DataUploader(ApplicationContext context)
	{
		private readonly char[] _separators = ['\r', '\n'];
		private readonly char _splitChar = ';';
		private readonly ApplicationContext _context = context;

		public async Task<string> ConvertTextToEducationalRecords(string inputText, string uploadingDbName, CancellationToken cancellationToken)
		{
			string[] lines = inputText.Split(_separators, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
			//try
			//{
			string[] firstDataRow = lines[1].Split(_splitChar);
			string disciplineName = firstDataRow[0];

			if (await IsUploadedDbWithNameExistsAsync(uploadingDbName, cancellationToken))
				return "база данных с таким названием уже существует";

			Discipline discipline = await AddDisciplineIfNotExistsAsync(disciplineName, cancellationToken);

			var uploadingDb = new UploadedDb
			{
				UploadedDbName = uploadingDbName,
				UploadDate = DateTime.Now,
				DisciplineId = discipline.DisciplineId,
				UserId = 1 //(await _context.Users.FirstAsync(cancellationToken)).UserId,
			};

			await _context.UploadedDbs.AddAsync(uploadingDb, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			List<EducationalRecord> records = [];

			foreach (var line in lines)
				records.Add(new EducationalRecord(line, _splitChar));

			records = ProcessPresence(records);
			List<bool> controlPoints = GetControlPoints(records);

			var data = records.Zip(controlPoints, (x, y) => (x, y));

			///TODO: compute hash
			///TODO: determine the similarity measure of the dataset

			Stopwatch sw = Stopwatch.StartNew();

			DataExistanceVerifier verifier = new(_context, records);
			verifier.Verify();

			sw.Stop();
			await Console.Out.WriteLineAsync($"Verifying time: {sw.Elapsed.TotalMilliseconds} ms");
			sw.Restart();

			foreach (var (record, isControlPoint) in data)
			{
				UploadedDbResult uploadedDbResult = await GetOrCreateDbResultAsync(record, uploadingDb, cancellationToken);
				UploadedDbRecord uploadedDbRecord = CreateDbRecord(record, uploadedDbResult, isControlPoint);
				await _context.AddAsync(uploadedDbRecord, cancellationToken);
			}

			sw.Stop();
			await Console.Out.WriteLineAsync($"Inserting time: {sw.Elapsed.TotalMilliseconds} ms");
			sw.Restart();

			await _context.SaveChangesAsync(cancellationToken);

			sw.Stop();
			await Console.Out.WriteLineAsync($"Saving time: {sw.Elapsed.TotalMilliseconds} ms");
			return "База данных успешно загружена";
			//}
			//catch (Exception e)
			//{
			//	return $"Во время загрузки базы данных произошла ошибка: {e.Message}\n" +
			//		$"Inner exception: {e.InnerException}";
			//}
		}

		public async Task<UploadedDbResult> GetOrCreateDbResultAsync(EducationalRecord record, UploadedDb uploadingDb, CancellationToken cancellationToken)
		{
			long studentId = GetExistingStudentId(record);

			var uploadedDbResult = await _context.UploadedDbResults
				.Include(dbResult => dbResult.StudyGroup)
				.Include(dbResult => dbResult.StudyDirection)
			.FirstOrDefaultAsync(
				dbResult => dbResult.StudentId == studentId &&
				dbResult.StudyDirection!.StudyDirectionName == record.StudyDirectionName &&
				dbResult.StudyGroup!.StudyGroupName == record.GroupName &&
				dbResult.TotalScore == record.TotalScore &&
				dbResult.UploadedDbId == uploadingDb.UploadedDbId,
				cancellationToken);

			if (uploadedDbResult is null)
			{
				uploadedDbResult = new()
				{
					UploadedDbId = uploadingDb.UploadedDbId,
					StudentId = studentId,
					StudyDirectionId = (await _context.StudyDirections.FirstAsync(sd => sd.StudyDirectionName == record.StudyDirectionName, cancellationToken)).StudyDirectionId,
					StudyGroupId = (await _context.StudyGroups.FirstAsync(sd => sd.StudyGroupName == record.GroupName, cancellationToken)).StudyGroupId,
					TotalScore = record.TotalScore,
					Rating = record.Rating
				};
				await _context.UploadedDbResults.AddAsync(uploadedDbResult, cancellationToken);
				await _context.SaveChangesAsync(cancellationToken);
			}

			return uploadedDbResult;
		}

		public UploadedDbRecord CreateDbRecord(EducationalRecord record, UploadedDbResult uploadedDbResult, bool isControlPoint)
		{
			return new()
			{
				ThemeScore = record.ControlPoint is null && record.PairScore is null? null : record.ControlPoint ?? 0 + record.PairScore ?? 0,
				IsControlPoint = isControlPoint,
				Presence = record.Presence,
				UploadedDbResultId = uploadedDbResult.UploadedDbResultId,
				PairThemeId = _context.PairThemes.First(theme => theme.PairThemeName == record.PairThemeName).PairThemeId,
				Hash = ""
			};
		}

		public List<EducationalRecord> ProcessPresence(IEnumerable<EducationalRecord> educationalRecords)
		{
			var groupedPresenceDict = educationalRecords
				.GroupBy(er => new { er.GroupName, er.PairThemeName })
				.ToDictionary(group => group.Key, group => group.All(er => er.Presence is null));

			List<EducationalRecord> processedEducationalRecords = educationalRecords.Select(er =>
			{
				if (er.Presence is null && !groupedPresenceDict[new { er.GroupName, er.PairThemeName }])
					er.Presence = false;
				return er;
			}).ToList();

			List<bool> controlPoint = GetControlPoints(processedEducationalRecords);

			return processedEducationalRecords;
		}

		public List<bool> GetControlPoints(IEnumerable<EducationalRecord> educationalRecords)
		{
			List<bool> controlPoints = [];
			var groupedControlPointsDict = educationalRecords
				.GroupBy(er => new { er.GroupName, er.PairThemeName })
				.ToDictionary(group => group.Key, group => group.Any(er => er.ControlPoint is not null));

			foreach (var er in educationalRecords)
				controlPoints.Add(groupedControlPointsDict[new { er.GroupName, er.PairThemeName }]);

			return controlPoints;
		}

		public async Task<bool> IsUploadedDbWithNameExistsAsync(string uploadedDbName, CancellationToken cancellationToken) =>
			await _context.UploadedDbs.AnyAsync(uploadedDb => uploadedDb.UploadedDbName == uploadedDbName, cancellationToken);

		public async Task<Discipline> AddDisciplineIfNotExistsAsync(string disciplineName, CancellationToken cancellationToken)
		{
			Discipline? discipline = await _context.Disciplines.FirstOrDefaultAsync(discipline => discipline.DisciplineName == disciplineName, cancellationToken);
			if (discipline is null)
			{
				discipline = new Discipline { DisciplineName = disciplineName };
				await _context.Disciplines.AddAsync(discipline, cancellationToken);
				await _context.SaveChangesAsync(cancellationToken);
			}
			return discipline;
		}

		public long GetExistingStudentId(EducationalRecord record)
		{
			if (string.IsNullOrEmpty(record.Email))
				return _context.Students
					.First(student => student.StudentId >= 10_000_000_000 && student.StudentName == record.StudentName).StudentId;

			string startString = "stud", endString = "@";
			int startIndex = record.Email.IndexOf(startString);
			int endIndex = record.Email.IndexOf(endString);

			if (startIndex != -1 && endIndex != -1)
			{
				bool result = long.TryParse(record.Email.Substring(startIndex + startString.Length, endIndex - startString.Length), out long id);
				if (result)
					return id;
				else
					return -1;
			}
			else
				return -1;
		}
	}
}
