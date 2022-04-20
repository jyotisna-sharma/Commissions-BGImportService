using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class UserDetail
    {
        #region "Data Member / public properties"  
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string NickName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string OfficePhone { get; set; }
        [DataMember]
        public string CellPhone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public double FirstYearDefault { get; set; }
        [DataMember]
        public double RenewalDefault { get; set; }
        [DataMember]
        public bool ReportForEntireAgency { get; set; }
        [DataMember]
        public bool ReportForOwnBusiness { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        #endregion 
    }
}
