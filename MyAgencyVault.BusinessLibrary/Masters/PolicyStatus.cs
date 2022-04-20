using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class PolicyStatus
    {
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }

        public static List<PolicyStatus> GetPolicyStatusList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from s in DataModel.MasterPolicyStatus
                        select new PolicyStatus
                        {
                            StatusId = s.PolicyStatusId,
                            Status = s.Name
                        }).ToList();
            }
        }
    }
}
