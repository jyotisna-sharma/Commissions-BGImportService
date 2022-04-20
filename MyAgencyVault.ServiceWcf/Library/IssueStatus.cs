using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IIssueStatus
    {
     
        #region IssueStatusBL
        [OperationContract]
        IssueStatus GetStatus(int StatusID);
       // List<IssueStatus> GetStatus(int StatusID);
        [OperationContract]
        List<IssueStatus> GetAllStatus();
        #endregion

    }
    public partial class MavService : IIssueStatus
    {
        #region IssueStatusBL
        //public List<IssueStatus> GetStatus(int StatusID)
        //{
        //    return IssueStatus.GetStatus(StatusID);
        //}
        public IssueStatus GetStatus(int StatusID)
        {
            return IssueStatus.GetStatus(StatusID);
        }

        public List<IssueStatus> GetAllStatus()
        {
            return IssueStatus.GetAllStatus();
        }
        #endregion
    }
}