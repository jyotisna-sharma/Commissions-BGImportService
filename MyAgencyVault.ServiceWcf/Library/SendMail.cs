using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;


namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ISendMail
    {
        [OperationContract()]
        bool SendMail(string toAddress, string subject, string body);

        [OperationContract()]
        bool SendMailToDev(string toAddress, string subject, string body);

        [OperationContract()]
        string GetAlertCommissionDepartmentMailId();
    }

    public partial class MavService : ISendMail
    {
        public bool SendMail(string toAddress, string subject, string body)
        {
            return MailServerDetail.sendMail(toAddress, subject, body);
        }

        public bool SendMailToDev(string toAddress, string subject, string body)
        {
            return MailServerDetail.sendMailtodev(toAddress, subject, body);
        }

        public string GetAlertCommissionDepartmentMailId()
        {
            return MailServerDetail.GetAlertCommissionDepartmentMail();
        }
    }
}