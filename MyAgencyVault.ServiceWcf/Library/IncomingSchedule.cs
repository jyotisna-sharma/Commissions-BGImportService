using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IIncomingSchedule
    {    
        [OperationContract]
        void AddUpdatePolicyIncomingSchedule(PolicyIncomingSchedule schedule);

        [OperationContract]
        PolicyIncomingSchedule GetPolicyIncomingSchedule(Guid PolicyId);
    }

    public partial class MavService : IIncomingSchedule
    {
        public void AddUpdatePolicyIncomingSchedule(PolicyIncomingSchedule schedule)
        {
            IncomingSchedule.AddUpdatePolicySchedule(schedule);
        }

        public PolicyIncomingSchedule GetPolicyIncomingSchedule(Guid PolicyId)
        {
            return IncomingSchedule.GetPolicyIncomingSchedule(PolicyId);
        }
    }
}
