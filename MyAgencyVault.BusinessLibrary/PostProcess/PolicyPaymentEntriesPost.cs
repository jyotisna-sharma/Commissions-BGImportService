using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Data.Objects.SqlClient;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PolicyPaymentEntriesPost
    {
        [DataMember]
        public Guid PaymentEntryID { get; set; }
        [DataMember]
        public Guid StmtID { get; set; }
        [DataMember]
        public Guid PolicyID { get; set; }
        [DataMember]
        public DateTime? InvoiceDate { get; set; }
        [DataMember]
        public decimal PaymentRecived { get; set; }
        [DataMember]
        public double CommissionPercentage { get; set; }
        [DataMember]
        public int NumberOfUnits { get; set; }
        [DataMember]
        public decimal DollerPerUnit { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public double? SplitPer { get; set; }
        [DataMember]
        public decimal TotalPayment { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Guid CreatedBy { get; set; }
        [DataMember]
        public int? PostStatusID { get; set; }
        [DataMember]
        public Guid ClientId { get; set; }
        [DataMember]
        public decimal Bonus { get; set; }
        [DataMember]
        public Guid? DEUEntryId { get; set; }
        [DataMember]
        public int StmtNumber { get; set; }
        [DataMember]
        public String BatchNumber { get; set; }
        [DataMember]
        public DateTime? EntryDate { get; set; }
        [DataMember]
        public string Pageno { get; set; }
        [DataMember]
        public Guid? FollowUpVarIssueId { get; set; }
        //Newly added 29102013
        [DataMember]
        public int? FollowUpIssueResolveOrClosed { get; set; }

        //Newly added 21012014 for unlink payment from commision dashboard
        [DataMember]
        public bool? IsLinkPayment { get; set; }

        private static decimal? expectedpayment;

        public static decimal? Expectedpayment
        {
            get { return PolicyPaymentEntriesPost.expectedpayment; }
            set { PolicyPaymentEntriesPost.expectedpayment = value; }
        }

        public static void AddUpadate(PolicyPaymentEntriesPost policypaymententriespost)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _popaenpost = (from u in DataModel.PolicyPaymentEntries
                                       where (u.PaymentEntryId == policypaymententriespost.PaymentEntryID)
                                       select u).FirstOrDefault();
                    if (_popaenpost == null)
                    {
                        _popaenpost = new DLinq.PolicyPaymentEntry
                        {
                            PaymentEntryId = ((policypaymententriespost.PaymentEntryID == Guid.Empty) ? Guid.NewGuid() : policypaymententriespost.PaymentEntryID),
                            InvoiceDate = policypaymententriespost.InvoiceDate,
                            PaymentRecived = policypaymententriespost.PaymentRecived,
                            //IncomigPercentage = policypaymententriespost.IncomingPercentage,
                            CommissionPercentage = policypaymententriespost.CommissionPercentage,
                            NumberOfUnits = policypaymententriespost.NumberOfUnits,
                            DollerPerUnit = policypaymententriespost.DollerPerUnit,
                            Fee = policypaymententriespost.Fee,
                            //SharePercentage = policypaymententriespost.SharePercentage,
                            SplitPercentage = policypaymententriespost.SplitPer,
                            TotalPayment = policypaymententriespost.TotalPayment,
                            CreatedOn = policypaymententriespost.CreatedOn,
                            CreatedBy = policypaymententriespost.CreatedBy,
                            //ClientId = policypaymententriespost.ClientId,
                            Bonus = policypaymententriespost.Bonus,
                            // IsPaid = policypaymententriespost.IsPaid,
                            FollowUpVarIssueId = policypaymententriespost.FollowUpVarIssueId,
                            IsLinkPayment = policypaymententriespost.IsLinkPayment,
                            //Added code 29102013
                            //To idenify issue is manaully closed or resolved
                            // FollowUpIssueResolveOrClosed = policypaymententriespost.FollowUpIssueResolveOrClosed,
                        };
                        //_dtUsers.MasterRole = _masterRole;
                        _popaenpost.StatementReference.Value = (from f in DataModel.Statements where f.StatementId == policypaymententriespost.StmtID select f).FirstOrDefault();
                        _popaenpost.PolicyReference.Value = (from f in DataModel.Policies where f.PolicyId == policypaymententriespost.PolicyID select f).FirstOrDefault();
                        // _popaenpost.FollowupIssueReference.Value = (from f in DataModel.FollowupIssues where f.IssueId == policypaymententriespost.IssueID select f).FirstOrDefault();
                        _popaenpost.MasterPostStatuReference.Value = (from f in DataModel.MasterPostStatus where f.PostStatusID == policypaymententriespost.PostStatusID select f).FirstOrDefault();
                        _popaenpost.EntriesByDEUReference.Value = (from f in DataModel.EntriesByDEUs where f.DEUEntryID == policypaymententriespost.DEUEntryId select f).FirstOrDefault();
                        DataModel.AddToPolicyPaymentEntries(_popaenpost);
                    }
                    else
                    {
                        _popaenpost.InvoiceDate = policypaymententriespost.InvoiceDate;
                        _popaenpost.PaymentRecived = policypaymententriespost.PaymentRecived;
                        _popaenpost.CommissionPercentage = policypaymententriespost.CommissionPercentage;
                        _popaenpost.NumberOfUnits = policypaymententriespost.NumberOfUnits;
                        _popaenpost.DollerPerUnit = policypaymententriespost.DollerPerUnit;
                        _popaenpost.Fee = policypaymententriespost.Fee;
                        _popaenpost.SplitPercentage = policypaymententriespost.SplitPer;
                        _popaenpost.TotalPayment = policypaymententriespost.TotalPayment;
                        _popaenpost.CreatedOn = policypaymententriespost.CreatedOn;
                        _popaenpost.CreatedBy = policypaymententriespost.CreatedBy;
                        //_popaenpost.ClientId = policypaymententriespost.ClientId;
                        _popaenpost.Bonus = policypaymententriespost.Bonus;
                        _popaenpost.IsLinkPayment = policypaymententriespost.IsLinkPayment;
                        // _popaenpost.IsPaid = policypaymententriespost.IsPaid;                    
                    }
                    _popaenpost.StatementReference.Value = (from f in DataModel.Statements where f.StatementId == policypaymententriespost.StmtID select f).FirstOrDefault();
                    _popaenpost.PolicyReference.Value = (from f in DataModel.Policies where f.PolicyId == policypaymententriespost.PolicyID select f).FirstOrDefault();
                    // _popaenpost.FollowupIssueReference.Value = (from f in DataModel.FollowupIssues where f.IssueId == policypaymententriespost.IssueID select f).FirstOrDefault();
                    _popaenpost.MasterPostStatuReference.Value = (from f in DataModel.MasterPostStatus where f.PostStatusID == policypaymententriespost.PostStatusID select f).FirstOrDefault();
                    _popaenpost.EntriesByDEUReference.Value = (from f in DataModel.EntriesByDEUs where f.DEUEntryID == policypaymententriespost.DEUEntryId select f).FirstOrDefault();

                    DataModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Issue to addupdate payment in PolicyPaymentEntry (AddUpadate)  :" + ex.StackTrace.ToString(), true);
            }
        }

        public static void AddUpadateResolvedorClosed(PolicyPaymentEntriesPost policypaymententriespost)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _popaenpost = (from u in DataModel.PolicyPaymentEntries
                                       where (u.PaymentEntryId == policypaymententriespost.PaymentEntryID)
                                       select u).FirstOrDefault();
                    if (_popaenpost == null)
                    {
                        _popaenpost = new DLinq.PolicyPaymentEntry
                        {
                            PaymentEntryId = ((policypaymententriespost.PaymentEntryID == Guid.Empty) ? Guid.NewGuid() : policypaymententriespost.PaymentEntryID),
                            InvoiceDate = policypaymententriespost.InvoiceDate,
                            PolicyId = policypaymententriespost.PolicyID,
                            CreatedOn = policypaymententriespost.CreatedOn,
                            CreatedBy = policypaymententriespost.CreatedBy,
                            FollowUpVarIssueId = policypaymententriespost.FollowUpVarIssueId,
                            //Added code 29102013
                            //To idenify issue is manaully closed or resolved
                            FollowUpIssueResolveOrClosed = policypaymententriespost.FollowUpIssueResolveOrClosed,
                        };

                        DataModel.AddToPolicyPaymentEntries(_popaenpost);
                        DataModel.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog(ex.StackTrace.ToString(), true);
            }
        }

        public static List<PolicyPaymentEntriesPost> GetAllResolvedorClosedIssueId(Guid? objpolicyId)
        {
            List<PolicyPaymentEntriesPost> objAllResolvedorClosed = new List<PolicyPaymentEntriesPost>();

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    objAllResolvedorClosed = (from u in DataModel.PolicyPaymentEntries
                                              where (u.PolicyId == objpolicyId && u.FollowUpIssueResolveOrClosed != null)
                                              select new PolicyPaymentEntriesPost
                                              {
                                                  PaymentEntryID = u.PaymentEntryId,
                                                  StmtID = u.StatementId ?? Guid.Empty,
                                                  PolicyID = u.PolicyId ?? Guid.Empty,
                                                  InvoiceDate = u.InvoiceDate ?? null,
                                                  CreatedOn = u.CreatedOn.Value,
                                                  CreatedBy = u.CreatedBy ?? Guid.Empty,
                                                  FollowUpVarIssueId = u.FollowUpVarIssueId,
                                                  //Added code 29102013
                                                  //To idenify issue is manaully closed or resolved
                                                  FollowUpIssueResolveOrClosed = u.FollowUpIssueResolveOrClosed,

                                              }
                                      ).ToList();
                }
                catch
                {
                }
                return objAllResolvedorClosed;
            }
        }

        //Update status when manully resolved or closed the status
        //When 1 then resolved
        //When 2 then closed
        public static void UpadateResolvedOrClosedbyManualy(Guid PaymentEntryID, int intId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _popaenpost = (from u in DataModel.PolicyPaymentEntries
                                   where (u.PaymentEntryId == PaymentEntryID)
                                   select u).FirstOrDefault();
                if (_popaenpost == null)
                {
                    _popaenpost = new DLinq.PolicyPaymentEntry
                    {
                        //Added code 29102013
                        //To idenify issue is manaully closed or resolved
                        FollowUpIssueResolveOrClosed = intId,
                    };
                    DataModel.SaveChanges();
                }
            }
        }

        public static void DeletePolicyPayentIdWise(Guid PaymentEntryId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PolicyPaymentEntry pom = DataModel.PolicyPaymentEntries.FirstOrDefault(s => s.PaymentEntryId == PaymentEntryId);
                if (pom != null)
                {
                    DataModel.DeleteObject(pom);
                    DataModel.SaveChanges();
                }
            }
        }

        public static List<PolicyPaymentEntriesPost> GetAllPaymentEntriesOfRange(DateTime FromDate, DateTime ToDate, Guid PolicyID)
        {
            return GetPolicyPaymentEntryPolicyIDWise(PolicyID).Where(p => p.InvoiceDate >= FromDate).Where(p => p.InvoiceDate <= ToDate).ToList<PolicyPaymentEntriesPost>();
        }

        public static double GetAllPaymentEntriesSumInRange(DateTime FromDate, DateTime ToDate, Guid PolicyID)
        {
            ActionLogger.Logger.WriteImportPolicyLog("GetAllPaymentEntriesSumInRange request: from -" + FromDate + ", To: " + ToDate + ", POlicyID: " + PolicyID, true);
            double sum = 0;
            try
            {
                var list = GetPolicyPaymentEntryPolicyIDWise(PolicyID).Where(p => p.CreatedOn >= FromDate).Where(p => p.CreatedOn <= ToDate).ToList<PolicyPaymentEntriesPost>();
                if (list != null && list.Count > 0)
                {
                    sum = (double) list.Sum(x => x.PaymentRecived);
                }
            }
            catch(Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetAllPaymentEntriesSumInRange ex:" + ex.Message, true);
            }
            return sum;
        }

        public static PolicyPaymentEntriesPost GetPolicyPaymentEntry(Guid PolicyPaymentEntryID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                PolicyPaymentEntriesPost _popaenpost = (from u in DataModel.PolicyPaymentEntries
                                                        where (u.PaymentEntryId == PolicyPaymentEntryID)
                                                        select new PolicyPaymentEntriesPost
                                                        {
                                                            PaymentEntryID = u.PaymentEntryId,
                                                            StmtID = u.StatementId ?? Guid.Empty,
                                                            PolicyID = u.PolicyId ?? Guid.Empty,
                                                            //IssueID = u.IssueID.Value,
                                                            InvoiceDate = u.InvoiceDate.Value,
                                                            PaymentRecived = u.PaymentRecived.Value,
                                                            CommissionPercentage = u.CommissionPercentage.Value,
                                                            NumberOfUnits = u.NumberOfUnits.Value,
                                                            DollerPerUnit = u.DollerPerUnit.Value,
                                                            Fee = u.Fee.Value,
                                                            SplitPer = u.SplitPercentage.Value,
                                                            TotalPayment = u.TotalPayment.Value,
                                                            CreatedOn = u.CreatedOn.Value,
                                                            CreatedBy = u.CreatedBy ?? Guid.Empty,
                                                            PostStatusID = u.PostStatusID.Value,
                                                            //ClientId = u.ClientId ?? Guid.Empty,
                                                            Bonus = u.Bonus.Value,
                                                            //  IsPaid = u.IsPaid,
                                                            // PercentageOfPremium=u.PerOfPremium.Value,
                                                            DEUEntryId = u.DEUEntryId ?? Guid.Empty,
                                                            StmtNumber = u.Statement.StatementNumber,
                                                            FollowUpVarIssueId = u.FollowUpVarIssueId,
                                                            //Added code 29102013
                                                            //To idenify issue is manaully closed or resolved
                                                            FollowUpIssueResolveOrClosed = u.FollowUpIssueResolveOrClosed,
                                                            //Check payment is link or not
                                                            IsLinkPayment = u.IsLinkPayment ?? null,

                                                        }
                                   ).ToList().FirstOrDefault();
                return _popaenpost;
            }
        }

        public static List<PolicyPaymentEntriesPost> GetPolicyPaymentEntryStatementWise(Guid StmtId)
        {
            var _popaenpost = new List<PolicyPaymentEntriesPost>();
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetPolicyPaymentEntryStatementWise:process begins with PolicyPaymentEntries " + " " + "StatementId:" + StmtId, true);
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    _popaenpost = (from u in DataModel.PolicyPaymentEntries
                                   where (u.StatementId == StmtId)
                                   select new PolicyPaymentEntriesPost
                                   {
                                       PaymentEntryID = u.PaymentEntryId,
                                       StmtID = u.StatementId.Value,
                                       PolicyID = u.PolicyId.Value,
                                       //IssueID = u.IssueID.Value,
                                       InvoiceDate = u.InvoiceDate.Value,
                                       PaymentRecived = u.PaymentRecived.Value,
                                       CommissionPercentage = u.CommissionPercentage.Value,
                                       NumberOfUnits = u.NumberOfUnits.Value,
                                       DollerPerUnit = u.DollerPerUnit.Value,
                                       Fee = u.Fee.Value,
                                       SplitPer = u.SplitPercentage.Value,
                                       TotalPayment = u.TotalPayment.Value,
                                       CreatedOn = u.CreatedOn.Value,
                                       CreatedBy = u.CreatedBy.Value,
                                       PostStatusID = u.PostStatusID.Value,
                                       //ClientId = u.ClientId ?? Guid.Empty,
                                       //IsPaid = u.IsPaid,
                                       DEUEntryId = u.DEUEntryId ?? Guid.Empty,
                                       StmtNumber = u.Statement.StatementNumber,
                                       FollowUpVarIssueId = u.FollowUpVarIssueId,
                                       //Added code 29102013
                                       //To idenify issue is manaully closed
                                       FollowUpIssueResolveOrClosed = u.FollowUpIssueResolveOrClosed,
                                       //Check payment is link or not
                                       IsLinkPayment = u.IsLinkPayment ?? null,
                                   }).ToList();
                }
            }
            catch(Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetPolicyPaymentEntryStatementWise:Exception occur while fetching list of policyPaymentEntries "+" "+"StatementId:"+StmtId+" "+"Exception:"+ex.Message, true);
            }
            return _popaenpost;
        }
        public static List<PolicyPaymentEntriesPost> GetPolicyPaymentEntryPolicyIDWise(Guid PolicyID)
        {
            List<PolicyPaymentEntriesPost> _popaenpost = new List<PolicyPaymentEntriesPost>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var Result = DataModel.GetPolicyPaymentEntryPolicyIDWise(PolicyID);

                    _popaenpost = (from u in Result
                                   where (u.PolicyID == PolicyID)
                                   select new PolicyPaymentEntriesPost
                                   {
                                       PaymentEntryID = u.PaymentEntryID,
                                       StmtID = u.StatementId.Value,
                                       PolicyID = u.PolicyID.Value,
                                       // IssueID = u.IssueID.Value,
                                       InvoiceDate = u.InvoiceDate,
                                       PaymentRecived = u.PaymentRecived ?? 0,
                                       CommissionPercentage = u.CommissionPercentage ?? 0,
                                       NumberOfUnits = u.NumberOfUnits ?? 0,
                                       DollerPerUnit = u.DollerPerUnit ?? 0,
                                       Fee = u.Fee.Value,
                                       SplitPer = u.SplitPercentage ?? 0,
                                       TotalPayment = u.TotalPayment ?? 0,
                                       CreatedOn = u.CreatedOn.Value,
                                       CreatedBy = u.CreatedBy ?? Guid.Empty,
                                       PostStatusID = u.PostStatusID,
                                       //ClientId = u.ClientId ?? Guid.Empty,
                                       // IsPaid = u.IsPaid,
                                       DEUEntryId = u.DEUEntryId ?? Guid.Empty,
                                       StmtNumber = u.StatementNumber,
                                       FollowUpVarIssueId = u.FollowUpVarIssueId,
                                       BatchNumber = u.BatchNumber,
                                       EntryDate = u.EntryDate,
                                       Pageno = u.Pageno,
                                       //IsLinkPayment=
                                       IsLinkPayment = u.IsLinkPayment ?? null,

                                   }
                                       ).ToList();
                }
            }
            catch
            {
            }
            return _popaenpost;

            #region comment for sp
            //List<PolicyPaymentEntriesPost> _popaenpost = (from u in DataModel.PolicyPaymentEntries
            //                                              where (u.PolicyId == PolicyID)
            //                                              select new PolicyPaymentEntriesPost
            //                                              {
            //                                                  PaymentEntryID = u.PaymentEntryId,
            //                                                  StmtID = u.StatementId.Value,
            //                                                  PolicyID = u.PolicyId.Value,
            //                                                  // IssueID = u.IssueID.Value,
            //                                                  InvoiceDate = u.InvoiceDate,
            //                                                  PaymentRecived = u.PaymentRecived ?? 0,
            //                                                  CommissionPercentage = u.CommissionPercentage ?? 0,
            //                                                  NumberOfUnits = u.NumberOfUnits ?? 0,
            //                                                  DollerPerUnit = u.DollerPerUnit ?? 0,
            //                                                  Fee = u.Fee.Value,
            //                                                  SplitPer = u.SplitPercentage ?? 0,
            //                                                  TotalPayment = u.TotalPayment ?? 0,
            //                                                  CreatedOn = u.CreatedOn.Value,
            //                                                  CreatedBy = u.CreatedBy ?? Guid.Empty,
            //                                                  PostStatusID = u.PostStatusID,
            //                                                  //ClientId = u.ClientId ?? Guid.Empty,
            //                                                  // IsPaid = u.IsPaid,
            //                                                  DEUEntryId = u.DEUEntryId ?? Guid.Empty,
            //                                                  StmtNumber = u.Statement.StatementNumber,
            //                                                  FollowUpVarIssueId = u.FollowUpVarIssueId,
            //                                                  BatchNumber = SqlFunctions.StringConvert((decimal)u.Statement.Batch.BatchNumber) + "/" + SqlFunctions.StringConvert((decimal)u.Statement.StatementNumber).Trim(),
            //                                                  EntryDate = u.EntriesByDEU.EntryDate,
            //                                              }
            //                    ).ToList();
            //return _popaenpost;

            #endregion

        }

        public static List<PolicyPaymentEntriesPost> GetPolicyGreatestInvoiceDate(Guid PolicyID)
        {
            List<PolicyPaymentEntriesPost> PaymentInvoiceDate = new List<PolicyPaymentEntriesPost>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    PaymentInvoiceDate = (from u in DataModel.PolicyPaymentEntries
                                          where (u.PolicyId == PolicyID)
                                          select new PolicyPaymentEntriesPost
                                          {
                                              PaymentEntryID = u.PaymentEntryId,
                                              InvoiceDate = u.InvoiceDate
                                          }
                                        ).ToList();
                }

            }
            catch
            {
            }
            return PaymentInvoiceDate;
        }

        //add on 10/01/2012 get total payment at that invoice
        public static decimal GetTotalpaymentOnInvoiceDate(Guid policyID, DateTime dtTime)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<PolicyPaymentEntriesPost> PaymentInvoiceDate = (from u in DataModel.PolicyPaymentEntries
                                                                     where (u.PolicyId == policyID && u.InvoiceDate == dtTime)
                                                                     select new PolicyPaymentEntriesPost
                                                                     {
                                                                         //PaymentEntryID = u.PaymentEntryId,
                                                                         //InvoiceDate = u.InvoiceDate,
                                                                         TotalPayment = u.TotalPayment.Value,
                                                                     }

                                    ).ToList();


                Decimal D = PaymentInvoiceDate.Sum(P => P.TotalPayment);
                return D;
            }
        }

        public static PolicyPaymentEntriesPost GetPolicyPaymentEntryDEUEntryIdWise(Guid DeuEntryId)
        {
            PolicyPaymentEntriesPost _popaenpost = null;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    _popaenpost = (from u in DataModel.PolicyPaymentEntries
                                   where (u.DEUEntryId == DeuEntryId)
                                   select new PolicyPaymentEntriesPost
                                   {
                                       PaymentEntryID = u.PaymentEntryId,
                                       StmtID = u.StatementId ?? Guid.Empty,
                                       PolicyID = u.PolicyId ?? Guid.Empty,
                                       //IssueID = u.IssueID.Value,
                                       InvoiceDate = u.InvoiceDate.Value,
                                       PaymentRecived = u.PaymentRecived.Value,
                                       CommissionPercentage = u.CommissionPercentage.Value,
                                       NumberOfUnits = u.NumberOfUnits.Value,
                                       DollerPerUnit = u.DollerPerUnit.Value,
                                       Fee = u.Fee.Value,
                                       SplitPer = u.SplitPercentage.Value,
                                       TotalPayment = u.TotalPayment.Value,
                                       CreatedOn = u.CreatedOn.Value,
                                       CreatedBy = u.CreatedBy ?? Guid.Empty,
                                       PostStatusID = u.PostStatusID.Value,
                                       //ClientId = u.ClientId ?? Guid.Empty,
                                       Bonus = u.Bonus.Value,
                                       //  IsPaid = u.IsPaid,
                                       // PercentageOfPremium=u.PerOfPremium.Value,
                                       DEUEntryId = u.DEUEntryId ?? Guid.Empty,
                                       StmtNumber = u.Statement.StatementNumber,
                                       FollowUpVarIssueId = u.FollowUpVarIssueId,
                                       //Added code 29102013
                                       //To idenify issue is manaully closed
                                       FollowUpIssueResolveOrClosed = u.FollowUpIssueResolveOrClosed,
                                       //Check payment is link or not
                                       IsLinkPayment = u.IsLinkPayment ?? null,

                                   }
                                ).ToList().FirstOrDefault();
                }

            }
            catch
            {
            }
            return _popaenpost;
        }
        //   public double? PercentageOfPremium { get; set; }
        public static void UpdateExpectedPayment(decimal? expectedpayment, Guid PolicyPaymentEntryId)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.PolicyPaymentEntries.Where(p => p.PaymentEntryId == PolicyPaymentEntryId).FirstOrDefault().ExpectedPayment = expectedpayment;
                    DataModel.SaveChanges();
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// Upadte the Payment with the Followup issue id shows Variense in payment
        /// </summary>
        /// <param name="PolicyPaymententryId"></param>
        /// <param name="FollowupIssueId"></param>
        public static void UpdateVarPaymentIssueId(Guid PolicyPaymententryId, Guid FollowupIssueId)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.PolicyPaymentEntries.Where(p => p.PaymentEntryId == PolicyPaymententryId).FirstOrDefault().FollowUpVarIssueId = FollowupIssueId;
                    DataModel.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public static void UpdateVarPaymentIssueIDValue(Guid FollowUpId1, Guid? FollowUpId2)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DataModel.PolicyPaymentEntries.Where(p => p.FollowUpVarIssueId == FollowUpId1).FirstOrDefault().FollowUpVarIssueId = FollowUpId2;
                DataModel.SaveChanges();
            }
        }
    }
}

