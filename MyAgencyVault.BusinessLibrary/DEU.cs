using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Data;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public enum ExposedDeuField
    {
        [EnumMember]
        PolicyNumber,
        [EnumMember]
        Insured,
        [EnumMember]
        Carrier,
        [EnumMember]
        Product,
        [EnumMember]
        ModelAvgPremium,
        [EnumMember]
        PolicyMode,
        [EnumMember]
        Enrolled,
        [EnumMember]
        SplitPercentage,
        [EnumMember]
        Client,
        [EnumMember]
        CompType,
        [EnumMember]
        PayorSysId,
        [EnumMember]
        Renewal,
        [EnumMember]
        CompScheduleType,
        [EnumMember]
        InvoiceDate,
        [EnumMember]
        PaymentReceived,
        [EnumMember]
        CommissionPercentage,
        [EnumMember]
        NumberOfUnits,
        [EnumMember]
        DollerPerUnit,
        [EnumMember]
        Fee,
        [EnumMember]
        Bonus,
        [EnumMember]
        CommissionTotal,
        [EnumMember]
        OtherData,
        [EnumMember]
        CarrierName,
        [EnumMember]
        ProductName,
        [EnumMember]
        EntryDate,
    }

    [DataContract]
    public class ExposedDEU
    {
        [DataMember]
        public Guid DEUENtryID { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string Insured { get; set; }
        [DataMember]
        public string CarrierNickName { get; set; }
        [DataMember]
        public string CoverageNickName { get; set; }
        [DataMember]
        public decimal? PaymentRecived { get; set; }
        [DataMember]
        public int? Units { get; set; }
        [DataMember]
        public decimal? CommissionTotal { get; set; }
        [DataMember]
        public decimal? Fee { get; set; }
        [DataMember]
        public double? SplitPercentage { get; set; }
        [DataMember]
        public double? CommissionPercentage { get; set; }
        [DataMember]
        public DateTime? InvoiceDate { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public PostCompleteStatusEnum PostStatus { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public DateTime? EntryDate { get; set; }
        [DataMember]
        public string UnlinkClientName { get; set; }
        [DataMember]
        public Guid? GuidCarrierID { get; set; }
        [DataMember]
        public bool IsPaymentFailure { get; set; }

    }

    [DataContract]
    public class ModifiyableBatchStatementData
    {
        [DataMember]
        public ModifiableBatchData BatchData { get; set; }
        [DataMember]
        public ModifiableStatementData StatementData { get; set; }
        [DataMember]
        public ExposedDEU ExposedDeu { get; set; }
        [DataMember]
        public DeuSearchedPolicy SearchedPolicy { get; set; }
        [DataMember]
        public BasicInformationForProcess BasicInformationForProcessPolicy { get; set; }
    }

    [DataContract]
    public class DEU
    {
        #region DataMember

        [DataMember]
        public Guid DEUENtryID { get; set; }
        [DataMember]
        public DateTime? OriginalEffectiveDate { get; set; }
        [DataMember]
        public decimal? PaymentRecived { get; set; }//Premium
        [DataMember]
        public double? CommissionPercentage { get; set; }
        //[DataMember]
        //public decimal CommissionPaid { get; set; }
        [DataMember]
        public XmlFields XmlData { get; set; }
        [DataMember]
        public string Insured { get; set; }//Insured, GroupName
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public string Enrolled { get; set; }
        [DataMember]
        public string Eligible { get; set; }
        [DataMember]
        public string Link1 { get; set; }
        [DataMember]
        public double? SplitPer { get; set; }
        [DataMember]
        public int? PolicyMode { get; set; }
        [DataMember]
        public DateTime? TrackFromDate { get; set; }
        [DataMember]
        public string PMC { get; set; }
        [DataMember]
        public string PAC { get; set; }
        [DataMember]
        public string CompScheduleType { get; set; }
        [DataMember]
        public int? CompTypeID { get; set; }
        [DataMember]
        public Guid? ClientID { get; set; }
        [DataMember]
        public Guid? StmtID { get; set; }
        [DataMember]
        public int? PostStatusID { get; set; }
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public DateTime? InvoiceDate { get; set; }
        [DataMember]
        public Guid? PayorId { get; set; }
        [DataMember]
        public int? NoOfUnits { get; set; }
        [DataMember]
        public decimal? DollerPerUnit { get; set; }
        [DataMember]
        public decimal? Fee { get; set; }
        [DataMember]
        public decimal? Bonus { get; set; }
        [DataMember]
        public decimal? CommissionTotal { get; set; }
        [DataMember]
        public string PayorSysID { get; set; }
        [DataMember]
        public string Renewal { get; set; }
        [DataMember]
        public Guid? CarrierID { get; set; }
        [DataMember]
        public Guid? CoverageID { get; set; }
        [DataMember]
        public bool IsEntrybyCommissiondashBoard { get; set; }
        [DataMember]
        public string CarrierNickName { get; set; }
        [DataMember]
        public string CoverageNickName { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public Guid? CreatedBy { get; set; }
        [DataMember]
        public int? PostCompleteStatus { get; set; }
        [DataMember]
        public decimal? ModalAvgPremium { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public DateTime? EntryDate { get; set; }

        [DataMember]
        public Guid? TemplateID { get; set; }

        [DataMember]
        public string UnlinkClientName { get; set; }

        [DataMember]
        public Guid? GuidCarrierID { get; set; }
        //Load the Policy on which followup procedure is to be done
        Guid PolicyGuidID = new Guid("249AEBFF-32A4-4A10-8563-AB7678383C64");

        #endregion

        #region Methods
        //public static DEU GetLatestInvoiceDateRecord(Guid PolicyId)
        public DEU GetLatestInvoiceDateRecord(Guid PolicyId)
        {
            DEU _DEU = new DEU();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _DEUEntryies = (from f in DataModel.EntriesByDEUs
                                        where (f.PolicyID == PolicyId)
                                        select f).ToList<DLinq.EntriesByDEU>();
                    if (_DEUEntryies == null || _DEUEntryies.Count == 0) return null;
                    DateTime? maxDt = _DEUEntryies.Max(p => p.InvoiceDate);   // Change the Invoice Period To Invioce Date then uncommented

                    var _DEUEntry = _DEUEntryies.Where(p => p.InvoiceDate == maxDt).FirstOrDefault();

                    _DEU.DEUENtryID = _DEUEntry.DEUEntryID;
                    _DEU.OriginalEffectiveDate = _DEUEntry.OriginalEffectiveDate;
                    _DEU.PaymentRecived = _DEUEntry.PaymentReceived;
                    _DEU.CommissionPercentage = _DEUEntry.CommissionPercentage;
                    _DEU.Insured = _DEUEntry.Insured;
                    _DEU.PolicyNumber = _DEUEntry.PolicyNumber;
                    _DEU.Enrolled = _DEUEntry.Enrolled;
                    _DEU.Eligible = _DEUEntry.Eligible;
                    _DEU.Link1 = _DEUEntry.Link1;
                    _DEU.SplitPer = _DEUEntry.SplitPer;
                    _DEU.PolicyMode = _DEUEntry.PolicyModeID;
                    _DEU.TrackFromDate = _DEUEntry.TrackFromDate;
                    _DEU.CompScheduleType = _DEUEntry.CompScheduleType;
                    _DEU.CompTypeID = _DEUEntry.CompTypeID;
                    _DEU.ClientID = _DEUEntry.ClientID;
                    _DEU.StmtID = _DEUEntry.StatementID;
                    _DEU.PostStatusID = _DEUEntry.PostStatusID;
                    _DEU.PolicyId = _DEUEntry.PolicyID ?? Guid.Empty;
                    _DEU.InvoiceDate = _DEUEntry.InvoiceDate;
                    _DEU.PayorId = _DEUEntry.PayorId;
                    _DEU.NoOfUnits = _DEUEntry.NumberOfUnits;
                    _DEU.DollerPerUnit = _DEUEntry.DollerPerUnit;
                    _DEU.Fee = _DEUEntry.Fee;
                    _DEU.Bonus = _DEUEntry.Bonus;
                    _DEU.CommissionTotal = _DEUEntry.CommissionTotal;
                    _DEU.PayorSysID = _DEUEntry.PayorSysID;
                    _DEU.Renewal = _DEUEntry.Renewal;
                    _DEU.CarrierID = _DEUEntry.CarrierId;
                    _DEU.CoverageID = _DEUEntry.CoverageId;
                    _DEU.IsEntrybyCommissiondashBoard = _DEUEntry.IsEntrybyCommissiondashBoard ?? false;
                    _DEU.CreatedBy = _DEUEntry.CreatedBy;
                    _DEU.PostCompleteStatus = _DEUEntry.PostCompleteStatus;

                    _DEU.CarrierName = _DEUEntry.CarrierName;
                    _DEU.ProductName = _DEUEntry.ProductName;
                    _DEU.EntryDate = _DEUEntry.EntryDate;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetLatestInvoiceDateRecord :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("GetLatestInvoiceDateRecord :" + ex.InnerException.ToString(), true);
            }

            return _DEU;

        }


        public static DEU GetDEULatestInvoiceDateRecord(Guid DeuID)
        {
            DEU _DEU = new DEU();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _DEUEntryies = (from f in DataModel.EntriesByDEUs
                                        where (f.DEUEntryID == DeuID)
                                        select f).ToList<DLinq.EntriesByDEU>();
                    if (_DEUEntryies == null || _DEUEntryies.Count == 0) return null;

                    var _DEUEntry = _DEUEntryies.OrderBy(p => p.InvoiceDate).FirstOrDefault();

                    _DEU.DEUENtryID = _DEUEntry.DEUEntryID;
                    _DEU.OriginalEffectiveDate = _DEUEntry.OriginalEffectiveDate;
                    _DEU.PaymentRecived = _DEUEntry.PaymentReceived;
                    _DEU.CommissionPercentage = _DEUEntry.CommissionPercentage;
                    _DEU.Insured = _DEUEntry.Insured;
                    _DEU.PolicyNumber = _DEUEntry.PolicyNumber;
                    _DEU.Enrolled = _DEUEntry.Enrolled;
                    _DEU.Eligible = _DEUEntry.Eligible;
                    _DEU.Link1 = _DEUEntry.Link1;
                    _DEU.SplitPer = _DEUEntry.SplitPer;
                    _DEU.PolicyMode = _DEUEntry.PolicyModeID;
                    _DEU.TrackFromDate = _DEUEntry.TrackFromDate;
                    _DEU.CompScheduleType = _DEUEntry.CompScheduleType;
                    _DEU.CompTypeID = _DEUEntry.CompTypeID;
                    _DEU.ClientID = _DEUEntry.ClientID;
                    _DEU.StmtID = _DEUEntry.StatementID;
                    _DEU.PostStatusID = _DEUEntry.PostStatusID;
                    _DEU.PolicyId = _DEUEntry.PolicyID ?? Guid.Empty;
                    _DEU.InvoiceDate = _DEUEntry.InvoiceDate;
                    _DEU.PayorId = _DEUEntry.PayorId;
                    _DEU.NoOfUnits = _DEUEntry.NumberOfUnits;
                    _DEU.DollerPerUnit = _DEUEntry.DollerPerUnit;
                    _DEU.Fee = _DEUEntry.Fee;
                    _DEU.Bonus = _DEUEntry.Bonus;
                    _DEU.CommissionTotal = _DEUEntry.CommissionTotal;
                    _DEU.PayorSysID = _DEUEntry.PayorSysID;
                    _DEU.Renewal = _DEUEntry.Renewal;
                    _DEU.CarrierID = _DEUEntry.CarrierId;
                    _DEU.CoverageID = _DEUEntry.CoverageId;
                    _DEU.IsEntrybyCommissiondashBoard = _DEUEntry.IsEntrybyCommissiondashBoard ?? false;
                    _DEU.CreatedBy = _DEUEntry.CreatedBy;
                    _DEU.PostCompleteStatus = _DEUEntry.PostCompleteStatus;

                    _DEU.CarrierName = _DEUEntry.CarrierName;
                    _DEU.ProductName = _DEUEntry.ProductName;
                    _DEU.EntryDate = _DEUEntry.EntryDate;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetDEULatestInvoiceDateRecord :" + ex.StackTrace.ToStringDump(), true);
                if (ex.InnerException != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("GetDEULatestInvoiceDateRecord :" + ex.InnerException.Message, true);
                }
               // throw ex;
            }
            return _DEU;

        }

        //public static void AddupdateDeuEntry(DEU _DeuEntry)
        public void AddupdateDeuEntry(DEU _DeuEntry)
        {
            try
            {
                if (_DeuEntry == null)
                {
                    return;
                }
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _deu = (from p in DataModel.EntriesByDEUs where (p.DEUEntryID == _DeuEntry.DEUENtryID) select p).FirstOrDefault();
                    if (_deu == null)
                    {
                        _deu = new DLinq.EntriesByDEU();
                        _deu.DEUEntryID = _DeuEntry.DEUENtryID;
                        _deu.OriginalEffectiveDate = _DeuEntry.OriginalEffectiveDate;
                        _deu.PaymentReceived = _DeuEntry.PaymentRecived ?? 0;
                        _deu.CommissionPercentage = _DeuEntry.CommissionPercentage ?? 0;
                        _deu.Insured = _DeuEntry.Insured;
                        _deu.PolicyNumber = _DeuEntry.PolicyNumber;
                        _deu.Enrolled = _DeuEntry.Enrolled;
                        _deu.Eligible = _DeuEntry.Eligible;
                        _deu.Link1 = _DeuEntry.Link1;
                        _deu.SplitPer = _DeuEntry.SplitPer ?? 100;
                        _deu.PolicyModeID = _DeuEntry.PolicyMode;
                        _deu.TrackFromDate = _DeuEntry.TrackFromDate;
                        _deu.CompScheduleType = _DeuEntry.CompScheduleType;
                        _deu.CompTypeID = _DeuEntry.CompTypeID;
                        _deu.ClientValue = _DeuEntry.ClientName;
                        _deu.ClientID = _DeuEntry.ClientID;
                        _deu.StatementID = _DeuEntry.StmtID;
                        _deu.PostStatusID = _DeuEntry.PostStatusID;
                        _deu.PolicyID = _DeuEntry.PolicyId;
                        //_deu.PayorPolicyId= _DeuEntry.p
                        _deu.InvoiceDate = _DeuEntry.InvoiceDate;
                        _deu.PayorId = _DeuEntry.PayorId;
                        _deu.NumberOfUnits = _DeuEntry.NoOfUnits ?? 0;
                        _deu.DollerPerUnit = _DeuEntry.DollerPerUnit ?? 0;
                        _deu.Fee = _DeuEntry.Fee ?? 0;
                        _deu.Bonus = _DeuEntry.Bonus ?? 0;
                        _deu.CommissionTotal = _DeuEntry.CommissionTotal ?? 0;
                        _deu.PayorSysID = _DeuEntry.PayorSysID;
                        _deu.Renewal = _DeuEntry.Renewal;
                        _deu.CarrierId = _DeuEntry.CarrierID;
                        _deu.CoverageId = _DeuEntry.CoverageID;
                        _deu.IsEntrybyCommissiondashBoard = _DeuEntry.IsEntrybyCommissiondashBoard;

                        _deu.CreatedBy = _DeuEntry.CreatedBy;
                        _deu.PostCompleteStatus = _DeuEntry.PostCompleteStatus;

                        //Add  column
                        _deu.CoverageNickName = _DeuEntry.CoverageNickName;
                        _deu.CarrierName = _DeuEntry.CarrierName;
                        _deu.CarrierNickName = _DeuEntry.CarrierNickName;
                        _deu.ProductName = _DeuEntry.ProductName;
                        _deu.EntryDate = System.DateTime.Now;

                        DataModel.AddToEntriesByDEUs(_deu);
                    }
                    else
                    {
                        _deu.OriginalEffectiveDate = _DeuEntry.OriginalEffectiveDate;
                        _deu.PaymentReceived = _DeuEntry.PaymentRecived ?? 0;
                        _deu.CommissionPercentage = _DeuEntry.CommissionPercentage ?? 0;
                        _deu.Insured = _DeuEntry.Insured;
                        _deu.PolicyNumber = _DeuEntry.PolicyNumber;
                        _deu.Enrolled = _DeuEntry.Enrolled;
                        _deu.Link1 = _DeuEntry.Link1;
                        _deu.SplitPer = _DeuEntry.SplitPer ?? 100;
                        _deu.PolicyModeID = _DeuEntry.PolicyMode;
                        _deu.TrackFromDate = _DeuEntry.TrackFromDate;
                        _deu.CompScheduleType = _DeuEntry.CompScheduleType;
                        _deu.CompTypeID = _DeuEntry.CompTypeID;
                        _deu.ClientValue = _DeuEntry.ClientName;
                        _deu.ClientID = _DeuEntry.ClientID;
                        _deu.StatementID = _DeuEntry.StmtID;
                        _deu.PostStatusID = _DeuEntry.PostStatusID;
                        _deu.PolicyID = _DeuEntry.PolicyId;
                        //_deu.PayorPolicyId= _DeuEntry.p
                        _deu.NumberOfUnits = _DeuEntry.NoOfUnits ?? 0;
                        _deu.DollerPerUnit = _DeuEntry.DollerPerUnit ?? 0;
                        _deu.Fee = _DeuEntry.Fee ?? 0;
                        _deu.Bonus = _DeuEntry.Bonus ?? 0;
                        _deu.CommissionTotal = _DeuEntry.CommissionTotal ?? 0;
                        _deu.PayorSysID = _DeuEntry.PayorSysID;
                        _deu.Renewal = _DeuEntry.Renewal;
                        _deu.CarrierId = _DeuEntry.CarrierID;
                        _deu.CoverageId = _DeuEntry.CoverageID;
                        _deu.IsEntrybyCommissiondashBoard = _DeuEntry.IsEntrybyCommissiondashBoard;

                        _deu.CreatedBy = _DeuEntry.CreatedBy;

                        _deu.CoverageNickName = _DeuEntry.CoverageNickName;
                        _deu.ProductName = _DeuEntry.ProductName;
                        _deu.CarrierNickName = _DeuEntry.CarrierNickName;
                        _deu.CarrierName = _DeuEntry.CarrierName;

                        _deu.PostCompleteStatus = _DeuEntry.PostCompleteStatus;
                    }

                    DataModel.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("issue in AddupdateDeuEntry:" + ex.StackTrace.ToStringDump(), true);
                if (ex.InnerException != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("issue in AddupdateDeuEntry:" + ex.InnerException.ToStringDump(), true);
                }
               // throw ex;
            }
        }

        //public static void AddupdateUnlinkDeuEntry(DEU _DeuEntry, string strClientName)
        public void AddupdateUnlinkDeuEntry(DEU _DeuEntry, string strClientName)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _deu = (from p in DataModel.EntriesByDEUs where (p.DEUEntryID == _DeuEntry.DEUENtryID) select p).FirstOrDefault();
                    if (_deu == null)
                    {
                        _deu = new DLinq.EntriesByDEU();
                        _deu.DEUEntryID = _DeuEntry.DEUENtryID;
                        _deu.OriginalEffectiveDate = _DeuEntry.OriginalEffectiveDate;
                        _deu.PaymentReceived = _DeuEntry.PaymentRecived ?? 0;
                        _deu.CommissionPercentage = _DeuEntry.CommissionPercentage ?? 0;
                        _deu.Insured = _DeuEntry.Insured;
                        _deu.PolicyNumber = _DeuEntry.PolicyNumber;
                        _deu.Enrolled = _DeuEntry.Enrolled;
                        _deu.Eligible = _DeuEntry.Eligible;
                        _deu.Link1 = _DeuEntry.Link1;
                        _deu.SplitPer = _DeuEntry.SplitPer ?? 100;
                        _deu.PolicyModeID = _DeuEntry.PolicyMode;
                        _deu.TrackFromDate = _DeuEntry.TrackFromDate;
                        _deu.CompScheduleType = _DeuEntry.CompScheduleType;
                        _deu.CompTypeID = _DeuEntry.CompTypeID;
                        _deu.ClientValue = strClientName;
                        _deu.ClientID = _DeuEntry.ClientID;
                        _deu.StatementID = _DeuEntry.StmtID;
                        _deu.PostStatusID = _DeuEntry.PostStatusID;
                        _deu.PolicyID = _DeuEntry.PolicyId;
                        //_deu.PayorPolicyId= _DeuEntry.p
                        _deu.InvoiceDate = _DeuEntry.InvoiceDate;
                        _deu.PayorId = _DeuEntry.PayorId;
                        _deu.NumberOfUnits = _DeuEntry.NoOfUnits ?? 0;
                        _deu.DollerPerUnit = _DeuEntry.DollerPerUnit ?? 0;
                        _deu.Fee = _DeuEntry.Fee ?? 0;
                        _deu.Bonus = _DeuEntry.Bonus ?? 0;
                        _deu.CommissionTotal = _DeuEntry.CommissionTotal ?? 0;
                        _deu.PayorSysID = _DeuEntry.PayorSysID;
                        _deu.Renewal = _DeuEntry.Renewal;
                        _deu.CarrierId = _DeuEntry.CarrierID;
                        _deu.CoverageId = _DeuEntry.CoverageID;
                        _deu.IsEntrybyCommissiondashBoard = _DeuEntry.IsEntrybyCommissiondashBoard;

                        _deu.CreatedBy = _DeuEntry.CreatedBy;
                        _deu.PostCompleteStatus = _DeuEntry.PostCompleteStatus;

                        //Add  column
                        _deu.CoverageNickName = _DeuEntry.CoverageNickName;
                        _deu.ProductName = _DeuEntry.ProductName;
                        _deu.CarrierNickName = _DeuEntry.CarrierNickName;
                        _deu.CarrierName = _DeuEntry.CarrierName;
                        _deu.EntryDate = System.DateTime.Now;

                        //Oldest Client Name
                        _deu.UnlinkClientName = strClientName;

                        DataModel.AddToEntriesByDEUs(_deu);
                    }
                    else
                    {
                        _deu.OriginalEffectiveDate = _DeuEntry.OriginalEffectiveDate;
                        _deu.PaymentReceived = _DeuEntry.PaymentRecived ?? 0;
                        _deu.CommissionPercentage = _DeuEntry.CommissionPercentage ?? 0;
                        _deu.Insured = _DeuEntry.Insured;
                        _deu.PolicyNumber = _DeuEntry.PolicyNumber;
                        _deu.Enrolled = _DeuEntry.Enrolled;
                        _deu.Link1 = _DeuEntry.Link1;
                        _deu.SplitPer = _DeuEntry.SplitPer ?? 100;
                        _deu.PolicyModeID = _DeuEntry.PolicyMode;
                        _deu.TrackFromDate = _DeuEntry.TrackFromDate;
                        _deu.CompScheduleType = _DeuEntry.CompScheduleType;
                        _deu.CompTypeID = _DeuEntry.CompTypeID;
                        _deu.ClientValue = _DeuEntry.ClientName;
                        _deu.ClientID = _DeuEntry.ClientID;
                        _deu.StatementID = _DeuEntry.StmtID;
                        _deu.PostStatusID = _DeuEntry.PostStatusID;
                        _deu.PolicyID = _DeuEntry.PolicyId;
                        //_deu.PayorPolicyId= _DeuEntry.p
                        _deu.NumberOfUnits = _DeuEntry.NoOfUnits ?? 0;
                        _deu.DollerPerUnit = _DeuEntry.DollerPerUnit ?? 0;
                        _deu.Fee = _DeuEntry.Fee ?? 0;
                        _deu.Bonus = _DeuEntry.Bonus ?? 0;
                        _deu.CommissionTotal = _DeuEntry.CommissionTotal ?? 0;
                        _deu.PayorSysID = _DeuEntry.PayorSysID;
                        _deu.Renewal = _DeuEntry.Renewal;
                        _deu.CarrierId = _DeuEntry.CarrierID;
                        _deu.CoverageId = _DeuEntry.CoverageID;
                        _deu.IsEntrybyCommissiondashBoard = _DeuEntry.IsEntrybyCommissiondashBoard;

                        _deu.CreatedBy = _DeuEntry.CreatedBy;

                        _deu.CoverageNickName = _DeuEntry.CoverageNickName;
                        _deu.ProductName = _DeuEntry.ProductName;
                        _deu.CarrierNickName = _DeuEntry.CarrierNickName;
                        _deu.CarrierName = _DeuEntry.CarrierName;

                        _deu.PostCompleteStatus = _DeuEntry.PostCompleteStatus;
                    }

                    DataModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddupdateUnlinkDeuEntry:" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("AddupdateUnlinkDeuEntry:" + ex.InnerException.ToString(), true);
            }
        }
        /// <summary>
        /// This method is used to Add/Update the DEU data.
        /// </summary>
        /// <param name="deuFields"></param>
        /// <returns>True is data is added or false if updated</returns>
        //public static ModifiyableBatchStatementData AddUpdate(DEUFields deuFields, Guid oldDeuEntryID)
        public ModifiyableBatchStatementData AddUpdate(DEUFields deuFields, Guid oldDeuEntryID)
        {
            Guid Id = Guid.Empty;
            bool IsAddCase = false;
            ModifiyableBatchStatementData batchStatementData = new ModifiyableBatchStatementData();
            try
            {
                PolicyDetailsData TempPolicyDetailsData = PostUtill.GetPolicy(PolicyGuidID);
                if (TempPolicyDetailsData != null)
                {
                    DateTime? dtTempTime = TempPolicyDetailsData.TrackFromDate;
                    if (dtTempTime == null)
                    {
                        return batchStatementData;
                    }
                }

                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.CommandTimeout = 600000000;

                    DLinq.EntriesByDEU DeuEntry = null;
                    if (deuFields.DeuEntryId == Guid.Empty)
                    {
                        DeuEntry = new DLinq.EntriesByDEU();
                        DeuEntry.DEUEntryID = Guid.NewGuid();
                        DeuEntry.StatementID = deuFields.StatementId;
                        DeuEntry.Statement = DataModel.Statements.FirstOrDefault(s => s.StatementId == DeuEntry.StatementID);
                        DeuEntry.PayorId = deuFields.PayorId;
                        DeuEntry.Payor = DataModel.Payors.FirstOrDefault(s => s.PayorId == DeuEntry.PayorId);
                        DeuEntry.CreatedBy = deuFields.CurrentUser;

                        DEU DENew = GetDeuEntrytDateRecord(oldDeuEntryID);
                        if (DENew != null)
                        {
                            if (DENew.EntryDate == null)
                                DeuEntry.EntryDate = System.DateTime.Now;
                            else
                                DeuEntry.EntryDate = DENew.EntryDate;
                        }
                        else
                        {
                            DeuEntry.EntryDate = System.DateTime.Now;
                        }
                        //IsAddCase = true;

                    }
                    else
                    {
                        //Update DeuEntry case
                        DeuEntry = DataModel.EntriesByDEUs.FirstOrDefault(s => s.DEUEntryID == oldDeuEntryID);
                        IsAddCase = false;
                    }

                    Guid? carrierId = null;
                    Guid? coverageId = null;
                    XmlFields xmlFieldCollection = null;
                    DEU deuData = new DEU();
                    DeuEntry.PaymentReceived = 0;
                    DeuEntry.CommissionPercentage = 0;
                    DeuEntry.NumberOfUnits = 0;
                    DeuEntry.DollerPerUnit = 0;
                    DeuEntry.Fee = 0;
                    DeuEntry.Bonus = 0;
                    DeuEntry.CommissionTotal = 0;
                    DeuEntry.SplitPer = 100;

                    List<Carrier> AllCarriersInPayor = new List<Carrier>();
                    if (DeuEntry != null)
                    {
                        if (DeuEntry.PayorId != null)
                        {
                            AllCarriersInPayor = Carrier.GetPayorCarriers((Guid)DeuEntry.PayorId);
                        }

                        if (AllCarriersInPayor.Count == 1)
                        {
                            string strNickName = AllCarriersInPayor.FirstOrDefault().NickName;

                            if (deuFields.PayorId != null)
                            {
                                carrierId = BLHelper.GetCarrierId(strNickName, deuFields.PayorId.Value);
                                if (carrierId != null)
                                {
                                    DeuEntry.CarrierId = carrierId;
                                    deuData.CarrierID = DeuEntry.CarrierId;
                                }
                            }
                        }
                    }

                    foreach (DataEntryField field in deuFields.DeuFieldDataCollection)
                    {
                        switch (field.DeuFieldName)
                        {
                            case "PolicyNumber":

                                DeuEntry.PolicyNumber = BLHelper.CorrectPolicyNo(field.DeuFieldValue);
                                try
                                {
                                    string strValuePolicy = DeuEntry.PolicyNumber.Substring(0, 49);
                                    DeuEntry.PolicyNumber = strValuePolicy;
                                }
                                catch
                                {
                                }
                                deuData.PolicyNumber = DeuEntry.PolicyNumber;
                                if (string.IsNullOrEmpty(DeuEntry.ClientValue))
                                {
                                    DeuEntry.ClientValue = DeuEntry.PolicyNumber;
                                    deuData.ClientName = DeuEntry.PolicyNumber;

                                    DeuEntry.UnlinkClientName = DeuEntry.PolicyNumber;
                                    deuData.UnlinkClientName = DeuEntry.PolicyNumber;
                                }

                                break;

                            case "ModelAvgPremium":
                                if (!string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.ModalAvgPremium = field.DeuFieldValue;

                                decimal value = 0;
                                if (decimal.TryParse(DeuEntry.ModalAvgPremium, out value))
                                    deuData.ModalAvgPremium = value;

                                break;

                            case "Insured":
                                try
                                {
                                    if (AllCapitals(field.DeuFieldValue))
                                    {
                                        DeuEntry.Insured = FirstCharIsCapital(field.DeuFieldValue);
                                        string strInsuredValue = DeuEntry.Insured.Substring(0, 99);
                                        DeuEntry.Insured = strInsuredValue;
                                    }
                                    else
                                    {

                                        DeuEntry.Insured = field.DeuFieldValue;
                                        string strInsuredValue = DeuEntry.Insured.Substring(0, 99);
                                        DeuEntry.Insured = strInsuredValue;
                                    }

                                    deuData.Insured = DeuEntry.Insured;
                                }
                                catch
                                {
                                }
                                break;

                            case "OriginalEffectiveDate":
                                try
                                {
                                    DateTime date1 = DateTime.ParseExact(field.DeuFieldValue, "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
                                    DeuEntry.OriginalEffectiveDate = date1;
                                    deuData.OriginalEffectiveDate = DeuEntry.OriginalEffectiveDate;
                                }
                                catch
                                {
                                    try
                                    {
                                        deuData.OriginalEffectiveDate = Convert.ToDateTime(field.DeuFieldValue);
                                    }
                                    catch
                                    {
                                    }
                                }
                                break;

                            case "InvoiceDate":
                                try
                                {
                                    DateTime date2 = DateTime.ParseExact(field.DeuFieldValue, "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
                                    DeuEntry.InvoiceDate = date2;
                                    deuData.InvoiceDate = DeuEntry.InvoiceDate;
                                }
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        deuData.InvoiceDate = Convert.ToDateTime(field.DeuFieldValue);
                                    }
                                    catch
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog("Wrong InvoiceDate date format :" + ex.StackTrace.ToString(), true);
                                    }

                                }
                                break;


                            case "EffectiveDate":

                                try
                                {
                                    if (!string.IsNullOrEmpty(field.DeuFieldValue))
                                    {
                                        DateTime dateffective = DateTime.ParseExact(field.DeuFieldValue, "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
                                        DeuEntry.OriginalEffectiveDate = dateffective;
                                        deuData.OriginalEffectiveDate = DeuEntry.OriginalEffectiveDate;
                                    }
                                    else
                                    {
                                        DeuEntry.OriginalEffectiveDate = null;
                                        deuData.OriginalEffectiveDate = DeuEntry.OriginalEffectiveDate;
                                    }
                                }
                                catch
                                {
                                    try
                                    {
                                        deuData.OriginalEffectiveDate = Convert.ToDateTime(field.DeuFieldValue);
                                    }
                                    catch
                                    {
                                    }

                                }
                                break;

                            case "PaymentReceived":
                                if (string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.PaymentReceived = 0;
                                else
                                {
                                    string strValue = field.DeuFieldValue;
                                    strValue = strValue.Replace("(", "");
                                    strValue = strValue.Replace(")", "");
                                    //DeuEntry.PaymentReceived = decimal.Parse(field.DeuFieldValue);
                                    DeuEntry.PaymentReceived = decimal.Parse(strValue);
                                }
                                deuData.PaymentRecived = DeuEntry.PaymentReceived;
                                break;

                            case "CommissionPercentage":
                                if (string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.CommissionPercentage = 0;
                                else
                                    DeuEntry.CommissionPercentage = double.Parse(field.DeuFieldValue);
                                deuData.CommissionPercentage = DeuEntry.CommissionPercentage;
                                break;

                            case "Renewal":
                                DeuEntry.Renewal = field.DeuFieldValue;
                                deuData.Renewal = DeuEntry.Renewal;
                                break;

                            case "Enrolled":
                                DeuEntry.Enrolled = field.DeuFieldValue;
                                deuData.Enrolled = DeuEntry.Enrolled;
                                break;

                            case "Eligible":
                                DeuEntry.Eligible = field.DeuFieldValue;
                                deuData.Eligible = DeuEntry.Eligible;
                                break;

                            case "Link1":
                                DeuEntry.Link1 = field.DeuFieldValue;
                                deuData.Link1 = DeuEntry.Link1;
                                break;

                            case "SplitPercentage":
                                if (string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.SplitPer = 100;
                                else
                                    DeuEntry.SplitPer = double.Parse(field.DeuFieldValue);

                                deuData.SplitPer = DeuEntry.SplitPer;
                                break;

                            case "PolicyMode":
                                DeuEntry.PolicyModeID = BLHelper.GetPolicyMode(field.DeuFieldValue);
                                DeuEntry.PolicyModeValue = field.DeuFieldValue;
                                deuData.PolicyMode = DeuEntry.PolicyModeID;
                                break;

                            case "Carrier":
                                if (deuFields.PayorId != null)
                                {
                                    carrierId = BLHelper.GetCarrierId(field.DeuFieldValue, deuFields.PayorId.Value);

                                    if (carrierId != null)
                                    {
                                        DeuEntry.CarrierId = carrierId;
                                        DeuEntry.CarrierNickName = field.DeuFieldValue;
                                        deuData.CarrierID = DeuEntry.CarrierId;
                                    }
                                    //Carrier nick name (Entered by DEU)
                                    DeuEntry.CarrierName = field.DeuFieldValue;
                                }

                                break;

                            case "Product":

                                if (carrierId != null)

                                    coverageId = BLHelper.GetProductId(field.DeuFieldValue, deuFields.PayorId.Value, carrierId.Value);
                                if (coverageId != null)
                                {
                                    DeuEntry.CoverageId = coverageId;
                                    DeuEntry.CoverageNickName = field.DeuFieldValue;
                                    deuData.CoverageID = DeuEntry.CoverageId;
                                }
                                //Product or Coverage nick name (Entered by DEU)
                                DeuEntry.ProductName = field.DeuFieldValue;

                                break;

                            case "PayorSysId":
                                DeuEntry.PayorSysID = field.DeuFieldValue;
                                deuData.PayorSysID = DeuEntry.PayorSysID;
                                break;

                            case "CompScheduleType":
                                if (string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.CompScheduleType = null;
                                else
                                    DeuEntry.CompScheduleType = field.DeuFieldValue;
                                deuData.CompScheduleType = DeuEntry.CompScheduleType;

                                break;

                            case "CompType":
                                ActionLogger.Logger.WriteImportPolicyLog("CompType in deuaddupdate: " + field.DeuFieldValue, true);

                                if (string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.CompTypeID = null;
                                else
                                    //DeuEntry.CompTypeID = BLHelper.getCompTypeId(field.DeuFieldValue);
                                    DeuEntry.CompTypeID = BLHelper.getCompTypeIdByName(field.DeuFieldValue);
                                deuData.CompTypeID = DeuEntry.CompTypeID;
                                break;

                            case "Client":
                                bool isCapital = AllCapitals(field.DeuFieldValue);

                                DeuEntry.ClientID = BLHelper.GetClientId(field.DeuFieldValue, deuFields.LicenseeId.Value);

                                if (isCapital)
                                {
                                    //DeuEntry.ClientValue = FirstCharIsCapital(field.DeuFieldValue);
                                    //DeuEntry.UnlinkClientName = DeuEntry.ClientValue;
                                    //deuData.UnlinkClientName = DeuEntry.ClientValue;
                                    try
                                    {
                                        DeuEntry.ClientValue = FirstCharIsCapital(field.DeuFieldValue);
                                        string strValue = DeuEntry.ClientValue.Substring(0, 99);
                                        DeuEntry.ClientValue = strValue;
                                        DeuEntry.UnlinkClientName = strValue;
                                        deuData.UnlinkClientName = strValue;
                                    }
                                    catch
                                    {
                                    }
                                }
                                else
                                {
                                    //DeuEntry.ClientValue = field.DeuFieldValue;
                                    //DeuEntry.UnlinkClientName = field.DeuFieldValue;
                                    //deuData.UnlinkClientName = field.DeuFieldValue;

                                    try
                                    {
                                        DeuEntry.ClientValue = field.DeuFieldValue;
                                        string strValue = DeuEntry.ClientValue.Substring(0, 99);
                                        DeuEntry.ClientValue = strValue;
                                        DeuEntry.UnlinkClientName = strValue;
                                        deuData.UnlinkClientName = strValue;
                                    }
                                    catch
                                    {
                                    }
                                }
                                //Update insured with client name
                                if (string.IsNullOrEmpty(DeuEntry.Insured))
                                {
                                    DeuEntry.Insured = DeuEntry.ClientValue;
                                }



                                deuData.ClientID = DeuEntry.ClientID;

                                break;

                            case "NumberOfUnits":
                                if (string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.NumberOfUnits = 0;
                                else
                                {
                                    decimal dCNoOfUnit = Convert.ToDecimal(field.DeuFieldValue);
                                    DeuEntry.NumberOfUnits = Convert.ToInt32(dCNoOfUnit);

                                }
                                deuData.NoOfUnits = DeuEntry.NumberOfUnits;
                                break;

                            case "DollerPerUnit":
                                if (string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.DollerPerUnit = 0;
                                else
                                    DeuEntry.DollerPerUnit = decimal.Parse(field.DeuFieldValue);
                                deuData.DollerPerUnit = DeuEntry.DollerPerUnit;
                                break;

                            case "Fee":
                                if (string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.Fee = 0;
                                else
                                    DeuEntry.Fee = decimal.Parse(field.DeuFieldValue);
                                deuData.Fee = DeuEntry.Fee;
                                break;

                            case "Bonus":
                                if (string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.Bonus = 0;
                                else
                                    DeuEntry.Bonus = decimal.Parse(field.DeuFieldValue);
                                deuData.Bonus = DeuEntry.Bonus;
                                break;

                            case "CommissionTotal":
                                if (string.IsNullOrEmpty(field.DeuFieldValue))
                                    DeuEntry.CommissionTotal = 0;
                                else
                                    DeuEntry.CommissionTotal = decimal.Parse(field.DeuFieldValue);
                                deuData.CommissionTotal = DeuEntry.CommissionTotal;
                                break;

                            default:
                                if (xmlFieldCollection == null)
                                    xmlFieldCollection = new XmlFields();

                                DataEntryField xmlField = new DataEntryField();
                                xmlField.DeuFieldName = field.DeuFieldName;
                                xmlField.DeuFieldValue = field.DeuFieldValue;
                                xmlField.DeuFieldType = field.DeuFieldType;
                                xmlFieldCollection.Add(xmlField);
                                break;
                        }
                    }

                    deuFields.DeuData = deuData;

                    if (xmlFieldCollection != null)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(XmlFields));
                        StringWriter stringWriter = new StringWriter();
                        serializer.Serialize(stringWriter, xmlFieldCollection);
                        string xmlObject = stringWriter.ToString();
                        DeuEntry.OtherData = xmlObject;
                    }

                    if (IsAddCase)
                    {
                        DataModel.AddToEntriesByDEUs(DeuEntry);
                    }

                    Id = DeuEntry.DEUEntryID;

                    DataModel.SaveChanges();

                    //batchStatementData.BatchData = Batch.CreateModifiableBatchData(DeuEntry.Statement.Batch);
                    Batch objBatch = new Batch();
                    batchStatementData.BatchData = objBatch.CreateModifiableBatchData(DeuEntry.Statement.Batch);
                    batchStatementData.StatementData = Statement.CreateModifiableStatementData(DeuEntry.Statement);
                    batchStatementData.ExposedDeu = CreateExposedDEU(DeuEntry);
                }
            }

            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Issue in function AddUpdate StackTrace " + ex.StackTrace.ToStringDump(), true);
                ActionLogger.Logger.WriteImportPolicyLog("Issue in AddUpdate Message " + ex.Message.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("Issue in AddUpdate InnerException " + ex.InnerException.ToStringDump(), true);
            }
            return batchStatementData;
        }

        #region"Font case"

        public static bool AllCapitals(string inputString)
        {
            foreach (char c in inputString)
            {
                if (char.IsLower(c))
                    return false;
            }
            return true;

        }

        public static string FirstCharIsCapital(string stringToModify)
        {

            StringBuilder sb = new StringBuilder();
            try
            {
                if (!string.IsNullOrEmpty(stringToModify))
                {
                    string[] array = stringToModify.Split(' ');

                    for (int i = 0; i < array.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(array[i]))
                        {
                            string firstLetter = array[i].Substring(0, 1);
                            string secondPart = array[i].Substring(1);
                            sb.Append(firstLetter.ToUpper() + secondPart.ToLower() + " ");
                        }

                    }
                }

            }
            catch
            {
            }

            if (sb.Length > 0)
            {
                return sb.ToString().Remove(sb.Length - 1);
            }
            else
            {
                return sb.ToString();
            }

        }

        #endregion

        public static DEU GetDeuEntrytDateRecord(Guid deuStatementD)
        {
            DEU _DEU = new DEU();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _DEUEntryies = (from f in DataModel.EntriesByDEUs
                                        where (f.DEUEntryID == deuStatementD)
                                        select f).ToList<DLinq.EntriesByDEU>();
                    if (_DEUEntryies == null || _DEUEntryies.Count == 0) return null;


                    foreach (var item in _DEUEntryies)
                    {
                        _DEU.EntryDate = item.EntryDate;
                        break;
                    }


                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetDeuEntrytDateRecord " + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("GetDeuEntrytDateRecord " + ex.Message.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("GetDeuEntrytDateRecord " + ex.InnerException.ToString(), true);
            }

            return _DEU;

        }

        public static void UpdateTotalAmountAndEntry(Guid deuStatementD, Guid DeuEntryId)
        {
            decimal? totalEnteredAmount = 0;
            int intTotalEntry = 0;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.CommandTimeout = 600000000;

                    var _DEUEntryies = (from f in DataModel.PolicyPaymentEntries
                                        where (f.StatementId == deuStatementD)
                                        select f).ToList<DLinq.PolicyPaymentEntry>();


                    intTotalEntry = _DEUEntryies.Count;

                    if (intTotalEntry > 0)
                    {
                        foreach (var item in _DEUEntryies)
                        {
                            totalEnteredAmount = totalEnteredAmount + item.TotalPayment;
                        }

                        //Update total entry and entry amount
                        DLinq.EntriesByDEU DeuEntry = DataModel.EntriesByDEUs.FirstOrDefault(s => s.DEUEntryID == DeuEntryId);
                        if (DeuEntry != null)
                        {
                            DeuEntry.Statement.Entries = intTotalEntry;
                            DeuEntry.Statement.EnteredAmount = totalEnteredAmount;
                        }
                        DataModel.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("UpdateTotalAmountAndEntry ex.StackTrace :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("UpdateTotalAmountAndEntry ex.Message:" + ex.Message.ToString(), true);
                //throw ex;
            }

        }

        //public static ModifiyableBatchStatementData UpdateBatchStatementDataOnSuccessfullDeuPost(Guid DeuEntryId, Guid UserId)
        public ModifiyableBatchStatementData UpdateBatchStatementDataOnSuccessfullDeuPost(Guid DeuEntryId, Guid UserId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                ModifiyableBatchStatementData batchStatementData = new ModifiyableBatchStatementData();

                try
                {
                    DLinq.EntriesByDEU DeuEntry = DataModel.EntriesByDEUs.FirstOrDefault(s => s.DEUEntryID == DeuEntryId);
                    if (DeuEntry != null)
                    {
                        DeuEntry.Statement.Entries = DeuEntry.Statement.Entries + 1;

                        if (DeuEntry.Statement.Entries == 1)
                        {
                            DeuEntry.Statement.StatementStatusId = 1;
                            if (DeuEntry.Statement.Batch.Statements.Count == 1)
                            {
                                DeuEntry.Statement.Batch.EntryStatusId = (int)EntryStatus.InDataEntry;
                                DeuEntry.Statement.Batch.AssignedUserCredentialId = UserId;
                            }
                        }

                        DeuEntry.Statement.EnteredAmount += (DeuEntry.CommissionTotal ?? 0);
                        DataModel.SaveChanges();
                        //Update total entered amount and entry
                        UpdateTotalAmountAndEntry(DeuEntry.Statement.StatementId, DeuEntryId);
                        //batchStatementData.BatchData = Batch.CreateModifiableBatchData(DeuEntry.Statement.Batch);
                        Batch objBatch = new Batch();
                        batchStatementData.BatchData = objBatch.CreateModifiableBatchData(DeuEntry.Statement.Batch);
                        batchStatementData.StatementData = Statement.CreateModifiableStatementData(DeuEntry.Statement);
                        batchStatementData.ExposedDeu = CreateExposedDEU(DeuEntry);
                    }
                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("UpdateBatchStatementDataOnSuccessfullDeuPost :" + ex.StackTrace.ToString(), true);
                    if (ex.InnerException != null)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("UpdateBatchStatementDataOnSuccessfullDeuPost :" + ex.InnerException.ToString(), true);
                    }
                   // throw ex;
                }

                return batchStatementData;
            }
        }

        //public static void UpdateDeuEntryStatus(Guid entryId, bool IsCompleted)
        //{
        //    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //    {
        //        DLinq.EntriesByDEU deu = DataModel.EntriesByDEUs.FirstOrDefault(s => s.DEUEntryID == entryId);

        //        if (deu != null)
        //        {
        //            if (IsCompleted)
        //            {
        //                deu.PostCompleteStatus = 3;
        //            }
        //            else
        //            {
        //                DataModel.DeleteObject(deu);
        //            }
        //        }
        //        DataModel.SaveChanges();
        //    }
        //}

        public static DeuSearchedPolicy GetSearchedDeuPolicy(Guid PolicyId)
        {
            throw new NotImplementedException();
        }

        //public static ModifiyableBatchStatementData DeleteDeuEntry(Guid DeuEntryId)
        //{
        //    ModifiyableBatchStatementData batchStatementData = new ModifiyableBatchStatementData();
        //    ActionLogger.Logger.WriteImportPolicyLog("starting delete ModifiyableBatchStatementData DeleteDeuEntry ", true);
        //    try
        //    {
        //        using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //        {
        //            DLinq.EntriesByDEU DeuEntry = DataModel.EntriesByDEUs.FirstOrDefault(s => s.DEUEntryID == DeuEntryId);
        //            if (DeuEntry != null)
        //            {
        //                DeuEntry.Statement.Entries = DeuEntry.Statement.Entries - 1;

        //                if (DeuEntry.Statement.Entries == 0)
        //                {
        //                    DeuEntry.Statement.StatementStatusId = 0;
        //                    DeuEntry.Statement.EnteredAmount = 0;
        //                }
        //                else
        //                {
        //                    DeuEntry.Statement.EnteredAmount -= (DeuEntry.CommissionTotal ?? 0);
        //                }

        //                bool IsBatchStarted = false;
        //                foreach (DLinq.Statement statement in DeuEntry.Statement.Batch.Statements)
        //                {
        //                    if (statement.Entries != 0)
        //                    {
        //                        IsBatchStarted = true;
        //                        break;
        //                    }
        //                }

        //                if (!IsBatchStarted)
        //                {
        //                    DeuEntry.Statement.Batch.EntryStatusId = 4;
        //                }

        //                //Update total entered amount and entry
        //                ActionLogger.Logger.WriteImportPolicyLog("starting UpdateTotalAmountAndEntry DeleteDeuEntry ", true);
        //                UpdateTotalAmountAndEntry(DeuEntry.Statement.StatementId, DeuEntryId);
        //                ActionLogger.Logger.WriteImportPolicyLog("after UpdateTotalAmountAndEntry DeleteDeuEntry ", true);
        //                ActionLogger.Logger.WriteImportPolicyLog("starting CreateModifiableBatchData DeleteDeuEntry ", true);
        //                Batch objBatch = new Batch();
        //                //batchStatementData.BatchData = Batch.CreateModifiableBatchData(DeuEntry.Statement.Batch);
        //                batchStatementData.BatchData = objBatch.CreateModifiableBatchData(DeuEntry.Statement.Batch);
        //                ActionLogger.Logger.WriteImportPolicyLog("Ending CreateModifiableBatchData DeleteDeuEntry ", true);
        //                ActionLogger.Logger.WriteImportPolicyLog("Starting CreateModifiableStatementData DeleteDeuEntry ", true);
        //                batchStatementData.StatementData = Statement.CreateModifiableStatementData(DeuEntry.Statement);
        //                ActionLogger.Logger.WriteImportPolicyLog("Ending CreateModifiableStatementData DeleteDeuEntry ", true);
        //                batchStatementData.ExposedDeu = null;

        //                DataModel.DeleteObject(DeuEntry);
        //                DataModel.SaveChanges();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ActionLogger.Logger.WriteImportPolicyLog("Issue in DeleteDeuEntry ", true);
        //        ActionLogger.Logger.WriteImportPolicyLog(ex.Message.ToString(), true);
        //    }
        //    return batchStatementData;
        //}

        public static ModifiyableBatchStatementData DeleteDeuEntry(Guid DeuEntryId)
        {
            ModifiyableBatchStatementData batchStatementData = new ModifiyableBatchStatementData();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.EntriesByDEU DeuEntry = DataModel.EntriesByDEUs.FirstOrDefault(s => s.DEUEntryID == DeuEntryId);
                    if (DeuEntry != null)
                    {
                        DeuEntry.Statement.Entries = DeuEntry.Statement.Entries - 1;

                        if (DeuEntry.Statement.Entries == 0)
                        {
                            DeuEntry.Statement.StatementStatusId = 0;
                            DeuEntry.Statement.EnteredAmount = 0;
                        }
                        else
                        {
                            DeuEntry.Statement.EnteredAmount -= (DeuEntry.CommissionTotal ?? 0);
                        }

                        bool IsBatchStarted = false;
                        foreach (DLinq.Statement statement in DeuEntry.Statement.Batch.Statements)
                        {
                            if (statement.Entries != 0)
                            {
                                IsBatchStarted = true;
                                break;
                            }
                        }

                        if (!IsBatchStarted)
                        {
                            DeuEntry.Statement.Batch.EntryStatusId = 4;
                        }

                        //Update total entered amount and entry

                        UpdateTotalAmountAndEntry(DeuEntry.Statement.StatementId, DeuEntryId);
                        Batch objBatch = new Batch();
                        //batchStatementData.BatchData = Batch.CreateModifiableBatchData(DeuEntry.Statement.Batch);
                        batchStatementData.BatchData = objBatch.CreateModifiableBatchData(DeuEntry.Statement.Batch);
                        batchStatementData.StatementData = Statement.CreateModifiableStatementData(DeuEntry.Statement);
                        batchStatementData.ExposedDeu = null;

                        DataModel.DeleteObject(DeuEntry);
                        DataModel.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("ModifiyableBatchStatementData DeleteDeuEntry " + ex.InnerException.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("ModifiyableBatchStatementData DeleteDeuEntry " + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("ModifiyableBatchStatementData DeleteDeuEntry " + ex.Message.ToString(), true);
             //   throw ex;
            }
            return batchStatementData;
        }

        //public void DeleteDeuEntryByID(Guid _DeuEntryID)
        //{

        //    if (_DeuEntryID == Guid.Empty)
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //        {
        //            DataModel.CommandTimeout = 360000;
        //            DLinq.EntriesByDEU _deu = (from c in DataModel.EntriesByDEUs
        //                                       where (c.DEUEntryID == _DeuEntryID)
        //                                       select c).FirstOrDefault();

        //            if (_deu != null)
        //            {
        //                DataModel.EntriesByDEUs.DeleteObject(_deu);
        //                DataModel.SaveChanges();                        
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ActionLogger.Logger.WriteImportPolicyLog("Issue while deleting by ID : " + ex.Message.ToString(), true);
        //        ActionLogger.Logger.WriteImportPolicyLog(ex.Message.ToString(), true);
        //    }
        //}


        public void DeleteDeuEntryByID(Guid _DeuEntryID)
        {
            if (_DeuEntryID == Guid.Empty)
            {
                return;
            }
            try
            {
                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                string adoConnStr = sc.ConnectionString;

                using (SqlConnection con = new SqlConnection(adoConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_deleteDeuEntry", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DEUEntryID", _DeuEntryID);
                        con.Open();
                        int intCount = cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeleteDeuEntryByID : " + ex.Message, true);
                ActionLogger.Logger.WriteImportPolicyLog("DeleteDeuEntryByID : " + ex.StackTrace.ToString(), true);
               // throw ex;
            }
        }

        public void DeleteDeuEntryAndPaymentEntryByDeuID(Guid _DeuEntryID)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.CommandTimeout = 360000;

                    DLinq.PolicyPaymentEntry _PaymentEntry = (from c in DataModel.PolicyPaymentEntries
                                                              where (c.DEUEntryId == _DeuEntryID)
                                                              select c).FirstOrDefault();

                    if (_PaymentEntry != null)
                    {

                        DataModel.PolicyPaymentEntries.DeleteObject(_PaymentEntry);
                        DataModel.SaveChanges();
                    }

                    DLinq.EntriesByDEU _deu = (from c in DataModel.EntriesByDEUs
                                               where (c.DEUEntryID == _DeuEntryID)
                                               select c).FirstOrDefault();

                    if (_deu != null)
                    {

                        DataModel.EntriesByDEUs.DeleteObject(_deu);
                        DataModel.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeleteDeuEntryAndPaymentEntryByDeuID : " + ex.InnerException.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("DeleteDeuEntryAndPaymentEntryByDeuID : " + ex.StackTrace.ToString(), true);
            }
        }

        //public static ExposedDEU CreateExposedDEU(DLinq.EntriesByDEU deu)
        public ExposedDEU CreateExposedDEU(DLinq.EntriesByDEU deu)
        {
            ExposedDEU exposedDeu = new ExposedDEU();

            try
            {
                exposedDeu.IsPaymentFailure = false;
                exposedDeu.DEUENtryID = deu.DEUEntryID;
                exposedDeu.ClientName = deu.ClientValue;
                exposedDeu.UnlinkClientName = deu.ClientValue;
                exposedDeu.Insured = deu.Insured;
                exposedDeu.PaymentRecived = deu.PaymentReceived;

                PolicyDetailsData pol = (new Policy()).GetPolicyStting(deu.PolicyID ?? Guid.Empty);
                exposedDeu.PolicyNumber = (pol != null && pol.PolicyId != Guid.Empty) ? pol.PolicyNumber : deu.PolicyNumber;

                exposedDeu.Units = deu.NumberOfUnits;
                exposedDeu.InvoiceDate = deu.InvoiceDate;
                exposedDeu.CommissionTotal = deu.CommissionTotal;
                exposedDeu.Fee = deu.Fee;
                exposedDeu.SplitPercentage = deu.SplitPer;
                exposedDeu.EntryDate = deu.EntryDate;

                exposedDeu.CarrierNickName = Carrier.GetCarrierNickName(deu.PayorId ?? Guid.Empty, deu.CarrierId ?? Guid.Empty);
                if (string.IsNullOrEmpty(exposedDeu.CarrierNickName))
                    exposedDeu.CarrierNickName = deu.CarrierName;

                exposedDeu.CoverageNickName = Coverage.GetCoverageNickName(deu.PayorId ?? Guid.Empty, deu.CarrierId ?? Guid.Empty, deu.CoverageId ?? Guid.Empty);

                if (string.IsNullOrEmpty(exposedDeu.CoverageNickName))
                    exposedDeu.CoverageNickName = deu.ProductName;

                exposedDeu.CarrierName = deu.CarrierName;
                exposedDeu.ProductName = deu.ProductName;

                exposedDeu.PostStatus = (PostCompleteStatusEnum)(deu.PostCompleteStatus ?? 0);
                exposedDeu.CommissionPercentage = deu.CommissionPercentage;
                DLinq.PolicyPaymentEntry entry = deu.PolicyPaymentEntries.FirstOrDefault();
                if (entry != null)
                {
                    exposedDeu.CreatedDate = entry.CreatedOn;
                }
                else
                {
                    ActionLogger.Logger.WriteImportPolicyLog("CreateExposedDEU: no payment entry for the DEU entry, flag false ", true);
                    exposedDeu.IsPaymentFailure = true;
                }
                
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("CreateExposedDEU " + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("CreateExposedDEU " + ex.InnerException.ToString(), true);
                //throw ex;
            }
            return exposedDeu;
        }

        //public static List<DEU> GetDEUPolicyIdWise(Guid Policyid)
        public List<DEU> GetDEUPolicyIdWise(Guid Policyid)
        {
            List<DEU> _deu = null;

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    _deu = (from f in DataModel.EntriesByDEUs
                            where (f.PolicyID == Policyid)
                            select
      new DEU

      {

          DEUENtryID = f.DEUEntryID,
          OriginalEffectiveDate = f.OriginalEffectiveDate,
          PaymentRecived = f.PaymentReceived ?? 0,
          CommissionPercentage = f.CommissionPercentage ?? 0,
          Insured = f.Insured,
          PolicyNumber = f.PolicyNumber,
          Enrolled = f.Enrolled,
          Eligible = f.Eligible,
          Link1 = f.Link1,
          SplitPer = f.SplitPer,
          PolicyMode = f.PolicyModeID,
          TrackFromDate = f.TrackFromDate,
          CompScheduleType = f.CompScheduleType,
          CompTypeID = f.CompTypeID,
          ClientID = f.ClientID ?? Guid.Empty,
          ClientName = f.Client.Name,
          StmtID = f.StatementID ?? Guid.Empty,
          PostStatusID = f.PostStatusID,
          PolicyId = f.PolicyID ?? Guid.Empty,
          InvoiceDate = f.InvoiceDate,
          PayorId = f.PayorId,
          NoOfUnits = f.NumberOfUnits,
          DollerPerUnit = f.DollerPerUnit ?? 0,
          Fee = f.Fee ?? 0,
          Bonus = f.Bonus ?? 0,
          CommissionTotal = f.CommissionTotal ?? 0,
          PayorSysID = f.PayorSysID,
          Renewal = f.Renewal,
          CarrierID = f.CarrierId,
          CoverageID = f.CoverageId,
          IsEntrybyCommissiondashBoard = f.IsEntrybyCommissiondashBoard ?? false,
          CreatedBy = f.CreatedBy ?? Guid.Empty,
          EntryDate = f.EntryDate,
          CarrierName = f.CarrierName,
          ProductName = f.ProductName,
          PostCompleteStatus = f.PostCompleteStatus ?? 0,
      }
                ).ToList();

                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetDEUPolicyIdWise :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("GetDEUPolicyIdWise :" + ex.InnerException.ToString(), true);
            }
            return _deu;

        }

        public static List<DataEntryField> GetDeuFields(Guid DeuEntryID)
        {
            List<DataEntryField> deuFields = new List<DataEntryField>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    List<string> _DeuFields = new List<string>();
                    foreach (var field in Enum.GetValues(typeof(ExposedDeuField)))
                    {
                        _DeuFields.Add(field.ToString());
                    }

                    DLinq.EntriesByDEU deuData = DataModel.EntriesByDEUs.FirstOrDefault(s => s.DEUEntryID == DeuEntryID);

                    foreach (string field in _DeuFields)
                    {
                        GetDeuFieldData(deuData, field, deuFields);
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetDeuFields " + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("GetDeuFields " + ex.InnerException.ToString(), true);
            }
            return deuFields;
        }

        public static void GetDeuFieldData(DLinq.EntriesByDEU deuData, string FieldName, List<DataEntryField> entries)
        {
            DataEntryField deuField = new DataEntryField();
            try
            {
                switch (FieldName)
                {
                    case "PolicyNumber":
                        deuField.DeuFieldName = FieldName;
                        if (deuData != null)
                            deuField.DeuFieldValue = deuData.PolicyNumber;
                        deuField.DeuFieldType = 3;

                        entries.Add(deuField);

                        break;
                    case "Insured":
                        deuField.DeuFieldName = FieldName;
                        if (deuData != null)
                            deuField.DeuFieldValue = deuData.Insured;
                        deuField.DeuFieldType = 3;
                        entries.Add(deuField);
                        break;
                    case "Carrier":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.Carrier != null)
                                deuField.DeuFieldValue = deuData.Carrier.CarrierNickNames.FirstOrDefault(s => s.PayorId == deuData.PayorId).NickName;
                            else
                                //deuField.DeuFieldValue = string.Empty;
                                deuField.DeuFieldValue = deuData.CarrierName; ;

                        deuField.DeuFieldType = 3;
                        entries.Add(deuField);
                        break;
                    case "Product":
                        deuField.DeuFieldName = FieldName;
                        if (deuData != null)
                            if (deuData.Carrier != null && deuData.Coverage != null)
                                deuField.DeuFieldValue = deuData.Coverage.CoverageNickNames.FirstOrDefault(s => s.PayorId == deuData.PayorId && s.CarrierId == deuData.CarrierId).NickName;
                            else
                                //deuField.DeuFieldValue = string.Empty;
                                deuField.DeuFieldValue = deuData.ProductName; ;

                        deuField.DeuFieldType = 3;
                        entries.Add(deuField);
                        break;
                    case "ModelAvgPremium":
                        deuField.DeuFieldName = FieldName;
                        if (deuData != null)
                            deuField.DeuFieldValue = deuData.ModalAvgPremium;
                        deuField.DeuFieldType = 2;
                        entries.Add(deuField);
                        break;
                    case "PolicyMode":
                        deuField.DeuFieldName = FieldName;
                        if (deuData != null)
                            deuField.DeuFieldValue = deuData.PolicyModeValue;
                        deuField.DeuFieldType = 3;
                        entries.Add(deuField);
                        break;
                    case "Enrolled":
                        deuField.DeuFieldName = FieldName;
                        if (deuData != null)
                            deuField.DeuFieldValue = deuData.Enrolled;
                        deuField.DeuFieldType = 2;
                        entries.Add(deuField);
                        break;
                    case "SplitPercentage":
                        deuField.DeuFieldName = FieldName;
                        if (deuData != null)
                            deuField.DeuFieldValue = deuData.SplitPer.ToString();
                        deuField.DeuFieldType = 2;
                        entries.Add(deuField);
                        break;
                    case "Client":
                        deuField.DeuFieldName = FieldName;
                        if (deuData != null)
                            deuField.DeuFieldValue = deuData.ClientValue;
                        deuField.DeuFieldType = 3;
                        entries.Add(deuField);
                        break;
                    case "CompType":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.CompTypeID != null)
                                deuField.DeuFieldValue = deuData.CompTypeID.ToString();
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 3;
                        entries.Add(deuField);
                        break;
                    case "PayorSysId":
                        deuField.DeuFieldName = FieldName;
                        if (deuData != null)
                            deuField.DeuFieldValue = deuData.PayorSysID;
                        deuField.DeuFieldType = 3;
                        entries.Add(deuField);
                        break;
                    case "Renewal":
                        deuField.DeuFieldName = FieldName;
                        if (deuData != null)
                            deuField.DeuFieldValue = deuData.Renewal;
                        deuField.DeuFieldType = 3;
                        entries.Add(deuField);
                        break;
                    case "CompScheduleType":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.CompScheduleType != null)
                                deuField.DeuFieldValue = deuData.CompScheduleType.ToString();
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 3;
                        entries.Add(deuField);
                        break;
                    case "InvoiceDate":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.InvoiceDate != null)
                                deuField.DeuFieldValue = deuData.InvoiceDate.Value.ToString("MM/dd/yyyy");
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 1;
                        entries.Add(deuField);
                        break;
                    case "Effective":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.OriginalEffectiveDate != null)
                                deuField.DeuFieldValue = deuData.OriginalEffectiveDate.Value.ToString("MM/dd/yyyy");
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 1;
                        entries.Add(deuField);
                        break;

                    case "PaymentReceived":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.PaymentReceived != null)
                                deuField.DeuFieldValue = deuData.PaymentReceived.ToString();
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 2;
                        entries.Add(deuField);
                        break;
                    case "CommissionPercentage":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.CommissionPercentage != null)
                                deuField.DeuFieldValue = deuData.CommissionPercentage.ToString();
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 2;
                        entries.Add(deuField);
                        break;
                    case "NumberOfUnits":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.NumberOfUnits != null)
                                deuField.DeuFieldValue = deuData.NumberOfUnits.ToString();
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 2;
                        entries.Add(deuField);
                        break;
                    case "DollerPerUnit":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.DollerPerUnit != null)
                                deuField.DeuFieldValue = deuData.DollerPerUnit.ToString();
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 2;
                        entries.Add(deuField);
                        break;
                    case "Fee":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.Fee != null)
                                deuField.DeuFieldValue = deuData.Fee.ToString();
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 2;
                        entries.Add(deuField);
                        break;
                    case "Bonus":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.Bonus != null)
                                deuField.DeuFieldValue = deuData.Bonus.ToString();
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 2;
                        entries.Add(deuField);
                        break;
                    case "CommissionTotal":
                        deuField.DeuFieldName = FieldName;

                        if (deuData != null)
                            if (deuData.CommissionTotal != null)
                                deuField.DeuFieldValue = deuData.CommissionTotal.ToString();
                            else
                                deuField.DeuFieldValue = string.Empty;

                        deuField.DeuFieldType = 2;
                        entries.Add(deuField);
                        break;

                    case "EntryDate":
                        deuField.DeuFieldName = FieldName;
                        break;

                    case "OtherData":
                        if (deuData != null)
                        {
                            if (deuData.OtherData != null)
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(XmlFields));
                                StringReader stringReader = new StringReader(deuData.OtherData);
                                XmlFields xmlFields = (XmlFields)serializer.Deserialize(stringReader);

                                foreach (DataEntryField field in xmlFields.FieldCollection)
                                {
                                    deuField = new DataEntryField();
                                    deuField.DeuFieldName = field.DeuFieldName;
                                    deuField.DeuFieldType = field.DeuFieldType;
                                    deuField.DeuFieldValue = field.DeuFieldValue;
                                    entries.Add(deuField);
                                }
                            }
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetDeuFieldData :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("GetDeuFieldData :" + ex.InnerException.ToString(), true);
            }

        }

        public static DEU GetDeuEntryidWise(Guid DeuEntryID)
        {
            ActionLogger.Logger.WriteImportPolicyLog("GetDeuEntryidWise:process begins for DeuEntryId: " + DeuEntryID, true);
            if (DeuEntryID == Guid.Empty)
                return null;
            DEU _deu = null;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.EntriesByDEU deuEntry = DataModel.EntriesByDEUs.FirstOrDefault(s => s.DEUEntryID == DeuEntryID);
                    Guid? carrierId, coverageId;
                    string strClient = string.Empty;

                    if (deuEntry != null)
                    {
                        if (deuEntry.ClientID != null)
                        {
                            Client objClient = Client.GetClient(deuEntry.ClientID.Value);

                            if (objClient != null)
                            {
                                strClient = objClient.Name;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(strClient))
                                {
                                    strClient = deuEntry.ClientValue;
                                }
                            }
                        }

                    }

                    if (deuEntry != null)
                    {
                        carrierId = deuEntry.CarrierId;
                        coverageId = deuEntry.CoverageId;
                        _deu = new DEU();
                        _deu.DEUENtryID = deuEntry.DEUEntryID;
                        _deu.OriginalEffectiveDate = deuEntry.OriginalEffectiveDate;
                        _deu.PaymentRecived = deuEntry.PaymentReceived ?? 0;
                        _deu.CommissionPercentage = deuEntry.CommissionPercentage ?? 0;
                        _deu.Insured = deuEntry.Insured;
                        _deu.PolicyNumber = deuEntry.PolicyNumber;
                        _deu.Enrolled = deuEntry.Enrolled;
                        _deu.Eligible = deuEntry.Eligible;
                        _deu.Link1 = deuEntry.Link1;
                        _deu.SplitPer = deuEntry.SplitPer;
                        _deu.PolicyMode = deuEntry.PolicyModeID;
                        _deu.TrackFromDate = deuEntry.TrackFromDate;
                        _deu.CompScheduleType = deuEntry.CompScheduleType;
                        _deu.CompTypeID = deuEntry.CompTypeID;
                        _deu.ClientID = deuEntry.ClientID ?? Guid.Empty;
                        //_deu.ClientName = deuEntry.ClientID != null ? Client.GetClient(deuEntry.ClientID.Value).Name : deuEntry.ClientValue;
                        _deu.ClientName = deuEntry.ClientID != null ? strClient : deuEntry.ClientValue;
                        _deu.StmtID = deuEntry.StatementID ?? Guid.Empty;
                        _deu.PostStatusID = deuEntry.PostStatusID;
                        _deu.PolicyId = deuEntry.PolicyID ?? Guid.Empty;
                        _deu.InvoiceDate = deuEntry.InvoiceDate;
                        _deu.PayorId = deuEntry.PayorId;
                        _deu.NoOfUnits = deuEntry.NumberOfUnits;
                        _deu.DollerPerUnit = deuEntry.DollerPerUnit ?? 0;
                        _deu.Fee = deuEntry.Fee ?? 0;
                        _deu.Bonus = deuEntry.Bonus ?? 0;
                        //_deu.CommissionPaid1 = deuEntry.CommissionPaid1 ?? 0;
                        //_deu.CommissionPaid2 = deuEntry.CommissionPaid2 ?? 0;
                        _deu.CommissionTotal = deuEntry.CommissionTotal ?? 0;
                        //_deu.AmountBilled = deuEntry.BilledAmount ?? 0;
                        _deu.PayorSysID = deuEntry.PayorSysID;
                        _deu.Renewal = deuEntry.Renewal;
                        _deu.CarrierID = deuEntry.CarrierId;
                        _deu.CoverageID = deuEntry.CoverageId;
                        _deu.IsEntrybyCommissiondashBoard = deuEntry.IsEntrybyCommissiondashBoard ?? false;
                        _deu.CreatedBy = deuEntry.CreatedBy ?? Guid.Empty;
                        _deu.PostCompleteStatus = deuEntry.PostCompleteStatus ?? 0;

                        _deu.CarrierName = deuEntry.CarrierName;
                        _deu.ProductName = deuEntry.ProductName;
                        _deu.EntryDate = deuEntry.EntryDate;

                        _deu.UnlinkClientName = deuEntry.UnlinkClientName;

                        if (deuEntry.Statement != null)
                        {
                            _deu.TemplateID = deuEntry.Statement.TemplateID;
                        }

                        if (!string.IsNullOrEmpty(deuEntry.OtherData))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(XmlFields));
                            StringReader stringReader = new StringReader(deuEntry.OtherData);
                            XmlFields xmlFields = (XmlFields)serializer.Deserialize(stringReader);
                            _deu.XmlData = xmlFields;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetDeuEntryidWise exception :" + ex.StackTrace.ToStringDump(), true);
                if (ex.InnerException != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("GetDeuEntryidWise Inner exception :" + ex.InnerException.Message, true);
                }
                //throw ex;
            }

            return _deu;
        }

        public static DEU GetDeuEntryidWiseForUnlikingPolicy(Guid DeuEntryID)
        {
            if (DeuEntryID == Guid.Empty)
                return null;
            DEU _deu = null;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.EntriesByDEU deuEntry = DataModel.EntriesByDEUs.FirstOrDefault(s => s.DEUEntryID == DeuEntryID);
                    Guid? carrierId, coverageId;

                    if (deuEntry != null)
                    {
                        carrierId = deuEntry.CarrierId;
                        coverageId = deuEntry.CoverageId;

                        _deu = new DEU();

                        _deu.DEUENtryID = deuEntry.DEUEntryID;
                        _deu.OriginalEffectiveDate = deuEntry.OriginalEffectiveDate;
                        _deu.PaymentRecived = deuEntry.PaymentReceived ?? 0;
                        _deu.CommissionPercentage = deuEntry.CommissionPercentage ?? 0;
                        _deu.Insured = deuEntry.Insured;
                        _deu.PolicyNumber = deuEntry.PolicyNumber;
                        _deu.Enrolled = deuEntry.Enrolled;
                        _deu.Eligible = deuEntry.Eligible;
                        _deu.Link1 = deuEntry.Link1;
                        _deu.SplitPer = deuEntry.SplitPer;
                        _deu.PolicyMode = deuEntry.PolicyModeID;
                        _deu.TrackFromDate = deuEntry.TrackFromDate;
                        _deu.CompScheduleType = deuEntry.CompScheduleType;
                        _deu.CompTypeID = deuEntry.CompTypeID;
                        _deu.ClientID = deuEntry.ClientID ?? Guid.Empty;

                        _deu.ClientName = deuEntry.ClientValue != null ? deuEntry.ClientValue : Client.GetClient(deuEntry.ClientID.Value).Name;

                        _deu.StmtID = deuEntry.StatementID ?? Guid.Empty;
                        _deu.PostStatusID = deuEntry.PostStatusID;
                        _deu.PolicyId = deuEntry.PolicyID ?? Guid.Empty;
                        _deu.InvoiceDate = deuEntry.InvoiceDate;
                        _deu.PayorId = deuEntry.PayorId;
                        _deu.NoOfUnits = deuEntry.NumberOfUnits;
                        _deu.DollerPerUnit = deuEntry.DollerPerUnit ?? 0;
                        _deu.Fee = deuEntry.Fee ?? 0;
                        _deu.Bonus = deuEntry.Bonus ?? 0;
                        //_deu.CommissionPaid1 = deuEntry.CommissionPaid1 ?? 0;
                        //_deu.CommissionPaid2 = deuEntry.CommissionPaid2 ?? 0;
                        _deu.CommissionTotal = deuEntry.CommissionTotal ?? 0;
                        //_deu.AmountBilled = deuEntry.BilledAmount ?? 0;
                        _deu.PayorSysID = deuEntry.PayorSysID;
                        _deu.Renewal = deuEntry.Renewal;
                        _deu.CarrierID = deuEntry.CarrierId;
                        _deu.CoverageID = deuEntry.CoverageId;
                        _deu.IsEntrybyCommissiondashBoard = deuEntry.IsEntrybyCommissiondashBoard ?? false;
                        _deu.CreatedBy = deuEntry.CreatedBy ?? Guid.Empty;
                        _deu.PostCompleteStatus = deuEntry.PostCompleteStatus ?? 0;

                        _deu.CarrierName = deuEntry.CarrierName;
                        _deu.ProductName = deuEntry.ProductName;
                        _deu.EntryDate = deuEntry.EntryDate;
                        _deu.UnlinkClientName = deuEntry.UnlinkClientName;

                        if (!string.IsNullOrEmpty(deuEntry.OtherData))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(XmlFields));
                            StringReader stringReader = new StringReader(deuEntry.OtherData);
                            XmlFields xmlFields = (XmlFields)serializer.Deserialize(stringReader);
                            _deu.XmlData = xmlFields;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetDeuEntryidWiseForUnlikingPolicy :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("GetDeuEntryidWiseForUnlikingPolicy :" + ex.InnerException.ToString(), true);
            }
            return _deu;
        }

        public static bool IsPaymentFromCommissionDashBoardByPaymentEntryId(Guid PolicPaymentId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PolicyPaymentEntry policypaymentEntry = DataModel.PolicyPaymentEntries.Where(p => p.PaymentEntryId == PolicPaymentId).FirstOrDefault();
                if (policypaymentEntry != null)
                {
                    DLinq.EntriesByDEU entriesbyDEU = DataModel.EntriesByDEUs.Where(p => p.DEUEntryID == policypaymentEntry.DEUEntryId).FirstOrDefault();
                    if (entriesbyDEU == null) return false;
                    return entriesbyDEU.IsEntrybyCommissiondashBoard ?? false;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsPaymentFromCommissionDashBoardByDEUEntryId(Guid DeuEntryId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.EntriesByDEU entriesbyDEU = DataModel.EntriesByDEUs.Where(p => p.DEUEntryID == DeuEntryId).FirstOrDefault();
                if (entriesbyDEU == null) return false;
                return entriesbyDEU.IsEntrybyCommissiondashBoard ?? false;
            }
        }

        public static string GetProductTypeNickName(Guid policyID, Guid PayorID, Guid CarrierID, Guid CoverageID)
        {
            string strNickName = string.Empty;

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var VarCovergeNickName = from p in DataModel.EntriesByDEUs
                                             where (p.PolicyID == policyID && p.PayorId == PayorID && p.CarrierId == CarrierID && p.CoverageId == CoverageID)
                                             select p.ProductName;

                    foreach (var item in VarCovergeNickName)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            strNickName = Convert.ToString(item);
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetProductTypeNickName :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("GetProductTypeNickName :" + ex.InnerException.ToString(), true);
            }

            return strNickName;
        }

        #endregion
    }

    [DataContract]
    [XmlRoot]
    public class XmlFields
    {
        public List<DataEntryField> FieldCollection { get; set; }

        public XmlFields()
        {
            FieldCollection = new List<DataEntryField>();
        }

        public void Add(DataEntryField field)
        {
            FieldCollection.Add(field);
        }
    }

    [DataContract]
    public class DataEntryField
    {
        [XmlAttribute]
        [DataMember]
        public string DeuFieldName { get; set; }
        [XmlElement]
        [DataMember]
        public byte DeuFieldType { get; set; }
        [XmlElement]
        [DataMember]
        public string DeuFieldValue { get; set; }
    }
}
