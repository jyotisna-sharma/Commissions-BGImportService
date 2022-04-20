using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
  

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface  IClientAndPayment
    {  
            /// <summary>
        /// get the summary of client-wise total payment, received through a statement, given in the parameter.
        /// <param name="statementId"/>
        /// </summary>
        /// <param name="statementId"></param>
        /// <returns></returns>
        [OperationContract]
         List<ClientAndPayment> GetClientsAndPayments(Guid StatementId);
        
    }
    public partial class MavService : IClientAndPayment      
    {
       
        public List<ClientAndPayment> GetClientsAndPayments(Guid StatementId)
        {
            return ClientAndPayment.GetClientsAndPayments(StatementId);
        }
    }
}
