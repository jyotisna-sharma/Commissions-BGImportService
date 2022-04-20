using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAgencyVault.BusinessLibrary.Base
{
    public interface IFile
    {
        bool DeleteFile();
        void ViewFile();
    }
}
