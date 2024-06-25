
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("uploaded_db_result")]
	public class UploadedDbResult : IEquatable<UploadedDbResult>
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
		[Precision(7, 2)]
		public decimal TotalScore { get; set; }

		[Column("rating")]
		public string? Rating { get; set; }

		public virtual UploadedDb? UploadedDb { get; set; }
		public virtual Student? Student { get; set; }
		public virtual StudyDirection? StudyDirection { get; set; }
		public virtual StudyGroup? StudyGroup { get; set; }

		public virtual ICollection<UploadedDbRecord> UploadedDbRecords { get; set; } = [];

		public bool Equals(UploadedDbResult? other)
		{
			if (other is null) return false;

			return Student!.Equals(other.Student) &&
				   UploadedDbId == other.UploadedDbId &&
				   StudyDirection!.Equals(other.StudyDirection) &&
				   StudyGroup!.Equals(other.StudyGroup) &&
				   TotalScore == other.TotalScore &&
				   Rating == other.Rating;
		}

		public override bool Equals(object? obj) => 
			Equals(obj as UploadedDbResult);

		public override int GetHashCode() => 
			HashCode.Combine(Student, UploadedDbId, StudyDirection, StudyGroup, TotalScore, Rating);
	}
}
