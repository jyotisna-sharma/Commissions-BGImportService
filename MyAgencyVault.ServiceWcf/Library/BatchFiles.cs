using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;


namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IBatchFiles
    {
        [OperationContract]
        List<BatchFiles> fillBatchFilesData();
        [OperationContract]
        bool DeleteBatchFile(BatchFiles batchFile);
    }

    public partial class MavService : IBatchFiles
    {
        public List<BatchFiles> fillBatchFilesData()
        {
            return BatchFiles.fillBatchFilesData();
        }

        public bool DeleteBatchFile(BatchFiles batchFile)
        {
            return BatchFiles.DeleteBatchFile(batchFile);
        }
    }
}