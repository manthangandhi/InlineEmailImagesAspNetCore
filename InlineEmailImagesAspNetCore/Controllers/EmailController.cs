using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InlineEmailImagesAspNetCore.Controllers
{
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public string Index()
        {
            return SendMail();
        }

        private static string SendMail()
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    var newMail = new MailMessage { From = new MailAddress("[YOUR EMAIL ADDRESS]") };
                    newMail.To.Add(new MailAddress("[TO EMAIL ADDRESS]"));
                    newMail.Subject = "Inline Image Mail Sample";
                    newMail.IsBodyHtml = true;
                    var logo = Directory.GetCurrentDirectory() + "/Logo/logo.png";
                    var inlineLogo = new LinkedResource(logo, "image/png")
                    {
                        ContentId = Guid.NewGuid().ToString(),
                        TransferEncoding = TransferEncoding.Base64
                    };
                    var body = $@"<p>Hello email user!</p>
                            <img src=""cid:{inlineLogo.ContentId}"" />
                            <p>This is the example for inline image in email.</p>";
                    var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                    view.LinkedResources.Add(inlineLogo);
                    newMail.AlternateViews.Add(view);
                    client.Host = "[Your SMTP HOST]";
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = true;
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("[YOUR EMAIL ADDRESS]", "[YOUR EMAIL PASSWORD]");
                    client.TargetName = "[YOUR HOST TARGET NAME]";
                    client.Send(newMail);
                }

                return "Mail Sent!";
            }
            catch (Exception e)
            {
                return "Error in sending email." + e.Message;
            }
        }
    }
}
