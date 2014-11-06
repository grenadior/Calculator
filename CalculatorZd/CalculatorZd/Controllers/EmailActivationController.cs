using System;
using System.Web.Mvc;
using BO;
using BO.Implementation;
using CalculatorZd.Models;
using Common.Api;
using DA;
using Localization.WebResources.Common;
using Microsoft.AspNet.Identity;
using Utils;

namespace CalculatorZd
{
    public class EmailActivationController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid activationCode = Guid.NewGuid();
                string emailRecipient = SessionManager.FirmInfo.AdminEmail;

               OperationStatus status = FirmAdapter.InsertActivationCode(activationCode, SessionManager.FirmInfo.ID, emailRecipient,DateTime.Now.AddDays(2),(int)ActivationType.ActivationEmail);
                if (status == OperationStatus.Success)
                {
                    bool result = MailService.SendEmail(activationCode.ToString(), emailRecipient, !SessionManager.FirmInfo.IsNonActivated);
                    ViewBag.Status = result;
                    if (result)
                    {
                        ViewBag.Message = String.Format(Strings.EmailActivationCodeSend, emailRecipient,
                            "SendActivationEmailLink/");
                    }
                    else
                    {
                        ViewBag.Message = String.Format(Strings.EmailActivationCodeSendFailure);
                    }
                }
               
                return View();
            }
            //TODO переделать url sending code
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Activate(string id)
        {
            var status = OperationStatus.Failure;

            if (User.Identity.IsAuthenticated)
            {
                
                Guid code;
                if (Guid.TryParse(id, out code))
                {
                    Firm firm = FirmsManager.GetFirmIDByActivationCode(code, (int)ActivationType.ActivationEmail);

                    if (firm.ID != Guid.Empty)
                    {
                        status = FirmsManager.ActivateEmail(code);

                        if (status == OperationStatus.Success)
                        {
                            SessionManager.FirmInfo = FirmsManager.GetFirmByID(firm.ID);
                            ViewBag.Message = String.Format(Strings.EmailActivationSuccess);
                        }
                        else
                        {
                            ViewBag.Message = String.Format(Strings.EmailActivationCodeFailure);
                        }
                    }
                    else
                    {
                        ViewBag.Message = String.Format(Strings.EmailActivationCodeFailure);
                    }
                }

                ViewBag.Status = status == OperationStatus.Success;
                return View("ActivationdEmail");
            }
            return RedirectToAction("Login", "Account");
        }
    }
}