using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IIssueResults
    {
        #region IssueResultBL
        [OperationContract]
        IssueResults GetResults(int ResultsID);
        //List<IssueResults> GetResults(int ResultsID);
         [OperationContract]
         List<IssueResults> GetAllResults();
        #endregion

    }
    public partial class MavService : IIssueResults
    {
        //public List<IssueResults> GetResults(int ResultesID)
        //{
        //    return IssueResults.GetResults(ResultesID);
        //}
        public IssueResults GetResults(int ResultesID)
        {
            return IssueResults.GetResults(ResultesID);
        }
        public List<IssueResults> GetAllResults()
        {
            return IssueResults.GetAllResults();
        }
    }
}