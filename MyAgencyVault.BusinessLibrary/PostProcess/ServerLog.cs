using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyAgencyVault.BusinessLibrary
{
    public class ServerLog
    {
        public static StreamWriter Write(Guid EntryId, bool isStart, StreamWriter fs, string message,string classname, string function, string UserId)
        {
            
            string starttext=isStart?"[Start Date] - ":"[End Date] - ";
            string startdate=starttext+DateTime.Today;
            fs.WriteLine(startdate);
            fs.WriteLine("Function Name : "+function);
            fs.WriteLine("Class Name : " + classname);
            fs.WriteLine("UserID : " + UserId);
            fs.WriteLine("Entry Id : " + EntryId);
            fs.WriteLine("--------------");
            
            return fs;

        }
    }
}
