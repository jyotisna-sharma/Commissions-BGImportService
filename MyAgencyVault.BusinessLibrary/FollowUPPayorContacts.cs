using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
   public class FollowUPPayorContacts
    {
        #region "Datamembers aka public properties."

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string ConatcPerf { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Fax { get; set; }

        [DataMember]
        public int? Priority { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public long? zip { get; set; }

        #endregion
    }
}
