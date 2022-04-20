using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPolicyLearnedField
    {

        #region IEditable<PolicyLearnedField> Members
        [OperationContract]
        void AddUpdatePolicyLearnedField(PolicyLearnedFieldData PolicyLernField, string strProductType);
        [OperationContract]
        void DeletePolicyLearnedField(PolicyLearnedFieldData PolicyLernField);
        [OperationContract]
        PolicyLearnedFieldData GetPolicyLearnedField();
        [OperationContract]
        bool IsValidPolicyLearnedField(PolicyLearnedFieldData PolicyLernField);
        [OperationContract]
        PolicyLearnedFieldData GetPolicyLearnedFieldsPolicyWise(Guid _PolicyID);

        [OperationContract]
        void AddUpdateHistoryLearned(Guid PolicyId);
        [OperationContract]
        PolicyLearnedFieldData GetPolicyLearnedFieldsHistoryPolicyWise(Guid _PolicyID);
        [OperationContract]
        void DeleteLearnedHistory(Guid PolicyId);
        //[OperationContract]
        //void DeleteByPolicyLearnedHistory(Guid PolicyId);

        [OperationContract]
        DateTime? GetPolicyLearnedFieldAutoTerminationDate(Guid PolicyId);

        [OperationContract]
        string GetPolicyLearnedCoverageNickName(Guid policyID, Guid PayorID, Guid CarrierID, Guid CoverageID);


        #endregion
    }
    public partial class MavService : IPolicyLearnedField 
    {

        #region IPolicyLearnedField Members

        public void AddUpdatePolicyLearnedField(PolicyLearnedFieldData PolicyLernField,string strProductType)
        {
            PolicyLearnedField.AddUpdateLearned(PolicyLernField, strProductType);
        }

        public void DeletePolicyLearnedField(PolicyLearnedFieldData PolicyLernField)
        {
            PolicyLearnedField.Delete(PolicyLernField);
        }

        public PolicyLearnedFieldData GetPolicyLearnedField()
        {
            throw new NotImplementedException();
        }

        public bool IsValidPolicyLearnedField(PolicyLearnedFieldData PolicyLernField)
        {
          return PolicyLearnedField.IsValid(PolicyLernField);
        }
        public PolicyLearnedFieldData GetPolicyLearnedFieldsPolicyWise(Guid _PolicyID)
        {
            return PolicyLearnedField.GetPolicyLearnedFieldsOnPOlicy(_PolicyID); //GetPolicyLearnedFieldsPolicyWise(_PolicyID);
        }

        public  string GetPolicyLearnedCoverageNickName(Guid policyID, Guid PayorID, Guid CarrierID, Guid CoverageID)
        {
            return PolicyLearnedField.GetPolicyLearnedCoverageNickName(policyID, PayorID, CarrierID, CoverageID);
        }

        #endregion

        #region IPolicyLearnedField Members


        public void AddUpdateHistoryLearned(Guid PolicyId)
        {
            PolicyLearnedField.AddUpdateHistoryLearned(PolicyId);
        }

        public PolicyLearnedFieldData GetPolicyLearnedFieldsHistoryPolicyWise(Guid _PolicyID)
        {
          return  PolicyLearnedField.GetPolicyLearnedFieldsHistoryPolicyWise(_PolicyID);
        }

        public void DeleteLearnedHistory(Guid PolicyId)
        {
            PolicyLearnedField.DeleteLearnedHistory(PolicyId);
        }

        //public void DeleteByPolicyLearnedHistory(Guid PolicyId)
        //{
        //    PolicyLearnedField.DeleteByPolicyLearnedHistory(PolicyId);
        //}

        public DateTime? GetPolicyLearnedFieldAutoTerminationDate(Guid PolicyId)
        {
            return PolicyLearnedField.GetPolicyLearnedFieldAutoTerminationDate(PolicyId);
        }

        #endregion
    }
}
