using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.IO;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class ExportCardPayeeInfo
    {
        [DataMember]
        public string InvoiceNumber { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Amount { get; set; }

        [DataMember]
        public string PaymentMethod { get; set; }

        [DataMember]
        public string TransactionType { get; set; }

        [DataMember]
        public string AuthorizationCode { get; set; }

        [DataMember]
        public string TransactionID { get; set; }

        [DataMember]
        public string CreditCardNumber { get; set; }

        [DataMember]
        public string CreditCardExpirationDate { get; set; }

        [DataMember]
        public string BankAccountNumber { get; set; }

        [DataMember]
        public string BankAccountType { get; set; }

        [DataMember]
        public string BankABARoutingCode { get; set; }

        [DataMember]
        public string BankName { get; set; }

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
        public string CustomerPhone { get; set; }

        [DataMember]
        public string CustomerFax { get; set; }

        [DataMember]
        public string CustomerEmail { get; set; }

        public ExportCardPayeeInfo()
        {
            InvoiceNumber = string.Empty;
            Description = string.Empty;
            Amount = string.Empty;
            PaymentMethod = string.Empty;
            TransactionType = string.Empty;
            AuthorizationCode = string.Empty;
            TransactionID = string.Empty;
            CreditCardNumber = string.Empty;
            CreditCardExpirationDate = string.Empty;
            BankAccountNumber = string.Empty;
            BankAccountType = string.Empty;
            BankABARoutingCode = string.Empty;
            BankName = string.Empty;
            CustomerID = string.Empty;
            CustomerFirstName = string.Empty;
            CustomerLastName = string.Empty;
            CustomerCompany = string.Empty;
            CustomerAddress = string.Empty;
            CustomerCity = string.Empty;
            CustomerState = string.Empty;
            CustomerZIP = string.Empty;
            CustomerPhone = string.Empty;
            CustomerFax = string.Empty;
            CustomerEmail = string.Empty;
        }

        public string getLine()
        {
            string fieldSep = ",";
            return (InvoiceNumber + fieldSep + Description + fieldSep + Amount + fieldSep + PaymentMethod + fieldSep + TransactionType + fieldSep + AuthorizationCode + fieldSep + TransactionID + fieldSep + CreditCardNumber + fieldSep + CreditCardExpirationDate + fieldSep + BankAccountNumber + fieldSep + BankAccountType + fieldSep + BankABARoutingCode + fieldSep + BankName + fieldSep + CustomerID + fieldSep + CustomerFirstName + fieldSep + CustomerLastName + fieldSep + CustomerCompany + fieldSep + CustomerAddress + fieldSep + CustomerCity + fieldSep + CustomerState + fieldSep + CustomerZIP + fieldSep + CustomerPhone + fieldSep + CustomerFax + fieldSep + CustomerEmail);
        }

        /// <summary>
        /// Call this function after inserting the row in the invoice table for called
        /// Invoice...
        /// </summary>
        /// <param name="invoiceNo">Invoice No</param>
        /// <param name="isCardPayee">True if card payee customer</param>
        /// <returns></returns>
        public static ExportCardPayeeInfo fillExportCardPayeeInfo(long invoiceNo, bool isCardPayee, DLinq.CommissionDepartmentEntities DataModel)
        {
            ExportCardPayeeInfo exportInfo = null;
            var invoiceRow = (from m in DataModel.Invoices where m.InvoiceId == invoiceNo select m).First();

            if (invoiceRow != null)
            {
                exportInfo = new ExportCardPayeeInfo();
                exportInfo.InvoiceNumber = invoiceRow.InvoiceId.ToString();
                exportInfo.Description = "CommissionsDept Services";
                exportInfo.Amount = invoiceRow.InvoiceAmount.ToString();

                if (isCardPayee)
                    exportInfo.PaymentMethod = "CC";
                else
                    exportInfo.PaymentMethod = "CD";

                exportInfo.CustomerID = invoiceRow.Licensee.LicenseeId.ToString();
                exportInfo.CustomerFirstName = invoiceRow.Licensee.ContactFirst;
                exportInfo.CustomerLastName = invoiceRow.Licensee.ContactLast;
                exportInfo.CustomerCompany = invoiceRow.Licensee.Company;
                exportInfo.CustomerAddress = invoiceRow.Licensee.Address1 + "," + invoiceRow.Licensee.Address2;
                exportInfo.CustomerCity = invoiceRow.Licensee.City;
                exportInfo.CustomerState = invoiceRow.Licensee.State;
                exportInfo.CustomerZIP = invoiceRow.Licensee.ZipCode.ToString();
                exportInfo.CustomerPhone = invoiceRow.Licensee.Phone;
                exportInfo.CustomerFax = invoiceRow.Licensee.Fax;
                exportInfo.CustomerEmail = invoiceRow.Licensee.Email;
            }
            return exportInfo;
        }

        public static bool CreateExportFile(List<ExportCardPayeeInfo> infos,string filename)
        {
            StreamWriter sw = new StreamWriter(Path.GetTempPath() + filename, false, Encoding.ASCII);
            bool fileSuccessfullyCreated = true;
            try
            {
                if (infos != null)
                {
                    foreach (ExportCardPayeeInfo e in infos)
                    {
                        sw.WriteLine(e.getLine());
                    }
                }
            }
            catch
            {
                fileSuccessfullyCreated = false;
            }
            finally
            {
                sw.Close();
            }
            return fileSuccessfullyCreated;
        }
    }
}
