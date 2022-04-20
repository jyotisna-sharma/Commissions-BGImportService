using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPayorDefaults 
    {
        #region IEditable<PayorDefaults> Members
        [OperationContract]
        void AddUpdatePayorDefaults(PayorDefaults PyrToolDafults);
        [OperationContract]
        void DeletePayorDefaults(PayorDefaults PyrToolDafults);
        [OperationContract]
        List<PayorDefaults> GetPayorDefaults();
        [OperationContract]
        bool IsValidPayorDefaults(PayorDefaults PyrToolDafults);

        #endregion
       
        /// <summary>
        /// update the related payor's default statement dates.
        /// </summary>

        [OperationContract]
        void UpdateStatementDates(PayorDefaults PyrToolDafults);
        [OperationContract]
        PayorDefaults GetPayorDefaultBy(Guid globalPayerId);

    }
    public partial class MavService : IPayorDefaults
    {
        #region IPayorDefaults Members

        public void AddUpdatePayorDefaults(PayorDefaults PyrToolDafults)
        {
            PyrToolDafults.AddUpdate();
        }

        public void DeletePayorDefaults(PayorDefaults PyrToolDafults)
        {
            PyrToolDafults.Delete();
        }

        public List<PayorDefaults> GetPayorDefaults()
        {
            return PayorDefaults.GetPayorDefault();
        }

        public bool IsValidPayorDefaults(PayorDefaults PyrToolDafults)
        {
            return PyrToolDafults.IsValid();
        }

        public void UpdateStatementDates(PayorDefaults PyrToolDafults)
        {
            throw new NotImplementedException();
        }

        public PayorDefaults GetPayorDefaultBy(Guid globalPayerId)
        {
            return PayorDefaults.GetPayorDefault(globalPayerId);
        }

       
        #endregion
    }
}
