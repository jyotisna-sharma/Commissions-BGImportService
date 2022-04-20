using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ICoverage
    {
        [OperationContract]
        ReturnStatus AddUpdateDeleteCoverage(Coverage Covrage, OperationSet operationType);

        [OperationContract]
        Coverage GetCarrierCoverage(Guid PayorId, Guid CarrierId, Guid CoverageId);

        [OperationContract]
        List<Coverage> GetCoverages(Guid LicenseeID);

        [OperationContract]
        List<Coverage> GetCarrierCoverages(Guid CarrierId);

        [OperationContract]
        List<DisplayedCoverage> GetDisplayedCarrierCoverages(Guid LicenseeId);

        [OperationContract]
        List<Coverage> GetPayorCarrierCoverages(Guid PayorId, Guid CarrierId);

        [OperationContract]
        List<CoverageNickName> GetAllNickNames(Guid PayorId, Guid CarrierId, Guid CoverageId);

        [OperationContract]
        bool IsValidCoverage(string carrierNickName, string coverageNickName, Guid payorId);

        [OperationContract]
        string GetCoverageNickName(Guid PayorId, Guid CarrierId, Guid CoverageId);

        [OperationContract]
        DisplayedCoverage GetCoverageForPolicy(Guid DisplayedCoverageID);

        [OperationContract]
        ReturnStatus DeleteNickName(Guid guidPayorID, Guid guidCarrierID, Guid guidPreviousCoverageId, string strPrviousNickName);

        [OperationContract]
        ReturnStatus DeleteProductType(Guid guidPayorID, Guid guidCarrierID, Guid guidCoverageId, string strNickNames);

        [OperationContract]
        List<DisplayedCoverage> GetPayorCoverages(Guid PayorId);
    }

    public partial class MavService : ICoverage
    {

        public ReturnStatus AddUpdateDeleteCoverage(Coverage Covrage,OperationSet operationType)
        {
            return Covrage.AddUpdateDelete(Covrage,operationType);
        }

        public Coverage GetCarrierCoverage(Guid PayorId, Guid CarrierId, Guid CoverageId)
        {
            return Coverage.GetCarrierCoverage(PayorId, CarrierId, CoverageId);
        }

        public List<Coverage> GetCoverages(Guid LicenseeID)
        {
            return Coverage.GetCoverages(LicenseeID);
        }

        public List<Coverage> GetCarrierCoverages(Guid CarrierId)
        {
            return Coverage.GetCarrierCoverages(CarrierId);
        }

        public List<DisplayedCoverage> GetPayorCoverages(Guid PayorId)
        {
            return Coverage.GetPayorCoverages(PayorId);
        }

        public List<DisplayedCoverage> GetDisplayedCarrierCoverages(Guid LicenseeId)
        {
            return Coverage.GetDisplayedCarrierCoverages(LicenseeId);
        }

        public List<Coverage> GetPayorCarrierCoverages(Guid PayorId, Guid CarrierId)
        {
            return Coverage.GetCarrierCoverages(PayorId, CarrierId);
        }

        public List<CoverageNickName> GetAllNickNames(Guid PayorId, Guid CarrierId, Guid CoverageId)
        {
            return Coverage.GetAllNickNames(PayorId, CarrierId, CoverageId);
        }

        public bool IsValidCoverage(string carrierNickName, string coverageNickName, Guid payorId)
        {
            return Coverage.IsValidCoverage(carrierNickName, coverageNickName, payorId);
        }

        public string GetCoverageNickName(Guid PayorId, Guid CarrierId, Guid CoverageId)
        {
            return Coverage.GetCoverageNickName(PayorId, CarrierId, CoverageId);
        }
        public DisplayedCoverage GetCoverageForPolicy(Guid DisplayedCoverageID)
        {
            return Coverage.GetCoverageForPolicy(DisplayedCoverageID);
        }

        public ReturnStatus DeleteNickName(Guid guidPayorID, Guid guidCarrierID, Guid guidPreviousCoverageId, string strPrviousNickName)
        {
           return Coverage.DeleteNickName(guidPayorID, guidCarrierID, guidPreviousCoverageId, strPrviousNickName);
        }

        public ReturnStatus DeleteProductType(Guid guidPayorID, Guid guidCarrierID, Guid guidCoverageId, string strNickNames)
        {
            return Coverage.DeleteProductType(guidPayorID, guidCarrierID, guidCoverageId, strNickNames);
        }
    }
}
