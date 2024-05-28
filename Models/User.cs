using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreDashboard.Models
{
	[Table("user")]
	public class User
	{
		public User() { }

		public User(int id, string name, string email, string password, int userTypeId) 
		{ 
			UserId = id;
			UserName = name;
			UserEmail = email;
			UserPassword = password;
			UserTypeId = userTypeId;
		}

		[Key]
		[Column("user_id")]
		public int UserId { get; set; }

		[Column("user_name")]
		[Display(Name = "Имя пользователя")]
		public string UserName { get; set; } = null!;

		[Column("user_email")]
		[Display(Name = "Адрес электронной почты")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Введите адрес корректно")]
        public string UserEmail { get; set; } = null!;

		[Column("user_password")]
        [Display(Name = "Пароль")]
        public string UserPassword { get; set; } = null!;

		[Column("user_type_id")]
		[ForeignKey("user_type_id")]
		public int UserTypeId { get; set; }

		[Display(Name = "Тип пользователя")]
		public virtual UserType? UserType { get; set; }

		public virtual ICollection<UploadedDb> UploadedDbs { get; set; } = [];
	}
}
