using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ILinkPaymentReciptRecords
    {
        [OperationContract]
        List<LinkPaymentReciptRecords> GetLinkPaymentReciptRecordsByPolicyId(Guid PolicyId);
        [OperationContract]
        void AddUpdateLinkPaymentReciptRecords(LinkPaymentReciptRecords _LinkPaymentReciptRecords);

    }
    public partial class MavService : ILinkPaymentReciptRecords
    {
        #region ILinkPaymentReciptRecords Members

        public List<LinkPaymentReciptRecords> GetLinkPaymentReciptRecordsByPolicyId(Guid PolicyId)
        {
            return LinkPaymentReciptRecords.GetLinkPaymentReciptRecordsByPolicyId(PolicyId);
        }

        public void AddUpdateLinkPaymentReciptRecords(LinkPaymentReciptRecords _LinkPaymentReciptRecords)
        {
            _LinkPaymentReciptRecords.AddUpdateLinkPaymentReciptRecords(_LinkPaymentReciptRecords);
        }

        #endregion
    }
}