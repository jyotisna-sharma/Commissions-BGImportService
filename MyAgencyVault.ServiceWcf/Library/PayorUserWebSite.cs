using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPayorUserWebSite
    {
        #region IEditable<PayorUserWebSite> Members
        
        [OperationContract]
        void AddUpdatePayorUserWebSite(PayorSiteLoginInfo plSiteinfo);
        [OperationContract]
        void DeletePayorUserWebSite(PayorSiteLoginInfo plSiteinfo);
        [OperationContract]
        List<PayorSiteLoginInfo> GetPayorUsers(Guid licId,Guid payorId);
        [OperationContract]
        List<PayorSiteLoginInfo> GetLicenseeUsers(Guid licId);

        #endregion
    }
    public partial class MavService : IPayorUserWebSite
    {


        #region IPayorUserWebSite Members

        public List<PayorSiteLoginInfo> GetPayorUsers(Guid licId, Guid payorId)
        {
            return PayorSiteLoginInfo.GetPayorUserWebSite(licId, payorId);
        }

        public List<PayorSiteLoginInfo> GetLicenseeUsers(Guid licId)
        {
            return PayorSiteLoginInfo.GetLicenseeUsers(licId);
        }

        public void AddUpdatePayorUserWebSite(PayorSiteLoginInfo  plSiteinfo)
        {
            plSiteinfo.AddUpdate();
        }

        public void DeletePayorUserWebSite(PayorSiteLoginInfo plSiteinfo)
        {
            plSiteinfo.Delete();
        }
        
        #endregion
    }
}
