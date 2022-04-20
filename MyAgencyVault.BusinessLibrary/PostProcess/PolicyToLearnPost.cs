using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Reflection;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PolicyToLearnPost
    {
        #region "Add Update policy to learn field without renewal"
        public static void AddUpdatPolicyToLearn(Guid PolicyId, DEUFields deufields = null)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("PolicyId", PolicyId);
                PolicyDetailsData _Policy = Policy.GetPolicyData(parameters).FirstOrDefault();

                var _policyLearnedField = (from p in DataModel.PolicyLearnedFields where (p.PolicyId == PolicyId) select p).FirstOrDefault();
                if (_policyLearnedField == null)
                {
                    _policyLearnedField = new DLinq.PolicyLearnedField
                    {
                        PolicyId = PolicyId,
                        ClientID = _Policy.ClientId,
                        Insured = _Policy.Insured,
                        PolicyNumber = _Policy.PolicyNumber,
                        Effective = _Policy.OriginalEffectiveDate,
                        Enrolled = _Policy.Enrolled,
                        Eligible = _Policy.Eligible,
                        SplitPercentage = Convert.ToDecimal(_Policy.SplitPercentage),
                        CompTypeID = _Policy.IncomingPaymentTypeId,
                        PolicyModeId = _Policy.PolicyModeId,
                        PayorId = _Policy.PayorId == Guid.Empty ? null : _Policy.PayorId,
                        CarrierId = _Policy.CarrierID == Guid.Empty ? null : _Policy.CarrierID,
                        CoverageId = _Policy.CoverageId == Guid.Empty ? null : _Policy.CoverageId,
                        LastModifiedOn = DateTime.Today,
                        LastModifiedUserCredentialid = _Policy.CreatedBy,
                        //TrackFrom = _Policy.TrackFromDate,
                        ModalAvgPremium = _Policy.ModeAvgPremium,

                        UserCredentialId = _Policy.UserCredentialId,
                        AccoutExec = _Policy.AccoutExec
                       
                    };
                    _policyLearnedField.TrackFrom = PostUtill.CalculateTrackFromDate(_Policy);
                    _policyLearnedField.PAC = "$" + Convert.ToString(PostUtill.CalculatePAC(PolicyId));
                    _policyLearnedField.PMC = "$" + Convert.ToString(PostUtill.CalculatePMC(PolicyId));

                    try
                    {
                        if (string.IsNullOrEmpty(_policyLearnedField.PolicyNumber))
                        {
                            _policyLearnedField.PolicyNumber = _policyLearnedField.Insured;
                        }
                    }
                    catch
                    {
                    }

                    if (deufields != null)
                    {
                        DEU _DEU = deufields.DeuData;
                        PropertyInfo[] Properties = typeof(DLinq.PolicyLearnedField).GetProperties();
                        foreach (PropertyInfo property in Properties)
                        {
                            PropertyInfo SourceProperty = typeof(DEU).GetProperty(property.Name);
                            if (SourceProperty != null && property.PropertyType != typeof(Guid) && property.PropertyType != typeof(Guid?))
                            {
                                var Value = SourceProperty.GetValue(_DEU, null);
                                if (Value != null)
                                {
                                    property.SetValue(_policyLearnedField, Value, null);
                                }
                            }
                        }
                        _policyLearnedField.CoverageNickName = deufields.DeuData.ProductName;
                        _policyLearnedField.CarrierNickName = deufields.DeuData.CarrierName;

                        _policyLearnedField.ProductType = deufields.DeuData.ProductName;
                    }
                    DataModel.AddToPolicyLearnedFields(_policyLearnedField);

                }
                else
                {
                    List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);

                    bool bCnt = (_PolicyPaymentEntriesPost == null || _PolicyPaymentEntriesPost.Count == 0) ? true : false;

                    if ((_policyLearnedField.ClientID == null || _policyLearnedField.ClientID == Guid.Empty) || bCnt)
                        _policyLearnedField.ClientID = _Policy.ClientId;

                    if ((String.IsNullOrEmpty(_policyLearnedField.Insured)) || bCnt)
                        _policyLearnedField.Insured = _Policy.Insured;

                    if ((String.IsNullOrEmpty(_policyLearnedField.PolicyNumber)) || bCnt)
                        _policyLearnedField.PolicyNumber = _Policy.PolicyNumber;

                    if ((_policyLearnedField.Effective == null) || bCnt)
                        _policyLearnedField.Effective = _Policy.OriginalEffectiveDate;

                    if ((String.IsNullOrEmpty(_policyLearnedField.Enrolled)) || bCnt)
                        _policyLearnedField.Enrolled = _Policy.Enrolled;

                    if ((String.IsNullOrEmpty(_policyLearnedField.Eligible)) || bCnt)
                        _policyLearnedField.Eligible = _Policy.Eligible;

                    if ((_policyLearnedField.SplitPercentage == null) || bCnt)
                        _policyLearnedField.SplitPercentage = Convert.ToDecimal(_Policy.SplitPercentage);

                    if ((_policyLearnedField.CompTypeID == null) || bCnt)
                        _policyLearnedField.CompTypeID = _Policy.IncomingPaymentTypeId;

                    if ((_policyLearnedField.PolicyModeId == null) || bCnt)
                    _policyLearnedField.PolicyModeId = _Policy.PolicyModeId;

                    if ((_policyLearnedField.PayorId == null || _policyLearnedField.PayorId == Guid.Empty) || bCnt)
                        _policyLearnedField.PayorId = _Policy.CarrierID == Guid.Empty ? null : _Policy.PayorId;

                    if ((_policyLearnedField.CarrierId == null || _policyLearnedField.CarrierId == Guid.Empty) || bCnt)
                        _policyLearnedField.CarrierId = _Policy.CarrierID == Guid.Empty ? null : _Policy.CarrierID;

                    if ((_policyLearnedField.CoverageId == null || _policyLearnedField.CoverageId == Guid.Empty) || bCnt)
                        _policyLearnedField.CoverageId = _Policy.CoverageId == Guid.Empty ? null : _Policy.CoverageId;

                    //if ((_policyLearnedField.TrackFrom == null) || bCnt)
                    //    _policyLearnedField.TrackFrom = _Policy.TrackFromDate;

                    _policyLearnedField.TrackFrom = PostUtill.CalculateTrackFromDate(_Policy);

                    if ((_policyLearnedField.ModalAvgPremium == null || _policyLearnedField.ModalAvgPremium == 0) || bCnt)
                        _policyLearnedField.ModalAvgPremium = _Policy.ModeAvgPremium;

                    _policyLearnedField.LastModifiedOn = DateTime.Today;
                    _policyLearnedField.LastModifiedUserCredentialid = _Policy.CreatedBy;


                    try
                    {

                        _policyLearnedField.UserCredentialId = _Policy.UserCredentialId;
                        _policyLearnedField.AccoutExec = _Policy.AccoutExec;
                    }
                    catch
                    {
                    }
                   // _policyLearnedField.ProductType = _Policy.ProductType;


                }
                DataModel.SaveChanges();
            }
        }

        public static void AddUpdatPolicyToLearn(DLinq.Policy _Policy, DEUFields deufields = null)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    //Dictionary<string, object> parameters = new Dictionary<string, object>();
                    //parameters.Add("PolicyId", PolicyId);
                    //PolicyDetailsData _Policy = Policy.GetPolicyData(parameters).FirstOrDefault();
                    var _policyLearnedField = (from p in DataModel.PolicyLearnedFields where (p.PolicyId == _Policy.PolicyId) select p).FirstOrDefault();
                    if (_policyLearnedField == null)
                    {
                        _policyLearnedField = new DLinq.PolicyLearnedField
                        {
                            PolicyId = _Policy.PolicyId,
                            ClientID = _Policy.Client.ClientId,
                            Insured = _Policy.Insured,
                            PolicyNumber = _Policy.PolicyNumber,
                            Effective = _Policy.OriginalEffectiveDate,
                            Enrolled = _Policy.Enrolled,
                            Eligible = _Policy.Eligible,
                            SplitPercentage = Convert.ToDecimal(_Policy.SplitPercentage),
                            CompTypeID = _Policy.IncomingPaymentTypeId,
                            PolicyModeId = _Policy.PolicyModeId,
                            PayorId = _Policy.PayorId == Guid.Empty ? null : _Policy.PayorId,
                            CoverageId = _Policy.CoverageId == Guid.Empty ? null : _Policy.CoverageId,
                            LastModifiedOn = DateTime.Today,
                            LastModifiedUserCredentialid = _Policy.CreatedBy,
                            //TrackFrom = _Policy.TrackFromDate,
                            ModalAvgPremium = _Policy.MonthlyPremium,

                            UserCredentialId = _Policy.UserCredentialId,
                            AccoutExec = _Policy.AccoutExec

                        };
                        if (_Policy.Carrier == null)
                        {
                            _policyLearnedField.CarrierId = null;
                        }
                        else
                        {
                            _policyLearnedField.CarrierId = _Policy.Carrier.CarrierId;
                        }

                        //Update effective date
                        _policyLearnedField.Effective = _Policy.OriginalEffectiveDate;
                        _policyLearnedField.TrackFrom = PostUtill.CalculateTrackFromDate(_Policy.PolicyId);
                        _policyLearnedField.PAC = "$" + Convert.ToString(PostUtill.CalculatePAC(_Policy.PolicyId));
                        _policyLearnedField.PMC = "$" + Convert.ToString(PostUtill.CalculatePMC(_Policy.PolicyId));
                        if (deufields != null)
                        {
                            DEU _DEU = deufields.DeuData;
                            PropertyInfo[] Properties = typeof(DLinq.PolicyLearnedField).GetProperties();
                            foreach (PropertyInfo property in Properties)
                            {
                                PropertyInfo SourceProperty = typeof(DEU).GetProperty(property.Name);
                                if (SourceProperty != null && property.PropertyType != typeof(Guid) && property.PropertyType != typeof(Guid?))
                                {
                                    var Value = SourceProperty.GetValue(_DEU, null);
                                    if (Value != null)
                                    {
                                        property.SetValue(_policyLearnedField, Value, null);
                                    }
                                }
                            }
                            _policyLearnedField.CoverageNickName = deufields.DeuData.ProductName;
                            _policyLearnedField.CarrierNickName = deufields.DeuData.CarrierName;
                            _policyLearnedField.ProductType = deufields.DeuData.ProductName;

                            try
                            {
                                if (string.IsNullOrEmpty(_policyLearnedField.PolicyNumber))
                                {
                                    _policyLearnedField.PolicyNumber = _policyLearnedField.Insured;
                                }
                            }
                            catch(Exception ex)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog("AddUpdatPolicyToLearn:Exception occurs while processing" + ex.Message, true);
                               // throw ex;
                            }
                        }
                        DataModel.AddToPolicyLearnedFields(_policyLearnedField);

                    }
                    else
                    {
                        List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(_Policy.PolicyId);

                        bool bCnt = (_PolicyPaymentEntriesPost == null || _PolicyPaymentEntriesPost.Count == 0) ? true : false;

                        if ((_policyLearnedField.ClientID == null || _policyLearnedField.ClientID == Guid.Empty) || bCnt)
                            _policyLearnedField.ClientID = _Policy.Client.ClientId;

                        if ((String.IsNullOrEmpty(_policyLearnedField.Insured)) || bCnt)
                            _policyLearnedField.Insured = _Policy.Insured;

                        if ((String.IsNullOrEmpty(_policyLearnedField.PolicyNumber)) || bCnt)
                            _policyLearnedField.PolicyNumber = _Policy.PolicyNumber;
                       

                        if ((String.IsNullOrEmpty(_policyLearnedField.Enrolled)) || bCnt)
                            _policyLearnedField.Enrolled = _Policy.Enrolled;

                        if ((String.IsNullOrEmpty(_policyLearnedField.Eligible)) || bCnt)
                            _policyLearnedField.Eligible = _Policy.Eligible;

                        if ((_policyLearnedField.SplitPercentage == null) || bCnt)
                            _policyLearnedField.SplitPercentage = Convert.ToDecimal(_Policy.SplitPercentage);

                        if ((_policyLearnedField.CompTypeID == null) || bCnt)
                            _policyLearnedField.CompTypeID = _Policy.IncomingPaymentTypeId;

                        //if ((_policyLearnedField.PolicyModeId == null) || bCnt)
                        _policyLearnedField.PolicyModeId = _Policy.PolicyModeId;

                        if ((_policyLearnedField.PayorId == null || _policyLearnedField.PayorId == Guid.Empty) || bCnt)
                            _policyLearnedField.PayorId = _Policy.Carrier.CarrierId == Guid.Empty ? null : _Policy.PayorId;

                        if ((_policyLearnedField.CarrierId == null || _policyLearnedField.CarrierId == Guid.Empty) || bCnt)
                            if (_Policy.Carrier != null)
                            {
                                _policyLearnedField.CarrierId = _Policy.Carrier.CarrierId == Guid.Empty ? Guid.Empty : _Policy.Carrier.CarrierId;
                            }
                            else
                            {
                                _policyLearnedField.CarrierId = null;
                            }

                        if ((_policyLearnedField.CoverageId == null || _policyLearnedField.CoverageId == Guid.Empty) || bCnt)
                            _policyLearnedField.CoverageId = _Policy.CoverageId == Guid.Empty ? null : _Policy.CoverageId;

                        _policyLearnedField.Effective = _Policy.OriginalEffectiveDate;

                        _policyLearnedField.TrackFrom = PostUtill.CalculateTrackFromDate(_Policy.PolicyId);

                        if ((_policyLearnedField.ModalAvgPremium == null || _policyLearnedField.ModalAvgPremium == 0) || bCnt)
                            _policyLearnedField.ModalAvgPremium = _Policy.MonthlyPremium;

                        _policyLearnedField.LastModifiedOn = DateTime.Today;
                        _policyLearnedField.LastModifiedUserCredentialid = _Policy.CreatedBy;

                        try
                        {
                            _policyLearnedField.UserCredentialId = _Policy.UserCredentialId;
                            _policyLearnedField.AccoutExec = _Policy.AccoutExec;
                        }
                        catch
                        {
                        }

                    }
                    DataModel.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdatPolicyToLearn :" + ex.InnerException.ToString(), true);
            }
        }

        #endregion

        #region "Add policy to learn field with renewal"
        public static void AddPolicyToLearn(Guid PolicyId, string strRenewal, string strCoverageNickName,string strProductType, Guid? CurrentUserID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("PolicyId", PolicyId);
                    PolicyDetailsData _Policy = Policy.GetPolicyData(parameters).FirstOrDefault();

                    var _policyLearnedField = (from p in DataModel.PolicyLearnedFields where (p.PolicyId == PolicyId) select p).FirstOrDefault();
                    if (_policyLearnedField == null)
                    {
                        _policyLearnedField = new DLinq.PolicyLearnedField
                        {
                            PolicyId = PolicyId,
                            ClientID = _Policy.ClientId,
                            Insured = _Policy.Insured,
                            PolicyNumber = _Policy.PolicyNumber,
                            Effective = _Policy.OriginalEffectiveDate,
                            Enrolled = _Policy.Enrolled,
                            Eligible = _Policy.Eligible,
                            SplitPercentage = Convert.ToDecimal(_Policy.SplitPercentage),
                            CompTypeID = _Policy.IncomingPaymentTypeId,
                            PolicyModeId = _Policy.PolicyModeId,
                            PayorId = _Policy.PayorId == Guid.Empty ? null : _Policy.PayorId,
                            CarrierId = _Policy.CarrierID == Guid.Empty ? null : _Policy.CarrierID,
                            CoverageId = _Policy.CoverageId == Guid.Empty ? null : _Policy.CoverageId,
                            LastModifiedOn = DateTime.Today,
                            LastModifiedUserCredentialid = CurrentUserID, // Acme - get current user _Policy.CreatedBy,
                            Renewal = _Policy.RenewalPercentage,
                            Advance = _Policy.Advance,
                            //TrackFrom = _Policy.TrackFromDate,
                            ModalAvgPremium = _Policy.ModeAvgPremium,
                            ProductType = _Policy.ProductType,

                            UserCredentialId = _Policy.UserCredentialId,
                            AccoutExec = _Policy.AccoutExec,
                        };

                        //update effective date
                        _policyLearnedField.Effective = _Policy.OriginalEffectiveDate;
                        _policyLearnedField.TrackFrom = PostUtill.CalculateTrackFromDate(PolicyId);
                        _policyLearnedField.Renewal = strRenewal;
                        _policyLearnedField.PAC = "$" + Convert.ToString(PostUtill.CalculatePAC(PolicyId));
                        _policyLearnedField.PMC = "$" + Convert.ToString(PostUtill.CalculatePMC(PolicyId));
                        //Update coverage nick name
                        _policyLearnedField.CoverageNickName = strCoverageNickName;
                        DataModel.AddToPolicyLearnedFields(_policyLearnedField);

                    }
                    else
                    {
                        List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);

                        bool bCnt = (_PolicyPaymentEntriesPost == null || _PolicyPaymentEntriesPost.Count == 0) ? true : false;

                        if ((_policyLearnedField.ClientID == null || _policyLearnedField.ClientID == Guid.Empty) || bCnt)
                            _policyLearnedField.ClientID = _Policy.ClientId;

                        if ((String.IsNullOrEmpty(_policyLearnedField.Insured)) || bCnt)
                            _policyLearnedField.Insured = _Policy.Insured;

                        if ((String.IsNullOrEmpty(_policyLearnedField.PolicyNumber)) || bCnt)
                            _policyLearnedField.PolicyNumber = _Policy.PolicyNumber;

                        //if ((_policyLearnedField.Effective == null) || bCnt)
                        //    _policyLearnedField.Effective = _Policy.OriginalEffectiveDate;

                        if ((String.IsNullOrEmpty(_policyLearnedField.Enrolled)) || bCnt)
                            _policyLearnedField.Enrolled = _Policy.Enrolled;

                        if ((String.IsNullOrEmpty(_policyLearnedField.Eligible)) || bCnt)
                            _policyLearnedField.Eligible = _Policy.Eligible;

                        //if ((_policyLearnedField.SplitPercentage == null) || bCnt)
                        //    _policyLearnedField.SplitPercentage = Convert.ToDecimal(_Policy.SplitPercentage);

                        _policyLearnedField.SplitPercentage = Convert.ToDecimal(_Policy.SplitPercentage);

                        if ((_policyLearnedField.CompTypeID == null) || bCnt)
                            _policyLearnedField.CompTypeID = _Policy.IncomingPaymentTypeId;

                        //update renewal percentage (change on 6 jan 2012)
                        if ((_policyLearnedField.Renewal == null) || _policyLearnedField.Renewal == "0" || bCnt)
                            _policyLearnedField.Renewal = strRenewal;

                        //if ((_policyLearnedField.PolicyModeId == null) || bCnt)
                        _policyLearnedField.PolicyModeId = _Policy.PolicyModeId;

                        _policyLearnedField.PAC = "$" + Convert.ToString(PostUtill.CalculatePAC(PolicyId));
                        _policyLearnedField.PMC = "$" + Convert.ToString(PostUtill.CalculatePMC(PolicyId));

                        if ((_policyLearnedField.PayorId == null || _policyLearnedField.PayorId == Guid.Empty) || bCnt)
                            _policyLearnedField.PayorId = _Policy.PayorId == Guid.Empty ? null : _Policy.PayorId;

                        if ((_policyLearnedField.CarrierId == null || _policyLearnedField.CarrierId == Guid.Empty) || bCnt)
                            _policyLearnedField.CarrierId = _Policy.CarrierID == Guid.Empty ? null : _Policy.CarrierID;

                        if ((_policyLearnedField.CoverageId == null || _policyLearnedField.CoverageId == Guid.Empty) || bCnt)
                            _policyLearnedField.CoverageId = _Policy.CoverageId == Guid.Empty ? null : _Policy.CoverageId;

                        //if ((_policyLearnedField.TrackFrom == null) || bCnt)
                        //    _policyLearnedField.TrackFrom = _Policy.TrackFromDate;

                        //Update effective date 
                        _policyLearnedField.Effective = _Policy.OriginalEffectiveDate;

                        _policyLearnedField.TrackFrom = PostUtill.CalculateTrackFromDate(PolicyId);

                        if ((_policyLearnedField.ModalAvgPremium == null || _policyLearnedField.ModalAvgPremium == 0) || bCnt)
                            _policyLearnedField.ModalAvgPremium = _Policy.ModeAvgPremium;


                        _policyLearnedField.LastModifiedOn = DateTime.Today;
                        _policyLearnedField.LastModifiedUserCredentialid = CurrentUserID; // _Policy.CreatedBy;

                        //Update coverage nick name
                        _policyLearnedField.CoverageNickName = strCoverageNickName;
                        _policyLearnedField.ProductType = strProductType;

                        _policyLearnedField.UserCredentialId = _Policy.UserCredentialId;
                        _policyLearnedField.AccoutExec = _Policy.AccoutExec;

                    }
                    DataModel.SaveChanges();
                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("Exception AddPolicyToLearn: " + ex.Message, true);
                }
            }
        #endregion
        }

        public static void AddLearnedAfterImport(Guid PolicyId, string strRenewal, string strCoverageNickName,string strProductType, string ImportID, string agencyName  = "",string PolPlanID="")
        {
            ActionLogger.Logger.WriteImportPolicyLog("AddlearnedafterImport starts: importID - " + ImportID, true, agencyName);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("PolicyId", PolicyId);
                    PolicyDetailsData _Policy = Policy.GetPolicyDetailsOnPolicyID(PolicyId, agencyName);// (parameters).FirstOrDefault();
                    ActionLogger.Logger.WriteImportPolicyLog("AddlearnedafterImport Policy Fetched:", true, agencyName);
                    var _policyLearnedField = (from p in DataModel.PolicyLearnedFields where (/*p.ImportPolicyID == ImportID &&*/ p.PolicyId == PolicyId) select p).FirstOrDefault();
                    ActionLogger.Logger.WriteImportPolicyLog("AddlearnedafterImport Object Created:", true, agencyName);
                    if (_policyLearnedField == null)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("In new ", true, agencyName);
                        _policyLearnedField = new DLinq.PolicyLearnedField
                        {
                            PolicyId = PolicyId,
                            ClientID = _Policy.ClientId,
                            Insured = _Policy.Insured,
                            PolicyNumber = _Policy.PolicyNumber,
                            Effective = _Policy.OriginalEffectiveDate,
                            Enrolled = _Policy.Enrolled,
                            Eligible = _Policy.Eligible,
                            SplitPercentage = Convert.ToDecimal(_Policy.SplitPercentage),
                            CompTypeID = _Policy.IncomingPaymentTypeId,
                            PolicyModeId = _Policy.PolicyModeId,
                            PayorId = _Policy.PayorId == Guid.Empty ? null : _Policy.PayorId,
                            CarrierId = _Policy.CarrierID == Guid.Empty ? null : _Policy.CarrierID,
                            CoverageId = _Policy.CoverageId == Guid.Empty ? null : _Policy.CoverageId,
                            LastModifiedOn = DateTime.Today,
                            LastModifiedUserCredentialid = _Policy.CreatedBy,
                            Renewal = _Policy.RenewalPercentage,
                            Advance = _Policy.Advance,
                            TrackFrom = _Policy.TrackFromDate,
                            ModalAvgPremium = _Policy.ModeAvgPremium,
                            ProductType = _Policy.ProductType,
                            AutoTerminationDate = _Policy.PolicyTerminationDate,
                            UserCredentialId = _Policy.UserCredentialId,
                            AccoutExec = _Policy.AccoutExec,
                            ImportPolicyID = ImportID,
                            BG_PolicyPlanID = PolPlanID,
                        };

                        //update effective date
                        _policyLearnedField.Effective = _Policy.OriginalEffectiveDate;
                        _policyLearnedField.TrackFrom = PostUtill.CalculateTrackFromDate(PolicyId, agencyName); //Acme commented - as it will always be null here, per the logic 
                        _policyLearnedField.Renewal = strRenewal;
                        _policyLearnedField.PAC = "$" + Convert.ToString(PostUtill.CalculatePAC(PolicyId, agencyName));
                        _policyLearnedField.PMC = "$" + Convert.ToString(PostUtill.CalculatePMC(PolicyId, agencyName));
                        //Update coverage nick name
                        _policyLearnedField.CoverageNickName = strCoverageNickName;
                        _policyLearnedField.BG_PolicyPlanID = PolPlanID;
                        DataModel.AddToPolicyLearnedFields(_policyLearnedField);

                    }
                    else
                    {
                        // List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);
                        ActionLogger.Logger.WriteImportPolicyLog("In update ", true, agencyName);
                        //    bool bCnt = (_PolicyPaymentEntriesPost == null || _PolicyPaymentEntriesPost.Count == 0) ? true : false;
                        ActionLogger.Logger.WriteImportPolicyLog("AddlearnedafterImport update case ", true, agencyName);

                        _policyLearnedField.ImportPolicyID = ImportID;

                        if ((_policyLearnedField.ClientID == null || _policyLearnedField.ClientID == Guid.Empty))
                            _policyLearnedField.ClientID = _Policy.ClientId;

                        if ((String.IsNullOrEmpty(_policyLearnedField.Insured)))
                            _policyLearnedField.Insured = _Policy.Insured;

                        if ((String.IsNullOrEmpty(_policyLearnedField.PolicyNumber)))
                            _policyLearnedField.PolicyNumber = _Policy.PolicyNumber;

                        if ((String.IsNullOrEmpty(_policyLearnedField.Enrolled)))
                            _policyLearnedField.Enrolled = _Policy.Enrolled;

                        if ((String.IsNullOrEmpty(_policyLearnedField.Eligible)))
                            _policyLearnedField.Eligible = _Policy.Eligible;

                        if ((_policyLearnedField.SplitPercentage == null))
                            _policyLearnedField.SplitPercentage = Convert.ToDecimal(_Policy.SplitPercentage);


                        if ((_policyLearnedField.CompTypeID == null))
                            _policyLearnedField.CompTypeID = _Policy.IncomingPaymentTypeId;

                        //update renewal percentage (change on 6 jan 2012)
                        if ((_policyLearnedField.Renewal == null) || _policyLearnedField.Renewal == "0")
                            _policyLearnedField.Renewal = strRenewal;

                        if ((_policyLearnedField.PolicyModeId == null))
                            _policyLearnedField.PolicyModeId = _Policy.PolicyModeId;

                        if ((_policyLearnedField.PayorId == null || _policyLearnedField.PayorId == Guid.Empty))
                            _policyLearnedField.PayorId = _Policy.PayorId == Guid.Empty ? null : _Policy.PayorId;

                        if ((_policyLearnedField.CarrierId == null || _policyLearnedField.CarrierId == Guid.Empty))
                            _policyLearnedField.CarrierId = _Policy.CarrierID == Guid.Empty ? null : _Policy.CarrierID;

                        if ((_policyLearnedField.CoverageId == null || _policyLearnedField.CoverageId == Guid.Empty))
                            _policyLearnedField.CoverageId = _Policy.CoverageId == Guid.Empty ? null : _Policy.CoverageId;

                        //if ((_policyLearnedField.TrackFrom == null))
                        //    _policyLearnedField.TrackFrom = _Policy.TrackFromDate;

                        if (_policyLearnedField.TrackFrom == null)
                            _policyLearnedField.TrackFrom = PostUtill.CalculateTrackFromDate(PolicyId, agencyName);

                        if (_policyLearnedField.ModalAvgPremium == null || _policyLearnedField.ModalAvgPremium == 0)
                            _policyLearnedField.ModalAvgPremium = _Policy.ModeAvgPremium;


                        if (_policyLearnedField.LastModifiedUserCredentialid == null)
                            _policyLearnedField.LastModifiedUserCredentialid = _Policy.CreatedBy;

                        //Update coverage nick name
                        if (_policyLearnedField.CoverageNickName == null)
                            _policyLearnedField.CoverageNickName = strCoverageNickName;

                        if (_policyLearnedField.ProductType == null)
                            _policyLearnedField.ProductType = strProductType;

                        if (_policyLearnedField.UserCredentialId == null)
                            _policyLearnedField.UserCredentialId = _Policy.UserCredentialId;

                        //if (_policyLearnedField.BG_PolicyPlanID == null)
                        _policyLearnedField.BG_PolicyPlanID = PolPlanID;

                        _policyLearnedField.LastModifiedOn = DateTime.Now;

                        //Must update fields
                        if (_Policy.AccoutExec != null) _policyLearnedField.AccoutExec = _Policy.AccoutExec;
                        if (_Policy.OriginalEffectiveDate != null) _policyLearnedField.Effective = _Policy.OriginalEffectiveDate;
                        if (_Policy.PolicyTerminationDate != null) _policyLearnedField.AutoTerminationDate = _Policy.PolicyTerminationDate;
                        
                        //Updating PAC/PMC -  as they should whenever any change in incoming schedule,mode 
                    //    if (string.IsNullOrEmpty(_policyLearnedField.PAC))
                            _policyLearnedField.PAC = "$" + Convert.ToString(PostUtill.CalculatePAC(PolicyId, agencyName));
                      //  if (string.IsNullOrEmpty(_policyLearnedField.PMC))
                            _policyLearnedField.PMC = "$" + Convert.ToString(PostUtill.CalculatePMC(PolicyId, agencyName));
                    }
                    DataModel.SaveChanges();
                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("Exception AddlearnedafterImport: " + ex.Message, true, agencyName);
                    if(ex.InnerException != null)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("Exception inner AddlearnedafterImport: " + ex.InnerException.ToStringDump(), true, agencyName);
                    }
                }
            }
        }
    }

}
