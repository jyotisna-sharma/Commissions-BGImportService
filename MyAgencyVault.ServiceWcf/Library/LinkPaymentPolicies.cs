using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;


namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ILinkPaymentPolicies
    {
        [OperationContract]
        List<LinkPaymentPolicies> GetPendingPoliciesForLinkedPolicy(Guid LicenseeId);
        [OperationContract]
        List<LinkPaymentPolicies> GetAllPoliciesForLinkedPolicy(Guid LicenseeId);
        [OperationContract]
        void AddUpdateLinkPayment(LinkPaymentPolicies _LinkPaymentPolicies);
        [OperationContract]

        void DoLinkPolicy(Guid LicenseeId, bool IsReverse, bool IsLinkWithExistingPolicy, Guid PendingPolicyId,
            Guid ClientId, Guid activePolicyId, Guid PolicyPaymentEntryId, Guid CurrentUser, Guid PendingPayorId,
            Guid ActivePayorId, bool IsAgencyVersion, bool IsPaidMarked, bool IsScheduleMatches, UserRole _UserRole);
        [OperationContract]
        void MakePolicyActive(Guid PolicyId,Guid ClientId, Guid? CurrentUserID);

        [OperationContract]
        bool ScheduleMatches(Guid EntryId, Guid ActivePolicyId);
      
    }
    public partial class MavService : ILinkPaymentPolicies
    {

        #region ILinkPaymentPendingPolicies Members

        public List<LinkPaymentPolicies> GetPendingPoliciesForLinkedPolicy(Guid LicenseeId)
        {
            return LinkPaymentPolicies.GetPendingPoliciesForLinkedPolicy(LicenseeId);
        }

        public List<LinkPaymentPolicies> GetAllPoliciesForLinkedPolicy(Guid LicenseeId)
        {
            return LinkPaymentPolicies.GetAllPoliciesForLinkedPolicy(LicenseeId);
        }

        public void AddUpdateLinkPayment(LinkPaymentPolicies _LinkPaymentPolicies)
        {
            _LinkPaymentPolicies.AddUpdate(_LinkPaymentPolicies);
        }
      
        public void DoLinkPolicy(Guid LicenseeId, bool IsReverse, bool IsLinkWithExistingPolicy, Guid PendingPolicyId,
            Guid ClientId, Guid activePolicyId, Guid PolicyPaymentEntryId, Guid CurrentUser, Guid PendingPayorId,
            Guid ActivePayorId, bool IsAgencyVersion, bool IsPaidMarked, bool IsScheduleMatches, UserRole _UserRole)
        {
            LinkPaymentPolicies.DoLinkPolicy(LicenseeId, IsReverse, IsLinkWithExistingPolicy, PendingPolicyId,
             ClientId, activePolicyId, PolicyPaymentEntryId, CurrentUser, PendingPayorId,
             ActivePayorId, IsAgencyVersion, IsPaidMarked, IsScheduleMatches,  _UserRole);
        }
        #endregion

        #region ILinkPaymentPolicies Members
        #endregion

        #region ILinkPaymentPolicies Members


        public void MakePolicyActive(Guid PolicyId,Guid ClientId, Guid? CurrentUserID)
        {
            LinkPaymentPolicies.MakePolicyActive(PolicyId,ClientId, CurrentUserID);
        }

        #endregion

        #region ILinkPaymentPolicies Members


        public bool ScheduleMatches(Guid EntryId, Guid ActivePolicyId)
        {
           return LinkPaymentPolicies.ScheduleMatches(EntryId, ActivePolicyId);
        }

        #endregion
    }

}