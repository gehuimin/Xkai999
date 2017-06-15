using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
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
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "呢称")]
        public string NickName { get; set; }

        [Required]
        [RegularExpression("[\\d]{11,13}")]
        [Display(Name = "手机号")]
        public string MobilePhone { get; set; }

        [Required]
        [RegularExpression("[\\d]{17}[0-9Xx]{1}")]
        [Display(Name = "身份证号")]
        public string PersonalIdentity { get; set; }
    }


    public class MyInfoViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 2)]
        [Display(Name = "呢称")]
        public string NickName { get; set; }

        [Required]
        [RegularExpression("[\\d]{11,13}")]
        [Display(Name = "手机号")]
        public string MobilePhone { get; set; }

        [Required]
        [RegularExpression("[\\d]{17}[0-9Xx]{1}")]
        [Display(Name = "身份证号")]
        public string PersonalIdentity { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class SetAdminRoleViewModel
    {
        [Display(Name = "登录用户名")]
        public string UserName { get; set; }

        [Display(Name = "呢称")]
        public string NickName { get; set; }

        [Display(Name = "是否管理员")]
        public bool IsAdmin { get; set; }
    }
}
