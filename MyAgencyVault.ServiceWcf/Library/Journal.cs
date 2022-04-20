using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract] 
    interface IJournal
    {
        [OperationContract()]
        List<LicenseeInvoiceJournal> getJournalEntriesByLicensee(Guid LicenseeID);
        [OperationContract()]
        List<LicenseeInvoiceJournal> getJournalEntriesByInvoiceID(long InvoiceId);
        [OperationContract()]
        List<LicenseeInvoiceJournal> getJournalEntriesByLicenseeIDInvocieID(Guid LicenseeID, long InvoiceId);
        [OperationContract()]
        List<LicenseeInvoiceJournal> getAllJournalEntries();
        [OperationContract()]
        bool DeleteJournalEntry(LicenseeInvoiceJournal journalEntry);
        [OperationContract()]
        long InsertJournalEntry(LicenseeInvoiceJournal journalEntry);
        [OperationContract()]
        bool UpdateJournalEntry(LicenseeInvoiceJournal journalEntry);
    }

    public partial class MavService : IJournal
    {

        public List<LicenseeInvoiceJournal> getJournalEntriesByLicensee(Guid LicenseeID)
        {
            JournalHelper journalHelper = new JournalHelper();
            return journalHelper.getJournalEntries(LicenseeID);
        }

        public List<LicenseeInvoiceJournal> getJournalEntriesByInvoiceID(long InvoiceId)
        {
            JournalHelper journalHelper = new JournalHelper();
            return journalHelper.getJournalEntries(InvoiceId);
        }

        public List<LicenseeInvoiceJournal> getJournalEntriesByLicenseeIDInvocieID(Guid LicenseeID, long InvoiceId)
        {
            JournalHelper journalHelper = new JournalHelper();
            return journalHelper.getJournalEntries(LicenseeID, InvoiceId);
        }

        public List<LicenseeInvoiceJournal> getAllJournalEntries()
        {
            JournalHelper journalHelper = new JournalHelper();
            return journalHelper.getAllJournalEntries();
        }

        public bool DeleteJournalEntry(LicenseeInvoiceJournal journalEntry)
        {
            JournalHelper journalHelper = new JournalHelper();
            return journalHelper.DeleteJournalEntry(journalEntry);
        }

        public long InsertJournalEntry(LicenseeInvoiceJournal journalEntry)
        {
            JournalHelper journalHelper = new JournalHelper();
            return journalHelper.InsertJournalEntry(journalEntry);
        }

        public bool UpdateJournalEntry(LicenseeInvoiceJournal journalEntry)
        {
            JournalHelper journalHelper = new JournalHelper();
            return journalHelper.UpdateJournalEntry(journalEntry);
        }
    }

}
