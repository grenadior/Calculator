using CalculatorZd.Models;
using Common.Api;
using DA;
using Localization.WebResources.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;
using Utils;
using FirmsManager = BO.Implementation.FirmsManager;

namespace CalculatorZd.Controllers
{
      [Authorize]
    public class SendActivationEmailLinkController : Controller
    {
          public SendActivationEmailLinkController() : this(IdentityConfig.Secrets, IdentityConfig.Logins, IdentityConfig.Users, IdentityConfig.Roles) { }

        public SendActivationEmailLinkController (IUserSecretStore secrets, IUserLoginStore logins, IUserStore users, IRoleStore roles)
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


        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {       
               var model = new EmailActivationSendModels();
                model.Email = SessionManager.FirmInfo.AdminEmail;
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult SendEmailActivationCode(EmailActivationSendModels model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    string userId = User.Identity.GetUserId();
                    Guid activationCode = Guid.NewGuid();
                    var status = OperationStatus.Failure;
                    bool emailSend = SendActivationEmailLink(activationCode.ToString(), model.Email, !SessionManager.FirmInfo.IsNonActivated);
                    if (emailSend)
                    {
                        if (SessionManager.FirmInfo.IsNonActivated)
                            status = FirmAdapter.InsertActivationCode(activationCode, new Guid(userId), model.Email, DateTime.Now.AddDays(2), (int)ActivationType.ActivationEmail);
                        else
                        {
                            status = FirmAdapter.InsertActivationCode(activationCode, new Guid(userId), model.Email, DateTime.Now.AddDays(2), (int)ActivationType.ChangeEmail);
                        }
                    }
                   
                    var modelEmail = new EmailActivationSendModels()
                    {
                        Status = status
                    };

                    ViewBag.Message = status == OperationStatus.Failure ? Strings.EmailActivationCodeSendFailure : String.Format(Strings.EmailActivationCodeSend, model.Email, "Index?email=" + model.Email);
                    ViewBag.Status = status;
                    return View("SendEmailActivation", modelEmail);
                  
                }
                else 
                {
                    return View("Index", model);
                }
               
            }
            return RedirectToAction("Login", "Account");
        }

        private bool SendActivationEmailLink(string activationCode, string recipientEmail, bool isEmailActivated)
        {
            return MailService.SendEmail(activationCode, recipientEmail, isEmailActivated);
        }
	}
}