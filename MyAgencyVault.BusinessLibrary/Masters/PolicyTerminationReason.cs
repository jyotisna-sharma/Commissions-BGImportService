using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class PolicyTerminationReason
    {

        [DataMember]
        public int? TerminationReasonId { get; set; }
        [DataMember]
        public string TerminationReason { get; set; }

        public static List<PolicyTerminationReason> GetTerminationReasonList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from P in DataModel.MasterPolicyTerminationReasons
                        orderby P.Name
                        select new PolicyTerminationReason
                        {
                            TerminationReasonId = P.PTReasonId,
                            TerminationReason = P.Name,

                        }).ToList();
            }
        }

        public static List<PolicyTerminationReason> GetTerminationReasonListWithBlankAdded()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<PolicyTerminationReason> list = (from P in DataModel.MasterPolicyTerminationReasons
                                                      orderby P.Name
                                                      select new PolicyTerminationReason
                                                      {
                                                          TerminationReasonId = P.PTReasonId,
                                                          TerminationReason = P.Name,

                                                      }).ToList();
                list.Add(new PolicyTerminationReason { TerminationReasonId = null, TerminationReason = string.Empty });
                return list;
            }
        }
    }
}
