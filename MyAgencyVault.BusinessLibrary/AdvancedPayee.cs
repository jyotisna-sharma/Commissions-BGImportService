using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public  class AdvancedPayee : IOutgoingSchedule, IEditable<AdvancedPayee>
    {            
        #region IEditable<AdvancedPayee> Members

        public void AddUpdate()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public AdvancedPayee GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region IOutgoingSchedule Members
        [DataMember]
        public string PrimaryAgent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        [DataMember]
        public string NickName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        [DataMember]
        public double FirstYearRate
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        [DataMember]
        public double RenewalRate
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        #endregion
        #region "Other Advanced Payee Public properties"
        [DataMember]
        public DateTime FromEffiectivDate { get; set; }
        public DateTime ToEffectiveDate { get; set; }
        #endregion 
    }
}
