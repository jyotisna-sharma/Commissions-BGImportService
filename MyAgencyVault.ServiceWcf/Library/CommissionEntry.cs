using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]  
    interface ICommissionEntry
    {
        #region IEditable<CommissionEntry> Members
        [OperationContract]
        void AddUpdateCommissionEntry(CommissionEntry CommisionEnt);
        [OperationContract]
        void DeleteCommissionEntry(CommissionEntry CommisionEnt);
        [OperationContract]
        CommissionEntry GetCommissionEntry();
        [OperationContract]
        bool IsValidCommissionEntry(CommissionEntry CommisionEnt);
        #endregion
    }
    public partial class MavService : ICommissionEntry 
    {

        public void AddUpdateCommissionEntry(CommissionEntry CommisionEnt)
        {
            CommisionEnt.AddUpdate() ;
        }

        public void DeleteCommissionEntry(CommissionEntry CommisionEnt)
        {
            CommisionEnt.Delete() ;
        }

        public CommissionEntry GetCommissionEntry()
        {
            throw new NotImplementedException();
        }

        public bool IsValidCommissionEntry(CommissionEntry CommisionEnt)
        {
            return CommisionEnt.IsValid();
        }
    }
}
