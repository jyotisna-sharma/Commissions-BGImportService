using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ILicenseeInvoice
    {
        [OperationContract]
        List<LicenseeInvoice> getAllInvoice();
        [OperationContract]
        LicenseeInvoice getInvoiceByID(long Id);
        [OperationContract]
        string getExportBatchName(Guid? ExportedBatchId);
        [OperationContract]
        DateTime? getLatestBillingDate();
    }
    public partial class MavService : ILicenseeInvoice
    {
        public List<LicenseeInvoice> getAllInvoice()
        {
            return LicenseeInvoiceHelper.getAllInvoice();
        }

        public LicenseeInvoice getInvoiceByID(long Id)
        {
            return LicenseeInvoiceHelper.getInvoiceByID(Id);
        }

        public string getExportBatchName(Guid? ExportedBatchId)
        {
            return LicenseeInvoiceHelper.getExportBatchName(ExportedBatchId);
        }

        public DateTime? getLatestBillingDate()
        {
            return LicenseeInvoiceHelper.getLatestBillingDate();
        }
    }
}