using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary.Base
{    
    public interface IOutgoingSchedule
    { 
        string PrimaryAgent { get; set; }        
        string NickName { get; set; }        
        double FirstYearRate { get; set; }        
        double RenewalRate { get; set; }
    }
}
