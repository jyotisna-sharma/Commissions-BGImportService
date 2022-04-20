using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Data;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class IncomingPament : IEditable<IncomingPament>
    {
        #region IEditable<IncomingPament> Members

        public void AddUpdate()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public IncomingPament GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// unlink the payment entry from the policy, to which currently it is associated to.
        /// </summary>
        /// <returns>return true on successfull attempts, else false</returns>
        public bool UnlinkIncomingPayments()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// link this payment entry to and existing policy.
        /// </summary>
        /// <returns>return true on successfull attempts, else false</returns>
        public bool LinkIncomingPaymentsToAnExistingPolicy(Guid policyId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// make the linked policy to be active. 
        /// </summary>
        /// <returns>return true on successfull attempts, else false</returns>
        public bool ActivateNewPolicy()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// developer need to recheck and think of the requirement of this funciton.
        /// </summary>
        /// <param name="policyID"></param>
        /// <returns>returns all the incoming payments related to a policyid given in the parameter.</returns>
        public static List<IncomingPament> GetIncomingPayments(Guid policyID)
        {
            throw new NotImplementedException();
        }

        public static void UpdateInvoiceDate(PolicyPaymentEntriesPost paymentEntry)
        {
            ActionLogger.Logger.WriteImportPolicyLog("UpdateInvoiceDate:paymentEntryID - " + paymentEntry.ToStringDump(), true);
            try
            {
                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                string adoConnStr = sc.ConnectionString;

                using (SqlConnection con = new SqlConnection(adoConnStr))
                {
                    ActionLogger.Logger.WriteImportPolicyLog("UpdateInvoiceDate:connected - " + adoConnStr, true);
                    using (SqlCommand cmd = new SqlCommand("Usp_UpdateInvoiceDate", con))
                    {
                     //   ActionLogger.Logger.WriteImportPolicyLog("UpdateInvoiceDate:exec1 - ", true);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PaymentEntryID", paymentEntry.PaymentEntryID);
                        cmd.Parameters.AddWithValue("@newInvoice", Convert.ToDateTime(paymentEntry.InvoiceDate));
                        cmd.Parameters.AddWithValue("@CommissionPercentage", paymentEntry.CommissionPercentage);
                        cmd.Parameters.AddWithValue("@NumberOfUnits", paymentEntry.NumberOfUnits);
                        cmd.Parameters.AddWithValue("@PaymentRecived", paymentEntry.PaymentRecived);
                        cmd.Parameters.AddWithValue("@DollerPerUnit", paymentEntry.DollerPerUnit);
                        con.Open();
                        cmd.ExecuteNonQuery();

                        ActionLogger.Logger.WriteImportPolicyLog("UpdateInvoiceDate:success", true);
                    }
                }
                /*
                Guid policy = Guid.Empty;
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {


                    DLinq.PolicyPaymentEntry _record = (from p in DataModel.PolicyPaymentEntries
                                   where (p.PaymentEntryId == paymentEntryID )
                                   select p).FirstOrDefault();
                
                    if (_record != null)
                    {
                        _record.InvoiceDate = newInvoiceDate; 
                        DataModel.SaveChanges();
                        ActionLogger.Logger.WriteImportPolicyLog(" Success saving invoice date in payment entries: " + _record.InvoiceDate, true);

                        DLinq.EntriesByDEU _deuRecord = (from p in DataModel.EntriesByDEUs
                                          where (p.DEUEntryID == _record.DEUEntryId)
                                          select p).FirstOrDefault();
                        if (_deuRecord != null)
                        {
                            _deuRecord.InvoiceDate = newInvoiceDate;
                            DataModel.SaveChanges();
                            ActionLogger.Logger.WriteImportPolicyLog(" Success saving invoice date in deu table: " + _deuRecord.InvoiceDate, true);
                        }
                    }
                    
                }*/
            }
            catch(Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception saving invoice date: " + ex.Message, true);
      
            }

        }
    }
}
