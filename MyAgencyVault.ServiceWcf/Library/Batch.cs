using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IBatch
    {
        #region IEditable<Batch> Members
        [OperationContract]
        int AddUpdateIBatch(Batch Btch);
        [OperationContract]
        BatchAddOutput AddUpdateBatchWithBatchOutput(Batch batch);
        [OperationContract]
        bool DeleteBatch(Guid BatchId, UserRole _UserRole);
        //add by neha
        [OperationContract]
        void AddUpdateBatchNote(Int32 BatchNumber, string BatchNote);
        #endregion

        /// <summary>
        /// GetCurrentBatchList(all/ of an agency/ of a year, / of an id / name of file)
        /// can be overloaded to facilitate above search filter criteria.
        /// </summary>
        /// 
        [OperationContract]
        List<Batch> GetBatchesForReportManager();

        [OperationContract]
        List<Batch> GetAllBatchForReportManagerForAllLicensee();

        [OperationContract]
        List<Batch> GetBatchForReportManagerByLicenssID(Guid LicenseID);

        [OperationContract]
        List<Batch> GetBatchesForDeuEntries();

        [OperationContract]
        List<Batch> GetCurrentBatch(Guid licenceID, DateTime CreatedOn);

        [OperationContract]
        List<Batch> GetBatchesForReportManagerByLicID(Guid licenceID);

        /// <summary>
        /// make the download status of the listed batches to be ........ask pankaj 
        /// </summary>
        /// 
        [OperationContract]
        void ClearDownloadStatus();

        /// <summary>        
        /// To Do:  it might be removed to be implemented on the UI code behind. 
        /// don't know. future implementor of this function can better recognize the conditions.
        /// </summary>
        /// 
        [OperationContract]
        void LaunchWebSite();

        /// <summary>
        /// make the batch status to be closed.
        /// </summary>
        [OperationContract]
        bool CloseBatch(Batch batch);

        [OperationContract]
        bool OpenBatch(int intBatchNumber);

        [OperationContract]
        string BatchName(int intBatchnumber);

        [OperationContract]
        List<Statement> GetCheckAmount(Guid? BatchID, Guid? PayorID);

        [OperationContract]
        string BatchNameById(Guid BatchID);
        /// <summary>
        /// To Do:  it might be removed to be implemented on the UI code behind. 
        /// don't know. future implementor of this function can better recognize the conditions.
        /// purpose : faciltate to open up the pdf/excel file on the client machine to view the content in the file.
        /// </summary>
        [OperationContract]
        void ViewBatchFile(String FilePath, String FileType);

        /// <summary>
        /// compile and compose a file of all statement and entries for the licensee, and upload to the server specified folder.
        /// Giving specific system generated name to the file. ---
        /// </summary>
        [OperationContract]
        void UploadBatchOfLocalPayor();

        [OperationContract]
        List<Statement> GetBatchStatementList(Batch Btch);

        [OperationContract]
        void ExportBatch();

        [OperationContract]
        bool GetBatchPaidStatus(Guid BatchId);

        //[OperationContract]
        //bool SetBatchesAsPaid(List<Guid> BatchIds, List<Guid> payeeID, List<string> lstFilter);

        [OperationContract]
        bool SetBatchesAsPaid(List<Guid> BatchIds);

        [OperationContract]
        bool SetBatchToUnPaidStatus(int BatchNumber);

        [OperationContract]
        bool SetBatchesToPaid(List<Guid> BatchIds, string strFilter, List<Guid> lstPayee);

        [OperationContract]
        string SetBatchesToPaidInReports(List<Guid> BatchIds, string strFilter, List<Guid> lstPayee);

        [OperationContract]
        void UpdateBatchFileName(int intBatchNumber, string strFileName);
    }

    [ServiceContract]
    interface IDownloadBatch
    {
        [OperationContract]
        List<DownloadBatch> GetDownloadBatchList();

        [OperationContract]
        bool isBatchPartiallyOrFullyPaid(DownloadBatch batch);

        [OperationContract]
        void DeleteDownloadBatch(DownloadBatch batch, UserRole _UserRole);

        [OperationContract]
        ImportFileData ImportBatchFile(DownloadBatch batch, UserRole _UserRole);

        [OperationContract]
        DateTime? UpdateEntryStatus(DownloadBatch batch);

        [OperationContract]
        void ClearDownloadBatch(DownloadBatch batch, UserRole _UserRole);
    }

    public partial class MavService : IBatch, IDownloadBatch
    {

        #region IBatch Members
        //add by neha
        public void AddUpdateBatchNote(Int32 BatchNumber, string BatchNote)
        {
            Batch objBatch = new Batch();
            objBatch.AddUpdateBatchNote(BatchNumber, BatchNote);
        }
        public int AddUpdateIBatch(Batch Btch)
        {
            return Btch.AddUpdate();
        }

        public BatchAddOutput AddUpdateBatchWithBatchOutput(Batch batch)
        {
            return batch.AddUpdateBatch();
        }

        public bool DeleteBatch(Guid BatchId, UserRole _UserRole)
        {
            Batch objBatch = new Batch();
            return objBatch.DeleteBatch(BatchId, _UserRole);
        }

        public List<Batch> GetBatchesForReportManager()
        {
            // Batch objBatch = new Batch();
            ActionLogger.Logger.WriteImportPolicyLog("GetBatchesForReportManager:start for process",true);
            return Batch.GetBatchListForReportManager();
        }

        public List<Batch> GetAllBatchForReportManagerForAllLicensee()
        {
            Batch objBatch = new Batch();
            return objBatch.GetAllBatchForReportManagerForAllLicensee();
        }

        public List<Batch> GetBatchForReportManagerByLicenssID(Guid LicenseID)
        {
            Batch objBatch = new Batch();
            return objBatch.GetBatchForReportManagerByLicenssID(LicenseID);
        }

        public List<Batch> GetBatchesForDeuEntries()
        {
            Batch objBatch = new Batch();
            return objBatch.GetBatchesForDeuEntries();
        }

        public List<Batch> GetCurrentBatch(Guid licenceID, DateTime CreatedOn)
        {
            Batch objBatch = new Batch();
            return objBatch.GetCurrentBatch(licenceID, CreatedOn);
        }


        public List<Batch> GetBatchesForReportManagerByLicID(Guid licenceID)
        {
            Batch objBatch = new Batch();
            return objBatch.GetCurrentBatchForReportBYLicID(licenceID);
        }

        public void ClearDownloadStatus()
        {
            throw new NotImplementedException();
        }

        public void LaunchWebSite()
        {
            throw new NotImplementedException();
        }

        public bool CloseBatch(Batch batch)
        {
            return batch.CloseBatch();
        }

        public bool OpenBatch(int intBatchNumber)
        {
            Batch objBatch = new Batch();
            return objBatch.OpenBatch(intBatchNumber);
            
        }

        public string BatchName(int intBatchnumber)
        {
            Batch objBatch = new Batch();
            return objBatch.BatchName(intBatchnumber);
        }

        public List<Statement> GetCheckAmount(Guid? BatchID, Guid? PayorID)
        {
            Batch objBatch = new Batch();
            return objBatch.GetCheckAmount(BatchID, PayorID);
        }

        public string BatchNameById(Guid BatchID)
        {
            Batch objBatch = new Batch();
            return objBatch.BatchNameById(BatchID);
        }

        public void ViewBatchFile(string FilePath, string FileType)
        {
            throw new NotImplementedException();
        }

        public void UploadBatchOfLocalPayor()
        {
            throw new NotImplementedException();
        }

        public List<Statement> GetBatchStatementList(Batch Btch)
        {
            Batch objBatch = new Batch();
            return objBatch.GetStatementList(Btch.BatchId);
        }

        public void ExportBatch()
        {
            Batch.ExportBatch();
        }

        public bool GetBatchPaidStatus(Guid BatchId)
        {
            Batch objBatch = new Batch();
            return objBatch.GetBatchPaidStatus(BatchId);
        }

        public bool SetBatchesAsPaid(List<Guid> BatchIds)
        {
            Batch objBatch = new Batch();
            return objBatch.SetBatchesAsPaid(BatchIds);
        }

        public bool SetBatchToUnPaidStatus(int BatchNumber)
        {
            Batch objBatch = new Batch();
            return objBatch.SetBatchToUnPaidStatus(BatchNumber);
        }

        public bool SetBatchesToPaid(List<Guid> BatchIds, string strFilter, List<Guid> lstPayee)
        {
            Batch objBatch = new Batch();
            return objBatch.SetBatchesToPaid(BatchIds, strFilter, lstPayee);
        }

        public string SetBatchesToPaidInReports(List<Guid> BatchIds, string strFilter, List<Guid> lstPayee)
        {
            Batch objBatch = new Batch();
            return objBatch.SetBatchesToPaidInReports(BatchIds, strFilter, lstPayee);
        }

        public void UpdateBatchFileName(int intBatchNumber, string strFileName)
        {
            Batch objBatch = new Batch();
            objBatch.UpdateBatchFileName(intBatchNumber, strFileName);
        }

        #endregion

        #region Download Batch Members

        public List<DownloadBatch> GetDownloadBatchList()
        {
            DownloadBatch objDownloadBatch = new DownloadBatch();
            return objDownloadBatch.GetDownloadBatchList();
        }

        public bool isBatchPartiallyOrFullyPaid(DownloadBatch batch)
        {
            return batch.isBatchPartiallyOrFullyPaid();
        }

        public void DeleteDownloadBatch(DownloadBatch batch, UserRole _UserRole)
        {
            batch.DeleteDownloadBatch( _UserRole);
        }

        public ImportFileData ImportBatchFile(DownloadBatch batch, UserRole _UserRole)
        {
            return batch.ImportBatchFile( _UserRole);
        }

        public DateTime? UpdateEntryStatus(DownloadBatch batch)
        {
            return batch.UpdateEntryStatus();
        }

        public void ClearDownloadBatch(DownloadBatch batch, UserRole _UserRole)
        {
            batch.ClearDownloadBatch( _UserRole);
        }

        #endregion
    }

}
