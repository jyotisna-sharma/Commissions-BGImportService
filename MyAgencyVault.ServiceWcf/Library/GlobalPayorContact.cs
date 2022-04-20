using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IGlobalPayorContact
    {       
        #region IEditable<GlobalPayorContact> Members
        [OperationContract]
        void AddUpdateGlobalPayorContact(GlobalPayorContact gpc);
       
        [OperationContract]
        void DeleteGlobalPayorContact(GlobalPayorContact gpc);
       
        [OperationContract]
        GlobalPayorContact GetGlobalPayorContact(Guid id);

        [OperationContract]
        bool IsValidGlobalPayorContact(GlobalPayorContact gpc);

        [OperationContract]
        List<GlobalPayorContact> getContacts(Guid PayorId);
        #endregion
    }

    public partial  class MavService : IGlobalPayorContact
    {
        #region IGlobalPayorContact Members

        public void  AddUpdateGlobalPayorContact(GlobalPayorContact gpc)
        {
            gpc.AddUpdate();
        }

        public void DeleteGlobalPayorContact(GlobalPayorContact  gpc)
        {
            gpc.Delete();
        }

        public GlobalPayorContact  GetGlobalPayorContact(Guid id)
        {
            throw new NotImplementedException();
            //return GlobalPayorContact.GetOfId(id);
        }

        public bool  IsValidGlobalPayorContact(GlobalPayorContact  gpc)
        {
            return gpc.IsValid();
        }

        public List<GlobalPayorContact> getContacts(Guid PayorId)
        {
            return GlobalPayorContact.getContacts(PayorId);
        }

        #endregion       
    }
}
