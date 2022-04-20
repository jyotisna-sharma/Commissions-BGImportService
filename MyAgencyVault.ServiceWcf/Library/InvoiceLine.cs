using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IInvoiceLine
    {
        [OperationContract]
        InvoiceLine getInvoiceLines(long invoiceId, bool includePolicyData);
        [OperationContract]
        List<InvoiceLineJournalData> getInvoiceLinesForJournal(Guid licenseeId);
    }

    public partial class MavService : IInvoiceLine
    {

        public InvoiceLine getInvoiceLines(long invoiceId, bool includePolicyData)
        {
            InvoiceLineHelper invoiceLineHelper = new InvoiceLineHelper();
            return invoiceLineHelper.getInvoiceLines(invoiceId, includePolicyData);
        }

        public List<InvoiceLineJournalData> getInvoiceLinesForJournal(Guid licenseeId)
        {
            InvoiceLineHelper invoiceLineHelper = new InvoiceLineHelper();
            return invoiceLineHelper.getInvoiceLinesForJournal(licenseeId);
        }
    }
}