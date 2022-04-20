using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using DataAccessLayer.LinqtoEntity;
using System.Data;
using System.Transactions;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class BatchFiles
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FileType { get; set; }

        [DataMember]
        public DateTime? CreatedOn { get; set; }

        [DataMember]
        public bool IsDeletable { get; set; }

        public static List<BatchFiles> fillBatchFilesData()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<BatchFiles> batchFileCollection = new List<BatchFiles>();
                var v1 = from dv in DataModel.ExportBatchFiles select dv;
                var v2 = from dv in DataModel.ImportBatchFiles orderby dv.CreatedOn descending select dv;

                foreach (var v in v1)
                {
                    BatchFiles file = new BatchFiles();
                    file.FileName = v.FileName;
                    file.FileType = "Exported";
                    file.CreatedOn = v.CreatedOn;
                    if (v.IsFileImported.HasValue)
                        file.IsDeletable = (!v.IsFileImported.Value);
                    else
                        file.IsDeletable = false;

                    batchFileCollection.Add(file);
                }

                bool isDeletable = true;
                foreach (var v in v2)
                {
                    BatchFiles file = new BatchFiles();
                    file.IsDeletable = isDeletable;
                    file.FileName = v.FileName;
                    file.FileType = "Imported";
                    file.CreatedOn = v.CreatedOn;
                    batchFileCollection.Add(file);
                    isDeletable = false;
                }

                return batchFileCollection;
            }
        }

        public static bool DeleteBatchFile(BatchFiles batchFile)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                TransactionOptions options = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromMinutes(2)
                };

                bool retVal = true;
                try
                {
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, options))
                    {
                        if (batchFile.FileType == "Imported")
                        {
                            ImportBatchFile tempBatchFile = null;
                            tempBatchFile = (from m in DataModel.ImportBatchFiles where m.FileName == batchFile.FileName select m).First();
                            DataModel.DeleteObject(tempBatchFile);
                        }
                        else
                        {
                            DLinq.ExportBatchFile tempBatchFile = null;
                            tempBatchFile = (from m in DataModel.ExportBatchFiles where m.FileName == batchFile.FileName select m).First();

                            foreach (DLinq.Invoice invoice in tempBatchFile.Invoices)
                                invoice.Licensee.DueBalance -= invoice.InvoiceAmount;
                            
                            DataModel.DeleteObject(tempBatchFile);
                            DataModel.SaveChanges();

                            DateTime? latestBillingDate = LicenseeInvoiceHelper.getLatestBillingDate();

                            if (latestBillingDate != null)
                            {
                                if(batchFile.FileName.Contains("Card"))
                                    ExportDate.setCardPayeeExportDate(latestBillingDate.Value.AddMonths(-1));
                                else
                                    ExportDate.setCheckPayeeExportDate(latestBillingDate.Value.AddMonths(-1));
                            }
                            else
                            {
                                if (batchFile.FileName.Contains("Card"))
                                    ExportDate.setCardPayeeExportDate(null);
                                else
                                    ExportDate.setCheckPayeeExportDate(null);
                            }
                        }
                        DataModel.SaveChanges();
                        transaction.Complete();
                    }
                }
                catch
                {
                    retVal = false;
                }

                return retVal;
            }
        }
    }
}
