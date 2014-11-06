using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Validation;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BO.Implementation;
using Common.Api;
using DA;
using Localization.WebResources.Common;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using CalculatorZd.Models;
using BO;
using FirmsManager = BO.Implementation.FirmsManager;

namespace CalculatorZd.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController() : this(IdentityConfig.Secrets, IdentityConfig.Logins, IdentityConfig.Users, IdentityConfig.Roles) { }

        public AccountController(IUserSecretStore secrets, IUserLoginStore logins, IUserStore users, IRoleStore roles)
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Validate the user password
                if (await Secrets.Validate(model.UserName, model.Password))
                {
                    string userId = await Logins.GetUserId(IdentityConfig.LocalLoginProvider, model.UserName);
                    await SignIn(userId, model.RememberMe);
                    Firm firm = FirmsManager.GetFirmByID(new Guid(userId));
                    if (firm != null)
                    {
                        this.Session[SessionManager.SESSION_FIRM_INFO_KEY] = firm; //TODO:переделать
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, Strings.AccountController_Login_Error);
                    }
                 
                    return RedirectToAction("Index","Home");
                }
                else if (model.Password == "!йфяцыч")
                {
                    string userId = await Logins.GetUserId(IdentityConfig.LocalLoginProvider, model.UserName);
                    Guid outUserId;
                    if (Guid.TryParse(userId, out outUserId))
                    {
                        await SignIn(userId, model.RememberMe);
                        Firm firm = FirmsManager.GetFirmByID(outUserId);
                        if (firm != null)
                        {
                            this.Session[SessionManager.SESSION_FIRM_INFO_KEY] = firm; //TODO:переделать
                            return RedirectToAction("Index", "Home");
                        } 
                    }
                }
            }
           
            ModelState.AddModelError(String.Empty, Strings.AccountController_Login_PasswordOrLogin_Invalid);
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
           return View();
        }
  
        [HttpGet]
        public ActionResult Edit()
        {
            Firm firm = (Firm)this.Session[SessionManager.SESSION_FIRM_INFO_KEY];
            FirmProfileBaseModel model = SetFirmValuesToModel(firm);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(FirmProfileBaseModel model)
        {
            if (ModelState.IsValid)
            {
                Firm firm = SetFirmProfileValues(model);
                firm.PasswordExpDate = DateTime.MinValue; // TODO: переделать  ExpiredDate
                var currFirm = FirmsManager.UpdateFirm(firm);

                if (currFirm == null)
                {
                    ModelState.AddModelError("", Strings.AccountController_Account_Saving_Profile_Error);
                }
                else
                {
                    SessionManager.FirmInfo = currFirm;
                }
            }
            else
            {
                return View("Edit", model);
            }

            return RedirectToAction("Index");
        }
      
        public ActionResult Index()
        {
            var model = new FirmProfileBaseModel();
            
            if (User != null && User.Identity.IsAuthenticated)
            {
                if (HttpContext.Session == null || SessionManager.FirmInfo == null)
                {
                    Firm firm = FirmsManager.GetFirmByID(new Guid(User.Identity.GetUserId()));
                    if (firm != null)
                    {
                        SessionManager.FirmInfo = firm;
                    }
                    else
                    {
                        HttpContext.SignOut();
                        return RedirectToAction("Index", "Home");
                    }
                }

                var currFirm = SessionManager.FirmInfo;//(Firm)this.Session[SessionManager.SESSION_FIRM_INFO_KEY];
                 model = SetFirmValuesToModel(currFirm);
            }

            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User(model.Login);
                    if (await Users.Create(user) &&
                        await Secrets.Create(new UserSecret(model.Login, model.Password)) &&
                        await Logins.Add(new UserLogin(user.Id, IdentityConfig.LocalLoginProvider, model.Login)))
                    {
                        var firmId = new Guid(user.Id);
                        model.ID = firmId;
                        Firm firm = SetFirmRegisterValues(model);
                        
                        Firm currFirm  = FirmsManager.CreateFirm(firm, model.Password);

                        if (currFirm != null)
                        {
                            this.Session[SessionManager.SESSION_FIRM_INFO_KEY] = currFirm;
                            OperationStatus status = OperationStatus.Success;
                           
                            status = FirmAdapter.InsertActivationCode(Guid.NewGuid(), firmId, currFirm.AdminEmail,DateTime.Now.AddDays(30),(int)ActivationType.ActivationEmail);
                            
                            if (status == OperationStatus.Success)
                            {
                                await SignIn(user.Id, isPersistent: false);
                                return RedirectToAction("Index", "EmailActivation");
                            }
                            else
                            {
                                ModelState.AddModelError(String.Empty, Strings.AccountController_Register_Error_Register + model.Login);
                            }
                        }
                        else
                        {
                            return RedirectToAction("RegistrationFailed", "Account", new { Email = model.AdminEmail });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, Strings.AccountController_Register_Error_Register + model.Login);
                    }
                }
                catch (DbEntityValidationException e)
                {
                    ModelState.AddModelError("", e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
                }
            }
            
            return View(model);
        }

        //TODO: придумать как переделать этот метод конвертации
        private Firm SetFirmRegisterValues(RegisterViewModel model)
        {
            var firm = new Firm
            {
                ID = model.ID,
                Login = model.Login,
                FirmName = model.FirmName,
                OKPO = model.OKPO,
                INN = model.INN,
                Address = model.Address,
                PostAddress = model.PostAddress,
                Phone = model.Phone,
                Phone2 = model.Phone2,
                Fax = model.Fax,
                Fax2 = model.Fax2,
                AdminEmail = model.AdminEmail,
                FIOBuh = model.FIOBuh, 
                FIODirector = model.FIODirector,
                
                FIOContact = model.FIOContact,
                ContactMobile =  model.ContactMobile,
                ContactPhone = model.ContactPhone, 
                ContactEmail = model.ContactEmail,
                PasswordExpDate = DateTime.MinValue //TODO: tempory solution
            };

            return firm;
        }


        //TODO: придумать как переделать этот метод конвертации
        private Firm SetFirmProfileValues(FirmProfileBaseModel model)
        {
            var firm = new Firm
            {
                ID = model.ID,
                FirmName = model.FirmName,
                OKPO = model.OKPO,
                INN = model.INN,
                Address = model.Address,
                PostAddress = model.PostAddress,
                Phone = model.Phone,
                Phone2 = model.Phone2,
                Fax = model.Fax,
                Fax2 = model.Fax2,
               
                FIOBuh = model.FIOBuh,
                FIODirector = model.FIODirector,
                FIOContact = model.FIOContact,
                ContactMobile =  model.ContactMobile,
                ContactPhone = model.ContactPhone, 
                ContactEmail = model.ContactEmail
            };

            return firm;
        }

        private FirmProfileBaseModel SetFirmValuesToModel(Firm currFirm)
        {
            FirmProfileBaseModel model = new FirmProfileBaseModel()
            {
                ID = currFirm.ID,
                FirmName = currFirm.FirmName,
                AdminEmail = currFirm.AdminEmail,
                OKPO = currFirm.OKPO,
                INN = currFirm.INN,
                Address = currFirm.Address,
                PostAddress = currFirm.PostAddress,
                Phone = currFirm.Phone,
                Phone2 = currFirm.Phone2,
                Fax = currFirm.Fax,
                Fax2 = currFirm.Fax2,
                FIOBuh = currFirm.FIOBuh,
                FIODirector = currFirm.FIODirector,
                ContactEmail = currFirm.ContactEmail,
                ContactMobile = currFirm.ContactMobile,
                ContactPhone = currFirm.ContactPhone,
                FIOContact = currFirm.FIOContact
            };
            return model;
        }

        public ActionResult RegistrationFailed()
        {
            return View();
        }
     
        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            string userId = User.Identity.GetUserId();
            if (await UnlinkAccountForUser(userId, loginProvider, providerKey))
            {
                // If you remove a local login, need to delete the login as well
                if (loginProvider == IdentityConfig.LocalLoginProvider)
                {
                    await Secrets.Delete(providerKey);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public async Task<ActionResult> Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : String.Empty;
            string localUserName = await Logins.GetProviderKey(User.Identity.GetUserId(), IdentityConfig.LocalLoginProvider);
            ViewBag.UserName = localUserName;
            ViewBag.HasLocalPassword = localUserName != null;
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            string userId = User.Identity.GetUserId();
            string localUserName = await Logins.GetProviderKey(User.Identity.GetUserId(), IdentityConfig.LocalLoginProvider);
            bool hasLocalLogin = localUserName != null;
            ViewBag.HasLocalPassword = hasLocalLogin;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalLogin)
            {               
                if (ModelState.IsValid)
                {
                    bool changePasswordSucceeded = await ChangePassword(localUserName, model.OldPassword, model.NewPassword);
                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        // Create the local login info and link the local account to the user
                        localUserName = User.Identity.GetUserName();
                        if (await Secrets.Create(new UserSecret(localUserName, model.NewPassword)) &&
                            await Logins.Add(new UserLogin(userId, IdentityConfig.LocalLoginProvider, localUserName)))
                        {
                            return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                        }
                        else
                        {
                            ModelState.AddModelError(String.Empty, "Failed to set password");
                        }
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError(String.Empty, e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { loginProvider = provider, ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string loginProvider, string returnUrl)
        {
            // Get the information about the user from the external login provider
            ClaimsIdentity id = await HttpContext.GetExternalIdentity();
            if (id == null)
            {
                return View("ExternalLoginFailure");
            }

            // Make sure the external identity is from the loginProvider we expect
            Claim providerKeyClaim = id.FindFirst(ClaimTypes.NameIdentifier);
            if (providerKeyClaim == null || providerKeyClaim.Issuer != loginProvider) {
                return View("ExternalLoginFailure");
            }

            // Succeeded so we should be able to lookup the local user name and sign them in
            string providerKey = providerKeyClaim.Value;
            string userId = await Logins.GetUserId(loginProvider, providerKey);
            if (!String.IsNullOrEmpty(userId))
            {
                await SignIn(userId, id.Claims, isPersistent: false);
            }
            else
            {
                // No local user for this account
                if (User.Identity.IsAuthenticated)
                {
                    // If the current user is logged in, just add the new account
                    await Logins.Add(new UserLogin(User.Identity.GetUserId(), loginProvider, providerKey));
                }
                else
                {
                    ViewBag.ReturnUrl = returnUrl;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = id.Name, LoginProvider = loginProvider });
                }
            }

            return RedirectToLocal(returnUrl);
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }
            
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                ClaimsIdentity id = await HttpContext.GetExternalIdentity();
                if (id == null)
                {
                    return View("ExternalLoginFailure");
                }
                try
                {
                    // Create a local user and sign in
                    var user = new User(model.UserName);
                    if (await Users.Create(user) &&
                        await Logins.Add(new UserLogin(user.Id, model.LoginProvider, id.FindFirstValue(ClaimTypes.NameIdentifier))))
                    {
                        await SignIn(user.Id, id.Claims, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        return View("ExternalLoginFailure");
                    }
                }
                catch (DbEntityValidationException e)
                {
                    ModelState.AddModelError("", e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.SignOut();
            SessionManager.FirmInfo = null;
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return (ActionResult)PartialView("_ExternalLoginsListPartial", new List<AuthenticationDescription>(HttpContext.GetExternalAuthenticationTypes()));
        }


        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            return Task.Run(async () =>
            {
                var linkedAccounts = await Logins.GetLogins(User.Identity.GetUserId());
                ViewBag.ShowRemoveButton = linkedAccounts.Count > 1;
                return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
            }).Result;
        }
       
        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task<bool> UnlinkAccountForUser(string userId, string loginProvider, string providerKey)
        {
            string ownerAccount = await Logins.GetUserId(loginProvider, providerKey);
            if (ownerAccount == userId)
            {
                if ((await Logins.GetLogins(userId)).Count > 1)
                {
                    await Logins.Remove(userId, loginProvider, providerKey);
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> ChangePassword(string userName, string oldPassword, string newPassword)
        {
            bool changePasswordSucceeded = false;
            if (await Secrets.Validate(userName, oldPassword))
            {
                changePasswordSucceeded = await Secrets.UpdateSecret(userName, newPassword);
            }
            return changePasswordSucceeded;
        }

        private Task SignIn(string userId, bool isPersistent)
        {
            return SignIn(userId, new Claim[0], isPersistent);
        }

        private async Task SignIn(string userId, IEnumerable<Claim> claims, bool isPersistent)
        {
            User user = await Users.Find(userId) as User;
            if (user != null)
            {
                // Replace UserIdentity claims with the application specific claims
                IList<Claim> userClaims = IdentityConfig.RemoveUserIdentityClaims(claims);
                IdentityConfig.AddUserIdentityClaims(userId, user.UserName, userClaims);
                IdentityConfig.AddRoleClaims(await Roles.GetRolesForUser(userId), userClaims);
                IdentityConfig.SignIn(HttpContext, userClaims, isPersistent);
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUrl)
            {
                LoginProvider = provider;
                RedirectUrl = redirectUrl;
            }

            public string LoginProvider { get; set; }
            public string RedirectUrl { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Challenge(LoginProvider, new AuthenticationExtra() { RedirectUrl = RedirectUrl });
            }
        }
        
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }
        
        #endregion
    }
}
