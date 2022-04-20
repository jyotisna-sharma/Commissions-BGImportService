using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;
using MyAgencyVault.EmailFax;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IFollowupIssue
    {
        [OperationContract]
        void test();

        [OperationContract]
        List<DisplayFollowupIssue> GetIssues(Guid policyID);

        [OperationContract]
        List<DisplayFollowupIssue> GetAllIssues(int Status, Guid PayorID, Guid AgencyID, bool Followup);

        [OperationContract]
        List<IssuePolicyDetail> GetIssueDetail(Guid PolicyId, Guid FollowUpIssueid);

        [OperationContract]
        List<FollowupIncomingPament> GetIncomingPayment(Guid PolicyId);

        [OperationContract]
        string GetIssuesNote(Guid IssueID);

        [OperationContract]
        List<FollowUPPayorContacts> GetPayorContact(Guid PolicyId);

        [OperationContract]
        void AddUpdatePolicyIssueNotes(DisplayFollowupIssue followupiss);

        [OperationContract]
        void AddUpdatePolicyIssueNotesScr(Guid ModifiedBy, Guid IssueId, string PolicyIssueNote);

        [OperationContract]
        void AddUpdatePolicyIssuePayment(DisplayFollowupIssue followupiss);

        [OperationContract]
        void AddUpdateIssue(DisplayFollowupIssue followupiss);

        [OperationContract]
        string CheckrefershButton(Guid policyId);

        [OperationContract]
        void AddPaymentData(Guid IssueId, decimal Payment);

        [OperationContract]
        void EmailToAgencyPayor(MailData _MailData);

        [OperationContract]
        void SendCloseStatemant(MailData _MailData, string strMailBody);

        [OperationContract]
        void SendMailToCloseBatch(MailData _MailData, string strBatchNumber, string strMailBody);

        [OperationContract]
        void SendMailOfCarrierProduct(MailData _MailData, string strMailBody);

        [OperationContract]
        void SendMailToUpload(MailData _MailData, string strMailBody);

        [OperationContract]
        void SendRemainderMail(MailData _MailData, string strMailBody, DateTime dtCutOfDay);

        [OperationContract]
        void SendLoginLogoutMail(MailData _MailData, string strLoginLogoutType, string strMailBody);

        [OperationContract]
        void SendNotificationMail(MailData _MailData, string strSubject, string strMailBody);

        [OperationContract]
        void SendLinkedPolicyConfirmationMail(MailData _MailData, string strMailBody, string strPendingPolicy, string strActivePolicy);

        [OperationContract]
        List<DisplayFollowupIssue> GetFewIssueAccordingtoMode(int Status, Guid PayorID, Guid AgencyID, bool Followup, int intDays);

        [OperationContract]
        List<DisplayFollowupIssue> GetFewIssueAccordingtoModeScr(int Status, Guid PayorID, Guid AgencyID, bool Followup, int intDays);

        [OperationContract]
        List<DisplayFollowupIssue> GetFewIssueForCommissionDashBoard(Guid PolicyId);

        [OperationContract]
        MasterIssuesOption FillMasterIssueOptions();

        [OperationContract]
        void UpdateIssuesSrc(DisplayFollowupIssue _DisplayFollowupIssue, int PreviousStatusId, Guid ModifiedBy);

        [OperationContract]
        Guid? GetFollowUpIssueForPaymentEntry(PolicyPaymentEntriesPost PaymentEntry);

        [OperationContract]
        void DeleteIssue(Guid IssueID);

        // Remove issue from comision ddasboard, only i change the status of issue
        [OperationContract]
        void RemoveCommisionIssue(Guid IssueID);

    }

    public partial class MavService : IFollowupIssue
    {
        public MasterIssuesOption FillMasterIssueOptions()
        {
            return FollowupIssue.FillMasterIssueOptions();
        }
        public void AddPaymentData(Guid IssueId, decimal Payment)
        {
            FollowupIssue.AddPaymentData(IssueId, Payment);
        }

        public void test()
        {
            throw new NotImplementedException();
        }


        public List<DisplayFollowupIssue> GetIssues(Guid policyID)
        {
            return FollowupIssue.GetIssues(policyID);
        }

        public Guid? GetFollowUpIssueForPaymentEntry(PolicyPaymentEntriesPost PaymentEntry)
        {
            DisplayFollowupIssue issue = FollowupIssue.GetFollowupissueOfPayment(PaymentEntry);
            if (issue != null)
            {
                return issue.IssueId;
            }
            else
            {
                return null;
            }
        }
        public List<DisplayFollowupIssue> GetAllIssues(int Status, Guid PayorID, Guid AgencyID, bool Followup)
        {
            return FollowupIssue.GetAllIssues(Status, PayorID, AgencyID, Followup, 180);
        }

        public List<IssuePolicyDetail> GetIssueDetail(Guid PolicyId, Guid FollowUpIssueid)
        {
            return FollowupIssue.GetIssueDetail(PolicyId, FollowUpIssueid);
        }

        public List<FollowupIncomingPament> GetIncomingPayment(Guid PolicyId)
        {
            return FollowupIssue.GetIncomingPayment(PolicyId);
        }

        public string GetIssuesNote(Guid IssueID)
        {
            return FollowupIssue.GetIssuesNote(IssueID);
        }

        public List<FollowUPPayorContacts> GetPayorContact(Guid PolicyId)
        {
            return FollowupIssue.GetPayorContact(PolicyId);
        }
        public void AddUpdatePolicyIssueNotes(DisplayFollowupIssue followupiss)
        {
            FollowupIssue.AddUpdatePolicyIssueNotes(followupiss);
        }
        public void AddUpdatePolicyIssuePayment(DisplayFollowupIssue followupiss)
        {
            FollowupIssue.AddUpdatePolicyIssuePayment(followupiss);
        }
        public void AddUpdateIssue(DisplayFollowupIssue followupiss)
        {
            FollowupIssue.AddUpdate(followupiss);
        }
        public  string CheckrefershButton(Guid policyId)
        {
           string FollowupDate= FollowupIssue.CheckrefershButton(policyId);

            return FollowupDate;
        }
        #region IFollowupIssue Members


        public void EmailToAgencyPayor(MailData _MailData)
        {
            FollowupIssue objFollowupIssue = new FollowupIssue();
            objFollowupIssue.EmailToAgencyPayor(_MailData);
        }

        public void SendCloseStatemant(MailData _MailData, string strMailBody)
        {
            FollowupIssue objFollowupIssue = new FollowupIssue();
            objFollowupIssue.SendCloseStatemant(_MailData, strMailBody);
        }

        public void SendMailToCloseBatch(MailData _MailData, string strBatchNumber, string strMailBody)
        {
            FollowupIssue objFollowupIssue = new FollowupIssue();
            objFollowupIssue.SendMailToCloseBatch(_MailData, strBatchNumber, strMailBody);
        }

        public void SendLoginLogoutMail(MailData _MailData, string strLoginLogoutType, string strMailBody)
        {
            FollowupIssue objFollowupIssue = new FollowupIssue();
            objFollowupIssue.SendLoginLogoutMail(_MailData, strLoginLogoutType, strMailBody);
        }

        public void SendMailOfCarrierProduct(MailData _MailData, string strMailBody)
        {
            FollowupIssue objFollowupIssue = new FollowupIssue();
            objFollowupIssue.SendMailOfCarrierProduct(_MailData, strMailBody);
        }

        public void SendMailToUpload(MailData _MailData, string strMailBody)
        {
            FollowupIssue objFollowupIssue = new FollowupIssue();
            objFollowupIssue.SendMailToUpload(_MailData, strMailBody);
        }

        public void SendRemainderMail(MailData _MailData, string strMailBody, DateTime dtCutOfDay)
        {
            FollowupIssue objFollowupIssue = new FollowupIssue();
            objFollowupIssue.SendRemainderMail(_MailData, strMailBody, dtCutOfDay);
        }

        public void SendNotificationMail(MailData _MailData, string strSubject, string strMailBody)
        {
            FollowupIssue objFollowupIssue = new FollowupIssue();
            objFollowupIssue.SendNotificationMail(_MailData, strSubject, strMailBody);
        }

        public void SendLinkedPolicyConfirmationMail(MailData _MailData, string strMailBody, string strPendingPolicy, string strActivePolicy)
        {
            FollowupIssue objFollowupIssue = new FollowupIssue();
            objFollowupIssue.SendLinkedPolicyConfirmationMail(_MailData, strMailBody, strPendingPolicy, strActivePolicy);
        }

        public List<DisplayFollowupIssue> GetFewIssueAccordingtoMode(int Status, Guid PayorID, Guid AgencyID, bool Followup, int intDays)
        {
            return FollowupIssue.GetFewIssueAccordingtoMode(Status, PayorID, AgencyID, Followup, intDays);
        }
        public List<DisplayFollowupIssue> GetFewIssueAccordingtoModeScr(int Status, Guid PayorID, Guid AgencyID, bool Followup, int intDays)
        {
            return FollowupIssue.GetFewIssueAccordingtoModeForFollowupScr(Status, PayorID, AgencyID, Followup, intDays);

        }
        public List<DisplayFollowupIssue> GetFewIssueForCommissionDashBoard(Guid PolicyId)
        {
            return FollowupIssue.GetFewIssueForCommissionDashBoard(PolicyId);
        }
        public void DeleteIssue(Guid IssueId)
        {
            FollowupIssue.DeleteIssue(IssueId);
        }

        public void RemoveCommisionIssue(Guid IssueID)
        {
            FollowupIssue objFollowupIssue = new FollowupIssue();
            objFollowupIssue.RemoveCommisionIssue(IssueID);
        }

        #endregion

        #region IFollowupIssue Members


        public void AddUpdatePolicyIssueNotesScr(Guid ModifiedBy, Guid IssueId, string PolicyIssueNote)
        {
            FollowupIssue.AddUpdatePolicyIssueNotesScr(ModifiedBy, IssueId, PolicyIssueNote);
        }

        #endregion

        #region IFollowupIssue Members
        public void UpdateIssuesSrc(DisplayFollowupIssue _DisplayFollowupIssue, int PreviousStatusId, Guid ModifiedBy)
        {
            FollowupIssue.UpdateIssuesSrc(_DisplayFollowupIssue, PreviousStatusId, ModifiedBy);
        }

        #endregion
    }
}
