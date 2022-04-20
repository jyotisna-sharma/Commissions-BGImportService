using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{

    /// <summary>
    /// this may be .. that it is not require.. seems to be similar to IncomingPayment
    /// </summary>
    [DataContract]
    public class CommissionEntry : IEditable<CommissionEntry>
    {
        #region IEditable<CommissionEntry> Members

        public void AddUpdate()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public CommissionEntry GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region "Data members aka - public properties."
        //properties to detail about the payment / commission entry.
        [DataMember]
        public Guid PaymentEntryID { get; set; }
        [DataMember]
        public Guid statementID { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public Guid IssueID { get; set; }
        [DataMember]
        public DateTime? InvoiceDate { get; set; }
        [DataMember]
        public double? PaymentRecived { get; set; }
        [DataMember]
        public double CommissionPercentage { get; set; }
        [DataMember]
        public int? NoOfUnit { get; set; }
        [DataMember]
        public int DollarPerUnit { get; set; }
        [DataMember]
        public double Fee { get; set; }
        [DataMember]
        public int? SplitPer { get; set; }
        [DataMember]
        public int TotalPayment { get; set; }
        [DataMember]
        public DateTime? CreatedOn { get; set; }
        [DataMember]
        public Guid? CreatedBy { get; set; }
        #endregion 

    }
}
