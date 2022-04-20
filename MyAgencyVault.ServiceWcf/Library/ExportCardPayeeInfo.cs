using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;


namespace MyAgencyVault.WcfService
{

    [ServiceContract]
    interface IExportCardPayeeInfo
    {
        [OperationContract]
        List<ExportCardPayeeInfo> fillExportCardPayeeInfo();
    }

    public partial class MavService : IExportCardPayeeInfo
    {
        public List<ExportCardPayeeInfo> fillExportCardPayeeInfo()
        {
            return null;
        }
    }
}