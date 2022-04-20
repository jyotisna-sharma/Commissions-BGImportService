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
    interface IComDeptSupport
    {
        [OperationContract]
        List<ComDeptSupportData> getSupportFiles(ComDeptSupport comSupport);
    }

    public partial class MavService : IComDeptSupport
    {
        public List<ComDeptSupportData> getSupportFiles(ComDeptSupport comSupport)
        {
            return comSupport.GetSupportFiles();
        }
    }
}