using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Collections;
using System.Data;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class LastViewPolicy
    {
        [DataMember]
        public Guid? PolicyId { get; set; }

        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        public Guid? UserCredentialId { get; set; }

        [DataMember]
        public Guid? Clientid { get; set; }

        [DataMember]
        public string ClientName { get; set; }

        [DataMember]
        public int SNo { get; set; }

        public static void DeleteLastViewRecordPolicyIdWise(Guid PolicyId)
        {
            List<DLinq.LastPolicyViewed> lastviewpolicylst = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                lastviewpolicylst = (from e in DataModel.LastPolicyVieweds where e.PolicyId == PolicyId select e).ToList();
                foreach (DLinq.LastPolicyViewed lastviewpolicy in lastviewpolicylst)
                {
                    if (lastviewpolicy != null)
                    {
                        DataModel.DeleteObject(lastviewpolicy);
                        DataModel.SaveChanges();
                    }
                }
            }
        }

        public static void DeleteLastViewRecordClientIdWise(Guid ClientId)
        {
            List<DLinq.LastPolicyViewed> lastviewpolicylst = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                lastviewpolicylst = (from e in DataModel.LastPolicyVieweds where e.Clientid == ClientId select e).ToList();
                foreach (DLinq.LastPolicyViewed lastviewpolicy in lastviewpolicylst)
                {
                    if (lastviewpolicy != null)
                    {
                        DataModel.DeleteObject(lastviewpolicy);
                        DataModel.SaveChanges();
                    }
                }
            }
        }
        public static List<LastViewPolicy> GetLastViewPolicy(Guid userCredentialID)
        {
            if (userCredentialID == null)
                return null;

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.LastPolicyVieweds
                        where f.UserCredential.UserCredentialId == userCredentialID && f.Client.ClientId != null
                        select new LastViewPolicy
                        {
                            ID = f.ID,
                            UserCredentialId = f.UserCredential.UserCredentialId,
                            Clientid = f.Client.ClientId,
                            ClientName = f.Client.Name,
                            SNo = f.SNo,
                            PolicyId = f.PolicyId,
                        }).OrderByDescending(o => o.SNo).ToList();

            }
        }

        public static List<LastViewPolicy> GetAllLastViewPolicy()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.LastPolicyVieweds
                        select new LastViewPolicy
                        {
                            ID = f.ID,
                            UserCredentialId = f.UserCredential.UserCredentialId,
                            Clientid = f.Client.ClientId,
                            ClientName = f.Client.Name,
                            SNo = f.SNo,
                            PolicyId = f.PolicyId,
                        }).OrderByDescending(o => o.SNo).ToList();

            }
        }

        private static string GetPolicyClientDetail(Guid PolicyID)
        {
            string Detail = "";
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var De = from f in DataModel.Policies
                         .Include("Client")
                         where f.PolicyId == PolicyID
                         select new
                      {
                          Detail = f.PolicyId.ToString() + " " + f.Client.Name
                      };

                return Detail;
            }

        }

        public void AddUpdate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.LastPolicyViewed lastviewpolicy = null;
                lastviewpolicy = new DLinq.LastPolicyViewed();
                lastviewpolicy.ID = Guid.NewGuid();

                lastviewpolicy.UserCredentialId = this.UserCredentialId;
                lastviewpolicy.Clientid = this.Clientid;
                lastviewpolicy.PolicyId = this.PolicyId;
                DataModel.AddToLastPolicyVieweds(lastviewpolicy);
                DataModel.SaveChanges();

            }
        }


        public static void DeleteRecordCredientialWise(Guid userCredentialID)
        {
            List<DLinq.LastPolicyViewed> lastviewpolicylst = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                lastviewpolicylst = (from e in DataModel.LastPolicyVieweds where e.UserCredential.UserCredentialId == userCredentialID select e).ToList();
                int count = lastviewpolicylst.Count();
                if (count < 11)
                    return;

                int sno = lastviewpolicylst.Min(p => p.SNo);
                DLinq.LastPolicyViewed lastviewpolicy = (from e in DataModel.LastPolicyVieweds
                                                         where e.SNo == sno
                                                         select e).FirstOrDefault();
                if (lastviewpolicy != null)
                {
                    DataModel.DeleteObject(lastviewpolicy);
                    DataModel.SaveChanges();
                }
            }
        }

        private static object lockObject = new object();
        public static void SaveLastViewedClients(List<LastViewPolicy> lastViewedClients, Guid userId)
        {
            try
            {
                lock (lockObject)
                {
                    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                    {
                        List<DLinq.LastPolicyViewed> lastviewpolicylst = (from e in DataModel.LastPolicyVieweds where e.UserCredential.UserCredentialId == userId select e).ToList();

                        foreach (DLinq.LastPolicyViewed policyViewed in lastviewpolicylst)
                            DataModel.LastPolicyVieweds.DeleteObject(policyViewed);

                        int count = 1;
                        foreach (LastViewPolicy viewPolicy in lastViewedClients)
                        {
                            DLinq.LastPolicyViewed policyViewed = new DLinq.LastPolicyViewed();
                            policyViewed.ID = Guid.NewGuid();
                            policyViewed.UserCredentialId = userId;
                            policyViewed.Clientid = viewPolicy.Clientid;
                            policyViewed.PolicyId = viewPolicy.PolicyId;
                            policyViewed.SNo = count++;

                            DataModel.LastPolicyVieweds.AddObject(policyViewed);
                        }
                        DataModel.SaveChanges();
                    }
                }
            }
            catch
            {
            }
        }

        public static List<LastViewPolicy> GetLastViewedClients(Guid userID)
        {
            if (userID == Guid.Empty)
                return null;

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.LastPolicyVieweds
                        where f.UserCredential.UserCredentialId == userID
                        orderby f.SNo
                        select new LastViewPolicy
                        {
                            ID = f.ID,
                            UserCredentialId = f.UserCredential.UserCredentialId,
                            Clientid = f.Client.ClientId,
                            ClientName = f.Client.Name,
                            SNo = f.SNo,
                            PolicyId = f.PolicyId,
                        }).ToList();

            }
        }
    }
}
