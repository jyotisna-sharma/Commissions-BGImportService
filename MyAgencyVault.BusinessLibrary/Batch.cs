using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Globalization;
using System.Transactions;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public enum UploadStatus
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Available = 1,
        [EnumMember]
        InProgress = 2,
        [EnumMember]
        Completed = 3,
        [EnumMember]
        Manual = 4,
        [EnumMember]
        Automatic = 5,
        [EnumMember]
        ImportXls = 6
    }

    [DataContract]
    public enum EntryStatus
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Unassigned = 1,
        [EnumMember]
        ImportPending = 2,
        [EnumMember]
        ImportUnsuccessfull = 3,
        [EnumMember]
        PendingDataEntry = 4,
        [EnumMember]
        InDataEntry = 5,
        [EnumMember]
        BatchCompleted = 6,
        [EnumMember]
        PartialUnpaid = 7,
        [EnumMember]
        Paid = 8,
        [EnumMember]
        Importedfiletype = 9

    }

    [DataContract]
    public class ModifiableBatchData
    {
        [DataMember]
        public Guid BatchId { get; set; }
        [DataMember]
        public DateTime LastModifiedDate { get; set; }
        [DataMember]
        public EntryStatus EntryStatus { get; set; }
    }

    [DataContract]
    public class Batch
    {
        [DataMember]
        public string TotalStatementAmount { get; set; }

        #region IEditable<Batch> Members

        public void AddUpdateBatchNote(Int32 BatchNumber, string BatchNote)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.Batch _AddbatchNote = (from pn in DataModel.Batches where pn.BatchNumber == BatchNumber select pn).FirstOrDefault();
                if (_AddbatchNote != null)
                {
                    _AddbatchNote.BatchNote = BatchNote;
                    DataModel.SaveChanges();
                }

            }

        }

        public BatchAddOutput AddUpdateBatch()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                int batchNumber = AddUpdate();
                BatchAddOutput output = new BatchAddOutput();
                output.BatchNumber = batchNumber;
                output.LicenseeName = DataModel.Licensees.FirstOrDefault(s => s.LicenseeId == this.LicenseeId).Company;

                return output;
            }
        }

        public int AddUpdate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                bool addCase = false;

                var _uploadBatch = (from pn in DataModel.Batches where pn.BatchId == this.BatchId select pn).FirstOrDefault();
                if (_uploadBatch == null)
                {
                    addCase = true;
                    _uploadBatch = new DLinq.Batch
                    {
                        BatchId = this.BatchId,
                        BatchNumber = this.BatchNumber,
                        EntryStatusId = (int)this.EntryStatus,
                        UploadStatusId = (int)this.UploadStatus,
                        LastModifiedDate = DateTime.Now,
                        FileType = this.FileType,
                        LicenseeId = this.LicenseeId,
                        SiteLoginID = this.SiteId,
                        PayorId = this.PayorId,
                        IsManuallyUploaded = this.IsManuallyUploaded
                    };

                    if (_uploadBatch.UploadStatusId == (int)UploadStatus.Manual || _uploadBatch.UploadStatusId == (int)UploadStatus.Automatic || _uploadBatch.UploadStatusId == (int)UploadStatus.ImportXls)
                        _uploadBatch.CreatedOn = DateTime.Now;
                    else
                        _uploadBatch.CreatedOn = null;

                    DataModel.AddToBatches(_uploadBatch);
                }
                else
                {

                    _uploadBatch.BatchNumber = this.BatchNumber;
                    _uploadBatch.EntryStatusId = (int)this.EntryStatus;
                    _uploadBatch.UploadStatusId = (int)this.UploadStatus;
                    _uploadBatch.FileType = this.FileType;
                    _uploadBatch.LastModifiedDate = DateTime.Now;
                    _uploadBatch.FileName = this.FileName;
                }
                DataModel.SaveChanges();

                if (addCase)
                {
                    _uploadBatch.FileName = DataModel.Licensees.FirstOrDefault(s => s.LicenseeId == this.LicenseeId).Company + "_" + _uploadBatch.BatchNumber.ToString() + "." + _uploadBatch.FileType;
                    DataModel.SaveChanges();
                }
                return _uploadBatch.BatchNumber;
            }
        }

        public void UpdateBatchByBatchId(Guid batchID, int batchEntryStatus, int batchUploadStatus)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _uploadBatch = (from pn in DataModel.Batches where pn.BatchId == batchID select pn).FirstOrDefault();

                _uploadBatch.EntryStatusId = batchEntryStatus;
                _uploadBatch.UploadStatusId = batchUploadStatus;
                _uploadBatch.LastModifiedDate = DateTime.Now;
                _uploadBatch.CreatedOn = DateTime.Now;
                DataModel.SaveChanges();
            }
        }

        public void UpdateBatchFileName(int intBatchNumber, string strFileName)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                DLinq.Batch currentBatch = (from p in DataModel.Batches where p.BatchNumber == intBatchNumber select p).FirstOrDefault();
                if (currentBatch != null)
                {
                    currentBatch.FileName = strFileName;
                }
                DataModel.SaveChanges();
            }
        }


        public bool DeleteBatch(Guid BatchId, UserRole _UserRole)
        {
            ActionLogger.Logger.WriteImportPolicyLog("DeleteBatch : BatchID - " + BatchId + ", USerRole - " + _UserRole.ToString(), true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.Statement> statements = DataModel.Statements.Where(s => s.BatchId == BatchId).ToList();

                int entryCount = 0;
                foreach (DLinq.Statement statement in statements)
                {
                    entryCount += statement.EntriesByDEUs.Count;
                }

                TransactionOptions options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromMinutes(entryCount * 1)
                };

                bool deleteSuccessfull = true;

                try
                {
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, options))
                    {
                        foreach (DLinq.Statement statement in statements)
                        {
                            List<Guid> entryIds = statement.EntriesByDEUs.Select(s => s.DEUEntryID).ToList();
                            for (int index = 0; index < entryIds.Count; index++)
                            {
                                PostUtill.PostStart(PostEntryProcess.Delete, entryIds[index], Guid.Empty, Guid.Empty, _UserRole, PostEntryProcess.Delete, string.Empty, string.Empty);
                            }
                        }

                        List<Guid> statementIds = statements.Select(s => s.StatementId).ToList();
                        //var statementIds = statements.Select(s => s.StatementId).ToList();
                        for (int index = 0; index < statementIds.Count; index++)
                        {
                            Guid guidStatement = statementIds[index];
                            DataModel.DeleteObject(DataModel.Statements.FirstOrDefault(s => s.StatementId == guidStatement));

                        }

                        //DataModel.SaveChanges();

                        DLinq.Batch batch = DataModel.Batches.FirstOrDefault(s => s.BatchId == BatchId);
                        if (batch != null)
                        {
                            Guid guidBatch = batch.BatchId;
                            DataModel.Batches.DeleteObject(DataModel.Batches.FirstOrDefault(s => s.BatchId == guidBatch));
                            DataModel.SaveChanges();
                        }


                        transaction.Complete();
                    }
                }
                catch (Exception)
                {
                    deleteSuccessfull = false;
                }

                return deleteSuccessfull;
            }
        }

        public bool ClearBatch(Guid BatchId, UserRole _UserRole)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.Statement> statements = DataModel.Statements.Where(s => s.BatchId == BatchId).ToList();

                int entryCount = 0;
                foreach (DLinq.Statement statement in statements)
                {
                    entryCount += statement.EntriesByDEUs.Count;
                }

                TransactionOptions options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromMinutes(entryCount * 1)
                };

                bool clearSuccessfull = true;
                try
                {
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, options))
                    {
                        foreach (DLinq.Statement statement in statements)
                        {
                            List<Guid> entryIds = statement.EntriesByDEUs.Select(s => s.DEUEntryID).ToList();
                            for (int index = 0; index < entryIds.Count; index++)
                            {
                                PostUtill.PostStart(PostEntryProcess.Delete, entryIds[index], Guid.Empty, Guid.Empty, _UserRole, PostEntryProcess.Delete, string.Empty, string.Empty);
                            }
                        }

                        List<Guid> statementIds = statements.Select(s => s.StatementId).ToList();
                        for (int index = 0; index < statementIds.Count; index++)
                        {
                            DataModel.DeleteObject(DataModel.Statements.FirstOrDefault(s => s.StatementId == statementIds[index]));
                        }

                        DLinq.Batch batch = DataModel.Batches.FirstOrDefault(s => s.BatchId == BatchId);
                        batch.UploadStatusId = 1;
                        batch.EntryStatusId = 1;

                        DataModel.SaveChanges();
                        transaction.Complete();
                    }
                }
                catch (Exception)
                {
                    clearSuccessfull = false;
                }

                return clearSuccessfull;
            }
        }

        #endregion

        #region "Data Members aka - |Public properties"
        [DataMember]
        public Guid BatchId { get; set; }
        [DataMember]
        public int BatchNumber { get; set; }
        [DataMember]
        public Guid? PayorId { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public DateTime LastModifiedDate { get; set; }
        [DataMember]
        public UploadStatus UploadStatus { get; set; }
        [DataMember]
        public Guid LicenseeId { get; set; }
        [DataMember]
        public string LicenseeName { get; set; }
        [DataMember]
        public EntryStatus EntryStatus { get; set; }
        [DataMember]
        public string FileType { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public Guid? SiteId { get; set; }
        //[DataMember]
        //public bool IsDeleted { get; set; }
        [DataMember]
        public string AssignedDeuUserName { get; set; }
        [DataMember]
        public bool? CreatedFromUpload { get; set; }
        //[DataMember]
        //public string BatchPaidStatus { get; set; }
        [DataMember]
        public bool? IsManuallyUploaded { get; set; }
        //Added By sunil
        [DataMember]
        public List<Statement> BatchStatements { get; set; }

        [DataMember]

        public string BatchNote { get; set; }

        private List<Statement> _BatchStatements;
        #endregion

        //public static Batch GetBatchViaBatchId(Guid batchid)
        public Batch GetBatchViaBatchId(Guid batchid)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var batchlst = (from dv in DataModel.Batches
                                    where dv.BatchId == batchid
                                    select new Batch
                                    {
                                        BatchNumber = dv.BatchNumber,
                                        BatchId = dv.BatchId,
                                        UploadStatus = (UploadStatus)(dv.UploadStatusId ?? 0),
                                        LicenseeId = dv.LicenseeId,
                                        LicenseeName = dv.Licensee.Company,
                                        CreatedDate = dv.CreatedOn,
                                        LastModifiedDate = dv.LastModifiedDate.Value,
                                        EntryStatus = (EntryStatus)(dv.EntryStatusId ?? 0),
                                        SiteId = dv.SiteLoginID,
                                        PayorId = dv.PayorId,
                                        AssignedDeuUserName = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == dv.AssignedUserCredentialId).UserName,
                                        FileName = dv.FileName
                                    }).ToList();

                    return batchlst.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetBatchViaBatchId:Excption occurs while process" + ex.Message, true);
                return null;
               // throw ex;
            }
        }

        //public static Batch GetBatchEntryViaEntryId(Guid PolicyPaymentEntryId)
        public Batch GetBatchEntryViaEntryId(Guid PolicyPaymentEntryId)
        {
            Batch _Batch = null;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    Guid StmtId = (from f in DataModel.PolicyPaymentEntries
                                   where (f.PaymentEntryId == PolicyPaymentEntryId)
                                   select f.Statement.StatementId).FirstOrDefault<Guid>();

                    Guid batchId = (from f in DataModel.Statements
                                    where (f.StatementId == StmtId)
                                    select f.Batch.BatchId).FirstOrDefault<Guid>();

                    var TempBatch = (from f in DataModel.Batches
                                     where (f.BatchId == batchId)
                                     select f).FirstOrDefault();
                    _Batch = new Batch()
                    {
                        BatchId = TempBatch.BatchId,
                        BatchNumber = TempBatch.BatchNumber,
                        LicenseeId = TempBatch.LicenseeId,
                        CreatedDate = TempBatch.CreatedOn,
                        FileType = TempBatch.FileType,
                        EntryStatus = (EntryStatus)(TempBatch.EntryStatusId ?? 0),
                        //IsDeleted = TempBatch.IsDeleted,
                        AssignedDeuUserName = TempBatch.AssignedUserCredentialId == Guid.Empty ? "super" : DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == TempBatch.AssignedUserCredentialId).UserName,
                        CreatedFromUpload = TempBatch.CreatedFromUpload ?? false,
                        UploadStatus = (UploadStatus)(TempBatch.UploadStatusId ?? 0),
                        FileName = TempBatch.FileName ?? string.Empty,
                        //LastModifiedDate = TempBatch.LastModifiedDate.Value,
                        LastModifiedDate = TempBatch.LastModifiedDate.Value,
                        IsManuallyUploaded = TempBatch.IsManuallyUploaded ?? false,
                        SiteId = TempBatch.SiteLoginID ?? new Guid(),
                    };

                }
            }
            catch (Exception ex)
            {

            }
            return _Batch;
        }
        /// <summary>
        /// GetCurrentBatchList(all/ of an agency/ of a year, / of an id / name of file)
        /// can be overloaded to facilitate above search filter criteria.
        /// </summary>
        public static List<Batch> GetBatchListForReportManager()
        {
            ActionLogger.Logger.WriteImportPolicyLog("GetBatchListForReportManager: processing start", true);
            var batchlst = new List<Batch>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.CommandTimeout = 6000000;
                    batchlst = (from dv in DataModel.Batches
                                where dv.EntryStatusId != 9 && dv.UploadStatusId != 6
                                select new Batch
                                {
                                    BatchNumber = dv.BatchNumber,
                                    BatchId = dv.BatchId,
                                    UploadStatus = (UploadStatus)(dv.UploadStatusId ?? 0),
                                    LicenseeId = dv.LicenseeId,
                                    LicenseeName = dv.Licensee.Company,
                                    CreatedDate = dv.CreatedOn,
                                    LastModifiedDate = dv.LastModifiedDate.Value,
                                    EntryStatus = (EntryStatus)(dv.EntryStatusId ?? 0),
                                    SiteId = dv.SiteLoginID,
                                    PayorId = dv.PayorId,
                                    AssignedDeuUserName = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == dv.AssignedUserCredentialId).UserName,
                                    FileName = dv.FileName
                                }).ToList();
                }
                ActionLogger.Logger.WriteImportPolicyLog("GetBatchListForReportManager: processing End succesffully", true);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetBatchListForReportManager: processing fails Exception occurs:" + ex.Message, true);
            }
            ActionLogger.Logger.WriteImportPolicyLog("GetBatchListForReportManager: processing End succesffully with batchlist:" + batchlst.ToStringDump(), true);
            ActionLogger.Logger.WriteImportPolicyLog("GetBatchListForReportManager: processing End succesffully with batchlist:" + batchlst.Count, true);
            return batchlst;
        }

        public List<Batch> GetAllBatchForReportManagerForAllLicensee()
        {
            List<Batch> batchlst = new List<Batch>();
            try
            {
                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                System.Data.EntityClient.EntityConnection ec = (System.Data.EntityClient.EntityConnection)ctx.Connection;
                System.Data.SqlClient.SqlConnection sc = (System.Data.SqlClient.SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                string adoConnStr = sc.ConnectionString;

                DateTime? nullDateTime = null;

                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(adoConnStr))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Usp_GetBatchListForReportManager", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        con.Open();
                        System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
                        // Call Read before accessing data. 
                        while (reader.Read())
                        {
                            try
                            {
                                Batch objbatch = new Batch();

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["BatchId"])))
                                    {
                                        objbatch.BatchId = reader["BatchId"] == null ? Guid.Empty : (Guid)reader["BatchId"];
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["BatchNumber"])))
                                    {
                                        objbatch.BatchNumber = reader["BatchNumber"] == null ? 0 : Convert.ToInt32(reader["BatchNumber"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["EntryStatusId"])))
                                    {
                                        objbatch.EntryStatus = (EntryStatus)(reader["EntryStatusId"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["UploadStatusId"])))
                                    {
                                        objbatch.UploadStatus = (UploadStatus)(reader["UploadStatusId"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["CreatedOn"])))
                                    {
                                        objbatch.CreatedDate = reader["CreatedOn"] == null ? nullDateTime : Convert.ToDateTime(reader["CreatedOn"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["LicenseeId"])))
                                    {
                                        objbatch.LicenseeId = (Guid)(reader["LicenseeId"]);
                                    }
                                }
                                catch
                                {
                                }

                                batchlst.Add(objbatch);
                            }
                            catch
                            {
                            }

                        }

                        reader.Close();
                    }
                }


            }
            catch
            {
            }
            return batchlst;
        }

        public List<Batch> GetBatchForReportManagerByLicenssID(Guid LicenseeId)
        {
            List<Batch> batchlst = new List<Batch>();
            try
            {
                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                System.Data.EntityClient.EntityConnection ec = (System.Data.EntityClient.EntityConnection)ctx.Connection;
                System.Data.SqlClient.SqlConnection sc = (System.Data.SqlClient.SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                string adoConnStr = sc.ConnectionString;

                DateTime? nullDateTime = null;

                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(adoConnStr))
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Usp_GetBatchListForReportManagerByLicenssID", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LicenseeId", LicenseeId);
                        con.Open();
                        System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();
                        // Call Read before accessing data. 
                        while (reader.Read())
                        {
                            try
                            {
                                Batch objbatch = new Batch();

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["BatchId"])))
                                    {
                                        objbatch.BatchId = reader["BatchId"] == null ? Guid.Empty : (Guid)reader["BatchId"];
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["BatchNumber"])))
                                    {
                                        objbatch.BatchNumber = reader["BatchNumber"] == null ? 0 : Convert.ToInt32(reader["BatchNumber"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["EntryStatusId"])))
                                    {
                                        objbatch.EntryStatus = (EntryStatus)(reader["EntryStatusId"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["UploadStatusId"])))
                                    {
                                        objbatch.UploadStatus = (UploadStatus)(reader["UploadStatusId"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["CreatedOn"])))
                                    {
                                        objbatch.CreatedDate = reader["CreatedOn"] == null ? nullDateTime : Convert.ToDateTime(reader["CreatedOn"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    objbatch.LicenseeId = LicenseeId;
                                }
                                catch
                                {
                                }

                                batchlst.Add(objbatch);
                            }
                            catch
                            {
                            }

                        }

                        reader.Close();
                    }
                }


            }
            catch
            {
            }
            return batchlst;
        }

        public List<Batch> GetBatchesForDeuEntries()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                #region comment for sp
                List<Batch> batches = (from dv in DataModel.Batches
                                           //where dv.IsDeleted == false
                                           //&&
                                       where (dv.UploadStatusId == 3 || dv.UploadStatusId == 4) && 
                                       (dv.EntryStatusId == 1 || dv.EntryStatusId == 3 || dv.EntryStatusId == 4 || dv.EntryStatusId == 5) && (dv.Licensee.IsDeleted == false)
                                       select new Batch
                                       {
                                           BatchNumber = dv.BatchNumber,
                                           BatchId = dv.BatchId,
                                           UploadStatus = (UploadStatus)(dv.UploadStatusId ?? 0),
                                           EntryStatus = (EntryStatus)(dv.EntryStatusId ?? 0),
                                           LicenseeId = dv.LicenseeId,
                                           LicenseeName = dv.Licensee.Company,
                                           FileName = dv.FileName,
                                           SiteId = dv.SiteLoginID,
                                           AssignedDeuUserName = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == dv.AssignedUserCredentialId).UserName,
                                           //AssignedDeuUserName = dv.AssignedUserCredentialId.Value,
                                           CreatedDate = dv.CreatedOn,
                                           LastModifiedDate = dv.LastModifiedDate.Value
                                       }).ToList();

                return batches;
                #endregion

            }
        }

        //public static List<Batch> GetBatchList(UploadStatus? uploadStatus)
        public List<Batch> GetBatchList(UploadStatus? uploadStatus)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                //var batchlst = "";
                var batchlst = (from dv in DataModel.Batches
                                select new Batch
                                {
                                    BatchNumber = dv.BatchNumber,
                                    BatchId = dv.BatchId,
                                    UploadStatus = (UploadStatus)(dv.UploadStatusId ?? 0),
                                    LicenseeId = dv.LicenseeId,
                                    LicenseeName = dv.Licensee.Company,
                                    CreatedDate = dv.CreatedOn ?? DateTime.MinValue,
                                    LastModifiedDate = dv.LastModifiedDate == null ? DateTime.MinValue : dv.LastModifiedDate.Value,
                                    EntryStatus = (EntryStatus)(dv.EntryStatusId ?? 0),
                                    SiteId = dv.SiteLoginID,
                                    PayorId = dv.PayorId,
                                    AssignedDeuUserName = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == dv.AssignedUserCredentialId).UserName,
                                    FileName = dv.FileName
                                }).ToList();

                if (uploadStatus != null)
                {
                    batchlst = batchlst.Where(s => s.UploadStatus == uploadStatus.Value).ToList();
                }
                return batchlst;
            }
        }

        public List<Batch> GetCurrentBatch(Guid licenceID, DateTime CreatedOn)
        {
            if (CreatedOn.Year > 1001)
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    return (from dv in DataModel.Batches
                            where dv.LicenseeId == licenceID && dv.CreatedOn != null && dv.CreatedOn.Value.Year == CreatedOn.Year
                            select new Batch
                            {
                                BatchNumber = dv.BatchNumber,
                                BatchId = dv.BatchId,
                                UploadStatus = (UploadStatus)(dv.UploadStatusId ?? 0),
                                EntryStatus = (EntryStatus)(dv.EntryStatusId ?? 0),
                                LicenseeId = dv.LicenseeId,
                                LicenseeName = dv.Licensee.Company,
                                FileName = dv.FileName,
                                BatchNote = dv.BatchNote,
                                AssignedDeuUserName = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == dv.AssignedUserCredentialId).UserName,
                                //AssignedDeuUserName = dv.AssignedUserCredentialId.Value,
                                CreatedDate = dv.CreatedOn,
                                LastModifiedDate = dv.LastModifiedDate.Value
                            }).ToList();
                }
            }
            else
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.CommandTimeout = 6000000;
                    var batchlst = new List<Batch>();
                    try
                    {
                        batchlst = (from dv in DataModel.Batches
                                    where dv.LicenseeId == licenceID && dv.EntryStatusId != 9 && dv.UploadStatusId != 6
                                    select new Batch
                                    {
                                        BatchNumber = dv.BatchNumber,
                                        BatchId = dv.BatchId,
                                        UploadStatus = (UploadStatus)(dv.UploadStatusId ?? 0),
                                        LicenseeId = dv.LicenseeId,
                                        LicenseeName = dv.Licensee.Company,
                                        CreatedDate = dv.CreatedOn,
                                        LastModifiedDate = dv.LastModifiedDate.Value,
                                        EntryStatus = (EntryStatus)(dv.EntryStatusId ?? 0),
                                        SiteId = dv.SiteLoginID,
                                        PayorId = dv.PayorId,
                                        AssignedDeuUserName = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == dv.AssignedUserCredentialId).UserName,
                                        FileName = dv.FileName
                                    }).ToList();

                    }
                    catch
                    {
                    }
                    if (batchlst != null)
                    {
                        batchlst = batchlst.OrderByDescending(i => i.CreatedDate.Value.Date).ThenByDescending(i => i.BatchNumber).ToList();
                    }
                    return batchlst;
                }

            }

        }

        public List<Batch> GetCurrentBatchForReportBYLicID(Guid licenceID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from dv in DataModel.Batches
                        where dv.LicenseeId == licenceID
                        select new Batch
                        {
                            BatchNumber = dv.BatchNumber,
                            BatchId = dv.BatchId,
                            UploadStatus = (UploadStatus)(dv.UploadStatusId ?? 0),
                            EntryStatus = (EntryStatus)(dv.EntryStatusId ?? 0),
                            LicenseeId = dv.LicenseeId,
                            LicenseeName = dv.Licensee.Company,
                            FileName = dv.FileName,
                            BatchNote = dv.BatchNote,
                            AssignedDeuUserName = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == dv.AssignedUserCredentialId).UserName,
                            CreatedDate = dv.CreatedOn,
                            LastModifiedDate = dv.LastModifiedDate.Value
                        }).ToList();
            }
        }

        //public static ModifiableBatchData CreateModifiableBatchData(DLinq.Batch batch)
        public ModifiableBatchData CreateModifiableBatchData(DLinq.Batch batch)
        {
            ModifiableBatchData batchData = new ModifiableBatchData();
            try
            {
                batchData.BatchId = batch.BatchId;
                batchData.EntryStatus = (EntryStatus)(batch.EntryStatusId ?? 0);
                batchData.LastModifiedDate = batch.LastModifiedDate.Value;
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("CreateModifiableBatchData :" + ex.StackTrace.ToStringDump(), true);
                if (ex.InnerException != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("CreateModifiableBatchData :" + ex.InnerException.ToString(), true);
                }
              //  throw ex;
            }


            return batchData;
        }
        /// <summary>
        /// make the download status of the listed batches to be ........ask pankaj 
        /// </summary>
        public void ClearDownloadStatus()
        {

        }
        /// <summary>        
        /// To Do:  it might be removed to be implemented on the UI code behind. 
        /// don't know. future implementor of this function can better recognize the conditions.
        /// </summary>
        public void LaunchWebSite()
        {

        }

        /// <summary>
        /// make the batch status to be closed.
        /// </summary>
        public bool CloseBatch()
        {
            ActionLogger.Logger.WriteImportPolicyLog("CloseBatch request BatchNumber: " + this.BatchNumber, true);

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                bool deletedSucc = false;

                DLinq.Batch currentBatch = (from p in DataModel.Batches where p.BatchNumber == this.BatchNumber select p).FirstOrDefault();
                if (currentBatch != null)
                {
                    if (currentBatch.Statements.Count != 0)
                    {
                        int Count = currentBatch.Statements.Count(s => s.MasterStatementStatu.StatementStatusId != 2);
                        if (Count == 0)
                        {
                            currentBatch.EntryStatusId = (int)EntryStatus.BatchCompleted;
                            currentBatch.MasterBatchEntryStatu = DataModel.MasterBatchEntryStatus.FirstOrDefault(s => s.BatchEntryStatusId == 6);
                            DataModel.SaveChanges();
                            deletedSucc = true;
                            ActionLogger.Logger.WriteImportPolicyLog("CloseBatch success BatchNumber: " + this.BatchNumber, true);
                        }
                    }
                }
                return deletedSucc;
            }
        }

        public bool OpenBatch(int intBatchNumber)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                bool OpenSucc = false;
                DLinq.Batch currentBatch = (from p in DataModel.Batches where p.BatchNumber == intBatchNumber select p).FirstOrDefault();
                if (currentBatch != null)
                {
                    if (currentBatch.Statements.Count != 0)
                    {
                        #region "Commented code
                        //int Count = currentBatch.Statements.Count(s => s.MasterStatementStatu.StatementStatusId != 2);
                        //if (Count == 0)
                        //{
                        //    currentBatch.EntryStatusId = (int)EntryStatus.InDataEntry;
                        //    currentBatch.MasterBatchEntryStatu = DataModel.MasterBatchEntryStatus.FirstOrDefault(s => s.BatchEntryStatusId == 5);
                        //    DataModel.SaveChanges();
                        //    OpenSucc = true;
                        //}
                        #endregion

                        currentBatch.EntryStatusId = (int)EntryStatus.InDataEntry;
                        currentBatch.MasterBatchEntryStatu = DataModel.MasterBatchEntryStatus.FirstOrDefault(s => s.BatchEntryStatusId == 5);
                        DataModel.SaveChanges();
                        OpenSucc = true;

                    }
                }
                return OpenSucc;
            }
        }

        public string BatchName(int intBatchNumber)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                string strFileName = string.Empty;
                DLinq.Batch currentBatch = (from p in DataModel.Batches where p.BatchNumber == intBatchNumber select p).FirstOrDefault();
                if (currentBatch != null)
                {
                    strFileName = currentBatch.FileName;
                }
                return strFileName;
            }
        }

        public Guid GetBatchID(int intBatchNumber)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                Guid BatchID = Guid.Empty;
                DLinq.Batch currentBatch = (from p in DataModel.Batches where p.BatchNumber == intBatchNumber select p).FirstOrDefault();
                if (currentBatch != null)
                {
                    BatchID = currentBatch.BatchId;
                }
                return BatchID;
            }
        }

        public string BatchNameById(Guid BatchID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                string strFileName = string.Empty;
                DLinq.Batch currentBatch = (from p in DataModel.Batches where p.BatchId == BatchID select p).FirstOrDefault();
                if (currentBatch != null)
                {
                    strFileName = currentBatch.FileName;
                }
                return strFileName;
            }
        }

        /// <summary>
        /// To Do:  it might be removed to be implemented on the UI code behind. 
        /// don't know. future implementor of this function can better recognize the conditions.
        /// purpose : faciltate to open up the pdf/excel file on the client machine to view the content in the file.
        /// gaurav - will be in View Model
        /// </summary>
        public void ViewBatchFile(String FilePath, String FileType)
        {

        }

        /// <summary>
        /// compile and compose a file of all statement and entries for the licensee, and upload to the server specified folder.
        /// Giving specific system generated name to the file. ---
        /// </summary>
        public void UploadBatchOfLocalPayor()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>list of statements included in the batch</returns>
        public List<Statement> GetStatementList(Guid BatchId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                //var result = DataModel.GetStatementList(BatchId);
                //List<Statement> BatchStatements = (from p in result
                //                                   where p.BatchId == BatchId
                //                                   select new Statement
                //                                   {
                //                                       StatementID = p.StatementID,
                //                                       StatementNumber = p.StatementNumber,
                //                                       StatementDate = p.StatementDate.Value,
                //                                       CheckAmount = p.CheckAmount,
                //                                       BalanceForOrAdjustment = p.BalAdj,
                //                                       NetAmount = p.CheckAmount ?? 0 + p.BalAdj ?? 0,
                //                                       CompletePercentage = (double)((p.EnteredAmount ?? 0) / ((p.CheckAmount ?? 0 + p.BalAdj ?? 0) == 0 ? int.MaxValue : (p.CheckAmount ?? 0 + p.BalAdj ?? 0))) * 100,
                //                                       BatchId = p.BatchId.Value,
                //                                       StatusId = p.StatementStatusId,
                //                                       Entries = p.Entries.Value,
                //                                       EnteredAmount = p.EnteredAmount.Value,
                //                                       CreatedDate = p.Createdon.Value,
                //                                       LastModified = p.LastModified.Value,
                //                                       PayorId = p.PayorId.Value,
                //                                       CreatedBy = p.CreatedBy.Value,
                //                                       TemplateID = p.TemplateID,
                //                                       FromPage = p.FromPage,
                //                                       ToPage = p.ToPage,
                //                                       //CreatedByDEU = DataModel.UserDetails.FirstOrDefault(dv => dv.UserCredentialId == p.CreatedBy).LastName ?? "Super"
                //                                       CreatedByDEU = p.LastName,
                //                                   }).ToList();
                #region Comment for sp
                List<Statement> BatchStatements = (from p in DataModel.Statements
                                                   where p.BatchId == BatchId
                                                   select new Statement
                                                   {
                                                       StatementID = p.StatementId,
                                                       StatementNumber = p.StatementNumber,
                                                       StatementDate = p.StatementDate.Value,
                                                       CheckAmount = p.CheckAmount,
                                                       BalanceForOrAdjustment = p.BalAdj,
                                                       NetAmount = p.CheckAmount ?? 0 + p.BalAdj ?? 0,
                                                       CompletePercentage = (double)((p.EnteredAmount ?? 0) / ((p.CheckAmount ?? 0 + p.BalAdj ?? 0) == 0 ? int.MaxValue : (p.CheckAmount ?? 0 + p.BalAdj ?? 0))) * 100,
                                                       BatchId = p.BatchId.Value,
                                                       StatusId = p.MasterStatementStatu.StatementStatusId,
                                                       Entries = p.Entries.Value,
                                                       EnteredAmount = p.EnteredAmount.Value,
                                                       CreatedDate = p.CreatedOn.Value,
                                                       LastModified = p.LastModified.Value,
                                                       PayorId = p.PayorId.Value,
                                                       CreatedBy = p.CreatedBy.Value,
                                                       TemplateID = p.TemplateID,
                                                       FromPage = p.FromPage,
                                                       ToPage = p.ToPage,
                                                       CreatedByDEU = DataModel.UserDetails.FirstOrDefault(dv => dv.UserCredentialId == p.CreatedBy).LastName ?? "Super"
                                                   }).ToList();
                #endregion
                return BatchStatements;
            }
        }

        public List<Statement> GetCheckAmount(Guid? BatchId, Guid? payorID)
        {
            List<Statement> BatchStatements = new List<Statement>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    BatchStatements = (from p in DataModel.Statements
                                       where p.BatchId == BatchId && p.PayorId == payorID
                                       select new Statement
                                       {
                                           StatementID = p.StatementId,
                                           StatementNumber = p.StatementNumber,
                                           StatementDate = p.StatementDate.Value,
                                           CheckAmount = p.CheckAmount

                                       }).ToList();

                }
            }
            catch
            {
            }
            return BatchStatements;
        }
        /// <summary>        
        /// </summary>
        public static void ExportBatch()
        {
        }

        public bool GetBatchPaidStatus(Guid BatchId)
        {
            bool IsBatchPaid = false;

            //List<Statement> _StatementLst = Statement.GetStatementList(BatchId);

            Statement objStatement = new Statement();
            List<Statement> _StatementLst = objStatement.GetStatementList(BatchId);

            foreach (Statement stmt in _StatementLst)
            {
                List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPostLst = PolicyPaymentEntriesPost.GetPolicyPaymentEntryStatementWise(stmt.StatementID);
                foreach (PolicyPaymentEntriesPost ppep in _PolicyPaymentEntriesPostLst)
                {
                    List<PolicyOutgoingDistribution> _PolicyOutgoingDistributionLst = PolicyOutgoingDistribution.GetOutgoingPaymentByPoicyPaymentEntryId(ppep.PaymentEntryID);
                    IsBatchPaid = _PolicyOutgoingDistributionLst.All(p => p.IsPaid == true);
                }
            }
            return IsBatchPaid;
        }

        #region "Add Set as paid"
        /// <summary>
        /// ModifiedBy:Ankit khandelwal  
        /// ModifiedOn:13-11-2018
        /// Purpose:Handle exception when occurs in Getting policypaymentEntries.
        /// </summary>
        /// <param name="BatchId"></param>
        /// <returns></returns>
        public int GetBatchStatusMarkPaid(Guid BatchId)
        {            //just assign an numeric value
            int IsBatchPaid = 10;
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetBatchStatus:Processing begins with BatchId  : " + BatchId, true);
            try
            {
                Statement objStatement = new Statement();
                List<Statement> _StatementLst = objStatement.GetStatementList(BatchId);

                if (_StatementLst.Count > 0)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetBatchStatus: getting Count of statement for BatchId:" + BatchId + " " + "Statement Count" + _StatementLst.Count, true);
                    foreach (Statement stmt in _StatementLst)
                    {
                        List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPostLst = PolicyPaymentEntriesPost.GetPolicyPaymentEntryStatementWise(stmt.StatementID);
                        foreach (PolicyPaymentEntriesPost ppep in _PolicyPaymentEntriesPostLst)
                        {
                            List<PolicyOutgoingDistribution> _PolicyOutgoingDistributionLst = PolicyOutgoingDistribution.GetOutgoingPaymentByPoicyPaymentEntryId(ppep.PaymentEntryID);

                            List<PolicyOutgoingDistribution> _PolicyPaid = new List<PolicyOutgoingDistribution>(_PolicyOutgoingDistributionLst.Where(p => p.IsPaid == true));

                            List<PolicyOutgoingDistribution> _PolicyUnPaid = new List<PolicyOutgoingDistribution>(_PolicyOutgoingDistributionLst.Where(p => p.IsPaid == false));

                            if (_PolicyPaid.Count > 0 && _PolicyUnPaid.Count > 0)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "PolicyUnPaid count: " + _PolicyUnPaid.Count, true);
                                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "PolicyPaid count: " + _PolicyPaid.Count, true);
                                IsBatchPaid = 2;
                                return IsBatchPaid;
                            }
                            else if (_PolicyPaid.Count > 0)
                            {
                                IsBatchPaid = 0;
                            }
                            else if (_PolicyUnPaid.Count > 0)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "PolicyUnPaid count: " + _PolicyUnPaid.Count, true);
                                IsBatchPaid = 1;
                                return IsBatchPaid;
                            }
                            else
                            {
                                IsBatchPaid = 3;
                                return IsBatchPaid;
                            }
                        }
                        if (_PolicyPaymentEntriesPostLst.Count == 0 || _PolicyPaymentEntriesPostLst == null)
                        {
                            IsBatchPaid = 4;
                            break;
                        }
                    }
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetBatchStatus _StatementLst  processed : " + _StatementLst.Count, true);
                }
                else
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetBatchStatus no statement list ", true);
                    IsBatchPaid = 10;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetBatchStatus:Exception occur while getting BatchStatus:" + ex.Message, true);
            }
            return IsBatchPaid;
        }


        #region "Add Set as paid"
        public int GetBatchStatus(Guid BatchId)
        {            //just assign an numeric value
            int IsBatchPaid = 10;
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetBatchStatus started  : " + BatchId, true);
            try
            {
                //List<Statement> _StatementLst = Statement.GetStatementList(BatchId);
                //Added instance method
                Statement objStatement = new Statement();
                List<Statement> _StatementLst = objStatement.GetStatementList(BatchId);

                if (_StatementLst.Count > 0)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetBatchStatus _StatementLst  : " + _StatementLst.Count, true);
                    foreach (Statement stmt in _StatementLst)
                    {
                        List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPostLst = PolicyPaymentEntriesPost.GetPolicyPaymentEntryStatementWise(stmt.StatementID);
                        foreach (PolicyPaymentEntriesPost ppep in _PolicyPaymentEntriesPostLst)
                        {
                            List<PolicyOutgoingDistribution> _PolicyOutgoingDistributionLst = PolicyOutgoingDistribution.GetOutgoingPaymentByPoicyPaymentEntryId(ppep.PaymentEntryID);

                            List<PolicyOutgoingDistribution> _PolicyPaid = new List<PolicyOutgoingDistribution>(_PolicyOutgoingDistributionLst.Where(p => p.IsPaid == true));

                            List<PolicyOutgoingDistribution> _PolicyUnPaid = new List<PolicyOutgoingDistribution>(_PolicyOutgoingDistributionLst.Where(p => p.IsPaid == false));

                            if (_PolicyPaid.Count > 0 && _PolicyUnPaid.Count > 0)
                            {
                                IsBatchPaid = 2;
                                return IsBatchPaid;
                            }
                            else if (_PolicyPaid.Count > 0)
                            {
                                IsBatchPaid = 0;
                            }
                            else if (_PolicyUnPaid.Count > 0)
                            {
                                IsBatchPaid = 1;
                            }
                        }
                    }
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetBatchStatus _StatementLst  processed : " + _StatementLst.Count, true);
                }
                else
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "GetBatchStatus no statement list ", true);
                    IsBatchPaid = 10;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetBatchStatus exception: " + ex.Message, true);
            }
            return IsBatchPaid;
        }

        /// <summary>
        /// Acme - Method to set batch status from report interface
        /// and return suitable message 
        /// </summary>
        /// <param name="BatchIds"></param>
        /// <param name="strFilter"></param>
        /// <param name="lstPayee"></param>
        /// <returns></returns>
        public string SetBatchesToPaidInReports(List<Guid> BatchIds, string strFilter, List<Guid> lstPayee)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "----- SetBatchesToPaidInReports starting ------", true);
            // string msg = "Marking Batches as Paid operation Successfully Completed.";
            string msg = "success";
            string msgFail = "Marking Batches as Paid operation is failed due to some reason. Please try again or contact administrator.";
            //string msgFail = "";
            string failedBatches = string.Empty;
            string allBatches = string.Empty;
            Dictionary<string, string> lstFailed = new Dictionary<string, string>();

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                foreach (Guid id in BatchIds)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "Processing Batch: " + id.ToString(), true);
                    DLinq.Batch batch = DataModel.Batches.Where(s => s.BatchId == id).FirstOrDefault();
                    allBatches += batch.BatchNumber + "\n";
                    try
                    {
                        if (SetToPaid(id, strFilter, lstPayee, DataModel))
                        {
                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "SetToPaid success for Batch: " + id.ToString(), true);
                            try
                            {
                                int intValue = GetBatchStatusMarkPaid(id);
                                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "Batch status found as  : " + intValue, true);
                                if (intValue == 2 || intValue == 1)
                                {
                                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "Batch is Unpaid/PartialPaid", true);
                                    //failedBatches += batch.BatchNumber + "\n";
                                    if (intValue == 2)
                                    {
                                        batch.EntryStatusId = (int)EntryStatus.PartialUnpaid;
                                    }
                                    //    operationSuccessfullyCompleted = false;
                                }
                                else if (intValue == 0)
                                {
                                    batch.EntryStatusId = (int)EntryStatus.Paid;
                                }
                                else if (intValue == 3 || intValue == 4 || intValue == 10)
                                {
                                    failedBatches += batch.BatchNumber + "\n";
                                    if (intValue == 3)
                                    {
                                        lstFailed.Add(Convert.ToString(batch.BatchNumber), "Failed with status 3: Possibility of missing outgoing with a payment entry");
                                    }
                                    else if (intValue == 4)
                                    {
                                        lstFailed.Add(Convert.ToString(batch.BatchNumber), "Failed with status 4: Payment entries list is blank for some statement");
                                    }
                                    else if (intValue == 10)
                                    {
                                        lstFailed.Add(Convert.ToString(batch.BatchNumber), "Failed with status 10: Statement list is blank");
                                    }

                                    ActionLogger.Logger.WriteImportPolicyLog("SetBatchesToPaidInReports: exception occur in GetOutgoingPaymentByPoicyPaymentEntryId  Batch# " + id + ",  " + batch.BatchNumber, true);
                                }
                                DataModel.SaveChanges();
                                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "Batch : " + id.ToString() + " updated successfully", true);
                            }
                            catch (Exception ex)
                            {
                                failedBatches += batch.BatchNumber + "\n";
                                lstFailed.Add(Convert.ToString(batch.BatchNumber), "Exception occurred: " + ex.Message);
                                ActionLogger.Logger.WriteImportPolicyLog("SetBatchesToPaid exception : Batch # " + id + ",  " + ex.Message, true);
                            }
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "SetToPaid failure for Batch: " + id.ToString(), true);
                            //Send internal email
                            //     operationSuccessfullyCompleted = false;
                            failedBatches += batch.BatchNumber + "\n";
                            lstFailed.Add(Convert.ToString(batch.BatchNumber), "SetToPaid process failed");
                            //string body = "An issue occurred while setting batch to Paid with Batch# " + batch.BatchNumber + ".";
                            //MailServerDetail.sendMail("jyotisna@acmeminds.com", "Commissions exception: Batch# " + batch.BatchNumber, body);
                        }
                    }
                    catch (Exception ex)
                    {
                        //     operationSuccessfullyCompleted = false;
                        failedBatches += batch.BatchNumber + "\n";
                        lstFailed.Add(Convert.ToString(batch.BatchNumber), "Exception occurred in main catch: " + ex.Message);
                        ActionLogger.Logger.WriteImportPolicyLog("SetBatchesToPaid exception : Batch # " + id + ",  " + ex.Message, true);
                    }
                }

            }
            if (string.IsNullOrEmpty(failedBatches))
            {
                ActionLogger.Logger.WriteImportPolicyLog("No FailedBatches ", true);
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "----- SetBatchesToPaidInReports Ending ------", true);
                return msg;
            }
            else
            {
                ActionLogger.Logger.WriteImportPolicyLog("FailedBatches found:  " + failedBatches, true);
                msgFail = msgFail + "<br><br>Failed Batches: <br>" + failedBatches;
                string body = "An issue occurred while setting batch to Paid with following Batch# <br>" + failedBatches;
                foreach (var item in lstFailed)
                {
                    body += item.Key + " - " + item.Value + "<br>";
                }

                body += "<br>Request Details are as follows: ";
                body += "<br>Batches: <br>" + allBatches;
                body += "Filter: " + strFilter;
                body += "<br>Payees: " + string.Join(", ", lstPayee.ToArray());
                MailServerDetail.sendMailtodev("deudev@acmeminds.com", "Commissions error in changing batch status ", body);
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "----- SetBatchesToPaidInReports Ending ------", true);
                return msgFail;
            }
        }

        public bool SetBatchesToPaid(List<Guid> BatchIds, string strFilter, List<Guid> lstPayee)
        {
            bool operationSuccessfullyCompleted = true;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                foreach (Guid id in BatchIds)
                {
                    DLinq.Batch batch = DataModel.Batches.Where(s => s.BatchId == id).FirstOrDefault();
                    try
                    {
                        if (SetToPaid(id, strFilter, lstPayee, DataModel))
                        {
                            try
                            {

                                int intValue = GetBatchStatus(id);
                                if (intValue == 2)
                                {
                                    batch.EntryStatusId = (int)EntryStatus.PartialUnpaid;
                                    operationSuccessfullyCompleted = false;
                                }
                                else if (intValue == 0)
                                {
                                    batch.EntryStatusId = (int)EntryStatus.Paid;
                                }
                                else if (intValue == 1)
                                {
                                    operationSuccessfullyCompleted = false;
                                }
                                DataModel.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                //Send internal email
                                operationSuccessfullyCompleted = false;
                                string body = "An exction has occurred while setting batch to Paid. Batch# " + batch.BatchNumber + ", exception occurred: " + ex.Message;
                                MailServerDetail.sendMail("jyotisna@acmeminds.com", "Commissions exception: Batch# " + batch.BatchNumber, body);
                            }
                        }
                        else
                        {
                            //Send internal email
                            operationSuccessfullyCompleted = false;
                            string body = "An issue occurred while setting batch to Paid with Batch# " + batch.BatchNumber + ".";
                            MailServerDetail.sendMail("jyotisna@acmeminds.com", "Commissions exception: Batch# " + batch.BatchNumber, body);
                        }
                    }
                    catch (Exception ex)
                    {
                        operationSuccessfullyCompleted = false;
                        ActionLogger.Logger.WriteImportPolicyLog("SetBatchesToPaid exception : Batch # " + id + ",  " + ex.Message, true);
                    }
                }

            }

            return operationSuccessfullyCompleted;
            //TransactionOptions options = new TransactionOptions
            //{
            //    IsolationLevel = IsolationLevel.ReadCommitted,
            //    Timeout = TimeSpan.FromMinutes(5)
            //};
            /*
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                bool operationSuccessfullyCompleted = true;
                try
                {
                    //using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, options))
                    //{
                    foreach (Guid id in BatchIds)
                    {

                        if (!SetToPaid(id, strFilter, lstPayee, DataModel))
                        {
                            operationSuccessfullyCompleted = false;
                            break;
                        }
                    }

                    if (operationSuccessfullyCompleted)
                    {
                        DataModel.SaveChanges();

                        foreach (Guid id in BatchIds)
                        {
                            DLinq.Batch batch = DataModel.Batches.Where(s => s.BatchId == id).FirstOrDefault();
                            //if (batch != null && batch.EntryStatusId < (int)EntryStatus.Paid)
                            //{ 
                            if (batch != null)
                            {
                                //Check batch status            
                                int intValue = GetBatchStatus(id);
                                if (intValue == 2)
                                {
                                    batch.EntryStatusId = (int)EntryStatus.PartialUnpaid;
                                }
                                //else if (intValue == 1)
                                //{
                                //    batch.EntryStatusId = (int)EntryStatus.BatchCompleted;
                                //}
                                else if (intValue == 0)
                                {
                                    batch.EntryStatusId = (int)EntryStatus.Paid;
                                }

                            }
                        }

                        DataModel.SaveChanges();
                        //}
                        //transaction.Complete();
                    }
                }
                catch
                {
                    operationSuccessfullyCompleted = false;
                }

                return operationSuccessfullyCompleted;
            }*/
        }

        private static bool SetToPaid(Guid batchId, string strFilter, List<Guid> lstPayee, DLinq.CommissionDepartmentEntities DataModel)
        {
            bool operationSuccessfullyCompleted = true;
            try
            {
                DLinq.Batch batch = DataModel.Batches.Where(s => s.BatchId == batchId).FirstOrDefault();
                //if (batch != null)
                //{
                //    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "SetToPaid starting for Batch: " + batch.BatchNumber.ToString(), true);
                //    foreach (DLinq.Statement stmt in batch.Statements)
                //    {
                //        foreach (DLinq.PolicyPaymentEntry ppep in stmt.PolicyPaymentEntries)
                //        {
                //            foreach (var item in ppep.PolicyOutgoingPayments)
                //            {
                //                    foreach (var payee in lstPayee)
                //                    {
                //                        if (item.RecipientUserCredentialId == (Guid)payee)
                //                        {
                //                            item.IsPaid = true;
                //                        }

                //                    }
                //            }
                //        }
                //    }

                //    DataModel.SaveChanges();
                //    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "SetToPaid success for Batch: " + batch.BatchNumber.ToString(), true);
                //}
                if (batch != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "SetToPaid starting for Batch: " + batch.BatchNumber.ToString(), true);
                    foreach (DLinq.Statement stmt in batch.Statements)
                    {
                        foreach (DLinq.PolicyPaymentEntry ppep in stmt.PolicyPaymentEntries)
                        {
                            try
                            {
                                foreach (var item in ppep.PolicyOutgoingPayments)
                                {
                                    try
                                    {
                                        foreach (var payee in lstPayee)
                                        {
                                            if (item.RecipientUserCredentialId == (Guid)payee)
                                            {
                                                item.IsPaid = true;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "Exception occur while settopaid  for Batch: " + batch.BatchNumber.ToString(), true);
                                        throw ex;
                                    }

                                }
                                DataModel.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "SetToPaid:Exception occured while set to paid for policypaymententry: " + ppep + " " + "Exception: " + ex.Message, true);
                                operationSuccessfullyCompleted = false;
                            }
                        }
                        ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + "SetToPaid success for Batch: " + batch.BatchNumber.ToString(), true);
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("SetToPaid exception: BatchID ->" + batchId + ", strfilter: " + strFilter + ", exception: " + ex.Message, true);
                operationSuccessfullyCompleted = false;
            }


            return operationSuccessfullyCompleted;
        }
        #endregion "Add Set as paid"

        public bool SetBatchesAsPaid(List<Guid> BatchIds)
        {
            TransactionOptions options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(5)
            };

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                bool operationSuccessfullyCompleted = true;
                try
                {
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, options))
                    {
                        foreach (Guid id in BatchIds)
                        {

                            if (!SetAsPaid(id, DataModel))
                            {
                                operationSuccessfullyCompleted = false;
                                break;
                            }
                        }

                        if (operationSuccessfullyCompleted)
                        {
                            foreach (Guid id in BatchIds)
                            {
                                DLinq.Batch batch = DataModel.Batches.Where(s => s.BatchId == id).FirstOrDefault();
                                //if (batch != null && batch.EntryStatusId < (int)EntryStatus.Paid)
                                //{
                                if (batch != null)
                                {
                                    batch.EntryStatusId = (int)EntryStatus.Paid;
                                }
                            }
                            DataModel.SaveChanges();
                        }
                        transaction.Complete();
                    }
                }
                catch
                {
                    operationSuccessfullyCompleted = false;
                }

                return operationSuccessfullyCompleted;
            }
        }

        //private static bool SetAsPaid(Guid batchId, DLinq.CommissionDepartmentEntities DataModel)
        private bool SetAsPaid(Guid batchId, DLinq.CommissionDepartmentEntities DataModel)
        {
            bool operationSuccessfullyCompleted = true;
            try
            {
                DLinq.Batch batch = DataModel.Batches.Where(s => s.BatchId == batchId).FirstOrDefault();
                //if (batch != null && batch.EntryStatusId < (int)EntryStatus.Paid)
                //{
                if (batch != null)
                {
                    foreach (DLinq.Statement stmt in batch.Statements)
                    {
                        foreach (DLinq.PolicyPaymentEntry ppep in stmt.PolicyPaymentEntries)
                        {
                            ppep.PolicyOutgoingPayments.ToList().ForEach(p => p.IsPaid = true);
                        }
                    }
                }
                //}
            }
            catch (Exception)
            {
                operationSuccessfullyCompleted = false;
            }

            return operationSuccessfullyCompleted;
        }

        public bool SetBatchToUnPaidStatus(int BatchNumber)
        {
            ActionLogger.Logger.WriteImportPolicyLog("SetBatchToUnPaidStatus request: " + BatchNumber, true);

            bool isValue = false;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.Batch batch = DataModel.Batches.Where(s => s.BatchNumber == BatchNumber).FirstOrDefault();

                    if (batch != null)
                    {
                        if (batch.BatchId != null)
                        {
                            Batch objBatch = new Batch();
                            int intValue = objBatch.GetBatchStatus(batch.BatchId);
                            if (intValue == 2)
                            {
                                batch.EntryStatusId = (int)EntryStatus.PartialUnpaid;
                            }
                            else if (intValue == 0)
                            {
                                batch.EntryStatusId = (int)EntryStatus.Paid;
                            }
                            DataModel.SaveChanges();
                            isValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isValue = false;
                ActionLogger.Logger.WriteImportPolicyLog("SetBatchToUnPaidStatus exception: " + ex.Message, true);
            }

            return isValue;
        }
    }

    [DataContract]
    public class BatchAddOutput
    {
        [DataMember]
        public int BatchNumber { get; set; }
        [DataMember]
        public string LicenseeName { get; set; }
    }

    [DataContract]
    public class DownloadBatch
    {
        [DataMember]
        public Guid BatchId { get; set; }
        [DataMember]
        public int BatchNumber { get; set; }
        [DataMember]
        public Guid PayorId { get; set; }
        [DataMember]
        public string PayorName { get; set; }
        [DataMember]
        public string LicenseeName { get; set; }
        [DataMember]
        public Guid LicenseeId { get; set; }
        [DataMember]
        public string FileType { get; set; }
        [DataMember]
        public UploadStatus UploadStatus { get; set; }
        [DataMember]
        public EntryStatus EntryStatus { get; set; }
        [DataMember]
        public Guid? SiteId { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        //public static List<DownloadBatch> GetDownloadBatchList()
        public List<DownloadBatch> GetDownloadBatchList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DownloadBatch> downloadBatches = (from dv in DataModel.Batches
                                                       where (dv.UploadStatusId == 1 || dv.UploadStatusId == 2 || dv.UploadStatusId == 3)
                                                       select new DownloadBatch
                                                       {
                                                           BatchNumber = dv.BatchNumber,
                                                           BatchId = dv.BatchId,
                                                           UploadStatus = (UploadStatus)(dv.UploadStatusId ?? 0),
                                                           LicenseeName = dv.Licensee.Company,
                                                           LicenseeId = dv.LicenseeId,
                                                           CreatedDate = dv.CreatedOn,
                                                           SiteId = dv.SiteLoginID,
                                                           FileType = dv.FileType,
                                                           EntryStatus = (EntryStatus)(dv.EntryStatusId ?? 0)
                                                       }).ToList();

                foreach (DownloadBatch batch in downloadBatches)
                {
                    DLinq.Batch myBatch = DataModel.Batches.Where(s => s.BatchId == batch.BatchId).FirstOrDefault();
                    batch.PayorId = myBatch.PayorId.Value;
                    batch.PayorName = myBatch.Payor.PayorName;
                }

                return downloadBatches;
            }
        }
        /// </summary>
        public void DeleteDownloadBatch(UserRole _UserRole)
        {
            Batch objBatch = new Batch();
            objBatch.DeleteBatch(this.BatchId, _UserRole);
        }

        public ImportFileData ImportBatchFile(UserRole _UserRole)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                ImportFileData ImportData = new ImportFileData { BatchId = this.BatchId, BatchNumber = this.BatchNumber, IsImportedSuccessfully = false };
                DLinq.Template template = DataModel.Templates.FirstOrDefault(s => s.PayorID == this.PayorId);
                bool IsImportedSuccessfully = false;
                DLinq.Statement statement = null;
                decimal CheckAmount = 0;
                List<Guid> DeuEntryIds = null;

                try
                {
                    if (template != null)
                    {
                        ExcelUtility excelUtility = new ExcelUtility(@"D:\123.xls", template);
                        excelUtility.Reset();
                        string Data = string.Empty;
                        string[] excelData = null;

                        while ((Data = excelUtility.Read()) != string.Empty)
                        {
                            excelData = Data.Split('#');
                            CheckAmount += SaveAutoDeuData(excelData, template.FieldMappings.ToList());
                        }
                        excelUtility.Dispose();

                        statement = new DLinq.Statement();
                        statement.BatchId = this.BatchId;
                        statement.PayorId = this.PayorId;
                        statement.StatementDate = DateTime.Today;
                        statement.CheckAmount = CheckAmount;
                        statement.BalAdj = 0;
                        statement.Entries = 0;
                        statement.CreatedOn = DateTime.Now;
                        statement.EnteredAmount = 0;
                        statement.LastModified = DateTime.Now;
                        statement.StatementId = Guid.NewGuid();
                        statement.StatementStatusId = 0;
                        statement.CreatedBy = DataModel.UserCredentials.FirstOrDefault(p => p.RoleId == 1).UserCredentialId;
                        DataModel.Statements.AddObject(statement);
                        DataModel.SaveChanges();

                        DeuEntryIds = SaveDeuEntries(statement);

                        List<PostProcessReturnStatus> statuses = GenricMapperPost.GenricMapperPostStart(DeuEntryIds, _UserRole);

                        IsImportedSuccessfully = true;
                        if (DeuEntryIds.Count != statuses.Count)
                            IsImportedSuccessfully = false;
                        else
                        {
                            bool IncompleteExist = statuses.Exists(s => s.IsComplete == false);

                            if (IncompleteExist)
                                IsImportedSuccessfully = false;
                            else
                                IsImportedSuccessfully = true;
                        }

                    }

                    if (IsImportedSuccessfully)
                    {
                        statement.EnteredAmount = CheckAmount;
                        statement.Entries = statement.EntriesByDEUs.Count;
                        statement.StatementStatusId = 2;

                        DLinq.Batch batch = DataModel.Batches.FirstOrDefault(s => s.BatchId == this.BatchId);
                        batch.EntryStatusId = 6;

                        DataModel.SaveChanges();
                    }
                    else
                    {
                        DeleteDeuEntries(DeuEntryIds);
                        DataModel.Statements.DeleteObject(statement);

                        DLinq.Batch batch = DataModel.Batches.FirstOrDefault(s => s.BatchId == this.BatchId);
                        batch.EntryStatusId = 3;
                        DataModel.SaveChanges();
                    }
                }
                catch
                {

                }

                ImportData.IsImportedSuccessfully = IsImportedSuccessfully;
                return ImportData;
            }
        }

        private void DeleteDeuEntries(List<Guid> DeuEntryIds)
        {
            if (DeuEntryIds == null)
                return;

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                foreach (Guid id in DeuEntryIds)
                {
                    DLinq.EntriesByDEU deuEntry = DataModel.EntriesByDEUs.FirstOrDefault(s => s.DEUEntryID == id);
                    if (deuEntry != null)
                        DataModel.EntriesByDEUs.DeleteObject(deuEntry);
                }
                DataModel.SaveChanges();
            }
        }

        private List<Guid> SaveDeuEntries(DLinq.Statement statement)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.AutoDEUEntry> entries = DataModel.AutoDEUEntries.Where(s => s.BatchId == this.BatchId).ToList();

                List<Guid> DeuEntryIds = new List<Guid>();
                foreach (DLinq.AutoDEUEntry autoEntry in entries)
                {
                    DLinq.EntriesByDEU deuEntry = new EntriesByDEU();

                    deuEntry.StatementID = statement.StatementId;
                    deuEntry.PayorId = statement.PayorId;
                    deuEntry.PolicyNumber = BLHelper.CorrectPolicyNo(autoEntry.PolicyNumber);
                    deuEntry.Insured = autoEntry.Insured;
                    deuEntry.OriginalEffectiveDate = autoEntry.OriginalEffectiveDate;
                    deuEntry.InvoiceDate = autoEntry.InvoiceDate;
                    deuEntry.PaymentReceived = autoEntry.PaymentReceived;
                    deuEntry.CommissionPercentage = autoEntry.CommissionPercentage;
                    deuEntry.Renewal = autoEntry.Renewal;
                    deuEntry.Enrolled = autoEntry.Enrolled;
                    deuEntry.Eligible = autoEntry.Eligible;
                    deuEntry.SplitPer = autoEntry.SplitPer;

                    deuEntry.PolicyModeValue = autoEntry.PolicyModeValue;
                    deuEntry.PolicyModeID = BLHelper.GetPolicyMode(deuEntry.PolicyModeValue);

                    deuEntry.CarrierId = BLHelper.GetCarrierId(autoEntry.Carrier, this.PayorId);
                    deuEntry.CoverageId = BLHelper.GetProductId(autoEntry.Coverage, this.PayorId, deuEntry.CarrierId);

                    deuEntry.PayorSysID = autoEntry.PayorSysID;
                    deuEntry.CompScheduleType = autoEntry.CompScheduleType;

                    deuEntry.ClientValue = autoEntry.ClientValue;
                    deuEntry.ClientID = BLHelper.GetClientId(deuEntry.ClientValue, this.LicenseeId);

                    deuEntry.NumberOfUnits = autoEntry.NumberOfUnits;
                    deuEntry.DollerPerUnit = autoEntry.DollerPerUnit;
                    deuEntry.Fee = autoEntry.Fee;
                    deuEntry.Bonus = autoEntry.Bonus;
                    deuEntry.CommissionTotal = autoEntry.CommissionTotal;

                    deuEntry.DEUEntryID = Guid.NewGuid();
                    DataModel.EntriesByDEUs.AddObject(deuEntry);

                    DeuEntryIds.Add(deuEntry.DEUEntryID);
                }
                DataModel.SaveChanges();
                return DeuEntryIds;
            }
        }
        /// <param name="Data"></param>
        /// <param name="FieldMapping"></param>
        public decimal SaveAutoDeuData(string[] Data, List<FieldMapping> FieldMapping)
        {
            int DataIndex = 0;
            bool IsValidEntry = true;

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                AutoDEUEntry DeuEntry = new AutoDEUEntry();

                DeuEntry.Id = Guid.NewGuid();
                DeuEntry.BatchId = this.BatchId;
                DeuEntry.PaymentReceived = 0;
                DeuEntry.CommissionPercentage = 0;
                DeuEntry.NumberOfUnits = 0;
                DeuEntry.DollerPerUnit = 0;
                DeuEntry.Fee = 0;
                DeuEntry.Bonus = 0;
                DeuEntry.CommissionTotal = 0;
                DeuEntry.SplitPer = 100;

                foreach (FieldMapping field in FieldMapping)
                {
                    switch (field.DBFieldName)
                    {
                        case "PolicyNumber":
                            DeuEntry.PolicyNumber = Data[DataIndex];
                            break;

                        case "Insured":
                            DeuEntry.Insured = Data[DataIndex];
                            break;

                        case "OriginalEffectiveDate":
                            //DateTime date1 = DateTime.ParseExact(Data[DataIndex],field.ExcelFieldFormat, DateTimeFormatInfo.InvariantInfo);
                            DateTime date1 = DateTime.ParseExact(Data[DataIndex], "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
                            DeuEntry.OriginalEffectiveDate = date1;
                            break;

                        case "InvoiceDate":
                            //DateTime date2 = DateTime.ParseExact(Data[DataIndex], field.ExcelFieldFormat, DateTimeFormatInfo.InvariantInfo);
                            DateTime date2 = DateTime.ParseExact(Data[DataIndex], "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
                            DeuEntry.InvoiceDate = date2;
                            break;

                        case "PaymentReceived":
                            if (string.IsNullOrEmpty(Data[DataIndex]))
                                DeuEntry.PaymentReceived = 0;
                            else
                            {
                                string value = Data[DataIndex];
                                value = value.Replace("$", "");
                                value = value.Replace(",", "");
                                value = value.Replace("%", "");
                                DeuEntry.PaymentReceived = decimal.Parse(value);
                            }
                            break;

                        case "CommissionPercentage":
                            if (string.IsNullOrEmpty(Data[DataIndex]))
                                DeuEntry.CommissionPercentage = 0;
                            else
                            {
                                string value = Data[DataIndex];
                                value = value.Replace("$", "");
                                value = value.Replace(",", "");
                                value = value.Replace("%", "");
                                DeuEntry.CommissionPercentage = double.Parse(value);
                            }
                            break;

                        case "Renewal":
                            DeuEntry.Renewal = Data[DataIndex];
                            break;

                        case "Enrolled":
                            DeuEntry.Enrolled = Data[DataIndex];
                            break;

                        case "Eligible":
                            DeuEntry.Eligible = Data[DataIndex];
                            break;

                        case "SplitPercentage":
                            if (string.IsNullOrEmpty(Data[DataIndex]))
                                DeuEntry.SplitPer = 100;
                            else
                            {
                                string value = Data[DataIndex];
                                value = value.Replace("$", "");
                                value = value.Replace(",", "");
                                value = value.Replace("%", "");
                                DeuEntry.SplitPer = double.Parse(value);
                            }
                            break;

                        case "PolicyMode":
                            DeuEntry.PolicyModeValue = Data[DataIndex];
                            break;

                        case "Carrier":
                            DeuEntry.Carrier = Data[DataIndex];
                            IsValidEntry = Carrier.IsValidCarrier(DeuEntry.Carrier, this.PayorId);
                            break;

                        case "Product":
                            DeuEntry.Coverage = Data[DataIndex];
                            IsValidEntry = Coverage.IsValidCoverage(DeuEntry.Carrier, DeuEntry.Coverage, this.PayorId);
                            break;

                        case "PayorSysId":
                            DeuEntry.PayorSysID = Data[DataIndex];
                            break;

                        case "CompScheduleType":
                            if (string.IsNullOrEmpty(Data[DataIndex]))
                                DeuEntry.CompScheduleType = null;
                            else
                                DeuEntry.CompScheduleType = Data[DataIndex];
                            break;

                        //case "CompType":
                        //    if (string.IsNullOrEmpty(Data[DataIndex]))
                        //        DeuEntry.CompTypeID = null;
                        //    else
                        //        DeuEntry.CompTypeID = DEU.getCompTypeId(field.DeuFieldValue);
                        //    deuData.CompTypeID = DeuEntry.CompTypeID;
                        //    break;

                        case "Client":
                            DeuEntry.ClientValue = Data[DataIndex];
                            break;

                        case "NumberOfUnits":
                            if (string.IsNullOrEmpty(Data[DataIndex]))
                                DeuEntry.NumberOfUnits = 0;
                            else
                            {
                                string value = Data[DataIndex];
                                value = value.Replace("$", "");
                                value = value.Replace(",", "");
                                value = value.Replace("%", "");
                                DeuEntry.NumberOfUnits = int.Parse(value);
                            }
                            break;

                        case "DollerPerUnit":
                            if (string.IsNullOrEmpty(Data[DataIndex]))
                                DeuEntry.DollerPerUnit = 0;
                            else
                            {
                                string value = Data[DataIndex];
                                value = value.Replace("$", "");
                                value = value.Replace(",", "");
                                value = value.Replace("%", "");

                                DeuEntry.DollerPerUnit = decimal.Parse(value);
                            }
                            break;

                        case "Fee":
                            if (string.IsNullOrEmpty(Data[DataIndex]))
                                DeuEntry.Fee = 0;
                            else
                            {
                                string value = Data[DataIndex];
                                value = value.Replace("$", "");
                                value = value.Replace(",", "");
                                value = value.Replace("%", "");

                                DeuEntry.Fee = decimal.Parse(value);
                            }
                            break;

                        case "Bonus":
                            if (string.IsNullOrEmpty(Data[DataIndex]))
                                DeuEntry.Bonus = 0;
                            else
                            {
                                string value = Data[DataIndex];
                                value = value.Replace("$", "");
                                value = value.Replace(",", "");
                                value = value.Replace("%", "");

                                DeuEntry.Bonus = decimal.Parse(value);
                            }
                            break;

                        case "CommissionTotal":
                            if (string.IsNullOrEmpty(Data[DataIndex]))
                                DeuEntry.CommissionTotal = 0;
                            else
                            {
                                string value = Data[DataIndex];
                                value = value.Replace("$", "");
                                value = value.Replace(",", "");
                                value = value.Replace("%", "");

                                DeuEntry.CommissionTotal = decimal.Parse(value);
                            }
                            break;

                    }

                    if (!IsValidEntry)
                        break;

                    DataIndex++;
                }

                if (!IsValidEntry)
                {
                    MailServerDetail.sendMail(string.Empty, "Nickname is not found in system", "");
                    throw new OperationCanceledException();
                }

                DataModel.AutoDEUEntries.AddObject(DeuEntry);
                DataModel.SaveChanges();

                return DeuEntry.CommissionTotal.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool isBatchPartiallyOrFullyPaid()
        {
            bool PartiallyOrFullyPaid = false;

            //List<Statement> statements = Statement.GetStatementList(this.BatchId);

            //Added instance method
            Statement objStatement = new Statement();
            List<Statement> statements = objStatement.GetStatementList(this.BatchId);

            if (statements != null)
            {
                foreach (Statement statement in statements)
                {
                    bool paid = statement.IsStatementPartiallyOrFullyPaid();
                    if (paid)
                    {
                        PartiallyOrFullyPaid = true;
                        break;
                    }
                }
            }
            return PartiallyOrFullyPaid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EntryStatus"></param>
        /// <returns></returns>
        public DateTime? UpdateEntryStatus()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.Batch batch = DataModel.Batches.FirstOrDefault(s => s.BatchId == this.BatchId);
                batch.EntryStatusId = (int)this.EntryStatus;
                batch.UploadStatusId = (int)this.UploadStatus;

                if (batch.UploadStatusId == 3 && batch.CreatedOn == null)
                    batch.CreatedOn = DateTime.Now;

                DataModel.SaveChanges();
                return batch.CreatedOn;
            }
        }

        public void ClearDownloadBatch(UserRole _UserRole)
        {
            Batch objBatch = new Batch();
            objBatch.ClearBatch(this.BatchId, _UserRole);
        }
    }

    [DataContract]
    public class ImportFileData
    {
        [DataMember]
        public Guid BatchId { get; set; }
        [DataMember]
        public int BatchNumber { get; set; }
        [DataMember]
        public bool IsImportedSuccessfully { get; set; }
    }


}
#endregion