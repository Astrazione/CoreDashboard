using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("user_type")]
	public class UserType
	{
		[Key]
		[Column("user_type_id")]
		public int UserTypeId { get; set; }

		[Column("user_type_name")]
		public string UserTypeName { get; set; } = null!;

		public virtual ICollection<User> Users { get; set; } = [];
	}
}
