using System;
using System.Collections.Generic;
using System.Linq;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Transactions;
using System.Globalization;
using System.Linq.Expressions;
using MyAgencyVault.BusinessLibrary.Base;
using System.Reflection;
using System.Data.EntityClient;
using System.Data.SqlClient;


namespace MyAgencyVault.BusinessLibrary.TempCollection
{
    [DataContract]
    public class PostProcessReturnStatus
    {
        [DataMember]
        public Guid DeuEntryId { get; set; }
        [DataMember]
        public bool IsComplete { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public PostEntryProcess PostEntryStatus { get; set; }
        [DataMember]
        public ModifiyableBatchStatementData BatchStatementData { get; set; }
    }
    [DataContract]
    public enum DataType
    {
        [EnumMember]
        Date = 1,
        [EnumMember]
        Numeric = 2,
        [EnumMember]
        Text = 3
    }

    public class BasicInformationForProcess
    {
        public PostStatus PostStatus { get; set; }
        public bool IsPaymentToHO { get; set; }
        public Guid PolicyId { get; set; }

    }

    [DataContract]
    public class DEUFields
    {
        [DataMember]
        public Guid DeuEntryId { get; set; }
        [DataMember]
        public List<DataEntryField> DeuFieldDataCollection { get; set; }//it is not used by PostProcess.it is only for DEU table datasave from DEU.
        [DataMember]
        public List<DeuSearchedPolicy> SearchedPolicyList { get; set; }
        [DataMember]
        public DeuSearchedPolicy SelectedPolicy { get; set; }
        [DataMember]
        public Guid? PayorId { get; set; }
        [DataMember]
        public Guid BatchId { get; set; }
        [DataMember]
        public Guid? LicenseeId { get; set; }
        [DataMember]
        public Guid StatementId { get; set; }
        [DataMember]
        public Guid CurrentUser { get; set; }//it is the ID of DEU.
        [DataMember]
        public DEU DeuData { get; set; }
    }

    [DataContract]
    public class UniqueIdenitfier
    {
        [DataMember]
        public string ColumnName { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string MaskedText { get; set; }

    }

    public class LockStatus
    {

    }
    //deuFields have searchedPolicy
    public class PostUtill
    {
        public static Dictionary<Guid, object> LockDic = new Dictionary<Guid, object>();
        public static DEU GetDeuCollection(PolicyPaymentEntriesPost PolicySelectedIncomingPaymentCommissionDashBoard, PolicyDetailsData SelectedPolicy)
        {
            DEU _DEU = new DEU();

            _DEU.DEUENtryID = Guid.NewGuid();
            _DEU.OriginalEffectiveDate = SelectedPolicy.OriginalEffectiveDate;
            _DEU.PaymentRecived = PolicySelectedIncomingPaymentCommissionDashBoard.PaymentRecived;
            _DEU.CommissionPercentage = PolicySelectedIncomingPaymentCommissionDashBoard.CommissionPercentage;
            _DEU.Insured = SelectedPolicy.Insured;
            _DEU.PolicyNumber = SelectedPolicy.PolicyNumber;
            _DEU.Enrolled = SelectedPolicy.Enrolled;
            _DEU.Eligible = SelectedPolicy.Eligible;
            _DEU.SplitPer = PolicySelectedIncomingPaymentCommissionDashBoard.SplitPer;
            _DEU.PolicyMode = SelectedPolicy.PolicyModeId;
            _DEU.TrackFromDate = SelectedPolicy.TrackFromDate.Value;
            //_DEU.CompScheduleTypeID=SelectedPolicy
            _DEU.CompTypeID = SelectedPolicy.IncomingPaymentTypeId;
            _DEU.ClientID = SelectedPolicy.ClientId ?? Guid.Empty;
            _DEU.StmtID = PolicySelectedIncomingPaymentCommissionDashBoard.StmtID;
            //_DEU.PostStatusID=
            _DEU.PolicyId = SelectedPolicy.PolicyId;
            _DEU.InvoiceDate = PolicySelectedIncomingPaymentCommissionDashBoard.InvoiceDate;
            _DEU.PayorId = SelectedPolicy.PayorId;
            _DEU.NoOfUnits = PolicySelectedIncomingPaymentCommissionDashBoard.NumberOfUnits;
            _DEU.DollerPerUnit = PolicySelectedIncomingPaymentCommissionDashBoard.DollerPerUnit;
            _DEU.Fee = PolicySelectedIncomingPaymentCommissionDashBoard.Fee;
            _DEU.Bonus = PolicySelectedIncomingPaymentCommissionDashBoard.Bonus;
            _DEU.CommissionTotal = PolicySelectedIncomingPaymentCommissionDashBoard.TotalPayment;
            _DEU.CarrierID = SelectedPolicy.CarrierID;
            _DEU.CoverageID = SelectedPolicy.CoverageId;
            _DEU.IsEntrybyCommissiondashBoard = true;
            _DEU.CreatedBy = PolicySelectedIncomingPaymentCommissionDashBoard.CreatedBy;
            _DEU.PostCompleteStatus = 0;
            return _DEU;

        }


        //public Dictionary<Guid> 
        /// <summary>
        /// this is called afetr data is saved in DEU 
        /// PostEntryStatus
        ///     0-First post
        ///     1-RePost
        ///     2-Delete
        /// </summary>
        /// <param name="deuFields"></param>
        /// <returns></returns>
        //public static PostProcessReturnStatus PostStart(DEUFields deuFields, int postEntryStatus, BasicInformationForProcess _BasicInformationForProcess)
        public static PostProcessReturnStatus PostStart(PostEntryProcess _PostEntryProcess, Guid DeuEntryId, Guid RepostNewDeuEntryId)
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(15)
            };

            TransactionScope transaction = null;
            if (Transaction.Current == null)
            {
                transaction = new TransactionScope(TransactionScopeOption.Required, options);
            }

            bool isException = false;
            //Guid tpolicyid = DEU.GetDeuEntryidWise(DeuEntryId).PolicyId;
            //object _LockStatus = GetLockObject(tpolicyid);
            //lock (_LockStatus)


            PostProcessReturnStatus _PostProcessReturnStatus = new PostProcessReturnStatus() { DeuEntryId = DeuEntryId, IsComplete = false, ErrorMessage = null, PostEntryStatus = _PostEntryProcess };
            try
            {
                if (_PostEntryProcess == PostEntryProcess.FirstPost)
                {


                    DEUFields deuFields = FillDEUFields(DeuEntryId);
                    BasicInformationForProcess _BasicInformationForProcess = PostUtill.GetPolicyToProcess(deuFields);
                    //--ForCommission DashBoard Validation not to Post Entry
                    if (deuFields.DeuData.IsEntrybyCommissiondashBoard)
                    {
                        if (_BasicInformationForProcess.IsPaymentToHO)
                        {
                            _PostProcessReturnStatus.ErrorMessage = "Payment is not Postable";
                            throw new Exception("Payment is not Postable");
                            // return _PostProcessReturnStatus;
                        }
                    }
                    //--------------------------------------------------------------
                    //BasicInformationForProcess _BasicInformationForProcess = GetPolicyToProcess(deuFields);//policy id, Poststaus,payment entry  
                    PolicyDetailsData policytoDeu = GetPolicy(_BasicInformationForProcess.PolicyId);
                    deuFields.DeuData.PolicyId = _BasicInformationForProcess.PolicyId;
                    deuFields.DeuData.PostStatusID = (int)_BasicInformationForProcess.PostStatus;
                    deuFields.DeuData.ClientID = (deuFields.DeuData.ClientID == null || deuFields.DeuData.ClientID == Guid.Empty) ? policytoDeu.ClientId : deuFields.DeuData.ClientID;
                    deuFields.DeuData.ClientName = Client.GetClient(deuFields.DeuData.ClientID.Value).Name;
                    deuFields.DeuData.PostCompleteStatus = (int)PostCompleteStatusEnum.InProgress;
                    deuFields.DeuData.Insured = string.IsNullOrEmpty(deuFields.DeuData.Insured) ? policytoDeu.Insured : deuFields.DeuData.Insured;

                   
                    //DEU.AddupdateDeuEntry(deuFields.DeuData);
                    //Create object and added by vinod

                    DEU objDEU = new DEU();
                    objDEU.AddupdateDeuEntry(deuFields.DeuData);

                    object _LockStatus = GetLockObject(deuFields.DeuData.PolicyId);
                    lock (_LockStatus)
                    {
                        _PostProcessReturnStatus = ProcessSearchedPoilcy(deuFields, _BasicInformationForProcess, _PostProcessReturnStatus);

                        RemoveLockObject(deuFields.DeuData.PolicyId);
                        if (_PostProcessReturnStatus.ErrorMessage == "Payment is not Postable")
                        {
                            throw new Exception("Payment is not Postable");
                        }
                    }
                    if (deuFields.DeuData.IsEntrybyCommissiondashBoard)
                    {
                        DEU de = DEU.GetDeuEntryidWise(_PostProcessReturnStatus.DeuEntryId);
                        de.PostCompleteStatus = (int)PostCompleteStatusEnum.Unsuccessful;

                        //DEU.AddupdateDeuEntry(de);
                        //Create object and added by vinod

                        DEU objdeu = new DEU();
                        objdeu.AddupdateDeuEntry(de);
                    }
                    _PostProcessReturnStatus.IsComplete = true;

                }
                else if (_PostEntryProcess == PostEntryProcess.RePost)
                {
                    if (RepostNewDeuEntryId != null && RepostNewDeuEntryId != Guid.Empty)
                    {
                        PostProcessReturnStatus _tempPostProcessReturnStatus = PostStart(PostEntryProcess.Delete, DeuEntryId, RepostNewDeuEntryId);
                        if (_tempPostProcessReturnStatus.IsComplete)
                        {
                            _tempPostProcessReturnStatus = PostStart(PostEntryProcess.FirstPost, RepostNewDeuEntryId, Guid.Empty);
                            _PostProcessReturnStatus = _tempPostProcessReturnStatus;
                            _PostProcessReturnStatus.PostEntryStatus = PostEntryProcess.RePost;
                        }
                    }
                    else if (RepostNewDeuEntryId == null || RepostNewDeuEntryId == Guid.Empty)
                    {
                        _PostProcessReturnStatus.ErrorMessage = "Repost Id is Empty";
                        _PostProcessReturnStatus.IsComplete = false;
                        _PostProcessReturnStatus.DeuEntryId = RepostNewDeuEntryId;

                    }
                    if (DEU.GetDeuEntryidWise(_PostProcessReturnStatus.DeuEntryId).IsEntrybyCommissiondashBoard)
                    {
                        DEU de = DEU.GetDeuEntryidWise(_PostProcessReturnStatus.DeuEntryId);
                        de.PostCompleteStatus = (int)PostCompleteStatusEnum.Unsuccessful;
                        //DEU.AddupdateDeuEntry(de);

                        //Create object and added by vinod
                        DEU objDEU = new DEU();
                        objDEU.AddupdateDeuEntry(de);
                    }
                    _PostProcessReturnStatus.ErrorMessage = "Null";
                    _PostProcessReturnStatus.IsComplete = true;
                }
                else if (_PostEntryProcess == PostEntryProcess.Delete)
                {
                    bool flag = false;
                    PolicyPaymentEntriesPost PolicySelectedIncomingPaymenttemp = PolicyPaymentEntriesPost.GetPolicyPaymentEntryDEUEntryIdWise(DeuEntryId);
                    if (PolicySelectedIncomingPaymenttemp == null)//post enrty was unsuccessful
                    {
                        _PostProcessReturnStatus.BatchStatementData = DEU.DeleteDeuEntry(DeuEntryId);
                        _PostProcessReturnStatus.IsComplete = true;
                        _PostProcessReturnStatus.ErrorMessage = "DEU Entry Deleted";
                        return _PostProcessReturnStatus;
                    }
                    DEUFields deuFields = FillDEUFields(DeuEntryId);

                    flag = PolicyOutgoingDistribution.IsEntryMarkPaid(PolicySelectedIncomingPaymenttemp.PaymentEntryID);

                    if (flag == true)
                    {
                        _PostProcessReturnStatus.IsComplete = false;
                        _PostProcessReturnStatus.ErrorMessage = "Paid/Semi Paid Entry";
                        return _PostProcessReturnStatus;
                    }

                    PolicyOutgoingDistribution.DeleteByPolicyIncomingPaymentId(PolicySelectedIncomingPaymenttemp.PaymentEntryID);
                    PolicyPaymentEntriesPost.DeletePolicyPayentIdWise(PolicySelectedIncomingPaymenttemp.PaymentEntryID);

                    _PostProcessReturnStatus.BatchStatementData = DEU.DeleteDeuEntry(deuFields.DeuEntryId);

                    //List<DEU> _DEULst = DEU.GetDEUPolicyIdWise(PolicySelectedIncomingPaymenttemp.PolicyID);

                    DEU objDeu = new DEU();
                    List<DEU> _DEULst = objDeu.GetDEUPolicyIdWise(PolicySelectedIncomingPaymenttemp.PolicyID);

                    if (_DEULst != null && _DEULst.Count != 0)
                    {
                        DEU _DEU = _DEULst.Where(p => p.InvoiceDate == _DEULst.Max(p1 => p1.InvoiceDate)).FirstOrDefault();
                        DEUFields _DEUFields = PostUtill.FillDEUFields(_DEU.DEUENtryID);
                        //DEU _LatestDEUrecord = DEU.GetLatestInvoiceDateRecord(deuFields.DeuData.PolicyId);

                        //Create  instance                       
                        DEU _LatestDEUrecord = objDeu.GetLatestInvoiceDateRecord(deuFields.DeuData.PolicyId);

                        Guid PolicyId = DEULearnedPost.AddDataDeuToLearnedPost(_LatestDEUrecord);
                        LearnedToPolicyPost.AddUpdateLearnedToPolicy(PolicyId);
                        PolicyToLearnPost.AddUpdatPolicyToLearn(PolicyId);
                        // PolicyToLearnPost.AddUpdatPolicyToLearn(_PolicyLearnedField);//Need to Test
                    }
                    else
                    {
                        PolicyDetailsData ppolicy = GetPolicy(PolicySelectedIncomingPaymenttemp.PolicyID);
                        if (ppolicy.PolicyStatusId == (int)_PolicyStatus.Pending)
                        {
                            try
                            {
                                PolicyLearnedField.DeleteByPolicy(ppolicy.PolicyId);
                                Policy.DeletePolicyFromDB(ppolicy);

                            }
                            catch
                            {

                            }
                            RemoveClient(ppolicy.ClientId ?? Guid.Empty, ppolicy.PolicyLicenseeId ?? Guid.Empty);
                            //DeleteFollowup
                        }
                        else
                        {
                            PolicyDetailsData Policyhistory = Policy.GetPolicyHistoryIdWise(PolicySelectedIncomingPaymenttemp.PolicyID);
                            if (Policyhistory != null)
                                Policy.AddUpdatePolicy(Policyhistory);

                            PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsHistoryPolicyWise(PolicySelectedIncomingPaymenttemp.PolicyID);
                            if (_PolicyLearnedField != null)
                                PolicyLearnedField.AddUpdateLearned(_PolicyLearnedField, _PolicyLearnedField.ProductType);
                        }
                    }
                    //DEU de = DEU.GetDeuEntryidWise(_PostProcessReturnStatus.DeuEntryId);
                    //de.PostCompleteStatus = (int)PostCompleteStatusEnum.Unsuccessful;
                    //DEU.AddupdateDeuEntry(de);
                    _PostProcessReturnStatus.ErrorMessage = "Null";
                    _PostProcessReturnStatus.IsComplete = true;

                }
                //trancation commit
                if (transaction != null)
                    transaction.Complete();
            }
            catch (Exception)
            {
                isException = true;
            }

            if (isException)
            {
                //DEU de = DEU.GetDeuEntryidWise(_PostProcessReturnStatus.DeuEntryId);
                //de.PostCompleteStatus = (int)PostCompleteStatusEnum.Unsuccessful;
                //DEU.AddupdateDeuEntry(de);
                _PostProcessReturnStatus.IsComplete = false;


            }


            return _PostProcessReturnStatus;

        }

        private static void RemoveLockObject(Guid tpolicyid)
        {
            LockDic.Remove(tpolicyid);
        }

        private static object GetLockObject(Guid tpolicyid)
        {
            object _LockStatus = null;
            try
            {
                _LockStatus = LockDic[tpolicyid];
            }
            catch
            {

            }
            if (_LockStatus == null)
            {
                _LockStatus = new object();
                LockDic.Add(tpolicyid, _LockStatus);

            }
            return _LockStatus;
        }


        public static void RemoveClient(Guid ClientId, Guid LicenseeId)
        {
            if (ClientId == Guid.Empty || LicenseeId == Guid.Empty) return;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PolicyLicenseeId", LicenseeId);
            parameters.Add("IsDeleted", false);
            parameters.Add("PolicyClientId", ClientId);
            int num = Policy.GetPolicyData(parameters).ToList().Count;
            if (num > 0)
                return;
            Client _Client = new Client()
            {
                ClientId = ClientId,
            };
            Client.DeleteClient(_Client);
        }

        public static DEUFields FillDEUFields(Guid DeuEntryId)
        {
            DEUFields deufields = new DEUFields();
            DEU _DEU = DEU.GetDeuEntryidWise(DeuEntryId);
            deufields.DeuEntryId = DeuEntryId;
            deufields.StatementId = _DEU.StmtID ?? Guid.Empty;
            deufields.PayorId = _DEU.PayorId;
            deufields.BatchId = Statement.GetStatement(deufields.StatementId).BatchId;
            //deufields.LicenseeId = Batch.GetBatchViaBatchId(deufields.BatchId).LicenseeId;
            Batch objBatch = new Batch();
            deufields.LicenseeId = objBatch.GetBatchViaBatchId(deufields.BatchId).LicenseeId;
            deufields.CurrentUser = _DEU.CreatedBy ?? Guid.Empty;
            deufields.DeuData = DEU.GetDeuEntryidWise(DeuEntryId);
            if (_DEU.IsEntrybyCommissiondashBoard)
            {
                deufields.SearchedPolicyList = GetDeuSearchedPolicyviaPolicyId(_DEU.PolicyId);
            }
            else
            {
                List<UniqueIdenitfier> _UniqueIndenties = GetUniqueIdentifierForPayor(deufields.PayorId, DeuEntryId);//It is to be check
                deufields.SearchedPolicyList = GetPoliciesFromUniqueIdentifier(_UniqueIndenties, deufields.LicenseeId ?? Guid.Empty, deufields.PayorId ?? Guid.Empty);
            }

            return deufields;
        }

        private static List<DeuSearchedPolicy> GetDeuSearchedPolicyviaPolicyId(Guid PolicyId)
        {
            List<DeuSearchedPolicy> _PolicyLst = new List<DeuSearchedPolicy>();
            PolicyDetailsData poli = GetPolicy(PolicyId);

            DeuSearchedPolicy dsp = new DeuSearchedPolicy();
            dsp.CarrierName = poli.CarrierName;
            dsp.ClientName = poli.ClientName;
            //   dsp.CompSchedule = poli.s;
            dsp.CompType = PolicyIncomingPaymentType.GetIncomingPaymentTypeList()
                            .Where(p => p.PaymentTypeId == poli.IncomingPaymentTypeId.Value).FirstOrDefault().PaymenProcedureName;
            dsp.Insured = poli.Insured;
            dsp.LastModifiedDate = poli.CreatedOn.Value;
            dsp.PaymentMode = poli.PolicyModeId;
            dsp.PolicyId = poli.PolicyId;
            dsp.PolicyNumber = poli.PolicyNumber;
            dsp.PolicyStatus = poli.PolicyStatusId ?? 0;
            dsp.ProductName = poli.CoverageName;
            _PolicyLst.Add(dsp);

            return _PolicyLst;
        }

        private static List<UniqueIdenitfier> GetUniqueIdentifierForPayor(Guid? PayorId, Guid DeuEntryId)
        {
            List<UniqueIdenitfier> _UniqueIndenties = new List<UniqueIdenitfier>();
            PayorTool PayorTool = PayorTool.GetPayorToolMgr((Guid)PayorId);
            List<PayorToolField> _PayorToolField = PayorTool.ToolFields.Where(p => p.IsPartOfPrimaryKey == true).ToList();
            DEU _DEU = DEU.GetDeuEntryidWise(DeuEntryId);
            foreach (PayorToolField ptf in _PayorToolField)
            {
                UniqueIdenitfier _UniqueIdenitfier = new UniqueIdenitfier();

                _UniqueIdenitfier.ColumnName = ptf.EquivalentDeuField;
                _UniqueIdenitfier.MaskedText = ptf.MaskText;
                _UniqueIdenitfier.Text = GetValueFromDEUTableForAColumn(_DEU, _UniqueIdenitfier.ColumnName);
                _UniqueIndenties.Add(_UniqueIdenitfier);
            }

            return _UniqueIndenties;
        }

        private static string GetValueFromDEUTableForAColumn(DEU DeuEntry, string ColumnName)
        {
            string value = null;
            switch (ColumnName)
            {
                case "Effective":
                    value = DeuEntry.OriginalEffectiveDate.Value.ToString("MM/dd/yyyy");
                    break;
                //case "PaymentReceived":
                //    value = DeuEntry.PaymentRecived.ToString();
                //    break;
                //case "CommissionPercentage":
                //    value = DeuEntry.CommissionPercentage.ToString();
                //    break;

                case "Insured":
                    value = DeuEntry.Insured.ToString();
                    break;
                case "PolicyNumber":
                    value = DeuEntry.PolicyNumber.ToString();
                    break;
                //case "Enrolled":
                //    value = DeuEntry.Enrolled.ToString();
                //    break;
                //case "Eligible":
                //    value = DeuEntry.Eligible.ToString();
                //    break;

                //case "Link1":
                //    value = DeuEntry.Link1.ToString();
                //    break;
                //case "SplitPercentage":
                //    value = DeuEntry.SplitPer.ToString();
                //    break;
                case "PolicyMode":
                    value = DeuEntry.PolicyMode.ToString();
                    break;
                case "TrackFromDate":
                    value = DeuEntry.TrackFromDate.Value.ToString("MM/dd/yyyy");
                    break;
                //case "PMC":
                //    value = DeuEntry.PMC.ToString();
                //    break;
                //case "PAC":
                //    value = DeuEntry.PAC.ToString();
                //   break;
                //case "CompScheduleType":
                //    value = DeuEntry.CompScheduleTypeID.ToString();
                //    break;
                case "CompType":
                    value = DeuEntry.CompTypeID.ToString();
                    break;
                case "Client":
                    value = DeuEntry.ClientID.ToString();
                    break;
                //case "InvoiceDate":
                //    value = DeuEntry.InvoiceDate.ToString();
                //    break;
                case "Payor":
                    value = DeuEntry.PayorId.ToString();
                    break;
                //case "PayorSysId":
                //    value = DeuEntry.PayorSysID.ToString();
                //    break;
                //case "Renewal":
                //    value = DeuEntry.Renewal.ToString();
                //    break;
                case "Carrier":
                    value = Carrier.GetCarrierNickName(DeuEntry.PayorId ?? Guid.Empty, DeuEntry.CarrierID ?? Guid.Empty);
                    break;
                case "Product":
                    value = Coverage.GetCoverageNickName(DeuEntry.PayorId ?? Guid.Empty, DeuEntry.CarrierID ?? Guid.Empty,
                        DeuEntry.CoverageID ?? Guid.Empty);
                    break;
                default:
                    PropertyInfo identifier = typeof(DEU).GetProperty(ColumnName);
                    if (identifier != null)
                    {
                        value = identifier.GetValue(DeuEntry, null).ToString();
                    }
                    break;

            }
            return value;

        }
        public static int CalculatePMC(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                System.Data.Objects.ObjectParameter objParam = new System.Data.Objects.ObjectParameter("pMC", typeof(Int32));
                //DataModel.CalculatePMC(PolicyId, objParam);
                return (int)objParam.Value;
            }
        }
        //  public static List<Policy> GetPoliciesFromUniqueIdentifier(DEUFields deuFields)
        public static List<DeuSearchedPolicy> GetPoliciesFromUniqueIdentifier(List<UniqueIdenitfier> _UniqueIndenties, Guid LicenseId, Guid PayorId)
        {

            List<PolicyDetailsData> _policy = null;
            List<DeuSearchedPolicy> _PolicyLst = new List<DeuSearchedPolicy>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PayorId", PayorId);
            _policy = Policy.GetPolicyData(parameters).ToList();
            Guid? carrierId = null, coverageId = null;
            Expression<Func<DLinq.Policy, bool>> tempParameters = p => p.PolicyLicenseeId == LicenseId;
            Expression<Func<DLinq.Policy, bool>> expressionParameters = p => p.IsDeleted == false;
           // Expression<Func<DLinq.Policy, bool>> tempParameters;
            foreach (UniqueIdenitfier DFD in _UniqueIndenties)
            {
                switch (DFD.ColumnName)
                {
                    case "Effective":
                        DateTime EffectiveDate = DateTime.ParseExact(DFD.Text, "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
                        parameters.Add("OriginalEffectiveDate",EffectiveDate);
                        break;
                    case "Insured":
                        parameters.Add("Insured",DFD.Text);
                        break;
                    case "PolicyNumber":
                        parameters.Add("PolicyNumber",DFD.Text);
                        break;
                    case "Enrolled":
                        parameters.Add("Enrolled", DFD.Text);
                        break;
                    case "Eligible":
                        parameters.Add("Eligible",DFD.Text);
                        break;
                    case "SplitPercentage":
                        double? value = Convert.ToDouble(DFD.Text);
                        parameters.Add("SplitPercentage",value);
                        break;
                    case "PolicyMode":
                        Int32? PolicyMode = Convert.ToInt32(DFD.Text);
                        parameters.Add("PolicyModeId", PolicyMode);
                        break;
                    case "TrackFromDate":
                        DateTime TrackFrom = DateTime.ParseExact(DFD.Text, "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
                        parameters.Add("TrackFromDate", TrackFrom);
                        break;
                    case "CompType":
                        int? PaymentId = Convert.ToInt32(DFD.Text);
                        parameters.Add("IncomingPaymentTypeId", PaymentId);
                        break;
                    case "Client":
                        Guid? clientId = BLHelper.GetClientId(DFD.Text, LicenseId);
                        parameters.Add("PolicyClientId", clientId);
                        break;
                    case "Payor":
                        Guid? PayorIdval = Guid.Parse(DFD.Text);
                        parameters.Add("PayorId", PayorIdval);
                        break;
                    case "Carrier":
                        carrierId = BLHelper.GetCarrierId(DFD.Text, PayorId);
                        if (carrierId == null || carrierId == Guid.Empty)
                        {
                            string carriernickname = DFD.Text;
                            tempParameters = p => p.PolicyLearnedField.CarrierNickName == carriernickname;
                            expressionParameters = expressionParameters.And(tempParameters);
                        }
                        else
                        {
                            parameters.Add("CarrierId", carrierId);
                        }
                        break;
                    case "Product":
                         coverageId = BLHelper.GetProductId(DFD.Text, PayorId, carrierId);
                        if (coverageId == null || coverageId == Guid.Empty)
                        {
                            string CoverageNickName = DFD.Text;
                            tempParameters = p => p.PolicyLearnedField.CoverageNickName == CoverageNickName;
                            expressionParameters = expressionParameters.And(tempParameters);
                        }
                        else
                        {
                            parameters.Add("CoverageId", coverageId);
                        }
                        break;
                    case "CompScheduleType":
                        string compScheduleType = DFD.Text;
                        tempParameters = p => p.PolicyLearnedField.CompScheduleType == compScheduleType;
                        expressionParameters = expressionParameters.And(tempParameters);
                        break;
                }
            }
            _policy = Policy.GetPolicyData(parameters,expressionParameters).ToList();
            foreach (PolicyDetailsData poli in _policy)
            {
                DeuSearchedPolicy dsp = new DeuSearchedPolicy();
                dsp.CarrierName = poli.CarrierName;
                dsp.ClientName = poli.ClientName;
                
                dsp.CompType = PolicyIncomingPaymentType.GetIncomingPaymentTypeList()
                                .Where(p => p.PaymentTypeId == (poli.IncomingPaymentTypeId ?? 1)).FirstOrDefault().PaymenProcedureName;
                dsp.Insured = poli.Insured;
                dsp.LastModifiedDate = poli.CreatedOn.Value;
                dsp.PaymentMode = poli.PolicyModeId;
                dsp.PolicyId = poli.PolicyId;
                dsp.PolicyNumber = poli.PolicyNumber;
                dsp.PolicyStatus = poli.PolicyStatusId ?? 0;
                dsp.ProductName = poli.CoverageName;
                _PolicyLst.Add(dsp);
            }
            return _PolicyLst;
        }

        //public static BasicInformationForProcess _BasicInformationForProcess = new BasicInformationForProcess();
        /// <summary>
        /// This is called 
        /// </summary>       
        /// <param name="IsAgencyVersionLicense"></param>
        /// <param name="deuFields"></param>
        /// <returns>Policy Id</returns>
        public static BasicInformationForProcess GetPolicyToProcess(DEUFields deuFields)
        {
            BasicInformationForProcess _BasicInformationForProcess = new BasicInformationForProcess();
            //  Policy _Policy = new Policy();
            // Guid _PolicyId = Guid.Empty;
            int SearchedPolicyCnt = 0;// SearchedPolicy.Count;

            if (deuFields.SearchedPolicyList != null)
                SearchedPolicyCnt = deuFields.SearchedPolicyList.Count;

            bool IsAgencyVersionLicense = BillingLineDetail.IsAgencyVersionLicense(deuFields.LicenseeId.Value);
            if (SearchedPolicyCnt == 0)
            {
                _BasicInformationForProcess.IsPaymentToHO = true;
                _BasicInformationForProcess.PostStatus = PostStatus.NoLink;

                if (IsAgencyVersionLicense || !IsAgencyVersionLicense)//if agency or not agency
                {
                    _BasicInformationForProcess.PolicyId = CreateNewPendingPolicy(deuFields);//Discuss on that  policy is to be created on under which agent or HO--it is created on HO
                }
            }
            else if (SearchedPolicyCnt > 1)
            {
                int PendingPolicyCnt = deuFields.SearchedPolicyList.Where(p => p.PolicyStatus == (int)_PolicyStatus.Pending).ToList().Count;
                // int PendingPolicyCnt = SearchedPolicy.Where(p => p.PolicyStatusId == (int)_PolicyStatus.Pending).ToList().Count;
                if (PendingPolicyCnt == 1)
                {
                    _BasicInformationForProcess.IsPaymentToHO = true;
                    if (!IsAgencyVersionLicense)//if agency or not agency
                    {
                        _BasicInformationForProcess.PostStatus = PostStatus.NoAgency;
                        _BasicInformationForProcess.PolicyId = deuFields.SearchedPolicyList.Where(p => p.PolicyStatus == (int)_PolicyStatus.Pending).FirstOrDefault().PolicyId;
                        // _Policy = SearchedPolicy.Where(p => p.PolicyStatusId == (int)_PolicyStatus.Pending).ToList<Policy>().FirstOrDefault();

                    }
                    else if (IsAgencyVersionLicense)
                    {
                        _BasicInformationForProcess.PostStatus = PostStatus.NoLink;

                        _BasicInformationForProcess.PolicyId = deuFields.SearchedPolicyList.Where(p => p.PolicyStatus == (int)_PolicyStatus.Pending).FirstOrDefault().PolicyId;

                    }
                }
                else if (PendingPolicyCnt == 0)
                {
                    _BasicInformationForProcess.IsPaymentToHO = true;
                    if (!IsAgencyVersionLicense)//if agency or not agency
                    {
                        _BasicInformationForProcess.PostStatus = PostStatus.NoAgency;
                        _BasicInformationForProcess.PolicyId = CreateNewPendingPolicy(deuFields);//Discuss on that  policy is to be created on under which agent or HO

                    }
                    else
                    {
                        _BasicInformationForProcess.PostStatus = PostStatus.NoLink;
                        _BasicInformationForProcess.PolicyId = CreateNewPendingPolicy(deuFields);//Discuss on that  policy is to be created on under which agent or HO

                    }
                }
                else if (PendingPolicyCnt > 1)//this case is need to confirm
                {
                    _BasicInformationForProcess.IsPaymentToHO = true;
                    if (!IsAgencyVersionLicense)//if agency or not agency
                    {
                        _BasicInformationForProcess.PostStatus = PostStatus.NoAgency;
                        DateTime LatestPendingPolicyDate = deuFields.SearchedPolicyList.Where(p => p.PolicyStatus == (int)_PolicyStatus.Pending).Max(p => p.LastModifiedDate);
                        _BasicInformationForProcess.PolicyId = deuFields.SearchedPolicyList.Where(p => p.PolicyStatus == (int)_PolicyStatus.Pending).Where(p => p.LastModifiedDate == LatestPendingPolicyDate).FirstOrDefault().PolicyId;

                    }
                    else
                    {
                        _BasicInformationForProcess.PostStatus = PostStatus.NoLink;
                        DateTime LatestPendingPolicyDate = deuFields.SearchedPolicyList.Where(p => p.PolicyStatus == (int)_PolicyStatus.Pending).Max(p => p.LastModifiedDate);
                        _BasicInformationForProcess.PolicyId = deuFields.SearchedPolicyList.Where(p => p.PolicyStatus == (int)_PolicyStatus.Pending).Where(p => p.LastModifiedDate == LatestPendingPolicyDate).FirstOrDefault().PolicyId;

                    }
                }
            }
            else if (SearchedPolicyCnt == 1)
            {
                int SearchPendingStatuscnt = deuFields.SearchedPolicyList.Where(p => p.PolicyStatus == (int)_PolicyStatus.Pending).ToList().Count;
                if (SearchPendingStatuscnt == 1)
                {
                    _BasicInformationForProcess.IsPaymentToHO = true;
                    if (!IsAgencyVersionLicense)//if agency or not agency
                    {
                        _BasicInformationForProcess.PostStatus = PostStatus.NoAgency;
                        _BasicInformationForProcess.PolicyId = deuFields.SearchedPolicyList.Where(p => p.PolicyStatus == (int)_PolicyStatus.Pending).FirstOrDefault().PolicyId;
                    }
                    else
                    {
                        _BasicInformationForProcess.PostStatus = PostStatus.NoLink;
                        _BasicInformationForProcess.PolicyId = deuFields.SearchedPolicyList.Where(p => p.PolicyStatus == (int)_PolicyStatus.Pending).FirstOrDefault().PolicyId;
                    }

                }
                else//Not Pending (Either Active/Terminated)
                {
                    if (IsAgencyVersionLicense)//if agency or not agency
                    {
                        _BasicInformationForProcess.PolicyId = Guid.Empty;
                        DeuSearchedPolicy findpolicy = deuFields.SearchedPolicyList.First();
                        PolicySchedule _PolicySchedule = CheckForIncomingTypeOfSchedule(findpolicy.PolicyId);

                        if (_PolicySchedule == PolicySchedule.None)//Schedule Not Matches
                        {
                            _BasicInformationForProcess.PostStatus = PostStatus.Ag_NoSplits;
                            _BasicInformationForProcess.PolicyId = CreateNewPendingPolicy(deuFields);
                            _BasicInformationForProcess.IsPaymentToHO = true;
                        }
                        else //Schedle Exits-- Check for Match or not match
                        {
                            PolicyPaymentEntriesPost _PolicyPaymentEntriesPost = GetInMemoryCollectionOfPolicyPaymentEntriesFromDEUInMemoryCollection(deuFields, findpolicy.PolicyId);
                            bool IsMatches = CheckForIncomingScheduleVariance(_PolicyPaymentEntriesPost);//CheckForScheduleMatches();
                            if (IsMatches)
                            {
                                _BasicInformationForProcess.PolicyId = deuFields.SearchedPolicyList.FirstOrDefault().PolicyId;
                                _BasicInformationForProcess.PostStatus = PostStatus.Linked_Agency;
                                _BasicInformationForProcess.IsPaymentToHO = false;

                            }
                            else
                            {
                                _BasicInformationForProcess.PostStatus = PostStatus.Ag_NoMSplits;
                                _BasicInformationForProcess.PolicyId = CreateNewPendingPolicy(deuFields);//Discuss on that  policy is to be created on under which agent or HO
                                _BasicInformationForProcess.IsPaymentToHO = true;
                            }
                        }
                        //if (_PolicySchedule == PolicySchedule.Basic)//Schedule Matches
                        //{
                        //    PolicyToolIncommingShedule _Schedule = GetBasicScheduleOfPolicy(findpolicy.PolicyId);
                        //}
                        //else if (_PolicySchedule == PolicySchedule.Advance)//Schedule Matches
                        //{
                        //    List<IncomingSchedule> _IncomingSchedule = IncomingSchedule.GetAdvanceScheduleListPolicyWise(findpolicy.PolicyId);
                        //}
                        //else if (_PolicySchedule == PolicySchedule.None)//Schedule Not Matches
                        //{

                        //}
                    }
                    else
                    {
                        _BasicInformationForProcess.PostStatus = PostStatus.Linked_NoAg;
                        _BasicInformationForProcess.IsPaymentToHO = true;
                        _BasicInformationForProcess.PolicyId = deuFields.SearchedPolicyList.FirstOrDefault().PolicyId;

                    }
                }

            }

            return _BasicInformationForProcess;

        }

        private static PolicyPaymentEntriesPost GetInMemoryCollectionOfPolicyPaymentEntriesFromDEUInMemoryCollection(DEUFields deuFields, Guid PolicyId)
        {
            PolicyPaymentEntriesPost _PolicyPaymentEntriesPost = new PolicyPaymentEntriesPost();
            _PolicyPaymentEntriesPost.PolicyID = PolicyId;
            _PolicyPaymentEntriesPost.InvoiceDate = deuFields.DeuData.InvoiceDate;
            _PolicyPaymentEntriesPost.PaymentRecived = deuFields.DeuData.PaymentRecived ?? 0;
            _PolicyPaymentEntriesPost.CommissionPercentage = deuFields.DeuData.CommissionPercentage ?? 0;
            _PolicyPaymentEntriesPost.SplitPer = deuFields.DeuData.SplitPer;
            _PolicyPaymentEntriesPost.ClientId = deuFields.DeuData.ClientID ?? Guid.Empty;
            _PolicyPaymentEntriesPost.NumberOfUnits = deuFields.DeuData.NoOfUnits ?? 0;
            _PolicyPaymentEntriesPost.DollerPerUnit = deuFields.DeuData.DollerPerUnit ?? 0;
            _PolicyPaymentEntriesPost.Fee = deuFields.DeuData.Fee ?? 0;
            _PolicyPaymentEntriesPost.Bonus = deuFields.DeuData.Bonus ?? 0;
            _PolicyPaymentEntriesPost.TotalPayment = deuFields.DeuData.CommissionTotal ?? 0;
            _PolicyPaymentEntriesPost.PostStatusID = deuFields.DeuData.PostStatusID;

            //foreach (DeuFieldData field in deuFieldsdata)
            //{
            //    switch (field.MappedFields.DEUMappedField)
            //    {

            //        case "InvoiceDate":
            //            _PolicyPaymentEntriesPost.InvoiceDate = Convert.ToDateTime(field.Text);
            //            break;

            //        case "PaymentReceived":
            //            if (string.IsNullOrEmpty(field.Text))
            //                _PolicyPaymentEntriesPost.PaymentRecived = 0;
            //            else
            //                _PolicyPaymentEntriesPost.PaymentRecived = Convert.ToDecimal(field.Text);
            //            break;

            //        case "CommissionPercentage":
            //            if (string.IsNullOrEmpty(field.Text))
            //                _PolicyPaymentEntriesPost.CommissionPercentage = 0;
            //            else
            //                _PolicyPaymentEntriesPost.CommissionPercentage = Convert.ToDecimal(field.Text);
            //            break;



            //        case "SplitPer":
            //            _PolicyPaymentEntriesPost.SplitPer = Convert.ToDecimal(field.Text);
            //            break;



            //        case "Client":
            //            _PolicyPaymentEntriesPost.ClientId = DEU.GetClientId(field.Text, LicenseeId);
            //            break;

            //        case "NumberOfUnits":
            //            if (string.IsNullOrEmpty(field.Text))
            //                _PolicyPaymentEntriesPost.NumberOfUnits = 0;
            //            else
            //                _PolicyPaymentEntriesPost.NumberOfUnits = Convert.ToInt32(field.Text);
            //            break;

            //        case "DollerOfUnits":
            //            if (string.IsNullOrEmpty(field.Text))
            //                _PolicyPaymentEntriesPost.DollerPerUnit = 0;
            //            else
            //                _PolicyPaymentEntriesPost.DollerPerUnit = Convert.ToDecimal(field.Text);
            //            break;

            //        case "Fee":
            //            if (string.IsNullOrEmpty(field.Text))
            //                _PolicyPaymentEntriesPost.Fee = 0;
            //            else
            //                _PolicyPaymentEntriesPost.Fee = Convert.ToDecimal(field.Text);
            //            break;

            //        case "Bonus":
            //            if (string.IsNullOrEmpty(field.Text))
            //                _PolicyPaymentEntriesPost.Bonus = 0;
            //            else
            //                _PolicyPaymentEntriesPost.Bonus = Convert.ToDecimal(field.Text);

            //            break;


            //        case "CommissionTotal":
            //            //  savedCommissionTotal = DeuEntry.CommissionTotal;
            //            if (string.IsNullOrEmpty(field.Text))
            //                _PolicyPaymentEntriesPost.TotalPayment = 0;
            //            else
            //                _PolicyPaymentEntriesPost.TotalPayment = Convert.ToDecimal(field.Text);
            //            break;

            //        default:

            //            break;
            //    }
            //--The Field swhich are remain ,not in DEU...
            //}

            return _PolicyPaymentEntriesPost;

        }




        //private static PolicyToolIncommingShedule Get
        public static MasterPolicyMode ModeEntryFromDeu(PolicyDetailsData _policy, int? deuenteredmode)
        {
            MasterPolicyMode _FollowUpMode = MasterPolicyMode.Monthly;

            int? LearnedMode = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(_policy.PolicyId).PolicyModeId;

            int? policyDetail = GetPolicy(_policy.PolicyId).PolicyModeId;
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntries = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(_policy.PolicyId);

            //1
            if (_policy.PolicyStatusId == (int)_PolicyStatus.Pending && LearnedMode == null && policyDetail == null && _PolicyPaymentEntries.Count == 0)
            {
                _FollowUpMode = MasterPolicyMode.Monthly;
            }
            //2
            else if (_policy.PolicyStatusId == (int)_PolicyStatus.Pending && LearnedMode == null && policyDetail == null && _PolicyPaymentEntries.Count == 1)
            {

                _FollowUpMode = MasterPolicyMode.Monthly;

            }
            //3
            else if (_policy.PolicyStatusId == (int)_PolicyStatus.Pending && LearnedMode == null && policyDetail == null && _PolicyPaymentEntries.Count >= 2)
            {
                _FollowUpMode = GetPolicyModeFromOldRecipts(_policy.PolicyId);
            }
            ////4
            else if (_policy.PolicyStatusId == (int)_PolicyStatus.Pending && deuenteredmode != null && LearnedMode != null && policyDetail != null && deuenteredmode != LearnedMode && LearnedMode == policyDetail)
            {
                _FollowUpMode = (MasterPolicyMode)deuenteredmode;

                PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(_policy.PolicyId);
                _PolicyLearnedField.PolicyModeId = (int)_FollowUpMode;
                PolicyLearnedField.AddUpdateLearned(_PolicyLearnedField, _PolicyLearnedField.ProductType);


            }
            //5
            else if (_policy.PolicyStatusId != (int)_PolicyStatus.Pending && deuenteredmode != null && LearnedMode == null && policyDetail == null)
            {
                PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(_policy.PolicyId);
                _PolicyLearnedField.PolicyModeId = deuenteredmode;
                PolicyLearnedField.AddUpdateLearned(_PolicyLearnedField, _PolicyLearnedField.ProductType);

                _policy.PolicyModeId = deuenteredmode.Value;
                Policy.AddUpdatePolicy(_policy);

                _FollowUpMode = (MasterPolicyMode)deuenteredmode;


            }

                //6
            else if (_policy.PolicyStatusId != (int)_PolicyStatus.Pending && deuenteredmode == null && LearnedMode == null && policyDetail == null)
            {
                _FollowUpMode = GetPolicyModeFromOldRecipts(_policy.PolicyId);
            }
            /////
            //7
            else if (_policy.PolicyStatusId != (int)_PolicyStatus.Pending && deuenteredmode == null && LearnedMode != null && LearnedMode == policyDetail)
            {
                _FollowUpMode = (MasterPolicyMode)LearnedMode;
                //PolicyLearnedField _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(_policy.PolicyId);
                //_PolicyLearnedField.PolicyModeId = (int)_FollowUpMode;
                //PolicyLearnedField.AddUpdateLearned(_PolicyLearnedField);
            }
            //8
            else if (_policy.PolicyStatusId != (int)_PolicyStatus.Pending && deuenteredmode != null && LearnedMode != null && policyDetail != null && deuenteredmode != LearnedMode && LearnedMode == policyDetail)
            {
                PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(_policy.PolicyId);
                _PolicyLearnedField.PolicyModeId = deuenteredmode;
                PolicyLearnedField.AddUpdateLearned(_PolicyLearnedField, _PolicyLearnedField.ProductType);

                _policy.PolicyModeId = deuenteredmode.Value;
                Policy.AddUpdatePolicy(_policy);

                _FollowUpMode = GetPolicyModeFromOldRecipts(_policy.PolicyId);
            }
            return _FollowUpMode;
        }
        private static MasterPolicyMode GetPolicyModeFromOldRecipts(Guid _PolicyId)
        {

            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(_PolicyId);

            List<DateTime?> UniqueDateTime = _PolicyPaymentEntriesPost.Select(c => c.InvoiceDate).Distinct().ToList();

            List<PolicyPaymentEntriesPost> _TempPolicyPaymentEntriesPost = new List<PolicyPaymentEntriesPost>(_PolicyPaymentEntriesPost);
            _PolicyPaymentEntriesPost.Clear();
            foreach (DateTime dt in UniqueDateTime)
            {
                _PolicyPaymentEntriesPost.Add(_TempPolicyPaymentEntriesPost.Where(p => p.InvoiceDate == dt).FirstOrDefault());
            }
            _PolicyPaymentEntriesPost = _PolicyPaymentEntriesPost.OrderBy(p => p.InvoiceDate).ToList<PolicyPaymentEntriesPost>();
            int minDiff = 12;
            if (_PolicyPaymentEntriesPost.Count == 1) return MasterPolicyMode.Monthly;
            for (int idx = 0; idx < _PolicyPaymentEntriesPost.Count - 1; idx++)
            {

                int diff = (GetNumberOfMonthBetweenTwoDays(_PolicyPaymentEntriesPost[idx + 1].InvoiceDate.Value, _PolicyPaymentEntriesPost[idx].InvoiceDate.Value));
                if (minDiff == 1) break;
                if (minDiff > diff)
                {
                    minDiff = diff;
                }


            }

            if (minDiff % 12 == 0)
            {
                return MasterPolicyMode.Annually;
            }
            else if (minDiff % 6 == 0)
            {
                return MasterPolicyMode.HalfYearly;
            }
            else if (minDiff % 3 == 0)
            {
                return MasterPolicyMode.Quarterly;
            }
            else if (minDiff % 1 == 0)
            {
                return MasterPolicyMode.Monthly;
            }


            return MasterPolicyMode.Monthly;
        }

        private static int GetNumberOfMonthBetweenTwoDays(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }


        public static PostProcessReturnStatus ProcessSearchedPoilcy(DEUFields deuFields, BasicInformationForProcess _BasicInformationForProcess, PostProcessReturnStatus _PostProcessReturnStatus)
        {
            //Policy _Policy = GetPolicy(_BasicInformationForProcess.PolicyId);
            PostStatus poststatus = _BasicInformationForProcess.PostStatus;
            bool IsTrackPayment = false;
            //DEU _DeuEntry = null;
            Guid PaymentEntryID = Guid.Empty;
            bool IsPaymentToHO = _BasicInformationForProcess.IsPaymentToHO;// false;//Verify Check
            PaymentMode _PaymentMode = PaymentMode.Monthly;

            PaymentEntryID = EntryInPoicyPamentEntries(deuFields, _BasicInformationForProcess);//remain to Complete--Completed

            //DEU _LatestDEUrecord = DEU.GetLatestInvoiceDateRecord(deuFields.DeuData.PolicyId);

            //Added by vinod ..Create object of instance method
            DEU objDeu = new DEU();
            DEU _LatestDEUrecord = objDeu.GetLatestInvoiceDateRecord(deuFields.DeuData.PolicyId);            

            Guid PolicyIdDeuToLrn = DEULearnedPost.AddDataDeuToLearnedPost(_LatestDEUrecord);
            LearnedToPolicyPost.AddUpdateLearnedToPolicy(PolicyIdDeuToLrn);
            PolicyToLearnPost.AddUpdatPolicyToLearn(PolicyIdDeuToLrn);

            IsTrackPayment = GetPolicyTrackPayment(_BasicInformationForProcess.PolicyId).IsTrackPayment;

            MasterPolicyMode _MasterPolicyMode = ModeEntryFromDeu(GetPolicy(deuFields.DeuData.PolicyId), deuFields.DeuData.PolicyMode.Value);
            _PaymentMode = ConvertMode(_MasterPolicyMode);
            // FollowUpProcedure(IsTrackPayment, _PaymentMode, deuFields.DeuData.PolicyId, deuFields.DeuData.InvoiceDate.Value, PaymentEntryID);
            if (!IsPaymentToHO)
                IsPaymentToHO = !CheckForOutgoingScheduleVariance(PaymentEntryID);
            //--ForCommission DashBoard Validation not to Post Entry
            if (deuFields.DeuData.IsEntrybyCommissiondashBoard)
            {
                if (IsPaymentToHO)
                {
                    _PostProcessReturnStatus.ErrorMessage = "Payment is not Postable";
                    return _PostProcessReturnStatus;
                }
            }
            //--------------------------------------------------------------
            EntryInPolicyOutGoingPayment(IsPaymentToHO, PaymentEntryID, deuFields.DeuData.PolicyId, Convert.ToDateTime(deuFields.DeuData.InvoiceDate), deuFields.LicenseeId.Value);//Need to check with Column i recieved
            _PostProcessReturnStatus.ErrorMessage = "Completed";
            return _PostProcessReturnStatus;
        }

        public static PaymentMode ConvertMode(MasterPolicyMode _MasterPolicyMode)
        {
            PaymentMode _PaymentMode = PaymentMode.Monthly;
            switch (_MasterPolicyMode)
            {
                case MasterPolicyMode.Annually:
                    _PaymentMode = PaymentMode.Yearly;
                    break;
                case MasterPolicyMode.HalfYearly:
                    _PaymentMode = PaymentMode.HalfYearly;
                    break;
                case MasterPolicyMode.Monthly:
                    _PaymentMode = PaymentMode.Monthly;
                    break;
                case MasterPolicyMode.OneTime:
                    _PaymentMode = PaymentMode.OneTime;
                    break;
                case MasterPolicyMode.Quarterly:
                    _PaymentMode = PaymentMode.Quarterly;
                    break;
                case MasterPolicyMode.Random:
                    _PaymentMode = PaymentMode.Random;
                    break;
                default:
                    break;
            }
            return _PaymentMode;
        }



        /// <summary>
        /// 000   ---Work remaingin 
        /// </summary>
        /// <param name="IsPaymentToHO"></param>
        /// <param name="PaymentEntryID"></param>
        /// <param name="PolicyId"></param>
        /// <param name="LicenseeId"></param>
        /// <param name="PaidAmount"></param>
        public static void EntryInPolicyOutGoingPayment(bool IsPaymentToHO, Guid PaymentEntryID, Guid PolicyId, DateTime? InvoiceDate, Guid LicenseeId)
        {
            PolicySchedule _PolicySchedule = CheckForOutgoingTypeOfSchedule(PolicyId);
            PolicyDetailsData _Policy = GetPolicy(PolicyId);
            PolicyPaymentEntriesPost _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntry(PaymentEntryID);
            decimal Premium = _PolicyPaymentEntriesPost.PaymentRecived;
            decimal DollerPerUnits = _PolicyPaymentEntriesPost.DollerPerUnit;

            FirstYrRenewalYr IsFirstYr = FirstYrRenewalYr.None;


            if (IsPaymentToHO)
            {
                double Amount = Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment);
                Guid _HouseOwnerID = GetPolicyHouseOwner(LicenseeId);
                // if (_PolicySchedule == PolicySchedule.Basic)
                {

                    PolicyOutgoingDistribution _PolicyOutgoingDistribution = new PolicyOutgoingDistribution();
                    _PolicyOutgoingDistribution.OutgoingPaymentId = Guid.NewGuid();
                    _PolicyOutgoingDistribution.PaymentEntryId = PaymentEntryID;
                    _PolicyOutgoingDistribution.RecipientUserCredentialId = _HouseOwnerID;
                    _PolicyOutgoingDistribution.PaidAmount = Amount;
                    _PolicyOutgoingDistribution.IsPaid = false;
                    _PolicyOutgoingDistribution.CreatedOn = DateTime.Today;
                    PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutgoingDistribution);


                }
                //else if (_PolicySchedule == PolicySchedule.Advance)
                //{
                //    PolicyOutgoingDistribution _PolicyOutgoingDistribution = new PolicyOutgoingDistribution();
                //    _PolicyOutgoingDistribution.OutgoingPaymentId = Guid.NewGuid();
                //    _PolicyOutgoingDistribution.PaymentEntryId = PaymentEntryID;
                //    _PolicyOutgoingDistribution.RecipientUserCredentialId = _HouseOwnerID;
                //    _PolicyOutgoingDistribution.PaidAmount = Amount;
                //     _PolicyOutgoingDistribution.IsPaid = false;
                //    PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutgoingDistribution);

                //}

            }
            else
            {
                if (_PolicySchedule == PolicySchedule.Basic)
                {
                    IsFirstYr = IsUseFirstYear(InvoiceDate, PolicyId);

                    List<OutGoingPayment> _outgoingSchedule = GetBasicOutGoingScheduleOfPolicy(PolicyId);
                    //if (_outgoingSchedule == null || _outgoingSchedule.Count == 0)
                    //{
                    //    return false;
                    //}
                    if (_outgoingSchedule.FirstOrDefault().ScheduleTypeId == (int)MasterBasicOutgoinSchedule.PercentageOfCommission)
                    {
                        foreach (OutGoingPayment _OutGoingPayment in _outgoingSchedule)
                        {
                            PolicyOutgoingDistribution _PolicyOutgoingDistribution = new PolicyOutgoingDistribution();
                            _PolicyOutgoingDistribution.OutgoingPaymentId = Guid.NewGuid();
                            _PolicyOutgoingDistribution.PaymentEntryId = PaymentEntryID;
                            _PolicyOutgoingDistribution.RecipientUserCredentialId = _OutGoingPayment.PayeeUserCredentialId;
                            if (IsFirstYr == FirstYrRenewalYr.FirstYear)
                            {
                                _PolicyOutgoingDistribution.PaidAmount = double.Parse(_PolicyPaymentEntriesPost.TotalPayment.ToString()) * _OutGoingPayment.FirstYearPercentage / 100;
                                _PolicyOutgoingDistribution.Payment = _OutGoingPayment.FirstYearPercentage;
                            }
                            else if (IsFirstYr == FirstYrRenewalYr.Renewal)
                            {
                                _PolicyOutgoingDistribution.PaidAmount = double.Parse(_PolicyPaymentEntriesPost.TotalPayment.ToString()) * _OutGoingPayment.RenewalPercentage / 100;
                                _PolicyOutgoingDistribution.Payment = _OutGoingPayment.RenewalPercentage;
                            }
                            _PolicyOutgoingDistribution.CreatedOn = DateTime.Now;
                            _PolicyOutgoingDistribution.IsPaid = false;
                            _PolicyOutgoingDistribution.CreatedOn = DateTime.Today;
                            //_PolicyOutgoingDistribution.ReferencedOutgoingScheduleId = _OutGoingPayment.OutgoingScheduleId;
                            PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutgoingDistribution);
                            //   Flag = true;//Ask again
                        }

                    }
                    else if (_outgoingSchedule.FirstOrDefault().ScheduleTypeId == (int)MasterBasicOutgoinSchedule.PercentageOfPremium)
                    {


                        foreach (OutGoingPayment _OutGoingPayment in _outgoingSchedule)
                        {
                            PolicyOutgoingDistribution _PolicyOutgoingDistribution = new PolicyOutgoingDistribution();
                            _PolicyOutgoingDistribution.OutgoingPaymentId = Guid.NewGuid();
                            _PolicyOutgoingDistribution.PaymentEntryId = PaymentEntryID;
                            _PolicyOutgoingDistribution.RecipientUserCredentialId = _OutGoingPayment.PayeeUserCredentialId;
                            if (IsFirstYr == FirstYrRenewalYr.FirstYear)
                            {
                                _PolicyOutgoingDistribution.PaidAmount = double.Parse(Premium.ToString()) * _OutGoingPayment.FirstYearPercentage.Value / 100;
                                _PolicyOutgoingDistribution.Premium = _OutGoingPayment.FirstYearPercentage;
                            }
                            else if (IsFirstYr == FirstYrRenewalYr.Renewal)
                            {
                                _PolicyOutgoingDistribution.PaidAmount = double.Parse(Premium.ToString()) * _OutGoingPayment.RenewalPercentage.Value / 100;
                                _PolicyOutgoingDistribution.Premium = _OutGoingPayment.RenewalPercentage;


                            }
                            _PolicyOutgoingDistribution.CreatedOn = DateTime.Now;
                            _PolicyOutgoingDistribution.IsPaid = false;
                            //  _PolicyOutgoingDistribution.ReferencedOutgoingScheduleId = _OutGoingPayment.OutgoingScheduleId;
                            PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutgoingDistribution);

                        }
                    }

                }
                else if (_PolicySchedule == PolicySchedule.Advance)
                {
                    PolicyOutgoingSchedule _AdvanceSchedule = GetAdvanceOutgoingScheduleOfPolicy(PolicyId);
                    _AdvanceSchedule = FillNullDateWithMaxSystemDate(_AdvanceSchedule);
                    //if (_AdvanceSchedule.OutgoingScheduleList == null)
                    //    _AdvanceSchedule.OutgoingScheduleList = new List<OutgoingScheduleEntry>();

                    int AdvanceOutgoingType = _AdvanceSchedule.ScheduleTypeId;
                    // decimal calAmount = 0;
                    if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PercentageofPremium_scale)
                    {
                        List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                            .Where(p => p.EffectiveFromDate <= InvoiceDate)
                            .Where(p => p.EffectiveToDate >= InvoiceDate)
                           .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString())).ToList();

                        List<Guid> PayeeList = _OutgoingShedule.Select(p => p.PayeeUserCredentialId).Distinct().ToList();

                        foreach (Guid payee in PayeeList)
                        {

                            decimal TempPremiumAmt = Premium;
                            List<OutgoingScheduleEntry> _tempOutg = _OutgoingShedule.Where(p => p.PayeeUserCredentialId == payee).OrderBy(p => p.FromRange).ToList();
                            foreach (OutgoingScheduleEntry outg in _tempOutg)
                            {
                                PolicyOutgoingDistribution _PolicyOutgoingDistribution = new PolicyOutgoingDistribution();
                                _PolicyOutgoingDistribution.OutgoingPaymentId = Guid.NewGuid();
                                _PolicyOutgoingDistribution.PaymentEntryId = PaymentEntryID;
                                _PolicyOutgoingDistribution.RecipientUserCredentialId = outg.PayeeUserCredentialId;

                                // if (Convert.ToDecimal(outg.ToRange.Value) <= TempPremiumAmt)
                                if (TempPremiumAmt >
                               Convert.ToDecimal(outg.ToRange
                               -
                               (outg.FromRange + GetValidFromForcalCulation(outg.FromRange))
                               ))
                                {
                                    _PolicyOutgoingDistribution.PaidAmount = (outg.ToRange - (outg.FromRange + GetValidFromForcalCulation(outg.FromRange))) * (outg.Rate) / 100;
                                    _PolicyOutgoingDistribution.Premium = outg.Rate;
                                    TempPremiumAmt = TempPremiumAmt - Convert.ToDecimal(outg.ToRange.Value);
                                }
                                else
                                {
                                    _PolicyOutgoingDistribution.PaidAmount = Convert.ToDouble(TempPremiumAmt) * outg.Rate / 100;
                                    _PolicyOutgoingDistribution.Premium = outg.Rate;
                                    TempPremiumAmt = 0;
                                    break;
                                }
                                _PolicyOutgoingDistribution.IsPaid = false;
                                _PolicyOutgoingDistribution.CreatedOn = DateTime.Now;
                                // _PolicyOutgoingDistribution.ReferencedOutgoingAdvancedScheduleId = outg.CoveragesScheduleId;
                                PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutgoingDistribution);
                            }
                        }
                    }
                    else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PercentageofPremium_target)
                    {
                        //Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.TotalPayment.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.TotalPayment.ToString())).ToList().FirstOrDefault().Rate == _PolicyPaymentEntriesPost.PercentageOfPremium ? true : false;
                        List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                            .Where(p => p.EffectiveFromDate <= InvoiceDate)
                            .Where(p => p.EffectiveToDate >= InvoiceDate)
                            .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString()))
                            .Where(p => p.ToRange >= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString())).ToList();

                        foreach (OutgoingScheduleEntry outg in _OutgoingShedule)
                        {
                            PolicyOutgoingDistribution _PolicyOutgoingDistribution = new PolicyOutgoingDistribution();
                            _PolicyOutgoingDistribution.OutgoingPaymentId = Guid.NewGuid();
                            _PolicyOutgoingDistribution.PaymentEntryId = PaymentEntryID;
                            _PolicyOutgoingDistribution.RecipientUserCredentialId = outg.PayeeUserCredentialId;
                            _PolicyOutgoingDistribution.PaidAmount = double.Parse(Premium.ToString()) * outg.Rate / 100;

                            _PolicyOutgoingDistribution.Premium = outg.Rate;

                            _PolicyOutgoingDistribution.CreatedOn = DateTime.Now;
                            // _PolicyOutgoingDistribution.ReferencedOutgoingAdvancedScheduleId = outg.CoveragesScheduleId;
                            _PolicyOutgoingDistribution.IsPaid = false;
                            PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutgoingDistribution);
                        }

                    }
                    else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PerHeadFee_scale)
                    {
                        // List<OutgoingShedule> _OutgoingShedule = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();
                        List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                            .Where(p => p.EffectiveFromDate <= InvoiceDate)
                            .Where(p => p.EffectiveToDate >= InvoiceDate)
                            .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();

                        int NumberOfHead = _PolicyPaymentEntriesPost.NumberOfUnits;
                        List<Guid> PayeeList = _OutgoingShedule.Select(p => p.PayeeUserCredentialId).Distinct().ToList();

                        foreach (Guid payee in PayeeList)
                        {
                            int temoNUmberOfHead = NumberOfHead;
                            List<OutgoingScheduleEntry> _tempOutg = _OutgoingShedule.Where(p => p.PayeeUserCredentialId == payee).OrderBy(p => p.FromRange).ToList();
                            foreach (OutgoingScheduleEntry outg in _tempOutg)
                            {
                                PolicyOutgoingDistribution _PolicyOutgoingDistribution = new PolicyOutgoingDistribution();
                                _PolicyOutgoingDistribution.OutgoingPaymentId = Guid.NewGuid();
                                _PolicyOutgoingDistribution.PaymentEntryId = PaymentEntryID;
                                _PolicyOutgoingDistribution.RecipientUserCredentialId = outg.PayeeUserCredentialId;
                                // if (outg.ToRange <= Convert.ToDouble(temoNUmberOfHead))
                                if (temoNUmberOfHead > (outg.ToRange - (outg.FromRange + GetValidFromForcalCulation(outg.FromRange))))
                                {
                                    _PolicyOutgoingDistribution.PaidAmount = ((outg.ToRange - (outg.FromRange + GetValidFromForcalCulation(outg.FromRange)))) * (outg.Rate);
                                    _PolicyOutgoingDistribution.OutGoingPerUnit = Convert.ToDecimal(outg.Rate);
                                    temoNUmberOfHead = temoNUmberOfHead - Convert.ToInt32(outg.ToRange);
                                }
                                else
                                {
                                    _PolicyOutgoingDistribution.PaidAmount = temoNUmberOfHead * outg.Rate;
                                    _PolicyOutgoingDistribution.OutGoingPerUnit = Convert.ToDecimal(outg.Rate);

                                    temoNUmberOfHead = 0;
                                    break;
                                }
                                _PolicyOutgoingDistribution.IsPaid = false;
                                _PolicyOutgoingDistribution.CreatedOn = DateTime.Now;
                                // _PolicyOutgoingDistribution.ReferencedOutgoingAdvancedScheduleId = outg.CoveragesScheduleId;
                                PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutgoingDistribution);
                            }
                        }
                    }
                    else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PerHeadFee_target)
                    {

                        // Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString())).ToList().FirstOrDefault().Rate == double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString()) ? true : false;
                        List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                            .Where(p => p.EffectiveFromDate <= InvoiceDate)
                            .Where(p => p.EffectiveToDate >= InvoiceDate)
                            .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString()))
                            .Where(p => p.ToRange >= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();
                        foreach (OutgoingScheduleEntry outg in _OutgoingShedule)
                        {
                            PolicyOutgoingDistribution _PolicyOutgoingDistribution = new PolicyOutgoingDistribution();
                            _PolicyOutgoingDistribution.OutgoingPaymentId = Guid.NewGuid();
                            _PolicyOutgoingDistribution.PaymentEntryId = PaymentEntryID;
                            _PolicyOutgoingDistribution.RecipientUserCredentialId = outg.PayeeUserCredentialId;
                            _PolicyOutgoingDistribution.PaidAmount = outg.Rate * _PolicyPaymentEntriesPost.NumberOfUnits;

                            _PolicyOutgoingDistribution.OutGoingPerUnit = Convert.ToDecimal(outg.Rate);
                            _PolicyOutgoingDistribution.IsPaid = false;
                            _PolicyOutgoingDistribution.CreatedOn = DateTime.Now;
                            //_PolicyOutgoingDistribution.ReferencedOutgoingAdvancedScheduleId = outg.CoveragesScheduleId;
                            PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutgoingDistribution);
                        }

                    }
                    else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.FlatDollar_modal)
                    {
                        //Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.Rate == double.Parse(_PolicyPaymentEntriesPost.Fee.ToString())).ToList().FirstOrDefault().Rate == double.Parse(_PolicyPaymentEntriesPost.Fee.ToString()) ? true : false;
                        List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                            .Where(p => p.EffectiveFromDate <= InvoiceDate)
                            .Where(p => p.EffectiveToDate >= InvoiceDate).ToList();
                        foreach (OutgoingScheduleEntry outg in _OutgoingShedule)
                        {
                            PolicyOutgoingDistribution _PolicyOutgoingDistribution = new PolicyOutgoingDistribution();
                            _PolicyOutgoingDistribution.OutgoingPaymentId = Guid.NewGuid();
                            _PolicyOutgoingDistribution.PaymentEntryId = PaymentEntryID;
                            _PolicyOutgoingDistribution.RecipientUserCredentialId = outg.PayeeUserCredentialId;
                            _PolicyOutgoingDistribution.PaidAmount = outg.Rate;
                            _PolicyOutgoingDistribution.IsPaid = false;
                            _PolicyOutgoingDistribution.CreatedOn = DateTime.Now;
                            // _PolicyOutgoingDistribution.ReferencedOutgoingAdvancedScheduleId = outg.CoveragesScheduleId;
                            PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutgoingDistribution);
                        }

                    }
                }
                //else
                //{
                //}
                //List<OutgoingShedule> _OutgoingSheduleLst = GetAllPayeeForDistributeAmount(_DeuEntry.PolicyId);

            }
            //throw new NotImplementedException();
        }

        private static PolicyOutgoingSchedule GetAllPayeeForDistributeAmount(Guid PolicyId)
        {
            return OutgoingSchedule.GetPolicyOutgoingSchedule(PolicyId);
        }



        public static Guid GetPolicyHouseOwner(Guid PolicyLicenID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                Guid UserCredID = (from f in DataModel.UserCredentials

                                   where (f.LicenseeId == PolicyLicenID)
                                   select f).Where(f => f.IsHouseAccount == true).FirstOrDefault().UserCredentialId;
                return UserCredID;

            }

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>PolicyEntryID</returns>
        // private static Guid EntryInPoicyPamentEntries(DEUFields deuFields, Guid FollowUpIssueID, PostStatus PostStatusID, Policy _Policy, Guid CreatedBy)
        // private static Guid EntryInPoicyPamentEntries(DEUFields deuFields, PostStatus PostStatusID, Policy _Policy, Guid CreatedBy)
        private static Guid EntryInPoicyPamentEntries(DEUFields deuFields, BasicInformationForProcess _BasicInformationForProcess)
        {
            Guid PolicyEnteryID = Guid.NewGuid();
            Guid FollowUpID = Guid.Empty;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                var _policyentry = (from p in DataModel.PolicyPaymentEntries where (p.PaymentEntryId == PolicyEnteryID) select p).FirstOrDefault();
                if (_policyentry == null)
                {
                    _policyentry = new DLinq.PolicyPaymentEntry();
                    _policyentry.PaymentEntryId = PolicyEnteryID;
                    _policyentry.StatementReference.Value = (from f in DataModel.Statements where f.StatementId == deuFields.StatementId select f).FirstOrDefault();
                    _policyentry.PolicyReference.Value = (from f in DataModel.Policies where f.PolicyId == _BasicInformationForProcess.PolicyId select f).FirstOrDefault();

                    //_policyentry.CreatedOn = DateTime.Today;
                    //_policyentry.CreatedBy = deuFields.CurrentUser;
                    //_policyentry.MasterPostStatuReference.Value = (from f in DataModel.MasterPostStatus where f.PostStatusID == (int)_BasicInformationForProcess.PostStatus select f).FirstOrDefault();
                    //////////////////////////////////////
                    _policyentry.InvoiceDate = deuFields.DeuData.InvoiceDate;
                    _policyentry.PaymentRecived = deuFields.DeuData.PaymentRecived;
                    _policyentry.CommissionPercentage = deuFields.DeuData.CommissionPercentage;
                    _policyentry.NumberOfUnits = deuFields.DeuData.NoOfUnits;
                    _policyentry.Fee = deuFields.DeuData.Fee;
                    _policyentry.SplitPercentage = deuFields.DeuData.SplitPer;

                    _policyentry.TotalPayment = deuFields.DeuData.CommissionTotal;
                    _policyentry.CreatedOn = DateTime.Today;
                    _policyentry.CreatedBy = deuFields.CurrentUser;
                    _policyentry.MasterPostStatuReference.Value = (from f in DataModel.MasterPostStatus where f.PostStatusID == (int)_BasicInformationForProcess.PostStatus select f).FirstOrDefault();//Fill the Data in Master Post Status
                    _policyentry.Bonus = deuFields.DeuData.Bonus;

                    _policyentry.DollerPerUnit = deuFields.DeuData.DollerPerUnit;
                    _policyentry.DEUEntryId = deuFields.DeuData.DEUENtryID;



                    DataModel.AddToPolicyPaymentEntries(_policyentry);
                }
                DataModel.SaveChanges();
            }

            return PolicyEnteryID;
        }

        public static Guid CreateNewPendingPolicy(DEUFields deuFields)
        {
            Guid NewPendingPolicyID = Guid.NewGuid();

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                var _policy = (from p in DataModel.Policies where (p.PolicyId == NewPendingPolicyID) select p).FirstOrDefault();
                if (_policy == null)
                {
                    _policy = new DLinq.Policy();
                    _policy.PolicyId = NewPendingPolicyID;
                    _policy.PolicyStatusId = (int)_PolicyStatus.Pending;
                    _policy.PolicyNumber = deuFields.DeuData.PolicyNumber;
                    _policy.PolicyModeId = (int)MasterPolicyMode.Monthly;
                    _policy.ClientReference.Value = CreateNewClientForPendingPolicy(deuFields.DeuData, DataModel, deuFields.LicenseeId.Value);
                    _policy.Insured = string.IsNullOrEmpty(deuFields.DeuData.Insured) ? _policy.Client.Name : deuFields.DeuData.Insured;
                    _policy.LicenseeReference.Value = (from f in DataModel.Licensees where f.LicenseeId == deuFields.LicenseeId select f).FirstOrDefault();
                    _policy.CarrierId = deuFields.DeuData.CarrierID;
                    _policy.CoverageId = deuFields.DeuData.CoverageID;
                    _policy.CreatedOn = DateTime.Today;
                    _policy.PayorId = deuFields.DeuData.PayorId;
                    _policy.CreatedBy = GetPolicyHouseOwner(deuFields.LicenseeId.Value);
                    _policy.IsTrackPayment = true;
                    _policy.PolicyType = "New";
                    _policy.SplitPercentage = 100;
                    _policy.IncomingPaymentTypeId = (int)MasterIncoimgPaymentType.commission;
                    Carrier carr = Carrier.GetPayorCarrier(_policy.PayorId.Value, _policy.CarrierId.Value);
                    _policy.IsTrackIncomingPercentage = carr.IsTrackIncomingPercentage;
                    _policy.IsTrackMissingMonth = carr.IsTrackMissingMonth;
                    DataModel.AddToPolicies(_policy);

                }
                DataModel.SaveChanges();
                PolicyToLearnPost.AddUpdatPolicyToLearn(_policy.PolicyId);
                LearnedToPolicyPost.AddUpdateLearnedToPolicy(_policy.PolicyId);

                Policy.AddUpdatePolicyHistory(_policy.PolicyId);
                PolicyLearnedField.AddUpdateHistoryLearned(_policy.PolicyId);
            }
            return NewPendingPolicyID;
        }

        private static DLinq.Client CreateNewClientForPendingPolicy(DEU dEU, DLinq.CommissionDepartmentEntities DataModel, Guid licenceId)
        {
            Guid ClientId;

            if (dEU.ClientID != null && dEU.ClientID != Guid.Empty)
            {
                ClientId = dEU.ClientID.Value;
            }
            else
            {
                Client _client = new Client();
                ClientId = Guid.NewGuid();
                _client.ClientId = ClientId;
                _client.Address = "";
                _client.Zip = "";
                _client.State = "";
                _client.Name = String.IsNullOrEmpty(dEU.ClientName) == true ? dEU.PolicyNumber : dEU.ClientName;
                _client.IsDeleted = false;
                _client.City = "";
                _client.Email = "";
                _client.LicenseeId = licenceId;
                _client.AddUpdate();
            }
            return (from f in DataModel.Clients where f.ClientId == ClientId select f).FirstOrDefault();
        }

        public static PolicyDetailsData GetPolicyTrackPayment(Guid SePolicyID)
        {
            PolicyDetailsData objPolicyDetailsData = new PolicyDetailsData();
            DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
            EntityConnection ec = (EntityConnection)ctx.Connection;
            SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
            string adoConnStr = sc.ConnectionString;

            using (SqlConnection con = new SqlConnection(adoConnStr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetPolicyTrackFromDate", con))
                {
                    cmd.CommandType =System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PolicyId", SePolicyID);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    // Call Read before accessing data. 
                    while (reader.Read())
                    {

                        try
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["TrackFromDate"])))
                            {

                                objPolicyDetailsData.OriginalEffectiveDate = Convert.ToDateTime(reader["TrackFromDate"]);
                            }
                        }
                        catch
                        {
                        }

                    }

                }
            }
            return objPolicyDetailsData;
        }

        public static PolicyDetailsData GetPolicy(Guid SePolicyID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PolicyId", SePolicyID);
            return Policy.GetPolicyData(parameters).FirstOrDefault();
        }


        #region FollowUPProcess
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsTrackPayment"></param>
        /// <param name="_PaymentType"></param>
        /// <param name="policy"></param>
        public static void FollowUpProcedure(bool IsTrackPayment, PaymentMode _PaymentType, Guid PolicyID, DateTime InvoiceDate, Guid PaymentEntryId)
        {
            if (IsTrackPayment)
            {

                List<FollowUpDate> _FollowUpDate = CalculateFollowUpDateRange(_PaymentType, PolicyID);

                List<PolicyPaymentEntriesPost> _PolicyPaymentEntries = PolicyPaymentEntriesPost.GetAllPaymentEntriesOfRange(_FollowUpDate.FirstOrDefault().FromDate.Value, _FollowUpDate.FirstOrDefault().ToDate.Value, PolicyID);

                //For Missing Payment
                if (_PolicyPaymentEntries.Count == 0)
                {
                    RegisterIssueAgainstMissingPayment(PolicyID, InvoiceDate);
                    AutoPolicyTerminateProcess(_PaymentType, _FollowUpDate, PolicyID);

                }

                // else if (CheckForIncomingScheduleVariance(PolicyID, InvoiceDate, PaymentEntryId, null))//It is to modify
                {

                    RegisterIssueAgainstScheduleVariance(PolicyID, InvoiceDate);

                }

            }
        }


        /// <summary>
        /// It calculate the Category of Issue for missing month
        /// </summary>
        /// <param name="PolicyID"></param>
        /// <param name="MissingMonth"></param>
        /// <returns></returns>
        private static FollowUpIssueCategory CalculateIssueCategoryForMissingMonth(Guid PolicyID, DateTime InvoiceDate)
        {
            PolicyDetailsData _Policy = GetPolicy(PolicyID);

            if (_Policy.OriginalEffectiveDate.Value.Month == InvoiceDate.Month)
            {
                return FollowUpIssueCategory.MissFirst;
            }
            else
            {
                return FollowUpIssueCategory.MissInv;
            }
        }

        private static FollowUpIssueCategory CalculateIssueCategoryForSchedule(Guid PolicyID, DateTime Invoicedate)
        {
            bool ISScheduleMisMatch = false;

            ISScheduleMisMatch = CheckForScheduleMatches();
            if (ISScheduleMisMatch)
            {
                return FollowUpIssueCategory.VarSchedule;
            }
            return FollowUpIssueCategory.NotAnyIssue;
        }
        /// <summary>
        /// 000
        /// </summary>
        /// <param name="PolicyId"></param>
        private static void RegisterIssueAgainstMissingPayment(Guid PolicyId, DateTime InvoiceDate)
        {
            DisplayFollowupIssue _FollowUpIssue = new DisplayFollowupIssue();
            FollowUpIssueCategory _FollowUpIssueCategory = CalculateIssueCategoryForSchedule(PolicyId, InvoiceDate) != FollowUpIssueCategory.NotAnyIssue ? CalculateIssueCategoryForSchedule(PolicyId, InvoiceDate) : CalculateIssueCategoryForMissingMonth(PolicyId, InvoiceDate);
            _FollowUpIssue.IssueId = Guid.NewGuid();
            _FollowUpIssue.IssueStatusId = (int)FollowUpIssueStatus.Open;
            _FollowUpIssue.IssueCategoryID = (int)_FollowUpIssueCategory;
            _FollowUpIssue.IssueResultId = (int)FollowUpResult.Pending;
            _FollowUpIssue.IssueReasonId = (int)FollowUpIssueReason.Pending;
            _FollowUpIssue.InvoiceDate = InvoiceDate;
            //_FollowUpIssue.NextFollowupDate=
            //_FollowUpIssue.Payment=   //it is calculated by some formula
            _FollowUpIssue.PolicyId = PolicyId;
            _FollowUpIssue.PreviousStatusId = (int)FollowUpIssueStatus.Open;
            FollowupIssue.AddUpdate(_FollowUpIssue);//Check this function is working properly or not

        }
        private void RemoveIssueAgainstMissingPayment(Guid PolicyID, PaymentMode _PaymentType)
        {
            PolicyToolIncommingShedule _Schedule;
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntries;
            // List<FollowUpDate> _FollowUpDate = CalculateFollowUpDateRange(_PaymentType, PolicyID);
            List<DisplayFollowupIssue> _FollowUpIssue = FollowupIssue.GetIssues(PolicyID);
            List<DisplayFollowupIssue> _FollowUpIssueMissFrist = _FollowUpIssue.FindAll(p => p.IssueCategoryID == (int)FollowUpIssueCategory.MissFirst);
            List<DisplayFollowupIssue> _FollowUpIssueMissInv = _FollowUpIssue.FindAll(p => p.IssueCategoryID == (int)FollowUpIssueCategory.MissInv);
            _FollowUpIssue = new List<DisplayFollowupIssue>();
            _FollowUpIssue.AddRange(_FollowUpIssueMissFrist);
            _FollowUpIssue.AddRange(_FollowUpIssueMissInv);
            if (_FollowUpIssue.Count == 0)
                return;
            _Schedule = GetBasicIncomingScheduleOfPolicy(PolicyID);//Right now i am checking for simple schedule ,,for advance schedule i did al these thing later
            PolicyToolIncommingShedule _incomingSchedule = _Schedule;
            foreach (DisplayFollowupIssue flwis in _FollowUpIssue)
            {
                _PolicyPaymentEntries = PolicyPaymentEntriesPost.GetAllPaymentEntriesOfRange(flwis.InvoiceDate.Value, flwis.InvoiceDate.Value, PolicyID);
                if (_PolicyPaymentEntries.Count == 0)
                {
                    return;
                }
                else
                {
                    flwis.IssueId = flwis.IssueId;
                    flwis.IssueStatusId = (int)FollowUpIssueStatus.Closed;
                    flwis.IssueCategoryID = flwis.IssueCategoryID;
                    flwis.IssueResultId = (int)FollowUpResult.Resolved_CD;
                    flwis.IssueReasonId = flwis.IssueReasonId;
                    flwis.InvoiceDate = flwis.InvoiceDate;
                    //flwis.NextFollowupDate=
                    //flwis.Payment=   //it is calculated by some formula
                    flwis.PolicyId = flwis.PolicyId;
                    flwis.PreviousStatusId = flwis.PreviousStatusId;
                    FollowupIssue.AddUpdate(flwis);//Check this function is working properly or not
                    if (GetBasicSchedulePercentageForCalculation(PolicyID, flwis.InvoiceDate.Value) != (double)_PolicyPaymentEntries.FirstOrDefault().CommissionPercentage)
                    {
                        RegisterIssueAgainstScheduleVariance(PolicyID, flwis.InvoiceDate.Value);//--Test
                    }


                }

            }

        }
        private static double? GetBasicSchedulePercentageForCalculation(Guid PolicyId, DateTime InvoiceDate)
        {
            if (GetPolicy(PolicyId).OriginalEffectiveDate == InvoiceDate)
            {
                return GetBasicIncomingScheduleOfPolicy(PolicyId).FirstYearPercentage;
            }
            else
            {
                return GetBasicIncomingScheduleOfPolicy(PolicyId).RenewalPercentage;
            }
        }
        private void RemoveIssueAgainstChangeInSchedule(Guid PolicyId)
        {
        }
        private void RegisterIssueAgainstVarCompDue()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 000
        /// </summary>
        /// <param name="PolicyId"></param>
        private static void RegisterIssueAgainstScheduleVariance(Guid PolicyId, DateTime InvoiceDate)
        {
            bool IsEffectiveDateAvail;
            bool IsFirstYearAndRenewalEqual;
            bool IsSchedulePerAvail;
            IsEffectiveDateAvail = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId).Effective != null ? false : true;
            IsFirstYearAndRenewalEqual = CheckForFirstYearAndRenewal(PolicyId);//Create Funtion for that in PolicyToolIncomingSchedule
            IsSchedulePerAvail = CheckForPercentageAvilable(PolicyId);
            if ((!IsEffectiveDateAvail && !IsFirstYearAndRenewalEqual) || (!IsSchedulePerAvail))
            {
                return;
            }

            DisplayFollowupIssue _FollowUpIssue = new DisplayFollowupIssue();
            _FollowUpIssue.IssueId = Guid.NewGuid();
            _FollowUpIssue.IssueStatusId = (int)FollowUpIssueStatus.Open;
            _FollowUpIssue.IssueCategoryID = (int)FollowUpIssueCategory.VarSchedule;
            _FollowUpIssue.IssueResultId = (int)FollowUpResult.Pending;
            _FollowUpIssue.IssueReasonId = (int)FollowUpIssueReason.Pending;
            _FollowUpIssue.InvoiceDate = InvoiceDate;
            //_FollowUpIssue.NextFollowupDate=
            //_FollowUpIssue.Payment=   //it is calculated by some formula
            _FollowUpIssue.PolicyId = PolicyId;
            _FollowUpIssue.PreviousStatusId = (int)FollowUpIssueStatus.Open;
            FollowupIssue.AddUpdate(_FollowUpIssue);//Check this function is working properly or not
        }

        private static bool CheckForFirstYearAndRenewal(Guid PolicyId)
        {
            double FYP = GetBasicIncomingScheduleOfPolicy(PolicyId).FirstYearPercentage.Value;
            double RP = GetBasicIncomingScheduleOfPolicy(PolicyId).RenewalPercentage.Value;
            return (FYP == RP ? true : false);
        }

        private static bool CheckForPercentageAvilable(Guid PolicyId)
        {
            double FYP = GetBasicIncomingScheduleOfPolicy(PolicyId).FirstYearPercentage.Value;
            return ((FYP == null || FYP == 0) ? false : true);
        }

        private bool CheckForVarianceinDoller()
        {
            throw new NotImplementedException();
        }
        private static bool CheckForScheduleMatches()
        {
            bool Flag = false;

            return Flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PolicyId"></param>
        /// <param name="InvoiceDate"></param>
        /// <param name="EntryId"></param>
        /// <param name="_TempPolicyPaymentEntriesPost"></param>
        /// <returns></returns>
        private static bool CheckForIncomingScheduleVariance(PolicyPaymentEntriesPost _TempPolicyPaymentEntriesPost)
        //private static bool CheckForIncomingScheduleVariance(Guid PolicyId, DateTime? InvoiceDate, Guid EntryId, PolicyPaymentEntriesPost _TempPolicyPaymentEntriesPost)
        {
            double TruncationError = Convert.ToDouble(Masters.SystemConstant.GetKeyValue("TruncationError"));


            bool Flag = false;
            PolicySchedule _PolicySchedule = CheckForIncomingTypeOfSchedule(_TempPolicyPaymentEntriesPost.PolicyID);
            PolicyDetailsData _Policy = GetPolicy(_TempPolicyPaymentEntriesPost.PolicyID);
            double SplitPer = _Policy.SplitPercentage ?? 100;
            PolicyPaymentEntriesPost _PolicyPaymentEntriesPost = _TempPolicyPaymentEntriesPost;

            //if (EntryId == null || EntryId == Guid.Empty)
            //{
            //    _PolicyPaymentEntriesPost = _TempPolicyPaymentEntriesPost;
            //}
            //else
            //{
            //    _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntry(EntryId);
            //}
            decimal Premium = _PolicyPaymentEntriesPost.PaymentRecived;
            FirstYrRenewalYr IsFirstYr = FirstYrRenewalYr.None;

            if (_PolicySchedule == PolicySchedule.None)
            {
                Flag = false;
            }
            else if (_PolicySchedule == PolicySchedule.Basic)
            {
                IsFirstYr = IsUseFirstYear(_PolicyPaymentEntriesPost.InvoiceDate, _PolicyPaymentEntriesPost.PolicyID);
                if (IsFirstYr == FirstYrRenewalYr.None)
                {
                    Flag = false;
                    return Flag;
                }
                PolicyToolIncommingShedule _Schedule;

                _Schedule = GetBasicIncomingScheduleOfPolicy(_PolicyPaymentEntriesPost.PolicyID);
                PolicyToolIncommingShedule _incomingSchedule = _Schedule;
                if (_incomingSchedule == null || _incomingSchedule.IncomingScheduleId == Guid.Empty) return Flag;//14-mar-2011 

                if (_incomingSchedule.ScheduleTypeId == (int)MasterBasicIncomingSchedule.PercentageOfPremium)
                {


                    if (IsFirstYr == FirstYrRenewalYr.FirstYear)
                    {
                        double finalres = (Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment)
                            - Convert.ToDouble(Premium) * _incomingSchedule.FirstYearPercentage * SplitPer / 10000).Value;
                        if (Math.Abs(finalres) <= TruncationError)
                        {
                            Flag = true;
                        }
                    }
                    else
                    {
                        double finalres = (Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment)
                                            - Convert.ToDouble(Premium) * _incomingSchedule.RenewalPercentage * SplitPer / 10000).Value;
                        if (Math.Abs(finalres) <= TruncationError)
                        {
                            Flag = true;
                        }
                    }
                }
                else if (_incomingSchedule.ScheduleTypeId == (int)MasterBasicIncomingSchedule.PerHead)
                {
                    //if (IsFirstYr == FirstYrRenewalYr.FirstYear ?
                    //    Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment) ==
                    //    _PolicyPaymentEntriesPost.NumberOfUnits * _incomingSchedule.FirstYearPercentage * SplitPer / 100
                    //    :
                    //    Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment) ==
                    //   _PolicyPaymentEntriesPost.NumberOfUnits * _incomingSchedule.RenewalPercentage * SplitPer / 100)
                    //{
                    //    Flag = true;
                    //}

                    if (IsFirstYr == FirstYrRenewalYr.FirstYear)
                    {
                        double finalres = (Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment) -
                                          _PolicyPaymentEntriesPost.NumberOfUnits * _incomingSchedule.FirstYearPercentage * SplitPer / 100).Value;

                        if (Math.Abs(finalres) <= TruncationError)
                        {
                            Flag = true;
                        }
                    }
                    else
                    {
                        double finalres = (Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment) -
                                          _PolicyPaymentEntriesPost.NumberOfUnits * _incomingSchedule.RenewalPercentage * SplitPer / 100).Value;

                        if (Math.Abs(finalres) <= TruncationError)
                        {
                            Flag = true;
                        }
                    }
                }
            }
            else if (_PolicySchedule == PolicySchedule.Advance)
            {
                decimal calAmount = 0;

                PolicyIncomingSchedule _AdvanceSchedule = GetAdvanceIncomingScheduleOfPolicy(_PolicyPaymentEntriesPost.PolicyID);
                if (_AdvanceSchedule == null || _AdvanceSchedule.IncomingScheduleList == null || _AdvanceSchedule.IncomingScheduleList.Count == 0) return Flag;
                int AdvanceIncomingType = _AdvanceSchedule.ScheduleTypeId;
                _AdvanceSchedule = FillNullDateWithMaxSystemDate(_AdvanceSchedule);
                //Percentage Of Premium --Scale
                if (AdvanceIncomingType == (int)MasterAdvanceScheduleType.PercentageofPremium_scale)
                {
                    double tempPremium = Convert.ToDouble(Premium);
                    double? incomingperimum = 0;
                    List<IncomingScheduleEntry> _InComingShedule = _AdvanceSchedule.IncomingScheduleList
                        .Where(p => p.EffectiveFromDate <= _PolicyPaymentEntriesPost.InvoiceDate)
                        .Where(p => p.EffectiveToDate >= _PolicyPaymentEntriesPost.InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString())).OrderBy(p => p.FromRange).ToList();
                    foreach (IncomingScheduleEntry intg in _InComingShedule)
                    {
                        if (tempPremium >
                            //(intg.ToRange - (intg.FromRange + GetValidFromForcalCulation(intg.FromRange)))
                            ((intg.ToRange - intg.FromRange) + GetValidFromForcalCulation(intg.FromRange))
                            )
                        {
                            incomingperimum += Convert.ToDouble((intg.ToRange - intg.FromRange) + GetValidFromForcalCulation(intg.FromRange)) * intg.Rate * SplitPer / 10000;
                            tempPremium = tempPremium - Convert.ToDouble(intg.ToRange);
                        }
                        else
                        {
                            incomingperimum += tempPremium * intg.Rate * SplitPer / 10000;
                            tempPremium = 0;
                            break;
                        }
                    }


                    //  if (incomingperimum == Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment))
                    double finalres = (incomingperimum - Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment)).Value;
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                //Percentage Of Target--target
                else if (AdvanceIncomingType == (int)MasterAdvanceScheduleType.PercentageofPremium_target)
                {
                    double? perimumofper = 0;
                    // Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.TotalPayment.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.TotalPayment.ToString())).ToList().FirstOrDefault().Rate == _PolicyPaymentEntriesPost.PercentageOfPremium ? true : false;
                    List<IncomingScheduleEntry> _InComingShedule = _AdvanceSchedule.IncomingScheduleList
                        .Where(p => p.EffectiveFromDate <= _PolicyPaymentEntriesPost.InvoiceDate)
                        .Where(p => p.EffectiveToDate >= _PolicyPaymentEntriesPost.InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString()))
                        .Where(p => p.ToRange >= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString())).ToList();
                    foreach (IncomingScheduleEntry intg in _InComingShedule)
                    {
                        perimumofper += Convert.ToDouble(Premium) * intg.Rate * SplitPer / 10000;
                    }


                    //if (perimumofper == Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment))
                    double finalres = (perimumofper - Convert.ToDouble(_PolicyPaymentEntriesPost.TotalPayment)).Value;

                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }

                    //PerHead--Scale
                else if (AdvanceIncomingType == (int)MasterAdvanceScheduleType.PerHeadFee_scale)
                {
                    // List<OutgoingShedule> _OutgoingShedule = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();
                    List<IncomingScheduleEntry> _InComingShedule = _AdvanceSchedule.IncomingScheduleList
                        .Where(p => p.EffectiveFromDate <= _PolicyPaymentEntriesPost.InvoiceDate)
                        .Where(p => p.EffectiveToDate >= _PolicyPaymentEntriesPost.InvoiceDate)
                        .Where(p => p.FromRange <= Convert.ToDouble(_PolicyPaymentEntriesPost.NumberOfUnits)).ToList();
                    int TempHead = _PolicyPaymentEntriesPost.NumberOfUnits;

                    foreach (IncomingScheduleEntry intg in _InComingShedule)
                    {
                        if (TempHead > (intg.ToRange - (intg.FromRange + GetValidFromForcalCulation(intg.FromRange))))
                        {
                            calAmount += Convert.ToDecimal(intg.ToRange - (intg.FromRange + GetValidFromForcalCulation(intg.FromRange))) * Convert.ToDecimal(intg.Rate) * Convert.ToDecimal(SplitPer) / 100;
                            TempHead = TempHead - Convert.ToInt32(intg.ToRange);
                        }
                        else
                        {
                            calAmount += TempHead * Convert.ToDecimal(intg.Rate) * Convert.ToDecimal(SplitPer) / 100;
                            TempHead = 0;
                            break;
                        }

                    }

                    // if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                else if (AdvanceIncomingType == (int)MasterAdvanceScheduleType.PerHeadFee_target)
                {
                    //Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString())).ToList().FirstOrDefault().Rate == double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString()) ? true : false;
                    List<IncomingScheduleEntry> _InComingShedule = _AdvanceSchedule.IncomingScheduleList
                        .Where(p => p.EffectiveFromDate <= _PolicyPaymentEntriesPost.InvoiceDate)
                        .Where(p => p.EffectiveToDate >= _PolicyPaymentEntriesPost.InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString()))
                        .Where(p => p.ToRange >= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();
                    foreach (IncomingScheduleEntry outg in _InComingShedule)
                    {
                        calAmount += _PolicyPaymentEntriesPost.NumberOfUnits * Convert.ToDecimal(outg.Rate.ToString()) * Convert.ToDecimal(SplitPer) / 100;
                    }

                    //if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                else if (AdvanceIncomingType == (int)MasterAdvanceScheduleType.FlatDollar_modal)
                {
                    // Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.Rate == double.Parse(_PolicyPaymentEntriesPost.Fee.ToString())).ToList().FirstOrDefault().Rate == double.Parse(_PolicyPaymentEntriesPost.Fee.ToString()) ? true : false;
                    List<IncomingScheduleEntry> _InComingShedule =
                        _AdvanceSchedule.IncomingScheduleList.Where(p => p.EffectiveFromDate <= _PolicyPaymentEntriesPost.InvoiceDate).
                        Where(p => p.EffectiveToDate >= _PolicyPaymentEntriesPost.InvoiceDate).ToList();
                    foreach (IncomingScheduleEntry outg in _InComingShedule)
                    {
                        calAmount += Convert.ToDecimal(outg.Rate) * Convert.ToDecimal(SplitPer) / 100;
                    }
                    // if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }

            }
            return Flag;
        }

        public static PolicyIncomingSchedule FillNullDateWithMaxSystemDate(PolicyIncomingSchedule _AdvanceSchedule)
        {
            foreach (IncomingScheduleEntry ise in _AdvanceSchedule.IncomingScheduleList)
            {
                if (ise.EffectiveToDate.HasValue == false || ise.EffectiveToDate == null)
                {
                    ise.EffectiveToDate = DateTime.MaxValue;
                }
            }
            return _AdvanceSchedule;
        }

        public static double? GetValidFromForcalCulation(double? FromRange)
        {
            double result = 0;

            try
            {
                if (FromRange == 0)
                    result = 0;
                else
                    result = 1;
            }
            catch
            {
                result = 0;
            }
            return result;

        }
        public static FirstYrRenewalYr IsUseFirstYear(DateTime? _invoicedate, Guid PolicyId)
        {
            FirstYrRenewalYr Flag = FirstYrRenewalYr.None;
            PolicyDetailsData _policy = GetPolicy(PolicyId);
            PolicyToolIncommingShedule _PolicyToolIncommingShedule = GetBasicIncomingScheduleOfPolicy(PolicyId);
            double FirstYr = _PolicyToolIncommingShedule.FirstYearPercentage ?? 0;
            double Renewal = _PolicyToolIncommingShedule.RenewalPercentage ?? 0;
            DateTime? EffDate = _policy.OriginalEffectiveDate;
            if (EffDate != null)
            {
                if ((_invoicedate >= _policy.OriginalEffectiveDate.Value) && (_invoicedate <= _policy.OriginalEffectiveDate.Value.AddYears(1)))
                {
                    Flag = FirstYrRenewalYr.FirstYear;
                }
                else if (_invoicedate > _policy.OriginalEffectiveDate.Value.AddYears(1))
                {
                    Flag = FirstYrRenewalYr.Renewal;
                }
            }
            else
            {
                if (FirstYr != Renewal)
                {
                    Flag = FirstYrRenewalYr.None;
                }
                else
                {
                    Flag = FirstYrRenewalYr.FirstYear;//Use any here first year and renewal both are equal
                }
            }

            return Flag;
        }
        public static bool CheckForOutgoingScheduleVariance(Guid PaymentEntryId)
        // public static bool CheckForOutgoingScheduleVariance(Guid PolicyId, DateTime InvoiceDate, Guid PaymentEntryId)
        {

            double TruncationError = Convert.ToDouble(Masters.SystemConstant.GetKeyValue("TruncationError"));

            bool Flag = false;
            PolicyPaymentEntriesPost _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntry(PaymentEntryId);
            decimal Premium = _PolicyPaymentEntriesPost.PaymentRecived;
            decimal DollerPerUnits = _PolicyPaymentEntriesPost.DollerPerUnit;
            decimal TotalPayment = _PolicyPaymentEntriesPost.TotalPayment;
            DateTime InvoiceDate = _PolicyPaymentEntriesPost.InvoiceDate.Value;

            Guid PolicyId = _PolicyPaymentEntriesPost.PolicyID;
            PolicySchedule _PolicySchedule = CheckForOutgoingTypeOfSchedule(PolicyId);
            PolicyDetailsData _Policy = GetPolicy(PolicyId);

            FirstYrRenewalYr IsFirstYr = FirstYrRenewalYr.None;


            if (_PolicySchedule == PolicySchedule.None)
            {
                Flag = false;
            }
            else if (_PolicySchedule == PolicySchedule.Basic)
            {
                IsFirstYr = IsUseFirstYear(InvoiceDate, PolicyId);
                if (IsFirstYr == FirstYrRenewalYr.None)
                {
                    Flag = false;
                    return Flag;
                }
                List<OutGoingPayment> _Schedule;
                // decimal calamount = 0;
                _Schedule = GetBasicOutGoingScheduleOfPolicy(PolicyId);
                List<OutGoingPayment> _outgoingSchedule = _Schedule;
                if (_outgoingSchedule.FirstOrDefault().ScheduleTypeId == (int)MasterBasicOutgoinSchedule.PercentageOfCommission)
                {
                    Flag = true;//After dicuss
                    //decimal calAmount = 0;
                    //foreach (OutGoingPayment _OutGoingPayment in _outgoingSchedule)
                    //{
                    //    if (IsFirstYr == FirstYrRenewalYr.FirstYear)
                    //    {
                    //        calAmount += TotalPayment * decimal.Parse(_OutGoingPayment.FirstYearPercentage.Value.ToString()) / 100;
                    //    }
                    //    else if (IsFirstYr == FirstYrRenewalYr.Renewal)
                    //    {
                    //        calAmount += TotalPayment * decimal.Parse(_OutGoingPayment.RenewalPercentage.Value.ToString()) / 100;

                    //    }

                    //}
                    //double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    //if (Math.Abs(finalres) <= TruncationError)
                    //{
                    //    Flag = true;
                    //}

                }
                else if (_outgoingSchedule.FirstOrDefault().ScheduleTypeId == (int)MasterBasicOutgoinSchedule.PercentageOfPremium)
                {
                    decimal calAmount = 0;
                    foreach (OutGoingPayment _OutGoingPayment in _outgoingSchedule)
                    {
                        if (IsFirstYr == FirstYrRenewalYr.FirstYear)
                        {
                            calAmount += Premium * decimal.Parse(_OutGoingPayment.FirstYearPercentage.Value.ToString()) / 100;
                        }
                        else if (IsFirstYr == FirstYrRenewalYr.Renewal)
                        {
                            calAmount += Premium * decimal.Parse(_OutGoingPayment.RenewalPercentage.Value.ToString()) / 100;

                        }
                    }

                    // if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                //if (GetBasicSchedulePercentageForCalculation(PolicyId, InvoiceDate) != (double)PolicyPaymentEntriesPost.GetAllPaymentEntriesOfRange(InvoiceDate, InvoiceDate, PolicyId).FirstOrDefault().CommissionPercentage)
                //{
                //    Flag = true;
                //}
                //Flag = false;
            }
            else if (_PolicySchedule == PolicySchedule.Advance)
            {
                PolicyOutgoingSchedule _AdvanceSchedule = GetAdvanceOutgoingScheduleOfPolicy(PolicyId);
                int AdvanceOutgoingType = _AdvanceSchedule.ScheduleTypeId;
                decimal calAmount = 0;

                _AdvanceSchedule = FillNullDateWithMaxSystemDate(_AdvanceSchedule);
                ///////////////////
                if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PercentageofPremium_scale)
                {
                    List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList.Where(p => p.EffectiveFromDate <= InvoiceDate)
                        .Where(p => p.EffectiveToDate >= InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString())).ToList();

                    List<Guid> PayeeList = _OutgoingShedule.Select(p => p.PayeeUserCredentialId).Distinct().ToList();
                    calAmount = 0;
                    foreach (Guid payee in PayeeList)
                    {

                        decimal TempPremiumAmt = Premium;
                        List<OutgoingScheduleEntry> _tempOutg = _OutgoingShedule.Where(p => p.PayeeUserCredentialId == payee).OrderBy(p => p.FromRange).ToList();
                        foreach (OutgoingScheduleEntry outg in _tempOutg)
                        {
                            //if (Convert.ToDecimal(outg.ToRange.Value) <= TempPremiumAmt)
                            if (TempPremiumAmt >
                                Convert.ToDecimal(outg.ToRange
                                -
                                (outg.FromRange + GetValidFromForcalCulation(outg.FromRange))
                                ))
                            {
                                calAmount += Convert.ToDecimal(outg.ToRange - (outg.FromRange + GetValidFromForcalCulation(outg.FromRange))) * Convert.ToDecimal(outg.Rate) / 100;
                                TempPremiumAmt = TempPremiumAmt - Convert.ToDecimal(outg.ToRange.Value);
                            }
                            else
                            {
                                calAmount += TempPremiumAmt * decimal.Parse(outg.Rate.ToString()) / 100;
                                TempPremiumAmt = 0;
                                break;

                            }
                        }
                    }

                    // if (calAmount == Convert.ToDecimal(_PolicyPaymentEntriesPost.TotalPayment))
                    double finalres = Convert.ToDouble((calAmount - Convert.ToDecimal(_PolicyPaymentEntriesPost.TotalPayment)));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PercentageofPremium_target)
                {
                    calAmount = 0;
                    //Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.TotalPayment.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.TotalPayment.ToString())).ToList().FirstOrDefault().Rate == _PolicyPaymentEntriesPost.PercentageOfPremium ? true : false;
                    List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                        .Where(p => p.EffectiveFromDate <= InvoiceDate)
                        .Where(p => p.EffectiveToDate >= InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString()))
                        .Where(p => p.ToRange >= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString())).ToList();
                    foreach (OutgoingScheduleEntry outg in _OutgoingShedule)
                    {
                        calAmount += Premium * decimal.Parse(outg.Rate.ToString()) / 100;
                    }

                    //if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble(calAmount - _PolicyPaymentEntriesPost.TotalPayment);
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PerHeadFee_scale)
                {

                    // List<OutgoingShedule> _OutgoingShedule = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();
                    List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                        .Where(p => p.EffectiveFromDate <= InvoiceDate)
                        .Where(p => p.EffectiveToDate >= InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();
                    int NumberOfHead = _PolicyPaymentEntriesPost.NumberOfUnits;
                    List<Guid> PayeeList = _OutgoingShedule.Select(p => p.PayeeUserCredentialId).Distinct().ToList();
                    calAmount = 0;
                    foreach (Guid payee in PayeeList)
                    {

                        int temoNUmberOfHead = NumberOfHead;
                        List<OutgoingScheduleEntry> _tempOutg = _OutgoingShedule.Where(p => p.PayeeUserCredentialId == payee).OrderBy(p => p.FromRange).ToList();
                        foreach (OutgoingScheduleEntry outg in _tempOutg)
                        {

                            // if (outg.ToRange <= Convert.ToDouble(temoNUmberOfHead))
                            if (temoNUmberOfHead > (outg.ToRange - (outg.FromRange + GetValidFromForcalCulation(outg.FromRange))))
                            {
                                calAmount += Convert.ToDecimal(outg.ToRange - (outg.FromRange + GetValidFromForcalCulation(outg.FromRange))) * Convert.ToDecimal(outg.Rate);
                                temoNUmberOfHead = temoNUmberOfHead - Convert.ToInt32(outg.ToRange);
                            }
                            else
                            {
                                calAmount += temoNUmberOfHead * Convert.ToDecimal(outg.Rate);
                                temoNUmberOfHead = 0;
                                break;
                            }

                        }
                    }

                    //if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PerHeadFee_target)
                {
                    calAmount = 0;
                    // Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString())).ToList().FirstOrDefault().Rate == double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString()) ? true : false;
                    List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                        .Where(p => p.EffectiveFromDate <= InvoiceDate)
                        .Where(p => p.EffectiveToDate >= InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString()))
                        .Where(p => p.ToRange >= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();
                    foreach (OutgoingScheduleEntry outg in _OutgoingShedule)
                    {
                        calAmount += Convert.ToDecimal(outg.Rate) * _PolicyPaymentEntriesPost.NumberOfUnits;
                    }
                    //if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.FlatDollar_modal)
                {
                    calAmount = 0;
                    //Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.Rate == double.Parse(_PolicyPaymentEntriesPost.Fee.ToString())).ToList().FirstOrDefault().Rate == double.Parse(_PolicyPaymentEntriesPost.Fee.ToString()) ? true : false;
                    List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                        .Where(p => p.EffectiveFromDate <= InvoiceDate)
                        .Where(p => p.EffectiveToDate >= InvoiceDate).ToList();
                    foreach (OutgoingScheduleEntry outg in _OutgoingShedule)
                    {
                        calAmount += decimal.Parse(outg.Rate.ToString());
                    }
                    // if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble(calAmount - _PolicyPaymentEntriesPost.TotalPayment);
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
            }
            return Flag;
        }
        //-------------
        public static bool CheckForOutgoingScheduleVariance(Guid PaymentEntryId, Guid ActivePolicyId)
        // public static bool CheckForOutgoingScheduleVariance(Guid PolicyId, DateTime InvoiceDate, Guid PaymentEntryId)
        {

            double TruncationError = Convert.ToDouble(Masters.SystemConstant.GetKeyValue("TruncationError"));

            bool Flag = false;
            PolicyPaymentEntriesPost _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntry(PaymentEntryId);
            decimal Premium = _PolicyPaymentEntriesPost.PaymentRecived;
            decimal DollerPerUnits = _PolicyPaymentEntriesPost.DollerPerUnit;
            decimal TotalPayment = _PolicyPaymentEntriesPost.TotalPayment;
            DateTime InvoiceDate = _PolicyPaymentEntriesPost.InvoiceDate.Value;

            // Guid PolicyId = _PolicyPaymentEntriesPost.PolicyID;
            PolicySchedule _PolicySchedule = CheckForOutgoingTypeOfSchedule(ActivePolicyId);
            PolicyDetailsData _Policy = GetPolicy(ActivePolicyId);

            FirstYrRenewalYr IsFirstYr = FirstYrRenewalYr.None;


            if (_PolicySchedule == PolicySchedule.None)
            {
                Flag = false;
            }
            else if (_PolicySchedule == PolicySchedule.Basic)
            {
                IsFirstYr = PostUtill.IsUseFirstYear(InvoiceDate, ActivePolicyId);
                if (IsFirstYr == FirstYrRenewalYr.None)
                {
                    Flag = false;
                    return Flag;
                }
                List<OutGoingPayment> _Schedule;
                // decimal calamount = 0;
                _Schedule = PostUtill.GetBasicOutGoingScheduleOfPolicy(ActivePolicyId);
                if (_Schedule == null || _Schedule.Count == 0)
                {
                    return false;
                }
                List<OutGoingPayment> _outgoingSchedule = _Schedule;
                if (_outgoingSchedule.FirstOrDefault().ScheduleTypeId == (int)MasterBasicOutgoinSchedule.PercentageOfCommission)
                {
                    Flag = true;//After dicuss
                    //decimal calAmount = 0;
                    //foreach (OutGoingPayment _OutGoingPayment in _outgoingSchedule)
                    //{
                    //    if (IsFirstYr == FirstYrRenewalYr.FirstYear)
                    //    {
                    //        calAmount += TotalPayment * decimal.Parse(_OutGoingPayment.FirstYearPercentage.Value.ToString()) / 100;
                    //    }
                    //    else if (IsFirstYr == FirstYrRenewalYr.Renewal)
                    //    {
                    //        calAmount += TotalPayment * decimal.Parse(_OutGoingPayment.RenewalPercentage.Value.ToString()) / 100;

                    //    }

                    //}
                    //double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    //if (Math.Abs(finalres) <= TruncationError)
                    //{
                    //    Flag = true;
                    //}

                }
                else if (_outgoingSchedule.FirstOrDefault().ScheduleTypeId == (int)MasterBasicOutgoinSchedule.PercentageOfPremium)
                {
                    decimal calAmount = 0;
                    foreach (OutGoingPayment _OutGoingPayment in _outgoingSchedule)
                    {
                        if (IsFirstYr == FirstYrRenewalYr.FirstYear)
                        {
                            calAmount += Premium * decimal.Parse(_OutGoingPayment.FirstYearPercentage.Value.ToString()) / 100;
                        }
                        else if (IsFirstYr == FirstYrRenewalYr.Renewal)
                        {
                            calAmount += Premium * decimal.Parse(_OutGoingPayment.RenewalPercentage.Value.ToString()) / 100;

                        }
                    }

                    // if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                //if (GetBasicSchedulePercentageForCalculation(PolicyId, InvoiceDate) != (double)PolicyPaymentEntriesPost.GetAllPaymentEntriesOfRange(InvoiceDate, InvoiceDate, PolicyId).FirstOrDefault().CommissionPercentage)
                //{
                //    Flag = true;
                //}
                //Flag = false;
            }
            else if (_PolicySchedule == PolicySchedule.Advance)
            {
                PolicyOutgoingSchedule _AdvanceSchedule = PostUtill.GetAdvanceOutgoingScheduleOfPolicy(ActivePolicyId);
                if (_AdvanceSchedule == null || _AdvanceSchedule.OutgoingScheduleList == null || _AdvanceSchedule.OutgoingScheduleList.Count == 0)
                {
                    return false;
                }
                int AdvanceOutgoingType = _AdvanceSchedule.ScheduleTypeId;
                decimal calAmount = 0;

                _AdvanceSchedule = PostUtill.FillNullDateWithMaxSystemDate(_AdvanceSchedule);
                ///////////////////
                if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PercentageofPremium_scale)
                {
                    List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList.Where(p => p.EffectiveFromDate <= InvoiceDate)
                        .Where(p => p.EffectiveToDate >= InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString())).ToList();

                    List<Guid> PayeeList = _OutgoingShedule.Select(p => p.PayeeUserCredentialId).Distinct().ToList();
                    calAmount = 0;
                    foreach (Guid payee in PayeeList)
                    {

                        decimal TempPremiumAmt = Premium;
                        List<OutgoingScheduleEntry> _tempOutg = _OutgoingShedule.Where(p => p.PayeeUserCredentialId == payee).OrderBy(p => p.FromRange).ToList();
                        foreach (OutgoingScheduleEntry outg in _tempOutg)
                        {
                            //if (Convert.ToDecimal(outg.ToRange.Value) <= TempPremiumAmt)
                            if (TempPremiumAmt >
                                Convert.ToDecimal(outg.ToRange
                                -
                                (outg.FromRange + PostUtill.GetValidFromForcalCulation(outg.FromRange))
                                ))
                            {
                                calAmount += Convert.ToDecimal(outg.ToRange - (outg.FromRange + GetValidFromForcalCulation(outg.FromRange))) * Convert.ToDecimal(outg.Rate) / 100;
                                TempPremiumAmt = TempPremiumAmt - Convert.ToDecimal(outg.ToRange.Value);
                            }
                            else
                            {
                                calAmount += TempPremiumAmt * decimal.Parse(outg.Rate.ToString()) / 100;
                                TempPremiumAmt = 0;
                                break;

                            }
                        }
                    }

                    // if (calAmount == Convert.ToDecimal(_PolicyPaymentEntriesPost.TotalPayment))
                    double finalres = Convert.ToDouble((calAmount - Convert.ToDecimal(_PolicyPaymentEntriesPost.TotalPayment)));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PercentageofPremium_target)
                {
                    calAmount = 0;
                    //Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.TotalPayment.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.TotalPayment.ToString())).ToList().FirstOrDefault().Rate == _PolicyPaymentEntriesPost.PercentageOfPremium ? true : false;
                    List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                        .Where(p => p.EffectiveFromDate <= InvoiceDate)
                        .Where(p => p.EffectiveToDate >= InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString()))
                        .Where(p => p.ToRange >= double.Parse(_PolicyPaymentEntriesPost.PaymentRecived.ToString())).ToList();
                    foreach (OutgoingScheduleEntry outg in _OutgoingShedule)
                    {
                        calAmount += Premium * decimal.Parse(outg.Rate.ToString()) / 100;
                    }

                    //if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble(calAmount - _PolicyPaymentEntriesPost.TotalPayment);
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PerHeadFee_scale)
                {

                    // List<OutgoingShedule> _OutgoingShedule = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();
                    List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                        .Where(p => p.EffectiveFromDate <= InvoiceDate)
                        .Where(p => p.EffectiveToDate >= InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();
                    int NumberOfHead = _PolicyPaymentEntriesPost.NumberOfUnits;
                    List<Guid> PayeeList = _OutgoingShedule.Select(p => p.PayeeUserCredentialId).Distinct().ToList();
                    calAmount = 0;
                    foreach (Guid payee in PayeeList)
                    {

                        int temoNUmberOfHead = NumberOfHead;
                        List<OutgoingScheduleEntry> _tempOutg = _OutgoingShedule.Where(p => p.PayeeUserCredentialId == payee).OrderBy(p => p.FromRange).ToList();
                        foreach (OutgoingScheduleEntry outg in _tempOutg)
                        {

                            // if (outg.ToRange <= Convert.ToDouble(temoNUmberOfHead))
                            if (temoNUmberOfHead > (outg.ToRange - (outg.FromRange + GetValidFromForcalCulation(outg.FromRange))))
                            {
                                calAmount += Convert.ToDecimal(outg.ToRange - (outg.FromRange + GetValidFromForcalCulation(outg.FromRange))) * Convert.ToDecimal(outg.Rate);
                                temoNUmberOfHead = temoNUmberOfHead - Convert.ToInt32(outg.ToRange);
                            }
                            else
                            {
                                calAmount += temoNUmberOfHead * Convert.ToDecimal(outg.Rate);
                                temoNUmberOfHead = 0;
                                break;
                            }

                        }
                    }

                    //if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.PerHeadFee_target)
                {
                    calAmount = 0;
                    // Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.FromRange > double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString())).Where(p => p.ToRange < double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString())).ToList().FirstOrDefault().Rate == double.Parse(_PolicyPaymentEntriesPost.DollerPerUnit.ToString()) ? true : false;
                    List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                        .Where(p => p.EffectiveFromDate <= InvoiceDate)
                        .Where(p => p.EffectiveToDate >= InvoiceDate)
                        .Where(p => p.FromRange <= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString()))
                        .Where(p => p.ToRange >= double.Parse(_PolicyPaymentEntriesPost.NumberOfUnits.ToString())).ToList();
                    foreach (OutgoingScheduleEntry outg in _OutgoingShedule)
                    {
                        calAmount += Convert.ToDecimal(outg.Rate) * _PolicyPaymentEntriesPost.NumberOfUnits;
                    }
                    //if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble((calAmount - _PolicyPaymentEntriesPost.TotalPayment));
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
                else if (AdvanceOutgoingType == (int)MasterAdvanceScheduleType.FlatDollar_modal)
                {
                    calAmount = 0;
                    //Flag = _AdvanceSchedule.Where(p => p.EffectiveFromDate < InvoiceDate).Where(p => p.EffectiveToDate > InvoiceDate).Where(p => p.Rate == double.Parse(_PolicyPaymentEntriesPost.Fee.ToString())).ToList().FirstOrDefault().Rate == double.Parse(_PolicyPaymentEntriesPost.Fee.ToString()) ? true : false;
                    List<OutgoingScheduleEntry> _OutgoingShedule = _AdvanceSchedule.OutgoingScheduleList
                        .Where(p => p.EffectiveFromDate <= InvoiceDate)
                        .Where(p => p.EffectiveToDate >= InvoiceDate).ToList();
                    foreach (OutgoingScheduleEntry outg in _OutgoingShedule)
                    {
                        calAmount += decimal.Parse(outg.Rate.ToString());
                    }
                    // if (calAmount == _PolicyPaymentEntriesPost.TotalPayment)
                    double finalres = Convert.ToDouble(calAmount - _PolicyPaymentEntriesPost.TotalPayment);
                    if (Math.Abs(finalres) <= TruncationError)
                    {
                        Flag = true;
                    }
                }
            }
            return Flag;
        }
        //-----------------

        private static PolicyOutgoingSchedule FillNullDateWithMaxSystemDate(PolicyOutgoingSchedule _AdvanceSchedule)
        {
            foreach (OutgoingScheduleEntry ise in _AdvanceSchedule.OutgoingScheduleList)
            {
                if (ise.EffectiveToDate.HasValue == false || ise.EffectiveToDate == null)
                {
                    ise.EffectiveToDate = DateTime.MaxValue;
                }
            }
            return _AdvanceSchedule;
        }


        private static PolicySchedule CheckForIncomingTypeOfSchedule(Guid policyid)
        {
            bool? flag = GetPolicy(policyid).IsIncomingBasicSchedule;
            if (flag == null)
                return PolicySchedule.None;
            return flag.Value ? PolicySchedule.Basic : PolicySchedule.Advance;
        }
        public static PolicySchedule CheckForOutgoingTypeOfSchedule(Guid PolicyId)
        {

            return PolicySchedule.Basic;

            //bool? flag = GetPolicy(PolicyId).IsOutGoingBasicSchedule;
            //if (flag == null)
            //    return PolicySchedule.None;
            //return flag.Value ? PolicySchedule.Basic : PolicySchedule.Advance;

        }
        private static PolicyToolIncommingShedule GetBasicIncomingScheduleOfPolicy(Guid PolicyID)
        {
            PolicyToolIncommingShedule PolicyIncomSche = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PolicyIncomingSchedule _policyincomingschedule = (from f in DataModel.PolicyIncomingSchedules where f.PolicyId.Value == PolicyID select f).FirstOrDefault();

                if (_policyincomingschedule != null)
                {
                    PolicyIncomSche = new PolicyToolIncommingShedule();
                    PolicyIncomSche.FirstYearPercentage = _policyincomingschedule.FirstYearPercentage;
                    PolicyIncomSche.IncomingScheduleId = _policyincomingschedule.IncomingScheduleId;
                    PolicyIncomSche.PolicyId = _policyincomingschedule.PolicyId.Value;
                    PolicyIncomSche.RenewalPercentage = _policyincomingschedule.RenewalPercentage;
                    // PolicyIncomSche.SplitPercentage = _policyincomingschedule.SplitPercentage;
                    PolicyIncomSche.ScheduleTypeId = _policyincomingschedule.ScheduleTypeId.Value;
                }
                return PolicyIncomSche;
            }
        }
        public static List<OutGoingPayment> GetBasicOutGoingScheduleOfPolicy(Guid PolicyId)
        {
            return OutGoingPayment.GetOutgoingSheduleForPolicy(PolicyId);
        }
        private static PolicyIncomingSchedule GetAdvanceIncomingScheduleOfPolicy(Guid PolicyId)
        {
            PolicyIncomingSchedule _IncomingSchedule = IncomingSchedule.GetPolicyIncomingSchedule(PolicyId);
            return _IncomingSchedule;

        }
        public static PolicyOutgoingSchedule GetAdvanceOutgoingScheduleOfPolicy(Guid PolicyId)
        {
            PolicyOutgoingSchedule _OutGoingSchedule = OutgoingSchedule.GetPolicyOutgoingSchedule(PolicyId);
            return _OutGoingSchedule;

        }
        public static List<FollowUpDate> CalculateFollowUpDateRange(PaymentMode _PaymentType, Guid PolicyId)
        {
            DateTime? FromDate = null;
            DateTime? ToDate = null;

            switch (_PaymentType.ToString())
            {
                case "Monthly":
                    FromDate = FromMonthlyDate(PolicyId);
                    ToDate = ToMOnthlyDate(PolicyId);
                    break;
                case "Quarterly":
                    FromDate = FromQuaterlyDate(PolicyId);
                    ToDate = ToQuaterlyDate(PolicyId);
                    break;
                case "HalfYearly":
                    FromDate = FromHalfYearlyDate(PolicyId);
                    ToDate = ToHalfYearlyDate(PolicyId);
                    break;
                case "OneTime":
                    FromDate = null;
                    ToDate = null;
                    break;
                case "Random":
                    FromDate = null;
                    ToDate = null;
                    break;

            }
            List<FollowUpDate> followupdate = new List<FollowUpDate>();
            followupdate.Add(new FollowUpDate(FromDate, ToDate, _PaymentType));

            return followupdate;
        }


        #region MonthlyMode
        private static DateTime FromMonthlyDate(Guid PolicyId)
        {
            PolicyLearnedFieldData _po = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId); //PolicyToLearnPost.GetPolicyLearnedFieldsPolicyWise1(PolicyId);
            DateTime _dtTFD = _po.TrackFrom.Value;
            //int month = _dtTFD.Month;
            //int year = _dtTFD.Year;
            //_dtTFD = new DateTime(year, month, 1);
            return FirstDate(_dtTFD);
            //return _dtTFD;
        }
        private static DateTime ToMOnthlyDate(Guid PolicyId)
        {
            PolicyLearnedFieldData _po = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            DateTime ToDay = DateTime.Today;
            //DateTime TempDate = ToDay.AddDays(-93);
            DateTime TempDate = ToDay.AddDays(-63);
            DateTime? AdjustTD = null;
            if (_po.AutoTerminationDate != null)
            {
                DateTime xdate = _po.AutoTerminationDate.Value.AddDays(-1);
                xdate = LastDate(xdate);//adjusted TD.
                AdjustTD = xdate;
                //return xdate;
            }
            if (AdjustTD == null)
            {
                ToDay = TempDate;
            }
            else
            {
                ToDay = TempDate < AdjustTD.Value ? TempDate : AdjustTD.Value;

            }
            ToDay = LastDate(ToDay.AddMonths(-1));
            return ToDay;
        }
        #endregion

        #region QuaterlyMode
        private static DateTime FromQuaterlyDate(Guid PolicyId)
        {
            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            DateTime _EffDt = _pld.Effective.Value;
            DateTime _TFDt = _pld.TrackFrom.Value;
            DateGroup DG;
            List<DateRange> _daterange;
            bool IsPaymentRecived;
            IsPaymentRecived = DoPaymentRecived(PolicyId);
            if (_EffDt != null)
            {
                _EffDt = FirstDate(_EffDt);


                _TFDt = FirstDate(_EffDt);
                DG = new DateGroup(_EffDt, PaymentMode.Quarterly);
                _daterange = DG.GetAllDateRange();

                int rngnum = GetRangeOFDate(PaymentMode.Quarterly, _TFDt.Month, _daterange);
                _TFDt = FirstDateOfRange(rngnum, _daterange);
                return _TFDt;
            }

            else if (_EffDt == null && IsPaymentRecived)
            {
                DateTime? OID = GetOldestInvoiceDate(PolicyId);
                OID = FirstDate((DateTime)OID);
                DG = new DateGroup((DateTime)OID, PaymentMode.Quarterly);
                _daterange = DG.GetAllDateRange();
                _TFDt = FirstDate(_TFDt);
                int rngnum = GetRangeOFDate(PaymentMode.Quarterly, _TFDt.Month, _daterange);
                DateTime _rngFDt = FirstDateOfRange(rngnum, _daterange);
                if (_TFDt < _rngFDt)
                {
                    return _rngFDt;
                }
                else
                {
                    return _TFDt;
                }
            }
            else if (_EffDt == null && !IsPaymentRecived)
            {

                return FirstDate(_TFDt);
            }
            return FirstDate(_TFDt);//Check for this for default
        }
        private static DateTime ToQuaterlyDate(Guid PolicyId)//Confusion---Need To Disscuss at 1-2 -2011 for virtualtable creation
        {
            // DateTime FromDate = FromQuaterlyDate(policy); 

            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            DateTime _EffDt = _pld.Effective.Value;
            DateTime _TFDt = _pld.TrackFrom.Value;
            DateGroup DG;
            bool IsPaymentRecived;
            IsPaymentRecived = DoPaymentRecived(PolicyId);
            List<DateRange> _daterange = null;
            if (_EffDt != null)
            {
                _EffDt = FirstDate(_EffDt);
                _TFDt = FirstDate(_EffDt);
                DG = new DateGroup(_EffDt, PaymentMode.Quarterly);
                _daterange = DG.GetAllDateRange();
            }
            else if (_EffDt == null && IsPaymentRecived)
            {
                DateTime? OID = GetOldestInvoiceDate(PolicyId);
                OID = FirstDate((DateTime)OID);
                DG = new DateGroup((DateTime)OID, PaymentMode.Quarterly);
                _daterange = DG.GetAllDateRange();
            }


            DateTime _ToDt = ToMOnthlyDate(PolicyId);
            int rngnum = GetRangeOFDate(PaymentMode.Quarterly, _ToDt.Month, _daterange);
            if (LastDateOfRange(rngnum, _daterange) == _ToDt)
            {

                return _ToDt;
            }
            else
            {
                rngnum = rngnum - 1;
                return LastDateOfRange(rngnum, _daterange);
            }
        }



        #endregion

        #region HalfYearlyMode
        private static DateTime FromHalfYearlyDate(Guid PolicyId)
        {
            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            DateTime _EffDt = _pld.Effective.Value;
            DateTime _TFDt = _pld.TrackFrom.Value;
            DateGroup DG;
            List<DateRange> _daterange;
            bool IsPaymentRecived;
            IsPaymentRecived = DoPaymentRecived(PolicyId);
            if (_EffDt != null)
            {
                _EffDt = FirstDate(_EffDt);


                _TFDt = FirstDate(_EffDt);
                DG = new DateGroup(_EffDt, PaymentMode.HalfYearly);
                _daterange = DG.GetAllDateRange();

                int rngnum = GetRangeOFDate(PaymentMode.HalfYearly, _TFDt.Month, _daterange);
                _TFDt = FirstDateOfRange(rngnum, _daterange);
                return _TFDt;
            }

            else if (_EffDt == null && IsPaymentRecived)
            {
                DateTime? OID = GetOldestInvoiceDate(PolicyId);
                OID = FirstDate((DateTime)OID);
                DG = new DateGroup((DateTime)OID, PaymentMode.HalfYearly);
                _daterange = DG.GetAllDateRange();
                _TFDt = FirstDate(_TFDt);
                int rngnum = GetRangeOFDate(PaymentMode.HalfYearly, _TFDt.Month, _daterange);
                DateTime _rngFDt = FirstDateOfRange(rngnum, _daterange);
                if (_TFDt < _rngFDt)
                {
                    return _rngFDt;
                }
                else
                {
                    return _TFDt;
                }
            }
            else if (_EffDt == null && !IsPaymentRecived)
            {

                return FirstDate(_TFDt);
            }
            return FirstDate(_TFDt);//Check for this for default
        }
        private static DateTime ToHalfYearlyDate(Guid PolicyId)
        {
            PolicyLearnedFieldData _pld = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            DateTime _EffDt = _pld.Effective.Value;
            DateTime _TFDt = _pld.TrackFrom.Value;
            DateGroup DG;
            bool IsPaymentRecived;
            IsPaymentRecived = DoPaymentRecived(PolicyId);
            List<DateRange> _daterange = null;
            if (_EffDt != null)
            {
                _EffDt = FirstDate(_EffDt);
                _TFDt = FirstDate(_EffDt);
                DG = new DateGroup(_EffDt, PaymentMode.HalfYearly);
                _daterange = DG.GetAllDateRange();
            }
            else if (_EffDt == null && IsPaymentRecived)
            {
                DateTime? OID = GetOldestInvoiceDate(PolicyId);
                OID = FirstDate((DateTime)OID);
                DG = new DateGroup((DateTime)OID, PaymentMode.HalfYearly);
                _daterange = DG.GetAllDateRange();
            }


            DateTime _ToDt = ToMOnthlyDate(PolicyId);
            int rngnum = GetRangeOFDate(PaymentMode.HalfYearly, _ToDt.Month, _daterange);
            if (LastDateOfRange(rngnum, _daterange) == _ToDt)
            {

                return _ToDt;
            }
            else
            {
                rngnum = rngnum - 1;
                return LastDateOfRange(rngnum, _daterange);
            }
        }
        #endregion

        public static DateTime? GetOldestInvoiceDate(Guid PolicyId)
        {
            List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);
            if (_PolicyPaymentEntriesPost == null || _PolicyPaymentEntriesPost.Count == 0)
                return null;

            return _PolicyPaymentEntriesPost.Min(p => p.InvoiceDate);
        }
        //public static DateTime? GetLatestInvoiceDate(Guid PolicyId)
        //{
        //    List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);
        //    if (_PolicyPaymentEntriesPost == null || _PolicyPaymentEntriesPost.Count == 0)
        //        return null;

        //    return _PolicyPaymentEntriesPost.Min(p => p.InvoiceDate);
        //}
        /// <summary>
        /// It Taste IsPayment is recived for a ParticularPolicy
        /// </summary>
        /// <param name="PolicyId">Paas the Poilcy ID</param>
        /// <returns>true if policy get otherwise false</returns>
        private static bool DoPaymentRecived(Guid PolicyId)
        {
            return PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId).Count > 0 ? true : false;

        }


        private static int GetRangeOFDate(PaymentMode paymentMode, int month, List<DateRange> _daterange)
        {
            return _daterange.Where(p => p.STARTDATE.Month == month).FirstOrDefault().RANGE;

        }
        public static DateTime LastDate(DateTime dt)
        {
            return dt.AddMonths(1).AddDays(-dt.Day);
        }
        private DateTime LastDate(int month, int year)
        {
            DateTime dt = new DateTime(year, month, 1);
            return dt.AddMonths(1).AddDays(-dt.Day);
        }
        public static DateTime FirstDate(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
        private static DateTime FirstDateOfRange(int range, List<DateRange> _daterange)
        {
            List<DateRange> _finalrng = _daterange.Where(p => p.RANGE == range).ToList<DateRange>();
            return _finalrng[0].STARTDATE;

        }
        private static DateTime LastDateOfRange(int range, List<DateRange> _daterange)
        {
            List<DateRange> _finalrng = _daterange.Where(p => p.RANGE == range).ToList<DateRange>();
            return _finalrng[0].STARTDATE;
        }

        #endregion

        private static void AutoPolicyTerminateProcess(PaymentMode _paymentmode, List<FollowUpDate> _followupdate, Guid PolicyId)
        {
            DateGroup DG;
            List<DateRange> Drng;
            int missPayemt = 0;
            DateTime? CalTermDate = null;
            switch (_paymentmode)
            {
                case PaymentMode.HalfYearly:
                    DG = new DateGroup(_followupdate.FirstOrDefault().FromDate.Value, _paymentmode);
                    Drng = DG.GetReverseDateRange();
                    missPayemt = 0;
                    foreach (DateRange dr in Drng)
                    {
                        List<PolicyPaymentEntriesPost> _PolicyPaymentEntries = PolicyPaymentEntriesPost.GetAllPaymentEntriesOfRange(dr.STARTDATE, dr.ENDDATE, PolicyId);
                        missPayemt += _PolicyPaymentEntries.Count != 0 ? 1 : 0;
                    }
                    if (missPayemt == 1)
                    {
                        CalTermDate = Drng.FirstOrDefault().STARTDATE;

                        //Terminate the policy--Done
                        UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);

                    }
                    break;
                case PaymentMode.Monthly:
                    DG = new DateGroup(_followupdate.FirstOrDefault().FromDate.Value, _paymentmode);
                    Drng = DG.GetReverseDateRange();
                    missPayemt = 0;
                    foreach (DateRange dr in Drng)
                    {
                        List<PolicyPaymentEntriesPost> _PolicyPaymentEntries = PolicyPaymentEntriesPost.GetAllPaymentEntriesOfRange(dr.STARTDATE, dr.ENDDATE, PolicyId);
                        missPayemt += _PolicyPaymentEntries.Count != 0 ? 1 : 0;
                    }
                    if (missPayemt == 3)
                    {
                        CalTermDate = Drng.FirstOrDefault().STARTDATE;

                        //Terminate the policy
                        UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);

                    }
                    break;
                case PaymentMode.Quarterly:
                    DG = new DateGroup(_followupdate.FirstOrDefault().FromDate.Value, _paymentmode);
                    Drng = DG.GetReverseDateRange();
                    missPayemt = 0;
                    foreach (DateRange dr in Drng)
                    {
                        List<PolicyPaymentEntriesPost> _PolicyPaymentEntries = PolicyPaymentEntriesPost.GetAllPaymentEntriesOfRange(dr.STARTDATE, dr.ENDDATE, PolicyId);
                        missPayemt += _PolicyPaymentEntries.Count != 0 ? 1 : 0;
                    }
                    if (missPayemt == 2)
                    {
                        CalTermDate = Drng.FirstOrDefault().STARTDATE;

                        //Terminate the policy
                        UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);

                    }
                    break;
                case PaymentMode.Yearly:
                    DG = new DateGroup(_followupdate.FirstOrDefault().FromDate.Value, _paymentmode);
                    Drng = DG.GetReverseDateRange();
                    missPayemt = 0;
                    foreach (DateRange dr in Drng)
                    {
                        List<PolicyPaymentEntriesPost> _PolicyPaymentEntries = PolicyPaymentEntriesPost.GetAllPaymentEntriesOfRange(dr.STARTDATE, dr.ENDDATE, PolicyId);
                        missPayemt += _PolicyPaymentEntries.Count != 0 ? 1 : 0;
                    }
                    if (missPayemt == 1)
                    {
                        CalTermDate = Drng.FirstOrDefault().STARTDATE;

                        //Terminate the policy
                        UpdateTheAutoTermDateOfLearned(PolicyId, CalTermDate);

                    }
                    break;
            }
            if (missPayemt != 0 && CalTermDate != null)
            {
                PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);

                _PolicyLearnedField.AutoTerminationDate = CalTermDate.Value;
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _policyLearned = (from p in DataModel.PolicyLearnedFields where (p.PolicyId == _PolicyLearnedField.PolicyId) select p).FirstOrDefault();
                    if (_policyLearned == null)
                    {
                        return;

                    }
                    else
                    {

                        _policyLearned.AutoTerminationDate = _PolicyLearnedField.AutoTerminationDate;
                    }
                    DataModel.SaveChanges();
                }
            }

        }

        private static void UpdateTheAutoTermDateOfLearned(Guid PolicyId, DateTime? CalTermDate)
        {
            PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            _PolicyLearnedField.AutoTerminationDate = CalTermDate;
            PolicyLearnedField.AddUpdateLearned(_PolicyLearnedField, _PolicyLearnedField.ProductType);
        }

        public static DateTime? DateComparer(DateTime? date1, DateTime? date2)
        {
            if (date1 != null && date2 != null)
            {
                return DateTime.Compare(date1.Value, date2.Value) <= 0 ? date1 : date2;
            }
            else if (date1 == null)
            {
                return date2;
            }
            else if (date2 == null)
            {
                return date1;
            }
            return null;
        }
        public static DateTime? CalculateTrackFromDate(Guid PolicyId)
        {
            DateTime? TFdate;
            DateTime? PolicyTrackFrmDate = PostUtill.GetPolicy(PolicyId).TrackFromDate;
            PolicyLearnedFieldData _PolicyLearnedField = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
            DateTime? LearnedFieldTrackFrmDate = _PolicyLearnedField == null ? null : _PolicyLearnedField.TrackFrom;
            DateTime? olderstInvoiceDate = GetOldestInvoiceDate(PolicyId);
            
            TFdate = DateComparer(DateComparer(PolicyTrackFrmDate, LearnedFieldTrackFrmDate), olderstInvoiceDate);

            return TFdate;

        }

        #region PMC
        /// <summary>
        /// Not in Use---Not completed Function
        /// </summary>
        /// <param name="PolicyId"></param>
        /// <param name="paymententry"></param>
        /// <param name="typeofschedule"></param>
        /// <returns></returns>
        public static double ProjectedMonthlyCompensation(Guid PolicyId, PolicyPaymentEntriesPost paymententry, TypeOFIncomingPolicySchedule typeofschedule)
        {


            PolicyDetailsData policy = GetPolicy(PolicyId);
            double PMC = 0;

            switch (typeofschedule)
            {
                case TypeOFIncomingPolicySchedule.Advance:
                    break;
                case TypeOFIncomingPolicySchedule.PercentageOfPremium:
                    double? calculatePer = 0;
                    double modepremium = Convert.ToDouble(policy.ModeAvgPremium);
                    FirstYrRenewalYr frstyrrenew = IsUseFirstYear(paymententry.InvoiceDate, PolicyId);//Check this function 11-4-2011
                    if (frstyrrenew == FirstYrRenewalYr.FirstYear || frstyrrenew == FirstYrRenewalYr.None)
                    {
                        calculatePer = GetBasicIncomingScheduleOfPolicy(PolicyId).FirstYearPercentage;
                    }
                    else
                    {
                        calculatePer = GetBasicIncomingScheduleOfPolicy(PolicyId).RenewalPercentage;

                    }
                    PMC = modepremium * calculatePer.Value / 100;
                    break;
                case TypeOFIncomingPolicySchedule.PerHead:
                    break;
            }
            return PMC;
        }
        #endregion

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
    /// <summary>
    /// 
    /// </summary>
    public class DateGroup
    {
        DateTime DATE;
        PaymentMode paymode;
        public DateGroup(DateTime dt, PaymentMode paymentmode)
        {
            DATE = dt;
            paymode = paymentmode;
        }
        List<DateRange> _DtRngs = new List<DateRange>();
        List<DateGroup> _DtGrp = new List<DateGroup>();
        public List<DateRange> GetReverseDateRange()
        {
            DateTime strdt;
            DateTime enddt;
            int rng = 0;
            if (paymode == PaymentMode.Quarterly)
            {
                rng++;
                strdt = PostUtill.FirstDate(DATE);
                enddt = PostUtill.LastDate(DATE).AddMonths(3);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));
                rng++;
                strdt = strdt.AddMonths(-3);
                enddt = enddt.AddMonths(-3);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));
            }
            else if (paymode == PaymentMode.Monthly)
            {
                rng++;
                strdt = PostUtill.FirstDate(DATE);
                enddt = PostUtill.LastDate(DATE);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                rng++;
                strdt = strdt.AddMonths(-1);
                enddt = enddt.AddMonths(-1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                rng++;
                strdt = strdt.AddMonths(-1);
                enddt = enddt.AddMonths(-1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));
            }
            else if (paymode == PaymentMode.HalfYearly)
            {
                rng++;
                strdt = PostUtill.FirstDate(DATE);
                enddt = PostUtill.LastDate(DATE).AddMonths(6);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));
            }

            else if (paymode == PaymentMode.Yearly)
            {
                rng++;
                strdt = PostUtill.FirstDate(DATE);
                enddt = PostUtill.LastDate(DATE).AddMonths(12);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));
            }
            _DtRngs.Reverse();
            return _DtRngs;
        }
        public List<DateRange> GetAllDateRange()
        {
            DateTime strdt;
            DateTime enddt;
            int rng = 0;
            if (paymode == PaymentMode.Quarterly)
            {
                rng++;
                strdt = PostUtill.FirstDate(DATE);
                enddt = PostUtill.LastDate(DATE);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                rng++;
                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));


                rng++;
                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));


                rng++;
                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

            }
            else if (paymode == PaymentMode.HalfYearly)
            {
                rng++;
                strdt = PostUtill.FirstDate(DATE);
                enddt = PostUtill.LastDate(DATE);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));


                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));


                rng++;
                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));


                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));

                strdt = strdt.AddMonths(1);
                enddt = enddt.AddMonths(1);
                _DtRngs.Add(new DateRange(strdt, enddt, rng));
            }
            return _DtRngs;
        }
    }
    public class DateRange
    {
        public DateTime STARTDATE;
        public DateTime ENDDATE;
        public int RANGE;
        public DateRange(DateTime sdate, DateTime edate, int range)
        {
            STARTDATE = sdate;
            ENDDATE = edate;
            RANGE = range;
        }
    }

}

///Rules For Search Policy

///Step 1-- Get the Collection of searched Policy in SearchedPolictLst by List<Policy> GetPoliciesFromUniqueIdentifier(DEUFields deuFields)
///Step 2-- Conunt the Number of policy int SearcherdPolicyCnt  =SearchedPolictLst .Count
///Step 3-- we have to done the payment entryies
///         there is some cases
///             case 1-- if(SearcherdPolicyCnt  ==0)
///                         Check for Agency Version
///                             if(Agency)
///                               Call the function for making New Panding Policy by CreateNewPolicy()
///                                 Do a entry for this in PolicyPamentEntries
///                                 Do a entry for this in DEU //Dont no how they are connected                             
///                                 Do the entry for this payment in Outgoing Payment with the entryID of PolicyPamentEntries that is curently added
///                                   Give the all payment to HO  
///                             if(Not Agency)
///                               Call the function for making New Panding Policy by CreateNewPolicy()
///                                 Do a entry for this in PolicyPamentEntries
///                                 Do a entry for this in DEU //Dont no how they are connected                             
///                                 Do the entry for this payment in Outgoing Payment with the entryID of PolicyPamentEntries that is curently added
///                                   Give the all payment to HO  
///              case 2-- if(SearchedPolicyCnt >1)                     
///                           Check for any Pending Policy
///                              (1)if(Pending Policy is 1)
///                            Check for Agency Version
///                                     if(Agency)
///                                           Do a entry for this in PolicyPamentEntries for pending policy
///                                           Do a entry for this in DEU //Dont no how they are connected                             
///                                           Do the entry for this payment in Outgoing Payment with the entryID of PolicyPamentEntries that is curently added
///                                              Give the all payment to HO  
///                                    if(Not Agency)
///                                          Do a entry for this in PolicyPamentEntries
///                                           Do a entry for this in DEU //Dont no how they are connected                             
///                                           Do the entry for this payment in Outgoing Payment with the entryID of PolicyPamentEntries that is curently added
///                                           Give the all payment to HO                                        
///                  
///                                       (2)if(no pending policy)    
///                                            if(Agency)
///                                          Call the function for making New Panding Policy by CreateNewPolicy()
///                                           Do a entry for this in PolicyPamentEntries
///                                           Do a entry for this in DEU //Dont no how they are connected                             
///                                           Do the entry for this payment in Outgoing Payment with the entryID of PolicyPamentEntries that is curently added
///                                              Give the all payment to HO  
///                                    if(Not Agency)
///                                           Call the function for making New Panding Policy by CreateNewPolicy()
///                                          Do a entry for this in PolicyPamentEntries
///                                           Do a entry for this in DEU //Dont no how they are connected                             
///                                           Do the entry for this payment in Outgoing Payment with the entryID of PolicyPamentEntries that is curently added
///                                           Give the all payment to HO  

///                                  (3)if(more than 1 pending policy)
///                                  ////////////////////////////////////////
///                  case 3--if( SearchedPolicyCnt ==1)
///                         Check for Pending
///                             (1) if (pending)
///                                     if(Agency)
///                                           Do a entry for this in PolicyPamentEntries for pending policy
///                                           Do a entry for this in DEU //Dont no how they are connected                             
///                                           Do the entry for this payment in Outgoing Payment with the entryID of PolicyPamentEntries that is curently added
///                                              Give the all payment to HO  
///                                    if(Not Agency)
///                                          Do a entry for this in PolicyPamentEntries
///                                           Do a entry for this in DEU //Dont no how they are connected                             
///                                           Do the entry for this payment in Outgoing Payment with the entryID of PolicyPamentEntries that is curently added
///                                           Give the all payment to HO 
///                               (2) if(not pending)
///                                         if(agency)
///                                          Do a entry for this in PolicyPamentEntries
///                                          Do a entry for this in DEU //Dont no how they are connected
///                                          Do the entry for this payment in Outgoing Payment with the entryID of PolicyPamentEntries that is curently added
///                                          Distribute the Payment to the listed person in the poicy
///                                         if(not agency)
///                                            Do a entry for this in PolicyPamentEntries
///                                           Do a entry for this in DEU //Dont no how they are connected                             
///                                           Do the entry for this payment in Outgoing Payment with the entryID of PolicyPamentEntries that is curently added
///                                           Give the all payment to HO 


///                                           Note--A rule runs before distribution
///                                                 and a status is saved for each entery in DEU n Payment Policy