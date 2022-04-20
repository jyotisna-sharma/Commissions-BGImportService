using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;  
using MyAgencyVault.BusinessLibrary;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IClient
    {
        #region IEditable<Client> Members
        [OperationContract]
        void AddUpdateClient(Client Clnt);
        [OperationContract]
        void DeleteClients(Client Clnt);
        [OperationContract]
        Client GetClient();
        [OperationContract]
        bool IsValidClient(Client Clnt);

        #endregion
        /// <summary>
        /// <param name="PolicyId"/>
        /// <param name="LicenseeId"/>
        /// <param name="BatchId"/>
        /// <param name="StatementId"/>
        /// </summary>
        /// condition apply : (all/ all viewable to the user/ all under the licensee/)
        /// <returns></returns>
        [OperationContract]
        List<Client> GetClientList(Guid? LicenseeId);
               

        [OperationContract]
        List<Client> GetRefreshedClientList(Guid LicenseeId, List<Guid> ClientIds);

        [OperationContract]
        List<Client> GetAllClientList();
        /// <summary>
        /// <param name="ClientId"/>
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PolicyDetailsData> GetClientPolicies(Client Clnt);

        [OperationContract]
        bool CheckClientPolicyIssueExists(Guid ClientId);

        [OperationContract]
        Client GetClientByClientName(string strClientName, Guid LicID);

        [OperationContract]
        Client GetClientByClientNameTest(string strClientName, Guid LicID);

        [OperationContract]
        Client GetClientByClientID(Guid ClientID, Guid LicID);

        [OperationContract]
        int GetAllClientCountinLic(Guid LicID);

        [OperationContract]
        int GetAllClientCount();

        [OperationContract]
        IEnumerable<Client> GetAllClientByLicChunck(Guid LicenseeId, int skip, int take);
    }

    public partial class MavService : IClient
    {
        #region IClient Members

        public void AddUpdateClient(Client Clnt)
        {
            Clnt.AddUpdate();
        }

        public void DeleteClients(Client Clnt)
        {
            Clnt.Delete();
        }

        public List<Client> GetAllClientList()
        {
            return Client.GetClientList(Guid.Empty);
        }

        public List<Client> GetRefreshedClientList(Guid LicenseeId, List<Guid> ClientIds)
        {
            return Client.GetRefreshedClientList(LicenseeId, ClientIds);
        }

        public Client GetClient()
        {
            throw new NotImplementedException();
        }

        public bool IsValidClient(Client Clnt)
        {
            return Clnt.IsValid();
        }

        public List<Client> GetClientList(Guid? LicenseeId)
        {
            return Client.GetClientList(LicenseeId);
        }

        public int GetAllClientCount()
        {
            return Client.GetAllClientCount();
        }

        public int GetAllClientCountinLic(Guid LicID)
        {
            return Client.GetAllClientCountinLic(LicID);
        }

        public Client GetClientByClientID(Guid ClientID, Guid LicID)
        {
            return Client.GetClientByClientID(ClientID, LicID);
        }

        public List<PolicyDetailsData> GetClientPolicies(Client Clnt)
        {
            return Clnt.GetPolicies();
        }

        public IEnumerable<Client> GetAllClientByLicChunck(Guid LicenseeId, int skip, int take)
        {
            return Client.GetAllClientByLicChunck(LicenseeId, skip, take);
        }

        public Client GetClientByClientName(string strClientName, Guid LicID)
        {
            Client objClient = new Client();
            return objClient.GetClientByClientName(strClientName, LicID);
        }

        public Client GetClientByClientNameTest(string strClientName, Guid LicID)
        {
            Client objClient = new Client();
            return objClient.GetClientByClientName(strClientName, LicID);
        }

        public bool CheckClientPolicyIssueExists(Guid ClientId)
        {
            return Client.CheckClientPolicyIssueExists(ClientId);
        }

        #endregion
    }
}
