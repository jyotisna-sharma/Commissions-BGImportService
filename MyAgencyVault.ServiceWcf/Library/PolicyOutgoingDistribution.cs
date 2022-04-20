using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;

namespace MyAgencyVault.WcfService
{
     [ServiceContract] 
    interface IPolicyOutgoingDistribution
    {
         [OperationContract]
         
         bool IsEntryMarkPaid(Guid PaymentEntryId);
        [OperationContract]

        bool IfPaymentHasPaidEntry(Guid PaymentEntryId);
        [OperationContract]
         
         List<PolicyOutgoingDistribution> GetOutgoingPaymentByPoicyPaymentEntryId(Guid EntryId);
         [OperationContract]
         
         void DeleteByPolicyIncomingPaymentId(Guid PaymentEntryId);
         [OperationContract]
         
         void AddUpdateOutgoingPaymentEntry(PolicyOutgoingDistribution _PolicyOutgoingDistribution);
         [OperationContract]
         
         void AddUpdateOutGoingPaymentEntries(List<PolicyOutgoingDistribution> _PolicyOutgoingDistribution);
         [OperationContract]
        
         void DeleteOutGoingPaymentViaOutgoingPaymentId(Guid OutgoingPaymentid);
    }
     public partial class MavService : IPolicyOutgoingDistribution
     {
        #region IPolicyOutgoingDistribution Members


        public bool IfPaymentHasPaidEntry(Guid PaymentEntryId)
        {
            return PolicyOutgoingDistribution.IfPaymentHasPaidEntry(PaymentEntryId);
        }
        public bool IsEntryMarkPaid(Guid PaymentEntryId)
         {
             return PolicyOutgoingDistribution.IsEntryMarkPaid(PaymentEntryId);
         }

         #endregion

         #region IPolicyOutgoingDistribution Members

         
         public List<PolicyOutgoingDistribution> GetOutgoingPaymentByPoicyPaymentEntryId(Guid EntryId)
         {
            return  PolicyOutgoingDistribution.GetOutgoingPaymentByPoicyPaymentEntryId(EntryId);
         }

         #endregion

         #region IPolicyOutgoingDistribution Members

         
         public void DeleteByPolicyIncomingPaymentId(Guid PaymentEntryId)
         {
             PolicyOutgoingDistribution.DeleteByPolicyIncomingPaymentId(PaymentEntryId);
         }

         #endregion

         #region IPolicyOutgoingDistribution Members

         
         public void AddUpdateOutgoingPaymentEntry(PolicyOutgoingDistribution _PolicyOutgoingDistribution)
         {
              PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutgoingDistribution);
         }

         #endregion

         #region IPolicyOutgoingDistribution Members

         
         public void AddUpdateOutGoingPaymentEntries(List<PolicyOutgoingDistribution> _PolicyOutgoingDistribution)
         {
             foreach (PolicyOutgoingDistribution _PolicyOutg in _PolicyOutgoingDistribution)
             {
                 PolicyOutgoingDistribution.AddUpdateOutgoingPaymentEntry(_PolicyOutg);
             }
         }

         #endregion

         #region IPolicyOutgoingDistribution Members
         
         public void DeleteOutGoingPaymentViaOutgoingPaymentId(Guid OutgoingPaymentid)
         {
             //Acme added the following to track direct deletions 
               try
               {
                   string body = PolicyOutgoingDistribution.GetOutgoingDeleteEmailContent(OutgoingPaymentid);
                   PolicyOutgoingDistribution.DeleteById(OutgoingPaymentid);
                   if (!string.IsNullOrEmpty(body))
                   {
                       try
                       {
                           if (System.ServiceModel.Web.WebOperationContext.Current != null && System.ServiceModel.Web.WebOperationContext.Current.IncomingRequest.Headers != null && System.ServiceModel.Web.WebOperationContext.Current.IncomingRequest.Headers.Count > 0)
                           {
                               Guid userId = new Guid(System.ServiceModel.Web.WebOperationContext.Current.IncomingRequest.Headers["Username"]);
                               string user = User.GetUserNameOnID(userId);
                               if (!string.IsNullOrEmpty(user))
                               {
                                   MailServerDetail.sendMail("deudev@acmeminds.com", "Commissions Alert: Outgoing payment manually deleted by " + user + " from the system", body);
                               }
                               else
                               {
                                   MailServerDetail.sendMail("deudev@acmeminds.com", "Commissions Alert: Outgoing payment manually deleted from the system", body);
                               }
                           }
                         
                       }
                       catch (Exception ex)
                       {
                           MailServerDetail.sendMail("deudev@acmeminds.com", "Commissions Alert: Outgoing payment manually deleted from the system", body);
                       }
                   }
               }
               catch (Exception ex)
               {
                   
               }
         }

         #endregion
     }
}
