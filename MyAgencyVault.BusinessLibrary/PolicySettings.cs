using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PolicySettings : IEditable<PolicySettings>
    {
        #region IEditable<PolicySettings> Members

        public void AddUpdate()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public PolicySettings GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region "data members aka - public properties"
        [DataMember]
        public bool IsTrackMissingMonths { get; set; }
        [DataMember]
        public bool IsTrackIncomingPayments { get; set; }
        #endregion 
    }
}
