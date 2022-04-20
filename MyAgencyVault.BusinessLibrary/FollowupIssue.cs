using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using MyAgencyVault.EmailFax;
using System.Linq.Expressions;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Data;
namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class DisplayFollowupIssue
    {
        #region "Datamembers aka - public properties"
        [DataMember]
        public Guid IssueId { get; set; }
        [DataMember]
        public int? IssueStatusId { get; set; }
        [DataMember]
        public int? IssueCategoryID { get; set; }
        [DataMember]
        public int? IssueResultId { get; set; }
        [DataMember]
        public int? IssueReasonId { get; set; }

        [DataMember]
        public bool? IsTrackMissingMonth { get; set; }

        [DataMember]
        public bool? IsTrackIncomingPercentage { get; set; }

        [DataMember]
        public bool? IsTrackPayment { get; set; }

        [DataMember]
        public string Client { get; set; }

        [DataMember]
        public DateTime? InvoiceDate { get; set; }
        [DataMember]
        public DateTime? NextFollowupDate { get; set; }
        [DataMember]
        public string PolicyIssueNote { get; set; }
        [DataMember]
        public decimal? Payment { get; set; }
        [DataMember]
        public Guid? PolicyId { get; set; }
        [DataMember]
        public int? PreviousStatusId { get; set; }
        [DataMember]
        public DateTime? LastModifiedDate { get; set; }
        [DataMember]
        public int IssueTrackNumber { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public Guid? ModifiedBy { get; set; }
        [DataMember]
        public DateTime? FromDate { get; set; }
        [DataMember]
        public DateTime? ToDate { get; set; }
        [DataMember]
        public string Payor { get; set; }
        [DataMember]
        public string Agency { get; set; }



        [DataMember]
        public string Insured { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public Guid? PayorId { get; set; }
        [DataMember]
        public Guid LicenseeId { get; set; }
        [DataMember]
        public int Content { get; set; }
        [DataMember]
        public DateTime? AutoTerminationDate { get; set; }

        [DataMember]
        public bool? isDeleted { get; set; }

        [DataMember]
        public bool? IsPMCVariance { get; set; }
        [DataMember]
        public bool? IsResolvedFromCommDashboard { get; set; }
        [DataMember]
        public bool? IsRemovedFromCommDashboard { get; set; }

        #endregion
    }
    [DataContract]
    public class MasterIssuesOption
    {
        [DataMember]
        public List<IssueCategory> IssueCategories { get; set; }
        [DataMember]
        public List<IssueReasons> IssueReasons { get; set; }
        [DataMember]
        public List<IssueResults> IssueResults { get; set; }
        [DataMember]
        public List<IssueStatus> IssueStatus { get; set; }
    }
    [DataContract]
    public class FollowupIssue : IEditable<DisplayFollowupIssue>
    {
        public static MasterIssuesOption FillMasterIssueOptions()
        {
            MasterIssuesOption _masterIssueOption = new MasterIssuesOption();
            _masterIssueOption.IssueCategories = IssueCategory.GetAllCategory();
            _masterIssueOption.IssueReasons = IssueReasons.GetAllReason();
            _masterIssueOption.IssueResults = IssueResults.GetAllResults();
            _masterIssueOption.IssueStatus = IssueStatus.GetAllStatus();
            return _masterIssueOption;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PolicyPaymentEntriesPost"></param>
        /// <returns></returns>
        public static DisplayFollowupIssue GetFollowupissueOfPayment(PolicyPaymentEntriesPost PolicyPaymentEntriesPost)
        {
            DisplayFollowupIssue _FollowupIssuetemp = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.FollowupIssue _FollowupIssuerd = DataModel.FollowupIssues.Where(p => (p.PolicyId == PolicyPaymentEntriesPost.PolicyID) && (p.FromDate < PolicyPaymentEntriesPost.InvoiceDate && p.ToDate > PolicyPaymentEntriesPost.InvoiceDate)).FirstOrDefault();
                if (_FollowupIssuerd != null)
                {
                    _FollowupIssuetemp = new DisplayFollowupIssue();
                    _FollowupIssuetemp.IssueId = _FollowupIssuerd.IssueId;
                    _FollowupIssuetemp.IssueStatusId = _FollowupIssuerd.IssueStatusId;
                    _FollowupIssuetemp.IssueCategoryID = _FollowupIssuerd.IssueCategoryId;
                    _FollowupIssuetemp.IssueResultId = _FollowupIssuerd.IssueResultId;
                    _FollowupIssuetemp.IssueReasonId = _FollowupIssuerd.IssueReasonId;
                    _FollowupIssuetemp.InvoiceDate = _FollowupIssuerd.InvoiceDate;
                    _FollowupIssuetemp.NextFollowupDate = _FollowupIssuerd.NextFollowUpDate;
                    _FollowupIssuetemp.PolicyIssueNote = _FollowupIssuerd.Notes;
                    _FollowupIssuetemp.Payment = _FollowupIssuerd.Payment;
                    _FollowupIssuetemp.PolicyId = _FollowupIssuerd.PolicyId;
                    _FollowupIssuetemp.Payor = _FollowupIssuerd.Policy.Payor.PayorName;
                    _FollowupIssuetemp.Agency = _FollowupIssuerd.Policy.Licensee.Company;
                    _FollowupIssuetemp.Client = Convert.ToString(_FollowupIssuerd.Policy.Client);
                    _FollowupIssuetemp.Insured = _FollowupIssuerd.Policy.Insured;
                    _FollowupIssuetemp.PolicyNumber = _FollowupIssuerd.Policy.PolicyNumber;
                    _FollowupIssuetemp.PreviousStatusId = _FollowupIssuerd.PreviousStatusId;
                    _FollowupIssuetemp.LastModifiedDate = _FollowupIssuerd.LastModifiedDate;
                    _FollowupIssuetemp.IssueTrackNumber = _FollowupIssuerd.IssueTrackNumber;
                    _FollowupIssuetemp.CreatedDate = _FollowupIssuerd.CreatedDate;
                    _FollowupIssuetemp.ModifiedBy = _FollowupIssuerd.ModifiedBy;
                    _FollowupIssuetemp.FromDate = _FollowupIssuerd.FromDate;
                    _FollowupIssuetemp.ToDate = _FollowupIssuerd.ToDate;
                    _FollowupIssuetemp.PayorId = _FollowupIssuerd.Policy.Payor.PayorId;
                    _FollowupIssuetemp.LicenseeId = _FollowupIssuerd.Policy.Licensee.LicenseeId;
                    _FollowupIssuetemp.Content = 1;
                    _FollowupIssuetemp.isDeleted = _FollowupIssuerd.IsDeleted;
                }
                return _FollowupIssuetemp;
            }
        }
        /// <summary>
        /// Check for varience Issue Exists 
        /// </summary>
        /// <param name="PolicyPaymentEntriesPost"></param>
        public static bool CheckForPaymentVarIssueExists(PolicyPaymentEntriesPost PolicyPaymentEntriesPost)
        {
            bool flag = false;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.FollowupIssue> FollowupIssueLst = DataModel.FollowupIssues
                    .Where(p => ((p.InvoiceDate == PolicyPaymentEntriesPost.InvoiceDate) && (p.PolicyId == PolicyPaymentEntriesPost.PolicyID)))
                    .ToList();
                if (FollowupIssueLst != null && FollowupIssueLst.Count > 1)
                {
                    flag = true;
                }
                return flag;
            }
        }

        /// <summary>
        /// GetIssuesForFollowProcess
        /// </summary>
        /// <param name="policyID"></param>
        /// <returns></returns>

        public static List<DisplayFollowupIssue> GetIssuesForFollowProcess(Guid policyID)
        {
            List<DisplayFollowupIssue> _FollowupIssue = new List<DisplayFollowupIssue>();

            try
            {

                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                string adoConnStr = sc.ConnectionString;

                DateTime? nullDateTime = null;
                int? nullint = null;
                //decimal? Nulldecimal = null;

                using (SqlConnection con = new SqlConnection(adoConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetFollowUPProcMissing", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PolicyId", policyID);

                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        // Call Read before accessing data. 
                        while (reader.Read())
                        {
                            try
                            {
                                DisplayFollowupIssue objPolicyDetailsData = new DisplayFollowupIssue();

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["IssueId"])))
                                    {
                                        objPolicyDetailsData.IssueId = reader["IssueId"] == null ? Guid.Empty : (Guid)reader["IssueId"];
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["IssueCategoryId"])))
                                    {
                                        objPolicyDetailsData.IssueCategoryID = reader["IssueCategoryId"] == null ? nullint : (int)reader["IssueCategoryId"];
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["IssueStatusId"])))
                                    {
                                        objPolicyDetailsData.IssueStatusId = reader["IssueStatusId"] == null ? nullint : (int)reader["IssueStatusId"];
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["IssueResultId"])))
                                    {
                                        objPolicyDetailsData.IssueResultId = reader["IssueResultId"] == null ? nullint : (int)reader["IssueResultId"];
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["IssueReasonId"])))
                                    {
                                        objPolicyDetailsData.IssueReasonId = reader["IssueReasonId"] == null ? nullint : (int)reader["IssueReasonId"];
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["PolicyId"])))
                                    {
                                        objPolicyDetailsData.PolicyId = reader["PolicyId"] == null ? Guid.Empty : (Guid)reader["PolicyId"];
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["PreviousStatusId"])))
                                    {
                                        objPolicyDetailsData.PreviousStatusId = reader["PreviousStatusId"] == null ? nullint : Convert.ToInt32(reader["PreviousStatusId"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["IssueTrackNumber"])))
                                    {
                                        objPolicyDetailsData.IssueTrackNumber = Convert.ToInt32(reader["IssueTrackNumber"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["InvoiceDate"])))
                                    {
                                        objPolicyDetailsData.InvoiceDate = reader["InvoiceDate"] == null ? nullDateTime : Convert.ToDateTime(reader["InvoiceDate"]);
                                    }
                                }
                                catch
                                {
                                }
                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["NextFollowUpDate"])))
                                    {
                                        objPolicyDetailsData.NextFollowupDate = reader["NextFollowUpDate"] == null ? nullDateTime : Convert.ToDateTime(reader["NextFollowUpDate"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["FromDate"])))
                                    {
                                        objPolicyDetailsData.FromDate = reader["FromDate"] == null ? nullDateTime : Convert.ToDateTime(reader["FromDate"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["ToDate"])))
                                    {
                                        objPolicyDetailsData.ToDate = reader["ToDate"] == null ? nullDateTime : Convert.ToDateTime(reader["ToDate"]);
                                    }
                                }
                                catch
                                {
                                }

                                //try
                                //{
                                //    if (!string.IsNullOrEmpty(Convert.ToString(reader["Payment"])))
                                //    {
                                //        objPolicyDetailsData.Payment = reader["Payment"] == null ? Nulldecimal : Convert.ToDecimal(reader["Payment"]);
                                //    }
                                //}
                                //catch
                                //{
                                //}

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["IsDeleted"])))
                                    {
                                        if (reader["IsDeleted"] != null)
                                        {
                                            objPolicyDetailsData.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                                        }
                                    }

                                }
                                catch
                                {
                                }



                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["CreatedDate"])))
                                    {
                                        objPolicyDetailsData.CreatedDate = reader["CreatedDate"] == null ? nullDateTime : Convert.ToDateTime(reader["CreatedDate"]);
                                    }

                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["ModifiedBy"])))
                                    {
                                        objPolicyDetailsData.ModifiedBy = (Guid)(reader["ModifiedBy"]);
                                    }
                                }
                                catch
                                {
                                }

                                _FollowupIssue.Add(objPolicyDetailsData);
                            }
                            catch
                            {
                            }

                        }

                        // Call Close when done reading.
                        reader.Close();
                    }
                }
            }
            catch
            {
            }

            return _FollowupIssue;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyID"></param>
        /// <returns></returns>
        public static List<DisplayFollowupIssue> GetIssues(Guid policyID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var Result = DataModel.GetIssues(policyID);
                List<DisplayFollowupIssue> _FollowupIssue = (from p in Result
                                                             where (p.PolicyId == policyID)
                                                             select new DisplayFollowupIssue
                                                             {
                                                                 IssueId = (Guid?)p.IssueId ?? Guid.Empty,
                                                                 IssueStatusId = p.IssueStatusId.Value,
                                                                 IssueCategoryID = p.IssueCategoryId.Value,
                                                                 IssueResultId = p.IssueResultId.Value,
                                                                 IssueReasonId = p.IssueReasonId.Value,
                                                                 InvoiceDate = p.InvoiceDate,
                                                                 NextFollowupDate = p.NextFollowUpDate,
                                                                 PolicyIssueNote = p.PolicyIssueNote,
                                                                 Payment = p.Payment,
                                                                 PolicyId = p.PolicyId.Value,
                                                                 Payor = p.PayorName,
                                                                 Agency = p.Agency,
                                                                 Insured = p.Insured,
                                                                 PolicyNumber = p.PolicyNumber,
                                                                 PreviousStatusId = p.PreviousStatusId,
                                                                 LastModifiedDate = p.LastModifiedDate,
                                                                 IssueTrackNumber = p.IssueTrackNumber,
                                                                 CreatedDate = p.CreatedDate,
                                                                 ModifiedBy = p.ModifiedBy,
                                                                 FromDate = p.FromDate,
                                                                 ToDate = p.ToDate,
                                                                 PayorId = p.PayorId,
                                                                 LicenseeId = p.LicenseeId,
                                                                 Content = p.Content,
                                                                 IsTrackIncomingPercentage = p.IsTrackIncomingPercentage,
                                                                 IsTrackMissingMonth = p.IsTrackMissingMonth,
                                                                 IsTrackPayment = p.IsTrackPayment,
                                                                 //PolicyPaymentEntryId = p.PolicyPaymentId,
                                                                 isDeleted = p.IsDeleted,
                                                                 IsResolvedFromCommDashboard = p.IsResolvedFromCommDashboard,
                                                                 IsRemovedFromCommDashboard = p.IsRemovedFromCommDashboard

                                                             }).ToList();
                return _FollowupIssue;
                #region for sp
                //var Result = (from f in DataModel.FollowupIssues
                //              where f.PolicyId== policyID
                //              select f);
                //List<DisplayFollowupIssue> _FollowupIssue = (from p in Result
                //                                             where (p.PolicyId == policyID)
                //                                             select new DisplayFollowupIssue
                //                                      {
                //                                        IssueId = (Guid?)p.IssueId ?? Guid.Empty,
                //                                        IssueStatusId = p.IssueStatusId.Value,

                //                                        IssueCategoryID = p.IssueCategoryId.Value,

                //                                        IssueResultId = p.IssueResultId.Value,
                //                                        IssueReasonId = p.IssueReasonId.Value,
                //                                        InvoiceDate = p.InvoiceDate,
                //                                        NextFollowupDate = p.NextFollowUpDate,
                //                                        PolicyIssueNote = p.Notes,
                //                                        Payment = p.Payment,
                //                                        PolicyId = p.PolicyId.Value,
                //                                        Payor = p.Policy.Payor.PayorName,
                //                                        Agency = p.Policy.Licensee.Company,
                //                                        Insured = p.Policy.Insured,
                //                                        PolicyNumber = p.Policy.PolicyNumber,
                //                                        PreviousStatusId = p.PreviousStatusId,
                //                                        LastModifiedDate = p.LastModifiedDate,
                //                                        IssueTrackNumber = p.IssueTrackNumber,
                //                                        CreatedDate = p.CreatedDate,
                //                                        ModifiedBy = p.ModifiedBy,
                //                                        FromDate = p.FromDate,
                //                                        ToDate = p.ToDate,
                //                                        PayorId = p.Policy.Payor.PayorId,
                //                                        LicenseeId = p.Policy.Licensee.LicenseeId,
                //                                        Content = 1,
                //                                        IsTrackIncomingPercentage = p.Policy.IsTrackIncomingPercentage,
                //                                        IsTrackMissingMonth = p.Policy.IsTrackMissingMonth,
                //                                        IsTrackPayment = p.Policy.IsTrackPayment,
                //                                        //PolicyPaymentEntryId = p.PolicyPaymentId,

                //                                      }).ToList();
                //return _FollowupIssue.OrderBy(p=>p.InvoiceDate).ToList();
                // List<DisplayFollowupIssue> _FollowupIssue1 = new List<DisplayFollowupIssue>();
                #endregion
            }

        }

        public static List<DisplayFollowupIssue> GetFewIssueForCommissionDashBoard(Guid PolicyId)
        {
            List<DisplayFollowupIssue> _followIssLst = new List<DisplayFollowupIssue>();
            List<DisplayFollowupIssue> _FollowupIssue = GetIssues(PolicyId);
            List<DisplayFollowupIssue> _FollowUpVar = _FollowupIssue.Where(p => (p.IssueCategoryID != (int)FollowUpIssueCategory.MissFirst)
                    && (p.IssueCategoryID != (int)FollowUpIssueCategory.MissInv)).ToList();

            List<DisplayFollowupIssue> _FollowUpMiss = _FollowupIssue.Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst)
                   || (p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();

            //List<Guid?> _PolicyIdLst = _FollowUpMiss.Select(p => p.PolicyId).Distinct().ToList();
            if (_FollowUpMiss != null && _FollowUpMiss.Count != 0)
            {
                DisplayFollowupIssue folloissue = _FollowUpMiss.Where(p => p.PolicyId == PolicyId).FirstOrDefault();

                DateTime? AutoTerminationDate = PolicyLearnedField.GetPolicyLearnedFieldAutoTerminationDate(PolicyId);

                if (AutoTerminationDate == null) return _FollowupIssue;//return all followUp issues if there is no Autotrm date in learned
                int DiffMonth = FollowUpUtill.MonthBetweenTwoYear(folloissue.FromDate.Value, folloissue.ToDate ?? DateTime.Today);
                int issueCnt = 0;
                if (DiffMonth == 1)
                {
                    issueCnt = 3;

                }
                else if (DiffMonth == 3)
                {
                    issueCnt = 2;

                }
                else if (DiffMonth == 6)
                {
                    issueCnt = 2;

                }
                else if (DiffMonth == 12)
                {
                    issueCnt = 1;
                }
                else
                {
                    //  _followIssLst = new List<FollowupIssue>();
                    // _followIssLst.AddRange(_FollowupIssue);
                }

                if (_followIssLst == null)
                    _followIssLst = new List<DisplayFollowupIssue>();

                _FollowUpMiss = _FollowUpMiss.OrderBy(f => f.FromDate).ToList();
                _followIssLst.AddRange(_FollowUpMiss.Where(p => (p.FromDate <= AutoTerminationDate)).ToList());
                List<DisplayFollowupIssue> restIssues = _FollowUpMiss.Where(p => (p.FromDate > AutoTerminationDate)).ToList();
                foreach (DisplayFollowupIssue fs in restIssues)
                {
                    issueCnt--;
                    if (issueCnt < 0) break;
                    _followIssLst.Add(fs);

                }
            }
            if (_FollowUpVar != null && _FollowUpVar.Count != 0)
            {
                _followIssLst.AddRange(_FollowUpVar);
            }
            // return _followIssLst;

            return _followIssLst.OrderBy(p => p.PolicyNumber).ThenBy(p => p.InvoiceDate).ToList();

        }

        ////public static List<DisplayFollowupIssue> GetFewIssueForCommissionDashBoard(Guid PolicyId)
        ////{
        ////    List<DisplayFollowupIssue> _followIssLst = new List<DisplayFollowupIssue>();
        ////    List<DisplayFollowupIssue> _FollowupIssue = GetIssues(PolicyId);
        ////    List<DisplayFollowupIssue> _FollowUpVar = _FollowupIssue.Where(p => (p.IssueCategoryID != (int)FollowUpIssueCategory.MissFirst)
        ////            && (p.IssueCategoryID != (int)FollowUpIssueCategory.MissInv)).ToList();

        ////    List<DisplayFollowupIssue> _FollowUpMiss = _FollowupIssue.Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst)
        ////           || (p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();

        ////    //List<Guid?> _PolicyIdLst = _FollowUpMiss.Select(p => p.PolicyId).Distinct().ToList();
        ////    if (_FollowUpMiss != null && _FollowUpMiss.Count != 0)
        ////    {
        ////        DisplayFollowupIssue folloissue = _FollowUpMiss.Where(p => p.PolicyId == PolicyId).FirstOrDefault();
        ////        PolicyLearnedFieldData _policyLrnd = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
        ////        if (_policyLrnd.AutoTerminationDate == null) return _FollowupIssue;//return all followUp issues if there is no Autotrm date in learned
        ////        int DiffMonth = FollowUpUtill.MonthBetweenTwoYear(folloissue.FromDate.Value, folloissue.ToDate ?? DateTime.Today);
        ////        int issueCnt = 0;
        ////        if (DiffMonth == 1)
        ////        {
        ////            issueCnt = 3;

        ////        }
        ////        else if (DiffMonth == 3)
        ////        {
        ////            issueCnt = 2;

        ////        }
        ////        else if (DiffMonth == 6)
        ////        {
        ////            issueCnt = 2;

        ////        }
        ////        else if (DiffMonth == 12)
        ////        {
        ////            issueCnt = 1;
        ////        }
        ////        else
        ////        {
        ////            //  _followIssLst = new List<FollowupIssue>();
        ////            // _followIssLst.AddRange(_FollowupIssue);
        ////        }

        ////        if (_followIssLst == null)
        ////            _followIssLst = new List<DisplayFollowupIssue>();

        ////        _FollowUpMiss = _FollowUpMiss.OrderBy(f => f.FromDate).ToList();
        ////        _followIssLst.AddRange(_FollowUpMiss.Where(p => (p.FromDate <= _policyLrnd.AutoTerminationDate)).ToList());
        ////        List<DisplayFollowupIssue> restIssues = _FollowUpMiss.Where(p => (p.FromDate > _policyLrnd.AutoTerminationDate)).ToList();
        ////        foreach (DisplayFollowupIssue fs in restIssues)
        ////        {
        ////            issueCnt--;
        ////            if (issueCnt < 0) break;
        ////            _followIssLst.Add(fs);

        ////        }
        ////    }
        ////    if (_FollowUpVar != null && _FollowUpVar.Count != 0)
        ////    {
        ////        _followIssLst.AddRange(_FollowUpVar);
        ////    }
        ////    // return _followIssLst;

        ////    return _followIssLst.OrderBy(p => p.PolicyNumber).ThenBy(p => p.InvoiceDate).ToList();

        ////}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="PayorNickName"></param>
        /// <param name="Agency"></param>
        /// <param name="FollowUp_OnlyView">This feature is needede to ask--Not Implemented  till 10 feb 2011</param>
        /// <returns></returns>
        public static List<DisplayFollowupIssue> GetAllIssues(int Status, Guid? PayorID, Guid? AgencyID, bool Followup, int? intDays)
        {
            List<DisplayFollowupIssue> _FollowupIssue = new List<DisplayFollowupIssue>();

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    #region comment

                    //Expression<Func<DLinq.FollowupIssue, bool>> final = p => p.IssueId != null;

                    //List<DLinq.FollowupIssue> tempResult = new List<DLinq.FollowupIssue>();
                    //Expression<Func<DLinq.FollowupIssue, bool>> parameter;
                    //DataModel.CommandTimeout = 3600;
                    //if (Status != 0)
                    //{
                    //    parameter = p => p.IssueStatusId == Status;
                    //    final = final.And(parameter);
                    //}
                    //if (PayorID != Guid.Empty)
                    //{
                    //    parameter = p => p.Policy.PayorId == PayorID;
                    //    final = final.And(parameter);
                    //}
                    //else
                    //{
                    //    parameter = p => p.Policy.Payor.IsGlobal == true;
                    //    final = final.And(parameter);
                    //}
                    //if (AgencyID != Guid.Empty)
                    //{
                    //    parameter = p => p.Policy.Licensee.LicenseeId == AgencyID;
                    //    final = final.And(parameter);
                    //}


                    //var Result = (from f in DataModel.FollowupIssues
                    //                  .Where(final)
                    //              select f);




                    //List<DisplayFollowupIssue> tempFoll = new List<DisplayFollowupIssue>();

                    //// _FollowupIssue
                    //Dictionary<Guid?, bool> LicenseeFollowUp = new Dictionary<Guid?, bool>();

                    //bool AddFollowUpdIssue = false;
                    //foreach (var followupIssue in Result)
                    //{
                    //    if (followupIssue.IssueCategoryId == (int)FollowUpIssueCategory.MissFirst || followupIssue.IssueCategoryId == (int)FollowUpIssueCategory.MissInv)
                    //    {
                    //        if (followupIssue != null)
                    //        {
                    //            if (followupIssue.Policy != null)
                    //            {
                    //                AddFollowUpdIssue = followupIssue.Policy.IsTrackMissingMonth;
                    //            }

                    //        }

                    //    }
                    //    else
                    //    {
                    //        if (followupIssue != null)
                    //        {
                    //            if (followupIssue.Policy != null)
                    //            {
                    //                AddFollowUpdIssue = followupIssue.Policy.IsTrackIncomingPercentage;
                    //            }
                    //        }
                    //    }

                    //    if (!AddFollowUpdIssue)
                    //        continue;

                    //    Guid? LicenceId = followupIssue.Policy.PolicyLicenseeId;
                    //    if (!LicenseeFollowUp.ContainsKey(LicenceId))
                    //    {
                    //        LicenseeFollowUp.Add(LicenceId, BillingLineDetail.IsFollowUpLicensee(LicenceId.Value));
                    //    }
                    //    if (Followup)
                    //    {
                    //        if (LicenseeFollowUp[LicenceId])
                    //        {
                    //            tempResult.Add(followupIssue);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (!LicenseeFollowUp[LicenceId])
                    //        {
                    //            tempResult.Add(followupIssue);
                    //        }
                    //    }
                    //}
                    //List<DisplayFollowupIssue> _FollowupIssue = (from p in Result
                    //                                             select new DisplayFollowupIssue
                    //                                      {
                    //                                        IssueId = (Guid?)p.IssueId ?? Guid.Empty,
                    //                                        IssueStatusId = p.IssueStatusId,
                    //                                        IssueCategoryID = p.IssueCategoryId,

                    //                                        IssueResultId = p.IssueResultId,
                    //                                        IssueReasonId = p.IssueReasonId,
                    //                                        InvoiceDate = p.InvoiceDate,
                    //                                        NextFollowupDate = p.NextFollowUpDate,
                    //                                        PolicyIssueNote = p.Notes,
                    //                                        Payment = p.Payment,
                    //                                        PolicyId = p.PolicyId,
                    //                                        Payor = p.Policy.Payor.PayorName,
                    //                                        Agency = p.Policy.Licensee.Company,
                    //                                        Insured = p.Policy.Insured,
                    //                                        PolicyNumber = p.Policy.PolicyNumber,
                    //                                        PreviousStatusId = p.PreviousStatusId ?? 0,
                    //                                        LastModifiedDate = p.LastModifiedDate,
                    //                                        IssueTrackNumber = p.IssueTrackNumber,
                    //                                        CreatedDate = p.CreatedDate,
                    //                                        ModifiedBy = p.ModifiedBy,
                    //                                        FromDate = p.FromDate,
                    //                                        ToDate = p.ToDate,
                    //                                        PayorId = p.Policy.Payor.PayorId,
                    //                                        LicenseeId = p.Policy.Licensee.LicenseeId,
                    //                                        Content = 1,
                    //                                      }).ToList();



                    //List<string> query = DataModel.SP_Select_FirstFollowuP_Result(Status, PayorID, AgencyID).ToList();


                    #endregion
                    //Set Time out proerty to data model for time out property
                    DataModel.CommandTimeout = 360000;
                    //ActionLogger.Logger.WriteImportPolicyLog("Before followup" + DateTime.Now.ToLongTimeString(), true);

                    //var Result = DataModel.Sp_SelectFollowupRecord(AgencyID, Followup);
                    //var Result = DataModel.FollowupFilter(AgencyID, Followup, intDays);  

                    var Result = DataModel.AllFollowupFilter(AgencyID, Followup, intDays);
                    //ActionLogger.Logger.WriteImportPolicyLog("After followup" + DateTime.Now.ToLongTimeString(), true);

                    _FollowupIssue = Result.Select(p => new DisplayFollowupIssue()
                    {
                        IssueId = (Guid?)p.IssueId ?? Guid.Empty,
                        IssueStatusId = p.IssueStatusId,
                        IssueCategoryID = p.IssueCategoryId,
                        IssueResultId = p.IssueResultId,
                        IssueReasonId = p.IssueReasonId,
                        InvoiceDate = p.InvoiceDate,
                        NextFollowupDate = p.NextFollowUpDate,
                        PolicyIssueNote = p.PolicyIssueNote,
                        //AutoTerminationDate=p.AutoTermDate,
                        Payment = p.Payment,
                        PolicyId = p.PolicyId,
                        Payor = p.Payor,
                        Agency = p.Agency,
                        Client = p.Client,
                        Insured = p.Insured,
                        PayorId = p.PayorID,
                        PolicyNumber = p.PolicyNumber,
                        LicenseeId = (Guid?)p.LicenseeId ?? Guid.Empty,
                        PreviousStatusId = p.PreviousStatusId ?? 0,
                        LastModifiedDate = p.LastModifiedDate,
                        IssueTrackNumber = p.IssueTrackNumber ?? 0,
                        CreatedDate = p.CreatedDate,
                        ModifiedBy = p.ModifiedBy,
                        FromDate = p.FromDate,
                        ToDate = p.ToDate,
                        Content = 1,
                        IsTrackIncomingPercentage = p.IsTrackIncomingPercentage,
                        IsTrackMissingMonth = p.IsTrackMissingMonth,
                        IsTrackPayment = p.IsTrackPayment,

                    }).ToList();

                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog(ex.StackTrace.ToString() + DateTime.Now.ToLongTimeString(), true);
            }

            return _FollowupIssue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="PayorNickName"></param>
        /// <param name="Agency"></param>
        /// <param name="FollowUp_OnlyView">This feature is needede to ask--Not Implemented  till 10 feb 2011</param>
        /// <returns></returns>
        public static List<DisplayFollowupIssue> GetAllIssuesFollowupScr(int Status, Guid? PayorID, Guid? AgencyID, bool Followup, int intDays)
        {
            List<DisplayFollowupIssue> _FollowupIssueLst = GetAllIssues(Status, PayorID, AgencyID, Followup, intDays);
            return _FollowupIssueLst;
        }

        public static List<DisplayFollowupIssue> GetFewIssueAccordingtoMode(int Status, Guid PayorID, Guid AgencyID, bool Followup, int intDays)
        {

            List<DisplayFollowupIssue> _followIssLst = new List<DisplayFollowupIssue>();
            List<DisplayFollowupIssue> _FollowupIssue = GetAllIssues(Status, PayorID, AgencyID, Followup, intDays);

            List<DisplayFollowupIssue> _FollowUpMiss1 = _FollowupIssue.Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst)
                   || (p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();


            if (_FollowUpMiss1 != null && _FollowUpMiss1.Count != 0)
            {
                List<Guid?> _PolicyIdLst = _FollowUpMiss1.Select(p => p.PolicyId).Distinct().ToList();
                foreach (Guid gid in _PolicyIdLst)
                {
                    List<DisplayFollowupIssue> _FollowUpVar = _FollowupIssue.Where(p => (p.IssueCategoryID != (int)FollowUpIssueCategory.MissFirst)
                   && (p.IssueCategoryID != (int)FollowUpIssueCategory.MissInv) && (p.PolicyId == gid)).ToList();

                    List<DisplayFollowupIssue> _FollowUpMiss = _FollowupIssue.Where(p => ((p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst)
                              || (p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)) && (p.PolicyId == gid)).ToList();
                    if (_FollowUpMiss != null && _FollowUpMiss.Count != 0)
                    {
                        DisplayFollowupIssue folloissue = _FollowUpMiss.FirstOrDefault();
                        DateTime? AutoTerminationDate;
                        using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                        {
                            AutoTerminationDate = (from polAutoTerm in DataModel.PolicyLearnedFields
                                                   where polAutoTerm.PolicyId == gid
                                                   select polAutoTerm.AutoTerminationDate).FirstOrDefault();
                        }

                        //var vBalue = from p in _FollowupIssue where p.PolicyId == gid select p.AutoTerminationDate;

                        //AutoTerminationDate = vBalue.FirstOrDefault();

                        if (AutoTerminationDate == null)
                        {
                            //_followIssLst.AddRange(_FollowupIssue);
                            //continue;
                        }
                        int DiffMonth = FollowUpUtill.MonthBetweenTwoYear(folloissue.FromDate.Value, folloissue.ToDate ?? DateTime.Today);
                        int issueCnt = 0;
                        if (DiffMonth == 1)
                        {
                            issueCnt = 3;

                        }
                        else if (DiffMonth == 3)
                        {
                            issueCnt = 2;

                        }
                        else if (DiffMonth == 6)
                        {
                            issueCnt = 2;

                        }
                        else if (DiffMonth == 12)
                        {
                            issueCnt = 1;
                        }
                        else
                        {
                            //_followIssLst = new List<DisplayFollowupIssue>();
                            //_followIssLst.AddRange(_FollowupIssue);
                        }

                        if (_followIssLst == null)
                            _followIssLst = new List<DisplayFollowupIssue>();

                        if (AutoTerminationDate != null)
                        {
                            _FollowUpMiss = _FollowUpMiss.OrderBy(f => f.FromDate).ToList();

                            _followIssLst.AddRange(_FollowUpMiss.Where(p => (p.FromDate <= AutoTerminationDate)).ToList());
                            List<DisplayFollowupIssue> restIssues = _FollowUpMiss.Where(p => (p.FromDate > AutoTerminationDate)).ToList();
                            foreach (DisplayFollowupIssue fs in restIssues)
                            {
                                issueCnt--;
                                if (issueCnt < 0) break;
                                _followIssLst.Add(fs);

                            }
                        }
                        else
                        {
                            _followIssLst.AddRange(_FollowUpMiss);
                        }
                    }
                    // return _followIssLst;
                    if (_FollowUpVar != null && _FollowUpVar.Count != 0)
                    {
                        _followIssLst.AddRange(_FollowUpVar);
                    }
                }
            }
            return _followIssLst.OrderBy(p => p.PolicyNumber).ThenBy(p => p.InvoiceDate).ToList();

        }

        public static List<DisplayFollowupIssue> GetFewIssueAccordingtoModeForFollowupScr(int Status, Guid? PayorID, Guid? AgencyID, bool Followup, int intDay)
        {
            if (AgencyID == Guid.Empty)
            {
                AgencyID = null;
            }

            if (PayorID == Guid.Empty)
            {
                PayorID = null;
            }
            List<DisplayFollowupIssue> _followIssLst = new List<DisplayFollowupIssue>();
            List<DisplayFollowupIssue> _FollowupIssue = GetAllIssuesFollowupScr(Status, PayorID, AgencyID, Followup, intDay);

            #region"Comented code"

            //List<DisplayFollowupIssue> _FollowUpVar = _FollowupIssue.Where(p => (p.IssueCategoryID != (int)FollowUpIssueCategory.MissFirst)
            //         && (p.IssueCategoryID != (int)FollowUpIssueCategory.MissInv)).ToList();

            //List<DisplayFollowupIssue> _FollowUpMiss1 = _FollowupIssue.Where(p => (p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst)
            //       || (p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)).ToList();

            //if (_FollowUpMiss1 != null && _FollowUpMiss1.Count != 0)
            //{
            //    List<Guid?> _PolicyIdLst = _FollowUpMiss1.Select(p => p.PolicyId).Distinct().ToList();
            //    foreach (Guid gid in _PolicyIdLst)
            //    {
            //        DateTime? AutoTerminationDate;

            //        List<DisplayFollowupIssue> _FollowUpVar = _FollowupIssue.Where(p => (p.IssueCategoryID != (int)FollowUpIssueCategory.MissFirst)
            //       && (p.IssueCategoryID != (int)FollowUpIssueCategory.MissInv) && (p.PolicyId == gid)).ToList();

            //        List<DisplayFollowupIssue> _FollowUpMiss = _FollowupIssue.Where(p => ((p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst)
            //                  || (p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)) && (p.PolicyId == gid)).ToList();
            //        if (_FollowUpMiss != null && _FollowUpMiss.Count != 0)
            //        {
            //            DisplayFollowupIssue folloissue = _FollowUpMiss.FirstOrDefault();
            //            //PolicyLearnedFieldData _policyLrnd = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(folloissue.PolicyId.Value);
            //            var vBalue = from p in _FollowupIssue where p.PolicyId == gid select p.AutoTerminationDate;

            //            AutoTerminationDate = vBalue.FirstOrDefault();


            //            if (AutoTerminationDate == null)
            //            {
            //                //_followIssLst.AddRange(_FollowupIssue);
            //                //continue;
            //            }
            //            int DiffMonth = FollowUpUtill.MonthBetweenTwoYear(folloissue.FromDate.Value, folloissue.ToDate ?? DateTime.Today);
            //            int issueCnt = 0;
            //            if (DiffMonth == 1)
            //            {
            //                issueCnt = 3;

            //            }
            //            else if (DiffMonth == 3)
            //            {
            //                issueCnt = 2;

            //            }
            //            else if (DiffMonth == 6)
            //            {
            //                issueCnt = 2;

            //            }
            //            else if (DiffMonth == 12)
            //            {
            //                issueCnt = 1;
            //            }
            //            else
            //            {
            //                //_followIssLst = new List<DisplayFollowupIssue>();
            //                //_followIssLst.AddRange(_FollowupIssue);

            //            }

            //            if (_followIssLst == null)
            //                _followIssLst = new List<DisplayFollowupIssue>();

            //            if (AutoTerminationDate != null)
            //            {
            //                _FollowUpMiss = _FollowUpMiss.OrderBy(f => f.FromDate).ToList();

            //                _followIssLst.AddRange(_FollowUpMiss.Where(p => (p.FromDate <= AutoTerminationDate)).ToList());
            //                List<DisplayFollowupIssue> restIssues = _FollowUpMiss.Where(p => (p.FromDate > AutoTerminationDate)).ToList();
            //                foreach (DisplayFollowupIssue fs in restIssues)
            //                {
            //                    issueCnt--;
            //                    if (issueCnt < 0) break;
            //                    _followIssLst.Add(fs);

            //                }
            //            }
            //            else
            //            {
            //                _followIssLst.AddRange(_FollowUpMiss);
            //            }
            //        }
            //        // return _followIssLst;
            //        if (_FollowUpVar != null && _FollowUpVar.Count != 0)
            //        {
            //            _followIssLst.AddRange(_FollowUpVar);
            //        }
            //    }

            //}
            //_followIssLst = _followIssLst.Where(p => p.PolicyId == new Guid("D2BA2BD3-9CD4-4ACA-B781-5CB4B34922A2")).ToList();
            //ActionLogger.Logger.WriteImportPolicyLog("11 " + DateTime.Now.ToLongTimeString(), true);

            //return _FollowupIssue.OrderBy(p => p.PolicyNumber).ThenBy(p => p.InvoiceDate).ToList();
            //ActionLogger.Logger.WriteImportPolicyLog("11 " + DateTime.Now.ToLongTimeString(), true);
            #endregion

            return _FollowupIssue.OrderBy(p => p.PolicyNumber).ThenBy(p => p.InvoiceDate).ToList();
        }

        private static bool CheckForPolicySettingForFollowUp(DisplayFollowupIssue followupIssue)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                bool Result = false;
                if (followupIssue.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst || followupIssue.IssueCategoryID == (int)FollowUpIssueCategory.MissInv)
                {
                    Result = (from f in DataModel.Policies
                              where (f.PolicyId == followupIssue.PolicyId)
                              select f.IsTrackMissingMonth).FirstOrDefault();
                }
                else
                {
                    Result = (from f in DataModel.Policies
                              where (f.PolicyId == followupIssue.PolicyId)
                              select f.IsTrackIncomingPercentage).FirstOrDefault();
                }
                return Result;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<IssuePolicyDetail> GetIssueDetail(Guid PolicyId, Guid FollowUpIssueId)
        {
            if (PolicyId == Guid.Empty || PolicyId == null) return new List<IssuePolicyDetail>();

            List<IssuePolicyDetail> oIssuePolicyDetail = new List<IssuePolicyDetail>();
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                //ActionLogger.Logger.WriteImportPolicyLog("BEFORE GetIssueDetail _PMC" + DateTime.Now.ToLongTimeString(), true);
                var vPaymentPMC = 0.0;
                try
                {
                    vPaymentPMC = Convert.ToDouble(PostUtill.CalculatePMC(PolicyId));
                }
                catch
                {
                    vPaymentPMC = 0;
                    // ActionLogger.Logger.WriteImportPolicyLog(ex.ToString() + " BEFORE GetIssueDetail _PMC" + DateTime.Now.ToLongTimeString(), true);
                }


                var vPolicyDetail = from p in DataModel.Policies
                                    where (p.PolicyId == PolicyId)
                                    select new
                                    {
                                        p.PolicyType,
                                        p.Enrolled,
                                        p.Eligible,
                                        p.MonthlyPremium,
                                        p.SplitPercentage,
                                        p.IsIncomingBasicSchedule,
                                        p.IsOutGoingBasicSchedule,
                                        p.PolicyTerminationDate,

                                        // p.TrackFromDate,
                                    };

                var vClientDetail = from p in DataModel.Policies
                                    join cli in DataModel.Clients on p.Client.ClientId equals cli.ClientId
                                    where (p.PolicyId == PolicyId)
                                    select new
                                    {
                                        cli.Name,
                                    };

                var vInsuredDetail = from p in DataModel.Policies
                                     join cli in DataModel.Policies on p.PolicyId equals cli.PolicyId
                                     where (p.PolicyId == PolicyId)
                                     select new
                                     {
                                         cli.Insured,
                                     };

                var vStatmentDetail = from p in DataModel.PolicyPaymentEntries
                                      where (p.FollowUpVarIssueId == FollowUpIssueId)
                                      select new
                                      {
                                          p.Statement.StatementNumber,

                                      };

                var vIncomingPaymentDetail = from p in DataModel.Policies
                                             join pis in DataModel.PolicyIncomingSchedules on p.PolicyId equals pis.Policy.PolicyId//Incoming
                                             where (p.PolicyId == PolicyId)
                                             select new
                                             {
                                                 pis.FirstYearPercentage,
                                                 pis.RenewalPercentage,

                                             };

                string primaryPayee = "";
                if (vPolicyDetail.FirstOrDefault().IsOutGoingBasicSchedule.HasValue)
                {
                    if (vPolicyDetail.FirstOrDefault().IsOutGoingBasicSchedule.Value)
                    {
                        var temp = (from p in DataModel.PolicyOutgoingSchedules
                                    where ((p.PolicyId == PolicyId) && (p.IsPrimaryAgent == true))
                                    select p);
                        if (temp == null || temp.Count() == 0)
                        {
                            primaryPayee = "";
                        }
                        else
                        {
                            Guid? id = (from p in DataModel.PolicyOutgoingSchedules
                                        where ((p.PolicyId == PolicyId) && (p.IsPrimaryAgent == true))
                                        select p).FirstOrDefault().PayeeUserCredentialId;
                            //primaryPayee = User.GetAllUsers().Where(p => p.UserCredentialID == id).FirstOrDefault().FirstName;
                            User Firstuser = User.GetAllUsers().Where(p => p.UserCredentialID == id).FirstOrDefault();
                            if (Firstuser != null)
                                primaryPayee = Firstuser.FirstName;
                        }

                    }
                    else
                    {
                        var temp = (from p in DataModel.PolicyOutgoingAdvancedSchedules
                                    where ((p.PolicyId == PolicyId) && (p.IsPrimaryAgent == true))
                                    select p);
                        if (temp == null || temp.Count() == 0)
                        {
                            primaryPayee = "";
                        }
                        else
                        {
                            Guid? id = (from p in DataModel.PolicyOutgoingAdvancedSchedules
                                        where ((p.PolicyId == PolicyId) && (p.IsPrimaryAgent == true))
                                        select p).FirstOrDefault().PayeeUserCredentialId;
                            primaryPayee = User.GetAllUsers().Where(p => p.UserCredentialID == id).FirstOrDefault().FirstName;
                        }
                    }


                }

                var ValuesFromDB = (from p in DataModel.Policies
                                    where p.PolicyId == PolicyId
                                    select new
                                    {
                                        carrierName = p.Carrier.CarrierName,
                                        Productname = p.Coverage.ProductName,
                                        LearnedEffective = p.PolicyLearnedField.Effective,
                                        LearnedTrackFrom = p.PolicyLearnedField.TrackFrom,
                                        LearnedAutoTerm = p.PolicyLearnedField.AutoTerminationDate,
                                        PolicyStatus = p.MasterPolicyStatu.Name,
                                        PolicyMode = p.MasterPolicyMode.Name,
                                        TerminationReason = p.MasterPolicyTerminationReason.Name

                                    }).FirstOrDefault();
                var vCarrierDetail = from p in DataModel.Policies
                                     join pac in DataModel.Carriers on p.Carrier.CarrierId equals pac.CarrierId
                                     where (p.PolicyId == PolicyId)
                                     select new
                                     {
                                         pac.CarrierName,
                                     };

                var vFollowUpDetail = from p in DataModel.FollowupIssues
                                      where (p.IssueId == FollowUpIssueId)
                                      select new
                                      {

                                          p.LastModifiedDate,
                                          p.CreatedDate,
                                          p.IssueTrackNumber,
                                          p.ModifiedBy,
                                      };
                string FollowUpLastModifiedUser = "";
                if (vFollowUpDetail.FirstOrDefault().ModifiedBy != null)
                {
                    if (vFollowUpDetail.FirstOrDefault().ModifiedBy != Guid.Empty)
                        FollowUpLastModifiedUser = User.GetAllUsers().Where(p => p.UserCredentialID == vFollowUpDetail.FirstOrDefault().ModifiedBy).FirstOrDefault().FirstName;
                }
                else
                {
                    FollowUpLastModifiedUser = "Super";
                }
                try
                {
                    IssuePolicyDetail _IssuePolicyDetail = new IssuePolicyDetail();
                    _IssuePolicyDetail.Payment = Convert.ToDecimal(vPaymentPMC);
                    if (vPolicyDetail.FirstOrDefault() != null)
                    {
                        _IssuePolicyDetail.Enroll_Eligible = vPolicyDetail.FirstOrDefault().Enrolled + "/" + vPolicyDetail.FirstOrDefault().Eligible;
                        _IssuePolicyDetail.Type = vPolicyDetail.FirstOrDefault().PolicyType;
                        _IssuePolicyDetail.ModalPremium = vPolicyDetail.FirstOrDefault().MonthlyPremium;
                        _IssuePolicyDetail.SharePercentage = vPolicyDetail.FirstOrDefault().SplitPercentage;
                        _IssuePolicyDetail.PolicyTermDate = vPolicyDetail.FirstOrDefault().PolicyTerminationDate.ToString();
                        if (vPolicyDetail.FirstOrDefault().IsIncomingBasicSchedule.HasValue)
                        {
                            if (vPolicyDetail.FirstOrDefault().IsIncomingBasicSchedule.Value)
                            {
                                _IssuePolicyDetail.Schedule = vIncomingPaymentDetail.FirstOrDefault().FirstYearPercentage + " %" + vIncomingPaymentDetail.FirstOrDefault().RenewalPercentage + " %";
                            }
                            else
                            {
                                _IssuePolicyDetail.Schedule = "Advance Schedule";
                            }

                        }

                    }
                    //if (vClientDetail.FirstOrDefault() != null)
                    //{
                    //  _IssuePolicyDetail.Client = vClientDetail.FirstOrDefault().Name;

                    //}

                    if (vInsuredDetail.FirstOrDefault() != null)
                    {
                        _IssuePolicyDetail.Insured = vInsuredDetail.FirstOrDefault().Insured;

                    }
                    if (vClientDetail.FirstOrDefault() != null)
                    {
                        _IssuePolicyDetail.Client = vClientDetail.FirstOrDefault().Name;

                    }
                    try
                    {
                        if (vStatmentDetail.FirstOrDefault() != null)
                        {
                            _IssuePolicyDetail.StatementNumber = vStatmentDetail.FirstOrDefault().StatementNumber;

                        }
                        else
                        {
                            _IssuePolicyDetail.StatementNumber = 0;
                        }
                    }
                    catch
                    {
                        _IssuePolicyDetail.StatementNumber = 0;
                    }

                    _IssuePolicyDetail.PrimaryPayee = primaryPayee;
                    if (vIncomingPaymentDetail.FirstOrDefault() != null)
                    {
                        _IssuePolicyDetail.Schedule = vIncomingPaymentDetail.FirstOrDefault().FirstYearPercentage.ToString() + @"%/" + vIncomingPaymentDetail.FirstOrDefault().RenewalPercentage + "%";


                    }
                    if (vCarrierDetail.FirstOrDefault() != null)
                    {
                        _IssuePolicyDetail.Carrier = vCarrierDetail.FirstOrDefault().CarrierName;

                    }
                    if (ValuesFromDB.Productname != null)
                    {
                        _IssuePolicyDetail.Product = ValuesFromDB.Productname;

                    }
                    if (ValuesFromDB.LearnedEffective != null)
                    {
                        _IssuePolicyDetail.Effective = ValuesFromDB.LearnedEffective.Value.ToString();

                    }
                    if (ValuesFromDB.LearnedTrackFrom != null)
                    {
                        _IssuePolicyDetail.TrackFrom = ValuesFromDB.LearnedTrackFrom.Value.ToString();
                    }
                    if (ValuesFromDB.LearnedAutoTerm != null)
                    {
                        _IssuePolicyDetail.AutoTrmDate = ValuesFromDB.LearnedAutoTerm.Value.ToString();
                    }

                    if (ValuesFromDB.PolicyStatus != null)
                    {
                        _IssuePolicyDetail.Status = ValuesFromDB.PolicyStatus;

                    }
                    //  oIssuePolicyDetail.TrmReason = vTermissionReason.FirstOrDefault().Name;
                    if (ValuesFromDB.PolicyMode != null)
                    {
                        _IssuePolicyDetail.Mode = ValuesFromDB.PolicyMode;
                    }
                    if (ValuesFromDB.TerminationReason != null)
                    {
                        _IssuePolicyDetail.TrmReason = ValuesFromDB.TerminationReason;
                    }

                    if (vFollowUpDetail.FirstOrDefault() != null)
                    {
                        _IssuePolicyDetail.LastUpdate = vFollowUpDetail.FirstOrDefault().LastModifiedDate.ToString();
                        _IssuePolicyDetail.Created = vFollowUpDetail.FirstOrDefault().CreatedDate.ToString();
                        _IssuePolicyDetail.TrackingNumber = vFollowUpDetail.FirstOrDefault().IssueTrackNumber.ToString();
                        _IssuePolicyDetail.LastUser = FollowUpLastModifiedUser;

                    }
                    oIssuePolicyDetail.Add(_IssuePolicyDetail);

                }
                catch
                {
                }
                return oIssuePolicyDetail;
            }
        }

        public static List<FollowupIncomingPament> GetIncomingPayment(Guid? PolicyId)
        {
            if (PolicyId == Guid.Empty || PolicyId == null) return new List<FollowupIncomingPament>();
            List<FollowupIncomingPament> _FollowupIncomingPament = null;
            //  FollowupIncomingPament oFollowupIncomingPament = new FollowupIncomingPament();
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    _FollowupIncomingPament = (from ppe in DataModel.PolicyPaymentEntries
                                               where ppe.Policy.PolicyId == PolicyId

                                               select new FollowupIncomingPament
                                               {
                                                   InvoiceDate = ppe.InvoiceDate ?? null,
                                                   PaymentRecived = ppe.PaymentRecived ?? null,
                                                   CommissionPercentage = ppe.CommissionPercentage ?? null,
                                                   NumberOfUnits = ppe.NumberOfUnits ?? null,
                                                   DollerPerUnit = ppe.DollerPerUnit ?? null,
                                                   Fee = ppe.Fee ?? null,
                                                   SplitPer = ppe.SplitPercentage ?? null,
                                                   TotalPayment = ppe.TotalPayment ?? null,
                                                   Statement = ppe.Statement.StatementNumber,
                                                   Batch = ppe.Statement.Batch.BatchNumber,
                                               }).ToList();
                }
                catch
                {

                }

            }
            return _FollowupIncomingPament;
        }

        public static string GetIssuesNote(Guid IssueID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var Notes = from fup in DataModel.FollowupIssues
                            where fup.IssueId == IssueID
                            select new
                            {
                                fup.Notes,

                            };
                return Notes.ToString();

            }
        }

        public static List<FollowUPPayorContacts> GetPayorContact(Guid? PolicyId)
        {
            if (PolicyId == Guid.Empty || PolicyId == null) return new List<FollowUPPayorContacts>();
            List<FollowUPPayorContacts> _FollowUPPayorContacts = null;

            Guid iPayorID = Guid.Empty;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                var gPayorID = from pay in DataModel.Policies
                               where pay.PolicyId == PolicyId
                               select new
                               {
                                   pay.Payor.PayorId,

                               };
                iPayorID = gPayorID.FirstOrDefault().PayorId;

                _FollowUPPayorContacts = (from gpc in DataModel.GlobalPayorContacts
                                          where gpc.GlobalPayorId == iPayorID
                                          select new FollowUPPayorContacts
                                          {
                                              FirstName = gpc.FirstName,
                                              LastName = gpc.LastName,
                                              ConatcPerf = gpc.ContactPref,
                                              Phone = gpc.OfficePhone,
                                              Email = gpc.email,
                                              Fax = gpc.Fax,
                                              Priority = gpc.Priority,
                                              City = gpc.city,
                                              State = gpc.state,
                                              zip = gpc.ZipCode
                                          }).ToList();
                return _FollowUPPayorContacts;
            }
        }

        public static void AddUpdatePolicyIssueNotesScr(Guid ModifiedBy, Guid IssueId, string PolicyIssueNote)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.FollowupIssue _followUpIssueLst = (from c in DataModel.FollowupIssues
                                                             where c.IssueId == IssueId
                                                             select c).FirstOrDefault();
                    if (_followUpIssueLst == null)
                    {

                    }
                    else//Update
                    {

                        _followUpIssueLst.Notes = PolicyIssueNote;

                    }
                    DataModel.SaveChanges();
                }
            }
            catch
            {

            };
        }

        public static void AddUpdatePolicyIssueNotes(DisplayFollowupIssue Issue)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.FollowupIssue _followUpIssueLst = (from c in DataModel.FollowupIssues
                                                             where c.IssueId == Issue.IssueId
                                                             select c).FirstOrDefault();
                    if (_followUpIssueLst == null)
                    {

                    }
                    else//Update
                    {

                        _followUpIssueLst.Notes = Issue.PolicyIssueNote;

                    }
                    DataModel.SaveChanges();
                }
            }
            catch
            {

            };
        }
        /// <summary>
        /// Author:Ankit khandelwal
        /// CreatedOn:26-10-2018
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public static string CheckrefershButton(Guid policyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                var LastFollowuprunDate = (from c in DataModel.Policies
                                           where c.PolicyId == policyId
                                           select c.LastFollowUpRuns).FirstOrDefault();
                if (LastFollowuprunDate != null)
                {
                    string FollowuprunDate = "";
                    if (!string.IsNullOrEmpty(Convert.ToString(LastFollowuprunDate)))
                    {
                        FollowuprunDate = LastFollowuprunDate.ToString().Trim();
                    }
                    return FollowuprunDate;
                }
                else
                {
                    return "";
                }
            }
        }
        public static void AddUpdatePolicyIssuePayment(DisplayFollowupIssue Issue)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.FollowupIssue _followUpIssueLst = (from c in DataModel.FollowupIssues
                                                             where c.IssueId == Issue.IssueId
                                                             select c).FirstOrDefault();
                    if (_followUpIssueLst == null)
                    {

                    }
                    else//Update
                    {

                        _followUpIssueLst.Payment = Issue.Payment;

                    }
                    DataModel.SaveChanges();
                }
            }
            catch
            {

            };
        }

        #region IEditable<FollowupIssue> Members

        public static void AddUpdate(DisplayFollowupIssue _FollowupIssue)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.FollowupIssue _followUpIssueLst = (from c in DataModel.FollowupIssues
                                                             where c.IssueId == _FollowupIssue.IssueId
                                                             select c).FirstOrDefault();
                    if (_followUpIssueLst == null)
                    {
                        _followUpIssueLst = new DLinq.FollowupIssue();


                        if (_FollowupIssue.IssueId == null)
                        {
                            _FollowupIssue.IssueId = Guid.NewGuid();
                        }
                        else
                        {
                            _FollowupIssue.IssueId = _FollowupIssue.IssueId;
                        }
                        _followUpIssueLst.IssueId = _FollowupIssue.IssueId;//Acme fixed Jan 18, 2017 for issue - On refresh of follow up panel, issues disappered and never came back due to this missing statement 

                        _followUpIssueLst.MasterIssueCategoryReference.Value = (from f in DataModel.MasterIssueCategories where f.IssueCategoryId == _FollowupIssue.IssueCategoryID.Value select f).FirstOrDefault();//this.IssueCategoryID;
                        _followUpIssueLst.MasterIssueStatuReference.Value = (from f in DataModel.MasterIssueStatus where f.IssueStatusId == _FollowupIssue.IssueStatusId.Value select f).FirstOrDefault();
                        _followUpIssueLst.MasterIssueResultReference.Value = (from f in DataModel.MasterIssueResults where f.IssueResultId == _FollowupIssue.IssueResultId.Value select f).FirstOrDefault();
                        _followUpIssueLst.MasterIssueReasonReference.Value = (from f in DataModel.MasterIssueReasons where f.IssueReasonId == _FollowupIssue.IssueReasonId.Value select f).FirstOrDefault();
                        _followUpIssueLst.InvoiceDate = _FollowupIssue.InvoiceDate;
                        _followUpIssueLst.NextFollowUpDate = _FollowupIssue.NextFollowupDate;
                        _followUpIssueLst.Notes = _FollowupIssue.PolicyIssueNote;
                        _followUpIssueLst.Payment = _FollowupIssue.Payment;
                        _followUpIssueLst.LastModifiedDate = DateTime.Now;
                        _followUpIssueLst.CreatedDate = DateTime.Now;
                        //_followUpIssueLst.LastModifiedDate = DateTime.Today;
                        //_followUpIssueLst.CreatedDate = DateTime.Today;
                        _followUpIssueLst.ModifiedBy = _FollowupIssue.ModifiedBy;
                        _followUpIssueLst.FromDate = _FollowupIssue.FromDate;
                        _followUpIssueLst.ToDate = _FollowupIssue.ToDate;
                        //_followUpIssueLst.PolicyPaymentId = _FollowupIssue.PolicyPaymentEntryId;
                        _followUpIssueLst.PolicyId = _FollowupIssue.PolicyId;
                        _followUpIssueLst.IsDeleted = false;
                        // // _followUpIssueLst.Payor = ReferenceMaster.GetReferencedPayor(this.PayerId, DataModel);
                        DataModel.AddToFollowupIssues(_followUpIssueLst);
                    }
                    else//Update
                    {
                        //_followUpIssueLst.IssueId = _FollowupIssue.IssueId;
                        _followUpIssueLst.PreviousStatusId = _followUpIssueLst.IssueStatusId;

                        _followUpIssueLst.MasterIssueCategoryReference.Value = (from f in DataModel.MasterIssueCategories where f.IssueCategoryId == _FollowupIssue.IssueCategoryID.Value select f).FirstOrDefault();//this.IssueCategoryID;
                        _followUpIssueLst.MasterIssueStatuReference.Value = (from f in DataModel.MasterIssueStatus where f.IssueStatusId == _FollowupIssue.IssueStatusId.Value select f).FirstOrDefault();
                        _followUpIssueLst.MasterIssueResultReference.Value = (from f in DataModel.MasterIssueResults where f.IssueResultId == _FollowupIssue.IssueResultId.Value select f).FirstOrDefault();
                        _followUpIssueLst.MasterIssueReasonReference.Value = (from f in DataModel.MasterIssueReasons where f.IssueReasonId == _FollowupIssue.IssueReasonId.Value select f).FirstOrDefault();
                        _followUpIssueLst.InvoiceDate = _FollowupIssue.InvoiceDate;
                        _followUpIssueLst.NextFollowUpDate = _FollowupIssue.NextFollowupDate;
                        _followUpIssueLst.Notes = _FollowupIssue.PolicyIssueNote;
                        _followUpIssueLst.Payment = _FollowupIssue.Payment;
                        //_followUpIssueLst.LastModifiedDate = DateTime.Today;
                        _followUpIssueLst.LastModifiedDate = DateTime.Now;
                        _followUpIssueLst.ModifiedBy = _FollowupIssue.ModifiedBy;
                        //_followUpIssueLst.PolicyPaymentId = _FollowupIssue.PolicyPaymentEntryId;
                        _followUpIssueLst.PolicyId = _FollowupIssue.PolicyId;
                        _followUpIssueLst.IsDeleted = _FollowupIssue.isDeleted;
                        _followUpIssueLst.PreviousStatusId = _followUpIssueLst.IssueStatusId;
                        //Added by Acme - 01/10/2020
                        _followUpIssueLst.IsResolvedFromCommDashboard = _FollowupIssue.IsResolvedFromCommDashboard;

                    }
                    DataModel.SaveChanges();
                }
            }
            catch
            {

            }
        }

        public static void AddPaymentData(Guid IssueId, decimal Payment)
        {

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.FollowupIssue _followUpIssueLst = (from c in DataModel.FollowupIssues
                                                         where c.IssueId == IssueId
                                                         select c).FirstOrDefault();
                if (_followUpIssueLst != null && _followUpIssueLst.IssueId != Guid.Empty)
                {
                    _followUpIssueLst.Payment = Payment;
                    DataModel.SaveChanges();
                }

            }

        }

        public static void ResolveIssue(DisplayFollowupIssue _FollowupIssue)
        {

        }

        public static void UpdateIssuesSrc(DisplayFollowupIssue _DisplayFollowupIssue, int PreviousStatusId, Guid ModifiedBy)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.FollowupIssue _followUpIssueLst = (from c in DataModel.FollowupIssues
                                                             where c.IssueId == _DisplayFollowupIssue.IssueId
                                                             select c).FirstOrDefault();
                    if (_followUpIssueLst == null)
                    {
                        //_followUpIssueLst = new DLinq.FollowupIssue();


                        //_followUpIssueLst.IssueId = Guid.NewGuid();


                        //_followUpIssueLst.InvoiceDate = this.InvoiceDate;

                        //_followUpIssueLst.NextFollowUpDate = this.NextFollowupDate;


                        //_followUpIssueLst.Notes = this.PolicyIssueNote;
                        //_followUpIssueLst.Payment = this.Payment;

                        //_followUpIssueLst.LastModifiedDate = DateTime.Today;
                        //_followUpIssueLst.ModifiedBy = _followUpIssueLst.ModifiedBy;

                        //_followUpIssueLst.FromDate = this.FromDate;
                        //_followUpIssueLst.ToDate = this.ToDate;

                        //_followUpIssueLst.PolicyId = this.PolicyId;


                        //DataModel.AddToFollowupIssues(_followUpIssueLst);
                    }
                    else//Update
                    {
                        _followUpIssueLst.PreviousStatusId = PreviousStatusId;
                        _followUpIssueLst.NextFollowUpDate = _DisplayFollowupIssue.NextFollowupDate;
                        _followUpIssueLst.IssueCategoryId = _DisplayFollowupIssue.IssueCategoryID.Value;
                        _followUpIssueLst.IssueStatusId = _DisplayFollowupIssue.IssueStatusId.Value;
                        _followUpIssueLst.IssueResultId = _DisplayFollowupIssue.IssueResultId.Value;
                        _followUpIssueLst.IssueReasonId = _DisplayFollowupIssue.IssueReasonId.Value;
                        //_followUpIssueLst.LastModifiedDate = DateTime.Today;
                        _followUpIssueLst.LastModifiedDate = DateTime.Now;
                        _followUpIssueLst.ModifiedBy = _followUpIssueLst.ModifiedBy;
                        _followUpIssueLst.PolicyId = _DisplayFollowupIssue.PolicyId;


                    }
                    DataModel.SaveChanges();
                }
            }
            catch
            {

            };
        }

        public static void Delete(Guid IssueID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.FollowupIssue _followUpObj = (from c in DataModel.FollowupIssues//
                                                    where (c.IssueId == IssueID && c.IssueResultId != 3)
                                                    select c).FirstOrDefault();
                if (_followUpObj != null)//
                {
                    DataModel.DeleteObject(_followUpObj);   // _followUpObj.IsDeleted = true;
                    DataModel.SaveChanges();
                }
            }
        }

        public static void DeleteIssue(Guid IssueID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.FollowupIssue _followUpObj = (from c in DataModel.FollowupIssues
                                                    where (c.IssueId == IssueID)
                                                    select c).FirstOrDefault();
                if (_followUpObj != null)
                {
                    DataModel.DeleteObject(_followUpObj);   // _followUpObj.IsDeleted = true;
                    DataModel.SaveChanges();
                }
            }
        }
        //Added by vinod 
        //Remove issue which is not not deleted
        public static void DeleteFollowupIssueExceptCloseIssue(Guid IssueID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.FollowupIssue _followUpObj = (from c in DataModel.FollowupIssues
                                                    where (c.IssueId == IssueID && (c.IsDeleted == null || c.IsDeleted == false))
                                                    select c).FirstOrDefault();
                if (_followUpObj != null)
                {
                    DataModel.DeleteObject(_followUpObj);   // _followUpObj.IsDeleted = true;
                    DataModel.SaveChanges();
                }
            }
        }

        public void RemoveCommisionIssue(Guid IssueID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.FollowupIssue _followUpObj = (from c in DataModel.FollowupIssues
                                                    where (c.IssueId == IssueID)
                                                    select c).FirstOrDefault();
                if (_followUpObj != null)
                {
                    //DataModel.DeleteObject(_followUpObj);   
                    _followUpObj.IsDeleted = true;
                    _followUpObj.IsRemovedFromCommDashboard = true;
                    DataModel.SaveChanges();
                }
            }
        }

        public static void DeleteFollowupByPolicyId(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.FollowupIssue> _followUpObj = (from c in DataModel.FollowupIssues//
                                                          where (c.PolicyId == PolicyId)
                                                          select c).ToList<DLinq.FollowupIssue>();

                if (_followUpObj != null)//
                {
                    foreach (DLinq.FollowupIssue _FollowupIssue in _followUpObj)
                    {
                        DataModel.DeleteObject(_FollowupIssue);
                        // _followUpObj.IsDeleted = true;
                        DataModel.SaveChanges();
                    }
                }
            }
        }
        //public static void DeleteFollowUpbyPolicyPaymentEntryId(Guid PolicyPaymentEntryId)
        public static void DeletePolicyPayment_And_FollowUpbyPolicyPaymentEntryId(Guid PolicyPaymentEntryId, PostEntryProcess _ActualPostEntryStatus)
        {
            try
            {
                Guid? FollowUpVarIssueId = PolicyPaymentEntriesPost.GetPolicyPaymentEntry(PolicyPaymentEntryId).FollowUpVarIssueId;

                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    List<DLinq.PolicyPaymentEntry> _PolicyPaymentEntriesPostLst = DataModel.PolicyPaymentEntries.Where(p => p.FollowUpVarIssueId == FollowUpVarIssueId).ToList();
                    DLinq.PolicyPaymentEntry PolicyPaymentEntryrecord = DataModel.PolicyPaymentEntries.Where(p => p.PaymentEntryId == PolicyPaymentEntryId).FirstOrDefault();

                    if (PolicyPaymentEntryrecord != null)
                    {
                        DataModel.DeleteObject(PolicyPaymentEntryrecord);
                        DataModel.SaveChanges();
                    }

                    if (_PolicyPaymentEntriesPostLst != null && _PolicyPaymentEntriesPostLst.Count == 1 && _ActualPostEntryStatus != PostEntryProcess.RePost)
                    {
                        DLinq.FollowupIssue FollowupIssueRecord = DataModel.FollowupIssues.Where(p => p.IssueId == FollowUpVarIssueId).FirstOrDefault();
                        if (FollowupIssueRecord != null)
                        {
                            DataModel.DeleteObject(FollowupIssueRecord);
                            DataModel.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeletePolicyPayment_And_FollowUpbyPolicyPaymentEntryId :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("DeletePolicyPayment_And_FollowUpbyPolicyPaymentEntryId :" + ex.InnerException.ToString(), true);
             //  throw ex;
            }

        }

        public FollowupIssue GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        public void EmailToAgencyPayor(MailData _MailData)
        {
            _MailData.CommDeptFaxNumber = Masters.SystemConstant.GetKeyValue("CommDeptFax");
            _MailData.CommDeptMail = Masters.SystemConstant.GetKeyValue("CommDeptMailId");
            _MailData.CommDeptPhoneNumber = Masters.SystemConstant.GetKeyValue("CommDeptPhoneNumber");
            _MailData.FromMail = Masters.SystemConstant.GetKeyValue("MailServerEMail");
            _MailData.HostName = Masters.SystemConstant.GetKeyValue("MailServerName");
            _MailData.Port = Masters.SystemConstant.GetKeyValue("MailServerPortNo");
            if (_MailData.ToMail == "FaxAgency")
                _MailData.ToMail = Masters.SystemConstant.GetKeyValue("FaxAgency");
            else if (_MailData.ToMail == "FaxPayor")
                _MailData.ToMail = Masters.SystemConstant.GetKeyValue("FaxPayor");
            _MailData.MailLogoPath = Masters.SystemConstant.GetKeyValue("MailLogoPath");
            _MailData.Password = Masters.SystemConstant.GetKeyValue("MailServerPassword");
            _MailData.UserName = Masters.SystemConstant.GetKeyValue("MailServerUserName");
            new EmailFax.OutLookEmailFax(_MailData).SendEmailWithAttachment();
        }

        public void SendCloseStatemant(MailData _MailData, string strMailBody)
        {
            _MailData.CommDeptMail = Masters.SystemConstant.GetKeyValue("CommDeptMailId");
            _MailData.FromMail = Masters.SystemConstant.GetKeyValue("MailServerEMail");
            _MailData.HostName = Masters.SystemConstant.GetKeyValue("MailServerName");
            _MailData.Port = Masters.SystemConstant.GetKeyValue("MailServerPortNo");
            _MailData.Password = Masters.SystemConstant.GetKeyValue("MailServerPassword");
            _MailData.UserName = Masters.SystemConstant.GetKeyValue("MailServerUserName");
            new EmailFax.OutLookEmailFax(_MailData).OutLookLaunch(_MailData, strMailBody);
        }

        public void SendMailToCloseBatch(MailData _MailData, string strBatchNumber, string strMailBody)
        {
            _MailData.CommDeptMail = Masters.SystemConstant.GetKeyValue("CommDeptMailId");
            _MailData.FromMail = Masters.SystemConstant.GetKeyValue("MailServerEMail");
            _MailData.HostName = Masters.SystemConstant.GetKeyValue("MailServerName");
            _MailData.Port = Masters.SystemConstant.GetKeyValue("MailServerPortNo");
            _MailData.Password = Masters.SystemConstant.GetKeyValue("MailServerPassword");
            _MailData.UserName = Masters.SystemConstant.GetKeyValue("MailServerUserName");
            new EmailFax.OutLookEmailFax(_MailData).SendMailToCloseBatch(_MailData, strBatchNumber, strMailBody);
        }

        public void SendMailOfCarrierProduct(MailData _MailData, string strMailBody)
        {
            _MailData.CommDeptMail = Masters.SystemConstant.GetKeyValue("CommDeptMailId");
            _MailData.HostName = Masters.SystemConstant.GetKeyValue("MailServerName");
            _MailData.Port = Masters.SystemConstant.GetKeyValue("MailServerPortNo");
            _MailData.Password = Masters.SystemConstant.GetKeyValue("MailServerPassword");
            _MailData.UserName = Masters.SystemConstant.GetKeyValue("MailServerUserName");
            new EmailFax.OutLookEmailFax(_MailData).SendMailToServiceDepartment(_MailData, strMailBody);
        }

        public void SendMailToUpload(MailData _MailData, string strMailBody)
        {
            _MailData.CommDeptMail = Masters.SystemConstant.GetKeyValue("CommDeptMailId");
            _MailData.HostName = Masters.SystemConstant.GetKeyValue("MailServerName");
            _MailData.Port = Masters.SystemConstant.GetKeyValue("MailServerPortNo");
            _MailData.Password = Masters.SystemConstant.GetKeyValue("MailServerPassword");
            _MailData.UserName = Masters.SystemConstant.GetKeyValue("MailServerUserName");
            new EmailFax.OutLookEmailFax(_MailData).SendMailToUpload(_MailData, strMailBody);
        }

        public void SendRemainderMail(MailData _MailData, string strMailBody, DateTime dtCutOfDate)
        {
            _MailData.CommDeptMail = Masters.SystemConstant.GetKeyValue("CommDeptMailId");
            _MailData.HostName = Masters.SystemConstant.GetKeyValue("MailServerName");
            _MailData.Port = Masters.SystemConstant.GetKeyValue("MailServerPortNo");
            _MailData.Password = Masters.SystemConstant.GetKeyValue("MailServerPassword");
            _MailData.UserName = Masters.SystemConstant.GetKeyValue("MailServerUserName");
            new EmailFax.OutLookEmailFax(_MailData).SendRemaiderMail(_MailData, strMailBody, dtCutOfDate);
        }

        public void SendLinkedPolicyConfirmationMail(MailData _MailData, string strMailBody, string strPendingPolicy, string strActivePolicy)
        {
            _MailData.CommDeptMail = Masters.SystemConstant.GetKeyValue("CommDeptMailId");
            _MailData.HostName = Masters.SystemConstant.GetKeyValue("MailServerName");
            _MailData.Port = Masters.SystemConstant.GetKeyValue("MailServerPortNo");
            _MailData.Password = Masters.SystemConstant.GetKeyValue("MailServerPassword");
            _MailData.UserName = Masters.SystemConstant.GetKeyValue("MailServerUserName");
            new EmailFax.OutLookEmailFax(_MailData).SendLinkedPolicyConfirmationMail(_MailData, strMailBody, strPendingPolicy, strActivePolicy);
        }

        public void SendLoginLogoutMail(MailData _MailData, string strLoginLogoutType, string strMailBody)
        {
            _MailData.CommDeptMail = Masters.SystemConstant.GetKeyValue("CommDeptMailId");
            _MailData.HostName = Masters.SystemConstant.GetKeyValue("MailServerName");
            _MailData.Port = Masters.SystemConstant.GetKeyValue("MailServerPortNo");
            _MailData.Password = Masters.SystemConstant.GetKeyValue("MailServerPassword");
            _MailData.UserName = Masters.SystemConstant.GetKeyValue("MailServerUserName");
            new EmailFax.OutLookEmailFax(_MailData).SendLoginLogoutMail(_MailData, strLoginLogoutType, strMailBody);
        }

        public void SendNotificationMail(MailData _MailData, string strSubject, string strMailBody)
        {
            _MailData.CommDeptMail = Masters.SystemConstant.GetKeyValue("CommDeptMailId");
            _MailData.HostName = Masters.SystemConstant.GetKeyValue("MailServerName");
            _MailData.Port = Masters.SystemConstant.GetKeyValue("MailServerPortNo");
            _MailData.Password = Masters.SystemConstant.GetKeyValue("MailServerPassword");
            _MailData.UserName = Masters.SystemConstant.GetKeyValue("MailServerUserName");
            new EmailFax.OutLookEmailFax(_MailData).SendNotificationMail(_MailData, strSubject, strMailBody);
        }

        #endregion

        void IEditable<DisplayFollowupIssue>.AddUpdate()
        {
            //throw new NotImplementedException();
        }

        void IEditable<DisplayFollowupIssue>.Delete()
        {
            //throw new NotImplementedException();
        }

        DisplayFollowupIssue IEditable<DisplayFollowupIssue>.GetOfID()
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}
