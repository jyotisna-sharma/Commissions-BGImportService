using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class LinkPaymentReciptRecords
    {
        [DataMember]
        public Guid PaymentEntryID { get; set; }
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public DateTime? InvoiceDate { get; set; }
        [DataMember]
        public decimal? PaymentRecived { get; set; }
        [DataMember]
        public double? CommissionPercentage { get; set; }//incoming Percentage
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
        public bool IsSelected { get; set; }


        [DataMember]
        public bool IsAllChecked { get; set; }

        //[DataMember]
        //public Guid? LicenseId { get; set; }
        //[DataMember]
        //public Guid? PayorId { get; set; }

        public static List<LinkPaymentReciptRecords> GetLinkPaymentReciptRecordsByPolicyId(Guid PolicyId)
        {
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);

            List<LinkPaymentReciptRecords> _LinkPaymentReciptRecords = (from u in _PolicyPaymentEntriesPost                                                                        
                                                                        where (u.PolicyID == PolicyId)
                                                                        select new LinkPaymentReciptRecords
                                                                        {
                                                                            PaymentEntryID = u.PaymentEntryID,
                                                                            PolicyId = u.PolicyID,
                                                                            PolicyNumber = PostUtill.GetPolicy(PolicyId).PolicyNumber,
                                                                            InvoiceDate = u.InvoiceDate,
                                                                            PaymentRecived = u.PaymentRecived,
                                                                            CommissionPercentage = u.CommissionPercentage,
                                                                            NumberOfUnits = u.NumberOfUnits,
                                                                            DollerPerUnit = u.DollerPerUnit,
                                                                            Fee = u.Fee,
                                                                            SplitPer = u.SplitPer,
                                                                            TotalPayment = u.TotalPayment,
                                                                            //by default set true
                                                                            IsSelected=true,
                                                                            //All checked
                                                                            IsAllChecked=true,
                                                                           
                                                                        }
                                    ).ToList();
            //Policy _Policy= PostUtill.GetPolicy(PolicyId);

            //_LinkPaymentReciptRecords.ForEach(p => p.PayorId = _Policy.PayorId);
            //_LinkPaymentReciptRecords.ForEach(p => p.LicenseId = _Policy.PolicyLicenseeId);            

            return _LinkPaymentReciptRecords;
        }
        public void AddUpdateLinkPaymentReciptRecords(LinkPaymentReciptRecords _LinkPaymentReciptRecords)
        {

        }

    }
}
