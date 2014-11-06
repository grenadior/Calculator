using System.ComponentModel.DataAnnotations;
using BO.Implementation;
using Common.Api;
using Common.Api.Validation;
using Localization.WebResources.WebResources;

namespace CalculatorZd.Models
{
    public class EmailActivationModels
    {
        public string Email;
        public EmailActivationStatus Status;
    }

    public class EmailActivationSendModels
    {
        public OperationStatus Status;

        [Display(Name = "Email_DisplayName", ResourceType = typeof (AccountResource))]
        [Required(ErrorMessage = "Введите e-mail")]
        [Email(ErrorMessage = "Неверный формат e-mail")]
        public string Email { get; set; }
    }
}