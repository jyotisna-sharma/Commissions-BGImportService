using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace MyAgencyVault.EmailFax
{
    public class EmailFax
    {
        public string From = string.Empty;
        public string To = string.Empty;
        public string User = string.Empty;
        public string Password = string.Empty;
        public string Subject = string.Empty;
        public string Body = string.Empty;
        public string AttachmentPath = string.Empty;
        public string Host = "127.0.0.1";
        public int Port = 587;
        public string CC = string.Empty;
        public string BCC = string.Empty;
        public bool IsHtml = true;
        public int SendUsing = 0;//0 = Network, 1 = PickupDirectory, 2 = SpecifiedPickupDirectory
        public bool UseSSL = true;
        public int AuthenticationMode = 1;//0 = No authentication, 1 = Plain Text, 2 = NTLM authentication

        public EmailFax(string UserName,string Passowrd,string FromEmail,string ToEmail,string HostName,string PortNo,string MailSubject,string MailBody)
        {
            User = UserName;
            Password = Passowrd;
            From = FromEmail;
            To = ToEmail;            
            Host = HostName;
            Port = Convert.ToInt32(PortNo);
            Subject = MailSubject;
            Body = MailBody;
        }
        public void SendEmail()
        {
            new Thread(new ThreadStart(SendMessage)).Start();
        }
        /// <summary>
        /// Send Email Message method.
        /// </summary>
        private void SendMessage()
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("Send message started: To - " + To, true);
                MailMessage oMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(Host);

                oMessage.From = new MailAddress(From,"service@commissionsdept.com");
                oMessage.To.Add(To);
                oMessage.Subject = Subject;
                oMessage.IsBodyHtml = IsHtml;
                oMessage.Body = Body;

                if (!string.IsNullOrEmpty(CC)) // (CC != string.Empty)
                    oMessage.CC.Add(CC);

                if (!string.IsNullOrEmpty(BCC))
                    oMessage.Bcc.Add(BCC);

                switch (SendUsing)
                {
                    case 0:
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        break;
                    case 1:
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                        break;
                    case 2:
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        break;
                    default:
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        break;

                }
                if (AuthenticationMode > 0)
                {
                    smtpClient.Credentials = new NetworkCredential(User, Password);
                }

                smtpClient.Port = Port;
                smtpClient.EnableSsl = UseSSL;
                // Create and add the attachment
                if (AttachmentPath != string.Empty)
                {
                    oMessage.Attachments.Add(new Attachment(AttachmentPath));
                }

                try
                {
                    // Deliver the message    
                    smtpClient.Send(oMessage);
                    ActionLogger.Logger.WriteImportPolicyLog("Send message Success : To - " + To, true);
                }

                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("Exception in Send message 1 : To - " + To + ", ex: " + ex.Message, true);
                   // ex.ToString();
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception in Send message 2: To - " + To + ", ex: " + ex.Message, true);
               // ex.ToString();
            }
        }
    }
}
