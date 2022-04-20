using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPolicyDetails 
    {
        #region IEditable<PolicyDetails> Members
        [OperationContract]
        void AddUpdatePolicyDetails(PolicyDetailsData PolicyDtl);
        [OperationContract]
        void DeletePolicyDetails(PolicyDetailsData PolicyDtl);
        [OperationContract]
        PolicyDetailsData GetPolicyDetails(PolicyDetailsData PolicyDtl);
        [OperationContract]
        bool IsValidPolicyDetails(PolicyDetailsData PolicyDtl);

        #endregion
    }
    public partial class MavService : IPolicyDetails
    {

        #region IPolicyDetails Members

        public void AddUpdatePolicyDetails(PolicyDetailsData PolicyDtl)
        {
            throw new NotImplementedException();
        }

        public void DeletePolicyDetails(PolicyDetailsData PolicyDtl)
        {
            throw new NotImplementedException();
        }

        public PolicyDetailsData GetPolicyDetails(PolicyDetailsData PolicyDtl)
        {
            throw new NotImplementedException();
        }

        public bool IsValidPolicyDetails(PolicyDetailsData PolicyDtl)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
