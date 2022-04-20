using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IInvoice 
    {
        #region IEditable<Invoice> Members
        [OperationContract]
        void AddUpdateInvoice(Invoice Invoce);
        [OperationContract]
        void DeleteInvoice(Invoice Invoce);
        [OperationContract]
        Invoice GetInvoice();
        [OperationContract]
        bool IsValidInvoice(Invoice Invoce);
     
        #endregion
       
    }
    public partial class MavService : IInvoice
    {
        public void AddUpdateInvoice(Invoice Invoce)
        {
            Invoce.AddUpdate();
        }

        public void DeleteInvoice(Invoice Invoce)
        {
            Invoce.Delete();  
        }

        public Invoice GetInvoice()
        {
            throw new NotImplementedException();
        }

        public bool IsValidInvoice(Invoice Invoce)
        {
            return Invoce.IsValid();  
        }
    }
}
