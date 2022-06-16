using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Transactions;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MyAgencyVault.BusinessLibrary
{
   

    public class ImportPolicyUIS 
    {
       public struct IPolicyObj
        {
            public string PolicyId;

            public string PolicyNumber;

            public int PolicyStatusId;

            public string PolicyType;

            public string PolicyClientId;

            public Guid PolicyLicenseeId;

            public string OriginalEffectiveDate;

            public string TrackFromDate;

            public string PolicyModeId;

            public string MonthlyPremium;

            public string Coverage;

            public string SubmittedThrough;

            public string Enrolled;

            public string Eligible;

            public string PolicyTerminationDate;

            public string TerminationReasonId;

            public string IsTrackMissingMonth;

            public string IsTrackIncomingPercentage;

            public Boolean IsTrackPayment;

            public string IncomingPaymentTypeId;

            public string IsDeleted;

            public string CreatedOn;

            public string IsIncomingBasicSchedule;

            public string IsOutGoingBasicSchedule;

            public string SplitPercentage;

            public string Insured;

            public string ActivatedOn;

            public string IsLocked;

            public string LastFollowUpRuns;

            public string IsManuallyChanged;

            public string RowVersion;

            public string Advance;

            public string ProductType;

            public string AccoutExec;

            public string LastNoMissIssueDate;

            public string LastNoVarIssueDate;

            public string IsCustomBasicSchedule;

            public string CustomScheduleDateType;

            public string IsTieredSchedule;

            public string LastModifiedOn;

            public string IsCreatedFromWeb;
            
            public string IslastModifiedFromWeb;

            public string Segment;

            public string ExpectedCommissions;

            public string CommissionType;

            public string Agent;

            public string importedPolicyID;

            public string ClientName;

            public string PolicyPlanID;

            public string importedID;

            public string Payor;

            public string carrier;

            public string Client;

            public string PayorId;

            public string CarrierId;

            public string FirstYearPercentage;

            public string RenewalPercentage;

            public string IncomingScheduleID;

            public string Group;

            public string PlanStatusDescription;

            public string PayorCommissionDept;

            public string TierNumber;

            public string agencyName;
            //Incoming
            public string IncomingMode;

            public string IncomingCustomModeType;

            public string PaymentType;

            public string CoBrokerSplit;

            public bool CoBrokerFlag;

            public string CommissionsFirstYear;

            public string CommissionsRenewal;

            public bool CommFirstRenwalFlag;

        }

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

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                #region Agents List
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
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Agent list fetched " + AgentList.ToStringDump(), true, agencyName);
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
                #endregion

                #region Add data to Object from table
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IPolicyObj initialPolicyData = new IPolicyObj();
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Iteration: " + i, true, agencyName);

                    Dictionary<string, string> errMsgPolicy = new Dictionary<string, string>();

                    //PolicyPlanId
                    if (dt.Columns.Contains("PolicyPlanID"))
                    {
                        initialPolicyData.PolicyPlanID = Convert.ToString(dt.Rows[i]["PolicyPlanID"]);
                    }
                    //ImportPolicyId
                    if (dt.Columns.Contains(policyIDKey))
                    {
                        initialPolicyData.importedPolicyID = dt.Columns.Contains(policyIDKey) ? Convert.ToString(dt.Rows[i][policyIDKey]).Trim() : Convert.ToString(Guid.NewGuid()).ToUpper();// Assuming new policy, new ID is generated 
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: importedPolicyID: " + initialPolicyData.importedPolicyID, true, agencyName);
                    }
                    //if importedID is null or empty
                    if (string.IsNullOrEmpty(initialPolicyData.importedPolicyID))
                    {
                        errMsgPolicy.Add("Original Plan ID", "Original plan ID found missing");
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Original plan ID found null/blank, skipping record", true, agencyName);
                        AddImportStatusToDB(initialPolicyData.importedPolicyID, false, false, initialPolicyData.PolicyPlanID, agencyName);
                        errorCount++;
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(initialPolicyData.importedID, initialPolicyData.PolicyPlanID, output);
                        errorList.Add(m);
                        continue;
                    }
                    //Insured and ClientName
                    if (dt.Columns.Contains("AccountName") || dt.Columns.Contains("Account Name"))
                    {
                        initialPolicyData.ClientName = dt.Columns.Contains("AccountName") ? Convert.ToString(dt.Rows[i]["AccountName"]) : dt.Columns.Contains("Account Name") ? Convert.ToString(dt.Rows[i]["Account Name"]) : null;
                    }
                    //check client exist
                    if (string.IsNullOrEmpty(initialPolicyData.ClientName))
                    {
                        errMsgPolicy.Add("AccountName", "Account name found missing");
                        string output = Newtonsoft.Json.JsonConvert.SerializeObject(errMsgPolicy);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Account name found null/blank, skipping record", true, agencyName);
                        AddImportStatusToDB(initialPolicyData.importedPolicyID, false, false, initialPolicyData.PolicyPlanID, agencyName);
                        errorCount++;
                        Benefits_ErrorMsg m = new Benefits_ErrorMsg(initialPolicyData.importedID, initialPolicyData.PolicyPlanID, output);
                        errorList.Add(m);
                        continue;
                    }
                    initialPolicyData.Insured = initialPolicyData.ClientName;
                    //Status
                    if (dt.Columns.Contains("Status__c") && !String.IsNullOrEmpty(Convert.ToString("Status__c"))) //read according to existing method??
                    {
                        try
                        {
                            string strString = Convert.ToString(dt.Rows[i]["Status__c"]);
                            initialPolicyData.PolicyStatusId = (strString.ToLower() == "active") ? 0 : (strString.ToLower() == "pending") ? 2 : 1;
                        }
                        catch (Exception ex)
                        {
                            errMsgPolicy.Add("PlanStatusDescription", ex.Message);
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: PlanStatusDescription fields  : " + ex.Message, true);
                            AddImportStatusToDB(initialPolicyData.importedPolicyID, false, false, initialPolicyData.PolicyPlanID, agencyName);
                        }
                    }
                    //Policy#
                    if (dt.Columns.Contains("Policy_Number__c"))
                    {
                        initialPolicyData.PolicyNumber = Convert.ToString(dt.Rows[i]["Policy_Number__c"]);
                    }
                    //Original Effective Date
                    if (dt.Columns.Contains("StartDate__c"))
                    {
                        initialPolicyData.OriginalEffectiveDate = Convert.ToString(dt.Rows[i]["StartDate__c"]);
                    }
                    // Mode  
                    if (dt.Columns.Contains("ModalNumber"))
                    {
                        initialPolicyData.PolicyModeId = Convert.ToString(dt.Rows[i]["ModalNumber"]);
                    }
                    //Monthly Premium
                    if (dt.Columns.Contains("MonthlyPremium__c"))
                    {
                        initialPolicyData.MonthlyPremium = Convert.ToString(dt.Rows[i]["MonthlyPremium__c"]);
                    }
                    //Payor
                    if (dt.Columns.Contains("PayorCommissionDept"))
                    {
                        initialPolicyData.Payor = Convert.ToString(dt.Rows[i]["PayorCommissionDept"]);
                    }
                    //Carrier
                    if (dt.Columns.Contains("CarrierCommissionDept"))
                    {
                        initialPolicyData.carrier = Convert.ToString(dt.Rows[i]["CarrierCommissionDept"]);
                    }
                    //Policy Term Date
                    if (dt.Columns.Contains("Termination_Date__c"))
                    {
                        initialPolicyData.PolicyTerminationDate = Convert.ToString(dt.Rows[i]["Termination_Date__c"]);
                    }
                    //Term Reason
                    if (dt.Columns.Contains("TerminationReason"))
                    {
                        string strTermReason = Convert.ToString(dt.Rows[i]["TerminationReason"]);
                        initialPolicyData.TerminationReasonId = Convert.ToString(PolicTermisionID(strTermReason));
                    }
                    //Submitted Through
                    if (dt.Columns.Contains("Submitted Through"))
                    {
                        initialPolicyData.SubmittedThrough = Convert.ToString(dt.Rows[i]["Submitted Through"]);
                    }
                    //Enrolled
                    if (dt.Columns.Contains("CurrentEnrolled__c"))
                    {
                        initialPolicyData.Enrolled = Convert.ToString(dt.Rows[i]["CurrentEnrolled__c"]);
                    }
                    //Account Exec
                    if (dt.Columns.Contains("AccountOwnerName"))
                    {
                        initialPolicyData.AccoutExec = Convert.ToString(dt.Rows[i]["AccountOwnerName"]);
                    }
                    //isTrackPayment
                    initialPolicyData.IsTrackPayment = false;
                    if (dt.Columns.Contains("IsTrackPayment"))
                    {
                        initialPolicyData.IsTrackPayment = Convert.ToBoolean(dt.Rows[i]["IsTrackPayment"]);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy - isTrackPayment found in request  : " + initialPolicyData.IsTrackPayment, true, agencyName);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(LicID)))
                    {
                        initialPolicyData.PolicyLicenseeId = LicID;
                    }
                    if (agencyName != null)
                    {
                        initialPolicyData.agencyName = agencyName;
                    }
                    if (dt.Columns.Contains("CoverageType__c"))
                    {
                        initialPolicyData.Coverage = Convert.ToString(dt.Rows[i]["CoverageType__c"]);
                    }
                    if (dt.Columns.Contains("Business_Segment__c"))
                    {
                        initialPolicyData.Segment = Convert.ToString(dt.Rows[i]["Business_Segment__c"]);
                    }
                    if (dt.Columns.Contains("GeneralAgentLU__c"))
                    {
                        initialPolicyData.SubmittedThrough = Convert.ToString(dt.Rows[i]["GeneralAgentLU__c"]);
                    }
                    //Incoming Schedule
                    /*
                    if (dt.Columns.Contains("Commission Type") || dt.Columns.Contains("CommissionType"))
                    {
                        initialPolicyData.CommissionType = dt.Columns.Contains("Commission Type") ? Convert.ToString(dt.Rows[i]["Commission Type"]) : dt.Columns.Contains("CommissionType") ? Convert.ToString(dt.Rows[i]["CommissionType"]) : null;
                    }
                    if (dt.Columns.Contains("IncomingMode"))
                    {
                        initialPolicyData.IncomingMode = Convert.ToString(dt.Rows[i]["IncomingMode"]);
                    }
                    if (dt.Columns.Contains("IncomingCustomModeType"))
                    {
                        initialPolicyData.IncomingCustomModeType = Convert.ToString(dt.Rows[i]["IncomingCustomModeType"]);
                    }
                    if (dt.Columns.Contains("PaymentType"))
                    {
                        initialPolicyData.PaymentType = Convert.ToString(dt.Rows[i]["PaymentType"]);
                    }
                    if (dt.Columns.Contains("CoBrokerSplit"))
                    {
                        initialPolicyData.CoBrokerSplit = Convert.ToString(dt.Rows[i]["CoBrokerSplit"]);
                        initialPolicyData.CoBrokerFlag = true;
                    }
                    if ((dt.Columns.Contains("Commissions - First Year %") || dt.Columns.Contains("CommissionsFirstYear")) && ((dt.Columns.Contains("Commissions - Renewal %") || dt.Columns.Contains("CommissionsRenewal"))))
                    {
                        initialPolicyData.CommissionsFirstYear = dt.Columns.Contains("Commissions - First Year %") ? Convert.ToString(dt.Rows[i]["Commissions - First Year %"]) : dt.Columns.Contains("CommissionsFirstYear") ? Convert.ToString(dt.Rows[i]["CommissionsFirstYear"]) : "0";
                        initialPolicyData.CommissionsRenewal = dt.Columns.Contains("Commissions - Renewal %") ? Convert.ToString(dt.Rows[i]["Commissions - Renewal %"]) : dt.Columns.Contains("CommissionsRenewal") ? Convert.ToString(dt.Rows[i]["CommissionsRenewal"]) : "0";
                        initialPolicyData.CommFirstRenwalFlag = true;
                    }
                    */

                    #endregion

                    //validate data
                    // AddUpdatePoicy_UIS(initialPolicyData, dt, ref i, errMsgPolicy, errorList, ref errorCount, LicID, CompTypeList, ref agencyName);
                    AddUpdatePoicy_UIS(initialPolicyData, errMsgPolicy, errorList, ref errorCount, CompTypeList, AgentList);
                }
            }

            return null;
        }

        public static Benefits_PolicyImportStatus AddUpdatePoicy_UIS(IPolicyObj initialPolicyData, Dictionary<string, string> errMsgPolicy, List<Benefits_ErrorMsg> errorList, ref int errorCount, ObservableCollection<CompType> CompTypeList, IEnumerable<dynamic> AgentList)
        {
            DLinq.Policy objPolicy = new DLinq.Policy();
            PolicyToolIncommingShedule inSchedule = null;

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                //Check policy Exist or not
                bool isNewPolicy = false;
                Guid policyID = Guid.Empty;
                bool isExisting = false;

                policyID = Policy.IsPolicyExistingWithImportID(initialPolicyData.importedPolicyID, initialPolicyData.PolicyLicenseeId, initialPolicyData.agencyName);
                if (policyID == Guid.Empty)
                {
                    isExisting = false;
                    policyID = Guid.NewGuid();
                }
                else
                {
                    isExisting = true;
                }

                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: policyID: " + initialPolicyData.importedPolicyID + ", benefits ID: " + initialPolicyData.PolicyPlanID, true, initialPolicyData.agencyName);
                if (isExisting)
                {
                    objPolicy = (from p in DataModel.Policies where p.PolicyId == policyID select p).FirstOrDefault();
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Existing policy", true, initialPolicyData.agencyName);

                    //check if client exists or deleted in the system
                    //if deleted, then add the given as new 
                    if (objPolicy.Client != null && Convert.ToBoolean(objPolicy.Client.IsDeleted))
                    {
                        objPolicy.PolicyClientId = null;
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy: Client found deleted , so setting as null ", true, initialPolicyData.agencyName);
                    }
                }
                else
                {
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy: New policy", true, initialPolicyData.agencyName);
                    isNewPolicy = true;
                    objPolicy.PolicyId = policyID;

                    objPolicy.PolicyLicenseeId = initialPolicyData.PolicyLicenseeId;
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

                ActionLogger.Logger.WriteImportPolicyLog("Import Policy isTrackPayment : " + initialPolicyData.IsTrackPayment, true, initialPolicyData.agencyName);
                objPolicy.IsTrackPayment = initialPolicyData.IsTrackPayment;

                UpdateBenefitsOptionalFields(initialPolicyData, isNewPolicy, ref objPolicy, ref errMsgPolicy);

                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: optional fields , to be filled when blanks init done : " + initialPolicyData.importedPolicyID, true, initialPolicyData.agencyName);
                #endregion

                #region Payor
                //Updated only if CD is blank
                if (isNewPolicy || objPolicy.PayorId == null)
                {
                    DLinq.Payor py = UpdatePayorForBG(isNewPolicy, DataModel, ref errMsgPolicy,initialPolicyData, objPolicy);
                    if (py != null)
                    {
                        objPolicy.PayorId = py.PayorId;
                        objPolicy.PayorReference.Value = py;
                    }
                }
                #endregion

                #region Carrier 
                //Updated only if CD is blank
                if (isNewPolicy || objPolicy.CarrierId == null)
                {
                    DLinq.Carrier carrer = UpdateCarrierForBG(initialPolicyData,isNewPolicy,DataModel, ref errMsgPolicy);
                    if (carrer != null)
                    {
                        objPolicy.CarrierId = carrer.CarrierId;
                        objPolicy.CarrierReference.Value = carrer;
                    }
                }
                #endregion

                #region product
                if (isNewPolicy || objPolicy.CoverageId == null)
                {
                    UpdateCoverageForBG(initialPolicyData, isNewPolicy, objPolicy, DataModel, ref errMsgPolicy);
                }
                #endregion

                #region Business Segment - In Pending
                if (!String.IsNullOrEmpty(initialPolicyData.Segment))
                {

                }
                #endregion

                #region Product Type - Not to be sent by UIS Insurance
                #endregion

                #region Submitted Through
                if (!String.IsNullOrEmpty(initialPolicyData.SubmittedThrough))
                {
                    objPolicy.SubmittedThrough = Convert.ToString(initialPolicyData.SubmittedThrough);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy adding Submitted Through : " + objPolicy.SubmittedThrough, true, initialPolicyData.agencyName);
                }
                #endregion

                #region Incoming schedule

                bool allowImportedSchedule = true;
                Guid SettingsScheduleID = Guid.Empty;
                bool oldInScheduleExists = false;

                if (!isNewPolicy)
                {
                    PolicyToolIncommingShedule oldSchedule = PolicyToolIncommingShedule.GettingPolicyIncomingSchedule(objPolicy.PolicyId, initialPolicyData.agencyName);
                    if (oldSchedule != null && oldSchedule.IncomingScheduleID != Guid.Empty)
                    {
                        if (
                            (oldSchedule.Mode == Mode.Standard && (oldSchedule.FirstYearPercentage != 0 || oldSchedule.RenewalPercentage != 0))
                            || oldSchedule.Mode == Mode.Custom //assuming this will always have value
                           )
                        {
                            oldInScheduleExists = true;
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy - Old policy and old incoming schedule exists with non-zero values: ", true, initialPolicyData.agencyName);
                        }
                    }
                }


                int? incomingPaymentType = 1;
                if (!String.IsNullOrEmpty(initialPolicyData.CommissionType))
                {
                    string strCommisionType = Convert.ToString(initialPolicyData.CommissionType);
                    incomingPaymentType = PolicCompType(strCommisionType, CompTypeList, initialPolicyData.agencyName);
                }
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy incomingPaymentType: " + incomingPaymentType, true, initialPolicyData.agencyName);
                PayorIncomingSchedule payorSchedule = null;

                //Configure incoming schedule only when not existing or new policy
                if (isNewPolicy || !oldInScheduleExists)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy - incoming schedule to be read: ", true, initialPolicyData.agencyName);
                    //objPolicy.IncomingPaymentTypeId = incomingPaymentType;
                    //objPolicy.SplitPercentage = 100;

                    inSchedule = new PolicyToolIncommingShedule();
                    inSchedule.PolicyId = objPolicy.PolicyId;
                    inSchedule.IncomingScheduleID = Guid.NewGuid();
                    inSchedule.CustomType = CustomMode.Graded;

                    //Set default value
                    AttachedDefaulPolicyFields(ref inSchedule, initialPolicyData,ref objPolicy,ref errMsgPolicy, isNewPolicy);

                }


                #endregion

                #region Commn fields - alwasys update with insert/Update

                bool OutPercentOfPremium = false;

                //Advance - Moved to update always
                AddUpdateMandatoryFields(initialPolicyData, isNewPolicy, ref objPolicy, ref errMsgPolicy);

                //Account Exec
                if (!String.IsNullOrEmpty(initialPolicyData.AccoutExec))
                {
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy AccountOwner Name found ", true, initialPolicyData.agencyName);
                    string AccoutExec = "";
                    Guid UserCredentialId;
                    string errMsg = "";
                    //return AccoutExec detail
                    getAccountOwner(AgentList,initialPolicyData , out AccoutExec, out UserCredentialId,out errMsg);

                    if (String.IsNullOrEmpty(errMsg))
                    {
                        if (!String.IsNullOrEmpty(AccoutExec) && UserCredentialId != null)
                        {
                            objPolicy.AccoutExec = AccoutExec;
                            objPolicy.UserCredentialId = UserCredentialId;
                        }
                        else
                        {
                            errMsgPolicy.Add("AccountOwnerName", "AccountOwner not available");
                            ActionLogger.Logger.WriteImportPolicyLog("Account owner NOT found in system by name : ", true, initialPolicyData.agencyName);
                            AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                        }
                    }
                    else
                    {
                        errMsgPolicy.Add("AccountOwnerName", errMsg);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: AccountOwnerName  fields  : " + errMsg, true, initialPolicyData.agencyName);
                        AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                    }
                }

                ActionLogger.Logger.WriteImportPolicyLog("Import Policy: mandatory fields init done : " + initialPolicyData.importedPolicyID, true, initialPolicyData.agencyName);
                #endregion
            }

            return null;
        }

        static void AddImportStatusToDB(string policyID, bool isNew, bool isSuccess, string benefitsID, string agencyName = "")//find1
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddImportStatusToDB request: policyID -  " + policyID + ", isNew - " + isNew + ", isSuccess - " + isSuccess, true, agencyName);
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

        static void UpdateBenefitsOptionalFields(IPolicyObj initialPolicyData, bool isNewPolicy, ref DLinq.Policy objPolicy, ref Dictionary<string, string> errMsgPolicy)
        {
            //PolicyNumber
            if (isNewPolicy || string.IsNullOrEmpty(objPolicy.PolicyNumber))
            {
                try
                {
                    //if (dt.Columns.Contains("Policy_Number__c"))
                    //{
                    //    objPolicy.PolicyNumber = Convert.ToString(dt.Rows[rowIndex]["Policy_Number__c"]);
                    //}
                    if (initialPolicyData.PolicyNumber != null && !String.IsNullOrEmpty(initialPolicyData.PolicyNumber))
                    {
                        objPolicy.PolicyNumber = Convert.ToString(initialPolicyData.PolicyNumber);
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("Policy_Number__c", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: Policy_Number__c fields exception : " + ex.Message, true, initialPolicyData.agencyName);
                    AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                }
            }
            //Enrolled
            if (initialPolicyData.Enrolled != null && !String.IsNullOrEmpty(initialPolicyData.Enrolled))
            {
                try
                {
                    //if (dt.Columns.Contains("CurrentEnrolled__c"))
                    //{
                    //    objPolicy.Enrolled = Convert.ToString(dt.Rows[rowIndex]["CurrentEnrolled__c"]);
                    //}
                    objPolicy.Enrolled = Convert.ToString(initialPolicyData.Enrolled);
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("CurrentEnrolled__c", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: CurrentEnrolled__c fields exception : " + ex.Message, true, initialPolicyData.agencyName);
                    AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                }
            }
            //Eligible
            if (initialPolicyData.Eligible != null && !String.IsNullOrEmpty(initialPolicyData.Eligible))
            {
                try
                {
                    //objPolicy.Eligible = Convert.ToString(dt.Rows[rowIndex]["NumberofTotalEmployees__c"]);
                    objPolicy.Eligible = Convert.ToString(initialPolicyData.Eligible);
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("NumberofTotalEmployees__c", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: NumberofTotalEmployees__c fields exception : " + ex.Message, true, initialPolicyData.agencyName);
                    AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                }
            }
            //PolicyModeId
            if (isNewPolicy || String.IsNullOrEmpty(Convert.ToString(objPolicy.PolicyModeId)))
            {
                try
                {
                    //if (dt.Columns.Contains("ModalNumber"))
                    //{
                    //    string strMode = Convert.ToString(dt.Rows[rowIndex]["ModalNumber"]);
                    //    objPolicy.PolicyModeId = (!string.IsNullOrEmpty(strMode)) ? PolicyModeID(strMode) : PolicyModeID("0");
                    //}

                    string strMode = Convert.ToString(initialPolicyData.PolicyModeId);
                    objPolicy.PolicyModeId = (!string.IsNullOrEmpty(strMode)) ? PolicyModeID(strMode) : PolicyModeID("0");
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("ModalNumber", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: ModalNumber fields exception : " + ex.Message, true, initialPolicyData.agencyName);
                    AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                }
            }

            //premium
            if (!String.IsNullOrEmpty(initialPolicyData.MonthlyPremium))
            {
                try
                {
                    //string strPremiuum = Convert.ToString(dt.Rows[rowIndex]["MonthlyPremium__c"]);
                    string strPremiuum = initialPolicyData.MonthlyPremium;
                    decimal prem = 0;
                    decimal.TryParse(strPremiuum, out prem);
                    if (prem == 0)
                    {
                        decimal.TryParse(strPremiuum, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out prem);
                    }
                    objPolicy.MonthlyPremium = prem;
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("MonthlyPremium__c", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: MonthlyPremium__c fields exception : " + ex.Message, true, initialPolicyData.agencyName);
                    AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                }
            }
            //Original effective date
            if (!String.IsNullOrEmpty(initialPolicyData.OriginalEffectiveDate)) // discuss to ma'am
            {
                try
                {
                    // string effDate = Convert.ToString(dt.Rows[rowIndex]["StartDate__c"]);
                    string effDate = initialPolicyData.OriginalEffectiveDate;
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy string effDate: " + effDate, true, initialPolicyData.agencyName);
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
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy datetime effDate: " + objPolicy.OriginalEffectiveDate, true, initialPolicyData.agencyName);
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("StartDate__c", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: StartDate__c  fields  : " + ex.Message, true, initialPolicyData.agencyName);
                    AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                }
            }
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

        //Pyors
        static DLinq.Payor UpdatePayorForBG(bool isNewPolicy, DLinq.CommissionDepartmentEntities DataModel,ref Dictionary<string, string> errMsgPolicy, IPolicyObj initialPolicyData, DLinq.Policy objPolicy)
        {
            DLinq.Payor pyor = null;
            if (isNewPolicy || String.IsNullOrEmpty(Convert.ToString(objPolicy.PayorId)))
            {
                try
                {
                    string strPayor = Convert.ToString(initialPolicyData.Payor);
                    if (!string.IsNullOrEmpty(strPayor))
                    {
                       pyor = getPayorsForBg(strPayor, initialPolicyData, DataModel,ref pyor, isNewPolicy,ref errMsgPolicy);
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("PayorCommissionDept", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: payor fields exception : " + ex.Message, true, initialPolicyData.agencyName);
                    AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                }
            }
            return pyor;
        }

        static DLinq.Payor getPayorsForBg (string strPayor, IPolicyObj initialPolicyData, DLinq.CommissionDepartmentEntities DataModel, ref DLinq.Payor pyor, bool isNewPolicy, ref Dictionary<string, string> errMsgPolicy)
        {
            DLinq.Payor py = (from p in DataModel.Payors
                              where ((p.PayorName.ToLower() == strPayor.ToLower() || (p.PayorName.ToLower() != strPayor.ToLower() && p.NickName.ToLower() == strPayor.ToLower())))
                                      && ((p.IsGlobal || (!p.IsGlobal && p.LicenseeId == initialPolicyData.PolicyLicenseeId)))
                              select p).FirstOrDefault();

            if (py != null) //If found existing 
            {
                pyor = py;
            }
            else // if not existing, return error 
            {
                errMsgPolicy.Add("PayorCommissionDept", "Payor not available");
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: payor fields exception : Payor not available", true, initialPolicyData.agencyName);
                AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
            }

            return pyor;
        }

        //Carrier
        static DLinq.Carrier UpdateCarrierForBG(IPolicyObj initialPolicyData, bool isNewPolicy, DLinq.CommissionDepartmentEntities DataModel,ref Dictionary<string, string> errMsgPolicy)
        {
            DLinq.Carrier carrer = null;
            if (isNewPolicy || String.IsNullOrEmpty(initialPolicyData.carrier))
            {
                try
                {
                    string strCarr = Convert.ToString(initialPolicyData.carrier);
                    if (!string.IsNullOrEmpty(strCarr))
                    {
                       carrer = getCarrierForBg(initialPolicyData,isNewPolicy,DataModel,ref errMsgPolicy,strCarr,ref carrer);
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("CarrierCommissionDept", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: carrier fields exception : " + ex.Message, true, initialPolicyData.agencyName);
                    AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                }
            }
            return carrer;
        }

        static DLinq.Carrier getCarrierForBg(IPolicyObj initialPolicyData,bool isNewPolicy, DLinq.CommissionDepartmentEntities DataModel, ref Dictionary<string, string> errMsgPolicy, string strCarr,ref DLinq.Carrier carrer)
        {
            DLinq.Carrier cr = (from p in DataModel.Carriers
                                where (p.CarrierName.ToLower() == strCarr.ToLower() && (p.IsGlobal || (!p.IsGlobal && p.LicenseeId == initialPolicyData.PolicyLicenseeId)))
                                select p).FirstOrDefault();
            if (cr == null)
            {
                DLinq.CarrierNickName crN = (from p in DataModel.CarrierNickNames where p.NickName == strCarr select p).FirstOrDefault();
                if (crN != null)
                {
                    carrer = crN.CarrierReference.Value;
                }
                else
                {
                    errMsgPolicy.Add("CarrierCommissionDept", "Carrier not available");
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: carrier fields exception : carrier not available", true, initialPolicyData.agencyName);
                    AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                }
            }
            else
            {
                carrer = cr;
            }
            return carrer;
        }

        //Coverage
        static void UpdateCoverageForBG(IPolicyObj initialPolicyData, bool isNewPolicy, DLinq.Policy objPolicy, DLinq.CommissionDepartmentEntities DataModel, ref Dictionary<string, string> errMsgPolicy)
        {
            if (isNewPolicy || (objPolicy.CoverageId == null || String.IsNullOrEmpty(Convert.ToString(objPolicy.CoverageId))))
            {
                try
                {
                    string strProduct = Convert.ToString(initialPolicyData.Coverage);
                    if (!string.IsNullOrEmpty(strProduct))
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy adding LineOfCoverage : " + strProduct, true, initialPolicyData.agencyName);
                        DLinq.Coverage cov = (from p in DataModel.Coverages where p.ProductName == strProduct select p).FirstOrDefault();
                        if (cov != null) // When product found
                        {
                            objPolicy.CoverageId = cov.CoverageId;
                            objPolicy.CoverageReference.Value = cov;
                        }
                        else
                        {
                            errMsgPolicy.Add("CoverageType__c", "Coverage not available");
                            ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: Coverage fields exception : Coverage  not available", true, initialPolicyData.agencyName);
                            AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    errMsgPolicy.Add("CoverageType__c", ex.Message);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: CoverageType__c : " + ex.Message, true, initialPolicyData.agencyName);
                    AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                }
            }
        }
        // Pending - segment
        static void UpdateSegmentForBG(IPolicyObj initialPolicyData, bool isNewPolicy, DLinq.Policy objPolicy, DLinq.CommissionDepartmentEntities DataModel, ref Dictionary<string, string> errMsgPolicy)
        {
            if (!String.IsNullOrEmpty(initialPolicyData.Segment))
            {
                try
                {
                    string strSegment = Convert.ToString(initialPolicyData.Segment);
                    ActionLogger.Logger.WriteImportPolicyLog("Import Policy adding Segment : " + strSegment, true, initialPolicyData.agencyName);
                   // var cov = (from p in DataModel where p.ProductName == strSegment select p).FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
            }
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

        static void AddUpdateMandatoryFields (IPolicyObj initialPolicyData, bool isNewPolicy, ref DLinq.Policy objPolicy, ref Dictionary<string, string> errMsgPolicy)
        {
            //Status
            objPolicy.PolicyStatusId = initialPolicyData.PolicyStatusId;

            //Term Date
            if (initialPolicyData.PolicyStatusId == 1)
            {
                if (!String.IsNullOrEmpty(initialPolicyData.PolicyTerminationDate))
                {
                    try
                    {
                        string termDate = initialPolicyData.PolicyTerminationDate;
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy string termDate: " + termDate, true, initialPolicyData.agencyName);
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
                    catch (Exception ex)
                    {
                        errMsgPolicy.Add("PlanEndDate", ex.Message);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: PlanEndDate  fields  : " + ex.Message, true, initialPolicyData.agencyName);
                        AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                    }
                }
                //term reason
                if (initialPolicyData.TerminationReasonId != null)
                {
                    try
                    {
                        objPolicy.TerminationReasonId = Convert.ToInt32(initialPolicyData.TerminationReasonId);
                    }
                    catch (Exception ex)
                    {
                        errMsgPolicy.Add("TerminationReason", ex.Message);
                        ActionLogger.Logger.WriteImportPolicyLog("Import Policy exception: TerminationReason  fields  : " + ex.Message, true, initialPolicyData.agencyName);
                        AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
                    }
                }
            }
        }
        //Account Exec
        static void getAccountOwner(IEnumerable<dynamic> AgentList,IPolicyObj initialPolicyData, out string AccoutExec, out Guid UserCredentialId, out string errMsg)
        {
            AccoutExec = "";
            UserCredentialId = Guid.Empty;
            errMsg = "";
            try
            {
                if (AgentList != null)
                {
                    var objUser = AgentList.FirstOrDefault(d => (d.FirstName + " " + d.LastName).ToLower() == initialPolicyData.AccoutExec.ToLower() || (!string.IsNullOrEmpty(d.NickName)
                                                               && d.NickName.ToLower() == initialPolicyData.AccoutExec.ToLower()));
                    if (objUser != null)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Account owner found in system", true, initialPolicyData.agencyName);
                        //Need to get nick name
                        if (!String.IsNullOrEmpty(objUser.NickName))
                        {
                            AccoutExec = objUser.NickName;
                            UserCredentialId = Convert.ToString(objUser.UserCredentialId); //tempGuid;
                        }
                        else
                        {
                            AccoutExec = objUser.UserName;
                            UserCredentialId = objUser.UserCredentialId; //tempGuid;
                        }
                        bool isexec = (new User().CheckAccoutExec(objUser.UserCredentialId, initialPolicyData.agencyName));
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }

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


        static void AttachedDefaulPolicyFields (ref PolicyToolIncommingShedule inSchedule, IPolicyObj initialPolicyData,ref DLinq.Policy objPolicy, ref Dictionary<string, string> errMsgPolicy, bool isNewPolicy)
        {
            try
            {
                objPolicy.IncomingPaymentTypeId = 1;
                objPolicy.SplitPercentage = 100;

                //Mode
                int mod = 0;
                inSchedule.Mode = (Mode)mod;

                //Custom Type
                int CustomMod = 1;
                inSchedule.CustomType = (CustomMode)CustomMod;

                inSchedule.FirstYearPercentage = 0;
                inSchedule.RenewalPercentage = 0;

                //ScheduleTypeId
                inSchedule.ScheduleTypeId = 1;

                //Policy Type
                objPolicy.PolicyType = "New";

                //Track From Date
                string effDate = initialPolicyData.OriginalEffectiveDate;
                double dblEff = 0;
                Double.TryParse(effDate, out dblEff);
                if (dblEff > 0)
                {
                    objPolicy.TrackFromDate = DateTime.FromOADate(dblEff);
                }
                else if (!string.IsNullOrEmpty(effDate))
                {
                    objPolicy.TrackFromDate = DateTime.Parse(effDate, System.Globalization.CultureInfo.CurrentCulture); //Convert.ToDateTime(effDate);
                }

                //Produt Type
                objPolicy.ProductType = "";
            }
            catch (Exception ex)
            {
                errMsgPolicy.Add("NewBusiness", ex.Message);
                ActionLogger.Logger.WriteImportPolicyLog("Import Policy Exception: New Business field  exception : " + ex.Message, true);
                AddImportStatusToDB(initialPolicyData.importedPolicyID, isNewPolicy, false, initialPolicyData.PolicyPlanID, initialPolicyData.agencyName);
            }
        }
    }
}