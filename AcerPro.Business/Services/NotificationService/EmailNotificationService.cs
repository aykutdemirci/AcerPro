using AcerPro.Dto.ViewModels;
using AcerPro.Dto;
using System;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace AcerPro.Business.Services.NotificationService
{
    public class EmailNotificationService : INotificationService
    {
        public void SendNotification(string message)
        {
            try
            {
                var currentSessionUser = (UserViewModel)HttpContext.Current.Session[Constants.SessionKey];

                using (MailMessage mm = new MailMessage())
                {
                    mm.From = new MailAddress("acerprocase@outlook.com", "AcerPro Case");
                    mm.To.Add(currentSessionUser.Email);
                    mm.Subject = "Application health notification";
                    mm.Body = message;
                    mm.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp-mail.outlook.com";
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Credentials = new NetworkCredential("acerprocase@outlook.com", "20_acerpro_23");
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
