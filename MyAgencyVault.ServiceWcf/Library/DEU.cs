using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IDataEntryUnit
    {
        [OperationContract]
        ModifiyableBatchStatementData AddUpdateDEU(DEUFields deuFields);

        [OperationContract]
        void AddupdateDeuEntry(DEU _DeuEntry);

        [OperationContract]

        DEU GetDeuEntryidWise(Guid DeuEntryID);

        [OperationContract]
        List<DataEntryField> GetDeuFields(Guid DeuEntryID);

        //[OperationContract]
        //ModifiyableBatchStatementData BatchStatementUpdateOnSuccessfullDeuPost(Guid DeuEntryId);

        //[OperationContract]
        //void UpdateDeuEntryStatus(Guid entryId, bool IsCompleted);

        [OperationContract]
        bool IsPaymentFromCommissionDashBoardByPaymentEntryId(Guid PolicPaymentId);
        [OperationContract]
        bool IsPaymentFromCommissionDashBoardByDEUEntryId(Guid DeuEntryId);
        [OperationContract]
        List<ExposedDEU> GetDeuEntriesForStatement(Guid StatementId);

        [OperationContract]
        void DeleteDeuEntryByID(Guid DeuEntryId);

        [OperationContract]
        void DeleteDeuEntryAndPaymentEntryByDeuID(Guid DeuEntryId);

        [OperationContract]
        string GetProductTypeNickName(Guid policyID, Guid PayorID, Guid CarrierID, Guid CoverageID);
        
    }

    public partial class MavService : IDataEntryUnit
    {
        public ModifiyableBatchStatementData AddUpdateDEU(DEUFields deuFields)
        {
            Guid olddeuEntryID = new Guid();         
            DEU objDeu = new DEU();           
            return objDeu.AddUpdate(deuFields, olddeuEntryID);
        }
        
        public void AddupdateDeuEntry(DEU _DeuEntry)
        {
            //DEU.AddupdateDeuEntry(_DeuEntry);           
            DEU objDEU = new DEU();
            objDEU.AddupdateDeuEntry(_DeuEntry);
        }
        public DEU GetDeuEntryidWise(Guid DeuEntryID)
        {
            return DEU.GetDeuEntryidWise(DeuEntryID);
        }

        public void DeleteDeuEntryByID(Guid DeuEntryId)
        {
            DEU objDEU = new DEU();
            objDEU.DeleteDeuEntryByID(DeuEntryId);
        }
        
        public void DeleteDeuEntryAndPaymentEntryByDeuID(Guid DeuEntryId)
        {
            DEU objDEU = new DEU();
            objDEU.DeleteDeuEntryAndPaymentEntryByDeuID(DeuEntryId);
        }

        public List<DataEntryField> GetDeuFields(Guid DeuEntryID)
        {
            return DEU.GetDeuFields(DeuEntryID);
        }
        public List<ExposedDEU> GetDeuEntriesForStatement(Guid StatementId)
        {
            return Statement.GetDeuEntriesforStatement(StatementId);
        }

        public string GetProductTypeNickName(Guid policyID, Guid PayorID, Guid CarrierID, Guid CoverageID)
        {
            return DEU.GetProductTypeNickName(policyID, PayorID, CarrierID, CoverageID);
        }
        //public ModifiyableBatchStatementData BatchStatementUpdateOnSuccessfullDeuPost(Guid DeuEntryId)
        //{
        //    return DEU.BatchStatementUpdateOnSuccessfullDeuPost(DeuEntryId);
        //}

        //public void UpdateDeuEntryStatus(Guid entryId, bool IsCompleted)
        //{
        //    DEU.UpdateDeuEntryStatus(entryId, IsCompleted);
        //}

        #region IDataEntryUnit Members


        public bool IsPaymentFromCommissionDashBoardByPaymentEntryId(Guid PolicPaymentId)
        {
            return DEU.IsPaymentFromCommissionDashBoardByPaymentEntryId(PolicPaymentId);
        }

        public bool IsPaymentFromCommissionDashBoardByDEUEntryId(Guid DeuEntryId)
        {
            return DEU.IsPaymentFromCommissionDashBoardByDEUEntryId(DeuEntryId);

        }

        #endregion
    }
}