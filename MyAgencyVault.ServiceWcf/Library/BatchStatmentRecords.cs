using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;


namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IBatchStatmentRecords
    {
        [OperationContract]
        List<BatchStatmentRecords> GetBatchStatment(Guid BatchId);

        [OperationContract]
        List<BatchStatmentRecords> GetBatchStatmentWithoutCalculation(Guid BatchId);

        [OperationContract]
        void AddUpdateBatchStatmentRecord(BatchStatmentRecords _BatchStatmentRecord);
        [OperationContract]
        decimal GetBatchTotal(Guid BatchId);

    }
    public partial class MavService : IBatchStatmentRecords
    {
        #region IBatchStatmentRecords Members

        public List<BatchStatmentRecords> GetBatchStatment(Guid BatchId)
        {
            return BatchStatmentRecords.GetBatchStatment(BatchId);
        }

        public List<BatchStatmentRecords> GetBatchStatmentWithoutCalculation(Guid BatchId)
        {
            return BatchStatmentRecords.GetBatchStatmentWithoutCalculation(BatchId);
        }

        public void AddUpdateBatchStatmentRecord(BatchStatmentRecords _BatchStatmentRecord)
        {
            _BatchStatmentRecord.AddUpdateBatchStatmentRecord(_BatchStatmentRecord);
        }
        public decimal GetBatchTotal(Guid BatchId)
        {
            return BatchStatmentRecords.GetBatchTotal(BatchId);
        }
        #endregion
    }
}