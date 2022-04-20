using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MyAgencyVault.BusinessLibrary.Base
{
   interface IEditable<T> 
    {
        void AddUpdate();
        void Delete();
        T GetOfID();
        //bool IsValid();
    }
}
