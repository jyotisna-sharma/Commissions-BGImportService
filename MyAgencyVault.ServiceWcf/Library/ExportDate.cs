using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    public interface IExportDate
    {
        [OperationContract]
        ExportDate GetExportDate();
    }

    public partial class MavService : IExportDate
    {
        public ExportDate GetExportDate()
        {
            return ExportDate.getExportDate();
        }
    }
}