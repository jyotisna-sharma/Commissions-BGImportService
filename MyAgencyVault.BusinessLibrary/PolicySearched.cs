using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Linq.Expressions;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PolicySearched
    {
        [DataMember]
        public Guid ClienID { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string Insured { get; set; }
        [DataMember]
        public string PayorName { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Carrier { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Product { get; set; }
        [DataMember]
        public string CompSchedule { get; set; }
        [DataMember]
        public string CompType { get; set; }
        [DataMember]
        public string Mode { get; set; }
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public Guid UserCredId { get; set; }
        [DataMember]
        public Guid LicenseeId { get; set; }

        public static List<LinkedUser> GetLinkedUser(Guid UserCredentialID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<LinkedUser> _permissions = null;
                List<DLinq.UserCredential> linkedUsers = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == UserCredentialID).UserCredentials.Where(s => s.IsDeleted == false).ToList();
                
                _permissions = (from usr in linkedUsers
                                select new LinkedUser
                                {
                                    UserId = usr.UserCredentialId
                                }).ToList();

                return _permissions ?? new List<LinkedUser>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policynumber"></param>
        /// <param name="carrier"></param>
        /// <param name="payor"></param>
        /// <param name="UserCridenID">is not null in case of agent login otherwise null</param>
        /// <param name="Role"></param>
        /// <param name="LicenseID"></param>
        /// <returns></returns>
        //public static List<PolicySearched> GetLinkedPolicies(string policynumber, string carrier, string payor, Guid UserCridenID, UserRole Role, Guid LicenseID)
        //{
        //    if (Role == UserRole.SuperAdmin || Role == UserRole.Administrator || Role == UserRole.HO)
        //    {
        //        try
        //        {
        //            List<PolicySearched> _polyser = new List<PolicySearched>();
        //            _polyser.AddRange(GetSearchedPolicies(policynumber, carrier, payor, LicenseID, Guid.Empty,false));//UserCridenID should be null
        //            return _polyser;
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //    else if (Role == UserRole.Agent)
        //    {
        //        try
        //        {
        //            List<PolicySearched> _polyser = new List<PolicySearched>();
        //            List<LinkedUser> _usepre = GetLinkedUser(UserCridenID);
        //            foreach (LinkedUser upe in _usepre)
        //            {
        //                if (UserCridenID != upe.UserId)
        //                {
        //                     List<PolicySearched> repolicy= GetSearchedPolicies(policynumber, carrier, payor, LicenseID, (Guid)upe.UserId, false);
        //                    if(repolicy!=null)
        //                    _polyser.AddRange(repolicy);

        //                }
        //            }
        //            List<PolicySearched> repolicy1=  GetSearchedPolicies(policynumber, carrier, payor,LicenseID, UserCridenID,true);
        //                    if(repolicy1!=null)
        //            _polyser.AddRange(repolicy1);
                   
        //            ////
        //            //List<PolicySearched> tempPolicySearched = null;
        //            //foreach (PolicySearched ps in _polyser)
        //            //{
        //            //    tempPolicySearched = new List<PolicySearched>();
        //            //    if (!tempPolicySearched.Exists(p => p.PolicyId == ps.PolicyId))
        //            //    {
        //            //        tempPolicySearched.Add(ps);
        //            //    }
        //            //}
        //            //if (tempPolicySearched != null)
        //            //{
        //            //    _polyser = tempPolicySearched;
        //            //}

        //                    List<Guid> PolicyIdLsttemp = _polyser.Select(p => p.PolicyId).Distinct().ToList();
        //                    _polyser = _polyser.FindAll(s => PolicyIdLsttemp.Exists(p => p == s.PolicyId));
        //            /////
                    
        //            return _polyser;
        //        }
        //        catch
        //        {
        //            return null;

        //        }
        //    }
        //    return null;

        //}

        public  List<PolicySearched> GetLinkedPolicies(string strClient,string strInsured, string policynumber, string carrier, string payor, Guid UserCridenID, UserRole Role, Guid LicenseID)
        {
            if (Role == UserRole.SuperAdmin || Role == UserRole.Administrator || Role == UserRole.HO)
            {
                try
                {
                    List<PolicySearched> _polyser = new List<PolicySearched>();
                    _polyser.AddRange(GetSearchedPolicies(strClient, strInsured,policynumber, carrier, payor, LicenseID, Guid.Empty, false));//UserCridenID should be null
                    return _polyser;
                }
                catch
                {
                    return null;
                }
            }
            else if (Role == UserRole.Agent)
            {
                try
                {
                    List<PolicySearched> _polyser = new List<PolicySearched>();
                    List<LinkedUser> _usepre = GetLinkedUser(UserCridenID);
                    foreach (LinkedUser upe in _usepre)
                    {
                        if (UserCridenID != upe.UserId)
                        {
                            List<PolicySearched> repolicy = GetSearchedPolicies(strClient, strInsured,policynumber, carrier, payor, LicenseID, (Guid)upe.UserId, false);
                            if (repolicy != null)
                                _polyser.AddRange(repolicy);

                        }
                    }
                    List<PolicySearched> repolicy1 = GetSearchedPolicies(strClient,strInsured, policynumber, carrier, payor, LicenseID, UserCridenID, true);
                    if (repolicy1 != null)
                        _polyser.AddRange(repolicy1);

                    ////
                    //List<PolicySearched> tempPolicySearched = null;
                    //foreach (PolicySearched ps in _polyser)
                    //{
                    //    tempPolicySearched = new List<PolicySearched>();
                    //    if (!tempPolicySearched.Exists(p => p.PolicyId == ps.PolicyId))
                    //    {
                    //        tempPolicySearched.Add(ps);
                    //    }
                    //}
                    //if (tempPolicySearched != null)
                    //{
                    //    _polyser = tempPolicySearched;
                    //}

                    List<Guid> PolicyIdLsttemp = _polyser.Select(p => p.PolicyId).Distinct().ToList();
                    _polyser = _polyser.FindAll(s => PolicyIdLsttemp.Exists(p => p == s.PolicyId));

                    return _polyser;
                }
                catch
                {
                    return null;

                }
            }
            return null;

        }

        ///For Super User/Admin/Ho
        #region Super User/Admin/Ho/agent
       ///
        ///if usercredid is not null means agent
        ///
        //public static List<PolicySearched> GetSearchedPolicies(string policynumber, string carrier, string payor, Guid LicenseeId, Guid UserCridenID,bool SerchOwnPolicyForAgent)
        //{
         
        //    List<PolicySearched> pos = null;
        //    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //    {
        //        if (!(string.IsNullOrEmpty(policynumber)) && !(string.IsNullOrEmpty(carrier)) && !(string.IsNullOrEmpty(payor)))
        //        {
        //            pos =
        //                (from f in DataModel.Policies
        //                where
        //                  ((f.PolicyNumber.Contains(policynumber))
        //                    && (f.Carrier.CarrierName.Contains(carrier))
        //                      && (f.Payor.PayorName.Contains(payor))
        //                   // && (f.UserCredential.LicenseeId == LicenseeId)
        //                    && (f.PolicyLicenseeId== LicenseeId)
        //                && (f.IsDeleted == false)
        //                && (f.Client.IsDeleted == false))
        //                select new PolicySearched
        //                {
        //                    ClienID = f.Client.ClientId,
        //                    ClientName = f.Client.Name,
        //                    Insured = f.Insured,
        //                    PayorName = f.Payor.PayorName,

        //                    PolicyNumber = f.PolicyNumber,
        //                    State = f.Client.State,
        //                    Carrier = f.Carrier.CarrierName,

        //                    Status = f.MasterPolicyStatu.Name,

        //                    Product = f.Coverage.ProductName,
        //                    PolicyId = f.PolicyId,
        //                    UserCredId=f.UserCredential.UserCredentialId,
        //                    LicenseeId = f.Licensee.LicenseeId,

        //                }).OrderBy(a => a.ClientName)
        //                    .ThenBy(b => b.Insured)
        //                    .ThenBy(c => c.PayorName)
        //                    .ThenBy(d => d.Carrier)
        //                    .ThenBy(e => e.Product)
        //                    .ThenBy(f => f.PolicyNumber)
        //                    .ThenBy(g => g.Status)
        //                    .ThenBy(h => h.State)
        //                    .ToList();

        //        }

        //        else if (!(string.IsNullOrEmpty(policynumber)) && !(string.IsNullOrEmpty(carrier)) && (string.IsNullOrEmpty(payor)))
        //        {
        //            pos = (from f in DataModel.Policies
        //                    .Include("Client")
        //                   .Include("Payor")
        //                   .Include("Carrier")
        //                   .Include("Coverage")
        //                   .Include("UserCredential")

        //                   where
        //                    ((f.PolicyNumber.Contains(policynumber))
        //                        && (f.Carrier.CarrierName.Contains(carrier))

        //                   && (f.PolicyLicenseeId == LicenseeId)

        //                   && (f.IsDeleted == false)
        //                   && (f.Client.IsDeleted == false))
        //                   select new PolicySearched
        //                   {
        //                       ClienID = f.Client.ClientId,
        //                       ClientName = f.Client.Name,
        //                       Insured = f.Insured,
        //                       PayorName = f.Payor.PayorName,

        //                       PolicyNumber = f.PolicyNumber,
        //                       State = f.Client.State,
        //                       Carrier = f.Carrier.CarrierName,

        //                       Status = f.MasterPolicyStatu.Name,

        //                       Product = f.Coverage.ProductName,
        //                       PolicyId = f.PolicyId,
        //                    UserCredId=f.UserCredential.UserCredentialId,

        //                       LicenseeId = f.Licensee.LicenseeId,
        //                   }).OrderBy(a => a.Carrier)
        //                    .ThenBy(b => b.Insured)
        //                    .ThenBy(c => c.PayorName)
        //                    .ThenBy(d => d.Carrier)
        //                    .ThenBy(e => e.Product)
        //                    .ThenBy(f => f.PolicyNumber)
        //                    .ThenBy(g => g.Status)
        //                    .ThenBy(h => h.State)
        //                    .ToList();
        //        }
        //        else if ((string.IsNullOrEmpty(policynumber)) && !(string.IsNullOrEmpty(carrier)) && !(string.IsNullOrEmpty(payor)))
        //        {
        //            pos = (from f in DataModel.Policies
        //                   .Include("Client")
        //                   .Include("Payor")
        //                   .Include("Carrier")
        //                   .Include("Coverage")
        //                   .Include("UserCredential")
        //                   where
        //                         ((f.Carrier.CarrierName.Contains(carrier))
        //                       && (f.Payor.PayorName.Contains(payor))

        //                    && (f.PolicyLicenseeId == LicenseeId)

        //                   && (f.IsDeleted == false)
        //                   && (f.Client.IsDeleted == false))
        //                   select new PolicySearched
        //                   {
        //                       ClienID = f.Client.ClientId,
        //                       ClientName = f.Client.Name,
        //                       Insured = f.Insured,
        //                       PayorName = f.Payor.PayorName,

        //                       PolicyNumber = f.PolicyNumber,
        //                       State = f.Client.State,
        //                       Carrier = f.Carrier.CarrierName,

        //                       Status = f.MasterPolicyStatu.Name,

        //                       Product = f.Coverage.ProductName,
        //                       PolicyId = f.PolicyId,
        //                    UserCredId=f.UserCredential.UserCredentialId,

        //                       LicenseeId = f.Licensee.LicenseeId,
        //                   }).OrderBy(a => a.Carrier)
        //                   .ThenBy(b => b.Insured)
        //                    .ThenBy(c => c.PayorName)
        //                    .ThenBy(d => d.Carrier)
        //                    .ThenBy(e => e.Product)
        //                    .ThenBy(f => f.PolicyNumber)
        //                    .ThenBy(g => g.Status)
        //                    .ThenBy(h => h.State)
        //                    .ToList();
        //        }
        //        else if (!(string.IsNullOrEmpty(policynumber)) && (string.IsNullOrEmpty(carrier)) && !(string.IsNullOrEmpty(payor)))
        //        {
        //            pos = (from f in DataModel.Policies
        //                    .Include("Client")
        //                   .Include("Payor")
        //                   .Include("Carrier")
        //                   .Include("Coverage")
        //                   .Include("UserCredential")
        //                   where
        //                    ((f.PolicyNumber.Contains(policynumber))

        //                   && (f.Payor.PayorName.Contains(payor))



        //                   && (f.PolicyLicenseeId == LicenseeId)

        //                   && (f.IsDeleted == false)
        //                   && (f.Client.IsDeleted == false))
        //                   select new PolicySearched
        //                   {
        //                       ClienID = f.Client.ClientId,
        //                       ClientName = f.Client.Name,
        //                       Insured = f.Insured,
        //                       PayorName = f.Payor.PayorName,

        //                       PolicyNumber = f.PolicyNumber,
        //                       State = f.Client.State,
        //                       Carrier = f.Carrier.CarrierName,

        //                       Status = f.MasterPolicyStatu.Name,

        //                       Product = f.Coverage.ProductName,
        //                       PolicyId = f.PolicyId,
        //                    UserCredId=f.UserCredential.UserCredentialId,

        //                       LicenseeId = f.Licensee.LicenseeId,
        //                   }).OrderBy(a => a.Carrier)
        //                   .ThenBy(b => b.Insured)
        //                    .ThenBy(c => c.PayorName)
        //                    .ThenBy(d => d.Carrier)
        //                    .ThenBy(e => e.Product)
        //                    .ThenBy(f => f.PolicyNumber)
        //                    .ThenBy(g => g.Status)
        //                    .ThenBy(h => h.State)
        //                    .ToList();
        //        }
        //        else if (!(string.IsNullOrEmpty(policynumber)) && (string.IsNullOrEmpty(carrier)) && (string.IsNullOrEmpty(payor)))
        //        {
        //            pos = (from f in DataModel.Policies
        //                   //.Include("Client")
        //                   //.Include("Payor")
        //                   //.Include("Carrier")
        //                   //.Include("Coverage")
        //                   //.Include("UserCredential")

        //                   where
        //                 (
        //               ((f.IsDeleted == false)
        //                && (f.Client.IsDeleted == false)
        //                  && (f.PolicyLicenseeId == LicenseeId)


        //                   )
                              
        //                        && ((f.Client.Name.Contains(policynumber))
        //                       || (f.Insured.Contains(policynumber))

        //                         || (f.Payor.PayorName.Contains(policynumber))
        //                       || (f.Carrier.CarrierName.Contains(policynumber))//
        //                        || (f.PolicyNumber.Contains(policynumber)))
                             
        //                   )

        //                   select new PolicySearched
        //                   {
        //                       ClienID = f.Client.ClientId,
        //                       ClientName = f.Client.Name,
        //                       Insured = f.Insured,
        //                       PayorName = f.Payor.PayorName,

        //                       PolicyNumber = f.PolicyNumber,
        //                       State = f.Client.State,
        //                       Carrier = f.Carrier.CarrierName,

        //                       Status = f.MasterPolicyStatu.Name,

        //                       Product = f.Coverage.ProductName,
        //                       PolicyId = f.PolicyId,
        //                    UserCredId=f.UserCredential.UserCredentialId,

        //                       LicenseeId = f.Licensee.LicenseeId,
        //                   }).OrderBy(a => a.Carrier)
        //                    .ThenBy(b => b.Insured)
        //                    .ThenBy(c => c.PayorName)
        //                    .ThenBy(d => d.Carrier)
        //                    .ThenBy(e => e.Product)
        //                    .ThenBy(f => f.PolicyNumber)
        //                    .ThenBy(g => g.Status)
        //                    .ThenBy(h => h.State)
        //                    .ToList();
        //        }
        //        else if ((string.IsNullOrEmpty(policynumber)) && !(string.IsNullOrEmpty(carrier)) && (string.IsNullOrEmpty(payor)))
        //        {
        //            pos = (from f in DataModel.Policies
        //                    .Include("Client")
        //                   .Include("Payor")
        //                   .Include("Carrier")
        //                   .Include("Coverage")
        //                   .Include("UserCredential")
        //                   where
        //                     (
        //               ((f.IsDeleted == false)
        //                && (f.Client.IsDeleted == false)
        //                   && (f.PolicyLicenseeId == LicenseeId)


        //                   )

        //                    && ((f.Client.Name.Contains(carrier))
        //                   || (f.Insured.Contains(carrier))

        //                    || (f.Payor.PayorName.Contains(carrier))
        //                   || (f.Carrier.CarrierName.Contains(carrier))//
        //                    || (f.PolicyNumber.Contains(carrier)))

        //                    )

        //                   select new PolicySearched
        //                   {
        //                       ClienID = f.Client.ClientId,
        //                       ClientName = f.Client.Name,
        //                       Insured = f.Insured,
        //                       PayorName = f.Payor.PayorName,

        //                       PolicyNumber = f.PolicyNumber,
        //                       State = f.Client.State,
        //                       Carrier = f.Carrier.CarrierName,

        //                       Status = f.MasterPolicyStatu.Name,

        //                       Product = f.Coverage.ProductName,
        //                       PolicyId = f.PolicyId,
        //                    UserCredId=f.UserCredential.UserCredentialId,

        //                       LicenseeId = f.Licensee.LicenseeId,
        //                   }).OrderBy(a => a.Carrier)
        //                    .ThenBy(b => b.Insured)
        //                    .ThenBy(c => c.PayorName)
        //                    .ThenBy(d => d.Carrier)
        //                    .ThenBy(e => e.Product)
        //                    .ThenBy(f => f.PolicyNumber)
        //                    .ThenBy(g => g.Status)
        //                    .ThenBy(h => h.State)
        //                    .ToList();
        //        }

        //        else if ((string.IsNullOrEmpty(policynumber)) && (string.IsNullOrEmpty(carrier)) && !(string.IsNullOrEmpty(payor)))
        //        {
        //            pos = (from f in DataModel.Policies
        //                    .Include("Client")
        //                   .Include("Payor")
        //                   .Include("Carrier")
        //                   .Include("Coverage")
        //                   .Include("UserCredential")
        //                   where
        //                     (
        //               ((f.IsDeleted == false)
        //                && (f.Client.IsDeleted == false)
        //                  && (f.PolicyLicenseeId == LicenseeId)


        //                   )

        //                    && ((f.Client.Name.Contains(payor))
        //                   || (f.Insured.Contains(payor))

        //                    || (f.Payor.PayorName.Contains(payor))
        //                   || (f.Carrier.CarrierName.Contains(payor))//
        //                    || (f.PolicyNumber.Contains(payor)))

        //                    )

        //                   select new PolicySearched
        //                   {
        //                       ClienID = f.Client.ClientId,
        //                       ClientName = f.Client.Name,
        //                       Insured = f.Insured,
        //                       PayorName = f.Payor.PayorName,

        //                       PolicyNumber = f.PolicyNumber,
        //                       State = f.Client.State,
        //                       Carrier = f.Carrier.CarrierName,

        //                       Status = f.MasterPolicyStatu.Name,

        //                       Product = f.Coverage.ProductName,
        //                       PolicyId = f.PolicyId,
        //                    UserCredId=f.UserCredential.UserCredentialId,

        //                       LicenseeId = f.Licensee.LicenseeId,
        //                   }).OrderBy(a => a.Carrier)
        //                  .ThenBy(b => b.Insured)
        //                    .ThenBy(c => c.PayorName)
        //                    .ThenBy(d => d.Carrier)
        //                    .ThenBy(e => e.Product)
        //                    .ThenBy(f => f.PolicyNumber)
        //                    .ThenBy(g => g.Status)
        //                    .ThenBy(h => h.State)
        //                    .ToList();
        //        }
        //        else
        //        {
        //            pos = null;
        //        }

        //        List<PolicySearched> TempPos = null;
        //        if (SerchOwnPolicyForAgent)
        //        {
        //            TempPos = pos.Where(p => p.UserCredId == UserCridenID).ToList();
                  
        //        }
                
        //        if (UserCridenID != null && UserCridenID != Guid.Empty)
        //        {
        //            List<Guid> _Policyidlst = GetPayeePolicy(UserCridenID);
        //            if (_Policyidlst != null)
        //                pos = pos.FindAll(s => _Policyidlst.Exists(p => p == s.PolicyId));
        //            else
        //            {
        //                pos = null;
        //            }
        //        }
        //        if (TempPos != null)
        //        {
        //            if (pos == null)
        //                pos = new List<PolicySearched>();
        //            pos.AddRange(TempPos);
        //           //List<Guid>PolicyIdLsttemp= pos.Select(p => p.PolicyId).Distinct().ToList();
        //           //pos = pos.FindAll(s => PolicyIdLsttemp.Exists(p => p == s.PolicyId));
        //        }

        //        return pos;                
        //    }
        //}


        public static List<PolicySearched> GetSearchedPolicies(string strClient, string strInsured, string policynumber, string carrier, string payor, Guid LicenseeId, Guid UserCridenID, bool SerchOwnPolicyForAgent)
        {

            List<PolicySearched> pos = null;

            List<PolicySearched> allPolicy = null;

            if( (string.IsNullOrEmpty(strClient)) && (string.IsNullOrEmpty(strInsured)) && (string.IsNullOrEmpty(policynumber)) && (string.IsNullOrEmpty(carrier)) && (string.IsNullOrEmpty(payor)))
            {
                pos = null;
                return pos;
            }

            try
            {

                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    allPolicy = (from f in DataModel.Policies
                                 where ((f.PolicyLicenseeId == LicenseeId) && (f.IsDeleted == false) && (f.Client.IsDeleted == false))
                                 select new PolicySearched
                                 {
                                     ClienID = f.Client.ClientId,
                                     ClientName = f.Client.Name,
                                     Insured = f.Insured,
                                     PayorName = f.Payor.PayorName,

                                     PolicyNumber = f.PolicyNumber,
                                     State = f.Client.State,
                                     Carrier = f.Carrier.CarrierName,

                                     Status = f.MasterPolicyStatu.Name,

                                     Product = f.Coverage.ProductName,
                                     PolicyId = f.PolicyId,
                                     UserCredId = f.UserCredential.UserCredentialId,
                                     LicenseeId = f.Licensee.LicenseeId,

                                 }).OrderBy(a => a.ClientName)
                                .ThenBy(b => b.Insured)
                                .ThenBy(c => c.PayorName)
                                .ThenBy(d => d.Carrier)
                                .ThenBy(e => e.Product)
                                .ThenBy(f => f.PolicyNumber)
                                .ThenBy(g => g.Status)
                                .ThenBy(h => h.State)
                                .ToList();

                    pos = new List<PolicySearched>(allPolicy);

                    if (!string.IsNullOrEmpty(strClient))
                    {
                        pos = new List<PolicySearched>(pos.Where(t => t.ClientName != null && t.ClientName.ToUpper().Contains(strClient.Trim().ToUpper())));
                    }

                    if (!string.IsNullOrEmpty(strInsured))
                    {
                        pos = new List<PolicySearched>(pos.Where(t => t.Insured != null && t.Insured.ToUpper().Contains(strInsured.Trim().ToUpper())));
                    }

                    if (!string.IsNullOrEmpty(policynumber))
                    {
                        pos = new List<PolicySearched>(pos.Where(t => t.PolicyNumber != null && t.PolicyNumber.ToUpper().Contains(policynumber.Trim().ToUpper())));
                    }

                    if (!string.IsNullOrEmpty(carrier))
                    {
                        pos = new List<PolicySearched>(pos.Where(t => t.Carrier != null && t.Carrier.ToUpper().Contains(carrier.Trim().ToUpper())));
                    }

                    if (!string.IsNullOrEmpty(payor))
                    {
                        pos = new List<PolicySearched>(pos.Where(t => t.PayorName != null && t.PayorName.ToUpper().Contains(payor.Trim().ToUpper())));
                    }

                    List<PolicySearched> TempPos = null;
                    if (SerchOwnPolicyForAgent)
                    {
                        TempPos = pos.Where(p => p.UserCredId == UserCridenID).ToList();
                    }

                    if (UserCridenID != null && UserCridenID != Guid.Empty)
                    {
                        List<Guid> _Policyidlst = GetPayeePolicy(UserCridenID);
                        if (_Policyidlst != null)
                            pos = pos.FindAll(s => _Policyidlst.Exists(p => p == s.PolicyId));
                        else
                        {
                            pos = null;
                        }
                    }
                    if (TempPos != null)
                    {
                        if (pos == null)
                            pos = new List<PolicySearched>();
                        pos.AddRange(TempPos);
                    }
                }
            }
            catch
            {
                pos = null;
            }

            return pos;
        }
        #endregion

        private static List<Guid> GetPayeePolicy(Guid UserCredId)
        {
           // UserCredId = Guid.Parse("4e4fbddc-e59c-462b-a0b2-35f59f8b07ab");
            List<Guid> _Policyid = new List<Guid>();
            
            List<Guid> _AllOutgoingPolicies = OutgoingSchedule.GetAllPoliciesForUser(UserCredId);
            List<OutGoingPayment> _OutGoingPayment = OutGoingPayment.GetOutgoingShedule().ToList().Where(p => p.PayeeUserCredentialId == UserCredId).ToList();
            _Policyid.AddRange(_OutGoingPayment.Select(f => f.PolicyId).ToList());
            _Policyid.AddRange(_AllOutgoingPolicies);
            return _Policyid.Count != 0 ? _Policyid : null;
        }
      
    }
}


