using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using MyAgencyVault.BusinessLibrary;
using System.Collections;
using System.Data;
using System.ServiceModel;


namespace MyAgencyVault.WcfService
{
      
    [ServiceContract]
    interface IPolicySearched
    {
        //[OperationContract]
        //List<PolicySearched> GetSearchedPolicies(Guid ClientID, Guid PayorID,string searchText);
        [OperationContract]
        List<PolicySearched> GetAllSearchedPolicies(string strClient, string strInsured, string policynumber, string carrier, string payor, Guid UserCridenID, UserRole Role, Guid LicenseID);
    }
    public partial class MavService : IPolicySearched
    {


        //public List<PolicySearched> GetSearchedPolicies(Guid ClientID, Guid PayorID, string searchText)
        //{
        //    return PolicySearched.GetSearchedPolicies(ClientID, PayorID,searchText);
        //}
        public List<PolicySearched> GetAllSearchedPolicies(string strClient,string strInsured, string policynumber, string carrier, string payor, Guid UserCridenID,UserRole Role,Guid LicenseID)
        {
           // return PolicySearched.GetAllSearchedPolicies(policynumber, carrier, payor, UserCridenID);
            PolicySearched objPolicySerched = new PolicySearched();
            return objPolicySerched.GetLinkedPolicies(strClient, strInsured, policynumber, carrier, payor, UserCridenID, Role, LicenseID);
        }

    }
}