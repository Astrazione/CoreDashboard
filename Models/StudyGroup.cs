using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("study_group")]
	public class StudyGroup
	{
		[Key]
		[Column("study_group_id")]
		public int StudyGroupId { get; set; }

		[Column("study_group_name")]
		public string StudyGroupName { get; set; } = null!;

		[Column("teacher_id")]
		[ForeignKey("teacher_id")]
		public int TeacherId { get; set; }

		public Teacher? Teacher { get; set; }

		public ICollection<UploadedDbResult> UploadedDbResults { get; set; } = [];
	}
}
