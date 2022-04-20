using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PolicyLearnedFieldData
    {
        #region "data members aka - public properties"
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public string Insured { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public DateTime? Effective { get; set; }
        [DataMember]
        public DateTime? TrackFrom { get; set; }
        [DataMember]
        public string Renewal { get; set; }
        [DataMember]
        public Guid? CarrierId { get; set; }
        [DataMember]
        public Guid? CoverageId { get; set; }
        [DataMember]
        public string PAC { get; set; }
        [DataMember]
        public string PMC { get; set; }
        [DataMember]
        public decimal? ModalAvgPremium { get; set; }
        [DataMember]
        public int? PolicyModeId { get; set; }
        [DataMember]
        public string Enrolled { get; set; }
        [DataMember]
        public string Eligible { get; set; }
        [DataMember]
        public DateTime? AutoTerminationDate { get; set; }
        [DataMember]
        public string PayorSysId { get; set; }
        [DataMember]
        public string Link1 { get; set; }
        [DataMember]
        public decimal? Link2 { get; set; }
        [DataMember]
        public DateTime? LastModifiedOn { get; set; }
        [DataMember]
        public Guid? LastModifiedUserCredentialId { get; set; }
        [DataMember]
        public Guid ClientID { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string CarrierNickName { get; set; }
        [DataMember]
        public string CoverageNickName { get; set; }
        [DataMember]
        public int? CompTypeId { get; set; }
        [DataMember]
        public string CompScheduleType { get; set; }
        [DataMember]
        public Guid? PayorId { get; set; }
        [DataMember]
        public DateTime? PreviousEffectiveDate { get; set; }
        [DataMember]
        public int? PreviousPolicyModeid { get; set; }
        [DataMember]
        public DateTime? PrevoiusTrackFromDate { get; set; }
        [DataMember]
        public int? Advance { get; set; }

        //Added new property
        //Seleted product type for policy manager
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public string ImportPolicyID { get; set; }
        [DataMember]
        public string LastModifiedUserName { get; set; }
        [DataMember]
        public string LastModifiedDetail { get; set; }
        #endregion
    }
    public class PolicyLearnedField : IEditable<PolicyLearnedFieldData>
    {
        #region IEditable<PolicyLearnedField> Members
        public void AddUpdate()
        {
        }

        public static void AddUpdateLearned(PolicyLearnedFieldData PolicyLernField, string strProductType)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    // var _policyLearnedField = (from p in DataModel.PolicyLearnedFields where (p.PolicyLearnedFieldId == PolicyLernField.PolicyLearnedFieldId) select p).FirstOrDefault();
                    var _policyLearnedField = (from p in DataModel.PolicyLearnedFields where (p.PolicyId == PolicyLernField.PolicyId) select p).FirstOrDefault();
                    // List<PolicyLearnedField> _sPl = GetPolicyLearnedFieldsPolicyWise(this.PolicyId);
                    if (_policyLearnedField == null)
                    {
                        _policyLearnedField = new DLinq.PolicyLearnedField
                        {
                            // PolicyLearnedFieldId = (PolicyLernField.PolicyLearnedFieldId == null || PolicyLernField.PolicyLearnedFieldId==Guid.Empty) ? Guid.NewGuid() : PolicyLernField.PolicyLearnedFieldId,
                            Insured = PolicyLernField.Insured,
                            PolicyNumber = PolicyLernField.PolicyNumber,
                            Effective = PolicyLernField.Effective,
                            TrackFrom = PolicyLernField.TrackFrom,
                            Renewal = PolicyLernField.Renewal,
                            //Calculate PAC And PMC
                            PAC = "$" + Convert.ToString(PostUtill.CalculatePAC(PolicyLernField.PolicyId)),
                            PMC = "$" + Convert.ToString(PostUtill.CalculatePMC(PolicyLernField.PolicyId)),
                            ModalAvgPremium = PolicyLernField.ModalAvgPremium,
                            PolicyModeId = PolicyLernField.PolicyModeId,
                            Enrolled = PolicyLernField.Enrolled,
                            Eligible = PolicyLernField.Eligible,
                            AutoTerminationDate = PolicyLernField.AutoTerminationDate,
                            PayorSysID = PolicyLernField.PayorSysId,
                            Link1 = PolicyLernField.Link1,
                            SplitPercentage = PolicyLernField.Link2,
                            LastModifiedOn = PolicyLernField.LastModifiedOn,
                            LastModifiedUserCredentialid = PolicyLernField.LastModifiedUserCredentialId,
                            CompScheduleType = PolicyLernField.CompScheduleType,
                            //Save carier nick Name
                            CarrierNickName = PolicyLernField.CarrierNickName,
                            //Save Coverage nick Name
                            CoverageNickName = PolicyLernField.CoverageNickName,
                            ProductType = PolicyLernField.ProductType,
                            ImportPolicyID = PolicyLernField.ImportPolicyID, //acme added
                        };
                        _policyLearnedField.PolicyReference.Value = (from s in DataModel.Policies where s.PolicyId == PolicyLernField.PolicyId select s).FirstOrDefault();
                        _policyLearnedField.MasterPolicyModeReference.Value = (from s in DataModel.MasterPolicyModes where s.PolicyModeId == PolicyLernField.PolicyModeId select s).FirstOrDefault();
                        _policyLearnedField.PayorReference.Value = (from s in DataModel.Payors where s.PayorId == PolicyLernField.PayorId select s).FirstOrDefault();
                        _policyLearnedField.CarrierReference.Value = (from s in DataModel.Carriers where s.CarrierId == PolicyLernField.CarrierId select s).FirstOrDefault();
                        _policyLearnedField.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == PolicyLernField.CoverageId select s).FirstOrDefault();
                        _policyLearnedField.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == PolicyLernField.ClientID select s).FirstOrDefault();

                        _policyLearnedField.MasterIncomingPaymentTypeReference.Value = (from s in DataModel.MasterIncomingPaymentTypes where s.IncomingPaymentTypeId == PolicyLernField.CompTypeId select s).FirstOrDefault();
                        DataModel.AddToPolicyLearnedFields(_policyLearnedField);

                    }
                    else
                    {

                        _policyLearnedField.PreviousEffectiveDate = _policyLearnedField.Effective;
                        _policyLearnedField.PreviousPolicyModeId = _policyLearnedField.PolicyModeId;
                        _policyLearnedField.Insured = PolicyLernField.Insured;
                        _policyLearnedField.PolicyNumber = PolicyLernField.PolicyNumber;
                        _policyLearnedField.Effective = PolicyLernField.Effective;
                        _policyLearnedField.TrackFrom = PolicyLernField.TrackFrom;
                        _policyLearnedField.Renewal = PolicyLernField.Renewal;
                        _policyLearnedField.Effective = PolicyLernField.Effective;
                        _policyLearnedField.PAC = "$" + Convert.ToString(PostUtill.CalculatePAC(PolicyLernField.PolicyId));
                        _policyLearnedField.PMC = "$" + Convert.ToString(PostUtill.CalculatePMC(PolicyLernField.PolicyId));
                        _policyLearnedField.ModalAvgPremium = PolicyLernField.ModalAvgPremium;
                        _policyLearnedField.PolicyModeId = PolicyLernField.PolicyModeId;
                        _policyLearnedField.Enrolled = PolicyLernField.Enrolled;
                        _policyLearnedField.Eligible = PolicyLernField.Eligible;
                        _policyLearnedField.AutoTerminationDate = PolicyLernField.AutoTerminationDate;
                        _policyLearnedField.PayorSysID = PolicyLernField.PayorSysId;
                        _policyLearnedField.Link1 = PolicyLernField.Link1;
                        _policyLearnedField.SplitPercentage = PolicyLernField.Link2;
                        _policyLearnedField.CompScheduleType = PolicyLernField.CompScheduleType;
                        _policyLearnedField.LastModifiedOn = PolicyLernField.LastModifiedOn;
                        _policyLearnedField.LastModifiedUserCredentialid = PolicyLernField.LastModifiedUserCredentialId;
                        //update carrier nick name
                        _policyLearnedField.CarrierNickName = PolicyLernField.CarrierNickName;
                        //update coverage nick name          
                        _policyLearnedField.CoverageNickName = PolicyLernField.CoverageNickName;

                        _policyLearnedField.ProductType = PolicyLernField.ProductType;
                        //Acme added 
                        _policyLearnedField.ImportPolicyID = (!string.IsNullOrEmpty(PolicyLernField.ImportPolicyID)) ? PolicyLernField.ImportPolicyID : _policyLearnedField.ImportPolicyID;

                        _policyLearnedField.PolicyReference.Value = (from s in DataModel.Policies where s.PolicyId == PolicyLernField.PolicyId select s).FirstOrDefault();
                        _policyLearnedField.PayorReference.Value = (from s in DataModel.Payors where s.PayorId == PolicyLernField.PayorId select s).FirstOrDefault();
                        _policyLearnedField.CarrierReference.Value = (from s in DataModel.Carriers where s.CarrierId == PolicyLernField.CarrierId select s).FirstOrDefault();
                        _policyLearnedField.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == PolicyLernField.CoverageId select s).FirstOrDefault();
                        _policyLearnedField.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == PolicyLernField.ClientID select s).FirstOrDefault();
                        _policyLearnedField.MasterIncomingPaymentTypeReference.Value = (from s in DataModel.MasterIncomingPaymentTypes where s.IncomingPaymentTypeId == PolicyLernField.CompTypeId select s).FirstOrDefault();

                    }
                    DataModel.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdateLearned ex.message" + ex.Message.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdateLearned ex.StackTrace" + ex.StackTrace.ToString(), true);
            }
        }

        public static void UpdateLearnedFieldsMode(Guid _PolicyID, int PolicyMode)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _poLearFiel = (from p in DataModel.PolicyLearnedFields where (p.PolicyId == _PolicyID) select p).FirstOrDefault();

                if (_poLearFiel != null)
                {
                    //update smart mode
                    _poLearFiel.PolicyModeId = PolicyMode;
                    DataModel.SaveChanges();
                    //Update Policy
                    Policy.UpdateMode(_PolicyID, PolicyMode);
                }

            }

        }
        public static PolicyLearnedFieldData GetPolicyLearnedFieldsOnPOlicy(Guid policyId)
        {
            PolicyLearnedFieldData learned = null;
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GetSmartFieldsForPolicy", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@policyId", policyId);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            learned = new PolicyLearnedFieldData()
                            {
                                PolicyId = policyId,
                                Insured = dr.IsDBNull("Insured") ? "" : (string)dr["Insured"],
                                ClientName = dr.IsDBNull("ClientName") ? "" : (string)dr["ClientName"],
                                PolicyNumber = dr.IsDBNull("PolicyNumber") ? "" : (string)dr["PolicyNumber"],
                                Effective = (dr["Effective"] == DBNull.Value) ? (DateTime?)null : (DateTime?)dr["Effective"],
                                TrackFrom = (dr["TrackFrom"] == DBNull.Value) ? (DateTime?)null : (DateTime?)dr["TrackFrom"],
                                PAC = dr.IsDBNull("PAC") ? "" : (string)dr["PAC"],
                                PMC = dr.IsDBNull("PMC") ? "" : (string)dr["PMC"],
                                ModalAvgPremium = dr.IsDBNull("ModalAvgPremium") ? 0 : (decimal)dr["ModalAvgPremium"],
                                Renewal = dr.IsDBNull("Renewal") ? "" : (string)dr["Renewal"],
                                //RenewalPercentage= Convert.ToDecimal(ss.Renewal),
                                CarrierId = dr.IsDBNull("CarrierId") ? (Guid?)null : (Guid)dr["CarrierId"],
                                CoverageId = dr.IsDBNull("CoverageId") ? (Guid?)null : (Guid)dr["CoverageId"],
                                CompTypeId = dr.IsDBNull("CompTypeId") ? 0 : (int)dr["CompTypeId"],
                                //Add to get carrier nick name
                                CarrierNickName = dr.IsDBNull("CarrierNickName") ? "" : (string)dr["CarrierNickName"],
                                //Add to get covrage nick name
                                CoverageNickName = dr.IsDBNull("CoverageNickName") ? "" : (string)dr["CoverageNickName"],
                                PolicyModeId = dr.IsDBNull("PolicyModeId") ? 0 : (int)dr["PolicyModeId"],
                                Enrolled = dr.IsDBNull("Enrolled") ? "" : (string)dr["Enrolled"],
                                Eligible = dr.IsDBNull("Eligible") ? "" : (string)dr["Eligible"],
                                AutoTerminationDate = dr.IsDBNull("AutoTerminationDate") ? (DateTime?)null : (DateTime?)dr["AutoTerminationDate"],
                                PayorSysId = dr.IsDBNull("PayorSysId") ? "" : (string)dr["PayorSysId"],
                                Link1 = dr.IsDBNull("Link1") ? "" : (string)dr["Link1"],
                                Link2 = dr.IsDBNull("SplitPercentage") ? 0 : (decimal)dr["SplitPercentage"],
                                LastModifiedOn = dr.IsDBNull("PolicyModifiedDate") ? (DateTime?)null : (DateTime?)dr["PolicyModifiedDate"],
                                LastModifiedUserCredentialId = dr.IsDBNull("LastModifiedUserCredentialId") ? (Guid?)null : (Guid)dr["LastModifiedUserCredentialId"],
                                ClientID = dr.IsDBNull("ClientID") ? Guid.Empty : (Guid)dr["ClientID"],
                                CompScheduleType = dr.IsDBNull("CompScheduleType") ? "" : (string)dr["CompScheduleType"],
                                PayorId = dr.IsDBNull("PayorId") ? (Guid?)null : (Guid)dr["PayorId"],
                                PreviousEffectiveDate = dr.IsDBNull("PreviousEffectiveDate") ? (DateTime?)null : (DateTime?)dr["PreviousEffectiveDate"],
                                PreviousPolicyModeid = dr.IsDBNull("PreviousPolicyModeid") ? 0 : (int)dr["PreviousPolicyModeid"],
                                ProductType = dr.IsDBNull("ProductType") ? "" : (string)dr["ProductType"],
                                ImportPolicyID = dr.IsDBNull("ImportPolicyID") ? "" : (string)dr["ImportPolicyID"],
                                LastModifiedUserName = dr.IsDBNull("UserName") ? "" : (string)dr["UserName"]
                            };
                        }
                    }
                }
                ActionLogger.Logger.WriteImportPolicyLog("GetPolicyLearnedFieldsOnPOlicy success ", true);

                if (learned != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("GetPolicyLearnedFieldsOnPOlicy setting last modified ", true);
                    learned.LastModifiedDetail = "";
                    string msg = learned.LastModifiedOn == null ? "" : " on " + Convert.ToDateTime(learned.LastModifiedOn).ToString("MM/dd/yyyy hh:mm tt");

                    if (!string.IsNullOrEmpty(learned.LastModifiedUserName))
                    {
                        learned.LastModifiedDetail = "Last modified by " + learned.LastModifiedUserName + msg;
                    }
                    else if (learned.LastModifiedOn != null)
                    {
                        learned.LastModifiedDetail = "Last modified" + msg;
                    }
                    //else
                    //{
                    //    learned.LastModifiedDetail = "";
                    //}
                    ActionLogger.Logger.WriteImportPolicyLog("GetPolicyLearnedFieldsOnPOlicy last modified " + learned.LastModifiedDetail, true);
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception GetPolicyLearnedFieldsOnPOlicy: " + ex.Message, true);
            }

            return learned;
        }
        public static PolicyLearnedFieldData GetPolicyLearnedFieldsPolicyWise(Guid _PolicyID, string agencyName = "")
        {
            PolicyLearnedFieldData _poLearFiel = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                #region comment for sp
                try
                {

                    _poLearFiel = (from ss in DataModel.PolicyLearnedFields
                                   where (ss.PolicyId == _PolicyID)
                                   select new PolicyLearnedFieldData
                                   {
                                       PolicyId = ss.Policy.PolicyId,
                                       Insured = ss.Insured,
                                       PolicyNumber = ss.PolicyNumber,
                                       Effective = ss.Effective.Value,
                                       TrackFrom = ss.TrackFrom.Value,
                                       PAC = ss.PAC,
                                       PMC = ss.PMC,
                                       ModalAvgPremium = ss.ModalAvgPremium,
                                       Renewal = ss.Renewal,
                                       CarrierId = ss.Carrier.CarrierId,
                                       CoverageId = ss.Coverage.CoverageId,
                                       CompTypeId = ss.MasterIncomingPaymentType.IncomingPaymentTypeId,
                                       //Add to get carrier nick name
                                       CarrierNickName = ss.CarrierNickName,
                                       //Add to get covrage nick name
                                       CoverageNickName = ss.CoverageNickName,
                                       PolicyModeId = ss.MasterPolicyMode.PolicyModeId,
                                       Enrolled = ss.Enrolled,
                                       Eligible = ss.Eligible,
                                       AutoTerminationDate = ss.AutoTerminationDate,
                                       PayorSysId = ss.PayorSysID,
                                       Link1 = ss.Link1,
                                       Link2 = ss.SplitPercentage,
                                       LastModifiedOn = ss.LastModifiedOn,
                                       LastModifiedUserCredentialId = ss.LastModifiedUserCredentialid,
                                       ClientID = ss.Client.ClientId,
                                       CompScheduleType = ss.CompScheduleType,
                                       PayorId = ss.Payor.PayorId,
                                       PreviousEffectiveDate = ss.PreviousEffectiveDate,
                                       PreviousPolicyModeid = ss.PreviousPolicyModeId,
                                       ProductType = ss.ProductType,
                                       ImportPolicyID = ss.ImportPolicyID,
                                   }
                             ).ToList().FirstOrDefault();
                    #endregion

                    if (_poLearFiel == null)
                        return _poLearFiel;

                    if (_poLearFiel.ClientID != Guid.Empty)
                    {
                        DLinq.Client client = DataModel.Clients.FirstOrDefault(s => s.ClientId == _poLearFiel.ClientID);
                        if (client != null)
                            _poLearFiel.ClientName = client.Name;
                    }
                    if (_poLearFiel.LastModifiedUserCredentialId != null)
                    {
                        _poLearFiel.LastModifiedUserName = DataModel.UserCredentials.FirstOrDefault(x => x.UserCredentialId == _poLearFiel.LastModifiedUserCredentialId).UserName;
                    }


                    if (_poLearFiel.PayorId == null || _poLearFiel.PayorId == Guid.Empty || _poLearFiel.CarrierId == null || _poLearFiel.CarrierId == Guid.Empty)
                        return _poLearFiel;
                    //{
                    _poLearFiel.CarrierNickName = Carrier.GetCarrierNickName(_poLearFiel.PayorId ?? Guid.Empty, _poLearFiel.CarrierId ?? Guid.Empty);
                    // }


                    if (_poLearFiel.PayorId == null || _poLearFiel.PayorId == Guid.Empty || _poLearFiel.CarrierId == null || _poLearFiel.CarrierId == Guid.Empty || _poLearFiel.CoverageId == null || _poLearFiel.CoverageId == Guid.Empty)
                        return _poLearFiel;

                    //Need to change yesterday
                    //_poLearFiel.CoverageNickName = Coverage.GetCoverageNickName(_poLearFiel.PayorId ?? Guid.Empty, _poLearFiel.CarrierId ?? Guid.Empty, _poLearFiel.CoverageId ?? Guid.Empty);
                    _poLearFiel.CoverageNickName = GetPolicyLearnedCoverageNickName((Guid)_PolicyID, (Guid)_poLearFiel.PayorId, (Guid)_poLearFiel.CarrierId, (Guid)_poLearFiel.CoverageId, agencyName);
                    _poLearFiel.PrevoiusTrackFromDate = _poLearFiel.TrackFrom;


                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("GetPolicyLearnedFieldsPolicyWise:Exception getting smart fields: " + ex.Message, true, agencyName);
                    // throw ex;
                }
            }
            return _poLearFiel;

        }

        public static DateTime? GetPolicyLearnedFieldsTrackDate(Guid _PolicyID)
        {
            DateTime? dtGetTime = null;

            PolicyLearnedFieldData _poLearFiel = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                _poLearFiel = (from ss in DataModel.PolicyLearnedFields
                               where (ss.PolicyId == _PolicyID)
                               select new PolicyLearnedFieldData
                               {
                                   PolicyId = ss.Policy.PolicyId,
                                   TrackFrom = ss.TrackFrom,
                               }).ToList().FirstOrDefault();


                if (_poLearFiel != null)
                {
                    if (_poLearFiel.TrackFrom != null)
                    {
                        dtGetTime = Convert.ToDateTime(_poLearFiel.TrackFrom);
                    }
                    else
                    {
                        dtGetTime = null;
                    }
                }

                else
                {
                    dtGetTime = null;
                }

                return dtGetTime;

            }

        }

        public static DateTime? GetPolicyLearnedFieldAutoTerminationDate(Guid _PolicyID)
        {
            DateTime? _poLearFielDateTime = null;
            PolicyLearnedFieldData _poLearFiel = null;
            string adoConnStr = DBConnection.GetConnectionString();
            SqlConnection con = null;

            try
            {
                using (con = new SqlConnection(adoConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetAutoterminaationDate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PolicyID", _PolicyID);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        // Call Read before accessing data. 
                        while (reader.Read())
                        {
                            try
                            {
                                _poLearFiel = new PolicyLearnedFieldData();
                                if (!string.IsNullOrEmpty(Convert.ToString(reader["AutoTerminationDate"])))
                                {
                                    _poLearFiel.AutoTerminationDate = Convert.ToDateTime(reader["AutoTerminationDate"]);
                                }
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
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
            return _poLearFielDateTime;

        }

        //public static DateTime? GetPolicyLearnedFieldAutoTerminationDate(Guid _PolicyID)
        //{
        //    DateTime? _poLearFielDateTime = null;

        //    PolicyLearnedFieldData _poLearFiel = null;
        //    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //    {
        //        _poLearFiel = (from ss in DataModel.PolicyLearnedFields
        //                       where (ss.PolicyId == _PolicyID)
        //                       select new PolicyLearnedFieldData
        //                       {
        //                           PolicyId = ss.Policy.PolicyId,
        //                           AutoTerminationDate = ss.AutoTerminationDate,
        //                       }).ToList().FirstOrDefault();


        //        if (_poLearFiel != null)
        //        {
        //            if (_poLearFiel.AutoTerminationDate != null)
        //            {
        //                _poLearFielDateTime = Convert.ToDateTime(_poLearFiel.AutoTerminationDate);
        //            }
        //            else
        //            {
        //                _poLearFielDateTime = null;
        //            }
        //        }

        //        else
        //        {
        //            _poLearFielDateTime = null;
        //        }

        //        return _poLearFielDateTime;

        //    }
        //}

        public static string GetPolicyLearnedCoverageNickName(Guid policyID, Guid PayorID, Guid CarrierID, Guid CoverageID, string agencyName = "")
        {
            string strNickName = string.Empty;

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var strCovergeNickNaick = from p in DataModel.PolicyLearnedFields
                                              where (p.PolicyId == policyID && p.PayorId == PayorID && p.CarrierId == CarrierID && p.CoverageId == CoverageID)
                                              select p.CoverageNickName;

                    foreach (var item in strCovergeNickNaick)
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

        public void Delete()
        {
        }

        public static void Delete(PolicyLearnedFieldData policyLearnedFieldData)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policylearnedrecord = (from p in DataModel.PolicyLearnedFields where (p.PolicyId == policyLearnedFieldData.PolicyId) select p).FirstOrDefault();
                if (_policylearnedrecord == null) return;
                DataModel.DeleteObject(_policylearnedrecord);
                DataModel.SaveChanges();
            }
        }

        public static void DeleteByPolicy(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policylearnedrecord = (from p in DataModel.PolicyLearnedFields where (p.PolicyId == PolicyId) select p).FirstOrDefault();
                if (_policylearnedrecord == null) return;
                DataModel.DeleteObject(_policylearnedrecord);
                DataModel.SaveChanges();
            }
        }

        public static bool IsValid(PolicyLearnedFieldData policy)
        {
            return true;
        }

        public static void AddUpdateHistoryLearned(Guid PolicyId)
        {
            try
            {
                List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);
                if (_PolicyPaymentEntriesPost.Count != 0) return;
                PolicyLearnedFieldData PolicyLernField = GetPolicyLearnedFieldsPolicyWise(PolicyId);
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    // var _policyLearnedField = (from p in DataModel.PolicyLearnedFields where (p.PolicyLearnedFieldId == PolicyLernField.PolicyLearnedFieldId) select p).FirstOrDefault();
                    var _policyLearnedField = (from p in DataModel.PolicyLearnedFieldsHistories where (p.PolicyId == PolicyLernField.PolicyId) select p).FirstOrDefault();
                    // List<PolicyLearnedField> _sPl = GetPolicyLearnedFieldsPolicyWise(this.PolicyId);
                    if (_policyLearnedField == null)
                    {
                        _policyLearnedField = new DLinq.PolicyLearnedFieldsHistory
                        {
                            // PolicyLearnedFieldId = (PolicyLernField.PolicyLearnedFieldId == null || PolicyLernField.PolicyLearnedFieldId==Guid.Empty) ? Guid.NewGuid() : PolicyLernField.PolicyLearnedFieldId,
                            Insured = PolicyLernField.Insured,
                            PolicyNumber = PolicyLernField.PolicyNumber,
                            Effective = PolicyLernField.Effective,
                            TrackFrom = PolicyLernField.TrackFrom,
                            Renewal = PolicyLernField.Renewal,
                            //CompSchedule = this.CompSchedule,
                            // CompType = this.CompType,
                            PAC = Convert.ToString(PolicyLernField.PAC),
                            PMC = Convert.ToString(PolicyLernField.PMC),
                            ModalAvgPremium = PolicyLernField.ModalAvgPremium,
                            PolicyModeId = PolicyLernField.PolicyModeId,//
                            Enrolled = PolicyLernField.Enrolled,
                            Eligible = PolicyLernField.Eligible,
                            AutoTerminationDate = PolicyLernField.AutoTerminationDate,
                            PayorSysID = PolicyLernField.PayorSysId,
                            Link1 = PolicyLernField.Link1,
                            SplitPercentage = PolicyLernField.Link2,
                            LastModifiedOn = PolicyLernField.LastModifiedOn,
                            LastModifiedUserCredentialid = PolicyLernField.LastModifiedUserCredentialId,//
                            CompScheduleType = PolicyLernField.CompScheduleType,
                            PreviousEffectiveDate = PolicyLernField.PreviousEffectiveDate,
                            PreviousPolicyModeId = PolicyLernField.PreviousPolicyModeid,
                            //03/03/2012
                            //Save carier nick Name
                            CarrierNickName = PolicyLernField.CarrierNickName,
                            //Save Coverage nick Name
                            CoverageNickName = PolicyLernField.CoverageNickName,
                            Advance = PolicyLernField.Advance,
                            ProductType = PolicyLernField.ProductType,
                            ImportPolicyID = PolicyLernField.ImportPolicyID,
                        };
                        _policyLearnedField.PolicyReference.Value = (from s in DataModel.Policies where s.PolicyId == PolicyLernField.PolicyId select s).FirstOrDefault();
                        _policyLearnedField.MasterPolicyModeReference.Value = (from s in DataModel.MasterPolicyModes where s.PolicyModeId == PolicyLernField.PolicyModeId select s).FirstOrDefault();
                        _policyLearnedField.PayorReference.Value = (from s in DataModel.Payors where s.PayorId == PolicyLernField.PayorId select s).FirstOrDefault();
                        _policyLearnedField.CarrierReference.Value = (from s in DataModel.Carriers where s.CarrierId == PolicyLernField.CarrierId select s).FirstOrDefault();
                        _policyLearnedField.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == PolicyLernField.CoverageId select s).FirstOrDefault();
                        _policyLearnedField.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == PolicyLernField.ClientID select s).FirstOrDefault();

                        _policyLearnedField.MasterIncomingPaymentTypeReference.Value = (from s in DataModel.MasterIncomingPaymentTypes where s.IncomingPaymentTypeId == PolicyLernField.CompTypeId select s).FirstOrDefault();
                        // _policyLearnedField.MasterScheduleTypeReference.Value = (from s in DataModel.MasterScheduleTypes where s.ScheduleTypeId == PolicyLernField.CompScheduleTypeId select s).FirstOrDefault();
                        DataModel.AddToPolicyLearnedFieldsHistories(_policyLearnedField);

                    }
                    else
                    {
                        //_policyLearnedField.PolicyLearnedFieldId = Guid.NewGuid();
                        //_policyLearnedField.ClientName = this.ClientName;
                        _policyLearnedField.Insured = PolicyLernField.Insured;
                        _policyLearnedField.PolicyNumber = PolicyLernField.PolicyNumber;
                        _policyLearnedField.Effective = PolicyLernField.Effective;
                        _policyLearnedField.TrackFrom = PolicyLernField.TrackFrom;
                        _policyLearnedField.Renewal = PolicyLernField.Renewal;
                        // _policyLearnedField.CompSchedule = this.CompSchedule;
                        // _policyLearnedField.CompType = this.CompType;
                        _policyLearnedField.PAC = Convert.ToString(PolicyLernField.PAC);
                        _policyLearnedField.PMC = Convert.ToString(PolicyLernField.PMC);
                        _policyLearnedField.ModalAvgPremium = PolicyLernField.ModalAvgPremium;
                        _policyLearnedField.PolicyModeId = PolicyLernField.PolicyModeId;
                        _policyLearnedField.Enrolled = PolicyLernField.Enrolled;
                        _policyLearnedField.Eligible = PolicyLernField.Eligible;
                        _policyLearnedField.AutoTerminationDate = PolicyLernField.AutoTerminationDate;
                        _policyLearnedField.PayorSysID = PolicyLernField.PayorSysId;
                        _policyLearnedField.Link1 = PolicyLernField.Link1;
                        _policyLearnedField.SplitPercentage = PolicyLernField.Link2;
                        _policyLearnedField.CompScheduleType = PolicyLernField.CompScheduleType;
                        _policyLearnedField.LastModifiedOn = PolicyLernField.LastModifiedOn;
                        _policyLearnedField.LastModifiedUserCredentialid = PolicyLernField.LastModifiedUserCredentialId;
                        _policyLearnedField.PolicyReference.Value = (from s in DataModel.Policies where s.PolicyId == PolicyLernField.PolicyId select s).FirstOrDefault();
                        _policyLearnedField.PayorReference.Value = (from s in DataModel.Payors where s.PayorId == PolicyLernField.PayorId select s).FirstOrDefault();
                        _policyLearnedField.CarrierReference.Value = (from s in DataModel.Carriers where s.CarrierId == PolicyLernField.CarrierId select s).FirstOrDefault();
                        _policyLearnedField.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == PolicyLernField.CoverageId select s).FirstOrDefault();
                        _policyLearnedField.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == PolicyLernField.ClientID select s).FirstOrDefault();
                        _policyLearnedField.MasterIncomingPaymentTypeReference.Value = (from s in DataModel.MasterIncomingPaymentTypes where s.IncomingPaymentTypeId == PolicyLernField.CompTypeId select s).FirstOrDefault();
                        //  _policyLearnedField.MasterScheduleTypeReference.Value = (from s in DataModel.MasterScheduleTypes where s.ScheduleTypeId == PolicyLernField.CompScheduleTypeId select s).FirstOrDefault();
                        _policyLearnedField.PreviousEffectiveDate = PolicyLernField.PreviousEffectiveDate;
                        _policyLearnedField.PreviousPolicyModeId = PolicyLernField.PreviousPolicyModeid;
                        //update carrier nick name
                        _policyLearnedField.CarrierNickName = PolicyLernField.CarrierNickName;
                        //update coverage nick name
                        _policyLearnedField.CoverageNickName = PolicyLernField.CoverageNickName;
                        _policyLearnedField.ProductType = PolicyLernField.ProductType;
                        _policyLearnedField.Advance = PolicyLernField.Advance;
                        _policyLearnedField.ImportPolicyID = PolicyLernField.ImportPolicyID;
                    }
                    DataModel.SaveChanges();
                    // LearnedToPolicyPost.AddUpdateLearnedToPolicy(GetPolicyLearnedFieldsPolicyWise(_policyLearnedField.PolicyId));
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdateHistoryLearned :" + ex.InnerException.ToString(), true);
            }
        }

        public static void AddUpdateHistoryLearnedNotCheckPayment(Guid PolicyId)
        {
            //List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(PolicyId);
            //if (_PolicyPaymentEntriesPost.Count != 0) return;
            PolicyLearnedFieldData PolicyLernField = GetPolicyLearnedFieldsPolicyWise(PolicyId);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                // var _policyLearnedField = (from p in DataModel.PolicyLearnedFields where (p.PolicyLearnedFieldId == PolicyLernField.PolicyLearnedFieldId) select p).FirstOrDefault();
                var _policyLearnedField = (from p in DataModel.PolicyLearnedFieldsHistories where (p.PolicyId == PolicyLernField.PolicyId) select p).FirstOrDefault();
                // List<PolicyLearnedField> _sPl = GetPolicyLearnedFieldsPolicyWise(this.PolicyId);
                if (_policyLearnedField == null)
                {
                    _policyLearnedField = new DLinq.PolicyLearnedFieldsHistory
                    {
                        // PolicyLearnedFieldId = (PolicyLernField.PolicyLearnedFieldId == null || PolicyLernField.PolicyLearnedFieldId==Guid.Empty) ? Guid.NewGuid() : PolicyLernField.PolicyLearnedFieldId,
                        Insured = PolicyLernField.Insured,
                        PolicyNumber = PolicyLernField.PolicyNumber,
                        Effective = PolicyLernField.Effective,
                        TrackFrom = PolicyLernField.TrackFrom,
                        Renewal = PolicyLernField.Renewal,
                        //   CompSchedule = this.CompSchedule,
                        // CompType = this.CompType,
                        PAC = Convert.ToString(PolicyLernField.PAC),
                        PMC = Convert.ToString(PolicyLernField.PMC),
                        ModalAvgPremium = PolicyLernField.ModalAvgPremium,
                        PolicyModeId = PolicyLernField.PolicyModeId,//
                        Enrolled = PolicyLernField.Enrolled,
                        Eligible = PolicyLernField.Eligible,
                        AutoTerminationDate = PolicyLernField.AutoTerminationDate,
                        PayorSysID = PolicyLernField.PayorSysId,
                        Link1 = PolicyLernField.Link1,
                        SplitPercentage = PolicyLernField.Link2,
                        LastModifiedOn = PolicyLernField.LastModifiedOn,
                        LastModifiedUserCredentialid = PolicyLernField.LastModifiedUserCredentialId,//
                        CompScheduleType = PolicyLernField.CompScheduleType,
                        PreviousEffectiveDate = PolicyLernField.PreviousEffectiveDate,
                        PreviousPolicyModeId = PolicyLernField.PreviousPolicyModeid,
                        ImportPolicyID = PolicyLernField.ImportPolicyID,
                    };
                    _policyLearnedField.PolicyReference.Value = (from s in DataModel.Policies where s.PolicyId == PolicyLernField.PolicyId select s).FirstOrDefault();
                    _policyLearnedField.MasterPolicyModeReference.Value = (from s in DataModel.MasterPolicyModes where s.PolicyModeId == PolicyLernField.PolicyModeId select s).FirstOrDefault();
                    _policyLearnedField.PayorReference.Value = (from s in DataModel.Payors where s.PayorId == PolicyLernField.PayorId select s).FirstOrDefault();
                    _policyLearnedField.CarrierReference.Value = (from s in DataModel.Carriers where s.CarrierId == PolicyLernField.CarrierId select s).FirstOrDefault();
                    _policyLearnedField.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == PolicyLernField.CoverageId select s).FirstOrDefault();
                    _policyLearnedField.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == PolicyLernField.ClientID select s).FirstOrDefault();

                    _policyLearnedField.MasterIncomingPaymentTypeReference.Value = (from s in DataModel.MasterIncomingPaymentTypes where s.IncomingPaymentTypeId == PolicyLernField.CompTypeId select s).FirstOrDefault();
                    // _policyLearnedField.MasterScheduleTypeReference.Value = (from s in DataModel.MasterScheduleTypes where s.ScheduleTypeId == PolicyLernField.CompScheduleTypeId select s).FirstOrDefault();
                    DataModel.AddToPolicyLearnedFieldsHistories(_policyLearnedField);

                }
                else
                {
                    // _policyLearnedField.PolicyLearnedFieldId = Guid.NewGuid();
                    // _policyLearnedField.ClientName = this.ClientName;
                    _policyLearnedField.Insured = PolicyLernField.Insured;
                    _policyLearnedField.PolicyNumber = PolicyLernField.PolicyNumber;
                    _policyLearnedField.Effective = PolicyLernField.Effective;
                    _policyLearnedField.TrackFrom = PolicyLernField.TrackFrom;
                    _policyLearnedField.Renewal = PolicyLernField.Renewal;
                    // _policyLearnedField.CompSchedule = this.CompSchedule;
                    // _policyLearnedField.CompType = this.CompType;
                    _policyLearnedField.PAC = Convert.ToString(PolicyLernField.PAC);
                    _policyLearnedField.PMC = Convert.ToString(PolicyLernField.PMC);
                    _policyLearnedField.ModalAvgPremium = PolicyLernField.ModalAvgPremium;
                    _policyLearnedField.PolicyModeId = PolicyLernField.PolicyModeId;
                    _policyLearnedField.Enrolled = PolicyLernField.Enrolled;
                    _policyLearnedField.Eligible = PolicyLernField.Eligible;
                    _policyLearnedField.AutoTerminationDate = PolicyLernField.AutoTerminationDate;
                    _policyLearnedField.PayorSysID = PolicyLernField.PayorSysId;
                    _policyLearnedField.Link1 = PolicyLernField.Link1;
                    _policyLearnedField.SplitPercentage = PolicyLernField.Link2;
                    _policyLearnedField.CompScheduleType = PolicyLernField.CompScheduleType;
                    _policyLearnedField.LastModifiedOn = PolicyLernField.LastModifiedOn;
                    _policyLearnedField.LastModifiedUserCredentialid = PolicyLernField.LastModifiedUserCredentialId;
                    _policyLearnedField.ImportPolicyID = PolicyLernField.ImportPolicyID;
                    _policyLearnedField.PolicyReference.Value = (from s in DataModel.Policies where s.PolicyId == PolicyLernField.PolicyId select s).FirstOrDefault();
                    _policyLearnedField.PayorReference.Value = (from s in DataModel.Payors where s.PayorId == PolicyLernField.PayorId select s).FirstOrDefault();
                    _policyLearnedField.CarrierReference.Value = (from s in DataModel.Carriers where s.CarrierId == PolicyLernField.CarrierId select s).FirstOrDefault();
                    _policyLearnedField.CoverageReference.Value = (from s in DataModel.Coverages where s.CoverageId == PolicyLernField.CoverageId select s).FirstOrDefault();
                    _policyLearnedField.ClientReference.Value = (from s in DataModel.Clients where s.ClientId == PolicyLernField.ClientID select s).FirstOrDefault();

                    _policyLearnedField.MasterIncomingPaymentTypeReference.Value = (from s in DataModel.MasterIncomingPaymentTypes where s.IncomingPaymentTypeId == PolicyLernField.CompTypeId select s).FirstOrDefault();
                    //  _policyLearnedField.MasterScheduleTypeReference.Value = (from s in DataModel.MasterScheduleTypes where s.ScheduleTypeId == PolicyLernField.CompScheduleTypeId select s).FirstOrDefault();
                    _policyLearnedField.PreviousEffectiveDate = PolicyLernField.PreviousEffectiveDate;
                    _policyLearnedField.PreviousPolicyModeId = PolicyLernField.PreviousPolicyModeid;
                }
                DataModel.SaveChanges();

            }
        }

        public static PolicyLearnedFieldData GetPolicyLearnedFieldsHistoryPolicyWise(Guid _PolicyID)
        {
            PolicyLearnedFieldData _poLearFiel = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                _poLearFiel = (from ss in DataModel.PolicyLearnedFieldsHistories
                               where (ss.PolicyId == _PolicyID)
                               select new PolicyLearnedFieldData
                               {
                                   // PolicyLearnedFieldId = ss.PolicyLearnedFieldId,
                                   PolicyId = ss.Policy.PolicyId,
                                   // ClientName=ss.ClientName,
                                   Insured = ss.Insured,
                                   PolicyNumber = ss.PolicyNumber,
                                   Effective = ss.Effective.Value,
                                   TrackFrom = ss.TrackFrom.Value,
                                   PAC = ss.PAC,
                                   PMC = ss.PMC,
                                   ModalAvgPremium = ss.ModalAvgPremium,
                                   Renewal = ss.Renewal,
                                   CarrierId = ss.Carrier.CarrierId,
                                   CoverageId = ss.Coverage.CoverageId,
                                   // CompScheduleTypeId = ss.MasterScheduleType.ScheduleTypeId,
                                   CompTypeId = ss.MasterIncomingPaymentType.IncomingPaymentTypeId,
                                   //  CompSchedule=ss.CompSchedule,
                                   //  CompType=ss.CompType,
                                   //Add to get carrier nick name
                                   CarrierNickName = ss.CarrierNickName,
                                   //Add to get covrage nick name
                                   CoverageNickName = ss.CoverageNickName,
                                   PolicyModeId = ss.MasterPolicyMode.PolicyModeId,
                                   Enrolled = ss.Enrolled,
                                   Eligible = ss.Eligible,
                                   AutoTerminationDate = ss.AutoTerminationDate,
                                   PayorSysId = ss.PayorSysID,
                                   Link1 = ss.Link1,
                                   Link2 = ss.SplitPercentage,
                                   LastModifiedOn = ss.LastModifiedOn,
                                   LastModifiedUserCredentialId = ss.LastModifiedUserCredentialid,
                                   ClientID = ss.Client.ClientId,
                                   CompScheduleType = ss.CompScheduleType,
                                   PayorId = ss.Payor.PayorId,
                                   PreviousEffectiveDate = ss.PreviousEffectiveDate,
                                   PreviousPolicyModeid = ss.PreviousPolicyModeId,
                                   ImportPolicyID = ss.ImportPolicyID,
                               }
                       ).ToList().FirstOrDefault();
                if (_poLearFiel == null) return _poLearFiel;
                if (_poLearFiel.PayorId == null || _poLearFiel.PayorId == Guid.Empty || _poLearFiel.CarrierId == null || _poLearFiel.CarrierId == Guid.Empty) return _poLearFiel;
                {
                    _poLearFiel.CarrierNickName = Carrier.GetCarrierNickName(_poLearFiel.PayorId ?? Guid.Empty, _poLearFiel.CarrierId ?? Guid.Empty);
                }

                if (_poLearFiel.PayorId == null || _poLearFiel.PayorId == Guid.Empty || _poLearFiel.CarrierId == null || _poLearFiel.CarrierId == Guid.Empty || _poLearFiel.CoverageId == null || _poLearFiel.CoverageId == Guid.Empty) return _poLearFiel;
                _poLearFiel.CoverageNickName = Coverage.GetCoverageNickName(_poLearFiel.PayorId ?? Guid.Empty, _poLearFiel.CarrierId ?? Guid.Empty, _poLearFiel.CoverageId ?? Guid.Empty);

            }
            _poLearFiel.PrevoiusTrackFromDate = _poLearFiel.TrackFrom;
            return _poLearFiel;

        }

        public static void DeleteLearnedHistory(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policylearnedrecord = (from p in DataModel.PolicyLearnedFieldsHistories where (p.PolicyId == PolicyId) select p).FirstOrDefault();
                if (_policylearnedrecord == null) return;
                DataModel.DeleteObject(_policylearnedrecord);
                DataModel.SaveChanges();
            }
        }

        #endregion

        PolicyLearnedFieldData IEditable<PolicyLearnedFieldData>.GetOfID()
        {
            return null;
        }
    }
}
