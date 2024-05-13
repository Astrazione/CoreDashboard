using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("student")]
	public class Student
	{
		[Key]
		[Column("student_id")]
		public long StudentId { get; set; }

		[Column("student_name")]
		public string? StudentName { get; set; }

		[Column("student_email")]
		public string? StudentEmail { get; set; }

		public virtual ICollection<UploadedDbResult> UploadedDbResults { get; set; } = [];
	}
}
