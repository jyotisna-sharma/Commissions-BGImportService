using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.Collections;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IFormula
    {

        #region IEditable<Formula> Members

        [OperationContract]
        Formula GetFormula();
       
        #endregion
    }
    public partial class MavService : IFormula 
    {

        #region IFormula Members

        public Formula GetFormula()
        {
            return Formula.GetFormula();
        }

        #endregion
    }
}
