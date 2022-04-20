using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using System.IO;
using System.Threading;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class User : UserDetail
    {
        #region  "Data members i.e. public properties"
        [DataMember]
        public String UserName { get; set; }

        [DataMember]
        public static String testName { get; set; }

        [DataMember]
        public String RembStatus { get; set; }
        [DataMember]
        public String SStatus { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string PasswordHintQ { get; set; }
        [DataMember]
        public string PasswordHintA { get; set; }
        [DataMember]
        public Guid UserCredentialID { get; set; }
        [DataMember]
        public bool IsHouseAccount { get; set; }
        //Added 13012016
        [DataMember]
        public bool? IsAccountExec { get; set; }
        //Added 04062014
        [DataMember]
        public bool? DisableAgentEditing { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public bool? IsLicenseDeleted { get; set; }
        [DataMember]
        public Guid? LicenseeId { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime CreateOn { get; set; }
        [DataMember]
        public string LicenseeName { get; set; }
        [DataMember]
        public List<Client> UserClients { get; set; }
        [DataMember]
        public UserRole Role { get; set; }
        [DataMember]
        public string BGUserId { get; set; }
        [DataMember]
        public Guid AttachedToLicensee { get; set; }
        [IgnoreDataMember]
        private List<UserPermissions> _Permissions;
        [DataMember]
        public List<UserPermissions> Permissions
        {
            get
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    List<UserPermissions> _permissions = null;
                    _permissions = (this.Role == UserRole.DEP) ?  null : (from usr in DataModel.UserPermissions
                                    where (usr.UserCredential.UserCredentialId == this.UserCredentialID && usr.UserCredential.IsDeleted == false)
                                    select new UserPermissions
                                    {
                                        UserID = usr.UserCredential.UserCredentialId,
                                        Module = (MasterModule)usr.MasterModule.ModuleId,
                                        Permission = (ModuleAccessRight)usr.MasterAccessRight.AccessRightId,
                                        UserPermissionId = usr.UserPermissionId
                                    }).OrderBy(p => p.Module).ToList();
                    return _permissions ?? new List<UserPermissions>();
                }
            }
            set
            {
                _Permissions = value;
            }
        }
        /// <summary>
        /// all the users to which the current logged in user have the accessibility to see their data.
        /// </summary> 
        [IgnoreDataMember]
        private List<LinkedUser> _Linkedusers;
        [DataMember]
        public List<LinkedUser> LinkedUsers
        {
            get
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    List<LinkedUser> LnkUsers = null;

                    //if (this.Role == UserRole.Agent && this.IsHouseAccount == false)
                    //{
                    //    List<DLinq.UserCredential> connectedLinkedUsers = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == this.UserCredentialID).UserCredentials.Where(s => s.IsDeleted == false && s.UserCredentialId != this.UserCredentialID).ToList();

                    //    if (connectedLinkedUsers != null && connectedLinkedUsers.Count != 0)
                    //        LnkUsers = (from usr in connectedLinkedUsers
                    //                    select new LinkedUser
                    //                    {
                    //                        UserId = usr.UserCredentialId,
                    //                        LastName = usr.UserDetail.LastName,
                    //                        FirstName = usr.UserDetail.FirstName,
                    //                        NickName = usr.UserDetail.NickName,
                    //                        IsConnected = true,
                    //                        UserName = usr.UserName
                    //                    }).ToList();

                    //    List<Guid> connectedUserIds = null;
                    //    if (LnkUsers == null)
                    //        connectedUserIds = new List<Guid>();
                    //    else
                    //        connectedUserIds = LnkUsers.Select(s => s.UserId).ToList();

                    //    List<LinkedUser> notConnectedLinkedUsers = (from usr in DataModel.UserCredentials
                    //                                                where (!connectedUserIds.Contains(usr.UserCredentialId)) && usr.LicenseeId == this.LicenseeId && usr.RoleId == 3 && (usr.UserCredentialId != this.UserCredentialID) && (usr.IsDeleted == false)
                    //                                                select new LinkedUser
                    //                                                {
                    //                                                    UserId = usr.UserCredentialId,
                    //                                                    LastName = usr.UserDetail.LastName,
                    //                                                    FirstName = usr.UserDetail.FirstName,
                    //                                                    NickName = usr.UserDetail.NickName,
                    //                                                    IsConnected = false,
                    //                                                    UserName = usr.UserName
                    //                                                }).ToList();

                    //    if (LnkUsers != null)
                    //        LnkUsers.AddRange(notConnectedLinkedUsers);
                    //    else if (notConnectedLinkedUsers != null)
                    //        LnkUsers = notConnectedLinkedUsers;
                    //}
                    //else
                    //{
                    //    List<DLinq.UserCredential> allUsersExceptHO = DataModel.UserCredentials.Where(s => s.LicenseeId == this.LicenseeId && s.IsDeleted == false && s.RoleId == 3 && s.IsHouseAccount == false).ToList();
                    //    LnkUsers = (from usr in allUsersExceptHO
                    //                select new LinkedUser
                    //                {
                    //                    UserId = usr.UserCredentialId,
                    //                    LastName = usr.UserDetail.LastName,
                    //                    FirstName = usr.UserDetail.FirstName,
                    //                    NickName = usr.UserDetail.NickName,
                    //                    IsConnected = true,
                    //                    UserName = usr.UserName
                    //                }).ToList();
                    //}

                    return LnkUsers;
                }
            }
            set
            {
                _Linkedusers = value;
            }
        }
        [DataMember]
        public bool IsNewsToFlash { get; set; }
        [DataMember]
        public Guid? HouseOwnerId { get; set; }
        [DataMember]
        public Guid? AdminId { get; set; }
        [DataMember]
        public string WebDavPath { get; set; }

        #endregion

        #region IEditable<User> Members
        /// <summary>
        /// Author:Ankit khandelwal
        /// CreateOn:Sept 05,2018
        /// Purpose:Save user for Benefit Pro. with BGUserID
        /// </summary>
        /// <param name="userDetails"></param>
        /// <param name="LicenseeID"></param>
        /// <returns></returns>
        public string AddUpdateBGUser(out string type, out string message)
        {
            string update = "0";

            try
            {
                type = "0";
                message = "";
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _BGUserId = DataModel.UserCredentials.FirstOrDefault(s => s.BGUserId == BGUserId && s.LicenseeId == LicenseeId);
                    var options = new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                        Timeout = TimeSpan.FromMinutes(0.15)
                    };
                    if (_BGUserId != null)
                    {
                        DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                        EntityConnection ec = (EntityConnection)ctx.Connection;
                        SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                        string adoConnStr = sc.ConnectionString;
                        using (SqlConnection con = new SqlConnection(adoConnStr))
                        {
                            try
                            {
                                ActionLogger.Logger.WriteImportPolicyLog(" BGUserId exist in database BGUSerID:" + this.BGUserId, true);
                                using (SqlCommand cmd = new SqlCommand("usp_BgUpdateUserdetails", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@BGUserId", this.BGUserId);
                                    cmd.Parameters.AddWithValue("@FirstName", this.FirstName);
                                    cmd.Parameters.AddWithValue("@LastName", this.LastName);
                                    cmd.Parameters.AddWithValue("@Email", this.Email);
                                    cmd.Parameters.AddWithValue("@CellPhone", this.CellPhone);
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                }
                                update = "1";
                                type = "0";
                            }
                            catch (Exception ex)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog(ex.Message, true);
                                update = "1";
                                type = "2";
                                message = ex.Message;
                                throw ex;
                            }
                        }
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog(" Add new user  in database with BGUSerID:" + this.BGUserId, true);
                        var _UserExist = DataModel.UserDetails.FirstOrDefault(s => s.FirstName == FirstName && s.LastName == LastName && s.CellPhone == CellPhone && s.Email == Email);
                        //  ActionLogger.Logger.WriteImportPolicyLog(" Add new user  in _UserExist 1111" + this.BGUserId, true);
                        if (_UserExist == null)
                        {
                            try
                            {
                                ActionLogger.Logger.WriteImportPolicyLog(" Add new user  in _UserExist == null:" + this.BGUserId, true);
                                AddUpdateUser();
                                update = "1";
                                type = "1";
                            }
                            catch (Exception ex)
                            {
                                ActionLogger.Logger.WriteImportPolicyLog(ex.Message, true);
                                update = "1";
                                type = "2";
                                message = ex.Message;
                                throw ex;
                            }
                        }
                        else
                        {
                            //  ActionLogger.Logger.WriteImportPolicyLog(" Add new user  in _UserExist112: " + this.BGUserId, true);
                            var _GetBGUserId = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == _UserExist.UserCredentialId && s.LicenseeId == LicenseeId);
                            //  ActionLogger.Logger.WriteImportPolicyLog(" Add new user  in _UserExist113: " , true);
                            if (_GetBGUserId != null && _GetBGUserId.BGUserId == null)
                            {
                                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                                EntityConnection ec = (EntityConnection)ctx.Connection;
                                SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                                string adoConnStr = sc.ConnectionString;
                                using (SqlConnection con = new SqlConnection(adoConnStr))
                                {
                                    try
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog(" update  user BGUSerId  in database with BGUSerID:" + this.BGUserId, true);
                                        using (SqlCommand cmd = new SqlCommand("usp_BgUserIdUpdate", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@BGUserId", this.BGUserId);
                                            cmd.Parameters.AddWithValue("@FirstName", this.FirstName);
                                            cmd.Parameters.AddWithValue("@LastName", this.LastName);
                                            cmd.Parameters.AddWithValue("@Email", this.Email);
                                            cmd.Parameters.AddWithValue("@CellPhone", this.CellPhone);
                                            con.Open();
                                            cmd.ExecuteNonQuery();
                                            update = "1";
                                            type = "0";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ActionLogger.Logger.WriteImportPolicyLog(ex.Message, true);
                                        update = "1";
                                        type = "2";
                                        message = ex.Message;
                                        throw ex;
                                    }
                                }
                            }
                            else
                            {
                                ActionLogger.Logger.WriteImportPolicyLog(" update  user BGUSerId  in database with BGUSerID 222 :" + this.BGUserId, true);
                                this.UserCredentialID = Guid.Empty;
                                AddUpdateUser();
                                update = "1";
                                type = "1";

                            }

                        }
                    }
                }
                //  return objStatus;
            }
            catch (Exception ex)
            {
                update = "1";
                type = "2";
                ActionLogger.Logger.WriteImportPolicyLog("exception on save user: " + ex.Message, true);
                message = ex.Message;
                throw ex;

            }
            return update;
        }


        /// <summary>
        /// ModifiedBy:Ankit khandelwal
        /// ModifiedOn:Sept 05,2018
        /// Purpose:Save user for Benefit Pro. with BGUserID
        /// </summary>
        /// <param name="userDetails"></param>
        /// <param name="LicenseeID"></param>
        /// <returns></returns>
        public void AddUpdateUser()
        {
            try
            {
                Guid guidUserID;
                bool isNewUser = false;
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var _dtUsers = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == this.UserCredentialID);
                    var options = new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted,
                        Timeout = TimeSpan.FromMinutes(0.15)
                    };
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, options))
                    {
                        if (_dtUsers == null)
                        {
                            _dtUsers = new DLinq.UserCredential
                            {
                                UserCredentialId = ((this.UserCredentialID == Guid.Empty) ? Guid.NewGuid() : this.UserCredentialID),
                                UserName = this.UserName,
                                Password = this.Password,
                                PasswordHintAnswer = this.PasswordHintA,
                                PasswordHintQuestion = this.PasswordHintQ,
                                RoleId = (int)this.Role,
                                LicenseeId = this.LicenseeId,
                                CreatedOn = DateTime.Now,
                                BGUserId = this.BGUserId,
                                IsUserActiveOnWeb = (this.Role == UserRole.DEP) ? true : false //Added June 2020 for DEU user logon t oweb 

                            };
                            DataModel.AddToUserCredentials(_dtUsers);
                            isNewUser = true;
                        }
                        else
                        {
                            _dtUsers.UserName = this.UserName;
                            _dtUsers.Password = this.Password;
                            _dtUsers.PasswordHintAnswer = this.PasswordHintA;
                            _dtUsers.PasswordHintQuestion = this.PasswordHintQ;
                            _dtUsers.BGUserId = this.BGUserId;
                        }

                        var _dtUserDetail = DataModel.UserDetails.FirstOrDefault(s => s.UserCredentialId == this.UserCredentialID);

                        if (_dtUserDetail == null)
                        {
                            _dtUserDetail = new DLinq.UserDetail
                            {
                                UserCredentialId = _dtUsers.UserCredentialId,
                                FirstName = this.FirstName,
                                LastName = this.LastName,
                                Company = this.Company,
                                NickName = this.NickName,
                                Address = this.Address,
                                ZipCode = this.ZipCode.CustomParseToLong(),
                                City = this.City,
                                State = this.State,
                                Email = this.Email,
                                FirstYearDefault = this.FirstYearDefault,
                                RenewalDefault = this.RenewalDefault,
                                ReportForEntireAgency = this.ReportForEntireAgency,
                                ReportForOwnBusiness = this.ReportForOwnBusiness,
                                OfficePhone = this.OfficePhone,
                                CellPhone = this.CellPhone,
                                Fax = this.Fax,
                                AddPayeeOn = null
                            };
                            DataModel.AddToUserDetails(_dtUserDetail);
                        }
                        else
                        {
                            _dtUserDetail.FirstName = FirstName;
                            _dtUserDetail.LastName = LastName;
                            _dtUserDetail.Company = Company;
                            _dtUserDetail.NickName = NickName;
                            _dtUserDetail.Address = Address;
                            _dtUserDetail.ZipCode = ZipCode.CustomParseToLong();
                            _dtUserDetail.City = City;
                            _dtUserDetail.State = State;
                            _dtUserDetail.Email = Email;
                            _dtUserDetail.OfficePhone = OfficePhone;
                            _dtUserDetail.CellPhone = CellPhone;
                            _dtUserDetail.Fax = Fax;
                        }
                        //DataModel.SaveChanges();
                    guidUserID = _dtUsers.UserCredentialId;
                    DLinq.UserPermission _permision = null;

                       
                        if (this.Role != UserRole.DEP) //Jyotisna - May 22, 2020 - Following code doesn't work correctky for DEU, so calling another one at the end 
                        {
                            foreach (UserPermissions up in this._Permissions)
                            {
                                
                                up.UserID = guidUserID;
                                //up.UserPermissionId = guidUserID;
                                DLinq.MasterAccessRight _Right = ReferenceMaster.GetReferencedMasterAccessRight(up.Permission, DataModel);
                                DLinq.MasterModule _Module = ReferenceMaster.GetReferencedMasterModule(up.Module, DataModel);
                                DLinq.UserCredential _UC = _dtUsers;// ReferenceMaster.GetreferencedUserCredential(up.UserID.Value, DataModel);

                                _permision = (from p in DataModel.UserPermissions where p.UserPermissionId == up.UserPermissionId select p).FirstOrDefault();
                                if (_permision != null)
                                {
                                    _permision.MasterAccessRightReference.Value = _Right;
                                    _permision.UserCredentialReference.Value = _UC;
                                    _permision.MasterModuleReference.Value = _Module;
                                }
                                else
                                {
                                    _permision = new DLinq.UserPermission();
                                    _permision.MasterAccessRightReference.Value = _Right;
                                    _permision.UserCredentialReference.Value = _UC;
                                    _permision.MasterModuleReference.Value = _Module;
                                    _permision.UserPermissionId = up.UserPermissionId.Value;
                                    DataModel.AddToUserPermissions(_permision);

                                }
                                //DataModel.SaveChanges();
                            }
                        }
                    //Add default permission for the user...
                    //UpdateUserPermission();

                    ////Acme
                    //if (isNewUser)
                    //{
                    //    LinkNewUserToHouse();
                    //}
                    if (isNewUser)
                    {
                        if (_dtUsers != null)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("LinkNewUserToHouse newUser: " + _dtUsers.UserName, true);
                            var houseList = DataModel.UserCredentials.Where(s => s.LicenseeId == _dtUsers.LicenseeId && s.IsHouseAccount).ToList();
                            if (houseList != null && houseList.Count > 0)
                            {
                                foreach (DLinq.UserCredential user in houseList)
                                {
                                    user.UserCredentials.Add(_dtUsers);
                                }
                            }
                        }
                        else
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("LinkNewUserToHouse newUser is null", true);
                        }
                        //LinkNewUserToHouse();
                    }
                    DataModel.SaveChanges();
                    transaction.Complete();
                    }
                }

                //Jyotisna - May 22, 2020 - Save default permissions for new  DEP 
                if (isNewUser && this.Role == UserRole.DEP)
                {
                    SaveDefaultUserPermissions(this.UserCredentialID, (int) this.Role);
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("exception on save user: " + ex.Message, true);
            }
        }



        /// <summary>
        /// /Created By: Ankit khandelwal - Taken from web version 
        /// Created on: Sept 06, 2018
        /// Purpose: To save Default permission to user 
        /// </summary>
        /// <param name="userCredentialId"></param>
        /// <param name="roleId"></param>
        public static void SaveDefaultUserPermissions(Guid userCredentialId, int roleId)
        {
            try
            {
                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                string adoConnStr = sc.ConnectionString;
                using (SqlConnection con = new SqlConnection(adoConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_AddDefaultUserPermission", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userCredentialId", userCredentialId);
                        cmd.Parameters.AddWithValue("@roleId", roleId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("SaveDefaultUserPermissions exception: " + ex.Message, true);
                throw ex;
            }
        }

        //New method to save new user with house account

        public void LinkNewUserToHouse()
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var newUser = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == this.UserCredentialID);
                    if (newUser != null)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("LinkNewUserToHouse newUser: " + newUser.UserName, true);
                        var houseList = DataModel.UserCredentials.Where(s => s.LicenseeId == newUser.LicenseeId && s.IsHouseAccount).ToList();
                        if (houseList != null && houseList.Count > 0)
                        {
                            foreach (DLinq.UserCredential user in houseList)
                            {
                                user.UserCredentials.Add(newUser);
                            }
                        }
                        DataModel.SaveChanges();
                    }
                    else
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("LinkNewUserToHouse newUser is null", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("exception on LinkNewUserToHouse: " + ex.Message, true);
            }
        }

        public void AddUpdateUserPermission()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.UserDetail userDetail = DataModel.UserDetails.FirstOrDefault(s => s.UserCredentialId == this.UserCredentialID);

                if (userDetail != null)
                {
                    //added
                    userDetail.DisableAgentEditing = this.DisableAgentEditing;
                    userDetail.RenewalDefault = this.RenewalDefault;
                    userDetail.FirstYearDefault = this.FirstYearDefault;
                    userDetail.ReportForEntireAgency = this.ReportForEntireAgency;
                    userDetail.ReportForOwnBusiness = this.ReportForOwnBusiness;
                    DataModel.SaveChanges();
                }
            }

            bool bvalue = false;
            if (this.IsAccountExec == true)
            {
                bvalue = true;
            }
            try
            {
                UpdateAccountExec(this.UserCredentialID, bvalue);
            }
            catch
            {
            }

            try
            {
                UpdateUserPermission();
            }
            catch
            {
            }
            try
            {
                SaveLinkedUsers();
            }
            catch
            {
            }
            try
            {
                SaveLinkedUsers(this._Linkedusers, this.UserCredentialID);
            }
            catch
            {
            }
        }

        public bool Delete()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete user request, userId: " + this.UserCredentialID, true);

                int associtedPolicyCount = DataModel.Policies.Where(s => s.CreatedBy == this.UserCredentialID && s.IsDeleted == false).Count();
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete user request, associtedPolicyCount: " + associtedPolicyCount, true);

                bool UserPresentAsPayee = OutGoingPayment.IsUserPresentAsPayee(this.UserCredentialID) | OutgoingSchedule.IsUserPresentAsPayee(this.UserCredentialID);
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete user request, UserPresentAsPayee: " + UserPresentAsPayee, true);

                if (associtedPolicyCount != 0 || UserPresentAsPayee)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete user request, cannot be deleted, returning.. ", true);
                    return false;
                }

                var _dtUsers = (from u in DataModel.UserCredentials
                                where (u.UserCredentialId == this.UserCredentialID)
                                select u).FirstOrDefault();

                if (_dtUsers != null)
                    _dtUsers.IsDeleted = true;

                DataModel.SaveChanges();
                ActionLogger.Logger.WriteImportPolicyLog(DateTime.Now.ToString() + " Delete user request, user succesfully deleted: ", true);

                return true;
            }
        }

        public void UpdateAccountExec(Guid userCredencialID, bool bvalue)
        {
            try
            {
                if (CheckAccoutExec(userCredencialID))
                {
                    return;
                }
                else
                {
                    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                    {
                        DLinq.UserCredential UserCredential = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == userCredencialID);

                        if (UserCredential != null)
                        {
                            //added
                            UserCredential.IsAccountExec = bvalue;
                            DataModel.SaveChanges();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public bool CheckAccoutExec(Guid userCredencialID, bool bvalue)
        {
            bool bValue = false;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.Policy objPolicy = DataModel.Policies.FirstOrDefault(s => s.UserCredentialId == userCredencialID);

                    if (objPolicy != null)
                    {
                        bValue = true;
                    }
                }
            }
            catch
            {
            }

            return bValue;
        }

        public bool CheckAccoutExec(Guid userCredencialID, string agencyName  = "")
        {
            ActionLogger.Logger.WriteImportPolicyLog(" CheckAccoutExec , user:  " + UserCredentialID, true, agencyName);
            bool bValue = false;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.UserCredential UserCredential = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == userCredencialID);

                    if (UserCredential != null)
                    {
                        UserCredential.IsAccountExec = true;
                        DataModel.SaveChanges();
                        bValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception CheckAccoutExec , user:  " + UserCredentialID + ", ex: " + ex.Message, true, agencyName);
            }

            return bValue;
        }

        #endregion


        public static Boolean IsAccountExecPolicyExist(Guid? usercredentialId, string nickName)
        {
            Boolean result = false;
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("IsAccountExecPolicyExist processing begins " + "nickName: " + nickName, true);
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("IsAccountExecPolicyExist", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userCredentialId", usercredentialId);
                        cmd.Parameters.AddWithValue("@nickName", nickName);
                        cmd.Parameters.Add("@isPolicyExist", SqlDbType.Bit);
                        cmd.Parameters["@isPolicyExist"].Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteScalar();
                        //string strResult = Convert.ToString(cmd.ExecuteScalar());
                        result = Convert.ToBoolean(cmd.Parameters["@isPolicyExist"].Value);
                    }
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("IsAccountExecPolicyExist:Exception User: " + usercredentialId + "Exception:" + ex.Message, true);
            }
            return result;
        }
        public User GetUserWithinLicensee(Guid userCredencialID, Guid LicenseeID)
        {
            User objPolicy = new User();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    objPolicy = (from uc in DataModel.UserCredentials
                                 where (uc.UserCredentialId == userCredencialID) && (uc.IsDeleted == false) && (uc.LicenseeId == LicenseeID)
                                 select new User
                                 {
                                     UserCredentialID = uc.UserCredentialId

                                 }
                              ).FirstOrDefault();
                }

            }
            catch
            {
            }

            return objPolicy;
        }

        public static void HouseAccoutTransferProcess(User user)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.UserCredential NewHOUser = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == user.UserCredentialID);

                    if (NewHOUser == null)
                        throw new InvalidOperationException("Target user is not exist");

                    DLinq.UserCredential HOUser = DataModel.UserCredentials.First(s => s.LicenseeId == user.LicenseeId && s.IsDeleted == false && s.IsHouseAccount == true);
                    HOUser.IsHouseAccount = false;
                    NewHOUser.IsHouseAccount = true;

                    DataModel.Policies.Where(s => s.CreatedBy == HOUser.UserCredentialId && s.IsDeleted == false).ToList().ForEach(s => s.CreatedBy = NewHOUser.UserCredentialId);
                    DataModel.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public static List<UserPermissions> GetCurrentPermission(Guid UserCredentialId)
        {

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<UserPermissions> _permissions = null;
                _permissions = (from usr in DataModel.UserPermissions
                                where (usr.UserCredential.UserCredentialId == UserCredentialId && usr.UserCredential.IsDeleted == false)
                                select new UserPermissions
                                {
                                    UserID = usr.UserCredential.UserCredentialId,
                                    Module = (MasterModule)usr.MasterModule.ModuleId,
                                    Permission = (ModuleAccessRight)usr.MasterAccessRight.AccessRightId,
                                    UserPermissionId = usr.UserPermissionId
                                }).OrderBy(p => p.Module).ToList();
                return _permissions;
            }

        }
        /// <summary>
        /// invoke to change/update the various kind permission of the user.
        /// </summary>
        /// <returns>true if permissions updated successfully for this user, else false</returns>
        public void UpdateUserPermission()
        {
            try
            {
                if (_Permissions == null)
                    return;

                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.UserPermission _permision = null;

                    foreach (UserPermissions up in this._Permissions)
                    {
                        DLinq.MasterAccessRight _Right = ReferenceMaster.GetReferencedMasterAccessRight(up.Permission, DataModel);
                        DLinq.MasterModule _Module = ReferenceMaster.GetReferencedMasterModule(up.Module, DataModel);
                        DLinq.UserCredential _UC = ReferenceMaster.GetreferencedUserCredential(up.UserID.Value, DataModel);

                        _permision = (from p in DataModel.UserPermissions where p.UserPermissionId == up.UserPermissionId select p).FirstOrDefault();
                        if (_permision != null)
                        {
                            _permision.MasterAccessRightReference.Value = _Right;
                            _permision.UserCredentialReference.Value = _UC;
                            _permision.MasterModuleReference.Value = _Module;
                        }
                        else
                        {
                            _permision = new DLinq.UserPermission();
                            _permision.MasterAccessRightReference.Value = _Right;
                            _permision.UserCredentialReference.Value = _UC;
                            _permision.MasterModuleReference.Value = _Module;
                            _permision.UserPermissionId = up.UserPermissionId.Value;
                            DataModel.AddToUserPermissions(_permision);

                        }
                        DataModel.SaveChanges();
                    }
                }
            }
            catch
            {
            }
        }

        public static string getUserEmail(Guid UserID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                string userEmail = (from results in DataModel.UserDetails
                                    where results.UserCredentialId == UserID
                                    select results.Email).FirstOrDefault();
                return userEmail;
            }
        }

        public static User GetUserIdWise(Guid UserCredId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                User _user = (from uc in DataModel.UserCredentials

                              where (uc.UserCredentialId == UserCredId) && (uc.IsDeleted == false)
                              select new User
                              {
                                  UserCredentialID = uc.UserCredentialId,
                                  FirstName = uc.UserDetail.FirstName,
                                  LastName = uc.UserDetail.LastName,
                                  City = uc.UserDetail.City,
                                  Company = uc.UserDetail.Company,
                                  State = uc.UserDetail.State,
                                  NickName = uc.UserDetail.NickName,
                                  Email = uc.UserDetail.Email,
                                  Address = uc.UserDetail.Address,
                                  OfficePhone = uc.UserDetail.OfficePhone,
                                  CellPhone = uc.UserDetail.CellPhone,
                                  Fax = uc.UserDetail.Fax,
                                  LicenseeId = uc.Licensee.LicenseeId,
                                  LicenseeName = uc.Licensee.Company,
                                  UserName = uc.UserName,
                                  Password = uc.Password,
                                  PasswordHintQ = uc.PasswordHintQuestion,
                                  PasswordHintA = uc.PasswordHintAnswer,
                                  IsHouseAccount = uc.IsHouseAccount,
                                  IsAccountExec = uc.IsAccountExec,
                                  //Added
                                  DisableAgentEditing = uc.UserDetail.DisableAgentEditing,

                                  FirstYearDefault = uc.UserDetail.FirstYearDefault,
                                  RenewalDefault = uc.UserDetail.RenewalDefault,
                                  Role = (UserRole)uc.MasterRole.RoleId,
                                  IsNewsToFlash = uc.IsNewsToFlash
                              }
                         ).FirstOrDefault();


                //DLinq.UserDetail userDetail = DataModel.UserDetails.FirstOrDefault(s => s.UserCredentialId == _user.UserCredentialID);
                //if (userDetail.ZipCode != null)
                //    _user.ZipCode = userDetail.ZipCode.Value.ToString("D5");
                //else
                //    _user.ZipCode = null;

                return _user;
            }
        }

        public static Guid GetLicenseeUserCredentialId(Guid licId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                if (licId == null || licId == Guid.Empty)
                {
                    Guid userId = DataModel.UserCredentials.Where(s => s.MasterRole.RoleId == 1).First().UserCredentialId;
                    return userId;
                }
                else
                {
                    Guid userId = DataModel.UserCredentials.Where(s => s.MasterRole.RoleId == 2 && s.LicenseeId == licId).First().UserCredentialId;
                    return userId;
                }
            }
        }

        public static string GetUserNameOnID(Guid UserID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                if (UserID == null || UserID == Guid.Empty)
                {
                    return null;
                }
                else
                {
                    string name = DataModel.UserCredentials.Where(s => s.UserCredentialId == UserID).First().UserName;
                    return name;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private void SaveLinkedUsers()
        {
            DLinq.UserCredential linkedUser;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.UserCredential user = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == this.UserCredentialID && s.IsDeleted == false);
                user.UserCredentials.Clear();
                foreach (LinkedUser agent in this._Linkedusers)
                {
                    if (agent.IsConnected == true)
                    {
                        linkedUser = ReferenceMaster.GetreferencedUserCredential(agent.UserId, DataModel);
                        user.UserCredentials.Add(linkedUser);
                    }
                }
                DataModel.SaveChanges();
            }
        }

        ///<summary>
        /// 

        /// </summary>
        private void SaveLinkedUsers(List<LinkedUser> LinkedUser, Guid CurrentUserID)
        {
            DLinq.UserCredential linkedUser;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.UserCredential user = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == CurrentUserID && s.IsDeleted == false);
                user.UserCredentials.Clear();
                foreach (LinkedUser agent in LinkedUser)
                {
                    if (agent.IsConnected == true)
                    {
                        linkedUser = ReferenceMaster.GetreferencedUserCredential(agent.UserId, DataModel);
                        user.UserCredentials.Add(linkedUser);
                    }
                }
                DataModel.SaveChanges();
            }
        }

        /// <summary>
        /// for sign in purpose, validate the userid and password provided in the parameter.
        /// </summary>
        /// 
        public User GetValidIdentity(string UserName, string Password)
        {
            var pp = new User();
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("Login request: user - " + UserName + ", Pwd: " + Password, true);
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("Login request init DB: user - " + UserName + ", Pwd: " + Password, true);

                    DataModel.CommandTimeout = 600000000;
                    string key = System.Configuration.ConfigurationSettings.AppSettings["AESKey"];
                    string iv = System.Configuration.ConfigurationSettings.AppSettings["AESIV"];

                    byte[] encryptPassword = AESEncrypt.EncryptStringToBytes_Aes(Password, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));

                    string encryptedPassword = Convert.ToBase64String(encryptPassword);

                    pp = (from ud in DataModel.UserCredentials
                          where (ud.UserName == UserName && (ud.Password == Password || ud.Password == encryptedPassword) && ud.IsDeleted == false && ((ud.Licensee == null) || (ud.Licensee.LicenseStatusId != 1)))
                          select new User
                          {
                              UserName = ud.UserName,
                              Password = Password,
                              PasswordHintA = ud.PasswordHintAnswer,
                              PasswordHintQ = ud.PasswordHintQuestion,
                              Role = (UserRole)ud.MasterRole.RoleId,
                              UserCredentialID = ud.UserCredentialId,
                              IsHouseAccount = ud.IsHouseAccount,
                              DisableAgentEditing = ud.UserDetail.DisableAgentEditing,
                              IsDeleted = ud.IsDeleted,
                              IsLicenseDeleted = ud.Licensee.IsDeleted,
                              LicenseeId = (Guid?)ud.Licensee.LicenseeId ?? Guid.Empty,
                              IsNewsToFlash = ud.IsNewsToFlash
                          }).FirstOrDefault();

                    if (pp == null) return null;

                    ActionLogger.Logger.WriteImportPolicyLog("Login request: User " + pp.ToStringDump(), true);

                    if (!pp.LicenseeId.IsNullOrEmpty())
                    {
                        pp.HouseOwnerId = DataModel.UserCredentials.First(s => s.LicenseeId == pp.LicenseeId && s.IsHouseAccount == true && s.IsDeleted == false).UserCredentialId;
                        pp.AdminId = DataModel.UserCredentials.First(s => s.LicenseeId == pp.LicenseeId && s.RoleId == 2 && s.IsDeleted == false).UserCredentialId;
                    }
                    else
                    {
                        pp.HouseOwnerId = Guid.Empty;
                        pp.AdminId = Guid.Empty;
                    }

                    //if ((String.Compare(pp.UserName, UserName, true) != 0) || (String.Compare(pp.Password, Password, false) != 0) || (String.Compare(pp.Password, encryptedPassword, false) != 0))
                    //    return null;
                    //ActionLogger.Logger.WriteImportPolicyLog("Login request13: User ", true);

                    //Jyotisna  - May 20, 2020 - As permissions added for web version but crash in desktop, so avoiding for DEU users 
                    pp.Permissions = (pp.Role != UserRole.DEP) ? GetCurrentPermission(pp.UserCredentialID) : null;

                    pp.WebDavPath = SystemConstant.GetKeyValue("WebDevPath");
                    ActionLogger.Logger.WriteImportPolicyLog("Login request: Permissions found ", true);

                    if (pp.LicenseeId != Guid.Empty)
                        Licensee.SetLastLoginTime(pp.LicenseeId.Value);
                    ActionLogger.Logger.WriteImportPolicyLog("Login request success ", true);
                    /*
                   // System.Threading.Tasks.Task.Factory.StartNew(() => DeleteTempFolderContents());
                    int threadId;
                    // Create an instance of the test class.
                    AsyncDemo ad = new AsyncDemo();

                    // Create the delegate.
                    AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

                    // Initiate the asychronous call.
                    IAsyncResult result = caller.BeginInvoke(3000, out threadId, null, null);

                    Thread.Sleep(0);
                    Console.WriteLine("Main thread {0} does some work.",
                        Thread.CurrentThread.ManagedThreadId);

                    // Call EndInvoke to wait for the asynchronous call to complete,
                    // and to retrieve the results.
                    string returnValue = caller.EndInvoke(out threadId, result);*/
                    //    new System.Threading.Tasks.Task(() => DeleteTempFolderContents()).Start();
                    return pp;
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog(ex.Message, true);
                if (ex.InnerException != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog(ex.InnerException.Message, true);
                }
                ActionLogger.Logger.WriteImportPolicyLog(ex.StackTrace.ToString(), true);
            }
            return pp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        /// 
        public User GetForgetValidIdentity(string UserName)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var pp = (from ud in DataModel.UserCredentials
                          where (ud.UserName.ToLower() == UserName.ToLower() && ud.IsDeleted == false)
                          select ud).FirstOrDefault();
                User user = null;
                if (pp != null)
                {
                    //Get password and decrypt if encrypted 
                    bool isOnWeb = false;
                    bool.TryParse(Convert.ToString(pp.IsUserActiveOnWeb), out isOnWeb);
                    string pwd = pp.Password;
                    ActionLogger.Logger.WriteImportPolicyLog("Password found as: " + pwd  + ", isonweb: " + isOnWeb, true);
                    if (isOnWeb)
                    {
                        try
                        {
                            string key = System.Configuration.ConfigurationSettings.AppSettings["AESKey"];
                            string iv = System.Configuration.ConfigurationSettings.AppSettings["AESIV"];

                            string decryptPassword = AESEncrypt.DecryptStringFromBytes_Aes(Convert.FromBase64String(pp.Password), Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));
                            pwd = decryptPassword;
                            ActionLogger.Logger.WriteImportPolicyLog("Password decrypted: " + pwd, true);
                        }
                        catch (Exception ex)
                        {
                            ActionLogger.Logger.WriteImportPolicyLog("Password decrypted excption: " + ex.Message, true);
                        }
                    }
                    user = new User
                    {
                        UserName = pp.UserName,
                        Password = pwd,
                        PasswordHintA = pp.PasswordHintAnswer,
                        PasswordHintQ = pp.PasswordHintQuestion,
                        Role = (UserRole)pp.MasterRole.RoleId,
                        UserCredentialID = pp.UserCredentialId,
                        IsHouseAccount = pp.IsHouseAccount,
                        IsDeleted = pp.IsDeleted,
                        IsLicenseDeleted = pp.Licensee.IsDeleted,
                        LicenseeId = (Guid?)(pp.Licensee != null ? pp.Licensee.LicenseeId : Guid.Empty)

                    };

                    user.Email = DataModel.UserDetails.Where(s => s.UserCredential.UserCredentialId == pp.UserCredentialId).Select(s => s.Email).FirstOrDefault();
                }
                return user;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// add by vinod 
        /// to get userlist by user email 
        public static List<User> GetAllUsers(string strEmailAddress)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DataModel.CommandTimeout = 600000000;

                List<User> _usr = null;
                if (!string.IsNullOrEmpty(strEmailAddress))
                {
                    strEmailAddress = strEmailAddress.ToUpper();
                }

                _usr = (from uc in DataModel.UserCredentials
                        where uc.IsDeleted == false && uc.UserDetail != null && uc.UserDetail.Email.ToUpper() == strEmailAddress
                        select new User
                        {
                            UserCredentialID = uc.UserCredentialId,
                            IsAccountExec = uc.IsAccountExec,
                            FirstName = uc.UserDetail.FirstName,
                            LastName = uc.UserDetail.LastName,
                            //City = uc.UserDetail.City,
                            //Company = uc.UserDetail.Company,
                            //State = uc.UserDetail.State,
                            NickName = uc.UserDetail.NickName,
                            Email = uc.UserDetail.Email,
                            //Address = uc.UserDetail.Address,
                            //OfficePhone = uc.UserDetail.OfficePhone,
                            //CellPhone = uc.UserDetail.CellPhone,
                            //Fax = uc.UserDetail.Fax,
                            LicenseeId = uc.Licensee.LicenseeId,
                            LicenseeName = uc.Licensee.Company,
                            UserName = uc.UserName,
                            Password = uc.Password,
                            //DisableAgentEditing = uc.UserDetail.DisableAgentEditing,
                            //PasswordHintQ = uc.PasswordHintQuestion,
                            //PasswordHintA = uc.PasswordHintAnswer,
                            IsHouseAccount = uc.IsHouseAccount,
                            //FirstYearDefault = uc.UserDetail.FirstYearDefault,
                            //RenewalDefault = uc.UserDetail.RenewalDefault,
                            Role = (UserRole)uc.MasterRole.RoleId
                            //IsNewsToFlash = uc.IsNewsToFlash
                        }
                        ).ToList();
                foreach (User usr in _usr)
                {
                    DLinq.UserDetail userDetail = DataModel.UserDetails.FirstOrDefault(s => s.UserCredentialId == usr.UserCredentialID);
                    if (userDetail.ZipCode != null)
                        usr.ZipCode = userDetail.ZipCode.Value.ToString("D5");
                    else
                        usr.ZipCode = null;

                    if (!usr.LicenseeId.IsNullOrEmpty())
                    {
                        usr.HouseOwnerId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.IsHouseAccount == true && s.IsDeleted == false).UserCredentialId;
                        usr.AdminId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.RoleId == 2 && s.IsDeleted == false).UserCredentialId;
                    }
                    else
                    {
                        usr.HouseOwnerId = Guid.Empty;
                        usr.AdminId = Guid.Empty;
                    }
                }
                return _usr;
            }
        }

        public static List<User> GetAllPayee()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<User> _usr = null;

                _usr = (from uc in DataModel.UserCredentials
                        where uc.IsDeleted == false && uc.UserDetail != null && uc.RoleId == 3
                        select new User
                        {
                            IsHouseAccount = uc.IsHouseAccount,
                            IsAccountExec = uc.IsAccountExec,
                            UserCredentialID = uc.UserCredentialId,
                            FirstName = uc.UserDetail.FirstName,
                            LastName = uc.UserDetail.LastName,
                            //Company = uc.UserDetail.Company,                           
                            NickName = uc.UserDetail.NickName,
                            Email = uc.UserDetail.Email,
                            LicenseeId = uc.Licensee.LicenseeId,
                            //LicenseeName = uc.Licensee.Company,
                            UserName = uc.UserName,
                            Password = uc.Password,
                            //DisableAgentEditing = uc.UserDetail.DisableAgentEditing,
                            //PasswordHintQ = uc.PasswordHintQuestion,
                            //PasswordHintA = uc.PasswordHintAnswer,
                            //FirstYearDefault = uc.UserDetail.FirstYearDefault,
                            //RenewalDefault = uc.UserDetail.RenewalDefault,
                            Role = (UserRole)uc.MasterRole.RoleId
                            //IsNewsToFlash = uc.IsNewsToFlash
                        }
                        ).ToList();

                return _usr;
            }
        }

        public static List<User> GetAccountExecByLicencessID(Guid licensssID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<User> _usr = null;
                try
                {
                    DataModel.CommandTimeout = 600000000;

                    _usr = (from uc in DataModel.UserCredentials
                            where uc.IsDeleted == false && uc.UserDetail != null && uc.LicenseeId == licensssID & uc.IsAccountExec == true
                            select new User
                            {
                                IsHouseAccount = uc.IsHouseAccount,
                                IsAccountExec = uc.IsAccountExec,
                                UserCredentialID = uc.UserCredentialId,
                                FirstName = uc.UserDetail.FirstName,
                                LastName = uc.UserDetail.LastName,
                                Company = uc.UserDetail.Company,
                                NickName = uc.UserDetail.NickName,
                                Email = uc.UserDetail.Email,
                                LicenseeId = uc.Licensee.LicenseeId,
                                UserName = uc.UserName,
                                Password = uc.Password,
                                PasswordHintA = uc.PasswordHintAnswer,
                                Role = (UserRole)uc.MasterRole.RoleId

                            }
                            ).ToList();


                    _usr = _usr.OrderBy(s => s.NickName).ToList();
                }
                catch
                {
                }

                return _usr;
            }
        }

        public static int GetAccountExecCount(Guid licensssID)
        {
            int intCount = 0;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.CommandTimeout = 600000000;
                    intCount = (from uc in DataModel.UserCredentials where uc.IsDeleted == false && uc.UserDetail != null && uc.LicenseeId == licensssID & uc.IsAccountExec == true select uc).Count();
                }
            }
            catch
            {
            }
            return intCount;
        }

        public static int GetAllAccountExecCount()
        {
            int intCount = 0;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.CommandTimeout = 600000000;
                    intCount = (from uc in DataModel.UserCredentials where uc.IsDeleted == false && uc.UserDetail != null & uc.IsAccountExec == true select uc).Count();
                }
            }
            catch
            {
            }
            return intCount;
        }

        public int GetPayeeCount(Guid licensssID)
        {
            int intCount = 0;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.CommandTimeout = 600000000;
                    intCount = (from uc in DataModel.UserCredentials where uc.IsDeleted == false && uc.UserDetail != null && uc.LicenseeId == licensssID select uc).Count();
                }
            }
            catch
            {
            }
            return intCount;
        }

        public int GetAllPayeeCount()
        {
            int intCount = 0;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DataModel.CommandTimeout = 600000000;
                    intCount = (from uc in DataModel.UserCredentials where uc.IsDeleted == false && uc.UserDetail != null select uc).Count();
                }
            }
            catch
            {
            }
            return intCount;
        }

        public static List<User> GetAllPayeeByLicencessID(Guid licensssID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<User> _usr = null;
                DataModel.CommandTimeout = 600000000;

                _usr = (from uc in DataModel.UserCredentials
                        where uc.IsDeleted == false && uc.UserDetail != null && uc.LicenseeId == licensssID
                        select new User
                        {
                            IsHouseAccount = uc.IsHouseAccount,
                            IsAccountExec = uc.IsAccountExec,
                            UserCredentialID = uc.UserCredentialId,
                            FirstName = uc.UserDetail.FirstName,
                            LastName = uc.UserDetail.LastName,
                            //City = uc.UserDetail.City,
                            Company = uc.UserDetail.Company,
                            //State = uc.UserDetail.State,
                            NickName = uc.UserDetail.NickName,
                            Email = uc.UserDetail.Email,
                            Address = uc.UserDetail.Address,
                            //OfficePhone = uc.UserDetail.OfficePhone,
                            //CellPhone = uc.UserDetail.CellPhone,
                            //Fax = uc.UserDetail.Fax,
                            LicenseeId = uc.Licensee.LicenseeId,
                            LicenseeName = uc.Licensee.Company,
                            UserName = uc.UserName,
                            Password = uc.Password,
                            //DisableAgentEditing = uc.UserDetail.DisableAgentEditing,
                            //PasswordHintQ = uc.PasswordHintQuestion,
                            PasswordHintA = uc.PasswordHintAnswer,
                            //FirstYearDefault = uc.UserDetail.FirstYearDefault,
                            //RenewalDefault = uc.UserDetail.RenewalDefault,
                            Role = (UserRole)uc.MasterRole.RoleId
                            //IsNewsToFlash = uc.IsNewsToFlash
                        }
                        ).ToList();

                //foreach (User usr in _usr)
                //{
                //    DLinq.UserDetail userDetail = DataModel.UserDetails.FirstOrDefault(s => s.UserCredentialId == usr.UserCredentialID);
                //    if (userDetail.ZipCode != null)
                //        usr.ZipCode = userDetail.ZipCode.Value.ToString("D5");
                //    else
                //        usr.ZipCode = null;

                //    if (!usr.LicenseeId.IsNullOrEmpty())
                //    {
                //        usr.HouseOwnerId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.IsHouseAccount == true && s.IsDeleted == false).UserCredentialId;
                //        usr.AdminId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.RoleId == 2 && s.IsDeleted == false).UserCredentialId;
                //    }
                //    else
                //    {
                //        usr.HouseOwnerId = Guid.Empty;
                //        usr.AdminId = Guid.Empty;
                //    }
                //}

                //Acme added to order promary agents list 
                if (_usr != null)
                {
                    _usr = _usr.OrderBy(x => x.NickName).ToList();
                }

                return _usr;
            }
        }

        //public static List<User> GetAllPayeeByLicencessID(Guid licensssID)
        //{
        //    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //    {
        //        List<User> _usr = null;

        //        _usr = (from uc in DataModel.UserCredentials
        //                where uc.IsDeleted == false && uc.UserDetail != null && uc.IsHouseAccount == false && uc.RoleId == 3 && uc.LicenseeId == licensssID
        //                select new User
        //                {
        //                    IsHouseAccount = uc.IsHouseAccount,
        //                    UserCredentialID = uc.UserCredentialId,
        //                    FirstName = uc.UserDetail.FirstName,
        //                    LastName = uc.UserDetail.LastName,
        //                    City = uc.UserDetail.City,
        //                    Company = uc.UserDetail.Company,
        //                    State = uc.UserDetail.State,
        //                    NickName = uc.UserDetail.NickName,
        //                    Email = uc.UserDetail.Email,
        //                    Address = uc.UserDetail.Address,
        //                    OfficePhone = uc.UserDetail.OfficePhone,
        //                    CellPhone = uc.UserDetail.CellPhone,
        //                    Fax = uc.UserDetail.Fax,
        //                    LicenseeId = uc.Licensee.LicenseeId,
        //                    LicenseeName = uc.Licensee.Company,
        //                    UserName = uc.UserName,
        //                    Password = uc.Password,
        //                    DisableAgentEditing = uc.UserDetail.DisableAgentEditing,
        //                    PasswordHintQ = uc.PasswordHintQuestion,
        //                    PasswordHintA = uc.PasswordHintAnswer,
        //                    FirstYearDefault = uc.UserDetail.FirstYearDefault,
        //                    RenewalDefault = uc.UserDetail.RenewalDefault,
        //                    Role = (UserRole)uc.MasterRole.RoleId,
        //                    IsNewsToFlash = uc.IsNewsToFlash
        //                }
        //                ).ToList();

        //        //foreach (User usr in _usr)
        //        //{
        //        //    DLinq.UserDetail userDetail = DataModel.UserDetails.FirstOrDefault(s => s.UserCredentialId == usr.UserCredentialID);
        //        //    if (userDetail.ZipCode != null)
        //        //        usr.ZipCode = userDetail.ZipCode.Value.ToString("D5");
        //        //    else
        //        //        usr.ZipCode = null;

        //        //    if (!usr.LicenseeId.IsNullOrEmpty())
        //        //    {
        //        //        usr.HouseOwnerId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.IsHouseAccount == true && s.IsDeleted == false).UserCredentialId;
        //        //        usr.AdminId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.RoleId == 2 && s.IsDeleted == false).UserCredentialId;
        //        //    }
        //        //    else
        //        //    {
        //        //        usr.HouseOwnerId = Guid.Empty;
        //        //        usr.AdminId = Guid.Empty;
        //        //    }
        //        //}
        //        return _usr;
        //    }
        //}

        public static List<User> GetAllUsers()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DataModel.CommandTimeout = 600000000;

                List<User> _usr = null;

                _usr = (from uc in DataModel.UserCredentials
                        where uc.IsDeleted == false && uc.UserDetail != null
                        select new User
                        {
                            IsHouseAccount = uc.IsHouseAccount,
                            IsAccountExec = uc.IsAccountExec,
                            UserCredentialID = uc.UserCredentialId,
                            FirstName = uc.UserDetail.FirstName,
                            LastName = uc.UserDetail.LastName,
                            //City = uc.UserDetail.City,
                            Company = uc.UserDetail.Company,
                            //State = uc.UserDetail.State,
                            NickName = uc.UserDetail.NickName,
                            Email = uc.UserDetail.Email,
                            Address = uc.UserDetail.Address,
                            //OfficePhone = uc.UserDetail.OfficePhone,
                            //CellPhone = uc.UserDetail.CellPhone,
                            Fax = uc.UserDetail.Fax,
                            LicenseeId = uc.Licensee.LicenseeId,
                            LicenseeName = uc.Licensee.Company,
                            UserName = uc.UserName,
                            Password = uc.Password,
                            //Added 04062014    
                            //DisableAgentEditing = uc.UserDetail.DisableAgentEditing,
                            PasswordHintQ = uc.PasswordHintQuestion,
                            PasswordHintA = uc.PasswordHintAnswer,
                            //FirstYearDefault = uc.UserDetail.FirstYearDefault,
                            //RenewalDefault = uc.UserDetail.RenewalDefault,
                            Role = (UserRole)uc.MasterRole.RoleId
                            //IsNewsToFlash = uc.IsNewsToFlash
                        }
                        ).ToList();

                //foreach (User usr in _usr)
                //{
                //    DLinq.UserDetail userDetail = DataModel.UserDetails.FirstOrDefault(s => s.UserCredentialId == usr.UserCredentialID);
                //    if (userDetail.ZipCode != null)
                //        usr.ZipCode = userDetail.ZipCode.Value.ToString("D5");
                //    else
                //        usr.ZipCode = null;

                //    if (!usr.LicenseeId.IsNullOrEmpty())
                //    {
                //        usr.HouseOwnerId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.IsHouseAccount == true && s.IsDeleted == false).UserCredentialId;
                //        usr.AdminId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.RoleId == 2 && s.IsDeleted == false).UserCredentialId;
                //    }
                //    else
                //    {
                //        usr.HouseOwnerId = Guid.Empty;
                //        usr.AdminId = Guid.Empty;
                //    }
                //}
                return _usr;


            }
        }

        /// <summary>
        /// overloaded function be there to support various filter criteria.
        /// GetUsers()/all/all viewable to loggedinuser/all in the licensee/all of the given search criteria/of one userrole.
        /// </summary>
        /// <returns></returns>
        /// 

        public static IEnumerable<User> GetUsers(Guid? LicenseeId, UserRole RoleIdToView)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                IEnumerable<User> userList = null;

                userList = (from uc in DataModel.UserCredentials
                            where uc.IsDeleted == false && uc.MasterRole.RoleId == (int)RoleIdToView && (uc.Licensee.IsDeleted == false) && (uc.Licensee.LicenseeId == LicenseeId)
                            select new User
                            {
                                UserCredentialID = uc.UserCredentialId,
                                IsAccountExec = uc.IsAccountExec,
                                FirstName = uc.UserDetail.FirstName,
                                LastName = uc.UserDetail.LastName,
                                City = uc.UserDetail.City,
                                Company = uc.UserDetail.Company,
                                State = uc.UserDetail.State,
                                NickName = uc.UserDetail.NickName,
                                Email = uc.UserDetail.Email,
                                Address = uc.UserDetail.Address,
                                OfficePhone = uc.UserDetail.OfficePhone,
                                CellPhone = uc.UserDetail.CellPhone,
                                Fax = uc.UserDetail.Fax,
                                LicenseeId = uc.Licensee.LicenseeId,
                                LicenseeName = uc.Licensee.Company,
                                UserName = uc.UserName,
                                Password = uc.Password,
                                DisableAgentEditing = uc.UserDetail.DisableAgentEditing,
                                PasswordHintQ = uc.PasswordHintQuestion,
                                PasswordHintA = uc.PasswordHintAnswer,
                                IsHouseAccount = uc.IsHouseAccount,
                                FirstYearDefault = uc.UserDetail.FirstYearDefault,
                                RenewalDefault = uc.UserDetail.RenewalDefault,
                                IsNewsToFlash = uc.IsNewsToFlash,
                                Role = (UserRole)uc.MasterRole.RoleId
                            }).ToList();

                //foreach (User usr in userList)
                //{
                //    DLinq.UserDetail userDetail = DataModel.UserDetails.FirstOrDefault(s => s.UserCredentialId == usr.UserCredentialID);
                //    if (userDetail.ZipCode != null)
                //        usr.ZipCode = userDetail.ZipCode.Value.ToString("D5");
                //    else
                //        usr.ZipCode = null;

                //    if (!usr.LicenseeId.IsNullOrEmpty())
                //    {
                //        usr.HouseOwnerId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.IsHouseAccount == true && s.IsDeleted == false).UserCredentialId;
                //        usr.AdminId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.RoleId == 2 && s.IsDeleted == false).UserCredentialId;
                //    }
                //    else
                //    {
                //        usr.HouseOwnerId = Guid.Empty;
                //        usr.AdminId = Guid.Empty;
                //    }
                //}

                return userList;
            }
        }

        public static List<User> GetUsers(UserRole RoleId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<User> _usrlist = null;

                _usrlist = (from s in DataModel.UserCredentials
                            where (s.RoleId == (int)RoleId) && (s.IsDeleted == false) && ((4 == (int)RoleId) || s.Licensee.IsDeleted == false)
                            select new User
                            {
                                UserCredentialID = s.UserCredentialId,
                                IsAccountExec = s.IsAccountExec,
                                UserName = s.UserName,
                                Password = s.Password,
                                FirstName = s.UserDetail.FirstName,
                                LastName = s.UserDetail.LastName,
                                Role = (UserRole)s.MasterRole.RoleId,
                                IsNewsToFlash = s.IsNewsToFlash
                            }
                        ).ToList();

                foreach (User usr in _usrlist)
                {
                    if (!usr.LicenseeId.IsNullOrEmpty())
                    {
                        usr.HouseOwnerId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.IsHouseAccount == true && s.IsDeleted == false).UserCredentialId;
                        usr.AdminId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.RoleId == 2 && s.IsDeleted == false).UserCredentialId;
                    }
                    else
                    {
                        usr.HouseOwnerId = Guid.Empty;
                        usr.AdminId = Guid.Empty;
                    }
                }
                return _usrlist;
            }
        }

        public List<User> GetHouseUsers(Guid LincessID, int intRoleID, bool IsHouseAccount)
        {

            int intHouseAccount = 0;

            if (IsHouseAccount)
            {
                intHouseAccount = 1;
            }

            List<User> lstUser = new List<User>();

            try
            {

                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                string adoConnStr = sc.ConnectionString;

                using (SqlConnection con = new SqlConnection(adoConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetUserList", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LicenseeId", LincessID);
                        cmd.Parameters.AddWithValue("@RoleId", intRoleID);
                        cmd.Parameters.AddWithValue("@IsHouseAccount", intHouseAccount);
                        con.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            try
                            {
                                User objUser = new User();

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["UserCredentialID"])))
                                    {
                                        objUser.UserCredentialID = reader["UserCredentialID"] == null ? Guid.Empty : (Guid)reader["UserCredentialID"];
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["UserName"])))
                                    {
                                        objUser.UserName = reader["UserName"] == null ? string.Empty : Convert.ToString(reader["UserName"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["Password"])))
                                    {
                                        objUser.Password = Convert.ToString(reader["Password"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["FirstName"])))
                                    {
                                        objUser.FirstName = reader["FirstName"] == null ? string.Empty : Convert.ToString(reader["FirstName"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["LastName"])))
                                    {
                                        objUser.LastName = reader["LastName"] == null ? string.Empty : Convert.ToString(reader["LastName"]);
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["RoleId"])))
                                    {
                                        if (IsHouseAccount)
                                        {
                                            objUser.HouseOwnerId = objUser.UserCredentialID;
                                            objUser.IsHouseAccount = true;
                                            objUser.AdminId = null;

                                        }
                                        else
                                        {
                                            objUser.HouseOwnerId = null;
                                            objUser.IsHouseAccount = false;
                                            objUser.AdminId = null;
                                        }
                                    }
                                }
                                catch
                                {
                                }

                                try
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(reader["IsNewsToFlash"])))
                                    {
                                        objUser.IsNewsToFlash = Convert.ToBoolean(reader["IsNewsToFlash"]);
                                    }
                                }
                                catch
                                {
                                }

                                lstUser.Add(objUser);
                            }
                            catch
                            {
                            }

                        }
                        reader.Close();
                    }
                }
            }
            catch
            {
            }

            return lstUser;

        }

        public List<LinkedUser> GetLinkedUser(Guid UserCredentialId, UserRole RoleId, bool isHouseValue)
        {
            List<LinkedUser> LnkUsers = new List<LinkedUser>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {


                    List<DLinq.UserCredential> connectedLinkedUsers = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == UserCredentialId).UserCredentials.Where(s => s.IsDeleted == false && s.UserCredentialId != UserCredentialId).ToList();

                    if (connectedLinkedUsers != null && connectedLinkedUsers.Count != 0)
                        LnkUsers = (from usr in connectedLinkedUsers
                                    select new LinkedUser
                                    {
                                        UserId = usr.UserCredentialId,
                                        LastName = usr.UserDetail.LastName,
                                        FirstName = usr.UserDetail.FirstName,
                                        NickName = usr.UserDetail.NickName,
                                        IsConnected = true,
                                        UserName = usr.UserName
                                    }).ToList();

                }

            }

            catch
            {
            }

            return LnkUsers;

        }

        public List<LinkedUser> GetAllLinkedUser(List<User> objUser, Guid GuidLicID, Guid UserCredentialId, int intRole, bool boolHouseAccount)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<LinkedUser> LnkUsers = null;

                foreach (var item in objUser)
                {
                    if (intRole == 3 && this.IsHouseAccount == boolHouseAccount)
                    {
                        List<DLinq.UserCredential> connectedLinkedUsers = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == item.UserCredentialID).UserCredentials.Where(s => s.IsDeleted == false && s.UserCredentialId != item.UserCredentialID).ToList();

                        if (connectedLinkedUsers != null && connectedLinkedUsers.Count != 0)
                            LnkUsers = (from usr in connectedLinkedUsers
                                        select new LinkedUser
                                        {
                                            UserId = usr.UserCredentialId,
                                            LastName = usr.UserDetail.LastName,
                                            FirstName = usr.UserDetail.FirstName,
                                            NickName = usr.UserDetail.NickName,
                                            IsConnected = true,
                                            UserName = usr.UserName
                                        }).ToList();

                        List<Guid> connectedUserIds = null;
                        if (LnkUsers == null)
                            connectedUserIds = new List<Guid>();
                        else
                            connectedUserIds = LnkUsers.Select(s => s.UserId).ToList();

                        List<LinkedUser> notConnectedLinkedUsers = (from usr in DataModel.UserCredentials
                                                                    where (!connectedUserIds.Contains(usr.UserCredentialId)) && usr.LicenseeId == item.LicenseeId && usr.RoleId == 3 && (usr.UserCredentialId != item.UserCredentialID) && (usr.IsDeleted == false)
                                                                    select new LinkedUser
                                                                    {
                                                                        UserId = usr.UserCredentialId,
                                                                        LastName = usr.UserDetail.LastName,
                                                                        FirstName = usr.UserDetail.FirstName,
                                                                        NickName = usr.UserDetail.NickName,
                                                                        IsConnected = false,
                                                                        UserName = usr.UserName
                                                                    }).ToList();

                        if (LnkUsers != null)
                            LnkUsers.AddRange(notConnectedLinkedUsers);
                        else if (notConnectedLinkedUsers != null)
                            LnkUsers = notConnectedLinkedUsers;
                    }
                    else
                    {
                        List<DLinq.UserCredential> allUsersExceptHO = DataModel.UserCredentials.Where(s => s.LicenseeId == item.LicenseeId && s.IsDeleted == false && s.RoleId == 3 && s.IsHouseAccount == false).ToList();
                        LnkUsers = (from usr in allUsersExceptHO
                                    select new LinkedUser
                                    {
                                        UserId = usr.UserCredentialId,
                                        LastName = usr.UserDetail.LastName,
                                        FirstName = usr.UserDetail.FirstName,
                                        NickName = usr.UserDetail.NickName,
                                        IsConnected = true,
                                        UserName = usr.UserName
                                    }).ToList();
                    }

                }

                return LnkUsers;
            }
        }

        public static IEnumerable<User> GetAllUsersByLicChunck(Guid LicenseeId, int skip, int take)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                IEnumerable<User> userList = null;

                try
                {
                    DataModel.CommandTimeout = 600000000;

                    userList = (from uc in DataModel.UserCredentials
                                where uc.IsDeleted == false && (uc.LicenseeId == LicenseeId) && uc.RoleId == 3
                                select new User
                                {
                                    UserCredentialID = uc.UserCredentialId,
                                    IsAccountExec = uc.IsAccountExec,
                                    FirstName = uc.UserDetail.FirstName,
                                    LastName = uc.UserDetail.LastName,
                                    City = uc.UserDetail.City,
                                    Company = uc.UserDetail.Company,
                                    State = uc.UserDetail.State,
                                    NickName = uc.UserDetail.NickName,
                                    Email = uc.UserDetail.Email,
                                    Address = uc.UserDetail.Address,
                                    OfficePhone = uc.UserDetail.OfficePhone,
                                    CellPhone = uc.UserDetail.CellPhone,
                                    Fax = uc.UserDetail.Fax,
                                    LicenseeId = uc.Licensee.LicenseeId,
                                    LicenseeName = uc.Licensee.Company,
                                    UserName = uc.UserName,
                                    Password = uc.Password,
                                    DisableAgentEditing = uc.UserDetail.DisableAgentEditing,
                                    PasswordHintQ = uc.PasswordHintQuestion,
                                    PasswordHintA = uc.PasswordHintAnswer,
                                    IsHouseAccount = uc.IsHouseAccount,
                                    FirstYearDefault = uc.UserDetail.FirstYearDefault,
                                    RenewalDefault = uc.UserDetail.RenewalDefault,
                                    Role = (UserRole)uc.MasterRole.RoleId,
                                    IsNewsToFlash = uc.IsNewsToFlash,
                                    BGUserId = uc.BGUserId
                                }).ToList();
                }
                catch
                {
                }

                return userList.OrderBy(d => d.NickName).Skip(skip).Take(take).ToList();
            }
        }

        public static List<User> GetAllUsersByChunck(int skip, int take)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DataModel.CommandTimeout = 600000000;

                List<User> _usr = null;

                try
                {

                    _usr = (from uc in DataModel.UserCredentials
                            where uc.IsDeleted == false && uc.UserDetail != null && uc.RoleId == 3
                            select new User
                            {
                                IsHouseAccount = uc.IsHouseAccount,
                                IsAccountExec = uc.IsAccountExec,
                                UserCredentialID = uc.UserCredentialId,
                                FirstName = uc.UserDetail.FirstName,
                                LastName = uc.UserDetail.LastName,
                                City = uc.UserDetail.City,
                                Company = uc.UserDetail.Company,
                                State = uc.UserDetail.State,
                                NickName = uc.UserDetail.NickName,
                                Email = uc.UserDetail.Email,
                                Address = uc.UserDetail.Address,
                                OfficePhone = uc.UserDetail.OfficePhone,
                                CellPhone = uc.UserDetail.CellPhone,
                                Fax = uc.UserDetail.Fax,
                                LicenseeId = uc.Licensee.LicenseeId,
                                LicenseeName = uc.Licensee.Company,
                                UserName = uc.UserName,
                                Password = uc.Password,
                                DisableAgentEditing = uc.UserDetail.DisableAgentEditing,
                                PasswordHintQ = uc.PasswordHintQuestion,
                                PasswordHintA = uc.PasswordHintAnswer,
                                FirstYearDefault = uc.UserDetail.FirstYearDefault,
                                RenewalDefault = uc.UserDetail.RenewalDefault,
                                Role = (UserRole)uc.MasterRole.RoleId,
                                IsNewsToFlash = uc.IsNewsToFlash,
                                BGUserId = uc.BGUserId
                            }
                            ).ToList();
                }
                catch
                {
                }

                return _usr.OrderBy(d => d.NickName).Skip(skip).Take(take).ToList();

            }
        }

        public static IEnumerable<User> GetUsers(Guid LicenseeId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DataModel.CommandTimeout = 600000000;

                IEnumerable<User> userList = null;
                userList = (from uc in DataModel.UserCredentials
                            where uc.IsDeleted == false && (uc.LicenseeId == LicenseeId)
                            select new User
                            {
                                UserCredentialID = uc.UserCredentialId,
                                IsAccountExec = uc.IsAccountExec,
                                FirstName = uc.UserDetail.FirstName,
                                LastName = uc.UserDetail.LastName,
                                City = uc.UserDetail.City,
                                Company = uc.UserDetail.Company,
                                State = uc.UserDetail.State,
                                NickName = uc.UserDetail.NickName,
                                Email = uc.UserDetail.Email,
                                Address = uc.UserDetail.Address,
                                OfficePhone = uc.UserDetail.OfficePhone,
                                CellPhone = uc.UserDetail.CellPhone,
                                Fax = uc.UserDetail.Fax,
                                LicenseeId = uc.Licensee.LicenseeId,
                                LicenseeName = uc.Licensee.Company,
                                UserName = uc.UserName,
                                Password = uc.Password,
                                DisableAgentEditing = uc.UserDetail.DisableAgentEditing,
                                PasswordHintQ = uc.PasswordHintQuestion,
                                PasswordHintA = uc.PasswordHintAnswer,
                                IsHouseAccount = uc.IsHouseAccount,
                                FirstYearDefault = uc.UserDetail.FirstYearDefault,
                                RenewalDefault = uc.UserDetail.RenewalDefault,
                                Role = (UserRole)uc.MasterRole.RoleId,
                                IsNewsToFlash = uc.IsNewsToFlash
                            }).ToList();
                //foreach (User usr in userList)
                //{
                //    DLinq.UserDetail userDetail = DataModel.UserDetails.FirstOrDefault(s => s.UserCredentialId == usr.UserCredentialID);
                //    if (userDetail.ZipCode != null)
                //        usr.ZipCode = userDetail.ZipCode.Value.ToString("D5");
                //    else
                //        usr.ZipCode = null;

                //    if (!usr.LicenseeId.IsNullOrEmpty())
                //    {
                //        usr.HouseOwnerId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.IsHouseAccount == true && s.IsDeleted == false).UserCredentialId;
                //        usr.AdminId = DataModel.UserCredentials.First(s => s.LicenseeId == usr.LicenseeId && s.RoleId == 2 && s.IsDeleted == false).UserCredentialId;
                //    }
                //    else
                //    {
                //        usr.HouseOwnerId = Guid.Empty;
                //        usr.AdminId = Guid.Empty;
                //    }
                //}
                return userList;
            }
        }

        public static IEnumerable<User> GetUsersForReports(Guid LicenseeId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DataModel.CommandTimeout = 600000000;

                IEnumerable<User> userList = null;
                userList = (from uc in DataModel.UserCredentials
                            where uc.IsDeleted == false && (uc.LicenseeId == LicenseeId)
                            select new User
                            {
                                UserCredentialID = uc.UserCredentialId,
                                FirstName = uc.UserDetail.FirstName,
                                LastName = uc.UserDetail.LastName,
                                NickName = uc.UserDetail.NickName,
                                LicenseeId = uc.Licensee.LicenseeId,
                                Role = (UserRole)uc.MasterRole.RoleId

                            }).ToList();

                return userList;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoggedInRoleId"></param>
        /// <param name="RoleIdToView"></param>
        /// <returns></returns>
        /// 
        public static void TurnOnNewsToFlashBit()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.UserCredential> userList = DataModel.UserCredentials.Where(uc => uc.IsDeleted == false && (uc.RoleId == 2 || uc.RoleId == 3) && uc.Licensee.IsDeleted == false).ToList();
                foreach (DLinq.UserCredential usr in userList)
                {
                    usr.IsNewsToFlash = true;
                }
                DataModel.SaveChanges();
            }
        }

        public static void TurnOffNewsToFlashBit(Guid userId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.UserCredential userCred = DataModel.UserCredentials.FirstOrDefault(s => s.UserCredentialId == userId);
                if (userCred != null)
                    userCred.IsNewsToFlash = false;

                DataModel.SaveChanges();
            }
        }

        public static bool IsUserNameExist(Guid userId, string userName)
        {
            bool userNameExist = false;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                if (DataModel.UserCredentials.Any(s => s.UserCredentialId == userId))
                    userNameExist = DataModel.UserCredentials.Any(s => s.UserName.ToUpper() == userName.ToUpper() && s.IsDeleted == false && s.UserCredentialId != userId);
                else
                    userNameExist = DataModel.UserCredentials.Any(s => s.UserName.ToUpper() == userName.ToUpper() && s.IsDeleted == false);
            }
            return userNameExist;
        }

        public void ImportHouseUsers(System.Data.DataTable TempTable, Guid LincessID)
        {
            try
            {
                string UserName = string.Empty;
                string Password = string.Empty;
                string Role = string.Empty;
                int RID = 3;
                string PasswordHintQuestion = string.Empty;
                string PasswordHintAnswer = string.Empty;
                bool IsHouseAccount = false;
                bool IsNewsToFlash = false;
                bool IsAccountExec = false;
                string FirstName = string.Empty;
                string LastName = string.Empty;
                string Company = string.Empty;
                string NickName = string.Empty;
                string Address = string.Empty;
                string ZipCode = string.Empty;
                string City = string.Empty;
                string State = string.Empty;
                string Email = string.Empty;
                string OfficePhone = string.Empty;
                string CellPhone = string.Empty;
                string Fax = string.Empty;
                string FirstYearDefault = string.Empty;
                string RenewalDefault = string.Empty;
                bool ReportForEntireAgency = false;
                bool ReportForOwnBusiness = false;
                string AddPayeeOn = string.Empty;
                bool DisableAgentEditing = false;
                string Moduletodisplay = string.Empty;
                int ModuleId = 11; //DEU
                int AccessRightId = 2;
                string AccessRight = string.Empty;

                if (TempTable != null)
                {
                    int intColIndex = TempTable.Columns.Count - 1;

                    for (int i = 0; i < TempTable.Rows.Count; i++)
                    {
                        if (intColIndex >= 0)
                        {
                            try
                            {
                                UserName = Convert.ToString(TempTable.Rows[i][0]);
                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 1)
                        {
                            try
                            {
                                Password = Convert.ToString(TempTable.Rows[i][1]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 2)
                        {
                            try
                            {
                                Role = Convert.ToString(TempTable.Rows[i][2]);
                                RID = RoleID(Role);
                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 3)
                        {
                            try
                            {
                                PasswordHintQuestion = Convert.ToString(TempTable.Rows[i][3]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 4)
                        {
                            try
                            {
                                PasswordHintAnswer = Convert.ToString(TempTable.Rows[i][4]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 5)
                        {
                            try
                            {
                                IsHouseAccount = trueOrFalse(Convert.ToString(TempTable.Rows[i][5]));
                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 6)
                        {
                            try
                            {
                                IsNewsToFlash = trueOrFalse(Convert.ToString(TempTable.Rows[i][6]));

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 7)
                        {
                            try
                            {
                                IsAccountExec = trueOrFalse(Convert.ToString(TempTable.Rows[i][7]));

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 8)
                        {
                            try
                            {
                                FirstName = Convert.ToString(TempTable.Rows[i][8]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 9)
                        {
                            try
                            {
                                LastName = Convert.ToString(TempTable.Rows[i][9]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 10)
                        {
                            try
                            {
                                Company = Convert.ToString(TempTable.Rows[i][10]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 11)
                        {
                            try
                            {
                                NickName = Convert.ToString(TempTable.Rows[i][11]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 12)
                        {
                            try
                            {
                                Address = Convert.ToString(TempTable.Rows[i][12]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 13)
                        {
                            try
                            {
                                ZipCode = Convert.ToString(TempTable.Rows[i][13]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 14)
                        {
                            try
                            {
                                City = Convert.ToString(TempTable.Rows[i][14]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 15)
                        {
                            try
                            {
                                State = Convert.ToString(TempTable.Rows[i][15]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 16)
                        {
                            try
                            {
                                Email = Convert.ToString(TempTable.Rows[i][16]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 17)
                        {
                            try
                            {
                                OfficePhone = Convert.ToString(TempTable.Rows[i][17]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 18)
                        {
                            try
                            {
                                CellPhone = Convert.ToString(TempTable.Rows[i][18]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 19)
                        {
                            try
                            {
                                Fax = Convert.ToString(TempTable.Rows[i][19]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 20)
                        {
                            try
                            {
                                FirstYearDefault = Convert.ToString(TempTable.Rows[i][20]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 21)
                        {
                            try
                            {
                                RenewalDefault = Convert.ToString(TempTable.Rows[i][21]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 22)
                        {
                            try
                            {
                                ReportForEntireAgency = trueOrFalse(Convert.ToString(TempTable.Rows[i][22]));

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 23)
                        {
                            try
                            {
                                ReportForOwnBusiness = trueOrFalse(Convert.ToString(TempTable.Rows[i][23]));

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 24)
                        {
                            try
                            {
                                AddPayeeOn = Convert.ToString(TempTable.Rows[i][24]);

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 25)
                        {
                            try
                            {
                                DisableAgentEditing = trueOrFalse(Convert.ToString(TempTable.Rows[i][25]));

                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 26)
                        {
                            try
                            {
                                Moduletodisplay = Convert.ToString(TempTable.Rows[i][26]);


                            }
                            catch
                            {
                            }
                        }

                        if (intColIndex >= 27)
                        {
                            try
                            {
                                AccessRight = Convert.ToString(TempTable.Rows[i][27]);

                            }
                            catch
                            {
                            }
                        }

                        //Check Nick name available or Not
                        bool bValue = IsNickNameExist(NickName);
                        if (!bValue)
                        {
                            AddUserCredentials(UserName, Password, RID, PasswordHintQuestion, PasswordHintAnswer, IsHouseAccount, IsNewsToFlash,
                            IsAccountExec, LincessID, FirstName, LastName, Company, NickName, Address, ZipCode, City, State, Email,
                            OfficePhone, CellPhone, Fax, FirstYearDefault, RenewalDefault, ReportForEntireAgency, ReportForOwnBusiness,
                            AddPayeeOn, DisableAgentEditing, Moduletodisplay, ModuleId, AccessRightId);
                        }
                    }

                }
            }
            catch
            {
            }

        }

        private int RoleID(string strRole)
        {
            int value = 3;//agent ID

            if (string.IsNullOrEmpty(strRole))
            {
                value = 3;
            }
            else if (strRole.ToLower() == "superadmin")
            {
                value = 1;
            }
            else if (strRole.ToLower() == "administrator")
            {
                value = 2;
            }
            else if (strRole.ToLower() == "agent")
            {
                value = 3;
            }
            else if (strRole.ToLower() == "dep")
            {
                value = 5;
            }
            return value;

        }

        private bool trueOrFalse(string strValue)
        {
            bool bvalue = false;//agent ID

            if (string.IsNullOrEmpty(strValue))
            {
                bvalue = false;
            }
            else if (strValue.ToLower() == "no")
            {
                bvalue = false;
            }
            else if (strValue.ToLower() == "yes")
            {
                bvalue = true;
            }

            return bvalue;

        }

        private bool IsNickNameExist(string strNickName)
        {
            bool bValue = false;

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    User _user = (from uc in DataModel.UserDetails

                                  where (uc.NickName.ToLower() == strNickName)
                                  select new User
                                  {
                                      UserCredentialID = uc.UserCredentialId,
                                      IsDeleted = uc.UserCredential.IsDeleted
                                  }
                             ).FirstOrDefault();

                    if (_user == null)
                    {
                        bValue = false;
                    }
                    else
                    {
                        if (_user.IsDeleted != null)
                        {
                            bValue = _user.IsDeleted;

                        }

                    }
                }
            }
            catch
            {
                bValue = false;
            }
            return bValue;
        }


        public void AddUserCredentials(string UserName, string Password, int RID, string PasswordHintQuestion, string PasswordHintAnswer, bool IsHouseAccount, bool IsNewsToFlash, bool IsAccountExec, Guid LicenseeId, string FirstName,
            string LastName, string Company, string NickName, string Address, string ZipCode, string City, string State, string Email, string OfficePhone,
            string CellPhone, string Fax, string FirstYearDefault, string RenewalDefault, bool ReportForEntireAgency, bool ReportForOwnBusiness, string AddPayeeOn,
            bool DisableAgentEditing, string Moduletodisplay, int ModuleId, int AccessRightId)
        {
            //using (TransactionScope transactionScope = new TransactionScope())
            //{
            try
            {

                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    Guid guidID = Guid.NewGuid();
                    //User credentials
                    try
                    {

                        var _dtUsers = new DLinq.UserCredential();
                        _dtUsers.UserCredentialId = guidID;
                        _dtUsers.UserName = UserName;
                        _dtUsers.Password = Password;
                        _dtUsers.PasswordHintAnswer = PasswordHintQuestion;
                        _dtUsers.PasswordHintQuestion = PasswordHintAnswer;
                        _dtUsers.RoleId = RID;
                        _dtUsers.LicenseeId = LicenseeId;
                        _dtUsers.CreatedOn = DateTime.Now;
                        DataModel.AddToUserCredentials(_dtUsers);
                    }
                    catch
                    {
                    }

                    //User details
                    try
                    {

                        var _dtUserDetail = new DLinq.UserDetail();
                        _dtUserDetail.UserCredentialId = guidID;
                        _dtUserDetail.FirstName = FirstName;
                        _dtUserDetail.LastName = LastName;
                        _dtUserDetail.Company = Company;
                        _dtUserDetail.NickName = NickName;
                        if (Address.Length > 50)
                        {
                            string strAddress = Address;
                            strAddress = strAddress.Substring(0, 49);
                            _dtUserDetail.Address = strAddress;
                        }
                        else
                        {
                            _dtUserDetail.Address = Address;
                        }
                        if (!string.IsNullOrEmpty(ZipCode))
                        {
                            _dtUserDetail.ZipCode = Convert.ToInt32(ZipCode);
                        }

                        _dtUserDetail.City = City;
                        _dtUserDetail.State = State;
                        _dtUserDetail.Email = Email;
                        _dtUserDetail.OfficePhone = OfficePhone;
                        _dtUserDetail.CellPhone = CellPhone;
                        _dtUserDetail.Fax = Fax;
                        _dtUserDetail.FirstYearDefault = Convert.ToDouble(FirstYearDefault);
                        _dtUserDetail.RenewalDefault = Convert.ToDouble(RenewalDefault);
                        _dtUserDetail.ReportForEntireAgency = ReportForEntireAgency;
                        _dtUserDetail.ReportForOwnBusiness = ReportForOwnBusiness;
                        _dtUserDetail.AddPayeeOn = DateTime.Now;
                        _dtUserDetail.DisableAgentEditing = DisableAgentEditing;
                        DataModel.AddToUserDetails(_dtUserDetail);
                    }
                    catch
                    {
                    }
                    //permission
                    try
                    {
                        var _permision = new DLinq.UserPermission();
                        _permision.UserPermissionId = Guid.NewGuid();
                        _permision.UserCredentialId = guidID;
                        _permision.ModuleId = ModuleId;
                        _permision.AccessRightId = AccessRightId;
                        DataModel.AddToUserPermissions(_permision);
                    }
                    catch
                    {
                    }

                    DataModel.SaveChanges();

                    //transactionScope.Complete();
                }
            }
            catch
            {
            }

            //}
        }

        //Acme - added to track login/logout time of the user
        public static void AddLoginLogoutTime(Guid UserID, string AppVersion, string Activity)
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddLoginLogoutTime request: UserID -  " + UserID + ", AppVersion - " + AppVersion + ", Activity - " + Activity, true);
                DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
                EntityConnection ec = (EntityConnection)ctx.Connection;
                SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
                string adoConnStr = sc.ConnectionString;

                using (SqlConnection con = new SqlConnection(adoConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_AddLoginLogoutEntry", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", UserID);
                        cmd.Parameters.AddWithValue("@AppVersion", AppVersion);
                        cmd.Parameters.AddWithValue("@Activity", Activity);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddLoginLogoutTime exception: " + ex.Message, true);
            }
        }

        #region Delete temp folder 
        long getDirSize(DirectoryInfo d)
        {
            long size = 0;
            try
            {
                size = d.EnumerateFiles().Sum(file => file.Length);
                size += d.EnumerateDirectories().Sum(dir => getDirSize(dir));
                ActionLogger.Logger.WriteImportPolicyLog("getTempDirSize  " + size.ToString(), true);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("getTempDirSize exception: " + ex.Message, true);
            }
            return size;
        }

        public void DeleteTempFolderContents()
        {
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeleteTempFolderContents started: ", true);
                string tempPath = System.IO.Path.GetTempPath();
                ActionLogger.Logger.WriteImportPolicyLog("DeleteTempFolderContents tempPath: " + tempPath, true);
                DirectoryInfo d = new DirectoryInfo(tempPath);
                long size = getDirSize(d);
                /*    long totalSize = d.EnumerateFiles().Sum(file => file.Length);
                    totalSize += d.EnumerateDirectories().Sum(dir => DirectorySize(dir, true));
                   */
                foreach (FileInfo fi in d.GetFiles())
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch (Exception ex)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("DeleteTempFolderContents exception deleting file: " + fi.FullName + ",  " + ex.Message, true);
                    }
                }
                foreach (DirectoryInfo dir in d.GetDirectories())
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        ActionLogger.Logger.WriteImportPolicyLog("DeleteTempFolderContents exception deleting subdirectory: " + dir.FullName + ",  " + ex.Message, true);
                    }
                }
                ActionLogger.Logger.WriteImportPolicyLog("DeleteTempFolderContents completed", true);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("DeleteTempFolderContents exception: " + ex.Message, true);
            }
        }

        #endregion 
    }


    public class AsyncDemo
    {
        // The method to be executed asynchronously.
        public string TestMethod(int callDuration, out int threadId)
        {
            Console.WriteLine("Test method begins.");
            Thread.Sleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;
            return String.Format("My call time was {0}.", callDuration.ToString());
        }
    }
    // The delegate must have the same signature as the method
    // it will call asynchronously.
    public delegate string AsyncMethodCaller(int callDuration, out int threadId);
}