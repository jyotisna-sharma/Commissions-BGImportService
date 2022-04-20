using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
   public class IssuePolicyDetail
    {
        #region "Datamembers aka public properties."

        [DataMember]
        public string Schedule { get; set; }

        [DataMember]
        public double? SharePercentage { get; set; }

        [DataMember]
        public int StatementNumber { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public decimal? Payment { get; set; }

        [DataMember]
        public string Client { get; set; }

        [DataMember]
        public string Insured { get; set; }

        [DataMember]
        public string PrimaryPayee { get; set; }

        [DataMember]
        public string Carrier { get; set; }

        [DataMember]
        public string Product { get; set; }

        [DataMember]
        public string Effective { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string TrmReason { get; set; }

        [DataMember]
        public string Enroll_Eligible { get; set; }

        [DataMember]
        public string Mode { get; set; }

        [DataMember]
        public decimal? ModalPremium { get; set; }

        [DataMember]
        public string LastUser { get; set; }

        [DataMember]
        public string LastUpdate { get; set; }

        [DataMember]
        public string TrackingNumber { get; set; }

        [DataMember]
        public string Created { get; set; }

        [DataMember]
        public string TrackFrom { get; set; }

        [DataMember]
        public string AutoTrmDate { get; set; }

        [DataMember]
        public string PolicyTermDate { get; set; }
        #endregion
    }
}
