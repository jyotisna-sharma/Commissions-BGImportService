using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPayee
    {
        #region IEditable<Payee> Members
        [OperationContract]
        void AddUpdatePayee(Payee Pyee);
        [OperationContract]
        void DeletePayee(Payee Pyee);
        [OperationContract]
        Payee GetPayee();
       [OperationContract]
        bool IsValidPayee(Payee Pyee);
               #endregion        
        
    }
    public partial class MavService : IPayee
    {
        #region IPayee Members

        public void AddUpdatePayee(Payee Pyee)
        {
            Pyee.AddUpdate();
        }

        public void DeletePayee(Payee Pyee)
        {
            Pyee.Delete();  
        }

        public Payee GetPayee()
        {
            throw new NotImplementedException();
        }

        public bool IsValidPayee(Payee Pyee)
        {
            return Pyee.IsValid();
        }

        #endregion
    }
}
