using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PayorSiteLoginInfo
    {
        #region IEditable<PayorUserWebSite> Members

        public void AddUpdate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorSiteLoginInfo existingPsLoginInfo = (from _p in DataModel.PayorSiteLoginInfoes
                                                                where
                                                                    _p.SiteLoginID == this.SiteID
                                                                select _p).FirstOrDefault();


                if (existingPsLoginInfo != null) //Update the current Records
                {
                    existingPsLoginInfo.Login = this.LogInName;
                    existingPsLoginInfo.Password = this.Password;
                    existingPsLoginInfo.PayorReference.Value = ReferenceMaster.GetReferencedPayor(this.PayorID, DataModel);

                    DataModel.SaveChanges();
                }
                else //Insert New recors
                {
                    existingPsLoginInfo = new DLinq.PayorSiteLoginInfo
                    {

                        SiteLoginID = this.SiteID,
                        Login = this.LogInName,
                        Password = this.Password
                    };
                
                    existingPsLoginInfo.PayorReference.Value = ReferenceMaster.GetReferencedPayor(this.PayorID, DataModel);
                    existingPsLoginInfo.LicenseeId = this.LicenseID.Value;
                    existingPsLoginInfo.Licensee = DataModel.Licensees.FirstOrDefault(s => s.LicenseeId == this.LicenseID);
                    DataModel.AddToPayorSiteLoginInfoes(existingPsLoginInfo);
                    DataModel.SaveChanges();
                 
                }

            }
        }

        public void Delete()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorSiteLoginInfo _info = (from i in DataModel.PayorSiteLoginInfoes
                                                  where i.SiteLoginID == this.SiteID
                                                  select i).FirstOrDefault();

                if (_info != null)
                {
                    DataModel.DeleteObject(_info);
                    DataModel.SaveChanges();
                }
            }
        }

        public PayorSiteLoginInfo GetOfID()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region "Data members aka - public properties"
        [DataMember]
        public Guid SiteID { get; set; }
        [DataMember]
        public string LogInName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public Guid PayorID { get; set; }
        [DataMember]
        public Guid UserID { get; set; }
        [DataMember]
        public Guid? LicenseID { get; set; }
        #endregion

        public static List<PayorSiteLoginInfo> GetPayorUserWebSite(Guid licId, Guid payorId)
        {
            if (licId == Guid.Empty || payorId == Guid.Empty)
                return null;

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return
                    (from p in DataModel.PayorSiteLoginInfoes
                     where p.PayorID == payorId && p.LicenseeId == licId
                     select new PayorSiteLoginInfo
                     {
                         LogInName = p.Login,
                         Password = p.Password,
                         PayorID = p.Payor.PayorId,
                         SiteID = p.SiteLoginID,
                         LicenseID = p.LicenseeId
                     }).OrderBy(param => param.LogInName).ToList();
            }
        }

        public static List<PayorSiteLoginInfo> GetPayorSiteLogins(Guid payorId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return
                    (from p in DataModel.PayorSiteLoginInfoes
                     where p.PayorID == payorId
                     select new PayorSiteLoginInfo
                     {
                         LogInName = p.Login,
                         Password = p.Password,
                         PayorID = p.Payor.PayorId,
                         SiteID = p.SiteLoginID
                     }).OrderBy(param => param.LogInName).ToList();
            }
        }

        public static List<PayorSiteLoginInfo> GetLicenseeUsers(Guid licenseeId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                if (licenseeId != Guid.Empty)
                    return (from hd in DataModel.PayorSiteLoginInfoes
                            where hd.LicenseeId == licenseeId
                            select new PayorSiteLoginInfo
                            {
                                LogInName = hd.Login,
                                Password = hd.Password,
                                PayorID = hd.Payor.PayorId,
                                SiteID = hd.SiteLoginID,
                                LicenseID = hd.LicenseeId
                            }).ToList();
                else


                    return (from hd in DataModel.PayorSiteLoginInfoes
                            where (hd.Licensee.IsDeleted == false && hd.Licensee.LicenseStatusId != 1)
                            select new PayorSiteLoginInfo
                            {
                                LogInName = hd.Login,
                                Password = hd.Password,
                                PayorID = hd.Payor.PayorId,
                                SiteID = hd.SiteLoginID,
                                LicenseID = hd.LicenseeId
                            }).ToList();

            }
        }
    }
}
