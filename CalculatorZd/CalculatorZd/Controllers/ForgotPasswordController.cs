using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BO;
using BO.Implementation;
using CalculatorZd.Models;
using Common.Api;
using DA;
using Microsoft.AspNet.Identity;
using Utils;

namespace CalculatorZd.Controllers
{
    public class ForgotPasswordController : Controller
    {
          public ForgotPasswordController() : this(IdentityConfig.Secrets, IdentityConfig.Logins, IdentityConfig.Users, IdentityConfig.Roles) { }

          public ForgotPasswordController(IUserSecretStore secrets, IUserLoginStore logins, IUserStore users, IRoleStore roles)
        {
            Secrets = secrets;
            Logins = logins;
            Users = users;
            Roles = roles;
        }

        public IUserSecretStore Secrets { get; private set; }
        public IUserLoginStore Logins { get; private set; }
        public IUserStore Users { get; private set; }
        public IRoleStore Roles { get; private set; }
        public ActionResult Index(string activationCode)
        {
            Guid code = Guid.Empty;

            if (!String.IsNullOrEmpty(activationCode) && Guid.TryParse(activationCode, out code) && code != Guid.Empty)
            {
                Firm firm = FirmAdapter.GetFirmIDByActivationCode(code, (int) ActivationType.ActivationChangePassword);

                if (firm!=null && firm.ID != Guid.Empty)
                {
                    var modelChPass = new ChangeUserPasswordViewModel();
                    modelChPass.UserName = firm.Login;
                    return View("ChangePassword", modelChPass);
                }
                else
                {
                    return View("ActivationCodeFailure");
                }
            }

            var model = new ForgetPasswordViewModel();
            return View("Index", model);
        }

        public ActionResult TryRestorePassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                Firm firmRestored = FirmsManager.GetLoginByEmail(model.Email);
                if (firmRestored != null)
                {
                    Guid activationCode = Guid.NewGuid();
                    if (FirmAdapter.CreateRestorePasswordActivationInfo(firmRestored.ID, activationCode, (int)ActivationType.ActivationChangePassword))
                    {
                        if (!MailService.SendRestorePasswordLink(activationCode.ToString(), firmRestored.AdminEmail))
                        {
                            model.StatusMailSending = MailSendingStatus.Failure;
                            return View("Index", model);
                        }
                    }
                    ViewBag.EmailForRestorePassword = model.Email;
                    ViewBag.MailProviderName = model.Email.Split('@')[1];
                }
                else
                {
                    var fpModel = new ForgetPasswordViewModel();
                    fpModel.EmailNotFound = true;
                    return View("Index", fpModel);
                }
                return View();
            }
            else
            {
                return View("Index", new ForgetPasswordViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangeUserPasswordViewModel model)
        {
            Guid actCode = Guid.Empty;
            if (ModelState.IsValid && Guid.TryParse(model.ActivationCode, out actCode))
            {
                bool changePasswordSucceeded = false;
                changePasswordSucceeded = await ChangePassword(model.UserName, model.NewPassword);
             
                if (changePasswordSucceeded)
                {
                    FirmAdapter.ActivateActivationCode(actCode, (int)ActivationType.ActivationChangePassword);
                    return RedirectToAction("PasswordChangedSuccess");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "The current password is incorrect or the new password is invalid.");
                }
            }
            return View();
        }

        public ActionResult PasswordChangedSuccess()
        {
            return View();
        }

        private async Task<bool> ChangePassword(string userName, string newPassword)
        {
            bool changePasswordSucceeded = await Secrets.UpdateSecret(userName, newPassword);
            return changePasswordSucceeded;
        }
    }
}