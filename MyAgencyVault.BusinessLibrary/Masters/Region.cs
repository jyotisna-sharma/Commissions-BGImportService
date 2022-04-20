using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DataAccessLayer.LinqtoEntity;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class Region
    {
        #region 
        [DataMember]
        public int RegionId { get; set; }
        [DataMember]
        public string RegionName { get; set; }
        #endregion 

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Region> GetRegionList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from r in DataModel.MasterPayorRegions
                        orderby r.SortOrder
                        select new Region
                        {
                            RegionId = r.PayorRegionId,
                            RegionName = r.Name
                        }).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Region GetRegion(int regId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from r in DataModel.MasterPayorRegions
                        where r.PayorRegionId == regId
                        select new Region
                        {
                            RegionId = r.PayorRegionId,
                            RegionName = r.Name
                        }).FirstOrDefault();
            }
        }
    }
}
