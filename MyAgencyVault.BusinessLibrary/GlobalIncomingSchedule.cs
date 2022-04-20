using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using MyAgencyVault.BusinessLibrary.Base;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Data.SqlClient;
using System.Data;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class IncomingScheduleEntry
    {
        [DataMember]
        public Guid CoveragesScheduleId { get; set; }
        [DataMember]
        public double? FromRange { get; set; }
        [DataMember]
        public double? ToRange { get; set; }
        [DataMember]
        public double? Rate { get; set; }
        [DataMember]
        public DateTime? EffectiveFromDate { get; set; }
        [DataMember]
        public DateTime? EffectiveToDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }

    [DataContract]
    public class OutgoingScheduleEntry
    {
        [DataMember]
        public Guid CoveragesScheduleId { get; set; }
        [DataMember]
        public double? FromRange { get; set; }
        [DataMember]
        public double? ToRange { get; set; }
        [DataMember]
        public double? Rate { get; set; }
        [DataMember]
        public DateTime? EffectiveFromDate { get; set; }
        [DataMember]
        public DateTime? EffectiveToDate { get; set; }
        [DataMember]
        public Guid PayeeUserCredentialId { get; set; }
        [DataMember]
        public string PayeeName { get; set; }
        [DataMember]
        public bool IsPrimaryAgent { get; set; }
    }

    [DataContract]
    public class GlobalIncomingSchedule
    {
        [DataMember]
        public Guid CarrierId { get; set; }
        [DataMember]
        public Guid CoverageId { get; set; }
        [DataMember]
        public string ScheduleTypeName { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public int ScheduleTypeId { get; set; }
        [DataMember]
        public List<IncomingScheduleEntry> IncomingScheduleList { get; set; }
        [DataMember]
        public bool IsModified { get; set; }
    }

    [DataContract]
    public class PolicyIncomingSchedule
    {
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public string ScheduleTypeName { get; set; }
        [DataMember]
        public int ScheduleTypeId { get; set; }
        [DataMember]
        public List<IncomingScheduleEntry> IncomingScheduleList { get; set; }
        [DataMember]
        public bool IsModified { get; set; }
    }

    [DataContract]
    public class PolicyIncomingPayType
    {
        [DataMember]
        public int IncomingPaymentTypeId { get; set; }
        [DataMember]
        public string Name { get; set; }

    }

    [DataContract]
    public class PolicyOutgoingSchedule
    {
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public string ScheduleTypeName { get; set; }
        [DataMember]
        public int ScheduleTypeId { get; set; }
        [DataMember]
        public List<OutgoingScheduleEntry> OutgoingScheduleList { get; set; }
    }

    [DataContract]
    public class IncomingSchedule
    {
        #region IEditable<IncomingSchedule> Members

        public static void AddUpdateGlobalSchedule(GlobalIncomingSchedule globalSchedule)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.GlobalCoveragesSchedule gcs = null;

                if (globalSchedule.IncomingScheduleList == null || globalSchedule.IncomingScheduleList.Count == 0)
                {
                    DeleteGlobalSchedule(globalSchedule.CarrierId, globalSchedule.CoverageId, DataModel);
                    return;
                }

                List<DLinq.GlobalCoveragesSchedule> schedule = (from e in DataModel.GlobalCoveragesSchedules
                                                                where e.CarrierId == globalSchedule.CarrierId && e.CoverageId == globalSchedule.CoverageId && e.IsDeleted == false
                                                                select e).ToList();

                foreach (DLinq.GlobalCoveragesSchedule entry in schedule)
                {
                    IncomingScheduleEntry tmpEntry = globalSchedule.IncomingScheduleList.FirstOrDefault(s => s.CoveragesScheduleId == entry.CoveragesScheduleId);
                    if (tmpEntry == null)
                        entry.IsDeleted = true;
                }

                foreach (IncomingScheduleEntry _globalCoveragesShedule in globalSchedule.IncomingScheduleList)
                {
                    DLinq.MasterScheduleType _objSheduleType = new DLinq.MasterScheduleType();

                    gcs = (from e in DataModel.GlobalCoveragesSchedules
                           where e.CoveragesScheduleId == _globalCoveragesShedule.CoveragesScheduleId
                           select e).FirstOrDefault();

                    if (gcs == null)
                    {
                        gcs = new DLinq.GlobalCoveragesSchedule
                        {
                            CoveragesScheduleId = _globalCoveragesShedule.CoveragesScheduleId,
                            FromRange = _globalCoveragesShedule.FromRange,
                            ToRange = _globalCoveragesShedule.ToRange,
                            EffectiveToDate = _globalCoveragesShedule.EffectiveToDate,
                            EffectiveFromDate = _globalCoveragesShedule.EffectiveFromDate,
                            Rate = _globalCoveragesShedule.Rate,
                            ScheduleTypeId = globalSchedule.ScheduleTypeId,
                            CoverageId = globalSchedule.CoverageId,
                            CarrierId = globalSchedule.CarrierId,
                            IsDeleted = false
                        };
                        DataModel.AddToGlobalCoveragesSchedules(gcs);
                    }
                    else
                    {
                        gcs.FromRange = _globalCoveragesShedule.FromRange;
                        gcs.ToRange = _globalCoveragesShedule.ToRange;
                        gcs.EffectiveToDate = _globalCoveragesShedule.EffectiveToDate;
                        gcs.EffectiveFromDate = _globalCoveragesShedule.EffectiveFromDate;
                        gcs.Rate = _globalCoveragesShedule.Rate;
                        gcs.ScheduleTypeId = globalSchedule.ScheduleTypeId;
                        gcs.CoverageId = globalSchedule.CoverageId;
                        gcs.CarrierId = globalSchedule.CarrierId;
                    }
                }

                DataModel.SaveChanges();
            }

        }

        private static void DeleteGlobalSchedule(Guid carrierId, Guid coverageId, DLinq.CommissionDepartmentEntities DataModel)
        {
            List<DLinq.GlobalCoveragesSchedule> schedule = DataModel.GlobalCoveragesSchedules.Where(s => s.CarrierId == carrierId && s.CoverageId == coverageId).ToList();
            foreach (DLinq.GlobalCoveragesSchedule entry in schedule)
                entry.IsDeleted = true;
            DataModel.SaveChanges();
        }

        public static void AddUpdatePolicySchedule(PolicyIncomingSchedule policySchedule)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PolicyIncomingAdvancedSchedule gcs = null;

                if (policySchedule.IncomingScheduleList == null || policySchedule.IncomingScheduleList.Count == 0)
                {
                    DeletePolicySchedule(policySchedule.PolicyId);
                    return;
                }

                List<DLinq.PolicyIncomingAdvancedSchedule> schedule = (from e in DataModel.PolicyIncomingAdvancedSchedules
                                                                       where e.PolicyId == policySchedule.PolicyId
                                                                       select e).ToList();

                foreach (DLinq.PolicyIncomingAdvancedSchedule entry in schedule)
                {
                    IncomingScheduleEntry tmpEntry = policySchedule.IncomingScheduleList.FirstOrDefault(s => s.CoveragesScheduleId == entry.IncomingAdvancedScheduleId);
                    if (tmpEntry == null)
                        DataModel.DeleteObject(entry);
                }

                foreach (IncomingScheduleEntry _policyIncominShedule in policySchedule.IncomingScheduleList)
                {
                    DLinq.MasterScheduleType _objSheduleType = new DLinq.MasterScheduleType();

                    gcs = (from e in DataModel.PolicyIncomingAdvancedSchedules
                           where e.IncomingAdvancedScheduleId == _policyIncominShedule.CoveragesScheduleId
                           select e).FirstOrDefault();

                    if (gcs == null)
                    {
                        gcs = new DLinq.PolicyIncomingAdvancedSchedule
                        {
                            PolicyId = policySchedule.PolicyId,
                            IncomingAdvancedScheduleId = _policyIncominShedule.CoveragesScheduleId,
                            FromRange = _policyIncominShedule.FromRange,
                            ToRange = _policyIncominShedule.ToRange,
                            EffectiveToDate = _policyIncominShedule.EffectiveToDate,
                            EffectiveFromDate = _policyIncominShedule.EffectiveFromDate,
                            Rate = _policyIncominShedule.Rate,
                            ScheduleTypeId = policySchedule.ScheduleTypeId
                        };
                        DataModel.AddToPolicyIncomingAdvancedSchedules(gcs);
                    }
                    else
                    {
                        gcs.FromRange = _policyIncominShedule.FromRange;
                        gcs.ToRange = _policyIncominShedule.ToRange;
                        gcs.EffectiveToDate = _policyIncominShedule.EffectiveToDate;
                        gcs.EffectiveFromDate = _policyIncominShedule.EffectiveFromDate;
                        gcs.Rate = _policyIncominShedule.Rate;
                        gcs.ScheduleTypeId = policySchedule.ScheduleTypeId;
                    }
                }

                DataModel.SaveChanges();
            }
        }

        public static void DeletePolicySchedule(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.PolicyIncomingAdvancedSchedule> schedule = DataModel.PolicyIncomingAdvancedSchedules.Where(s => s.PolicyId == PolicyId).ToList();
                foreach (DLinq.PolicyIncomingAdvancedSchedule entry in schedule)
                {
                    DataModel.DeleteObject(entry);
                    DataModel.SaveChanges();
                }
            }
        }

        #endregion

        #region  "Data members aka - public properties"

        [DataMember]
        public Guid CarrierId { get; set; }
        [DataMember]
        public Guid CoverageId { get; set; }
        [DataMember]
        public string ScheduleTypeName { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public int ScheduleTypeId { get; set; }
        [DataMember]
        public List<IncomingScheduleEntry> IncomingScheduleList { get; set; }
        [DataMember]
        public bool IsModified { get; set; }
        #endregion

        public static GlobalIncomingSchedule GetGlobalIncomingSchedule(Guid carrierId, Guid coverageId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                GlobalIncomingSchedule globalIncomingSchedule = new GlobalIncomingSchedule { CarrierId = carrierId, CoverageId = coverageId };
                foreach (DLinq.GlobalCoveragesSchedule sch in DataModel.GlobalCoveragesSchedules)
                {
                    if (sch.CarrierId == carrierId && sch.CoverageId == coverageId && sch.IsDeleted == false)
                    {
                        IncomingScheduleEntry scheduleEntry = new IncomingScheduleEntry
                        {
                            CoveragesScheduleId = sch.CoveragesScheduleId,
                            FromRange = sch.FromRange,
                            ToRange = sch.ToRange,
                            EffectiveFromDate = sch.EffectiveFromDate,
                            EffectiveToDate = sch.EffectiveToDate,
                            Rate = sch.Rate,
                        };

                        if (globalIncomingSchedule.IncomingScheduleList == null)
                        {
                            globalIncomingSchedule.IncomingScheduleList = new List<IncomingScheduleEntry>();

                            globalIncomingSchedule.CoverageId = coverageId;
                            globalIncomingSchedule.CarrierId = carrierId;
                            globalIncomingSchedule.CarrierName = sch.Carrier.CarrierName;
                            globalIncomingSchedule.ProductName = sch.Coverage.ProductName;
                            globalIncomingSchedule.ScheduleTypeId = sch.ScheduleTypeId;
                            globalIncomingSchedule.ScheduleTypeName = sch.MasterScheduleType.Name;
                        }

                        globalIncomingSchedule.IncomingScheduleList.Add(scheduleEntry);
                    }
                }
                return globalIncomingSchedule;
            }
        }

        public static PolicyIncomingSchedule GetPolicyIncomingSchedule(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                PolicyIncomingSchedule policyIncomingSchedule = new PolicyIncomingSchedule { PolicyId = PolicyId };
                var PolicyIncomingSchedules = from PIS in DataModel.PolicyIncomingAdvancedSchedules
                                              where PIS.PolicyId == PolicyId
                                              select PIS;
                foreach (DLinq.PolicyIncomingAdvancedSchedule sch in PolicyIncomingSchedules)
                {
                    if (sch.PolicyId == PolicyId)
                    {
                        IncomingScheduleEntry scheduleEntry = new IncomingScheduleEntry
                        {
                            CoveragesScheduleId = sch.IncomingAdvancedScheduleId,
                            FromRange = sch.FromRange,
                            ToRange = sch.ToRange,
                            EffectiveFromDate = sch.EffectiveFromDate,
                            EffectiveToDate = sch.EffectiveToDate,
                            Rate = sch.Rate,
                        };

                        if (policyIncomingSchedule.IncomingScheduleList == null)
                        {
                            policyIncomingSchedule.IncomingScheduleList = new List<IncomingScheduleEntry>();

                            policyIncomingSchedule.PolicyId = PolicyId;
                            policyIncomingSchedule.ScheduleTypeId = sch.ScheduleTypeId.Value;
                            policyIncomingSchedule.ScheduleTypeName = sch.MasterScheduleType.Name;
                        }

                        policyIncomingSchedule.IncomingScheduleList.Add(scheduleEntry);
                    }
                }
                return policyIncomingSchedule;
            }
        }

        public static void ChangeScheduleType(Guid carrierId, Guid coverageId, int scheduleType)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.GlobalCoveragesSchedule> gcs = null;

                gcs = (from e in DataModel.GlobalCoveragesSchedules
                       where e.CoverageId == coverageId && e.CarrierId == carrierId
                       select e).ToList();

                foreach (DLinq.GlobalCoveragesSchedule gc in gcs)
                {
                    gc.ScheduleTypeId = scheduleType;
                    gc.MasterScheduleType = DataModel.MasterScheduleTypes.FirstOrDefault(s => s.ScheduleTypeId == scheduleType);
                }
                DataModel.SaveChanges();
            }
        }
    }

    [DataContract]
    public class OutgoingSchedule
    {
        public static void AddUpdatePolicyOutgoingSchedule(PolicyOutgoingSchedule policySchedule)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PolicyOutgoingAdvancedSchedule gcs = null;

                if (policySchedule.OutgoingScheduleList == null || policySchedule.OutgoingScheduleList.Count == 0)
                {
                    DeleteOutgoingPolicySchedule(policySchedule.PolicyId);
                    return;
                }

                List<DLinq.PolicyOutgoingAdvancedSchedule> schedule = (from e in DataModel.PolicyOutgoingAdvancedSchedules
                                                                       where e.PolicyId == policySchedule.PolicyId
                                                                       select e).ToList();

                foreach (DLinq.PolicyOutgoingAdvancedSchedule entry in schedule)
                {
                    OutgoingScheduleEntry tmpEntry = policySchedule.OutgoingScheduleList.FirstOrDefault(s => s.CoveragesScheduleId == entry.OutgoingAdvancedScheduleId);
                    if (tmpEntry == null)
                        DataModel.DeleteObject(entry);
                }

                foreach (OutgoingScheduleEntry _policyOutgoingShedule in policySchedule.OutgoingScheduleList)
                {
                    DLinq.MasterScheduleType _objSheduleType = new DLinq.MasterScheduleType();

                    gcs = (from e in DataModel.PolicyOutgoingAdvancedSchedules
                           where e.OutgoingAdvancedScheduleId == _policyOutgoingShedule.CoveragesScheduleId
                           select e).FirstOrDefault();

                    if (gcs == null)
                    {
                        gcs = new DLinq.PolicyOutgoingAdvancedSchedule
                        {
                            PolicyId = policySchedule.PolicyId,
                            //   IsPrimaryAgent = _policyOutgoingShedule.IsPrimaryAgent,
                            PayeeUserCredentialId = _policyOutgoingShedule.PayeeUserCredentialId,
                            PayeeName = _policyOutgoingShedule.PayeeName,
                            OutgoingAdvancedScheduleId = _policyOutgoingShedule.CoveragesScheduleId,
                            FromRange = _policyOutgoingShedule.FromRange,
                            ToRange = _policyOutgoingShedule.ToRange,
                            EffectiveToDate = _policyOutgoingShedule.EffectiveToDate,
                            EffectiveFromDate = _policyOutgoingShedule.EffectiveFromDate,
                            Rate = _policyOutgoingShedule.Rate,
                            ScheduleTypeId = policySchedule.ScheduleTypeId,
                            ModifiedOn = DateTime.Now
                        };

                        DLinq.UserDetail userDetail = DataModel.UserDetails.FirstOrDefault(s => s.UserCredentialId == _policyOutgoingShedule.PayeeUserCredentialId);
                        if (userDetail.AddPayeeOn == null)
                            userDetail.AddPayeeOn = DateTime.Now;

                        DataModel.AddToPolicyOutgoingAdvancedSchedules(gcs);
                    }
                    else
                    {
                        //    gcs.IsPrimaryAgent = _policyOutgoingShedule.IsPrimaryAgent;
                        gcs.PayeeUserCredentialId = _policyOutgoingShedule.PayeeUserCredentialId;
                        gcs.PayeeName = _policyOutgoingShedule.PayeeName;
                        gcs.FromRange = _policyOutgoingShedule.FromRange;
                        gcs.ToRange = _policyOutgoingShedule.ToRange;
                        gcs.EffectiveToDate = _policyOutgoingShedule.EffectiveToDate;
                        gcs.EffectiveFromDate = _policyOutgoingShedule.EffectiveFromDate;
                        gcs.Rate = _policyOutgoingShedule.Rate;
                        gcs.ScheduleTypeId = policySchedule.ScheduleTypeId;
                        gcs.ModifiedOn = DateTime.Now;
                    }
                }

                DataModel.SaveChanges();
            }
        }

        public static void DeleteOutgoingPolicySchedule(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.PolicyIncomingAdvancedSchedule> schedule = DataModel.PolicyIncomingAdvancedSchedules.Where(s => s.PolicyId == PolicyId).ToList();
                foreach (DLinq.PolicyIncomingAdvancedSchedule entry in schedule)
                    DataModel.DeleteObject(entry);
                DataModel.SaveChanges();
            }
        }

        public static PolicyOutgoingSchedule GetPolicyOutgoingSchedule(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                PolicyOutgoingSchedule policyOutgoingSchedule = new PolicyOutgoingSchedule { PolicyId = PolicyId };
                foreach (DLinq.PolicyOutgoingAdvancedSchedule sch in DataModel.PolicyOutgoingAdvancedSchedules)
                {
                    if (sch.PolicyId == PolicyId)
                    {
                        OutgoingScheduleEntry scheduleEntry = new OutgoingScheduleEntry
                        {
                            //     IsPrimaryAgent = sch.IsPrimaryAgent,
                            PayeeUserCredentialId = sch.PayeeUserCredentialId.Value,
                            PayeeName = sch.PayeeName,
                            CoveragesScheduleId = sch.OutgoingAdvancedScheduleId,
                            FromRange = sch.FromRange,
                            ToRange = sch.ToRange,
                            EffectiveFromDate = sch.EffectiveFromDate,
                            EffectiveToDate = sch.EffectiveToDate,
                            Rate = sch.Rate,
                        };

                        if (policyOutgoingSchedule.OutgoingScheduleList == null)
                        {
                            policyOutgoingSchedule.OutgoingScheduleList = new List<OutgoingScheduleEntry>();

                            policyOutgoingSchedule.PolicyId = PolicyId;
                            policyOutgoingSchedule.ScheduleTypeId = sch.ScheduleTypeId.Value;
                            policyOutgoingSchedule.ScheduleTypeName = sch.MasterScheduleType.Name;
                        }

                        policyOutgoingSchedule.OutgoingScheduleList.Add(scheduleEntry);
                    }
                }
                return policyOutgoingSchedule;
            }
        }

        public static List<Guid> GetAllPoliciesForUser(Guid userCredId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.PolicyOutgoingAdvancedSchedule> schedule = (from e in DataModel.PolicyOutgoingAdvancedSchedules
                                                                       where e.PayeeUserCredentialId == userCredId
                                                                       select e).OrderBy(s => s.PolicyId).ToList();

                List<Guid> policies = new List<Guid>();
                foreach (DLinq.PolicyOutgoingAdvancedSchedule entry in schedule)
                {
                    Guid policyId = policies.FirstOrDefault(s => s == entry.PolicyId);
                    if (policyId == null)
                        policies.Add(entry.PolicyId.Value);
                }
                return policies;
            }
        }

        public static bool IsUserPresentAsPayee(Guid userId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                int count = DataModel.PolicyOutgoingAdvancedSchedules.Where(s => s.PayeeUserCredentialId == userId).Count();
                if (count != 0)
                    return true;
                else
                    return false;
            }
        }
    }


    [DataContract]
    public class PayorSchedules
    {
        #region Public Properties
        [DataMember]
        public Guid IncomingScheduleID { get; set; }

        [DataMember]
        public string Payor { get; set; }
        [DataMember]
        public string Carrier { get; set; }
        [DataMember]
        public string Coverage { get; set; }
        [DataMember]
        public string ProductType { get; set; }

        #endregion
    }

    [DataContract]
    public class PayorIncomingSchedule
    {
        #region Public Properties
        [DataMember]
        public string PayorName { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public string CoverageName { get; set; }

        [DataMember]
        public Guid IncomingScheduleID { get; set; }
        [DataMember]
        public Guid LicenseeID { get; set; }
        [DataMember]
        public Guid PayorID { get; set; }
        [DataMember]
        public Guid CarrierID { get; set; }
        [DataMember]
        public Guid CoverageID { get; set; }
        [DataMember]
        public int ScheduleTypeId { get; set; }
        [DataMember]
        public double? FirstYearPercentage { get; set; }
        [DataMember]
        public double? RenewalPercentage { get; set; }
        [DataMember]
        public double? SplitPercentage { get; set; }
        [DataMember]
        public string StringRenewalPercentage { get; set; }
        [DataMember]
        public string StringFirstYearPercentage { get; set; }
        [DataMember]
        public string StringSplitPercentage { get; set; }
        [DataMember]
        public int IncomingPaymentTypeID { get; set; }
        [DataMember]
        public int Advance { get; set; }
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public Guid? CreatedBy { get; set; }
        [DataMember]
        public Guid? ModifiedBy { get; set; }
        [DataMember]
        public DateTime? CreatedOn { get; set; }
        [DataMember]
        public DateTime? ModifiedOn { get; set; }
        [DataMember]
        public string IncomingPaymentTypeName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string ScheduleType { get; set; }

        [DataMember]
        public bool IsNamedSchedule { get; set; }


        [DataMember]
        public List<Graded> GradedSchedule { get; set; }

        [DataMember]
        public Mode Mode { get; set; }


        CustomMode _mode = CustomMode.Graded;
        [DataMember]
        public CustomMode CustomType
        {
            get { return _mode; }
            set { _mode = value; }
        }

        [DataMember]
        public List<NonGraded> NonGradedSchedule { get; set; }
        //   public CustomMode Mode1 { get => _mode; set => _mode = value; }
        #endregion

        public PayorIncomingSchedule()
        {
            this.CustomType = CustomMode.Graded;
        }

        public static PolicyToolIncommingShedule GetBasicScheduleOnSettingsID(Guid settingsID, string agencyName  = "")
        {
            PolicyToolIncommingShedule schedule = new PolicyToolIncommingShedule();
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_getIncomingScheuleOnSettingsID", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@scheduleID", settingsID);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            schedule.IncomingScheduleID = (Guid)dr["IncomingScheduleID"];

                            string frstYear = Convert.ToString(dr["FirstYearPercentage"]);
                            double fy = 0;
                            double.TryParse(frstYear, out fy);
                            schedule.FirstYearPercentage = fy;

                            string renewYear = Convert.ToString(dr["RenewalPercentage"]);
                            double ry = 0;
                            double.TryParse(renewYear, out ry);
                            schedule.RenewalPercentage = ry;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetScheduleOnSetingsID ex: " + ex.Message, true, agencyName);
                throw ex;
            }
            return schedule;
        }
        public static void SaveSchedule(PayorIncomingSchedule schedule, int overwrite = 0)
        {
            try
            {
                schedule.CustomType = (schedule.CustomType == 0) ? CustomMode.Graded : schedule.CustomType;
                if (schedule.Mode == Mode.Custom)
                {
                    schedule.FirstYearPercentage = 0;
                    schedule.RenewalPercentage = 0;

                }
                {
                    ActionLogger.Logger.WriteImportPolicyLog("SaveSchedule schedule" + schedule.ToStringDump(), true);
                    using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_savePayorSchedule", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ScheduleID", schedule.IncomingScheduleID);
                            cmd.Parameters.AddWithValue("@LicenseeID", schedule.LicenseeID);
                            cmd.Parameters.AddWithValue("@PayorID", schedule.PayorID);
                            cmd.Parameters.AddWithValue("@CarrierID", schedule.CarrierID);
                            cmd.Parameters.AddWithValue("@CoverageID", schedule.CoverageID);
                            cmd.Parameters.AddWithValue("@ProductType", schedule.ProductType);
                            cmd.Parameters.AddWithValue("@FirstYear", schedule.FirstYearPercentage);
                            cmd.Parameters.AddWithValue("@Renewal", schedule.RenewalPercentage);
                            cmd.Parameters.AddWithValue("@ScheduleTypeID", schedule.ScheduleTypeId);
                            cmd.Parameters.AddWithValue("@IncomingPaymentTypeId", schedule.IncomingPaymentTypeID);
                            cmd.Parameters.AddWithValue("@SplitPercentage", schedule.SplitPercentage);
                            cmd.Parameters.AddWithValue("@Advance", schedule.Advance);
                            cmd.Parameters.AddWithValue("@CreatedBy", schedule.CreatedBy);
                            cmd.Parameters.AddWithValue("@ModifiedBy", schedule.ModifiedBy);
                            cmd.Parameters.AddWithValue("@Overwrite", overwrite);
                            cmd.Parameters.AddWithValue("@Mode", (int)schedule.Mode);
                            cmd.Parameters.AddWithValue("@CustomType", (int)schedule.CustomType);
                            con.Open();

                            cmd.ExecuteNonQuery();
                        }
                    }
                    if (schedule.Mode == Mode.Custom)
                    {
                        if (schedule.CustomType == CustomMode.Graded)
                        {
                            SaveGradedSchedule(schedule);
                        }
                        else
                        {
                            SaveNonGradedSchedule(schedule);
                        }
                        OverwriteCustomSchedule(schedule.IncomingScheduleID, overwrite);
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("SaveSchedule exception: " + ex.Message, true);
            }
        }


        static void DeleteCustomSchedule(Guid scheduleId, Guid? policyId, string agencyName  = "")
        {
            ActionLogger.Logger.WriteImportPolicyLog("DeleteCustomSchedule:Processing begins with scheduleId" + scheduleId, true, agencyName);
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_DeleteCustomSchedule", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ScheduleId", scheduleId);
                        cmd.Parameters.AddWithValue("@PolicyID", policyId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeleteCustomSchedule:Exception occurs while delete with scheduleId" + scheduleId + " " + ex.Message, true, agencyName);
                throw ex;
            }
        }

        static void OverwriteCustomSchedule(Guid scheduleID, int overwrite)
        {
            ActionLogger.Logger.WriteImportPolicyLog("OverwriteCustomSchedule request - " + scheduleID + ", overWrite: " + overwrite, true);
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_OverwriteCustomSchedule", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PayorScheduleID", scheduleID);
                        cmd.Parameters.AddWithValue("@Overwrite", overwrite);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("OverwriteCustomSchedule ex: " + ex.Message, true);
                throw ex;
            }
        }

        public static double GetGradedPercentOnRange(double totalPremium)
        {
            double expectedPercent = 0;
            ActionLogger.Logger.WriteImportPolicyLog("GetGradedPercentOnRange request - " + totalPremium, true);
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    /* using (SqlCommand cmd = new SqlCommand("Usp_OverwriteCustomSchedule", con))
                     {
                         cmd.CommandType = System.Data.CommandType.StoredProcedure;
                         cmd.Parameters.AddWithValue("@PayorScheduleID", scheduleID);
                         cmd.Parameters.AddWithValue("@Overwrite", overwrite);
                         con.Open();
                         cmd.ExecuteNonQuery();
                     }*/
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetGradedPercentOnRange ex: " + ex.Message, true);
                throw ex;
            }
            return expectedPercent;
        }

        public static void SaveGradedSchedule(PayorIncomingSchedule schedule, string agencyName  = "", Guid? policyId = null)
        {
            try
            {
                DeleteCustomSchedule(schedule.IncomingScheduleID, policyId, agencyName);
                foreach (var Gradedchedule in schedule.GradedSchedule)
                {
                    using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_saveCustomSchedule", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ScheduleID", schedule.IncomingScheduleID);
                            cmd.Parameters.AddWithValue("@GradedFrom", Gradedchedule.From);
                            cmd.Parameters.AddWithValue("@GradedTo", Gradedchedule.To);
                            cmd.Parameters.AddWithValue("@GradedPercent", Gradedchedule.Percent);
                            cmd.Parameters.AddWithValue("@PolicyID", policyId);
                            // cmd.Parameters.AddWithValue("@CustomType", Gradedchedule.CustomType);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SaveNonGradedSchedule(PayorIncomingSchedule schedule, string agencyName  = "", Guid? policyId = null)
        {
            try
            {
                DeleteCustomSchedule(schedule.IncomingScheduleID, policyId, agencyName);
                foreach (var NonGradedchedule in schedule.NonGradedSchedule)
                {
                    using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_saveCustomSchedule", con))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PolicyID", policyId);
                            cmd.Parameters.AddWithValue("@ScheduleID", schedule.IncomingScheduleID);
                            cmd.Parameters.AddWithValue("@NonGradedPercent", NonGradedchedule.Percent);
                            cmd.Parameters.AddWithValue("@Year", NonGradedchedule.Year);
                            //   cmd.Parameters.AddWithValue("@CustomType", NonGradedchedule.CustomType);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("SaveNonGradedSchedule:Exception occurs while Save with schedule" + schedule + " " + ex.Message, true, agencyName);
                throw ex;
            }
        }
        public static void DeletePolicySchedule(Guid policyId, string agencyName  = "")
        {
            ActionLogger.Logger.WriteImportPolicyLog("DeletePolicySchedule:Processing begins with scheduleId" + policyId, true, agencyName);
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_DeletePolicySchedule", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PolicyId", policyId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeleteCustomSchedule:Exception occurs while delete with policyId" + policyId + " " + ex.Message, true, agencyName);
                throw ex;
            }
        }
        public static void DeleteSchedule(Guid ScheduleID)
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeleteSchedule ScheduleID" + ScheduleID, true);
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_deletePayorSchedule", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ScheduleID", ScheduleID);
                        con.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeleteSchedule exception: " + ex.Message, true);
            }
        }

        public static List<PayorIncomingSchedule> GetAllSchedules(Guid LicenseeID)
        {
            List<PayorIncomingSchedule> lstSchedules = new List<PayorIncomingSchedule>();
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetAllSchedules LicenseeID" + LicenseeID, true);
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getAllPayorScheduledList", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LicenseeID", LicenseeID);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            PayorIncomingSchedule schedule = new PayorIncomingSchedule();

                            schedule.IncomingScheduleID = (Guid)dr["IncomingScheduleID"];
                            schedule.PayorID = (Guid)(dr["PayorID"]);
                            schedule.CarrierID = (Guid)(dr["CarrierID"]);
                            schedule.CoverageID = (Guid)(dr["CoverageID"]);
                            schedule.ProductType = Convert.ToString(dr["ProductType"]);
                            schedule.ScheduleTypeId = (int)(dr["ScheduleTypeId"]);
                            schedule.IncomingPaymentTypeID = (int)(dr["IncomingPaymentTypeID"]);
                            schedule.IncomingPaymentTypeName = Convert.ToString(dr["Name"]);

                            string strIsNamed = Convert.ToString(dr["IsNamedSchedule"]);
                            bool isNamed = false;
                            bool.TryParse(strIsNamed, out isNamed);
                            schedule.IsNamedSchedule = isNamed; //Convert.ToBoolean(dr["IsNamedSchedule"]);

                            string frstYear = Convert.ToString(dr["FirstYearPercentage"]);
                            double fy = 0;
                            double.TryParse(frstYear, out fy);
                            schedule.FirstYearPercentage = fy;

                            string renewYear = Convert.ToString(dr["RenewalPercentage"]);
                            double ry = 0;
                            double.TryParse(renewYear, out ry);
                            schedule.RenewalPercentage = ry;

                            string split = Convert.ToString(dr["SplitPercentage"]);
                            double splitPer = 0;
                            double.TryParse(split, out splitPer);
                            schedule.SplitPercentage = splitPer;

                            string advanc = Convert.ToString(dr["Advance"]);
                            int adv = 0;
                            int.TryParse(advanc, out adv);
                            schedule.Advance = adv;

                            schedule.PayorName = Convert.ToString(dr["PayorName"]);
                            schedule.CarrierName = Convert.ToString(dr["CarrierName"]);
                            schedule.CoverageName = Convert.ToString(dr["ProductName"]);

                            string mod = Convert.ToString(dr["Mode"]);
                            int scheduleMode = 0;
                            int.TryParse(mod, out scheduleMode);
                            schedule.Mode = (Mode)scheduleMode;

                            string strType = Convert.ToString(dr["CustomType"]);
                            int intType = 0;
                            int.TryParse(strType, out intType);
                            intType = (intType == 0) ? 1 : intType;
                            schedule.CustomType = (CustomMode)intType;


                            if (schedule.Mode == Mode.Custom)
                            {
                                if (schedule.CustomType == CustomMode.Graded)
                                {
                                    schedule.GradedSchedule = GradedScheduleList(schedule.IncomingScheduleID);
                                }
                                else
                                {
                                    schedule.NonGradedSchedule = NonGradedScheduleList(schedule.IncomingScheduleID);
                                }
                            }

                            lstSchedules.Add(schedule);
                        }
                        dr.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetAllSchedules exception: " + ex.Message, true);
            }
            return lstSchedules;
        }

        public static PayorIncomingSchedule GetPayorScheduleDetails(Guid PayorID, Guid CarrierID, Guid CoverageID, Guid LicenseeID, string ProductType, int IncomingPaymentTypeID, string agencyName  = "")
        {
            PayorIncomingSchedule schedule = new PayorIncomingSchedule();
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetPayorScheduleDetails schedule: " + PayorID + ", Carrier: " + CarrierID + ", coverage: " + CoverageID + ", productyType: " + ProductType, true, agencyName);
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getSelectedPayorSchedule", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PayorID", PayorID);
                        cmd.Parameters.AddWithValue("@carrierID", CarrierID);
                        cmd.Parameters.AddWithValue("@CoverageID", CoverageID);
                        cmd.Parameters.AddWithValue("@LicenseeID", LicenseeID);
                        cmd.Parameters.AddWithValue("@ProductType", ProductType);
                        cmd.Parameters.AddWithValue("@IncomingPaymentTypeID", IncomingPaymentTypeID);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            schedule.IncomingScheduleID = (Guid)dr["IncomingScheduleID"];
                            schedule.PayorID = (Guid)(dr["PayorID"]);
                            schedule.CarrierID = (Guid)(dr["CarrierID"]);
                            schedule.CoverageID = (Guid)(dr["CoverageID"]);
                            schedule.ProductType = Convert.ToString(dr["ProductType"]);
                            schedule.ScheduleTypeId = (int)(dr["ScheduleTypeId"]);
                            schedule.IncomingPaymentTypeID = (int)(dr["IncomingPaymentTypeID"]);

                            string frstYear = Convert.ToString(dr["FirstYearPercentage"]);
                            double fy = 0;
                            double.TryParse(frstYear, out fy);
                            schedule.FirstYearPercentage = fy;

                            string renewYear = Convert.ToString(dr["RenewalPercentage"]);
                            double ry = 0;
                            double.TryParse(renewYear, out ry);
                            schedule.RenewalPercentage = ry;

                            string split = Convert.ToString(dr["SplitPercentage"]);
                            double splitPer = 0;
                            double.TryParse(split, out splitPer);
                            schedule.SplitPercentage = splitPer;

                            string advanc = Convert.ToString(dr["Advance"]);
                            int adv = 0;
                            int.TryParse(advanc, out adv);
                            schedule.Advance = adv;

                            string mod = Convert.ToString(dr["Mode"]);
                            int scheduleMode = 0;
                            int.TryParse(mod, out scheduleMode);
                            schedule.Mode = (Mode)scheduleMode;

                            string strType = Convert.ToString(dr["CustomType"]);
                            int intType = 0;
                            int.TryParse(strType, out intType);
                            intType = (intType == 0) ? 1 : intType;
                            schedule.CustomType = (CustomMode)intType;
                        }
                        dr.Close();
                    }
                }
                if (schedule.Mode == Mode.Custom)
                {
                    if (schedule.CustomType == CustomMode.Graded)
                    {
                        schedule.GradedSchedule = GradedScheduleList(schedule.IncomingScheduleID, agencyName);
                    }
                    else
                    {
                        schedule.NonGradedSchedule = NonGradedScheduleList(schedule.IncomingScheduleID, agencyName);
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetPayorScheduleDetails exception: " + ex.Message, true, agencyName);
            }
            return schedule;
        }

        public static List<Graded> GradedScheduleList(Guid incomingScheduleId, string agencyName  = "", Guid? PolicyId = null)
        {
            List<Graded> list = new List<Graded>();
            using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetGradedScheduleList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@incomingScheduleId", incomingScheduleId);
                    cmd.Parameters.AddWithValue("@PolicyID", PolicyId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Graded data = new Graded();
                        data.From = reader.IsDBNull("GradedFrom") ? 0.0 : Convert.ToDouble(reader["GradedFrom"]);
                        data.To = reader.IsDBNull("GradedTo") ? 0.0 : Convert.ToDouble(reader["GradedTo"]);
                        data.Percent = reader.IsDBNull("GradedPercent") ? 0 : Convert.ToDouble(reader["GradedPercent"]);
                        list.Add(data);
                    }
                }
            }
            return list;
        }

        public static List<NonGraded> NonGradedScheduleList(Guid incomingScheduleId, string agencyName  = "", Guid? PolicyId = null)
        {
            List<NonGraded> list = new List<NonGraded>();
            using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetGradedScheduleList", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@incomingScheduleId", incomingScheduleId);
                    cmd.Parameters.AddWithValue("@PolicyID", PolicyId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        NonGraded data = new NonGraded();
                        data.Year = reader.IsDBNull("YearNumber") ? 0 : (int)(reader["YearNumber"]);
                        data.Percent = reader.IsDBNull("NonGradedPercent") ? 0 : (double)(reader["NonGradedPercent"]);
                        list.Add(data);
                    }
                }
            }
            return list;
        }
    }
    [DataContract]
    public enum Mode
    {
        [EnumMember]
        Standard = 0,

        [EnumMember]
        Custom = 1
    }

    [DataContract]
    public enum CustomMode
    {
        [EnumMember]
        Graded = 1,

        [EnumMember]
        NonGraded = 2
    }
    [DataContract]
    public class Graded
    {
        [DataMember]
        public double From { get; set; }
        [DataMember]
        public double To { get; set; }
        [DataMember]
        public double Percent { get; set; }
        //[DataMember]
        //public int CustomType { get; set; }

    }
    public class NonGraded
    {
        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public double Percent { get; set; }
        //[DataMember]
        //public int CustomType { get; set; }

    }
}
