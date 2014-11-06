using System;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using BO.Implementation;
using Common.Api;
using DA.ExceptionAdapter;
using Localization.WebResources.Common;

namespace Utils
{
    public class MailService
    {
        public static bool SendEmail(string activationCode, string emailRecipient, bool isEmailActivated)
        {
            bool result = false;
            try
            {
                using (MailMessage mm = new MailMessage(String.Format("Name {0}", ServerProperties.Instance.FromMail), emailRecipient))
                {
                    StringBuilder sbMail = new StringBuilder();
                    string linkText = isEmailActivated
                        ? String.Format("Перейти на сайт {0} и сменить email администратора ",
                            ServerProperties.Instance.SiteName)
                        : String.Format("Активировать email на сайте {0} ",
                            ServerProperties.Instance.SiteName);
                    sbMail.Append("<b>Ссылка активации e-mail:</b>");

                    string url = String.Format("{0}:{1}/EmailActivation/Activate/{2}", HttpContext.Current.Request.Url.Host, ServerProperties.Instance.ReturnMainSiteHttpPort, activationCode);
                    sbMail.Append(String.Format(" <a href=\"http://{0}\">{1}</a>", url, linkText));

                    mm.Subject = isEmailActivated ? String.Format(Strings.ChangeEmailText, ServerProperties.Instance.SiteName) : String.Format(Strings.EmailActivationSubject, ServerProperties.Instance.SiteName);
                    mm.Body = sbMail.ToString();
                    mm.IsBodyHtml = true;
                    using (var sc = new SmtpClient(ServerProperties.Instance.EmailSMTPClient, 587))
                    {
                        sc.EnableSsl = true;
                        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                        sc.UseDefaultCredentials = true;
                        sc.Credentials = new NetworkCredential(ServerProperties.Instance.FromMail, EmailConstants.fromMail_Password);
                        sc.Send(mm);
                    }
                }
                result = true;
            }
            catch (SmtpException ex)
            {
                result = false;
                ExceptionAdapter.InsertException(ex.Message);
            }

            return result;
        }

        public static bool SendRestorePasswordLink(string activationCode, string emailRecipient)
        {
            bool result = false;
            try
            {
                using (MailMessage mm = new MailMessage(String.Format("Name {0}", ServerProperties.Instance.FromMail), emailRecipient))
                {
                    StringBuilder sbMail = new StringBuilder();
                    sbMail.Append("<b>Ссылка для смены пароля:</b><br>");
                    string url = String.Format("{0}:{1}/ForgotPassword?activationcode={2}", HttpContext.Current.Request.Url.Host, ServerProperties.Instance.ReturnMainSiteHttpPort, activationCode);
                    sbMail.Append(String.Format("<a href=\"http://{0}\">{1}</a>", url, String.Format("Перейти на {0} и сменить пароль", ServerProperties.Instance.SiteName)));
                  
                    mm.Subject = String.Format(Strings.ChangePasswordSubject, ServerProperties.Instance.SiteName);
                    mm.Body = sbMail.ToString();
                    mm.IsBodyHtml = true;
                    using (var sc = new SmtpClient(ServerProperties.Instance.EmailSMTPClient, 587))
                    {
                        sc.EnableSsl = true;
                        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                        sc.UseDefaultCredentials = true;
                        sc.Credentials = new NetworkCredential(ServerProperties.Instance.FromMail, EmailConstants.fromMail_Password);
                        sc.Send(mm);
                    }
                }
                result = true;
            }
            catch (SmtpException ex)
            {
                result = false;
                ExceptionAdapter.InsertException(ex.Message +  ex.InnerException.Message);
            }

            return result;
        }
    }
}