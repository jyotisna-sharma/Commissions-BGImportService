using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class LicenseeInvoice
    {
        [DataMember]
        public long InvoiceId { get; set; }
        [DataMember]
        public Guid? LicenseeId { get; set; }
        [DataMember]
        public DateTime? BillingStartDate { get; set; }
        [DataMember]
        public DateTime? BillingEndDate { get; set; }
        [DataMember]
        public DateTime? BillingDate { get; set; }
        [DataMember]
        public decimal? InvoiceAmount { get; set; }
        [DataMember]
        public decimal? DueBalance { get; set; }
        [DataMember]
        public DateTime? InvoiceGeneratedOn { get; set; }
        [DataMember]
        public Guid? ExportedBatchId { get; set; }
        [DataMember]
        public string BillingPeriod { get; set; }
    }

    public class LicenseeInvoiceHelper
    {
        public static List<LicenseeInvoice> getAllInvoice()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var products = (from se in DataModel.Invoices
                                where se.BillingStartDate != null && se.BillingEndDate != null
                                select new LicenseeInvoice
                                {
                                    InvoiceId = se.InvoiceId,
                                    LicenseeId = se.Licensee.LicenseeId,
                                    BillingStartDate = se.BillingStartDate,
                                    BillingEndDate = se.BillingEndDate,
                                    BillingDate = se.BillingDate,
                                    InvoiceAmount = se.InvoiceAmount,
                                    DueBalance = se.DueBalance,
                                    InvoiceGeneratedOn = se.InvoiceGeneratedOn,
                                    ExportedBatchId = se.ExportBatchFile.ExportBatchId
                                }).ToList();
                products.ForEach(s => s.BillingPeriod = s.BillingStartDate.Value.ToShortDateString() + "-" + s.BillingEndDate.Value.ToShortDateString());
                return products;
            }
        }

        public static LicenseeInvoice getInvoiceByID(long Id)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var invoice = (from se in DataModel.Invoices
                               where se.InvoiceId == Id
                               select new LicenseeInvoice
                               {
                                   InvoiceId = se.InvoiceId,
                                   LicenseeId = se.Licensee.LicenseeId,
                                   BillingStartDate = se.BillingStartDate,
                                   BillingEndDate = se.BillingEndDate,
                                   BillingDate = se.BillingDate,
                                   InvoiceAmount = se.InvoiceAmount,
                                   DueBalance = se.DueBalance,
                                   InvoiceGeneratedOn = se.InvoiceGeneratedOn,
                                   ExportedBatchId = se.ExportBatchFile.ExportBatchId
                                   //BillingPeriod = se.BillingStartDate.ToString() + "-" + se.BillingEndDate.ToString()
                               }).FirstOrDefault();

                return invoice;
            }
        }

        public static string getExportBatchName(Guid? ExportedBatchId)
        {
            string fileName = string.Empty;
            if (ExportedBatchId != null && ExportedBatchId.Value != Guid.Empty)
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    fileName = (from se in DataModel.ExportBatchFiles
                                    where se.ExportBatchId == ExportedBatchId.Value
                                    select se.FileName).FirstOrDefault();

                }
            }
            return fileName;
        }

        public static DateTime? getLatestBillingDate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DateTime? maxBillingDate = null;
                
                if(DataModel.Invoices.Count() != 0)
                    maxBillingDate = DataModel.Invoices.Max(s => s.BillingDate);

                return maxBillingDate;
            }
        }
    }
}
