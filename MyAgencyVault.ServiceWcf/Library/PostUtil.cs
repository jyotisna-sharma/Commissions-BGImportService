using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary.PostProcess;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPostUtil
    {
        [OperationContract]
        List<DeuSearchedPolicy> GetPoliciesFromUniqueIdentifier(List<UniqueIdenitfier> uniqueIdentifiers, Guid LicId, Guid PayorId);
        //[OperationContract]
        //PostProcessReturnStatus PostStart(PostEntryProcess _PostEntryProcess, Guid DeuEntryId, Guid RepostNewDeuEntryId, UserRole _UserRole);
        [OperationContract]
        PostProcessReturnStatus DeuPostStartWrapper(PostEntryProcess _PostEntryProcess, DEUFields deuFields, Guid deuEntryId, Guid userId, UserRole userRole);

        [OperationContract]
        PolicyDetailsData GetPolicyPU(Guid SePolicyID);

        [OperationContract]
        bool CheckForOutgoingScheduleVariance(Guid PaymentEntryId);

        [OperationContract]
        Guid GetPolicyHouseOwner(Guid PolicyLicenID);

        [OperationContract]
        List<PolicyPaymentEntriesPost> GetPolicyPaymentEntryForCommissionDashboard(Guid PolicyId);

        [OperationContract]
        List<PolicyOutgoingDistribution> GetPolicyOutgoingPaymentForCommissionDashboard(Guid PolicyPaymentEntryId);

        [OperationContract]
        List<DisplayFollowupIssue> GetPolicyCommissionIssuesForCommissionDashboard(Guid PolicyId);

        [OperationContract]
        void AddUpadatePolicyPaymentEntries(PolicyPaymentEntriesPost policypaymententriespost);

        [OperationContract]
        void AddUpadateResolvedorClosed(PolicyPaymentEntriesPost policypaymententriespost);

        [OperationContract]
        List<PolicyPaymentEntriesPost> GetAllResolvedorClosedIssueId(Guid? GuidPolicyId);

        [OperationContract]
        void UpadateResolvedOrClosedbyManualy(Guid PaymentEntryID, int intId);

        [OperationContract]
        PolicyPaymentEntriesPost GetPolicyPaymentPaymentEntryEntryIdWise(Guid PolicyEntryid);

        [OperationContract]
        void EntryInPolicyOutGoingPayment(bool IsPaymentToHO, Guid PaymentEntryID, Guid PolicyId, DateTime? InvoiceDate, Guid LicenseeId);

        [OperationContract]
        PostProcessReturnStatus RemoveCommissiondashBoardIncomingPayment(PolicyPaymentEntriesPost PolicySelectedIncomingPaymentCommissionDashBoard, UserRole _UserRole);

        [OperationContract]
        PostProcessReturnStatus UnlinkCommissiondashBoardIncomingPayment(PolicyPaymentEntriesPost PolicySelectedIncomingPaymentCommissionDashBoard, UserRole _UserRole);

        [OperationContract]
        PostProcessReturnStatus CommissionDashBoardPostStart(Guid BatchId, PolicyPaymentEntriesPost PaymentEntry, PostEntryProcess _PostEntryProcess, UserRole _UserRole, bool IsInvoiceEdited = false, Guid? UserId = null);

        [OperationContract]
        void FollowUpProcedure(FollowUpRunModules _FollowUpRunModules, DEU _DEU, Guid PolicyId, bool IsTrackPayment, bool IsEntryByCommissionDashboard, UserRole _UserRole, bool? PolicyModeChange);

        [OperationContract]
        List<PolicyPaymentEntriesPost> GetPolicyPaymentEntryPolicyIDWise(Guid PolicyID);

        [OperationContract]
        PostProcessReturnStatus CommissionDashBoardPostStartClienVMWrapper(PolicyDetailsData SelectedPolicy, PolicyPaymentEntriesPost PaymentEntry, PostEntryProcess _PostEntryProcess, UserRole _UserRole);
    }

    public partial class MavService : IPostUtil
    {
        public void EntryInPolicyOutGoingPayment(bool IsPaymentToHO, Guid PaymentEntryID, Guid PolicyId, DateTime? InvoiceDate, Guid LicenseeId)
        {
            PostUtill.EntryInPolicyOutGoingPayment(IsPaymentToHO, PaymentEntryID, PolicyId, InvoiceDate, LicenseeId);
        }
        public PolicyPaymentEntriesPost GetPolicyPaymentPaymentEntryEntryIdWise(Guid PolicyEntryid)
        {
            return CommissionDashboard.GetPolicyPaymentPaymentEntryEntryIdWise(PolicyEntryid);
        }
        public List<DeuSearchedPolicy> GetPoliciesFromUniqueIdentifier(List<UniqueIdenitfier> uniqueIdentifiers, Guid LicId, Guid PayorId)
        {
            return PostUtill.GetPoliciesFromUniqueIdentifier(uniqueIdentifiers, LicId, PayorId);
        }

        /// <summary>
        /// This function is only for to get DEUFields class structure at proxies classes.
        /// </summary>
        /// <param name="deuFields"></param>
        public void GetDEUFields(DEUFields deuFields)
        {
            return;
        }

        #region IPostUtil Members

        //public PostProcessReturnStatus PostStart(PostEntryProcess _PostEntryProcess, Guid DeuEntryId, Guid RepostNewDeuEntryId, UserRole _UserRole)
        //{
        //    return DeuPostProcessWrapper.DeuPostStartWrapper(_PostEntryProcess, deuFields, deuEntryId, userId, userRole);
        //}

        public PostProcessReturnStatus DeuPostStartWrapper(PostEntryProcess _PostEntryProcess, DEUFields deuFields, Guid deuEntryId, Guid userId, UserRole userRole)
        {
            return DeuPostProcessWrapper.DeuPostStartWrapper(_PostEntryProcess, deuFields, deuEntryId, userId, userRole);
        }

        public PolicyDetailsData GetPolicyPU(Guid SePolicyID)
        {
            return PostUtill.GetPolicy(SePolicyID);
        }

        public bool CheckForOutgoingScheduleVariance(Guid PaymentEntryId)
        {
            return PostUtill.CheckForOutgoingScheduleVariance(PaymentEntryId);
        }

        #endregion

        #region IPostUtil Members


        public Guid GetPolicyHouseOwner(Guid PolicyLicenID)
        {
            return PostUtill.GetPolicyHouseOwner(PolicyLicenID);
        }

        #endregion

        #region IPostUtil Members
        public List<PolicyPaymentEntriesPost> GetPolicyPaymentEntryForCommissionDashboard(Guid PolicyId)
        {
            return CommissionDashboard.GetPolicyPaymentEntry(PolicyId);
        }

        public List<PolicyOutgoingDistribution> GetPolicyOutgoingPaymentForCommissionDashboard(Guid PolicyPaymentEntryId)
        {
            return CommissionDashboard.GetPolicyOutgoingPayment(PolicyPaymentEntryId);
        }

        public List<DisplayFollowupIssue> GetPolicyCommissionIssuesForCommissionDashboard(Guid PolicyId)
        {
            return CommissionDashboard.GetPolicyCommissionIssues(PolicyId);
        }

        #endregion

        #region IPostUtil Members


        public void AddUpadatePolicyPaymentEntries(PolicyPaymentEntriesPost policypaymententriespost)
        {
            PolicyPaymentEntriesPost.AddUpadate(policypaymententriespost);
        }

        public void AddUpadateResolvedorClosed(PolicyPaymentEntriesPost policypaymententriespost)
        {
            PolicyPaymentEntriesPost.AddUpadateResolvedorClosed(policypaymententriespost);
        }
        //Added on 29102013
        public void UpadateResolvedOrClosedbyManualy(Guid PaymentEntryID, int intId)
        {
            PolicyPaymentEntriesPost.UpadateResolvedOrClosedbyManualy(PaymentEntryID, intId);
        }

        public List<PolicyPaymentEntriesPost> GetAllResolvedorClosedIssueId(Guid? GuidPolicyId)
        {
           return PolicyPaymentEntriesPost.GetAllResolvedorClosedIssueId(GuidPolicyId);
        }

        public PostProcessReturnStatus RemoveCommissiondashBoardIncomingPayment(PolicyPaymentEntriesPost PolicySelectedIncomingPaymentCommissionDashBoard, UserRole _UserRole)
        {
            return CommissionDashboard.RemoveCommissiondashBoardIncomingPayment(PolicySelectedIncomingPaymentCommissionDashBoard, _UserRole);
        }

        public PostProcessReturnStatus UnlinkCommissiondashBoardIncomingPayment(PolicyPaymentEntriesPost PolicySelectedIncomingPaymentCommissionDashBoard, UserRole _UserRole)
        {
            return CommissionDashboard.UnlinkCommissiondashBoardIncomingPayment(PolicySelectedIncomingPaymentCommissionDashBoard, _UserRole);

        }
        #endregion

        #region IPostUtil Members


        public PostProcessReturnStatus CommissionDashBoardPostStart(Guid BatchId, PolicyPaymentEntriesPost PaymentEntry, PostEntryProcess _PostEntryProcess, UserRole _UserRole, bool IsInvoiceEdited, Guid? UserId = null)
        {
            return CommissionDashboard.CommissionDashBoardPostStart(BatchId, PaymentEntry, _PostEntryProcess, _UserRole, IsInvoiceEdited, UserId);
        }

        #endregion

        #region IPostUtil Members


        public void FollowUpProcedure(FollowUpRunModules _FollowUpRunModules, DEU _DEU, Guid PolicyId, bool IsTrackPayment, bool IsEntryByCommissionDashboard, UserRole _UserRole, bool? PolicyModeChange)
        {
            FollowUpUtill.FollowUpProcedure(_FollowUpRunModules, _DEU, PolicyId, IsTrackPayment, IsEntryByCommissionDashboard, _UserRole, PolicyModeChange);
        }

        #endregion

        public List<PolicyPaymentEntriesPost> GetPolicyPaymentEntryPolicyIDWise(Guid PolicyID)
        {
            return PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyID);
        }

        #region IPostUtil Members


        public PostProcessReturnStatus CommissionDashBoardPostStartClienVMWrapper(PolicyDetailsData SelectedPolicy, PolicyPaymentEntriesPost PaymentEntry, PostEntryProcess _PostEntryProcess, UserRole _UserRole)
        {
            return CommissionDashboard.CommissionDashBoardPostStartClienVMWrapper(SelectedPolicy, PaymentEntry, _PostEntryProcess, _UserRole);
        }

        #endregion
    }
}