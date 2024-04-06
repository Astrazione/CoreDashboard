using Microsoft.EntityFrameworkCore;

namespace CoreDashboard.Models
{
	[PrimaryKey("RecordId")]
	public class EducationalRecord
	{
		public int RecordId { get; set; }
		public string Student { get; set; } = null!;
		public string DisciplineName { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public string TopicName { get; set; } = null!;
        public decimal TopicScore { get; set; }
		public char Presence { get; set; }
		public decimal ControlPoint { get; set; }
		public decimal TotalScore { get; set; }
		public string Rating { get; set; } = null!;
        public string StudyDirection { get; set; } = null!;
        public string Teacher { get; set; } = null!;

    }
}
