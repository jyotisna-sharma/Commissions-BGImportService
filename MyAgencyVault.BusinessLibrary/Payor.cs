using System;
using System.Collections.Generic;
using System.Linq;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DataAccessLayer.LinqtoEntity;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Collections.ObjectModel;
using System.Data.Objects;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public enum PayorStatus
    {
        [EnumMember]
        Active = 1,
        [EnumMember]
        InActive = 2,
        [EnumMember]
        All = 3
    }

    [DataContract]
    public class DisplayedPayor
    {
        [DataMember]
        public Guid PayorID { get; set; }
        [DataMember]
        public string PayorName { get; set; }
        [DataMember]
        public string NickName { get; set; }
        [DataMember]
        public int PayorTypeID { get; set; }


        public static List<DisplayedPayor> GetPayors()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DisplayedPayor> lstPayors = null;

                lstPayors = (from p in DataModel.Payors
                             orderby p.PayorName
                             select new DisplayedPayor
                             {
                                 PayorID = p.PayorId,
                                 PayorName = p.PayorName,
                                 PayorTypeID = p.PayorTypeId.Value,
                                 NickName = p.NickName
                             }).ToList();


                return lstPayors;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <returns></returns>
        public static List<DisplayedPayor> GetPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DisplayedPayor> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     orderby p.PayorName 
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }
                return lstPayors;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <returns></returns>
        public static int GetPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<int> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     select p.PayorStatusId.Value).ToList();
                    }
                }

                if (lstPayors == null)
                    return 0;
                else
                    return lstPayors.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <param name="PayerfillInfo"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static List<DisplayedPayor> GetPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take)
        {
           // ActionLogger.Logger.WriteImportPolicyLog("Filling Payor Begin: " + DateTime.Now.ToLongTimeString(), true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DisplayedPayor> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     orderby p.PayorName
                                     select new DisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }

                lstPayors = lstPayors.Skip(skip).Take(take).ToList();
               // ActionLogger.Logger.WriteImportPolicyLog("Filling Payor End: " + DateTime.Now.ToLongTimeString(), true);
                return lstPayors;
            }
        }
    }

    [DataContract]
    public class ConfigDisplayedPayor
    {
        [DataMember]
        public Guid PayorID { get; set; }
        [DataMember]
        public string PayorName { get; set; }
        [DataMember]
        public string NickName { get; set; }
        [DataMember]
        public int RegionID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <returns></returns>
        public static List<ConfigDisplayedPayor> GetPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<ConfigDisplayedPayor> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }
                return lstPayors;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <returns></returns>
        public static int GetPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<int> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     select p.PayorStatusId.Value).ToList();
                    }
                }

                if (lstPayors == null)
                    return 0;
                else
                    return lstPayors.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <param name="PayerfillInfo"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static List<ConfigDisplayedPayor> GetPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take)
        {
            // ActionLogger.Logger.WriteImportPolicyLog("Filling Payor Begin: " + DateTime.Now.ToLongTimeString(), true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<ConfigDisplayedPayor> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     select new ConfigDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName
                                     }).ToList();
                    }
                }

                lstPayors = lstPayors.Skip(skip).Take(take).ToList();
                return lstPayors;
            }
        }
    }

    [DataContract]
    public class SettingDisplayedPayor
    {
        [DataMember]
        public Guid PayorID { get; set; }
        [DataMember]
        public string PayorName { get; set; }
        [DataMember]
        public string NickName { get; set; }
        [DataMember]
        public int RegionID { get; set; }
        [DataMember]
        public int SourceType { get; set; }
        [DataMember]
        public bool IsGlobal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <returns></returns>
        public static List<SettingDisplayedPayor> GetPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<SettingDisplayedPayor> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID??0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }
                return lstPayors;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <returns></returns>
        public static int GetPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<int> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     select p.PayorStatusId.Value).ToList();
                    }
                }

                if (lstPayors == null)
                    return 0;
                else
                    return lstPayors.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <param name="PayerfillInfo"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static List<SettingDisplayedPayor> GetPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take)
        {
            // ActionLogger.Logger.WriteImportPolicyLog("Filling Payor Begin: " + DateTime.Now.ToLongTimeString(), true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<SettingDisplayedPayor> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID??0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     select new SettingDisplayedPayor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         RegionID = p.PayorRegionId.Value,
                                         NickName = p.NickName,
                                         SourceType = p.SourceID ?? 0,
                                         IsGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }

                lstPayors = lstPayors.Skip(skip).Take(take).ToList();
                return lstPayors;
            }
        }
    }

    [DataContract]
    public class Payor
    {
        #region "Public Properties"

        [DataMember]
        public Guid PayorID { get; set; }
        [DataMember]
        public string PayorName { get; set; }
        [DataMember]
        public string NickName { get; set; }
        [DataMember]
        public int PayorRegionID { get; set; }
        [DataMember]
        public Region Region { get; set; }
        [DataMember]
        public int PayorTypeID { get; set; }
        [DataMember]
        public bool ISGlobal { get; set; }
        [DataMember]
        public Int32? SourceType { get; set; }
        [DataMember]
        public int StatusID { get; set; }
        [DataMember]
        public Guid UserID { get; set; }
        [DataMember]
        public Guid? PayorLicensee { get; set; }// ARG--3Jan2011
        [DataMember]
        public List<PayorSiteLoginInfo> UserWebSiteInfo
        { get; set; }
        [DataMember]
        public PayorDefaults DefaultInfo { get; set; }
        [DataMember]
        public List<GlobalPayorContact> Contacts
        { get; set; }
        [DataMember]
        public List<Carrier> Carriers
        { get; set; }
        [DataMember]
        public IncomingSchedule IncomingScheduleInfo { get; set; }
        [DataMember]
        public List<string> IssuedFiles { get; set; }

        #endregion

        /// <summary>
        /// to do : need to verify the referenced licensee entry.
        /// </summary>
        public ReturnStatus AddUpdateDelete(Operation operationType)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                ReturnStatus status = null;
                if (operationType == Operation.Add)
                {
                    status = ValidatePayor(DataModel, Operation.Add);
                    if (!status.IsError)
                    {
                        DLinq.Payor existingPayor = (from p in DataModel.Payors where p.PayorId == this.PayorID && p.IsDeleted == false select p).FirstOrDefault();
                        if (existingPayor != null)
                        {
                            existingPayor.MasterPayorRegionReference.Value = ReferenceMaster.GetReferencedRegion(this.PayorRegionID, DataModel);
                            existingPayor.MasterSourceTypeReference.Value = ReferenceMaster.GetReferencedSourceType(Convert.ToInt16(this.SourceType), DataModel);
                            existingPayor.IsGlobal = this.ISGlobal;
                            existingPayor.PayorStatusId = this.StatusID;
                            existingPayor.PayorTypeId = this.PayorTypeID;
                            existingPayor.NickName = this.NickName;
                            existingPayor.PayorName = this.PayorName;
                        }
                        else
                        {
                            existingPayor = DLinq.Payor.CreatePayor(this.PayorID, false, false);
                            existingPayor.MasterPayorRegionReference.Value = ReferenceMaster.GetReferencedRegion(this.PayorRegionID, DataModel); ;
                            existingPayor.MasterSourceTypeReference.Value = ReferenceMaster.GetReferencedSourceType(Convert.ToInt16(this.SourceType), DataModel);
                            existingPayor.PayorStatusId = this.StatusID;
                            existingPayor.PayorTypeId = this.PayorTypeID;
                            existingPayor.PayorName = this.PayorName;
                            existingPayor.IsGlobal = this.ISGlobal;
                            existingPayor.LicenseeId = (this.PayorLicensee == Guid.Empty ? null : this.PayorLicensee);
                            existingPayor.CreatedBy = this.UserID;
                            existingPayor.NickName = this.NickName;
                            existingPayor.CreatedOn = DateTime.Now;
                            DataModel.AddToPayors(existingPayor);
                        }
                        DataModel.SaveChanges();
                    }
                    //Send mail to benefits email address
                    string mailBody = "<html>Hi,<br><br>New Payor has been added to Commissions Department with following details:<br><br><Table><tr> <td>Payor ID: </td><td> " +  this.PayorID + "</td>"+
                        "</tr><tr> <td>Payor Name: </td><td> "+ this.PayorName  +"</td></tr> </table><br><br>Regards,<br>Commissions Department </html>";
                    MailServerDetail.SendMailToBenefits("Payor", mailBody);
                }
                else if (operationType == Operation.Upadte)
                {
                    status = ValidatePayor(DataModel, Operation.Upadte);
                    if (!status.IsError)
                    {
                        DLinq.Payor payor = DataModel.Payors.FirstOrDefault(s => s.PayorId == this.PayorID && s.IsDeleted == false);
                        if (payor != null)
                        {
                            payor.PayorName = this.PayorName;
                            payor.NickName = this.NickName;
                            payor.PayorStatusId = this.StatusID;
                            payor.PayorRegionId = this.PayorRegionID;
                            payor.PayorTypeId = this.PayorTypeID;
                        }
                        DataModel.SaveChanges();
                    }
                }
                else if (operationType == Operation.Delete)
                {
                    status = ValidatePayor(DataModel, Operation.Delete);
                    if (!status.IsError)
                    {
                        DLinq.Payor _Payor = (from p in DataModel.Payors where p.PayorId == this.PayorID && p.IsDeleted == false select p).FirstOrDefault();
                        if (_Payor != null)
                        {
                            _Payor.IsDeleted = true;
                            DataModel.SaveChanges();
                        }
                    }
                }
                else
                {
                    status = new ReturnStatus { ErrorMessage = "Undefine payor operation.", IsError = true };
                }
                return status;
            }
        }

        public static ReturnStatus DeletePayor(Guid PayorId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.Payor payor = DataModel.Payors.FirstOrDefault(s => s.PayorId == PayorId);
                Payor delPayor = new Payor { PayorID = payor.PayorId };
                return delPayor.AddUpdateDelete(Operation.Delete);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <returns></returns>
        public static List<Payor> GetPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            // ActionLogger.Logger.WriteImportPolicyLog("Filling Payor Begin: " + DateTime.Now.ToLongTimeString(), true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<Payor> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }

                List<Region> regions = new List<Region>();
                foreach (Payor p in lstPayors)
                {
                    if (regions.Exists(s => s.RegionId == p.PayorRegionID))
                        p.Region = regions.FirstOrDefault(s => s.RegionId == p.PayorRegionID);
                    else
                    {
                        p.Region = DataModel.MasterPayorRegions.Where(s => s.PayorRegionId == p.PayorRegionID).Select(s => new Region { RegionId = s.PayorRegionId, RegionName = s.Name }).First();
                        regions.Add(p.Region);
                    }

                    if (PayerfillInfo != null && PayerfillInfo.IsCarriersRequired)
                        p.Carriers = Carrier.GetPayorCarriers(p.PayorID, PayerfillInfo.IsCoveragesRequired);
                    else
                        p.Carriers = null;

                    if (PayerfillInfo != null && PayerfillInfo.IsWebsiteLoginsRequired)
                        p.UserWebSiteInfo = PayorSiteLoginInfo.GetPayorSiteLogins(p.PayorID);
                    else
                        p.UserWebSiteInfo = null;

                    if (PayerfillInfo != null && PayerfillInfo.IsContactsRequired)
                        p.Contacts = GlobalPayorContact.getContacts(p.PayorID);
                    else
                        p.Contacts = null;

                    DLinq.Payor tmpPayor = DataModel.Payors.FirstOrDefault(s => s.PayorId == p.PayorID);

                    if (tmpPayor != null && tmpPayor.Batches != null)
                        p.IssuedFiles = tmpPayor.Batches.Where(s => s.EntryStatusId == 3).Select(s => s.FileName).ToList();
                    else
                        p.IssuedFiles = null;
                }

                // ActionLogger.Logger.WriteImportPolicyLog("Filling Payor End: " + DateTime.Now.ToLongTimeString(), true);
                return lstPayors;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <returns></returns>
        public static int GetPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<int> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     select p.PayorStatusId.Value
                                     ).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     select p.PayorStatusId.Value).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     select p.PayorStatusId.Value).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     select p.PayorStatusId.Value).ToList();
                    }
                }

                if (lstPayors == null)
                    return 0;
                else
                    return lstPayors.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <param name="PayerfillInfo"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static List<Payor> GetPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take)
        {
            // ActionLogger.Logger.WriteImportPolicyLog("Filling Payor Begin: " + DateTime.Now.ToLongTimeString(), true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<Payor> lstPayors = null;
                PayorStatus status = PayerfillInfo.PayorStatus;

                if (LicenseeId.HasValue && LicenseeId.Value != Guid.Empty)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 0
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId)) && p.PayorStatusId == 1
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && ((p.IsGlobal == true) || (p.LicenseeId == LicenseeId))
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }
                else if (LicenseeId == null)
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 0
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && p.PayorStatusId == 1
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false)
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }
                else
                {
                    if (status == PayorStatus.Active)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 0
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else if (status == PayorStatus.InActive)
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true) && p.PayorStatusId == 1
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                    else
                    {
                        lstPayors = (from p in DataModel.Payors
                                     where (p.IsDeleted == false) && (p.IsGlobal == true)
                                     select new Payor
                                     {
                                         PayorID = p.PayorId,
                                         PayorName = p.PayorName,
                                         PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                                         PayorTypeID = p.PayorTypeId.Value,
                                         SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                                         StatusID = p.PayorStatusId.Value,
                                         PayorLicensee = p.UserCredential.LicenseeId,//ARG--
                                         UserID = p.CreatedBy.Value,
                                         NickName = p.NickName,
                                         ISGlobal = p.IsGlobal
                                     }).ToList();
                    }
                }

                lstPayors = lstPayors.Skip(skip).Take(take).ToList();

                List<Region> regions = new List<Region>();
                foreach (Payor p in lstPayors)
                {
                    if (regions.Exists(s => s.RegionId == p.PayorRegionID))
                        p.Region = regions.FirstOrDefault(s => s.RegionId == p.PayorRegionID);
                    else
                    {
                        p.Region = DataModel.MasterPayorRegions.Where(s => s.PayorRegionId == p.PayorRegionID).Select(s => new Region { RegionId = s.PayorRegionId, RegionName = s.Name }).First();
                        regions.Add(p.Region);
                    }

                    if (PayerfillInfo != null && PayerfillInfo.IsCarriersRequired)
                        p.Carriers = Carrier.GetPayorCarriers(p.PayorID, PayerfillInfo.IsCoveragesRequired);
                    else
                        p.Carriers = null;

                    if (PayerfillInfo != null && PayerfillInfo.IsWebsiteLoginsRequired)
                        p.UserWebSiteInfo = PayorSiteLoginInfo.GetPayorSiteLogins(p.PayorID);
                    else
                        p.UserWebSiteInfo = null;

                    if (PayerfillInfo != null && PayerfillInfo.IsContactsRequired)
                        p.Contacts = GlobalPayorContact.getContacts(p.PayorID);
                    else
                        p.Contacts = null;

                    DLinq.Payor tmpPayor = DataModel.Payors.FirstOrDefault(s => s.PayorId == p.PayorID);

                    if (tmpPayor != null && tmpPayor.Batches != null)
                        p.IssuedFiles = tmpPayor.Batches.Where(s => s.EntryStatusId == 3).Select(s => s.FileName).ToList();
                    else
                        p.IssuedFiles = null;
                }

                //ActionLogger.Logger.WriteImportPolicyLog("Filling Payor End: " + DateTime.Now.ToLongTimeString(), true);
                return lstPayors;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PayorId"></param>
        /// <returns></returns>
        public static Payor GetPayorByID(Guid PayorId)
        {
            if (PayorId == Guid.Empty)
                return null;

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                Payor payor = null;

                payor = (from p in DataModel.Payors
                         where ((p.IsDeleted == false) && (p.PayorId == PayorId))
                         select new Payor
                         {
                             PayorID = p.PayorId,
                             NickName = p.NickName,
                             PayorName = p.PayorName,
                             PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                             PayorTypeID = p.PayorTypeId.Value,
                             SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                             StatusID = p.PayorStatusId.Value,
                             ISGlobal = p.IsGlobal,
                             UserID = p.CreatedBy.Value,
                             PayorLicensee = p.UserCredential.LicenseeId,
                         }).Single();

                payor.UserWebSiteInfo = null;
                payor.Contacts = null;
                payor.Carriers = null;
                payor.IssuedFiles = null;

                return payor;
            }
        }

        public static Payor GetPayorIDbyNickName(string NickName)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                Payor payor = null;

                payor = (from p in DataModel.Payors
                         where ((p.IsDeleted == false) && (p.NickName.ToLower() == NickName.ToLower()))
                         select new Payor
                         {
                             PayorID = p.PayorId,
                             NickName = p.NickName,
                             PayorName = p.PayorName,
                             PayorRegionID = p.MasterPayorRegion.PayorRegionId,
                             PayorTypeID = p.PayorTypeId.Value,
                             SourceType = (int?)p.MasterSourceType.SourceTypeId ?? 0,
                             StatusID = p.PayorStatusId.Value,
                             ISGlobal = p.IsGlobal,
                             UserID = p.CreatedBy.Value,
                             PayorLicensee = p.UserCredential.LicenseeId,
                         }).Single();

                payor.UserWebSiteInfo = null;
                payor.Contacts = null;
                payor.Carriers = null;
                payor.IssuedFiles = null;

                return payor;
            }
        }

        /// <summary>
        /// This function is used to validate the payor for system.
        /// </summary>
        /// <returns></returns>
        private ReturnStatus ValidatePayor(DLinq.CommissionDepartmentEntities DataModel, Operation operation)
        {
            ReturnStatus retStatus = new ReturnStatus();

            if (operation == Operation.Add)
            {
                DLinq.Payor payor = null;

                if (this.PayorLicensee == Guid.Empty) //Check Global payor which is available in all lincense 
                    payor = DataModel.Payors.FirstOrDefault(s => s.PayorName.ToUpper() == this.PayorName.ToUpper() && s.IsGlobal == true && s.IsDeleted == false);
                else //Check Local payor which is available in lincense 
                    payor = DataModel.Payors.FirstOrDefault(s => s.PayorName.ToUpper() == this.PayorName.ToUpper() && s.IsDeleted == false && (s.IsGlobal || s.LicenseeId == this.PayorLicensee));

                if (payor != null)
                {
                    retStatus.IsError = true;
                    retStatus.ErrorMessage = "Payor with specified name is already present. Please enter other name.";
                }
                else
                {
                    //payor = DataModel.Payors.FirstOrDefault(s => s.NickName == this.NickName && s.IsDeleted == false);
                    if (this.PayorLicensee == Guid.Empty) //Check Global payor which is available in all lincense 
                        payor = DataModel.Payors.FirstOrDefault(s => s.NickName == this.NickName && s.IsGlobal == true && s.IsDeleted == false);
                    else //Check Local payor which is available in lincense 
                        payor = DataModel.Payors.FirstOrDefault(s => s.NickName.ToUpper() == this.NickName.ToUpper() && s.IsDeleted == false && (s.IsGlobal || s.LicenseeId == this.PayorLicensee));
                    if (payor != null)
                    {
                        retStatus.IsError = true;
                        retStatus.ErrorMessage = "Payor with specified nickname is already present. Please enter other nickname.";
                    }
                }


                //Old

                //if (this.PayorLicensee == Guid.Empty) //Check Global payor which is available in all lincense 
                //    payor = DataModel.Payors.FirstOrDefault(s => s.PayorName.ToUpper() == this.PayorName.ToUpper() && s.IsDeleted == false);
                //else //Check Local payor which is available in lincense 
                //    payor = DataModel.Payors.FirstOrDefault(s => s.PayorName.ToUpper() == this.PayorName.ToUpper() && s.IsDeleted == false && (s.IsGlobal || s.LicenseeId == this.PayorLicensee));

                //if (payor != null)
                //{
                //    retStatus.IsError = true;
                //    retStatus.ErrorMessage = "Payor with specified name is already present. Please enter other name.";
                //}
                //else
                //{
                //    //payor = DataModel.Payors.FirstOrDefault(s => s.NickName == this.NickName && s.IsDeleted == false);
                //    if (this.PayorLicensee == Guid.Empty) //Check Global payor which is available in all lincense 
                //        payor = DataModel.Payors.FirstOrDefault(s => s.NickName == this.NickName && s.IsDeleted == false);
                //    else //Check Local payor which is available in lincense 
                //        payor = DataModel.Payors.FirstOrDefault(s => s.NickName.ToUpper() == this.NickName.ToUpper() && s.IsDeleted == false && (s.IsGlobal || s.LicenseeId == this.PayorLicensee));
                //    if (payor != null)
                //    {
                //        retStatus.IsError = true;
                //        retStatus.ErrorMessage = "Payor with specified nickname is already present. Please enter other nickname.";
                //    }
                //}
            }
            else if (operation == Operation.Upadte)
            {
                //int count = DataModel.Payors.Where(s => s.NickName == this.NickName && s.IsDeleted == false).ToList().Count;
                int count = 0;
                if (this.PayorLicensee == Guid.Empty) //Check Global payor which is available in all lincense 
                    count = count = DataModel.Payors.Where(s => s.NickName.ToUpper() == this.NickName.ToUpper() && s.PayorId != this.PayorID && s.IsGlobal == true && s.IsDeleted == false).ToList().Count;
                else //Check Local payor which is available in lincense 
                    count = DataModel.Payors.Where(s => s.NickName.ToUpper() == this.NickName.ToUpper() && s.PayorId != this.PayorID && s.IsDeleted == false && (s.IsGlobal || s.LicenseeId == this.PayorLicensee)).ToList().Count;
                if (count > 0)
                {
                    retStatus.IsError = true;
                    retStatus.ErrorMessage = "This payor nickname is already present. Please select other.";
                }
                //count = DataModel.Payors.Where(s => s.PayorName == this.PayorName && s.IsDeleted == false).ToList().Count;
                if (this.PayorLicensee == Guid.Empty) //Check Global payor which is available in all lincense 
                    count = count = DataModel.Payors.Where(s => s.PayorName.ToUpper() == this.PayorName.ToUpper() && s.PayorId != this.PayorID && s.IsGlobal == true && s.IsDeleted == false).ToList().Count;
                else //Check Local payor which is available in lincense 
                    count = DataModel.Payors.Where(s => s.PayorName.ToUpper() == this.PayorName.ToUpper() && s.PayorId != this.PayorID && s.IsDeleted == false && (s.IsGlobal || s.LicenseeId == this.PayorLicensee)).ToList().Count;
                if (count > 0)
                {
                    retStatus.IsError = true;
                    retStatus.ErrorMessage = "This payor name is already present. Please select other.";
                }

                ////int count = DataModel.Payors.Where(s => s.NickName == this.NickName && s.IsDeleted == false).ToList().Count;
                //int count = 0;
                //if (this.PayorLicensee == Guid.Empty) //Check Global payor which is available in all lincense 
                //    count = count = DataModel.Payors.Where(s => s.NickName.ToUpper() == this.NickName.ToUpper() && s.PayorId != this.PayorID && s.IsDeleted == false).ToList().Count;
                //else //Check Local payor which is available in lincense 
                //    count = DataModel.Payors.Where(s => s.NickName.ToUpper() == this.NickName.ToUpper() && s.PayorId != this.PayorID && s.IsDeleted == false && (s.IsGlobal || s.LicenseeId == this.PayorLicensee)).ToList().Count;
                //if (count > 0)
                //{
                //    retStatus.IsError = true;
                //    retStatus.ErrorMessage = "This payor nickname is already present. Please select other.";
                //}
                ////count = DataModel.Payors.Where(s => s.PayorName == this.PayorName && s.IsDeleted == false).ToList().Count;
                //if (this.PayorLicensee == Guid.Empty) //Check Global payor which is available in all lincense 
                //    count = count = DataModel.Payors.Where(s => s.PayorName.ToUpper() == this.PayorName.ToUpper() && s.PayorId != this.PayorID && s.IsDeleted == false).ToList().Count;
                //else //Check Local payor which is available in lincense 
                //    count = DataModel.Payors.Where(s => s.PayorName.ToUpper() == this.PayorName.ToUpper() && s.PayorId != this.PayorID &&s.IsDeleted == false && (s.IsGlobal || s.LicenseeId == this.PayorLicensee)).ToList().Count;
                //if (count > 0)
                //{
                //    retStatus.IsError = true;
                //    retStatus.ErrorMessage = "This payor name is already present. Please select other.";
                //}
            }
            else if (operation == Operation.Delete)
            {
                DLinq.Policy policy = (from po in DataModel.Policies
                                       where (po.PayorId == this.PayorID
                                       && po.IsDeleted == false)
                                       select po).FirstOrDefault();
                if (policy != null)
                {
                    retStatus.IsError = true;
                    retStatus.ErrorMessage = "Some policy refer this payor. You can not delete payor without deleting all the policies that refer this payor.";
                }

                DLinq.PayorTool payorTool = (from po in DataModel.PayorTools
                                             where (po.PayorId == this.PayorID
                                             && po.IsDeleted == false)
                                             select po).FirstOrDefault();

                if (payorTool != null)
                {
                    retStatus.IsError = true;
                    retStatus.ErrorMessage = "You can not delete this payor without first deleting the payor tool for this payor.";
                }
            }

            return retStatus;
        }

        public ReturnStatus ValdateLocalPayor(string strPayorName, string strNickName)
        {
            ReturnStatus retStatus = new ReturnStatus();
            DLinq.Payor payor = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                payor = DataModel.Payors.FirstOrDefault(s => s.PayorName.ToUpper() == strPayorName.ToUpper() && s.IsDeleted == false && s.IsGlobal == false && s.LicenseeId != null);

                if (payor != null)
                {
                    retStatus.IsError = true;
                    if (payor.Licensee != null)
                        retStatus.ErrorMessage = "Payor:  " + strPayorName + "   is already present in agency: " + payor.Licensee.Company.ToString();
                    else
                        retStatus.ErrorMessage = "Payor:  " + strPayorName + "   is already present in agency";

                }
                else
                {
                    //payor = DataModel.Payors.FirstOrDefault(s => s.NickName == this.NickName && s.IsDeleted == false);                    
                    payor = DataModel.Payors.FirstOrDefault(s => s.NickName.ToUpper() == strNickName.ToUpper() && s.IsDeleted == false && s.IsGlobal == false && s.LicenseeId != null);

                    if (payor != null)
                    {
                        retStatus.IsError = true;
                        retStatus.ErrorMessage = "Payor nick name:  " + strNickName + "   is already present in agency: " + payor.Licensee.Company.ToString();
                    }


                    //ReturnStatus retStatus = new ReturnStatus();
                    //DLinq.Payor payor = null;
                    //using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                    //{
                    //    if (this.PayorLicensee == Guid.Empty) //Check Global payor which is available in all lincense 
                    //        payor = DataModel.Payors.FirstOrDefault(s => s.PayorName.ToUpper() == strPayorName.ToUpper() && s.IsDeleted == false);
                    //    else //Check Local payor which is available in lincense 
                    //        payor = DataModel.Payors.FirstOrDefault(s => s.PayorName.ToUpper() == strPayorName.ToUpper() && s.IsDeleted == false && (s.IsGlobal || s.LicenseeId == this.PayorLicensee));

                    //    if (payor != null)
                    //    {
                    //        retStatus.IsError = true;
                    //        if (payor.Licensee != null)
                    //            retStatus.ErrorMessage = "Payor:  " + strPayorName + "   is already present in agency: " + payor.Licensee.Company.ToString();
                    //        else
                    //            retStatus.ErrorMessage = "Payor:  " + strPayorName + "   is already present in agency";

                    //    }
                    //    else
                    //    {
                    //        //payor = DataModel.Payors.FirstOrDefault(s => s.NickName == this.NickName && s.IsDeleted == false);
                    //        if (this.PayorLicensee == Guid.Empty) //Check Global payor which is available in all lincense 
                    //            payor = DataModel.Payors.FirstOrDefault(s => s.NickName.ToUpper() == strNickName.ToUpper() && s.IsDeleted == false);
                    //        else //Check Local payor which is available in lincense 
                    //            payor = DataModel.Payors.FirstOrDefault(s => s.NickName.ToUpper() == strNickName.ToUpper() && s.IsDeleted == false && (s.IsGlobal || s.LicenseeId == this.PayorLicensee));
                    //        if (payor != null)
                    //        {
                    //            retStatus.IsError = true;
                    //            retStatus.ErrorMessage = "Payor nick name:  " + strNickName + "   is already present in agency: " + payor.Licensee.Company.ToString();
                    //        }
                    //    }
                    //}

                }
                return retStatus;
            }
        }


        public static List<PayorObject> GetPayorsOnDate(DateTime dtStart, DateTime dtEnd)
        {
            List<PayorObject> objList = new List<PayorObject>();

            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetPayorsOnDate starts:  ", true);
                if (dtStart == DateTime.MinValue && dtEnd == DateTime.MinValue)
                {
                    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                    {
                        var payor = (from pl in DataModel.Payors
                                     where pl.IsDeleted != true
                                     select new { pl.PayorId, pl.PayorName, pl.NickName }).ToList();

                        foreach (var p in payor)
                        {
                            PayorObject objPayor = new PayorObject();
                            objPayor.PayorID = p.PayorId;
                            objPayor.PayorName = p.PayorName;
                            objPayor.NickName = p.NickName;
                            objList.Add(objPayor);
                        }
                    }
                }
                else
                {
                    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                    {
                        var payor = (from pl in DataModel.Payors
                                     where pl.IsDeleted != true && (pl.CreatedOn.HasValue && EntityFunctions.TruncateTime(pl.CreatedOn) > dtStart && EntityFunctions.TruncateTime(pl.CreatedOn) <= dtEnd)
                                     select new { pl.PayorId, pl.PayorName, pl.NickName }).ToList();

                        foreach (var p in payor)
                        {
                            PayorObject objPayor = new PayorObject();
                            objPayor.PayorID = p.PayorId;
                            objPayor.PayorName = p.PayorName;
                            objPayor.NickName = p.NickName;
                            objList.Add(objPayor);
                        }
                    }
                }
                ActionLogger.Logger.WriteImportPolicyLog("GetPayorsOnDate - completed successfully, records - :  " + objList.Count, true);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetPayorsOnDate - exception:  " + ex.Message, true);
            }
            return objList;
        }

    }

    [DataContract]
    public class PayorFillInfo
    {
        [DataMember]
        public bool IsCarriersRequired { get; set; }
        [DataMember]
        public bool IsCoveragesRequired { get; set; }
        [DataMember]
        public bool IsWebsiteLoginsRequired { get; set; }
        [DataMember]
        public bool IsContactsRequired { get; set; }
        [DataMember]
        public PayorStatus PayorStatus { get; set; }
    }
}
