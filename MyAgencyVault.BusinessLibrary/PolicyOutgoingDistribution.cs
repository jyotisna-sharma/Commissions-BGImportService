using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Data;

namespace MyAgencyVault.BusinessLibrary
{
    public class PolicyOutgoingDistribution
    {
        [DataMember]
        public Guid OutgoingPaymentId { get; set; }
        [DataMember]

        public Guid? PaymentEntryId { get; set; }
        [DataMember]

        public Guid? RecipientUserCredentialId { get; set; }
        [DataMember]

        public double? PaidAmount { get; set; } //it is TotalDueToPayee
        [DataMember]

        public DateTime? CreatedOn { get; set; }
       // [DataMember]

       // public Guid? ReferencedOutgoingScheduleId { get; set; }
       // [DataMember]

      //  public Guid? ReferencedOutgoingAdvancedScheduleId { get; set; }

        [DataMember]
        public bool? IsPaid { get; set; }

      
        [DataMember]
        public double? Premium { get; set; }  //It is %of Premium
        [DataMember]
        public decimal? OutGoingPerUnit { get; set; } //it is OutgoingPerunit
      
        [DataMember]
        public double? Payment { get; set; }//It is % of commission

        //Acme added : for delete alert on outgoing payments in commission dashboard 
        public static bool CheckIsPaymentFromDEU(Guid OutgoingEntryID)
        {
            bool result = false;
            try
            {
                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                string adoConnStr = sc.ConnectionString;
                using (SqlConnection con = new SqlConnection(adoConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_IsPaymentFromDEU", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OutgoingEntryID", OutgoingEntryID);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        // Call Read before accessing data. 
                        while (reader.Read())
                        {
                            string strResult = Convert.ToString(reader["IsEntrybyCommissiondashBoard"]);
                            result = (strResult == "1");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("CheckIsPaymentFromDEU :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("CheckIsPaymentFromDEU :" + ex.InnerException.ToString(), true);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PaymentEntryId"></param>
        /// <returns></returns>
        public static bool IsEntryMarkPaid(Guid PaymentEntryId)
        {
            List<PolicyOutgoingDistribution>_PolicyOutgoingDistributionLst
                = GetOutgoingPaymentByPoicyPaymentEntryId(PaymentEntryId);
            if (_PolicyOutgoingDistributionLst == null || _PolicyOutgoingDistributionLst.Count == 0) return false;
            return _PolicyOutgoingDistributionLst.Count(p => p.IsPaid == true) == 
                _PolicyOutgoingDistributionLst.Count ? true : false;
        }


        public static bool IfPaymentHasPaidEntry(Guid PaymentEntryId)
        {
            ActionLogger.Logger.WriteImportPolicyLog("IfPaymentHasPaidEntry request: PaymentEntryID - " + PaymentEntryId, true);
            try
            {
                List<PolicyOutgoingDistribution> _PolicyOutgoingDistributionLst
                    = GetOutgoingPaymentByPoicyPaymentEntryId(PaymentEntryId);
                if (_PolicyOutgoingDistributionLst == null || _PolicyOutgoingDistributionLst.Count == 0) return false;

                int paidCount = _PolicyOutgoingDistributionLst.Count(p => p.IsPaid == true);
                ActionLogger.Logger.WriteImportPolicyLog("IfPaymentHasPaidEntry paid count: " + paidCount, true);
                return paidCount > 0;
            }
            catch(Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("IfPaymentHasPaidEntry exception: PaymentEntryID - " + PaymentEntryId + ", ex: " + ex.Message, true);
                return false;
            }
        }

        public static bool AddUpdateOutgoingPaymentEntry(PolicyOutgoingDistribution _PolicyOutgoingDistribution)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "AddUpdateOutgoingPaymentEntry request: " + _PolicyOutgoingDistribution.PaymentEntryId + ", PaidAmount: " + _PolicyOutgoingDistribution.PaidAmount + ", outgoingID: " + _PolicyOutgoingDistribution.OutgoingPaymentId, true);
            bool bValue = true;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    Guid _OutGoingPayment = Guid.NewGuid();
                    DLinq.PolicyOutgoingPayment _ObjPolicyOutgoingPayment = (from m in DataModel.PolicyOutgoingPayments where m.OutgoingPaymentId == _PolicyOutgoingDistribution.OutgoingPaymentId select m).FirstOrDefault();

                    if (_ObjPolicyOutgoingPayment == null)
                    {
                        _ObjPolicyOutgoingPayment = new DLinq.PolicyOutgoingPayment
                        {
                            OutgoingPaymentId = _PolicyOutgoingDistribution.OutgoingPaymentId,
                            PaidAmount = _PolicyOutgoingDistribution.PaidAmount,
                            CreatedOn = _PolicyOutgoingDistribution.CreatedOn,
                            IsPaid = _PolicyOutgoingDistribution.IsPaid,
                            Premium = _PolicyOutgoingDistribution.Premium,
                            OutgoingPerUnit = _PolicyOutgoingDistribution.OutGoingPerUnit,
                            Payment = _PolicyOutgoingDistribution.Payment, 
                           // ReverseOutgoingPaymentID = _PolicyOutgoingDistribution.ReverseOutgoingPaymentId
                          //  ReverseOutgoingPaymentID  = _PolicyOutgoingDistribution.ReverseOutgoingPaymentId

                        };


                        _ObjPolicyOutgoingPayment.RecipientUserCredentialId = _PolicyOutgoingDistribution.RecipientUserCredentialId;
                        _ObjPolicyOutgoingPayment.PolicyPaymentEntryReference.Value = (from f in DataModel.PolicyPaymentEntries where f.PaymentEntryId == _PolicyOutgoingDistribution.PaymentEntryId select f).FirstOrDefault();
                        // _ObjPolicyOutgoingPayment.ReferencedOutgoingScheduleId= _PolicyOutgoingDistribution.ReferencedOutgoingScheduleId ;
                        // _ObjPolicyOutgoingPayment.ReferencedOutgoingAdvancedScheduleId =  _PolicyOutgoingDistribution.ReferencedOutgoingAdvancedScheduleId ;

                        DataModel.AddToPolicyOutgoingPayments(_ObjPolicyOutgoingPayment);
                        ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "AddUpdateOutgoingPaymentEntry new entry success ",true);
                    }
                    else
                    {
                        _ObjPolicyOutgoingPayment.OutgoingPaymentId = _PolicyOutgoingDistribution.OutgoingPaymentId;
                        _ObjPolicyOutgoingPayment.PaidAmount = _PolicyOutgoingDistribution.PaidAmount;
                        _ObjPolicyOutgoingPayment.CreatedOn = _PolicyOutgoingDistribution.CreatedOn;
                        _ObjPolicyOutgoingPayment.IsPaid = _PolicyOutgoingDistribution.IsPaid;
                        _ObjPolicyOutgoingPayment.Premium = _PolicyOutgoingDistribution.Premium;
                        _ObjPolicyOutgoingPayment.OutgoingPerUnit = _PolicyOutgoingDistribution.OutGoingPerUnit;
                        _ObjPolicyOutgoingPayment.Payment = _PolicyOutgoingDistribution.Payment;
                        _ObjPolicyOutgoingPayment.RecipientUserCredentialId = _PolicyOutgoingDistribution.RecipientUserCredentialId;
                        _ObjPolicyOutgoingPayment.PolicyPaymentEntryReference.Value = (from f in DataModel.PolicyPaymentEntries where f.PaymentEntryId == _PolicyOutgoingDistribution.PaymentEntryId select f).FirstOrDefault();
                        // _ObjPolicyOutgoingPayment.ReferencedOutgoingScheduleId = _PolicyOutgoingDistribution.ReferencedOutgoingScheduleId;
                        // _ObjPolicyOutgoingPayment.ReferencedOutgoingAdvancedScheduleId = _PolicyOutgoingDistribution.ReferencedOutgoingAdvancedScheduleId;
                        ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "AddUpdateOutgoingPaymentEntry update success ", true);
                    }
                    DataModel.SaveChanges();
                    bValue = true;
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "AddUpdateOutgoingPaymentEntry completed  ", true);
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "AddUpdateOutgoingPaymentEntry exception: " + ex.Message, true);
                bValue = false;
                
            }

            return bValue;

        }

        public static PolicyOutgoingDistribution GetOutgoingPaymentById(Guid OutgoingPaymentId)
        {
            PolicyOutgoingDistribution _PolicyOutgoingDistribution = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                _PolicyOutgoingDistribution = (from f in DataModel.PolicyOutgoingPayments
                                               where (f.OutgoingPaymentId == OutgoingPaymentId)
                                               select new PolicyOutgoingDistribution
                                               {
                                                   OutgoingPaymentId = f.OutgoingPaymentId,
                                                   PaymentEntryId = f.PaymentEntryId,
                                                   RecipientUserCredentialId = f.RecipientUserCredentialId,
                                                   PaidAmount = f.PaidAmount,
                                                   CreatedOn = f.CreatedOn,
                                                  // ReferencedOutgoingScheduleId = f.ReferencedOutgoingScheduleId,
                                                  // ReferencedOutgoingAdvancedScheduleId = f.ReferencedOutgoingAdvancedScheduleId,
                                                   IsPaid = f.IsPaid,
                                                   //18-Apr-2011
                                                   Premium = f.Premium??0,
                                                   OutGoingPerUnit = f.OutgoingPerUnit??0,
                                                   Payment = f.Payment??0,
                                                   //ReverseOutgoingPaymentId  = f.ReverseOutgoingPaymentID,
                                                   
                                                   
                                               }
                                                ).FirstOrDefault();
            }
            return _PolicyOutgoingDistribution;

        }

        public static List<PolicyOutgoingDistribution> GetOutgoingPaymentByPoicyPaymentEntryId(Guid EntryId,Guid recipientCredentials)
        {
          List<PolicyOutgoingDistribution> _PolicyOutgoingDistribution = null;
          using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
          {
            _PolicyOutgoingDistribution = (from f in DataModel.PolicyOutgoingPayments
                                           where (f.PaymentEntryId == EntryId && f.RecipientUserCredentialId== recipientCredentials)
                                           select new PolicyOutgoingDistribution
                                           {
                                             OutgoingPaymentId = f.OutgoingPaymentId,
                                             PaymentEntryId = f.PaymentEntryId,
                                             RecipientUserCredentialId = f.RecipientUserCredentialId,
                                             PaidAmount = f.PaidAmount,
                                             CreatedOn = f.CreatedOn,
                                             IsPaid = f.IsPaid,
                                             Premium = f.Premium ?? 0,
                                             OutGoingPerUnit = f.OutgoingPerUnit ?? 0,
                                             Payment = f.Payment ?? 0,
                                          //   ReverseOutgoingPaymentId =f.ReverseOutgoingPaymentID,
                                           }
                                            ).ToList();
          }
          return _PolicyOutgoingDistribution;
        }

        //public static List<PolicyOutgoingDistribution> GetOutgoingPaymentByPoicyPaymentEntryId(Guid EntryId)
        //{
        //    List<PolicyOutgoingDistribution> _PolicyOutgoingDistribution = null;
        //    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //    {
        //        _PolicyOutgoingDistribution = (from f in DataModel.PolicyOutgoingPayments
        //                                       where (f.PaymentEntryId == EntryId)
        //                                       select new PolicyOutgoingDistribution
        //                                       {
        //                                           OutgoingPaymentId = f.OutgoingPaymentId,
        //                                           PaymentEntryId = f.PaymentEntryId,
        //                                           RecipientUserCredentialId = f.RecipientUserCredentialId,
        //                                           PaidAmount = f.PaidAmount,
        //                                           CreatedOn = f.CreatedOn,
        //                                           IsPaid = f.IsPaid,
        //                                           Premium = f.Premium??0,
        //                                           OutGoingPerUnit = f.OutgoingPerUnit??0,
        //                                           Payment = f.Payment??0,
        //                                       }
        //                                        ).ToList();
        //    }
        //    return _PolicyOutgoingDistribution;
        //}

        public static List<PolicyOutgoingDistribution> GetOutgoingPaymentByPoicyPaymentEntryId(Guid EntryId)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetOutgoingPaymentByPoicyPaymentEntryId:Outgoing Payment request with PaymentEntryId: " + EntryId, true);
            List<PolicyOutgoingDistribution> _PolicyOutgoingDistribution = new List<PolicyOutgoingDistribution>();
            DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
            EntityConnection ec = (EntityConnection)ctx.Connection;
            SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
            string adoConnStr = sc.ConnectionString;
            using (SqlConnection con = new SqlConnection(adoConnStr))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_GetOutgoingPayment", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PaymentEntryId", EntryId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    // Call Read before accessing data. 
                    while (reader.Read())
                    {
                        try
                        {
                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetOutgoingPaymentByPoicyPaymentEntryId:Reading field values with PaymentEntryId: " + EntryId, true);
                            PolicyOutgoingDistribution objPolicyDetailsData = new PolicyOutgoingDistribution();
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["OutgoingPaymentId"])))
                            {
                                objPolicyDetailsData.OutgoingPaymentId = reader["OutgoingPaymentId"] == null ? Guid.Empty : (Guid)reader["OutgoingPaymentId"];
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["PaymentEntryId"])))
                            {
                                objPolicyDetailsData.PaymentEntryId = reader["PaymentEntryId"] == null ? Guid.Empty : (Guid)reader["PaymentEntryId"];
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["RecipientUserCredentialId"])))
                            {
                                objPolicyDetailsData.RecipientUserCredentialId = reader["RecipientUserCredentialId"] == null ? Guid.Empty : (Guid)reader["RecipientUserCredentialId"];
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["PaidAmount"])))
                            {
                                objPolicyDetailsData.PaidAmount = reader["PaidAmount"] == null ? 0 : Convert.ToDouble(reader["PaidAmount"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["CreatedOn"])))
                            {
                                objPolicyDetailsData.CreatedOn = Convert.ToDateTime(reader["CreatedOn"]);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(reader["IsPaid"])))
                            {
                                objPolicyDetailsData.IsPaid = (bool)reader["IsPaid"];
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["Premium"])))
                            {
                                objPolicyDetailsData.Premium = Convert.ToDouble(reader["Premium"]);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(reader["OutGoingPerUnit"])))
                            {
                                objPolicyDetailsData.OutGoingPerUnit = Convert.ToDecimal(reader["OutGoingPerUnit"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["Payment"])))
                            {
                                objPolicyDetailsData.Payment = Convert.ToDouble(reader["Payment"]);
                            }
                            _PolicyOutgoingDistribution.Add(objPolicyDetailsData);
                        }
                        catch(Exception ex)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetOutgoingPaymentByPoicyPaymentEntryId:Exception occur while fetching list of OutgoingPayments with PaymentEntryId: " + EntryId + " " + "Exception:" + " " + ex.Message, true);
                        }

                    }
                    // Call Close when done reading.
                    reader.Close();
                }
            }
            return _PolicyOutgoingDistribution;
        }
     
        public static void DeleteByPolicyIncomingPaymentId(Guid PaymentEntryId)
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "Outgoing Payment delete request with PaymentEntryId: " + PaymentEntryId, true);
                List<PolicyOutgoingDistribution> _PolicyOutgoingDistribution = GetOutgoingPaymentByPoicyPaymentEntryId(PaymentEntryId);
                foreach (PolicyOutgoingDistribution _po in _PolicyOutgoingDistribution)
                {
                    DeleteById(_po.OutgoingPaymentId);
                }
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "Outgoing Payments deleted with PaymentEntryId: " + PaymentEntryId, true);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeleteByPolicyIncomingPaymentId :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("DeleteByPolicyIncomingPaymentId :" + ex.InnerException.ToString(), true);
                //throw ex;
            }

        }

        /// <summary>
        /// Acme - get content for email triggered on manual deletion of outgoing payment 
        /// </summary>
        /// <param name="OutgoingID"></param>
        /// <returns></returns>
        public static string GetOutgoingDeleteEmailContent(Guid OutgoingID)
        {
            string body = string.Empty;
            try
            {
                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                string adoConnStr = sc.ConnectionString;
                using (SqlConnection con = new SqlConnection(adoConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GetOutgoingPaymentDate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OutgoingID", OutgoingID);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        // Call Read before accessing data. 
                        body = "An outgoing payment has been deleted from the system with outgoingID : " + OutgoingID;
                        while (reader.Read())
                        {
                            body += "\nPolicyNumber: " + Convert.ToString(reader["PolicyNumber"]);
                            body += "\nClient: " + Convert.ToString(reader["Name"]);
                            body += "\n: Agency" + Convert.ToString(reader["Company"]);
                            body += "\n: BatchNumber" + Convert.ToString(reader["BatchNumber"]);
                            body += "\n: StatementNumber" + Convert.ToString(reader["StatementNumber"]);
                            body += "\n: DEUEntryId" + Convert.ToString(reader["DEUEntryId"]);
                      //      body += "\n: OutgoingPaymentId" + Convert.ToString(reader["OutgoingPaymentId"]);
                            body += "\n: StatementId" + Convert.ToString(reader["StatementId"]);
                            body += "\n: PolicyID" + Convert.ToString(reader["policyID"]);
                            body += "\n: PaidAmount" + Convert.ToString(reader["PaidAmount"]);
                            body += "\n: IsPaid" + Convert.ToString(reader["IsPaid"]);
                            body += "\n: Agent" + Convert.ToString(reader["Agent"]);
                        }
                    }
                
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetOutgoingDeleteEmailContent :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("GetOutgoingDeleteEmailContent :" + ex.InnerException.ToString(), true);
            }
            return body;
        }


        public static void DeleteById(Guid OutgoingPaymentid)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "Outgoing Payment Delete request for Outgoing ID: " + OutgoingPaymentid, true);
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _OutgoingDes = (from f in DataModel.PolicyOutgoingPayments where f.OutgoingPaymentId == OutgoingPaymentid select f).FirstOrDefault();
                    if (_OutgoingDes != null)
                    {
                       // ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "Outgoing Payment from payment entry: " + _OutgoingDes.PaymentEntryId, true);
                        //Acme : added check to delete 2 entries when reverse payment deleted 
                        //if (_OutgoingDes.ReverseOutgoingPaymentID != null && _OutgoingDes.ReverseOutgoingPaymentID != Guid.Empty)
                        //{
                        //    Guid reversID = (Guid)_OutgoingDes.ReverseOutgoingPaymentID;
                        //    DataModel.DeleteObject(_OutgoingDes);
                        //    DataModel.SaveChanges();
                        //    //Delete the second entry too 
                        //    _OutgoingDes = (from f in DataModel.PolicyOutgoingPayments where f.ReverseOutgoingPaymentID == reversID select f).FirstOrDefault();
                        //    if (_OutgoingDes != null)
                        //    {
                        //        DataModel.DeleteObject(_OutgoingDes);
                        //        DataModel.SaveChanges();
                        //    }
                        //}
                        //else
                        {
                            //Acme - mail sent on deletion of payment 

                            //string body = "An outgoing payment has been deleted from the system with outgoingID : " + OutgoingPaymentid;
                            //if (_OutgoingDes.PolicyPaymentEntry != null && _OutgoingDes.PolicyPaymentEntry.PolicyId != null)
                            //{
                            //    body += "\n PolicyID: " + _OutgoingDes.PolicyPaymentEntry.PolicyId;
                            //}
                            //body += "\nPaymentEntryID: " + _OutgoingDes.PaymentEntryId;
                            //body += "\nRecipientUserCredentialId: " + _OutgoingDes.RecipientUserCredentialId;
                            //body += "\n: PaidAmount" + _OutgoingDes.PaidAmount;

                            DataModel.DeleteObject(_OutgoingDes);
                            DataModel.SaveChanges();
                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "  Outgoing Payment Deleted with Outgoing ID: " + OutgoingPaymentid, true);
                           
                            //Acme -sending mail on outgoing delete 
                           // MailServerDetail.sendMail("deudev@acmeminds.com", "Commissions Alert: Outgoing payment deleted from the system", body);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeleteById :" + ex.StackTrace.ToString(), true);
                ActionLogger.Logger.WriteImportPolicyLog("DeleteById :" + ex.InnerException.ToString(), true);
            }
        }
    }
}
