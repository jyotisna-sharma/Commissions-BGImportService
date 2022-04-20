using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class ApplicationFault
    {   
        [DataMember]
        public string Error { get; set; }
    }
}
