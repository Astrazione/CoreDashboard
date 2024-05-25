using CoreDashboard.Extensions;
using CoreDashboard.Models;
using CoreDashboard.Models.Extras;

namespace CoreDashboard.Services.DataUploadingSevice
{
    public class DataExistanceVerifier
	{
		readonly ApplicationContext _context;
		readonly IEnumerable<EducationalRecord> _records;

		public DataExistanceVerifier(ApplicationContext context, IEnumerable<EducationalRecord> records) => 
			(_context, _records) = (context, records);

		public void Verify()
		{
			VerifyStudents();
			VerifyPairThemes();
			VerifyStudyDirections();
			VerifyTeachers();
			VerifyStudyGroups();
			_context.SaveChanges();
		}

		void VerifyStudents()
		{
			
			var students = _records.Select(r => new { r.StudentName, r.Email }).Distinct().ToList();
			foreach (var student in students)
			{
				long studentId = GetStudentIdFromEmail(student.Email);

				if (studentId == -1)
					VerifyStudentWithoutId(student.StudentName, student.Email);
				else
					VerifyStudent(studentId, student.StudentName, student.Email);
			}
		}

		void VerifyPairThemes()
		{
			var pairThemeNames = _records.Select(r => r.PairThemeName).Distinct().ToList();
			pairThemeNames.ForEach(themeName => _context.PairThemes.AddIfNotExists(new PairTheme { PairThemeName = themeName, PairTypeId = themeName[themeName.IndexOf(' ') + 1] == 'Л'? 1 : 2 }, x => x.PairThemeName == themeName));
			_context.SaveChanges();
		}

		void VerifyStudyDirections()
		{
			var studyDirections = _records.Select(r => r.StudyDirectionName).Distinct().ToList();
			studyDirections.ForEach(sdName => _context.StudyDirections.AddIfNotExists(new StudyDirection { StudyDirectionName = sdName }, x => x.StudyDirectionName == sdName));
			_context.SaveChanges();
		}

		void VerifyStudyGroups()
		{
			var studyGroups = _records.Select(r => new { r.GroupName, r.TeacherName }).Distinct().ToList();
			studyGroups.ForEach(group => _context.StudyGroups.AddIfNotExists(
				new StudyGroup { 
					StudyGroupName = group.GroupName, 
					TeacherId = _context.Teachers.First(t => t.TeacherName == group.TeacherName).TeacherId }, x => x.StudyGroupName == group.GroupName));
			_context.SaveChanges();
		}

		void VerifyTeachers()
		{
			var teachers = _records.Select(r => r.TeacherName).Distinct().ToList();
			teachers.ForEach(teacher => _context.Teachers.AddIfNotExists(new Teacher { TeacherName = teacher }, x => x.TeacherName == teacher));
			_context.SaveChanges();
		}

		void VerifyStudentWithoutId(string name, string email)
		{
			long nextFreeId = _context.Students.Count() > 0 ? Math.Max(_context.Students.Max(x => x.StudentId), 10_000_000_000) + 1 : 10_000_000_000;
			_context.Students.AddIfNotExists(new() { StudentId = nextFreeId, StudentName = name, StudentEmail = email }, x => x.StudentName == name);
			_context.SaveChanges();
		}

		void VerifyStudent(long id, string name, string email) =>
			_context.Students.AddIfNotExists(new Student { StudentId = id, StudentName = name, StudentEmail = email }, x => x.StudentId == id);

		public static long GetStudentIdFromEmail(string? email)
		{
			if (string.IsNullOrEmpty(email))
				return -1;

			string startString = "stud", endString = "@";
			int startIndex = email.IndexOf(startString);
			int endIndex = email.IndexOf(endString);

			if (startIndex != -1 && endIndex != -1)
			{
				bool result = long.TryParse(email.Substring(startIndex + startString.Length, endIndex - startString.Length), out long id);
				if (result)
					return id;
				else return -1;
			}
			else
				return -1;
		}
	}
}
