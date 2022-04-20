using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPolicyToLearnPost
    {
         [OperationContract]
        void AddUpdatPolicyToLearn(Guid PolicyId);        
    }
    public partial class MavService : IPolicyToLearnPost
    {
        public void AddUpdatPolicyToLearn(Guid PolicyId)
        {
            PolicyToLearnPost.AddUpdatPolicyToLearn(PolicyId);
        }
    }
}