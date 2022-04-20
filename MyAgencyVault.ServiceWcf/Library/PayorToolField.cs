using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Drawing;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract] 
    interface IPayorToolField
    {

        #region IEditable<PayorToolField> Members

        [OperationContract]
        void AddUpdatePayorToolField(PayorToolField PyrToolFld);
        [OperationContract]
        void DeletePayorToolField(PayorToolField PyrToolFld);
       
        #endregion  
    }
    public partial class MavService : IPayorToolField
    {


        #region IPayorToolField Members

        public void AddUpdatePayorToolField(PayorToolField PyrToolFld)
        {
            PayorToolField.AddUpdate(PyrToolFld);
        }

        public void DeletePayorToolField(PayorToolField PyrToolFld)
        {
            PayorToolField.Delete(PyrToolFld);
        }

        #endregion
    }
}
