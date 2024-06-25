using Microsoft.EntityFrameworkCore;
using CoreDashboard.Models;
using CoreDashboard.Models.Extras;
using static CoreDashboard.Services.DataUploadingSevice.DataGetters;
using System.Diagnostics;
using System.Threading;

namespace CoreDashboard.Services.DataUploadingSevice
{
	public class DataUploader(ApplicationContext context)
	{
		private readonly char[] _separators = ['\r', '\n'];
		private readonly char _splitChar = ';';
		private readonly ApplicationContext _context = context;

		public async Task<string> ConvertTextToEducationalRecords(string inputText, string uploadingDbName, CancellationToken cancellationToken, int userId)
		{
			string[] lines = inputText.Split(_separators, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
			string[] firstDataRow = lines[1].Split(_splitChar);
			string disciplineName = firstDataRow[0];

			if (await IsUploadedDbWithNameExistsAsync(uploadingDbName, cancellationToken))
				return "База данных с таким названием уже существует";

			Discipline discipline = await AddDisciplineIfNotExistsAsync(disciplineName, cancellationToken);

			var uploadingDb = new UploadedDb
			{
				UploadedDbName = uploadingDbName,
				UploadDate = DateTime.Now,
				DisciplineId = discipline.DisciplineId,
				UserId = userId
			};

			await _context.UploadedDbs.AddAsync(uploadingDb, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			List<EducationalRecord> records = [];

			foreach (var line in lines)
				records.Add(new EducationalRecord(line, _splitChar));

			records = ProcessPresence(records);
			List<bool> controlPoints = GetControlPoints(records);

			var data = records.Zip(controlPoints, (x, y) => (x, y));

			///TODO: determine the similarity measure of the dataset

			Stopwatch sw = Stopwatch.StartNew();

			DataExistanceVerifier verifier = new(_context, records, uploadingDb);
			await verifier.Verify(cancellationToken);

			sw.Stop();
			await Console.Out.WriteLineAsync($"Verifying time: {sw.Elapsed.TotalMilliseconds} ms");
			sw.Restart();

			var uploadedDbResults = verifier.UploadedDbResults;

			foreach (var (record, isControlPoint) in data)
			{
				UploadedDbResult uploadedDbResult = GetDbResultByRecord(uploadedDbResults, record, uploadingDb);
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
		}

		public UploadedDbResult GetDbResultByRecord(IEnumerable<UploadedDbResult> uploadedDbResults, EducationalRecord record, UploadedDb uploadingDb)
		{
			return uploadedDbResults
				.First(
					dbResult => dbResult.Student!.StudentName == record.StudentName &&
					dbResult.StudyDirection!.StudyDirectionName == record.StudyDirectionName &&
					dbResult.StudyGroup!.StudyGroupName == record.GroupName &&
					dbResult.TotalScore == record.TotalScore &&
					dbResult.UploadedDbId == uploadingDb.UploadedDbId
				);
		}

		public UploadedDbRecord CreateDbRecord(EducationalRecord record, UploadedDbResult uploadedDbResult, bool isControlPoint)
		{
			return new()
			{
				ThemeScore = record.ControlPoint is null && record.PairScore is null? null : record.ControlPoint ?? 0 + record.PairScore ?? 0,
				IsControlPoint = isControlPoint,
				Presence = record.Presence,
				UploadedDbResultId = uploadedDbResult.UploadedDbResultId,
				PairThemeId = _context.PairThemes.First(theme => theme.PairThemeName == record.PairThemeName).PairThemeId
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
	}
}
