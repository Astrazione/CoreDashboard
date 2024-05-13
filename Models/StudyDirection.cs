using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("study_direction")]
	public class StudyDirection
	{
		[Key]
		[Column("study_direction_id")]
		public int StudyDirectionId { get; set; }

		[Column("study_direction_name")]
		public string? StudyDirectionName { get; set; }

		public virtual ICollection<UploadedDbResult> UploadedDbResults { get; set; } = [];
	}
}
