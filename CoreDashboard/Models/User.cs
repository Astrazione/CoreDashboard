using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("user")]
	public class User : IdentityUser
	{
		[Key]
		[Column("user_id")]
		public int UserId { get; set; }

		[Column("user_name")]
		public string UserName { get; set; } = null!;

		[Column("user_email")]
		public string UserEmail { get; set; } = null!;

		[Column("user_password")]
		public string UserPassword { get; set; } = null!;

		[Column("user_type_id")]
		[ForeignKey("user_type_id")]
		public int UserTypeId { get; set; }

		public virtual UserType? UserType { get; set; }

		public virtual ICollection<UploadedDb> UploadedDbs { get; set; } = [];
	}
}
