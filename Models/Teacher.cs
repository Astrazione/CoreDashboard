using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("teacher")]
	public class Teacher
	{
		[Key]
		[Column("teacher_id")]
		public int TeacherId { get; set; }

		[Column("teacher_name")]
		public string? TeacherName { get; set; }

		public ICollection<StudyGroup> StudyGroups { get; set; } = [];
	}
}
