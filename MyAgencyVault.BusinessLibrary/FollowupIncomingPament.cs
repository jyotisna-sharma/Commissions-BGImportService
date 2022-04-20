using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
   public class FollowupIncomingPament
    {
        #region "Datamembers aka public properties."
        
        [DataMember]
        public DateTime? InvoiceDate { get; set; }

        [DataMember]
        public decimal? PaymentRecived { get; set; }

        [DataMember]
        public double? CommissionPercentage { get; set; }

        [DataMember]
        public int? NumberOfUnits { get; set; }

        [DataMember]
        public decimal? DollerPerUnit { get; set; }

        [DataMember]
        public decimal? Fee { get; set; }

        [DataMember]
        public double? SplitPer { get; set; }

        [DataMember]
        public decimal? TotalPayment { get; set; }

        [DataMember]
        public int Statement { get; set; }

        [DataMember]
        public int Batch { get; set; }

        #endregion
    }
}
