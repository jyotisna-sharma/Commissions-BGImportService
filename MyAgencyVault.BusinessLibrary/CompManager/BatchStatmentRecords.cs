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
    public class BatchStatmentRecords
    {
        [DataMember]
        public Guid PayorId { get; set; }
        [DataMember]
        public string PayorNickName { get; set; }

        [DataMember]
        public string PayorName { get; set; }

        [DataMember]
        public Guid StatmentId { get; set; }
        [DataMember]
        public int? StatmentNumber { get; set; }
        [DataMember]
        public decimal? CheckAmount { get; set; }
        [DataMember]
        public decimal? House { get; set; }
        [DataMember]
        public decimal? Remaining { get; set; }
        [DataMember]
        public double? DonePer { get; set; }
        [DataMember]
        public int Entries { get; set; }
        [DataMember]
        public int? StmtStatus { get; set; }
        [DataMember]
        public decimal? BalAdj { get; set; }

        [DataMember]
        public decimal? EnterAmount { get; set; }
        public static decimal GetBatchTotal(Guid BatchId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                decimal? TotalStatementAmount = (from stValue in DataModel.Statements
                                                 where stValue.BatchId == BatchId
                                                 select stValue.EnteredAmount).Sum();
                return Convert.ToDecimal(TotalStatementAmount);
            }
        }

        private static double SumValueOperation(double? objDbPaidAmount, ref double totalValue)
        {
            double dbPaidAmount = 0;
            try
            {
                if (objDbPaidAmount != null)
                {
                    dbPaidAmount = Convert.ToDouble(objDbPaidAmount);
                }
            }
            catch (Exception)
            {
            }

            totalValue += dbPaidAmount;
            return totalValue;
        }

        //private static double SumValueOperation(double Value, ref double totalValue)
        //{
        //    return totalValue += Value;
        //}

        private static BatchStatmentRecords SetStatementValues(DLinq.Statement statement, Guid HouseOwner)
        {
            BatchStatmentRecords _BSR = new BatchStatmentRecords();

            try
            {
                _BSR.PayorId = statement.PayorId ?? Guid.Empty;
                Payor payor = Payor.GetPayorByID(_BSR.PayorId);

                if (payor != null)
                    _BSR.PayorNickName = payor.NickName;
                else
                    _BSR.PayorNickName = string.Empty;

                _BSR.StatmentId = statement.StatementId;
                _BSR.StatmentNumber = statement.StatementNumber;
                _BSR.CheckAmount = statement.CheckAmount;

                //_BSR.CheckAmount = ((statement.CheckAmount ?? 0) + (statement.BalAdj ?? 0));

                double HouseValue = 0;
                double totalValue = 0;

                //statement.PolicyPaymentEntries.ToList().ForEach(pStatement => pStatement.PolicyOutgoingPayments.ToList().ForEach(pOutGoing => SumValueOperation(pOutGoing.PaidAmount.Value, ref totalValue)));
                //statement.PolicyPaymentEntries.ToList().ForEach(pStatement => pStatement.PolicyOutgoingPayments.ToList().Where(pOutGoing => pOutGoing.RecipientUserCredentialId == HouseOwner).ToList().ForEach(pOutGoing1 => SumValueOperation(pOutGoing1.PaidAmount.Value, ref HouseValue)));

                statement.PolicyPaymentEntries.ToList().ForEach(pStatement => pStatement.PolicyOutgoingPayments.ToList().ForEach(pOutGoing => SumValueOperation(pOutGoing.PaidAmount, ref totalValue)));
                statement.PolicyPaymentEntries.ToList().ForEach(pStatement => pStatement.PolicyOutgoingPayments.ToList().Where(pOutGoing => pOutGoing.RecipientUserCredentialId == HouseOwner).ToList().ForEach(pOutGoing1 => SumValueOperation(pOutGoing1.PaidAmount, ref HouseValue)));

                decimal? totaldismon = (decimal?)totalValue;
                _BSR.House = (decimal?)HouseValue;// GetHouseOwnerDistributedMoneyStmtWise(_BSR.StatmentId, BatchId);
                //_BSR.Remaining = _BSR.CheckAmount - totaldismon;
                //Added after suggestion of kevin and Eric
                //TotalDonePercent = (((NetCheck – RemainingAmt) / Net Check)) 
                //Remaining = Net Check – Entered Amount
                //Net check fpormula
                //net check = CheckAmount- BalAdj

                decimal dcNetCheck = Convert.ToDecimal(_BSR.CheckAmount - statement.BalAdj);
                //Update formula
                _BSR.Remaining = dcNetCheck - totaldismon;

                _BSR.DonePer = 0;

                //if (_BSR.CheckAmount.HasValue)
                //{
                //    if (_BSR.CheckAmount.Value != 0 && totaldismon != 0)
                //        _BSR.DonePer = (((double)(totaldismon ?? 0)) * 100) / ((double)_BSR.CheckAmount.Value);
                //}

                if (dcNetCheck != 0 && dcNetCheck > 0)
                {
                    _BSR.DonePer = Convert.ToDouble((dcNetCheck - _BSR.Remaining) / dcNetCheck);
                    if (_BSR.DonePer > 0)
                    {
                        _BSR.DonePer = _BSR.DonePer * 100;
                    }
                }

                _BSR.Entries = (int)statement.Entries;
                _BSR.StmtStatus = statement.MasterStatementStatu.StatementStatusId;
            }
            catch (Exception)
            {
            }
            return _BSR;
        }

        public static List<BatchStatmentRecords> GetBatchStatment(Guid BatchId)
        {

            List<BatchStatmentRecords> _BatchStatmentRecords = null;
            try
            {
                _BatchStatmentRecords = new List<BatchStatmentRecords>();
                //List<Statement> _TempStatement = Statement.GetStatementList(BatchId);

                //Added instance method
                Statement objStatement = new Statement();
                List<Statement> _TempStatement = objStatement.GetStatementList(BatchId);

                Batch objBatch = new Batch();
                //Batch batch = Batch.GetBatchViaBatchId(BatchId);
                Batch batch = objBatch.GetBatchViaBatchId(BatchId);

                Guid HouseOwner = new Guid();
                if (batch != null)
                    HouseOwner = PostUtill.GetPolicyHouseOwner(batch.LicenseeId);

                using (DLinq.CommissionDepartmentEntities DataModel = new CommissionDepartmentEntities())
                {
                    List<DLinq.Statement> Statements = (from statementRecords in DataModel.Statements
                                                        where statementRecords.BatchId == BatchId
                                                        select statementRecords).ToList();

                    Statements.ToList().ForEach(statement => _BatchStatmentRecords.Add(SetStatementValues(statement, HouseOwner)));
                }
            }
            catch
            {
            }
            return _BatchStatmentRecords;
        }

        public static List<BatchStatmentRecords> GetBatchStatmentWithoutCalculation(Guid BatchId)
        {
            List<BatchStatmentRecords> _BatchStatmentRecords = null;
            _BatchStatmentRecords = new List<BatchStatmentRecords>();

            try
            {

                //List<Statement> _TempStatement = Statement.GetStatementList(BatchId);
                //Added instance 
                Statement objStatement = new Statement();
                List<Statement> _TempStatement = objStatement.GetStatementList(BatchId);
                //Guid HouseOwner = PostUtill.GetPolicyHouseOwner(Batch.GetBatchViaBatchId(BatchId).LicenseeId);

                foreach (Statement _smt in _TempStatement)
                {

                    BatchStatmentRecords _BSR = new BatchStatmentRecords();
                    _BSR.PayorId = _smt.PayorId ?? Guid.Empty;
                    Payor payor = Payor.GetPayorByID(_BSR.PayorId);

                    if (payor != null)
                    {
                        _BSR.PayorNickName = payor.NickName;
                        _BSR.PayorName = payor.PayorName;
                    }
                    else
                    {
                        _BSR.PayorNickName = string.Empty;
                        _BSR.PayorName = string.Empty;
                    }

                    _BSR.StatmentId = _smt.StatementID;
                    _BSR.StatmentNumber = _smt.StatementNumber;

                    //_BSR.CheckAmount = _smt.EnteredAmount;
                    _BSR.CheckAmount = _smt.CheckAmount;
                    _BSR.Entries = _smt.Entries;
                    _BSR.StmtStatus = _smt.StatusId;
                    _BSR.BalAdj = _smt.BalanceForOrAdjustment;  
                    _BatchStatmentRecords.Add(_BSR);
                }
            }
            catch(Exception)
            {
            }
            return _BatchStatmentRecords;
        }

        public static decimal? GetDistributedMoneyinStmt(Guid StmtId)
        {
            double DistributedAmt = 0;
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryStatementWise(StmtId);
            if (_PolicyPaymentEntriesPost != null)
            {
                foreach (PolicyPaymentEntriesPost ppep in _PolicyPaymentEntriesPost)
                {
                    List<PolicyOutgoingDistribution> _PolicyOutgoingDistribution =
                        PolicyOutgoingDistribution.GetOutgoingPaymentByPoicyPaymentEntryId(ppep.PaymentEntryID);
                    if (_PolicyOutgoingDistribution != null)
                    {
                        foreach (PolicyOutgoingDistribution _PolOutDis in _PolicyOutgoingDistribution)
                        {
                            DistributedAmt += _PolOutDis.PaidAmount ?? 0;
                        }
                    }
                }
            }
            return (decimal)DistributedAmt;

        }

        public static decimal? GetHouseOwnerDistributedMoneyStmtWise(Guid StmtId, Guid BatchId)
        {
            double? HouseAmt = 0;
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryStatementWise(StmtId);
            if (_PolicyPaymentEntriesPost != null)
            {
                //Guid HouseOwner = PostUtill.GetPolicyHouseOwner(Batch.GetBatchViaBatchId(BatchId).LicenseeId);
                Batch objBatch = new Batch();
                Guid HouseOwner = PostUtill.GetPolicyHouseOwner(objBatch.GetBatchViaBatchId(BatchId).LicenseeId);

                foreach (PolicyPaymentEntriesPost ppep in _PolicyPaymentEntriesPost)
                {
                    List<PolicyOutgoingDistribution> _PolicyOutgoingDistribution =
                        PolicyOutgoingDistribution.GetOutgoingPaymentByPoicyPaymentEntryId(ppep.PaymentEntryID);
                    if (_PolicyOutgoingDistribution != null)
                    {
                        PolicyOutgoingDistribution _PolicyOutgoingDis =
                            _PolicyOutgoingDistribution.Where(p => p.RecipientUserCredentialId == HouseOwner).FirstOrDefault();
                        if (_PolicyOutgoingDis != null)
                        {
                            HouseAmt += _PolicyOutgoingDis.PaidAmount;
                        }

                    }


                }
            }
            if (HouseAmt == null)
                HouseAmt = 0;

            return (decimal)HouseAmt;
        }

        public static decimal? GetHouseOwnerDistributedMoneyStmtWise(Guid StmtId, Guid BatchId, Guid HouseOwner)
        {
            double? HouseAmt = 0;
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryStatementWise(StmtId);
            if (_PolicyPaymentEntriesPost != null)
            {
                foreach (PolicyPaymentEntriesPost ppep in _PolicyPaymentEntriesPost)
                {
                    PolicyOutgoingDistribution _PolicyOutgoingDistribution = PolicyOutgoingDistribution.GetOutgoingPaymentByPoicyPaymentEntryId(ppep.PaymentEntryID, HouseOwner).FirstOrDefault();
                    if (_PolicyOutgoingDistribution != null)
                    {
                        HouseAmt += _PolicyOutgoingDistribution.PaidAmount;
                    }
                }
            }
            if (HouseAmt == null)
                HouseAmt = 0;

            return (decimal)HouseAmt;
        }

        public void AddUpdateBatchStatmentRecord(BatchStatmentRecords _BatchStatmentRecord)
        {
        }
    }

}
