using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary.Base;
using System.Linq.Expressions;
using System.Data;
using System.Collections.ObjectModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPolicy
    {
        [OperationContract]
        void AddUpdatePolicy(PolicyDetailsData policy);

        [OperationContract]
        PolicySavedStatus SavePolicyData(PolicyDetailsData Policy, PolicyDetailsData ReplacedPolicy);

        [OperationContract]
        PolicySavedStatus SavePolicy(PolicyDetailsData Policy, PolicyDetailsData ReplacedPolicy, string strRenewal,string strCoverageNickName, Guid? CurrentUserID);

        [OperationContract]
        void UpdateRPolicyStatus(PolicyDetailsData policy);

        [OperationContract]
        void UpdatePolicySetting(PolicyDetailsData policy);

        [OperationContract]
        void DeletePolicy(PolicyDetailsData policy);

        [OperationContract]
        bool IsTrackPaymentChecked(Guid PolicyId);

        [OperationContract]
        int? GetPolicyStatusID(Guid PolicyId);

        [OperationContract]
        Batch GenerateBatch(PolicyDetailsData _policy);

        [OperationContract]
        Statement GenerateStatment(Guid BatchId, Guid PayorId, decimal PaymentRecived, Guid CreatedBy);

        [OperationContract]
        void AddUpdatePolicyHistory(Guid PolicyId);

        [OperationContract]
        PolicyDetailsData GetPolicyHistoryIdWise(Guid PolicyId);

        [OperationContract]
        void DeletePolicyHistory(PolicyDetailsData _policyrecord);

        [OperationContract]
        void DeletePolicyHistoryPermanetById(PolicyDetailsData _Policy);

        [OperationContract]
        bool CheckForPolicyPaymentExists(Guid Policyid);

        [OperationContract]
        List<PolicyDetailsData> GetPolicydata(Dictionary<string, object> parameters);

        [OperationContract]
        List<PolicyDetailsData> GetPoliciesLicenseeWise(Guid LicenseeId, _PolicyStatus? policyStatus, Guid? ClientId);

        [OperationContract]
        void UpdatePolicyTermDate(Guid policyID, DateTime? dtTermReson);

        [OperationContract]
        decimal CalculatePMC(Guid PolicyId);

        [OperationContract]
        decimal CalculatePAC(Guid PolicyId);

        [OperationContract]
        PolicyDetailsData GetPolicyStting(Guid PolicyID);

        [OperationContract]
        DateTime? GetFollowUpDate(Guid PolicyID);

        [OperationContract]
        string GetPolicyProductType(Guid policyID, Guid PayorID, Guid CarrierID, Guid CoverageID);

        [OperationContract]
        List<PolicyDetailsData> GetPolicyClientWise(Guid LicenseeId, Guid ClientId);

        [OperationContract]
        string GetPolicyUniqueKeyName();

        //[OperationContract]
        //Guid IsPolicyExistingWithImportID(string ImportPolicyID);


        [OperationContract]
        string CompareExcel(DataTable dt);

        [OperationContract]
        PolicyImportStatus ImportPolicy(DataTable tbExcel, ObservableCollection<User> GlobalAgentList, Guid LicenseeID, ObservableCollection<CompType> CompTypeList);

        [OperationContract]
        void SavePolicyLastUpdated(Guid PolicyID, Guid? CurrentUserID);
    }

    public partial class MavService : IPolicy
    {

        #region IPolicy Members


        public void AddUpdatePolicy(PolicyDetailsData policy)
        {
            Policy.AddUpdatePolicy(policy);
        }

        public PolicySavedStatus SavePolicyData(PolicyDetailsData PolicySave, PolicyDetailsData ReplacedPolicy)
        {
            return Policy.SavePolicyData(PolicySave, ReplacedPolicy);
        }

        public void SavePolicyLastUpdated(Guid PolicyID, Guid? CurrentUserID)
        {
            Policy.ModifyPolicyLastUpdated(PolicyID, CurrentUserID);
        }
        public PolicySavedStatus SavePolicy(PolicyDetailsData PolicySave, PolicyDetailsData ReplacedPolicy, string strRenewal,string strCoverageNickName, Guid? CurrentUserID)
        {
            return Policy.SavePolicy(PolicySave, ReplacedPolicy, strRenewal, strCoverageNickName, CurrentUserID);
        }

        public void UpdateRPolicyStatus(PolicyDetailsData policy)
        {
            Policy.UpdateRPolicyStatus(policy);
        }
        public List<PolicyDetailsData> GetPoliciesLicenseeWise(Guid LicenseeId, _PolicyStatus? policyStatus, Guid? ClientId)
        {
            return null;
        }
        public void UpdatePolicySetting(PolicyDetailsData policy)
        {
            Policy.UpdatePolicySetting(policy);
        }

        public void DeletePolicy(PolicyDetailsData policy)
        {
            Policy.MarkPolicyDeleted(policy);
        }

        public List<PolicyDetailsData> GetPolicydata(Dictionary<string, object> parameters)
        {
            return Policy.GetPolicyData(parameters);
        }

        public PolicyDetailsData GetPolicyStting(Guid PolicyID)
        {
            Policy objPolicy = new Policy();
            return objPolicy.GetPolicyStting(PolicyID);
        }

        public bool IsTrackPaymentChecked(Guid PolicyId)
        {
            return Policy.IsTrackPaymentChecked(PolicyId);
        }
        public int? GetPolicyStatusID(Guid PolicyId)
        {
            return Policy.GetPolicyStatusID(PolicyId);
        }

        public void UpdatePolicyTermDate(Guid policyID, DateTime? dtTermReson)
        {
            Policy objPolicy = new Policy();
            objPolicy.UpdatePolicyTermDate(policyID, dtTermReson);
        }

        public DateTime? GetFollowUpDate(Guid PolicyID)
        {
            DateTime? dt = new DateTime();
            Policy objPolicy = new Policy();
            dt = objPolicy.GetFollowUpDate(PolicyID);
            return dt;
        }

        public decimal CalculatePMC(Guid PolicyId)
        {
            return Policy.GetPMC(PolicyId);
        }

        public decimal CalculatePAC(Guid PolicyId)
        {
            return Policy.GetPAC(PolicyId);
        }

        #endregion

        #region IPolicy Members


        public Batch GenerateBatch(PolicyDetailsData _policy)
        {
            return Policy.GenerateBatch(_policy);
        }

        #endregion

        #region IPolicy Members


        public Statement GenerateStatment(Guid BatchId, Guid PayorId, decimal PaymentRecived, Guid CreatedBy)
        {
            return Policy.GenerateStatment(BatchId, PayorId, PaymentRecived, CreatedBy);
        }

        #endregion

        #region IPolicy Members


        public void AddUpdatePolicyHistory(Guid PolicyId)
        {
            Policy.AddUpdatePolicyHistory(PolicyId);
        }

        public PolicyDetailsData GetPolicyHistoryIdWise(Guid PolicyId)
        {
            return Policy.GetPolicyHistoryIdWise(PolicyId);

        }

        public void DeletePolicyHistory(PolicyDetailsData _policyrecord)
        {
            Policy.DeletePolicyHistory(_policyrecord);

        }

        public void DeletePolicyHistoryPermanetById(PolicyDetailsData _Policy)
        {
            Policy.DeletePolicyHistoryPermanentById(_Policy);
        }

        public string GetPolicyProductType(Guid policyID, Guid PayorID, Guid CarrierID, Guid CoverageID)
        {
            return Policy.GetPolicyProductType(policyID, PayorID, CarrierID, CoverageID);
        }

        #endregion

        #region IPolicy Members


        public bool CheckForPolicyPaymentExists(Guid Policyid)
        {
            return Policy.CheckForPolicyPaymentExists(Policyid);
        }
        //Added by vinod
        public List<PolicyDetailsData> GetPolicyClientWise(Guid LicenseeId, Guid ClientId)
        {
            return Policy.GetPolicyClientWise(LicenseeId, ClientId);
        }

        #endregion
        #region Import Policy Members

        public PolicyImportStatus ImportPolicy(DataTable tbExcel, ObservableCollection<User> GlobalAgentList, Guid LicenseeID, ObservableCollection<CompType> CompTypeList)
        {
            PolicyImportStatus status = Policy.ImportPolicy(tbExcel, GlobalAgentList, LicenseeID, CompTypeList);
            return status;
        }
        public string GetPolicyUniqueKeyName()
        {
            return Policy.GetPolicyIdKeyForImport();
        }

        //public Guid IsPolicyExistingWithImportID(string ImportPolicyID)
        //{
        //    return Policy.IsPolicyExistingWithImportID(ImportPolicyID);
        //}


        public string CompareExcel(DataTable dt)
        {
            return Policy.CheckExcelFormat(dt);
        }

        #endregion
    }
}
