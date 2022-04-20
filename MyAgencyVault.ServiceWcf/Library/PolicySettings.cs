using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPolicySettings
    {
        #region IEditable<PolicySettings> Members
        [OperationContract]
        void AddUpdatePolicySettings(PolicySettings PolcySett);
        [OperationContract]
        void DeletePolicySettings(PolicySettings PolcySett);
        [OperationContract]
        PolicySettings GetPolicySettings();
        [OperationContract]
        bool IsValidPolicySettings(PolicySettings PolcySett);   

        #endregion
    }
    public partial class MavService : IPolicySettings
    {

        #region IPolicySettings Members

        public void AddUpdatePolicySettings(PolicySettings PolcySett)
        {
            PolcySett.AddUpdate();
        }

        public void DeletePolicySettings(PolicySettings PolcySett)
        {
            PolcySett.Delete();
        }

        public PolicySettings GetPolicySettings()
        {
            throw new NotImplementedException();
        }

        public bool IsValidPolicySettings(PolicySettings PolcySett)
        {
            return PolcySett.IsValid();
        }

        #endregion
    }
}
