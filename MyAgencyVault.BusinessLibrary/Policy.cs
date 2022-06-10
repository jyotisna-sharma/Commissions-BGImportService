using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Transactions;
using System.Data;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;
using System.Globalization;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
namespace MyAgencyVault.BusinessLibrary
{




    [DataContract]
    public class PolicySavedStatus
    {
        [DataMember]
        public bool IsError { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public class PayorObject
    {
        [DataMember]
        public Guid PayorID { get; set; }
        [DataMember]
        public string PayorName { get; set; }
        [DataMember]
        public string NickName { get; set; }
    }

    [DataContract]
    public class PayorCarrierObject
    {
        [DataMember]
        public Guid PayorID { get; set; }
        [DataMember]
        public List<CarrierIDName> Carriers { get; set; }
    }

    [DataContract]
    public class CarrierObject
    {
        [DataMember]
        public Guid PayorID { get; set; }
        [DataMember]
        public Guid CarrierID { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
    }

    [DataContract]
    public class CarrierIDName
    {
        [DataMember]
        public Guid CarrierID { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string CarrierNickName { get; set; }
    }
    public class Benefits_UserMsg
    {
        [DataMember]
        public string BGUserId { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
    public class Benefits_UserResponseStatus
    {
        [DataMember]
        public int NewCount { get; set; }
        [DataMember]
        public int UpdateCount { get; set; }
        [DataMember]
        public int ErrorCount { get; set; }

        [DataMember]
        public List<Benefits_UserMsg> ErrorUserList { get; set; }

        //[DataMember]
        //public List<Benefits_UserMsg> ErrorUserList { get; set; }
    }

    [DataContract]
    public class PolicyImportStatus
    {
        [DataMember]
        public int ImportCount { get; set; }
        [DataMember]
        public int UpdateCount { get; set; }
        [DataMember]
        public int ErrorCount { get; set; }
        [DataMember]
        public List<string> ErrorList { get; set; }
    }

    [DataContract]
    public class Benefits_PolicyImportStatus
    {
        [DataMember]
        public int NewCount { get; set; }
        [DataMember]
        public int UpdateCount { get; set; }
        [DataMember]
        public int ErrorCount { get; set; }
        [DataMember]
        public List<Benefits_ErrorMsg> ErrorList { get; set; }
        [DataMember]
        public List<Benefits_PolicyID> SuccessList { get; set; }
    }

    [DataContract]
    public class Caravus_PolicyImportStatus
    {
        [DataMember]
        public int NewCount { get; set; }
        [DataMember]
        public int UpdateCount { get; set; }
        [DataMember]
        public int ErrorCount { get; set; }
        [DataMember]
        public List<Benefits_ErrorMsg> ErrorList { get; set; }
        [DataMember]
        public List<Benefits_PolicyID> IDList { get; set; }

    }

    [DataContract]
    public class Benefits_PolicyID
    {
        [DataMember]
        public string OriginalPlanID { get; set; }
        [DataMember]
        public string PolicyPlanID { get; set; }
        [DataMember]
        public string CDPolicyID { get; set; }
    }

    [DataContract]
    public class Benefits_ErrorMsg
    {
        [DataMember]
        public string OriginalPlanID { get; set; }
        //[DataMember]
        //public string CDPolicyID { get; set; }
        [DataMember]
        public string PolicyPlanID { get; set; }
        [DataMember]
        public string Message { get; set; }


        public Benefits_ErrorMsg(string origID, string policyID, string msg)
        {
            OriginalPlanID = origID;
            PolicyPlanID = policyID;
            Message = msg;
            // CDPolicyID = cdPolicyID;
        }

        [DataContract]
        public class errMsg
        {
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public string Val { get; set; }

            public errMsg(string n, string v)
            {
                Name = n;
                Val = v;
            }
        }
    }
    public class GetAgentList
    {
        public string ProducerName { get; set; }
        public string ProducerId { get; set; }
        public string ProducerFirstYear { get; set; }
        public string ProducerRenewal { get; set; }
        public string importedPolicyID { get; set; }
        public bool isNewPolicy { get; set; }
        public string benefits_policyID { get; set; }

        public string ProducerExecutionId { get; set; }
        public string ProducerExecutionName { get; set; }
    }
    //new added
    public class GetPolicyList
    {
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public int PolicyStatusId { get; set; }
        [DataMember]
        public string PolicyType { get; set; }
        [DataMember]
        public Guid PolicyClientId { get; set; }
        [DataMember]
        public Guid PolicyLicenseeId { get; set; }
        [DataMember]
        public DateTime OriginalEffectiveDate { get; set; }
        [DataMember]
        public DateTime TrackFromDate { get; set; }
        [DataMember]
        public int PolicyModeId { get; set; }
        [DataMember]
        public decimal MonthlyPremium { get; set; }
        [DataMember]
        public Guid CoverageId { get; set; }
        [DataMember]
        public string SubmittedThrough { get; set; }
        [DataMember]
        public string Enrolled { get; set; }
        [DataMember]
        public string Eligible { get; set; }
        [DataMember]
        public DateTime PolicyTerminationDate { get; set; }
        [DataMember]
        public int TerminationReasonId { get; set; }
        [DataMember]
        public Boolean IsTrackMissingMonth { get; set; }
        [DataMember]
        public Boolean IsTrackIncomingPercentage { get; set; }
        [DataMember]
        public Boolean IsTrackPayment { get; set; }
        [DataMember]
        public int IncomingPaymentTypeId { get; set; }
        [DataMember]
        public Boolean IsDeleted { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Boolean IsIncomingBasicSchedule { get; set; }
        [DataMember]
        public Boolean IsOutGoingBasicSchedule { get; set; }
        [DataMember]
        public double SplitPercentage { get; set; }
        [DataMember]
        public string Insured { get; set; }
        [DataMember]
        public DateTime ActivatedOn { get; set; }
        [DataMember]
        public Boolean IsLocked { get; set; }
        [DataMember]
        public DateTime LastFollowUpRuns { get; set; }
        [DataMember]
        public Boolean IsManuallyChanged { get; set; }
        [DataMember]
        public byte RowVersion { get; set; }
        [DataMember]
        public int Advance { get; set; }
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public string AccoutExec { get; set; }
        [DataMember]
        public DateTime LastNoMissIssueDate { get; set; }
        [DataMember]
        public DateTime LastNoVarIssueDate { get; set; }
        [DataMember]
        public Boolean IsCustomBasicSchedule { get; set; }
        [DataMember]
        public string CustomScheduleDateType { get; set; }
        [DataMember]
        public Boolean IsTieredSchedule { get; set; }
        [DataMember]
        public DateTime LastModifiedOn { get; set; }
        [DataMember]
        public Boolean IsCreatedFromWeb { get; set; }
        [DataMember]
        public Boolean IslastModifiedFromWeb { get; set; }
        [DataMember]
        public string Segment { get; set; }
        [DataMember]
        public decimal ExpectedCommissions { get; set; }
        [DataMember]
        public string CommissionType { get; set; }
        [DataMember]
        public Guid Agent { get; set; }
        [DataMember]
        public string importedPolicyID { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string PolicyPlanID { get; set; }
        [DataMember]
        public string importedID { get; set; }
        [DataMember]
        public string Payor { get; set; }
        [DataMember]
        public string carrier { get; set; }
        [DataMember]
        public string Client { get; set; }
        [DataMember]
        public string Product { get; set; }
    }

    public class Policy
    {

        #region Import Policy
        public static string GetPolicyIdKeyForImport()
        {
            return System.Configuration.ConfigurationSettings.AppSettings["PolicyIDKeyName"];
        }

        public static bool IsPolicyExistingWithPolicyID_New(string PolicyID, string agencyName = "")//find1
        {
            //Guid policyID = Guid.Empty;
            ActionLogger.Logger.WriteImportPolicyLog("IsPolicyExistingWithPolicyID_New: " + PolicyID, true, agencyName);
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Select policyID from policies where policyID ='" + PolicyID + "'", con))
                    {
                        cmd.Parameters.AddWithValue("@PolicyId", PolicyID);
                        con.Open();

                        object obj = cmd.ExecuteScalar();
                        if (obj != null)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("IsPolicyExistingWithPolicyID_New TRUE: " + PolicyID, true, agencyName);
                            return true;
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("IsPolicyExistingWithPolicyID_New FALSE: " + PolicyID, true, agencyName);
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("IsPolicyExistingWithPolicyID Exception for : " + PolicyID + ", msg: " + ex.Message, true, agencyName);
                return false;
            }
        }

        public static bool IsPolicyExistingWithPolicyID(string PolicyID)
        {
            //Guid policyID = Guid.Empty;
            ActionLogger.Logger.WriteImportPolicyLog("policy: " + PolicyID, true);
            try
            {
                Guid policy = new Guid(PolicyID.ToUpper());
                ActionLogger.Logger.WriteImportPolicyLog("policy: " + policy, true);
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _policy = (from p in DataModel.Policies
                                   where ((Guid)(p.PolicyId) == policy && p.IsDeleted == false)
                                   select p).FirstOrDefault();
                    if (_policy != null)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("IsPolicyExistingWithPolicyID true for : " + PolicyID, true);
                        return true;
                    }
                }
                ActionLogger.Logger.WriteImportPolicyLog("IsPolicyExistingWithPolicyID FALSE for : " + PolicyID, true);

                return false;
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("IsPolicyExistingWithPolicyID Exception for : " + PolicyID + ", msg: " + ex.Message, true);
                return false;
            }
        }
        public static Guid IsPolicyExistingWithImportID(string ImportID, Guid LicID, string agencyName = "")
        {
            //Guid policyID = Guid.Empty;
            Guid policy = Guid.Empty;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.PolicyLearnedFields
                               join o in DataModel.Policies on p.PolicyId equals o.PolicyId
                               where (p.ImportPolicyID == ImportID && o.PolicyLicenseeId == LicID && o.IsDeleted == false)
                               select p).FirstOrDefault();
                if (_policy != null)
                {
                    policy = _policy.PolicyId;
                }
            }
            return policy;
        }

        static int PolicCompType(string strCompType, ObservableCollection<CompType> CompTypeTypeLst, string agencyName = "")
        {
            int intStatus = 1; //Changed by acme as per Kevin's advise to keep "Commissions" as default option.

            try
            {
                if (string.IsNullOrEmpty(strCompType))
                {
                    //Default Pending
                    return intStatus = 1;
                }
                CompType objComp = CompTypeTypeLst.Where(p => p.Names.ToLower() == strCompType.ToLower()).FirstOrDefault();
                if (objComp != null)
                {
                    if (objComp.IncomingPaymentTypeID != null)
                    {
                        intStatus = Convert.ToInt32(objComp.IncomingPaymentTypeID);
                    }
                }
                else
                {
                    objComp = CompTypeTypeLst.Where(p => p.PaymentTypeName.ToLower() == strCompType.ToLower()).FirstOrDefault();

                    if (objComp != null)
                    {
                        if (objComp.IncomingPaymentTypeID != null)
                        {
                            intStatus = Convert.ToInt32(objComp.IncomingPaymentTypeID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("PolicCompType Exception ex: " + ex.Message, true, agencyName);
            }
            return intStatus;
        }

        public static void InsertIntoMembers(DataTable dataTable)//find1
        {
            using (SqlConnection connection = new SqlConnection(DBConnection.GetConnectionString()))
            {
                SqlTransaction transaction = null;
                connection.Open();
                try
                {
                    transaction = connection.BeginTransaction();

                    using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                    {
                        sqlBulkCopy.DestinationTableName = "Policies";
                        sqlBulkCopy.ColumnMappings.Add("PolicyId", "PolicyId");
                        sqlBulkCopy.ColumnMappings.Add("PolicyNumber", "PolicyNumber");
                        sqlBulkCopy.ColumnMappings.Add("PolicyStatusId", "PolicyStatusId");
                        sqlBulkCopy.ColumnMappings.Add("PolicyType", "PolicyType");
                        sqlBulkCopy.ColumnMappings.Add("PolicyClientId", "PolicyClientId");

                        sqlBulkCopy.ColumnMappings.Add("PolicyLicenseeId", "PolicyLicenseeId");
                        sqlBulkCopy.ColumnMappings.Add("OriginalEffectiveDate", "OriginalEffectiveDate");
                        sqlBulkCopy.ColumnMappings.Add("TrackFromDate", "TrackFromDate");
                        sqlBulkCopy.ColumnMappings.Add("PolicyModeId", "PolicyModeId");
                        sqlBulkCopy.ColumnMappings.Add("MonthlyPremium", "MonthlyPremium");

                        sqlBulkCopy.ColumnMappings.Add("CoverageId", "CoverageId");
                        sqlBulkCopy.ColumnMappings.Add("SubmittedThrough", "SubmittedThrough");

                        sqlBulkCopy.ColumnMappings.Add("Enrolled", "Enrolled");
                        sqlBulkCopy.ColumnMappings.Add("Eligible", "Eligible");
                        sqlBulkCopy.ColumnMappings.Add("PolicyTerminationDate", "PolicyTerminationDate");
                        sqlBulkCopy.ColumnMappings.Add("TerminationReasonId", "TerminationReasonId");
                        sqlBulkCopy.ColumnMappings.Add("IsTrackMissingMonth", "IsTrackMissingMonth");
                        sqlBulkCopy.ColumnMappings.Add("IsTrackIncomingPercentage", "IsTrackIncomingPercentage");
                        sqlBulkCopy.ColumnMappings.Add("IsTrackPayment", "IsTrackPayment");
                        sqlBulkCopy.ColumnMappings.Add("IncomingPaymentTypeId", "IncomingPaymentTypeId");

                        sqlBulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");
                        sqlBulkCopy.ColumnMappings.Add("ReplacedBy", "ReplacedBy");
                        sqlBulkCopy.ColumnMappings.Add("PayorId", "PayorId");
                        sqlBulkCopy.ColumnMappings.Add("DuplicateFrom", "DuplicateFrom");
                        sqlBulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                        sqlBulkCopy.ColumnMappings.Add("CreatedOn", "CreatedOn");
                        sqlBulkCopy.ColumnMappings.Add("IsIncomingBasicSchedule", "IsIncomingBasicSchedule");
                        sqlBulkCopy.ColumnMappings.Add("IsOutGoingBasicSchedule", "IsOutGoingBasicSchedule");
                        sqlBulkCopy.ColumnMappings.Add("CarrierId", "CarrierId");

                        sqlBulkCopy.ColumnMappings.Add("SplitPercentage", "SplitPercentage");
                        sqlBulkCopy.ColumnMappings.Add("Insured", "Insured");
                        sqlBulkCopy.ColumnMappings.Add("ActivatedOn", "ActivatedOn");
                        sqlBulkCopy.ColumnMappings.Add("IsLocked", "IsLocked");
                        sqlBulkCopy.ColumnMappings.Add("LastFollowUpRuns", "LastFollowUpRuns");
                        sqlBulkCopy.ColumnMappings.Add("RowVersion", "RowVersion");
                        sqlBulkCopy.ColumnMappings.Add("Advance", "Advance");
                        sqlBulkCopy.ColumnMappings.Add("ProductType", "ProductType");
                        sqlBulkCopy.ColumnMappings.Add("UserCredentialId", "UserCredentialId");
                        sqlBulkCopy.ColumnMappings.Add("AccoutExec", "AccoutExec");

                        sqlBulkCopy.ColumnMappings.Add("LastNoVarIssueDate", "LastNoVarIssueDate");
                        sqlBulkCopy.ColumnMappings.Add("LastNoMissIssueDate", "LastNoMissIssueDate");

                        sqlBulkCopy.WriteToServer(dataTable);
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

            }
        }

        public static PolicyImportStatus ImportPolicy_faster(DataTable dt, ObservableCollection<User> GlobalAgentList, Guid LicID, ObservableCollection<CompType> CompTypeList)
        {
            #region Variables
            PolicyImportStatus objStatus = new PolicyImportStatus();
            string policyIDKey = System.Configuration.ConfigurationSettings.AppSettings["PolicyIDKeyName"];
            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyIDKey: " + policyIDKey, true);
            int addCount = 0;
            int updateCount = 0;
            int errorCount = 0;
            List<string> errorList = new List<string>();


            List<OutGoingPayment> OutGoingField = new List<OutGoingPayment>();
            PolicyToolIncommingShedule inSchedule = null;
            string strProductType = string.Empty;
            string covNickName = string.Empty;
            Guid houseOwner = Guid.Empty;

            #endregion
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Data model init", true);
                var AgentList = (from p in DataModel.UserCredentials
                                 join o in DataModel.UserDetails on p.UserCredentialId equals o.UserCredentialId
                                 where p.LicenseeId == LicID /*&& p.RoleId == 3*/ && p.IsDeleted == false
                                 select new
                                 {
                                     p.UserCredentialId,
                                     o.NickName,
                                     p.UserName,
                                     p.RoleId,
                                     o.FirstName,
                                     o.LastName
                                 }).ToList();
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Agent list fetched", true);

                #region temp policies table
                DataTable dtPolicies = new DataTable("tempPolicies");

                dtPolicies.Columns.Add("PolicyId", typeof(Guid));
                dtPolicies.Columns.Add("PolicyNumber", typeof(string));
                dtPolicies.Columns.Add("PolicyStatusId", typeof(int));
                dtPolicies.Columns.Add("PolicyType", typeof(string));
                dtPolicies.Columns.Add("PolicyClientId", typeof(Guid));
                dtPolicies.Columns.Add("PolicyLicenseeId", typeof(Guid));
                dtPolicies.Columns.Add("OriginalEffectiveDate", typeof(DateTime));
                dtPolicies.Columns.Add("TrackFromDate", typeof(DateTime));
                dtPolicies.Columns.Add("PolicyModeId", typeof(int));

                dtPolicies.Columns.Add("MonthlyPremium", typeof(double));
                dtPolicies.Columns.Add("CoverageId", typeof(Guid));
                dtPolicies.Columns.Add("Submittedthrough", typeof(string));
                dtPolicies.Columns.Add("Enrolled", typeof(string));
                dtPolicies.Columns.Add("Eligible", typeof(string));
                dtPolicies.Columns.Add("PolicyTerminationDate", typeof(DateTime));
                dtPolicies.Columns.Add("TerminationReasonId", typeof(int));
                dtPolicies.Columns.Add("IsTrackMissingMonth", typeof(bool));
                dtPolicies.Columns.Add("IsTrackIncomingPercentage", typeof(bool));
                dtPolicies.Columns.Add("IsTrackPayment", typeof(bool));

                dtPolicies.Columns.Add("IncomingPaymentTypeId", typeof(int));
                dtPolicies.Columns.Add("IsDeleted", typeof(bool));
                dtPolicies.Columns.Add("ReplacedBy", typeof(Guid));
                dtPolicies.Columns.Add("PayorId", typeof(Guid));
                dtPolicies.Columns.Add("DuplicateFrom", typeof(Guid));
                dtPolicies.Columns.Add("CreatedBy", typeof(Guid));
                dtPolicies.Columns.Add("CreatedOn", typeof(DateTime));
                dtPolicies.Columns.Add("IsIncomingBasicSchedule", typeof(bool));
                dtPolicies.Columns.Add("IsOutGoingBasicSchedule", typeof(bool));
                dtPolicies.Columns.Add("CarrierId", typeof(Guid));

                dtPolicies.Columns.Add("SplitPercentage", typeof(decimal));
                dtPolicies.Columns.Add("Insured", typeof(string));
                dtPolicies.Columns.Add("ActivatedOn", typeof(DateTime));
                dtPolicies.Columns.Add("IsLocked", typeof(bool));
                dtPolicies.Columns.Add("LastFollowUpRuns", typeof(DateTime));
                dtPolicies.Columns.Add("RowVersion", typeof(byte[]));
                dtPolicies.Columns.Add("Advance", typeof(int));
                dtPolicies.Columns.Add("ProductType", typeof(string));
                dtPolicies.Columns.Add("UserCredentialId", typeof(Guid));
                dtPolicies.Columns.Add("AccoutExec", typeof(string));

                dtPolicies.Columns.Add("LastNoVarIssueDate", typeof(DateTime));
                dtPolicies.Columns.Add("LastNoMissIssueDate", typeof(DateTime));

                #endregion

                #region Temp policy learnedfields

                DataTable dtLearned = new DataTable("tempLearnedFields");
                dtLearned.Columns.Add("PolicyId", typeof(Guid));
                dtLearned.Columns.Add("Insured", typeof(string));
                dtLearned.Columns.Add("PolicyNumber", typeof(string));
                dtLearned.Columns.Add("Effective", typeof(DateTime));
                dtLearned.Columns.Add("TrackFrom", typeof(DateTime));
                dtLearned.Columns.Add("PAC", typeof(string));
                dtLearned.Columns.Add("PMC", typeof(string));

                dtLearned.Columns.Add("ModalAvgPremium", typeof(decimal));
                dtLearned.Columns.Add("PolicyModeId", typeof(int));
                dtLearned.Columns.Add("Enrolled", typeof(string));
                dtLearned.Columns.Add("Eligible", typeof(string));
                dtLearned.Columns.Add("AutoTerminationDate", typeof(DateTime));
                dtLearned.Columns.Add("Link1", typeof(string));
                dtLearned.Columns.Add("LastModifiedOn", typeof(DateTime));
                dtLearned.Columns.Add("LastModifiedUserCredentialid", typeof(Guid));
                dtLearned.Columns.Add("ClientID", typeof(Guid));
                dtLearned.Columns.Add("CompTypeID", typeof(int));

                dtLearned.Columns.Add("PayorSysID", typeof(string));
                dtLearned.Columns.Add("Renewal", typeof(string));
                dtLearned.Columns.Add("SplitPercentage", typeof(decimal));
                dtLearned.Columns.Add("CompScheduleType", typeof(string));

                dtLearned.Columns.Add("CarrierId", typeof(Guid));
                dtLearned.Columns.Add("CoverageId", typeof(Guid));
                dtLearned.Columns.Add("PayorId", typeof(Guid));

                dtLearned.Columns.Add("PreviousEffectiveDate", typeof(DateTime));
                dtLearned.Columns.Add("PreviousPolicyModeId", typeof(int));
                dtLearned.Columns.Add("CarrierNickName", typeof(string));
                dtLearned.Columns.Add("CoverageNickName", typeof(string));

                dtLearned.Columns.Add("Advance", typeof(int));
                dt.Columns.Add("ProductType", typeof(string));
                dtLearned.Columns.Add("UserCredentialId", typeof(Guid));

                dt.Columns.Add("AccoutExec", typeof(string));
                dt.Columns.Add("ImportPolicyID", typeof(string));

                #endregion



                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow drPolicy = dtPolicies.NewRow();
                    dtPolicies.Rows.Add(drPolicy);

                    DLinq.Policy objPolicy = new DLinq.Policy();
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Iteration: " + i, true);

                    string importedPolicyID = dt.Columns.Contains(policyIDKey) ? Convert.ToString(dt.Rows[i][policyIDKey]).Trim() : (dt.Columns.Contains("Life Insurance Plan ID") ? Convert.ToString(dt.Rows[i]["Life Insurance Plan ID"]).Trim() : "");
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: importedPolicyID: " + importedPolicyID, true);
                    OutGoingField = new List<OutGoingPayment>();
                    try
                    {
                        bool isNewPolicy = false;
                        Guid policyID = IsPolicyExistingWithImportID(importedPolicyID, LicID);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyID: " + policyID, true);
                        if (policyID != Guid.Empty)
                        {
                            objPolicy = (from p in DataModel.Policies where p.PolicyId == policyID select p).FirstOrDefault();
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Existing policy", true);


                            //check if client exists or deleted in the system
                            //if deleted, then add the given as new 
                            if (objPolicy.Client != null && Convert.ToBoolean(objPolicy.Client.IsDeleted))
                            {
                                objPolicy.PolicyClientId = null;
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Client found deleted , so setting as null ", true);
                            }
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: New policy", true);
                            isNewPolicy = true;
                            objPolicy.PolicyId = Guid.NewGuid();
                            drPolicy["PolicyId"] = objPolicy.PolicyId;

                            objPolicy.IsTrackPayment = true;
                            drPolicy["IsTrackPayment"] = objPolicy.IsTrackPayment;

                            objPolicy.PolicyLicenseeId = LicID;
                            drPolicy["PolicyLicenseeId"] = objPolicy.PolicyLicenseeId;

                            objPolicy.TerminationReasonId = null;
                            drPolicy["TerminationReasonId"] = objPolicy.TerminationReasonId;

                            objPolicy.IsTrackMissingMonth = true;
                            drPolicy["IsTrackMissingMonth"] = objPolicy.IsTrackMissingMonth;

                            objPolicy.CreatedOn = DateTime.Today;
                            drPolicy["CreatedOn"] = objPolicy.CreatedOn;

                            objPolicy.IsIncomingBasicSchedule = true;
                            drPolicy["IsIncomingBasicSchedule"] = objPolicy.IsIncomingBasicSchedule;

                            objPolicy.IsOutGoingBasicSchedule = true;
                            drPolicy["IsOutGoingBasicSchedule"] = objPolicy.IsOutGoingBasicSchedule;

                            objPolicy.CreatedBy = new Guid("AA38DF84-2E30-43CA-AED3-7276224D1B7E");
                            drPolicy["CreatedBy"] = objPolicy.CreatedBy;

                            objPolicy.IsDeleted = false;
                            drPolicy["IsDeleted"] = objPolicy.IsDeleted;

                        }

                        #region Fields that should be updated when blank or woth new policy
                        try
                        {

                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.PolicyType))
                            {
                                if (dt.Columns.Contains("New?") || dt.Columns.Contains("New Business?"))
                                {
                                    string strTypeOfPolicy = dt.Columns.Contains("New?") ? Convert.ToString(dt.Rows[i]["New?"]) : (dt.Columns.Contains("New Business?") ? Convert.ToString(dt.Rows[i]["New Business?"]) : "");
                                    objPolicy.PolicyType = (!string.IsNullOrEmpty(strTypeOfPolicy) && (strTypeOfPolicy.ToLower() == "rewrite" || strTypeOfPolicy.ToLower() == "replace")) ? "Replace" : "New";

                                }
                                else if (isNewPolicy)
                                {
                                    objPolicy.PolicyType = "New";
                                }
                                drPolicy["PolicyType"] = objPolicy.PolicyType;
                            }
                            //if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Insured))
                            //{
                            //    if (dt.Columns.Contains("insured?"))
                            //    {
                            //        objPolicy.Insured = Convert.ToString(dt.Rows[i]["insured"]);
                            //    }
                            //    else
                            //    {
                            //        objPolicy.Insured = objPolicy.Client.Name;
                            //    }
                            //}
                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.PolicyNumber))
                            {
                                if (dt.Columns.Contains("Group #"))
                                {
                                    objPolicy.PolicyNumber = Convert.ToString(dt.Rows[i]["Group #"]);
                                    drPolicy["PolicyNumber"] = objPolicy.PolicyNumber;
                                }
                            }
                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.SubmittedThrough))
                            {
                                if (dt.Columns.Contains("Submitted Through"))
                                {
                                    objPolicy.SubmittedThrough = Convert.ToString(dt.Rows[i]["Submitted Through"]);
                                    drPolicy["SubmittedThrough"] = objPolicy.SubmittedThrough;
                                }
                            }
                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Enrolled))
                            {
                                if (dt.Columns.Contains("Number of Covered Lives"))
                                {
                                    objPolicy.Enrolled = Convert.ToString(dt.Rows[i]["Number of Covered Lives"]);
                                    drPolicy["Enrolled"] = objPolicy.Enrolled;
                                }
                            }
                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Eligible))
                            {
                                if (dt.Columns.Contains("eligible"))
                                {
                                    objPolicy.Eligible = Convert.ToString(dt.Rows[i]["eligible"]);
                                    drPolicy["Eligible"] = objPolicy.Eligible;
                                }
                            }
                            if (isNewPolicy || (objPolicy.Advance == null))
                            {
                                if (dt.Columns.Contains("Advanced Payment Number"))
                                {
                                    objPolicy.Advance = (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Advanced Payment Number"]))) ? Convert.ToInt32(Convert.ToString(dt.Rows[i]["Advanced Payment Number"])) : 0;
                                    drPolicy["Advance"] = objPolicy.Advance;
                                }
                            }
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: optional fields , to be filled when blanks init done : " + policyIDKey, true);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: optional fields exception : " + ex.Message, true);
                            continue;
                        }
                        //Incoming Schedule 
                        #region Incoming Schedule
                        bool oldInScheduleExists = false;
                        DLinq.PolicyIncomingSchedule tempSched = null;
                        //Check if incoming is already set with 0 splits
                        if (objPolicy.IsIncomingBasicSchedule == true)
                        {
                            tempSched = objPolicy.PolicyIncomingSchedules.FirstOrDefault();
                            if (tempSched != null)
                            {
                                if (tempSched.FirstYearPercentage > 0 || tempSched.RenewalPercentage > 0)
                                {
                                    oldInScheduleExists = true;
                                }
                                //else if (tempSched.FirstYearPercentage == 0 && tempSched.RenewalPercentage == 0)
                                //{
                                //    PolicyToolIncommingShedule.DeleteSchedule(objPolicy.PolicyId); //If schedule occurs with 0 split, then delete before adding new
                                //}
                            }
                        }

                        if (isNewPolicy || (objPolicy.IsIncomingBasicSchedule != true) || (!oldInScheduleExists))
                        {
                            if (dt.Columns.Contains("Commission Type") && dt.Columns.Contains("Payment Type") && dt.Columns.Contains("Commissions - First Year %") && dt.Columns.Contains("Commissions - Renewal %") && dt.Columns.Contains("Co Broker Split"))
                            {
                                if (tempSched != null && tempSched.FirstYearPercentage == 0 && tempSched.RenewalPercentage == 0)
                                {
                                    PolicyToolIncommingShedule.DeleteSchedule(objPolicy.PolicyId); //If schedule occurs with 0 split, then delete before adding new
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy old schedule is deleted", true);
                                }


                                string strCommisionType = Convert.ToString(dt.Rows[i]["Commission Type"]);
                                objPolicy.IncomingPaymentTypeId = PolicCompType(strCommisionType, CompTypeList);
                                string strOutPercentOfPremium = Convert.ToString(dt.Rows[i]["Payment Type"]);
                                string strInFirstYear = Convert.ToString(dt.Rows[i]["Commissions - First Year %"]);
                                string strInRenewYear = Convert.ToString(dt.Rows[i]["Commissions - Renewal %"]);
                                string strSplit = Convert.ToString(dt.Rows[i]["Co Broker Split"]);
                                objPolicy.SplitPercentage = (!string.IsNullOrEmpty(strSplit)) ? Convert.ToDouble(strSplit.Replace("%", "")) : 0;

                                inSchedule = new PolicyToolIncommingShedule();
                                inSchedule.PolicyId = objPolicy.PolicyId;
                                inSchedule.IncomingScheduleId = Guid.NewGuid();
                                inSchedule.ScheduleTypeId = (!string.IsNullOrEmpty(strOutPercentOfPremium) && strOutPercentOfPremium.ToLower().Contains("head")) ? 2 : 1;
                                inSchedule.FirstYearPercentage = (!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                                inSchedule.RenewalPercentage = (!string.IsNullOrEmpty(strInRenewYear)) ? Convert.ToDouble(strInRenewYear) : 0;
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule init done : " + policyIDKey, true);
                            }
                            else
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule not found in excel", true);
                            }
                        }
                        #endregion


                        try
                        {
                            //Track date
                            if (isNewPolicy || objPolicy.TrackFromDate == null)
                            {
                                if (dt.Columns.Contains("Track From"))
                                {
                                    if (dt.Rows[i]["Track From"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Track From"])))
                                    {
                                        string trackDate = Convert.ToString(dt.Rows[i]["Track From"]);
                                        objPolicy.TrackFromDate = Convert.ToDateTime(trackDate);
                                        drPolicy["TrackFromDate"] = objPolicy.TrackFromDate;
                                    }
                                }
                            }
                            //Mode
                            if (isNewPolicy || objPolicy.PolicyModeId == null)
                            {
                                if (dt.Columns.Contains("Modal Number"))
                                {
                                    string strMode = Convert.ToString(dt.Rows[i]["Modal Number"]);
                                    objPolicy.PolicyModeId = (!string.IsNullOrEmpty(strMode)) ? PolicyModeID(strMode) : PolicyModeID("0");
                                    drPolicy["PolicyModeId"] = objPolicy.PolicyModeId;
                                }
                            }

                            //premium
                            if (isNewPolicy || objPolicy.MonthlyPremium == null)
                            {
                                if (dt.Columns.Contains("Modal Premium"))
                                {
                                    string strPremiuum = Convert.ToString(dt.Rows[i]["Modal Premium"]);
                                    objPolicy.MonthlyPremium = (!string.IsNullOrEmpty(strPremiuum)) ? Convert.ToDecimal(strPremiuum) : 0;
                                    drPolicy["MonthlyPremium"] = objPolicy.MonthlyPremium;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy execption :premium/mode/trackDate : " + ex.Message, true);
                            continue;
                        }
                        //Payor
                        #region Payor
                        try
                        {
                            if (isNewPolicy || objPolicy.PayorId == null)
                            {
                                if (dt.Columns.Contains("Payor Commission Dept"))
                                {
                                    string strPayor = Convert.ToString(dt.Rows[i]["Payor Commission Dept"]);
                                    if (!string.IsNullOrEmpty(strPayor))
                                    {
                                        DLinq.Payor py = (from p in DataModel.Payors where p.PayorName.ToLower() == strPayor.ToLower() select p).FirstOrDefault();
                                        if (py == null)
                                        {
                                            py = (from p in DataModel.Payors where p.NickName.ToLower() == strPayor.ToLower() select p).FirstOrDefault();
                                            if (py != null)
                                            {
                                                objPolicy.PayorId = py.PayorId;
                                                objPolicy.PayorReference.Value = py;
                                                drPolicy["PayorId"] = objPolicy.PayorId;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Carrier
                            if (isNewPolicy || objPolicy.CarrierId == null)
                            {
                                if (dt.Columns.Contains("Carrier Commission Dept"))
                                {
                                    string strCarr = Convert.ToString(dt.Rows[i]["Carrier Commission Dept"]);
                                    if (!string.IsNullOrEmpty(strCarr))
                                    {
                                        DLinq.Carrier cr = (from p in DataModel.Carriers where p.CarrierName.ToLower() == strCarr.ToLower() select p).FirstOrDefault();
                                        if (cr == null)
                                        {
                                            DLinq.CarrierNickName crN = (from p in DataModel.CarrierNickNames where p.PayorId == objPolicy.PayorId select p).FirstOrDefault();
                                            if (crN == null)
                                            {
                                                crN = (from p in DataModel.CarrierNickNames where p.NickName == strCarr select p).FirstOrDefault();
                                            }
                                            if (crN != null)
                                            {
                                                objPolicy.CarrierId = crN.CarrierId;
                                                objPolicy.CarrierReference.Value = crN.CarrierReference.Value;
                                            }
                                        }
                                        else
                                        {
                                            objPolicy.CarrierId = cr.CarrierId;
                                            objPolicy.CarrierReference.Value = cr;
                                        }
                                        drPolicy["CarrierId"] = objPolicy.CarrierId;
                                    }
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: payor/carrier : " + ex.Message, true);
                            continue;
                        }

                        try
                        {
                            #region Product
                            if (isNewPolicy || objPolicy.CoverageId == null)
                            {
                                if (dt.Columns.Contains("Line Of Coverage"))
                                {
                                    string strProduct = Convert.ToString(dt.Rows[i]["Line Of Coverage"]);
                                    DLinq.Coverage cov = (from p in DataModel.Coverages where p.ProductName == strProduct select p).FirstOrDefault();
                                    if (cov != null)
                                    {
                                        objPolicy.CoverageId = cov.CoverageId;
                                        objPolicy.CoverageReference.Value = cov;
                                        drPolicy["CoverageId"] = objPolicy.CoverageId;
                                    }
                                }
                            }
                            #endregion

                            #region Product Type
                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.ProductType))
                            {
                                if (dt.Columns.Contains("Product Type"))
                                {
                                    strProductType = Convert.ToString(dt.Rows[i]["Product Type"]);
                                    DLinq.CoverageNickName covName = (from p in DataModel.CoverageNickNames where p.NickName == strProductType select p).FirstOrDefault();
                                    covNickName = (covName != null) ? covName.NickName : string.Empty; //to be used later in code, so kept separate
                                    objPolicy.ProductType = (!string.IsNullOrEmpty(covNickName)) ? covNickName : strProductType;
                                    drPolicy["ProductType"] = objPolicy.ProductType;
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: product : " + ex.Message, true);
                            continue;
                        }


                        #endregion

                        #region Common fields - always update with insert/Update
                        bool OutPercentOfPremium = false;
                        try
                        {
                            //Status
                            if (dt.Columns.Contains("Plan Status Description") && dt.Rows[i]["Plan Status Description"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Plan Status Description"])))
                            {
                                string strStatus = dt.Rows[i]["Plan Status Description"].ToString();
                                objPolicy.PolicyStatusId = (strStatus.ToLower() == "active") ? 0 : (strStatus.ToLower() == "pending") ? 2 : 1;
                                drPolicy["PolicyStatusId"] = objPolicy.PolicyStatusId;
                            }
                            else if (dt.Columns.Contains("Current Plan Status") && dt.Rows[i]["Current Plan Status"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Current Plan Status"])))
                            {
                                string strStatus = dt.Rows[i]["Current Plan Status"].ToString();
                                objPolicy.PolicyStatusId = (strStatus.ToLower() == "active") ? 0 : (strStatus.ToLower() == "pending") ? 2 : 1;
                                drPolicy["PolicyStatusId"] = objPolicy.PolicyStatusId;
                            }

                            //Original effective date
                            if (dt.Columns.Contains("Original Plan Start Date"))
                            {
                                if (dt.Rows[i]["Original Plan Start Date"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Original Plan Start Date"])))
                                {
                                    string effDate = Convert.ToString(dt.Rows[i]["Original Plan Start Date"]);
                                    objPolicy.OriginalEffectiveDate = Convert.ToDateTime(effDate);
                                    drPolicy["OriginalEffectiveDate"] = objPolicy.OriginalEffectiveDate;
                                }
                            }
                            //Account Exec
                            if (dt.Columns.Contains("Account Owner: Full Name"))
                            {
                                if (dt.Rows[i]["Account Owner: Full Name"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Account Owner: Full Name"])))
                                {
                                    string acctExec = Convert.ToString(dt.Rows[i]["Account Owner: Full Name"]);
                                    var objUser = AgentList.Where(d => (d.FirstName + " " + d.LastName).ToLower() == acctExec.ToLower()).FirstOrDefault(); //User.GetUserIdWise(tempGuid);// (from p in DataModel.UserCredentials where (p.UserCredentialId == tempGuid && p.RoleId == 3 && p.IsDeleted == false) select p).FirstOrDefault();

                                    Guid tempGuid = Guid.Empty;
                                    Guid.TryParse(acctExec, out tempGuid);
                                    if (tempGuid != Guid.Empty)
                                    {
                                        objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                    }
                                    //Guid tempGuid = new Guid(acctExec);
                                    // User objUser = GlobalAgentList.Where(d => d.UserCredentialID == tempGuid).FirstOrDefault();
                                    if (objUser != null /*&& objUser.Role == UserRole.Agent*/)
                                    {
                                        //Need to get nick name
                                        if (string.IsNullOrEmpty(objUser.NickName))
                                        {
                                            objPolicy.AccoutExec = objUser.NickName;
                                            objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                        }
                                        else
                                        {
                                            objPolicy.AccoutExec = objUser.UserName;
                                            objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                        }
                                        bool isexec = (new User().CheckAccoutExec(objUser.UserCredentialId)); //Sets the flag of accExec true for this userID 
                                        drPolicy["AccoutExec"] = objPolicy.AccoutExec;
                                    }
                                }
                            }
                            //Term Date
                            if (dt.Columns.Contains("Plan End Date"))
                            {
                                if (dt.Rows[i]["Plan End Date"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Plan End Date"])))
                                {
                                    string termDate = Convert.ToString(dt.Rows[i]["Plan End Date"]);
                                    objPolicy.PolicyTerminationDate = Convert.ToDateTime(termDate);
                                    drPolicy["PolicyTerminationDate"] = objPolicy.PolicyTerminationDate;
                                }
                            }
                            //Term reason
                            if (dt.Columns.Contains("Termination Reason"))
                            {
                                if (dt.Rows[i]["Termination Reason"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Termination Reason"])))
                                {
                                    objPolicy.TerminationReasonId = PolicTermisionID(Convert.ToString(dt.Rows[i]["Termination Reason"]));
                                    drPolicy["TerminationReasonId"] = objPolicy.TerminationReasonId;
                                }
                            }
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: mandatory fields init done : " + policyIDKey, true);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: mandatory fields  : " + ex.Message, true);
                            continue;
                        }
                        //Outgoing split 
                        try
                        {
                            #region Outgoing Split
                            if (dt.Columns.Contains("Producer 1: Full Name") && dt.Columns.Contains("Producer 1 First Year %") && dt.Columns.Contains("Producer 1 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 1: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 1: Full Name"]);

                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //User objUser = UsrLst.Where(u => u.UserName.ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            //  Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            ///var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }

                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 1 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 1 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                //    OutgoingRecord.IsPrimaryAgent = true;
                                                OutgoingRecord.FirstYearPercentage = string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = string.IsNullOrEmpty(strRenewalPer) ? 0 : Convert.ToDouble(strRenewalPer);
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for pri1: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dt.Columns.Contains("Producer 2: Full Name") && dt.Columns.Contains("Producer 2 First Year %") && dt.Columns.Contains("Producer 2 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 2: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 2: Full Name"]);
                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            //var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }
                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 2 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 2 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                //OutgoingRecord.IsPrimaryAgent = false;
                                                OutgoingRecord.FirstYearPercentage = string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = string.IsNullOrEmpty(strRenewalPer) ? 0 : Convert.ToDouble(strRenewalPer);
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for ag1: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dt.Columns.Contains("Producer 3: Full Name") && dt.Columns.Contains("Producer 3 First Year %") && dt.Columns.Contains("Producer 3 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 3: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 3: Full Name"]);
                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //User objUser = UsrLst.Where(u => u.UserName.ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            //   Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            // var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }
                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 3 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 3 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                //     OutgoingRecord.IsPrimaryAgent = false;
                                                OutgoingRecord.FirstYearPercentage = string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = string.IsNullOrEmpty(strRenewalPer) ? 0 : Convert.ToDouble(strRenewalPer);
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for ag2: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dt.Columns.Contains("Producer 4: Full Name") && dt.Columns.Contains("Producer 4 First Year %") && dt.Columns.Contains("Producer 4 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 4: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 4: Full Name"]);
                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //User objUser = UsrLst.Where(u => u.UserName.ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            //Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            //var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }
                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 4 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 4 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                //   OutgoingRecord.IsPrimaryAgent = false;
                                                OutgoingRecord.FirstYearPercentage = string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = string.IsNullOrEmpty(strRenewalPer) ? 0 : Convert.ToDouble(strRenewalPer);
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for ag3: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dt.Columns.Contains("Producer 5: Full Name") && dt.Columns.Contains("Producer 5 First Year %") && dt.Columns.Contains("Producer 5 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 5: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 5: Full Name"]);
                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //User objUser = UsrLst.Where(u => u.UserName.ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            //Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            //var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }
                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 5 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 5 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                //       OutgoingRecord.IsPrimaryAgent = false;
                                                OutgoingRecord.FirstYearPercentage = string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = string.IsNullOrEmpty(strRenewalPer) ? 0 : Convert.ToDouble(strRenewalPer);
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for ag4: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dt.Columns.Contains("Producer 6: Full Name") && dt.Columns.Contains("Producer 6 First Year %") && dt.Columns.Contains("Producer 6 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 6: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 6: Full Name"]);
                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            //var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }
                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 6 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 6 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                //           OutgoingRecord.IsPrimaryAgent = false;
                                                OutgoingRecord.FirstYearPercentage = string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = string.IsNullOrEmpty(strRenewalPer) ? 0 : Convert.ToDouble(strRenewalPer);
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for ag5: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }
                            //Check for House if present
                            //try
                            //{
                            //    string strHouse = Convert.ToString(dt.Rows[i]["houseuc"]);
                            //    string strhousefy = Convert.ToString(dt.Rows[i]["housefy"]);
                            //    string strhousery = Convert.ToString(dt.Rows[i]["housern"]);
                            //    if (!string.IsNullOrEmpty(strHouse))
                            //    {
                            //        OutGoingPayment OutgoingRecord = new OutGoingPayment();
                            //        OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                            //        OutgoingRecord.PolicyId = objPolicy.PolicyId;
                            //        OutgoingRecord.IsPrimaryAgent = true;
                            //        OutgoingRecord.FirstYearPercentage = string.IsNullOrEmpty(strhousefy) ? 0 : Convert.ToDouble(strhousefy);
                            //        OutgoingRecord.RenewalPercentage = string.IsNullOrEmpty(strhousery) ? 0 : Convert.ToDouble(strhousery);
                            //        //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                            //        //houseOwner = 
                            //        OutgoingRecord.PayeeUserCredentialId = new Guid(strHouse);
                            //        OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                            //        if (OutgoingRecord.FirstYearPercentage > 0 || OutgoingRecord.RenewalPercentage > 0)
                            //            OutGoingField.Add(OutgoingRecord);
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    ActionLogger.Logger.WriteImportPolicyLog("Exception setting house owner: " + ex.Message + ", so chcking frim licensee ", true);
                            //    continue;
                            //}
                            #endregion

                            #endregion
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: outgoing split init done : " + policyIDKey, true);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: outgoing fields  : " + ex.Message, true);
                            continue;
                        }
                        #region Save Data
                        try
                        {
                            if (errorList == null || !errorList.Contains(importedPolicyID))
                            {
                                #region Client
                                try
                                {

                                    if (isNewPolicy || objPolicy.PolicyClientId == null)
                                    {
                                        string client = Convert.ToString(dt.Rows[i]["Account Name"]);
                                        Client objClient = (new Client()).GetClientByClientName(client, LicID);
                                        //Get Client ID by Get Client name
                                        if (objClient == null)
                                        {
                                            //Create new client
                                            objClient = new Client();
                                            objClient.ClientId = Guid.NewGuid();
                                            string strClientValue = string.Empty;

                                            strClientValue = (client.Length > 49) ? client.Substring(0, 49) : client;

                                            //objClnt.Name = policy.ClientName;
                                            objClient.Name = strClientValue;
                                            objClient.LicenseeId = LicID;
                                            objClient.IsDeleted = false;
                                            Client.AddUpdateClient(client, LicID, objClient.ClientId);
                                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: client saved as new : " + policyIDKey, true);
                                        }
                                        objPolicy.ClientReference.Value = (from p in DataModel.Clients where p.ClientId == objClient.ClientId select p).FirstOrDefault();
                                    }
                                    //following requires client reference, so added at last
                                    if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Insured))
                                    {
                                        if (dt.Columns.Contains("insured?"))
                                        {
                                            objPolicy.Insured = Convert.ToString(dt.Rows[i]["insured"]);
                                        }
                                        else
                                        {
                                            objPolicy.Insured = objPolicy.Client.Name;
                                        }
                                        drPolicy["Insured"] = objPolicy.Insured;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    errorCount++;
                                    errorList.Add(importedPolicyID);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: client : " + ex.Message, true);
                                    continue;
                                }
                                #endregion
                                if (isNewPolicy)
                                {
                                    InsertIntoMembers(dtPolicies);
                                    //DataModel.AddToPolicies(objPolicy);
                                    //DataModel.SaveChanges();
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy : policy saved successfully", true);
                                    PolicyToLearnPost.AddLearnedAfterImport(objPolicy.PolicyId, "", covNickName, strProductType, importedPolicyID);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy : learned fields saved successfully", true);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Policy saved as new : " + policyIDKey, true);

                                }
                                else
                                {
                                    DataModel.SaveChanges();
                                    AddUpdatePolicyHistory(objPolicy.PolicyId);
                                    PolicyLearnedField.AddUpdateHistoryLearned(objPolicy.PolicyId);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Policy updated : " + policyIDKey, true);
                                }


                                //Save Incoming 
                                if (inSchedule != null)
                                {
                                    inSchedule.AddUpdate();
                                    ActionLogger.Logger.WriteImportPolicyLog("Incoming schedule added for the policy", true);
                                }

                                //Save Outgoing
                                //Delete old schdule- outgoing  payments witj old schedule will remian intact, new will take effect on new entries only.
                                //Check only if new outgoing schedule exists, then overwrite the old
                                //This is to exclude the case when policy is existing and excel has blank entries. Without this, existing schedule will be overwritten
                                // by 100% to house for existing policy 
                                if (isNewPolicy || OutGoingField.Count > 0)
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule adding to the policy", true);
                                    OutGoingField = CompleteOutgoingSchedule(OutGoingField, objPolicy.PolicyId, LicID);
                                    OutGoingPayment.DeletePolicyOutGoingSchedulebyPolicyId(objPolicy.PolicyId);
                                    ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule removed from the policy", true);
                                    OutGoingPayment.AddUpdate(OutGoingField);
                                    ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule added for the policy", true);
                                }

                                if (isNewPolicy) addCount++;
                                else updateCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: saving data fields  : " + ex.Message, true);
                            continue;
                        }
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Exception adding policy: " + ex.Message, true);
                        errorCount++;
                        errorList.Add(importedPolicyID);
                    }
                }
            }

            return objStatus;
        }

        static OutGoingPayment GetOutgoingPaymentForImport(string agent, Guid userID, Guid LicID, string firstYear, string renewal, Guid policyId, bool OutPercentOfPremium)
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetOutgoingPaymentForImport request: " + agent, true);
                char[] spCharac = System.Configuration.ConfigurationSettings.AppSettings["AgentCharactersToTrim"].ToCharArray();  //{ '(', '[', '-' };

                int spIndex = agent.IndexOfAny(spCharac);
                agent = (spIndex > 0) ? agent.Substring(0, spIndex - 1) : agent;
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " strPrimaryBroker: " + agent, true);


                //  var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == agent.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == agent.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == agent.ToLower())).FirstOrDefault();
                //  Guid userID = Guid.Empty;
                ////Check if incoming value is Guid 
                //if (userID == Guid.Empty)
                //{
                //    //Guid tempGuid = Guid.Empty;
                //    //Guid.TryParse(agent, out tempGuid);
                //    //if (tempGuid != Guid.Empty)
                //    //{
                //    //    objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                //    //}
                //    //  //If still null, create new 
                //    //if (objUser == null)
                //    //{
                //         userID  = AddNewUserFromBenefits(agent, LicID);
                //    //}
                //}
                ////else
                ////{
                ////    userID = objUser.UserCredentialId;
                ////}
                // ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " userID found as: " + userID, true);


                //   if (objUser != null)
                if (userID != Guid.Empty)
                {

                    OutGoingPayment OutgoingRecord = new OutGoingPayment();
                    OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                    OutgoingRecord.PolicyId = policyId;

                    double frst = 0; double renew = 0;
                    double.TryParse(firstYear, out frst);
                    double.TryParse(renewal, out renew);

                    OutgoingRecord.FirstYearPercentage = frst;// string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                    OutgoingRecord.RenewalPercentage = renew;// string.IsNullOrEmpty(strRenewalPer) ? 0 : Convert.ToDouble(strRenewalPer);
                                                             //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                    OutgoingRecord.PayeeUserCredentialId = userID;// objUser.UserCredentialId;
                    OutgoingRecord.ScheduleTypeId = OutPercentOfPremium ? 1 : 2;
                    return OutgoingRecord;
                    //  OutGoingField.Add(OutgoingRecord);
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetOutgoingPaymentForImport exception: " + ex.Message, true);
                return null;
            }
        }

        public static void UpdateBGUserID(Guid UserCrdentialId, string BGUserId, string agencyName = "")
        {
            ActionLogger.Logger.WriteImportPolicyLog("UpdateBGUserID: process starts with BGUserId: " + BGUserId + " " + "UserCrdentialId:" + UserCrdentialId, true, agencyName);
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var bgUserIdUpdate = (from p in DataModel.UserCredentials
                                          where p.UserCredentialId == UserCrdentialId
                                          select p).SingleOrDefault();
                    bgUserIdUpdate.BGUserId = BGUserId;
                    DataModel.SaveChanges();
                }
                ActionLogger.Logger.WriteImportPolicyLog("UpdateBGUserID: process success with BGUserId: " + BGUserId + " " + "UserCrdentialId:" + UserCrdentialId, true, agencyName);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("UpdateBGUserID process Fails with BGUserId: " + BGUserId + " " + "UserCrdentialId:" + UserCrdentialId, true, agencyName);
            }
        }



        public static void GetOutgoingSplitdata_Caravus(GetAgentList getdata, Guid LicenseeId, ref List<OutGoingPayment> OutGoingField, DLinq.Policy objPolicy, IEnumerable<dynamic> AgentList, bool OutPercentOfPremium, ref Dictionary<string, string> errMsgPolicy, string agencyName = "")
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetOutgoingSplitdata process starts ", true);

                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    List<string> myList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(getdata.ProducerId))
                    {

                        string currentUserBGUserId = getdata.ProducerId;
                        Guid userID = Guid.Empty;
                        var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                        if (getBGUserId == null)
                        {
                            //if (!string.IsNullOrEmpty(getdata.ProducerName))
                            //{
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(getdata.ProducerName)))
                            {
                                string strPrimaryBroker = Convert.ToString(getdata.ProducerName).Trim();
                                try
                                {
                                    var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                                    if (objUser == null)
                                    {
                                        errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + "not available in the system");
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                                        AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                    }
                                    else
                                    {
                                        userID = objUser.UserCredentialId;
                                        OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicenseeId, getdata.ProducerFirstYear, getdata.ProducerRenewal, objPolicy.PolicyId, OutPercentOfPremium);
                                        if (OutgoingRecord != null)
                                        {

                                            objPolicy.PrimaryAgent = userID;
                                            OutGoingField.Add(OutgoingRecord);
                                            //Add check for updating BgUserId by Ankit-08-10-2018
                                            UpdateBGUserID(userID, currentUserBGUserId);
                                        }
                                        else
                                        {
                                            errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system");
                                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                                            AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add(getdata.ProducerExecutionName, ex.Message);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + getdata.ProducerExecutionId + "  fields  : " + ex.Message, true);
                                    AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                    throw ex;

                                }
                            }
                            else
                            {
                                errMsgPolicy.Add(getdata.ProducerExecutionId, getdata.ProducerExecutionId + " " + "not available in the system");
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + getdata.ProducerExecutionName + " not available in the system ", true);
                                AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);

                            }

                        }
                        else
                        {
                            userID = getBGUserId.UserCredentialId;
                            OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(currentUserBGUserId, userID, LicenseeId, getdata.ProducerFirstYear, getdata.ProducerRenewal, objPolicy.PolicyId, OutPercentOfPremium);
                            if (OutgoingRecord != null)
                            {
                                OutGoingField.Add(OutgoingRecord);
                            }
                            else
                            {
                                errMsgPolicy.Add(getdata.ProducerExecutionId, getdata.ProducerExecutionId + " " + "not available in the system");
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + getdata.ProducerExecutionId + " not available in the system ", true);
                                AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);

                            }
                        }
                    }
                    else
                    {
                        //if (!string.IsNullOrEmpty(getdata.ProducerName))
                        //{
                        if (!string.IsNullOrWhiteSpace(getdata.ProducerName))
                        {
                            string strPrimaryBroker = Convert.ToString(getdata.ProducerName).Trim();
                            try
                            {

                                Guid userID = Guid.Empty;
                                ////Check user on name
                                var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                                if (objUser == null)
                                {
                                    errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system");
                                    //errorCount++;
                                    //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                                    //errorList.Add(m);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                                    AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                    //continue;
                                }
                                else
                                {
                                    userID = objUser.UserCredentialId;
                                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicenseeId, getdata.ProducerFirstYear, getdata.ProducerRenewal, objPolicy.PolicyId, OutPercentOfPremium);
                                    if (OutgoingRecord != null)
                                    {
                                        objPolicy.PrimaryAgent = userID;
                                        //OutgoingRecord.IsPrimaryAgent = true;
                                        OutGoingField.Add(OutgoingRecord);
                                    }
                                    else
                                    {
                                        errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system"); ;
                                        //errorCount++;
                                        //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                                        //    errorList.Add(m);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                                        AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                        //continue;
                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system");
                                //errorCount++;
                                //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1 fields: " + ex.Message);
                                //    errorList.Add(m);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "fields  : " + ex.Message, true);
                                AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                //  continue;
                                throw ex;
                            }
                            //}
                        }
                        else
                        {
                            errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system");
                            //errorCount++;
                            //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                            //    errorList.Add(m);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                            AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                            //continue;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system");
                //errorCount++;
                //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1 fields: " + ex.Message);
                //    errorList.Add(m);
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                throw ex;
            }
        }


        static string GetCDUserID(IEnumerable<dynamic> AgentList, string currentUserBGUserId, string payeeName, string agencyName = "")
        {
            ActionLogger.Logger.WriteImportPolicyLog("Import Policy GetCDUserID:  currentUserBGUserId  : " + currentUserBGUserId + ", payeeName: " + payeeName, true, agencyName);
            string userID = "";
            try
            {
                if (!string.IsNullOrEmpty(currentUserBGUserId))
                {
                    var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                    if (getBGUserId == null)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy GetCDUserID: userID is null", true, agencyName);
                        if (!string.IsNullOrWhiteSpace(payeeName))
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy GetCDUserID: payeename check ", true, agencyName);
                            string strPrimaryBroker = Convert.ToString(payeeName).Trim();
                            try
                            {
                                var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                                if (objUser != null)
                                {
                                    userID = Convert.ToString(objUser.UserCredentialId);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy GetCDUserID: payeename found ", true, agencyName);
                                }
                                else
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy GetCDUserID: payeename NOT found ", true, agencyName);
                                }
                            }
                            catch (Exception ex)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:  fields  : " + ex.Message, true, agencyName);

                            }
                        }
                    }
                    else
                    {
                        userID = Convert.ToString(getBGUserId.UserCredentialId);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy GetCDUserID: payee found on User ID ", true, agencyName);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(payeeName))
                {
                    string strPrimaryBroker = Convert.ToString(payeeName).Trim();
                    try
                    {
                        var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        if (objUser != null)
                        {
                            userID = Convert.ToString(objUser.UserCredentialId);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy GetCDUserID: payee found on name ", true, agencyName);
                        }
                    }
                    catch (Exception ex)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:fields  : " + ex.Message, true, agencyName);
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:  fields  : " + ex.Message, true, agencyName);
            }

            return userID;
        }

        /// <summary>
        /// Created by: Ankit
        /// CreatedOn:sept 06,2018
        /// </summary>
        /// <param name="getdata"></param>
        /// <param name="LicenseeId"></param>
        /// <param name="OutGoingField"></param>
        /// <param name="objPolicy"></param>
        /// <param name="AgentList"></param>
        /// <param name="OutPercentOfPremium"></param>
        /// <param name="errMsgPolicy"></param>
        public static void GetOutgoingSplitdata(GetAgentList getdata, Guid LicenseeId, ref List<OutGoingPayment> OutGoingField, DLinq.Policy objPolicy, IEnumerable<dynamic> AgentList, bool OutPercentOfPremium, ref Dictionary<string, string> errMsgPolicy, string agencyName = "")
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetOutgoingSplitdata process starts ", true);

                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    List<string> myList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(getdata.ProducerId))
                    {

                        string currentUserBGUserId = getdata.ProducerId;
                        Guid userID = Guid.Empty;
                        var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                        if (getBGUserId == null)
                        {
                            //if (!string.IsNullOrEmpty(getdata.ProducerName))
                            //{
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(getdata.ProducerName)))
                            {
                                string strPrimaryBroker = Convert.ToString(getdata.ProducerName).Trim();
                                try
                                {
                                    var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                                    if (objUser == null)
                                    {
                                        errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + "not available in the system");
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                                        AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                    }
                                    else
                                    {
                                        userID = objUser.UserCredentialId;
                                        OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicenseeId, getdata.ProducerFirstYear, getdata.ProducerRenewal, objPolicy.PolicyId, OutPercentOfPremium);
                                        if (OutgoingRecord != null)
                                        {

                                            objPolicy.PrimaryAgent = userID;
                                            OutGoingField.Add(OutgoingRecord);
                                            //Add check for updating BgUserId by Ankit-08-10-2018
                                            UpdateBGUserID(userID, currentUserBGUserId);
                                        }
                                        else
                                        {
                                            errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system");
                                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                                            AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add(getdata.ProducerExecutionName, ex.Message);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + getdata.ProducerExecutionId + "  fields  : " + ex.Message, true);
                                    AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                    throw ex;

                                }
                            }
                            else
                            {
                                errMsgPolicy.Add(getdata.ProducerExecutionId, getdata.ProducerExecutionId + " " + "not available in the system");
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + getdata.ProducerExecutionName + " not available in the system ", true);
                                AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);

                            }

                        }
                        else
                        {
                            userID = getBGUserId.UserCredentialId;
                            OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(currentUserBGUserId, userID, LicenseeId, getdata.ProducerFirstYear, getdata.ProducerRenewal, objPolicy.PolicyId, OutPercentOfPremium);
                            if (OutgoingRecord != null)
                            {
                                OutGoingField.Add(OutgoingRecord);
                            }
                            else
                            {
                                errMsgPolicy.Add(getdata.ProducerExecutionId, getdata.ProducerExecutionId + " " + "not available in the system");
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + getdata.ProducerExecutionId + " not available in the system ", true);
                                AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);

                            }
                        }
                    }
                    else
                    {
                        //if (!string.IsNullOrEmpty(getdata.ProducerName))
                        //{
                        if (!string.IsNullOrWhiteSpace(getdata.ProducerName))
                        {
                            string strPrimaryBroker = Convert.ToString(getdata.ProducerName).Trim();
                            try
                            {

                                Guid userID = Guid.Empty;
                                ////Check user on name
                                var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                                if (objUser == null)
                                {
                                    errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system");
                                    //errorCount++;
                                    //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                                    //errorList.Add(m);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                                    AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                    //continue;
                                }
                                else
                                {
                                    userID = objUser.UserCredentialId;
                                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicenseeId, getdata.ProducerFirstYear, getdata.ProducerRenewal, objPolicy.PolicyId, OutPercentOfPremium);
                                    if (OutgoingRecord != null)
                                    {
                                        objPolicy.PrimaryAgent = userID;
                                        //OutgoingRecord.IsPrimaryAgent = true;
                                        OutGoingField.Add(OutgoingRecord);
                                    }
                                    else
                                    {
                                        errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system"); ;
                                        //errorCount++;
                                        //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                                        //    errorList.Add(m);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                                        AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                        //continue;
                                    }
                                }


                            }
                            catch (Exception ex)
                            {
                                errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system");
                                //errorCount++;
                                //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1 fields: " + ex.Message);
                                //    errorList.Add(m);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "fields  : " + ex.Message, true);
                                AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                                //  continue;
                                throw ex;
                            }
                            //}
                        }
                        else
                        {
                            errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system");
                            //errorCount++;
                            //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                            //    errorList.Add(m);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                            AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                            //continue;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errMsgPolicy.Add(getdata.ProducerExecutionName, getdata.ProducerExecutionName + " " + " not available in the system");
                //errorCount++;
                //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1 fields: " + ex.Message);
                //    errorList.Add(m);
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + getdata.ProducerExecutionName + "  not available in the system ", true);
                AddImportStatusToDB(getdata.importedPolicyID, getdata.isNewPolicy, false, getdata.benefits_policyID, agencyName);
                throw ex;
            }
        }
        /// <summary>
        /// Modified By:Ankit khandelwal
        /// Modified On:17-10-18
        /// Purpose:Import process for benefit pro 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="LicID"></param>
        /// <param name="CompTypeList"></param>
        /// <returns></returns>
        public static Benefits_PolicyImportStatus ImportPolicy_Benefits(DataTable dt, Guid LicID, ObservableCollection<CompType> CompTypeList, string agencyName = "")
        {
            #region Variables
            Benefits_PolicyImportStatus objStatus = new Benefits_PolicyImportStatus();

            /* Following to be used in case   1-many mapping exists as anticipated before, not in use for now 
            * 
            * List<string> lstKeys = System.Configuration.ConfigurationSettings.AppSettings["BenefitsPolicyKeys"].Split(',').ToList<String>();
              string policyIDKey = "";
              foreach (string s in lstKeys)
              {
                  if (dt.Columns.Contains(s))
                  {
                      policyIDKey = s;
                      break;
                  }
              }*/

            string policyIDKey = System.Configuration.ConfigurationSettings.AppSettings["PolicyIDKeyName_Benefits"]; // "OriginalPlanID";

            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyIDKey: " + policyIDKey, true);
            char[] spCharac = System.Configuration.ConfigurationSettings.AppSettings["AgentCharactersToTrim"].ToCharArray();  //{ '(', '[', '-' };

            //Response object structure
            int addCount = 0;
            int updateCount = 0;
            int errorCount = 0;
            List<Benefits_ErrorMsg> errorList = new List<Benefits_ErrorMsg>();


            List<OutGoingPayment> OutGoingField = new List<OutGoingPayment>();
            PolicyToolIncommingShedule inSchedule = null;
            string strProductType = string.Empty;
            string covNickName = string.Empty;
            Guid houseOwner = Guid.Empty;

            #endregion

            #region Agents List
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Data model init", true);
                var AgentList = (from p in DataModel.UserCredentials
                                 join o in DataModel.UserDetails on p.UserCredentialId equals o.UserCredentialId
                                 where p.LicenseeId == LicID /*&& p.RoleId == 3*/ && p.IsDeleted == false
                                 select new
                                 {
                                     p.UserCredentialId,
                                     o.NickName,
                                     p.UserName,
                                     p.RoleId,
                                     o.FirstName,
                                     o.LastName,
                                     p.BGUserId
                                 }).ToList();
                // ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Agent list fetched " + AgentList.ToStringDump(), true);

                #endregion

                #region Get Agency's track date default
                DateTime? dtTrack = DateTime.MinValue;
                try
                {
                    //Jyotisna - Dec 17, 2018 - Hard coded check as advised by Kevin for "Benefit Pro" required in BG integration
                    if (LicID == new Guid("20FCF5E6-70DA-44C1-9FC0-AF73500BFB6F"))
                    {
                        DateTime dtBPro = DateTime.MinValue;
                        DateTime.TryParse(System.Configuration.ConfigurationManager.AppSettings["Benefits_Pro_TrackDate"], out dtBPro);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy default trackDatefrom benefits pro: " + dtBPro, true);
                        dtTrack = dtBPro;
                    }
                    else
                    {

                        var strTrack = (from l in DataModel.Licensees where l.LicenseeId == LicID select new { l.TrackDateDefault }).FirstOrDefault();
                        if (strTrack != null)
                        {
                            if (strTrack.TrackDateDefault != null)// && strTrack.TrackDateDefault > 
                                dtTrack = strTrack.TrackDateDefault;
                        }
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy trackDatefrom agency: " + dtTrack, true);
                    }

                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("track date calculation failed" + ex.Message, true);
                }

                #endregion

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DLinq.Policy objPolicy = new DLinq.Policy();
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Iteration: " + i, true);

                    string importedPolicyID = dt.Columns.Contains(policyIDKey) ? Convert.ToString(dt.Rows[i][policyIDKey]).Trim() : "";// (dt.Columns.Contains("Life Insurance Plan ID") ? Convert.ToString(dt.Rows[i]["Life Insurance Plan ID"]).Trim() : "");
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: importedPolicyID: " + importedPolicyID, true);

                    string benefits_policyID = string.Empty;
                    if (dt.Columns.Contains("PolicyPlanID")) //|| dt.Columns.Contains("New Business?"))
                    {
                        benefits_policyID = Convert.ToString(dt.Rows[i]["PolicyPlanID"]);
                    }

                    Dictionary<string, string> errMsgPolicy = new Dictionary<string, string>();
                    //List<Benefits_ErrorMsg.errMsg> errMsgPolicy = new List<Benefits_ErrorMsg.errMsg>();
                    // List<string> errMsgPolicy = new List<string>();
                    //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, errMsgPolicy);
                    if (string.IsNullOrEmpty(importedPolicyID))
                    {
                        errMsgPolicy.Add("OriginalPlanID", "Unique policy ID found missing");
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Policy ID found null/blank, skipping record", true);
                        AddImportStatusToDB(importedPolicyID, false, false, benefits_policyID, agencyName);
                        errorCount++;
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, output);
                        errorList.Add(m);
                        continue;
                    }
                    string clientName = string.Empty;
                    if (dt.Columns.Contains("AccountName"))
                    {
                        clientName = Convert.ToString(dt.Rows[i]["AccountName"]);
                    }
                    else if (dt.Columns.Contains("Account Name"))
                    {
                        clientName = Convert.ToString(dt.Rows[i]["Account Name"]);
                    }


                    if (string.IsNullOrEmpty(clientName))
                    {
                        errMsgPolicy.Add("AccountName", "Account name found missing");
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Account name found null/blank, skipping record", true);
                        AddImportStatusToDB(importedPolicyID, false, false, benefits_policyID, agencyName);
                        errorCount++;
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, output);
                        errorList.Add(m);
                        continue;
                    }

                    OutGoingField = new List<OutGoingPayment>();
                    bool isNewPolicy = false;
                    //try
                    //{
                    Guid policyID = IsPolicyExistingWithImportID(importedPolicyID, LicID);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyID: " + policyID + ", benefits ID: " + benefits_policyID, true);
                    if (policyID != Guid.Empty)
                    {
                        objPolicy = (from p in DataModel.Policies where p.PolicyId == policyID select p).FirstOrDefault();
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Existing policy", true);


                        //check if client exists or deleted in the system
                        //if deleted, then add the given as new 
                        if (objPolicy.Client != null && Convert.ToBoolean(objPolicy.Client.IsDeleted))
                        {
                            objPolicy.PolicyClientId = null;
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Client found deleted , so setting as null ", true);
                        }
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: New policy", true);
                        isNewPolicy = true;
                        objPolicy.PolicyId = Guid.NewGuid();
                        objPolicy.IsTrackPayment = true;
                        objPolicy.PolicyLicenseeId = LicID;
                        objPolicy.TerminationReasonId = null;
                        objPolicy.IsTrackMissingMonth = true;
                        objPolicy.CreatedOn = DateTime.Today;
                        objPolicy.IsIncomingBasicSchedule = true;
                        objPolicy.IsOutGoingBasicSchedule = true;
                        objPolicy.CreatedBy = new Guid("AA38DF84-2E30-43CA-AED3-7276224D1B7E");
                        objPolicy.IsDeleted = false;
                    }

                    #region Fields that should be updated when blank or with new policy
                    //try
                    //{

                    if (isNewPolicy || string.IsNullOrEmpty(objPolicy.PolicyType))
                    {
                        try
                        {
                            if (dt.Columns.Contains("NewBusiness")) //|| dt.Columns.Contains("New Business?"))
                            {
                                string strTypeOfPolicy = Convert.ToString(dt.Rows[i]["NewBusiness"]); //dt.Columns.Contains("New?") ? Convert.ToString(dt.Rows[i]["New?"]) : (dt.Columns.Contains("New Business?") ? Convert.ToString(dt.Rows[i]["New Business?"]) : "");
                                objPolicy.PolicyType = (!string.IsNullOrEmpty(strTypeOfPolicy) && (strTypeOfPolicy.ToLower() == "rewrite" || strTypeOfPolicy.ToLower() == "replace")) ? "Replace" : "New";
                            }
                            else if (isNewPolicy)
                            {
                                objPolicy.PolicyType = "New";
                            }
                        }
                        catch (Exception ex)
                        {
                            errMsgPolicy.Add("NewBusiness", ex.Message);
                            //errorCount++;
                            ////errorList.Add(importedPolicyID, "New Business field: " + ex.Message);
                            //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "NewBusiness: " + ex.Message);
                            //errorList.Add(m);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: New Business field  exception : " + ex.Message, true);
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            //continue;
                        }
                    }

                    if (isNewPolicy || string.IsNullOrEmpty(objPolicy.PolicyNumber))
                    {
                        try
                        {
                            if (dt.Columns.Contains("Group"))
                            {
                                objPolicy.PolicyNumber = Convert.ToString(dt.Rows[i]["Group"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            errMsgPolicy.Add("Group", ex.Message);
                            //errorCount++;
                            ////errorList.Add(importedPolicyID, "Group field: " + ex.Message);
                            //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Group: " + ex.Message);
                            //errorList.Add(m);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: Group fields exception : " + ex.Message, true);
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            // continue;
                        }
                    }

                    objPolicy.SubmittedThrough = ""; //Not to be sent by benefits and to be left blank as per kevin Aug 17, 2017

                    if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Enrolled))
                    {
                        try
                        {
                            if (dt.Columns.Contains("NumberofCoveredLives"))
                            {
                                objPolicy.Enrolled = Convert.ToString(dt.Rows[i]["NumberofCoveredLives"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            errMsgPolicy.Add("NumberofCoveredLives", ex.Message);
                            //errorCount++;
                            //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "NumberofCoveredLives: " + ex.Message);
                            //errorList.Add(m);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: NumberofCoveredLives fields exception : " + ex.Message, true);
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            //continue;
                        }
                    }
                    if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Eligible))
                    {
                        try
                        {
                            if (dt.Columns.Contains("Eligible"))
                            {
                                objPolicy.Eligible = Convert.ToString(dt.Rows[i]["Eligible"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            errMsgPolicy.Add("Eligible", ex.Message);
                            //errorCount++;
                            //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Eligible: " + ex.Message);
                            //errorList.Add(m);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: Eligible fields exception : " + ex.Message, true);
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            //continue;
                        }
                    }

                    /*
                     *Commented after advised by Kevin to update the following every time - Aug 21, 2019
                     * if (isNewPolicy || (objPolicy.Advance == null))
                     {
                         try
                         {
                             if (dt.Columns.Contains("AdvancedPaymentNumber"))
                             {
                                 objPolicy.Advance = (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["AdvancedPaymentNumber"]))) ? Convert.ToInt32(Convert.ToString(dt.Rows[i]["AdvancedPaymentNumber"])) : 0;
                             }
                         }
                         catch (Exception ex)
                         {
                             errMsgPolicy.Add("AdvancedPaymentNumber", ex.Message);
                             //errorCount++;
                             //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "AdvancedPaymentNumber: " + ex.Message);
                             //    errorList.Add(m);
                             ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: AdvancedPaymentNumber fields exception : " + ex.Message, true);
                             AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                             //continue;
                         }
                     }*/
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: optional fields , to be filled when blanks init done : " + policyIDKey, true);


                    //Mode
                    if (isNewPolicy || objPolicy.PolicyModeId == null)
                    {
                        try
                        {
                            if (dt.Columns.Contains("ModalNumber"))
                            {
                                string strMode = Convert.ToString(dt.Rows[i]["ModalNumber"]);
                                objPolicy.PolicyModeId = (!string.IsNullOrEmpty(strMode)) ? PolicyModeID(strMode) : PolicyModeID("0");
                            }
                        }
                        catch (Exception ex)
                        {
                            errMsgPolicy.Add("ModalNumber", ex.Message);
                            //errorCount++;
                            //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "ModalNumber: " + ex.Message);
                            //errorList.Add(m);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: ModalNumber fields exception : " + ex.Message, true);
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            //continue;
                        }
                    }

                    //premium
                    if (isNewPolicy || objPolicy.MonthlyPremium == null)
                    {
                        try
                        {
                            if (dt.Columns.Contains("ModalPremium"))
                            {
                                string strPremiuum = Convert.ToString(dt.Rows[i]["ModalPremium"]);
                                decimal prem = 0;
                                decimal.TryParse(strPremiuum, out prem);
                                if (prem == 0)
                                {
                                    decimal.TryParse(strPremiuum, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out prem);
                                }
                                objPolicy.MonthlyPremium = prem;// (!string.IsNullOrEmpty(strPremiuum)) ? Convert.ToDecimal(strPremiuum) : 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            errMsgPolicy.Add("ModalPremium", ex.Message);
                            //errorCount++;
                            //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "ModalPremium: " + ex.Message);
                            //errorList.Add(m);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: ModalPremium fields exception : " + ex.Message, true);
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            //continue;
                        }
                    }

                    //Payor  
                    #region Payor
                    //try
                    //{
                    if (isNewPolicy || objPolicy.PayorId == null)
                    {
                        if (dt.Columns.Contains("PayorCommissionDept"))
                        {
                            try
                            {
                                string strPayor = Convert.ToString(dt.Rows[i]["PayorCommissionDept"]);
                                if (!string.IsNullOrEmpty(strPayor))
                                {
                                    //Compare payors - if name matches -  payor to be global or belonging to same agency.
                                    DLinq.Payor py = (from p in DataModel.Payors
                                                      where ((p.PayorName.ToLower() == strPayor.ToLower() || (p.PayorName.ToLower() != strPayor.ToLower() && p.NickName.ToLower() == strPayor.ToLower())))
                           && ((p.IsGlobal || (!p.IsGlobal && p.LicenseeId == LicID)))
                                                      select p).FirstOrDefault();
                                    //if (py == null)
                                    // {
                                    if (py != null) //If found existing 
                                    {
                                        objPolicy.PayorId = py.PayorId;
                                        objPolicy.PayorReference.Value = py;
                                    }
                                    else  // if not existing, return error  
                                    {
                                        errMsgPolicy.Add("PayorCommissionDept", "Payor not available");
                                        //errorCount++;
                                        //        Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "PayorCommissionDept: Payor not available");
                                        //        errorList.Add(m);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: payor fields exception : Payor not available", true);
                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                                    }

                                    // }
                                }
                            }
                            catch (Exception ex)
                            {
                                errMsgPolicy.Add("PayorCommissionDept", ex.Message);
                                //errorCount++;
                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "PayorCommissionDept: " + ex.Message);
                                //errorList.Add(m);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: payor fields exception : " + ex.Message, true);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                //continue;
                            }
                        }

                    }
                    #endregion

                    #region Carrier
                    if (isNewPolicy || objPolicy.CarrierId == null)
                    {
                        if (dt.Columns.Contains("CarrierCommissionDept"))
                        {
                            try
                            {
                                string strCarr = Convert.ToString(dt.Rows[i]["CarrierCommissionDept"]);
                                if (!string.IsNullOrEmpty(strCarr))
                                {
                                    DLinq.Carrier cr = (from p in DataModel.Carriers where (p.CarrierName.ToLower() == strCarr.ToLower() && (p.IsGlobal || (!p.IsGlobal && p.LicenseeId == LicID))) select p).FirstOrDefault();
                                    if (cr == null)
                                    {
                                        DLinq.CarrierNickName crN = (from p in DataModel.CarrierNickNames where p.NickName == strCarr select p).FirstOrDefault();
                                        // if (crN == null)
                                        //{
                                        //    crN = (from p in DataModel.CarrierNickNames where p.NickName == strCarr select p).FirstOrDefault();
                                        //}
                                        if (crN != null) //Carrier found in system by nickname
                                        {
                                            objPolicy.CarrierId = crN.CarrierId;
                                            objPolicy.CarrierReference.Value = crN.CarrierReference.Value;
                                        }
                                        else
                                        {
                                            errMsgPolicy.Add("CarrierCommissionDept", "Carrier not available");
                                            //errorCount++;
                                            //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "CarrierCommissionDept: Carrier not available");
                                            //errorList.Add(m);
                                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: carrier fields exception : carrier not available", true);
                                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                                        }
                                    }
                                    else  //Carrier found by name
                                    {
                                        objPolicy.CarrierId = cr.CarrierId;
                                        objPolicy.CarrierReference.Value = cr;
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                errMsgPolicy.Add("CarrierCommissionDept", ex.Message);
                                //errorCount++;
                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "CarrierCommissionDept: " + ex.Message);
                                //errorList.Add(m);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: carrier fields exception : " + ex.Message, true);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                //continue;
                            }
                        }
                    }
                    // }
                    #endregion

                    #region Product
                    if (isNewPolicy || objPolicy.CoverageId == null)
                    {
                        if (dt.Columns.Contains("LineOfCoverage"))
                        {
                            try
                            {
                                string strProduct = Convert.ToString(dt.Rows[i]["LineOfCoverage"]);
                                if (!string.IsNullOrEmpty(strProduct))
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy adding LineOfCoverage : " + strProduct, true);
                                    DLinq.Coverage cov = (from p in DataModel.Coverages where p.ProductName == strProduct select p).FirstOrDefault();
                                    if (cov != null) // When product found
                                    {
                                        objPolicy.CoverageId = cov.CoverageId;
                                        objPolicy.CoverageReference.Value = cov;
                                    }
                                    else
                                    {
                                        errMsgPolicy.Add("LineOfCoverage", "Coverage not available");
                                        //errMsgPolicy.Add(new Benefits_ErrorMsg.errMsg("LineOfCoverage", "Coverage not available"));
                                        //errorCount++;
                                        //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "LineOfCoverage: Coverage not available");
                                        //errorList.Add(m);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: Coverage fields exception : Coverage  not available", true);
                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                        //continue;

                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                                errMsgPolicy.Add("LineOfCoverage", ex.Message);
                                //errorCount++;
                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "LineOfCoverage: " + ex.Message);
                                //errorList.Add(m);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: LineOfCoverage : " + ex.Message, true);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                //continue;
                            }
                        }
                    }
                    #endregion

                    #region Product Type - Not to be sent by benefits
                    /*Not to be sent by benefits 
                             * if (isNewPolicy || string.IsNullOrEmpty(objPolicy.ProductType))
                            {
                                if (dt.Columns.Contains("Product_Type"))
                                {
                                    strProductType = Convert.ToString(dt.Rows[i]["Product_Type"]);
                                    DLinq.CoverageNickName covName = (from p in DataModel.CoverageNickNames where p.NickName == strProductType select p).FirstOrDefault();
                                    covNickName = (covName != null) ? covName.NickName : string.Empty; //to be used later in code, so kept separate
                                    objPolicy.ProductType = (!string.IsNullOrEmpty(covNickName)) ? covNickName : strProductType;
                                }
                            }*/
                    #endregion


                    #endregion

                    //Incoming Schedule 
                    #region Incoming Schedule

                    bool allowImportedSchedule = true;
                    Guid SettingsScheduleID = Guid.Empty;
                    bool oldInScheduleExists = false;

                    //Check if old policy and schedules exist
                    if (!isNewPolicy)
                    {
                        PolicyToolIncommingShedule oldSchedule = PolicyToolIncommingShedule.GettingPolicyIncomingSchedule(objPolicy.PolicyId);
                        if (oldSchedule != null && oldSchedule.IncomingScheduleId != Guid.Empty)
                        {
                            if ((oldSchedule.Mode == Mode.Standard && (oldSchedule.FirstYearPercentage != 0 || oldSchedule.RenewalPercentage != 0))
                                || oldSchedule.Mode == Mode.Custom //assuming this will always have value 
                                )
                            {
                                oldInScheduleExists = true;
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy - Old policy and old incoming schedule exists with non-zero values: ", true);
                            }
                        }
                    }

                    int? incomingPaymentType = 1;
                    if (dt.Columns.Contains("Commission Type"))
                    {
                        string strCommisionType = Convert.ToString(dt.Rows[i]["Commission Type"]);
                        incomingPaymentType = PolicCompType(strCommisionType, CompTypeList);
                    }
                    else if (dt.Columns.Contains("CommissionType"))
                    {
                        string strCommisionType = Convert.ToString(dt.Rows[i]["CommissionType"]);
                        incomingPaymentType = PolicCompType(strCommisionType, CompTypeList);
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy incomingPaymentType: " + incomingPaymentType, true);
                    PayorIncomingSchedule payorSchedule = null;

                    //Configure incoming schedule only when not existing or new policy
                    if (isNewPolicy || !oldInScheduleExists)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy - incoming schedule to be read: ", true);
                        objPolicy.IncomingPaymentTypeId = incomingPaymentType;
                        objPolicy.SplitPercentage = 100;

                        inSchedule = new PolicyToolIncommingShedule();
                        inSchedule.PolicyId = objPolicy.PolicyId;
                        inSchedule.IncomingScheduleID = Guid.NewGuid();
                        inSchedule.CustomType = CustomMode.Graded;
                        inSchedule.Mode = Mode.Standard;
                        inSchedule.FirstYearPercentage = 0; //(!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                        inSchedule.RenewalPercentage = 0;

                        string strOutPercentOfPremium = "% of Premium";
                        if (dt.Columns.Contains("Payment Type"))
                        {
                            strOutPercentOfPremium = Convert.ToString(dt.Rows[i]["Payment Type"]);
                        }
                        inSchedule.ScheduleTypeId = (!string.IsNullOrEmpty(strOutPercentOfPremium) && strOutPercentOfPremium.ToLower().Contains("head")) ? 2 : 1;
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy - incoming schedule init with default values : ", true);

                        if (dt.Columns.Contains("Co Broker Split"))
                        {
                            string strSplit = Convert.ToString(dt.Rows[i]["Co Broker Split"]);
                            double splitPer = 0;
                            double.TryParse(strSplit, out splitPer);
                            objPolicy.SplitPercentage = splitPer == 0 ? 100 : splitPer;
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy - Split % read as : " + splitPer + ", set as: " + objPolicy.SplitPercentage, true);
                        }
                        //First check excel for any incoming schedule - if present , import
                        //if (dt.Columns.Contains("Commissions - First Year %") && dt.Columns.Contains("Commissions - Renewal %"))
                        //{
                        if ((dt.Columns.Contains("Commissions - First Year %") || dt.Columns.Contains("CommissionsFirstYear")) && ((dt.Columns.Contains("Commissions - Renewal %") || dt.Columns.Contains("CommissionsRenewal"))))
                        {
                            string strInFirstYear = dt.Columns.Contains("Commissions - First Year %") ? Convert.ToString(dt.Rows[i]["Commissions - First Year %"]) : dt.Columns.Contains("CommissionsFirstYear") ? Convert.ToString(dt.Rows[i]["CommissionsFirstYear"]) : "0";
                            string strInRenewYear = dt.Columns.Contains("Commissions - Renewal %") ? Convert.ToString(dt.Rows[i]["Commissions - Renewal %"]) : dt.Columns.Contains("CommissionsRenewal") ? Convert.ToString(dt.Rows[i]["CommissionsRenewal"]) : "0";
                            double frst = 0; double renew = 0;
                            double.TryParse(strInFirstYear, out frst);
                            double.TryParse(strInRenewYear, out renew);

                            inSchedule.FirstYearPercentage = frst; //(!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                            inSchedule.RenewalPercentage = renew;
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy - first year% read as : " + frst + ", renewal as: " + renew, true);

                            if (frst == 0 && renew == 0)
                            {
                                //get payor configuration if present 
                                if (objPolicy.PayorId != null && objPolicy.CarrierId != null && objPolicy.CoverageId != null && incomingPaymentType != null && !string.IsNullOrEmpty(objPolicy.ProductType))
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy reading payor configuration : ", true);
                                    payorSchedule = PayorIncomingSchedule.GetPayorScheduleDetails((Guid)objPolicy.PayorId, (Guid)objPolicy.CarrierId, (Guid)objPolicy.CoverageId, (Guid)objPolicy.PolicyLicenseeId, objPolicy.ProductType, (int)incomingPaymentType);
                                    if (payorSchedule != null && payorSchedule.IncomingScheduleID != Guid.Empty)
                                    {
                                        allowImportedSchedule = false;
                                        SettingsScheduleID = payorSchedule.IncomingScheduleID;
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration found: SettingsScheduleID - " + SettingsScheduleID, true);
                                        //apply later after policy is saved.
                                    }
                                    else
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration NOT found: " + incomingPaymentType, true);
                                    }
                                }
                            }
                            else
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule init as in excel 1st : " + frst + ", renewal: " + renew, true);
                            }

                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Frst/renewal columns not found in excel, looking for payor config", true);
                            if (objPolicy.PayorId != null && objPolicy.CarrierId != null && objPolicy.CoverageId != null && incomingPaymentType != null && !string.IsNullOrEmpty(objPolicy.ProductType))
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy reading payor configuration : ", true);
                                PayorIncomingSchedule schedule = PayorIncomingSchedule.GetPayorScheduleDetails((Guid)objPolicy.PayorId, (Guid)objPolicy.CarrierId, (Guid)objPolicy.CoverageId, (Guid)objPolicy.PolicyLicenseeId, objPolicy.ProductType, (int)incomingPaymentType);
                                if (schedule != null && schedule.IncomingScheduleID != Guid.Empty)
                                {
                                    allowImportedSchedule = false;
                                    SettingsScheduleID = schedule.IncomingScheduleID;
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration found: SettingsScheduleID - " + SettingsScheduleID, true);
                                    //apply later after policy is saved.
                                }
                                else
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration NOT found: " + incomingPaymentType, true);
                                }
                            }

                        }
                    }

                    #endregion
                    #region Incoming Schedule old

                    /*   PayorIncomingSchedule schedule = null;
                       bool allowImportedSchedule = true;
                       Guid SettingsScheduleID = Guid.Empty;
                       bool oldInScheduleExists = false;
                       DLinq.PolicyIncomingSchedule tempSched = null;
                       //Check if incoming is already set with 0 splits
                       if (objPolicy.IsIncomingBasicSchedule == true)
                       {
                           tempSched = objPolicy.PolicyIncomingSchedules.FirstOrDefault();
                           if (tempSched != null)
                           {
                               if (tempSched.FirstYearPercentage > 0 || tempSched.RenewalPercentage > 0)
                               {
                                   oldInScheduleExists = true;
                               }
                           }
                       }

                       int? incomingPaymentType = 1;
                       if (dt.Columns.Contains("Commission Type"))
                       {
                           string strCommisionType = Convert.ToString(dt.Rows[i]["Commission Type"]);
                           incomingPaymentType = PolicCompType(strCommisionType, CompTypeList);
                       }
                       else if (dt.Columns.Contains("CommissionType"))
                       {
                           string strCommisionType = Convert.ToString(dt.Rows[i]["CommissionType"]);
                           incomingPaymentType = PolicCompType(strCommisionType, CompTypeList);
                       }

                       ActionLogger.Logger.WriteImportPolicyLog("IncomingPaymentType found as: " + incomingPaymentType, true);

                       //New check to overwrite schedule if found configured in the system, else will work the same way, incomingPaymentType taken from excel
                       if (objPolicy.PayorId != null && objPolicy.CarrierId != null && objPolicy.CoverageId != null && incomingPaymentType != null && !string.IsNullOrEmpty(objPolicy.ProductType))
                       {
                           schedule = PayorIncomingSchedule.GetPayorScheduleDetails((Guid)objPolicy.PayorId, (Guid)objPolicy.CarrierId, (Guid)objPolicy.CoverageId, (Guid)objPolicy.PolicyLicenseeId, objPolicy.ProductType, (int)incomingPaymentType);
                           if (schedule != null && schedule.IncomingScheduleID != Guid.Empty)
                           {
                               allowImportedSchedule = false;
                               SettingsScheduleID = schedule.IncomingScheduleID;
                               //apply later after policy is saved.
                           }
                       }
                       if (allowImportedSchedule)
                       {

                           //bool oldInScheduleExists = false;
                           //DLinq.PolicyIncomingSchedule tempSched = null;
                           ////Check if incoming is already set with 0 splits
                           //if (objPolicy.IsIncomingBasicSchedule == true)
                           //{
                           //    tempSched = objPolicy.PolicyIncomingSchedules.FirstOrDefault();
                           //    if (tempSched != null)
                           //    {
                           //        if (tempSched.FirstYearPercentage > 0 || tempSched.RenewalPercentage > 0)
                           //        {
                           //            oldInScheduleExists = true;
                           //        }
                           //        //else if (tempSched.FirstYearPercentage == 0 && tempSched.RenewalPercentage == 0)
                           //        //{
                           //        //    PolicyToolIncommingShedule.DeleteSchedule(objPolicy.PolicyId); //If schedule occurs with 0 split, then delete before adding new
                           //        //}
                           //    }
                           //}

                           //string strCommisionType = Convert.ToString(dt.Rows[i]["Commission Type"]);
                           //objPolicy.IncomingPaymentTypeId = PolicCompType(strCommisionType, CompTypeList);

                           if (isNewPolicy || (objPolicy.IsIncomingBasicSchedule != true) || (!oldInScheduleExists))
                           {
                               if ((dt.Columns.Contains("Commission Type") || dt.Columns.Contains("CommissionType")) && dt.Columns.Contains("Payment Type") && dt.Columns.Contains("Commissions - First Year %") && dt.Columns.Contains("Commissions - Renewal %") && dt.Columns.Contains("Co Broker Split"))
                               {
                                   ActionLogger.Logger.WriteImportPolicyLog("Import Policy incoming schedule add process entered", true);
                                   if (tempSched != null && tempSched.FirstYearPercentage == 0 && tempSched.RenewalPercentage == 0)
                                   {
                                       PolicyToolIncommingShedule.DeleteSchedule(objPolicy.PolicyId); //If schedule occurs with 0 split, then delete before adding new
                                       ActionLogger.Logger.WriteImportPolicyLog("Import Policy old schedule is deleted", true);
                                   }


                                   // string strCommisionType = Convert.ToString(dt.Rows[i]["Commission Type"]);
                                   objPolicy.IncomingPaymentTypeId = incomingPaymentType; // PolicCompType(strCommisionType, CompTypeList);
                                   string strOutPercentOfPremium = Convert.ToString(dt.Rows[i]["Payment Type"]);
                                   string strInFirstYear = Convert.ToString(dt.Rows[i]["Commissions - First Year %"]);
                                   string strInRenewYear = Convert.ToString(dt.Rows[i]["Commissions - Renewal %"]);
                                   string strSplit = Convert.ToString(dt.Rows[i]["Co Broker Split"]);
                                   double splitPer = 0;
                                   double.TryParse(strSplit, out splitPer);
                                   objPolicy.SplitPercentage = splitPer;

                                   inSchedule = new PolicyToolIncommingShedule();
                                   inSchedule.PolicyId = objPolicy.PolicyId;
                                   inSchedule.IncomingScheduleId = Guid.NewGuid();
                                   inSchedule.ScheduleTypeId = (!string.IsNullOrEmpty(strOutPercentOfPremium) && strOutPercentOfPremium.ToLower().Contains("head")) ? 2 : 1;
                                   double frst = 0; double renew = 0;
                                   double.TryParse(strInFirstYear, out frst);
                                   double.TryParse(strInRenewYear, out renew);
                                   inSchedule.FirstYearPercentage = frst; //(!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                                   inSchedule.RenewalPercentage = renew;// (!string.IsNullOrEmpty(strInRenewYear)) ? Convert.ToDouble(strInRenewYear) : 0;
                                   ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule init done : " + policyIDKey, true);
                               }
                               else
                               {
                                   ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule not found in excel, setting default with 0 values ", true);
                                   objPolicy.IncomingPaymentTypeId = incomingPaymentType; // PolicCompType(strCommisionType, CompTypeList);
                                   objPolicy.SplitPercentage = 100;

                                   inSchedule = new PolicyToolIncommingShedule();
                                   inSchedule.PolicyId = objPolicy.PolicyId;
                                   inSchedule.IncomingScheduleId = Guid.NewGuid();
                                   inSchedule.ScheduleTypeId = 1;
                                   inSchedule.FirstYearPercentage = 0; //(!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                                   inSchedule.RenewalPercentage = 0;// (!string.IsNullOrEmpty(strInRenewYear)) ? Convert.ToDouble(strInRenewYear) : 0;
                                   ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule default init done : " + policyIDKey, true);
                               }
                           }
                       }*/
                    #endregion


                    #region Common fields - always update with insert/Update
                    bool OutPercentOfPremium = false;
                    //try
                    //{
                    //Advance - Moved to update always as per Kevin - Aug 21, 2019
                    if (dt.Columns.Contains("AdvancedPaymentNumber"))
                    {
                        string strAdvance = Convert.ToString(dt.Rows[i]["AdvancedPaymentNumber"]);
                        int adv = 0;
                        Int32.TryParse(strAdvance, out adv);

                        if (adv > 0)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Advanced  payment number from file : " + adv + ", updating...", true);
                            objPolicy.Advance = adv;
                        } // (!string.IsNullOrEmpty()) ? Convert.ToInt32(Convert.ToString(dt.Rows[i]["Advanced Payment Number"])) : 0;
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Advanced  payment number from file : " + strAdvance + ", NOT updating...", true);
                        }
                        //string strAdvance = Convert.ToString(dt.Rows[i]["AdvancedPaymentNumber"]);
                        //int adv = 0;
                        //Int32.TryParse(strAdvance, out adv);
                        //objPolicy.Advance = adv; // (!string.IsNullOrEmpty()) ? Convert.ToInt32(Convert.ToString(dt.Rows[i]["Advanced Payment Number"])) : 0;
                    }
                    //Status
                    if (dt.Columns.Contains("PlanStatusDescription") && dt.Rows[i]["PlanStatusDescription"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PlanStatusDescription"])))
                    {
                        try
                        {
                            string strStatus = dt.Rows[i]["PlanStatusDescription"].ToString();
                            objPolicy.PolicyStatusId = (strStatus.ToLower() == "active") ? 0 : (strStatus.ToLower() == "pending") ? 2 : 1;
                        }
                        catch (Exception ex)
                        {
                            errMsgPolicy.Add("PlanStatusDescription", ex.Message);
                            //errorCount++;
                            //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "PlanStatusDescription: " + ex.Message);
                            //errorList.Add(m);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: PlanStatusDescription fields  : " + ex.Message, true);
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            //continue;
                        }
                    }
                    //else if (dt.Columns.Contains("Current Plan Status") && dt.Rows[i]["Current Plan Status"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Current Plan Status"])))
                    //{
                    //    string strStatus = dt.Rows[i]["Current Plan Status"].ToString();
                    //    objPolicy.PolicyStatusId = (strStatus.ToLower() == "active") ? 0 : (strStatus.ToLower() == "pending") ? 2 : 1;
                    //}

                    //Original effective date
                    if (dt.Columns.Contains("OriginalPlanStartDate"))
                    {
                        if (dt.Rows[i]["OriginalPlanStartDate"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["OriginalPlanStartDate"])))
                        {
                            try
                            {
                                string effDate = Convert.ToString(dt.Rows[i]["OriginalPlanStartDate"]);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy string effDate: " + effDate, true);
                                //check if value in double, then fetch OA Date
                                double dblEff = 0;
                                Double.TryParse(effDate, out dblEff);
                                if (dblEff > 0)
                                {
                                    objPolicy.OriginalEffectiveDate = DateTime.FromOADate(dblEff);
                                }
                                else
                                {
                                    objPolicy.OriginalEffectiveDate = DateTime.Parse(effDate, System.Globalization.CultureInfo.CurrentCulture); //Convert.ToDateTime(effDate);
                                }
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy datetime effDate: " + objPolicy.OriginalEffectiveDate, true);
                            }
                            catch (Exception ex)
                            {
                                errMsgPolicy.Add("OriginalPlanStartDate", ex.Message);
                                //errorCount++;
                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "OriginalPlanStartDate: " + ex.Message);
                                //errorList.Add(m);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: OriginalPlanStartDate  fields  : " + ex.Message, true);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                //continue;
                            }
                        }
                    }
                    //Account Exec
                    if (dt.Columns.Contains("AccountOwnerName") || dt.Columns.Contains("AccountOwnerId"))
                    {
                        if (dt.Columns.Contains("AccountOwnerId"))  //if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["AccountOwnerId"])))
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwnerID received: " + Convert.ToString(dt.Rows[i]["AccountOwnerId"]), true);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwnerName " + Convert.ToString(dt.Rows[i]["AccountOwnerName"]), true);
                            if (dt.Rows[i]["AccountOwnerId"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["AccountOwnerId"])))
                            {
                                string currentUserBGUserId = Convert.ToString(dt.Rows[i]["AccountOwnerId"]);
                                Guid userID = Guid.Empty;
                                var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                                if (getBGUserId == null)
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwner: user NOT found in Db  by Owner ID", true);
                                    if (dt.Columns.Contains("AccountOwnerName"))
                                    {
                                        if (dt.Rows[i]["AccountOwnerName"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["AccountOwnerName"])))
                                        {
                                            try
                                            {

                                                string acctExec = Convert.ToString(dt.Rows[i]["AccountOwnerName"]).Trim();
                                                var objUser = AgentList.FirstOrDefault(d => (d.FirstName + " " + d.LastName).ToLower() == acctExec.ToLower() || (!string.IsNullOrEmpty(d.NickName) && d.NickName.ToLower() == acctExec.ToLower()));//.FirstOrDefault(); //User.GetUserIdWise(tempGuid);// (from p in DataModel.UserCredentials where (p.UserCredentialId == tempGuid && p.RoleId == 3 && p.IsDeleted == false) select p).FirstOrDefault();
                                                //Guid tempGuid = Guid.Empty;
                                                //Guid.TryParse(acctExec, out tempGuid);
                                                //if (tempGuid != Guid.Empty)
                                                //{
                                                //    objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                                //}
                                                //Guid tempGuid = new Guid(acctExec);
                                                // User objUser = GlobalAgentList.Where(d => d.UserCredentialID == tempGuid).FirstOrDefault();
                                                if (objUser != null /*&& objUser.Role == UserRole.Agent*/)
                                                {
                                                    ActionLogger.Logger.WriteImportPolicyLog("Account owner found in system by name", true);
                                                    //Need to get nick name
                                                    if (!string.IsNullOrEmpty(objUser.NickName))
                                                    {

                                                        objPolicy.AccoutExec = objUser.NickName;
                                                        objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                                        UpdateBGUserID(objUser.UserCredentialId, currentUserBGUserId);
                                                    }
                                                    else
                                                    {
                                                        objPolicy.AccoutExec = objUser.UserName;
                                                        objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                                    }
                                                    bool isexec = (new User().CheckAccoutExec(objUser.UserCredentialId)); //Sets the flag of accExec true for this userID 
                                                }
                                                else //User not in system, need to add 
                                                {
                                                    errMsgPolicy.Add("AccountOwnerName", "AccountOwner not available");
                                                    ActionLogger.Logger.WriteImportPolicyLog("AccountOwner not found in the system by name ", true);
                                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                errMsgPolicy.Add("AccountOwnerName", ex.Message);
                                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: AccountOwnerName  fields  : " + ex.Message, true);
                                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwner: user found in Db by ID ", true);
                                    userID = getBGUserId.UserCredentialId;
                                    if (userID != null /*&& objUser.Role == UserRole.Agent*/)
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog("Account owner found in system", true);
                                        //Need to get nick name
                                        if (!string.IsNullOrEmpty(getBGUserId.NickName))
                                        {
                                            objPolicy.AccoutExec = getBGUserId.NickName;
                                            objPolicy.UserCredentialId = getBGUserId.UserCredentialId; //tempGuid;
                                        }
                                        else
                                        {
                                            objPolicy.AccoutExec = getBGUserId.UserName;
                                            objPolicy.UserCredentialId = getBGUserId.UserCredentialId; //tempGuid;
                                        }
                                        bool isexec = (new User().CheckAccoutExec(getBGUserId.UserCredentialId)); //Sets the flag of accExec true for this userID 
                                    }
                                    else //User not in system, need to add 
                                    {
                                        errMsgPolicy.Add("AccountOwnerId", "AccountOwnerId not available");
                                        ActionLogger.Logger.WriteImportPolicyLog("Account owner found in system with ID as null ", true);
                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                    }

                                }
                            }
                        }
                        else
                        {
                            if (dt.Columns.Contains("AccountOwnerName"))
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwner Name found ", true);
                                if (dt.Rows[i]["AccountOwnerName"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["AccountOwnerName"])))
                                {
                                    try
                                    {
                                        string acctExec = Convert.ToString(dt.Rows[i]["AccountOwnerName"]).Trim();
                                        var objUser = AgentList.FirstOrDefault(d => (d.FirstName + " " + d.LastName).ToLower() == acctExec.ToLower() || (!string.IsNullOrEmpty(d.NickName) && d.NickName.ToLower() == acctExec.ToLower()));//.FirstOrDefault(); //User.GetUserIdWise(tempGuid);// (from p in DataModel.UserCredentials where (p.UserCredentialId == tempGuid && p.RoleId == 3 && p.IsDeleted == false) select p).FirstOrDefault();

                                        //Guid tempGuid = Guid.Empty;
                                        //Guid.TryParse(acctExec, out tempGuid);
                                        //if (tempGuid != Guid.Empty)
                                        //{
                                        //    objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                        //}
                                        //Guid tempGuid = new Guid(acctExec);
                                        // User objUser = GlobalAgentList.Where(d => d.UserCredentialID == tempGuid).FirstOrDefault();
                                        if (objUser != null /*&& objUser.Role == UserRole.Agent*/)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog("Account owner found in system", true);
                                            //Need to get nick name
                                            if (!string.IsNullOrEmpty(objUser.NickName))
                                            {
                                                objPolicy.AccoutExec = objUser.NickName;
                                                objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                            }
                                            else
                                            {
                                                objPolicy.AccoutExec = objUser.UserName;
                                                objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                            }
                                            bool isexec = (new User().CheckAccoutExec(objUser.UserCredentialId)); //Sets the flag of accExec true for this userID 
                                        }
                                        else //User not in system, need to add 
                                        {
                                            errMsgPolicy.Add("AccountOwnerName", "AccountOwner not available");
                                            ActionLogger.Logger.WriteImportPolicyLog("Account owner NOT found in system by name : ", true);
                                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errMsgPolicy.Add("AccountOwnerName", ex.Message);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: AccountOwnerName  fields  : " + ex.Message, true);
                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                                    }

                                }
                            }
                        }
                    }

                    //Term Date
                    // if (objPolicy.PolicyStatusId == 1)

                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: status is terminated, so updating plan end date and reason ", true);
                    if (dt.Columns.Contains("PlanEndDate"))
                    {
                        if (dt.Rows[i]["PlanEndDate"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PlanEndDate"])))
                        {
                            try
                            {
                                string termDate = Convert.ToString(dt.Rows[i]["PlanEndDate"]);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy string termDate: " + termDate, true);
                                //check if value in double, then fetch OA Date
                                double dblTerm = 0;
                                Double.TryParse(termDate, out dblTerm);
                                if (dblTerm > 0)
                                {
                                    objPolicy.PolicyTerminationDate = DateTime.FromOADate(dblTerm);
                                }
                                else
                                {
                                    objPolicy.PolicyTerminationDate = Convert.ToDateTime(termDate);
                                }
                            }
                            //  objPolicy.PolicyTerminationDate = Convert.ToDateTime(termDate);
                            catch (Exception ex)
                            {
                                errMsgPolicy.Add("PlanEndDate", ex.Message);
                                //errorCount++;
                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "PlanEndDate: " + ex.Message);
                                //errorList.Add(m);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: PlanEndDate  fields  : " + ex.Message, true);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                //continue;
                            }
                        }


                        //Term reason
                        if (dt.Columns.Contains("TerminationReason"))
                        {
                            if (dt.Rows[i]["TerminationReason"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TerminationReason"])))
                            {
                                try
                                {
                                    objPolicy.TerminationReasonId = PolicTermisionID(Convert.ToString(dt.Rows[i]["TerminationReason"]));
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add("TerminationReason", ex.Message);
                                    //errorCount++;
                                    //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "TerminationReason: " + ex.Message);
                                    //errorList.Add(m);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: TerminationReason  fields  : " + ex.Message, true);
                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                    //continue;
                                }
                            }
                        }
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: status is NOT terminated, so ignoring plan end date and reason ", true);
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: mandatory fields init done : " + policyIDKey, true);


                    try
                    {
                        #region Outgoing Split
                        #region new region
                        if (dt.Columns.Contains("Producer1Name") || dt.Columns.Contains("Producer1Id"))
                        {
                            string producerName = ""; string producerID = "";
                            if (dt.Columns.Contains("Producer1Name"))
                            {
                                producerName = Convert.ToString(dt.Rows[i]["Producer1Name"]);
                            }

                            if (dt.Columns.Contains("Producer1Id"))
                            {
                                producerID = Convert.ToString(dt.Rows[i]["Producer1Id"]);
                            }

                            if (!string.IsNullOrWhiteSpace(producerName) || !string.IsNullOrWhiteSpace(producerID))
                            {
                                try
                                {
                                    GetAgentList getdata = new GetAgentList();
                                    getdata.ProducerName = producerName; //Convert.ToString(dt.Rows[i]["Producer1Name"]);
                                    getdata.ProducerId = producerID; // Convert.ToString(dt.Rows[i]["Producer1Id"]);
                                    getdata.ProducerFirstYear = Convert.ToString(dt.Rows[i]["Producer1FirstYear"]);
                                    getdata.ProducerRenewal = Convert.ToString(dt.Rows[i]["Producer1Renewal"]);
                                    getdata.importedPolicyID = importedPolicyID;
                                    getdata.isNewPolicy = isNewPolicy;
                                    getdata.benefits_policyID = benefits_policyID;
                                    getdata.ProducerExecutionId = "producer1Id";
                                    getdata.ProducerExecutionName = "producer1Name";
                                    GetOutgoingSplitdata(getdata, LicID, ref OutGoingField, objPolicy, AgentList, OutPercentOfPremium, ref errMsgPolicy);
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add("Exception while fetching Producer1 details .Please try after sometime.", ex.Message);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer1  fields  : " + ex.Message, true);
                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                }
                            }
                        }
                        if (dt.Columns.Contains("Producer2Name") || dt.Columns.Contains("Producer2Id"))
                        {
                            string producerName = ""; string producerID = "";
                            if (dt.Columns.Contains("Producer2Name"))
                            {
                                producerName = Convert.ToString(dt.Rows[i]["Producer2Name"]);
                            }

                            if (dt.Columns.Contains("Producer2Id"))
                            {
                                producerID = Convert.ToString(dt.Rows[i]["Producer2Id"]);
                            }

                            if (!string.IsNullOrWhiteSpace(producerName) || !string.IsNullOrWhiteSpace(producerID))
                            {
                                //if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer2Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer2Id"])))
                                //{
                                try
                                {
                                    GetAgentList getdata = new GetAgentList();
                                    getdata.ProducerName = producerName;
                                    getdata.ProducerId = producerID;
                                    getdata.ProducerFirstYear = Convert.ToString(dt.Rows[i]["Producer2FirstYear"]);
                                    getdata.ProducerRenewal = Convert.ToString(dt.Rows[i]["Producer2Renewal"]);
                                    getdata.importedPolicyID = importedPolicyID;
                                    getdata.isNewPolicy = isNewPolicy;
                                    getdata.benefits_policyID = benefits_policyID;
                                    getdata.ProducerExecutionId = "producer2Id";
                                    getdata.ProducerExecutionName = "producer2Name";
                                    GetOutgoingSplitdata(getdata, LicID, ref OutGoingField, objPolicy, AgentList, OutPercentOfPremium, ref errMsgPolicy);
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add("Exception while fetching Producer2 details .Please try after sometime.", ex.Message);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer2  fields  : " + ex.Message, true);
                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                }
                            }
                        }
                        if (dt.Columns.Contains("Producer3Name") || dt.Columns.Contains("Producer3Id"))
                        {
                            //if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer3Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer3Id"])))
                            //{
                            string producerName = ""; string producerID = "";
                            if (dt.Columns.Contains("Producer3Name"))
                            {
                                producerName = Convert.ToString(dt.Rows[i]["Producer3Name"]);
                            }

                            if (dt.Columns.Contains("Producer3Id"))
                            {
                                producerID = Convert.ToString(dt.Rows[i]["Producer3Id"]);
                            }

                            if (!string.IsNullOrWhiteSpace(producerName) || !string.IsNullOrWhiteSpace(producerID))
                            {
                                try
                                {
                                    GetAgentList getdata = new GetAgentList();
                                    getdata.ProducerName = producerName;
                                    getdata.ProducerId = producerID;
                                    getdata.ProducerFirstYear = Convert.ToString(dt.Rows[i]["Producer3FirstYear"]);
                                    getdata.ProducerRenewal = Convert.ToString(dt.Rows[i]["Producer3Renewal"]);
                                    getdata.importedPolicyID = importedPolicyID;
                                    getdata.isNewPolicy = isNewPolicy;
                                    getdata.benefits_policyID = benefits_policyID;
                                    getdata.ProducerExecutionId = "producer3Id";
                                    getdata.ProducerExecutionName = "producer3Name";
                                    GetOutgoingSplitdata(getdata, LicID, ref OutGoingField, objPolicy, AgentList, OutPercentOfPremium, ref errMsgPolicy);
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add("Exception while fetching Producer3 details .Please try after sometime.", ex.Message);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer3  fields  : " + ex.Message, true);
                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                }
                            }
                        }
                        if (dt.Columns.Contains("Producer4Name") || dt.Columns.Contains("Producer4Id"))
                        {
                            //if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer4Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer4Id"])))
                            //{
                            string producerName = ""; string producerID = "";
                            if (dt.Columns.Contains("Producer4Name"))
                            {
                                producerName = Convert.ToString(dt.Rows[i]["Producer4Name"]);
                            }

                            if (dt.Columns.Contains("Producer4Id"))
                            {
                                producerID = Convert.ToString(dt.Rows[i]["Producer4Id"]);
                            }

                            if (!string.IsNullOrWhiteSpace(producerName) || !string.IsNullOrWhiteSpace(producerID))
                            {
                                try
                                {
                                    GetAgentList getdata = new GetAgentList();
                                    getdata.ProducerName = producerName; // Convert.ToString(dt.Rows[i]["Producer4Name"]);
                                    getdata.ProducerId = producerID; // Convert.ToString(dt.Rows[i]["Producer4Id"]);
                                    getdata.ProducerFirstYear = Convert.ToString(dt.Rows[i]["Producer4FirstYear"]);
                                    getdata.ProducerRenewal = Convert.ToString(dt.Rows[i]["Producer4Renewal"]);
                                    getdata.importedPolicyID = importedPolicyID;
                                    getdata.isNewPolicy = isNewPolicy;
                                    getdata.benefits_policyID = benefits_policyID;
                                    getdata.ProducerExecutionId = "producer4Id";
                                    getdata.ProducerExecutionName = "producer4Name";
                                    GetOutgoingSplitdata(getdata, LicID, ref OutGoingField, objPolicy, AgentList, OutPercentOfPremium, ref errMsgPolicy);
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add("Exception while fetching Producer4 details .Please try after sometime.", ex.Message);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer4  fields  : " + ex.Message, true);
                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                }
                            }
                        }
                        if (dt.Columns.Contains("Producer5Name") || dt.Columns.Contains("Producer5Id"))
                        {
                            //if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer5Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer5Id"])))
                            //{
                            string producerName = ""; string producerID = "";
                            if (dt.Columns.Contains("Producer5Name"))
                            {
                                producerName = Convert.ToString(dt.Rows[i]["Producer5Name"]);
                            }

                            if (dt.Columns.Contains("Producer5Id"))
                            {
                                producerID = Convert.ToString(dt.Rows[i]["Producer5Id"]);
                            }

                            if (!string.IsNullOrWhiteSpace(producerName) || !string.IsNullOrWhiteSpace(producerID))
                            {
                                try
                                {
                                    GetAgentList getdata = new GetAgentList();
                                    getdata.ProducerName = producerName; // Convert.ToString(dt.Rows[i]["Producer5Name"]);
                                    getdata.ProducerId = producerID; // Convert.ToString(dt.Rows[i]["Producer5Id"]);
                                    getdata.ProducerFirstYear = Convert.ToString(dt.Rows[i]["Producer5FirstYear"]);
                                    getdata.ProducerRenewal = Convert.ToString(dt.Rows[i]["Producer5Renewal"]);
                                    getdata.importedPolicyID = importedPolicyID;
                                    getdata.isNewPolicy = isNewPolicy;
                                    getdata.benefits_policyID = benefits_policyID;
                                    getdata.ProducerExecutionId = "producer5Id";
                                    getdata.ProducerExecutionName = "producer5Name";
                                    GetOutgoingSplitdata(getdata, LicID, ref OutGoingField, objPolicy, AgentList, OutPercentOfPremium, ref errMsgPolicy);
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add("Exception while fetching Producer1 details .Please try after sometime.", ex.Message);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer1  fields  : " + ex.Message, true);
                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                }
                            }
                        }
                        if (dt.Columns.Contains("Producer6Name") || dt.Columns.Contains("Producer6Id"))
                        {
                            //if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer6Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer6Id"])))
                            //{
                            string producerName = ""; string producerID = "";
                            if (dt.Columns.Contains("Producer6Name"))
                            {
                                producerName = Convert.ToString(dt.Rows[i]["Producer6Name"]);
                            }

                            if (dt.Columns.Contains("Producer6Id"))
                            {
                                producerID = Convert.ToString(dt.Rows[i]["Producer6Id"]);
                            }

                            if (!string.IsNullOrWhiteSpace(producerName) || !string.IsNullOrWhiteSpace(producerID))
                            {
                                try
                                {
                                    GetAgentList getdata = new GetAgentList();
                                    getdata.ProducerName = producerName; // Convert.ToString(dt.Rows[i]["Producer6name"]);
                                    getdata.ProducerId = producerID; // Convert.ToString(dt.Rows[i]["Producer6Id"]);
                                    getdata.ProducerFirstYear = Convert.ToString(dt.Rows[i]["Producer6FirstYear"]);
                                    getdata.ProducerRenewal = Convert.ToString(dt.Rows[i]["Producer6Renewal"]);
                                    getdata.importedPolicyID = importedPolicyID;
                                    getdata.isNewPolicy = isNewPolicy;
                                    getdata.benefits_policyID = benefits_policyID;
                                    getdata.ProducerExecutionId = "producer6Id";
                                    getdata.ProducerExecutionName = "producer6Name";
                                    GetOutgoingSplitdata(getdata, LicID, ref OutGoingField, objPolicy, AgentList, OutPercentOfPremium, ref errMsgPolicy);
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add("Exception while fetching Producer6 details .Please try after sometime.", ex.Message);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer6  fields  : " + ex.Message, true);
                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                }
                            }
                        }
                        #endregion
                        //if (dt.Columns.Contains("Producer1Name") || dt.Columns.Contains("Producer1Id"))
                        //{
                        //    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer1Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer1Id"])))
                        //    {
                        //        try
                        //        {
                        //            if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer1Id"])))
                        //            {
                        //                string currentUserBGUserId = Convert.ToString(dt.Rows[i]["Producer1Id"]);
                        //                Guid userID = Guid.Empty;
                        //                var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                        //                if (getBGUserId == null)
                        //                {
                        //                    if (dt.Columns.Contains("Producer1Name") && dt.Columns.Contains("Producer1FirstYear") && dt.Columns.Contains("Producer1Renewal"))
                        //                    {
                        //                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer1Name"])))
                        //                        {
                        //                            string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer1Name"]).Trim();
                        //                            try
                        //                            {
                        //                                var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                                if (objUser == null)
                        //                                {
                        //                                    errMsgPolicy.Add("Producer1Name", "Producer1 not available in the system");
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer1  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                }
                        //                                else
                        //                                {
                        //                                    userID = objUser.UserCredentialId;
                        //                                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer1FirstYear"]), Convert.ToString(dt.Rows[i]["Producer1Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                    if (OutgoingRecord != null)
                        //                                    {

                        //                                        objPolicy.PrimaryAgent = userID;
                        //                                        OutGoingField.Add(OutgoingRecord);
                        //                                        //Add check for updating BgUserId by Ankit-08-10-2018
                        //                                        UpdateBGUserID(userID, currentUserBGUserId);
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        errMsgPolicy.Add("Producer1Name", "Producer1 not available in the system");
                        //                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer1  not available in the system ", true);
                        //                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    }
                        //                                }
                        //                            }
                        //                            catch (Exception ex)
                        //                            {
                        //                                errMsgPolicy.Add("Producer1Name", ex.Message);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer1  fields  : " + ex.Message, true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                // continue;

                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            errMsgPolicy.Add("Producer1Name", "getdata.ProducerId" + "not available in the system");
                        //                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer1Name" + " not available in the system ", true);
                        //                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                        }
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    userID = getBGUserId.UserCredentialId;
                        //                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(currentUserBGUserId, userID, LicID, Convert.ToString(dt.Rows[i]["Producer1FirstYear"]), Convert.ToString(dt.Rows[i]["Producer1Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                    if (OutgoingRecord != null)
                        //                    {
                        //                        OutGoingField.Add(OutgoingRecord);
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("BGUserId", "BGUserId not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: BGUserId  not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (dt.Columns.Contains("Producer1Name") && dt.Columns.Contains("Producer1FirstYear") && dt.Columns.Contains("Producer1Renewal"))
                        //                {
                        //                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer1Name"])))
                        //                    {
                        //                        string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer1Name"]).Trim();
                        //                        try
                        //                        {

                        //                            Guid userID = Guid.Empty;
                        //                            ////Check user on name
                        //                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                            if (objUser == null)
                        //                            {
                        //                                errMsgPolicy.Add("Producer1Name", "Producer1 not available in the system");
                        //                                //errorCount++;
                        //                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                //errorList.Add(m);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer1  not available in the system ", true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                //continue;
                        //                            }
                        //                            else
                        //                            {
                        //                                userID = objUser.UserCredentialId;
                        //                                OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer1FirstYear"]), Convert.ToString(dt.Rows[i]["Producer1Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                if (OutgoingRecord != null)
                        //                                {
                        //                                    objPolicy.PrimaryAgent = userID;
                        //                                    //OutgoingRecord.IsPrimaryAgent = true;
                        //                                    OutGoingField.Add(OutgoingRecord);
                        //                                }
                        //                                else
                        //                                {
                        //                                    errMsgPolicy.Add("Producer1Name", "Producer1 not available in the system");
                        //                                    //errorCount++;
                        //                                    //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                    //    errorList.Add(m);
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer1  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    //continue;
                        //                                }
                        //                            }


                        //                        }
                        //                        catch (Exception ex)
                        //                        {
                        //                            errMsgPolicy.Add("Producer1Name", ex.Message);
                        //                            //errorCount++;
                        //                            //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1 fields: " + ex.Message);
                        //                            //    errorList.Add(m);
                        //                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer1  fields  : " + ex.Message, true);
                        //                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                            //continue;
                        //                        }
                        //                        //}
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    errMsgPolicy.Add("Producer1details", "Producer1details" + "not available in the system");
                        //                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer1details" + " not available in the system ", true);
                        //                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                }
                        //            }
                        //        }

                        //        catch (Exception ex)
                        //        {
                        //            errMsgPolicy.Add("Exception while fetching Producer1 details .Please try after sometime.", ex.Message);
                        //            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer1  fields  : " + ex.Message, true);
                        //            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //            throw ex;
                        //        }
                        //    }
                        //}
                        //if (dt.Columns.Contains("Producer2Name") || dt.Columns.Contains("Producer2Id"))
                        //{
                        //    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer2Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer2Id"])))
                        //    {
                        //        try
                        //        {
                        //            if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer2Id"])))
                        //            {
                        //                string currentUserBGUserId = Convert.ToString(dt.Rows[i]["Producer2Id"]);
                        //                Guid userID = Guid.Empty;
                        //                var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                        //                if (getBGUserId == null)
                        //                {
                        //                    if (dt.Columns.Contains("Producer2Name") && dt.Columns.Contains("Producer2FirstYear") && dt.Columns.Contains("Producer2Renewal"))
                        //                    {
                        //                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer2Name"])))
                        //                        {
                        //                            string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer2Name"]).Trim();
                        //                            try
                        //                            {
                        //                                var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                                if (objUser == null)
                        //                                {
                        //                                    errMsgPolicy.Add("Producer2Name", "Producer2 not available in the system");
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer2  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                }
                        //                                else
                        //                                {
                        //                                    userID = objUser.UserCredentialId;
                        //                                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer2FirstYear"]), Convert.ToString(dt.Rows[i]["Producer2Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                    if (OutgoingRecord != null)
                        //                                    {

                        //                                        objPolicy.PrimaryAgent = userID;
                        //                                        OutGoingField.Add(OutgoingRecord);
                        //                                        //Add check for updating BgUserId by Ankit-08-10-2018
                        //                                        UpdateBGUserID(userID, currentUserBGUserId);
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        errMsgPolicy.Add("Producer2Name", "Producer2 not available in the system");
                        //                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer2  not available in the system ", true);
                        //                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    }
                        //                                }
                        //                            }
                        //                            catch (Exception ex)
                        //                            {
                        //                                errMsgPolicy.Add("Producer2Name", ex.Message);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer2  fields  : " + ex.Message, true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                // continue;

                        //                            }
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("Producer2Name", "Producer2Name" + "not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer2Name" + " not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                    }
                        //                }

                        //                else
                        //                {
                        //                    userID = getBGUserId.UserCredentialId;
                        //                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(currentUserBGUserId, userID, LicID, Convert.ToString(dt.Rows[i]["Producer2FirstYear"]), Convert.ToString(dt.Rows[i]["Producer2Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                    if (OutgoingRecord != null)
                        //                    {
                        //                        OutGoingField.Add(OutgoingRecord);
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("BGUserId", "BGUserId not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: BGUserId  not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (dt.Columns.Contains("Producer2Name") && dt.Columns.Contains("Producer2FirstYear") && dt.Columns.Contains("Producer2Renewal"))
                        //                {
                        //                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer2Name"])))
                        //                    {
                        //                        string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer2Name"]).Trim();
                        //                        try
                        //                        {

                        //                            Guid userID = Guid.Empty;
                        //                            ////Check user on name
                        //                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                            if (objUser == null)
                        //                            {
                        //                                errMsgPolicy.Add("Producer2Name", "Producer2 not available in the system");
                        //                                //errorCount++;
                        //                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                //errorList.Add(m);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer2  not available in the system ", true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                //continue;
                        //                            }
                        //                            else
                        //                            {
                        //                                userID = objUser.UserCredentialId;
                        //                                OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer2FirstYear"]), Convert.ToString(dt.Rows[i]["Producer2Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                if (OutgoingRecord != null)
                        //                                {
                        //                                    objPolicy.PrimaryAgent = userID;
                        //                                    //OutgoingRecord.IsPrimaryAgent = true;
                        //                                    OutGoingField.Add(OutgoingRecord);
                        //                                }
                        //                                else
                        //                                {
                        //                                    errMsgPolicy.Add("Producer2Name", "Producer2 not available in the system");
                        //                                    //errorCount++;
                        //                                    //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                    //    errorList.Add(m);
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer2  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    //continue;
                        //                                }
                        //                            }


                        //                        }
                        //                        catch (Exception ex)
                        //                        {
                        //                            errMsgPolicy.Add("Producer2Name", ex.Message);
                        //                            //errorCount++;
                        //                            //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1 fields: " + ex.Message);
                        //                            //    errorList.Add(m);
                        //                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer2  fields  : " + ex.Message, true);
                        //                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                            //continue;
                        //                        }
                        //                        //}
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    errMsgPolicy.Add("Producer2details", "Producer2details" + "not available in the system");
                        //                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer2details" + " not available in the system ", true);
                        //                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                }
                        //            }
                        //        }

                        //        catch (Exception ex)
                        //        {
                        //            errMsgPolicy.Add("Exception while fetching Producer2 details .Please try after sometime.", ex.Message);
                        //            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer2  fields  : " + ex.Message, true);
                        //            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //            throw ex;
                        //        }
                        //    }
                        //}
                        //if (dt.Columns.Contains("Producer3Name") || dt.Columns.Contains("Producer3Id"))
                        //{
                        //    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer3Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer3Id"])))
                        //    {
                        //        try
                        //        {


                        //            if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer3Id"])))
                        //            {
                        //                string currentUserBGUserId = Convert.ToString(dt.Rows[i]["Producer3Id"]);
                        //                Guid userID = Guid.Empty;
                        //                var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                        //                if (getBGUserId == null)
                        //                {
                        //                    if (dt.Columns.Contains("Producer3Name") && dt.Columns.Contains("Producer3FirstYear") && dt.Columns.Contains("Producer3Renewal"))
                        //                    {
                        //                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer3Name"])))
                        //                        {
                        //                            string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer3Name"]).Trim();
                        //                            try
                        //                            {
                        //                                var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                                if (objUser == null)
                        //                                {
                        //                                    errMsgPolicy.Add("Producer3Name", "Producer3 not available in the system");
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer3  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                }
                        //                                else
                        //                                {
                        //                                    userID = objUser.UserCredentialId;
                        //                                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer3FirstYear"]), Convert.ToString(dt.Rows[i]["Producer3Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                    if (OutgoingRecord != null)
                        //                                    {

                        //                                        objPolicy.PrimaryAgent = userID;
                        //                                        OutGoingField.Add(OutgoingRecord);
                        //                                        //Add check for updating BgUserId by Ankit-08-10-2018
                        //                                        UpdateBGUserID(userID, currentUserBGUserId);
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        errMsgPolicy.Add("Producer3Name", "Producer3 not available in the system");
                        //                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer3 not available in the system ", true);
                        //                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    }
                        //                                }
                        //                            }
                        //                            catch (Exception ex)
                        //                            {
                        //                                errMsgPolicy.Add("Producer3Name", ex.Message);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer3  fields  : " + ex.Message, true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                //continue;
                        //                            }
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("Producer3details", "Producer3details" + "not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer3details" + " not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    userID = getBGUserId.UserCredentialId;
                        //                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(currentUserBGUserId, userID, LicID, Convert.ToString(dt.Rows[i]["Producer3FirstYear"]), Convert.ToString(dt.Rows[i]["Producer3Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                    if (OutgoingRecord != null)
                        //                    {
                        //                        OutGoingField.Add(OutgoingRecord);
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("BGUserId", "BGUserId not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: BGUserId  not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (dt.Columns.Contains("Producer3Name") && dt.Columns.Contains("Producer3FirstYear") && dt.Columns.Contains("Producer3Renewal"))
                        //                {
                        //                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer3Name"])))
                        //                    {
                        //                        string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer3Name"]).Trim();
                        //                        try
                        //                        {

                        //                            Guid userID = Guid.Empty;
                        //                            ////Check user on name
                        //                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                            if (objUser == null)
                        //                            {
                        //                                errMsgPolicy.Add("Producer3Name", "Producer3 not available in the system");
                        //                                //errorCount++;
                        //                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                //errorList.Add(m);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer3  not available in the system ", true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                //continue;
                        //                            }
                        //                            else
                        //                            {
                        //                                userID = objUser.UserCredentialId;
                        //                                OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer3FirstYear"]), Convert.ToString(dt.Rows[i]["Producer3Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                if (OutgoingRecord != null)
                        //                                {
                        //                                    objPolicy.PrimaryAgent = userID;
                        //                                    //OutgoingRecord.IsPrimaryAgent = true;
                        //                                    OutGoingField.Add(OutgoingRecord);
                        //                                }
                        //                                else
                        //                                {
                        //                                    errMsgPolicy.Add("Producer3Name", "Producer3 not available in the system");
                        //                                    //errorCount++;
                        //                                    //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                    //    errorList.Add(m);
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer3  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    // continue;
                        //                                }
                        //                            }


                        //                        }
                        //                        catch (Exception ex)
                        //                        {
                        //                            errMsgPolicy.Add("Producer3Name", ex.Message);
                        //                            //errorCount++;
                        //                            //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1 fields: " + ex.Message);
                        //                            //    errorList.Add(m);
                        //                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer3  fields  : " + ex.Message, true);
                        //                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                            //continue;
                        //                        }
                        //                        //}
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    errMsgPolicy.Add("Producer3details", "Producer3details" + "not available in the system");
                        //                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer3details" + " not available in the system ", true);
                        //                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                }
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            errMsgPolicy.Add("Exception while fetching Producer3 details .Please try after sometime.", ex.Message);
                        //            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer3  fields  : " + ex.Message, true);
                        //            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //            throw ex;
                        //        }
                        //    }
                        //}
                        //if (dt.Columns.Contains("Producer4Name") || dt.Columns.Contains("Producer4Id"))
                        //{
                        //    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer4Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer4Id"])))
                        //    {
                        //        try
                        //        {

                        //            if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer4Id"])))
                        //            {
                        //                string currentUserBGUserId = Convert.ToString(dt.Rows[i]["Producer4Id"]);

                        //                Guid userID = Guid.Empty;
                        //                var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                        //                if (getBGUserId == null)
                        //                {
                        //                    if (dt.Columns.Contains("Producer4Name") && dt.Columns.Contains("Producer4FirstYear") && dt.Columns.Contains("Producer4Renewal"))
                        //                    {
                        //                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer4Name"])))
                        //                        {
                        //                            string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer4Name"]).Trim();
                        //                            try
                        //                            {
                        //                                var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                                if (objUser == null)
                        //                                {
                        //                                    errMsgPolicy.Add("Producer4Name", "Producer4 not available in the system");
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer4  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                }
                        //                                else
                        //                                {
                        //                                    userID = objUser.UserCredentialId;
                        //                                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer4FirstYear"]), Convert.ToString(dt.Rows[i]["Producer4Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                    if (OutgoingRecord != null)
                        //                                    {

                        //                                        objPolicy.PrimaryAgent = userID;
                        //                                        OutGoingField.Add(OutgoingRecord);
                        //                                        //Add check for updating BgUserId by Ankit-08-10-2018
                        //                                        UpdateBGUserID(userID, currentUserBGUserId);
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        errMsgPolicy.Add("Producer4Name", "Producer4 not available in the system");
                        //                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer4 not available in the system ", true);
                        //                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    }
                        //                                }
                        //                            }
                        //                            catch (Exception ex)
                        //                            {
                        //                                errMsgPolicy.Add("Producer4Name", ex.Message);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer4  fields  : " + ex.Message, true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                //continue;
                        //                            }
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("Producer4details", "Producer4details" + "not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer4details" + " not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    userID = getBGUserId.UserCredentialId;
                        //                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(currentUserBGUserId, userID, LicID, Convert.ToString(dt.Rows[i]["Producer4FirstYear"]), Convert.ToString(dt.Rows[i]["Producer4Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                    if (OutgoingRecord != null)
                        //                    {
                        //                        OutGoingField.Add(OutgoingRecord);
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("BGUserId", "BGUserId not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: BGUserId  not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (dt.Columns.Contains("Producer4Name") && dt.Columns.Contains("Producer4FirstYear") && dt.Columns.Contains("Producer4Renewal"))
                        //                {
                        //                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer4Name"])))
                        //                    {
                        //                        string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer4Name"]).Trim();
                        //                        try
                        //                        {

                        //                            Guid userID = Guid.Empty;
                        //                            ////Check user on name
                        //                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                            if (objUser == null)
                        //                            {
                        //                                errMsgPolicy.Add("Producer4Name", "Producer4 not available in the system");
                        //                                //errorCount++;
                        //                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                //errorList.Add(m);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer4  not available in the system ", true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                //continue;
                        //                            }
                        //                            else
                        //                            {
                        //                                userID = objUser.UserCredentialId;
                        //                                OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer4FirstYear"]), Convert.ToString(dt.Rows[i]["Producer4Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                if (OutgoingRecord != null)
                        //                                {
                        //                                    objPolicy.PrimaryAgent = userID;
                        //                                    //OutgoingRecord.IsPrimaryAgent = true;
                        //                                    OutGoingField.Add(OutgoingRecord);
                        //                                }
                        //                                else
                        //                                {
                        //                                    errMsgPolicy.Add("Producer4Name", "Producer4 not available in the system");
                        //                                    //errorCount++;
                        //                                    //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                    //    errorList.Add(m);
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer4  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    //continue;
                        //                                }
                        //                            }


                        //                        }
                        //                        catch (Exception ex)
                        //                        {
                        //                            errMsgPolicy.Add("Producer4Name", ex.Message);
                        //                            //errorCount++;
                        //                            //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1 fields: " + ex.Message);
                        //                            //    errorList.Add(m);
                        //                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer4  fields  : " + ex.Message, true);
                        //                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                            // continue;
                        //                        }
                        //                        //}
                        //                    }
                        //                }

                        //                else
                        //                {
                        //                    errMsgPolicy.Add("Producer3details", "Producer3details" + "not available in the system");
                        //                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer3details" + " not available in the system ", true);
                        //                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                }

                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            errMsgPolicy.Add("Exception while fetching Producer4 details .Please try after sometime.", ex.Message);
                        //            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer4  fields  : " + ex.Message, true);
                        //            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //            throw ex;
                        //        }
                        //    }

                        //}
                        //if (dt.Columns.Contains("Producer5Name") || dt.Columns.Contains("Producer5Id"))
                        //{
                        //    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer5Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer5Id"])))
                        //    {
                        //        try
                        //        {

                        //            if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer5Id"])))
                        //            {
                        //                string currentUserBGUserId = Convert.ToString(dt.Rows[i]["Producer5Id"]);
                        //                Guid userID = Guid.Empty;
                        //                var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                        //                if (getBGUserId == null)
                        //                {
                        //                    if (dt.Columns.Contains("Producer5Name") && dt.Columns.Contains("Producer5FirstYear") && dt.Columns.Contains("Producer5Renewal"))
                        //                    {
                        //                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer5Name"])))
                        //                        {
                        //                            string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer5Name"]).Trim();
                        //                            try
                        //                            {
                        //                                var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                                if (objUser == null)
                        //                                {
                        //                                    errMsgPolicy.Add("Producer5Name", "Producer5 not available in the system");
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer5  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                }
                        //                                else
                        //                                {
                        //                                    userID = objUser.UserCredentialId;
                        //                                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer5FirstYear"]), Convert.ToString(dt.Rows[i]["Producer5Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                    if (OutgoingRecord != null)
                        //                                    {

                        //                                        objPolicy.PrimaryAgent = userID;
                        //                                        OutGoingField.Add(OutgoingRecord);
                        //                                        //Add check for updating BgUserId by Ankit-08-10-2018
                        //                                        UpdateBGUserID(userID, currentUserBGUserId);
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        errMsgPolicy.Add("Producer5Name", "Producer5 not available in the system");
                        //                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer5 not available in the system ", true);
                        //                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    }
                        //                                }
                        //                            }
                        //                            catch (Exception ex)
                        //                            {
                        //                                errMsgPolicy.Add("Producer5Name", ex.Message);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer5  fields  : " + ex.Message, true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                // continue;
                        //                            }
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("Producer5details", "Producer5details" + "not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer5details" + " not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    userID = getBGUserId.UserCredentialId;
                        //                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(currentUserBGUserId, userID, LicID, Convert.ToString(dt.Rows[i]["Producer5FirstYear"]), Convert.ToString(dt.Rows[i]["Producer5Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                    if (OutgoingRecord != null)
                        //                    {
                        //                        OutGoingField.Add(OutgoingRecord);
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("BGUserId", "BGUserId not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: BGUserId  not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (dt.Columns.Contains("Producer5Name") && dt.Columns.Contains("Producer5FirstYear") && dt.Columns.Contains("Producer5Renewal"))
                        //                {
                        //                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer5Name"])))
                        //                    {
                        //                        string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer5Name"]).Trim();
                        //                        try
                        //                        {

                        //                            Guid userID = Guid.Empty;
                        //                            ////Check user on name
                        //                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                            if (objUser == null)
                        //                            {
                        //                                errMsgPolicy.Add("Producer5Name", "Producer5 not available in the system");
                        //                                //errorCount++;
                        //                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                //errorList.Add(m);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer5  not available in the system ", true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                //continue;
                        //                            }
                        //                            else
                        //                            {
                        //                                userID = objUser.UserCredentialId;
                        //                                OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer5FirstYear"]), Convert.ToString(dt.Rows[i]["Producer5Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                if (OutgoingRecord != null)
                        //                                {
                        //                                    objPolicy.PrimaryAgent = userID;
                        //                                    //OutgoingRecord.IsPrimaryAgent = true;
                        //                                    OutGoingField.Add(OutgoingRecord);
                        //                                }
                        //                                else
                        //                                {
                        //                                    errMsgPolicy.Add("Producer5Name", "Producer5 not available in the system");
                        //                                    //errorCount++;
                        //                                    //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                    //    errorList.Add(m)
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer5  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    //continue;
                        //                                }
                        //                            }


                        //                        }
                        //                        catch (Exception ex)
                        //                        {
                        //                            errMsgPolicy.Add("Producer5Name", ex.Message);
                        //                            //errorCount++;
                        //                            //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1 fields: " + ex.Message);
                        //                            //    errorList.Add(m);
                        //                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer5  fields  : " + ex.Message, true);
                        //                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                            // continue;
                        //                        }
                        //                        //}
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    errMsgPolicy.Add("Producer5details", "Producer5details" + "not available in the system");
                        //                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer5details" + " not available in the system ", true);
                        //                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                }
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            errMsgPolicy.Add("Exception while fetching Producer5 details .Please try after sometime.", ex.Message);
                        //            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer5  fields  : " + ex.Message, true);
                        //            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //            throw ex;
                        //        }
                        //    }
                        //}
                        //if (dt.Columns.Contains("Producer6Name") || dt.Columns.Contains("Producer6Id"))
                        //{
                        //    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer6Name"])) || !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer6Id"])))
                        //    {
                        //        try
                        //        {


                        //            if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer6Id"])))
                        //            {
                        //                string currentUserBGUserId = Convert.ToString(dt.Rows[i]["Producer6Id"]);
                        //                Guid userID = Guid.Empty;
                        //                var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                        //                if (getBGUserId == null)
                        //                {
                        //                    if (dt.Columns.Contains("Producer6Name") && dt.Columns.Contains("Producer6FirstYear") && dt.Columns.Contains("Producer6Renewal"))
                        //                    {
                        //                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer6Name"])))
                        //                        {
                        //                            string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer6Name"]).Trim();
                        //                            try
                        //                            {
                        //                                var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                                if (objUser == null)
                        //                                {
                        //                                    errMsgPolicy.Add("Producer6Name", "Producer6 not available in the system");
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer6  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                }
                        //                                else
                        //                                {
                        //                                    userID = objUser.UserCredentialId;
                        //                                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer6FirstYear"]), Convert.ToString(dt.Rows[i]["Producer5Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                    if (OutgoingRecord != null)
                        //                                    {

                        //                                        objPolicy.PrimaryAgent = userID;
                        //                                        OutGoingField.Add(OutgoingRecord);
                        //                                        //Add check for updating BgUserId by Ankit-08-10-2018
                        //                                        UpdateBGUserID(userID, currentUserBGUserId);
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        errMsgPolicy.Add("Producer6Name", "Producer6 not available in the system");
                        //                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer6 not available in the system ", true);
                        //                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    }
                        //                                }
                        //                            }
                        //                            catch (Exception ex)
                        //                            {
                        //                                errMsgPolicy.Add("Producer6Name", ex.Message);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer6  fields  : " + ex.Message, true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                //continue;
                        //                            }
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("Producer6details", "Producer3details" + "not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer6details" + " not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    userID = getBGUserId.UserCredentialId;
                        //                    OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(currentUserBGUserId, userID, LicID, Convert.ToString(dt.Rows[i]["Producer6FirstYear"]), Convert.ToString(dt.Rows[i]["Producer6Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                    if (OutgoingRecord != null)
                        //                    {
                        //                        OutGoingField.Add(OutgoingRecord);
                        //                    }
                        //                    else
                        //                    {
                        //                        errMsgPolicy.Add("BGUserId", "BGUserId not available in the system");
                        //                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: BGUserId  not available in the system ", true);
                        //                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (dt.Columns.Contains("Producer6Name") && dt.Columns.Contains("Producer6FirstYear") && dt.Columns.Contains("Producer6Renewal"))
                        //                {
                        //                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer6Name"])))
                        //                    {
                        //                        string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer6Name"]).Trim();
                        //                        try
                        //                        {

                        //                            Guid userID = Guid.Empty;
                        //                            ////Check user on name
                        //                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                        //                            if (objUser == null)
                        //                            {
                        //                                errMsgPolicy.Add("Producer6Name", "Producer6 not available in the system");
                        //                                //errorCount++;
                        //                                //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                //errorList.Add(m);
                        //                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer6  not available in the system ", true);
                        //                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                //continue;
                        //                            }
                        //                            else
                        //                            {
                        //                                userID = objUser.UserCredentialId;
                        //                                OutGoingPayment OutgoingRecord = GetOutgoingPaymentForImport(strPrimaryBroker, userID, LicID, Convert.ToString(dt.Rows[i]["Producer6FirstYear"]), Convert.ToString(dt.Rows[i]["Producer6Renewal"]), objPolicy.PolicyId, OutPercentOfPremium);
                        //                                if (OutgoingRecord != null)
                        //                                {
                        //                                    objPolicy.PrimaryAgent = userID;
                        //                                    //OutgoingRecord.IsPrimaryAgent = true;
                        //                                    OutGoingField.Add(OutgoingRecord);
                        //                                }
                        //                                else
                        //                                {
                        //                                    errMsgPolicy.Add("Producer6Name", "Producer6 not available in the system");
                        //                                    //errorCount++;
                        //                                    //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: Producer1 not available in the system ");
                        //                                    //    errorList.Add(m)
                        //                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer6  not available in the system ", true);
                        //                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                                    //continue;
                        //                                }
                        //                            }


                        //                        }
                        //                        catch (Exception ex)
                        //                        {
                        //                            errMsgPolicy.Add("Producer6Name", ex.Message);
                        //                            //errorCount++;
                        //                            //    Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1 fields: " + ex.Message);
                        //                            //    errorList.Add(m);
                        //                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer6  fields  : " + ex.Message, true);
                        //                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                            // continue;
                        //                        }
                        //                        //}
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    errMsgPolicy.Add("Producer6details", "Producer3details" + "not available in the system");
                        //                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + "Producer6details" + " not available in the system ", true);
                        //                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //                }

                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            errMsgPolicy.Add("Exception while fetching Producer6 details .Please try after sometime.", ex.Message);
                        //            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: Producer6  fields  : " + ex.Message, true);
                        //            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //            throw ex;
                        //        }
                        //    }
                        //}
                        #endregion

                        #endregion
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: outgoing split init done : " + policyIDKey, true);
                    }
                    catch (Exception ex)
                    {
                        errMsgPolicy.Add("ProducerName", "ProducerName not available in the system");
                        //errorCount++;
                        //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: " + ex.Message);
                        //errorList.Add(m);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: outgoing fields  : " + ex.Message, true);
                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //continue;

                    }

                    #region Track date - checked at last [after origin date found]
                    /* Not to be sent by benefits, poulated from agency's default track date so, no exception throw from here as well*/
                    if (isNewPolicy || objPolicy.TrackFromDate == null)
                    {

                        //if (dt.Columns.Contains("Track_From"))
                        //{
                        //if (dt.Rows[i]["Track_From"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Track_From"])))
                        if (dtTrack != null && dtTrack != DateTime.MinValue)
                        {
                            objPolicy.TrackFromDate = (objPolicy.OriginalEffectiveDate != null && objPolicy.OriginalEffectiveDate > dtTrack) ? objPolicy.OriginalEffectiveDate : dtTrack;
                        }
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy TRack Date : " + objPolicy.TrackFromDate, true);
                    }
                    #endregion

                    #region Save Data

                    //Check if any error, then return 
                    if (errMsgPolicy.Count > 0)
                    {
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        errorCount++;
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, output);
                        errorList.Add(m);
                        continue;
                    }

                    try
                    {
                        //Save only when policy ID not present in error list
                        if (errorList == null || (errorList != null && errorList.Count == 0) || (errorList != null && errorList.Count > 0 && errorList.Where(x => x.OriginalPlanID == importedPolicyID).ToList().Count == 0))
                        {
                            #region Client
                            try
                            {

                                if (isNewPolicy || objPolicy.PolicyClientId == null)
                                {
                                    string client = clientName;
                                    Client objClient = (new Client()).GetClientByClientName(client, LicID);
                                    //Get Client ID by Get Client name
                                    if (objClient == null)
                                    {
                                        //Create new client
                                        objClient = new Client();
                                        objClient.ClientId = Guid.NewGuid();
                                        string strClientValue = string.Empty;

                                        strClientValue = (client.Length > 49) ? client.Substring(0, 49) : client;

                                        //objClnt.Name = policy.ClientName;
                                        objClient.Name = strClientValue;
                                        objClient.LicenseeId = LicID;
                                        objClient.IsDeleted = false;
                                        Client.AddUpdateClient(client, LicID, objClient.ClientId);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: client saved as new : " + policyIDKey, true);
                                    }
                                    objPolicy.ClientReference.Value = (from p in DataModel.Clients where p.ClientId == objClient.ClientId select p).FirstOrDefault();
                                }
                                //following requires client reference, so added at last
                                if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Insured))
                                {
                                    if (dt.Columns.Contains("AccountName"))
                                    {
                                        objPolicy.Insured = Convert.ToString(dt.Rows[i]["AccountName"]);
                                    }
                                    else
                                    {
                                        objPolicy.Insured = objPolicy.Client.Name;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                errorCount++; //Allow this code here , as thisis the last error possibility
                                errMsgPolicy.Add("AccountName", ex.Message);
                                string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                                Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, output);
                                errorList.Add(m);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: client : " + ex.Message, true);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                continue;
                            }


                            #endregion
                            //try
                            //{
                            if (isNewPolicy)
                            {
                                DataModel.AddToPolicies(objPolicy);
                                DataModel.SaveChanges();
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy : policy saved successfully", true);
                                PolicyToLearnPost.AddLearnedAfterImport(objPolicy.PolicyId, "", covNickName, strProductType, importedPolicyID);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy : learned fields saved successfully", true);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Policy saved as new : " + policyIDKey, true);

                            }
                            else
                            {
                                DataModel.SaveChanges();
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy : policy updated successfully", true);
                                AddUpdatePolicyHistory(objPolicy.PolicyId);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy : policy history updated successfully", true);
                                //Learned fields to update , once policy fields are updated 
                                PolicyToLearnPost.AddLearnedAfterImport(objPolicy.PolicyId, "", covNickName, strProductType, importedPolicyID);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy : learned fields saved successfully", true);
                                PolicyLearnedField.AddUpdateHistoryLearned(objPolicy.PolicyId);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Policy updated : " + policyIDKey, true);
                            }

                            /*    if (SettingsScheduleID != Guid.Empty)
                                {
                                    UpdatePolicySchedule(SettingsScheduleID, objPolicy.PolicyId, objPolicy.Advance, payorSchedule);
                                }
                                //Save Incoming and delete old 
                                else if (inSchedule != null)
                                {
                                    //if (tempSched != null && tempSched.FirstYearPercentage == 0 && tempSched.RenewalPercentage == 0)
                                    //{
                                    //    PolicyToolIncommingShedule.DeleteSchedule(objPolicy.PolicyId); //If schedule occurs with 0 split, then delete before adding new
                                    //    ActionLogger.Logger.WriteImportPolicyLog("Import Policy old schedule is deleted", true);
                                    //}
                                    //inSchedule.AddUpdate();
                                    ActionLogger.Logger.WriteImportPolicyLog("Incoming schedule added for the policy", true);
                                }*/
                            //Save Incoming 
                            if (SettingsScheduleID != Guid.Empty)
                            {
                                UpdatePolicySchedule(SettingsScheduleID, objPolicy.PolicyId, objPolicy.Advance, payorSchedule);
                            }
                            else if (inSchedule != null)
                            {
                                PolicyToolIncommingShedule.SavePolicyIncomingSchedule(inSchedule);
                                //inSchedule.AddUpdate();
                                ActionLogger.Logger.WriteImportPolicyLog("Incoming schedule added for the policy", true);
                            }

                            //Save Outgoing
                            //Delete old schdule- outgoing  payments witj old schedule will remian intact, new will take effect on new entries only.
                            //Check only if new outgoing schedule exists, then overwrite the old
                            //This is to exclude the case when policy is existing and excel has blank entries. Without this, existing schedule will be overwritten
                            // by 100% to house for existing policy 
                            if (isNewPolicy || OutGoingField.Count > 0)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule adding to the policy", true);
                                OutGoingField = CompleteOutgoingSchedule(OutGoingField, objPolicy.PolicyId, LicID);
                                OutGoingPayment.DeletePolicyOutGoingSchedulebyPolicyId(objPolicy.PolicyId);
                                ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule removed from the policy", true);
                                OutGoingPayment.AddUpdate(OutGoingField);
                                ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule added for the policy", true);
                            }

                            if (isNewPolicy)
                                addCount++;
                            else
                                updateCount++;

                            //Adding status in DB
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, true, benefits_policyID, agencyName);
                        }

                        #endregion

                    }
                    catch (Exception ex)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Exception adding policy: " + ex.Message, true);
                        errorCount++;
                        errMsgPolicy.Add("SaveError", ex.Message);
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, output);
                        errorList.Add(m);
                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                    }
                }

                objStatus.NewCount = addCount;
                objStatus.UpdateCount = updateCount;
                objStatus.ErrorCount = errorCount;
                objStatus.ErrorList = errorList;
                return objStatus;
            }
        }

        static void UpdateCoverageForBG(DataTable dt, int rowIndex, string importedPolicyID, string benefits_policyID, bool isNewPolicy, DLinq.Policy objPolicy, DLinq.CommissionDepartmentEntities DataModel, Guid LicID, ref Dictionary<string, string> errMsgPolicy, string agencyName = "")
        {
            if (isNewPolicy || objPolicy.CoverageId == null)
            {
                string colName = "LineOfCoverage";
                if(Convert.ToString(LicID) == "8994CB52-101C-4A78-BFFF-414A0563A92D") // for UIS Insurance
                {
                    colName = "CoverageType__c";
                }
                if (dt.Columns.Contains(colName))
                {
                    try
                    {
                        string strProduct = Convert.ToString(dt.Rows[rowIndex][colName]);
                        if (!string.IsNullOrEmpty(strProduct))
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy adding " + colName + " : " + strProduct, true, agencyName);
                            DLinq.Coverage cov = (from p in DataModel.Coverages where p.ProductName == strProduct select p).FirstOrDefault();
                            if (cov != null) // When product found
                            {
                                objPolicy.CoverageId = cov.CoverageId;
                                objPolicy.CoverageReference.Value = cov;
                            }
                            else
                            {
                                errMsgPolicy.Add(colName, "Coverage not available");
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: Coverage fields exception : Coverage  not available", true, agencyName);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errMsgPolicy.Add(colName, ex.Message);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: " + colName + " : " + ex.Message, true, agencyName);
                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                    }
                }
            }
        }
        static DLinq.Carrier UpdateCarrierForBG(DataTable dt, int rowIndex, string importedPolicyID, string benefits_policyID, bool isNewPolicy, DLinq.CommissionDepartmentEntities DataModel, Guid LicID, ref Dictionary<string, string> errMsgPolicy, string agencyName = "")
        {
            DLinq.Carrier carrer = null;
            if (dt.Columns.Contains("CarrierCommissionDept"))
            {
                try
                {
                    string strCarr = Convert.ToString(dt.Rows[rowIndex]["CarrierCommissionDept"]);
                    if (!string.IsNullOrEmpty(strCarr))
                    {
                        //using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                        //{
                        DLinq.Carrier cr = (from p in DataModel.Carriers where (p.CarrierName.ToLower() == strCarr.ToLower() && (p.IsGlobal || (!p.IsGlobal && p.LicenseeId == LicID))) select p).FirstOrDefault();
                        if (cr == null)
                        {
                            DLinq.CarrierNickName crN = (from p in DataModel.CarrierNickNames where p.NickName == strCarr select p).FirstOrDefault();
                            if (crN != null) //Carrier found in system by nickname
                            {
                                carrer = crN.CarrierReference.Value;
                            }
                            else
                            {
                                errMsgPolicy.Add("CarrierCommissionDept", "Carrier not available");
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: carrier fields exception : carrier not available", true, agencyName);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                            }
                        }
                        else  //Carrier found by name
                        {
                            carrer = cr;
                        }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("CarrierCommissionDept", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: carrier fields exception : " + ex.Message, true, agencyName);
                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                }
            }
            return carrer;
        }
        static DLinq.Payor UpdatePayorForBG(DataTable dt, int rowIndex, string importedPolicyID, string benefits_policyID, bool isNewPolicy, DLinq.CommissionDepartmentEntities DataModel, Guid LicID, ref Dictionary<string, string> errMsgPolicy, string agencyName = "")
        {
            DLinq.Payor pyor = null;
            if (dt.Columns.Contains("PayorCommissionDept"))
            {
                try
                {
                    string strPayor = Convert.ToString(dt.Rows[rowIndex]["PayorCommissionDept"]);
                    if (!string.IsNullOrEmpty(strPayor))
                    {
                        //Compare payors - if name matches -  payor to be global or belonging to same agency.
                        //using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                        //{
                        DLinq.Payor py = (from p in DataModel.Payors
                                          where ((p.PayorName.ToLower() == strPayor.ToLower() || (p.PayorName.ToLower() != strPayor.ToLower() && p.NickName.ToLower() == strPayor.ToLower())))
               && ((p.IsGlobal || (!p.IsGlobal && p.LicenseeId == LicID)))
                                          select p).FirstOrDefault();

                        if (py != null) //If found existing 
                        {
                            pyor = py;
                            //objPolicy.PayorId = py.PayorId;
                            //objPolicy.PayorReference.Value = py;
                        }
                        else  // if not existing, return error  
                        {
                            errMsgPolicy.Add("PayorCommissionDept", "Payor not available");
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: payor fields exception : Payor not available", true, agencyName);
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                        }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("PayorCommissionDept", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: payor fields exception : " + ex.Message, true, agencyName);
                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                }
            }
            return pyor;
        }

        public static string calculatePolicyType(DateTime? OriginalEffectiveDate, Guid? ClientId, Guid LicenseeId, Guid PolicyId, Guid? CoverageId)
        {
            ActionLogger.Logger.WriteImportPolicyLog("calculatePolicyType start", true);

            string PolicyType = "";
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    //using (SqlCommand cmd = new SqlCommand("Select Count(1) from Policies where PolicyClientId ='" + ClientId + "'", con))
                    using (SqlCommand cmd = new SqlCommand("sp_getPolicyType", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ClientId", ClientId);
                        cmd.Parameters.AddWithValue("@CoverageId", CoverageId);
                        cmd.Parameters.AddWithValue("@LicenseeId", LicenseeId);
                        cmd.Parameters.AddWithValue("@OriginalEffectiveDate", OriginalEffectiveDate);
                        cmd.Parameters.AddWithValue("@PolicyId", PolicyId);
                        cmd.Parameters.Add("@PolicyType", SqlDbType.VarChar, 50);
                        cmd.Parameters["@PolicyType"].Direction = ParameterDirection.Output;

                        int i = cmd.ExecuteNonQuery();

                        PolicyType = Convert.ToString(cmd.Parameters["@PolicyType"].Value);

                        ActionLogger.Logger.WriteImportPolicyLog("calculatePolicyType PolicyType :" + PolicyType, true);
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("calculatePolicyType exception: " + ex.Message, true);
            }
            return PolicyType;
        }

        static void UpdateBenefitsOptionalFields(DataTable dt, int rowIndex, string importedPolicyID, string benefits_policyID, bool isNewPolicy, ref DLinq.Policy objPolicy, ref Dictionary<string, string> errMsgPolicy, Guid LicID, string agencyName = "")
        {
            #region remove policytype calculation becuaue it calculated based upon the values used in the policies
            ////if (isNewPolicy || string.IsNullOrEmpty(objPolicy.PolicyType))
            ////{
            //string PolicyType = "";
            //try
            //{
            //    //seeA
            //    //if (dt.Columns.Contains("NewBusiness")) //|| dt.Columns.Contains("New Business?"))
            //    //{
            //    //    string strTypeOfPolicy = Convert.ToString(dt.Rows[rowIndex]["NewBusiness"]); //dt.Columns.Contains("New?") ? Convert.ToString(dt.Rows[i]["New?"]) : (dt.Columns.Contains("New Business?") ? Convert.ToString(dt.Rows[i]["New Business?"]) : "");
            //    //    objPolicy.PolicyType = (!string.IsNullOrEmpty(strTypeOfPolicy) && (strTypeOfPolicy.ToLower() == "rewrite" || strTypeOfPolicy.ToLower() == "replace")) ? "Replace" : "New";
            //    //}
            //    //else if (isNewPolicy)
            //    //{
            //    //    objPolicy.PolicyType = "New";
            //    //}


            //    //if(isNewPolicy)
            //    //{
            //    //    objPolicy.IsManuallyChanged = false;
            //    //}


            //    objPolicy.IsManuallyChanged = isNewPolicy ? false : objPolicy.IsManuallyChanged;

            //    if (objPolicy.IsManuallyChanged == false)
            //    {
            //        PolicyType = calculatePolicyType(objPolicy.OriginalEffectiveDate, objPolicy.PolicyClientId, LicID, objPolicy.PolicyId, objPolicy.CoverageId);
            //        objPolicy.PolicyType = PolicyType;
            //    }
            //    //seeA
            //}
            //catch (Exception ex)
            //{
            //    errMsgPolicy.Add("NewBusiness", ex.Message);
            //    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: New Business field  exception : " + ex.Message, true, agencyName);
            //    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
            //}
            ////} 
            #endregion
            //PolicyNumber
            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.PolicyNumber))
            {
                string colName = "Group";
                if (Convert.ToString(LicID) == "8994CB52-101C-4A78-BFFF-414A0563A92D")
                {
                    colName = "Policy_Number__c";
                }
                try
                {
                    if (dt.Columns.Contains(colName))
                    {
                        objPolicy.PolicyNumber = Convert.ToString(dt.Rows[rowIndex][colName]);
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add(colName, ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: " + colName + " fields exception : " + ex.Message, true, agencyName);
                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                }
            }
            //Enrolled
            if (Convert.ToString(LicID) == "8994CB52-101C-4A78-BFFF-414A0563A92D")
            {
                try
                {
                    if (dt.Columns.Contains("NumberofTotalEmployees__c"))
                    {
                        objPolicy.Enrolled = Convert.ToString(dt.Rows[rowIndex]["NumberofTotalEmployees__c"]);
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("NumberofTotalEmployees__c", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: NumberofTotalEmployees__c fields exception : " + ex.Message, true, agencyName);
                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                }
            }
            else
            {
                if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Enrolled))
                {
                    try
                    {
                        if (dt.Columns.Contains("NumberofCoveredLives"))
                        {
                            objPolicy.Enrolled = Convert.ToString(dt.Rows[rowIndex]["NumberofCoveredLives"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        errMsgPolicy.Add("NumberofCoveredLives", ex.Message);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: NumberofCoveredLives fields exception : " + ex.Message, true, agencyName);
                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                    }
                }
            }
            //Eligible
            if (Convert.ToString(LicID) == "8994CB52-101C-4A78-BFFF-414A0563A92D")
            {
                try
                {
                    if (dt.Columns.Contains("NumberofTotalEmployees__c"))
                    {
                        objPolicy.Eligible = Convert.ToString(dt.Rows[rowIndex]["NumberofTotalEmployees__c"]);
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("NumberofTotalEmployees__c", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: NumberofTotalEmployees__c fields exception : " + ex.Message, true, agencyName);
                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                }
            }
            else
            {
                if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Eligible))
                {
                    try
                    {
                        if (dt.Columns.Contains("Eligible"))
                        {
                            objPolicy.Eligible = Convert.ToString(dt.Rows[rowIndex]["Eligible"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        errMsgPolicy.Add("Eligible", ex.Message);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: Eligible fields exception : " + ex.Message, true, agencyName);
                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                    }
                }
            }
            //PolicyModeId
            if (isNewPolicy || objPolicy.PolicyModeId == null)
            {
                try
                {
                    if (dt.Columns.Contains("ModalNumber"))
                    {
                        string strMode = Convert.ToString(dt.Rows[rowIndex]["ModalNumber"]);
                        objPolicy.PolicyModeId = (!string.IsNullOrEmpty(strMode)) ? PolicyModeID(strMode) : PolicyModeID("0");
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("ModalNumber", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: ModalNumber fields exception : " + ex.Message, true, agencyName);
                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                }
            }

            //premium
            if (Convert.ToString(LicID) == "8994CB52-101C-4A78-BFFF-414A0563A92D")
            {
                try
                {
                    if (dt.Columns.Contains("MonthlyPremium__c"))
                    {
                        string strPremiuum = Convert.ToString(dt.Rows[rowIndex]["MonthlyPremium__c"]);
                        decimal prem = 0;
                        decimal.TryParse(strPremiuum, out prem);
                        if (prem == 0)
                        {
                            decimal.TryParse(strPremiuum, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out prem);
                        }
                        objPolicy.MonthlyPremium = prem;
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("MonthlyPremium__c", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: MonthlyPremium__c fields exception : " + ex.Message, true, agencyName);
                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                }
            }
            else
            {
                if (isNewPolicy || objPolicy.MonthlyPremium == null)
                {
                    try
                    {
                        if (dt.Columns.Contains("ModalPremium"))
                        {
                            string strPremiuum = Convert.ToString(dt.Rows[rowIndex]["ModalPremium"]);
                            decimal prem = 0;
                            decimal.TryParse(strPremiuum, out prem);
                            if (prem == 0)
                            {
                                decimal.TryParse(strPremiuum, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out prem);
                            }
                            objPolicy.MonthlyPremium = prem;// (!string.IsNullOrEmpty(strPremiuum)) ? Convert.ToDecimal(strPremiuum) : 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        errMsgPolicy.Add("ModalPremium", ex.Message);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: ModalPremium fields exception : " + ex.Message, true, agencyName);
                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                    }
                }
            }

            //Original effective date
            if(Convert.ToString(LicID) == "8994CB52-101C-4A78-BFFF-414A0563A92D") // this conditon for UIS Insurance
            {
                try
                {
                    if(dt.Columns.Contains("StartDate__c"))
                    {
                        string effDate = Convert.ToString(dt.Rows[rowIndex]["StartDate__c"]);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy string effDate: " + effDate, true, agencyName);
                        //check if value in double, then fetch OA Date
                        double dblEff = 0;
                        Double.TryParse(effDate, out dblEff);
                        if(dblEff > 0)
                        {
                            objPolicy.OriginalEffectiveDate = DateTime.FromOADate(dblEff);
                        }
                        else if(!string.IsNullOrEmpty(effDate))
                        {
                            objPolicy.OriginalEffectiveDate = DateTime.Parse(effDate, System.Globalization.CultureInfo.CurrentCulture); //Convert.ToDateTime(effDate);
                        }
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy datetime effDate: " + objPolicy.OriginalEffectiveDate, true, agencyName);
                }
                catch(Exception ex)
                {
                    errMsgPolicy.Add("StartDate__c", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: StartDate__c  fields  : " + ex.Message, true, agencyName);
                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                }
            }
            else
            {
                if (isNewPolicy || objPolicy.OriginalEffectiveDate == null)
                {
                    try
                    {
                        if (dt.Columns.Contains("OriginalPlanStartDate"))
                        {
                            string effDate = Convert.ToString(dt.Rows[rowIndex]["OriginalPlanStartDate"]);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy string effDate: " + effDate, true, agencyName);
                            //check if value in double, then fetch OA Date
                            double dblEff = 0;
                            Double.TryParse(effDate, out dblEff);
                            if (dblEff > 0)
                            {
                                objPolicy.OriginalEffectiveDate = DateTime.FromOADate(dblEff);
                            }
                            else if (!string.IsNullOrEmpty(effDate))
                            {
                                objPolicy.OriginalEffectiveDate = DateTime.Parse(effDate, System.Globalization.CultureInfo.CurrentCulture); //Convert.ToDateTime(effDate);
                            }
                        }
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy datetime effDate: " + objPolicy.OriginalEffectiveDate, true, agencyName);
                    }
                    catch (Exception ex)
                    {
                        errMsgPolicy.Add("OriginalPlanStartDate", ex.Message);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: OriginalPlanStartDate  fields  : " + ex.Message, true, agencyName);
                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                    }
                }
            }
        }


        static void UpdateBenefitsMandatoryFields(DataTable dt, int rowIndex, string importedPolicyID, string benefits_policyID, bool isNewPolicy, ref DLinq.Policy objPolicy, ref Dictionary<string, string> errMsgPolicy, string agencyName = "")
        {
            //Advance field 
            if (dt.Columns.Contains("AdvancedPaymentNumber"))
            {
                string strAdvance = Convert.ToString(dt.Rows[rowIndex]["AdvancedPaymentNumber"]);
                int adv = 0;
                Int32.TryParse(strAdvance, out adv);

                if (adv > 0)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Advanced  payment number from file : " + adv + ", updating...", true, agencyName);
                    objPolicy.Advance = adv;
                }
                else
                {
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Advanced  payment number from file : " + strAdvance + ", NOT updating...", true, agencyName);
                }
            }

            //Status
            if (dt.Columns.Contains("PlanStatusDescription") && dt.Rows[rowIndex]["PlanStatusDescription"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[rowIndex]["PlanStatusDescription"])))
            {
                try
                {
                    string strStatus = dt.Rows[rowIndex]["PlanStatusDescription"].ToString();
                    //objPolicy.PolicyStatusId = (strStatus.ToLower() == "active") ? 0 : (strStatus.ToLower() == "pending") ? 2 : 1;

                    if (strStatus.ToLower() == "active")
                    {
                        objPolicy.PolicyStatusId = 0;
                    }
                    else if (strStatus.ToLower() == "pending")
                    {
                        //if (dt.Columns.Contains("OriginalPlanStartDate"))
                        //{
                        //    string effDate = Convert.ToString(dt.Rows[rowIndex]["OriginalPlanStartDate"]);
                        //    ActionLogger.Logger.WriteImportPolicyLog("Import Policy string effDate in setting status of policy " + effDate, true, agencyName);
                        //    //check if value in double, then fetch OA Date
                        //    double dblEff = 0;
                        //    Double.TryParse(effDate, out dblEff);
                        //    if (dblEff > 0)
                        //    {
                        //        objPolicy.OriginalEffectiveDate = DateTime.FromOADate(dblEff);
                        //    }
                        //    else if (!string.IsNullOrEmpty(effDate))
                        //    {
                        //        objPolicy.OriginalEffectiveDate = DateTime.Parse(effDate, System.Globalization.CultureInfo.CurrentCulture); //Convert.ToDateTime(effDate);
                        //    }

                        //    if (objPolicy.OriginalEffectiveDate != null && (objPolicy.OriginalEffectiveDate > DateTime.Today))
                        //    {
                        //        objPolicy.PolicyStatusId = 0;
                        //    }
                        //    else
                        //    {
                        //        objPolicy.PolicyStatusId = 2;
                        //    }
                        //}
                        //else
                        //{
                        //    objPolicy.PolicyStatusId = 2;
                        //}

                        objPolicy.PolicyStatusId = 2;
                        if (objPolicy.OriginalEffectiveDate != null && objPolicy.OriginalEffectiveDate > DateTime.Today)
                        {
                            objPolicy.PolicyStatusId = 0;
                        }
                    }
                    else
                    {
                        objPolicy.PolicyStatusId = 1;
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("PlanStatusDescription", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: PlanStatusDescription fields  : " + ex.Message, true, agencyName);
                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                }
            }

            //Original effective date
            /*if (dt.Columns.Contains("OriginalPlanStartDate"))
            {
                if (dt.Rows[rowIndex]["OriginalPlanStartDate"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[rowIndex]["OriginalPlanStartDate"])))
                {
                    try
                    {
                        string effDate = Convert.ToString(dt.Rows[rowIndex]["OriginalPlanStartDate"]);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy string effDate: " + effDate, true, agencyName);
                        //check if value in double, then fetch OA Date
                        double dblEff = 0;
                        Double.TryParse(effDate, out dblEff);
                        if (dblEff > 0)
                        {
                            objPolicy.OriginalEffectiveDate = DateTime.FromOADate(dblEff);
                        }
                        else
                        {
                            objPolicy.OriginalEffectiveDate = DateTime.Parse(effDate, System.Globalization.CultureInfo.CurrentCulture); //Convert.ToDateTime(effDate);
                        }
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy datetime effDate: " + objPolicy.OriginalEffectiveDate, true, agencyName);
                    }
                    catch (Exception ex)
                    {
                        errMsgPolicy.Add("OriginalPlanStartDate", ex.Message);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: OriginalPlanStartDate  fields  : " + ex.Message, true, agencyName);
                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                    }
                }
            }*/

            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: status is terminated, so updating plan end date and reason ", true, agencyName);
            if (dt.Columns.Contains("PlanEndDate"))
            {
                if (dt.Rows[rowIndex]["PlanEndDate"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[rowIndex]["PlanEndDate"])))
                {
                    try
                    {
                        string termDate = Convert.ToString(dt.Rows[rowIndex]["PlanEndDate"]);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy string termDate: " + termDate, true, agencyName);
                        //check if value in double, then fetch OA Date
                        double dblTerm = 0;
                        Double.TryParse(termDate, out dblTerm);
                        if (dblTerm > 0)
                        {
                            objPolicy.PolicyTerminationDate = DateTime.FromOADate(dblTerm);
                        }
                        else
                        {
                            objPolicy.PolicyTerminationDate = Convert.ToDateTime(termDate);
                        }
                    }
                    //  objPolicy.PolicyTerminationDate = Convert.ToDateTime(termDate);
                    catch (Exception ex)
                    {
                        errMsgPolicy.Add("PlanEndDate", ex.Message);
                        //errorCount++;
                        //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "PlanEndDate: " + ex.Message);
                        //errorList.Add(m);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: PlanEndDate  fields  : " + ex.Message, true, agencyName);
                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                        //continue;
                    }
                }


                //Term reason
                if (dt.Columns.Contains("TerminationReason"))
                {
                    if (dt.Rows[rowIndex]["TerminationReason"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[rowIndex]["TerminationReason"])))
                    {
                        try
                        {
                            objPolicy.TerminationReasonId = PolicTermisionID(Convert.ToString(dt.Rows[rowIndex]["TerminationReason"]));
                        }
                        catch (Exception ex)
                        {
                            errMsgPolicy.Add("TerminationReason", ex.Message);
                            //errorCount++;
                            //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "TerminationReason: " + ex.Message);
                            //errorList.Add(m);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: TerminationReason  fields  : " + ex.Message, true, agencyName);
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            //continue;
                        }
                    }
                }
            }
            else
            {
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: status is NOT terminated, so ignoring plan end date and reason ", true, agencyName);
            }
        }

        static void UpdateBenefitsAccountOwner(DataTable dt, int rowIndex, string importedPolicyID, string benefits_policyID, bool isNewPolicy, List<dynamic> AgentList, ref DLinq.Policy objPolicy, ref Dictionary<string, string> errMsgPolicy, string agencyName = "")
        {
            if (dt.Columns.Contains("AccountOwnerName") || dt.Columns.Contains("AccountOwnerId"))
            {
                if (dt.Columns.Contains("AccountOwnerId"))  //if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[rowIndex]["AccountOwnerId"])))
                {
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwnerID received: " + Convert.ToString(dt.Rows[rowIndex]["AccountOwnerId"]), true, agencyName);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwnerName " + Convert.ToString(dt.Rows[rowIndex]["AccountOwnerName"]), true, agencyName);
                    if (dt.Rows[rowIndex]["AccountOwnerId"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[rowIndex]["AccountOwnerId"])))
                    {
                        string currentUserBGUserId = Convert.ToString(dt.Rows[rowIndex]["AccountOwnerId"]);
                        Guid userID = Guid.Empty;
                        var getBGUserId = AgentList.Where(u => u.BGUserId == currentUserBGUserId).FirstOrDefault();
                        if (getBGUserId == null)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwner: user NOT found in Db  by Owner ID", true, agencyName);
                            if (dt.Columns.Contains("AccountOwnerName"))
                            {
                                if (dt.Rows[rowIndex]["AccountOwnerName"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[rowIndex]["AccountOwnerName"])))
                                {
                                    try
                                    {

                                        string acctExec = Convert.ToString(dt.Rows[rowIndex]["AccountOwnerName"]).Trim();
                                        var objUser = AgentList.FirstOrDefault(d => (d.FirstName + " " + d.LastName).ToLower() == acctExec.ToLower() || (!string.IsNullOrEmpty(d.NickName) && d.NickName.ToLower() == acctExec.ToLower()));//.FirstOrDefault(); //User.GetUserIdWise(tempGuid);// (from p in DataModel.UserCredentials where (p.UserCredentialId == tempGuid && p.RoleId == 3 && p.IsDeleted == false) select p).FirstOrDefault();
                                                                                                                                                                                                                                           // User objUser = GlobalAgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                        if (objUser != null /*&& objUser.Role == UserRole.Agent*/)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog("Account owner found in system by name", true, agencyName);
                                            //Need to get nick name
                                            if (!string.IsNullOrEmpty(objUser.NickName))
                                            {

                                                objPolicy.AccoutExec = objUser.NickName;
                                                objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                                UpdateBGUserID(objUser.UserCredentialId, currentUserBGUserId, agencyName);
                                            }
                                            else
                                            {
                                                objPolicy.AccoutExec = objUser.UserName;
                                                objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                            }
                                            bool isexec = (new User().CheckAccoutExec(objUser.UserCredentialId, agencyName)); //Sets the flag of accExec true for this userID 
                                        }
                                        else //User not in system, need to add 
                                        {
                                            errMsgPolicy.Add("AccountOwnerName", "AccountOwner not available");
                                            ActionLogger.Logger.WriteImportPolicyLog("AccountOwner not found in the system by name ", true, agencyName);
                                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errMsgPolicy.Add("AccountOwnerName", ex.Message);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: AccountOwnerName  fields  : " + ex.Message, true, agencyName);
                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                                    }
                                }
                            }
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwner: user found in Db by ID ", true, agencyName);
                            userID = getBGUserId.UserCredentialId;
                            if (userID != null /*&& objUser.Role == UserRole.Agent*/)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Account owner found in system", true, agencyName);
                                //Need to get nick name
                                if (!string.IsNullOrEmpty(getBGUserId.NickName))
                                {
                                    objPolicy.AccoutExec = getBGUserId.NickName;
                                    objPolicy.UserCredentialId = getBGUserId.UserCredentialId; //tempGuid;
                                }
                                else
                                {
                                    objPolicy.AccoutExec = getBGUserId.UserName;
                                    objPolicy.UserCredentialId = getBGUserId.UserCredentialId; //tempGuid;
                                }
                                bool isexec = (new User().CheckAccoutExec(getBGUserId.UserCredentialId, agencyName)); //Sets the flag of accExec true for this userID 
                            }
                            else //User not in system, need to add 
                            {
                                errMsgPolicy.Add("AccountOwnerId", "AccountOwnerId not available");
                                ActionLogger.Logger.WriteImportPolicyLog("Account owner found in system with ID as null ", true, agencyName);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            }
                        }
                    }
                }
                else
                {
                    if (dt.Columns.Contains("AccountOwnerName"))
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwner Name found ", true, agencyName);
                        if (dt.Rows[rowIndex]["AccountOwnerName"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[rowIndex]["AccountOwnerName"])))
                        {
                            try
                            {
                                string acctExec = Convert.ToString(dt.Rows[rowIndex]["AccountOwnerName"]).Trim();
                                var objUser = AgentList.FirstOrDefault(d => (d.FirstName + " " + d.LastName).ToLower() == acctExec.ToLower() || (!string.IsNullOrEmpty(d.NickName) && d.NickName.ToLower() == acctExec.ToLower()));//.FirstOrDefault(); //User.GetUserIdWise(tempGuid);// (from p in DataModel.UserCredentials where (p.UserCredentialId == tempGuid && p.RoleId == 3 && p.IsDeleted == false) select p).FirstOrDefault();

                                if (objUser != null /*&& objUser.Role == UserRole.Agent*/)
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Account owner found in system", true, agencyName);
                                    //Need to get nick name
                                    if (!string.IsNullOrEmpty(objUser.NickName))
                                    {
                                        objPolicy.AccoutExec = objUser.NickName;
                                        objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                    }
                                    else
                                    {
                                        objPolicy.AccoutExec = objUser.UserName;
                                        objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                    }
                                    bool isexec = (new User().CheckAccoutExec(objUser.UserCredentialId, agencyName)); //Sets the flag of accExec true for this userID 
                                }
                                else //User not in system, need to add 
                                {
                                    errMsgPolicy.Add("AccountOwnerName", "AccountOwner not available");
                                    ActionLogger.Logger.WriteImportPolicyLog("Account owner NOT found in system by name : ", true, agencyName);
                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                }
                            }
                            catch (Exception ex)
                            {
                                errMsgPolicy.Add("AccountOwnerName", ex.Message);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: AccountOwnerName  fields  : " + ex.Message, true, agencyName);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);

                            }

                        }
                    }
                }
            }

            //Make sure Account owner name exists- If not, return. Additional check to make sure eror msg list doesn't contain same name key 
            if (string.IsNullOrEmpty(objPolicy.AccoutExec) && !errMsgPolicy.ContainsKey("AccountOwnerName"))
            {
                errMsgPolicy.Add("AccountOwnerName", "Account Owner Name cannot be blank");
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: AccountOwnerName  field is missing  : ", true, agencyName);
                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                
            }
        }

        private static void detachObject(DLinq.CommissionDepartmentEntities objModel,DLinq.Policy objPolicy)
        {
            objModel.Detach(objPolicy);
            //objModel.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave);
        }
        public static void punchErrors(Dictionary<string, string> errMsgPolicy, List<Benefits_ErrorMsg> errorList, String Str1ErrMsgPol, String Str2ErrMsgPol,String Str1ActionLog, String Str2ActionLog,String importedPolicyID,String benefits_policyID,String agencyName,Int32 errorCount,String importedID)
        {
            errMsgPolicy.Add(Str1ErrMsgPol, Str2ErrMsgPol);
            String output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
            ActionLogger.Logger.WriteImportPolicyLog(Str1ActionLog, true, Str2ActionLog);
            AddImportStatusToDB(importedPolicyID, false, false, benefits_policyID, agencyName);
            errorCount++;
            Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedID, benefits_policyID, output);
            errorList.Add(m);
        }
        //new added
        public static Benefits_PolicyImportStatus ImportPolicy_Uis_Insurance(DataTable dt, Guid LicID, ObservableCollection<CompType> CompTypeList, string agencyName = "")
        {
            #region Variables
            Benefits_PolicyImportStatus objStatus = new Benefits_PolicyImportStatus(); 

            string policyIDKey = "Unique Policy ID";

            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyIDKey: " + policyIDKey, true, agencyName);
            char[] spCharac = System.Configuration.ConfigurationSettings.AppSettings["AgentCharactersToTrim"].ToCharArray();

            //Response object structure
            int addCount = 0;
            int updateCount = 0;
            int errorCount = 0;
            List<Benefits_ErrorMsg> errorList = new List<Benefits_ErrorMsg>();
            List<Benefits_PolicyID> idList = new List<Benefits_PolicyID>();

            List<OutGoingPayment> OutGoingField = new List<OutGoingPayment>();
           // PolicyToolIncommingShedule inSchedule = null;
            string strProductType = string.Empty;
            string covNickName = string.Empty;
            Guid houseOwner = Guid.Empty;
            #endregion

            #region Agents List
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Data model init", true, agencyName);
                var AgentList = (from p in DataModel.UserCredentials
                                 join o in DataModel.UserDetails on p.UserCredentialId equals o.UserCredentialId
                                 where p.LicenseeId == LicID && p.IsDeleted == false
                                 select new
                                 {
                                     p.UserCredentialId,
                                     o.NickName,
                                     p.UserName,
                                     p.RoleId,
                                     o.FirstName,
                                     o.LastName,
                                     p.BGUserId
                                 }).ToList();
                #endregion

                #region Get Agency's track date default 
                DateTime? dtTrack = DateTime.MinValue;
                try
                {
                    var strTrack = (from l in DataModel.Licensees where l.LicenseeId == LicID select new { l.TrackDateDefault }).FirstOrDefault();
                    if (strTrack != null)
                    {
                        if (strTrack.TrackDateDefault != null)// && strTrack.TrackDateDefault > 
                            dtTrack = strTrack.TrackDateDefault;
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy trackDatefrom agency: " + dtTrack, true, agencyName);

                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("track date calculation failed" + ex.Message, true, agencyName);
                }

                //List<GetPolicyList> policyLists = new List<GetPolicyList>();
                //foreach(DataRow row in dt.Rows)
                //{
                //    policyLists.Add(new GetPolicyList
                //    {
                //        importedID = Convert.ToString(row["OriginalPlanID"]),
                //        PolicyPlanID = Convert.ToString(row["OriginalPlanID"]),
                //        importedID = Convert.ToString(row["OriginalPlanID"]),
                //        importedID = Convert.ToString(row["OriginalPlanID"]),
                //        importedID = Convert.ToString(row["OriginalPlanID"]),
                //        importedID = Convert.ToString(row["OriginalPlanID"]),
                //        importedID = Convert.ToString(row["OriginalPlanID"])
                //    });
                //}

                #endregion
                #region Add data to Object from table
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GetPolicyList _PolicyObj = new GetPolicyList();
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Iteration: " + i, true, agencyName);

                    Dictionary<string, string> errMsgPolicy = new Dictionary<string, string>();

                    //importedID
                    if (dt.Columns.Contains("OriginalPlanID"))
                    {
                        _PolicyObj.importedID = Convert.ToString(dt.Rows[i]["OriginalPlanID"]);
                    }

                    //ImportPolicyId
                    if (dt.Columns.Contains(policyIDKey))
                    {
                        _PolicyObj.importedPolicyID = dt.Columns.Contains(policyIDKey) ? Convert.ToString(dt.Rows[i][policyIDKey]).Trim() : Convert.ToString(Guid.NewGuid()).ToUpper();// Assuming new policy, new ID is generated 
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: importedPolicyID: " + _PolicyObj.importedPolicyID, true, agencyName);
                    }
                    //PolicyPlanId
                    if (dt.Columns.Contains("PolicyPlanID"))
                    {
                        _PolicyObj.PolicyPlanID = Convert.ToString(dt.Rows[i]["PolicyPlanID"]);
                    }
                    //ClientName
                    if(dt.Columns.Contains("AccountName"))
                    {
                        _PolicyObj.ClientName = Convert.ToString(dt.Rows[i]["AccountName"]);
                    }
                    else if(dt.Columns.Contains("Account Name"))
                    {
                        _PolicyObj.ClientName = Convert.ToString(dt.Rows[i]["Account Name"]);
                    }
                    //Status
                    if(dt.Columns.Contains("Status__c"))
                    {
                        _PolicyObj.PolicyStatusId = Convert.ToInt32(dt.Rows[i]["Status__c"]);
                    }
                    //Insured
                    if(dt.Columns.Contains("AccountName"))
                    {
                        _PolicyObj.Insured = Convert.ToString(dt.Rows[i]["AccountName"]);
                    }
                    else
                    {
                        _PolicyObj.Insured = Convert.ToString(dt.Rows[i]["Account Name"]);
                    }
                    //Policy#
                    if(dt.Columns.Contains("Policy_Number__c"))
                    {
                        _PolicyObj.PolicyNumber = Convert.ToString(dt.Rows[i]["Policy_Number__c"]);
                    }
                    //Original Effective Date
                    if (dt.Columns.Contains("StartDate__c"))
                    {
                        _PolicyObj.OriginalEffectiveDate = Convert.ToDateTime(dt.Rows[i]["StartDate__c"]);
                    }
                    //Mode  // comment this code bcoz already update value in updateBenefitsOptionaFields - method
                    //if (dt.Columns.Contains("ModalNumber"))
                    //{
                    //    _PolicyObj.PolicyModeId = Convert.ToInt32(dt.Rows[i]["ModalNumber"]);
                    //}
                    //Monthly Premium
                    if (dt.Columns.Contains("MonthlyPremium__c"))
                    {
                        _PolicyObj.MonthlyPremium = Convert.ToInt32(dt.Rows[i]["MonthlyPremium__c"]);
                    }
                    //Payor
                    if (dt.Columns.Contains("PayorCommissionDept"))
                    {
                        _PolicyObj.Payor = Convert.ToString(dt.Rows[i]["PayorCommissionDept"]);
                    }
                    //Carrier
                    if (dt.Columns.Contains("CarrierCommissionDept"))
                    {
                        _PolicyObj.carrier = Convert.ToString(dt.Rows[i]["CarrierCommissionDept"]);
                    }
                    //Product
                    if (dt.Columns.Contains("LineOfCoverage"))
                    {
                        _PolicyObj.Product = Convert.ToString(dt.Rows[i]["LineOfCoverage"]);
                    }
                    //Product Type
                    if (dt.Columns.Contains("Product Type"))
                    {
                        _PolicyObj.PolicyType = Convert.ToString(dt.Rows[i]["Product Type"]);
                    }
                    //Policy Term Date
                    if (dt.Columns.Contains("Termination_Date__c"))
                    {
                        _PolicyObj.PolicyTerminationDate = Convert.ToDateTime(dt.Rows[i]["Termination_Date__c"]);
                    }
                    //Term Reason
                    if (dt.Columns.Contains("Termination Reason"))
                    {
                        _PolicyObj.TerminationReasonId = Convert.ToInt32(dt.Rows[i]["Termination Reason"]);
                    }
                    //Business Segment
                    if (dt.Columns.Contains("Business Segment"))
                    {
                        _PolicyObj.Segment = Convert.ToString(dt.Rows[i]["Business Segment"]);
                    }
                    //Submitted Through
                    if (dt.Columns.Contains("Submitted Through"))
                    {
                        _PolicyObj.SubmittedThrough = Convert.ToString(dt.Rows[i]["Submitted Through"]);
                    }
                    //Enrolled
                    if (dt.Columns.Contains("CurrentEnrolled__c"))
                    {
                        _PolicyObj.Enrolled = Convert.ToString(dt.Rows[i]["CurrentEnrolled__c"]);
                    }
                    //Account Exec
                    if (dt.Columns.Contains("AccountOwnerName"))
                    {
                        _PolicyObj.AccoutExec = Convert.ToString(dt.Rows[i]["AccountOwnerName"]);
                    }
                    //isTrackPayment
                    _PolicyObj.IsTrackPayment = false;
                    if (dt.Columns.Contains("IsTrackPayment"))
                    {
                        _PolicyObj.IsTrackPayment = Convert.ToBoolean(dt.Rows[i]["IsTrackPayment"]);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy - isTrackPayment found in request  : " + _PolicyObj.IsTrackPayment, true, agencyName);
                    }
                    #endregion

                    //validate data
                    ValidateObj(_PolicyObj, dt,ref i, errMsgPolicy,ref agencyName, errorList,ref errorCount, LicID, CompTypeList);
                }
            }


            return null;
        }
        public static void  ValidateObj(GetPolicyList _PolicyObj, DataTable dt,ref int i, Dictionary<string, string> errMsgPolicy,ref string agencyName, List<Benefits_ErrorMsg> errorList,ref int errorCount, Guid LicID, ObservableCollection<CompType> CompTypeList)
        {
            //match policy id obj
            DLinq.Policy objPolicy = new DLinq.Policy();
            PolicyToolIncommingShedule inSchedule = null;

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                for (int j = 0; j < i; j++)
                {
                    //if importedID is null or empty
                    if (string.IsNullOrEmpty(_PolicyObj.importedID))
                    {
                        errMsgPolicy.Add("Original Plan ID", "Original plan ID found missing");
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Original plan ID found null/blank, skipping record", true, agencyName);
                        AddImportStatusToDB(_PolicyObj.importedPolicyID, false, false, _PolicyObj.PolicyPlanID, agencyName);
                        errorCount++;
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(_PolicyObj.importedID, _PolicyObj.PolicyPlanID, output);
                        errorList.Add(m);
                        continue; // we can remove continue and for loop __??????????????????__
                    }
                    //check client exist
                    if (string.IsNullOrEmpty(_PolicyObj.ClientName))
                    {
                        errMsgPolicy.Add("AccountName", "Account name found missing");
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Account name found null/blank, skipping record", true, agencyName);
                        AddImportStatusToDB(_PolicyObj.importedPolicyID, false, false, _PolicyObj.PolicyPlanID, agencyName);
                        errorCount++;
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(_PolicyObj.importedID, _PolicyObj.PolicyPlanID, output);
                        errorList.Add(m);
                        continue;
                    }

                    //Check policy Exist or not
                    bool isNewPolicy = false;
                    Guid policyID = Guid.Empty;
                    bool isExisting = false;

                    policyID = IsPolicyExistingWithImportID(_PolicyObj.importedPolicyID, LicID, agencyName);
                    if (policyID == Guid.Empty)
                    {
                        isExisting = false;
                        policyID = Guid.NewGuid();
                    }
                    else
                    {
                        isExisting = true;
                    }

                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyID: " + _PolicyObj.importedPolicyID + ", benefits ID: " + _PolicyObj.PolicyPlanID, true, agencyName);
                    if (isExisting)
                    {
                        objPolicy = (from p in DataModel.Policies where p.PolicyId == policyID select p).FirstOrDefault();
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Existing policy", true, agencyName);

                        //check if client exists or deleted in the system
                        //if deleted, then add the given as new 
                        if (objPolicy.Client != null && Convert.ToBoolean(objPolicy.Client.IsDeleted))
                        {
                            objPolicy.PolicyClientId = null;
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Client found deleted , so setting as null ", true, agencyName);
                        }
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: New policy", true, agencyName);
                        isNewPolicy = true;
                        objPolicy.PolicyId = policyID;

                        objPolicy.PolicyLicenseeId = LicID;
                        objPolicy.TerminationReasonId = null;
                        objPolicy.IsTrackMissingMonth = true;
                        objPolicy.CreatedOn = DateTime.Today;
                        objPolicy.IsIncomingBasicSchedule = true;
                        objPolicy.IsOutGoingBasicSchedule = true;
                        objPolicy.CreatedBy = new Guid("AA38DF84-2E30-43CA-AED3-7276224D1B7E");
                        objPolicy.IsDeleted = false;
                    }
                    objPolicy.LastModifiedOn = DateTime.Now;
                    objPolicy.LastModifiedBy = new Guid("AA38DF84-2E30-43CA-AED3-7276224D1B7E");

                    #region Fields that should be updated when blank or with new policy

                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy isTrackPayment : " + _PolicyObj.IsTrackPayment, true, agencyName);
                    objPolicy.IsTrackPayment = _PolicyObj.IsTrackPayment;

                    // SubmittedThrough sent by the UIS Insurance
                    if(!string.IsNullOrEmpty(_PolicyObj.SubmittedThrough))
                    {
                        objPolicy.SubmittedThrough = _PolicyObj.SubmittedThrough;
                    }

                    UpdateBenefitsOptionalFields(dt, i, _PolicyObj.importedPolicyID,_PolicyObj.PolicyPlanID,isNewPolicy,ref objPolicy, ref errMsgPolicy, LicID, agencyName);

                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: optional fields , to be filled when blanks init done : " + _PolicyObj.importedPolicyID, true, agencyName);
                    #endregion

                    #region Payor
                    if(isNewPolicy || objPolicy.PayorId == null)
                    {
                        DLinq.Payor py = UpdatePayorForBG(dt, i, _PolicyObj.importedPolicyID, _PolicyObj.PolicyPlanID, isNewPolicy, DataModel, LicID, ref errMsgPolicy, agencyName);
                        if (py != null)
                        {
                            objPolicy.PayorId = py.PayorId;
                            objPolicy.PayorReference.Value = py;
                        }
                    }
                    #endregion

                    #region Carrier
                    if (isNewPolicy || objPolicy.CarrierId == null)
                    {
                        DLinq.Carrier carrer = UpdateCarrierForBG(dt, i, _PolicyObj.importedPolicyID, _PolicyObj.PolicyPlanID, isNewPolicy, DataModel, LicID, ref errMsgPolicy, agencyName);
                        if (carrer != null)
                        {
                            objPolicy.CarrierId = carrer.CarrierId;
                            objPolicy.CarrierReference.Value = carrer;
                        }
                    }
                    #endregion

                    #region product
                    if(isNewPolicy || objPolicy.CoverageId == null)
                    {
                        UpdateCoverageForBG(dt, i, _PolicyObj.importedPolicyID, _PolicyObj.PolicyPlanID, isNewPolicy, objPolicy, DataModel, LicID, ref errMsgPolicy, agencyName);
                    }
                    #endregion

                    #region Business Segment
                    if (dt.Columns.Contains("Business_Segment__c"))
                    {
                       // objPolicy.SegmentId = Convert.ToString(dt.Rows[i]["Business_Segment__c"]);
                    }
                    #endregion

                    #region Product Type - Not to be sent by UIS Insurance
                    #endregion

                    #region Submitted Through
                    if (isNewPolicy || string.IsNullOrEmpty(objPolicy.SubmittedThrough))
                    {
                        if (dt.Columns.Contains("GeneralAgentLU__c"))
                        {
                            objPolicy.SubmittedThrough = Convert.ToString(dt.Rows[i]["GeneralAgentLU__c"]);
                        }
                    }
                    #endregion

                    #region Incoming schedule
                    bool allowImportedSchedule = true;
                    Guid SettingsScheduleID = Guid.Empty;
                    bool oldInScheduleExists = false;

                    if (!isNewPolicy)
                    {
                        PolicyToolIncommingShedule oldSchedule = PolicyToolIncommingShedule.GettingPolicyIncomingSchedule(objPolicy.PolicyId, agencyName);
                        if (oldSchedule != null && oldSchedule.IncomingScheduleID != Guid.Empty)
                        {
                            if ((oldSchedule.Mode == Mode.Standard 
                                 && (oldSchedule.FirstYearPercentage != 0 || oldSchedule.RenewalPercentage != 0))
                                  || oldSchedule.Mode == Mode.Custom //assuming this will always have value
                                  )
                            {
                                oldInScheduleExists = true;
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy - Old policy and old incoming schedule exists with non-zero values: ", true, agencyName);
                            }
                        }
                    }

                    int? incomingPaymentType = 1;
                    if (dt.Columns.Contains("Commission Type"))
                    {
                        string strCommisionType = Convert.ToString(dt.Rows[i]["Commission Type"]);
                        incomingPaymentType = PolicCompType(strCommisionType, CompTypeList, agencyName);
                    }
                    else if (dt.Columns.Contains("CommissionType"))
                    {
                        string strCommisionType = Convert.ToString(dt.Rows[i]["CommissionType"]);
                        incomingPaymentType = PolicCompType(strCommisionType, CompTypeList, agencyName);
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy incomingPaymentType: " + incomingPaymentType, true, agencyName);
                    PayorIncomingSchedule payorSchedule = null;

                    //Configure incomin schedule only when not existing or new policy
                    if (isNewPolicy || !oldInScheduleExists)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy - incoming schedule to be read: ", true, agencyName);
                        objPolicy.IncomingPaymentTypeId = incomingPaymentType;
                        objPolicy.SplitPercentage = 100;

                        inSchedule = new PolicyToolIncommingShedule();
                        inSchedule.PolicyId = objPolicy.PolicyId;
                        inSchedule.IncomingScheduleID = Guid.NewGuid();
                        inSchedule.CustomType = CustomMode.Graded;

                        /*********** custom incoming from BG *****************/

                        int mod = 0;
                        Int32.TryParse(Convert.ToString(dt.Rows[i]["IncomingMode"]), out mod);
                        inSchedule.Mode = (Mode)mod;

                        if (inSchedule.Mode ==  Mode.Custom)
                        {
                            int customMod = 1;
                            Int32.TryParse(Convert.ToString(dt.Rows[i]["IncomingCustomModeType"]), out customMod);
                            inSchedule.CustomType = (CustomMode)customMod;
                        }

                        inSchedule.FirstYearPercentage = 0;
                        inSchedule.RenewalPercentage = 0;

                        string strOutPercentOfPremium = "% of Premium";
                        if (dt.Columns.Contains("PaymentType"))
                        {
                            strOutPercentOfPremium = Convert.ToString(dt.Rows[i]["PaymentType"]);
                        }
                        inSchedule.ScheduleTypeId = (!string.IsNullOrEmpty(strOutPercentOfPremium) && strOutPercentOfPremium.ToLower().Contains("head")) ? 2 : 1;
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy - incoming schedule init with default values : ", true, agencyName);
                        // add split percentage
                        if (dt.Columns.Contains("CoBrokerSplit"))
                        {
                            string strSplit = Convert.ToString(dt.Rows[i]["CoBrokerSplit"]);
                            double splitPer = 0;
                            double.TryParse(strSplit, out splitPer);
                            objPolicy.SplitPercentage = splitPer == 0 ? 100 : splitPer;
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy - Split % read as : " + splitPer + ", set as: " + objPolicy.SplitPercentage, true, agencyName);
                        }
                    }

                    //First check excel for any incoming schdule present then import 
                    if (inSchedule.Mode == Mode.Standard && ((dt.Columns.Contains("Commissions - First Year %") || dt.Columns.Contains("CommissionsFirstYear")) && ((dt.Columns.Contains("Commissions - Renewal %") || dt.Columns.Contains("CommissionsRenewal")))))
                    {
                        string strInFirstYear = dt.Columns.Contains("Commissions - First Year %") ? Convert.ToString(dt.Rows[i]["Commissions - First Year %"]) : dt.Columns.Contains("CommissionsFirstYear") ? Convert.ToString(dt.Rows[i]["CommissionsFirstYear"]) : "0";
                        string strInRenewYear = dt.Columns.Contains("Commissions - Renewal %") ? Convert.ToString(dt.Rows[i]["Commissions - Renewal %"]) : dt.Columns.Contains("CommissionsRenewal") ? Convert.ToString(dt.Rows[i]["CommissionsRenewal"]) : "0";
                        double frst = 0; double renew = 0;
                        double.TryParse(strInFirstYear, out frst);
                        double.TryParse(strInRenewYear, out renew);

                        inSchedule.FirstYearPercentage = frst;
                        inSchedule.RenewalPercentage = renew;

                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy - first year% read as : " + frst + ", renewal as: " + renew, true, agencyName);

                        if (frst == 0 && renew == 0)
                        {
                            //get payor configuration if present
                            if (objPolicy.PayorId != null && objPolicy.CarrierId != null && objPolicy.CoverageId != null && incomingPaymentType != null && !string.IsNullOrEmpty(objPolicy.ProductType))
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy reading payor configuration : ", true, agencyName);
                                payorSchedule = PayorIncomingSchedule.GetPayorScheduleDetails((Guid)objPolicy.PayorId, (Guid)objPolicy.CarrierId, (Guid)objPolicy.CoverageId, (Guid)objPolicy.PolicyLicenseeId, objPolicy.ProductType, (int)incomingPaymentType, agencyName);
                                if (payorSchedule != null && payorSchedule.IncomingScheduleID != Guid.Empty)
                                {
                                    allowImportedSchedule = false;
                                    SettingsScheduleID = payorSchedule.IncomingScheduleID;
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration found: SettingsScheduleID - " + SettingsScheduleID, true, agencyName);
                                    //apply later after policy is saved.
                                }
                                else
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration NOT found: " + incomingPaymentType, true, agencyName);
                                }
                            }
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule init as in excel 1st : " + frst + ", renewal: " + renew, true, agencyName);
                        }
                    }
                    else if (inSchedule.Mode == Mode.Custom)
                    {
                        if (dt.Columns.Contains("IncomingCustomList"))
                        {
                            DataTable dtIncoming = (dt.Rows[i]["IncomingCustomList"] as DataTable);
                            inSchedule.GradedSchedule = new List<Graded>();
                            inSchedule.NonGradedSchedule = new List<NonGraded>();
                            if (dtIncoming != null && dtIncoming.Rows.Count > 0)
                            {
                                try
                                {
                                    foreach (DataRow row in dtIncoming.Rows)
                                    {
                                        if (row.ItemArray == null || row.ItemArray.Count() == 0)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule row found blank", true, agencyName);
                                            continue;
                                        }
                                        if (inSchedule.CustomType == CustomMode.Graded)
                                        {
                                            Graded objGraded = new Graded();

                                            double from = 0;
                                            string strFrom = Convert.ToString(row["From"]);
                                            double.TryParse(strFrom, out from);
                                            objGraded.From = from;

                                            double to = 0;
                                            string strTo = Convert.ToString(row["To"]);
                                            double.TryParse(strTo, out to);
                                            objGraded.To = to;

                                            double percnt = 0;
                                            string strPercnt = Convert.ToString(row["Value"]);
                                            double.TryParse(strPercnt, out percnt);
                                            objGraded.Percent = percnt;

                                            inSchedule.GradedSchedule.Add(objGraded);
                                        }
                                        else
                                        {
                                            NonGraded objNonGraded = new NonGraded();

                                            int year = 0;
                                            string strYear = Convert.ToString(row["Year"]);
                                            int.TryParse(strYear, out year);
                                            objNonGraded.Year = year;

                                            double percnt = 0;
                                            string strPercnt = Convert.ToString(row["Value"]);
                                            double.TryParse(strPercnt, out percnt);
                                            objNonGraded.Percent = percnt;

                                            inSchedule.NonGradedSchedule.Add(objNonGraded);
                                        }
                                    }

                                    //Validation of the list
                                    string errIncoming = ValidateIncomingSchedule(inSchedule.GradedSchedule, inSchedule.NonGradedSchedule, (inSchedule.CustomType == CustomMode.Graded), inSchedule.Mode.ToString(), inSchedule.ScheduleTypeId, agencyName);
                                    if (!string.IsNullOrEmpty(errIncoming))
                                    {
                                        errMsgPolicy.Add("IncomingSchedule", errIncoming);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: IncomingSchedule fields error : " + errIncoming, true, agencyName);
                                        AddImportStatusToDB(_PolicyObj.importedPolicyID, isNewPolicy, false, _PolicyObj.PolicyPlanID, agencyName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add("IncomingSchedule", "Error adding Custom Schedule: " + ex.Message);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: IncomingSchedule fields missing ", true, agencyName);
                                    AddImportStatusToDB(_PolicyObj.importedPolicyID, isNewPolicy, false, _PolicyObj.PolicyPlanID, agencyName);
                                }
                            }
                            else
                            {
                                errMsgPolicy.Add("IncomingSchedule", "Custom Schedule list is missing");
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: IncomingSchedule fields missing", true, agencyName);
                                AddImportStatusToDB(_PolicyObj.importedPolicyID, isNewPolicy, false, _PolicyObj.PolicyPlanID, agencyName);
                            }
                        }
                        else
                        {
                            errMsgPolicy.Add("IncomingSchedule", "Custom Schedule list is missing");
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: IncomingSchedule fields missing", true, agencyName);
                            AddImportStatusToDB(_PolicyObj.importedPolicyID, isNewPolicy, false, _PolicyObj.PolicyPlanID, agencyName);
                        }

                    }

                    #endregion


                }
            }

        }
        public static Benefits_PolicyImportStatus ImportPolicy_Benefits_Caravus(DataTable dt, Guid LicID, ObservableCollection<CompType> CompTypeList, string agencyName = "")
        {
            #region Variables
            Benefits_PolicyImportStatus objStatus = new Benefits_PolicyImportStatus();

            string policyIDKey = string.Empty;



            /* Following to be used in case   1-many mapping exists as anticipated before, not in use for now 
            * 
            * List<string> lstKeys = System.Configuration.ConfigurationSettings.AppSettings["BenefitsPolicyKeys"].Split(',').ToList<String>();
              string policyIDKey = "";
              foreach (string s in lstKeys)
              {
                  if (dt.Columns.Contains(s))
                  {
                      policyIDKey = s;
                      break;
                  }
              }*/

                        if (Convert.ToString(LicID).ToUpper() == "A3E3FCEE-05B5-4D3A-9D62-E113AED72946")//Vanguard Benefit BG
            {
                policyIDKey = "CDPolicyID"; // System.Configuration.ConfigurationSettings.AppSettings["PolicyIDKeyName_Benefits"]; // "OriginalPlanID";
            }
            else
            {
                policyIDKey = "OriginalPlanID";
            }

            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyIDKey: " + policyIDKey, true, agencyName);
            char[] spCharac = System.Configuration.ConfigurationSettings.AppSettings["AgentCharactersToTrim"].ToCharArray();  //{ '(', '[', '-' };

            //Response object structure
            int addCount = 0;
            int updateCount = 0;
            int errorCount = 0;
            List<Benefits_ErrorMsg> errorList = new List<Benefits_ErrorMsg>();
            List<Benefits_PolicyID> idList = new List<Benefits_PolicyID>();

            List<OutGoingPayment> OutGoingField = new List<OutGoingPayment>();
            PolicyToolIncommingShedule inSchedule = null;
            string strProductType = string.Empty;
            string covNickName = string.Empty;
            Guid houseOwner = Guid.Empty;

            #endregion
            
            #region Agents List
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
               
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Data model init", true, agencyName);
                var AgentList = (from p in DataModel.UserCredentials
                                 join o in DataModel.UserDetails on p.UserCredentialId equals o.UserCredentialId
                                 where p.LicenseeId == LicID /*&& p.RoleId == 3*/ && p.IsDeleted == false
                                 select new
                                 {
                                     p.UserCredentialId,
                                     o.NickName,
                                     p.UserName,
                                     p.RoleId,
                                     o.FirstName,
                                     o.LastName,
                                     p.BGUserId
                                 }).ToList();
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Agent list fetched " + AgentList.ToStringDump(), true, agencyName);

                #endregion

                #region Get Agency's track date default 
                DateTime? dtTrack = DateTime.MinValue;
                try
                {
                    ////Jyotisna - Dec 17, 2018 - Hard coded check as advised by Kevin for "Benefit Pro" required in BG integration
                    //if (LicID == new Guid("20FCF5E6-70DA-44C1-9FC0-AF73500BFB6F"))
                    //{
                    //    DateTime dtBPro = DateTime.MinValue;
                    //    DateTime.TryParse(System.Configuration.ConfigurationManager.AppSettings["Benefits_Pro_TrackDate"], out dtBPro);
                    //    ActionLogger.Logger.WriteImportPolicyLog("Import Policy default trackDatefrom benefits pro: " + dtBPro, true,agencyName);
                    //    dtTrack = dtBPro;
                    //}
                    //else
                    //{

                    var strTrack = (from l in DataModel.Licensees where l.LicenseeId == LicID select new { l.TrackDateDefault }).FirstOrDefault();
                    if (strTrack != null)
                    {
                        if (strTrack.TrackDateDefault != null)// && strTrack.TrackDateDefault > 
                            dtTrack = strTrack.TrackDateDefault;
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy trackDatefrom agency: " + dtTrack, true, agencyName);
                    //}

                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("track date calculation failed" + ex.Message, true, agencyName);
                }

                #endregion
                //Boolean iterationError = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                  
                    //iterationError = false;
                    //Object objPolicy = new object();
                    DLinq.Policy objPolicy = new DLinq.Policy();
                    //objPolicy = obj;
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Iteration: " + i, true, agencyName);
                   
                        
                    string importedID = dt.Columns.Contains("OriginalPlanID") ? Convert.ToString(dt.Rows[i]["OriginalPlanID"]).Trim() : "";
                    string importedPolicyID = dt.Columns.Contains(policyIDKey) ? Convert.ToString(dt.Rows[i][policyIDKey]).Trim() : Convert.ToString(Guid.NewGuid()).ToUpper();// Assuming new policy, new ID is generated 
                    
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: importedPolicyID: " + importedPolicyID, true, agencyName);
                    Dictionary<string, string> errMsgPolicy = new Dictionary<string, string>();


                    string benefits_policyID = string.Empty;
                    if (dt.Columns.Contains("PolicyPlanID")) //|| dt.Columns.Contains("New Business?"))
                    {
                        benefits_policyID = Convert.ToString(dt.Rows[i]["PolicyPlanID"]);
                    }

                    //Case if - ID column present but blank value
                    if (Convert.ToString(LicID).ToUpper() == "A3E3FCEE-05B5-4D3A-9D62-E113AED72946")//Vanguard Benefit BG
                    {
                        if (string.IsNullOrEmpty(importedPolicyID))
                        {
                            importedPolicyID = Convert.ToString(Guid.NewGuid()).ToUpper(); //Nov 05, 2020- toupper() added as per BG request 
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: importedPolicyID Created as : " + importedPolicyID, true, agencyName);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(importedID))
                        {
                        errMsgPolicy.Add("Original Plan ID", "Original plan ID found missing");
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Original plan ID found null/blank, skipping record", true, agencyName);
                        AddImportStatusToDB(importedPolicyID, false, false, benefits_policyID, agencyName);
                        errorCount++;
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedID, benefits_policyID, output);
                        errorList.Add(m);                
                        continue;
                        }
                    }



                    string clientName = string.Empty;
                    if (dt.Columns.Contains("AccountName"))
                    {
                        clientName = Convert.ToString(dt.Rows[i]["AccountName"]);
                    }
                    else if (dt.Columns.Contains("Account Name"))
                    {
                        clientName = Convert.ToString(dt.Rows[i]["Account Name"]);
                    }


                    if (string.IsNullOrEmpty(clientName))
                    {
                       // iterationError = true;
                        errMsgPolicy.Add("AccountName", "Account name found missing");
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Account name found null/blank, skipping record", true, agencyName);
                        AddImportStatusToDB(importedPolicyID, false, false, benefits_policyID, agencyName);
                        errorCount++;
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedID, benefits_policyID, output);
                        errorList.Add(m);
                        //detachObject(DataModel, objPolicy);
                        continue;
                    }

                    OutGoingField = new List<OutGoingPayment>();
                    bool isNewPolicy = false;

                    Guid policyID = Guid.Empty;
                    bool isExisting = false;

                    if (Convert.ToString(LicID).ToUpper() == "A3E3FCEE-05B5-4D3A-9D62-E113AED72946")//Vanguard Benefit BG
                    {
                        isExisting = IsPolicyExistingWithPolicyID_New(importedPolicyID, agencyName);
                    }
                    else
                    {
                        policyID = IsPolicyExistingWithImportID(importedPolicyID, LicID, agencyName);
                        if (policyID == Guid.Empty)
                        {
                            isExisting = false;
                            policyID = Guid.NewGuid();
                        }
                        else
                        {
                            isExisting = true;
                        }
                    }

                    //Guid policyID;

                    if (Convert.ToString(LicID).ToUpper() == "A3E3FCEE-05B5-4D3A-9D62-E113AED72946")//Vanguard Benefit BG
                    {
                        try
                        {
                            policyID = new Guid(importedPolicyID);
                        }
                        catch (Exception ex)
                        {
                            errMsgPolicy.Add("CDPolicyID", "Invalid CD PolicyID");
                            string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Invalid CD PolicyID, skipping record" + ex.Message, true, agencyName);
                            AddImportStatusToDB(importedPolicyID, false, false, benefits_policyID, agencyName);
                            errorCount++;
                            Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedID, benefits_policyID, output);
                            errorList.Add(m);
                            //detachObject(DataModel, objPolicy);
                            continue;
                        }
                    }

                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyID: " + importedPolicyID + ", benefits ID: " + benefits_policyID, true, agencyName);
                    if (isExisting) // (policyID != Guid.Empty)
                    {
                        objPolicy = (from p in DataModel.Policies where p.PolicyId == policyID select p).FirstOrDefault();
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Existing policy", true, agencyName);


                        //check if client exists or deleted in the system
                        //if deleted, then add the given as new 
                        if (objPolicy.Client != null && Convert.ToBoolean(objPolicy.Client.IsDeleted))
                        {
                            objPolicy.PolicyClientId = null;
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Client found deleted , so setting as null ", true, agencyName);
                        }
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: New policy", true, agencyName);
                        isNewPolicy = true;
                        objPolicy.PolicyId = policyID; // importedPolicyID; //Guid.NewGuid();
                                                       // objPolicy.IsTrackPayment = true; Commented as to be sent by BG for Caravus 
                        objPolicy.PolicyLicenseeId = LicID;
                        objPolicy.TerminationReasonId = null;
                        objPolicy.IsTrackMissingMonth = true;
                        objPolicy.CreatedOn = DateTime.Today;
                        objPolicy.IsIncomingBasicSchedule = true;
                        objPolicy.IsOutGoingBasicSchedule = true;
                        objPolicy.CreatedBy = new Guid("AA38DF84-2E30-43CA-AED3-7276224D1B7E");
                        objPolicy.IsDeleted = false;
                    }
                    objPolicy.LastModifiedOn = DateTime.Now;
                    objPolicy.LastModifiedBy = new Guid("AA38DF84-2E30-43CA-AED3-7276224D1B7E");



                    #region Fields that should be updated when blank or with new policy
                    //try
                    //{

                    bool isTrackPayment = false;

                    //IsTrackPayment flag - to be always updated with policy import 
                    if (dt.Columns.Contains("IsTrackPayment")) //|| dt.Columns.Contains("New Business?"))
                    {

                        string strIsTrackFlag = Convert.ToString(dt.Rows[i]["IsTrackPayment"]); //dt.Columns.Contains("New?") ? Convert.ToString(dt.Rows[i]["New?"]) : (dt.Columns.Contains("New Business?") ? Convert.ToString(dt.Rows[i]["New Business?"]) : "");
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy - isTrackPayment found in request  : " + isTrackPayment, true, agencyName);

                        bool.TryParse(strIsTrackFlag, out isTrackPayment);

                    }
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy isTrackPayment : " + isTrackPayment, true, agencyName);
                    objPolicy.IsTrackPayment = isTrackPayment;


                    objPolicy.SubmittedThrough = ""; //Not to be sent by benefits and to be left blank as per kevin Aug 17, 2017

                    UpdateBenefitsOptionalFields(dt, i, importedPolicyID, benefits_policyID, isNewPolicy, ref objPolicy, ref errMsgPolicy, LicID, agencyName);

                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: optional fields , to be filled when blanks init done : " + policyIDKey, true, agencyName);

                    //Payor  
                    #region Payor
                    if (isNewPolicy || objPolicy.PayorId == null)
                    {
                        DLinq.Payor py = UpdatePayorForBG(dt, i, importedPolicyID, benefits_policyID, isNewPolicy, DataModel, LicID, ref errMsgPolicy, agencyName);
                        if (py != null)
                        {
                            objPolicy.PayorId = py.PayorId;
                            objPolicy.PayorReference.Value = py;
                        }
                    }
                    #endregion

                    #region Carrier
                    if (isNewPolicy || objPolicy.CarrierId == null)
                    {
                        DLinq.Carrier carrer = UpdateCarrierForBG(dt, i, importedPolicyID, benefits_policyID, isNewPolicy, DataModel, LicID, ref errMsgPolicy, agencyName);
                        if (carrer != null)
                        {
                            objPolicy.CarrierId = carrer.CarrierId;
                            objPolicy.CarrierReference.Value = carrer;
                        }
                    }
                    #endregion

                    #region Product
                    UpdateCoverageForBG(dt, i, importedPolicyID, benefits_policyID, isNewPolicy, objPolicy, DataModel, LicID, ref errMsgPolicy, agencyName);
                    #endregion

                    #region Product Type - Not to be sent by benefits
                    /*Not to be sent by benefits 
                             * if (isNewPolicy || string.IsNullOrEmpty(objPolicy.ProductType))
                            {
                                if (dt.Columns.Contains("Product_Type"))
                                {
                                    strProductType = Convert.ToString(dt.Rows[i]["Product_Type"]);
                                    DLinq.CoverageNickName covName = (from p in DataModel.CoverageNickNames where p.NickName == strProductType select p).FirstOrDefault();
                                    covNickName = (covName != null) ? covName.NickName : string.Empty; //to be used later in code, so kept separate
                                    objPolicy.ProductType = (!string.IsNullOrEmpty(covNickName)) ? covNickName : strProductType;
                                }
                            }*/
                    #endregion


                    #endregion

                    //Incoming Schedule 
                    #region Incoming Schedule

                    bool allowImportedSchedule = true;
                    Guid SettingsScheduleID = Guid.Empty;
                    bool oldInScheduleExists = false;

                    //Check if old policy and schedules exist
                    if (!isNewPolicy)
                    {
                        PolicyToolIncommingShedule oldSchedule = PolicyToolIncommingShedule.GettingPolicyIncomingSchedule(objPolicy.PolicyId, agencyName);
                        if (oldSchedule != null && oldSchedule.IncomingScheduleID != Guid.Empty)
                        {
                            if ((oldSchedule.Mode == Mode.Standard
                                && (oldSchedule.FirstYearPercentage != 0 || oldSchedule.RenewalPercentage != 0))
                                || oldSchedule.Mode == Mode.Custom //assuming this will always have value 
                                )
                            {
                                oldInScheduleExists = true;
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy - Old policy and old incoming schedule exists with non-zero values: ", true, agencyName);
                            }
                        }
                    }

                    int? incomingPaymentType = 1;
                    if (dt.Columns.Contains("Commission Type"))
                    {
                        string strCommisionType = Convert.ToString(dt.Rows[i]["Commission Type"]);
                        incomingPaymentType = PolicCompType(strCommisionType, CompTypeList, agencyName);
                    }
                    else if (dt.Columns.Contains("CommissionType"))
                    {
                        string strCommisionType = Convert.ToString(dt.Rows[i]["CommissionType"]);
                        incomingPaymentType = PolicCompType(strCommisionType, CompTypeList, agencyName);
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy incomingPaymentType: " + incomingPaymentType, true, agencyName);
                    PayorIncomingSchedule payorSchedule = null;

                    //Configure incoming schedule only when not existing or new policy
                    if (isNewPolicy || !oldInScheduleExists)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy - incoming schedule to be read: ", true, agencyName);
                        objPolicy.IncomingPaymentTypeId = incomingPaymentType;
                        objPolicy.SplitPercentage = 100;

                        inSchedule = new PolicyToolIncommingShedule();
                        inSchedule.PolicyId = objPolicy.PolicyId;
                        inSchedule.IncomingScheduleID = Guid.NewGuid();
                        inSchedule.CustomType = CustomMode.Graded;

                        /************ July 03, 2020 - custom incoming from BG***************/

                        int mod = 0;
                        Int32.TryParse(Convert.ToString(dt.Rows[i]["IncomingMode"]), out mod); // 0 or 1
                        inSchedule.Mode = (Mode)mod;

                        if (inSchedule.Mode == Mode.Custom)
                        {
                            int customMod = 1;
                            Int32.TryParse(Convert.ToString(dt.Rows[i]["IncomingCustomModeType"]), out customMod); // 1 or 2 
                            inSchedule.CustomType = (CustomMode)customMod;
                        }

                        inSchedule.FirstYearPercentage = 0; //(!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                        inSchedule.RenewalPercentage = 0;

                        string strOutPercentOfPremium = "% of Premium";
                        if (dt.Columns.Contains("PaymentType"))
                        {
                            strOutPercentOfPremium = Convert.ToString(dt.Rows[i]["PaymentType"]);
                        }
                        inSchedule.ScheduleTypeId = (!string.IsNullOrEmpty(strOutPercentOfPremium) && strOutPercentOfPremium.ToLower().Contains("head")) ? 2 : 1;
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy - incoming schedule init with default values : ", true, agencyName);

                        if (dt.Columns.Contains("CoBrokerSplit"))
                        {
                            string strSplit = Convert.ToString(dt.Rows[i]["CoBrokerSplit"]);
                            double splitPer = 0;
                            double.TryParse(strSplit, out splitPer);
                            objPolicy.SplitPercentage = splitPer == 0 ? 100 : splitPer;
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy - Split % read as : " + splitPer + ", set as: " + objPolicy.SplitPercentage, true, agencyName);
                        }
                        //First check excel for any incoming schedule - if present , import
                        //if (dt.Columns.Contains("Commissions - First Year %") && dt.Columns.Contains("Commissions - Renewal %"))
                        //{
                        if (inSchedule.Mode == Mode.Standard && ((dt.Columns.Contains("Commissions - First Year %") || dt.Columns.Contains("CommissionsFirstYear")) && ((dt.Columns.Contains("Commissions - Renewal %") || dt.Columns.Contains("CommissionsRenewal")))))
                        {
                            string strInFirstYear = dt.Columns.Contains("Commissions - First Year %") ? Convert.ToString(dt.Rows[i]["Commissions - First Year %"]) : dt.Columns.Contains("CommissionsFirstYear") ? Convert.ToString(dt.Rows[i]["CommissionsFirstYear"]) : "0";
                            string strInRenewYear = dt.Columns.Contains("Commissions - Renewal %") ? Convert.ToString(dt.Rows[i]["Commissions - Renewal %"]) : dt.Columns.Contains("CommissionsRenewal") ? Convert.ToString(dt.Rows[i]["CommissionsRenewal"]) : "0";
                            double frst = 0; double renew = 0;
                            double.TryParse(strInFirstYear, out frst);
                            double.TryParse(strInRenewYear, out renew);

                            inSchedule.FirstYearPercentage = frst; //(!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                            inSchedule.RenewalPercentage = renew;
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy - first year% read as : " + frst + ", renewal as: " + renew, true, agencyName);

                            if (frst == 0 && renew == 0)
                            {
                                //get payor configuration if present 
                                if (objPolicy.PayorId != null && objPolicy.CarrierId != null && objPolicy.CoverageId != null && incomingPaymentType != null && !string.IsNullOrEmpty(objPolicy.ProductType))
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy reading payor configuration : ", true, agencyName);
                                    payorSchedule = PayorIncomingSchedule.GetPayorScheduleDetails((Guid)objPolicy.PayorId, (Guid)objPolicy.CarrierId, (Guid)objPolicy.CoverageId, (Guid)objPolicy.PolicyLicenseeId, objPolicy.ProductType, (int)incomingPaymentType, agencyName);
                                    if (payorSchedule != null && payorSchedule.IncomingScheduleID != Guid.Empty)
                                    {
                                        allowImportedSchedule = false;
                                        SettingsScheduleID = payorSchedule.IncomingScheduleID;
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration found: SettingsScheduleID - " + SettingsScheduleID, true, agencyName);
                                        //apply later after policy is saved.
                                    }
                                    else
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration NOT found: " + incomingPaymentType, true, agencyName);
                                    }
                                }
                            }
                            else
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule init as in excel 1st : " + frst + ", renewal: " + renew, true, agencyName);
                            }

                        }
                        else if (inSchedule.Mode == Mode.Custom)
                        {
                            if (dt.Columns.Contains("IncomingCustomList"))
                            {
                                DataTable dtIncoming = (dt.Rows[i]["IncomingCustomList"] as DataTable);
                                inSchedule.GradedSchedule = new List<Graded>();
                                inSchedule.NonGradedSchedule = new List<NonGraded>();
                                if (dtIncoming != null && dtIncoming.Rows.Count > 0)
                                {
                                    try
                                    {
                                        foreach (DataRow row in dtIncoming.Rows)
                                        {
                                            if (row.ItemArray == null || row.ItemArray.Count() == 0)
                                            {
                                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule row found blank", true, agencyName);
                                                continue;
                                            }
                                            if (inSchedule.CustomType == CustomMode.Graded)
                                            {
                                                Graded objGraded = new Graded();

                                                double from = 0;
                                                string strFrom = Convert.ToString(row["Froms"]);
                                                double.TryParse(strFrom, out from);
                                                objGraded.From = from;

                                                double to = 0;
                                                string strTo = Convert.ToString(row["To"]);
                                                double.TryParse(strTo, out to);
                                                objGraded.To = to;

                                                double percnt = 0;
                                                string strPercnt = Convert.ToString(row["Value"]);
                                                double.TryParse(strPercnt, out percnt);
                                                objGraded.Percent = percnt;

                                                inSchedule.GradedSchedule.Add(objGraded);
                                            }
                                            else
                                            {
                                                NonGraded objNonGraded = new NonGraded();

                                                int year = 0;
                                                string strYear = Convert.ToString(row["Year"]);
                                                int.TryParse(strYear, out year);
                                                objNonGraded.Year = year;

                                                double percnt = 0;
                                                string strPercnt = Convert.ToString(row["Value"]);
                                                double.TryParse(strPercnt, out percnt);
                                                objNonGraded.Percent = percnt;

                                                inSchedule.NonGradedSchedule.Add(objNonGraded);
                                            }
                                        }


                                        //Validation of the list 
                                        string errIncoming = ValidateIncomingSchedule(inSchedule.GradedSchedule, inSchedule.NonGradedSchedule, (inSchedule.CustomType == CustomMode.Graded), inSchedule.Mode.ToString(), inSchedule.ScheduleTypeId, agencyName);
                                        if (!string.IsNullOrEmpty(errIncoming))
                                        {
                                            errMsgPolicy.Add("IncomingSchedule", errIncoming);
                                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: IncomingSchedule fields error : " + errIncoming, true, agencyName);
                                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        errMsgPolicy.Add("IncomingSchedule", "Error adding Custom Schedule: " + ex.Message);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: IncomingSchedule fields missing ", true, agencyName);
                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                    }
                                }
                                else
                                {
                                    errMsgPolicy.Add("IncomingSchedule", "Custom Schedule list is missing");
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: IncomingSchedule fields missing", true, agencyName);
                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                }
                            }
                            else
                            {
                                errMsgPolicy.Add("IncomingSchedule", "Custom Schedule list is missing");
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: IncomingSchedule fields missing", true, agencyName);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            }
                        }
                        else //Lastly apply payor config if found 
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Frst/renewal columns not found in excel, looking for payor config", true, agencyName);
                            if (objPolicy.PayorId != null && objPolicy.CarrierId != null && objPolicy.CoverageId != null && incomingPaymentType != null && !string.IsNullOrEmpty(objPolicy.ProductType))
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy reading payor configuration : ", true, agencyName);
                                PayorIncomingSchedule schedule = PayorIncomingSchedule.GetPayorScheduleDetails((Guid)objPolicy.PayorId, (Guid)objPolicy.CarrierId, (Guid)objPolicy.CoverageId, (Guid)objPolicy.PolicyLicenseeId, objPolicy.ProductType, (int)incomingPaymentType, agencyName);
                                if (schedule != null && schedule.IncomingScheduleID != Guid.Empty)
                                {
                                    allowImportedSchedule = false;
                                    SettingsScheduleID = schedule.IncomingScheduleID;
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration found: SettingsScheduleID - " + SettingsScheduleID, true, agencyName);
                                    //apply later after policy is saved.
                                }
                                else
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration NOT found: " + incomingPaymentType, true, agencyName);
                                }
                            }

                        }
                    }

                    #endregion

                    #region Common fields - always update with insert/Update
                    bool OutPercentOfPremium = false;
                    //try
                    //{
                    //Advance - Moved to update always as per Kevin - Aug 21, 2019
                    UpdateBenefitsMandatoryFields(dt, i, importedPolicyID, benefits_policyID, isNewPolicy, ref objPolicy, ref errMsgPolicy, agencyName);

                    //Account Exec
                    UpdateBenefitsAccountOwner(dt, i, importedPolicyID, benefits_policyID, isNewPolicy, AgentList.ToList<dynamic>(), ref objPolicy, ref errMsgPolicy, agencyName);

                    //Term Date
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: mandatory fields init done : " + policyIDKey, true, agencyName);


                    //try
                    //{
                    #region Outgoing Split

                    // Iscustom schedule
                    if (dt.Columns.Contains("IsCustomSchedule"))
                    {
                        bool isCustomschedule = false;
                        string strIsCustom = Convert.ToString(dt.Rows[i]["IsCustomSchedule"]);
                        bool.TryParse(strIsCustom, out isCustomschedule);
                        objPolicy.IsCustomBasicSchedule = isCustomschedule;

                        //Read date type if true
                        if (objPolicy.IsCustomBasicSchedule == true)
                        {
                            string outgoingScheduleType = "";
                            if (dt.Columns.Contains("OutgoingScheduleDateType"))
                            {
                                outgoingScheduleType = Convert.ToString(dt.Rows[i]["OutgoingScheduleDateType"]);
                                objPolicy.CustomScheduleDateType = outgoingScheduleType;
                            }
                            ActionLogger.Logger.WriteImportPolicyLog("CustomScheduleDateType - " + objPolicy.CustomScheduleDateType, true, agencyName);

                            //Make sure date is populated, for custom schedule - else error
                            if (string.IsNullOrEmpty(outgoingScheduleType))
                            {
                                errMsgPolicy.Add("OutgoingScheduleDateType", "OutgoingScheduleDateType must be present for Custom outgoing schedule.");
                                ActionLogger.Logger.WriteImportPolicyLog("OutgoingScheduleDateType must be present for Custom outgoing schedule", true, agencyName);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            }
                        }
                    }

                    // Is tiered schedule
                    if (dt.Columns.Contains("IsTieredSchedule"))
                    {
                        bool isTiered = false;
                        string strIsTiered = Convert.ToString(dt.Rows[i]["IsTieredSchedule"]);
                        bool.TryParse(strIsTiered, out isTiered);
                        objPolicy.IsTieredSchedule = isTiered;
                    }

                    //  schedule type 
                    int scheduleType = 2;
                    if (dt.Columns.Contains("OutgoingScheduleType"))
                    {
                        string strType = Convert.ToString(dt.Rows[i]["OutgoingScheduleType"]);
                        int.TryParse(strType, out scheduleType);
                    }



                    if (dt.Columns.Contains("OutgoingSchedule"))
                    {
                        DataTable dtOutgoing = (dt.Rows[i]["OutgoingSchedule"] as DataTable);
                        //ActionLogger.Logger.WriteImportPolicyLog("dtOutgoing: " + dtOutgoing.ToStringDump(), true, agencyName);

                        if (dtOutgoing != null)
                        {
                            foreach (DataRow row in dtOutgoing.Rows)
                            {
                                OutGoingPayment objOutgoing = new OutGoingPayment();
                                objOutgoing.ScheduleTypeId = scheduleType;

                                string payeeName = "";
                                string payeeID = "";

                                if (dtOutgoing.Columns.Contains("PayeeName"))
                                {
                                    payeeName = Convert.ToString(row["PayeeName"]);
                                }
                                if (dtOutgoing.Columns.Contains("PayeeId"))
                                {
                                    payeeID = Convert.ToString(row["PayeeID"]);
                                }

                                if (string.IsNullOrEmpty(payeeName) && string.IsNullOrEmpty(payeeID))
                                {
                                    continue;
                                }

                                //Here - ensured that atleast one of ID or name exists, so check CD user based on ID/Name
                                string cdPayeeID = GetCDUserID(AgentList, payeeID, payeeName, agencyName);
                                if (string.IsNullOrEmpty(cdPayeeID)) // Payee not found 
                                {
                                    if (!string.IsNullOrEmpty(payeeName))
                                    {
                                        string errorKey = "PayeeName - " + payeeName;
                                        if (!errMsgPolicy.ContainsKey(errorKey))
                                        {
                                            errMsgPolicy.Add(errorKey, payeeName + " not available in the system");
                                        }
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + payeeName + " not available in the system ", true, agencyName);
                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                    }
                                    else
                                    {
                                        string errorKey = "PayeeID - " + payeeID;
                                        if (!errMsgPolicy.ContainsKey(errorKey))
                                        {
                                            errMsgPolicy.Add(errorKey, payeeID + " not available in the system");
                                        }
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + payeeID + " not available in the system ", true, agencyName);
                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                    }
                                }
                                else // Payee exists - add schedule 
                                {
                                    try
                                    {
                                        objOutgoing.PayeeUserCredentialId = new Guid(cdPayeeID);
                                        objOutgoing.OutgoingScheduleId = Guid.NewGuid();
                                        objOutgoing.PolicyId = objPolicy.PolicyId;

                                        if (dtOutgoing.Columns.Contains("FirstYearPercentage"))
                                        {
                                            double frstyr = 0;
                                            string strFirstYr = Convert.ToString(row["FirstYearPercentage"]);
                                            double.TryParse(strFirstYr, out frstyr);
                                            objOutgoing.FirstYearPercentage = frstyr;
                                        }

                                        if (dtOutgoing.Columns.Contains("RenewalPercentage"))
                                        {
                                            double frstyr = 0;
                                            string strFirstYr = Convert.ToString(row["RenewalPercentage"]);
                                            double.TryParse(strFirstYr, out frstyr);
                                            objOutgoing.RenewalPercentage = frstyr;
                                        }

                                        if (dtOutgoing.Columns.Contains("CustomStartDate"))
                                        {
                                            DateTime dtstart = DateTime.MinValue;
                                            DateTime.TryParse(Convert.ToString(row["CustomStartDate"]), out dtstart);
                                            objOutgoing.CustomStartDate = dtstart;
                                        }

                                        if (dtOutgoing.Columns.Contains("CustomEndDate"))
                                        {
                                            DateTime dtend = DateTime.MinValue;
                                            DateTime.TryParse(Convert.ToString(row["CustomEndDate"]), out dtend);
                                            objOutgoing.CustomEndDate = dtend;
                                        }

                                        if (dtOutgoing.Columns.Contains("TierNumber"))
                                        {
                                            int tier = 0;
                                            int.TryParse(Convert.ToString(row["TierNumber"]), out tier);
                                            objOutgoing.TierNumber = tier;
                                        }

                                        if (dtOutgoing.Columns.Contains("SplitPercent"))
                                        {
                                            double split = 0;
                                            double.TryParse(Convert.ToString(row["SplitPercent"]), out split);
                                            objOutgoing.SplitPercent = split;
                                        }

                                        OutGoingField.Add(objOutgoing);
                                    }
                                    catch (Exception ex)
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: outgoing split exception : " + ex.Message, true, agencyName);
                                        errMsgPolicy.Add("Error reading outgoing schedule", ex.Message);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception:" + " An error ocurred while saving outgoing schedule in the system ", true, agencyName);
                                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                    }

                                }
                            }
                        }
                    }

                    if (OutGoingField != null && OutGoingField.Count > 0)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: outgoing schedule received with values, updating : ", true, agencyName);
                        string invalidMsg = ValidateOutgoingSchedule(OutGoingField, LicID, objPolicy.IsCustomBasicSchedule, objPolicy.IsTieredSchedule, objPolicy.OriginalEffectiveDate, oldInScheduleExists, inSchedule, SettingsScheduleID, objPolicy.PolicyId, agencyName);

                        if (!string.IsNullOrEmpty(invalidMsg) && !errMsgPolicy.ContainsKey(importedPolicyID))
                        {
                            //DLinq.Policy objPolicy = new DLinq.Policy();
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: outgoing split error : " + invalidMsg, true, agencyName);
                            errMsgPolicy.Add("Outgoing split error", invalidMsg);
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                            //iterationError = true;
                            //DataModel.Refresh(System.Data.Objects.RefreshMode.ClientWins, objPolicy);
                            //objPolicy = null;
                            //GC.Collect();
                            errorCount++;
                            Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Import Policy: outgoing split error : " + invalidMsg);
                            errorList.Add(m);
                            detachObject(DataModel, objPolicy);
                            
                            continue;
                            //objPolicy
                            //foreach (DLinq.Policy x in objPolicy)
                            //{
                            //    x.Dispose();
                            //}
                        }
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: outgoing schedule received blank : ", true, agencyName);
                        if (isNewPolicy)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: new policy - so adding default to house  : ", true, agencyName);
                            if (objPolicy.IsCustomBasicSchedule == true && objPolicy.OriginalEffectiveDate != null)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: new policy - custome schedule and effective present, adding 100% from eff date  : ", true, agencyName);
                                OutGoingField = CompleteOutgoingSchedule(null, objPolicy.PolicyId, LicID, agencyName, objPolicy.IsCustomBasicSchedule, objPolicy.OriginalEffectiveDate);
                            }
                            else
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: new policy - custome schedule and effective present false, adding 100% fin default schedule and setting custom Off : ", true, agencyName);
                                objPolicy.IsCustomBasicSchedule = false;
                                OutGoingField = CompleteOutgoingSchedule(null, objPolicy.PolicyId, LicID, agencyName);
                            }
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: existing policy - so No change  : ", true, agencyName);
                        }
                    }


                    #endregion

                    #endregion
                    //ActionLogger.Logger.WriteImportPolicyLog("Import Policy: outgoing split init done : " + policyIDKey, true,agencyName);
                    //}
                    //catch (Exception ex)
                    //{
                    //    errMsgPolicy.Add("ProducerName", "ProducerName not available in the system");
                    //    //errorCount++;
                    //    //Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedPolicyID, benefits_policyID, "Producer1Name: " + ex.Message);
                    //    //errorList.Add(m);
                    //    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: outgoing fields  : " + ex.Message, true,agencyName);
                    //    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                    //    //continue;

                    //}

                    #region Track date - checked at last [after origin date found]
                    /* Not to be sent by benefits, poulated from agency's default track date so, no exception throw from here as well*/
                    if (isNewPolicy || objPolicy.TrackFromDate == null)
                    {

                        //if (dt.Columns.Contains("Track_From"))
                        //{
                        //if (dt.Rows[i]["Track_From"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Track_From"])))
                        if (dtTrack != null && dtTrack != DateTime.MinValue)
                        {
                            objPolicy.TrackFromDate = (objPolicy.OriginalEffectiveDate != null && objPolicy.OriginalEffectiveDate > dtTrack) ? objPolicy.OriginalEffectiveDate : dtTrack;
                        }
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy TRack Date : " + objPolicy.TrackFromDate, true, agencyName);
                    }
                    #endregion


                    #region Save Data

                    //Check if any error, then return 
                    if (errMsgPolicy.Count > 0)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy Error list found, returning... ", true, agencyName);
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        errorCount++;
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedID, benefits_policyID, output);
                        errorList.Add(m);
                        detachObject(DataModel, objPolicy);
                        continue;
                    }

                    try
                    {
                        //Save only when policy ID not present in error list
                        if (errorList == null || (errorList != null && errorList.Count == 0) || (errorList != null && errorList.Count > 0 && errorList.Where(x => x.OriginalPlanID == importedPolicyID).ToList().Count == 0))
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy NO Error list found, starting save... ", true, agencyName);
                            #region Client
                            try
                            {

                                if (isNewPolicy || objPolicy.PolicyClientId == null)
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy new client saving:  ", true, agencyName);
                                    string client = clientName;
                                    Client objClient = (new Client()).GetClientByClientName(client, LicID, agencyName);
                                    //Get Client ID by Get Client name
                                    if (objClient == null)
                                    {
                                        //Create new client
                                        objClient = new Client();
                                        objClient.ClientId = Guid.NewGuid();
                                        string strClientValue = string.Empty;

                                        strClientValue = (client.Length > 49) ? client.Substring(0, 49) : client;

                                        //objClnt.Name = policy.ClientName;
                                        objClient.Name = strClientValue;
                                        objClient.LicenseeId = LicID;
                                        objClient.IsDeleted = false;
                                        Client.AddUpdateClient(client, LicID, objClient.ClientId, agencyName);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: client saved as new : " + policyIDKey, true, agencyName);
                                    }
                                    objPolicy.ClientReference.Value = (from p in DataModel.Clients where p.ClientId == objClient.ClientId select p).FirstOrDefault();
                                }
                                //following requires client reference, so added at last
                                if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Insured))
                                {
                                    if (dt.Columns.Contains("AccountName"))
                                    {
                                        objPolicy.Insured = Convert.ToString(dt.Rows[i]["AccountName"]);
                                    }
                                    else
                                    {
                                        objPolicy.Insured = objPolicy.Client.Name;
                                    }
                                }
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy client ID:  " + objPolicy.PolicyClientId + ", insured:  " + objPolicy.Insured, true, agencyName);

                                #region PolicyType calculation used here because PolicyType is calculated based on the values
                                //if (isNewPolicy || string.IsNullOrEmpty(objPolicy.PolicyType))
                                //{
                                string PolicyType = "";
                                try
                                {
                                    //seeA
                                    //if (dt.Columns.Contains("NewBusiness")) //|| dt.Columns.Contains("New Business?"))
                                    //{
                                    //    string strTypeOfPolicy = Convert.ToString(dt.Rows[rowIndex]["NewBusiness"]); //dt.Columns.Contains("New?") ? Convert.ToString(dt.Rows[i]["New?"]) : (dt.Columns.Contains("New Business?") ? Convert.ToString(dt.Rows[i]["New Business?"]) : "");
                                    //    objPolicy.PolicyType = (!string.IsNullOrEmpty(strTypeOfPolicy) && (strTypeOfPolicy.ToLower() == "rewrite" || strTypeOfPolicy.ToLower() == "replace")) ? "Replace" : "New";
                                    //}
                                    //else if (isNewPolicy)
                                    //{
                                    //    objPolicy.PolicyType = "New";
                                    //}


                                    //if(isNewPolicy)
                                    //{
                                    //    objPolicy.IsManuallyChanged = false;
                                    //}


                                    objPolicy.IsManuallyChanged = isNewPolicy ? false : objPolicy.IsManuallyChanged;

                                    if (objPolicy.IsManuallyChanged == false)
                                    {
                                        PolicyType = calculatePolicyType(objPolicy.OriginalEffectiveDate, objPolicy.PolicyClientId, LicID, objPolicy.PolicyId, objPolicy.CoverageId);
                                        objPolicy.PolicyType = PolicyType;
                                    }
                                    //seeA 
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    errMsgPolicy.Add("NewBusiness", ex.Message);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: New Business field  exception : " + ex.Message, true, agencyName);
                                    AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                }
                                //}
                            }
                            catch (Exception ex)
                            {
                                errorCount++; //Allow this code here , as thisis the last error possibility
                                errMsgPolicy.Add("AccountName", ex.Message);
                                string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                                Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedID, benefits_policyID, output);
                                errorList.Add(m);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: client : " + ex.Message, true, agencyName);
                                AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                                detachObject(DataModel, objPolicy);
                                continue;
                            }


                            #endregion
                            //try
                            //{

                            if (isNewPolicy)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("isNewPolicy", true, agencyName);
                                DataModel.AddToPolicies(objPolicy);
                                DataModel.SaveChanges();
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy : policy saved successfully", true, agencyName);
                                PolicyToLearnPost.AddLearnedAfterImport(objPolicy.PolicyId, "", covNickName, strProductType, importedID, agencyName);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy : learned fields saved successfully", true, agencyName);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Policy saved as new : " + policyIDKey, true, agencyName);
                                PolicyToLearnPost.AddLearnedAfterImport(objPolicy.PolicyId, "", covNickName, strProductType, importedID, agencyName, benefits_policyID);
                            }
                            else
                            {
                                if (
                                    objPolicy.PolicyId.ToString().ToUpper() == "ACF11C55-3DD3-4776-B78B-0BE82304B9C4" ||
                                    objPolicy.PolicyId.ToString().ToUpper() == "25213C08-D485-410E-BFFD-4DDEF866CDB9" ||
                                    objPolicy.PolicyId.ToString().ToUpper() == "DE7BCF2A-C5C8-4BAE-BEB4-5A78811119A6" ||
                                    objPolicy.PolicyId.ToString().ToUpper() == "64EDDE82-3A67-41B0-8B4E-486114F93FEB" ||
                                    objPolicy.PolicyId.ToString().ToUpper() == "B893D3C4-0A39-4B2E-B34C-9E3600303EFA" ||
                                    objPolicy.PolicyId.ToString().ToUpper() == "95B5A99D-DED7-4754-8853-E04147A69AAF"
                                 )
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("data is updated with PolicyId= " + objPolicy.PolicyId, true, agencyName);
                                }


                                ActionLogger.Logger.WriteImportPolicyLog("isNewPolicy else", true, agencyName);
                               
                                    DataModel.SaveChanges();
                               
                                //DataModel.Refresh();
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy : policy updated successfully", true, agencyName);
                                //  AddUpdatePolicyHistory(objPolicy.PolicyId);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy : policy history updated successfully", true, agencyName);
                                //Learned fields to update , once policy fields are updated 
                                PolicyToLearnPost.AddLearnedAfterImport(objPolicy.PolicyId, "", covNickName, strProductType, importedID, agencyName, benefits_policyID);

                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy : learned fields saved successfully", true, agencyName);
                                //   PolicyLearnedField.AddUpdateHistoryLearned(objPolicy.PolicyId);
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Policy updated : " + policyIDKey, true, agencyName);
                            }

                            /*    if (SettingsScheduleID != Guid.Empty)
                                {
                                    UpdatePolicySchedule(SettingsScheduleID, objPolicy.PolicyId, objPolicy.Advance, payorSchedule);
                                }
                                //Save Incoming and delete old 
                                else if (inSchedule != null)
                                {
                                    //if (tempSched != null && tempSched.FirstYearPercentage == 0 && tempSched.RenewalPercentage == 0)
                                    //{
                                    //    PolicyToolIncommingShedule.DeleteSchedule(objPolicy.PolicyId); //If schedule occurs with 0 split, then delete before adding new
                                    //    ActionLogger.Logger.WriteImportPolicyLog("Import Policy old schedule is deleted", true,agencyName);
                                    //}
                                    //inSchedule.AddUpdate();
                                    ActionLogger.Logger.WriteImportPolicyLog("Incoming schedule added for the policy", true,agencyName);
                                }*/
                            //Save Incoming 
                            if (SettingsScheduleID != Guid.Empty)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("in SettingsScheduleID", true, agencyName);
                                UpdatePolicySchedule(SettingsScheduleID, objPolicy.PolicyId, objPolicy.Advance, payorSchedule, agencyName);
                            }
                            else if (inSchedule != null)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("in inSchedule", true, agencyName);
                                PolicyToolIncommingShedule.SavePolicyIncomingSchedule(inSchedule, agencyName);
                                //inSchedule.AddUpdate();
                                ActionLogger.Logger.WriteImportPolicyLog("Incoming schedule added for the policy", true, agencyName);
                            }

                            //Save Outgoing
                            //Delete old schdule- outgoing  payments witj old schedule will remian intact, new will take effect on new entries only.
                            //Check only if new outgoing schedule exists, then overwrite the old
                            //This is to exclude the case when policy is existing and excel has blank entries. Without this, existing schedule will be overwritten
                            // by 100% to house for existing policy 
                            if (/*isNewPolicy || */  OutGoingField != null && OutGoingField.Count > 0)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule adding to the policy", true, agencyName);
                                //  OutGoingField = CompleteOutgoingSchedule(OutGoingField, objPolicy.PolicyId, LicID); Commented as any error is returned back in the response and this method doesn't handle tiered/custom cases
                                OutGoingPayment.DeletePolicyOutGoingSchedulebyPolicyId(objPolicy.PolicyId, agencyName);
                                ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule removed from the policy", true, agencyName);
                                OutGoingPayment.AddUpdate(OutGoingField, agencyName, (bool)objPolicy.IsCustomBasicSchedule, (bool)objPolicy.IsTieredSchedule);
                                ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule added for the policy", true, agencyName);
                            }
                            else
                            {
                                if (isNewPolicy) //adding 100% to house under default schedule, when no schedule present for new policy
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule - New policy and no outgoing scheudle present, adding to house ", true, agencyName);
                                    OutGoingField = CompleteOutgoingSchedule(null, objPolicy.PolicyId, LicID, agencyName);
                                    OutGoingPayment.AddUpdate(OutGoingField, agencyName, false, false);
                                    ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule added for the policy", true, agencyName);
                                }
                            }

                            if (isNewPolicy)
                                addCount++;
                            else
                                updateCount++;


                            //Adding status in DB
                            AddImportStatusToDB(importedPolicyID, isNewPolicy, true, benefits_policyID, agencyName);


                            //Add to idList
                            Benefits_PolicyID id = new Benefits_PolicyID();
                            id.CDPolicyID = Convert.ToString(objPolicy.PolicyId).ToUpper(); //Nov 05 -2020 , always to be in upper case as required by BG.
                            id.OriginalPlanID = importedID;
                            id.PolicyPlanID = benefits_policyID;
                            idList.Add(id);
                        }

                        #endregion

                    }
                    catch (Exception ex)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("error3", true, agencyName);
                        ActionLogger.Logger.WriteImportPolicyLog("Exception adding policy: " + ex.Message, true, agencyName);
                        if (ex.InnerException != null)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("inner exception: " + ex.InnerException, true, agencyName);
                        }
                        errorCount++;
                        errMsgPolicy.Add("SaveError", ex.Message);
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(importedID, benefits_policyID, output);
                        errorList.Add(m);
                        AddImportStatusToDB(importedPolicyID, isNewPolicy, false, benefits_policyID, agencyName);
                    }
                }

                objStatus.NewCount = addCount;
                objStatus.UpdateCount = updateCount;
                objStatus.ErrorCount = errorCount;
                objStatus.ErrorList = errorList;
                objStatus.SuccessList = idList;

                //if(errorCount > 0)
                //{
                //    ActionLogger.Logger.WriteImportPolicyLog("error: "+errorList.FirstOrDefault().Message, true,agencyName);
                //}

                return objStatus;
            }
        }




        static Guid AddNewUserFromBenefits(string name, Guid licenseeID, bool isExec = false)
        {
            ActionLogger.Logger.WriteImportPolicyLog("AddNewUserFromBenefits request: name - " + name, true);
            try
            {
                User u = new User();
                u.UserCredentialID = Guid.NewGuid();
                u.NickName = name;
                u.LicenseeId = licenseeID;//for benefits only 
                string[] arrNames = name.Trim().Split(' ');
                if (arrNames != null && arrNames.Length > 0)
                {
                    u.FirstName = arrNames[0];
                    if (arrNames.Length > 1)
                        u.LastName = arrNames[1];
                }
                u.Company = "Benefit Pro Insurance Services Inc";
                u.Role = UserRole.Agent;
                u.IsAccountExec = isExec;
                u.AddUpdateUser();
                ActionLogger.Logger.WriteImportPolicyLog("AddNewUserFromBenefits : user added successfully, name - " + name, true);

                //When account Exec is to be added , but account exec check fails, then return empty guid indicating failure.
                if (isExec && !u.CheckAccoutExec(u.UserCredentialID))
                {
                    return Guid.Empty;
                }

                u.LinkNewUserToHouse();
                //On success, return new ID
                return u.UserCredentialID;
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddNewUserFromBenefits exception - " + ex.Message, true);
                return Guid.Empty;
            }
        }


        static void AddImportStatusToDB(string policyID, bool isNew, bool isSuccess, string benefitsID, string agencyName = "")//find1
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddImportStatusToDB request: policyID -  " + policyID + ", isNew - " + isNew + ", isSuccess - " + isSuccess, true, agencyName);
                //DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                //EntityConnection ec = (EntityConnection)ctx.Connection;
                //SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                //string adoConnStr = sc.ConnectionString;

                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_AddImportStatus", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UniquePolicyID", policyID);
                        cmd.Parameters.AddWithValue("@IsNew", isNew);
                        cmd.Parameters.AddWithValue("@IsSuccess", isSuccess);
                        cmd.Parameters.AddWithValue("@BenefitsPolicyID", benefitsID);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception AddImportStatusToDB :  PolicyID -  " + policyID + ", ex: " + ex.Message, true, agencyName);
            }
        }

        static void UpdatePolicySchedule(Guid ScheduleID, Guid PolicyID, int? advance, PayorIncomingSchedule policyIncomingSchedule, string agencyName = "")
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("in UpdatePolicySchedule", true, agencyName);

                Guid newID;
                int adv = (advance == null) ? 0 : (int)advance;
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    ActionLogger.Logger.WriteImportPolicyLog("in connection string", true, agencyName);

                    using (SqlCommand cmd = new SqlCommand("sp_UpdatePolicyIncomingSchedule", con))
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("in sp call", true, agencyName);

                        cmd.CommandType = CommandType.StoredProcedure;

                        ActionLogger.Logger.WriteImportPolicyLog("ScheduleID:" + ScheduleID, true, agencyName);
                        ActionLogger.Logger.WriteImportPolicyLog("PolicyID:" + PolicyID, true, agencyName);
                        ActionLogger.Logger.WriteImportPolicyLog("advanceFromImport:" + adv, true, agencyName);

                        cmd.Parameters.AddWithValue("@IncomingScheduleId", ScheduleID);
                        cmd.Parameters.AddWithValue("@PolicyID", PolicyID);
                        cmd.Parameters.AddWithValue("@advanceFromImport", adv);

                        cmd.Parameters.Add("@newScheduleID", SqlDbType.UniqueIdentifier);
                        cmd.Parameters["@newScheduleID"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        newID = (Guid)cmd.Parameters["@newScheduleID"].Value;

                        ActionLogger.Logger.WriteImportPolicyLog("newID:" + newID, true, agencyName);
                    }
                }

                if (policyIncomingSchedule.Mode == Mode.Custom)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("in mode custom", true, agencyName);
                    //Assigning ID of new incoming schedule to be assigned further to graded/non-graded list
                    policyIncomingSchedule.IncomingScheduleID = newID;
                    if (policyIncomingSchedule.CustomType == CustomMode.Graded)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("in custom mode graded", true, agencyName);
                        PolicyToolIncommingShedule.SaveGradedSchedule(policyIncomingSchedule, agencyName, PolicyID);
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("in custom mode non graded", true, agencyName);
                        PolicyToolIncommingShedule.SaveNonGradedSchedule(policyIncomingSchedule, agencyName, PolicyID);
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception UpdatePolicySchedule :  PolicyID -  " + PolicyID + ", ex: " + ex.Message, true, agencyName);
            }
        }
        public static PolicyImportStatus ImportPolicy(DataTable dt, ObservableCollection<User> GlobalAgentList, Guid LicID, ObservableCollection<CompType> CompTypeList)
        {
            #region Variables
            PolicyImportStatus objStatus = new PolicyImportStatus();
            string policyIDKey = System.Configuration.ConfigurationSettings.AppSettings["PolicyIDKeyName"];
            char[] spCharac = System.Configuration.ConfigurationSettings.AppSettings["AgentCharactersToTrim"].ToCharArray();  //{ '(', '[', '-' };

            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyIDKey: " + policyIDKey, true);
            int addCount = 0;
            int updateCount = 0;
            int errorCount = 0;
            List<string> errorList = new List<string>();


            List<OutGoingPayment> OutGoingField = new List<OutGoingPayment>();
            PolicyToolIncommingShedule inSchedule = null;
            string strProductType = string.Empty;
            string covNickName = string.Empty;
            Guid houseOwner = Guid.Empty;

            #endregion
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Data model init", true);
                var AgentList = (from p in DataModel.UserCredentials
                                 join o in DataModel.UserDetails on p.UserCredentialId equals o.UserCredentialId
                                 where p.LicenseeId == LicID /*&& p.RoleId == 3*/ && p.IsDeleted == false
                                 select new
                                 {
                                     p.UserCredentialId,
                                     o.NickName,
                                     p.UserName,
                                     p.RoleId,
                                     o.FirstName,
                                     o.LastName
                                 }).ToList();
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Agent list fetched", true);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DLinq.Policy objPolicy = new DLinq.Policy();
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Iteration: " + i, true);
                    string importedPolicyID = dt.Columns.Contains(policyIDKey) ? Convert.ToString(dt.Rows[i][policyIDKey]).Trim() : (dt.Columns.Contains("Life Insurance Plan ID") ? Convert.ToString(dt.Rows[i]["Life Insurance Plan ID"]).Trim() : (dt.Columns.Contains("Policy Plan ID") ? Convert.ToString(dt.Rows[i]["Policy Plan ID"]).Trim() : ""));

                    //Added blank ID check - May 28, 2019
                    if (string.IsNullOrEmpty(importedPolicyID))
                    {
                        errorList.Add(importedPolicyID);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Policy ID found null/blank, skipping record", true);
                        errorCount++;
                        continue;
                    }


                    string clientName = string.Empty;
                    if (dt.Columns.Contains("AccountName"))
                    {
                        clientName = Convert.ToString(dt.Rows[i]["AccountName"]);
                    }
                    else if (dt.Columns.Contains("Account Name"))
                    {
                        clientName = Convert.ToString(dt.Rows[i]["Account Name"]);
                    }

                    if (string.IsNullOrEmpty(clientName))
                    {
                        errorList.Add(importedPolicyID);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Account Name found null/blank, skipping record", true);
                        errorCount++;
                        continue;
                    }

                    //string importedPolicyID = dt.Columns.Contains(policyIDKey) ? Convert.ToString(dt.Rows[i][policyIDKey]).Trim() : (dt.Columns.Contains("Life Insurance Plan ID") ? Convert.ToString(dt.Rows[i]["Life Insurance Plan ID"]).Trim() : "");
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: importedPolicyID: " + importedPolicyID, true);
                    OutGoingField = new List<OutGoingPayment>();
                    try
                    {
                        bool isNewPolicy = false;
                        Guid policyID = IsPolicyExistingWithImportID(importedPolicyID, LicID);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyID: " + policyID, true);
                        if (policyID != Guid.Empty)
                        {
                            objPolicy = (from p in DataModel.Policies where p.PolicyId == policyID select p).FirstOrDefault();
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Existing policy", true);


                            //check if client exists or deleted in the system
                            //if deleted, then add the given as new 
                            if (objPolicy.Client != null && Convert.ToBoolean(objPolicy.Client.IsDeleted))
                            {
                                objPolicy.PolicyClientId = null;
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Client found deleted , so setting as null ", true);
                            }
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: New policy", true);
                            isNewPolicy = true;
                            objPolicy.PolicyId = Guid.NewGuid();
                            objPolicy.IsTrackPayment = true;
                            objPolicy.PolicyLicenseeId = LicID;
                            objPolicy.TerminationReasonId = null;
                            objPolicy.IsTrackMissingMonth = true;
                            objPolicy.IsTrackIncomingPercentage = true;
                            objPolicy.CreatedOn = DateTime.Today;
                            objPolicy.IsIncomingBasicSchedule = true;
                            objPolicy.IsOutGoingBasicSchedule = true;
                            objPolicy.CreatedBy = new Guid("AA38DF84-2E30-43CA-AED3-7276224D1B7E");
                            objPolicy.IsDeleted = false;
                        }

                        #region Fields that should be updated when blank or woth new policy
                        try
                        {

                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.PolicyType))
                            {
                                if (dt.Columns.Contains("New?") || dt.Columns.Contains("New Business?"))
                                {
                                    string strTypeOfPolicy = dt.Columns.Contains("New?") ? Convert.ToString(dt.Rows[i]["New?"]) : (dt.Columns.Contains("New Business?") ? Convert.ToString(dt.Rows[i]["New Business?"]) : "");
                                    objPolicy.PolicyType = (!string.IsNullOrEmpty(strTypeOfPolicy) && (strTypeOfPolicy.ToLower() == "rewrite" || strTypeOfPolicy.ToLower() == "replace")) ? "Replace" : "New";
                                }
                                else if (isNewPolicy)
                                {
                                    objPolicy.PolicyType = "New";
                                }
                            }
                            //if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Insured))
                            //{
                            //    if (dt.Columns.Contains("insured?"))
                            //    {
                            //        objPolicy.Insured = Convert.ToString(dt.Rows[i]["insured"]);
                            //    }
                            //    else
                            //    {
                            //        objPolicy.Insured = objPolicy.Client.Name;
                            //    }
                            //}
                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.PolicyNumber))
                            {
                                if (dt.Columns.Contains("Group #"))
                                {
                                    objPolicy.PolicyNumber = Convert.ToString(dt.Rows[i]["Group #"]);
                                }
                            }
                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.SubmittedThrough))
                            {
                                if (dt.Columns.Contains("Submitted Through"))
                                {
                                    objPolicy.SubmittedThrough = Convert.ToString(dt.Rows[i]["Submitted Through"]);
                                }
                            }
                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Enrolled))
                            {
                                if (dt.Columns.Contains("Number of Covered Lives"))
                                {
                                    objPolicy.Enrolled = Convert.ToString(dt.Rows[i]["Number of Covered Lives"]);
                                }
                            }
                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Eligible))
                            {
                                if (dt.Columns.Contains("eligible"))
                                {
                                    objPolicy.Eligible = Convert.ToString(dt.Rows[i]["eligible"]);
                                }
                            }
                            //if (isNewPolicy || (objPolicy.Advance == null))
                            //{
                            //    if (dt.Columns.Contains("Advanced Payment Number"))
                            //    {
                            //        objPolicy.Advance = (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Advanced Payment Number"]))) ? Convert.ToInt32(Convert.ToString(dt.Rows[i]["Advanced Payment Number"])) : 0;
                            //    }
                            //}
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: optional fields , to be filled when blanks init done : " + policyIDKey, true);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: optional fields exception : " + ex.Message, true);
                            continue;
                        }


                        try
                        {
                            //Track date
                            if (isNewPolicy || objPolicy.TrackFromDate == null)
                            {
                                if (dt.Columns.Contains("Track From"))
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy trackDate coumn found: ", true);
                                    if (dt.Rows[i]["Track From"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Track From"])))
                                    {
                                        string trackDate = Convert.ToString(dt.Rows[i]["Track From"]);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy trackDate: " + trackDate, true);
                                        //DateTime dtTrac = DateTime.MinValue;
                                        //DateTime.TryParse(trackDate, out dtTrac);
                                        //objPolicy.TrackFromDate = dtTrac;// Convert.ToDateTime(trackDate);

                                        double dblEff = 0;
                                        Double.TryParse(trackDate, out dblEff);
                                        if (dblEff > 0)
                                        {
                                            objPolicy.TrackFromDate = DateTime.FromOADate(dblEff);
                                        }
                                        else
                                        {
                                            objPolicy.TrackFromDate = Convert.ToDateTime(trackDate);
                                        }
                                    }
                                }
                                else
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy trackDate column not found", true);
                                }
                            }
                            //Mode
                            if (isNewPolicy || objPolicy.PolicyModeId == null)
                            {
                                if (dt.Columns.Contains("Modal Number"))
                                {
                                    string strMode = Convert.ToString(dt.Rows[i]["Modal Number"]);
                                    objPolicy.PolicyModeId = (!string.IsNullOrEmpty(strMode)) ? PolicyModeID(strMode) : PolicyModeID("0");
                                }
                            }

                            //premium
                            if (isNewPolicy || objPolicy.MonthlyPremium == null)
                            {
                                if (dt.Columns.Contains("Modal Premium"))
                                {
                                    string strPremiuum = Convert.ToString(dt.Rows[i]["Modal Premium"]);
                                    decimal prem = 0;
                                    decimal.TryParse(strPremiuum, out prem);
                                    if (prem == 0)
                                    {
                                        decimal.TryParse(strPremiuum, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out prem);
                                    }
                                    objPolicy.MonthlyPremium = prem;// (!string.IsNullOrEmpty(strPremiuum)) ? Convert.ToDecimal(strPremiuum) : 0;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy execption :premium/mode/trackDate : " + ex.Message, true);
                            continue;
                        }
                        //Payor  
                        #region Payor
                        try
                        {
                            if (isNewPolicy || objPolicy.PayorId == null)
                            {
                                if (dt.Columns.Contains("Payor Commission Dept"))
                                {
                                    string strPayor = Convert.ToString(dt.Rows[i]["Payor Commission Dept"]);
                                    if (!string.IsNullOrEmpty(strPayor))
                                    {
                                        DLinq.Payor py = (from p in DataModel.Payors where (p.PayorName.ToLower() == strPayor.ToLower() || (p.PayorName.ToLower() != strPayor.ToLower() && p.NickName.ToLower() == strPayor.ToLower())) select p).FirstOrDefault();
                                        //if (py == null)
                                        {
                                            // py = (from p in DataModel.Payors where p.NickName.ToLower() == strPayor.ToLower() select p).FirstOrDefault();
                                            if (py != null)
                                            {
                                                objPolicy.PayorId = py.PayorId;
                                                objPolicy.PayorReference.Value = py;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Carrier
                            if (isNewPolicy || objPolicy.CarrierId == null)
                            {
                                if (dt.Columns.Contains("Carrier Commission Dept"))
                                {
                                    string strCarr = Convert.ToString(dt.Rows[i]["Carrier Commission Dept"]);
                                    if (!string.IsNullOrEmpty(strCarr))
                                    {
                                        DLinq.Carrier cr = (from p in DataModel.Carriers where p.CarrierName.ToLower() == strCarr.ToLower() select p).FirstOrDefault();
                                        if (cr == null)
                                        {
                                            DLinq.CarrierNickName crN = (from p in DataModel.CarrierNickNames where (p.PayorId == objPolicy.PayorId || (p.PayorId != objPolicy.PayorId && p.NickName == strCarr)) select p).FirstOrDefault();
                                            // if (crN == null)
                                            //{
                                            //    crN = (from p in DataModel.CarrierNickNames where p.NickName == strCarr select p).FirstOrDefault();
                                            //}
                                            if (crN != null)
                                            {
                                                objPolicy.CarrierId = crN.CarrierId;
                                                objPolicy.CarrierReference.Value = crN.CarrierReference.Value;
                                            }
                                        }
                                        else
                                        {
                                            objPolicy.CarrierId = cr.CarrierId;
                                            objPolicy.CarrierReference.Value = cr;
                                        }

                                    }
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: payor/carrier : " + ex.Message, true);
                            continue;
                        }

                        try
                        {
                            #region Product
                            if (isNewPolicy || objPolicy.CoverageId == null)
                            {
                                if (dt.Columns.Contains("Line Of Coverage"))
                                {
                                    string strProduct = Convert.ToString(dt.Rows[i]["Line Of Coverage"]);
                                    DLinq.Coverage cov = (from p in DataModel.Coverages where p.ProductName == strProduct select p).FirstOrDefault();
                                    if (cov != null)
                                    {
                                        objPolicy.CoverageId = cov.CoverageId;
                                        objPolicy.CoverageReference.Value = cov;
                                    }
                                }
                            }
                            #endregion

                            #region Product Type
                            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.ProductType))
                            {
                                if (dt.Columns.Contains("Product Type"))
                                {
                                    strProductType = Convert.ToString(dt.Rows[i]["Product Type"]);
                                    DLinq.CoverageNickName covName = (from p in DataModel.CoverageNickNames where p.NickName == strProductType select p).FirstOrDefault();
                                    covNickName = (covName != null) ? covName.NickName : string.Empty; //to be used later in code, so kept separate
                                    objPolicy.ProductType = (!string.IsNullOrEmpty(covNickName)) ? covNickName : strProductType;
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: product : " + ex.Message, true);
                            continue;
                        }

                        //Incoming Schedule 
                        #region Incoming Schedule

                        //    bool allowImportedSchedule = true;
                        Guid SettingsScheduleID = Guid.Empty;
                        bool oldInScheduleExists = false;
                        //  DLinq.PolicyIncomingSchedule tempSched = null;

                        //Check if incoming is already set with 0 splits
                        /*Acme - changes after custom schedule implementation
                         * if (objPolicy.IsIncomingBasicSchedule == true)
                         {
                             tempSched = objPolicy.PolicyIncomingSchedules.FirstOrDefault();
                             if (tempSched != null)
                             {
                                 if (tempSched.FirstYearPercentage > 0 || tempSched.RenewalPercentage > 0)
                                 {
                                     oldInScheduleExists = true;
                                 }
                             }
                         }*/

                        //Check if old policy and schedules exist
                        if (!isNewPolicy)
                        {
                            PolicyToolIncommingShedule oldSchedule = PolicyToolIncommingShedule.GettingPolicyIncomingSchedule(objPolicy.PolicyId);
                            if (oldSchedule != null)
                            {
                                if ((oldSchedule.Mode == Mode.Standard && (oldSchedule.FirstYearPercentage != 0 || oldSchedule.RenewalPercentage != 0))
                                    || oldSchedule.Mode == Mode.Custom //assuming this will always have value 
                                    )
                                {
                                    oldInScheduleExists = true;
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy - Old policy and old incoming schedule exists with non-zero values: ", true);
                                }
                            }
                        }

                        int? incomingPaymentType = 1;
                        if (dt.Columns.Contains("Commission Type"))
                        {
                            string strCommisionType = Convert.ToString(dt.Rows[i]["Commission Type"]);
                            incomingPaymentType = PolicCompType(strCommisionType, CompTypeList);
                        }
                        else if (dt.Columns.Contains("CommissionType"))
                        {
                            string strCommisionType = Convert.ToString(dt.Rows[i]["CommissionType"]);
                            incomingPaymentType = PolicCompType(strCommisionType, CompTypeList);
                        }
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy incomingPaymentType: " + incomingPaymentType, true);
                        PayorIncomingSchedule payorSchedule = null;

                        //Configure incoming schedule only when not existing or new policy
                        if (isNewPolicy || !oldInScheduleExists)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy - incoming schedule to be read: ", true);
                            objPolicy.IncomingPaymentTypeId = incomingPaymentType;
                            objPolicy.SplitPercentage = 100;

                            inSchedule = new PolicyToolIncommingShedule();
                            inSchedule.PolicyId = objPolicy.PolicyId;
                            inSchedule.IncomingScheduleID = Guid.NewGuid();
                            inSchedule.CustomType = CustomMode.Graded;
                            inSchedule.Mode = Mode.Standard;
                            inSchedule.FirstYearPercentage = 0; //(!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                            inSchedule.RenewalPercentage = 0;

                            string strOutPercentOfPremium = "% of Premium";
                            if (dt.Columns.Contains("Payment Type"))
                            {
                                strOutPercentOfPremium = Convert.ToString(dt.Rows[i]["Payment Type"]);
                            }
                            inSchedule.ScheduleTypeId = (!string.IsNullOrEmpty(strOutPercentOfPremium) && strOutPercentOfPremium.ToLower().Contains("head")) ? 2 : 1;
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy - incoming schedule init with default values : ", true);

                            if (dt.Columns.Contains("Co Broker Split"))
                            {
                                string strSplit = Convert.ToString(dt.Rows[i]["Co Broker Split"]);
                                double splitPer = 0;
                                double.TryParse(strSplit, out splitPer);
                                objPolicy.SplitPercentage = splitPer == 0 ? 100 : splitPer;
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy - Split % read as : " + splitPer + ", set as: " + objPolicy.SplitPercentage, true);
                            }
                            //First check excel for any incoming schedule - if present , import
                            if (dt.Columns.Contains("Commissions - First Year %") && dt.Columns.Contains("Commissions - Renewal %"))
                            {
                                string strInFirstYear = Convert.ToString(dt.Rows[i]["Commissions - First Year %"]);
                                string strInRenewYear = Convert.ToString(dt.Rows[i]["Commissions - Renewal %"]);
                                double frst = 0; double renew = 0;
                                double.TryParse(strInFirstYear, out frst);
                                double.TryParse(strInRenewYear, out renew);

                                inSchedule.FirstYearPercentage = frst; //(!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                                inSchedule.RenewalPercentage = renew;
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy - first year% read as : " + frst + ", renewal as: " + renew, true);

                                if (frst == 0 && renew == 0)
                                {
                                    //get payor configuration if present 
                                    if (objPolicy.PayorId != null && objPolicy.CarrierId != null && objPolicy.CoverageId != null && incomingPaymentType != null && !string.IsNullOrEmpty(objPolicy.ProductType))
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy reading payor configuration : ", true);
                                        payorSchedule = PayorIncomingSchedule.GetPayorScheduleDetails((Guid)objPolicy.PayorId, (Guid)objPolicy.CarrierId, (Guid)objPolicy.CoverageId, (Guid)objPolicy.PolicyLicenseeId, objPolicy.ProductType, (int)incomingPaymentType);
                                        if (payorSchedule != null && payorSchedule.IncomingScheduleID != Guid.Empty)
                                        {
                                            //allowImportedSchedule = false;
                                            SettingsScheduleID = payorSchedule.IncomingScheduleID;
                                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration found: SettingsScheduleID - " + SettingsScheduleID, true);
                                            //apply later after policy is saved.
                                        }
                                        else
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration NOT found: " + incomingPaymentType, true);
                                        }
                                    }
                                }
                                else
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule init as in excel 1st : " + frst + ", renewal: " + renew, true);
                                }

                            }
                            else
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Frst/renewal columns not found in excel, looking for payor config", true);
                                if (objPolicy.PayorId != null && objPolicy.CarrierId != null && objPolicy.CoverageId != null && incomingPaymentType != null && !string.IsNullOrEmpty(objPolicy.ProductType))
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy reading payor configuration : ", true);
                                    PayorIncomingSchedule schedule = PayorIncomingSchedule.GetPayorScheduleDetails((Guid)objPolicy.PayorId, (Guid)objPolicy.CarrierId, (Guid)objPolicy.CoverageId, (Guid)objPolicy.PolicyLicenseeId, objPolicy.ProductType, (int)incomingPaymentType);
                                    if (schedule != null && schedule.IncomingScheduleID != Guid.Empty)
                                    {
                                        //allowImportedSchedule = false;
                                        SettingsScheduleID = schedule.IncomingScheduleID;
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration found: SettingsScheduleID - " + SettingsScheduleID, true);
                                        //apply later after policy is saved.
                                    }
                                    else
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy payor configuration NOT found: " + incomingPaymentType, true);
                                    }
                                }

                            }
                        }

                        //New check to overwrite schedule if found configured in the system, else will work the same way, incomingPaymentType taken from excel
                        /* if (objPolicy.PayorId != null && objPolicy.CarrierId != null && objPolicy.CoverageId != null && incomingPaymentType != null && !string.IsNullOrEmpty(objPolicy.ProductType))
                         {
                             ActionLogger.Logger.WriteImportPolicyLog("Import Policy incomingPaymentType11: " + incomingPaymentType, true);
                             PayorIncomingSchedule schedule = PayorIncomingSchedule.GetPayorScheduleDetails((Guid)objPolicy.PayorId, (Guid)objPolicy.CarrierId, (Guid)objPolicy.CoverageId, (Guid)objPolicy.PolicyLicenseeId, objPolicy.ProductType, (int)incomingPaymentType);
                             if (schedule != null && schedule.IncomingScheduleID != Guid.Empty)
                             {
                                 allowImportedSchedule = false;
                                 SettingsScheduleID = schedule.IncomingScheduleID;
                                 //apply later after policy is saved.
                             }
                         }
                         ActionLogger.Logger.WriteImportPolicyLog("Import Policy allowImportedSchedule: " + allowImportedSchedule, true);
                         if (allowImportedSchedule)
                         {
                             if (isNewPolicy || (objPolicy.IsIncomingBasicSchedule != true) || (!oldInScheduleExists))
                             {
                                 if ((dt.Columns.Contains("Commission Type") || dt.Columns.Contains("CommissionType")) && dt.Columns.Contains("Payment Type") && dt.Columns.Contains("Commissions - First Year %") && dt.Columns.Contains("Commissions - Renewal %") && dt.Columns.Contains("Co Broker Split"))
                                 {
                                     if (tempSched != null && tempSched.FirstYearPercentage == 0 && tempSched.RenewalPercentage == 0)
                                     {
                                         PolicyToolIncommingShedule.DeleteSchedule(objPolicy.PolicyId); //If schedule occurs with 0 split, then delete before adding new
                                         ActionLogger.Logger.WriteImportPolicyLog("Import Policy old schedule is deleted", true);
                                     }


                                     // string strCommisionType = Convert.ToString(dt.Rows[i]["Commission Type"]);
                                     objPolicy.IncomingPaymentTypeId = incomingPaymentType; // PolicCompType(strCommisionType, CompTypeList);
                                     string strOutPercentOfPremium = Convert.ToString(dt.Rows[i]["Payment Type"]);
                                     string strInFirstYear = Convert.ToString(dt.Rows[i]["Commissions - First Year %"]);
                                     string strInRenewYear = Convert.ToString(dt.Rows[i]["Commissions - Renewal %"]);
                                     string strSplit = Convert.ToString(dt.Rows[i]["Co Broker Split"]);
                                     double splitPer = 0;
                                     double.TryParse(strSplit, out splitPer);
                                     objPolicy.SplitPercentage = splitPer;

                                     inSchedule = new PolicyToolIncommingShedule();
                                     inSchedule.PolicyId = objPolicy.PolicyId;
                                     inSchedule.IncomingScheduleID = Guid.NewGuid();
                                     inSchedule.Mode = Mode.Standard;
                                     inSchedule.CustomType = CustomMode.Graded;
                                     inSchedule.ScheduleTypeId = (!string.IsNullOrEmpty(strOutPercentOfPremium) && strOutPercentOfPremium.ToLower().Contains("head")) ? 2 : 1;
                                     double frst = 0; double renew = 0;
                                     double.TryParse(strInFirstYear, out frst);
                                     double.TryParse(strInRenewYear, out renew);
                                     inSchedule.FirstYearPercentage = frst; //(!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                                     inSchedule.RenewalPercentage = renew;// (!string.IsNullOrEmpty(strInRenewYear)) ? Convert.ToDouble(strInRenewYear) : 0;
                                     ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule init done : " + policyIDKey, true);
                                 }
                                 else
                                 {
                                     ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule not found in excel, setting default with 0 values ", true);
                                     objPolicy.IncomingPaymentTypeId = incomingPaymentType; // PolicCompType(strCommisionType, CompTypeList);
                                     objPolicy.SplitPercentage = 100;

                                     inSchedule = new PolicyToolIncommingShedule();
                                     inSchedule.PolicyId = objPolicy.PolicyId;
                                     inSchedule.IncomingScheduleID = Guid.NewGuid();
                                     inSchedule.Mode = Mode.Standard;
                                     inSchedule.CustomType = CustomMode.Graded;
                                     inSchedule.ScheduleTypeId = 1;
                                     inSchedule.FirstYearPercentage = 0; //(!string.IsNullOrEmpty(strInFirstYear)) ? Convert.ToDouble(strInFirstYear) : 0;
                                     inSchedule.RenewalPercentage = 0;// (!string.IsNullOrEmpty(strInRenewYear)) ? Convert.ToDouble(strInRenewYear) : 0;
                                     ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Incoming schedule default init done : " + policyIDKey, true);
                                 }
                             }
                         }*/
                        #endregion

                        #endregion

                        #region Common fields - always update with insert/Update
                        bool OutPercentOfPremium = false;
                        try
                        {
                            //Advance - Moved to update always as per Kevin - Aug 21, 2019
                            if (dt.Columns.Contains("Advanced Payment Number"))
                            {
                                string strAdvance = Convert.ToString(dt.Rows[i]["Advanced Payment Number"]);
                                int adv = 0;
                                Int32.TryParse(strAdvance, out adv);

                                if (adv > 0)
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Advanced  payment number from file : " + adv + ", updating...", true);
                                    objPolicy.Advance = adv;
                                } // (!string.IsNullOrEmpty()) ? Convert.ToInt32(Convert.ToString(dt.Rows[i]["Advanced Payment Number"])) : 0;
                                else
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Advanced  payment number from file : " + strAdvance + ", NOT updating...", true);
                                }

                            }
                            else
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Advanced  payment number not found: " + policyIDKey, true);
                            }
                            //Status
                            if (dt.Columns.Contains("Plan Status Description") && dt.Rows[i]["Plan Status Description"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Plan Status Description"])))
                            {
                                string strStatus = dt.Rows[i]["Plan Status Description"].ToString();
                                objPolicy.PolicyStatusId = (strStatus.ToLower() == "active") ? 0 : (strStatus.ToLower() == "pending") ? 2 : 1;
                            }
                            else if (dt.Columns.Contains("Current Plan Status") && dt.Rows[i]["Current Plan Status"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Current Plan Status"])))
                            {
                                string strStatus = dt.Rows[i]["Current Plan Status"].ToString();
                                objPolicy.PolicyStatusId = (strStatus.ToLower() == "active") ? 0 : (strStatus.ToLower() == "pending") ? 2 : 1;
                            }
                            else
                            {
                                objPolicy.PolicyStatusId = 1;
                            }
                            //Original effective date
                            if (dt.Columns.Contains("Original Plan Start Date"))
                            {
                                if (dt.Rows[i]["Original Plan Start Date"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Original Plan Start Date"])))
                                {
                                    string effDate = Convert.ToString(dt.Rows[i]["Original Plan Start Date"]);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy string effDate: " + effDate, true);
                                    //check if value in double, then fetch OA Date
                                    double dblEff = 0;
                                    Double.TryParse(effDate, out dblEff);
                                    if (dblEff > 0)
                                    {
                                        objPolicy.OriginalEffectiveDate = DateTime.FromOADate(dblEff);
                                    }
                                    else
                                    {
                                        objPolicy.OriginalEffectiveDate = DateTime.Parse(effDate, System.Globalization.CultureInfo.CurrentCulture); //Convert.ToDateTime(effDate);
                                    }
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy datetime effDate: " + objPolicy.OriginalEffectiveDate, true);
                                    if (isNewPolicy || objPolicy.TrackFromDate == null)
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy track from date seting from effective date: " + objPolicy.OriginalEffectiveDate, true);
                                        objPolicy.TrackFromDate = objPolicy.OriginalEffectiveDate;
                                    }
                                }
                            }
                            //Account Exec
                            if (dt.Columns.Contains("Account Owner: Full Name"))
                            {
                                if (dt.Rows[i]["Account Owner: Full Name"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Account Owner: Full Name"])))
                                {
                                    string acctExec = Convert.ToString(dt.Rows[i]["Account Owner: Full Name"]);
                                    var objUser = AgentList.Where(d => (d.FirstName + " " + d.LastName).ToLower() == acctExec.ToLower() || (!string.IsNullOrEmpty(d.NickName) && d.NickName.ToLower() == acctExec.ToLower())).FirstOrDefault(); //User.GetUserIdWise(tempGuid);// (from p in DataModel.UserCredentials where (p.UserCredentialId == tempGuid && p.RoleId == 3 && p.IsDeleted == false) select p).FirstOrDefault();

                                    Guid tempGuid = Guid.Empty;
                                    Guid.TryParse(acctExec, out tempGuid);
                                    if (tempGuid != Guid.Empty)
                                    {
                                        objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                    }
                                    //Guid tempGuid = new Guid(acctExec);
                                    // User objUser = GlobalAgentList.Where(d => d.UserCredentialID == tempGuid).FirstOrDefault();
                                    if (objUser != null /*&& objUser.Role == UserRole.Agent*/)
                                    {
                                        //Need to get nick name
                                        if (!string.IsNullOrEmpty(objUser.NickName))
                                        {
                                            objPolicy.AccoutExec = objUser.NickName;
                                            objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                        }
                                        else
                                        {
                                            objPolicy.AccoutExec = objUser.UserName;
                                            objPolicy.UserCredentialId = objUser.UserCredentialId; //tempGuid;
                                        }
                                        bool isexec = (new User().CheckAccoutExec(objUser.UserCredentialId)); //Sets the flag of accExec true for this userID 
                                    }
                                }
                            }
                            //Term Date
                            if (objPolicy.PolicyStatusId == 1)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: status is terminated, so updating plan end date and reason ", true);
                                if (dt.Columns.Contains("Plan End Date"))
                                {
                                    if (dt.Rows[i]["Plan End Date"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Plan End Date"])))
                                    {
                                        string termDate = Convert.ToString(dt.Rows[i]["Plan End Date"]);
                                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy string termDate: " + termDate, true);
                                        //check if value in double, then fetch OA Date
                                        double dblTerm = 0;
                                        Double.TryParse(termDate, out dblTerm);
                                        if (dblTerm > 0)
                                        {
                                            objPolicy.PolicyTerminationDate = DateTime.FromOADate(dblTerm);
                                        }
                                        else
                                        {
                                            objPolicy.PolicyTerminationDate = Convert.ToDateTime(termDate);
                                        }
                                        //  objPolicy.PolicyTerminationDate = Convert.ToDateTime(termDate);
                                    }
                                }
                                //Term reason
                                if (dt.Columns.Contains("Termination Reason"))
                                {
                                    if (dt.Rows[i]["Termination Reason"] != null && !String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Termination Reason"])))
                                    {
                                        objPolicy.TerminationReasonId = PolicTermisionID(Convert.ToString(dt.Rows[i]["Termination Reason"]));
                                    }
                                }
                            }
                            else
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: status is NOT terminated, so ignoring plan end date and reason ", true);
                            }
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: mandatory fields init done : " + policyIDKey, true);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: mandatory fields  : " + ex.Message, true);
                            continue;
                        }
                        //Outgoing split 
                        try
                        {
                            #region Outgoing Split
                            if (dt.Columns.Contains("Producer 1: Full Name") && dt.Columns.Contains("Producer 1 First Year %") && dt.Columns.Contains("Producer 1 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 1: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 1: Full Name"]);
                                    int spIndex = strPrimaryBroker.IndexOfAny(spCharac);
                                    strPrimaryBroker = (spIndex > 0) ? strPrimaryBroker.Substring(0, spIndex - 1) : strPrimaryBroker;
                                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " strPrimaryBroker pri1: " + strPrimaryBroker, true);

                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //User objUser = UsrLst.Where(u => u.UserName.ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            //  Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            ///var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                                            //var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower())).FirstOrDefault();
                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }

                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 1 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 1 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                //   OutgoingRecord.IsPrimaryAgent = true;95BD7F80-ADB3-4503-9A47-BB4CDB78E72A

                                                double frst = 0; double renew = 0;
                                                double.TryParse(strFirstPer, out frst);
                                                double.TryParse(strRenewalPer, out renew);

                                                OutgoingRecord.FirstYearPercentage = frst;// string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = renew;// string.IsNullOrEmpty(strRenewalPer) ? 0 : Convert.ToDouble(strRenewalPer);
                                                                                         //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for pri1: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dt.Columns.Contains("Producer 2: Full Name") && dt.Columns.Contains("Producer 2 First Year %") && dt.Columns.Contains("Producer 2 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 2: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 2: Full Name"]);

                                    int spIndex = strPrimaryBroker.IndexOfAny(spCharac);
                                    strPrimaryBroker = (spIndex > 0) ? strPrimaryBroker.Substring(0, spIndex - 1) : strPrimaryBroker;
                                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " strPrimaryBroker pri2: " + strPrimaryBroker, true);

                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            //var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            // var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower())).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();
                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }
                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 2 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 2 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                OutgoingRecord.IsPrimaryAgent = false;

                                                double frst = 0; double renew = 0;
                                                double.TryParse(strFirstPer, out frst);
                                                double.TryParse(strRenewalPer, out renew);

                                                OutgoingRecord.FirstYearPercentage = frst;// string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = renew;

                                                //OutgoingRecord.FirstYearPercentage = string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                //OutgoingRecord.RenewalPercentage = string.IsNullOrEmpty(strRenewalPer) ? 0 : Convert.ToDouble(strRenewalPer);
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for ag1: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dt.Columns.Contains("Producer 3: Full Name") && dt.Columns.Contains("Producer 3 First Year %") && dt.Columns.Contains("Producer 3 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 3: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 3: Full Name"]);

                                    int spIndex = strPrimaryBroker.IndexOfAny(spCharac);
                                    strPrimaryBroker = (spIndex > 0) ? strPrimaryBroker.Substring(0, spIndex - 1) : strPrimaryBroker;
                                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " strPrimaryBroker pri3: " + strPrimaryBroker, true);

                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //User objUser = UsrLst.Where(u => u.UserName.ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            //   Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            // var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            //var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower())).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();

                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }
                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 3 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 3 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                OutgoingRecord.IsPrimaryAgent = false;

                                                double frst = 0; double renew = 0;
                                                double.TryParse(strFirstPer, out frst);
                                                double.TryParse(strRenewalPer, out renew);

                                                OutgoingRecord.FirstYearPercentage = frst;// string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = renew;
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for ag2: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dt.Columns.Contains("Producer 4: Full Name") && dt.Columns.Contains("Producer 4 First Year %") && dt.Columns.Contains("Producer 4 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 4: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 4: Full Name"]);

                                    int spIndex = strPrimaryBroker.IndexOfAny(spCharac);
                                    strPrimaryBroker = (spIndex > 0) ? strPrimaryBroker.Substring(0, spIndex - 1) : strPrimaryBroker;
                                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " strPrimaryBroker pri4: " + strPrimaryBroker, true);

                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //User objUser = UsrLst.Where(u => u.UserName.ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            //Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            //var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            // var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower())).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();

                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }
                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 4 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 4 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                OutgoingRecord.IsPrimaryAgent = false;
                                                double frst = 0; double renew = 0;
                                                double.TryParse(strFirstPer, out frst);
                                                double.TryParse(strRenewalPer, out renew);

                                                OutgoingRecord.FirstYearPercentage = frst;// string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = renew;
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for ag3: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dt.Columns.Contains("Producer 5: Full Name") && dt.Columns.Contains("Producer 5 First Year %") && dt.Columns.Contains("Producer 5 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 5: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 5: Full Name"]);
                                    int spIndex = strPrimaryBroker.IndexOfAny(spCharac);
                                    strPrimaryBroker = (spIndex > 0) ? strPrimaryBroker.Substring(0, spIndex - 1) : strPrimaryBroker;
                                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " strPrimaryBroker pri5: " + strPrimaryBroker, true);


                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //User objUser = UsrLst.Where(u => u.UserName.ToLower() == strPrimaryBroker.ToLower()).FirstOrDefault();
                                            //Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            //var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            //var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower())).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();

                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }
                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 5 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 5 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                OutgoingRecord.IsPrimaryAgent = false;
                                                double frst = 0; double renew = 0;
                                                double.TryParse(strFirstPer, out frst);
                                                double.TryParse(strRenewalPer, out renew);

                                                OutgoingRecord.FirstYearPercentage = frst;// string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = renew;
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for ag4: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (dt.Columns.Contains("Producer 6: Full Name") && dt.Columns.Contains("Producer 6 First Year %") && dt.Columns.Contains("Producer 6 Renewal %"))
                            {
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[i]["Producer 6: Full Name"])))
                                {
                                    string strPrimaryBroker = Convert.ToString(dt.Rows[i]["Producer 6: Full Name"]);

                                    int spIndex = strPrimaryBroker.IndexOfAny(spCharac);
                                    strPrimaryBroker = (spIndex > 0) ? strPrimaryBroker.Substring(0, spIndex - 1) : strPrimaryBroker;
                                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " strPrimaryBroker pri6: " + strPrimaryBroker, true);

                                    if (!string.IsNullOrWhiteSpace(strPrimaryBroker))
                                    {
                                        try
                                        {
                                            //Guid guidBrokerID = new Guid(strPrimaryBroker);
                                            //var objUser = AgentList.Where(u => u.UserCredentialId == guidBrokerID).FirstOrDefault();
                                            //var objUser = AgentList.Where(u => (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower())).FirstOrDefault();
                                            var objUser = AgentList.Where(u => (u.RoleId == 3 && (u.FirstName + " " + u.LastName).ToLower() == strPrimaryBroker.ToLower() || (u.LastName + " " + u.FirstName).ToLower() == strPrimaryBroker.ToLower() || (!string.IsNullOrEmpty(u.NickName) && u.NickName.ToLower() == strPrimaryBroker.ToLower()))).FirstOrDefault();

                                            Guid tempGuid = Guid.Empty;
                                            Guid.TryParse(strPrimaryBroker, out tempGuid);
                                            if (tempGuid != Guid.Empty)
                                            {
                                                objUser = AgentList.Where(d => d.UserCredentialId == tempGuid).FirstOrDefault();
                                            }
                                            if (objUser != null)
                                            {
                                                string strFirstPer = Convert.ToString(dt.Rows[i]["Producer 6 First Year %"]);
                                                string strRenewalPer = Convert.ToString(dt.Rows[i]["Producer 6 Renewal %"]);
                                                OutGoingPayment OutgoingRecord = new OutGoingPayment();
                                                OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                                                OutgoingRecord.PolicyId = objPolicy.PolicyId;
                                                OutgoingRecord.IsPrimaryAgent = false;
                                                double frst = 0; double renew = 0;
                                                double.TryParse(strFirstPer, out frst);
                                                double.TryParse(strRenewalPer, out renew);

                                                OutgoingRecord.FirstYearPercentage = frst;// string.IsNullOrEmpty(strFirstPer) ? 0 : Convert.ToDouble(strFirstPer);
                                                OutgoingRecord.RenewalPercentage = renew;
                                                //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                                                OutgoingRecord.PayeeUserCredentialId = objUser.UserCredentialId;
                                                OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                                                OutGoingField.Add(OutgoingRecord);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Exception adding outgoing for ag5: " + ex.Message, true);
                                            continue;
                                        }
                                    }
                                }
                            }
                            //Check for House if present
                            //try
                            //{
                            //    string strHouse = Convert.ToString(dt.Rows[i]["houseuc"]);
                            //    string strhousefy = Convert.ToString(dt.Rows[i]["housefy"]);
                            //    string strhousery = Convert.ToString(dt.Rows[i]["housern"]);
                            //    if (!string.IsNullOrEmpty(strHouse))
                            //    {
                            //        OutGoingPayment OutgoingRecord = new OutGoingPayment();
                            //        OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                            //        OutgoingRecord.PolicyId = objPolicy.PolicyId;
                            //        OutgoingRecord.IsPrimaryAgent = true;
                            //        OutgoingRecord.FirstYearPercentage = string.IsNullOrEmpty(strhousefy) ? 0 : Convert.ToDouble(strhousefy);
                            //        OutgoingRecord.RenewalPercentage = string.IsNullOrEmpty(strhousery) ? 0 : Convert.ToDouble(strhousery);
                            //        //   OutgoingRecord.Payor = objPolicy.PayorNickName;
                            //        //houseOwner = 
                            //        OutgoingRecord.PayeeUserCredentialId = new Guid(strHouse);
                            //        OutgoingRecord.ScheduleTypeId = 2;// OutPercentOfPremium ? 1 : 2;
                            //        if (OutgoingRecord.FirstYearPercentage > 0 || OutgoingRecord.RenewalPercentage > 0)
                            //            OutGoingField.Add(OutgoingRecord);
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    ActionLogger.Logger.WriteImportPolicyLog("Exception setting house owner: " + ex.Message + ", so chcking frim licensee ", true);
                            //    continue;
                            //}
                            #endregion

                            #endregion
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: outgoing split init done : " + policyIDKey, true);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: outgoing fields  : " + ex.Message, true);
                            continue;
                        }
                        #region Save Data
                        try
                        {
                            if (errorList == null || !errorList.Contains(importedPolicyID))
                            {
                                #region Client
                                try
                                {

                                    if (isNewPolicy || objPolicy.PolicyClientId == null)
                                    {
                                        string client = clientName; // Convert.ToString(dt.Rows[i]["Account Name"]);
                                        Client objClient = (new Client()).GetClientByClientName(client, LicID);
                                        //Get Client ID by Get Client name
                                        if (objClient == null)
                                        {
                                            //Create new client
                                            objClient = new Client();
                                            objClient.ClientId = Guid.NewGuid();
                                            string strClientValue = string.Empty;

                                            strClientValue = (client.Length > 49) ? client.Substring(0, 49) : client;

                                            //objClnt.Name = policy.ClientName;
                                            objClient.Name = strClientValue;
                                            objClient.LicenseeId = LicID;
                                            objClient.IsDeleted = false;
                                            Client.AddUpdateClient(client, LicID, objClient.ClientId);
                                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy: client saved as new : " + policyIDKey, true);
                                        }
                                        objPolicy.ClientReference.Value = (from p in DataModel.Clients where p.ClientId == objClient.ClientId select p).FirstOrDefault();
                                    }
                                    //following requires client reference, so added at last
                                    if (isNewPolicy || string.IsNullOrEmpty(objPolicy.Insured))
                                    {
                                        if (dt.Columns.Contains("insured"))
                                        {
                                            objPolicy.Insured = Convert.ToString(dt.Rows[i]["insured"]);
                                        }
                                        else
                                        {
                                            objPolicy.Insured = objPolicy.Client.Name;
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    errorCount++;
                                    errorList.Add(importedPolicyID);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: client : " + ex.Message, true);
                                    continue;
                                }
                                #endregion
                                if (isNewPolicy)
                                {
                                    DataModel.AddToPolicies(objPolicy);
                                    DataModel.SaveChanges();
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy : policy saved successfully", true);
                                    PolicyToLearnPost.AddLearnedAfterImport(objPolicy.PolicyId, "", covNickName, strProductType, importedPolicyID);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy : learned fields saved successfully", true);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Policy saved as new : " + policyIDKey, true);

                                }
                                else
                                {
                                    DataModel.SaveChanges();
                                    AddUpdatePolicyHistory(objPolicy.PolicyId);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy : policy updated successfully", true);
                                    //Learned fields to update , once policy fields are updated 
                                    PolicyToLearnPost.AddLearnedAfterImport(objPolicy.PolicyId, "", covNickName, strProductType, importedPolicyID);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy : learned fields saved successfully", true);
                                    PolicyLearnedField.AddUpdateHistoryLearned(objPolicy.PolicyId);
                                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Policy updated : " + policyIDKey, true);
                                }


                                //Save Incoming 
                                if (SettingsScheduleID != Guid.Empty)
                                {
                                    UpdatePolicySchedule(SettingsScheduleID, objPolicy.PolicyId, objPolicy.Advance, payorSchedule);
                                }
                                else if (inSchedule != null)
                                {
                                    PolicyToolIncommingShedule.SavePolicyIncomingSchedule(inSchedule);
                                    //inSchedule.AddUpdate();
                                    ActionLogger.Logger.WriteImportPolicyLog("Incoming schedule added for the policy", true);
                                }

                                //Save Outgoing
                                //Delete old schdule- outgoing  payments witj old schedule will remian intact, new will take effect on new entries only.
                                //Check only if new outgoing schedule exists, then overwrite the old
                                //This is to exclude the case when policy is existing and excel has blank entries. Without this, existing schedule will be overwritten
                                // by 100% to house for existing policy 
                                if (isNewPolicy || OutGoingField.Count > 0)
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule adding to the policy", true);
                                    OutGoingField = CompleteOutgoingSchedule(OutGoingField, objPolicy.PolicyId, LicID);
                                    OutGoingPayment.DeletePolicyOutGoingSchedulebyPolicyId(objPolicy.PolicyId);
                                    ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule removed from the policy", true);
                                    OutGoingPayment.AddUpdate(OutGoingField);
                                    ActionLogger.Logger.WriteImportPolicyLog("Outgoing schedule added for the policy", true);
                                }

                                if (isNewPolicy) addCount++;
                                else updateCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            errorList.Add(importedPolicyID);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: saving data fields  : " + ex.Message, true);
                            continue;
                        }
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Exception adding policy: " + ex.Message, true);
                        errorCount++;
                        errorList.Add(importedPolicyID);
                    }
                }

            }
            objStatus.ImportCount = addCount;
            objStatus.UpdateCount = updateCount;
            objStatus.ErrorCount = errorCount;
            objStatus.ErrorList = errorList;
            return objStatus;
        }


        /// <summary>
        /// Method tovalidate tier numbers in the schedules.
        /// </summary>
        /// <param name="tierList"></param>
        /// <returns></returns>
        static string ValidateTierNumbers(List<int?> tierList, string agencyName = "")
        {
            string result = "";
            //If anyone tier missing, return error
            if (!tierList.Contains(1))
            {
                result = "Please configure Tier 1 for the schedule.";
                return result;
            }

            //If any number other that 1&2 exist in list, return error
            foreach (int? tier in tierList)
            {
                if (tier == null)
                {
                    result = "Tier number cannot be blank and must be mentioned as 1 or 2 for the schedule.";
                    return result;
                }
                if (tier != 1 && tier != 2)
                {
                    result = "'Tiered' schedule cannot have tier numbers other than 1 and 2 for the schedule.";
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// Gives first year and renewal% based on incoming schedule
        /// Standard or non-graded
        /// Never required for Graded 
        /// </summary>
        /// <param name="isOldIncomingPresent"></param>
        /// <param name="inSchedule"></param>
        /// <param name="settingsID"></param>
        /// <param name="policyID"></param>
        /// <param name="firstYrIncoming"></param>
        /// <param name="renewalYrIncoming"></param>
        /// <returns></returns>
        static void GetIncomingScheduleDetail(bool isOldIncomingPresent, PayorIncomingSchedule inSchedule, Guid settingsID, Guid policyID, DateTime? effective, bool IsCustomOutgoing, DateTime? customDate, out double? firstYrIncoming, out double? renewalYrIncoming, out double? expectedSplit, string agencyName = "")
        {

            firstYrIncoming = 0;
            renewalYrIncoming = 0;
            expectedSplit = 0;
            try
            {
                PolicyToolIncommingShedule schedule = null;
                if (isOldIncomingPresent)
                {
                    schedule = PolicyToolIncommingShedule.GettingPolicyIncomingSchedule(policyID, agencyName);
                }
                else
                {
                    if (settingsID != Guid.Empty) // this is kept here, as it will only be set when received schedule was Standard and blank. [Custom and blank non-graded is not allowed]
                    {
                        schedule = PayorIncomingSchedule.GetBasicScheduleOnSettingsID(settingsID, agencyName);
                    }
                    else //inSchedule present 
                    {
                        schedule = inSchedule as PolicyToolIncommingShedule;
                    }
                }

                //Get first/renewal from schedule
                if (schedule != null)
                {
                    int policyYear = (IsCustomOutgoing) ? getPolicyAgeFromEffective((DateTime)customDate, effective, agencyName) : getPolicyAgeFromEffective(DateTime.Now, effective, agencyName);

                    if (schedule.Mode == Mode.Custom && schedule.CustomType == CustomMode.NonGraded)
                    {
                        firstYrIncoming = schedule.NonGradedSchedule.Where(x => x.Year == 1).FirstOrDefault().Percent;


                        int maxYear = schedule.NonGradedSchedule.OrderByDescending(x => x.Year).FirstOrDefault().Year;
                        policyYear = (policyYear > maxYear) ? maxYear : policyYear;

                        //if (year > 1) //If policy current year is > 1, then use renewal year's %
                        //{
                        renewalYrIncoming = schedule.NonGradedSchedule.OrderBy(x => x.Year).Where(x => x.Year == policyYear).FirstOrDefault().Percent;
                        //}
                        if (policyYear == 1) // if policy is in year 1 , then use next higher year's % as renewal%
                        {
                            var year2Record = schedule.NonGradedSchedule.OrderBy(x => x.Year).Where(x => x.Year > 1).FirstOrDefault();
                            if (year2Record != null) //This also handles the case where only 1 year is defined in non-graded schedule .
                            {
                                renewalYrIncoming = year2Record.Percent;
                            }
                        }
                    }
                    else
                    {
                        firstYrIncoming = schedule.FirstYearPercentage;
                        renewalYrIncoming = schedule.RenewalPercentage;
                    }
                    expectedSplit = (policyYear == 1) ? firstYrIncoming : renewalYrIncoming;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetIncomingScheduleDetail ex: " + ex.Message, true, agencyName);
            }
        }

        static int getPolicyAgeFromEffective(DateTime secondDate, DateTime? effective, string agencyName = "")
        {
            int year = 1;
            try
            {
                if (effective != null) //if null, always assumed first year.
                {
                    year = (int)Math.Ceiling((secondDate - Convert.ToDateTime(effective)).TotalDays / 365.25D);
                    year = (year <= 0) ? 1 : year;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("getPolicyAgeFromEffective Exception: " + ex.Message, true, agencyName);
            }
            return year;

        }

        static string CheckValidIncomingModeForOutgoing(bool isOldIncomingPresent, PayorIncomingSchedule inSchedule, Guid settingsID, Guid policyID)
        {
            string result = "";
            if (isOldIncomingPresent)
            {
                PolicyToolIncommingShedule schedule = PolicyToolIncommingShedule.GettingPolicyIncomingSchedule(policyID);
                if (schedule.Mode == Mode.Custom && schedule.CustomType == CustomMode.Graded)
                    result = "Outgoing Schedule cannot be % of Premium, as incoming schedule is of Graded type";
            }
            else
            {
                if (settingsID != Guid.Empty)
                {
                    PolicyToolIncommingShedule schedule = PayorIncomingSchedule.GetBasicScheduleOnSettingsID(settingsID);
                    if (schedule.Mode == Mode.Custom && schedule.CustomType == CustomMode.Graded)
                        result = "Outgoing Schedule cannot be % of Premium, as incoming schedule is of Graded type";

                }
                else if (inSchedule != null && inSchedule.Mode == Mode.Custom && inSchedule.CustomType == CustomMode.Graded)
                {
                    result = "Outgoing Schedule cannot be % of Premium, as incoming schedule is of Graded type";
                }
            }

            return result;
        }
        static string ValidateOutgoingSchedule(List<OutGoingPayment> OutGoingField, Guid LicID, bool? IsCustomDate, bool? IsTieredSchedule, DateTime? Effective, bool isOldIncomingPresent, PayorIncomingSchedule inSchedule, Guid settingsID, Guid policyID, string agencyName = "")
        {
            string result = "";
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("ValidateOutgoingScheduleOnImport OutGoingField - " + OutGoingField.ToStringDump() + ", LicID: " + LicID, true, agencyName);
                ActionLogger.Logger.WriteImportPolicyLog("ValidateOutgoingScheduleOnImport IsCustomDate - " + IsCustomDate + ", IsTieredSchedule: " + IsTieredSchedule + ", effective: " + Effective, true, agencyName);
                //string result = "";

                //Check that outgoing schedule cannot have "% of premium" for graded incoming
                int scheduleTypeID = OutGoingField[0].ScheduleTypeId;
                 ActionLogger.Logger.WriteImportPolicyLog("ValidateOutgoingScheduleOnImport ScheduleTypeId - " + scheduleTypeID, true, agencyName);

                if (scheduleTypeID == 1)
                {
                    result = CheckValidIncomingModeForOutgoing(isOldIncomingPresent, inSchedule, settingsID, policyID);
                    ActionLogger.Logger.WriteImportPolicyLog("ValidateOutgoingScheduleOnImport result - " + result, true, agencyName);
                    if (!string.IsNullOrEmpty(result))
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("CheckValidIncomingModeForOutgoing false, returning - " + result, true, agencyName);
                        return result;
                    }
                }


                Guid houseAcct = PostUtill.GetPolicyHouseOwner(LicID);
                //Check if tier 2 present in outgoing schedule or not
                bool isTier2Present = OutGoingField.Where(x => x.TierNumber == 2).ToList().Count > 0;

                bool IsCustomDateSelected = false;
                bool.TryParse(Convert.ToString(IsCustomDate), out IsCustomDateSelected);

                bool IsTiered = false;
                bool.TryParse(Convert.ToString(IsTieredSchedule), out IsTiered);



                if (Effective == null)
                {
                    result = "Effective date cannot be missing when outgoing schedule present.";
                    ActionLogger.Logger.WriteImportPolicyLog("ValidateOutgoingScheduleOnImport effective date null returning ", true, agencyName);
                    return result;
                }

                if (!IsCustomDateSelected && OutGoingField != null && OutGoingField.Count != 0)
                {
                    //Check that there for tiered schedule, there cannot be more than 2 tiers in schedule
                    if (IsTiered && OutGoingField != null && OutGoingField.Count > 0)
                    {
                        //Validate tier numbers
                        List<int?> tierList = OutGoingField.Select(x => x.TierNumber).Distinct().ToList();
                        result = ValidateTierNumbers(tierList, agencyName);
                        if (!string.IsNullOrEmpty(result))
                            return result;

                        //Validate house account to exist only once in one tier
                        //-check no two house accounts in one schedule of Tier 1
                        int houseCount = OutGoingField.Where(x => (x.PayeeUserCredentialId == houseAcct) && x.TierNumber == 1).Count();
                        if (houseCount >= 2)
                        {
                            result = "House account is added more than once in Tier 1 in the outgoing schedule.";
                            return result;
                        }

                        //-check no two house accounts in one schedule of Tier 2
                        houseCount = OutGoingField.Where(x => (x.PayeeUserCredentialId == houseAcct) && x.TierNumber == 2).Count();
                        if (houseCount >= 2)
                        {
                            result = "House account is added more than once in Tier 2 in the outgoing schedule.";
                            return result;
                        }
                    }

                    //tiered schedule validation - House account should not be added more than once in basic scehdule
                    if (!IsTiered && houseAcct != null && houseAcct != Guid.Empty && OutGoingField.Where(p => p.PayeeUserCredentialId == houseAcct).Count() > 1)
                    {
                        result = "House account is added more than once in the outgoing schedule.";
                        return result;
                    }


                    double dbFirstYear = 0.0;
                    double dbRenewal = 0.0;


                    foreach (var item in OutGoingField)
                    {
                        //Acme added March 13, 2019 - to cover tiered schedule scenario
                        if (!IsTiered ||
                            (IsTiered && isTier2Present && (item.TierNumber == 2)) ||
                            (IsTiered && !isTier2Present && (item.TierNumber == 1))
                            )
                        {
                            dbFirstYear = dbFirstYear + Convert.ToDouble(item.FirstYearPercentage);
                            dbRenewal = dbRenewal + Convert.ToDouble(item.RenewalPercentage);
                        }
                    }

                    dbFirstYear = Convert.ToDouble(dbFirstYear.ToString("n2"));
                    dbRenewal = Convert.ToDouble(dbRenewal.ToString("n2"));

                    if ((OutGoingField.FirstOrDefault().ScheduleTypeId == 2))
                    {
                        //if ((OutGoingField.ToList().Sum(p => p.FirstYearPercentage) != 100) || (OutGoingField.ToList().Sum(p => p.RenewalPercentage) != 100))
                        if ((dbFirstYear > 100) || (dbRenewal > 100))
                        {
                            result = "Incoming FirstYear/Renewal cannot be greater than 100%.";
                            return result;
                        }
                        else if (dbFirstYear < 100 || dbRenewal < 100)
                        {
                            OutGoingField = CompleteOutgoingSchedule(OutGoingField, policyID, houseAcct, IsTiered, isTier2Present, false, 0, 0, 0, agencyName);
                        }

                    }
                    else
                    {
                        double? firstYrIncoming = 0;
                        double? renewalYrIncoming = 0;
                        double? expectedSplit = 0;

                        GetIncomingScheduleDetail(isOldIncomingPresent, inSchedule, settingsID, policyID, Effective, false, null, out firstYrIncoming, out renewalYrIncoming, out expectedSplit, agencyName);

                        if ((dbFirstYear > firstYrIncoming) || (dbRenewal > renewalYrIncoming))
                        {
                            result = "Outgoing FirstYear/Renewal cannot be greater than Incoming FirstYear/Renewal.";
                            return result;
                        }
                        else if (dbFirstYear < firstYrIncoming || dbRenewal < renewalYrIncoming)
                        {
                            OutGoingField = CompleteOutgoingSchedule(OutGoingField, policyID, houseAcct, IsTiered, isTier2Present, false, (double)firstYrIncoming, (double)renewalYrIncoming, 0, agencyName);
                        }
                        //double? expectedfirst = SelectedPolicyToolIncommingShedule.FirstYearPercentage;
                        //double? expectedRenewal = SelectedPolicyToolIncommingShedule.RenewalPercentage;

                        //Check if custom mode, then 
                        /*if (SelectedPolicyToolIncommingShedule.Mode == Mode.Custom && SelectedPolicyToolIncommingShedule.CustomType == CustomMode.NonGraded)
                        {
                            int year = getPolicyAgeFromEffective(DateTime.Now);
                            expectedfirst = SelectedPolicyToolIncommingShedule.NonGradedSchedule.Where(x => x.Year == 1).FirstOrDefault().Percent;

                            int maxYear = SelectedPolicyToolIncommingShedule.NonGradedSchedule.OrderByDescending(x => x.Year).FirstOrDefault().Year;
                            year = (year > maxYear) ? maxYear : year;
                            expectedRenewal = SelectedPolicyToolIncommingShedule.NonGradedSchedule.OrderBy(x => x.Year).Where(x => x.Year == year).FirstOrDefault().Percent;
                            //if (expectedRenewal == null || expectedRenewal == 0)
                            //{
                            //    expectedRenewal = SelectedPolicyToolIncommingShedule.NonGradedSchedule.Where(x => x.Year == maxYear).FirstOrDefault().Percent;
                            //}

                        }
                        if (IsTiered)
                        {
                            if ((OutGoingField.Where(x => (isTier2Present && x.TierNumber == 2) || (!isTier2Present && x.TierNumber == 1)).Sum(p => p.FirstYearPercentage) != firstYrIncoming) ||
                                (OutGoingField.Where(x => (isTier2Present && x.TierNumber == 2) || (!isTier2Present && x.TierNumber == 1)).Sum(p => p.RenewalPercentage) != renewalYrIncoming))
                            {
                                result = "Incoming FirstYear/Renewal not equal to Outgoing FirstYear/Renewal.";
                                return result;
                            }
                        }
                        else
                        {
                            if ((OutGoingField.Sum(p => p.FirstYearPercentage) != firstYrIncoming) ||
                                    (OutGoingField.Sum(p => p.RenewalPercentage) != renewalYrIncoming))
                            {
                                result = "Incoming FirstYear/Renewal not equal to Outgoing FirstYear/Renewal.";
                                return result;
                            }

                        }*/


                    }
                } //When custom date selected 
                else if (IsCustomDateSelected && (OutGoingField != null && OutGoingField.Count != 0))
                {
                    try
                    {
                        var sdates = (from s in OutGoingField
                                      select new { s.CustomStartDate }).Distinct().OrderBy(x => x.CustomStartDate).ToList();



                        ////No effective date, return 
                        //if(Effective == null)
                        //{
                        //    result = "Effective date cannot be missing when outgoing schedule present.";
                        //    return result;
                        //}

                        //No date to be null
                        if (sdates.Where(x => x.CustomStartDate == null).ToList().Count > 0)
                        {
                            result = "Missing start date in outgoing schedule.";
                            return result;
                            //PolicyDetailSaveToolTip = "Missing start date in outgoing schedule.";
                            //Changedcolor = true;
                            //Savecontent = "! Save";
                            //imagePath = null;
                            //return false;
                        }


                        //Tiered Schedule validations
                        if (IsTiered && sdates.Count > 0)
                        {
                            foreach (var i in sdates)
                            {
                                //Check tiers to be proper for each date
                                List<int?> tierList = OutGoingField.Where(x => x.CustomStartDate == i.CustomStartDate).Select(x => x.TierNumber).Distinct().ToList();
                                result = ValidateTierNumbers(tierList, agencyName);

                                if (!string.IsNullOrEmpty(result))
                                {
                                    return result.Replace(".", " starting " + i.CustomStartDate.Value.ToShortDateString() + ".");
                                }

                                //-check no two house accounts in one schedule of Tier 1
                                int houseCount = OutGoingField.Where(x => (x.CustomStartDate == i.CustomStartDate) && x.TierNumber == 1).Count(x => x.PayeeUserCredentialId == houseAcct);
                                if (houseCount >= 2)
                                {
                                    result = "House account is added more than once in Tier 1 in the outgoing schedule of " + i.CustomStartDate.Value.ToShortDateString() + ". Please make sure that house account appears only once per tier in schedule.";
                                    return result;
                                }

                                //-check no two house accounts in one schedule of Tier 2
                                houseCount = OutGoingField.Where(x => (x.CustomStartDate == i.CustomStartDate) && x.TierNumber == 2).Count(x => x.PayeeUserCredentialId == houseAcct);
                                if (houseCount >= 2)
                                {
                                    result = "House account is added more than once in Tier 2 in the outgoing schedule of " + i.CustomStartDate.Value.ToShortDateString() + ". Please make sure that house account appears only once per tier in schedule.";
                                    return result;
                                }
                            }
                        }

                        //-check no two house accounts in one schedule
                        if (!IsTiered && sdates.Count > 0)
                        {
                            foreach (var i in sdates)
                            {
                                int houseCount = OutGoingField.Where(x => (x.CustomStartDate == i.CustomStartDate)).Count(x => x.PayeeUserCredentialId == houseAcct);
                                if (houseCount >= 2)
                                {
                                    result = "House account is added more than once in the outgoing schedule of " + i.CustomStartDate.Value.ToShortDateString() + ". Please make sure that house account appears only once in schedule.";
                                    return result;
                                }
                            }
                        }

                        //Check that schedule must include origin effective date
                        if (Effective != null)
                        {
                            bool isEffectiveDateIncluded = false;
                            foreach (var i in sdates)
                            {
                                if (i.CustomStartDate < Effective) // if any date is less or equal eff date, then eff date included in schedule
                                {
                                    //isEffectiveDateIncluded = true;
                                    //break;
                                    result = "Outgoing schedule cannot start before effective date of the policy. Please check " + Convert.ToDateTime(i.CustomStartDate).ToShortDateString();
                                    return result;
                                }
                                else if (i.CustomStartDate == Effective)
                                {
                                    isEffectiveDateIncluded = true;
                                }
                            }

                            if (!isEffectiveDateIncluded)
                            {
                                result = "Outgoing schedule must start from effective date of the policy.";
                                return result;
                            }
                        }


                        //Fill end dates 
                        for (int i = 0; i < sdates.Count - 1; i++)
                        {
                            DateTime sDate = Convert.ToDateTime(sdates[i].CustomStartDate);
                            DateTime eDate = Convert.ToDateTime(sdates[i + 1].CustomStartDate).AddDays(-1);
                            var allEntries = OutGoingField.Where(c => c.CustomStartDate == sDate).ToList();
                            if (allEntries != null && allEntries.Count > 0)
                            {
                                allEntries.ForEach(cc => cc.CustomEndDate = eDate);
                            }
                        }

                        //100% when % of commission
                        //   if (OutPercentOfCommission)
                        if ((OutGoingField.FirstOrDefault().ScheduleTypeId == 2))
                        {
                            foreach (var i in sdates)
                            {
                                List<OutGoingPayment> lstOnDate = OutGoingField.Where(x => x.CustomStartDate == i.CustomStartDate).ToList();
                                if (lstOnDate.Count > 0)
                                {
                                    double split = lstOnDate.Sum<OutGoingPayment>(p => p.SplitPercent ?? 0);
                                    isTier2Present = lstOnDate.Where(x => x.TierNumber == 2).ToList().Count > 0;
                                    if (IsTiered)
                                    {
                                        split = lstOnDate.Where(x => ((isTier2Present && x.TierNumber == 2) || (!isTier2Present && x.TierNumber == 1))).Sum<OutGoingPayment>(p => p.SplitPercent ?? 0);
                                    }
                                    if (split > 100)
                                    {
                                        result = "Split percentage cannot be greater than 100 for the schedule starting from " + i.CustomStartDate.Value.ToShortDateString();
                                        return result;
                                    }
                                    else
                                    {
                                        CompleteOutgoingSchedule(OutGoingField, policyID, houseAcct, IsTiered, isTier2Present, true, 0, 0, 0, agencyName, i.CustomStartDate, lstOnDate[0].CustomEndDate); //assuming end date will be saem against all start dates
                                    }
                                }
                                else
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Custom Schedule found with no payee data for date: " + i, true, agencyName);
                                }
                            }
                        }
                        else // if (OutPercentOfPremium) //equal incoming when % of premium
                        {
                            foreach (var i in sdates)
                            {
                                List<OutGoingPayment> lstOnDate = OutGoingField.Where(x => x.CustomStartDate == i.CustomStartDate).ToList();
                                if (lstOnDate.Count > 0)
                                {
                                    double split = lstOnDate.Sum<OutGoingPayment>(p => p.SplitPercent ?? 0);
                                    isTier2Present = lstOnDate.Where(x => x.TierNumber == 2).ToList().Count > 0;
                                    if (IsTiered)
                                    {
                                        split = lstOnDate.Where(x => ((isTier2Present && x.TierNumber == 2) || (!isTier2Present && x.TierNumber == 1))).Sum<OutGoingPayment>(p => p.SplitPercent ?? 0);
                                    }
                                    double? expectedfirst = 0;
                                    double? expectedRenewal = 0;
                                    double? expectedSplit = 0;

                                    GetIncomingScheduleDetail(isOldIncomingPresent, inSchedule, settingsID, policyID, Effective, true, i.CustomStartDate, out expectedfirst, out expectedRenewal, out expectedSplit, agencyName);


                                    if (split > expectedSplit)
                                    {
                                        result = "Split percentage cannot be greater than " + expectedSplit + " for the schedule starting from " + i.CustomStartDate.Value.ToShortDateString();
                                        return result;
                                    }
                                    else
                                    {
                                        CompleteOutgoingSchedule(OutGoingField, policyID, houseAcct, IsTiered, isTier2Present, true, (double)expectedfirst, (double)expectedRenewal, (double)expectedSplit, agencyName, i.CustomStartDate, lstOnDate[0].CustomEndDate);//assuming end date will be saem against all start dates
                                    }
                                }
                                else
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog("Custom Schedule found with no payee data for date: " + i, true, agencyName);
                                }


                                //logic to include custom incoming schedule
                                //Check if custom mode, then 
                                /* if (SelectedPolicyToolIncommingShedule.Mode == Mode.Custom && SelectedPolicyToolIncommingShedule.CustomType == CustomMode.NonGraded)
                                 {

                                     expectedfirst = SelectedPolicyToolIncommingShedule.NonGradedSchedule.Where(x => x.Year == 1).FirstOrDefault().Percent;
                                     int year = getPolicyAgeFromEffective(i.CustomStartDate.Value);

                                     int maxYear = SelectedPolicyToolIncommingShedule.NonGradedSchedule.OrderByDescending(x => x.Year).FirstOrDefault().Year;
                                     year = (year > maxYear) ? maxYear : year;
                                     expectedRenewal = SelectedPolicyToolIncommingShedule.NonGradedSchedule.OrderBy(x => x.Year).Where(x => x.Year == year).FirstOrDefault().Percent;

                                 }
                                 */
                                //to confirm from kevin
                                //bool isfirstyear = (i.customstartdate == null) ? true : isusefirstyear(i.customstartdate.value, effective);
                                //if ((isfirstyear && split > expectedfirst) ||
                                //     (!isfirstyear && split > expectedrenewal))
                                //{
                                //    result = "outgoing split percentage cannot be greater than incoming firstyear/renewal for the schedule starting from " + i.customstartdate.value.toshortdatestring();
                                //    return result;
                                //}
                                //else if ((isfirstyear && split < expectedfirst) ||
                                //     (!isfirstyear && split < expectedrenewal))
                                //{
                                //    completeoutgoingschedule(outgoingfield, policyid, houseacct, istiered, istier2present, true,expectedfirst,expectedrenewal,expectedsplit, i.customstartdate.value, i.customenddat); //assuming end date will be saem against all start dates
                                //}
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("exception validating save policy: " + ex.Message, true, agencyName);
                    }
                }

                //return result;
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("exception validating save policy main: " + ex.Message, true, agencyName);
                if (ex.InnerException != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("exception validating save policy Inner exception: " + ex.InnerException, true, agencyName);
                }
            }
            return result;
        }

        static bool IsUseFirstYear(DateTime date, DateTime? OriginalEffectiveDate)
        {
            //PolicyDetailsData data = PostUtill.GetPolicyEffectiveDate(policyID);
            if (OriginalEffectiveDate != null)
            {
                //Changed by Acme on March 21 - 2014. as per Eric's feedback to correctthe logic
                //if ((_invoicedate >= EffDate.Value) && (_invoicedate <= EffDate.Value.AddYears(1)))
                if ((date >= OriginalEffectiveDate.Value) && (date < OriginalEffectiveDate.Value.AddYears(1)))
                {
                    return true;
                }
                else //if (date >= EffDate.Value.AddYears(1))
                {
                    return false;
                    //Flag = FirstYrRenewalYr.Renewal;
                }
            }
            else
                return true;
        }

        static List<OutGoingPayment> CompleteOutgoingSchedule(List<OutGoingPayment> OutGoingPayments, Guid PolicyID, Guid houseAcct, bool IsTiered, bool isTier2Present, bool isCustom, double incomingFrstYr, double incomingRenewalYr, double splitAmount, string agencyName = "", DateTime? customDate = null, DateTime? customEndDate = null)
        {
            ActionLogger.Logger.WriteImportPolicyLog(" CompleteOutgoingSchedule request:  " + OutGoingPayments.ToStringDump(), true, agencyName);
            ActionLogger.Logger.WriteImportPolicyLog(" CompleteOutgoingSchedule request: PolicyID - " + PolicyID + ", IsTiered: " + IsTiered + ", istier2present: " + isTier2Present, true, agencyName);

            List<OutGoingPayment> OutGoingField = OutGoingPayments;
            try
            {
                if (OutGoingField != null && OutGoingField.Count > 0)
                {
                    double dbFirstYear = 0.0;
                    double dbRenewal = 0.0;
                    double split = 0.0;
                    if (!isCustom)
                    {
                        foreach (var item in OutGoingField)
                        {
                            //Acme added March 13, 2019 - to cover tiered schedule scenario
                            if (!IsTiered ||
                                (IsTiered && isTier2Present && (item.TierNumber == 2)) ||
                                (IsTiered && !isTier2Present && (item.TierNumber == 1))
                                )
                            {
                                dbFirstYear = dbFirstYear + Convert.ToDouble(item.FirstYearPercentage);
                                dbRenewal = dbRenewal + Convert.ToDouble(item.RenewalPercentage);
                            }
                        }
                    }
                    else
                    {
                        split = OutGoingField.Where(x => x.CustomStartDate == customDate).Sum<OutGoingPayment>(p => p.SplitPercent ?? 0);
                        if (IsTiered)
                        {
                            split = OutGoingField.Where(x => (x.CustomStartDate == customDate) &&
                            ((isTier2Present && x.TierNumber == 2) || (!isTier2Present && x.TierNumber == 1))).Sum<OutGoingPayment>(p => p.SplitPercent ?? 0);
                        }
                    }

                    int scheduleType = OutGoingField.FirstOrDefault().ScheduleTypeId;

                    //  if ((OutGoingField.FirstOrDefault().ScheduleTypeId == 2))
                    // {
                    if ((scheduleType == 2 && ((!isCustom && (dbFirstYear < 100) || (dbRenewal < 100)) || (isCustom && split < 100))) ||
                         (scheduleType == 1 && ((!isCustom && (dbFirstYear < incomingFrstYr) || (dbRenewal < incomingRenewalYr)) || (isCustom && split < 100))))
                    {
                        OutGoingPayment OutgoingRecord = new OutGoingPayment();
                        OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                        OutgoingRecord.PolicyId = PolicyID;
                        OutgoingRecord.PayeeUserCredentialId = houseAcct;
                        OutgoingRecord.ScheduleTypeId = scheduleType; // 2;
                        OutgoingRecord.TierNumber = (IsTiered) ? ((isTier2Present) ? 2 : 1) : (int?)null;

                        if (!isCustom)
                        {
                            OutgoingRecord.FirstYearPercentage = (scheduleType == 2) ? Convert.ToDouble(100 - dbFirstYear) : Convert.ToDouble(incomingFrstYr - dbFirstYear);
                            OutgoingRecord.RenewalPercentage = (scheduleType == 2) ? Convert.ToDouble(100 - dbRenewal) : Convert.ToDouble(incomingRenewalYr - dbRenewal);
                        }
                        else
                        {
                            OutgoingRecord.SplitPercent = (scheduleType == 2) ? Convert.ToDouble(100 - split) : Convert.ToDouble(splitAmount - split);
                            OutgoingRecord.CustomStartDate = customDate;
                            OutgoingRecord.CustomEndDate = customEndDate;
                        }
                        OutGoingField.Add(OutgoingRecord);

                    }
                    // }
                }
                else
                {
                    //No previous payment, add new with house account 
                    if (OutGoingField == null)
                    {
                        OutGoingField = new List<OutGoingPayment>();
                    }
                    OutGoingPayment OutgoingRecord = new OutGoingPayment();
                    OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                    OutgoingRecord.PolicyId = PolicyID;
                    OutgoingRecord.FirstYearPercentage = 100;
                    OutgoingRecord.RenewalPercentage = 100;
                    OutgoingRecord.PayeeUserCredentialId = houseAcct;
                    OutgoingRecord.ScheduleTypeId = 2;
                    OutgoingRecord.TierNumber = (IsTiered) ? 1 : (int?)null; //Assign tier number 1 to blank list, when tiered schedule selected.  
                    OutGoingField.Add(OutgoingRecord);
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception CompleteOutgoingSchedule :  " + ex.Message, true, agencyName);
            }
            return OutGoingField;
        }

        static List<OutGoingPayment> CompleteOutgoingSchedule(List<OutGoingPayment> OutGoingPayments, Guid PolicyID, Guid LicID, string agencyName = "", bool? IsCustom = null, DateTime? Effective = null)
        {
            List<OutGoingPayment> OutGoingField = OutGoingPayments;
            Guid houseAcct = PostUtill.GetPolicyHouseOwner((Guid)LicID, agencyName);
            try
            {
                if (OutGoingField != null && OutGoingField.Count > 0)
                {
                    //Check total payment fy and ry 
                    double fyTotal = OutGoingField.Where(x => x.FirstYearPercentage != null).Sum(x => Convert.ToDouble(x.FirstYearPercentage));
                    double ryTotal = OutGoingField.Where(x => x.RenewalPercentage != null).Sum(x => Convert.ToDouble(x.RenewalPercentage));
                    if (fyTotal < 100 || ryTotal < 100)
                    {
                        OutGoingPayment OutgoingRecord = new OutGoingPayment();
                        OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                        OutgoingRecord.PolicyId = PolicyID;
                        //    OutgoingRecord.IsPrimaryAgent = false;
                        OutgoingRecord.FirstYearPercentage = Convert.ToDouble(100 - fyTotal);
                        OutgoingRecord.RenewalPercentage = Convert.ToDouble(100 - ryTotal);
                        OutgoingRecord.PayeeUserCredentialId = houseAcct;
                        OutgoingRecord.ScheduleTypeId = 2;
                        OutGoingField.Add(OutgoingRecord);
                    }
                }
                else
                {
                    //No previous payment, add new with house account 
                    if (OutGoingField == null)
                    {
                        OutGoingField = new List<OutGoingPayment>();
                    }
                    OutGoingPayment OutgoingRecord = new OutGoingPayment();
                    OutgoingRecord.OutgoingScheduleId = Guid.NewGuid();
                    OutgoingRecord.PolicyId = PolicyID;
                    // OutgoingRecord.IsPrimaryAgent = true;
                    if (IsCustom == true)
                    {
                        OutgoingRecord.SplitPercent = 100;
                        OutgoingRecord.CustomStartDate = Effective;
                    }
                    else
                    {
                        OutgoingRecord.FirstYearPercentage = 100;// string.IsNullOrEmpty(fy) ? 0 : Convert.ToDouble(fy);
                        OutgoingRecord.RenewalPercentage = 100;// string.IsNullOrEmpty(ry) ? 0 : Convert.ToDouble(ry);
                    }
                    OutgoingRecord.PayeeUserCredentialId = houseAcct;
                    OutgoingRecord.ScheduleTypeId = 2;
                    OutGoingField.Add(OutgoingRecord);
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception CompleteOutgoingSchedule :  " + ex.Message, true, agencyName);
            }
            return OutGoingField;
        }

        static int PolicyModeID(string strModeType)
        {
            int intStatus = 0;

            switch (strModeType.ToLower())
            {
                case "monthly":
                    intStatus = 0;
                    break;

                case "quarterly":
                    intStatus = 1;
                    break;

                case "semi-annually":
                    intStatus = 2;
                    break;

                case "annually":
                    intStatus = 3;
                    break;

                case "one time":
                    intStatus = 4;
                    break;

                case "random":
                    intStatus = 5;
                    break;

                default:
                    intStatus = 0;
                    break;
            }
            return intStatus;
        }


        static int PolicTermisionID(string strTermReason)
        {
            int intStatus = 0;

            switch (strTermReason.ToLower())
            {
                case "replaced by new policy":
                    intStatus = 0;
                    break;

                case "lost to competitor":
                    intStatus = 1;
                    break;

                case "voluntary":
                    intStatus = 2;
                    break;

                case "out of business":
                    intStatus = 3;
                    break;

                case "non-payment":
                    intStatus = 4;
                    break;

                case "per carrier":
                    intStatus = 5;
                    break;

                default:
                    intStatus = 0;
                    break;
            }
            return intStatus;
        }



        public static string CheckExcelFormat(DataTable dt)
        {
            string result = "";
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("Starting CheckExcelFormat: ", true);
                // DataTable tbRef = GetDefaultTemplateTable();

                /*for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string col = dt.Columns[j].ColumnName;
                    if (!tbRef.Columns.Contains(col))
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("CheckExcelFormat mismatch in  tbRef.Columns.Contain at: " + col, true);
                        result = "Your excel file contains additional column '" + col + "'" ;
                        return result;
                    }
                }*/

                /* Not comparing full template right now, only mandatiry fields as per discussion
                 * for (int j = 0; j < tbRef.Columns.Count; j++)
                 {
                     string col = tbRef.Columns[j].ColumnName;
                     if (!dt.Columns.Contains(col))
                     {
                         ActionLogger.Logger.WriteImportPolicyLog("CheckExcelFormat mismatch in dt.Columns.Contain at: " + col, true);
                         result = "Your excel file doesn't have required column '" + col + "'";
                         return result;
                     }
                 }*/
                string missCol = "";

                if (!dt.Columns.Contains("Vision Plan ID") && !dt.Columns.Contains("Policy Plan ID"))
                {
                    ActionLogger.Logger.WriteImportPolicyLog("CheckExcelFormat mismatch in  planid", true);
                    missCol += "\nVision Plan ID";
                    //result = "Your excel file contains additional column '" + col + "'";
                    //return result;
                }

                if (!dt.Columns.Contains("Account Name"))
                {
                    ActionLogger.Logger.WriteImportPolicyLog("CheckExcelFormat mismatch in  planid", true);
                    missCol += "\nAccount Name";
                }

                if (!dt.Columns.Contains("Original Plan Start Date"))
                {
                    ActionLogger.Logger.WriteImportPolicyLog("CheckExcelFormat mismatch in  planid", true);
                    missCol += "\nOriginal Plan Start Date";
                }
                if (!string.IsNullOrEmpty(missCol))
                {
                    result = "Your excel file is missing following columns:\n" + missCol;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception CheckExcelFormat: " + ex.Message, true);
                return result;
            }
            ActionLogger.Logger.WriteImportPolicyLog("CheckExcelFormat success ", true);
            return result;
        }

        static string ValidateIncomingSchedule(List<Graded> GradedList, List<NonGraded> NonGradedList, bool IsGraded, string SelectedIncomingMode, int ScheduleTypeId, string agencyName = "")
        {
            if (SelectedIncomingMode == "Custom")
            {
                var scheduleName = ScheduleTypeId == 1 ? "% of Premium" : "Per Head";
                if (IsGraded)
                {
                    if (GradedList == null || (GradedList != null && GradedList.Count == 0))
                    {
                        return "Please enter a range of values in incoming graded schedule.";
                    }
                    else
                    {
                        GradedList = new List<Graded>(GradedList.OrderBy(x => x.From));

                        for (int i = 0; i < GradedList.Count; i++)
                        {

                            if (GradedList[i].From == 0 || GradedList[i].To == 0 || GradedList[i].Percent == 0)
                                return "From, To or" + " " + scheduleName + " " + "field cannot be blank or '0'.";

                            if (GradedList[i].To <= GradedList[i].From)
                                return "To value must be greater than From value."; // + GradedList[i].From;

                            if (i > 0)
                            {
                                if (GradedList[i].From != GradedList[i - 1].To + 1)
                                {
                                    return "'From' value in the range must be the next number of 'To' value in previous range.";
                                }
                            }

                        }
                        if (GradedList.OrderBy(x => x.From).FirstOrDefault().From != 1)
                        {
                            return "Range is missing with 'From' value starting from '1'.";
                        }
                    }
                }
                else
                {
                    if (NonGradedList == null || (NonGradedList != null && NonGradedList.Count == 0))
                    {
                        return "Please enter values in incoming non-graded schedule.";
                    }
                    else
                    {
                        NonGradedList = new List<NonGraded>(NonGradedList.OrderBy(x => x.Year));

                        for (int i = 0; i < NonGradedList.Count; i++)
                        {
                            if (NonGradedList[i].Year == 0 || NonGradedList[i].Percent == 0)
                                return "Year and " + scheduleName + " " + "cannot be blank or 0 in schedule";

                            if (i > 0)
                            {
                                if (NonGradedList[i].Year != NonGradedList[i - 1].Year + 1)
                                {
                                    return "Year numbers must be consecutive without missing any value in between.";
                                }
                            }

                        }
                        if (NonGradedList.FirstOrDefault().Year != 1)
                        {
                            return "Year 1 schedule is missing.";
                        }
                    }
                }
            }
            return "";
        }
        #endregion


        #region Save and Delete Methods

        public static void ModifyPolicyLastUpdated(Guid PolicyID, Guid? CurrentUserID)
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("ModifyPolicyLastUpdated request, POlicyID: " + PolicyID + ", CurrentUser: " + CurrentUserID, true);
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _policy = (from p in DataModel.Policies where (p.PolicyId == PolicyID) select p).FirstOrDefault();
                    if (_policy != null)
                    {
                        _policy.LastModifiedBy = CurrentUserID;
                        _policy.LastModifiedOn = DateTime.Now;
                        DataModel.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("ModifyPolicyLastUpdated exception, POlicyID: " + PolicyID + ", ex: " + ex.Message, true);
            }
        }



        public static void AddUpdatePolicy(PolicyDetailsData _PolicyRecord)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.Policies where (p.PolicyId == _PolicyRecord.PolicyId) select p).FirstOrDefault();
                if (_policy == null)
                {
                    _policy = new DLinq.Policy
                    {
                        PolicyId = _PolicyRecord.PolicyId,
                        PolicyNumber = _PolicyRecord.PolicyNumber,
                        PolicyType = _PolicyRecord.PolicyType,
                        Insured = _PolicyRecord.Insured,
                        OriginalEffectiveDate = _PolicyRecord.OriginalEffectiveDate,
                        TrackFromDate = _PolicyRecord.TrackFromDate,
                        MonthlyPremium = _PolicyRecord.ModeAvgPremium,
                        SubmittedThrough = _PolicyRecord.SubmittedThrough,
                        Enrolled = _PolicyRecord.Enrolled,
                        Eligible = _PolicyRecord.Eligible,
                        PolicyTerminationDate = _PolicyRecord.PolicyTerminationDate,
                        IsTrackMissingMonth = _PolicyRecord.IsTrackMissingMonth,
                        IsTrackIncomingPercentage = _PolicyRecord.IsTrackIncomingPercentage,
                        IsTrackPayment = _PolicyRecord.IsTrackPayment,
                        IsDeleted = false,
                        ReplacedBy = _PolicyRecord.ReplacedBy,
                        DuplicateFrom = _PolicyRecord.DuplicateFrom,
                        CreatedOn = DateTime.Now,
                        IsIncomingBasicSchedule = _PolicyRecord.IsIncomingBasicSchedule,
                        IsOutGoingBasicSchedule = _PolicyRecord.IsOutGoingBasicSchedule,
                        SplitPercentage = _PolicyRecord.SplitPercentage,
                        Advance = _PolicyRecord.Advance,
                        ProductType = _PolicyRecord.ProductType,
                        UserCredentialId = _PolicyRecord.UserCredentialId,
                        AccoutExec = _PolicyRecord.AccoutExec,
                        //new fields
                        PrimaryAgent = _PolicyRecord.PrimaryAgent,
                        IsCustomBasicSchedule = _PolicyRecord.IsCustomBasicSchedule,
                        CustomScheduleDateType = _PolicyRecord.CustomDateType,
                        IsTieredSchedule = _PolicyRecord.IsTieredSchedule,
                        //Last modified information
                        LastModifiedOn = DateTime.Now,
                        LastModifiedBy = _PolicyRecord.LastModifiedBy,
                        IslastModifiedFromWeb = false,
                        IsCreatedFromWeb = false
                    };
                    _policy.MasterPolicyStatuReference.Value = (from m in DataModel.MasterPolicyStatus where m.PolicyStatusId == _PolicyRecord.PolicyStatusId select m).FirstOrDefault();
                    _policy.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == _PolicyRecord.ClientId select s).FirstOrDefault();
                    _policy.LicenseeReference.Value = (from l in DataModel.Licensees where l.LicenseeId == _PolicyRecord.PolicyLicenseeId select l).FirstOrDefault();

                    //_policy.MasterPolicyModeReference.Value = (from m in DataModel.MasterPolicyModes where m.PolicyModeId == _PolicyRecord.PolicyModeId select m).FirstOrDefault();
                    _policy.PolicyModeId = _PolicyRecord.PolicyModeId;

                    _policy.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == _PolicyRecord.CoverageId select s).FirstOrDefault();
                    _policy.MasterPolicyTerminationReasonReference.Value = (from s in DataModel.MasterPolicyTerminationReasons where s.PTReasonId == _PolicyRecord.TerminationReasonId select s).FirstOrDefault();
                    _policy.MasterIncomingPaymentTypeReference.Value = (from m in DataModel.MasterIncomingPaymentTypes where m.IncomingPaymentTypeId == _PolicyRecord.IncomingPaymentTypeId select m).FirstOrDefault();
                    _policy.PayorReference.Value = (from m in DataModel.Payors where m.PayorId == _PolicyRecord.PayorId select m).FirstOrDefault();
                    _policy.UserCredentialReference.Value = (from s in DataModel.UserCredentials where s.UserCredentialId == _PolicyRecord.CreatedBy select s).FirstOrDefault();
                    _policy.CarrierReference.Value = (from m in DataModel.Carriers where m.CarrierId == _PolicyRecord.CarrierID select m).FirstOrDefault();

                    if (_policy.PolicyStatusId == ((int)_PolicyStatus.Active))
                        _policy.ActivatedOn = _policy.CreatedOn;
                    else
                        _policy.ActivatedOn = null;

                    DataModel.AddToPolicies(_policy);

                }
                else
                {
                    _policy.PolicyId = _PolicyRecord.PolicyId;
                    _policy.PolicyNumber = _PolicyRecord.PolicyNumber;
                    _policy.PolicyType = _PolicyRecord.PolicyType;
                    _policy.Insured = _PolicyRecord.Insured;
                    _policy.OriginalEffectiveDate = _PolicyRecord.OriginalEffectiveDate;
                    _policy.TrackFromDate = _PolicyRecord.TrackFromDate;
                    _policy.MonthlyPremium = _PolicyRecord.ModeAvgPremium;
                    _policy.SubmittedThrough = _PolicyRecord.SubmittedThrough;
                    _policy.Enrolled = _PolicyRecord.Enrolled;
                    _policy.Eligible = _PolicyRecord.Eligible;
                    _policy.PolicyTerminationDate = _PolicyRecord.PolicyTerminationDate;
                    _policy.IsTrackMissingMonth = _PolicyRecord.IsTrackMissingMonth;
                    _policy.IsTrackIncomingPercentage = _PolicyRecord.IsTrackIncomingPercentage;
                    _policy.IsTrackPayment = _PolicyRecord.IsTrackPayment;
                    _policy.IsDeleted = _PolicyRecord.IsDeleted;
                    _policy.ReplacedBy = _PolicyRecord.ReplacedBy;
                    _policy.DuplicateFrom = _PolicyRecord.DuplicateFrom;
                    _policy.IsIncomingBasicSchedule = _PolicyRecord.IsIncomingBasicSchedule;
                    _policy.IsOutGoingBasicSchedule = _PolicyRecord.IsOutGoingBasicSchedule;
                    _policy.SplitPercentage = _PolicyRecord.SplitPercentage;
                    //recently added
                    _policy.Advance = _PolicyRecord.Advance;
                    _policy.ProductType = _PolicyRecord.ProductType;

                    //added 15012016
                    _policy.UserCredentialId = _PolicyRecord.UserCredentialId;
                    _policy.AccoutExec = _PolicyRecord.AccoutExec;


                    _policy.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == _PolicyRecord.ClientId select s).FirstOrDefault();
                    _policy.LicenseeReference.Value = (from l in DataModel.Licensees where l.LicenseeId == _PolicyRecord.PolicyLicenseeId select l).FirstOrDefault();

                    if ((_policy.PolicyStatusId == ((int)_PolicyStatus.Pending)) && (_PolicyRecord.PolicyStatusId == ((int)_PolicyStatus.Active)))
                    {
                        if (_policy.ActivatedOn == null)
                        {
                            _policy.ActivatedOn = DateTime.Now;
                        }
                    }

                    _policy.MasterPolicyStatuReference.Value = (from m in DataModel.MasterPolicyStatus where m.PolicyStatusId == _PolicyRecord.PolicyStatusId select m).FirstOrDefault();

                    if (_PolicyRecord.PolicyModeId != null)
                        _policy.PolicyModeId = _PolicyRecord.PolicyModeId;
                    else
                        _policy.PolicyModeId = new System.Nullable<int>();

                    if (_PolicyRecord.CoverageId == Guid.Empty)
                    {
                        _policy.CoverageId = null;
                    }
                    else
                    {
                        _policy.CoverageId = _PolicyRecord.CoverageId;
                    }

                    //Add /Update termination date
                    if ((_PolicyRecord.PolicyStatusId == ((int)_PolicyStatus.Pending)) || (_PolicyRecord.PolicyStatusId == ((int)_PolicyStatus.Active)))
                    {
                        _policy.TerminationReasonId = null;
                    }
                    else
                    {
                        if (_PolicyRecord.TerminationReasonId != null)
                        {
                            _policy.MasterPolicyTerminationReasonReference.Value = (from s in DataModel.MasterPolicyTerminationReasons where s.PTReasonId == _PolicyRecord.TerminationReasonId select s).FirstOrDefault();
                        }
                        else
                        {
                            _policy.TerminationReasonId = null;
                        }
                    }
                    // _policy.MasterPolicyTerminationReasonReference.Value = (from s in DataModel.MasterPolicyTerminationReasons where s.PTReasonId == _PolicyRecord.TerminationReasonId select s).FirstOrDefault();
                    _policy.MasterIncomingPaymentTypeReference.Value = (from m in DataModel.MasterIncomingPaymentTypes where m.IncomingPaymentTypeId == _PolicyRecord.IncomingPaymentTypeId select m).FirstOrDefault();

                    if (_PolicyRecord.PayorId == Guid.Empty)
                    {
                        _policy.PayorId = null;
                    }
                    else
                    {
                        _policy.PayorId = _PolicyRecord.PayorId;
                    }
                    // _policy.PayorReference.Value = (from m in DataModel.Payors where m.PayorId == _PolicyRecord.PayorId select m).FirstOrDefault();

                    _policy.UserCredentialReference.Value = (from s in DataModel.UserCredentials where s.UserCredentialId == _PolicyRecord.CreatedBy select s).FirstOrDefault();
                    if (_PolicyRecord.CarrierID == Guid.Empty)
                    {
                        _policy.CarrierId = null;
                    }
                    else
                    {
                        _policy.CarrierId = _PolicyRecord.CarrierID;
                    }
                    //New fields
                    _policy.PrimaryAgent = _PolicyRecord.PrimaryAgent;
                    _policy.IsCustomBasicSchedule = _PolicyRecord.IsCustomBasicSchedule;
                    _policy.CustomScheduleDateType = _PolicyRecord.CustomDateType;
                    _policy.IsTieredSchedule = _PolicyRecord.IsTieredSchedule;
                    // _policy.CarrierReference.Value = (from m in DataModel.Carriers where m.CarrierId == _PolicyRecord.CarrierID select m).FirstOrDefault();

                    //Last modified information
                    _policy.LastModifiedOn = DateTime.Now;
                    _policy.LastModifiedBy = _PolicyRecord.LastModifiedBy;
                    _policy.IslastModifiedFromWeb = false;


                }
                //DataModel.Policies.Where(p => p.PolicyId == _PolicyRecord.PolicyId).FirstOrDefault().CoverageId = null;
                DataModel.SaveChanges();
            }
        }

        public static void UpdatePendingPolicy(DEU _DEU)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _policy = (from p in DataModel.Policies where (p.PolicyId == _DEU.PolicyId) select p).FirstOrDefault();
                    if (_policy != null)
                    {
                        // in case of pending policy
                        //Update the following details
                        if (_policy.PolicyStatusId == 2)
                        {
                            if (_DEU.SplitPer != null)
                                _policy.SplitPercentage = _DEU.SplitPer;

                            if (_DEU.PolicyMode != null)
                                _policy.PolicyModeId = _DEU.PolicyMode;

                            if (_DEU.OriginalEffectiveDate != null)
                                _policy.OriginalEffectiveDate = _DEU.OriginalEffectiveDate;

                            //if (_DEU.TrackFromDate != null)
                            //    _policy.TrackFromDate = _DEU.TrackFromDate;

                            if (_DEU.Eligible != null)
                                _policy.Eligible = _DEU.Eligible;

                            if (_DEU.Enrolled != null)
                                _policy.Enrolled = _DEU.Enrolled;

                            if (_DEU.CompTypeID != null)
                                _policy.IncomingPaymentTypeId = _DEU.CompTypeID;


                            DataModel.SaveChanges();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("UpdatePendingPolicy:Exception occurs while processing" + ex.Message, true);
                throw ex;
            }

        }

        public static void UpdatePolicyClient(Guid policyId, Guid clientID)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _policy = (from p in DataModel.Policies where (p.PolicyId == policyId) select p).FirstOrDefault();
                    if (_policy != null)
                    {
                        _policy.PolicyClientId = clientID;
                        DataModel.SaveChanges();


                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("UpdatePolicyClient:Exception occurs while processing:" + ex.Message, true);
                throw ex;
            }
        }

        public static void UpdatePolicyClientLernedFields(Guid policyId, Guid clientID)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _policy = (from p in DataModel.PolicyLearnedFields where (p.PolicyId == policyId) select p).FirstOrDefault();
                    if (_policy != null)
                    {
                        _policy.ClientID = clientID;
                        DataModel.SaveChanges();


                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("UpdatePolicyClientLernedFields:Exception occurs while processing-" + ex.Message, true);
                throw ex;
            }
        }

        public static void UpdateMode(Guid _PolicyID, int PolicyMode)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.Policies where (p.PolicyId == _PolicyID) select p).FirstOrDefault();
                if (_policy != null)
                {
                    // in case of pending policy
                    if (_policy.PolicyStatusId == 2)
                    {
                        _policy.PolicyModeId = PolicyMode;
                        DataModel.SaveChanges();
                    }

                }
            }
        }

        public static PolicySavedStatus SavePolicyData(PolicyDetailsData OriginalPolicy, PolicyDetailsData ReplacedPolicy)
        {


            PolicySavedStatus status = new PolicySavedStatus();

            TransactionOptions options = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(15)
            };

            if (OriginalPolicy != null || ReplacedPolicy != null)
            {

                try
                {
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, options))
                    {
                        if (ReplacedPolicy != null)
                        {
                            Policy.AddUpdatePolicy(ReplacedPolicy);
                            Policy.AddUpdatePolicyHistory(ReplacedPolicy.PolicyId);
                            PolicyLearnedField.AddUpdateHistoryLearned(ReplacedPolicy.PolicyId);
                        }
                        if (OriginalPolicy != null)
                        {
                            Policy.AddUpdatePolicy(OriginalPolicy);
                            PolicyToLearnPost.AddUpdatPolicyToLearn(OriginalPolicy.PolicyId);
                            Policy.AddUpdatePolicyHistory(OriginalPolicy.PolicyId);
                            PolicyLearnedField.AddUpdateHistoryLearned(OriginalPolicy.PolicyId);
                        }
                        transaction.Complete();
                    }
                }
                catch
                {
                    status.IsError = true;
                    status.ErrorMessage = "Policy " + OriginalPolicy.PolicyNumber + " is not saved succesfully.";
                }
            }

            return status;
        }


        public static PolicySavedStatus SavePolicy(PolicyDetailsData OriginalPolicy, PolicyDetailsData ReplacedPolicy, string strRenewal, string strCoverageNickName, Guid? CurrentUserID)
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("Save POlicy request: " + OriginalPolicy.ToStringDump() + ", ReplacedPolicy: " + ReplacedPolicy.ToStringDump(), true);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("error logging save policy request: " + ex.Message, true);
            }

            PolicySavedStatus status = new PolicySavedStatus();
            SetPolicyTerminationDate(ref OriginalPolicy, ref ReplacedPolicy);

            TransactionOptions options = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(60)
            };

            if (OriginalPolicy != null || ReplacedPolicy != null)
            {
                try
                {
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, options))
                    {
                        if (ReplacedPolicy != null)
                        {
                            Policy.AddUpdatePolicy(ReplacedPolicy);
                            Policy.AddUpdatePolicyHistory(ReplacedPolicy.PolicyId);
                            PolicyLearnedField.AddUpdateHistoryLearned(ReplacedPolicy.PolicyId);
                        }
                        if (OriginalPolicy != null)
                        {
                            //ActionLogger.Logger.WriteImportPolicyLog(" " + " Call AddUpdatePolicy", true);
                            Policy.AddUpdatePolicy(OriginalPolicy);
                            //ActionLogger.Logger.WriteImportPolicyLog(" " + "Call AddPolicyToLearn", true);                           
                            PolicyToLearnPost.AddPolicyToLearn(OriginalPolicy.PolicyId, strRenewal, strCoverageNickName, OriginalPolicy.ProductType, CurrentUserID);
                            //ActionLogger.Logger.WriteImportPolicyLog(" " + "Call AddUpdatePolicyHistory", true);

                            //Thread thPolicyHistory = new Thread(() =>
                            //{
                            //    Policy.AddUpdatePolicyHistory(OriginalPolicy.PolicyId);
                            //});
                            //thPolicyHistory.IsBackground = true;
                            //thPolicyHistory.Start();

                            //Thread thPolicyLearnedHistory = new Thread(() =>
                            //{
                            //    PolicyLearnedField.AddUpdateHistoryLearned(OriginalPolicy.PolicyId);
                            //});
                            //thPolicyLearnedHistory.IsBackground = true;
                            //thPolicyLearnedHistory.Start();
                        }
                        transaction.Complete();
                    }
                }
                catch (Exception ex)
                {
                    status.IsError = true;
                    status.ErrorMessage = "Policy " + OriginalPolicy.PolicyNumber + " is not saved succesfully.";
                    ActionLogger.Logger.WriteImportPolicyLog("Exception saving policy : " + ex.Message, true);
                    if (ex.InnerException != null)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Inner Exception saving policy : " + ex.InnerException.Message, true);
                        ActionLogger.Logger.WriteImportPolicyLog("Inner Exception saving policy : " + ex.InnerException.StackTrace, true);
                    }

                    try
                    {
                        string body = "Error saving policy details : ";
                        if (OriginalPolicy != null)
                        {
                            body += "\nPolicy: " + OriginalPolicy.ToStringDump();
                        }
                        body += "\nException: " + ex.Message;
                        body += "\nStack Trace: " + ex.StackTrace;
                        MailServerDetail.sendMail("jyotisna@acmeminds.com", "Commissions Alert: Error saving policy data", body);
                    }
                    catch (Exception e)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Error sending policy save failure mail: " + e.Message, true);
                    }

                }
            }

            return status;
        }
        public static void SetPolicyTerminationDate(ref PolicyDetailsData OriginalPolicy, ref PolicyDetailsData ReplacedPolicy)
        {
            if (OriginalPolicy != null)
            {
                try
                {
                    var _policyId = OriginalPolicy.PolicyId;
                    ActionLogger.Logger.WriteImportPolicyLog("SetPolicyTerminationDate: Exceution Start for orignalPolicy Set policyId" + _policyId, true);
                    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                    {
                        var getPolicyDetails = (from p in DataModel.Policies where (p.PolicyId == _policyId) select p).FirstOrDefault();
                        if (getPolicyDetails != null)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("SetPolicyTerminationDate: Exceution Start for existing policyId" + _policyId, true);
                            if ((OriginalPolicy.PolicyStatusId == 0 || OriginalPolicy.PolicyStatusId == 2) && (getPolicyDetails.PolicyStatusId != 1))
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("SetPolicyTerminationDate: Exceution process for policyId" + _policyId + "PolicyStatusId:" + OriginalPolicy.PolicyStatusId, true);
                                if (OriginalPolicy.PolicyTerminationDate == null)
                                {
                                    OriginalPolicy.PolicyTerminationDate = getPolicyDetails.PolicyTerminationDate;
                                }
                                else
                                {
                                    OriginalPolicy.PolicyTerminationDate = OriginalPolicy.PolicyTerminationDate;
                                }
                            }
                            //else
                            //{

                            //}
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("SetPolicyTerminationDate: Exceution Start for New Orignal policy policyId" + OriginalPolicy.PolicyId, true);
                            OriginalPolicy.PolicyTerminationDate = OriginalPolicy.PolicyTerminationDate;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("SetPolicyTerminationDate: Exception occur while saving policyTerminationDate: " + ex.Message, true);
                }
            }
            else if (ReplacedPolicy != null)
            {
                try
                {
                    var _policyId = ReplacedPolicy.PolicyId;
                    ActionLogger.Logger.WriteImportPolicyLog("SetPolicyTerminationDate: Exceution Start for ReplacePolicy Set policyId" + _policyId, true);
                    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                    {
                        var getPolicyDetails = (from p in DataModel.Policies where (p.PolicyId == _policyId) select p).FirstOrDefault();
                        if (getPolicyDetails != null)
                        {
                            if ((ReplacedPolicy.PolicyStatusId == 0 || ReplacedPolicy.PolicyStatusId == 2) && (getPolicyDetails.PolicyStatusId != 1))
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("SetPolicyTerminationDate: Exceution process for policyId" + _policyId + "PolicyStatusId:" + ReplacedPolicy.PolicyStatusId, true);
                                if (ReplacedPolicy.PolicyTerminationDate == null)
                                {
                                    ReplacedPolicy.PolicyTerminationDate = getPolicyDetails.PolicyTerminationDate;
                                }
                                else
                                {
                                    ReplacedPolicy.PolicyTerminationDate = ReplacedPolicy.PolicyTerminationDate;
                                }
                            }
                            //else
                            //{

                            //}
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("SetPolicyTerminationDate: Exceution Start for New Replace  policy policyId" + OriginalPolicy.PolicyId, true);
                            ReplacedPolicy.PolicyTerminationDate = ReplacedPolicy.PolicyTerminationDate;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("SetPolicyTerminationDate: Exception occur while saving policyTerminationDate: " + ex.Message, true);
                }
            }


        }
        //public static PolicySavedStatus SavePolicy(PolicyDetailsData OriginalPolicy, PolicyDetailsData ReplacedPolicy, string strRenewal)
        //{
        //    PolicySavedStatus status = new PolicySavedStatus();

        //    TransactionOptions options = new TransactionOptions
        //    {
        //        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
        //        Timeout = TimeSpan.FromMinutes(15)
        //    };

        //    if (OriginalPolicy != null || ReplacedPolicy != null)
        //    {
        //        try
        //        {
        //            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, options))
        //            {
        //                if (ReplacedPolicy != null)
        //                {
        //                    Policy.AddUpdatePolicy(ReplacedPolicy);
        //                    Policy.AddUpdatePolicyHistory(ReplacedPolicy.PolicyId);
        //                    PolicyLearnedField.AddUpdateHistoryLearned(ReplacedPolicy.PolicyId);
        //                }
        //                if (OriginalPolicy != null)
        //                {
        //                    //ActionLogger.Logger.WriteImportPolicyLog(" " + " Call AddUpdatePolicy", true);
        //                    Policy.AddUpdatePolicy(OriginalPolicy);
        //                    //ActionLogger.Logger.WriteImportPolicyLog(" " + "Call AddPolicyToLearn", true);                           
        //                    PolicyToLearnPost.AddPolicyToLearn(OriginalPolicy.PolicyId, strRenewal);
        //                    //ActionLogger.Logger.WriteImportPolicyLog(" " + "Call AddUpdatePolicyHistory", true);
        //                    Thread thPolicyHistory = new Thread(() =>
        //                        {
        //                            Policy.AddUpdatePolicyHistory(OriginalPolicy.PolicyId);
        //                        });
        //                    thPolicyHistory.IsBackground = true;
        //                    thPolicyHistory.Start();

        //                    Thread thPolicyLearnedHistory = new Thread(() =>
        //                        {
        //                            PolicyLearnedField.AddUpdateHistoryLearned(OriginalPolicy.PolicyId);
        //                        });
        //                    thPolicyLearnedHistory.IsBackground = true;
        //                    thPolicyLearnedHistory.Start();
        //                }
        //                transaction.Complete();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            status.IsError = true;
        //            status.ErrorMessage = "Policy " + OriginalPolicy.PolicyNumber + " is not saved succesfully.";
        //        }
        //    }

        //    return status;
        //}

        public static void UpdateRPolicyStatus(PolicyDetailsData _policyr)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.Policies where (p.PolicyId == _policyr.PolicyId) select p).FirstOrDefault();
                if (_policy != null)
                {
                    _policy.MasterPolicyStatuReference.Value = (from m in DataModel.MasterPolicyStatus where m.PolicyStatusId == (int)_PolicyStatus.Terminated select m).FirstOrDefault();
                    DataModel.SaveChanges();
                }
            }
        }

        public void UpdatePolicyTermDate(Guid policyID, DateTime? dtTermReson)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.Policies where (p.PolicyId == policyID) select p).FirstOrDefault();
                if (_policy != null)
                {
                    _policy.MasterPolicyStatuReference.Value = (from m in DataModel.MasterPolicyStatus where m.PolicyStatusId == (int)_PolicyStatus.Terminated select m).FirstOrDefault();
                    // _policy.PolicyStatusId=1;
                    //Term reson "Per Carrier
                    _policy.TerminationReasonId = 5;
                    _policy.PolicyTerminationDate = dtTermReson;
                    DataModel.SaveChanges();
                }
            }
        }

        public static void UpdatePolicySetting(PolicyDetailsData policy)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DataModel.Policies.Where(p => p.PolicyId == policy.PolicyId).FirstOrDefault().IsTrackMissingMonth = policy.IsTrackMissingMonth;
                DataModel.Policies.Where(p => p.PolicyId == policy.PolicyId).FirstOrDefault().IsTrackIncomingPercentage = policy.IsTrackIncomingPercentage;
                DataModel.SaveChanges();
            }
        }

        public static string GetPolicyProductType(Guid policyID, Guid PayorID, Guid CarrierID, Guid CoverageID)
        {
            string strNickName = string.Empty;

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var strproductType = from p in DataModel.Policies
                                         where (p.PolicyId == policyID && p.PayorId == PayorID && p.CarrierId == CarrierID && p.CoverageId == CoverageID)
                                         select p.ProductType;

                    foreach (var item in strproductType)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            strNickName = Convert.ToString(item);
                        }

                    }

                }

            }
            catch
            {
            }
            return strNickName;
        }

        public static void UpdateLastFollowupRunsWithTodayDate(Guid PolicyId)
        {
            try
            {
                if (PolicyId == Guid.Empty) return;
                if (PolicyId == null) return;
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.UpdatelastFollowupRuns(PolicyId);
                }
            }
            catch
            {
            }

        }

        public static void MarkPolicyDeleted(PolicyDetailsData _policyrecord)
        {
            ActionLogger.Logger.WriteImportPolicyLog("MarkPolicyDeleted : PolicyID - " + _policyrecord.PolicyId + ", UserID: " + _policyrecord.UserCredentialId, true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    var _policy = (from p in DataModel.Policies where (p.PolicyId == _policyrecord.PolicyId) select p).FirstOrDefault();
                    if (_policy != null)
                    {
                        _policy.IsDeleted = true;
                        DataModel.SaveChanges();
                        ActionLogger.Logger.WriteImportPolicyLog("MarkPolicyDeleted : PolicyID - " + _policyrecord.PolicyId + " deleted successfully by UserID: " + _policyrecord.UserCredentialId, true);
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("MarkPolicyDeleted : PolicyID - " + _policyrecord.PolicyId + " not found for deletion", true);
                    }
                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("MarkPolicyDeleted exception: PolicyID - " + _policyrecord.PolicyId + ", exception: " + ex.Message, true);
                }
            }
        }

        public static void MarkPolicyDeletedById(Guid PolicyId)
        {
            ActionLogger.Logger.WriteImportPolicyLog("MarkPolicyDeletedById : PolicyID - " + PolicyId, true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    var _policy = (from p in DataModel.Policies where (p.PolicyId == PolicyId) select p).FirstOrDefault();
                    if (_policy != null)
                    {
                        _policy.IsDeleted = true;
                        DataModel.SaveChanges();
                    }
                    ActionLogger.Logger.WriteImportPolicyLog("MarkPolicyDeletedById completed : PolicyID - " + PolicyId, true);
                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("MarkPolicyDeletedById  exception: PolicyID - " + PolicyId + ", ex: " + ex.Message, true);
                }
            }
        }

        public static void DeletePolicyFromDB(PolicyDetailsData _Policy)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeletePolicyFromDB started: " + _Policy.PolicyId, true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.Policies where (p.PolicyId == _Policy.PolicyId) select p).FirstOrDefault();
                //Check null before going to delete on the basis of Policy ID
                if (_policy == null) return;
                DataModel.DeleteObject(_policy);
                DataModel.SaveChanges();

            }
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeletePolicyFromDB ended: " + _Policy.PolicyId, true);
        }

        public static void DeletePolicyFromDBById(Guid PolicyId)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeletePolicyFromDBById started: " + PolicyId, true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.Policies where (p.PolicyId == PolicyId) select p).FirstOrDefault();
                DataModel.DeleteObject(_policy);
                DataModel.SaveChanges();
            }
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeletePolicyFromDBById ended: " + PolicyId, true);
        }

        public static void DeletePolicyCascadeFromDBById(Guid PolicyId)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeletePolicyCascadeFromDBById started: " + PolicyId, true);
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    //PolicyOutgoingSchedule
                    List<DLinq.PolicyOutgoingSchedule> _PolicyOutgoingSchedulelst = DataModel.PolicyOutgoingSchedules.Where(p => p.PolicyId == PolicyId).ToList();
                    if (_PolicyOutgoingSchedulelst != null)
                    {
                        foreach (DLinq.PolicyOutgoingSchedule pos in _PolicyOutgoingSchedulelst)
                            DataModel.DeleteObject(pos);
                    }

                    //PolicyOutgoingAdvancedSchedule 
                    List<DLinq.PolicyOutgoingAdvancedSchedule> _PolicyOutgoingAdvancedScheduleLst = DataModel.PolicyOutgoingAdvancedSchedules
                                                                                                    .Where(p => p.PolicyId == PolicyId).ToList();
                    if (_PolicyOutgoingAdvancedScheduleLst != null)
                    {
                        foreach (DLinq.PolicyOutgoingAdvancedSchedule poas in _PolicyOutgoingAdvancedScheduleLst)
                        {
                            DataModel.DeleteObject(poas);
                        }

                    }
                    //PolicyNote
                    List<DLinq.PolicyNote> _PolicyNoteLst = DataModel.PolicyNotes.Where(p => p.PolicyId == PolicyId).ToList();
                    if (_PolicyNoteLst != null)
                    {
                        foreach (DLinq.PolicyNote pn in _PolicyNoteLst)
                        {
                            DataModel.DeleteObject(pn);
                        }
                    }

                    //PolicyLevelBillingDetail
                    List<DLinq.PolicyLevelBillingDetail> _PolicyLevelBillingDetailLst = DataModel.PolicyLevelBillingDetails.Where(p => p.PolicyId == PolicyId).ToList();
                    if (_PolicyLevelBillingDetailLst != null)
                    {
                        foreach (DLinq.PolicyLevelBillingDetail plbd in _PolicyLevelBillingDetailLst)
                        {
                            DataModel.DeleteObject(plbd);
                        }
                    }

                    //PolicyLearnedFieldsHistory
                    List<DLinq.PolicyLearnedFieldsHistory> _PolicyLearnedFieldsHistoryLst = DataModel.PolicyLearnedFieldsHistories.Where(p => p.PolicyId == PolicyId).ToList();
                    if (_PolicyLearnedFieldsHistoryLst != null)
                    {
                        foreach (DLinq.PolicyLearnedFieldsHistory plfh in _PolicyLearnedFieldsHistoryLst)
                        {
                            DataModel.DeleteObject(plfh);
                        }
                    }

                    //PolicyLearnedField
                    List<DLinq.PolicyLearnedField> _PolicyLearnedFieldLst = DataModel.PolicyLearnedFields.Where(p => p.PolicyId == PolicyId).ToList();
                    if (_PolicyLearnedFieldLst != null)
                    {
                        foreach (DLinq.PolicyLearnedField plf in _PolicyLearnedFieldLst)
                        {
                            DataModel.DeleteObject(plf);
                        }
                    }

                    //PolicyIncomingAdvancedSchedule
                    List<DLinq.PolicyIncomingAdvancedSchedule> _PolicyIncomingAdvancedScheduleLst = DataModel.PolicyIncomingAdvancedSchedules.Where(p => p.PolicyId == PolicyId).ToList();
                    if (_PolicyIncomingAdvancedScheduleLst != null)
                    {
                        foreach (DLinq.PolicyIncomingAdvancedSchedule pias in _PolicyIncomingAdvancedScheduleLst)
                        {
                            DataModel.DeleteObject(pias);
                        }
                    }

                    //PolicyIncomingSchedule
                    List<DLinq.PolicyIncomingSchedule> _PolicyIncomingScheduleLst = DataModel.PolicyIncomingSchedules
                                                                                        .Where(p => p.PolicyId == PolicyId).ToList();
                    if (_PolicyIncomingScheduleLst != null)
                    {
                        foreach (DLinq.PolicyIncomingSchedule pis in _PolicyIncomingScheduleLst)
                        {
                            DataModel.DeleteObject(pis);
                        }
                    }

                    //LastPolicyViewed
                    List<DLinq.LastPolicyViewed> _LastPolicyViewedLst = DataModel.LastPolicyVieweds
                                                                        .Where(p => p.PolicyId == PolicyId).ToList();

                    if (_LastPolicyViewedLst != null)
                    {
                        foreach (DLinq.LastPolicyViewed lpv in _LastPolicyViewedLst)
                        {

                            DataModel.DeleteObject(lpv);
                        }
                    }

                    //FollowupIssue
                    List<DLinq.FollowupIssue> _FollowupIssueLst = DataModel.FollowupIssues
                                                                .Where(p => p.PolicyId == PolicyId).ToList();
                    if (_FollowupIssueLst != null)
                    {
                        foreach (DLinq.FollowupIssue fil in _FollowupIssueLst)
                        {
                            DataModel.DeleteObject(fil);
                        }
                    }

                    //PolicyPaymentEntry
                    List<DLinq.PolicyPaymentEntry> _PolicyPaymentEntryLst = DataModel.PolicyPaymentEntries.Where(p => p.PolicyId == PolicyId).ToList();
                    if (_PolicyPaymentEntryLst != null)
                    {
                        foreach (DLinq.PolicyPaymentEntry ppel in _PolicyPaymentEntryLst)
                        {
                            List<DLinq.PolicyOutgoingPayment> _PolicyOutgoingPaymentLst = DataModel.PolicyOutgoingPayments
                                                            .Where(p => p.PaymentEntryId == ppel.PaymentEntryId).ToList();
                            foreach (DLinq.PolicyOutgoingPayment popl in _PolicyOutgoingPaymentLst)
                            {
                                DataModel.DeleteObject(popl);
                            }

                        }
                        foreach (DLinq.PolicyPaymentEntry ppel in _PolicyPaymentEntryLst)
                        {
                            var payment_followup = DataModel.FollowupIssues.Where(pf => pf.IssueId == ppel.FollowUpVarIssueId).AsEnumerable();
                            foreach (var pay_follow in payment_followup)
                            {
                                DataModel.DeleteObject(pay_follow);
                            }
                            DataModel.DeleteObject(ppel);
                        }
                    }

                    List<DLinq.EntriesByDEU> _entryByDEU = DataModel.EntriesByDEUs.Where(p => p.PolicyID == PolicyId).ToList();
                    if (_entryByDEU != null)
                    {
                        foreach (DLinq.EntriesByDEU ebd in _entryByDEU)
                        {
                            DataModel.DeleteObject(ebd);
                        }
                    }

                    var _policyHis = (from p in DataModel.PoliciesHistories where (p.PolicyId == PolicyId) select p).FirstOrDefault();
                    if (_policyHis != null)
                    {
                        DataModel.DeleteObject(_policyHis);
                    }

                    var _policy = (from p in DataModel.Policies where (p.PolicyId == PolicyId) select p).FirstOrDefault();
                    DataModel.DeleteObject(_policy);
                    DataModel.SaveChanges();
                }
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeletePolicyCascadeFromDBById ended: " + PolicyId, true);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeletePolicyCascadeFromDBById exception: " + PolicyId + "      " + ex.Message, true);
            }

        }

        public static bool CheckForPolicyPaymentExists(Guid Policyid)
        {
            bool flag = false;

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PolicyId", Policyid);
            PolicyDetailsData _policylst = Policy.GetPolicyData(parameters).FirstOrDefault();
            if (_policylst != null)
            {
                if (_policylst.policyPaymentEntries == null)
                {
                    _policylst.policyPaymentEntries = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(Policyid);
                }
                List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = _policylst.policyPaymentEntries;
                /*Acme - commented following after Kevin's email to check for payment entries only*/
                /*foreach (PolicyPaymentEntriesPost popy in _PolicyPaymentEntriesPost)
                {
                    List<PolicyOutgoingDistribution> _PolicyOutgoingDistributionLst = PolicyOutgoingDistribution.GetOutgoingPaymentByPoicyPaymentEntryId(popy.PaymentEntryID);
                    if (_PolicyOutgoingDistributionLst != null && _PolicyOutgoingDistributionLst.Count != 0)
                    {
                        flag = true;
                        return flag;
                    }
                }*/
                if (_PolicyPaymentEntriesPost != null && _PolicyPaymentEntriesPost.Count > 0)
                {
                    flag = true;
                    return flag;
                }
            }

            return flag;
        }

        #endregion

        #region Get Methods

        public static int? GetPolicyStatusID(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var policyStatusId = from pl in DataModel.Policies
                                     where (pl.IsDeleted == false) && (pl.PolicyId == PolicyId)
                                     select new { pl.PolicyStatusId }.PolicyStatusId;
                return (int?)policyStatusId.SingleOrDefault();
            }
        }

        public static bool FollowUpRunsRequired(Guid PolicyId)
        {
            bool flag = true;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DateTime? LastFollowUpRuns = DataModel.Policies.Where(p => p.PolicyId == PolicyId).FirstOrDefault().LastFollowUpRuns;
                int DaysCnt = Convert.ToInt32(SystemConstant.GetKeyValue(MasterSystemConst.NextFollowUpRunDaysCount.ToString()));
                if (LastFollowUpRuns.HasValue)
                {
                    DateTime finaldatetorun = LastFollowUpRuns.Value.AddDays(DaysCnt);
                    if (DateTime.Today > finaldatetorun)
                        flag = true;
                    else
                        flag = false;
                }
            }
            return flag;
        }

        public static bool IsTrackPaymentChecked(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var isTrackPaymentchecked = (from p in DataModel.Policies
                                             where p.PolicyId == PolicyId
                                             select new { p.IsTrackPayment }.IsTrackPayment).FirstOrDefault();
                return isTrackPaymentchecked;
            }

        }

        public DateTime? GetFollowUpDate(Guid PolicyID)
        {
            DateTime? dtFollowupDate = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                dtFollowupDate = (from pl in DataModel.Policies.Where(p => p.PolicyId == PolicyID)
                                  select pl.LastFollowUpRuns).FirstOrDefault();

                return dtFollowupDate;

            }

        }

        public static List<PolicyDetailsData> GetPolicyDataForWindowService(Dictionary<string, object> Parameters, Expression<Func<DLinq.Policy, bool>> ParameterExpression = null)
        {
            Expression<Func<DLinq.Policy, bool>> parametersFromHelperClass = HelperClass.getWhereClause<DLinq.Policy>(Parameters);
            if (ParameterExpression != null)
            {
                parametersFromHelperClass = parametersFromHelperClass.And(ParameterExpression);
            }
            return FillinAllDataForWindowService(parametersFromHelperClass, ParameterExpression);
        }

        private static List<PolicyDetailsData> FillinAllDataForWindowService(Expression<Func<DLinq.Policy, bool>> parameters, Expression<Func<DLinq.Policy, bool>> ParameterExpression = null)
        {
            List<DLinq.Policy> policies;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                policies = (from pl in DataModel.Policies
                            .Where(parameters)
                            select pl).ToList();
                List<PolicyDetailsData> endList = new List<PolicyDetailsData>();

                if (policies == null)
                {
                    return endList;
                }
                try
                {
                    endList = (from pl in policies
                               select new PolicyDetailsData
                               {
                                   //PolicyId = pl.PolicyId == null ? Guid.Empty : pl.PolicyId,
                                   PolicyId = pl.PolicyId,
                                   PolicyNumber = pl.PolicyNumber == null ? string.Empty : pl.PolicyNumber,
                                   PolicyStatusId = pl.MasterPolicyStatu.PolicyStatusId,
                                   PolicyStatusName = pl.MasterPolicyStatu.Name,
                                   PolicyType = pl.PolicyType == null ? string.Empty : pl.PolicyType,
                                   PolicyLicenseeId = pl.Licensee.LicenseeId,
                                   Insured = pl.Insured == null ? string.Empty : pl.Insured,
                                   OriginalEffectiveDate = pl.OriginalEffectiveDate,
                                   TrackFromDate = pl.TrackFromDate,
                                   PolicyModeId = (pl.MasterPolicyMode != null) ? pl.MasterPolicyMode.PolicyModeId : 0,
                                   ModeAvgPremium = pl.MonthlyPremium,
                                   SubmittedThrough = pl.SubmittedThrough == null ? string.Empty : pl.SubmittedThrough,
                                   Enrolled = pl.Enrolled == null ? string.Empty : pl.Enrolled,
                                   Eligible = pl.Eligible == null ? string.Empty : pl.Eligible,
                                   PolicyTerminationDate = pl.PolicyTerminationDate,
                                   TerminationReasonId = pl.TerminationReasonId,
                                   IsTrackMissingMonth = pl.IsTrackMissingMonth,
                                   IsTrackIncomingPercentage = pl.IsTrackIncomingPercentage,
                                   IsTrackPayment = pl.IsTrackPayment,
                                   IsDeleted = pl.IsDeleted,
                                   CarrierID = pl.Carrier == null ? Guid.Empty : pl.Carrier.CarrierId,
                                   CarrierName = pl.Carrier == null ? string.Empty : pl.Carrier.CarrierName,
                                   CoverageId = pl.Coverage == null ? Guid.Empty : pl.Coverage.CoverageId,
                                   CoverageName = pl.Coverage == null ? string.Empty : pl.Coverage.ProductName,
                                   ClientId = pl.Client == null ? Guid.Empty : pl.Client.ClientId,
                                   ClientName = pl.Client == null ? string.Empty : pl.Client.Name,
                                   ReplacedBy = pl.ReplacedBy,
                                   DuplicateFrom = pl.DuplicateFrom,
                                   IsIncomingBasicSchedule = pl.IsIncomingBasicSchedule,
                                   IsOutGoingBasicSchedule = pl.IsOutGoingBasicSchedule,
                                   PayorId = pl.Payor == null ? Guid.Empty : pl.Payor.PayorId,
                                   PayorName = pl.Payor == null ? string.Empty : pl.Payor.PayorName,
                                   PayorNickName = pl.Payor == null ? string.Empty : pl.Payor.NickName,
                                   SplitPercentage = pl.SplitPercentage,
                                   IncomingPaymentTypeId = pl.IncomingPaymentTypeId,
                                   PolicyIncomingPayType = pl.MasterIncomingPaymentType.Name,
                                   CreatedOn = pl.CreatedOn,
                                   RowVersion = pl.RowVersion,
                                   CreatedBy = pl.CreatedBy.Value,//--always check it will never null                                   
                                   IsSavedPolicy = true,
                                   CompType = pl.PolicyLearnedField == null ? null : pl.PolicyLearnedField.CompTypeID == null ? null : pl.PolicyLearnedField.CompTypeID,
                                   CompSchuduleType = pl.PolicyLearnedField == null ? string.Empty : pl.PolicyLearnedField.CompScheduleType == null ? string.Empty : pl.PolicyLearnedField.CompScheduleType,
                                   LastFollowUpRuns = pl.LastFollowUpRuns,

                               }).ToList();
                }
                catch
                {
                }

                endList = new List<PolicyDetailsData>(endList.Where(p => p.PolicyNumber != string.Empty)).ToList();
                endList = new List<PolicyDetailsData>(endList.Where(p => p.ClientId != Guid.Empty)).ToList();

                // endList.ForEach(p => p.PolicyPreviousData = FillPolicyDetailPreviousData(p));       

                return endList;
            }

        }

        //CReated by Acme - to return trackdate - no modification done in previous method "GetPolicyTrackDate"
        public static DateTime? GetPolicyTrackFromDate(Guid PolicyID, string agencyName = "")//find1
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetPolicyTrackFromDate policyID" + PolicyID, true, agencyName);
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Select TrackFromDate from policies where policyID ='" + PolicyID + "'", con))
                    {
                        cmd.Parameters.AddWithValue("@PolicyId", PolicyID);
                        con.Open();

                        object obj = cmd.ExecuteScalar();
                        if (obj != null)
                        {
                            DateTime dtTime = Convert.ToDateTime(obj);
                            ActionLogger.Logger.WriteImportPolicyLog("GetPolicyTrackFromDate date found: " + dtTime, true, agencyName);
                            return dtTime;
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("GetPolicyTrackFromDate date not found: ", true, agencyName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetPolicyTrackFromDate exception: " + ex.Message, true, agencyName);
            }
            return null;
        }

        public static DateTime? GetPolicyTrackDate(Guid PolicyID)//find1
        {

            DateTime? dtTime = null;
            DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
            EntityConnection ec = (EntityConnection)ctx.Connection;
            SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
            string adoConnStr = sc.ConnectionString;

            using (SqlConnection con = new SqlConnection(adoConnStr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetPolicyTrackFromDate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyId", PolicyID);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    // Call Read before accessing data. 
                    while (reader.Read())
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["TrackFromDate"])))
                            {
                                dtTime = Convert.ToDateTime(reader["TrackFromDate"]);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return dtTime;

        }

        public static List<PolicyDetailsData> GetPolicyData(Dictionary<string, object> Parameters, Expression<Func<DLinq.Policy, bool>> ParameterExpression = null)
        {
            try
            {
                for (int i = 0; i < Parameters.Count; i++)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("GetPolicyData parameters :  " + Parameters.ElementAt(i).Key + ", value: " + Parameters.ElementAt(i).Value, true);
                }
                Expression<Func<DLinq.Policy, bool>> parametersFromHelperClass = HelperClass.getWhereClause<DLinq.Policy>(Parameters);
                if (ParameterExpression != null)
                {
                    parametersFromHelperClass = parametersFromHelperClass.And(ParameterExpression);
                }
                return FillinAllData(parametersFromHelperClass, ParameterExpression);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetPolicyData:Exception occurs while processing" + ex.Message, true);
                throw ex;
            }

        }

        private static List<PolicyDetailsData> FillinAllData(Expression<Func<DLinq.Policy, bool>> parameters, Expression<Func<DLinq.Policy, bool>> ParameterExpression = null)
        {
            List<DLinq.Policy> policies;
            List<PolicyDetailsData> endList = new List<PolicyDetailsData>();
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    DataModel.CommandTimeout = 600000000;

                    policies = (from pl in DataModel.Policies.Where(parameters) select pl).ToList();

                    if (policies == null)
                    {
                        return endList;
                    }
                    //// ActionLogger.Logger.WriteImportPolicyLog("FillinAllData list policies  parameters :  " + parameters., true);


                    endList = (from pl in policies

                               select new PolicyDetailsData
                               {
                                   PolicyId = pl.PolicyId,
                                   PolicyNumber = pl.PolicyNumber == null ? string.Empty : pl.PolicyNumber,
                                   PolicyStatusId = pl.MasterPolicyStatu.PolicyStatusId,
                                   PolicyStatusName = pl.MasterPolicyStatu.Name,
                                   PolicyType = pl.PolicyType == null ? string.Empty : pl.PolicyType,
                                   PolicyLicenseeId = pl.Licensee.LicenseeId,
                                   Insured = pl.Insured == null ? string.Empty : pl.Insured,
                                   OriginalEffectiveDate = pl.OriginalEffectiveDate,
                                   TrackFromDate = pl.TrackFromDate,
                                   PolicyModeId = pl.MasterPolicyMode.PolicyModeId,
                                   ModeAvgPremium = pl.MonthlyPremium,
                                   SubmittedThrough = pl.SubmittedThrough == null ? string.Empty : pl.SubmittedThrough,
                                   Enrolled = pl.Enrolled == null ? string.Empty : pl.Enrolled,
                                   Eligible = pl.Eligible == null ? string.Empty : pl.Eligible,
                                   PolicyTerminationDate = pl.PolicyTerminationDate,
                                   TerminationReasonId = pl.TerminationReasonId,
                                   IsTrackMissingMonth = pl.IsTrackMissingMonth,
                                   IsTrackIncomingPercentage = pl.IsTrackIncomingPercentage,
                                   IsTrackPayment = pl.IsTrackPayment,
                                   IsDeleted = pl.IsDeleted,
                                   CarrierID = pl.Carrier == null ? Guid.Empty : pl.Carrier.CarrierId,
                                   CarrierName = pl.Carrier == null ? string.Empty : pl.Carrier.CarrierName,
                                   CoverageId = pl.Coverage == null ? Guid.Empty : pl.Coverage.CoverageId,
                                   CoverageName = pl.Coverage == null ? string.Empty : pl.Coverage.ProductName,

                                   ClientId = pl.Client == null ? Guid.Empty : pl.Client.ClientId,
                                   ClientName = pl.Client == null ? string.Empty : pl.Client.Name,

                                   ReplacedBy = pl.ReplacedBy,
                                   DuplicateFrom = pl.DuplicateFrom,
                                   IsIncomingBasicSchedule = pl.IsIncomingBasicSchedule,
                                   IsOutGoingBasicSchedule = pl.IsOutGoingBasicSchedule,
                                   PayorId = pl.Payor == null ? Guid.Empty : pl.Payor.PayorId,
                                   PayorName = pl.Payor == null ? string.Empty : pl.Payor.PayorName,
                                   PayorNickName = pl.Payor == null ? string.Empty : pl.Payor.NickName,
                                   SplitPercentage = pl.SplitPercentage == null ? 0.0 : pl.SplitPercentage,
                                   IncomingPaymentTypeId = pl.IncomingPaymentTypeId,
                                   PolicyIncomingPayType = pl.MasterIncomingPaymentType == null ? string.Empty : pl.MasterIncomingPaymentType.Name,
                                   CreatedOn = pl.CreatedOn,
                                   RowVersion = pl.RowVersion,
                                   CreatedBy = pl.CreatedBy.Value,
                                   IsSavedPolicy = true,
                                   CompType = pl.PolicyLearnedField == null ? null : pl.PolicyLearnedField.CompTypeID == null ? null : pl.PolicyLearnedField.CompTypeID,
                                   CompSchuduleType = pl.PolicyLearnedField == null ? string.Empty : pl.PolicyLearnedField.CompScheduleType == null ? string.Empty : pl.PolicyLearnedField.CompScheduleType,
                                   LastFollowUpRuns = pl.LastFollowUpRuns,
                                   Advance = pl.Advance == null ? null : pl.Advance,
                                   ProductType = pl.ProductType,
                                   //added
                                   AccoutExec = pl.AccoutExec,
                                   IsCustomBasicSchedule = pl.IsCustomBasicSchedule,
                                   CustomDateType = pl.CustomScheduleDateType,
                                   IsTieredSchedule = pl.IsTieredSchedule

                               }).ToList();


                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("FillinAllData " + ex.StackTrace.ToString(), true);
                    ActionLogger.Logger.WriteImportPolicyLog("FillinAllData " + ex.InnerException.ToString(), true);
                }
                //endList.ForEach(p => p.PolicyPreviousData = FillPolicyDetailPreviousData(p));

                endList = new List<PolicyDetailsData>(endList.Where(p => p.ClientId != Guid.Empty)).ToList();
                //  endList = new List<PolicyDetailsData>(endList.Where(p => p.PolicyNumber != string.Empty)).ToList(); Acme commented as data should be returned for blank policy numbers too.
                /* if (endList != null)
                 {
                     try
                     {
                        // ActionLogger.Logger.WriteImportPolicyLog("FillinAllData list count:  " + endList.Count, true);
                         foreach (PolicyDetailsData p in endList)
                         {
                           //  ActionLogger.Logger.WriteImportPolicyLog("FillinAllData list count:  policy: " + p.PolicyId + ", client: " + p.ClientId + ", Name: " + p.ClientName, true);
                         }
                     }
                     catch (Exception ex)
                     {
                         ActionLogger.Logger.WriteImportPolicyLog("FillinAllData logging exception:  " + ex.Message, true);
                     }
                 }*/
                return endList.OrderBy(p => p.PolicyNumber).ToList();
            }
        }

        public static PolicyDetailsData GetPolicyDetailsOnPolicyID(Guid policyID, string agencyName = "")
        {
            PolicyDetailsData policies = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    DataModel.CommandTimeout = 600000000;

                    policies = (from pl in DataModel.Policies
                                where pl.PolicyId == policyID
                                select new PolicyDetailsData
                                {
                                    PolicyId = pl.PolicyId,
                                    PolicyNumber = pl.PolicyNumber == null ? string.Empty : pl.PolicyNumber,
                                    PolicyStatusId = pl.MasterPolicyStatu.PolicyStatusId,
                                    PolicyStatusName = pl.MasterPolicyStatu.Name,
                                    PolicyType = pl.PolicyType == null ? string.Empty : pl.PolicyType,
                                    PolicyLicenseeId = pl.Licensee.LicenseeId,
                                    Insured = pl.Insured == null ? string.Empty : pl.Insured,
                                    OriginalEffectiveDate = pl.OriginalEffectiveDate,
                                    TrackFromDate = pl.TrackFromDate,
                                    PolicyModeId = pl.MasterPolicyMode.PolicyModeId,
                                    ModeAvgPremium = pl.MonthlyPremium,
                                    SubmittedThrough = pl.SubmittedThrough == null ? string.Empty : pl.SubmittedThrough,
                                    Enrolled = pl.Enrolled == null ? string.Empty : pl.Enrolled,
                                    Eligible = pl.Eligible == null ? string.Empty : pl.Eligible,
                                    PolicyTerminationDate = pl.PolicyTerminationDate,
                                    TerminationReasonId = pl.TerminationReasonId,
                                    IsTrackMissingMonth = pl.IsTrackMissingMonth,
                                    IsTrackIncomingPercentage = pl.IsTrackIncomingPercentage,
                                    IsTrackPayment = pl.IsTrackPayment,
                                    IsDeleted = pl.IsDeleted,
                                    CarrierID = pl.Carrier == null ? Guid.Empty : pl.Carrier.CarrierId,
                                    CarrierName = pl.Carrier == null ? string.Empty : pl.Carrier.CarrierName,
                                    CoverageId = pl.Coverage == null ? Guid.Empty : pl.Coverage.CoverageId,
                                    CoverageName = pl.Coverage == null ? string.Empty : pl.Coverage.ProductName,

                                    ClientId = pl.Client == null ? Guid.Empty : pl.Client.ClientId,
                                    ClientName = pl.Client == null ? string.Empty : pl.Client.Name,

                                    ReplacedBy = pl.ReplacedBy,
                                    DuplicateFrom = pl.DuplicateFrom,
                                    IsIncomingBasicSchedule = pl.IsIncomingBasicSchedule,
                                    IsOutGoingBasicSchedule = pl.IsOutGoingBasicSchedule,
                                    PayorId = pl.Payor == null ? Guid.Empty : pl.Payor.PayorId,
                                    PayorName = pl.Payor == null ? string.Empty : pl.Payor.PayorName,
                                    PayorNickName = pl.Payor == null ? string.Empty : pl.Payor.NickName,
                                    SplitPercentage = pl.SplitPercentage == null ? 0.0 : pl.SplitPercentage,
                                    IncomingPaymentTypeId = pl.IncomingPaymentTypeId,
                                    PolicyIncomingPayType = pl.MasterIncomingPaymentType == null ? string.Empty : pl.MasterIncomingPaymentType.Name,
                                    CreatedOn = pl.CreatedOn,
                                    RowVersion = pl.RowVersion,
                                    CreatedBy = pl.CreatedBy.Value,
                                    IsSavedPolicy = true,
                                    CompType = pl.PolicyLearnedField == null ? null : pl.PolicyLearnedField.CompTypeID == null ? null : pl.PolicyLearnedField.CompTypeID,
                                    CompSchuduleType = pl.PolicyLearnedField == null ? string.Empty : pl.PolicyLearnedField.CompScheduleType == null ? string.Empty : pl.PolicyLearnedField.CompScheduleType,
                                    LastFollowUpRuns = pl.LastFollowUpRuns,
                                    Advance = pl.Advance == null ? null : pl.Advance,
                                    ProductType = pl.ProductType,
                                    //added
                                    AccoutExec = pl.AccoutExec

                                }).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("GetPolicyDetailsOnPolicyID " + ex.StackTrace.ToString(), true, agencyName);
                    ActionLogger.Logger.WriteImportPolicyLog("GetPolicyDetailsOnPolicyID " + ex.InnerException.ToString(), true, agencyName);
                }
                return policies;
            }
        }

        public static List<PolicyDetailsData> GetPolicyClientWise(Guid LicenseeId, Guid ClientId)//find1
        {
            List<PolicyDetailsData> lstPolicyDetailsData = new List<PolicyDetailsData>();

            DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
            EntityConnection ec = (EntityConnection)ctx.Connection;
            SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
            string adoConnStr = sc.ConnectionString;

            DateTime? nullDateTime = null;
            int? nullint = null;
            bool? nullBool = null;
            Guid? nullGuid = null;
            decimal? Nulldecimal = null;

            using (SqlConnection con = new SqlConnection(adoConnStr))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_GetPolicyClientWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LicenseeId", LicenseeId);
                    cmd.Parameters.AddWithValue("@PolicyClientId", ClientId);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    // Call Read before accessing data. 
                    while (reader.Read())
                    {
                        try
                        {
                            PolicyDetailsData objPolicyDetailsData = new PolicyDetailsData();

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
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["PolicyNumber"])))
                                {
                                    objPolicyDetailsData.PolicyNumber = reader["PolicyNumber"] == null ? string.Empty : Convert.ToString(reader["PolicyNumber"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["PolicyStatusId"])))
                                {
                                    objPolicyDetailsData.PolicyStatusId = reader["PolicyStatusId"] == null ? nullint : Convert.ToInt32(reader["PolicyStatusId"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["PolicyStatusName"])))
                                {
                                    objPolicyDetailsData.PolicyStatusName = reader["PolicyStatusName"] == null ? string.Empty : Convert.ToString(reader["PolicyStatusName"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["PolicyType"])))
                                {
                                    objPolicyDetailsData.PolicyType = reader["PolicyType"] == null ? string.Empty : Convert.ToString(reader["PolicyType"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["LicenseeId"])))
                                {
                                    objPolicyDetailsData.PolicyLicenseeId = reader["LicenseeId"] == null ? nullGuid : (Guid)reader["LicenseeId"];
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["Insured"])))
                                {
                                    objPolicyDetailsData.Insured = reader["Insured"] == null ? string.Empty : Convert.ToString(reader["Insured"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["OriginalEffectiveDate"])))
                                {
                                    objPolicyDetailsData.OriginalEffectiveDate = reader["OriginalEffectiveDate"] == null ? nullDateTime : Convert.ToDateTime(reader["OriginalEffectiveDate"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["TrackFromDate"])))
                                {
                                    objPolicyDetailsData.TrackFromDate = reader["TrackFromDate"] == null ? nullDateTime : Convert.ToDateTime(reader["TrackFromDate"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["PolicyModeId"])))
                                {
                                    objPolicyDetailsData.PolicyModeId = reader["PolicyModeId"] == null ? nullint : Convert.ToInt32(reader["PolicyModeId"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["MonthlyPremium"])))
                                {
                                    objPolicyDetailsData.ModeAvgPremium = reader["MonthlyPremium"] == null ? Nulldecimal : Convert.ToDecimal(reader["MonthlyPremium"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["SubmittedThrough"])))
                                {
                                    objPolicyDetailsData.SubmittedThrough = reader["SubmittedThrough"] == null ? string.Empty : Convert.ToString(reader["SubmittedThrough"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["Enrolled"])))
                                {
                                    objPolicyDetailsData.Enrolled = reader["Enrolled"] == null ? string.Empty : Convert.ToString(reader["Enrolled"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["Eligible"])))
                                {
                                    objPolicyDetailsData.Eligible = reader["Eligible"] == null ? string.Empty : Convert.ToString(reader["Eligible"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["PolicyTerminationDate"])))
                                {
                                    objPolicyDetailsData.PolicyTerminationDate = reader["PolicyTerminationDate"] == null ? nullDateTime : Convert.ToDateTime(reader["PolicyTerminationDate"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["TerminationReasonId"])))
                                {
                                    objPolicyDetailsData.TerminationReasonId = reader["TerminationReasonId"] == null ? nullint : Convert.ToInt32(reader["TerminationReasonId"]);
                                }

                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["IsTrackMissingMonth"])))
                                {
                                    if (reader["IsTrackMissingMonth"] != null)
                                    {
                                        objPolicyDetailsData.IsTrackMissingMonth = Convert.ToBoolean(reader["IsTrackMissingMonth"]);
                                    }
                                }

                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["IsTrackIncomingPercentage"])))
                                {
                                    if (reader["IsTrackIncomingPercentage"] != null)
                                    {
                                        objPolicyDetailsData.IsTrackIncomingPercentage = Convert.ToBoolean(reader["IsTrackIncomingPercentage"]);
                                    }
                                }

                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["IsTrackPayment"])))
                                {
                                    if (reader["IsTrackPayment"] != null)
                                    {
                                        objPolicyDetailsData.IsTrackPayment = Convert.ToBoolean(reader["IsTrackPayment"]);
                                    }
                                }

                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["IsDeleted"])))
                                {
                                    if (reader["IsDeleted"] != null)
                                    {
                                        objPolicyDetailsData.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                                    }
                                }

                            }
                            catch
                            {
                            }
                            try
                            {

                                if (!string.IsNullOrEmpty(Convert.ToString(reader["CarrierID"])))
                                {
                                    objPolicyDetailsData.CarrierID = reader["CarrierID"] == null ? nullGuid : (Guid)(reader["CarrierID"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {

                                if (!string.IsNullOrEmpty(Convert.ToString(reader["CarrierName"])))
                                {
                                    objPolicyDetailsData.CarrierName = reader["CarrierName"] == null ? string.Empty : Convert.ToString(reader["CarrierName"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["CoverageId"])))
                                {
                                    objPolicyDetailsData.CoverageId = reader["CoverageId"] == null ? nullGuid : (Guid)(reader["CoverageId"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["ProductName"])))
                                {
                                    objPolicyDetailsData.CoverageName = reader["ProductName"] == null ? string.Empty : Convert.ToString(reader["ProductName"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["ClientId"])))
                                {
                                    objPolicyDetailsData.ClientId = reader["ClientId"] == null ? nullGuid : (Guid)(reader["ClientId"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["ClientsName"])))
                                {
                                    objPolicyDetailsData.ClientName = reader["ClientsName"] == null ? string.Empty : Convert.ToString(reader["ClientsName"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["ReplacedBy"])))
                                {
                                    objPolicyDetailsData.ReplacedBy = reader["ReplacedBy"] == null ? nullGuid : (Guid)(reader["ReplacedBy"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["DuplicateFrom"])))
                                {
                                    objPolicyDetailsData.DuplicateFrom = reader["DuplicateFrom"] == null ? nullGuid : (Guid)(reader["DuplicateFrom"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["IsIncomingBasicSchedule"])))
                                {
                                    objPolicyDetailsData.IsIncomingBasicSchedule = reader["IsIncomingBasicSchedule"] == null ? nullBool : Convert.ToBoolean(reader["IsIncomingBasicSchedule"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["IsOutGoingBasicSchedule"])))
                                {
                                    objPolicyDetailsData.IsOutGoingBasicSchedule = reader["IsOutGoingBasicSchedule"] == null ? nullBool : Convert.ToBoolean(reader["IsOutGoingBasicSchedule"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["CarrierID"])))
                                {
                                    objPolicyDetailsData.CarrierID = reader["CarrierID"] == null ? nullGuid : (Guid)(reader["CarrierID"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["PayorID"])))
                                {
                                    objPolicyDetailsData.PayorId = reader["PayorID"] == null ? nullGuid : (Guid)(reader["PayorID"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["PayorName"])))
                                {
                                    objPolicyDetailsData.PayorName = reader["PayorName"] == null ? string.Empty : Convert.ToString(reader["PayorName"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["SplitPercentage"])))
                                {
                                    objPolicyDetailsData.PayorNickName = reader["PayorNickName"] == null ? string.Empty : Convert.ToString(reader["PayorNickName"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["SplitPercentage"])))
                                {
                                    objPolicyDetailsData.SplitPercentage = reader["SplitPercentage"] == null ? 0 : Convert.ToDouble(reader["SplitPercentage"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["IncomingPaymentTypeId"])))
                                {
                                    objPolicyDetailsData.IncomingPaymentTypeId = reader["IncomingPaymentTypeId"] == null ? nullint : Convert.ToInt32(reader["IncomingPaymentTypeId"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["Name"])))
                                {
                                    objPolicyDetailsData.PolicyIncomingPayType = reader["Name"] == null ? string.Empty : Convert.ToString(reader["Name"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["CreatedOn"])))
                                {
                                    objPolicyDetailsData.CreatedOn = reader["CreatedOn"] == null ? nullDateTime : Convert.ToDateTime(reader["CreatedOn"]);
                                }

                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["RowVersion"])))
                                {
                                    objPolicyDetailsData.RowVersion = (Byte[])(reader["RowVersion"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["CreatedBy"])))
                                {
                                    objPolicyDetailsData.CreatedBy = (Guid)(reader["CreatedBy"]);
                                }
                            }
                            catch
                            {
                            }

                            objPolicyDetailsData.IsSavedPolicy = true;

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["CompTypeID"])))
                                {
                                    objPolicyDetailsData.CompSchuduleType = reader["CompTypeID"] == null ? string.Empty : Convert.ToString(reader["CompTypeID"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["CompScheduleType"])))
                                {
                                    objPolicyDetailsData.CompSchuduleType = reader["CompScheduleType"] == null ? string.Empty : Convert.ToString(reader["CompScheduleType"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["LastFollowUpRuns"])))
                                {
                                    objPolicyDetailsData.LastFollowUpRuns = reader["LastFollowUpRuns"] == null ? nullDateTime : Convert.ToDateTime(reader["LastFollowUpRuns"]);
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["Advance"])))
                                {
                                    objPolicyDetailsData.Advance = reader["Advance"] == null ? nullint : Convert.ToInt32(reader["Advance"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["AccoutExec"])))
                                {
                                    objPolicyDetailsData.AccoutExec = reader["AccoutExec"] == null ? null : Convert.ToString(reader["AccoutExec"]);
                                }
                            }
                            catch
                            {
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["UserCredentialId"])))
                                {
                                    objPolicyDetailsData.UserCredentialId = reader["UserCredentialId"] == null ? Guid.Empty : (Guid)(reader["UserCredentialId"]);
                                }
                            }
                            catch
                            {
                            }
                            //Acme - added with new split on dates feature
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["PrimaryAgent"])))
                            {
                                objPolicyDetailsData.PrimaryAgent = reader["PrimaryAgent"] == null ? Guid.Empty : (Guid)reader["PrimaryAgent"];
                            }
                            objPolicyDetailsData.IsCustomBasicSchedule = false;
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["IsCustomBasicSchedule"])))
                            {
                                string strIsCustom = Convert.ToString(reader["IsCustomBasicSchedule"]);
                                bool isCustom = false;
                                bool.TryParse(strIsCustom, out isCustom);
                                objPolicyDetailsData.IsCustomBasicSchedule = isCustom;

                                if (isCustom)
                                    objPolicyDetailsData.CustomDateType = Convert.ToString(reader["CustomScheduleDateType"]);
                            }
                            //Add a new check by Ankit For Hide PolicyTerminationDate on 21-11-18

                            if (objPolicyDetailsData.PolicyStatusId == 0 || objPolicyDetailsData.PolicyStatusId == 2)
                            {
                                objPolicyDetailsData.PolicyTerminationDate = null;
                                ActionLogger.Logger.WriteImportPolicyLog("Policy is Active/pending So policyTermination Date is not shown: " + objPolicyDetailsData.PolicyStatusId, true);
                            }
                            //Add new field 
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["IsTieredSchedule"])))
                            {
                                string strIsTiered = Convert.ToString(reader["IsTieredSchedule"]);
                                bool isTier = false;
                                bool.TryParse(strIsTiered, out isTier);
                                objPolicyDetailsData.IsTieredSchedule = isTier;
                            }

                            lstPolicyDetailsData.Add(objPolicyDetailsData);
                        }
                        catch (Exception ex)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Exception getting policy: " + ex.Message, true);
                        }

                    }

                    // Call Close when done reading.
                    reader.Close();
                }
            }

            return lstPolicyDetailsData;
        }

        //private static List<PolicyDetailsData> FillinAllData(Expression<Func<DLinq.Policy, bool>> parameters, Expression<Func<DLinq.Policy, bool>> ParameterExpression = null)
        //{
        //    //ActionLogger.Logger.WriteImportPolicyLog("Get policy started", true);
        //    List<DLinq.Policy> policies;
        //    List<PolicyDetailsData> endList = new List<PolicyDetailsData>();
        //    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //    {
        //        try
        //        {
        //            if (ParameterExpression == null)
        //            {
        //                string temp = null;
        //                string par = parameters.ToString();
        //                par = par.Replace("AndAlso", "And");
        //                string[] Splitstring = Regex.Split(par, "And");


        //                for (int i = 1; i < Splitstring.Count(); i++)
        //                {
        //                    temp = temp + "And" + Splitstring[i];
        //                }
        //                string re = parameters.ToString();

        //                var result = DataModel.FillPolicy(temp);

        //                //ActionLogger.Logger.WriteImportPolicyLog( "sp executed", true);

        //                if (result == null)
        //                {
        //                  //  ActionLogger.Logger.WriteImportPolicyLog("data is null", true);
        //                    return endList;
        //                }

        //                endList = (from pl in result
        //                           select new PolicyDetailsData
        //                           {
        //                               PolicyId = Guid.Parse(pl.PolicyId.ToString()),
        //                               PolicyNumber = pl.PolicyNumber == null ? string.Empty : pl.PolicyNumber,
        //                               PolicyStatusId = pl.PolicyStatusId,
        //                               PolicyStatusName = pl.PolicyStatusName,
        //                               PolicyType = pl.PolicyType == null ? string.Empty : pl.PolicyType,
        //                               PolicyLicenseeId = pl.LicenseeId,
        //                               Insured = pl.Insured == null ? string.Empty : pl.Insured,
        //                               OriginalEffectiveDate = pl.OriginalEffectiveDate,
        //                               TrackFromDate = pl.TrackFromDate,
        //                               PolicyModeId = pl.PolicyModeId,
        //                               ModeAvgPremium = pl.MonthlyPremium,
        //                               SubmittedThrough = pl.SubmittedThrough == null ? string.Empty : pl.SubmittedThrough,
        //                               Enrolled = pl.Enrolled == null ? string.Empty : pl.Enrolled,
        //                               Eligible = pl.Eligible == null ? string.Empty : pl.Eligible,
        //                               PolicyTerminationDate = pl.PolicyTerminationDate,
        //                               TerminationReasonId = pl.TerminationReasonId,
        //                               IsTrackMissingMonth = bool.Parse(pl.IsTrackMissingMonth.ToString()),
        //                               IsTrackIncomingPercentage = bool.Parse(pl.IsTrackIncomingPercentage.ToString()),
        //                               IsTrackPayment = bool.Parse(pl.IsTrackPayment.ToString()),
        //                               IsDeleted = bool.Parse(pl.IsDeleted.ToString()),
        //                               CarrierID = pl.CarrierID == null ? Guid.Empty : pl.CarrierID,
        //                               CarrierName = pl.CarrierName == null ? string.Empty : pl.CarrierName,
        //                               CoverageId = pl.CoverageId == null ? Guid.Empty : pl.CoverageId,
        //                               CoverageName = pl.ProductName == null ? string.Empty : pl.ProductName,
        //                               ClientId = pl.ClientId,
        //                               ClientName = pl.ClientsName,
        //                               ReplacedBy = pl.ReplacedBy,
        //                               DuplicateFrom = pl.DuplicateFrom,
        //                               IsIncomingBasicSchedule = pl.IsIncomingBasicSchedule,
        //                               IsOutGoingBasicSchedule = pl.IsOutGoingBasicSchedule,
        //                               PayorId = pl.PayorID == null ? Guid.Empty : pl.PayorID,
        //                               PayorName = pl.PayorName == null ? string.Empty : pl.PayorName,
        //                               PayorNickName = pl.PayorNickName == null ? string.Empty : pl.PayorNickName,
        //                               SplitPercentage = pl.SplitPercentage,
        //                               IncomingPaymentTypeId = pl.IncomingPaymentTypeId,
        //                               PolicyIncomingPayType = pl.Name,
        //                               CreatedOn = pl.CreatedOn,
        //                               RowVersion = pl.RowVersion,
        //                               CreatedBy = pl.CreatedBy.Value,//--always check it will never null                                   
        //                               IsSavedPolicy = true,
        //                               //CompType = pl.CompTypeID == null ? null : pl.CompTypeID,
        //                               CompSchuduleType = pl.CompScheduleType == null ? string.Empty : pl.CompScheduleType,  
        //                               LastFollowUpRuns=pl.LastFollowUpRuns,
        //                               Advance = pl.Advance == null ? null : pl.Advance,  


        //                           }).ToList();

        //            }

        //            else
        //            {
        //                policies = (from pl in DataModel.Policies
        //                            .Where(parameters)
        //                            select pl).ToList();

        //                if (policies == null)
        //                {
        //                    return endList;
        //                }
        //                endList = (from pl in policies
        //                           select new PolicyDetailsData
        //                           {
        //                               PolicyId = pl.PolicyId == null ? Guid.Empty : pl.PolicyId,
        //                               PolicyNumber = pl.PolicyNumber == null ? string.Empty : pl.PolicyNumber,
        //                               PolicyStatusId = pl.MasterPolicyStatu.PolicyStatusId,
        //                               PolicyStatusName = pl.MasterPolicyStatu.Name,
        //                               PolicyType = pl.PolicyType == null ? string.Empty : pl.PolicyType,
        //                               PolicyLicenseeId = pl.Licensee.LicenseeId,
        //                               Insured = pl.Insured == null ? string.Empty : pl.Insured,
        //                               OriginalEffectiveDate = pl.OriginalEffectiveDate,
        //                               TrackFromDate = pl.TrackFromDate,
        //                               PolicyModeId = pl.MasterPolicyMode.PolicyModeId,
        //                               ModeAvgPremium = pl.MonthlyPremium,
        //                               SubmittedThrough = pl.SubmittedThrough == null ? string.Empty : pl.SubmittedThrough,
        //                               Enrolled = pl.Enrolled == null ? string.Empty : pl.Enrolled,
        //                               Eligible = pl.Eligible == null ? string.Empty : pl.Eligible,
        //                               PolicyTerminationDate = pl.PolicyTerminationDate,
        //                               TerminationReasonId = pl.TerminationReasonId,
        //                               IsTrackMissingMonth = pl.IsTrackMissingMonth,
        //                               IsTrackIncomingPercentage = pl.IsTrackIncomingPercentage,
        //                               IsTrackPayment = pl.IsTrackPayment,
        //                               IsDeleted = pl.IsDeleted,
        //                               CarrierID = pl.Carrier == null ? Guid.Empty : pl.Carrier.CarrierId,
        //                               CarrierName = pl.Carrier == null ? string.Empty : pl.Carrier.CarrierName,
        //                               CoverageId = pl.Coverage == null ? Guid.Empty : pl.Coverage.CoverageId,
        //                               CoverageName = pl.Coverage == null ? string.Empty : pl.Coverage.ProductName,
        //                               ClientId = pl.Client.ClientId,
        //                               ClientName = pl.Client.Name,
        //                               ReplacedBy = pl.ReplacedBy,
        //                               DuplicateFrom = pl.DuplicateFrom,
        //                               IsIncomingBasicSchedule = pl.IsIncomingBasicSchedule,
        //                               IsOutGoingBasicSchedule = pl.IsOutGoingBasicSchedule,
        //                               PayorId = pl.Payor == null ? Guid.Empty : pl.Payor.PayorId,
        //                               PayorName = pl.Payor == null ? string.Empty : pl.Payor.PayorName,
        //                               PayorNickName = pl.Payor == null ? string.Empty : pl.Payor.NickName,
        //                               SplitPercentage = pl.SplitPercentage,
        //                               IncomingPaymentTypeId = pl.IncomingPaymentTypeId,
        //                               PolicyIncomingPayType = pl.MasterIncomingPaymentType.Name,
        //                               CreatedOn = pl.CreatedOn,
        //                               RowVersion = pl.RowVersion,
        //                               CreatedBy = pl.CreatedBy.Value,//--always check it will never null                                   
        //                               IsSavedPolicy = true,
        //                               CompType = pl.PolicyLearnedField == null ? null : pl.PolicyLearnedField.CompTypeID == null ? null : pl.PolicyLearnedField.CompTypeID,
        //                               CompSchuduleType = pl.PolicyLearnedField == null ? string.Empty : pl.PolicyLearnedField.CompScheduleType == null ? string.Empty : pl.PolicyLearnedField.CompScheduleType,
        //                               LastFollowUpRuns=pl.LastFollowUpRuns,
        //                               Advance = pl.Advance == null ? null : pl.Advance,                                      


        //                           }).ToList();
        //            }

        //        }
        //        catch(Exception ex)
        //        {
        //            //ActionLogger.Logger.WriteImportPolicyLog(ex.ToString(), true);
        //        }
        //        //if (policies.FirstOrDefault().PolicyLearnedField != null)
        //        //{
        //        //    //MapLearnedFields(endList, policies);
        //        //}
        //        endList.ForEach(p => p.PolicyPreviousData = FillPolicyDetailPreviousData(p));

        //        //if (policies.FirstOrDefault().PolicyPaymentEntries != null)
        //        //{
        //        //    //endList.ForEach(policy => MapPolicyIncomingPayment((policies.Where(p => p.PolicyId == policy.PolicyId).FirstOrDefault().PolicyPaymentEntries).ToList(), policy));
        //        //}

        //       // ActionLogger.Logger.WriteImportPolicyLog("end policy function", true);
        //       // ActionLogger.Logger.WriteImportPolicyLog(endList.Count.ToString(), true);

        //        return endList.OrderBy(p=>p.PolicyNumber).ToList();

        //        #region for Comment sp

        //        //policies = (from pl in DataModel.Policies
        //        //            .Where(parameters)
        //        //            select pl).ToList();
        //        //List<PolicyDetailsData> endList = new List<PolicyDetailsData>();

        //        //if (policies == null)
        //        //{
        //        //    return endList;
        //        //}
        //        //try
        //        //{
        //        //    endList = (from pl in policies
        //        //               select new PolicyDetailsData
        //        //               {
        //        //                   PolicyId = pl.PolicyId == null ? Guid.Empty : pl.PolicyId,
        //        //                   PolicyNumber = pl.PolicyNumber == null ? string.Empty : pl.PolicyNumber,
        //        //                   PolicyStatusId = pl.MasterPolicyStatu.PolicyStatusId,
        //        //                   PolicyStatusName = pl.MasterPolicyStatu.Name,
        //        //                   PolicyType = pl.PolicyType == null ? string.Empty : pl.PolicyType,
        //        //                   PolicyLicenseeId = pl.Licensee.LicenseeId,
        //        //                   Insured = pl.Insured == null ? string.Empty : pl.Insured,
        //        //                   OriginalEffectiveDate = pl.OriginalEffectiveDate,
        //        //                   TrackFromDate = pl.TrackFromDate,
        //        //                   PolicyModeId = pl.MasterPolicyMode.PolicyModeId,
        //        //                   ModeAvgPremium = pl.MonthlyPremium,
        //        //                   SubmittedThrough = pl.SubmittedThrough == null ? string.Empty : pl.SubmittedThrough,
        //        //                   Enrolled = pl.Enrolled == null ? string.Empty : pl.Enrolled,
        //        //                   Eligible = pl.Eligible == null ? string.Empty : pl.Eligible,
        //        //                   PolicyTerminationDate = pl.PolicyTerminationDate,
        //        //                   TerminationReasonId = pl.TerminationReasonId,
        //        //                   IsTrackMissingMonth = pl.IsTrackMissingMonth,
        //        //                   IsTrackIncomingPercentage = pl.IsTrackIncomingPercentage,
        //        //                   IsTrackPayment = pl.IsTrackPayment,
        //        //                   IsDeleted = pl.IsDeleted,
        //        //                   CarrierID = pl.Carrier == null ? Guid.Empty : pl.Carrier.CarrierId,
        //        //                   CarrierName = pl.Carrier == null ? string.Empty : pl.Carrier.CarrierName,
        //        //                   CoverageId = pl.Coverage == null ? Guid.Empty : pl.Coverage.CoverageId,
        //        //                   CoverageName = pl.Coverage == null ? string.Empty : pl.Coverage.ProductName,
        //        //                   ClientId = pl.Client.ClientId,
        //        //                   ClientName = pl.Client.Name,
        //        //                   ReplacedBy = pl.ReplacedBy,
        //        //                   DuplicateFrom = pl.DuplicateFrom,
        //        //                   IsIncomingBasicSchedule = pl.IsIncomingBasicSchedule,
        //        //                   IsOutGoingBasicSchedule = pl.IsOutGoingBasicSchedule,
        //        //                   PayorId = pl.Payor == null ? Guid.Empty : pl.Payor.PayorId,
        //        //                   PayorName = pl.Payor == null ? string.Empty : pl.Payor.PayorName,
        //        //                   PayorNickName = pl.Payor == null ? string.Empty : pl.Payor.NickName,
        //        //                   SplitPercentage = pl.SplitPercentage,
        //        //                   IncomingPaymentTypeId = pl.IncomingPaymentTypeId,
        //        //                   PolicyIncomingPayType=pl.MasterIncomingPaymentType.Name,
        //        //                   CreatedOn = pl.CreatedOn,
        //        //                   RowVersion = pl.RowVersion,
        //        //                   CreatedBy = pl.CreatedBy.Value,//--always check it will never null                                   
        //        //                   IsSavedPolicy = true,
        //        //                   CompType = pl.PolicyLearnedField == null ? null : pl.PolicyLearnedField.CompTypeID == null ? null : pl.PolicyLearnedField.CompTypeID,
        //        //                   CompSchuduleType = pl.PolicyLearnedField == null ? string.Empty : pl.PolicyLearnedField.CompScheduleType == null ? string.Empty : pl.PolicyLearnedField.CompScheduleType,


        //        //               }).ToList();
        //        //}
        //        //catch
        //        //{
        //        //}
        //        ////if (policies.FirstOrDefault().PolicyLearnedField != null)
        //        ////{
        //        ////    //MapLearnedFields(endList, policies);
        //        ////}
        //        //endList.ForEach(p => p.PolicyPreviousData = FillPolicyDetailPreviousData(p));
        //        ////if (policies.FirstOrDefault().PolicyPaymentEntries != null)
        //        ////{
        //        ////    //endList.ForEach(policy => MapPolicyIncomingPayment((policies.Where(p => p.PolicyId == policy.PolicyId).FirstOrDefault().PolicyPaymentEntries).ToList(), policy));
        //        ////}
        //        //return endList;

        //        #endregion
        //    }

        //}

        #endregion

        public PolicyDetailsData GetPolicyStting(Guid policyID)
        {

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                PolicyDetailsData _Policy = (from pl in DataModel.Policies
                                             where (pl.IsDeleted == false) && (pl.PolicyId == policyID)
                                             select new PolicyDetailsData
                                             {
                                                 PolicyId = pl.PolicyId,
                                                 IsTrackMissingMonth = pl.IsTrackMissingMonth,
                                                 IsTrackIncomingPercentage = pl.IsTrackIncomingPercentage,
                                                 PolicyNumber = pl.PolicyNumber,
                                             }).FirstOrDefault();

                return _Policy;
            }
        }

        #region Loading Releations for Policy
        private static void MapPolicyIncomingPayment(List<DLinq.PolicyPaymentEntry> Source, PolicyDetailsData Target)
        {
            List<PolicyPaymentEntriesPost> payments = (from u in Source
                                                       where u.PolicyId == Target.PolicyId
                                                       select new PolicyPaymentEntriesPost
                                                       {
                                                           PaymentEntryID = u.PaymentEntryId,
                                                           StmtID = u.StatementId.Value,
                                                           PolicyID = u.PolicyId.Value,
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
                                                           DEUEntryId = u.DEUEntryId ?? Guid.Empty,
                                                           StmtNumber = u.Statement.StatementNumber,
                                                           FollowUpVarIssueId = u.FollowUpVarIssueId,
                                                       }).ToList();

            Target.policyPaymentEntries = payments;
        }

        private static void MapPolicyOutGoingPayments(List<DLinq.PolicyOutgoingPayment> Source, PolicyPaymentEntriesPost Target)
        {
            List<PolicyOutgoingDistribution> policyIncomingSchedule = (from u in Source
                                                                       where u.PaymentEntryId == Target.PaymentEntryID
                                                                       select new PolicyOutgoingDistribution
                                                                       {
                                                                           PaymentEntryId = u.PaymentEntryId,
                                                                           CreatedOn = u.CreatedOn,
                                                                           IsPaid = u.IsPaid,
                                                                           OutgoingPaymentId = u.OutgoingPaymentId,
                                                                           OutGoingPerUnit = u.OutgoingPerUnit,
                                                                           RecipientUserCredentialId = u.RecipientUserCredentialId,
                                                                           PaidAmount = u.PaidAmount,
                                                                           Payment = u.Payment,
                                                                           Premium = u.Premium
                                                                       }).ToList();

        }

        private static void MapLearnedFields(List<PolicyDetailsData> policies, List<DLinq.Policy> Source)
        {
            var policyLearnedFields = (from plearned in Source
                                       select new PolicyLearnedFieldData
                                       {
                                           PolicyId = plearned.PolicyLearnedField.PolicyId,
                                           Insured = plearned.PolicyLearnedField.Insured,
                                           PolicyNumber = plearned.PolicyLearnedField.PolicyNumber,
                                           Effective = plearned.PolicyLearnedField.Effective,
                                           TrackFrom = plearned.PolicyLearnedField.TrackFrom,
                                           Renewal = plearned.PolicyLearnedField.Renewal,
                                           CarrierId = plearned.PolicyLearnedField.CarrierId,
                                           CoverageId = plearned.PolicyLearnedField.CoverageId,
                                           PAC = plearned.PolicyLearnedField.PAC,
                                           PMC = plearned.PolicyLearnedField.PMC,
                                           ModalAvgPremium = plearned.PolicyLearnedField.ModalAvgPremium,
                                           PolicyModeId = plearned.PolicyLearnedField.PolicyModeId,
                                           Enrolled = plearned.PolicyLearnedField.Enrolled,
                                           Eligible = plearned.PolicyLearnedField.Eligible,
                                           AutoTerminationDate = plearned.PolicyLearnedField.AutoTerminationDate,
                                           Link1 = plearned.PolicyLearnedField.Link1,
                                           PayorSysId = plearned.PolicyLearnedField.PayorSysID,
                                           LastModifiedOn = plearned.PolicyLearnedField.LastModifiedOn,
                                           LastModifiedUserCredentialId = plearned.PolicyLearnedField.LastModifiedUserCredentialid,
                                           CompTypeId = plearned.PolicyLearnedField.CompTypeID,
                                           CompScheduleType = plearned.PolicyLearnedField.CompScheduleType,
                                           PayorId = plearned.PolicyLearnedField.PayorId,
                                           PreviousEffectiveDate = plearned.PolicyLearnedField.PreviousEffectiveDate,
                                           PreviousPolicyModeid = plearned.PolicyLearnedField.PreviousPolicyModeId,
                                       }).ToList();
            policyLearnedFields.ForEach(plf => plf.CoverageNickName = Coverage.GetCoverageNickName(plf.PayorId ?? Guid.Empty, plf.CarrierId ?? Guid.Empty, plf.CoverageId ?? Guid.Empty));
            policyLearnedFields.ForEach(plf => plf.CarrierNickName = Carrier.GetCarrierNickName(plf.PayorId ?? Guid.Empty, plf.CarrierId ?? Guid.Empty));
            policies.ForEach(p => p.LearnedFields = policyLearnedFields.Where(pl => pl.PolicyId == p.PolicyId).FirstOrDefault());
        }
        #endregion

        public static decimal GetPMC(Guid policyID)
        {
            return PostUtill.CalculatePMC(policyID);
        }

        public static decimal GetPAC(Guid policyID)
        {
            return PostUtill.CalculatePAC(policyID);
        }

        #region Batch and Others
        public static Batch GenerateBatch(PolicyDetailsData _policy)
        {
            bool IsBatchPaid = false;
            Batch _batch = null;
            Batch objBatch = new Batch();
            //List<Batch> tempBatchlst = Batch.GetBatchList(UploadStatus.Automatic);
            List<Batch> tempBatchlst = objBatch.GetBatchList(UploadStatus.Automatic);

            tempBatchlst = tempBatchlst == null ? null : tempBatchlst
                   .Where(p => p.PayorId == _policy.PayorId).Where(p => p.LicenseeId == _policy.PolicyLicenseeId).ToList();

            foreach (Batch bth in tempBatchlst)
            {
                IsBatchPaid = objBatch.GetBatchPaidStatus(bth.BatchId);
                if (!IsBatchPaid)
                {
                    _batch = bth;
                    break;
                }
            }

            if (_batch == null)
            {
                _batch = new Batch();
                _batch.BatchId = Guid.NewGuid();
                _batch.LicenseeId = _policy.PolicyLicenseeId.Value;
                _batch.PayorId = _policy.PayorId;
                _batch.CreatedDate = DateTime.Today;
                _batch.FileType = null;
                _batch.UploadStatus = UploadStatus.Automatic;
                _batch.EntryStatus = EntryStatus.BatchCompleted;
                _batch.AssignedDeuUserName = null;
                _batch.CreatedFromUpload = null;
                _batch.FileName = null;
                _batch.LastModifiedDate = DateTime.Today;
                _batch.IsManuallyUploaded = null;
                _batch.SiteId = null;

                _batch.AddUpdate();
            }
            else
            {
                _batch.LastModifiedDate = DateTime.Today;
                _batch.AddUpdate();
            }
            return _batch;
        }

        public static Statement GenerateStatment(Guid BatchId, Guid PayorId, decimal PaymentRecived, Guid CreatedBy)
        {
            Statement _Statement = null;
            //List<Statement> StmtLst = Statement.GetStatementList(BatchId);
            //Added statement object
            Statement objStatement = new Statement();
            List<Statement> StmtLst = objStatement.GetStatementList(BatchId);

            if (StmtLst != null)
                _Statement = StmtLst.Where(p => p.PayorId == PayorId).FirstOrDefault();

            if (_Statement == null)
            {
                _Statement = new Statement();
                _Statement.StatementID = Guid.NewGuid();
                _Statement.BatchId = BatchId;
                _Statement.PayorId = PayorId;
                _Statement.CheckAmount = PaymentRecived;
                _Statement.StatusId = 2;
                _Statement.Entries = 1;
                _Statement.CreatedDate = DateTime.Now;
                _Statement.LastModified = DateTime.Now;
                _Statement.CreatedBy = CreatedBy;
                _Statement.BalanceForOrAdjustment = null;
                _Statement.EnteredAmount = PaymentRecived;
                _Statement.StatementDate = null;
                _Statement.AddUpdate();
            }
            else
            {
                _Statement.StatusId = 1;
                _Statement.CheckAmount += PaymentRecived;
                _Statement.EnteredAmount += PaymentRecived;
                _Statement.Entries += 1;
                _Statement.LastModified = DateTime.Now;
                _Statement.AddUpdate();
            }
            return _Statement;
        }

        public static void AddUpdatePolicyHistory(Guid PolicyId)
        {
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);
            if (_PolicyPaymentEntriesPost.Count != 0) return;
            //  Dictionary<string, object> parameters = new Dictionary<string, object>();
            // parameters.Add("PolicyId", PolicyId);
            PolicyDetailsData _PolicyRecord = GetPolicyDetailsOnPolicyID(PolicyId);  //GetPolicyData(parameters).FirstOrDefault();
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.PoliciesHistories where (p.PolicyId == _PolicyRecord.PolicyId) select p).FirstOrDefault();
                if (_policy == null)
                {
                    _policy = new DLinq.PoliciesHistory
                    {
                        PolicyId = _PolicyRecord.PolicyId,
                        PolicyNumber = _PolicyRecord.PolicyNumber,
                        PolicyType = _PolicyRecord.PolicyType,
                        Insured = _PolicyRecord.Insured,
                        OriginalEffectiveDate = _PolicyRecord.OriginalEffectiveDate,
                        TrackFromDate = _PolicyRecord.TrackFromDate,
                        MonthlyPremium = _PolicyRecord.ModeAvgPremium,
                        SubmittedThrough = _PolicyRecord.SubmittedThrough,
                        Enrolled = _PolicyRecord.Enrolled,
                        Eligible = _PolicyRecord.Eligible,
                        PolicyTerminationDate = _PolicyRecord.PolicyTerminationDate,
                        IsTrackMissingMonth = _PolicyRecord.IsTrackMissingMonth,
                        IsTrackIncomingPercentage = _PolicyRecord.IsTrackIncomingPercentage,
                        IsTrackPayment = _PolicyRecord.IsTrackPayment,
                        IsDeleted = false,
                        ReplacedBy = _PolicyRecord.ReplacedBy,
                        DuplicateFrom = _PolicyRecord.DuplicateFrom,
                        CreatedOn = DateTime.Now,
                        IsIncomingBasicSchedule = _PolicyRecord.IsIncomingBasicSchedule,
                        IsOutGoingBasicSchedule = _PolicyRecord.IsOutGoingBasicSchedule,
                        SplitPercentage = _PolicyRecord.SplitPercentage,
                        Advance = _PolicyRecord.Advance,
                        ProductType = _PolicyRecord.ProductType,
                    };
                    _policy.MasterPolicyStatuReference.Value = (from m in DataModel.MasterPolicyStatus where m.PolicyStatusId == _PolicyRecord.PolicyStatusId select m).FirstOrDefault();
                    _policy.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == _PolicyRecord.ClientId select s).FirstOrDefault();
                    _policy.LicenseeReference.Value = (from l in DataModel.Licensees where l.LicenseeId == _PolicyRecord.PolicyLicenseeId select l).FirstOrDefault();
                    _policy.MasterPolicyModeReference.Value = (from m in DataModel.MasterPolicyModes where m.PolicyModeId == _PolicyRecord.PolicyModeId select m).FirstOrDefault();
                    _policy.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == _PolicyRecord.CoverageId select s).FirstOrDefault();
                    _policy.MasterPolicyTerminationReasonReference.Value = (from s in DataModel.MasterPolicyTerminationReasons where s.PTReasonId == _PolicyRecord.TerminationReasonId select s).FirstOrDefault();
                    _policy.MasterIncomingPaymentTypeReference.Value = (from m in DataModel.MasterIncomingPaymentTypes where m.IncomingPaymentTypeId == _PolicyRecord.IncomingPaymentTypeId select m).FirstOrDefault();
                    _policy.PayorReference.Value = (from m in DataModel.Payors where m.PayorId == _PolicyRecord.PayorId select m).FirstOrDefault();
                    _policy.UserCredentialReference.Value = (from s in DataModel.UserCredentials where s.UserCredentialId == _PolicyRecord.CreatedBy select s).FirstOrDefault();
                    _policy.CarrierReference.Value = (from m in DataModel.Carriers where m.CarrierId == _PolicyRecord.CarrierID select m).FirstOrDefault();
                    DataModel.AddToPoliciesHistories(_policy);

                }
                else
                {
                    _policy.PolicyId = _PolicyRecord.PolicyId;
                    _policy.PolicyNumber = _PolicyRecord.PolicyNumber;
                    _policy.PolicyType = _PolicyRecord.PolicyType;
                    _policy.Insured = _PolicyRecord.Insured;
                    _policy.OriginalEffectiveDate = _PolicyRecord.OriginalEffectiveDate;
                    _policy.TrackFromDate = _PolicyRecord.TrackFromDate;
                    _policy.MonthlyPremium = _PolicyRecord.ModeAvgPremium;
                    _policy.SubmittedThrough = _PolicyRecord.SubmittedThrough;
                    _policy.Enrolled = _PolicyRecord.Enrolled;
                    _policy.Eligible = _PolicyRecord.Eligible;
                    _policy.PolicyTerminationDate = _PolicyRecord.PolicyTerminationDate;
                    _policy.IsTrackMissingMonth = _PolicyRecord.IsTrackMissingMonth;
                    _policy.IsTrackIncomingPercentage = _PolicyRecord.IsTrackIncomingPercentage;
                    _policy.IsTrackPayment = _PolicyRecord.IsTrackPayment;
                    _policy.IsDeleted = _PolicyRecord.IsDeleted;
                    _policy.ReplacedBy = _PolicyRecord.ReplacedBy;
                    _policy.DuplicateFrom = _PolicyRecord.DuplicateFrom;
                    _policy.CreatedOn = DateTime.Now;
                    _policy.IsIncomingBasicSchedule = _PolicyRecord.IsIncomingBasicSchedule;
                    _policy.IsOutGoingBasicSchedule = _PolicyRecord.IsOutGoingBasicSchedule;
                    _policy.SplitPercentage = _PolicyRecord.SplitPercentage;

                    _policy.ProductType = _PolicyRecord.ProductType;

                    _policy.MasterPolicyStatuReference.Value = (from m in DataModel.MasterPolicyStatus where m.PolicyStatusId == _PolicyRecord.PolicyStatusId select m).FirstOrDefault();
                    _policy.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == _PolicyRecord.ClientId select s).FirstOrDefault();
                    _policy.LicenseeReference.Value = (from l in DataModel.Licensees where l.LicenseeId == _PolicyRecord.PolicyLicenseeId select l).FirstOrDefault();
                    _policy.MasterPolicyModeReference.Value = (from m in DataModel.MasterPolicyModes where m.PolicyModeId == _PolicyRecord.PolicyModeId select m).FirstOrDefault();
                    _policy.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == _PolicyRecord.CoverageId select s).FirstOrDefault();
                    _policy.MasterPolicyTerminationReasonReference.Value = (from s in DataModel.MasterPolicyTerminationReasons where s.PTReasonId == _PolicyRecord.TerminationReasonId select s).FirstOrDefault();
                    _policy.MasterIncomingPaymentTypeReference.Value = (from m in DataModel.MasterIncomingPaymentTypes where m.IncomingPaymentTypeId == _PolicyRecord.IncomingPaymentTypeId select m).FirstOrDefault();
                    _policy.PayorReference.Value = (from m in DataModel.Payors where m.PayorId == _PolicyRecord.PayorId select m).FirstOrDefault();
                    _policy.UserCredentialReference.Value = (from s in DataModel.UserCredentials where s.UserCredentialId == _PolicyRecord.CreatedBy select s).FirstOrDefault();
                    _policy.CarrierReference.Value = (from m in DataModel.Carriers where m.CarrierId == _PolicyRecord.CarrierID select m).FirstOrDefault();

                }
                DataModel.SaveChanges();
            }
        }

        public static void AddUpdatePolicyHistory(DLinq.Policy _PolicyRecord, DLinq.CommissionDepartmentEntities DataModel)
        {
            try
            {
                List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(_PolicyRecord.PolicyId);
                if (_PolicyPaymentEntriesPost.Count != 0) return;

                var _policy = (from p in DataModel.PoliciesHistories where (p.PolicyId == _PolicyRecord.PolicyId) select p).FirstOrDefault();
                if (_policy == null)
                {
                    _policy = new DLinq.PoliciesHistory
                    {
                        PolicyId = _PolicyRecord.PolicyId,
                        PolicyNumber = _PolicyRecord.PolicyNumber,
                        PolicyType = _PolicyRecord.PolicyType,
                        Insured = _PolicyRecord.Insured,
                        OriginalEffectiveDate = _PolicyRecord.OriginalEffectiveDate,
                        TrackFromDate = _PolicyRecord.TrackFromDate,
                        MonthlyPremium = _PolicyRecord.MonthlyPremium,
                        SubmittedThrough = _PolicyRecord.SubmittedThrough,
                        Enrolled = _PolicyRecord.Enrolled,
                        Eligible = _PolicyRecord.Eligible,
                        PolicyTerminationDate = _PolicyRecord.PolicyTerminationDate,
                        IsTrackMissingMonth = _PolicyRecord.IsTrackMissingMonth,
                        IsTrackIncomingPercentage = _PolicyRecord.IsTrackIncomingPercentage,
                        IsTrackPayment = _PolicyRecord.IsTrackPayment,
                        IsDeleted = false,
                        ReplacedBy = _PolicyRecord.ReplacedBy,
                        DuplicateFrom = _PolicyRecord.DuplicateFrom,
                        CreatedOn = DateTime.Now,
                        IsIncomingBasicSchedule = _PolicyRecord.IsIncomingBasicSchedule,
                        IsOutGoingBasicSchedule = _PolicyRecord.IsOutGoingBasicSchedule,
                        SplitPercentage = _PolicyRecord.SplitPercentage,
                        ProductType = _PolicyRecord.ProductType,
                    };
                    _policy.MasterPolicyStatuReference.Value = _PolicyRecord.MasterPolicyStatu;//(from m in DataModel.MasterPolicyStatus where m.PolicyStatusId == _PolicyRecord.PolicyStatusId select m).FirstOrDefault();
                    _policy.ClientReference.Value = _PolicyRecord.Client;// (from s in DataModel.Clients where s.ClientId == _PolicyRecord.ClientId select s).FirstOrDefault();
                    _policy.LicenseeReference.Value = _PolicyRecord.Licensee;// (from l in DataModel.Licensees where l.LicenseeId == _PolicyRecord.PolicyLicenseeId select l).FirstOrDefault();
                    _policy.MasterPolicyModeReference.Value = _PolicyRecord.MasterPolicyMode;// (from m in DataModel.MasterPolicyModes where m.PolicyModeId == _PolicyRecord.PolicyModeId select m).FirstOrDefault();
                    _policy.CoverageReference.Value = _PolicyRecord.Coverage;// (from s in DataModel.Coverages where s.CoverageId == _PolicyRecord.CoverageId select s).FirstOrDefault();
                    _policy.MasterPolicyTerminationReasonReference.Value = _PolicyRecord.MasterPolicyTerminationReason;// (from s in DataModel.MasterPolicyTerminationReasons where s.PTReasonId == _PolicyRecord.TerminationReasonId select s).FirstOrDefault();
                    _policy.MasterIncomingPaymentTypeReference.Value = _PolicyRecord.MasterIncomingPaymentType;// (from m in DataModel.MasterIncomingPaymentTypes where m.IncomingPaymentTypeId == _PolicyRecord.IncomingPaymentTypeId select m).FirstOrDefault();
                    _policy.PayorReference.Value = _PolicyRecord.Payor;// (from m in DataModel.Payors where m.PayorId == _PolicyRecord.PayorId select m).FirstOrDefault();
                    _policy.UserCredentialReference.Value = _PolicyRecord.UserCredential;// (from s in DataModel.UserCredentials where s.UserCredentialId == _PolicyRecord.CreatedBy select s).FirstOrDefault();
                    _policy.CarrierReference.Value = _PolicyRecord.Carrier;// (from m in DataModel.Carriers where m.CarrierId == _PolicyRecord.CarrierID select m).FirstOrDefault();
                    DataModel.AddToPoliciesHistories(_policy);

                }
                else
                {
                    _policy.PolicyId = _PolicyRecord.PolicyId;
                    _policy.PolicyNumber = _PolicyRecord.PolicyNumber;
                    _policy.PolicyType = _PolicyRecord.PolicyType;
                    _policy.Insured = _PolicyRecord.Insured;
                    _policy.OriginalEffectiveDate = _PolicyRecord.OriginalEffectiveDate;
                    _policy.TrackFromDate = _PolicyRecord.TrackFromDate;
                    _policy.MonthlyPremium = _PolicyRecord.MonthlyPremium;
                    _policy.SubmittedThrough = _PolicyRecord.SubmittedThrough;
                    _policy.Enrolled = _PolicyRecord.Enrolled;
                    _policy.Eligible = _PolicyRecord.Eligible;
                    _policy.PolicyTerminationDate = _PolicyRecord.PolicyTerminationDate;
                    _policy.IsTrackMissingMonth = _PolicyRecord.IsTrackMissingMonth;
                    _policy.IsTrackIncomingPercentage = _PolicyRecord.IsTrackIncomingPercentage;
                    _policy.IsTrackPayment = _PolicyRecord.IsTrackPayment;
                    _policy.IsDeleted = _PolicyRecord.IsDeleted;
                    _policy.ReplacedBy = _PolicyRecord.ReplacedBy;
                    _policy.DuplicateFrom = _PolicyRecord.DuplicateFrom;
                    _policy.CreatedOn = DateTime.Now;
                    _policy.IsIncomingBasicSchedule = _PolicyRecord.IsIncomingBasicSchedule;
                    _policy.IsOutGoingBasicSchedule = _PolicyRecord.IsOutGoingBasicSchedule;
                    _policy.SplitPercentage = _PolicyRecord.SplitPercentage;

                    _policy.ProductType = _PolicyRecord.ProductType;

                    _policy.MasterPolicyStatuReference.Value = _PolicyRecord.MasterPolicyStatu;//(from m in DataModel.MasterPolicyStatus where m.PolicyStatusId == _PolicyRecord.PolicyStatusId select m).FirstOrDefault();
                    _policy.ClientReference.Value = _PolicyRecord.Client;// (from s in DataModel.Clients where s.ClientId == _PolicyRecord.ClientId select s).FirstOrDefault();
                    _policy.LicenseeReference.Value = _PolicyRecord.Licensee;// (from l in DataModel.Licensees where l.LicenseeId == _PolicyRecord.PolicyLicenseeId select l).FirstOrDefault();
                    _policy.MasterPolicyModeReference.Value = _PolicyRecord.MasterPolicyMode;// (from m in DataModel.MasterPolicyModes where m.PolicyModeId == _PolicyRecord.PolicyModeId select m).FirstOrDefault();
                    _policy.CoverageReference.Value = _PolicyRecord.Coverage;// (from s in DataModel.Coverages where s.CoverageId == _PolicyRecord.CoverageId select s).FirstOrDefault();
                    _policy.MasterPolicyTerminationReasonReference.Value = _PolicyRecord.MasterPolicyTerminationReason;// (from s in DataModel.MasterPolicyTerminationReasons where s.PTReasonId == _PolicyRecord.TerminationReasonId select s).FirstOrDefault();
                    _policy.MasterIncomingPaymentTypeReference.Value = _PolicyRecord.MasterIncomingPaymentType;// (from m in DataModel.MasterIncomingPaymentTypes where m.IncomingPaymentTypeId == _PolicyRecord.IncomingPaymentTypeId select m).FirstOrDefault();
                    _policy.PayorReference.Value = _PolicyRecord.Payor;// (from m in DataModel.Payors where m.PayorId == _PolicyRecord.PayorId select m).FirstOrDefault();
                    _policy.UserCredentialReference.Value = _PolicyRecord.UserCredential;// (from s in DataModel.UserCredentials where s.UserCredentialId == _PolicyRecord.CreatedBy select s).FirstOrDefault();
                    _policy.CarrierReference.Value = _PolicyRecord.Carrier;// (from m in DataModel.Carriers where m.CarrierId == _PolicyRecord.CarrierID select m).FirstOrDefault();
                }
                DataModel.SaveChanges();

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdatePolicyHistory :" + ex.InnerException.ToString(), true);
            }
        }

        public static void AddUpdatePolicyHistoryNotCheckPayment(Guid PolicyId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PolicyId", PolicyId);
            PolicyDetailsData _PolicyRecord = GetPolicyData(parameters).FirstOrDefault();
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.PoliciesHistories where (p.PolicyId == _PolicyRecord.PolicyId) select p).FirstOrDefault();
                if (_policy == null)
                {
                    _policy = new DLinq.PoliciesHistory
                    {
                        PolicyId = _PolicyRecord.PolicyId,
                        PolicyNumber = _PolicyRecord.PolicyNumber,
                        PolicyType = _PolicyRecord.PolicyType,
                        Insured = _PolicyRecord.Insured,
                        OriginalEffectiveDate = _PolicyRecord.OriginalEffectiveDate,
                        TrackFromDate = _PolicyRecord.TrackFromDate,
                        MonthlyPremium = _PolicyRecord.ModeAvgPremium,
                        SubmittedThrough = _PolicyRecord.SubmittedThrough,
                        Enrolled = _PolicyRecord.Enrolled,
                        Eligible = _PolicyRecord.Eligible,
                        PolicyTerminationDate = _PolicyRecord.PolicyTerminationDate,
                        IsTrackMissingMonth = _PolicyRecord.IsTrackMissingMonth,
                        IsTrackIncomingPercentage = _PolicyRecord.IsTrackIncomingPercentage,
                        IsTrackPayment = _PolicyRecord.IsTrackPayment,
                        IsDeleted = false,
                        ReplacedBy = _PolicyRecord.ReplacedBy,
                        DuplicateFrom = _PolicyRecord.DuplicateFrom,
                        CreatedOn = DateTime.Now,
                        IsIncomingBasicSchedule = _PolicyRecord.IsIncomingBasicSchedule,
                        IsOutGoingBasicSchedule = _PolicyRecord.IsOutGoingBasicSchedule,
                        SplitPercentage = _PolicyRecord.SplitPercentage,
                    };
                    _policy.MasterPolicyStatuReference.Value = (from m in DataModel.MasterPolicyStatus where m.PolicyStatusId == _PolicyRecord.PolicyStatusId select m).FirstOrDefault();
                    _policy.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == _PolicyRecord.ClientId select s).FirstOrDefault();
                    _policy.LicenseeReference.Value = (from l in DataModel.Licensees where l.LicenseeId == _PolicyRecord.PolicyLicenseeId select l).FirstOrDefault();
                    _policy.MasterPolicyModeReference.Value = (from m in DataModel.MasterPolicyModes where m.PolicyModeId == _PolicyRecord.PolicyModeId select m).FirstOrDefault();
                    _policy.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == _PolicyRecord.CoverageId select s).FirstOrDefault();
                    _policy.MasterPolicyTerminationReasonReference.Value = (from s in DataModel.MasterPolicyTerminationReasons where s.PTReasonId == _PolicyRecord.TerminationReasonId select s).FirstOrDefault();
                    _policy.MasterIncomingPaymentTypeReference.Value = (from m in DataModel.MasterIncomingPaymentTypes where m.IncomingPaymentTypeId == _PolicyRecord.IncomingPaymentTypeId select m).FirstOrDefault();
                    _policy.PayorReference.Value = (from m in DataModel.Payors where m.PayorId == _PolicyRecord.PayorId select m).FirstOrDefault();
                    _policy.UserCredentialReference.Value = (from s in DataModel.UserCredentials where s.UserCredentialId == _PolicyRecord.CreatedBy select s).FirstOrDefault();
                    _policy.CarrierReference.Value = (from m in DataModel.Carriers where m.CarrierId == _PolicyRecord.CarrierID select m).FirstOrDefault();
                    DataModel.AddToPoliciesHistories(_policy);

                }
                else
                {
                    _policy.PolicyId = _PolicyRecord.PolicyId;
                    _policy.PolicyNumber = _PolicyRecord.PolicyNumber;
                    _policy.PolicyType = _PolicyRecord.PolicyType;
                    _policy.Insured = _PolicyRecord.Insured;
                    _policy.OriginalEffectiveDate = _PolicyRecord.OriginalEffectiveDate;
                    _policy.TrackFromDate = _PolicyRecord.TrackFromDate;
                    _policy.MonthlyPremium = _PolicyRecord.ModeAvgPremium;
                    _policy.SubmittedThrough = _PolicyRecord.SubmittedThrough;
                    _policy.Enrolled = _PolicyRecord.Enrolled;
                    _policy.Eligible = _PolicyRecord.Eligible;
                    _policy.PolicyTerminationDate = _PolicyRecord.PolicyTerminationDate;
                    _policy.IsTrackMissingMonth = _PolicyRecord.IsTrackMissingMonth;
                    _policy.IsTrackIncomingPercentage = _PolicyRecord.IsTrackIncomingPercentage;
                    _policy.IsTrackPayment = _PolicyRecord.IsTrackPayment;
                    _policy.IsDeleted = _PolicyRecord.IsDeleted;
                    _policy.ReplacedBy = _PolicyRecord.ReplacedBy;
                    _policy.DuplicateFrom = _PolicyRecord.DuplicateFrom;
                    _policy.CreatedOn = DateTime.Now;
                    _policy.IsIncomingBasicSchedule = _PolicyRecord.IsIncomingBasicSchedule;
                    _policy.IsOutGoingBasicSchedule = _PolicyRecord.IsOutGoingBasicSchedule;
                    _policy.SplitPercentage = _PolicyRecord.SplitPercentage;
                    _policy.MasterPolicyStatuReference.Value = (from m in DataModel.MasterPolicyStatus where m.PolicyStatusId == _PolicyRecord.PolicyStatusId select m).FirstOrDefault();
                    _policy.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == _PolicyRecord.ClientId select s).FirstOrDefault();
                    _policy.LicenseeReference.Value = (from l in DataModel.Licensees where l.LicenseeId == _PolicyRecord.PolicyLicenseeId select l).FirstOrDefault();
                    _policy.MasterPolicyModeReference.Value = (from m in DataModel.MasterPolicyModes where m.PolicyModeId == _PolicyRecord.PolicyModeId select m).FirstOrDefault();
                    _policy.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == _PolicyRecord.CoverageId select s).FirstOrDefault();
                    _policy.MasterPolicyTerminationReasonReference.Value = (from s in DataModel.MasterPolicyTerminationReasons where s.PTReasonId == _PolicyRecord.TerminationReasonId select s).FirstOrDefault();
                    _policy.MasterIncomingPaymentTypeReference.Value = (from m in DataModel.MasterIncomingPaymentTypes where m.IncomingPaymentTypeId == _PolicyRecord.IncomingPaymentTypeId select m).FirstOrDefault();
                    _policy.PayorReference.Value = (from m in DataModel.Payors where m.PayorId == _PolicyRecord.PayorId select m).FirstOrDefault();
                    _policy.UserCredentialReference.Value = (from s in DataModel.UserCredentials where s.UserCredentialId == _PolicyRecord.CreatedBy select s).FirstOrDefault();
                    _policy.CarrierReference.Value = (from m in DataModel.Carriers where m.CarrierId == _PolicyRecord.CarrierID select m).FirstOrDefault();

                }
                DataModel.SaveChanges();
            }
        }

        public static PolicyDetailsData GetPolicyHistoryIdWise(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                PolicyDetailsData _Policy = (from pl in DataModel.PoliciesHistories
                                             where (pl.IsDeleted == false) && (pl.PolicyId == PolicyId)
                                             select new PolicyDetailsData
                                             {
                                                 PolicyId = pl.PolicyId,
                                                 PolicyNumber = pl.PolicyNumber,
                                                 PolicyStatusId = pl.MasterPolicyStatu.PolicyStatusId,
                                                 PolicyStatusName = pl.MasterPolicyStatu.Name,
                                                 PolicyType = pl.PolicyType,
                                                 PolicyLicenseeId = pl.Licensee.LicenseeId,
                                                 Insured = pl.Insured,
                                                 OriginalEffectiveDate = pl.OriginalEffectiveDate,
                                                 TrackFromDate = pl.TrackFromDate,
                                                 PolicyModeId = pl.MasterPolicyMode.PolicyModeId,
                                                 ModeAvgPremium = pl.MonthlyPremium,
                                                 SubmittedThrough = pl.SubmittedThrough,
                                                 Enrolled = pl.Enrolled,
                                                 Eligible = pl.Eligible,
                                                 PolicyTerminationDate = pl.PolicyTerminationDate,
                                                 TerminationReasonId = pl.TerminationReasonId,
                                                 IsTrackMissingMonth = pl.IsTrackMissingMonth,
                                                 IsTrackIncomingPercentage = pl.IsTrackIncomingPercentage,
                                                 IsTrackPayment = pl.IsTrackPayment,
                                                 IsDeleted = pl.IsDeleted,
                                                 CarrierID = pl.Carrier.CarrierId == null ? Guid.Empty : pl.Carrier.CarrierId,
                                                 CarrierName = pl.Carrier.CarrierName,
                                                 CoverageId = pl.Coverage.CoverageId == null ? Guid.Empty : pl.Coverage.CoverageId,
                                                 CoverageName = pl.Coverage.ProductName,
                                                 ClientId = pl.Client.ClientId,
                                                 ClientName = pl.Client.Name,
                                                 ReplacedBy = pl.ReplacedBy,
                                                 DuplicateFrom = pl.DuplicateFrom,
                                                 IsIncomingBasicSchedule = pl.IsIncomingBasicSchedule,
                                                 IsOutGoingBasicSchedule = pl.IsOutGoingBasicSchedule,
                                                 PayorId = pl.Payor.PayorId == null ? Guid.Empty : pl.Payor.PayorId,
                                                 PayorName = pl.Payor.PayorName,
                                                 SplitPercentage = pl.SplitPercentage,
                                                 IncomingPaymentTypeId = pl.IncomingPaymentTypeId,
                                                 CreatedOn = pl.CreatedOn,
                                                 CreatedBy = pl.CreatedBy.Value,//--always check it will never null
                                                 IsSavedPolicy = true,

                                             }).FirstOrDefault();
                if (_Policy != null)
                {
                    _Policy.PolicyPreviousData = FillPolicyDetailPreviousData(_Policy);
                }

                return _Policy;
            }
        }

        public static void DeletePolicyHistory(PolicyDetailsData _policyrecord)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.PoliciesHistories where (p.PolicyId == _policyrecord.PolicyId) select p).FirstOrDefault();
                if (_policy == null) return;
                _policy.IsDeleted = true;
                DataModel.SaveChanges();
            }
        }

        public static void DeletePolicyHistoryPermanentById(PolicyDetailsData _Policy)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policy = (from p in DataModel.PoliciesHistories where (p.PolicyId == _Policy.PolicyId) select p).FirstOrDefault();
                if (_policy == null) return;
                DataModel.DeleteObject(_policy);
                DataModel.SaveChanges();
            }
        }

        public static PolicyDetailPreviousData FillPolicyDetailPreviousData(PolicyDetailsData _Policy)
        {
            PolicyDetailPreviousData _PolicyDetailPreviousData = new PolicyDetailPreviousData();
            try
            {
                if (_Policy != null)
                {

                    _PolicyDetailPreviousData.OriginalEffectiveDate = _Policy.OriginalEffectiveDate;
                    _PolicyDetailPreviousData.PolicyModeId = _Policy.PolicyModeId;
                    _PolicyDetailPreviousData.TrackFromDate = _Policy.TrackFromDate;
                    _PolicyDetailPreviousData.PolicyTermdateDate = _Policy.PolicyTerminationDate;
                }
            }
            catch (Exception)
            {
            }


            return _PolicyDetailPreviousData;
        }

        public static PolicyDetailMasterData GetPolicyDetailMasterData()
        {
            PolicyDetailMasterData pdMasterData = new PolicyDetailMasterData();
            pdMasterData.Statuses = PolicyStatus.GetPolicyStatusList();
            pdMasterData.Modes = PolicyMode.GetPolicyModeListWithBlankAdded();
            pdMasterData.TerminationReasons = PolicyTerminationReason.GetTerminationReasonListWithBlankAdded();
            pdMasterData.IncomingPaymentTypes = PolicyIncomingPaymentType.GetIncomingPaymentTypeList();
            pdMasterData.IssueCategories = IssueCategory.GetAllCategory();
            pdMasterData.IssueReasons = IssueReasons.GetAllReason();
            pdMasterData.IssueResults = IssueResults.GetAllResults();
            pdMasterData.IssueStatuses = IssueStatus.GetAllStatus();
            pdMasterData.LearnedMasterIncomingPaymentTypes = PolicyIncomingPaymentType.GetIncomingPaymentTypeList(); ;
            pdMasterData.LearnedMasterPaymentsModes = PolicyMode.GetPolicyModeListWithBlankAdded();
            pdMasterData.IncomingAdvanceScheduleTypes = PolicyIncomingScheduleType.GetIncomingScheduleTypeList();
            pdMasterData.OutgoingAdvanceScheduleTypes = PolicyOutgoingScheduleType.GetOutgoingScheduleTypeList();

            return pdMasterData;
        }

        #endregion
    }


}
