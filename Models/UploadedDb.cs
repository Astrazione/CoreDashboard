using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("uploaded_db")]
	public class UploadedDb
	{
		[Key]
		[Column("uploaded_db_id")]
		public int UploadedDbId { get; set; }

		[Column("uploaded_db_name")]
		public string UploadedDbName { get; set; } = null!;

		[Column("upload_date")]
		public DateTime UploadDate { get; set; }

		[Column("user_id")]
		[ForeignKey("user_id")]
		public int UserId {  get; set; }

		[Column("discipline_id")]
		[ForeignKey("discipline_id")]
		public int DisciplineId { get; set; }

		public virtual User? User { get; set; }
		public virtual Discipline? Discipline { get; set; }

		public virtual ICollection<UploadedDbResult> UploadedDbResults { get; set; } = [];
	}
}
