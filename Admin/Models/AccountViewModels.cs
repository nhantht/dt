using DataAnnotationsExtensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredPhone", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        [Display(Name = "Phone", ResourceType = typeof(Admin.App_LocalResources.LanguageLabel))]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceName = "RequiredPassword", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Admin.App_LocalResources.LanguageLabel))]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string Message { get; set; }
    }

    public class RegisterViewModel
    {
        [StringLength(11, ErrorMessage = null, ErrorMessageResourceName = "RequiredLength", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error), MinimumLength = 6)]
        [Required(ErrorMessage = null, ErrorMessageResourceName = "RequiredPhone", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        [Integer(ErrorMessage = null, ErrorMessageResourceName = "OnlyInteger", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        [Display(Name = "Phone", ResourceType = typeof(Admin.App_LocalResources.LanguageLabel))]
        [Key]
        public string Phone { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = "RequiredEmail", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "InvalidEmailFormat", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        [Display(Name = "Email", ResourceType = typeof(Admin.App_LocalResources.LanguageLabel))]
        public string Email { get; set; }

        [EmailAddress]
        [Compare("Email", ErrorMessage = null, ErrorMessageResourceName = "NotEqualConfirmedEmail", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        [Display(Name = "ConfirmedEmail", ResourceType = typeof(Admin.App_LocalResources.LanguageLabel))]
        public string ConfirmedEmail { get; set; }

        //[Required]
        [Display(Name = "Fullname", ResourceType = typeof(Admin.App_LocalResources.LanguageLabel))]
        public string Fullname { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = "RequiredPassword", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        [StringLength(100, ErrorMessage = null, ErrorMessageResourceName = "RequiredLength", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error), MinimumLength = 8)]
        [RegularExpression(@"^(?=[^\d_].*?\d)\w(\w|[!@#$%]){7,20}", ErrorMessageResourceName = "PasswordRule", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Admin.App_LocalResources.LanguageLabel))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmedPassword", ResourceType = typeof(Admin.App_LocalResources.LanguageLabel))]
        [Compare("Password", ErrorMessage = null, ErrorMessageResourceName = "NotEqualConfirmedPassword", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        public string ConfirmPassword { get; set; }
        public string Message { get; set; }
    }
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=[^\d_].*?\d)\w(\w|[!@#$%]){7,20}", ErrorMessage = @"Error. Password must be at least 8 characters long must with one capital, one special character and one numerical character. It can not start with a special character or a digit.")]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Message { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredEmail", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "InvalidEmailFormat", ErrorMessageResourceType = typeof(Admin.App_LocalResources.Error))]
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
