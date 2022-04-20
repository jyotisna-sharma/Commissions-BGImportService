using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ICarrier
    {
        [OperationContract]
        ReturnStatus AddUpdateDeleteCarrier(Carrier Carr, OperationSet operationType);

        [OperationContract]
        List<Carrier> GetCarriersOnly(Guid LicenseeId);

        [OperationContract]
        List<Carrier> GetCarriersWith(Guid LicenseeId, bool isCoveragesRequired);

        [OperationContract]
        List<DisplayedCarrier> GetDispalyedCarriersWith(Guid LicenseeId, bool isCoveragesRequired);

        [OperationContract]
        List<Carrier> GetPayorCarriersOnly(Guid PayorId);

        [OperationContract]
        List<Carrier> GetPayorCarriersWith(Guid PayorId, bool isCoveragesRequired);

        [OperationContract]
        List<Guid> PayorCarrierGlobal(List<Guid> PayorList);

        [OperationContract]
        Carrier GetPayorCarrier(Guid PayorId, Guid CarrierId);

        [OperationContract]
        List<Carrier> GetPayorCarriers(Guid PayorId);

        [OperationContract]
        bool IsValidCarrier(string carrierNickName, Guid payorId);
    }
    public partial class MavService : ICarrier
    {
        public ReturnStatus AddUpdateDeleteCarrier(Carrier Carr, OperationSet operationType)
        {
            return Carr.AddUpdateDelete(operationType);
        }

        public List<Carrier> GetCarriersOnly(Guid LicenseeId)
        {
            return Carrier.GetCarriers(LicenseeId);
        }

        public List<DisplayedCarrier> GetDispalyedCarriersWith(Guid LicenseeId, bool isCoveragesRequired)
        {
            return Carrier.GetDispalyedCarriers(LicenseeId, isCoveragesRequired);
        }

        public List<Carrier> GetCarriersWith(Guid LicenseeId, bool isCoveragesRequired)
        {
            return Carrier.GetCarriers(LicenseeId, isCoveragesRequired);
        }

        public List<Carrier> GetPayorCarriersOnly(Guid PayorId)
        {
            return Carrier.GetPayorCarriers(PayorId);
        }

        public List<Carrier> GetPayorCarriersWith(Guid PayorId, bool isCoveragesRequired)
        {
            return Carrier.GetPayorCarriers(PayorId);
        }
        public List<Guid> PayorCarrierGlobal(List<Guid> PayorList)
        {
            return Carrier.PayorCarrierGlobal(PayorList);
        }
        public Carrier GetPayorCarrier(Guid PayorId, Guid CarrierId)
        {
            return Carrier.GetPayorCarrier(PayorId, CarrierId);
        }

        public List<Carrier> GetPayorCarriers(Guid PayorId)
        {
            return Carrier.GetPayorCarriers(PayorId);
        }

        public bool IsValidCarrier(string carrierNickName, Guid payorId)
        {
            return Carrier.IsValidCarrier(carrierNickName, payorId);
        }
    }
}
