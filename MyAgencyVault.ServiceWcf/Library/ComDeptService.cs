using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary.Masters;
using MyAgencyVault.BusinessLibrary;  

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IComDeptService
    {
        [OperationContract]
        void AddUpdateComDeptService(ComDeptService ComDptService);
        [OperationContract]
        void DeleteComDeptService(ComDeptService ComDptService);
        [OperationContract]
        ComDeptService GetComDeptService();
        [OperationContract]
        bool IsValidComDeptService(ComDeptService ComDptService);
        
    }
    public partial class MavService : IComDeptService 
    {


        public void AddUpdateComDeptService(ComDeptService ComDptService)
        {
            ComDptService.AddUpdate() ;
        }

        public void DeleteComDeptService(ComDeptService ComDptService)
        {
            ComDptService.Delete() ;
        }

        public ComDeptService GetComDeptService()
        {
            throw new NotImplementedException();
        }

        public bool IsValidComDeptService(ComDeptService ComDptService)
        {
            return  ComDptService.IsValid() ;
        }
    }
}