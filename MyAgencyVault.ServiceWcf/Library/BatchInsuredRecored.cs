using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;
namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IBatchInsuredRecored
    {
        [OperationContract]
        List<BatchInsuredRecored> GetBatchInsuredRecored(Guid stmtId);
        [OperationContract]
        List<InsuredPayment> GetInsuredPayments(Guid stmtId);
        [OperationContract]
        List<InsuredPayment> GetInsuredName(Guid stmtId);
        
    }
    public partial class MavService : IBatchInsuredRecored
    {
        #region IBatchInsuredRecored Members

        public List<BatchInsuredRecored> GetBatchInsuredRecored(Guid stmtId)
        {
            return BatchInsuredRecored.GetBatchInsuredRecored(stmtId);
        }

        public List<InsuredPayment> GetInsuredPayments(Guid stmtId)
        {
            return BatchInsuredRecored.GetInsuredPayments(stmtId);
        }

        public List<InsuredPayment> GetInsuredName(Guid stmtId)
        {
            return BatchInsuredRecored.GetInsuredName(stmtId);
        }

        #endregion
    }
}
