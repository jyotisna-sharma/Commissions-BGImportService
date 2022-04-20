using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class ClientAndPayment
    {
        #region "DataMembers aka - public properties."
        [DataMember]
        public Guid ClientId { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public double TotalPayments { get; set; }
        #endregion 
        /// <summary>
        /// get the summary of client-wise total payment, received through a statement, given in the parameter.
        /// <param name="statementId"/>
        /// </summary>
        /// <param name="statementId"></param>
        /// <returns></returns>
        public static List<ClientAndPayment> GetClientsAndPayments(Guid statementId)
        {
            throw new NotImplementedException();
        }

    }
}
