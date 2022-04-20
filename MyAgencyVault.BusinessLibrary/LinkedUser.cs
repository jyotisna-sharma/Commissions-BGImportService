using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class LinkedUser
    {
        #region "Public Properties aka - data members."
        
        [DataMember]
        public Guid  UserId { get; set; }
        [DataMember]
        public string FirstName{ get; set; }
        [DataMember]
        public string LastName{ get; set; }
        [DataMember]
        public string NickName { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public bool IsConnected { get; set; }
        [DataMember]
        public string BGUserId { get; set; }

        #endregion 

    }
}
