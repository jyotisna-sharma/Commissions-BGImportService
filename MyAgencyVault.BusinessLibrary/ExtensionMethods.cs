using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.BusinessLibrary
{
    public static class ExtensionMethods
    {
        public static long? CustomParseToLong(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
                return long.Parse(value);
        }

        public static bool IsNullOrEmpty(this Guid? value)
        {
            if (value == null || value == Guid.Empty)
                return true;
            else
                return false;
        }
    }
}
