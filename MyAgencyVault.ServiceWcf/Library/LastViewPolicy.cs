using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using MyAgencyVault.BusinessLibrary;
using System.Collections;
using System.Data;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary.Masters;
using MyAgencyVault.BusinessLibrary.Base;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ILastViewPolicy
    {
          #region IEditable<Licensee> Members
        [OperationContract]
        [FaultContract(typeof(ApplicationFault))]
        void AddLastViewPolicy(LastViewPolicy lastviewpolicy);
        #endregion
        [OperationContract]
        [FaultContract(typeof(ApplicationFault))]
        List<LastViewPolicy> GetLastViewPolicy(Guid userCredentialID);
        [OperationContract]
        [FaultContract(typeof(ApplicationFault))]
        List<LastViewPolicy> GetAllLastViewPolicy();
        [OperationContract]
        [FaultContract(typeof(ApplicationFault))]
        void DeleteRecordCredientialWise(Guid userCredentialID);
        [OperationContract]
        [FaultContract(typeof(ApplicationFault))]
        void SaveLastViewedClients(List<LastViewPolicy> lastViewedClients, Guid userId);
        [OperationContract]
        [FaultContract(typeof(ApplicationFault))]
        List<LastViewPolicy> GetLastViewedClients(Guid userID);
    }
    public partial class MavService : ILastViewPolicy
    {

        #region ICarrier Members
        public List<LastViewPolicy> GetLastViewPolicy(Guid userCredentialID)
        {
            try
            {

                return LastViewPolicy.GetLastViewPolicy(userCredentialID);
            }
            catch (Exception ex)
            {

                ApplicationFault theFault = new ApplicationFault();
                theFault.Error = "Some Error " + ex.Message.ToString();
                throw new FaultException<ApplicationFault>(theFault);

            }                     

        }
        public List<LastViewPolicy> GetAllLastViewPolicy()
        {
            try
            {
                return LastViewPolicy.GetAllLastViewPolicy();
            }

            catch (Exception ex)
            {

                ApplicationFault theFault = new ApplicationFault();
                theFault.Error = "Some Error " + ex.Message.ToString();
                throw new FaultException<ApplicationFault>(theFault);

            }   
        }
        
        #endregion

        public void AddLastViewPolicy(LastViewPolicy lastviewpolicy)
        {
            try
            {
                lastviewpolicy.AddUpdate();
            }
            catch (Exception)
            {

                //ApplicationFault theFault = new ApplicationFault();
                //theFault.Error = "Some Error " + ex.Message.ToString();
                //throw new FaultException<ApplicationFault>(theFault);

            }   
        }

        public void DeleteRecordCredientialWise(Guid userCredentialID)
        {
            try
            {
                LastViewPolicy.DeleteRecordCredientialWise(userCredentialID);
            }
            catch (Exception)
            {

                //ApplicationFault theFault = new ApplicationFault();
                //theFault.Error = "Some Error " + ex.Message.ToString();
                //throw new FaultException<ApplicationFault>(theFault);

            }   
        }

        public void SaveLastViewedClients(List<LastViewPolicy> lastViewedClients,Guid userId)
        {
            LastViewPolicy.SaveLastViewedClients(lastViewedClients, userId);
        }

        public List<LastViewPolicy> GetLastViewedClients(Guid userID)
        {
            return LastViewPolicy.GetLastViewedClients(userID);
        }
    }
}