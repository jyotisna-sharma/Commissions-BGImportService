using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class DeuSearchedPolicy
    {
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string Insured { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string ProductType { get; set; }

        [DataMember]
        public string CompSchedule { get; set; }
        [DataMember]
        public string CompType { get; set; }
        [DataMember]
        public int? PaymentMode { get; set; }
        [DataMember]
        public int PolicyStatus { get; set; }
        [DataMember]
        public DateTime LastModifiedDate { get; set; }


    }
}
