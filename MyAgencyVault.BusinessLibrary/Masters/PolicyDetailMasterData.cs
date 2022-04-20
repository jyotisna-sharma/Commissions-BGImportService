using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class PolicyDetailMasterData
    {
        [DataMember]
        public List<PolicyStatus> Statuses { get; set; }
        [DataMember]
        public List<PolicyTerminationReason> TerminationReasons { get; set; }
        [DataMember]
        public List<PolicyIncomingPaymentType> IncomingPaymentTypes { get; set; }
        [DataMember]
        public List<PolicyMode> Modes { get; set; }
        [DataMember]
        public List<IssueCategory> IssueCategories { get; set; }
        [DataMember]
        public List<IssueStatus> IssueStatuses { get; set; }
        [DataMember]
        public List<IssueReasons> IssueReasons { get; set; }
        [DataMember]
        public List<IssueResults> IssueResults { get; set; }
        [DataMember]
        public List<PolicyIncomingScheduleType> IncomingAdvanceScheduleTypes { get; set; }
        [DataMember]
        public List<PolicyOutgoingScheduleType> OutgoingAdvanceScheduleTypes { get; set; }
        [DataMember]
        public List<PolicyIncomingPaymentType> LearnedMasterIncomingPaymentTypes { get; set; }
        [DataMember]
        public List<PolicyMode> LearnedMasterPaymentsModes { get; set; }
    }
}
