using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IIssueReasons
    {
        [OperationContract]
       IssueReasons GetReasons(int ReasonsID);
       // List<IssueReasons> GetReasons(int ReasonsID);
        [OperationContract]
        List<IssueReasons> GetAllReason();
    }
    public partial class MavService : IIssueReasons
    {
        public IssueReasons GetReasons(int ReasonsID)
        {
            return IssueReasons.GetReasons(ReasonsID);
        }

        public List<IssueReasons> GetAllReason()
        {
            return IssueReasons.GetAllReason();
        }
    }
}