using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IIssueCategory
    {
        [OperationContract]
        IssueCategory GetCategory(int CategoryID);
       // List<IssueCategory> GetCategory(int CategoryID);
        
        [OperationContract]       
     List<IssueCategory> GetAllCategory();
     
    }
    public partial class MavService : IIssueCategory
    {
        //public List<IssueCategory> GetCategory(int CategoryID)
        //{
        //    return IssueCategory.GetCategory(CategoryID);
        //}
        public IssueCategory GetCategory(int CategoryID)
        {
            return IssueCategory.GetCategory(CategoryID);
        }
        public List<IssueCategory> GetAllCategory()
        {
            return IssueCategory.GetAllCategory();
        }
    }

}