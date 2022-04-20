using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{
    //[DataContract]
    //public class OutgoingShedule : IEditable <OutgoingShedule>
    //{
    //    #region IEditable<OutgoingPayment> Members

    //    public void AddUpdate()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static void AddUpdateOutGoingSchedule(List<OutgoingShedule> OutAdvanceSchedule,Guid SelPolicyId)
    //    {

    //        DeletePolicyWise(SelPolicyId);
    //        if (OutAdvanceSchedule.Count == 0) return;
    //        using (DLinq.CommissionDepartmentEntities DataModel = new DLinq.CommissionDepartmentEntities())
    //        {
    //            DLinq.PolicyOutgoingAdvancedSchedule gcs = null;

    //            foreach (OutgoingShedule _globalCoveragesShedule in OutAdvanceSchedule)
    //            {
    //                //DLinq.MasterScheduleType _objSheduleType = new DLinq.MasterScheduleType();
    //                //_objSheduleType = (from l in DataModel.MasterScheduleTypes where l.ScheduleTypeId == _globalCoveragesShedule.ScheduleTypeId select l).FirstOrDefault();

    //                //DLinq.Policy _objPolicy = new DLinq.Policy();
    //                //_objPolicy = (from l in DataModel.Policies where l.PolicyId == _globalCoveragesShedule.PolicyId select l).FirstOrDefault();

    //                //DLinq.UserCredential _objPayee = new DLinq.UserCredential();
    //                //_objPayee = (from l in DataModel.UserCredentials where l.UserCredentialId == _globalCoveragesShedule.PayeeUserCredentialId select l).FirstOrDefault();

    //                gcs = (from e in DataModel.PolicyOutgoingAdvancedSchedules
    //                       where e.OutgoingAdvancedScheduleId == _globalCoveragesShedule.OutgoingAdvancedScheduleId
    //                       select e).FirstOrDefault();

    //                if (gcs == null)
    //                {
    //                    gcs = new DLinq.PolicyOutgoingAdvancedSchedule
    //                    {

    //                        OutgoingAdvancedScheduleId = _globalCoveragesShedule.OutgoingAdvancedScheduleId,
    //                        FromRange = _globalCoveragesShedule.FromRange,
    //                        ToRange = _globalCoveragesShedule.ToRange,
    //                        EffectiveToDate = _globalCoveragesShedule.EffectiveToDate,
    //                        EffectiveFromDate = _globalCoveragesShedule.EffectiveFromDate,
    //                        Rate = _globalCoveragesShedule.Rate,
    //                        IsPrimaryAgent = _globalCoveragesShedule.IsPrimaryAgent,
    //                        PayeeName = _globalCoveragesShedule.PayeeName,
    //                        ModifiedOn=DateTime.Today,

    //                    };

    //                    gcs.MasterScheduleTypeReference.Value = (from l in DataModel.MasterScheduleTypes where l.ScheduleTypeId == _globalCoveragesShedule.ScheduleTypeId select l).FirstOrDefault(); ;
    //                    gcs.PolicyReference.Value = (from l in DataModel.Policies where l.PolicyId == _globalCoveragesShedule.PolicyId select l).FirstOrDefault();
    //                    gcs.UserCredentialReference.Value = (from l in DataModel.UserCredentials where l.UserCredentialId == _globalCoveragesShedule.PayeeUserCredentialId select l).FirstOrDefault();
    //                    DataModel.AddToPolicyOutgoingAdvancedSchedules(gcs);

    //                }
    //                else
    //                {
    //                    gcs.FromRange = _globalCoveragesShedule.FromRange;
    //                    gcs.ToRange = _globalCoveragesShedule.ToRange;
    //                    gcs.EffectiveToDate = _globalCoveragesShedule.EffectiveToDate;
    //                    gcs.EffectiveFromDate = _globalCoveragesShedule.EffectiveFromDate;
    //                    gcs.Rate = _globalCoveragesShedule.Rate;
    //                    gcs.IsPrimaryAgent = _globalCoveragesShedule.IsPrimaryAgent;
    //                    gcs.PayeeName = _globalCoveragesShedule.PayeeName;
    //                    gcs.ModifiedOn = DateTime.Today;



    //                }
    //                DataModel.SaveChanges();
    //            }

    //        }

    //    }

    //    public void Delete()
    //    {
    //        using (DLinq.CommissionDepartmentEntities DataModel = new DLinq.CommissionDepartmentEntities())
    //        {
    //            DLinq.PolicyOutgoingAdvancedSchedule _policyCoverageId = (from n in DataModel.PolicyOutgoingAdvancedSchedules
    //                                                                      where (n.OutgoingAdvancedScheduleId == this.OutgoingAdvancedScheduleId)
    //                                                                      select n).FirstOrDefault();

    //            DataModel.DeleteObject(_policyCoverageId);
    //            DataModel.SaveChanges();
    //        }
    //    }

    //    public static void DeletePolicyWise(Guid PolicyId)
    //    {
    //        using (DLinq.CommissionDepartmentEntities DataModel = new DLinq.CommissionDepartmentEntities())
    //        {
    //           List< DLinq.PolicyOutgoingAdvancedSchedule> _policyCoverageId = (from n in DataModel.PolicyOutgoingAdvancedSchedules
    //                                                                            where (n.PolicyId == PolicyId)
    //                                                                      select n).ToList();
    //           foreach (DLinq.PolicyOutgoingAdvancedSchedule pad in _policyCoverageId)
    //            {
    //                DataModel.DeleteObject(pad);
    //                DataModel.SaveChanges();
    //            }
    //        }
    //    }
    //    public OutgoingShedule GetOfID()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool IsValid()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    #endregion
    //    #region "Data members aka - public properties"
    //    [DataMember]
    //    public Guid OutgoingAdvancedScheduleId { get; set; }
    //    [DataMember]
    //    public Guid PolicyId { get; set; }
    //    [DataMember]
    //    public Guid PayeeUserCredentialId { get; set; }
    //    [DataMember]
    //    public string PayeeName { get; set; }
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
    //    public bool IsPrimaryAgent { get; set; }
       
    //    [DataMember]
    //    public DateTime ModifiedOn { get; set; }

    //    #endregion 
    //    /// <summary>
    //    /// ask pankaj sharma about its functionality
    //    /// </summary>
    //    public void ReverseOutgoingPayments()
    //    {
    //    }
    //    /// <summary>
    //    /// developer need to recheck and think of the requirement of this funciton.
    //    /// </summary>
    //    /// <param name="policyID"></param>
    //    /// <returns>returns all the outgoing payments related to a policyid given in the parameter.</returns>
    //    public static List<OutgoingShedule> GetOutgoingShedule()
    //    {
    //        using (Entity.DataModel)
    //        {
    //            return (from gc in Entity.DataModel.PolicyOutgoingAdvancedSchedules.Include("MasterScheduleTypes").Include("CarrierCoverages")

    //                    select new OutgoingShedule
    //                    {
    //                        OutgoingAdvancedScheduleId = gc.OutgoingAdvancedScheduleId,
    //                        FromRange = gc.FromRange,
    //                        ToRange = gc.ToRange,
    //                        EffectiveFromDate = gc.EffectiveFromDate,
    //                        EffectiveToDate = gc.EffectiveToDate,
    //                        Rate = gc.Rate,
    //                        ScheduleTypeId = gc.ScheduleTypeId.Value,
    //                        PolicyId = gc.Policy.PolicyId,
    //                        PayeeUserCredentialId = gc.UserCredential.UserCredentialId,
    //                        IsPrimaryAgent = gc.IsPrimaryAgent,
    //                        ModifiedOn = gc.ModifiedOn.Value,
    //                        PayeeName = gc.PayeeName,
                            
    //                    }).ToList();
    //        }
    //    }

    //    /// <summary>
    //    /// developer need to recheck and think of the requirement of this funciton.
    //    /// </summary>
    //    /// <param name="policyID"></param>
    //    /// <returns>returns all the outgoing payments related to a policyid given in the parameter.</returns>
    //    public static List<OutgoingShedule> GetOutgoingShedule(Guid policyId)
    //    {
    //        using (Entity.DataModel)
    //        {
    //            return (from gc in Entity.DataModel.PolicyOutgoingAdvancedSchedules
    //                       // .Include("MasterScheduleTypes").Include("CarrierCoverages")
    //                    where gc.PolicyId == policyId
    //                    select new OutgoingShedule
    //                    {
    //                        OutgoingAdvancedScheduleId = gc.OutgoingAdvancedScheduleId,
    //                        FromRange = gc.FromRange,
    //                        ToRange = gc.ToRange,
    //                        EffectiveFromDate = gc.EffectiveFromDate,
    //                        EffectiveToDate = gc.EffectiveToDate,
    //                        Rate = gc.Rate,
    //                        ScheduleTypeId = gc.ScheduleTypeId.Value,
    //                        PolicyId = gc.Policy.PolicyId,
    //                        PayeeUserCredentialId = gc.UserCredential.UserCredentialId,
    //                        IsPrimaryAgent=gc.IsPrimaryAgent,
    //                        ModifiedOn=gc.ModifiedOn.Value,
    //                        PayeeName = gc.PayeeName,

    //                    }).ToList();
    //        }
    //    }
    //}
}
