using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Data.SqlClient;
using System.Data;
using System.Data.EntityClient;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class Client : IEditable<Client>
    {
        #region IEditable<Client> Members

        public void AddUpdate()
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _clients = (from s in DataModel.Clients where (s.ClientId == this.ClientId) select s).FirstOrDefault();
                    if (_clients == null)
                    {
                        _clients = new DLinq.Client
                        {
                            ClientId = this.ClientId,
                            Address = this.Address,
                            Zip = this.Zip,
                            State = this.State,
                            Name = this.Name,
                            IsDeleted = false,
                            City = this.City,
                            Email = this.Email,
                            CreatedOn = DateTime.Now
                        };
                        DLinq.Licensee _license = ReferenceMaster.GetReferencedLicensee(this.LicenseeId, DataModel);
                        _clients.Licensee = _license;
                        DataModel.AddToClients(_clients);
                    }
                    else
                    {
                        _clients.Address = this.Address;
                        _clients.Zip = this.Zip;
                        _clients.State = this.State;
                        _clients.Name = this.Name;
                        _clients.City = this.City;
                        _clients.Email = this.Email;

                        if (_clients.IsDeleted == true)
                            _clients.IsDeleted = false;
                    }
                    DataModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdate client:" + ex.Message.ToString(), true);
            }
        }

        public static void DeleteCascadeClient(Guid PolicyId, Guid ClientId, Guid LicenseId)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeleteCascadeClient started - policyid: " + PolicyId + ", clientid: " + ClientId + ", licenseeid: " + LicenseId, true);
            Policy.DeletePolicyCascadeFromDBById(PolicyId);
            PostUtill.RemoveClient(ClientId, LicenseId);
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeleteCascadeClient ended - policyid: " + PolicyId + ", clientid: " + ClientId + ", licenseeid: " + LicenseId, true);
        }

        public void Delete()
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete started in client.cs - clientID: " + this.ClientId, true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    var client = (from s in DataModel.Clients where (s.ClientId == this.ClientId) select s).FirstOrDefault();

                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    if (client != null)
                    {
                        if (client.LicenseeId != null)
                        {
                            parameters.Add("PolicyLicenseeId", client.LicenseeId);
                        }
                        parameters.Add("PolicyClientId", client.ClientId);
                        parameters.Add("IsDeleted", false);
                        List<Guid> policyIds = Policy.GetPolicyData(parameters).Select(s => s.PolicyId).ToList();

                        foreach (Guid policyId in policyIds)
                        {
                            Policy.MarkPolicyDeletedById(policyId);
                        }

                        if (client != null)
                            client.IsDeleted = true;

                        DataModel.SaveChanges();
                    }
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete ended in client.cs - clientID: " + this.ClientId, true);
                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete exception in client.cs - clientID: " + this.ClientId + ", ex: " + ex.Message, true);
                }
            }
        }
        /// <summary>
        /// Physically delete the client
        /// for delete we need only ClientID in Instance
        /// </summary>
        /// <param name="_Client"></param>
        public static void DeleteClient(Client _Client)
        {
            ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeleteClient started in client.cs - clientID: " + _Client.ClientId, true);
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    var _clients = (from s in DataModel.Clients where (s.ClientId == _Client.ClientId) select s).FirstOrDefault();
                    if (_clients != null)
                    {
                        DataModel.DeleteObject(_clients);
                        DataModel.SaveChanges();
                    }
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeleteClient ended in client.cs - clientID: " + _Client.ClientId, true);
                }
                catch (Exception ex)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeleteClient exception in client.cs - clientID: " + _Client.ClientId + ", ex: " + ex.Message, true);
                    if (ex.InnerException != null)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " DeleteClient inner exception : " + ex.InnerException.Message, true);
                    }
                }
            }
        }
        public Client GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region "Data members aka =- public properties"
        [DataMember]
        public Guid ClientId { get; set; }
        [DataMember]
        public Guid? LicenseeId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string InsuredName { get; set; }//Insured Name May be different from Client
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember] //--don't know if it is required or not.
        public Guid LogInUserId { get; set; }
        #endregion

        #region
        /// <summary>
        /// <param name="PolicyId"/>
        /// <param name="LicenseeId"/>
        /// <param name="BatchId"/>
        /// <param name="StatementId"/>
        /// </summary>
        /// condition apply : (all/ all viewable to the user/ all under the licensee/)
        /// GetClients()(all/all in a licensee/all a user can see/ one of a given id/ one associated with a policy id/ )
        /// overloaded function is possible to implement different filter criteria.
        /// <returns></returns>
        /// 
        public static IEnumerable<Client> GetAllClientByLicChunck(Guid LicenseeId, int skip, int take)
        {
            IEnumerable<Client> clientLst = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                try
                {
                    clientLst = (from s in DataModel.Clients
                                 where s.IsDeleted == false && s.LicenseeId == LicenseeId
                                 select new Client
                                 {
                                     ClientId = s.ClientId,
                                     Address = s.Address,
                                     City = s.City,
                                     Email = s.Email,
                                     LicenseeId = s.Licensee.LicenseeId,
                                     IsDeleted = s.IsDeleted,
                                     Name = s.Name,
                                     State = s.State,
                                     Zip = s.Zip
                                 }).ToList();

                }
                catch
                {
                }
                return clientLst.OrderBy(d => d.Name).Skip(skip).Take(take).ToList();
            }
        }

        public static List<Client> GetClientList(Guid? licenseeId)
        {

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<Client> clientLst = new List<Client>();
                try
                {
                    clientLst = (from s in DataModel.Clients
                                 orderby s.Name
                                 where (s.IsDeleted == false && s.LicenseeId == licenseeId.Value && !string.IsNullOrEmpty(s.Name))
                                 select new Client
                                 {
                                     ClientId = s.ClientId,
                                     Address = s.Address,
                                     City = s.City,
                                     Email = s.Email,
                                     LicenseeId = s.Licensee.LicenseeId,
                                     IsDeleted = s.IsDeleted,
                                     Name = s.Name,
                                     State = s.State,
                                     Zip = s.Zip
                                 }).OrderBy(p => p.Name).ToList<Client>();


                }
                catch
                {
                }

                return clientLst;
            }
        }

        public static Client GetClientByClientID(Guid ClientID, Guid LicID)
        {

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                Client objClient = new Client();
                try
                {
                    objClient = (from s in DataModel.Clients
                                 orderby s.Name
                                 where (s.IsDeleted == false && s.LicenseeId == LicID && s.ClientId == ClientID && !string.IsNullOrEmpty(s.Name))
                                 select new Client
                                 {
                                     ClientId = s.ClientId,
                                     Address = s.Address,
                                     City = s.City,
                                     Email = s.Email,
                                     LicenseeId = s.Licensee.LicenseeId,
                                     IsDeleted = s.IsDeleted,
                                     Name = s.Name,
                                     State = s.State,
                                     Zip = s.Zip
                                 }).FirstOrDefault();


                }
                catch
                {
                }

                return objClient;
            }
        }

        public static int GetAllClientCountinLic(Guid? licenseeId)
        {
            int intCount = 0;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                try
                {
                    DataModel.CommandTimeout = 600000000;
                    intCount = (from uc in DataModel.Clients where uc.IsDeleted == false && uc.LicenseeId == licenseeId.Value && !string.IsNullOrEmpty(uc.Name) select uc).Count();

                }
                catch
                {
                }

                return intCount;

            }
        }

        public static int GetAllClientCount()
        {
            int intCount = 0;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                try
                {
                    DataModel.CommandTimeout = 600000000;
                    intCount = (from uc in DataModel.Clients where uc.IsDeleted == false && !string.IsNullOrEmpty(uc.Name) select uc).Count();

                }
                catch
                {
                }

                return intCount;

            }
        }

        //public static List<Client> GetClientList(Guid? licenseeId)
        //{
        //    List<Client> lstClient = new List<Client>();
        //    SqlConnection con = null;

        //    try
        //    {

        //        using (con = new SqlConnection(DBConnection.GetConnectionString()))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("Usp_GetClientList", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@LicenseeId", licenseeId);
        //                con.Open();

        //                SqlDataReader reader = cmd.ExecuteReader();
        //                // Call Read before accessing data. 
        //                while (reader.Read())
        //                {

        //                    Client objClient = new Client();
        //                    objClient.ClientId = (Guid)reader["ClientId"];
        //                    objClient.Address = Convert.ToString(reader["Address"]);
        //                    objClient.City = Convert.ToString(reader["City"]);
        //                    objClient.Email = Convert.ToString(reader["Email"]);
        //                    objClient.LicenseeId = (Guid)reader["LicenseeId"];
        //                    objClient.IsDeleted = (bool)reader["IsDeleted"];
        //                    objClient.Name = Convert.ToString(reader["Name"]);
        //                    objClient.State = Convert.ToString(reader["State"]);
        //                    objClient.Zip = Convert.ToString(reader["Zip"]);
        //                    lstClient.Add(objClient);

        //                }

        //                // Call Close when done reading.
        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Close();
        //        }
        //    }

        //    return lstClient;
        //}

        public static List<Client> GetRefreshedClientList(Guid LicenseeId, List<Guid> ClientIds)
        {
            //ActionLogger.Logger.WriteImportPolicyLog("GetRefreshedClientList Start: " + DateTime.Now.ToLongTimeString(), true); 
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var clientLst = (from s in DataModel.Clients
                                 orderby s.Name
                                 where (s.IsDeleted == false) && (!ClientIds.Contains(s.ClientId)) && LicenseeId == s.LicenseeId
                                 select new Client
                                 {
                                     ClientId = s.ClientId,
                                     Address = s.Address,
                                     City = s.City,
                                     Email = s.Email,
                                     LicenseeId = s.Licensee.LicenseeId,
                                     IsDeleted = s.IsDeleted,
                                     Name = s.Name,
                                     State = s.State,
                                     Zip = s.Zip
                                 }).ToList();
                //ActionLogger.Logger.WriteImportPolicyLog("GetRefreshedClientList End: " + DateTime.Now.ToLongTimeString(), true); 
                return clientLst;
            }
        }

        public static Client GetClient(Guid ClientId)
        {
            Client _client = new Client();
            try
            {

                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    _client = (from s in DataModel.Clients
                               where (s.IsDeleted == false && s.ClientId == ClientId)
                               select new Client
                               {
                                   ClientId = s.ClientId,
                                   Address = s.Address,
                                   City = s.City,
                                   Email = s.Email,
                                   LicenseeId = s.Licensee.LicenseeId,
                                   IsDeleted = s.IsDeleted,
                                   Name = s.Name,
                                   State = s.State,
                                   Zip = s.Zip
                               }).ToList().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetClient:Exception occur while processing" + ex.Message, true);
               // throw ex;

            }
            return _client;

        }
        /// <summary>
        /// fetch all the policyies of this client.
        /// </summary>
        /// <returns></returns>
        public List<PolicyDetailsData> GetPolicies()
        {
            Dictionary<string, object> paramaters = new Dictionary<string, object>();
            paramaters.Add("PolicyClientId", this.ClientId);
            List<PolicyDetailsData> _policylst = Policy.GetPolicyData(paramaters).ToList();
            return _policylst;
        }

        #endregion

        public Client GetClientByClientName(string strClientName, Guid LicID, string agencyName  = "")
        {
            Client _client = new Client();
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                _client = (from s in DataModel.Clients
                           where (s.IsDeleted == false && s.Name.ToLower() == strClientName.ToLower() && s.LicenseeId == LicID)
                           select new Client
                           {
                               ClientId = s.ClientId,
                               Address = s.Address,
                               City = s.City,
                               Email = s.Email,
                               LicenseeId = s.Licensee.LicenseeId,
                               IsDeleted = s.IsDeleted,
                               Name = s.Name,
                               State = s.State,
                               Zip = s.Zip
                           }).ToList().FirstOrDefault();
                return _client;
            }
        }

        public static bool CheckClientPolicyIssueExists(Guid ClientId)
        {
            bool flag = false;
            Dictionary<string, object> paramaters = new Dictionary<string, object>();
            paramaters.Add("PolicyClientId", ClientId);
            List<PolicyDetailsData> _policylst = Policy.GetPolicyData(paramaters).ToList();
            //Get policy list which is deleted 
            _policylst = _policylst.Where(p => p.IsDeleted == false).ToList();
            foreach (PolicyDetailsData po in _policylst)
            {
                if (FollowupIssue.GetIssues(po.PolicyId).Count >= 1)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
                return flag;
            foreach (PolicyDetailsData po in _policylst)
            {
                List<PolicyPaymentEntriesPost> _PolicyPaymentEntriesPost = PolicyPaymentEntriesPost.GetPolicyPaymentEntryPolicyIDWise(po.PolicyId);
                /*Acme - commented following after Kevin's email to check for payment entries only*/
                /*foreach (PolicyPaymentEntriesPost popy in _PolicyPaymentEntriesPost)
                {
                    List<PolicyOutgoingDistribution> _PolicyOutgoingDistributionLst = PolicyOutgoingDistribution.GetOutgoingPaymentByPoicyPaymentEntryId(popy.PaymentEntryID);
                    if (_PolicyOutgoingDistributionLst != null && _PolicyOutgoingDistributionLst.Count != 0)
                    {
                        flag = true;
                        return flag;
                    }
                }*/
                if (_PolicyPaymentEntriesPost != null && _PolicyPaymentEntriesPost.Count > 0)
                {
                    flag = true;
                    return flag;
                }

            }
            return flag;

        }

        public static Guid AddUpdateClient(string clientName, Guid LicID, Guid ClientID, string agencyName  = "")
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _clients = (from s in DataModel.Clients where (s.ClientId == ClientID) select s).FirstOrDefault();
                    if (_clients == null)
                    {
                        _clients = new DLinq.Client
                        {
                            ClientId = ClientID,
                            Name = clientName,
                            IsDeleted = false,
                            LicenseeId = LicID
                        };

                        DataModel.AddToClients(_clients);
                        DataModel.SaveChanges();
                    }


                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdateClient:Exception occurs while processing" + ex.Message, true, agencyName);
               // throw ex;
            }

            return ClientID;
        }
    }
}
