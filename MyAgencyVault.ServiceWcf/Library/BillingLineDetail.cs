using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface  IBillingLineDetail
    {
        #region IEditable<BillingLineDetail> Members
        [OperationContract]
        void AddUpdateBillingLineDetail(BillingLineDetail BillLinDtl);
        [OperationContract]
        void DeleteBillingLineDetail(BillingLineDetail BillLinDtl);
        [OperationContract]
        List<BillingLineDetail> GetBillingLineDetail();
        [OperationContract]
        bool IsValidBillingLineDetail(BillingLineDetail BillLinDtl);
        #endregion   
   
        #region ServiceProduct
          [OperationContract]
        List<ServiceProduct> GetAllServiceProduct();
          [OperationContract]
          void AddServiceProduct(ServiceProduct _serviceproduct);
        #endregion

        #region ServiceChargeType
          [OperationContract]
          List<ServiceChargeType> GetAllServiceChargeType();
          [OperationContract]
        void AddServiceChargeType(ServiceChargeType _servicechargetype);
        #endregion
        [OperationContract]
        List<ServiceProduct> GetAllServices();
        [OperationContract]
        List<ServiceChargeType> GetAllServiceCharge();
        [OperationContract]
        void Add(List<BillingLineDetail> collection, Guid LicenseeId);
        [OperationContract]
        bool IsAgencyVersionLicense(Guid LicenseeId);
        [OperationContract]
        bool IsFollowUpLicensee(Guid LicenseeId);
    }
    public partial class MavService : IBillingLineDetail
    {

        public void AddUpdateBillingLineDetail(BillingLineDetail BillLinDtl)
        {
            BillLinDtl.AddUpdate();
        }

        public void DeleteBillingLineDetail(BillingLineDetail BillLinDtl)
        {
            BillLinDtl.Delete();
        }

        public List<BillingLineDetail> GetBillingLineDetail()
        {
          return  BillingLineDetail.GetAllServiceLine();
        }

        public List<ServiceProduct> GetAllServices()
        {
            return BillingLineDetail.GetAllProducts();
        }

        public List<ServiceChargeType> GetAllServiceCharge()
        {
            return BillingLineDetail.GetAllProductCharge();
        }

        public bool IsValidBillingLineDetail(BillingLineDetail BillLinDtl)
        {
            return  BillLinDtl.IsValid() ;
        }

        #region ServiceProduct
        public List<ServiceProduct> GetAllServiceProduct()
        {
           return ServiceProduct.GetAllServiceProduct();
        }
        public void AddServiceProduct(ServiceProduct _serviceproduct)
        {
            _serviceproduct.AddUpdate();
        }
        #endregion

        #region ServiceChargeType

        public List<ServiceChargeType> GetAllServiceChargeType()
        {
            return ServiceChargeType.GetAllServiceChargeType();
        }

        public void AddServiceChargeType(ServiceChargeType _servicechargetype)
        {
            _servicechargetype.AddUpdate();
        }
        #endregion

        public void Add(List<BillingLineDetail> collection, Guid LicenseeId)
        {
            BillingLineDetail.Add(collection,LicenseeId);
        }

        #region IBillingLineDetail Members


        public bool IsAgencyVersionLicense(Guid LicenseeId)
        {
            return BillingLineDetail.IsAgencyVersionLicense(LicenseeId);
        }

        #endregion

        #region IBillingLineDetail Members


        public bool IsFollowUpLicensee(Guid LicenseeId)
        {
           return BillingLineDetail.IsFollowUpLicensee(LicenseeId);
        }

        #endregion
    }


}
