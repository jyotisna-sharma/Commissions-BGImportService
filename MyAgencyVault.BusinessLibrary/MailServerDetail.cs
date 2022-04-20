using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using MyAgencyVault.EmailFax;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class MailServerDetail
    {
        [DataMember]
        public string ServerName { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string PortNo { get; set; }

        public static MailServerDetail getMailServerDetail()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                MailServerDetail mail = new MailServerDetail();

                mail.ServerName = (from p in DataModel.MasterSystemConstants
                 where p.Name == "MailServerName"
                 select p.Value).FirstOrDefault();

                mail.Email = (from p in DataModel.MasterSystemConstants
                                 where p.Name == "MailServerEMail"
                                 select p.Value).FirstOrDefault();

                mail.UserName = (from p in DataModel.MasterSystemConstants
                                 where p.Name == "MailServerUserName"
                                 select p.Value).FirstOrDefault();

                mail.Password = (from p in DataModel.MasterSystemConstants
                                 where p.Name == "MailServerPassword"
                                 select p.Value).FirstOrDefault();

                mail.PortNo = (from p in DataModel.MasterSystemConstants
                                 where p.Name == "MailServerPortNo"
                                 select p.Value).FirstOrDefault();

                return mail;
            }
        }
        #region sendemail from dev

        // <--------------------------------------------------------------------------------------------------->
        /// <summary>
        /// Author:Ankit khandelwal
        /// Created on: 14-11-2018
        /// Purpose:Get the Server details from app config file.
        /// </summary>
        /// <returns></returns>
        public static MailServerDetail getDevMailServerDetail()
        {
            MailServerDetail mail = new MailServerDetail();
            mail.ServerName = System.Configuration.ConfigurationSettings.AppSettings["DevMailServerName"];
            mail.Email = System.Configuration.ConfigurationSettings.AppSettings["DevMailServerEMail"];
            mail.UserName = System.Configuration.ConfigurationSettings.AppSettings["DevMailServerUserName"];
            mail.Password = System.Configuration.ConfigurationSettings.AppSettings["DevMailServerPassword"];
            mail.PortNo = System.Configuration.ConfigurationSettings.AppSettings["DevMailServerPortNo"];
            return mail;
        }

        // <--------------------------------------------------------------------------------------------------->
        /// <summary>
        /// Author:Ankit khandelwal
        /// Created on: 14-11-2018
        /// Purpose:Get the Server details from app config file.
        /// </summary>
        /// <returns></returns>
        public static MailServerDetail getAllMailServerDetail()
        {
            MailServerDetail mail = new MailServerDetail();
            mail.ServerName = System.Configuration.ConfigurationSettings.AppSettings["AllMailServerName"];
            mail.Email = System.Configuration.ConfigurationSettings.AppSettings["AllMailServerEMail"];
            mail.UserName = System.Configuration.ConfigurationSettings.AppSettings["AllMailServerUserName"];
            mail.Password = System.Configuration.ConfigurationSettings.AppSettings["AllMailServerPassword"];
            mail.PortNo = System.Configuration.ConfigurationSettings.AppSettings["AllMailServerPortNo"];
            return mail;
        }
        /// <summary>
        /// Author:Ankit khandelwal 
        /// Created on:14-11-2018
        /// Purpose:to send a mail from testemail
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool sendMailtodev(string toAddress, string subject, string body)
        {
            MailServerDetail mInfo = getDevMailServerDetail();

            if (string.IsNullOrEmpty(toAddress))
                toAddress = mInfo.Email;

            bool mailSendSuccessfully = true;

            try
            {
                EmailFax.EmailFax wpfMail = new EmailFax.EmailFax(mInfo.UserName, mInfo.Password, mInfo.Email, toAddress, mInfo.ServerName, mInfo.PortNo, subject, body);
                wpfMail.SendEmail();
            }
            catch
            {
                mailSendSuccessfully = false;
            }

            return mailSendSuccessfully;
        }

        // <--------------------------------------------------------------------------------------------------->
        #endregion

        public static bool sendMail(string toAddress, string subject, string body)
        {
            MailServerDetail mInfo = getAllMailServerDetail();

            if (string.IsNullOrEmpty(toAddress))
                toAddress = mInfo.Email;

            bool mailSendSuccessfully = true;

            try
            {
                EmailFax.EmailFax wpfMail = new EmailFax.EmailFax(mInfo.UserName, mInfo.Password, mInfo.Email, toAddress, mInfo.ServerName, mInfo.PortNo, subject, body);
                wpfMail.SendEmail();
            }
            catch
            {
                mailSendSuccessfully = false;
            }

            return mailSendSuccessfully;
        }

        /// <summary>
        /// Send mail to benefits team on every new payor/carrier product add
        /// </summary>
        /// <param name="Type">Payor/Carrier/Product</param>
         /// <param name="body"></param>
        /// <returns></returns>
        public static bool SendMailToBenefits(string Type, string body)
        {
            MailServerDetail mInfo = getMailServerDetail();
            string toAddress = System.Configuration.ConfigurationSettings.AppSettings["Benefits_Email"];

            string subject = "New " + Type + " added to Commissions Department";
            
            bool mailSendSuccessfully = true;

            try
            {
                EmailFax.EmailFax wpfMail = new EmailFax.EmailFax(mInfo.UserName, mInfo.Password, mInfo.Email, toAddress, mInfo.ServerName, mInfo.PortNo, subject, body);
                wpfMail.IsHtml = true;
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["Benefits_Email_BCC"]))
                {
                    wpfMail.BCC = System.Configuration.ConfigurationSettings.AppSettings["Benefits_Email_BCC"];
                }
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["Benefits_Email_CC"]))
                {
                    wpfMail.CC = System.Configuration.ConfigurationSettings.AppSettings["Benefits_Email_CC"];
                }
                wpfMail.SendEmail();
            }
            catch
            {
                mailSendSuccessfully = false;
            }

            return mailSendSuccessfully;
        }

        public static bool sendMailWithAttachment(string toAddress, string subject, string body,string fileName)
        {
            MailServerDetail mInfo = getDevMailServerDetail();
            

            if (string.IsNullOrEmpty(toAddress))
                toAddress = mInfo.Email;

            bool mailSendSuccessfully = true;

            try
            {
                EmailFax.EmailFax wpfMail = new EmailFax.EmailFax(mInfo.UserName, mInfo.Password, mInfo.Email, toAddress, mInfo.ServerName, mInfo.PortNo, subject, body);
                wpfMail.IsHtml = true;
                wpfMail.BCC = System.Configuration.ConfigurationManager.AppSettings["ReportBCC"];
                wpfMail.AttachmentPath = fileName;
                wpfMail.SendEmail();
               
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Failure sending mail: " + ex.Message, true);
                mailSendSuccessfully = false;
            }

            return mailSendSuccessfully;
            
        }

        public static string GetAlertCommissionDepartmentMail()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                DLinq.MasterSystemConstant Constant = (from s in DataModel.MasterSystemConstants
                                                       where s.Name == "AlertCommissionDeptMailId"
                                                       select s).FirstOrDefault();
                return Constant.Value;
            }
        }
    }
}
