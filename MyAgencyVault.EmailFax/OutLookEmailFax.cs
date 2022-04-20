using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Net.Mime;
using System.Diagnostics;


namespace MyAgencyVault.EmailFax
{
    public class MailData
    {
        public string ReceiverName { get; set; }
        public string CarrierName { get; set; }
        public string ClientName { get; set; }
        public string PolicyNumber { get; set; }
        public string Product { get; set; }
        public string InvoiceDate { get; set; }
        public string Category { get; set; }
        public string AgencyName { get; set; }
        public string CommDeptMail { get; set; }
        public string CommDeptFaxNumber { get; set; }
        public string CommDeptPhoneNumber { get; set; }
        public string TrackNumber { get; set; }
        public string Created { get; set; }
        public string MailLogoPath { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }
        public string Password { get; set; }
        public string ToMail { get; set; }
        public string FromMail { get; set; }
        public string UserName { get; set; }


    }

    public class OutLookEmailFax
    {
        public MailData EmailContentdata = null;

        public OutLookEmailFax(MailData emailContentData)
        {
            EmailContentdata = emailContentData;
        }
        public void SendEmailWithAttachment()
        {
            new Thread(new ThreadStart(SendMessageWithAttachment)).Start();
        }
              
        private void SendMessageWithAttachment()
        {
            NetworkCredential cred = new NetworkCredential(EmailContentdata.UserName, EmailContentdata.Password);
            if (EmailContentdata.ToMail != null)
            {
                MailMessage mailMessage = new MailMessage(EmailContentdata.FromMail, EmailContentdata.ToMail);
                mailMessage.CC.Add(EmailContentdata.CommDeptMail);
                mailMessage.Subject = "CommissionDept Follow-Up Service: " + EmailContentdata.ClientName + " Policy # " + EmailContentdata.PolicyNumber;
                // string path = System.Windows.Forms.Application.StartupPath;
                string path = "c:\\" + EmailContentdata.MailLogoPath;
                // LinkedResource logo = new LinkedResource(path);
                //logo.ContentId = "Commission Dept";
                string MailBody = "<table style='font-family: Tahoma; font-size: 12px; width: 100%; height: 100%' " +
                    "cellpadding='0'cellspacing='0' baorder='1' bordercolor='red'><tr><td colspan='2'>Dear " +
                EmailContentdata.ReceiverName +
                    "</td></tr><tr><td colspan='2'>" +
                    "&nbsp;</td></tr><tr><td colspan='2'>Please advise on the " +
                EmailContentdata.CarrierName +
                    " policy below if there is an error in payment." +
                    "</td></tr><tr><td colspan='2'>&nbsp;</td></tr><tr><td colspan='2'>Client: <span style='padding-left: 50px'><b>" +
                EmailContentdata.ClientName +
                    "</b></span></td></tr><tr><td colspan='2'>Policy: <span style='padding-left: 50px'><b>" +
                EmailContentdata.PolicyNumber +
                    "</b></span></td></tr><tr>" +
                "<td colspan='2'>Product: <span style='padding-left: 50px'><b>" +
                EmailContentdata.Product +
                    "</b></span></td></tr><tr><td colspan='2'>Invoice: " +
                "<span style='padding-left: 50px'><b>" +
              EmailContentdata.InvoiceDate +
                    "</b></span></td></tr><tr><td colspan='2'>Issue: <span style='padding-left: 50px'><b>" +
                EmailContentdata.Category +
                "</b></span></td></tr><tr><td colspan='2'>&nbsp;</td></tr><tr><td colspan='2'>We appreciate your help in this manner.</td></tr><tr><td colspan='2'>" +
                "&nbsp;</td></tr><tr><td colspan='2'>Regards,</td></tr><tr><td colspan='2'>&nbsp;</td></tr><tr><td colspan='2'>CommissionsDept Follow-Up Team on behalf of " +
                EmailContentdata.AgencyName +
                "</td></tr><tr><td colspan='2'>Email: " +
                EmailContentdata.CommDeptMail +
                    "</td></tr><tr><td colspan='2'>Fax: " +
                EmailContentdata.CommDeptFaxNumber +
                    "</td></tr><tr><td colspan='2'>Phone: " +
                EmailContentdata.CommDeptPhoneNumber +
                "</td></tr><tr><td colspan='2'>&nbsp;</td></tr><tr><td colspan='2'><img src=cid:companylogo alt='' /></td></tr><tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2'>Follow-Up Tracking # " +
                EmailContentdata.TrackNumber +
                    "</td></tr><tr><td colspan='2'>Created: " +
                EmailContentdata.Created +
                    "</td></tr></table>";
                AlternateView av = AlternateView.CreateAlternateViewFromString(MailBody, null, MediaTypeNames.Text.Html);
                // av.LinkedResources.Add(logo);
                mailMessage.AlternateViews.Add(av);
                mailMessage.IsBodyHtml = true;
                SmtpClient mailClient = new SmtpClient(EmailContentdata.HostName, Convert.ToInt32(EmailContentdata.Port));
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.UseDefaultCredentials = false;
                mailClient.Timeout = 20000;
                mailClient.Credentials = cred;
                mailClient.Send(mailMessage);
                // OutLookLaunch();
            }

        }

        public void OutLookLaunch()
        {
            try
            {
                string path = "c:\\" + EmailContentdata.MailLogoPath;
                LinkedResource logo = new LinkedResource(path);
                logo.ContentId = "Commission Dept";
                string MailBody = "%0table style='font-family: Tahoma; font-size: 12px; width: 100%; height: 100%' " +
                    "cellpadding='0'cellspacing='0' baorder='1' bordercolor='red'%0tr%0td colspan='2'Dear " +
                EmailContentdata.ReceiverName +
                    "%0td%0tr%0tr%0td colspan='2'" +
                    "&nbsp;%0td%0tr%0tr%0td colspan='2'Please advise on the " +
                EmailContentdata.CarrierName +
                    " policy below if there is an error in payment." +
                    "%0td%0tr%0tr%0td colspan='2'&nbsp;%0td%0tr%0tr%0td colspan='2'Client: %0span style='padding-left: 50px'%0b" +
                EmailContentdata.ClientName +
                    "%0b%0span%0td%0tr%0tr%0td colspan='2'Policy: %0span style='padding-left: 50px'%0b" +
                EmailContentdata.PolicyNumber +
                    "%0b%0span%0td%0tr%0tr" +
                "%0td colspan='2'Product: %0span style='padding-left: 50px'%0b" +
                EmailContentdata.Product +
                    "%0b%0span%0td%0tr%0tr%0td colspan='2'Invoice: " +
                "%0span style='padding-left: 50px'%0b" +
              EmailContentdata.InvoiceDate +
                    "%0b%0span%0td%0tr%0tr%0td colspan='2'Issue: %0span style='padding-left: 50px'%0b" +
                EmailContentdata.Category +
                "%0b%0span%0td%0tr%0tr%0td colspan='2'&nbsp;%0td%0tr%0tr%0td colspan='2'We appreciate your help in this manner.%0td%0tr%0tr%0td colspan='2'" +
                "&nbsp;%0td%0tr%0tr%0td colspan='2'Regards,%0td%0tr%0tr%0td colspan='2'&nbsp;%0td%0tr%0tr%0td colspan='2'CommissionsDept Follow-Up Team on behalf of " +
                EmailContentdata.AgencyName +
                "%0td%0tr%0tr%0td colspan='2'Email: " +
                EmailContentdata.CommDeptMail +
                    "%0td%0tr%0tr%0td colspan='2'Fax: " +
                EmailContentdata.CommDeptFaxNumber +
                    "%0td%0tr%0tr%0td colspan='2'Phone: " +
                EmailContentdata.CommDeptPhoneNumber +
                "%0td%0tr%0tr%0td colspan='2'&nbsp;%0td%0tr%0tr%0td colspan='2'%0img src=cid:companylogo alt='' /%0td%0tr%0tr%0td colspan='2'&nbsp;%0td%0tr" +
                "%0tr%0td colspan='2'Follow-Up Tracking # " +
                EmailContentdata.TrackNumber +
                    "%0td%0tr%0tr%0td colspan='2'Created: " +
                EmailContentdata.Created +
                    "%0td%0tr%0table";
                StringBuilder MsgBilder = new StringBuilder();
                MsgBilder.Append("mailto:" + EmailContentdata.ToMail);

                MsgBilder.Append("&subject= CommissionDept Follow-Up Service: " + EmailContentdata.ClientName + " Policy # " + EmailContentdata.PolicyNumber);
                //  MsgBilder.Append("&body=%0b" + EmailContentdata.Product + "%0b");
                MsgBilder.Append("&body=" + MailBody);

                // string str = @"mailto:"+EmailContentdata.ToMail+"?subject=" + "CommissionDept Follow-Up Service: " + EmailContentdata.ClientName + " Policy # " + EmailContentdata.PolicyNumber + "&body=" + MailBody;
                Process.Start(MsgBilder.ToString());
            }
            catch
            {
            }
        }

        public void OutLookLaunch(MailData mailData, string strMailBody)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential(EmailContentdata.UserName, EmailContentdata.Password);
                if (EmailContentdata.ToMail != null)
                {
                    MailMessage mailMessage = new MailMessage(EmailContentdata.FromMail, EmailContentdata.ToMail);
                    //mailMessage.CC.Add(EmailContentdata.CommDeptMail);
                    mailMessage.Subject = "Carrier and/or Product not found" + EmailContentdata.ClientName + " Policy # " + EmailContentdata.PolicyNumber;
                    mailMessage.CC.Add("service@commissionsdept.com");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strMailBody, null, MediaTypeNames.Text.Html);
                    mailMessage.AlternateViews.Add(av);
                    mailMessage.IsBodyHtml = true;
                    SmtpClient mailClient = new SmtpClient(EmailContentdata.HostName, Convert.ToInt32(EmailContentdata.Port));
                    mailClient.EnableSsl = true;
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Timeout = 20000;
                    mailClient.Credentials = cred;
                    mailClient.Send(mailMessage);
                }
            }
            catch
            {
            }

        }

        public void SendMailToServiceDepartment(MailData mailData, string strMailBody)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential(EmailContentdata.UserName, EmailContentdata.Password);
                if (EmailContentdata.ToMail != null)
                {
                    MailMessage mailMessage = new MailMessage(mailData.FromMail, mailData.ToMail);
                    mailMessage.Subject = "Carrier and/or Product not found" + mailData.ClientName + " Policy # " + mailData.PolicyNumber;
                    mailMessage.CC.Add("service@commissionsdept.com");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strMailBody, null, MediaTypeNames.Text.Html);
                    mailMessage.AlternateViews.Add(av);
                    mailMessage.IsBodyHtml = true;
                    SmtpClient mailClient = new SmtpClient(mailData.HostName, Convert.ToInt32(mailData.Port));
                    mailClient.EnableSsl = true;
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Timeout = 20000;
                    mailClient.Credentials = cred;
                    mailClient.Send(mailMessage);
                }
            }
            catch
            {
            }

        }

        //Send mail to when upload batch
        public void SendMailToUpload(MailData mailData, string strMailBody)
        {
            try
            {
               
                NetworkCredential cred = new NetworkCredential(EmailContentdata.UserName, EmailContentdata.Password);

                if (EmailContentdata.ToMail != null)
                {
                    MailMessage mailMessage = new MailMessage(mailData.FromMail, mailData.ToMail);
                    mailMessage.Subject = "Upload batch in " + mailData.AgencyName;
                    mailMessage.CC.Add("service@commissionsdept.com");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strMailBody, null, MediaTypeNames.Text.Html);
                    mailMessage.AlternateViews.Add(av);
                    mailMessage.IsBodyHtml = true;
                    SmtpClient mailClient = new SmtpClient(mailData.HostName, Convert.ToInt32(mailData.Port));
                    mailClient.EnableSsl = true;
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Timeout = 20000;
                    mailClient.Credentials = cred;
                    mailClient.Send(mailMessage);
                }
            }
            catch
            {
            }

        }
        //Send Remainder mail
        public void SendRemaiderMail(MailData mailData, string strMailBody, DateTime dtMailDate)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential(EmailContentdata.UserName, EmailContentdata.Password);

                if (EmailContentdata.ToMail != null)
                {
                    MailMessage mailMessage = new MailMessage(mailData.FromMail, mailData.ToMail);
                   // mailMessage.Subject = "CommissionsDept Statement PDF Upload reminder - Monthly statements due by " + Convert.ToString(dtMailDate.DayOfWeek) + ", " + dtMailDate.ToShortDateString();
                    mailMessage.CC.Add("service@commissionsdept.com");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strMailBody, null, MediaTypeNames.Text.Html);
                    mailMessage.AlternateViews.Add(av);
                    mailMessage.IsBodyHtml = true;
                    SmtpClient mailClient = new SmtpClient(mailData.HostName, Convert.ToInt32(mailData.Port));
                    mailClient.EnableSsl = true;
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Timeout = 20000;
                    mailClient.Credentials = cred;
                    mailClient.Send(mailMessage);
                }
            }
            catch
            {
            }

        }

        //Send Mail When Closed Batch
        public void SendMailToCloseBatch(MailData mailData, string strBatchNaumber, string strMailBody)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential(EmailContentdata.UserName, EmailContentdata.Password);
                if (EmailContentdata.ToMail != null)
                {
                    MailMessage mailMessage = new MailMessage(EmailContentdata.FromMail, EmailContentdata.ToMail);
                    mailMessage.Subject = "Batch Closed: " + strBatchNaumber;
                    mailMessage.CC.Add("service@commissionsdept.com");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strMailBody, null, MediaTypeNames.Text.Html);
                    mailMessage.AlternateViews.Add(av);
                    mailMessage.IsBodyHtml = true;
                    SmtpClient mailClient = new SmtpClient(EmailContentdata.HostName, Convert.ToInt32(EmailContentdata.Port));
                    mailClient.EnableSsl = true;
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Timeout = 20000;
                    mailClient.Credentials = cred;
                    mailClient.Send(mailMessage);
                }
            }
            catch
            {
            }
        }

        //Send Mail When Closed Batch
        public void SendLinkedPolicyConfirmationMail(MailData mailData, string strMailBody, string strPendingPolicy, string strActivePolicy)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential(EmailContentdata.UserName, EmailContentdata.Password);

                if (EmailContentdata.ToMail != null)
                {
                    MailMessage mailMessage = new MailMessage(mailData.FromMail, mailData.ToMail);
                    mailMessage.Subject = "Confirmation message when policy is linked to active policy";
                    mailMessage.CC.Add("service@commissionsdept.com");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strMailBody, null, MediaTypeNames.Text.Html);
                    mailMessage.AlternateViews.Add(av);
                    mailMessage.IsBodyHtml = true;
                    SmtpClient mailClient = new SmtpClient(mailData.HostName, Convert.ToInt32(mailData.Port));
                    mailClient.EnableSsl = true;
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Timeout = 20000;
                    mailClient.Credentials = cred;
                    mailClient.Send(mailMessage);
                }
            }
            catch
            {
            }

        }

        //Send Mail When Closed Batch
        public void SendLoginLogoutMail(MailData mailData, string strSubject, string strMailBody)
        {
            try
            {
              
                NetworkCredential cred = new NetworkCredential(EmailContentdata.UserName, EmailContentdata.Password);
                if (EmailContentdata.ToMail != null)
                {
                    MailMessage mailMessage = new MailMessage(EmailContentdata.FromMail, EmailContentdata.ToMail);
                    mailMessage.Subject = strSubject;
                    mailMessage.CC.Add("service@commissionsdept.com");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strMailBody, null, MediaTypeNames.Text.Html);
                    mailMessage.AlternateViews.Add(av);
                    mailMessage.IsBodyHtml = true;
                    SmtpClient mailClient = new SmtpClient(EmailContentdata.HostName, Convert.ToInt32(EmailContentdata.Port));
                    mailClient.EnableSsl = true;
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Timeout = 20000;
                    mailClient.Credentials = cred;
                    mailClient.Send(mailMessage);
                }
            }
            catch
            {
            }
        }

        //Send Mail When Closed Batch
        public void SaveNotifyMail(MailData mailData, string strSubject, string strMailBody)
        {

            try
            {
               
                NetworkCredential cred = new NetworkCredential(EmailContentdata.UserName, EmailContentdata.Password);

                if (EmailContentdata.ToMail != null)
                {
                    MailMessage mailMessage = new MailMessage(mailData.FromMail, mailData.ToMail);
                    mailMessage.Subject = strSubject;
                    //mailMessage.CC.Add("service@commissionsdept.com");
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strMailBody, null, MediaTypeNames.Text.Html);
                    mailMessage.AlternateViews.Add(av);
                    mailMessage.IsBodyHtml = true;
                    SmtpClient mailClient = new SmtpClient(EmailContentdata.HostName, Convert.ToInt32(EmailContentdata.Port));
                    mailClient.EnableSsl = true;
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Timeout = 20000;
                    mailClient.Credentials = cred;
                    mailClient.Send(mailMessage);
                }
            }
            catch(Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog(ex.ToString() + DateTime.Now.ToLongTimeString(), true);
            }
        }

        //Used in import tool to send notification mail to eric
        public void SendNotificationMail(MailData mailData, string strSubject, string strMailBody)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential(EmailContentdata.UserName, EmailContentdata.Password);
                if (EmailContentdata.ToMail != null)
                {
                    MailMessage mailMessage = new MailMessage(mailData.FromMail, mailData.ToMail);                   
                    mailMessage.Subject = strSubject;
                    mailMessage.CC.Add("service@commissionsdept.com");
                    mailMessage.CC.Add("kwilson@commissionsdept.com");
                   
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strMailBody, null, MediaTypeNames.Text.Html);
                    mailMessage.AlternateViews.Add(av);
                    mailMessage.IsBodyHtml = true;
                    SmtpClient mailClient = new SmtpClient(EmailContentdata.HostName, Convert.ToInt32(EmailContentdata.Port));
                    mailClient.EnableSsl = true;
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailClient.UseDefaultCredentials = false;
                    mailClient.Timeout = 80000;
                    mailClient.Credentials = cred;
                    mailClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Issue in Import tool notification" + ex.ToString(), true);
            }
        }

    }
}
