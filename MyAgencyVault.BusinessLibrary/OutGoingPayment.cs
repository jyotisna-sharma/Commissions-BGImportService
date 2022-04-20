using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Data.SqlClient;

namespace MyAgencyVault.BusinessLibrary
{
    public class OutGoingPayment
    {
        #region "Data members aka - public properties"
        [DataMember]
        public Guid OutgoingScheduleId { get; set; }
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public string Payor { get; set; }
        [DataMember]
        public Guid PayeeUserCredentialId { get; set; }
        [DataMember]
        public double? FirstYearPercentage { get; set; }
        [DataMember]
        public double? RenewalPercentage { get; set; }
        [DataMember]
        public bool IsPrimaryAgent { get; set; }

        [DataMember]
        public bool IsEditDisable { get; set; }


        [DataMember]
        public int ScheduleTypeId { get; set; }
        [DataMember]
        public DateTime? CreatedOn { get; set; }

        //Acme- new parameters for  custom split feature 
        [DataMember]
        public DateTime? CustomStartDate { get; set; }
        [DataMember]
        public DateTime? CustomEndDate { get; set; }
        //[DataMember]
        //public string CustomDateType { get; set; }
        [DataMember]
        public double? SplitPercent { get; set; }
        [DataMember]
        public int? TierNumber { get; set; }


        #endregion

        #region IEditable<OutgoingPayment> Members
        public void AddUpdate()
        {
            throw new NotImplementedException();
        }

        public static void AddUpdate(List<OutGoingPayment> GlobalOutPayment, string agencyName = "", bool IsCustomSchedule = false, bool IsTieredSchedule = false)
        {
            ActionLogger.Logger.WriteImportPolicyLog("AddUpdate outgoing schedule request: " + GlobalOutPayment.ToStringDump(), true, agencyName);

            if (GlobalOutPayment != null && GlobalOutPayment.Count >= 1)
            {
                Guid policyId = GlobalOutPayment[0].PolicyId;
                DeleteOutgoingSchedules(policyId);
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {

                    DLinq.Policy policyData = (from e in DataModel.Policies
                                               where e.PolicyId == policyId
                                               select e).FirstOrDefault();
                    IsCustomSchedule = policyData.IsCustomBasicSchedule != null ? (bool)policyData.IsCustomBasicSchedule : false;
                    DLinq.PolicyOutgoingSchedule gcs = null;

                    foreach (OutGoingPayment _globalCoveragesShedule in GlobalOutPayment)
                    {
                        try
                        {
                            gcs = (from e in DataModel.PolicyOutgoingSchedules
                                   where e.OutgoingScheduleId == _globalCoveragesShedule.OutgoingScheduleId
                                   select e).FirstOrDefault();
                            if (gcs == null)
                            {
                                gcs = new DLinq.PolicyOutgoingSchedule
                                {
                                    OutgoingScheduleId = _globalCoveragesShedule.OutgoingScheduleId,
                                    FirstYearPercentage = (IsCustomSchedule) ? 0 : _globalCoveragesShedule.FirstYearPercentage,
                                    RenewalPercentage = (IsCustomSchedule) ? 0 : _globalCoveragesShedule.RenewalPercentage,
                                    //      IsPrimaryAgent = _globalCoveragesShedule.IsPrimaryAgent,
                                    ScheduleTypeId = _globalCoveragesShedule.ScheduleTypeId,
                                    CreatedOn = DateTime.Today,
                                    CustomStartDate = (IsCustomSchedule) ? _globalCoveragesShedule.CustomStartDate : null,
                                    CustomEndDate = (IsCustomSchedule) ? (_globalCoveragesShedule.CustomEndDate == DateTime.MinValue ? null : _globalCoveragesShedule.CustomEndDate) : null,
                                    SplitPercent = (IsCustomSchedule) ? _globalCoveragesShedule.SplitPercent : 0,
                                    TierNumber = (IsTieredSchedule) ? _globalCoveragesShedule.TierNumber : null,

                                };

                                gcs.PolicyReference.Value = (from f in DataModel.Policies where f.PolicyId == _globalCoveragesShedule.PolicyId select f).FirstOrDefault();
                                //gcs.UserDetailReference.Value = _objPayee;
                                gcs.UserCredentialReference.Value = (from f in DataModel.UserCredentials where f.UserCredentialId == _globalCoveragesShedule.PayeeUserCredentialId select f).FirstOrDefault();
                                DataModel.AddToPolicyOutgoingSchedules(gcs);

                            }
                            else
                            {
                                gcs.FirstYearPercentage = (IsCustomSchedule) ? 0 : _globalCoveragesShedule.FirstYearPercentage;
                                gcs.RenewalPercentage = (IsCustomSchedule) ? 0 : _globalCoveragesShedule.RenewalPercentage;
                                // gcs.IsPrimaryAgent = _globalCoveragesShedule.IsPrimaryAgent;
                                gcs.ScheduleTypeId = _globalCoveragesShedule.ScheduleTypeId;
                                gcs.CustomStartDate = (IsCustomSchedule) ? _globalCoveragesShedule.CustomStartDate : null;
                                gcs.CustomEndDate = (IsCustomSchedule) ? _globalCoveragesShedule.CustomEndDate : null;
                                gcs.SplitPercent = (IsCustomSchedule) ? _globalCoveragesShedule.SplitPercent : 0;
                                gcs.TierNumber = (IsTieredSchedule) ? _globalCoveragesShedule.TierNumber : null;
                            }
                            DataModel.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("AddUpdate outgoing schedule exception: " + ex.Message, true, agencyName);
                        }
                    }

                }
            }

        }
        public static void DeleteOutgoingSchedules(Guid policyId)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "DeleteOutgoingSchedules: Delete outgoing schedule with PolicyId-" + policyId, true);
            using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("usp_DeleteOutgoingSchedule", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PolicyId", policyId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "DeleteOutgoingSchedules: Delete outgoing schedule with PolicyId-" + policyId + "Exception:" + ex.Message, true);
                }
            }
        }

        public void Delete()
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete schedule started in outgoingPayment.cs - OutgoingScheduleId: " + this.OutgoingScheduleId, true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PolicyOutgoingSchedule _policyOutgoingSchedule = (from n in DataModel.PolicyOutgoingSchedules
                                                                        where (n.OutgoingScheduleId == this.OutgoingScheduleId)
                                                                        select n).FirstOrDefault();

                if (_policyOutgoingSchedule != null)
                {
                    DataModel.DeleteObject(_policyOutgoingSchedule);
                    DataModel.SaveChanges();
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete schedule success - OutgoingScheduleId: " + this.OutgoingScheduleId, true);
                }
                else
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete schedule failure 404 - OutgoingScheduleId: " + this.OutgoingScheduleId, true);
                }
            }
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete schedule ended in outgoingPayment.cs - OutgoingScheduleId: " + this.OutgoingScheduleId, true);
        }

        #endregion

        public static List<OutGoingPayment> GetOutgoingShedule()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from gc in DataModel.PolicyOutgoingSchedules
                        select new OutGoingPayment
                        {
                            RenewalPercentage = gc.RenewalPercentage,
                            FirstYearPercentage = gc.FirstYearPercentage,
                            //   IsPrimaryAgent = gc.IsPrimaryAgent,
                            OutgoingScheduleId = gc.OutgoingScheduleId,
                            PolicyId = gc.Policy.PolicyId,
                            PayeeUserCredentialId = gc.UserCredential.UserCredentialId,
                            ScheduleTypeId = gc.ScheduleTypeId.Value,
                            CreatedOn = gc.CreatedOn,
                            Payor = gc.UserCredential.UserDetail.NickName,
                            TierNumber = gc.TierNumber,
                        }).ToList();
            }
        }

        /// <summary>
        /// Gets schedule based on invoice/entered date
        ///Fetch only those schedules where invoice/entered date falls in one of the date ranges 
        /// </summary>
        /// 
        /// <param name="PolicyId"></param>
        /// <param name="DateType"></param>
        /// <param name="Invoice"></param>
        /// <param name="Entered"></param>
        /// <returns></returns>
        public static List<OutGoingPayment> GetCustomScheduleForPolicy(Guid PolicyId, string DateType, DateTime? Invoice, DateTime? Entered)
        {
            ActionLogger.Logger.WriteImportPolicyLog("GetCustomScheduleForPolicy: start processing,PolicyId: " + PolicyId + " returning :", true);
            if (!string.IsNullOrEmpty(DateType))
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("GetCustomScheduleForPolicy: DateType is null,PolicyId: " + PolicyId + " returning :" + "DateType:" + DateType, true);
                    if (DateType.ToLower() == "invoice")
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("GetCustomScheduleForPolicy: invoice ,PolicyId: " + PolicyId + " returning :", true);
                        return (from gc in DataModel.PolicyOutgoingSchedules
                                where (gc.PolicyId == PolicyId &&
                                           ((Invoice >= gc.CustomStartDate && Invoice <= gc.CustomEndDate) ||
                                             (Invoice >= gc.CustomStartDate && gc.CustomEndDate == null)
                                            )
                                  )
                                select new OutGoingPayment
                                {
                                    RenewalPercentage = gc.RenewalPercentage,
                                    FirstYearPercentage = gc.FirstYearPercentage,
                                    //    IsPrimaryAgent = gc.IsPrimaryAgent,
                                    OutgoingScheduleId = gc.OutgoingScheduleId,
                                    PolicyId = gc.Policy.PolicyId,
                                    PayeeUserCredentialId = gc.UserCredential.UserCredentialId,
                                    ScheduleTypeId = gc.ScheduleTypeId.Value,
                                    CreatedOn = gc.CreatedOn,
                                    Payor = gc.UserCredential.UserDetail.NickName,
                                    CustomStartDate = gc.CustomStartDate,
                                    CustomEndDate = gc.CustomEndDate,
                                    SplitPercent = (gc.SplitPercent == null) ? 0 : (double)(gc.SplitPercent),
                                    TierNumber = gc.TierNumber
                                }).ToList();
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("GetCustomScheduleForPolicy:  no datetype is invoice ,PolicyId: " + PolicyId + " returning :", true);
                        return (from gc in DataModel.PolicyOutgoingSchedules
                                where (gc.PolicyId == PolicyId &&
                                           ((Entered >= gc.CustomStartDate && Entered <= gc.CustomEndDate) ||
                                             (Entered >= gc.CustomStartDate && gc.CustomEndDate == null)
                                            ))
                                select new OutGoingPayment
                                {
                                    RenewalPercentage = gc.RenewalPercentage,
                                    FirstYearPercentage = gc.FirstYearPercentage,
                                    // IsPrimaryAgent = gc.IsPrimaryAgent,
                                    OutgoingScheduleId = gc.OutgoingScheduleId,
                                    PolicyId = gc.Policy.PolicyId,
                                    PayeeUserCredentialId = gc.UserCredential.UserCredentialId,
                                    ScheduleTypeId = gc.ScheduleTypeId.Value,
                                    CreatedOn = gc.CreatedOn,
                                    Payor = gc.UserCredential.UserDetail.NickName,
                                    CustomStartDate = gc.CustomStartDate,
                                    CustomEndDate = gc.CustomEndDate,
                                    SplitPercent = (gc.SplitPercent == null) ? 0 : (double)(gc.SplitPercent),
                                    TierNumber = gc.TierNumber
                                }).ToList();

                    }



                }
            }
            else
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetCustomScheduleForPolicy: DateType is null,PolicyId: " + PolicyId + " returning :", true);
                return null;
            }

        }

        /// <summary>
        /// Acme - changes to include custom split 
        /// </summary>
        /// <param name="PolicyId"></param>
        /// <returns></returns>
        public static List<OutGoingPayment> GetOutgoingSheduleForPolicy(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from gc in DataModel.PolicyOutgoingSchedules
                        where (gc.PolicyId == PolicyId)
                        select new OutGoingPayment
                        {
                            RenewalPercentage = gc.RenewalPercentage,
                            FirstYearPercentage = gc.FirstYearPercentage,
                            //    IsPrimaryAgent = gc.IsPrimaryAgent,
                            OutgoingScheduleId = gc.OutgoingScheduleId,
                            PolicyId = gc.Policy.PolicyId,
                            PayeeUserCredentialId = gc.UserCredential.UserCredentialId,
                            ScheduleTypeId = gc.ScheduleTypeId.Value,
                            CreatedOn = gc.CreatedOn,
                            Payor = gc.UserCredential.UserDetail.NickName,
                            CustomStartDate = gc.CustomStartDate,
                            CustomEndDate = gc.CustomEndDate,
                            SplitPercent = (gc.SplitPercent == null) ? 0 : (double)(gc.SplitPercent),
                            TierNumber = gc.TierNumber
                        }).ToList();
            }
        }

        public void DeleteSchedule(List<OutGoingPayment> DeleteOutPayment)
        {

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PolicyOutgoingSchedule gcs = null;
                foreach (OutGoingPayment _out in DeleteOutPayment)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeleteSchedule started in outgoingPayment.cs - OutgoingScheduleId: " + _out.OutgoingScheduleId, true);
                    gcs = (from e in DataModel.PolicyOutgoingSchedules
                           where e.OutgoingScheduleId == _out.OutgoingScheduleId
                           select e).FirstOrDefault();
                    if (gcs != null)
                    {
                        DataModel.DeleteObject(gcs);
                        DataModel.SaveChanges();
                    }
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeleteSchedule ended in outgoingPayment.cs - OutgoingScheduleId: " + _out.OutgoingScheduleId, true);
                }

            }
        }

        public static void DeletePolicyOutGoingSchedulebyPolicyId(Guid PolicyId, string agencyName = "")
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeletePolicyOutGoingSchedulebyPolicyId started in outgoingPayment.cs - PolicyId: " + PolicyId, true, agencyName);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.PolicyOutgoingSchedule> gcs = DataModel.PolicyOutgoingSchedules.Where(p => p.PolicyId == PolicyId).ToList<DLinq.PolicyOutgoingSchedule>();
                foreach (DLinq.PolicyOutgoingSchedule _out in gcs)
                {

                    DataModel.DeleteObject(_out);
                    DataModel.SaveChanges();
                }
            }
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeletePolicyOutGoingSchedulebyPolicyId ended in outgoingPayment.cs - PolicyId: " + PolicyId, true, agencyName);
        }

        public static bool IsUserPresentAsPayee(Guid userId)
        {

            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " IsUserPresentAsPayee userId: " + userId, true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                int count = DataModel.PolicyOutgoingSchedules.Where(s => s.PayeeUserCredentialId == userId).Count();
                if (count != 0)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " IsUserPresentAsPayee user found present in outgoing schedules: " + userId, true);
                    return true;
                }
                else
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " IsUserPresentAsPayee user not found present in outgoing schedules, checking payments: " + userId, true);

                    //Added by Jyotisna on Aug 29, 2018 to include check for outgoing payments as per Kevin
                    count = DataModel.PolicyOutgoingPayments.Where(s => s.RecipientUserCredentialId == userId).Count();
                    if (count != 0)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " IsUserPresentAsPayee user found present in outgoing payments: " + userId, true);
                        return true;
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " IsUserPresentAsPayee user not found present in outgoing payments, can be deleted: " + userId, true);
                        return false;
                    }
                }
            }
        }
    }
}
