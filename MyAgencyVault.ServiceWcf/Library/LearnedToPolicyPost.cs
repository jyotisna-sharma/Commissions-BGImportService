using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ILearnedToPolicyPost
    {
        [OperationContract]
        void AddLearnedToPolicy(Guid PolicyId);

    }
    public partial class MavService : ILearnedToPolicyPost
    {
        public void AddLearnedToPolicy(Guid PolicyId)
        {
            LearnedToPolicyPost.AddUpdateLearnedToPolicy(PolicyId);
        }

    }
}