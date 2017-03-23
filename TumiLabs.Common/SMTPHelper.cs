
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TumiLabs.Common;

namespace TumiLabs.Common
{
    public class SMTPHelper
    {
        //private readonly string _specifiedPickupDirectory = string.Empty;
        private readonly StringDictionary _specifiedPickupDirectory;
        private readonly SmtpClient _smtpClient;

        public SMTPHelper(StringDictionary specifiedPickupDirectory)
        {
            _specifiedPickupDirectory = specifiedPickupDirectory;

            _smtpClient = new SmtpClient();
            if (specifiedPickupDirectory == null || specifiedPickupDirectory.Count == 0)
            {
                //string host = SPAdministrationWebApplication.Local.OutboundMailServiceInstance.Server.Address;
                //_smtpClient.Host = host;
                //_smtpClient.UseDefaultCredentials = true;
            }
            else
            {
                if (specifiedPickupDirectory.ContainsKey("Host"))
                    _smtpClient.Host = specifiedPickupDirectory["Host"];

                if (specifiedPickupDirectory.ContainsKey("Port"))
                {
                    int intPuerto = -1;
                    if (int.TryParse(specifiedPickupDirectory["Port"], out intPuerto))
                        _smtpClient.Port = intPuerto;
                }


                if (specifiedPickupDirectory.ContainsKey("FromEmail") && specifiedPickupDirectory.ContainsKey("FromPass"))
                {
                    _smtpClient.Credentials = new NetworkCredential(specifiedPickupDirectory["FromEmail"], specifiedPickupDirectory["FromPass"]);
                }

                if (specifiedPickupDirectory.ContainsKey("EnableSsl"))
                {
                    bool bEnableSsl = false;
                    if (bool.TryParse(specifiedPickupDirectory["EnableSsl"], out bEnableSsl))
                        _smtpClient.EnableSsl = bEnableSsl;
                }

                //_smtpClient.PickupDirectoryLocation = _specifiedPickupDirectory;
                //_smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            }
        }

        public bool SendEmail(List<string> destinatarios, List<string> destinatariosCC, string subject, string body, bool IsBodyHtml = true)
        {
            string strEmail = string.Empty;
            string strEmailCopiaA = string.Empty;

            if (!_specifiedPickupDirectory.ContainsKey("FromEmail"))
            {
                //strEmail = SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
            }
            else
                strEmail = _specifiedPickupDirectory["FromEmail"];

            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            try
            {
                MailAddress address = new MailAddress(strEmail);
                mailMessage.From = address;
            }
            catch (Exception ex)
            {
                //Utils.WriteTextLog(string.Format("Server:{0}, Port: {1},Credentials ({2}):({3}, EnableSsl: {4})", _smtpClient.Host, _smtpClient.Port, _specifiedPickupDirectory["FromEmail"], _specifiedPickupDirectory["FromPass"], _smtpClient.EnableSsl));
                return false;
            }



            if (destinatarios != null)
            {
                for (int i = 0; i < destinatarios.Count; i++)
                    mailMessage.To.Add(destinatarios[i]);
            }

            if (destinatariosCC != null)
            {
                for (int i = 0; i < destinatariosCC.Count; i++)
                    mailMessage.CC.Add(destinatariosCC[i]);
            }

            string EmailNotificacionCopiaCarbon = ConfigurationManager.AppSettings["EmailNotificacionCopiaCarbon"];
            if (!string.IsNullOrEmpty(EmailNotificacionCopiaCarbon))
            {
                if (Utils.isValidEmail(EmailNotificacionCopiaCarbon))
                {
                    mailMessage.CC.Add(EmailNotificacionCopiaCarbon);
                }
            }

            if (!string.IsNullOrEmpty(strEmailCopiaA))
                mailMessage.CC.Add(strEmailCopiaA);

            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = IsBodyHtml;
            mailMessage.Body = body;
            mailMessage.BodyEncoding = Encoding.UTF8;

            try
            {
                _smtpClient.Send(mailMessage);
                //Utils.WriteTextLog(">>To:" + mailMessage.To + " >>CC: " + mailMessage.CC + ">>subject:" + subject + " >>body: " + body, "SMTPHelper.cs");
            }
            catch (Exception ex)
            {
                //Utils.WriteTextLog(">>To:" + mailMessage.To + " >>CC: " + mailMessage.CC + ">>subject:" + subject + " >>body: " + body + ">>Error:" + ex.ToString(), "SMTPHelper.cs");
                return false;
            }
            return true;
        }
    }
}
