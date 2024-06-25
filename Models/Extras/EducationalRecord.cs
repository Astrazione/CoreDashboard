using System.Globalization;

namespace CoreDashboard.Models.Extras
{
	public class EducationalRecord()
	{
		public string DisciplineName { get; set; } = null!;
		public string StudentName { get; set; } = null!;
		public string GroupName { get; set; } = null!;
		public string PairThemeName { get; set; } = null!;
		public decimal? PairScore { get; set; }
		public bool? Presence { get; set; }
		public decimal? ControlPoint { get; set; }
		public decimal TotalScore { get; set; }
		public string Rating { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string StudyDirectionName { get; set; } = null!;
		public string TeacherName { get; set; } = null!;

		public EducationalRecord(string line, char splitChar) : this()
		{
			var values = line.Split(splitChar);
			DisciplineName = values[0];
			StudentName = values[2];
			GroupName = values[3];
			PairThemeName = values[4];
			PairScore = string.IsNullOrEmpty(values[5]) ? null : decimal.Parse(values[5].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
			Presence = string.IsNullOrEmpty(values[6])
				? null
				: values[6].Equals("П", StringComparison.InvariantCultureIgnoreCase);
			ControlPoint = string.IsNullOrEmpty(values[7]) ? null : decimal.Parse(values[7].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
			TotalScore = decimal.Parse(values[8].Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
			Rating = values[9];
			Email = values[10];
			StudyDirectionName = values[11];
			TeacherName = values[12];
		}
	}
}
