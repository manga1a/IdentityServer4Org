using System.ComponentModel.DataAnnotations;

namespace IdentityServer4Org.Areas.Admin.Models
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name="New Password")]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm New Password")]
        public string ConfirmPassword { get; set; }
    }
}
