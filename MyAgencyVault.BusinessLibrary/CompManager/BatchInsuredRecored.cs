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
    public class InsuredPayment
    {
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public decimal? PaymentRecived { get; set; }
    }

    public class BatchInsuredRecored
    {
        [DataMember]
        public Guid PaymentEntryId { get; set; }
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public Guid ClientId { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string Insured { get; set; }
        [DataMember]
        public decimal? PaymentRecived { get; set; }
        
        public static List<BatchInsuredRecored> GetBatchInsuredRecored(Guid stmtId)
        {
            List<BatchInsuredRecored> _BatchInsuredRecored = new List<BatchInsuredRecored>();
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryStatementWise(stmtId);
            foreach (PolicyPaymentEntriesPost ppep in _PolicyPaymentEntriesPost)
            {
              PolicyDetailsData _Policy = PostUtill.GetPolicy(ppep.PolicyID);
                BatchInsuredRecored BIR = new BatchInsuredRecored();
                BIR.PaymentEntryId = ppep.PaymentEntryID;
                BIR.PolicyId = ppep.PolicyID;
                BIR.ClientId = _Policy.ClientId??Guid.Empty;
                Client clt = Client.GetClient(BIR.ClientId);
                BIR.ClientName = (clt == null ? "" : clt.Name);
                BIR.Insured = (clt == null ? "" : clt.InsuredName);
                BIR.PaymentRecived = ppep.TotalPayment;
                _BatchInsuredRecored.Add(BIR);
            }
            return _BatchInsuredRecored;
        }

        public static List<InsuredPayment> GetInsuredPayments(Guid stmtId)
        {
            List<InsuredPayment> _BatchInsuredRecored = new List<InsuredPayment>();
            try
            {
                List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryStatementWise(stmtId);
                foreach (PolicyPaymentEntriesPost ppep in _PolicyPaymentEntriesPost)
                {
                    PolicyDetailsData _Policy = PostUtill.GetPolicy(ppep.PolicyID);
                    if (_Policy == null)
                    {
                        return _BatchInsuredRecored;
                    }
                    InsuredPayment BIR = new InsuredPayment();

                    Client clt = Client.GetClient(_Policy.ClientId ?? Guid.Empty);
                    BIR.ClientName = (clt == null ? "" : clt.Name);
                    BIR.PaymentRecived = ppep.TotalPayment;
                    _BatchInsuredRecored.Add(BIR);
                }
            }
            catch
            {
            }

            var groupQuery = from income in _BatchInsuredRecored
                         group income by income.ClientName into result
                             select new InsuredPayment
                             {
                                 ClientName = result.Key,
                                 PaymentRecived = result.Sum(i => i.PaymentRecived)
                             };

            return groupQuery.ToList();
        }

        public static List<InsuredPayment> GetInsuredName(Guid stmtId)
        {
            List<InsuredPayment> _BatchInsuredRecored = new List<InsuredPayment>();
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryStatementWise(stmtId);
            foreach (PolicyPaymentEntriesPost ppep in _PolicyPaymentEntriesPost)
            {
                PolicyDetailsData _Policy = PostUtill.GetPolicy(ppep.PolicyID);
                InsuredPayment BIR = new InsuredPayment();
                Client clt = Client.GetClient(_Policy.ClientId ?? Guid.Empty);
                BIR.ClientName = (clt == null ? "" : clt.Name);               
                _BatchInsuredRecored.Add(BIR);
            }

            var groupQuery = from income in _BatchInsuredRecored
                             group income by income.ClientName into result
                             select new InsuredPayment
                             {
                                 ClientName = result.Key,
                                 
                             };

            return groupQuery.ToList();
        }
    }
}
