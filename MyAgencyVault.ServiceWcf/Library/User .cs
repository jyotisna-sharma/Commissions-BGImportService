using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IUser
    {
        [OperationContract]
        void AddUpdateUser(User Usr);

        [OperationContract]
        void AddUpdateUserPermissionAndOtherData(User Usr);

        [OperationContract]
        bool DeleteUserInfo(User Usr);

        [OperationContract(Name = "UsersWithRole")]
        List<User> GetUsers(UserRole RoleId);

        [OperationContract]
        List<User> GetHouseUsers(Guid LincessID, int intRoleID, bool IsHouseAccount);

        [OperationContract]
        List<User> GetAllUsers();

        [OperationContract]
        IEnumerable<User> GetUsersByLicensee(Guid LicenseeId);

        [OperationContract]
        IEnumerable<User> GetUsersForReports(Guid LicenseeId);


        [OperationContract(Name = "UsersWithLicenseeId")]
        IEnumerable<User> GetUsers(Guid? LicenseeId, UserRole RoleIdToView);

        [OperationContract]
        User GetValidIdentity(string UserName, string Password);

        [OperationContract]
        User GetValidIdentityUsingName(string UserName);

        [OperationContract]
        List<UserPermissions> GetCurrentPermission(Guid UserCredentialId);

        [OperationContract]
        Guid GetLicenseeUserCredentialId(Guid licId);

        [OperationContract]
        void HouseAccoutTransferProcess(User user);

        [OperationContract]
        User GetUserIdWise(Guid UserCredId);

        [OperationContract]
        string getUserEmail(Guid UserID);

        [OperationContract]
        void TurnOnNewsToFlashBit();

        [OperationContract]
        void TurnOffNewsToFlashBit(Guid userId);

        [OperationContract]
        bool IsUserNameExist(Guid userId, string userName);

        [OperationContract]
        List<LinkedUser> GetLinkedUser(Guid UserCredentialId, UserRole RoleId, bool isHouseValue);

        [OperationContract]
        List<User> GetAllPayee();

        [OperationContract]
        List<User> GetAllPayeeByLicencessID(Guid licID);

        [OperationContract]
        bool CheckAccoutExec(Guid userCredencialID);

        [OperationContract]
        void ImportHouseUsers(System.Data.DataTable TempTable, Guid LincessID);

        [OperationContract]
        List<User> GetAccountExecByLicencessID(Guid licensssID);

        [OperationContract]
        int GetPayeeCount(Guid licensssID);

        [OperationContract]
        int GetAllPayeeCount();

        [OperationContract]
        IEnumerable<User> GetAllUsersByLicChunck(Guid LicenseeId, int skip, int take);

        [OperationContract]
        IEnumerable<User> GetAllUsersByChunck(int skip, int take);

        [OperationContract]
        User GetUserWithinLicensee(Guid userCredencialID, Guid LicenseeID);

        [OperationContract]
        List<LinkedUser> GetAllLinkedUser(List<User> objUserList, Guid GuidLicID, Guid UserCredentialId, int intRole, bool boolHouseAccount);
        
        //[OperationContract]
        //List<LinkedUser> GetSeletedLinkedUser(Guid UserCredentialId, int RoleId, bool isHouseValue);
        [OperationContract]
        void UpdateAccountExec(Guid userCredencialID, bool bvalue);

        [OperationContract]
        int GetAccountExecCount(Guid licensssID);
        
        [OperationContract]
        int GetAllAccountExecCount();

        [OperationContract]
        void AddLoginLogoutTime(Guid UserID, string AppVersion, string Activity);

        [OperationContract]
        bool HasAssociatedPolicies(Guid? UserCredentialId, string NickName);

        //[OperationContract]
        //void DeleteTempFolderContents();
    }

    public partial class MavService : IUser
    {
        #region IUser Members

        public void AddLoginLogoutTime(Guid UserID, string AppVersion, string Activity)
        {
            User.AddLoginLogoutTime(UserID, AppVersion, Activity);
        }

        public List<User> GetAccountExecByLicencessID(Guid licensssID)
        {
            return User.GetAccountExecByLicencessID(licensssID);
        }

        public int GetAccountExecCount(Guid licensssID)
        {
            return User.GetAccountExecCount(licensssID);
        }

        public int GetAllAccountExecCount()
        {
            return User.GetAllAccountExecCount();
        }

        public Guid GetLicenseeUserCredentialId(Guid licId)
        {
            return User.GetLicenseeUserCredentialId(licId);
        }

        public void AddUpdateUser(User Usr)
        {
            ActionLogger.Logger.WriteImportPolicyLog("AddUpdate User request: " + Usr.ToStringDump(), true);
            Usr.AddUpdateUser();
        }

        public void AddUpdateUserPermissionAndOtherData(User Usr)
        {
            Usr.AddUpdateUserPermission();
        }

        public bool DeleteUserInfo(User Usr)
        {
            return Usr.Delete();
        }

        public List<User> GetUsers(UserRole RoleId)
        {
            return User.GetUsers(RoleId);
        }


        public List<User> GetHouseUsers(Guid LincessID, int intRoleID, bool IsHouseAccount)
        {
            User objUser = new User();
            return objUser.GetHouseUsers(LincessID, intRoleID, IsHouseAccount);
        }

        public IEnumerable<User> GetUsers(Guid? LicenseeId, UserRole RoleIdToView)
        {
            IEnumerable<User> _usr = User.GetUsers(LicenseeId, RoleIdToView);
            return _usr;
        }

        public User GetValidIdentity(string UserName, string Password)
        {
            User objUser = new User();
            return objUser.GetValidIdentity(UserName, Password);
        }

        public User GetValidIdentityUsingName(string UserName)
        {
            User objUser = new User();
            return objUser.GetForgetValidIdentity(UserName);
        }

        public List<LinkedUser> GetLinkedUser(Guid UserCredentialId, UserRole RoleId, bool isHouseValue)
        {
            User objUser = new User();
            return objUser.GetLinkedUser(UserCredentialId, RoleId, isHouseValue);
        }

        public int GetPayeeCount(Guid licensssID)
        {
            User objUser = new User();
            return objUser.GetPayeeCount(licensssID);
        }

        public int GetAllPayeeCount()
        {
            User objUser = new User();
            return objUser.GetAllPayeeCount();
        }

        public List<UserPermissions> GetCurrentPermission(Guid UserCredentialId)
        {
            return User.GetCurrentPermission(UserCredentialId);
        }

        public List<User> GetAllUsers()
        {
            return User.GetAllUsers();
        }

        public List<User> GetAllPayee()
        {
            return User.GetAllPayee();
        }

        public List<User> GetAllPayeeByLicencessID(Guid licID)
        {
            return User.GetAllPayeeByLicencessID(licID);
        }

        public IEnumerable<User> GetUsersByLicensee(Guid LicenseeId)
        {
            return User.GetUsers(LicenseeId);
        }

        public IEnumerable<User> GetUsersForReports(Guid LicenseeId)
        {
            return User.GetUsersForReports(LicenseeId);
        }

        

        public IEnumerable<User> GetAllUsersByLicChunck(Guid LicenseeId, int skip, int take)
        {
            return User.GetAllUsersByLicChunck(LicenseeId ,  skip,  take);
        }


        public IEnumerable<User> GetAllUsersByChunck(int skip, int take)
        {
            return User.GetAllUsersByChunck(skip, take);
        }

        public void HouseAccoutTransferProcess(User user)
        {
            User.HouseAccoutTransferProcess(user);
        }

        #endregion

        #region IUser Members

        public string getUserEmail(Guid UserID)
        {
            return User.getUserEmail(UserID);
        }
        public User GetUserIdWise(Guid UserCredId)
        {
            return User.GetUserIdWise(UserCredId);
        }

        #endregion


        public bool HasAssociatedPolicies(Guid? UserCredentialId, string NickName)
        {
            return User.IsAccountExecPolicyExist(UserCredentialId, NickName);
        }
        public void TurnOnNewsToFlashBit()
        {
            User.TurnOnNewsToFlashBit();
        }

        public void TurnOffNewsToFlashBit(Guid userId)
        {
            User.TurnOffNewsToFlashBit(userId);
        }

        public bool IsUserNameExist(Guid userId, string userName)
        {
            return User.IsUserNameExist(userId, userName);
        }

        public bool CheckAccoutExec(Guid userCredencialID)
        {
            User objUser = new User();
            return objUser.CheckAccoutExec(userCredencialID);
        }

        public User GetUserWithinLicensee(Guid userCredencialID, Guid LicenseeID)
        {
            User objUser = new User();
            return objUser.GetUserWithinLicensee(userCredencialID, LicenseeID);
        }

        public List<LinkedUser> GetAllLinkedUser(List<User> objUserList, Guid GuidLicID, Guid UserCredentialId, int intRole, bool boolHouseAccount)
        {
            User objUser = new User();
            return objUser.GetAllLinkedUser(objUserList, GuidLicID, UserCredentialId, intRole, boolHouseAccount);
        }

        public void ImportHouseUsers(System.Data.DataTable TempTable, Guid LincessID)
        {
            User objUser = new User();
            objUser.ImportHouseUsers(TempTable, LincessID);
        }

        public void UpdateAccountExec(Guid userCredencialID, bool bvalue)
        {
            User objUser = new User();
            objUser.UpdateAccountExec(userCredencialID, bvalue);
        }

        ////public void DeleteTempFolderContents()
        ////{
        ////      User objUser = new User();
        ////      objUser.DeleteTempFolderContents();
        ////}
    }
}
