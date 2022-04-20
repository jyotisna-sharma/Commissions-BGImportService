using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]   
    interface IIncomingPament
    {
        #region IEditable<IncomingPament> Members
        [OperationContract]
        void AddUpdateIncomingPament(IncomingPament IncPymnt);
        [OperationContract]
        void DeleteIncomingPament(IncomingPament IncPymnt);
        [OperationContract]
        IncomingPament GetIncomingPament();
        [OperationContract]
        bool IsValidIncomingPament(IncomingPament IncPymnt); 

        #endregion

        /// <summary>
        /// unlink the payment entry from the policy, to which currently it is associated to.
        /// </summary>
        /// <returns>return true on successfull attempts, else false</returns>
        [OperationContract]
        bool UnlinkIncomingPayments(IncomingPament IncPymnt);
        /// <summary>
        /// link this payment entry to and existing policy.
        /// </summary>
        /// <returns>return true on successfull attempts, else false</returns>
        [OperationContract]
        bool LinkIncomingPaymentsToAnExistingPolicy(Guid PolicyId);
        /// <summary>
        /// make the linked policy to be active. 
        /// </summary>
        /// <returns>return true on successfull attempts, else false</returns>
        [OperationContract]
        bool ActivateNewPolicy(IncomingPament IncPymnt);
        
        /// <summary>
        /// developer need to recheck and think of the requirement of this funciton.
        /// </summary>
        /// <param name="policyID"></param>
        /// <returns>returns all the incoming payments related to a policyid given in the parameter.</returns>
        [OperationContract]
        List<IncomingPament> GetIncomingPayments(Guid PolicyID);
       
    }
    public partial class MavService : IIncomingPament
    {

        #region IIncomingPament Members

        public void AddUpdateIncomingPament(IncomingPament IncPymnt)
        {
            IncPymnt.AddUpdate();
        }

        public void DeleteIncomingPament(IncomingPament IncPymnt)
        {
            IncPymnt.Delete();
        }

        public IncomingPament GetIncomingPament()
        {
            throw new NotImplementedException();
        }

        public bool IsValidIncomingPament(IncomingPament IncPymnt)
        {
            return IncPymnt.IsValid();
        }

        public bool UnlinkIncomingPayments(IncomingPament IncPymnt)
        {
             return IncPymnt.UnlinkIncomingPayments();
        }

        public bool LinkIncomingPaymentsToAnExistingPolicy(Guid PolicyId)
        {
            IncomingPament IncPymnt=new IncomingPament();
            return  IncPymnt.LinkIncomingPaymentsToAnExistingPolicy(PolicyId);   
        }

        public bool ActivateNewPolicy(IncomingPament IncPymnt)
        {
            return  IncPymnt.ActivateNewPolicy();
        }

        public List<IncomingPament> GetIncomingPayments(Guid PolicyID)
        {
            return  IncomingPament.GetIncomingPayments(PolicyID);
        }

        #endregion
    }
}
