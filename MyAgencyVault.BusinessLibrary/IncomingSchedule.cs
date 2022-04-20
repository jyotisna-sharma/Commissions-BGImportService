/// Incoimg Advence Schedule
///
///

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using MyAgencyVault.BusinessLibrary.Base;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{
    //[DataContract]
    //public class IncomingSchedule 
    //{
    //    #region IEditable<IncomingSchedule> Members        

    //    public static void AddUpdateIncomingSchedule(List<IncomingSchedule> IncSchedule,Guid SelePolicyId)
    //    {
    //        DeleteIncomingSchedulePolicyWise(SelePolicyId);
    //        using (DLinq.CommissionDepartmentEntities DataModel = new DLinq.CommissionDepartmentEntities())
    //        {
    //            DLinq.PolicyIncomingAdvancedSchedule gcs = null;

    //            foreach (IncomingSchedule _globalCoveragesShedule in IncSchedule)
    //            {
                    
    //                gcs = (from e in DataModel.PolicyIncomingAdvancedSchedules
    //                       where e.IncomingAdvancedScheduleId == _globalCoveragesShedule.IncomingAdvancedScheduleId
    //                       select e).FirstOrDefault();
    //                if (gcs == null)
    //                {
    //                    gcs = new DLinq.PolicyIncomingAdvancedSchedule
    //                    {

    //                        IncomingAdvancedScheduleId = _globalCoveragesShedule.IncomingAdvancedScheduleId,
    //                        FromRange = _globalCoveragesShedule.FromRange,
    //                        ToRange = _globalCoveragesShedule.ToRange,
    //                        EffectiveToDate = _globalCoveragesShedule.EffectiveToDate,
    //                        EffectiveFromDate = _globalCoveragesShedule.EffectiveFromDate,
    //                        Rate = _globalCoveragesShedule.Rate,

    //                    };

    //                    gcs.MasterScheduleTypeReference.Value = (from l in DataModel.MasterScheduleTypes where l.ScheduleTypeId == _globalCoveragesShedule.ScheduleTypeId select l).FirstOrDefault();
    //                    gcs.PolicyReference.Value = (from l in DataModel.Policies where l.PolicyId == _globalCoveragesShedule.PolicyId select l).FirstOrDefault(); ;
    //                    DataModel.AddToPolicyIncomingAdvancedSchedules(gcs);
                       
    //                }
    //                else
    //                {
    //                    gcs.FromRange = _globalCoveragesShedule.FromRange;
    //                    gcs.ToRange = _globalCoveragesShedule.ToRange;
    //                    gcs.EffectiveToDate = _globalCoveragesShedule.EffectiveToDate;
    //                    gcs.EffectiveFromDate = _globalCoveragesShedule.EffectiveFromDate;
    //                    gcs.Rate = _globalCoveragesShedule.Rate;
                                               
    //                }
    //                DataModel.SaveChanges();
    //            }

    //        }

    //    }
    //    public static void DeleteIncomingSchedulePolicyWise(Guid Policyid)
    //    {
    //        using (DLinq.CommissionDepartmentEntities DataModel = new DLinq.CommissionDepartmentEntities())
    //        {
    //           List<DLinq.PolicyIncomingAdvancedSchedule> _policyCoverageId =(from n in DataModel.PolicyIncomingAdvancedSchedules
    //             where (n.PolicyId == Policyid)
    //             select n).ToList();
    //           foreach (DLinq.PolicyIncomingAdvancedSchedule pad in _policyCoverageId)
    //           {
    //               DataModel.DeleteObject(pad);
    //               DataModel.SaveChanges();
    //           }
    //        }
    //    }
    //    public void Delete()
    //    {
    //        using (DLinq.CommissionDepartmentEntities DataModel = new DLinq.CommissionDepartmentEntities())
    //        {
    //            DLinq.PolicyIncomingAdvancedSchedule _policyCoverageId = (from n in DataModel.PolicyIncomingAdvancedSchedules
    //                                                                where (n.IncomingAdvancedScheduleId == this.IncomingAdvancedScheduleId)
    //                                                                select n).FirstOrDefault();

    //            DataModel.DeleteObject(_policyCoverageId);
    //            DataModel.SaveChanges();
    //        }
    //    }

       
    //    #endregion
    //    #region  "Data members aka - public properties"
    //    [DataMember]
    //    public Guid IncomingAdvancedScheduleId { get; set; }
    //    [DataMember]
    //    public Guid PolicyId { get; set; }
    //    [DataMember]
    //    public int ScheduleTypeId { get; set; }
    //    [DataMember]
    //    public double? FromRange { get; set; }
    //    [DataMember]
    //    public double? ToRange { get; set; }
    //    [DataMember]
    //    public double? Rate { get; set; }
    //    [DataMember]
    //    public DateTime? EffectiveFromDate { get; set; }
    //    [DataMember]
    //    public DateTime? EffectiveToDate { get; set; }
    //    [DataMember]
    //    public bool IsDeleted { get; set; }
    //    #endregion
    //    /// <summary>
    //    /// of a payor .. / policy.
    //    /// developer need to re think about its requirement.
    //    /// </summary>
    //    /// <returns></returns>
    //    public static List<IncomingSchedule> GetScheduleList()
    //    {
    //        using (Entity.DataModel)
    //        {
    //            return (from gc in Entity.DataModel.PolicyIncomingAdvancedSchedules.Include("MasterScheduleTypes").Include("Policies")

    //                    select new IncomingSchedule
    //                    {
    //                        IncomingAdvancedScheduleId = gc.IncomingAdvancedScheduleId,
    //                        FromRange = gc.FromRange,
    //                        ToRange = gc.ToRange,
    //                        EffectiveFromDate = gc.EffectiveFromDate,
    //                        EffectiveToDate = gc.EffectiveToDate,
    //                        Rate = gc.Rate,
    //                        ScheduleTypeId = gc.MasterScheduleType.ScheduleTypeId,
    //                        PolicyId = gc.Policy.PolicyId
    //                    }).ToList();
    //        }
    //    }

    //    public static List<IncomingSchedule> GetAdvanceScheduleListPolicyWise(Guid PolicyId)
    //    {
    //        using (Entity.DataModel)
    //        {
    //            return (from gc in Entity.DataModel.PolicyIncomingAdvancedSchedules
    //                        .Include("MasterScheduleTypes")
    //                        .Include("Policies")
    //                        where(gc.PolicyId==PolicyId)
    //                    select new IncomingSchedule
    //                    {
    //                        IncomingAdvancedScheduleId = gc.IncomingAdvancedScheduleId,
    //                        FromRange = gc.FromRange,
    //                        ToRange = gc.ToRange,
    //                        EffectiveFromDate = gc.EffectiveFromDate,
    //                        EffectiveToDate = gc.EffectiveToDate,
    //                        Rate = gc.Rate,
    //                        ScheduleTypeId = gc.MasterScheduleType.ScheduleTypeId,
    //                        PolicyId = gc.Policy.PolicyId
    //                    }).ToList();
    //        }
    //    }
    //}
}
