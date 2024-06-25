using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CoreDashboard.Models.ViewModels
{
    public class UserUpdateViewModel : User
    {
        [ValidateNever]
        public override string UserPassword { get; set; } = null!;
    }
}
