using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("pair_theme")]
	public class PairTheme
	{
		[Key]
		[Column("pair_theme_id")]
		public int PairThemeId { get; set; }

		[Column("pair_theme_name")]
		public string? PairThemeName { get; set;}

		[Column("pair_type_id")]
		[ForeignKey("pair_type_id")]
		public int PairTypeId { get; set; }

		public virtual ICollection<UploadedDbRecord> UploadedDbRecords { get; set; } = [];
	}
}
