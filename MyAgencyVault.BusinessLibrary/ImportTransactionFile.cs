using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Data;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class ImportTransactionFile
    {
        [DataMember]
        public string ResponseCode { get; set; }

        [DataMember]
        public string AuthorizationCode { get; set; }

        [DataMember]
        public string AddressVerificationStatus { get; set; }

        [DataMember]
        public string TransactionID { get; set; }

        [DataMember]
        public string SubmitDateTime { get; set; }

        [DataMember]
        public string CardNumber { get; set; }

        [DataMember]
        public string ExpirationDate { get; set; }

        [DataMember]
        public string InvoiceNumber { get; set; }

        [DataMember]
        public string InvoiceDescription { get; set; }

        [DataMember]
        public string TotalAmount { get; set; }

        [DataMember]
        public string Method { get; set; }

        [DataMember]
        public string ActionCode { get; set; }

        [DataMember]
        public string CustomerID { get; set; }

        [DataMember]
        public string CustomerFirstName { get; set; }

        [DataMember]
        public string CustomerLastName { get; set; }

        [DataMember]
        public string CustomerCompany { get; set; }

        [DataMember]
        public string CustomerAddress { get; set; }

        [DataMember]
        public string CustomerCity { get; set; }

        [DataMember]
        public string CustomerState { get; set; }

        [DataMember]
        public string CustomerZIP { get; set; }

        [DataMember]
        public string CustomerCountry { get; set; }

        [DataMember]
        public string CustomerPhone { get; set; }

        [DataMember]
        public string CustomerFax { get; set; }

        [DataMember]
        public string CustomerEmail { get; set; }

        [DataMember]
        public string ShipToFirstName { get; set; }

        [DataMember]
        public string ShipToLastName { get; set; }

        [DataMember]
        public string ShipToCompany { get; set; }

        [DataMember]
        public string ShipToAddress { get; set; }

        [DataMember]
        public string ShipToCity { get; set; }

        [DataMember]
        public string ShipToState { get; set; }

        [DataMember]
        public string ShipToZIP { get; set; }

        [DataMember]
        public string ShipToCountry { get; set; }

        [DataMember]
        public string L2Tax { get; set; }

        [DataMember]
        public string L2Duty { get; set; }

        [DataMember]
        public string L2Freight { get; set; }

        [DataMember]
        public string L2TaxExempt { get; set; }

        [DataMember]
        public string L2PurchaseOrderNumber { get; set; }

        [DataMember]
        public string ABARoutingNumber { get; set; }

        [DataMember]
        public string BankAccountNumber { get; set; }

        /// <summary>
        /// Call this function after inserting the row in the invoice table for called
        /// Invoice...
        /// </summary>
        /// <param name="invoiceNo">Invoice No</param>
        /// <param name="isCardPayee">True if card payee customer</param>
        /// <returns></returns>
        public static void fillImportTransactionFileInfo(string FileName, List<string> transactionLines)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                if (DataModel.Connection.State != ConnectionState.Open)
                    DataModel.Connection.Open();

                IDbTransaction transaction = DataModel.Connection.BeginTransaction();

                try
                {
                    string invoiceId = string.Empty;

                    DLinq.ImportBatchFile importBatchFile = new DLinq.ImportBatchFile();
                    importBatchFile.FileName = FileName;
                    importBatchFile.CreatedOn = DateTime.Today;
                    DataModel.AddToImportBatchFiles(importBatchFile);
                    DataModel.SaveChanges();

                    foreach (string str in transactionLines)
                    {
                        string[] fields = str.Split('\t');
                        ImportTransactionFile importFileData = new ImportTransactionFile(fields);

                        DLinq.Journal journal = new DLinq.Journal();

                        invoiceId = importFileData.InvoiceNumber;
                        DLinq.Invoice invoice = (from m in DataModel.Invoices where (long.Parse(invoiceId) == m.InvoiceId) select m).FirstOrDefault();
                        journal.Invoice = invoice;
                        journal.Licensee = invoice.Licensee;

                        if (invoice.ExportBatchFile.ExportBatchId != Guid.Empty)
                        {
                            Guid fileId = invoice.ExportBatchFile.ExportBatchId;
                            var FileRecord = (from m in DataModel.ExportBatchFiles where m.ExportBatchId == fileId select m).First();
                            FileRecord.IsFileImported = true;
                        }

                        journal.TransactionId = importFileData.TransactionID;
                        journal.TransactionDateTime = DateTime.Parse(importFileData.SubmitDateTime);
                        journal.IsManuallEntry = false;
                        journal.JournalAmount = decimal.Parse(importFileData.TotalAmount);
                        journal.CreatedOn = DateTime.Today;
                        journal.ReceivedDate = DateTime.Today;
                        journal.ImportBatchFile = importBatchFile;

                        if (importFileData.ResponseCode.Trim() == "1")
                        {
                            journal.PaymentStatus = "Payment is successfull" + "-" + "Authorization code =" + importFileData.AuthorizationCode;
                        }
                        else
                        {
                            string failure = string.Empty;
                            if (importFileData.ResponseCode.Trim() == "2")
                                failure = "Declined";
                            else if (importFileData.ResponseCode.Trim() == "3")
                                failure = "Error";
                            else
                                failure = "Held for Review";

                            journal.PaymentStatus = "Payment is failed" + "-" + failure;
                        }

                    }

                    DataModel.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public ImportTransactionFile(string[] records)
        {
            ResponseCode = records[0];
            AuthorizationCode = records[1];
            AddressVerificationStatus = records[2];
            TransactionID = records[3];
            SubmitDateTime = records[4];
            CardNumber = records[5];
            ExpirationDate = records[6];
            InvoiceNumber = records[7];
            InvoiceDescription = records[8];
            TotalAmount = records[9];
            Method = records[10];
            ActionCode = records[11];
            CustomerID = records[12];
            CustomerFirstName = records[13];
            CustomerLastName = records[14];
            CustomerCompany = records[15];
            CustomerAddress = records[16];
            CustomerCity = records[17];
            CustomerState = records[18];
            CustomerZIP = records[19];
            CustomerCountry = records[20];
            CustomerPhone = records[21];
            CustomerFax = records[22];
            CustomerEmail = records[23];
            ShipToFirstName = records[24];
            ShipToLastName = records[25];
            ShipToCompany = records[26];
            ShipToAddress = records[27];
            ShipToCity = records[28];
            ShipToState = records[29];
            ShipToZIP = records[30];
            ShipToCountry = records[31];
            L2Tax = records[32];
            L2Duty = records[33];
            L2Freight = records[34];
            L2TaxExempt = records[35];
            L2PurchaseOrderNumber = records[36];
            ABARoutingNumber = records[37];
            BankAccountNumber = records[38];
        }

        public ImportTransactionFile()
        {

        }
    }
}

