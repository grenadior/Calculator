using System;
using Common.Api.Validation;
using Localization.WebResources.Common;
using System.ComponentModel.DataAnnotations;

namespace CalculatorZd.Models 
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        public string LoginProvider { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение в поле {0} должно быть не меньше {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel : FirmProfileBaseModel
    {
        [Required]
        [Display(Name = "Логин")]
        [StringLength(12, ErrorMessageResourceType = typeof (Strings), ErrorMessageResourceName = "RegisterViewModel_Login_Логин_Length", MinimumLength = 6)]
        public string Login { get; set; }

        [Display(Name = "E-mail администратора")]
        [Required]
        [Email(ErrorMessage = "Неверный формат e-mail")]
        public string AdminEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение в поле {0} должно быть не меньше {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
    public class FirmProfileBaseModel
    {
        public Guid ID { get; set; }
        [Required]
        [Display(Name = "Наименование организации")]
        public string FirmName { get; set; }
        
        [Required]
        [Display(Name = "ОКПО")]
        public string OKPO { get; set; }

        [Required]
        [Display(Name = "ИНН")]
        public string INN { get; set; }
        
        [Required]
        [Display(Name = "Юридический адрес")]
        public string Address { get; set; }
        
        [Required]
        [Display(Name = "Почтовый адрес")]
        public string PostAddress { get; set; }

        [Required]
        [Display(Name = "Телефон 1")]
        public string Phone { get; set; }
      
        [Display(Name = "Телефон 2")]
        public string Phone2 { get; set; }
        
        [Display(Name = "Факс 1")]
        public string Fax { get; set; }
        
        [Display(Name = "Факс 2")]
        public string Fax2 { get; set; }

        [Required]
        [Display(Name = "ФИО директора")]
        public string FIODirector { get; set; }

        [Required]
        [Display(Name = "ФИО бухгалтера")]
        public string FIOBuh { get; set; }
        
        [Required]
        [Display(Name = "ФИО контактного лица")]
        public string FIOContact { get; set; }

        [Required]
        [Display(Name = "Телефон контакта")]
        public string ContactPhone { get; set; }

        [Required]
        [Display(Name = "Моб. телефон контакта")]
        public string ContactMobile { get; set; }

        [Required]
        [Display(Name = "E-mail контакта")]
        [Email(ErrorMessage = "Неверный формат e-mail")]
        public string ContactEmail { get; set; }
        
        [Display(Name = "E-mail администратора")]
        public string AdminEmail { get; set; }
    }
}
