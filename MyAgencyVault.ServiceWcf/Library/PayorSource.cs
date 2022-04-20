using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPayorSource
    {
        [OperationContract]
        void AddUpdatePayorSource(PayorSource source);
        [OperationContract]
        PayorSource GetPayorSource(PayorSource source);

        [OperationContract]
        void AddPayorConfigSource(PayorSource source);

        [OperationContract]
        void UpdateFollowUpDateAndserviceStatus(string Name, string Value);

    }

    public partial class MavService : IPayorSource
    {
        public void AddUpdatePayorSource(PayorSource source)
        {
            source.AddUpdate();
        }

        public PayorSource GetPayorSource(PayorSource source)
        {
            return source.GetPayorSource();
        }

        public void AddPayorConfigSource(PayorSource source)
        {
            source.AddUpdateConfigNotes();
        }

        public void UpdateFollowUpDateAndserviceStatus(string Name, string Value)
        {
            PayorSource.UpdateFollowUpDateAndserviceStatus(Name, Value);
        }
    }
}