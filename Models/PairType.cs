using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("pair_type")]
	public class PairType
	{
		[Key]
		[Column("pair_type_id")]
		public int PairTypeId { get; set; }

		[Column("pair_type_name")]
		public string PairTypeName { get; set; } = null!;

		public ICollection<PairTheme> PairThemes { get; set; } = [];
	}
}
