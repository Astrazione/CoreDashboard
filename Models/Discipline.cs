using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("discipline")]
	public class Discipline
	{
		[Key]
		[Column("discipline_id")]
		public int DisciplineId { get; set; }

		[Column("discipline_name")]
		public string DisciplineName { get; set; } = null!;

		public virtual ICollection<UploadedDb> UploadedDbs { get; set; } = [];
	}
}
