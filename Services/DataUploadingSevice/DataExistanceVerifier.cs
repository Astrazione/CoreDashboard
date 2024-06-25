using CoreDashboard.Extensions;
using CoreDashboard.Migrations;
using CoreDashboard.Models;
using CoreDashboard.Models.Extras;
using Microsoft.EntityFrameworkCore;
using static CoreDashboard.Services.DataUploadingSevice.DataGetters;

namespace CoreDashboard.Services.DataUploadingSevice
{
    public class DataExistanceVerifier
	{
		readonly ApplicationContext _context;
		readonly IEnumerable<EducationalRecord> _records;
		readonly UploadedDb _uploadedDb;

		public List<UploadedDbResult> UploadedDbResults { get; } = [];

		public DataExistanceVerifier(ApplicationContext context, IEnumerable<EducationalRecord> records, UploadedDb uploadedDb) => 
			(_context, _records, _uploadedDb) = (context, records, uploadedDb);

		public async Task Verify(CancellationToken cancellationToken)
		{
			VerifyStudents();
			VerifyPairThemes();
			VerifyStudyDirections();
			VerifyTeachers();
			await _context.SaveChangesAsync(cancellationToken);
			VerifyStudyGroups();
			await _context.SaveChangesAsync(cancellationToken);

			await VerifyResults(cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
		}

		async Task VerifyResults(CancellationToken cancellationToken)
		{
			var uniqueResultsUnformated = _records.Select(r => new UploadedDbResult
			{
				Student = GetExistingStudent(r),
				UploadedDbId = _uploadedDb.UploadedDbId,
				StudyDirection = _context.StudyDirections.First(sd => sd.StudyDirectionName == r.StudyDirectionName),
				StudyGroup = _context.StudyGroups.First(sd => sd.StudyGroupName == r.GroupName),
				TotalScore = r.TotalScore,
				Rating = r.Rating
			})
				.Distinct()
				.ToList();

			foreach (var result in uniqueResultsUnformated)
			{
				var uploadedDbResult = await _context.UploadedDbResults
					.Include(r => r.StudyGroup)
					.Include(r => r.StudyDirection)
					.FirstOrDefaultAsync(
						dbResult => dbResult.StudentId == result.StudentId &&
						dbResult.StudyDirectionId == result.StudyDirection!.StudyDirectionId &&
						dbResult.StudyGroupId == result.StudyGroup!.StudyGroupId &&
						dbResult.TotalScore == result.TotalScore &&
						dbResult.UploadedDbId == result.UploadedDbId,
						cancellationToken);

				if (uploadedDbResult is null)
				{
					UploadedDbResults.Add(result);
					await _context.UploadedDbResults.AddAsync(result, cancellationToken);
				}
				else
					UploadedDbResults.Add(uploadedDbResult);
			}
		}

		void VerifyStudents()
		{
			var students = _records.Select(r => new { r.StudentName, r.Email })
				.Distinct()
				.Select(r => 
					new Student { 
						StudentName = r.StudentName, 
						StudentEmail = r.Email 
					})
				.ToList();

			foreach (var student in students)
			{
				long studentId = GetStudentIdFromEmail(student.StudentEmail);

				if (studentId == -1)
					studentId = VerifyStudentWithoutId(student.StudentName!, student.StudentEmail!);
				else
					VerifyStudent(studentId, student.StudentName!, student.StudentEmail!);
			}
		}

		void VerifyPairThemes()
		{
			var pairs = _records.Select(r => new { r.PairThemeName, r.GroupName })
				.Distinct()
				.Select(r => new PairTheme { PairThemeName = r.PairThemeName, PairTypeId = GetPairType(r.GroupName) })
				.ToList();

			foreach (var pair in pairs)
				_context.PairThemes.AddIfNotExists(pair, x => x.PairThemeName == pair.PairThemeName);

			_context.SaveChanges();
		}

		void VerifyStudyDirections()
		{
			var studyDirections = _records.Select(r => r.StudyDirectionName).Distinct().ToList();
			studyDirections.ForEach(sdName => _context.StudyDirections.AddIfNotExists(new StudyDirection { StudyDirectionName = sdName }, x => x.StudyDirectionName == sdName));
		}

		void VerifyStudyGroups()
		{
			var studyGroups = _records.Select(r => new { r.GroupName, r.TeacherName }).Distinct().ToList();
			studyGroups.ForEach(group => _context.StudyGroups.AddIfNotExists(
				new StudyGroup { 
					StudyGroupName = group.GroupName, 
					TeacherId = _context.Teachers.First(t => t.TeacherName == group.TeacherName).TeacherId 
				}, x => x.StudyGroupName == group.GroupName));
		}

		void VerifyTeachers()
		{
			var teachers = _records.Select(r => r.TeacherName).Distinct().ToList();
			teachers.ForEach(teacher => _context.Teachers.AddIfNotExists(new Teacher { TeacherName = teacher }, x => x.TeacherName == teacher));
		}

		long VerifyStudentWithoutId(string name, string email)
		{
			long nextFreeId = _context.Students.Any() ? Math.Max(_context.Students.Max(x => x.StudentId), 10_000_000_000) + 1 : 10_000_000_000;
			_context.Students.AddIfNotExists(new() { StudentId = nextFreeId, StudentName = name, StudentEmail = email }, x => x.StudentName == name);
			_context.SaveChanges();
			return nextFreeId;
		}

		void VerifyStudent(long id, string name, string email) =>
			_context.Students.AddIfNotExists(new Student { StudentId = id, StudentName = name, StudentEmail = email }, x => x.StudentId == id);

		public Student GetExistingStudent(EducationalRecord record)
		{
			if (string.IsNullOrEmpty(record.Email))
				return _context.Students
					.First(student => student.StudentId >= 10_000_000_000 && student.StudentName == record.StudentName);

			return _context.Students
					.First(student => student.StudentId == GetStudentIdFromEmail(record.Email));
		}
	}
}
