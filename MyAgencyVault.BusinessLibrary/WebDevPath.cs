using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAgencyVault.BusinessLibrary
{
   public class WebDevPath
    {
        public string URL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DomainName { get; set; }
        public static WebDevPath GetWebDevPath(string KeyValue)
        {
            string[] Keys=KeyValue.Split(';');
            if (Keys.Count() == 4)
                return new WebDevPath { URL = Keys[0], UserName = Keys[1], Password = Keys[2], DomainName = Keys[3] };
            else
                return new WebDevPath { URL = Keys[0], UserName = Keys[1], Password = Keys[2], DomainName = null };
        }

    }
}
