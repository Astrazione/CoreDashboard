using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("uploaded_db_record")]
	public class UploadedDbRecord
	{
		[Key]
		[Column("uploaded_db_record_id")]
		public int UploadedDbRecordId { get; set; }

		[Column("theme_score")]
		[Precision(5, 2)]
		public decimal? ThemeScore { get; set; }

		[Column("is_control_point")]
		public bool IsControlPoint { get; set; }

		[Column("presence")]
		public bool? Presence { get; set; }

		[Column("uploaded_db_result_id")]
		[ForeignKey("uploaded_db_result_id")]
		public int UploadedDbResultId { get; set; }

		[Column("pair_theme_id")]
		[ForeignKey("pair_theme_id")]
		public int PairThemeId { get; set; }

		[Column("hash")]
		public string Hash { get; set; } = null!;

		public virtual UploadedDbResult? UploadedDbResult { get; set; }
		public virtual PairTheme? PairTheme { get; set; }
	}
}
