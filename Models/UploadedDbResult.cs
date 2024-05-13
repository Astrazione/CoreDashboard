
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("uploaded_db_result")]
	public class UploadedDbResult
	{
		[Key]
		[Column("uploaded_db_result_id")]
		public int UploadedDbResultId { get; set; }

		[Column("uploaded_db_id")]
		[ForeignKey("uploaded_db_id")]
		public int UploadedDbId { get; set; }

		[Column("student_id")]
		[ForeignKey("student_id")]
		public long StudentId { get; set; }

		[Column("study_direction_id")]
		[ForeignKey("study_direction_id")]
		public int StudyDirectionId { get; set; }

		[Column("study_group_id")]
		[ForeignKey("study_group_id")]
		public int StudyGroupId { get; set; }

		[Column("total_score")]
		[Precision(5, 2)]
		public decimal TotalScore { get; set; }

		[Column("rating")]
		public string? Rating { get; set; }

		public virtual UploadedDb? UploadedDb { get; set; }
		public virtual Student? Student { get; set; }
		public virtual StudyDirection? StudyDirection { get; set; }
		public virtual StudyGroup? StudyGroup { get; set; }

		public virtual ICollection<UploadedDbRecord> UploadedDbRecords { get; set; } = [];
	}
}
