using System;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Threading;

namespace MyAgencyVault.BusinessLibrary
{
    public class CommissionDashboard
    {
        public static List<PolicyPaymentEntriesPost> GetPolicyPaymentEntry(Guid PolicyId)
        {
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);
            return _PolicyPaymentEntriesPost;
        }

        public static List<PolicyOutgoingDistribution> GetPolicyOutgoingPayment(Guid PolicyPaymentEntryId)
        {
            List<PolicyOutgoingDistribution> _PolicyOutgoingDistribution = PolicyOutgoingDistribution.GetOutgoingPaymentByPoicyPaymentEntryId(PolicyPaymentEntryId);
            return _PolicyOutgoingDistribution;
        }

        public static List<DisplayFollowupIssue> GetPolicyCommissionIssues(Guid PolicyId)
        {
            List<DisplayFollowupIssue> _FollowupIssue = FollowupIssue.GetIssues(PolicyId);
            return _FollowupIssue;
        }

        public static PolicyPaymentEntriesPost GetPolicyPaymentPaymentEntryEntryIdWise(Guid PolicyEntryid)
        {
            return PolicyPaymentEntriesPost.GetPolicyPaymentEntry(PolicyEntryid);
        }

        public static PostProcessReturnStatus CommissionDashBoardPostStartClienVMWrapper(PolicyDetailsData SelectedPolicy, PolicyPaymentEntriesPost PaymentEntry, PostEntryProcess _PostEntryProcess, UserRole _UserRole)
        {
            #region Process
            try
            {
                if (_PostEntryProcess == PostEntryProcess.FirstPost)
                {
                    Batch batch = Policy.GenerateBatch(SelectedPolicy);
                    return CommissionDashBoardPostStart(batch.BatchId, PaymentEntry, _PostEntryProcess, _UserRole);
                }
                else
                    return CommissionDashBoardPostStart(Guid.Empty, PaymentEntry, _PostEntryProcess, _UserRole);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("CommissionDashBoardPostStartClienVMWrapper ex: " + ex.Message, true);
                return null;
            }
            finally
            {
                PolicyLocking.UnlockPolicy(SelectedPolicy.PolicyId);
            }

            #endregion
        }

        public static PostProcessReturnStatus CommissionDashBoardPostStart(Guid BatchId, PolicyPaymentEntriesPost PaymentEntry, PostEntryProcess _PostEntryProcess, UserRole _UserRole, bool isInvoiceEdited = false, Guid? _UserID = null)
        {
            ActionLogger.Logger.WriteImportPolicyLog("CommissionDashBoardPostStart request BAtchid: " + BatchId + ", Policy - " + PaymentEntry.PolicyID + ", totalPayment: " + PaymentEntry.TotalPayment + ", isInvoiceEdited: " + isInvoiceEdited, true);
            ActionLogger.Logger.WriteImportPolicyLog("CommissionDashBoardPostStart request PostEntryProcess: " + _PostEntryProcess + ", userid: " + _UserID, true);

            Guid UserID = (_UserID == null) ? Guid.Empty : (Guid)_UserID;
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(60)
            };

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, options))
            {
                PostProcessReturnStatus _PostProcessReturnStatus = new PostProcessReturnStatus() { DeuEntryId = Guid.Empty, IsComplete = false, ErrorMessage = null, PostEntryStatus = _PostEntryProcess };
                if (isInvoiceEdited) //Jan 25, 2018: new check added after bug found in the previous method while updating invoice date only. Now, the invoice date to be updated and all else skipped.
                {
                    PolicyPaymentEntriesPost pe = PolicyPaymentEntriesPost.GetPolicyPaymentEntry(PaymentEntry.PaymentEntryID);
                    PostUtill.newInvoice = PaymentEntry.InvoiceDate;
                    if (pe != null)
                    {
                        PostUtill.oldInvoice = pe.InvoiceDate;
                    }
                    IncomingPament.UpdateInvoiceDate(PaymentEntry);
                    PostUtill.SaveInvoiceChangeLog(PaymentEntry.PaymentEntryID, UserID, PaymentEntry.PolicyID);
             //       ActionLogger.Logger.WriteImportPolicyLog("CommissionDashBoardPostStart successfully updated invoice date , Policy - " + PaymentEntry.PolicyID + ", PaymententryID: " + PaymentEntry.PaymentEntryID + ", totalPayment: " + PaymentEntry.TotalPayment, true);
                    _PostProcessReturnStatus.IsComplete = true;
                    ts.Complete();
               //     ActionLogger.Logger.WriteImportPolicyLog("CommissionDashBoardPostStart tx committed", true);

                    return _PostProcessReturnStatus;
                }
                else
                {
                    if (_PostEntryProcess == PostEntryProcess.FirstPost)
                    {
                        PolicyDetailsData _Policy = PostUtill.GetPolicy(PaymentEntry.PolicyID);
                        Guid PayorId = _Policy.PayorId ?? Guid.Empty;
                        decimal PaymentRecived = PaymentEntry.TotalPayment;
                        Guid CreatedBy = PaymentEntry.CreatedBy;
                        Statement _Statement = Policy.GenerateStatment(BatchId, PayorId, PaymentRecived, CreatedBy);
                        PaymentEntry.StmtID = _Statement.StatementID;
                        DEU _DEU = PostUtill.GetDeuCollection(PaymentEntry, _Policy, isInvoiceEdited);
                        DEU objDEU = new DEU();
                        objDEU.AddupdateDeuEntry(_DEU);
                        _PostProcessReturnStatus = PostUtill.PostStart(_PostEntryProcess, _DEU.DEUENtryID, Guid.Empty, UserID, _UserRole, _PostEntryProcess, string.Empty, string.Empty, isInvoiceEdited);
                    }
                    else if (_PostEntryProcess == PostEntryProcess.RePost)
                    {
                        PolicyDetailsData _Policy = PostUtill.GetPolicy(PaymentEntry.PolicyID);
                        DEU _DEU = PostUtill.GetDeuCollection(PaymentEntry, _Policy, isInvoiceEdited);
                        DEU objDEU = new DEU();
                        objDEU.AddupdateDeuEntry(_DEU);
                        _PostProcessReturnStatus = PostUtill.PostStart(_PostEntryProcess, PaymentEntry.DEUEntryId.Value, _DEU.DEUENtryID, UserID, _UserRole, _PostEntryProcess, string.Empty, string.Empty, isInvoiceEdited);

                    }
                    if (_PostProcessReturnStatus.IsComplete)
                    {
                        ts.Complete();
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("CommissionDashBoardPostStart success BAtchid: " + BatchId + ", Policy - " + PaymentEntry.PolicyID + ", totalPayment: " + PaymentEntry.TotalPayment, true);
                    return _PostProcessReturnStatus;
                }
            }
        }

        public static PostProcessReturnStatus RemoveCommissiondashBoardIncomingPayment(PolicyPaymentEntriesPost PolicySelectedIncomingPaymentCommissionDashBoard, UserRole _UserRole)
        {
            ActionLogger.Logger.WriteImportPolicyLog("RemoveCommissiondashBoardIncomingPayment request DEuEntry: " + PolicySelectedIncomingPaymentCommissionDashBoard.DEUEntryId, true);
            try
            {
                var options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromMinutes(60)
                };
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    PostProcessReturnStatus _PostProcessReturnStatus = PostUtill.PostStart(PostEntryProcess.Delete,
                        PolicySelectedIncomingPaymentCommissionDashBoard.DEUEntryId.Value, Guid.Empty, Guid.Empty, _UserRole, PostEntryProcess.Delete, string.Empty, "Delete");

                    if (_PostProcessReturnStatus.IsComplete)
                    {
                        ts.Complete();
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("RemoveCommissiondashBoardIncomingPayment success DEuEntry: " + PolicySelectedIncomingPaymentCommissionDashBoard.DEUEntryId, true);
                    return _PostProcessReturnStatus;
                }
            }
            finally
            {
                //PolicyLocking.UnlockPolicy(PolicySelectedIncomingPaymentCommissionDashBoard.PolicyID);
            }
        }

        static void UnlinkPaymentPaid(DEU _DEU, string unlinkClientName, Guid PaymentEntryID, UserRole _UserRole)
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("UnlinkPaymentPaid  paymentEntry: " + PaymentEntryID +  ",  unlinkClientName: " + unlinkClientName  , true);
                //Get policy to associate with the entry
                DEUFields fields = PostUtill.FillDEUFields(_DEU.DEUENtryID);
                BasicInformationForProcess basicInfo = PostUtill.GetPolicyToProcess(fields, unlinkClientName);
                if (basicInfo != null)
                {
                    PolicyDetailsData newPolicy = PostUtill.GetPolicy(basicInfo.PolicyId);
                    PolicyDetailsData oldPolicy = PostUtill.GetPolicy(_DEU.PolicyId);
                    ActionLogger.Logger.WriteImportPolicyLog("UnlinkPaymentPaid  newPolicy: " + newPolicy.PolicyId + ",  oldpolicy: " + oldPolicy.PolicyId, true);
                    //update deu entry 

                    //update policypayment entry

                    //manage outgoing schedules
                    LinkPaymentPolicies.DoLinkPolicy((Guid)newPolicy.PolicyLicenseeId, true, true, _DEU.PolicyId, (Guid)oldPolicy.ClientId, basicInfo.PolicyId, PaymentEntryID,
                        Guid.Empty, (Guid)oldPolicy.PayorId, (Guid)newPolicy.PayorId, true, true, true, _UserRole);
                }
                ActionLogger.Logger.WriteImportPolicyLog("UnlinkPaymentPaid  success ", true);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("UnlinkPaymentPaid exception: " + ex.Message, true);
                if (ex.InnerException != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("UnlinkPaymentPaid inner exception: " + ex.InnerException.Message, true);
                }
            }
        }

        public static PostProcessReturnStatus UnlinkCommissiondashBoardIncomingPayment(PolicyPaymentEntriesPost PolicySelectedIncomingPaymentCommissionDashBoard, UserRole _UserRole)
        {
            ActionLogger.Logger.WriteImportPolicyLog("UnlinkCommissiondashBoardIncomingPayment request DEuEntry: " + PolicySelectedIncomingPaymentCommissionDashBoard.DEUEntryId, true);
        
            PostProcessReturnStatus _PostProcessReturnStatus = null;
            try
            {
                var options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromMinutes(60)
                };
               // using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    string strUnlink = string.Empty;
                    DEU _DEU = DEU.GetDeuEntryidWiseForUnlikingPolicy(PolicySelectedIncomingPaymentCommissionDashBoard.DEUEntryId ?? Guid.Empty);

                    if (_DEU != null)
                    {
                        strUnlink = Convert.ToString(_DEU.UnlinkClientName);
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("UnlinkCommissiondashBoardIncomingPayment strUnlink- " + strUnlink, true);
                    bool flag = PolicyOutgoingDistribution.IfPaymentHasPaidEntry(PolicySelectedIncomingPaymentCommissionDashBoard.PaymentEntryID);

                    if (!flag) //When unpaid outgoing entries 
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("UnlinkCommissiondashBoardIncomingPayment - Entries are unpaid " , true);
        
                        PostUtill.PostStart(PostEntryProcess.Delete, _DEU.DEUENtryID, Guid.Empty, Guid.Empty, _UserRole, PostEntryProcess.FirstPost, strUnlink, string.Empty);

                        DEU objDEU = new DEU();
                        objDEU.AddupdateUnlinkDeuEntry(_DEU, strUnlink);

                        _PostProcessReturnStatus = PostUtill.PostStart(PostEntryProcess.FirstPost, _DEU.DEUENtryID, Guid.Empty, Guid.Empty, _UserRole, PostEntryProcess.FirstPost, strUnlink, string.Empty);
                    }
                    else
                    {
                        try
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("UnlinkCommissiondashBoardIncomingPayment - Entries are paid ", true);
                            UnlinkPaymentPaid(_DEU, strUnlink, PolicySelectedIncomingPaymentCommissionDashBoard.PaymentEntryID, _UserRole);
                            _PostProcessReturnStatus = new PostProcessReturnStatus() { DeuEntryId = _DEU.DEUENtryID, IsComplete = true, ErrorMessage = null, PostEntryStatus = PostEntryProcess.RePost, IsClientDeleted = false };
                        }
                        catch (Exception ex)
                        {
                               _PostProcessReturnStatus = new PostProcessReturnStatus() { DeuEntryId = _DEU.DEUENtryID, IsComplete = false, ErrorMessage = null, PostEntryStatus = PostEntryProcess.RePost, IsClientDeleted = false };
                               ActionLogger.Logger.WriteImportPolicyLog("UnlinkCommissiondashBoardIncomingPayment exception: " + ex.Message, true);
                        }
         
                    }
                   
                    //if (_PostProcessReturnStatus.IsComplete)
                    //{
                    //    ts.Complete();
                    //}
                    ActionLogger.Logger.WriteImportPolicyLog("UnlinkCommissiondashBoardIncomingPayment success: ", true);
        
                    return _PostProcessReturnStatus;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("UnlinkCommissiondashBoardIncomingPayment exception: " + ex.Message, true);
                if (ex.InnerException != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("UnlinkCommissiondashBoardIncomingPayment inner exception: " + ex.InnerException.Message, true);
                }
                return _PostProcessReturnStatus;
            }
            finally
            {
            }
        }

    }
}
