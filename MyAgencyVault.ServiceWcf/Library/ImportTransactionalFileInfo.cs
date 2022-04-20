using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;


namespace MyAgencyVault.WcfService
{

    [ServiceContract]
    interface IImportTransactionFile
    {
        [OperationContract]
        void fillImportInfo(string FileName, List<string> transactionLines);
    }

    public partial class MavService : IImportTransactionFile
    {
        public void fillImportInfo(string FileName, List<string> transactionLines)
        {
            ImportTransactionFile.fillImportTransactionFileInfo(FileName, transactionLines);
        }
    }
}