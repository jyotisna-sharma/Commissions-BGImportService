using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract] 
    interface IReport
    {
        [OperationContract]
        List<Report> GetReports();

        [OperationContract]
        void SavePayeeStatementReport(PayeeStatementReport report);

        [OperationContract]
        PrintReportOutput PrintReport(Guid Id, string reportType, string Format);

        [OperationContract]
        void SaveAuditReport(AuditReport report);

        [OperationContract]
        void SaveManagementReport(ManagementReport report);

        [OperationContract]
        bool PrintReportAndSendMail(Guid Id, string reportType,Guid userId, string emailAddress = "");
    }
    public partial class MavService : IReport
    {
        public List<Report> GetReports()
        {
            return Report.GetReports();
        }

        public void SavePayeeStatementReport(PayeeStatementReport report)
        {
            Report.SavePayeeStatementReport(report);
        }

        public PrintReportOutput PrintReport(Guid Id, string reportType, string Format)
        {
            return Report.PrintReport(Id, reportType, Format);
        }

        public void SaveAuditReport(AuditReport report)
        {
            Report.SaveAuditReport(report);
        }

        public void SaveManagementReport(ManagementReport report)
        {
            Report.SaveManagementReport(report);
        }

        public bool PrintReportAndSendMail(Guid Id, string reportType, Guid userId, string EmailAddress = "")
        {
            return Report.PrintReportAndSendMail(Id, reportType, userId, EmailAddress);
        }
    }
}
