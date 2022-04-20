using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class ComDeptService : IEditable<ComDeptService>
    {
        #region IEditable<ComDeptService> Members

        public void AddUpdate()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public ComDeptService GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region "DataMembers aka- public properties"
        [DataMember]
        public int ServiceId { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int DefaultServChargeTypeId { get; set; }
        [DataMember]
        public double MinimumCost { get; set; }
        [DataMember]
        public int Range1 { get; set; }
        [DataMember]
        public int Range2 { get; set; }
        [DataMember]
        public double Rate { get; set; }
        [DataMember]
        public double Discount { get; set; }
        [DataMember]
        public bool IsTaxable { get; set; }
        #endregion 
    }
}
