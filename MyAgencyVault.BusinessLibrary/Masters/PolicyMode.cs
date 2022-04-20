using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class PolicyMode
    {
        [DataMember]
        public int? ModeId { get; set; }
        [DataMember]
        public string Mode { get; set; }

        public static List<PolicyMode> GetPolicyModeList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from s in DataModel.MasterPolicyModes
                        orderby s.Name
                        select new PolicyMode
                        {
                            ModeId = s.PolicyModeId,
                            Mode = s.Name
                        }).ToList();
            }
        }

        public static List<PolicyMode> GetPolicyModeListWithBlankAdded()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<PolicyMode> list = (from s in DataModel.MasterPolicyModes
                                         orderby s.Name
                                         select new PolicyMode
                                         {
                                             ModeId = s.PolicyModeId,
                                             Mode = s.Name
                                         }).ToList();
                //list.Add(new PolicyMode { ModeId = null, Mode = string.Empty });
                return list;
            }
        }

        public static PolicyMode GetPolicyModeByID(int ModeID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from s in DataModel.MasterPolicyModes
                        where (s.PolicyModeId == ModeID)
                        select new PolicyMode
                        {
                            ModeId = s.PolicyModeId,
                            Mode = s.Name
                        }).ToList().FirstOrDefault();
            }
        }
    }
}
