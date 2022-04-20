using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public enum UserRole
    { 
        [EnumMember]
        SuperAdmin = 1,
        [EnumMember]
        Administrator = 2,
        [EnumMember]
        Agent = 3,
        [EnumMember]
        DEP = 4,
        [EnumMember]
        HO = 5
    }
}
