using System;
using System.Collections.Generic;
using System.Linq;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Transactions;
using System.Globalization;
namespace MyAgencyVault.BusinessLibrary
{
    public class FollowUpUtill
    {
        public static bool IsCallForDelete { get; set; }
        public static bool IsAutoTrmDateUpadte = true;
        public static int i = 0;
        /// <summary>
        /// FollowUp Start From here
        /// </summary>
        /// <param name="_FollowUpRunModules"></param>
        /// <param name="_DEU">In Case of Payment Entry</param>
        /// <param name="PolicyId">in case of incoming Schedule change or PolicyDetail Change Otherwise null</param>

        public static void FollowUpProcedure(FollowUpRunModules _FollowUpRunModules, DEU _DEU, Guid PolicyId, bool IsTrackPayment, bool IsEntryByCommissionDashboard, UserRole _UserRole, bool? PolicyModeChange)
        {
            IsAutoTrmDateUpadte = true;
            try
            {
                if (IsTrackPayment)
                {
                    i++;

                    #region PaymentEntered
                    if (_FollowUpRunModules == FollowUpRunModules.PaymentEntered)
                    {
                        bool ReturnFlag = false;
                        DateTime? StoreFirstMissingMonth = null;
                        int noOfMissingCount = 0;
                        bool isPaymenyExist = false;
                        //Load the Policy on which followup procedure is to be done
                        //PolicyDetailsData policy = PostUtill.GetPolicy(PolicyId);
                        PolicyDetailsData policy = PostUtill.GetFollowupPolicy(PolicyId);
                        //Clear Auto termdate
                        if (policy != null)
                        {
                            ClearPolicySmartAutoTermDate(policy.PolicyId, _DEU);
                        }
                        //Acme added for PMC calculation para,eters
                        policy.LearnedFields = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(policy.PolicyId);

                        //Update Policy Last Followup runs date                  
                        //Policy.UpdateLastFollowupRunsWithTodayDate(PolicyId);
                        //Calculate mode
                        MasterPolicyMode? _MasterPolicyMode;
                        if (_DEU == null)
                        {
                            _MasterPolicyMode = PostUtill.ModeEntryFromDeu(policy, null, false);
                        }
                        else
                        {
                            _MasterPolicyMode = PostUtill.ModeEntryFromDeu(policy, _DEU.PolicyMode, true);
                        }
                        if (_MasterPolicyMode == null) return;

                        PaymentMode _PaymentMode = PostUtill.ConvertMode(_MasterPolicyMode.Value);

                        //Mode Randam
                        #region "Random"
                        if (_PaymentMode == PaymentMode.Random)
                        {
                            List<DisplayFollowupIssue> FollowupIssueLst1 = FollowupIssue.GetIssuesForFollowProcess(policy.PolicyId);
                            //InValided The Issue
                            foreach (DisplayFollowupIssue follw in FollowupIssueLst1)
                            {
                                UpdateIssueIdOfPaymentsForIssueId(follw.IssueId, null);
                                //FollowupIssue.DeleteIssue(follw.IssueId);
                                FollowupIssue.DeleteFollowupIssueExceptCloseIssue(follw.IssueId);
                            }
                            return;
                        }
                        #endregion

                        //Mode One time
                        #region "Mode One time"
                        if (_PaymentMode == PaymentMode.OneTime)
                        {
                            DisplayFollowupIssue follomissing = FollowupIssue.GetIssuesForFollowProcess(policy.PolicyId).Where(p => p.IssueCategoryID != (int)FollowUpIssueCategory.VarSchedule).FirstOrDefault();
                            if (follomissing != null)
                            {
                                if (follomissing.IssueStatusId != (int)FollowUpIssueStatus.Closed)
                                {
                                    follomissing.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                    if (IsEntryByCommissionDashboard)
                                    {
                                        if (_UserRole == UserRole.SuperAdmin)
                                            follomissing.IssueResultId = (int)FollowUpResult.Resolved_CD;

                                        else
                                            follomissing.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                    }
                                    else
                                    {
                                        follomissing.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                    }

                                    FollowupIssue.AddUpdate(follomissing);
                                }
                            }

                            //Get issse on policy ID
                            List<DisplayFollowupIssue> FollowupIssueLst = FollowupIssue.GetIssuesForFollowProcess(policy.PolicyId);
                            //Get payment on that policy
                            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPostForVarience = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(policy.PolicyId).ToList();

                            foreach (PolicyPaymentEntriesPost ppepfv in _PolicyPaymentEntriesPostForVarience)
                            {
                                DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ppepfv.PolicyID) && (p.InvoiceDate == ppepfv.InvoiceDate)).Where(p => p.IssueCategoryID == 3).FirstOrDefault();
                                bool flagvarience = PostUtill.CheckForIncomingScheduleVariance(ppepfv, policy.ModeAvgPremium);
                                if (flagvarience)
                                {
                                    if (FollowupIssuetemp == null)
                                        RegisterIssueAgainstScheduleVariance(ppepfv);
                                    else
                                        PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ppepfv.PaymentEntryID, FollowupIssuetemp.IssueId);
                                }
                                else
                                {
                                    if (FollowupIssuetemp != null)
                                    {
                                        if (FollowupIssuetemp.IssueStatusId != (int)FollowUpIssueStatus.Closed)
                                        {
                                            FollowupIssuetemp.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                            if (IsEntryByCommissionDashboard)
                                            {
                                                if (_UserRole == UserRole.SuperAdmin)
                                                    FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                else
                                                    FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                            }
                                            else
                                            {
                                                FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                            }
                                            FollowupIssue.AddUpdate(FollowupIssuetemp);
                                        }
                                    }
                                }
                            }
                            //terminate the policy
                            PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(policy.PolicyId);
                            DateTime? EffDate = _PolicyLearnedField.Effective;
                            if (EffDate != null)
                            {
                                DateTime CalTermDate = _PolicyLearnedField.Effective.Value.AddYears(1);
                                UpdateTheAutoTermDateOfLearned(policy.PolicyId, CalTermDate);
                            }
                            else if (EffDate == null)
                            {
                                PolicyPaymentEntriesPost _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryDEUEntryIdWise(policy.PolicyId);
                                DateTime? InvoiceDate = _PolicyPaymentEntriesPost.InvoiceDate;
                                InvoiceDate = InvoiceDate.Value.AddDays(1);
                                UpdateTheAutoTermDateOfLearned(policy.PolicyId, InvoiceDate);
                            }
                            return;
                        }
                        #endregion

                        FollowUpDate _FollowUpDate = CalculateFollowUpDateRange(_PaymentMode, policy, isPaymenyExist);

                        bool invoiceDateBelongsToRange = false;
                        //Advance payment
                        bool bAvailableIntoRange = false;
                        List<DateTime> dtAdavaceDateRange = new List<DateTime>();
                        int intMulitply = 0;
                        if (_FollowUpDate != null)
                        {
                            DateTime? originalEffectiveDate = policy.OriginalEffectiveDate;
                            //Need to find invoice date and serch range inserted invoice date   

                            int? intAdvance = null;
                            if (originalEffectiveDate != null)
                            {
                                intAdvance = policy.Advance;
                                intMulitply = GetRangValue(_PaymentMode);

                                for (int j = 0; j < intAdvance * intMulitply; j++)
                                {
                                    dtAdavaceDateRange.Add(originalEffectiveDate.Value.AddMonths(j));
                                }
                            }
                        }

                        //Get all payment at policy
                        List<PolicyPaymentEntriesPost> _AllPaymentEntriesOnPolicyFormissing = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(policy.PolicyId);

                        List<PolicyPaymentEntriesPost> _AllResolvedorClosedIssueId = PolicyPaymentEntriesPost.GetAllResolvedorClosedIssueId(policy.PolicyId);

                        if (_AllPaymentEntriesOnPolicyFormissing.Count > 0)
                        {
                            isPaymenyExist = true;
                            invoiceDateBelongsToRange = false;

                            for (int j = 0; j < _AllPaymentEntriesOnPolicyFormissing.Count; j++)
                            {
                                for (int k = 0; k < dtAdavaceDateRange.Count; k++)
                                {

                                    //Added vinod 09052015
                                    DateTime lastDayOfMonth = dtAdavaceDateRange[k].AddMonths(1).AddDays(-1);
                                    //Commented vinod 09052015
                                    //if (_AllPaymentEntriesOnPolicyFormissing[j].InvoiceDate.Equals(dtAdavaceDateRange[k]))
                                    //{
                                    if (_AllPaymentEntriesOnPolicyFormissing[j].InvoiceDate >= dtAdavaceDateRange[k] && _AllPaymentEntriesOnPolicyFormissing[j].InvoiceDate <= lastDayOfMonth)
                                    {
                                        invoiceDateBelongsToRange = true;
                                        break;
                                    }
                                    else
                                    {
                                        invoiceDateBelongsToRange = false;
                                    }
                                }
                                if (invoiceDateBelongsToRange)
                                {
                                    invoiceDateBelongsToRange = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            isPaymenyExist = false;
                        }

                        if (_FollowUpDate.FromDate == null && !ReturnFlag)
                        {
                            ReturnFlag = true;
                        }

                        if (_FollowUpDate.ToDate == null && !ReturnFlag)
                        {
                            ReturnFlag = true;
                        }

                        if (_FollowUpDate.FromDate > _FollowUpDate.ToDate && !ReturnFlag)
                        {
                            List<DisplayFollowupIssue> FollowupIssueLst1 = FollowupIssue.GetIssuesForFollowProcess(policy.PolicyId);
                            foreach (DisplayFollowupIssue follw in FollowupIssueLst1)
                            {
                                UpdateIssueIdOfPaymentsForIssueId(follw.IssueId, null);
                                //FollowupIssue.DeleteIssue(follw.IssueId);
                                FollowupIssue.DeleteFollowupIssueExceptCloseIssue(follw.IssueId);
                            }
                            ReturnFlag = true;
                        }

                        if (!ReturnFlag)
                        {
                            List<DisplayFollowupIssue> FollowupIssueLst = FollowupIssue.GetIssuesForFollowProcess(_DEU.PolicyId);

                            List<DateRange> _DateRangeForMissingLst = MakeFollowUpDateRangeForMissing(_FollowUpDate, _PaymentMode);
                            //InValided The Issue
                            //Close all missing invalid issue  against range
                            if (_DateRangeForMissingLst == null)
                            {
                                //return;
                            }
                            ////Close all issues beyond the range--- i think we need to check is really needed
                            bool isClosed = false;
                            FollowupIssueLst = FollowupIssue.GetIssues(_DEU.PolicyId);
                            List<DisplayFollowupIssue> _FollowupIssueDoInValid = FollowupIssueLst.Where(p => (p.FromDate < _FollowUpDate.FromDate || p.ToDate > _FollowUpDate.ToDate)).ToList();
                            foreach (DisplayFollowupIssue closedIssue in _FollowupIssueDoInValid)
                            {
                                if (closedIssue.IssueStatusId != (int)FollowUpIssueStatus.Closed)
                                {
                                    isClosed = true;
                                    UpdateIssueIdOfPaymentsForIssueId(closedIssue.IssueId, null);
                                    //FollowupIssue.DeleteIssue(closedIssue.IssueId);
                                    FollowupIssue.DeleteFollowupIssueExceptCloseIssue(closedIssue.IssueId);
                                }
                            }
                            //Only for remove roun trip from database
                            if (isClosed)
                            {
                                FollowupIssueLst = FollowupIssue.GetIssuesForFollowProcess(_DEU.PolicyId);
                                isClosed = false;
                            }

                            for (int idx = 0; idx < _DateRangeForMissingLst.Last().RANGE; idx++)
                            {
                                //First date of range
                                DateTime? FirstDate = _DateRangeForMissingLst.Where(p => p.RANGE == idx + 1).ToList()[0].STARTDATE;
                                //Last date of range
                                DateTime? LastDate = _DateRangeForMissingLst.Where(p => p.RANGE == idx + 1).ToList()[(int)_PaymentMode - 1].ENDDATE;

                                //Get the payment at given range
                                List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesFormissing = _AllPaymentEntriesOnPolicyFormissing.Where(p => p.InvoiceDate >= FirstDate.Value).Where(p => p.InvoiceDate <= LastDate.Value).ToList<PolicyPaymentEntriesPost>();

                                bool Rflag = ISExistsResolveIssuesForDateRange(FirstDate.Value, LastDate.Value, policy.PolicyId, FollowupIssueLst);

                                //If no payment found then
                                if (_PolicyPaymentEntriesFormissing.Count == 0)
                                {
                                    //Assing missing month
                                    StoreFirstMissingMonth = FirstDate;
                                    //Increament No of missing count for update policy term date
                                    noOfMissingCount++;

                                    //find the issue which has missing in given range (missing will be missing first or another invoice
                                    List<DisplayFollowupIssue> issfolllst = FollowupIssueLst.Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv))
                                    .Where(p => (p.FromDate == FirstDate.Value && p.ToDate == LastDate.Value)).ToList();

                                    //if issue found then GO
                                    if (issfolllst.Count() != 0)
                                    {
                                        if (Rflag)
                                        {
                                            StoreFirstMissingMonth = null;
                                            noOfMissingCount = 0;
                                        }

                                        if (FirstDate <= policy.OriginalEffectiveDate && policy.OriginalEffectiveDate <= LastDate)
                                        {
                                            DisplayFollowupIssue tempfis = issfolllst.FirstOrDefault();
                                            if (tempfis.IssueCategoryID != (int)FollowUpIssueCategory.MissFirst)
                                            {
                                                tempfis.IssueCategoryID = (int)FollowUpIssueCategory.MissFirst;
                                            }

                                            if (invoiceDateBelongsToRange)
                                            {
                                                foreach (var item in dtAdavaceDateRange)
                                                {
                                                    if (item.Equals(FirstDate.Value))
                                                    {
                                                        bAvailableIntoRange = true;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        bAvailableIntoRange = false;
                                                    }
                                                }
                                            }

                                            if (bAvailableIntoRange)
                                            {
                                                if (IsEntryByCommissionDashboard)
                                                {
                                                    if (_UserRole == UserRole.SuperAdmin)
                                                        tempfis.IssueResultId = (int)FollowUpResult.Resolved_CD;

                                                    else
                                                        tempfis.IssueResultId = (int)FollowUpResult.Resolved_Brk;

                                                }
                                                else
                                                {
                                                    tempfis.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                }

                                                tempfis.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                            }

                                            FollowupIssue.AddUpdate(tempfis);

                                        }
                                        else
                                        {
                                            DisplayFollowupIssue tempfis = issfolllst.FirstOrDefault();
                                            if (tempfis.IssueCategoryID != (int)FollowUpIssueCategory.MissInv)
                                            {
                                                tempfis.IssueCategoryID = (int)FollowUpIssueCategory.MissInv;
                                            }
                                            if (invoiceDateBelongsToRange)
                                            {
                                                foreach (var item in dtAdavaceDateRange)
                                                {
                                                    if (item.Equals(FirstDate.Value))
                                                    {
                                                        bAvailableIntoRange = true;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        bAvailableIntoRange = false;
                                                    }

                                                }
                                            }

                                            if (bAvailableIntoRange)
                                            {
                                                if (IsEntryByCommissionDashboard)
                                                {
                                                    if (_UserRole == UserRole.SuperAdmin)
                                                        tempfis.IssueResultId = (int)FollowUpResult.Resolved_CD;

                                                    else
                                                        tempfis.IssueResultId = (int)FollowUpResult.Resolved_Brk;

                                                }
                                                else
                                                {
                                                    tempfis.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                }

                                                tempfis.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                            }

                                            FollowupIssue.AddUpdate(tempfis);
                                        }
                                    }
                                    //if issue not then registed into missing
                                    else
                                    {
                                        if (FirstDate <= policy.OriginalEffectiveDate && policy.OriginalEffectiveDate <= LastDate)
                                        {
                                            RegisterIssueAgainstMissingPayment(policy, FollowUpIssueCategory.MissFirst, FirstDate.Value, LastDate.Value);
                                        }
                                        else
                                        {
                                            RegisterIssueAgainstMissingPayment(policy, FollowUpIssueCategory.MissInv, FirstDate.Value, LastDate.Value);
                                        }
                                    }
                                    //Update policy term date
                                    if (noOfMissingCount != 0 && StoreFirstMissingMonth != null && IsAutoTrmDateUpadte)
                                        AutoPolicyTerminateProcess(_PaymentMode, StoreFirstMissingMonth, policy.PolicyId, noOfMissingCount, null);

                                }
                                //if payment found then
                                else
                                {
                                   // Acme added:
                                    if (FirstDate != null && LastDate != null)
                                    {
                                        PostUtill.HandlePMCVariance(_PolicyPaymentEntriesFormissing, policy.LearnedFields.PMC, Convert.ToDateTime(FirstDate), Convert.ToDateTime(LastDate), PolicyId);
                                    }

                                    StoreFirstMissingMonth = null;
                                    noOfMissingCount = 0;
                                    List<DisplayFollowupIssue> FollowupIssueMissingtoclose = new List<DisplayFollowupIssue>();
                                    FollowupIssueMissingtoclose = FollowupIssueLst.Where(p => p.FromDate >= FirstDate).Where(p => p.ToDate <= LastDate).Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();
                                    //If Issue found then update the issue
                                    if (FollowupIssueMissingtoclose.Count > 0)
                                    {
                                        foreach (DisplayFollowupIssue _fossu in FollowupIssueMissingtoclose)
                                        {
                                            if (IsEntryByCommissionDashboard)
                                            {
                                                if (_UserRole == UserRole.SuperAdmin)
                                                    _fossu.IssueResultId = (int)FollowUpResult.Resolved_CD;

                                                else
                                                    _fossu.IssueResultId = (int)FollowUpResult.Resolved_Brk;

                                            }
                                            else
                                            {
                                                _fossu.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                            }

                                            _fossu.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                            FollowupIssue.AddUpdate(_fossu);
                                        }
                                    }
                                    //If Issue not found then registed into database
                                    else
                                    {
                                        if (FirstDate <= policy.OriginalEffectiveDate && policy.OriginalEffectiveDate <= LastDate)
                                        {
                                            RegisterIssueAgainstMissingPayment(policy, FollowUpIssueCategory.MissFirst, FirstDate.Value, LastDate.Value);
                                        }
                                        else
                                        {
                                            RegisterIssueAgainstMissingPayment(policy, FollowUpIssueCategory.MissInv, FirstDate.Value, LastDate.Value);
                                        }

                                        //Get updated folloup issue list
                                        FollowupIssueLst = FollowupIssue.GetIssuesForFollowProcess(_DEU.PolicyId);

                                        List<DisplayFollowupIssue> FollowupIssueMissingtoclose1 = FollowupIssueLst.Where(p => p.FromDate == FirstDate).Where(p => p.ToDate == LastDate).Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();
                                        foreach (DisplayFollowupIssue _fossu in FollowupIssueMissingtoclose1)
                                        {
                                            if (IsEntryByCommissionDashboard)
                                            {
                                                if (_UserRole == UserRole.SuperAdmin)
                                                    _fossu.IssueResultId = (int)FollowUpResult.Resolved_CD;

                                                else
                                                    _fossu.IssueResultId = (int)FollowUpResult.Resolved_Brk;

                                            }
                                            else
                                            {
                                                _fossu.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                            }

                                            _fossu.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                            FollowupIssue.AddUpdate(_fossu);
                                        }
                                    }
                                }
                            }
                            //Update Auto term date
                            if (IsAutoTrmDateUpadte)
                            {
                                //Need to get only policy learned field termination date
                                PolicyLearnedFieldData _polrndf = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
                                if (_polrndf.AutoTerminationDate == null && policy.PolicyTerminationDate != null)
                                {
                                    _polrndf.AutoTerminationDate = policy.PolicyTerminationDate;
                                    PolicyLearnedField.AddUpdateLearned(_polrndf, _polrndf.ProductType);
                                }
                            }

                            //foreach (PolicyPaymentEntriesPost ResolvedorClosedIssue in _AllResolvedorClosedIssueId)
                            //{
                            //    DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ResolvedorClosedIssue.PolicyID) && (p.InvoiceDate == ResolvedorClosedIssue.InvoiceDate)).FirstOrDefault();
                            //    PolicyPaymentEntriesPost objPolicyPaymentEntriesPost = _AllResolvedorClosedIssueId.Where(p => (p.PolicyID == ResolvedorClosedIssue.PolicyID) && (p.InvoiceDate == ResolvedorClosedIssue.InvoiceDate)).Where(p => p.FollowUpIssueResolveOrClosed == 1).FirstOrDefault();

                            //    if (objPolicyPaymentEntriesPost != null)
                            //    {
                            //        if (objPolicyPaymentEntriesPost.PaymentEntryID != null)
                            //        {
                            //            FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                            //            FollowupIssuetemp.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                            //            FollowupIssue.AddUpdate(FollowupIssuetemp);
                            //            PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ResolvedorClosedIssue.PaymentEntryID, FollowupIssuetemp.IssueId);
                            //        }
                            //    }
                            //}

                            #region"Variance calculation"
                            foreach (PolicyPaymentEntriesPost ppepfv in _AllPaymentEntriesOnPolicyFormissing)
                            {
                                DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ppepfv.PolicyID) && (p.InvoiceDate == ppepfv.InvoiceDate)).Where(p => p.IssueCategoryID == 3).FirstOrDefault();
                                //Check variance
                                bool flagvarience = PostUtill.CheckForIncomingScheduleVariance(ppepfv, policy.ModeAvgPremium);
                                if (flagvarience)
                                {
                                    if (FollowupIssuetemp == null)
                                        RegisterIssueAgainstScheduleVariance(ppepfv);

                                    else
                                        PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ppepfv.PaymentEntryID, FollowupIssuetemp.IssueId);
                                }
                                else
                                {
                                    if (FollowupIssuetemp != null)
                                    {
                                        if (FollowupIssuetemp.IssueStatusId != (int)FollowUpIssueStatus.Closed)
                                        {
                                            FollowupIssuetemp.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                            if (IsEntryByCommissionDashboard)
                                            {
                                                if (_UserRole == UserRole.SuperAdmin)
                                                    FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;

                                                else
                                                    FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                            }
                                            else
                                            {
                                                FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                            }
                                            FollowupIssue.AddUpdate(FollowupIssuetemp);
                                        }

                                    }
                                }
                            }

                            FollowupIssueLst = FollowupIssue.GetIssuesForFollowProcess(PolicyId);
                            try
                            {
                                foreach (PolicyPaymentEntriesPost ResolvedorClosedIssue in _AllResolvedorClosedIssueId)
                                {
                                    DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ResolvedorClosedIssue.PolicyID) && (p.InvoiceDate == ResolvedorClosedIssue.InvoiceDate)).FirstOrDefault();
                                    PolicyPaymentEntriesPost objPolicyPaymentEntriesPost = _AllResolvedorClosedIssueId.Where(p => (p.PolicyID == ResolvedorClosedIssue.PolicyID) && (p.InvoiceDate == ResolvedorClosedIssue.InvoiceDate)).Where(p => p.FollowUpIssueResolveOrClosed == 1).FirstOrDefault();

                                    if (objPolicyPaymentEntriesPost != null)
                                    {
                                        if (objPolicyPaymentEntriesPost.PaymentEntryID != null)
                                        {
                                            if (FollowupIssuetemp != null)
                                            {
                                                FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                                FollowupIssuetemp.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                                FollowupIssue.AddUpdate(FollowupIssuetemp);
                                                PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ResolvedorClosedIssue.PaymentEntryID, FollowupIssuetemp.IssueId);
                                            }
                                        }
                                    }

                                }
                            }
                            catch
                            {
                            }
                            #endregion


                        }
                        else
                        {
                            //Get issse on policy ID
                            List<DisplayFollowupIssue> FollowupIssueLst = FollowupIssue.GetIssuesForFollowProcess(policy.PolicyId);
                            //Get payment on that policy
                            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPostForVarience = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(policy.PolicyId).ToList();

                            foreach (PolicyPaymentEntriesPost ppepfv in _PolicyPaymentEntriesPostForVarience)
                            {
                                DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ppepfv.PolicyID) && (p.InvoiceDate == ppepfv.InvoiceDate)).Where(p => p.IssueCategoryID == 3).FirstOrDefault();
                                bool flagvarience = PostUtill.CheckForIncomingScheduleVariance(ppepfv, policy.ModeAvgPremium);
                                if (flagvarience)
                                {
                                    if (FollowupIssuetemp == null)
                                        RegisterIssueAgainstScheduleVariance(ppepfv);
                                    else
                                        PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ppepfv.PaymentEntryID, FollowupIssuetemp.IssueId);
                                }
                                else
                                {
                                    if (FollowupIssuetemp != null)
                                    {
                                        if (FollowupIssuetemp.IssueStatusId != (int)FollowUpIssueStatus.Closed)
                                        {
                                            FollowupIssuetemp.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                            if (IsEntryByCommissionDashboard)
                                            {
                                                if (_UserRole == UserRole.SuperAdmin)
                                                    FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                else
                                                    FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                            }
                                            else
                                            {
                                                FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                            }
                                            FollowupIssue.AddUpdate(FollowupIssuetemp);
                                        }
                                    }
                                }
                            }

                            FollowupIssueLst = FollowupIssue.GetIssuesForFollowProcess(PolicyId);
                            try
                            {
                                foreach (PolicyPaymentEntriesPost ResolvedorClosedIssue in _AllResolvedorClosedIssueId)
                                {
                                    DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ResolvedorClosedIssue.PolicyID) && (p.InvoiceDate == ResolvedorClosedIssue.InvoiceDate)).FirstOrDefault();
                                    PolicyPaymentEntriesPost objPolicyPaymentEntriesPost = _AllResolvedorClosedIssueId.Where(p => (p.PolicyID == ResolvedorClosedIssue.PolicyID) && (p.InvoiceDate == ResolvedorClosedIssue.InvoiceDate)).Where(p => p.FollowUpIssueResolveOrClosed == 1).FirstOrDefault();

                                    if (objPolicyPaymentEntriesPost != null)
                                    {
                                        if (objPolicyPaymentEntriesPost.PaymentEntryID != null)
                                        {
                                            if (FollowupIssuetemp != null)
                                            {
                                                FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                                FollowupIssuetemp.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                                FollowupIssue.AddUpdate(FollowupIssuetemp);
                                                PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ResolvedorClosedIssue.PaymentEntryID, FollowupIssuetemp.IssueId);
                                            }
                                        }
                                    }

                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    #endregion

                    #region PaymentDelete
                    else if (_FollowUpRunModules == FollowUpRunModules.PaymentDeleted)
                    {
                        IsCallForDelete = true;
                        //DEU _DEUTemp = DEU.GetLatestInvoiceDateRecord(PolicyId);

                        //Added by vinod
                        DEU objDEU = new DEU();
                        DEU _DEUTemp = objDEU.GetLatestInvoiceDateRecord(PolicyId);

                        //if (_DEUTemp != null)
                        //{
                        //    FollowUpProcedure(FollowUpRunModules.PaymentEntered, _DEUTemp, Guid.Empty, true, IsEntryByCommissionDashboard, _UserRole, PolicyModeChange);
                        //}
                        if (_DEUTemp != null)
                        {
                            FollowUpProcedure(FollowUpRunModules.PaymentEntered, _DEUTemp, PolicyId, true, IsEntryByCommissionDashboard, _UserRole, PolicyModeChange);
                        }
                        else
                        {
                            PolicyDetailsData _Pode = PostUtill.GetPolicy(PolicyId);
                            if (_Pode.PolicyModeId != (int)_PolicyStatus.Pending)
                            {
                                FollowUpProcedure(FollowUpRunModules.PolicyDetailChange, null, PolicyId, _Pode.IsTrackPayment, IsEntryByCommissionDashboard, _UserRole, PolicyModeChange);
                            }

                        }
                        IsCallForDelete = false;

                    }
                    #endregion

                    #region IncomingScheduleChange
                    else if (_FollowUpRunModules == FollowUpRunModules.IncomingScheduleChange)
                    {

                        //****Update Policy Last Followup runs date****
                        //Policy.UpdateLastFollowupRunsWithTodayDate(PolicyId);
                        PolicyDetailsData policy = new PolicyDetailsData();
                        List<DisplayFollowupIssue> _FollowupIssuelst = new List<DisplayFollowupIssue>();

                        List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPostlst = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);

                        //Get If payment is available 

                        if (_PolicyPaymentEntriesPostlst.Count > 0)
                        {
                            policy = PostUtill.GetPolicy(PolicyId);
                            _FollowupIssuelst = FollowupIssue.GetIssuesForFollowProcess(PolicyId);
                        }
                        foreach (PolicyPaymentEntriesPost pope in _PolicyPaymentEntriesPostlst)
                        {
                            bool IncomingVariance = PostUtill.CheckForIncomingScheduleVariance(pope, policy.ModeAvgPremium);
                            //FollowupIssue foll = _FollowupIssuelst.Where(p => p.PolicyPaymentEntryId == pope.PaymentEntryID).FirstOrDefault();
                            DisplayFollowupIssue foll = _FollowupIssuelst.Where(p => (p.PolicyId == pope.PolicyID) && (p.InvoiceDate == pope.InvoiceDate)).Where(p => p.IssueCategoryID == 3).FirstOrDefault();
                            if (IncomingVariance)
                            {
                                if (foll == null)
                                {
                                    //RegisterIssueAgainstScheduleVariance(pope);
                                    RegisterIssueAgainstScheduleVariance(pope);
                                }
                                else
                                {
                                    if (policy.IsTrackIncomingPercentage == false)
                                    {
                                        List<DisplayFollowupIssue> AllIssueList = FollowupIssue.GetIssues(PolicyId);
                                        List<DisplayFollowupIssue> ForDeleteIssuelist = new List<DisplayFollowupIssue>(AllIssueList.Where(p => (p.IssueStatusId == (int)FollowUpIssueStatus.Open || p.IssueCategoryID == (int)FollowUpResult.Resolved_CD) && (p.IssueCategoryID == (int)FollowUpIssueCategory.VarCompDue || p.IssueCategoryID == (int)FollowUpIssueCategory.VarSchedule)));
                                        foreach (var item in ForDeleteIssuelist)
                                        {
                                            UpdateIssueIdOfPaymentsForIssueId(item.IssueId, null);
                                            //FollowupIssue.DeleteIssue(item.IssueId);
                                            FollowupIssue.DeleteFollowupIssueExceptCloseIssue(item.IssueId);
                                        }
                                    }
                                    else
                                    {
                                        PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(pope.PaymentEntryID, foll.IssueId);
                                    }

                                }
                            }
                            else
                            {
                                if (foll != null)
                                {
                                    foll.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                    if (IsEntryByCommissionDashboard)
                                    {
                                        if (_UserRole == UserRole.SuperAdmin)
                                        {
                                            foll.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                        }
                                        else
                                        {
                                            foll.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                        }
                                    }
                                    else
                                    {
                                        foll.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                    }
                                    FollowupIssue.AddUpdate(foll);
                                }
                            }
                        }

                        //If policy settings. Track incoming %=. F. 
                        //Delete issues with (status=open or result =Resolved_CD) and (category=VarSchedule or category=VarCompDue)                    
                        if (policy.IsTrackIncomingPercentage == false)
                        {
                            List<DisplayFollowupIssue> AllIssueList = FollowupIssue.GetIssuesForFollowProcess(PolicyId);
                            List<DisplayFollowupIssue> ForDeleteIssuelist = new List<DisplayFollowupIssue>(AllIssueList.Where(p => (p.IssueStatusId == (int)FollowUpIssueStatus.Open || p.IssueCategoryID == (int)FollowUpResult.Resolved_CD) && (p.IssueCategoryID == (int)FollowUpIssueCategory.VarCompDue || p.IssueCategoryID == (int)FollowUpIssueCategory.VarSchedule)));
                            foreach (var item in ForDeleteIssuelist)
                            {
                                UpdateIssueIdOfPaymentsForIssueId(item.IssueId, null);
                                //FollowupIssue.DeleteIssue(item.IssueId);
                                FollowupIssue.DeleteFollowupIssueExceptCloseIssue(item.IssueId);
                            }
                        }
                        //If policy settings. Track missing month=. F. 
                        //Delete issues with (status=open or result =Resolved_CD) and (category=miss first or category=miss inv) 
                        if (policy.IsTrackMissingMonth == false)
                        {
                            List<DisplayFollowupIssue> AllIssueList = FollowupIssue.GetIssuesForFollowProcess(PolicyId);
                            List<DisplayFollowupIssue> ForDeleteIssuelist = new List<DisplayFollowupIssue>(AllIssueList.Where(p => (p.IssueStatusId == (int)FollowUpIssueStatus.Open || p.IssueCategoryID == (int)FollowUpResult.Resolved_CD) && (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)));
                            foreach (var item in ForDeleteIssuelist)
                            {
                                UpdateIssueIdOfPaymentsForIssueId(item.IssueId, null);
                                //FollowupIssue.DeleteIssue(item.IssueId);
                                FollowupIssue.DeleteFollowupIssueExceptCloseIssue(item.IssueId);
                            }
                        }
                    }
                    #endregion

                    #region PolicyDetailChange
                    else if (_FollowUpRunModules == FollowUpRunModules.PolicyDetailChange)
                    {
                        //****Update Policy Last Followup runs date****
                        try
                        {
                            //ActionLogger.Logger.WriteImportPolicyLog("FolloWpUp Started", true);
                            //ActionLogger.Logger.WriteImportPolicyLog(System.DateTime.Now.ToString(), true);
                            //Policy.UpdateLastFollowupRunsWithTodayDate(PolicyId);
                            bool isPaymenyExist = false;
                            bool ReturnFlag = false;
                            List<DisplayFollowupIssue> FollowupIssueLst = FollowupIssue.GetIssues(PolicyId);

                            List<DisplayFollowupIssue> FollowupIssueLstRemoveIssue = FollowupIssueLst.Where(p => p.isDeleted == true).ToList();

                            DateTime? StoreFirstMissingMonth = null;
                            int noOfMissingCount = 0;
                            PolicyDetailsData policy = PostUtill.GetPolicy(PolicyId);

                            MasterPolicyMode? _MasterPolicyMode;
                            if (_DEU == null)
                            {
                                _MasterPolicyMode = PostUtill.ModeEntryFromDeu(policy, null, false);
                            }
                            else
                            {
                                _MasterPolicyMode = PostUtill.ModeEntryFromDeu(policy, _DEU.PolicyMode, true);
                            }

                            if (_MasterPolicyMode == null) return;
                            PaymentMode _PaymentMode = PostUtill.ConvertMode(_MasterPolicyMode.Value);
                            FollowUpDate _FollowUpDate = CalculateFollowUpDateRange(_PaymentMode, policy, isPaymenyExist);
                            //Advance payment
                            bool invoiceDateBelongsToRange = false;
                            bool bAvailableIntoRange = false;
                            List<DateTime> dtAdavaceDateRange = new List<DateTime>();
                            int intMulitply = 0;
                            if (_FollowUpDate != null)
                            {
                                DateTime? originalEffectiveDate = policy.OriginalEffectiveDate;
                                //Need to find invoice date and serch range inserted invoice date   

                                int? intAdvance = null;
                                if (originalEffectiveDate != null)
                                {
                                    intAdvance = policy.Advance;
                                    intMulitply = GetRangValue(_PaymentMode);

                                    for (int j = 0; j < intAdvance * intMulitply; j++)
                                    {
                                        dtAdavaceDateRange.Add(originalEffectiveDate.Value.AddMonths(j));
                                    }
                                }
                            }

                            //Get all payment at policy
                            List<PolicyPaymentEntriesPost> _AllPaymentEntriesOnPolicyFormissing = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(policy.PolicyId);
                            List<PolicyPaymentEntriesPost> _AllResolvedorClosedIssueId = PolicyPaymentEntriesPost.GetAllResolvedorClosedIssueId(policy.PolicyId);

                            if (_AllPaymentEntriesOnPolicyFormissing.Count > 0)
                            {
                                isPaymenyExist = true;
                                invoiceDateBelongsToRange = false;

                                for (int j = 0; j < _AllPaymentEntriesOnPolicyFormissing.Count; j++)
                                {
                                    //Need to chngr
                                    for (int k = 0; k < dtAdavaceDateRange.Count; k++)
                                    {
                                        //Added vinod 09052015
                                        //DateTime lastDayOfMonth = dtAdavaceDateRange[k].AddMonths(1).AddDays(-1);
                                        //Commented vinod 09052015
                                        //if (_AllPaymentEntriesOnPolicyFormissing[j].InvoiceDate.Equals(dtAdavaceDateRange[k]))
                                        //{
                                        DateTime lastDayOfMonth = dtAdavaceDateRange[k].AddMonths(1).AddDays(-1);
                                        if (_AllPaymentEntriesOnPolicyFormissing[j].InvoiceDate >= dtAdavaceDateRange[k] && _AllPaymentEntriesOnPolicyFormissing[j].InvoiceDate <= lastDayOfMonth)
                                        {
                                            invoiceDateBelongsToRange = true;
                                            break;
                                        }
                                        //if (_AllPaymentEntriesOnPolicyFormissing[j].InvoiceDate.Equals(dtAdavaceDateRange[k]))
                                        //{
                                        //    invoiceDateBelongsToRange = true;
                                        //    break;
                                        //}
                                        else
                                        {
                                            invoiceDateBelongsToRange = false;
                                        }
                                    }
                                    if (invoiceDateBelongsToRange)
                                    {
                                        invoiceDateBelongsToRange = true;
                                        break;
                                    }


                                }
                            }
                            else
                            {
                                isPaymenyExist = false;
                            }

                            #region Random
                            if (_PaymentMode == PaymentMode.Random)
                            {
                                FollowupIssueLst = FollowupIssue.GetIssues(policy.PolicyId);
                                //InValided The Issue
                                foreach (DisplayFollowupIssue follw in FollowupIssueLst)
                                {
                                    UpdateIssueIdOfPaymentsForIssueId(follw.IssueId, null);
                                    //FollowupIssue.DeleteIssue(follw.IssueId);
                                    FollowupIssue.DeleteFollowupIssueExceptCloseIssue(follw.IssueId);
                                }
                                return;
                            }
                            #endregion

                            #region OneTime
                            if (_PaymentMode == PaymentMode.OneTime)
                            {
                                if (PolicyModeChange == null && !ReturnFlag)
                                {
                                    ReturnFlag = true;
                                }

                                FollowUpDate _FollowUpDate1 = CalculateFollowUpDateRange(_PaymentMode, policy, true);
                                if (_FollowUpDate1.FromDate == null && !PolicyModeChange.Value && !ReturnFlag)
                                {
                                    ReturnFlag = true;
                                }
                                if (PolicyModeChange ?? false )
                                {
                                    if (PolicyModeChange.Value)
                                    {
                                        
                                    }
                                    //InValided The Issue
                                    foreach (DisplayFollowupIssue follw in FollowupIssueLst)
                                    {
                                        UpdateIssueIdOfPaymentsForIssueId(follw.IssueId, null);
                                        //FollowupIssue.DeleteIssue(follw.IssueId);
                                        FollowupIssue.DeleteFollowupIssueExceptCloseIssue(follw.IssueId);
                                    }
                                    if (_FollowUpDate1.FromDate == null && !ReturnFlag)
                                    {
                                        ReturnFlag = true;
                                    }
                                }

                                if (!ReturnFlag || IsCallForDelete)
                                {
                                    //List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesFormissing1 = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(policy.PolicyId);
                                    bool Rflag = ISExistsResolveIssuesForDateRange(_FollowUpDate1.FromDate.Value, DateTime.Today, policy.PolicyId, FollowupIssueLst);
                                    if (_AllPaymentEntriesOnPolicyFormissing.Count == 0)
                                    {
                                        List<DisplayFollowupIssue> issfolllst = FollowupIssueLst.Where(p => (p.FromDate == _FollowUpDate1.FromDate && p.ToDate == null))
                                           .Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv))
                                           .ToList();
                                        if (issfolllst.Count() == 0)
                                        {
                                            RegisterIssueAgainstMissingPayment(policy, FollowUpIssueCategory.MissFirst, _FollowUpDate1.FromDate.Value, null);
                                        }
                                    }

                                }
                                else
                                {
                                    FollowupIssueLst = FollowupIssue.GetIssues(PolicyId);
                                    //List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPostForVarience1 = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId).ToList();
                                    foreach (PolicyPaymentEntriesPost ppepfv in _AllPaymentEntriesOnPolicyFormissing)
                                    {
                                        DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ppepfv.PolicyID) && (p.InvoiceDate == ppepfv.InvoiceDate)).Where(p => p.IssueCategoryID == 3).FirstOrDefault();
                                        bool flagvarience = PostUtill.CheckForIncomingScheduleVariance(ppepfv, policy.ModeAvgPremium);

                                        if (!flagvarience)
                                        {
                                            if (FollowupIssuetemp == null)
                                                RegisterIssueAgainstScheduleVariance(ppepfv);
                                            else
                                                PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ppepfv.PaymentEntryID, FollowupIssuetemp.IssueId);
                                        }
                                        else
                                        {
                                            if (FollowupIssuetemp != null)
                                            {

                                                if (FollowupIssuetemp.IssueStatusId != (int)FollowUpIssueStatus.Closed)
                                                {
                                                    FollowupIssuetemp.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                                    if (IsEntryByCommissionDashboard)
                                                    {
                                                        if (_UserRole == UserRole.SuperAdmin)
                                                            FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                        else
                                                            FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                                    }
                                                    else
                                                    {
                                                        FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                    }

                                                    // FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                    FollowupIssue.AddUpdate(FollowupIssuetemp);
                                                }
                                            }
                                        }
                                    }
                                }
                                return;
                            }
                            #endregion

                            if (PolicyModeChange == null && !ReturnFlag)
                            {
                                ReturnFlag = true;
                            }
                            //ActionLogger.Logger.WriteImportPolicyLog("before calculated mode", true);
                            //FollowUpDate _FollowUpDate = CalculateFollowUpDateRange(_PaymentMode, policy, isPaymenyExist);
                            //ActionLogger.Logger.WriteImportPolicyLog("After calculated mode", true);

                            if (_FollowUpDate.FromDate == null && !(PolicyModeChange ?? false) && !ReturnFlag)
                            {
                                ReturnFlag = true;
                            }

                            if (_FollowUpDate.ToDate == null && !(PolicyModeChange ?? false) && !ReturnFlag)
                            {
                                ReturnFlag = true;
                            }

                            if ((_FollowUpDate.FromDate > _FollowUpDate.ToDate) && !(PolicyModeChange ?? false) && !ReturnFlag)
                            {
                                ReturnFlag = true;
                            }

                            List<DateRange> _DateRangeForMissingLst = null;

                            if ((_FollowUpDate.FromDate != null) && (_FollowUpDate.ToDate != null) && ((_FollowUpDate.FromDate < _FollowUpDate.ToDate)))
                            {
                                _DateRangeForMissingLst = MakeFollowUpDateRangeForMissing(_FollowUpDate, _PaymentMode);
                                ReturnFlag = false;
                            }

                            if (_DateRangeForMissingLst == null)
                            {
                                //Delete missing issue and retrn updatee from history table
                                foreach (DisplayFollowupIssue follw in FollowupIssueLst)
                                {
                                    UpdateIssueIdOfPaymentsForIssueId(follw.IssueId, null);
                                    //FollowupIssue.DeleteIssue(follw.IssueId);
                                    FollowupIssue.DeleteFollowupIssueExceptCloseIssue(follw.IssueId);
                                }
                                //return;
                            }

                            if (PolicyModeChange ?? false)
                            {
                                //InValided The Issue 
                                //delete the previous issue if mode change 
                                if (PolicyModeChange.Value)
                                {
                                    foreach (DisplayFollowupIssue follw in FollowupIssueLst)
                                    {
                                        UpdateIssueIdOfPaymentsForIssueId(follw.IssueId, null);
                                        //FollowupIssue.DeleteIssue(follw.IssueId);
                                        FollowupIssue.DeleteFollowupIssueExceptCloseIssue(follw.IssueId);
                                    }
                                    if (_FollowUpDate.FromDate == null && !ReturnFlag)
                                    {
                                        ReturnFlag = true;
                                    }

                                    if (_FollowUpDate.ToDate == null && !ReturnFlag)
                                    {
                                        ReturnFlag = true;
                                    }

                                    if ((_FollowUpDate.FromDate > _FollowUpDate.ToDate) && !ReturnFlag)
                                    {
                                        ReturnFlag = true;
                                    }
                                }

                            }
                            else
                            {
                                if (!ReturnFlag || IsCallForDelete)
                                {
                                    //Get the issue range and delete before and after issue of the range
                                    List<DisplayFollowupIssue> _FollowupIssueDoInValid = FollowupIssueLst.Where(p => (p.FromDate < _FollowUpDate.FromDate || p.ToDate > _FollowUpDate.ToDate)).ToList();

                                    foreach (DisplayFollowupIssue closedIssue in _FollowupIssueDoInValid)
                                    {
                                        if (closedIssue.IssueStatusId != (int)FollowUpIssueStatus.Closed)
                                        {
                                            UpdateIssueIdOfPaymentsForIssueId(closedIssue.IssueId, null);
                                            //FollowupIssue.DeleteIssue(closedIssue.IssueId);
                                            FollowupIssue.DeleteFollowupIssueExceptCloseIssue(closedIssue.IssueId);
                                        }
                                    }

                                    //Get the follow up issue
                                    FollowupIssueLst = FollowupIssue.GetIssues(PolicyId);

                                    //GEt Issue which is not closed and not the varriance into the payment
                                    foreach (DisplayFollowupIssue follw in FollowupIssueLst.Where(p => p.IssueStatusId != (int)FollowUpIssueStatus.Closed).Where(p => p.IssueCategoryID != (int)FollowUpIssueCategory.VarSchedule))
                                    {
                                        bool flag = false;

                                        for (int idx = 0; idx < _DateRangeForMissingLst.Last().RANGE; idx++)
                                        {
                                            DateTime? FirstDate = _DateRangeForMissingLst.Where(p => p.RANGE == idx + 1).ToList()[0].STARTDATE;
                                            DateTime? LastDate = _DateRangeForMissingLst.Where(p => p.RANGE == idx + 1).ToList()[(int)_PaymentMode - 1].ENDDATE;

                                            if (follw.FromDate == FirstDate && follw.ToDate == LastDate)
                                            {
                                                flag = true;
                                                break;
                                            }
                                            else
                                            {
                                                flag = false;
                                            }

                                        }
                                        if (!flag)
                                        {
                                            UpdateIssueIdOfPaymentsForIssueId(follw.IssueId, null);
                                            //FollowupIssue.DeleteIssue(follw.IssueId);
                                            FollowupIssue.DeleteFollowupIssueExceptCloseIssue(follw.IssueId);
                                        }
                                    }
                                }

                            }

                            if (!ReturnFlag || IsCallForDelete)
                            {

                                //Get the follow up issue
                                FollowupIssueLst = FollowupIssue.GetIssues(PolicyId);
                                //Get all payment at policy

                                //List<PolicyPaymentEntriesPost> _AllPaymentEntriesOnPolicyFormissing = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(policy.PolicyId);

                                for (int idx = 0; idx < _DateRangeForMissingLst.Last().RANGE; idx++)
                                {
                                    DateTime? FirstDate = null;
                                    DateTime? LastDate = null;

                                    FirstDate = _DateRangeForMissingLst.Where(p => p.RANGE == idx + 1).ToList()[0].STARTDATE;
                                    LastDate = _DateRangeForMissingLst.Where(p => p.RANGE == idx + 1).ToList()[(int)_PaymentMode - 1].ENDDATE;


                                    //List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesFormissing = PolicyPaymentEntriesPost.GetAllPaymentEntriesOfRange(FirstDate.Value, LastDate.Value, policy.PolicyId);
                                    //Get the payment at given range
                                    List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesFormissing = _AllPaymentEntriesOnPolicyFormissing.Where(p => p.InvoiceDate >= FirstDate.Value).Where(p => p.InvoiceDate <= LastDate.Value).ToList<PolicyPaymentEntriesPost>();

                                    bool Rflag = ISExistsResolveIssuesForDateRange(FirstDate.Value, LastDate.Value, policy.PolicyId, FollowupIssueLst);
                                    if (_PolicyPaymentEntriesFormissing.Count == 0)
                                    {
                                        StoreFirstMissingMonth = FirstDate;
                                        noOfMissingCount++;
                                        List<DisplayFollowupIssue> issfolllst = FollowupIssueLst.Where(p => (p.FromDate == FirstDate && p.ToDate == LastDate)).Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();
                                        if (issfolllst.Count() != 0)
                                        {
                                            if (Rflag)
                                            {
                                                StoreFirstMissingMonth = null;
                                                noOfMissingCount = 0;
                                            }
                                            if (FirstDate <= policy.OriginalEffectiveDate && policy.OriginalEffectiveDate <= LastDate)
                                            {
                                                DisplayFollowupIssue tempfis = issfolllst.FirstOrDefault();
                                                if (tempfis.IssueCategoryID != (int)FollowUpIssueCategory.MissFirst)
                                                {
                                                    tempfis.IssueCategoryID = (int)FollowUpIssueCategory.MissFirst;
                                                    //FollowupIssue.AddUpdate(tempfis);
                                                }

                                                if (invoiceDateBelongsToRange)
                                                {
                                                    foreach (var item in dtAdavaceDateRange)
                                                    {
                                                        if (item.Equals(FirstDate.Value))
                                                        {
                                                            bAvailableIntoRange = true;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            bAvailableIntoRange = false;
                                                        }

                                                    }
                                                }

                                                if (bAvailableIntoRange)
                                                {
                                                    if (IsEntryByCommissionDashboard)
                                                    {
                                                        if (_UserRole == UserRole.SuperAdmin)
                                                            tempfis.IssueResultId = (int)FollowUpResult.Resolved_CD;

                                                        else
                                                            tempfis.IssueResultId = (int)FollowUpResult.Resolved_Brk;

                                                    }
                                                    else
                                                    {
                                                        tempfis.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                    }

                                                    tempfis.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                                }

                                                FollowupIssue.AddUpdate(tempfis);
                                            }
                                            else
                                            {
                                                DisplayFollowupIssue tempfis = issfolllst.FirstOrDefault();
                                                if (tempfis.IssueCategoryID != (int)FollowUpIssueCategory.MissInv)
                                                {
                                                    tempfis.IssueCategoryID = (int)FollowUpIssueCategory.MissInv;
                                                    // FollowupIssue.AddUpdate(tempfis);
                                                }

                                                if (invoiceDateBelongsToRange)
                                                {
                                                    foreach (var item in dtAdavaceDateRange)
                                                    {
                                                        if (item.Equals(FirstDate.Value))
                                                        {
                                                            bAvailableIntoRange = true;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            bAvailableIntoRange = false;
                                                        }
                                                    }
                                                }

                                                if (bAvailableIntoRange)
                                                {
                                                    if (IsEntryByCommissionDashboard)
                                                    {
                                                        if (_UserRole == UserRole.SuperAdmin)
                                                            tempfis.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                        else
                                                            tempfis.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                                    }
                                                    else
                                                    {
                                                        tempfis.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                    }
                                                    tempfis.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                                }
                                                FollowupIssue.AddUpdate(tempfis);
                                            }
                                        }
                                        else
                                        {
                                            if (FirstDate <= policy.OriginalEffectiveDate && policy.OriginalEffectiveDate <= LastDate)
                                            {
                                                DisplayFollowupIssue objItem = FollowupIssueLstRemoveIssue.Where(p => p.FromDate == FirstDate && p.ToDate == LastDate).FirstOrDefault();
                                                //objItem find then no need to add into database
                                                if (objItem == null)
                                                    RegisterIssueAgainstMissingPayment(policy, FollowUpIssueCategory.MissFirst, FirstDate.Value, LastDate.Value);
                                            }
                                            else
                                            {
                                                DisplayFollowupIssue objItem = FollowupIssueLstRemoveIssue.Where(p => p.FromDate == FirstDate && p.ToDate == LastDate).FirstOrDefault();
                                                //objItem find then no need to add into database
                                                if (objItem == null)
                                                    RegisterIssueAgainstMissingPayment(policy, FollowUpIssueCategory.MissInv, FirstDate.Value, LastDate.Value);
                                            }

                                            if (invoiceDateBelongsToRange)
                                            {
                                                foreach (var item in dtAdavaceDateRange)
                                                {
                                                    if (item.Equals(FirstDate.Value))
                                                    {
                                                        bAvailableIntoRange = true;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        bAvailableIntoRange = false;
                                                    }
                                                }
                                            }

                                            if (bAvailableIntoRange)
                                            {
                                                FollowupIssueLst = FollowupIssue.GetIssues(policy.PolicyId);
                                                List<DisplayFollowupIssue> issueFollowUp = FollowupIssueLst.Where(p => (p.FromDate == FirstDate && p.ToDate == LastDate)).Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();
                                                DisplayFollowupIssue tempIssue = issueFollowUp.FirstOrDefault();

                                                if (IsEntryByCommissionDashboard)
                                                {
                                                    if (_UserRole == UserRole.SuperAdmin)
                                                        tempIssue.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                    else
                                                        tempIssue.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                                }
                                                else
                                                {
                                                    tempIssue.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                }
                                                tempIssue.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                                FollowupIssue.AddUpdate(tempIssue);
                                            }
                                        }


                                        if (noOfMissingCount != 0 && StoreFirstMissingMonth != null && IsAutoTrmDateUpadte)
                                            AutoPolicyTerminateProcess(_PaymentMode, StoreFirstMissingMonth, policy.PolicyId, noOfMissingCount, null);
                                    }
                                    else
                                    {
                                        //Acme added:
                                        //Acme added:
                                        if (FirstDate != null && LastDate != null)
                                        {
                                            PostUtill.HandlePMCVariance(_PolicyPaymentEntriesFormissing, policy.LearnedFields.PMC, Convert.ToDateTime(FirstDate), Convert.ToDateTime(LastDate), PolicyId);
                                        }

                                        StoreFirstMissingMonth = null;
                                        noOfMissingCount = 0;
                                        //List<DisplayFollowupIssue> FollowupIssueMissingtoclose = FollowupIssueLst.Where(p => p.FromDate == FirstDate).Where(p => p.ToDate == LastDate).Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();

                                        List<DisplayFollowupIssue> FollowupIssueMissingtoclose = FollowupIssueLst.Where(p => p.FromDate >= FirstDate).Where(p => p.ToDate <= LastDate).Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();

                                        if (FollowupIssueMissingtoclose.Count > 0)
                                        {
                                            foreach (DisplayFollowupIssue _fossu in FollowupIssueMissingtoclose)
                                            {
                                                if (IsEntryByCommissionDashboard)
                                                {
                                                    if (_UserRole == UserRole.SuperAdmin)
                                                        _fossu.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                    else
                                                        _fossu.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                                }
                                                else
                                                {
                                                    _fossu.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                }
                                                _fossu.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                                FollowupIssue.AddUpdate(_fossu);
                                            }
                                        }
                                        else
                                        {
                                            if (FirstDate <= policy.OriginalEffectiveDate && policy.OriginalEffectiveDate <= LastDate)
                                            {
                                                DisplayFollowupIssue objItem = FollowupIssueLstRemoveIssue.Where(p => p.FromDate == FirstDate && p.ToDate == LastDate).FirstOrDefault();
                                                //objItem find then no need to add into database
                                                if (objItem == null)
                                                    RegisterIssueAgainstMissingPayment(policy, FollowUpIssueCategory.MissFirst, FirstDate.Value, LastDate.Value);
                                            }
                                            else
                                            {
                                                DisplayFollowupIssue objItem = FollowupIssueLstRemoveIssue.Where(p => p.FromDate == FirstDate && p.ToDate == LastDate).FirstOrDefault();
                                                //objItem find then no need to add into database
                                                if (objItem == null)
                                                    RegisterIssueAgainstMissingPayment(policy, FollowUpIssueCategory.MissInv, FirstDate.Value, LastDate.Value);
                                            }

                                            FollowupIssueLst = FollowupIssue.GetIssues(policy.PolicyId);

                                            List<DisplayFollowupIssue> FollowupIssueMissingtoclose1 = FollowupIssueLst.Where(p => p.FromDate == FirstDate).Where(p => p.ToDate == LastDate).Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();
                                            foreach (DisplayFollowupIssue _fossu in FollowupIssueMissingtoclose1)
                                            {
                                                if (IsEntryByCommissionDashboard)
                                                {
                                                    if (_UserRole == UserRole.SuperAdmin)
                                                        _fossu.IssueResultId = (int)FollowUpResult.Resolved_CD;

                                                    else
                                                        _fossu.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                                }
                                                else
                                                {
                                                    _fossu.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                }

                                                _fossu.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                                FollowupIssue.AddUpdate(_fossu);
                                            }

                                        }
                                    }
                                }
                            }

                            //FollowupIssueLst = FollowupIssue.GetIssues(PolicyId);                           

                            //try
                            //{
                            //    foreach (PolicyPaymentEntriesPost ResolvedorClosedIssue in _AllResolvedorClosedIssueId)
                            //    {
                            //        DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ResolvedorClosedIssue.PolicyID) && (p.InvoiceDate == ResolvedorClosedIssue.InvoiceDate)).FirstOrDefault();
                            //        PolicyPaymentEntriesPost objPolicyPaymentEntriesPost = _AllResolvedorClosedIssueId.Where(p => (p.PolicyID == ResolvedorClosedIssue.PolicyID) && (p.InvoiceDate == ResolvedorClosedIssue.InvoiceDate)).Where(p => p.FollowUpIssueResolveOrClosed == 1).FirstOrDefault();

                            //        if (objPolicyPaymentEntriesPost != null)
                            //        {
                            //            if (objPolicyPaymentEntriesPost.PaymentEntryID != null)
                            //            {
                            //                if (FollowupIssuetemp != null)
                            //                {
                            //                    FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                            //                    FollowupIssuetemp.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                            //                    FollowupIssue.AddUpdate(FollowupIssuetemp);
                            //                    PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ResolvedorClosedIssue.PaymentEntryID, FollowupIssuetemp.IssueId);
                            //                }
                            //            }
                            //        }

                            //    }
                            //}
                            //catch
                            //{
                            //}

                            foreach (PolicyPaymentEntriesPost ppepfv in _AllPaymentEntriesOnPolicyFormissing)
                            {
                                DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ppepfv.PolicyID) && (p.InvoiceDate == ppepfv.InvoiceDate)).Where(p => p.IssueCategoryID == 3).FirstOrDefault();

                                bool flagvarience = PostUtill.CheckForIncomingScheduleVariance(ppepfv, policy.ModeAvgPremium);
                                if (flagvarience)
                                {
                                    //if (FollowupIssuetemp == null)
                                        RegisterIssueAgainstScheduleVariance(ppepfv);
                                   // else
                                        //PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ppepfv.PaymentEntryID, FollowupIssuetemp.IssueId);
                                }
                                else
                                {
                                    if (FollowupIssuetemp != null)
                                    {
                                        if (FollowupIssuetemp.IssueStatusId != (int)FollowUpIssueStatus.Closed)
                                        {
                                            FollowupIssuetemp.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                            if (IsEntryByCommissionDashboard)
                                            {
                                                if (_UserRole == UserRole.SuperAdmin)
                                                    FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                                else
                                                    FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                            }
                                            else
                                            {
                                                FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_CD;
                                            }
                                            FollowupIssue.AddUpdate(FollowupIssuetemp);
                                        }
                                    }
                                }
                            }

                            FollowupIssueLst = FollowupIssue.GetIssues(PolicyId);                          
                            try
                            {
                                foreach (PolicyPaymentEntriesPost ResolvedorClosedIssue in _AllResolvedorClosedIssueId)
                                {
                                    DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ResolvedorClosedIssue.PolicyID) && (p.InvoiceDate == ResolvedorClosedIssue.InvoiceDate)).FirstOrDefault();
                                    PolicyPaymentEntriesPost objPolicyPaymentEntriesPost = _AllResolvedorClosedIssueId.Where(p => (p.PolicyID == ResolvedorClosedIssue.PolicyID) && (p.InvoiceDate == ResolvedorClosedIssue.InvoiceDate)).Where(p => p.FollowUpIssueResolveOrClosed == 1).FirstOrDefault();

                                    if (objPolicyPaymentEntriesPost != null)
                                    {
                                        if (objPolicyPaymentEntriesPost.PaymentEntryID != null)
                                        {
                                            if (FollowupIssuetemp != null)
                                            {
                                                FollowupIssuetemp.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                                                FollowupIssuetemp.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                                                FollowupIssue.AddUpdate(FollowupIssuetemp);
                                                PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ResolvedorClosedIssue.PaymentEntryID, FollowupIssuetemp.IssueId);
                                            }
                                        }
                                    }

                                }
                            }
                            catch
                            {
                            }

                            //If policy settings. Track incoming %=. F. 
                            //Delete issues with (status=open or result =Resolved_CD) and (category=VarSchedule or category=VarCompDue)                    
                            if (policy.IsTrackIncomingPercentage == false)
                            {
                                List<DisplayFollowupIssue> AllIssueList = FollowupIssue.GetIssues(PolicyId);
                                List<DisplayFollowupIssue> ForDeleteIssuelist = new List<DisplayFollowupIssue>(AllIssueList.Where(p => (p.IssueStatusId == (int)FollowUpIssueStatus.Open || p.IssueCategoryID == (int)FollowUpResult.Resolved_CD) && (p.IssueCategoryID == (int)FollowUpIssueCategory.VarCompDue || p.IssueCategoryID == (int)FollowUpIssueCategory.VarSchedule)));
                                foreach (var item in ForDeleteIssuelist)
                                {
                                    UpdateIssueIdOfPaymentsForIssueId(item.IssueId, null);
                                    //FollowupIssue.DeleteIssue(item.IssueId);
                                    FollowupIssue.DeleteFollowupIssueExceptCloseIssue(item.IssueId);
                                }
                            }
                            //If policy settings. Track missing month=. F. 
                            //Delete issues with (status=open or result =Resolved_CD) and (category=miss first or category=miss inv) 
                            if (policy.IsTrackMissingMonth == false)
                            {
                                List<DisplayFollowupIssue> AllIssueList = FollowupIssue.GetIssues(PolicyId);
                                List<DisplayFollowupIssue> ForDeleteIssuelist = new List<DisplayFollowupIssue>(AllIssueList.Where(p => (p.IssueStatusId == (int)FollowUpIssueStatus.Open || p.IssueCategoryID == (int)FollowUpResult.Resolved_CD) && (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)));
                                foreach (var item in ForDeleteIssuelist)
                                {
                                    UpdateIssueIdOfPaymentsForIssueId(item.IssueId, null);
                                    //FollowupIssue.DeleteIssue(item.IssueId);
                                    FollowupIssue.DeleteFollowupIssueExceptCloseIssue(item.IssueId);
                                }
                            }
                            //ActionLogger.Logger.WriteImportPolicyLog("FolloWpUp End", true);
                            //ActionLogger.Logger.WriteImportPolicyLog(System.DateTime.Now.ToString(), true);
                        }
                        catch (Exception ex)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog(ex.StackTrace.ToString(), true);
                        }
                    }

                    #endregion

                    #region ResolveIssues
                    /*
                     * Need only 
                     *  FollowupRunModule enum-ResolveIssue
                     *  PolicyId-
                     *  IsTrackPayment-
                     *  IsEntryByCommissionDashBoard-false
                     *  UserRole-
                     */
                    else if (_FollowUpRunModules == FollowUpRunModules.ResolveIssue)
                    {

                        //Policy.UpdateLastFollowupRunsWithTodayDate(PolicyId);

                        PolicyDetailsData policy = PostUtill.GetPolicy(PolicyId);
                        MasterPolicyMode? _MasterPolicyMode;
                        if (_DEU == null)
                        {
                            _MasterPolicyMode = PostUtill.ModeEntryFromDeu(policy, null, false);
                        }
                        else
                        {
                            _MasterPolicyMode = PostUtill.ModeEntryFromDeu(policy, _DEU.PolicyMode, true);
                        }
                        if (_MasterPolicyMode == null) return;
                        PaymentMode _PaymentMode = PostUtill.ConvertMode(_MasterPolicyMode.Value);

                        DateTime? StoreFirstMissingMonth = null;
                        int noOfMissingCount = 0;

                        List<DisplayFollowupIssue> _FollowupIssueLst = FollowupIssue.GetIssues(PolicyId);
                        if (_FollowupIssueLst != null && _FollowupIssueLst.Count != 0)
                        {
                            _FollowupIssueLst = _FollowupIssueLst.OrderBy(p => p.FromDate).ToList();
                        }
                        foreach (DisplayFollowupIssue follo in _FollowupIssueLst)
                        {
                            if (follo.IssueStatusId == (int)FollowUpIssueStatus.Closed)
                            {
                                StoreFirstMissingMonth = null;
                                noOfMissingCount = 0;
                            }
                            else
                            {
                                StoreFirstMissingMonth = follo.FromDate;
                                noOfMissingCount++;
                                if (noOfMissingCount != 0 && StoreFirstMissingMonth != null && IsAutoTrmDateUpadte)
                                    AutoPolicyTerminateProcess(_PaymentMode, StoreFirstMissingMonth, PolicyId, noOfMissingCount, null);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    if (PolicyModeChange == true)
                    {
                        List<DisplayFollowupIssue> AllIssueList = FollowupIssue.GetIssues(PolicyId);
                        List<DisplayFollowupIssue> ForDeleteIssuelist = new List<DisplayFollowupIssue>(AllIssueList.Where(p => p.IssueStatusId == (int)FollowUpIssueStatus.Open || (p.IssueStatusId == (int)FollowUpIssueStatus.Closed && p.IssueCategoryID == (int)FollowUpResult.Resolved_CD)));
                        foreach (var item in ForDeleteIssuelist)
                        {
                            //FollowupIssue.DeleteIssue(item.IssueId);
                            FollowupIssue.DeleteFollowupIssueExceptCloseIssue(item.IssueId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IsCallForDelete = false;
                //****Update Policy Last Followup runs date****
                //Policy.UpdateLastFollowupRunsWithTodayDate(PolicyId);

                //ActionLogger.Logger.WriteImportPolicyLog(" issue while running follow up. FolloWpUp Failed", true);
                ActionLogger.Logger.WriteImportPolicyLog(ex.StackTrace.ToString(), true);
                //ActionLogger.Logger.WriteImportPolicyLog("-----------", true);
                #if DEBUG
                                //throw new Exception();
                #endif
            }

        }


        public class clsAdvancePayment
        {
            DateTime? stariMonthDate { get; set; }
            DateTime? EndMonthDate { get; set; }
        }

        private static List<DisplayFollowupIssue> distinct(List<DisplayFollowupIssue> FollowupIssueLst)
        {
            List<DateTime?> UniqueDateTime = FollowupIssueLst.Select(c => c.InvoiceDate).Distinct().ToList();

            List<DisplayFollowupIssue> _TempDisplayFollowupIssue = new List<DisplayFollowupIssue>(FollowupIssueLst);

            FollowupIssueLst.Clear();

            foreach (DateTime dt in UniqueDateTime)
            {
                FollowupIssueLst.Add(_TempDisplayFollowupIssue.Where(p => p.InvoiceDate == dt).FirstOrDefault());
            }
            return FollowupIssueLst;
        }

        private static void FollowupProcedureforOnTimePayment(Guid PolicyId, decimal? Premium, bool IsEntryByCommissionDashboard, List<DisplayFollowupIssue> FollowupIssueLst, UserRole _UserRole, Guid DeuentryId)
        {
            // If Paymentmode is one time load first followup issue is exists
            DisplayFollowupIssue follomissing = FollowupIssueLst.Where(p => p.IssueCategoryID != (int)FollowUpIssueCategory.VarSchedule).FirstOrDefault();

            UpdateResultIdforFollowUpdIssue(follomissing, IsEntryByCommissionDashboard, _UserRole);

            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPostForVarience = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId).ToList();
            foreach (PolicyPaymentEntriesPost ppepfv in _PolicyPaymentEntriesPostForVarience)
            {
                DisplayFollowupIssue FollowupIssuetemp = FollowupIssueLst.Where(p => (p.PolicyId == ppepfv.PolicyID) && (p.InvoiceDate == ppepfv.InvoiceDate)).Where(p => p.IssueCategoryID == 3).FirstOrDefault();

                bool flagvarience = PostUtill.CheckForIncomingScheduleVariance(ppepfv, Premium);
                //bool flagvarience = PostUtill.CheckForIncomingScheduleVariance(ppepfv, Premium,DateTime.Now);
                if (flagvarience)
                {
                    if (FollowupIssuetemp == null)
                        RegisterIssueAgainstScheduleVariance(ppepfv);

                    else
                        PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(ppepfv.PaymentEntryID, FollowupIssuetemp.IssueId);

                }
                else
                    UpdateResultIdforFollowUpdIssue(FollowupIssuetemp, IsEntryByCommissionDashboard, _UserRole);
            }
            //terminate the policy
            PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            DateTime? EffDate = _PolicyLearnedField.Effective;
            if (EffDate != null)
            {
                DateTime CalTermDate = _PolicyLearnedField.Effective.Value.AddYears(1);
                UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);
            }
            else if (EffDate == null)
            {
                PolicyPaymentEntriesPost _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryDEUEntryIdWise(DeuentryId);
                DateTime? InvoiceDate = _PolicyPaymentEntriesPost.InvoiceDate;
                InvoiceDate = InvoiceDate.Value.AddDays(1);
                UpdateTheAutoTermDateOfLearned(PolicyId, InvoiceDate);
            }
        }

        private static void UpdateResultIdforFollowUpdIssue(DisplayFollowupIssue followupIssue, bool IsEntryByCommissionDashboard, UserRole _UserRole)
        {
            if (followupIssue != null)
            {
                //Mark the status of the Followup issue as closed
                //if (followupIssue.IssueStatusId != (int)FollowUpIssueStatus.Closed)
                //{
                followupIssue.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                //Mark the IssueResult as per the user if it is entered through Commission dashboard else mark it to Resolved_CD
                if (IsEntryByCommissionDashboard)
                {
                    if (_UserRole == UserRole.SuperAdmin)
                    {
                        followupIssue.IssueResultId = (int)FollowUpResult.Resolved_CD;
                    }
                    else
                    {
                        followupIssue.IssueResultId = (int)FollowUpResult.Resolved_Brk;
                    }

                }
                else
                {
                    followupIssue.IssueResultId = (int)FollowUpResult.Resolved_CD;
                }
                //FollowupIssue.DeleteIssue(followupIssue.IssueId);
                FollowupIssue.DeleteFollowupIssueExceptCloseIssue(followupIssue.IssueId);
                FollowupIssue.AddUpdate(followupIssue);

            }
        }

        public static void UpdateIssueIdOfPaymentsForIssueId(Guid IssueId, Guid? ResultIssueId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.PolicyPaymentEntry> _PolicyPaymentEntryLst = DataModel.PolicyPaymentEntries.Where(p => p.FollowUpVarIssueId == IssueId).ToList();
                _PolicyPaymentEntryLst.ForEach(p => p.FollowUpVarIssueId = ResultIssueId);
                DataModel.SaveChanges();
            }
        }

        private static List<DateRange> CreateFollowUpissueTrackDateRangeFromIssues(Guid PolicyId)
        {

            List<DateRange> _DateRangeLst = null;
            //List<FollowupIssue> _FollowupIssueLst = FollowupIssue.GetIssues(PolicyId);
            //if (_FollowupIssueLst != null && _FollowupIssueLst.Count > 0)
            //{
            //    _DateRangeLst = new List<DateRange>();
            //}

            //DateRange _DateRange = null;
            //int range = 0;
            //foreach (FollowupIssue follissu in _FollowupIssueLst)
            //{
            //    range++;
            //    _DateRange = new DateRange(follissu.FromDate, follissu.ToDate, range);
            //    _DateRangeLst.Add(_DateRange);
            //}
            return _DateRangeLst;
        }

        public static bool ISExistsResolveIssuesForDateRange(DateTime FromDate, DateTime ToDate, Guid PolicyId, List<DisplayFollowupIssue> followupIssueList)
        {
            //List<DisplayFollowupIssue> _FollowupIssue = FollowupIssue.GetIssues(PolicyId);

            List<DisplayFollowupIssue> _FollowupIssue = new List<DisplayFollowupIssue>((followupIssueList).ToList());

            if (_FollowupIssue != null && _FollowupIssue.Count != 0)
            {
                _FollowupIssue = _FollowupIssue.Where(p => ((p.IssueCategoryID.Value == (int)FollowUpIssueCategory.MissFirst || p.IssueCategoryID.Value == (int)FollowUpIssueCategory.MissInv) && (p.FromDate >= FromDate && p.ToDate <= ToDate))).ToList();
            }
            if (_FollowupIssue.Where(p => p.IssueStatusId == (int)FollowUpIssueStatus.Closed).Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void ClearPolicySmartAutoTermDate(Guid PolicyId, DEU deu)
        {
            try
            {
                PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
                if (_PolicyLearnedField != null)
                {
                    if (deu != null)
                    {
                        _PolicyLearnedField.Effective = deu.OriginalEffectiveDate;
                    }
                }
                _PolicyLearnedField.AutoTerminationDate = null;
                PolicyLearnedField.AddUpdateLearned(_PolicyLearnedField, _PolicyLearnedField.ProductType);
            }
            catch(Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("ClearPolicySmartAutoTermDate ex.message" + ex.Message.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("ClearPolicySmartAutoTermDate ex.StackTrace" + ex.StackTrace.ToString(), true);
            }
            //No use to query DB again as this variable is not used
            //_PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
        }

        public static int MonthBetweenTwoYear(DateTime dt1, DateTime dt2)
        {
            //Commented on  26 5 2011 By Ankur
            //int y = dt2.Year - dt1.Year;
            //int M = ((y * 12) + Math.Abs((dt2.Month - dt1.Month)));
            //return M;
            int noOfMonth = 0;
            DateTime _LoopDate = dt1;
            DateTime _EndDate = dt2;

            while (_LoopDate <= _EndDate)
            {
                System.Globalization.DateTimeFormatInfo d = new System.Globalization.DateTimeFormatInfo();

                _LoopDate = _LoopDate.AddMonths(1);
                noOfMonth++;
            }
            return noOfMonth;
        }
        /// <summary>
        /// Make a date range for From date and To date according Mode
        /// </summary>
        /// <param name="_FollowUpDate"></param>
        /// <param name="_PaymentMode"></param>
        /// <returns></returns>
        public static List<DateRange> MakeFollowUpDateRangeForMissing(FollowUpDate _FollowUpDate, PaymentMode _PaymentMode)
        {
            int NoofMonth = MonthBetweenTwoYear(_FollowUpDate.FromDate.Value, _FollowUpDate.ToDate.Value);
            int RemoveMonth = NoofMonth % (int)_PaymentMode;
            int finalTrackMonth = NoofMonth - RemoveMonth;

            List<DateRange> _DtRngs = new List<DateRange>();
            DateTime? DATE = _FollowUpDate.FromDate;

            for (int idx = 1; idx <= (finalTrackMonth / (int)_PaymentMode); idx++)
            {
                for (int idx2 = 1; idx2 <= (int)_PaymentMode; idx2++)
                {
                    DateTime? strdt = FollowUpUtill.FirstDate(DATE);
                    DateTime? enddt = FollowUpUtill.LastDate(DATE);

                    _DtRngs.Add(new DateRange(strdt, enddt, idx));
                    DATE = DATE.Value.AddMonths(1);
                }
            }


            return _DtRngs;
        }

        private static List<DateRange> MakeFollowUpDateRangeForVariance(FollowUpDate _FollowUpDate, PaymentMode _PaymentMode)
        {
            int NoofMonth = _FollowUpDate.ToDate.Value.Month - _FollowUpDate.FromDate.Value.Month;
            int RemoveMonth = NoofMonth % (int)_PaymentMode;
            int finalTrackMonth = NoofMonth - RemoveMonth;

            List<DateRange> _DtRngs = new List<DateRange>();
            DateTime? DATE = _FollowUpDate.FromDate;
            for (int idx = 1; idx <= (finalTrackMonth / (int)_PaymentMode); idx++)
            {

                for (int idx2 = 1; idx2 <= (int)_PaymentMode; idx2++)
                {

                    DateTime? strdt = FollowUpUtill.FirstDate(DATE);
                    DateTime? enddt = FollowUpUtill.LastDate(DATE);
                    _DtRngs.Add(new DateRange(strdt, enddt, idx));
                    DATE.Value.AddMonths(1);
                }
            }
            return _DtRngs;
        }

        /// <summary>
        /// Calculate FollowUp Date
        /// </summary>
        /// <param name="_PaymentType"></param>
        /// <param name="PolicyId"></param>
        /// <returns></returns>
        //public static FollowUpDate CalculateFollowUpDateRange(PaymentMode _PaymentType, PolicyDetailsData Policy)
        public static FollowUpDate CalculateFollowUpDateRange(PaymentMode _PaymentType, PolicyDetailsData Policy, bool IsPaymentExist)
        {
            DateTime? FromDate = null;
            DateTime? ToDate = null;

            switch (_PaymentType.ToString())
            {
                case "Monthly":
                    FromDate = FromMonthlyDate(Policy);
                    ToDate = ToMOnthlyDate(Policy, FromDate);
                    break;
                case "Quarterly":
                    FromDate = FromQuaterlyDate(Policy, IsPaymentExist);
                    ToDate = ToQuaterlyDate(Policy, FromDate, IsPaymentExist);
                    break;
                case "HalfYearly":
                    FromDate = FromHalfYearlyDate(Policy.PolicyId, IsPaymentExist);
                    ToDate = ToHalfYearlyDate(Policy, FromDate, IsPaymentExist);
                    break;
                case "Yearly":
                    FromDate = FromYearlyDate(Policy.PolicyId, IsPaymentExist);
                    ToDate = ToYearlyDate(Policy, FromDate, IsPaymentExist);
                    break;
                case "OneTime":
                    FromDate = FromOneTime(Policy);
                    ToDate = null;//ToOneTime(PolicyId);          
                    break;
                case "Random":
                    FromDate = null;
                    ToDate = null;
                    break;

            }

            FollowUpDate followupdate = new FollowUpDate(FromDate, ToDate, _PaymentType);
            return followupdate;
        }

        private static int GetRangValue(PaymentMode _PaymentType)
        {
            int intVAlue = 0;
            switch (_PaymentType.ToString())
            {
                case "Monthly":
                    intVAlue = 1;
                    break;
                case "Quarterly":
                    intVAlue = 3;
                    break;
                case "HalfYearly":
                    intVAlue = 6;
                    break;
                case "Yearly":
                    intVAlue = 12;
                    break;
                case "OneTime":
                    intVAlue = 1;
                    break;
                case "Random":
                    intVAlue = 0;
                    break;
            }

            return intVAlue;
        }

        private static int? GetRangeOFDate(PaymentMode paymentMode, DateTime LieDate, List<DateRange> _daterange)
        {
            DateRange drrr = _daterange.Where(p => p.STARTDATE.Value <= LieDate).Where(p => p.ENDDATE.Value >= LieDate).FirstOrDefault();
            if (drrr == null)
            {
                return null;
            }
            return drrr.RANGE;

        }

        public static DateTime? LastDate(DateTime? dt)
        {
            if (dt == null) return dt;
            return dt.Value.AddMonths(1).AddDays(-dt.Value.Day);
        }

        public static DateTime? FirstDate(DateTime? dt)
        {
            if (dt == null) return dt;
            return new DateTime(dt.Value.Year, dt.Value.Month, 1);
        }

        private static DateTime? FirstDateOfRange(int range, List<DateRange> _daterange)
        {
            List<DateRange> _finalrng = _daterange.Where(p => p.RANGE == range).ToList<DateRange>();

            return _finalrng[0].STARTDATE;

        }

        private static DateTime? LastDateOfRange(int range, List<DateRange> _daterange)
        {
            List<DateRange> _finalrng = _daterange.Where(p => p.RANGE == range).ToList<DateRange>();
            if (_finalrng.Count == 0) return null;
            return _finalrng[_finalrng.Count - 1].ENDDATE;
        }


        public static void RegisterIssueAgainstPMCVariance(Guid policyID, DateTime? FromDate, DateTime? ToDate, PolicyPaymentEntriesPost paymentEntry)
        {
            //     PolicyDetailsData _Policy = PostUtill.GetPolicy(PolicyPaymentEntriesPost.PolicyID);
            //  if (_Policy.IsTrackIncomingPercentage)
            {
                DisplayFollowupIssue _FollowUpIssue = new DisplayFollowupIssue();
                _FollowUpIssue.IssueId = Guid.NewGuid();
                _FollowUpIssue.IssueStatusId = (int)FollowUpIssueStatus.Open;
                _FollowUpIssue.IssueCategoryID = (int)FollowUpIssueCategory.VarSchedule;
                _FollowUpIssue.IssueResultId = (int)FollowUpResult.Pending;
                _FollowUpIssue.IssueReasonId = (int)FollowUpIssueReason.Pending;
                _FollowUpIssue.InvoiceDate = FromDate;
                // _FollowUpIssue.PolicyPaymentEntryId = PolicyPaymentEntriesPost.PaymentEntryID;
                _FollowUpIssue.FromDate = FromDate;
                _FollowUpIssue.ToDate = ToDate;
                _FollowUpIssue.PolicyId = policyID;
                _FollowUpIssue.PreviousStatusId = (int)FollowUpIssueStatus.Open;

                FollowupIssue.AddUpdate(_FollowUpIssue);

                //   PolicyPaymentEntriesPost.UpdateExpectedPayment(PolicyPaymentEntriesPost.Expectedpayment, PaymentEntryID);
                PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(paymentEntry.PaymentEntryID, _FollowUpIssue.IssueId);
            }
        }



        /// <summary>
        /// Register issue Against Missing Payment
        /// </summary>
        /// <param name="PolicyId"></param>
        public static void RegisterIssueAgainstMissingPayment(PolicyDetailsData _Policy, FollowUpIssueCategory _FollowUpIssueCategory, DateTime? Fromdate, DateTime? Todate)
        {
            try
            {
                if (_Policy.IsTrackMissingMonth)
                {
                    DisplayFollowupIssue _FollowUpIssue = new DisplayFollowupIssue();

                    _FollowUpIssue.IssueId = Guid.NewGuid();
                    _FollowUpIssue.IssueStatusId = (int)FollowUpIssueStatus.Open;
                    _FollowUpIssue.IssueCategoryID = (int)_FollowUpIssueCategory;
                    _FollowUpIssue.IssueResultId = (int)FollowUpResult.Pending;
                    _FollowUpIssue.IssueReasonId = (int)FollowUpIssueReason.Pending;
                    _FollowUpIssue.InvoiceDate = Fromdate;
                    _FollowUpIssue.PolicyId = _Policy.PolicyId;
                    _FollowUpIssue.PreviousStatusId = (int)FollowUpIssueStatus.Open;
                    _FollowUpIssue.FromDate = Fromdate;
                    _FollowUpIssue.ToDate = Todate;
                    FollowupIssue.AddUpdate(_FollowUpIssue);
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("RegisterIssueAgainstMissingPayment :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("RegisterIssueAgainstMissingPayment :" + ex.InnerException.ToString(), true);
            }


        }

        //Need to pass policy data ,not to policy ID
        public static void UpdateTheAutoTermDateOfLearned(Guid PolicyId, DateTime? CalTermDate)
        {
            //Policy _Policy = PostUtill.GetPolicy(PolicyId);
            //DateTime? oldestInvoiceDate = Convert.ToDateTime(PostUtill.GetOldestInvoiceDate(PolicyId));

            //if (_Policy.PolicyTerminationDate != null)
            //{
            //    CalTermDate = PostUtill.GetGreaterDate(_Policy.PolicyTerminationDate.Value, CalTermDate.Value);
            //}
            //else
            //{
            //    if (oldestInvoiceDate != null)
            //    {
            //        CalTermDate = oldestInvoiceDate;
            //    }
            //}
            //PolicyLearnedField _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            //_PolicyLearnedField.AutoTerminationDate = CalTermDate;
            //PolicyLearnedField.AddUpdateLearned(_PolicyLearnedField);
            //IsAutoTrmDateUpadte = false;

            try
            {

                PolicyDetailsData _Policy = PostUtill.GetPolicy(PolicyId);

                if (_Policy.PolicyTerminationDate != null)
                {
                    //check greater invoice date 
                    DateTime? oldestInvoiceDate = PostUtill.GetGreaterInvoiceDate(PolicyId);
                    //compare with PolicyTerminationDate and CalTermDate
                    CalTermDate = PostUtill.GetGreaterDate(_Policy.PolicyTerminationDate.Value, CalTermDate.Value);

                    if (oldestInvoiceDate != null)
                    {
                        //compare with oldest invoice date
                        if (Convert.ToDateTime(oldestInvoiceDate.Value) >= Convert.ToDateTime(CalTermDate.Value))
                            CalTermDate = Convert.ToDateTime(oldestInvoiceDate.Value).AddMonths(1);

                    }
                }
                PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
                _PolicyLearnedField.AutoTerminationDate = CalTermDate;
                PolicyLearnedField.AddUpdateLearned(_PolicyLearnedField, _PolicyLearnedField.ProductType);
                IsAutoTrmDateUpadte = false;
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("UpdateTheAutoTermDateOfLearned :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("UpdateTheAutoTermDateOfLearned :" + ex.InnerException.ToString(), true);
            }

        }
        /// <summary>
        /// Policy Terminate Process
        /// </summary>
        /// <param name="_paymentmode"></param>
        /// <param name="MissingPaymentMonth"></param>
        /// <param name="PolicyId"></param>
        /// <param name="NoOfMissingMonth"></param>
        /// <param name="PaymentRecivedInvoiceDateOneTime"></param>
        public static void AutoPolicyTerminateProcess(PaymentMode _paymentmode, DateTime? MissingPaymentMonth, Guid PolicyId, int? NoOfMissingMonth, DateTime? PaymentRecivedInvoiceDateOneTime)
        {
            try
            {
                int? missPayemt = NoOfMissingMonth;
                DateTime? CalTermDate = null;

                DateTime? InvoiceDate = PaymentRecivedInvoiceDateOneTime;

                switch (_paymentmode)
                {
                    case PaymentMode.HalfYearly:
                    case PaymentMode.Yearly:

                        if (missPayemt == 1)
                        {
                            ////check greater invoice date 
                            DateTime? oldestInvoiceDate = PostUtill.GetGreaterInvoiceDate(PolicyId);

                            if (oldestInvoiceDate != null)
                            {
                                if (Convert.ToDateTime(MissingPaymentMonth.Value) > Convert.ToDateTime(oldestInvoiceDate))
                                {
                                    CalTermDate = MissingPaymentMonth;
                                }
                                else
                                {
                                    //need to change if eric confirmed
                                    if (PaymentMode.HalfYearly == _paymentmode)
                                        CalTermDate = Convert.ToDateTime(oldestInvoiceDate);
                                    else
                                        CalTermDate = Convert.ToDateTime(oldestInvoiceDate);
                                }

                                //CalTermDate = Convert.ToDateTime(oldestInvoiceDate).AddMonths(1);
                                //Terminate the policy--Done
                                UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);
                            }
                            else
                            {
                                CalTermDate = MissingPaymentMonth;
                                //Terminate the policy--Done
                                UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);
                            }

                        }
                        break;
                    case PaymentMode.Monthly:

                        if (missPayemt == 3)
                        {
                            ////check greater invoice date 
                            DateTime? oldestInvoiceDate = PostUtill.GetGreaterInvoiceDate(PolicyId);

                            if (oldestInvoiceDate != null)
                            {
                                //compare with get calculated term date and oldest invoice date
                                CalTermDate = Convert.ToDateTime(oldestInvoiceDate).AddMonths(1);
                                //Terminate the policy--Done
                                UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);
                            }
                            else
                            {
                                CalTermDate = MissingPaymentMonth.Value.AddMonths(-2);
                                //Terminate the policy
                                UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);
                            }

                            //CalTermDate = MissingPaymentMonth.Value.AddMonths(-2);
                            ////Terminate the policy
                            //UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);


                        }
                        break;
                    case PaymentMode.Quarterly:

                        if (missPayemt == 2)
                        {
                            ////check greater invoice date 
                            DateTime? oldestInvoiceDate = PostUtill.GetGreaterInvoiceDate(PolicyId);

                            if (oldestInvoiceDate != null)
                            {
                                //compare with get calculated term date and oldest invoice date
                                if (Convert.ToDateTime(MissingPaymentMonth.Value) > Convert.ToDateTime(oldestInvoiceDate))
                                {
                                    CalTermDate = MissingPaymentMonth.Value.AddMonths(-3);
                                }
                                else
                                {
                                    CalTermDate = Convert.ToDateTime(oldestInvoiceDate).AddMonths(1);
                                }
                                //Terminate the policy--Done
                                UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);
                            }
                            else
                            {
                                CalTermDate = MissingPaymentMonth.Value.AddMonths(-3);
                                //Terminate the policy
                                UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);
                            }

                            //CalTermDate = MissingPaymentMonth.Value.AddMonths(-3);
                            ////Terminate the policy
                            //UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);

                        }
                        break;
                    case PaymentMode.OneTime:
                        //if (PaymentRecivedInvoiceDateOneTime == null) return;
                        PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
                        DateTime? EffDate = _PolicyLearnedField.Effective;
                        if (EffDate != null)
                        {
                            CalTermDate = _PolicyLearnedField.Effective.Value.AddYears(1);
                        }
                        else
                        {
                            if (PaymentRecivedInvoiceDateOneTime != null)
                            {
                                CalTermDate = PaymentRecivedInvoiceDateOneTime.Value.AddDays(1);
                            }

                        }
                        UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);
                        break;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AutoPolicyTerminateProcess :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("AutoPolicyTerminateProcess :" + ex.InnerException.ToString(), true);
            }

        }

        public static void RegisterIssueAgainstScheduleVariance(PolicyPaymentEntriesPost PolicyPaymentEntriesPost)
        {
            try
            {
                PolicyDetailsData _Policy = PostUtill.GetPolicy(PolicyPaymentEntriesPost.PolicyID);
                if (_Policy.IsTrackIncomingPercentage)
                {

                    DisplayFollowupIssue _FollowUpIssue = new DisplayFollowupIssue();
                    _FollowUpIssue.IssueId = Guid.NewGuid();
                    _FollowUpIssue.IssueStatusId = (int)FollowUpIssueStatus.Open;
                    _FollowUpIssue.IssueCategoryID = (int)FollowUpIssueCategory.VarSchedule;
                    _FollowUpIssue.IssueResultId = (int)FollowUpResult.Pending;
                    _FollowUpIssue.IssueReasonId = (int)FollowUpIssueReason.Pending;
                    _FollowUpIssue.InvoiceDate = PolicyPaymentEntriesPost.InvoiceDate;
                    // _FollowUpIssue.PolicyPaymentEntryId = PolicyPaymentEntriesPost.PaymentEntryID;

                    _FollowUpIssue.PolicyId = PolicyPaymentEntriesPost.PolicyID;
                    _FollowUpIssue.PreviousStatusId = (int)FollowUpIssueStatus.Open;

                    FollowupIssue.AddUpdate(_FollowUpIssue);

                    PolicyPaymentEntriesPost.UpdateExpectedPayment(PolicyPaymentEntriesPost.Expectedpayment, PolicyPaymentEntriesPost.PaymentEntryID);
                    PolicyPaymentEntriesPost.UpdateVarPaymentIssueId(PolicyPaymentEntriesPost.PaymentEntryID, _FollowUpIssue.IssueId);
                    //FollowupIssue.UpdateExpectedPayment(FollowupIssue.Expectedpayment, _FollowUpIssue.IssueId);
                }
                else
                {
                    //delete issue when IsTrackIncomingPercentage==false
                    //FollowupIssue.DeleteIssue(FollowupIssueId);
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("RegisterIssueAgainstScheduleVariance :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("RegisterIssueAgainstScheduleVariance :" + ex.InnerException.ToString(), true);
            }

        }


        #region MonthlyMode

        private static DateTime? FromMonthlyDate(PolicyDetailsData Policy)
        {
            PolicyLearnedFieldData _po = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(Policy.PolicyId); //PolicyToLearnPost.GetPolicyLearnedFieldsPolicyWise1(PolicyId);
            DateTime? _dtTFD = _po.TrackFrom;
            //DateTime? _dtEffdt = _po.Effective;
            //DateTime? resDate = PostUtill.GetGreaterDate(_dtTFD, _dtEffdt);
            //return FirstDate(resDate);
            return FirstDate(_dtTFD);

        }

        /*
         * if									
         * user term date is avilable 									
         * use lesser between 		Minus the 93 days from today date. (Today-93)						
         * and	day perior the user trm date			and adjust it		
         * 
         * else									
         * If there is issue with closed with reason term date then								
         * minimum(policy term closed Issue invoice date) -1 day wil be To Date for the range							
         * else								
         * We generate maximum 3 open missing issues from FROM date only.							
         * end if								
         * end if									
         * 
         * 
         * the default TO date is always ---TO date would just be today - 93									

         */
        private static DateTime? ToMOnthlyDate(PolicyDetailsData _Policy, DateTime? FromDate)
        {
            //PolicyDetailsData _Policy = PostUtill.GetPolicy(PolicyId);
            //PolicyLearnedField _po = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            DateTime? ToDay = DateTime.Today;
            try
            {
                //DateTime TempDate = ToDay.Value.AddDays(-93);
                //New update after discuss with kevin & eric on 12062014
                DateTime TempDate = ToDay.Value.AddDays(-63);
                // DateTime? AdjustTD = null;
                if (_Policy.PolicyTerminationDate != null)
                {
                    //check greater invoice date 
                    DateTime? oldestInvoiceDate = PostUtill.GetGreaterInvoiceDate(_Policy.PolicyId);
                    if (oldestInvoiceDate != null)
                    {
                        ToDay = PostUtill.DateComparer(_Policy.PolicyTerminationDate, TempDate);
                        ToDay = LastDate(ToDay.Value.AddMonths(-1));
                        if (oldestInvoiceDate > _Policy.PolicyTerminationDate)
                        {
                            ToDay = oldestInvoiceDate;
                        }
                    }
                    else
                    {
                        ToDay = PostUtill.DateComparer(_Policy.PolicyTerminationDate, TempDate);
                        ToDay = LastDate(ToDay.Value.AddMonths(-1));
                    }
                    //before
                    //ToDay = PostUtill.DateComparer(_Policy.PolicyTerminationDate, TempDate);
                    //ToDay = LastDate(ToDay.Value.AddMonths(-1));
                }
                else
                {
                    List<DisplayFollowupIssue> _FollowupIssueLst = FollowupIssue.GetIssues(_Policy.PolicyId)
                                                                .Where(p => (p.IssueStatusId == (int)FollowUpIssueStatus.Closed && p.IssueReasonId == (int)FollowUpIssueReason.PolicyTerm)).ToList();
                    if (_FollowupIssueLst != null && _FollowupIssueLst.Count > 0)
                    {
                        DateTime? mindate = _FollowupIssueLst.Min(p => p.InvoiceDate);
                        mindate = mindate.Value.AddDays(-1);
                        ToDay = mindate;
                    }
                    else
                    {
                        ToDay = TempDate;
                        ToDay = LastDate(ToDay.Value.AddMonths(-1));
                        //ToDay = FromDate.Value.AddMonths(3);
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("ToMOnthlyDate :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("ToMOnthlyDate :" + ex.InnerException.ToString(), true);
            }

            return ToDay;
        }
        #endregion
        /// <summary>
        /// It Taste IsPayment is recived for a ParticularPolicy
        /// </summary>
        /// <param name="PolicyId">Paas the Poilcy ID</param>
        /// <returns>true if policy get otherwise false</returns>
        private static bool DoPaymentRecived(Guid PolicyId)
        {
            return PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId).Count > 0 ? true : false;
        }

        #region Onetime
        private static DateTime? FromOneTime(PolicyDetailsData Policy)
        {
            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(Policy.PolicyId);
            DateTime? _EffDt = _pld.Effective;
            DateTime? _TFDt = _pld.TrackFrom;
            return FirstDate(_TFDt);//Check for this for default
        }

        private static DateTime? ToOneTime(Guid PolicyId)
        {
           
            DateTime? ToDay = DateTime.Today;
            try
            {
                //DateTime TempDate = ToDay.Value.AddDays(-93);
                PolicyDetailsData _Policy = PostUtill.GetPolicy(PolicyId);
                PolicyLearnedFieldData _po = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
                DateTime TempDate = ToDay.Value.AddDays(-63);
                // DateTime? AdjustTD = null;
                if (_Policy.PolicyTerminationDate != null)
                {
                    ToDay = PostUtill.DateComparer(_Policy.PolicyTerminationDate, TempDate);
                    ToDay = LastDate(ToDay.Value.AddMonths(-1));
                }
                else
                {
                    ToDay = TempDate;
                    ToDay = LastDate(ToDay.Value.AddMonths(-1));
                    //ToDay = FromDate.Value.AddMonths(3);
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("ToOneTime :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("ToOneTime :" + ex.InnerException.ToString(), true);
            }

            return ToDay;
        }
        #endregion

        #region QuaterlyMode
        private static DateTime? FromQuaterlyDate(PolicyDetailsData Policy, bool isPaymentExist)
        {
            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(Policy.PolicyId);
            DateTime? _EffDt = _pld.Effective;
            DateTime? _TFDt = _pld.TrackFrom;
            DateGroup DG;
            List<DateRange> _daterange;
            //bool IsPaymentRecived;
            //IsPaymentRecived = DoPaymentRecived(Policy.PolicyId);
            if (_EffDt != null)
            {
                _EffDt = FirstDate(_EffDt);


                _TFDt = FirstDate(_TFDt);
                DateTime? lesserDate = null;
                if (_EffDt <= _TFDt)
                {
                    lesserDate = _EffDt;
                }
                else
                {
                    lesserDate = _TFDt;
                }

                DG = new DateGroup(lesserDate, PaymentMode.Quarterly);
                _daterange = DG.GetAllDateRange();


                int? rngnum = GetRangeOFDate(PaymentMode.Quarterly, _TFDt.Value, _daterange);
                if (rngnum != null)
                {
                    _TFDt = FirstDateOfRange(rngnum.Value, _daterange);
                }
                else
                {
                    return null;
                }
                return _TFDt;
            }

            else if (_EffDt == null && isPaymentExist)
            {
                DateTime? OID = PostUtill.GetOldestInvoiceDate(Policy.PolicyId);
                OID = FirstDate((DateTime)OID);
                DG = new DateGroup((DateTime)OID, PaymentMode.Quarterly);
                _daterange = DG.GetAllDateRange();
                _TFDt = FirstDate(_TFDt);
                int? rngnum = GetRangeOFDate(PaymentMode.Quarterly, _TFDt.Value, _daterange);
                if (rngnum == null)
                {
                    return _TFDt;
                }
                DateTime? _rngFDt = FirstDateOfRange(rngnum.Value, _daterange);

                #region vinod will check for remove
                if (_TFDt < _rngFDt)
                {
                    return _rngFDt;
                }
                else
                {
                    return _TFDt;
                }
                #endregion

            }
            else if (_EffDt == null && !isPaymentExist)
            {

                return FirstDate(_TFDt);
            }
            return FirstDate(_TFDt);//Check for this for default 
        }

        private static DateTime? ToQuaterlyDate(PolicyDetailsData Policy, DateTime? Fromdate, bool isPaymentExist)
        {
            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(Policy.PolicyId);
            DateTime? _EffDt = _pld.Effective;
            DateTime? _TFDt = _pld.TrackFrom;
            DateGroup DG;
            //bool IsPaymentRecived;
            //IsPaymentRecived = DoPaymentRecived(Policy.PolicyId);
            List<DateRange> _daterange = null;
            if (_EffDt != null)
            {
                _EffDt = FirstDate(_EffDt);
                _TFDt = FirstDate(_TFDt);
                DateTime? lesserDate = null;
                if (_EffDt <= _TFDt)
                {
                    lesserDate = _EffDt;
                }
                else
                {
                    lesserDate = _TFDt;
                }

                DG = new DateGroup(lesserDate, PaymentMode.Quarterly);
                _daterange = DG.GetAllDateRange();
            }
            else if (_EffDt == null && isPaymentExist)
            {
                DateTime? OID = PostUtill.GetOldestInvoiceDate(Policy.PolicyId);
                OID = FirstDate((DateTime)OID);
                DG = new DateGroup((DateTime)OID, PaymentMode.Quarterly);
                _daterange = DG.GetAllDateRange();
            }
            else if (_EffDt == null && !isPaymentExist)
            {
                DG = new DateGroup(_TFDt, PaymentMode.Quarterly);
                _daterange = DG.GetAllDateRange();
                //  return _TFDt = FirstDate(_TFDt);
            }

            DateTime? _ToDt = ToMOnthlyDate(Policy, Fromdate);
            int? rngnum = GetRangeOFDate(PaymentMode.Quarterly, _ToDt.Value, _daterange);
            if (rngnum == null) return null;
            if (LastDateOfRange(rngnum.Value, _daterange) == _ToDt)
            {

                return _ToDt;
            }
            else
            {
                rngnum = rngnum - 1;
                return LastDateOfRange(rngnum.Value, _daterange);
            }
        }
        #endregion

        #region HalfYearlyMode
        private static DateTime? FromHalfYearlyDate(Guid PolicyId, bool isPaymentExist)
        {
            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            DateTime? _EffDt = _pld.Effective;
            DateTime? _TFDt = _pld.TrackFrom;
            DateGroup DG;
            List<DateRange> _daterange;
            //bool IsPaymentRecived;
            //IsPaymentRecived = DoPaymentRecived(PolicyId);
            if (_EffDt != null)
            {
                _EffDt = FirstDate(_EffDt);


                _TFDt = FirstDate(_TFDt);
                DateTime? lesserDate = null;
                if (_EffDt <= _TFDt)
                {
                    lesserDate = _EffDt;
                }
                else
                {
                    lesserDate = _TFDt;
                }
                DG = new DateGroup(lesserDate, PaymentMode.HalfYearly);
                _daterange = DG.GetAllDateRange();

                int? rngnum = GetRangeOFDate(PaymentMode.HalfYearly, _TFDt.Value, _daterange);
                if (rngnum != null)
                {
                    _TFDt = FirstDateOfRange(rngnum.Value, _daterange);
                }
                else
                {
                    return null;
                }

                return _TFDt;
            }

            else if (_EffDt == null && isPaymentExist)
            {
                DateTime? OID = PostUtill.GetOldestInvoiceDate(PolicyId);
                OID = FirstDate((DateTime)OID);
                DG = new DateGroup((DateTime)OID, PaymentMode.HalfYearly);
                _daterange = DG.GetAllDateRange();
                _TFDt = FirstDate(_TFDt);
                int? rngnum = GetRangeOFDate(PaymentMode.HalfYearly, _TFDt.Value, _daterange);
                if (rngnum == null)
                {
                    return _TFDt;
                }
                DateTime? _rngFDt = FirstDateOfRange(rngnum.Value, _daterange);
                if (_TFDt < _rngFDt)
                {
                    return _rngFDt;
                }
                else
                {
                    return _TFDt;
                }
            }
            else if (_EffDt == null && !isPaymentExist)
            {

                return FirstDate(_TFDt);
            }
            return FirstDate(_TFDt);//Check for this for default
        }

        private static DateTime? ToHalfYearlyDate(PolicyDetailsData Policy, DateTime? Fromdate, bool isPaymentExist)
        {
            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(Policy.PolicyId);
            DateTime? _EffDt = _pld.Effective;
            DateTime? _TFDt = _pld.TrackFrom;
            DateGroup DG;
            //bool IsPaymentRecived;
            //IsPaymentRecived = DoPaymentRecived(Policy.PolicyId);
            List<DateRange> _daterange = null;
            if (_EffDt != null)
            {
                _EffDt = FirstDate(_EffDt);
                _TFDt = FirstDate(_TFDt);
                DateTime? lesserDate = null;
                if (_EffDt <= _TFDt)
                {
                    lesserDate = _EffDt;
                }
                else
                {
                    lesserDate = _TFDt;
                }
                DG = new DateGroup(lesserDate, PaymentMode.HalfYearly);
                _daterange = DG.GetAllDateRange();
            }
            else if (_EffDt == null && isPaymentExist)
            {
                DateTime? OID = PostUtill.GetOldestInvoiceDate(Policy.PolicyId);
                OID = FirstDate((DateTime)OID);
                DG = new DateGroup((DateTime)OID, PaymentMode.HalfYearly);
                _daterange = DG.GetAllDateRange();
            }
            else if (_EffDt == null && !isPaymentExist)
            {
                DG = new DateGroup(_TFDt, PaymentMode.HalfYearly);
                _daterange = DG.GetAllDateRange();
                //  return _TFDt = FirstDate(_TFDt);
            }

            DateTime? _ToDt = ToMOnthlyDate(Policy, Fromdate);
            int? rngnum = GetRangeOFDate(PaymentMode.HalfYearly, _ToDt.Value, _daterange);
            if (rngnum == null) return null;
            if (LastDateOfRange(rngnum.Value, _daterange) == _ToDt)
            {

                return _ToDt;
            }
            else
            {
                rngnum = rngnum - 1;
                return LastDateOfRange(rngnum.Value, _daterange);
            }
        }
        #endregion

        #region YearlyMode

        private static DateTime? FromYearlyDate(Guid PolicyId, bool isPaymentExist)
        {
            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            DateTime? _EffDt = _pld.Effective;
            DateTime? _TFDt = _pld.TrackFrom;
            DateGroup DG;
            List<DateRange> _daterange;
            //bool IsPaymentRecived;
            //IsPaymentRecived = DoPaymentRecived(PolicyId);
            if (_EffDt != null)
            {
                _EffDt = FirstDate(_EffDt);

                _TFDt = FirstDate(_TFDt);
                DateTime? lesserDate = null;
                if (_EffDt <= _TFDt)
                {
                    lesserDate = _EffDt;
                }
                else
                {
                    lesserDate = _TFDt;
                }
                //DG = new DateGroup(_EffDt, PaymentMode.Yearly);
                DG = new DateGroup(lesserDate, PaymentMode.Yearly);
                _daterange = DG.GetAllDateRange();

                //int rngnum = GetRangeOFDate(PaymentMode.Yearly, _TFDt.Value, _daterange);
                int? rngnum = GetRangeOFDate(PaymentMode.Yearly, _TFDt.Value, _daterange);
                if (rngnum != null)
                {
                    _TFDt = FirstDateOfRange(rngnum.Value, _daterange);
                }
                else
                {
                    return null;
                }

                return _TFDt;
            }

            else if (_EffDt == null && isPaymentExist)
            {
                DateTime? OID = PostUtill.GetOldestInvoiceDate(PolicyId);
                OID = FirstDate((DateTime)OID);
                DG = new DateGroup((DateTime)OID, PaymentMode.Yearly);
                _daterange = DG.GetAllDateRange();
                _TFDt = FirstDate(_TFDt);
                int? rngnum = GetRangeOFDate(PaymentMode.Yearly, _TFDt.Value, _daterange);
                if (rngnum == null)
                {
                    return _TFDt;
                }
                DateTime? _rngFDt = FirstDateOfRange(rngnum.Value, _daterange);

                if (_TFDt < _rngFDt)
                {
                    return _rngFDt;
                }
                else
                {
                    return _TFDt;
                }
            }
            else if (_EffDt == null && !isPaymentExist)
            {

                return FirstDate(_TFDt);
            }
            return FirstDate(_TFDt);//Check for this for default
        }

        private static DateTime? ToYearlyDate(PolicyDetailsData Policy, DateTime? FromDate, bool isPaymentExist)
        {
            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(Policy.PolicyId);
            DateTime? _EffDt = _pld.Effective;
            DateTime? _TFDt = _pld.TrackFrom;
            DateGroup DG;
            //bool IsPaymentRecived;
            //IsPaymentRecived = DoPaymentRecived(Policy.PolicyId);
            List<DateRange> _daterange = null;
            if (_EffDt != null)
            {
                _EffDt = FirstDate(_EffDt);
                _TFDt = FirstDate(_TFDt);
                DateTime? lesserDate = null;
                if (_EffDt <= _TFDt)
                {
                    lesserDate = _EffDt;
                }
                else
                {
                    lesserDate = _TFDt;
                }
                DG = new DateGroup(lesserDate, PaymentMode.Yearly);
                _daterange = DG.GetAllDateRange();
            }
            else if (_EffDt == null && isPaymentExist)
            {
                DateTime? OID = PostUtill.GetOldestInvoiceDate(Policy.PolicyId);
                OID = FirstDate((DateTime)OID);
                DG = new DateGroup((DateTime)OID, PaymentMode.Yearly);
                _daterange = DG.GetAllDateRange();
            }
            else if (_EffDt == null && !isPaymentExist)
            {
                DG = new DateGroup(_TFDt, PaymentMode.Yearly);
                _daterange = DG.GetAllDateRange();
                //  return _TFDt = FirstDate(_TFDt);
            }

            DateTime? _ToDt = ToMOnthlyDate(Policy, FromDate);
            int? rngnum = GetRangeOFDate(PaymentMode.Yearly, _ToDt.Value, _daterange);
            if (rngnum == null) return null;
            if (LastDateOfRange(rngnum.Value, _daterange) == _ToDt)
            {

                return _ToDt;
            }
            else
            {
                rngnum = rngnum - 1;
                // rngnum = 1;
                return LastDateOfRange(rngnum.Value, _daterange);
            }
        }

        #endregion

        public static void OpenMissisngIssueIfAny(DateTime? invoicedate, Guid PolicyId)
        {
            try
            {

                List<DisplayFollowupIssue> _FollowupIssueLst = FollowupIssue.GetIssues(PolicyId);
                if (_FollowupIssueLst != null && _FollowupIssueLst.Count != 0)
                {
                    _FollowupIssueLst = _FollowupIssueLst.Where(p => (p.FromDate <= invoicedate && p.ToDate >= invoicedate)).ToList();
                    if (_FollowupIssueLst != null && _FollowupIssueLst.Count != 0)
                    {
                        DisplayFollowupIssue follo = _FollowupIssueLst.FirstOrDefault();
                        if (follo.IssueStatusId == (int)FollowUpIssueStatus.Closed)
                        {
                            follo.IssueStatusId = (int)FollowUpIssueStatus.Open;
                            follo.IssueResultId = (int)FollowUpResult.Pending;
                            FollowupIssue.AddUpdate(follo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("OpenMissisngIssueIfAny :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("OpenMissisngIssueIfAny :" + ex.InnerException.ToString(), true);
            }

        }
    }

    public class DateGroup
    {
        DateTime? DATE;
        PaymentMode paymode;
        public DateGroup(DateTime? dt, PaymentMode paymentmode)
        {
            DATE = dt;
            paymode = paymentmode;
        }
        List<DateRange> _DtRngs = new List<DateRange>();
        List<DateGroup> _DtGrp = new List<DateGroup>();

        public List<DateRange> GetAllDateRange()
        {
            DateTime? strdt;
            DateTime? enddt;
            int rng = 0;
            int temp = 0;
            if (paymode == PaymentMode.Quarterly)
            {
                DateTime? strdt1 = FollowUpUtill.FirstDate(DATE);

                for (DateTime dt = strdt1.Value; dt <= FollowUpUtill.LastDate(DateTime.Today); dt = dt.AddMonths(1))
                {
                    if (temp % (int)paymode == 0)
                    {
                        rng++;
                    }
                    temp++;
                    strdt = dt;
                    enddt = FollowUpUtill.LastDate(dt);
                    _DtRngs.Add(new DateRange(strdt, enddt, rng));
                }
            }
            else if (paymode == PaymentMode.HalfYearly)
            {
                DateTime? strdt1 = FollowUpUtill.FirstDate(DATE);

                for (DateTime dt = strdt1.Value; dt <= FollowUpUtill.LastDate(DateTime.Today); dt = dt.AddMonths(1))
                {
                    if (temp % (int)paymode == 0)
                    {
                        rng++;
                    }

                    temp++;
                    strdt = dt;
                    enddt = FollowUpUtill.LastDate(dt);

                    _DtRngs.Add(new DateRange(strdt, enddt, rng));

                }

            }

            else if (paymode == PaymentMode.Yearly)
            {
                DateTime? strdt1 = FollowUpUtill.FirstDate(DATE);

                for (DateTime dt = strdt1.Value; dt <= FollowUpUtill.LastDate(DateTime.Today); dt = dt.AddMonths(1))
                {
                    if (temp % (int)paymode == 0)
                    {
                        rng++;
                    }
                    temp++;
                    strdt = dt;
                    enddt = FollowUpUtill.LastDate(dt);

                    _DtRngs.Add(new DateRange(strdt, enddt, rng));
                }

            }
            else if (paymode == PaymentMode.OneTime)
            {
                DateTime? strdt1 = FollowUpUtill.FirstDate(DATE);
                strdt = strdt1.Value;
                enddt = FollowUpUtill.LastDate(DateTime.Today);
                _DtRngs.Add(new DateRange(strdt, enddt, 1));
            }
            return _DtRngs;
        }
    }
    public class DateRange
    {
        public DateTime? STARTDATE;
        public DateTime? ENDDATE;
        public int RANGE;
        public DateRange(DateTime? sdate, DateTime? edate, int range)
        {
            STARTDATE = sdate;
            ENDDATE = edate;
            RANGE = range;
        }
    }
    public class FollowUpDate
    {
        public DateTime? FromDate;
        public DateTime? ToDate;
        public PaymentMode Paymentmode;
        public FollowUpDate(DateTime? Frmdt, DateTime? Todt, PaymentMode Paymode)
        {
            FromDate = Frmdt;
            ToDate = Todt;
            Paymentmode = Paymode;
        }

    }

}
