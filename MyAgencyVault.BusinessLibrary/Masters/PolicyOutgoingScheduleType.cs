using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class PolicyOutgoingScheduleType
    {
        #region
        [DataMember]
        public int ScheduleTypeId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        #endregion 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<PolicyOutgoingScheduleType> GetOutgoingScheduleTypeList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from s in DataModel.MasterScheduleTypes
                        orderby s.Name
                        select new PolicyOutgoingScheduleType
                        {
                            Name = s.Name,
                            ScheduleTypeId = s.ScheduleTypeId,
                            Description = s.Description

                        }).ToList();
            }
        }
    }
}
