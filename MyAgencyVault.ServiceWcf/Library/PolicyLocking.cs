using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPolicyLocking
    {
        [OperationContract]
        bool LockPolicy(Guid PolicyId);
        [OperationContract]
        bool UnlockPolicy(Guid PolicyId);
       
    }
    public partial class MavService : IPolicyLocking
    {
        #region IPolicyLocking Members

        public bool LockPolicy(Guid PolicyId)
        {
           return PolicyLocking.LockPolicy(PolicyId);
        }

        public bool UnlockPolicy(Guid PolicyId)
        {
            return PolicyLocking.UnlockPolicy(PolicyId);
        }

        #endregion
        
    }
}