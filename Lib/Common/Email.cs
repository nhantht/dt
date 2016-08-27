using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;

namespace Lib.Common
{
    public class Email
    {
        private string ERR_MESS = string.Empty;
        public bool SendEmail(string[] TO, string SUBJECT, string MESS)
        {
            try
            {
                System.Web.Mail.SmtpMail.SmtpServer = ConfigurationManager.AppSettings["EMAIL_SERVER"];

                MailMessage mail = new MailMessage();

                System.Web.Mail.MailMessage mess = new System.Web.Mail.MailMessage();
                mess.From = "\"DT\" <" + ConfigurationManager.AppSettings["EMAIL"] + ">";

                mess.BodyFormat = System.Web.Mail.MailFormat.Html;
                mess.BodyEncoding = System.Text.Encoding.UTF8;
                mess.Body = MESS;
                mess.Subject = SUBJECT;
                mess.To = "";
                mess.Bcc = "truonghoangthiennhan@gmail.com";
                foreach (string to in TO)
                {
                    if (mess.To.Length > 0)
                        mess.To += ";";

                    mess.To += to;

                }
                System.Web.Mail.SmtpMail.Send(mess);

                return true;
            }
            catch (Exception ex)
            {
                return SendEmail02(TO, SUBJECT, MESS);
            }
        }
        public bool SendEmail02(string[] TO, string SUBJECT, string MESS)
        {
            try
            {
                System.Web.Mail.SmtpMail.SmtpServer = ConfigurationManager.AppSettings["EMAIL_SERVER02"];


                System.Net.Mail.MailAddress fromAddress = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["EMAIL02"], "ChuongDuong");
                string fromPassword = ConfigurationManager.AppSettings["PASSWORD"].ToString();
                string subject = SUBJECT;
                string body = MESS;

                var smtp = new System.Net.Mail.SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["EMAIL_SERVER02"].ToString(),
                    Port = int.Parse(ConfigurationManager.AppSettings["SERVER_EMAIL_PORT"].ToString()),
                    EnableSsl = true,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
                };
                var message = new MailMessage();
                message.From = fromAddress;
                foreach (string to in TO)
                {
                    if (to.Trim().Length > 0)
                        message.To.Add(to);
                }
                message.Bcc.Add("truonghoangthiennhan@gmail.com");

                message.IsBodyHtml = true;
                message.Subject = subject;
                message.Body = body;

                smtp.Send(message);

                return true;
            }
            catch (Exception ex) { ERR_MESS = ex.Message; return false; }
        }
    }
}
