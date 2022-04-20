using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{

    [ServiceContract]
    interface IOutgoingSchedule
    {
        [OperationContract]
        void AddUpdateOutgoingShedule(PolicyOutgoingSchedule outgoingSchedule);
        
        [OperationContract]
        PolicyOutgoingSchedule GetOutgoingSheduleBy(Guid policyId);
    }

    public partial class MavService : IOutgoingSchedule
    {
        public void AddUpdateOutgoingShedule(PolicyOutgoingSchedule outgoingSchedule)
        {
            OutgoingSchedule.AddUpdatePolicyOutgoingSchedule(outgoingSchedule);
        }

        public PolicyOutgoingSchedule GetOutgoingSheduleBy(Guid policyId)
        {
            return OutgoingSchedule.GetPolicyOutgoingSchedule(policyId);
        }
    }
}