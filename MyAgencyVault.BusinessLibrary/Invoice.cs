using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class Invoice : IEditable<Invoice>
    {
        #region IEditable<Invoice> Members

        public void AddUpdate()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Invoice GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region "Public properties"
        //other invoice properties to recognize a invoice.
        [DataMember]
        public BillingLineDetail BillingDetail { get; set; }
        #endregion 
    }
}
