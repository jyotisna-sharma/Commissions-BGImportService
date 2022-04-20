using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public enum ModuleAccessRight
    {
        [EnumMember]
        Read = 1,
        [EnumMember]
        Write = 2,
        [EnumMember]
        NoAccess = 3
    }

    [DataContract]
    public enum MasterModule
    {
        [EnumMember]
        PeopleManager = 1,
        [EnumMember]
        PolicyManager = 2,
        [EnumMember]
        Settings = 3,
        [EnumMember]
        FollowUpManger = 4,
        [EnumMember]
        HelpUpdate = 5,
        [EnumMember]
        CompManager = 6,
        [EnumMember]
        ReportManager = 7,
        [EnumMember]
        Dashboard = 13
    }

    [DataContract]
    public class UserPermissions
    {
        [DataMember]
        public Guid ? UserPermissionId { get; set; }
        [DataMember]
        public Guid ? UserID { get; set; }
        [DataMember]
        public MasterModule Module { get; set; }
        [DataMember]
        public ModuleAccessRight Permission { get; set; }
    }
}
