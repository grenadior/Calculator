using System.ComponentModel.DataAnnotations;
using Common.Api.Validation;

namespace CalculatorZd.Models
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [Display(Name = "Введите email")]
        [Email(ErrorMessage = "Неверный формат e-mail")]
        public string Email { get; set; }
        public bool EmailNotFound { get; set; }
        public MailSendingStatus StatusMailSending { get; set; }
    }

    public enum MailSendingStatus
    {
        None,
        Failure,
        Success
    }
    public class ChangeUserPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }

        public string UserName { get; set; }
        public string ActivationCode  { get; set; }

    }
}