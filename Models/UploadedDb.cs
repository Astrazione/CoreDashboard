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
		[Display(Name = "Название базы данных")]
		public string UploadedDbName { get; set; } = null!;

		[Column("upload_date", TypeName = "Date")]
		[Display(Name = "Дата загрузки")]
		[DisplayFormat(DataFormatString = "{0:d}")]
		public DateTime UploadDate { get; set; }

		[Column("user_id")]
		[ForeignKey("user_id")]
		public int? UserId {  get; set; }

		[Column("discipline_id")]
		[ForeignKey("discipline_id")]
		public int DisciplineId { get; set; }

		[Display(Name = "Пользователь")]
		public virtual User? User { get; set; }
		[Display(Name = "Дисциплина")]
		public virtual Discipline? Discipline { get; set; }

		public virtual ICollection<UploadedDbResult> UploadedDbResults { get; set; } = [];
	}
}
