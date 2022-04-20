using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    public static class ReferenceMaster
    {
        /// <summary>
        /// fetch the referenced source type of given id.
        /// </summary>
        /// <param name="SourceTypeId"></param>
        /// <returns></returns>
        public static DLinq.MasterSourceType GetReferencedSourceType(int SourceTypeId, DLinq.CommissionDepartmentEntities DataModel)
        {
            return (from s in DataModel.MasterSourceTypes
             where s.SourceTypeId == SourceTypeId
             select s).FirstOrDefault();
        }

        /// <summary>
        /// fetch the referenced master payor type entity object of given payortypeid.
        /// </summary>
        /// <param name="PayorTypeID"></param>
        /// <returns></returns>
        public static DLinq.MasterPayorType GetReferencedPayorType(int PayorTypeID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from t in DataModel.MasterPayorTypes
                        where t.PayorType == PayorTypeID
                        select t).FirstOrDefault();
            }
        }

        /// <summary>
        /// fetch the referenced region of payor of given payorregion id.
        /// </summary>
        /// <param name="PayorRegionID"></param>
        /// <returns></returns>
        public static DLinq.MasterPayorRegion GetReferencedRegion(int PayorRegionID,DLinq.CommissionDepartmentEntities DataModel)
        {            
            return (from r in DataModel.MasterPayorRegions
             where (r.PayorRegionId == PayorRegionID) orderby r.SortOrder
             select r).FirstOrDefault();
           
        }
        /// <summary>
        /// fetch the payorcarrier entity object of given carrier id.
        /// </summary>
        /// <param name="CarrierId"></param>
        /// <returns></returns>
        public static DLinq.Carrier GetReferencedCarrier(Guid CarrierId, DLinq.CommissionDepartmentEntities DataModel)
        {
            return (from c in DataModel.Carriers
             where c.CarrierId == CarrierId
             select c).FirstOrDefault();
        }

        /// <summary>
        /// fetch the file type entity object of given filetypeid.
        /// </summary>
        /// <param name="FileTypeId"></param>
        /// <returns></returns>
        public static DLinq.MasterFileType GetReferencedFileType(int FileTypeId, DLinq.CommissionDepartmentEntities DataModel)
        {
            return (from l in DataModel.MasterFileTypes where l.FileTypeId == FileTypeId select l).FirstOrDefault();
        }

        /// <summary>
        /// fetch user detail entity object of given user detail in param
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static DLinq.UserDetail GetReferencedUserDetail(User user, DLinq.CommissionDepartmentEntities DataModel)
        {
            var _userDetail = (from s in DataModel.UserDetails where (s.UserCredentialId == user.UserCredentialID) select s).FirstOrDefault();
            if (_userDetail == null)
            {
                _userDetail = new DLinq.UserDetail { UserCredentialId = user.UserCredentialID == Guid.Empty ? Guid.NewGuid() : user.UserCredentialID, FirstName = user.FirstName, LastName = user.LastName,
                Address = user.Address,                
                CellPhone = user.CellPhone,
                City = user.City,
                Company = user.Company,
                Email = user.Email,
                Fax = user.Fax,
                FirstYearDefault = user.FirstYearDefault,
                NickName = user.NickName,
                OfficePhone = user.OfficePhone,
                RenewalDefault = user.RenewalDefault,
                State = user.State,
                ZipCode = long.Parse(user.ZipCode),
                };
            }
            else
            {
                _userDetail.FirstName = user.FirstName;
                _userDetail.LastName = user.LastName;
                _userDetail.Address = user.Address;
                _userDetail.CellPhone = user.CellPhone;
                _userDetail.City = user.City;
                _userDetail.Company = user.Company;
                _userDetail.Email = user.Email;
                _userDetail.Fax = user.Fax;
                _userDetail.FirstYearDefault = user.FirstYearDefault;
                _userDetail.NickName = user.NickName;
                _userDetail.OfficePhone = user.OfficePhone;
                _userDetail.RenewalDefault = user.RenewalDefault;
                _userDetail.State = user.State;
                _userDetail.ZipCode = long.Parse(user.ZipCode);
            }
            return _userDetail;
        }

       
        /// <summary>
        /// fetch Payor entity object of given id.
        /// </summary>
        /// <param name="PayerId"></param>
        /// <returns></returns>
        public static DLinq.Payor GetReferencedPayor(Guid? PayorId,DLinq.CommissionDepartmentEntities DataModel)
        {
            return (from p in DataModel.Payors
                    where p.PayorId == PayorId
                    select p).FirstOrDefault();

        }
        /// <summary>
        ///  Return the reference of MasterAccessrigt
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DataModel"></param>
        /// <returns></returns>
        public static DLinq.MasterAccessRight GetReferencedMasterAccessRight(ModuleAccessRight accessRight, DLinq.CommissionDepartmentEntities DataModel)
        {
            return (from p in DataModel.MasterAccessRights
                    where p.AccessRightId == (int)accessRight
                    select p).FirstOrDefault();
        }

        public static DLinq.MasterModule GetReferencedMasterModule(MasterModule module, DLinq.CommissionDepartmentEntities DataModel)
        {

            return (from p in DataModel.MasterModules
                    where p.ModuleId == (int)module
                        select p).FirstOrDefault();
            
        }
        /// <summary>
        /// fetch the licenseee from the table of licensee id .
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <returns></returns>
        public static DLinq.Licensee GetReferencedLicensee(Guid? LicenseeId, DLinq.CommissionDepartmentEntities DataModel)
        {
           
            return (from u in DataModel.Licensees
                        where u.LicenseeId == LicenseeId
                        select u).FirstOrDefault();
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <param name="DataModel"></param>
        /// <returns></returns>
        public static DLinq.MasterService GetReferencedServiceName(int? ServieId, DLinq.CommissionDepartmentEntities DataModel)
        {

            return (from u in DataModel.MasterServices
                    where u.ServiceId == ServieId
                    select u).FirstOrDefault();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LicenseeId"></param>
        /// <param name="DataModel"></param>
        /// <returns></returns>
        public static DLinq.MasterServiceChargeType GetReferencedServiceChargeType(int? ServiceChargeType, DLinq.CommissionDepartmentEntities DataModel)
        {

            return (from u in DataModel.MasterServiceChargeTypes
                    where u.SCTypeId == ServiceChargeType
                    select u).FirstOrDefault();

        }

        /// <summary>
        /// fetch the master role of given id if exist in the master role table.
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static DLinq.MasterRole GetReferencedRole(int RoleId, DLinq.CommissionDepartmentEntities DataModel)
        {
            return (from e in DataModel.MasterRoles where (e.RoleId == RoleId) select e).First();
        }
        /// <summary>
        /// fetch the master status of given id if exist in the master licenseestatus table.
        /// </summary>
        /// <param name="LicenseeStatusId"></param>
        /// <returns></returns>
        public static DLinq.MasterLicenseStatu GetReferencedLicenseeStatus(int LicenseeStatusId, DLinq.CommissionDepartmentEntities DataModel)
        {
            return (from l in DataModel.MasterLicenseStatus where (l.LicenseStatusId == LicenseeStatusId) select l).FirstOrDefault();
        }
        
        public static DLinq.UserCredential GetreferencedUserCredential(Guid ID, DLinq.CommissionDepartmentEntities DataModel)
        {
            return (from u in DataModel.UserCredentials
                   where u.UserCredentialId == ID
             select u).FirstOrDefault();

        }
        public static DLinq.MasterPayorToolMaskFieldType GetreferencedMaskFieldType(int MaskID, DLinq.CommissionDepartmentEntities DataModel)
        {
            return ((from l in DataModel.MasterPayorToolMaskFieldTypes where l.PTMaskFieldTypeId == MaskID select l).FirstOrDefault());
        }
        public static DLinq.MasterPayorToolAvailableField GetreferencedPayorToolAvailableField(int ID, DLinq.CommissionDepartmentEntities DataModel)
        {
            return ((from l in DataModel.MasterPayorToolAvailableFields where l.PTAvailableFieldId == ID select l).FirstOrDefault());
        }
        public static DLinq.PayorTool GetreferencedPayorTool(Guid ID, DLinq.CommissionDepartmentEntities DataModel)
        {
            return ((from l in DataModel.PayorTools where l.PayorToolId == ID select l).FirstOrDefault());
        }
        

    }
}
