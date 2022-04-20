using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;

namespace MyAgencyVault.WcfService
{
    [ServiceContract] 
     interface  IAdvancedPayee 
     {       
     [OperationContract]
     void AddUpdateAdvancedPayee(AdvancedPayee AdvPayee);
     [OperationContract]
     void DeleteAdvancedPayee(AdvancedPayee AdvPayee);
     [OperationContract]
     AdvancedPayee GetAdvancedPayee(Guid Id);
     [OperationContract]
     bool IsValidAdvancedPayee(AdvancedPayee AdvPayee);
}
    public partial class MavService : IAdvancedPayee 
    {
        
        #region IAdvancedPayee Members

        public void AddUpdateAdvancedPayee(AdvancedPayee AdvPayee)
        {
            AdvPayee.AddUpdate();
        }

        public void DeleteAdvancedPayee(AdvancedPayee AdvPayee)
        {
            AdvPayee.Delete();  
        }

        public AdvancedPayee GetAdvancedPayee(Guid Id)
        {
             
            throw new NotImplementedException();
        }

        public bool IsValidAdvancedPayee(AdvancedPayee AdvPayee)
        {
            return AdvPayee.IsValid();
        }

        #endregion
    }

}
