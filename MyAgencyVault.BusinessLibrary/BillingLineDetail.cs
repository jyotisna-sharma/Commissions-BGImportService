using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using DataAccessLayer.LinqtoEntity;
using System.Linq.Expressions;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class BillingLineDetail : IEditable<BillingLineDetail>
    {
        #region
        /// <summary>
        /// Service
        /// </summary>
        [DataMember]
        public Guid LicenseeServiceID { get; set; }
        [DataMember]
        public Guid LicenseeID { get; set; }
        [DataMember]
        public ServiceProduct Service { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public ServiceChargeType ServiceChargeType { get; set; }
        [DataMember]
        public double? Min { get; set; }
        [DataMember]
        public int? Range1 { get; set; }
        [DataMember]
        public int? Range2 { get; set; }
        [DataMember]
        public double? Rate { get; set; }
        [DataMember]
        public double? Discount { get; set; }
        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public bool IsTaxable { get; set; }
        [DataMember]
        public DateTime? ModifiedOn { get; set; }
        [DataMember]
        public decimal? EstimatedCharge { get; set; }

        #endregion
        public static List<BillingLineDetail> GetAllServiceLine()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<BillingLineDetail> BDLst = (from ser in DataModel.ServiceLines
                                                 select new BillingLineDetail
                                                 {
                                                     LicenseeServiceID = ser.ServiceLineId,
                                                     LicenseeID = ser.Licensee.LicenseeId,
                                                     Service = new ServiceProduct { ServiceName = ser.MasterService.ServiceName, ServiceID = ser.MasterService.ServiceId },
                                                     ServiceChargeType = new ServiceChargeType { ServiceChargeName = ser.MasterServiceChargeType.Name, ServiceChargeTypeID = ser.MasterServiceChargeType.SCTypeId },
                                                     Min = ser.Min,
                                                     Range1 = ser.Range1,
                                                     Range2 = ser.Range2,
                                                     Rate = ser.Rate,
                                                     Discount = ser.Discount,
                                                     StartDate = ser.StartDate,
                                                     EndDate = ser.EndDate,
                                                     IsTaxable = ser.IsTaxable,
                                                     ModifiedOn = ser.ModifiedOn
                                                 }).ToList();
                return BDLst;
            }
        }
        public static List<BillingLineDetail> GetAllServiceLine(Expression<Func<DLinq.ServiceLine, bool>> parameters)
        {
          using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
          {
            List<BillingLineDetail> BDLst = (from ser in DataModel.ServiceLines
                                             .Where(parameters)
                                             select new BillingLineDetail
                                             {
                                               LicenseeServiceID = ser.ServiceLineId,
                                               LicenseeID = ser.Licensee.LicenseeId,
                                               Service = new ServiceProduct { ServiceName = ser.MasterService.ServiceName, ServiceID = ser.MasterService.ServiceId },
                                               ServiceChargeType = new ServiceChargeType { ServiceChargeName = ser.MasterServiceChargeType.Name, ServiceChargeTypeID = ser.MasterServiceChargeType.SCTypeId },
                                               Min = ser.Min,
                                               Range1 = ser.Range1,
                                               Range2 = ser.Range2,
                                               Rate = ser.Rate,
                                               Discount = ser.Discount,
                                               StartDate = ser.StartDate,
                                               EndDate = ser.EndDate,
                                               IsTaxable = ser.IsTaxable,
                                               ModifiedOn = ser.ModifiedOn
                                             }).ToList();
            return BDLst;
          }
        }
        /// <summary>
        /// 
        /// </summary>
        public static List<ServiceProduct> GetAllProducts()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var products = (from se in DataModel.MasterServices
                                select new ServiceProduct
                                {
                                    ServiceID = se.ServiceId,
                                    ServiceName = se.ServiceName,
                                    ServiceDescription = ""
                                }).ToList();
                return products;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<ServiceChargeType> GetAllProductCharge()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var serviceCharges = (from se in DataModel.MasterServiceChargeTypes
                                      select new ServiceChargeType
                                      {
                                          ServiceChargeTypeID = se.SCTypeId,
                                          ServiceChargeName = se.Name,
                                          ServiceChargeDescription = ""
                                      });
                return serviceCharges.ToList();
            }


        }

        #region IEditable<BillingLineDetail> Members

        public void AddUpdate()
        {
 
        }

        public static void Add(List<BillingLineDetail> collection, Guid LicenseeId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var serviceNameList = (from m in DataModel.MasterServices select m.ServiceName).ToList();
                var serviceLineList = (from m in DataModel.ServiceLines where m.Licensee.LicenseeId == LicenseeId select m.ServiceLineId).ToList();
                var billingLineList = (from m in collection where m.LicenseeID == LicenseeId select m.LicenseeServiceID).ToList();

                foreach (Guid id in serviceLineList)
                {
                    if (!billingLineList.Contains(id))
                    {
                        ServiceLine line = (from m in DataModel.ServiceLines where m.ServiceLineId == id select m).First();
                        if (line != null)
                            DataModel.DeleteObject(line);
                    }
                }
                DataModel.SaveChanges();

                foreach (BillingLineDetail serLine in collection)
                {
                    if (!serviceNameList.Contains(serLine.ServiceName))
                    {
                        //Add service to the service table....
                        var serviceIdList = (from m in DataModel.MasterServices select m.ServiceId).ToList();

                        int maxVal = 0;
                        if(serviceIdList != null && serviceIdList.Count != 0)
                            maxVal = serviceIdList.Max();
                        else
                            maxVal = 1;

                        DLinq.MasterService service = new DLinq.MasterService { ServiceName = serLine.ServiceName, ServiceId = maxVal + 1 };
                        DataModel.AddToMasterServices(service);
                        DataModel.SaveChanges();

                        serLine.Service = new ServiceProduct { ServiceName = service.ServiceName, ServiceID = service.ServiceId };
                        serviceNameList.Add(service.ServiceName);
                        DataModel.SaveChanges();

                    }

                    var serviceLineIds = (from m in DataModel.ServiceLines where m.Licensee.LicenseeId == LicenseeId select m.ServiceLineId).ToList();
                    if (!serviceLineIds.Contains(serLine.LicenseeServiceID))
                    {
                        //Add case
                        DLinq.ServiceLine servLine = new DLinq.ServiceLine
                        {
                            ServiceLineId = Guid.NewGuid(),
                            Range1 = serLine.Range1,
                            Range2 = serLine.Range2,
                            Rate = serLine.Rate,
                            StartDate = serLine.StartDate,
                            EndDate = serLine.EndDate,
                            IsTaxable = serLine.IsTaxable,
                            ModifiedOn = serLine.ModifiedOn,
                            Min = serLine.Min,
                            Discount = serLine.Discount,
                        };


                        DLinq.Licensee _license = ReferenceMaster.GetReferencedLicensee(serLine.LicenseeID, DataModel);
                        servLine.Licensee = _license;

                        if (serLine.Service != null)
                        {
                          DLinq.MasterService _service = ReferenceMaster.GetReferencedServiceName(serLine.Service.ServiceID, DataModel);
                          servLine.MasterService = _service;
                        }
                        DLinq.MasterServiceChargeType _serviceChargeType = ReferenceMaster.GetReferencedServiceChargeType(serLine.ServiceChargeType.ServiceChargeTypeID, DataModel);
                        servLine.MasterServiceChargeType = _serviceChargeType;

                        DataModel.AddToServiceLines(servLine);
                        DataModel.SaveChanges();
                    }
                    else
                    {
                        //Update case
                        DLinq.ServiceLine servLine = (from m in DataModel.ServiceLines where m.ServiceLineId == serLine.LicenseeServiceID select m).First();

                        servLine.Range1 = serLine.Range1;
                        servLine.Range2 = serLine.Range2;
                        servLine.Rate = serLine.Rate;
                        servLine.StartDate = serLine.StartDate;
                        servLine.EndDate = serLine.EndDate;
                        servLine.IsTaxable = serLine.IsTaxable;
                        servLine.ModifiedOn = serLine.ModifiedOn;
                        servLine.Min = serLine.Min;
                        servLine.Discount = serLine.Discount;

                        DLinq.Licensee _license = ReferenceMaster.GetReferencedLicensee(serLine.LicenseeID, DataModel);
                        servLine.Licensee = _license;

                        DLinq.MasterService _service = ReferenceMaster.GetReferencedServiceName(serLine.Service.ServiceID, DataModel);
                        servLine.MasterService = _service;

                        DLinq.MasterServiceChargeType _serviceChargeType = ReferenceMaster.GetReferencedServiceChargeType(serLine.ServiceChargeType.ServiceChargeTypeID, DataModel);
                        servLine.MasterServiceChargeType = _serviceChargeType;
                        DataModel.SaveChanges();
                    }

                }
            }
        }

        public void Delete()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.ServiceLine serLine = (from e in DataModel.ServiceLines
                                                          where e.ServiceLineId == this.LicenseeServiceID
                                                          select e).FirstOrDefault();
                if (serLine != null)
                {
                    DataModel.DeleteObject(serLine);
                    DataModel.SaveChanges();
                }
            }
        }

        public BillingLineDetail GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region " Data Members aka - public properties"
        //properties to detail about all billing line items and other info.

        #endregion

        public static bool IsAgencyVersionLicense(Guid LicenseeId)
        {
            
            bool flag = false;
            DateTime todate = DateTime.Today;
            BillingLineDetail _billinglinedetail = BillingLineDetail.GetAllServiceLine().Where(x => x.LicenseeID == LicenseeId && x.Service.ServiceID == 1).FirstOrDefault();
            if (_billinglinedetail == null)
                return flag;
            DateTime _todatydate = DateTime.Today;

            if (_billinglinedetail.StartDate <= todate && todate <= _billinglinedetail.EndDate)
            {
                flag = true;
            }
            return flag;
        }

        public static bool IsFollowUpLicensee(Guid LicenseeId)
        {
            if (LicenseeId == null || LicenseeId == Guid.Empty) return false;
            bool flag = false;
            DateTime todate = DateTime.Today;
            Expression<Func<DLinq.ServiceLine,bool>> licenseeparam = p=>p.LicenseeId==LicenseeId;
            Expression<Func<DLinq.ServiceLine, bool>> ServiceParam = p => p.ServiceId == 2;
            licenseeparam = licenseeparam.And(ServiceParam);
            BillingLineDetail _billinglinedetail = BillingLineDetail.GetAllServiceLine(licenseeparam).FirstOrDefault();//.Where(x => x.LicenseeID == LicenseeId && x.Service.ServiceID == 2).FirstOrDefault();
            if (_billinglinedetail == null)
                return flag;
            DateTime _todatydate = DateTime.Today;

            if (_billinglinedetail.StartDate <= todate && todate <= _billinglinedetail.EndDate)
            {
                flag = true;
            }
            return flag;
        }
    }

    [DataContract]
    public class InvoiceLine
    {
        [DataMember]
        public Guid LicenseeID { get; set; }
        [DataMember]
        public long InvoiceID { get; set; }
        [DataMember]
        public List<InvoiceLineServiceData> InvoiceServiceLineData { get; set; }
    }

    [DataContract]
    public class InvoiceLineServiceData
    {
        [DataMember]
        public Guid InvoiceLineID { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string ServiceChargeType { get; set; }
        [DataMember]
        public int ConsumedUnit { get; set; }
        [DataMember]
        public double? Rate { get; set; }
        [DataMember]
        public decimal? Discount { get; set; }
        [DataMember]
        public decimal SubTotal { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public List<InvoiceLinePolicyData> InvoicePolicyLineData { get; set; }
    }

    [DataContract]
    public class InvoiceLinePolicyData
    {
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public int ConsumedUnit { get; set; }
    }

    public class InvoiceLineHelper
    {
        public InvoiceLine getInvoiceLines(long invoiceId,bool includePolicyData)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.InvoiceBillingLineDetail> invoiceLines = (from m in DataModel.InvoiceBillingLineDetails
                                                  where m.Invoice.InvoiceId == invoiceId
                                                  select m).ToList();


                Guid licenseeId = (from m in DataModel.Invoices
                                   where m.InvoiceId == invoiceId
                                   select m.Licensee.LicenseeId).First();

                InvoiceLine invoiceLine = new InvoiceLine();
                invoiceLine.InvoiceID = invoiceId;
                invoiceLine.LicenseeID = licenseeId;


                invoiceLine.InvoiceServiceLineData = (from m in invoiceLines
                                                      select new InvoiceLineServiceData
                                                      {
                                                         ServiceName = m.MasterService.ServiceName,
                                                         ServiceChargeType = m.MasterServiceChargeType.Name,
                                                         ConsumedUnit = m.ConsumedUnit,
                                                         Rate = m.Rate,
                                                         Discount = m.Discount,
                                                         SubTotal = m.ChargedAmount - m.Tax,
                                                         Tax = m.Tax,
                                                         Total = m.ChargedAmount                                                    }
                                                      ).ToList();

                if (includePolicyData && invoiceLine.InvoiceServiceLineData != null)
                {
                    foreach (InvoiceLineServiceData serviceDate in invoiceLine.InvoiceServiceLineData)
                    {
                        List<DLinq.PolicyLevelBillingDetail> policyLevelInvoiceDetail = (from m in DataModel.PolicyLevelBillingDetails
                                                             where m.BillingLineId == serviceDate.InvoiceLineID
                                                             select m).ToList();
                        serviceDate.InvoicePolicyLineData = (from m in policyLevelInvoiceDetail select new InvoiceLinePolicyData { PolicyId = m.PolicyId, ConsumedUnit = m.TotalUnits??0 }).ToList();
                    }
                }
                else
                {
                    invoiceLine.InvoiceServiceLineData.ForEach(s => s.InvoicePolicyLineData = null);
                }
                return invoiceLine;
            }
        }

        public List<InvoiceLineJournalData> getInvoiceLinesForJournal(Guid licenseeId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.InvoiceBillingLineDetail> invoiceLines = (from m in DataModel.InvoiceBillingLineDetails
                                                                     where m.Invoice.Licensee.LicenseeId == licenseeId
                                                                     select m).ToList();
                List<InvoiceLineJournalData> detailJournalData = new List<InvoiceLineJournalData>();
                foreach(DLinq.InvoiceBillingLineDetail billingLine in invoiceLines)
                {
                    if (billingLine.PolicyLevelBillingDetails.Count != 0)
                    {
                        foreach (DLinq.PolicyLevelBillingDetail policyData in billingLine.PolicyLevelBillingDetails)
                        {
                            InvoiceLineJournalData line = new InvoiceLineJournalData();
                            line.InvoiceID = billingLine.Invoice.InvoiceId;
                            line.PolicyID = policyData.Policy.PolicyNumber;
                            line.BillingDate = billingLine.Invoice.BillingDate.Value;
                            line.ServiceName = billingLine.MasterService.ServiceName;
                            line.ServiceChargeType = billingLine.MasterServiceChargeType.Name;
                            line.ConsumedUnit = policyData.TotalUnits.Value;
                            line.Rate = billingLine.Rate??0;

                            if (billingLine.ConsumedUnit != 0)
                            {
                                line.Discount = (decimal)((line.ConsumedUnit * 1.0) / billingLine.ConsumedUnit) * (billingLine.Discount ?? 0);
                                line.Tax = (decimal)((line.ConsumedUnit * 1.0) / billingLine.ConsumedUnit) * billingLine.Tax;
                                line.Total = (decimal)((line.ConsumedUnit * 1.0) / billingLine.ConsumedUnit) * billingLine.ChargedAmount;
                                line.SubTotal = line.Total - line.Tax;
                            }
                            else
                            {
                                line.Discount = 0;
                                line.Tax = 0;
                                line.Total = 0;
                                line.SubTotal = 0;
                            }
                            detailJournalData.Add(line);
                        }
                    }
                    else
                    {
                        InvoiceLineJournalData line = new InvoiceLineJournalData();
                        line.InvoiceID = billingLine.Invoice.InvoiceId;
                        line.PolicyID = string.Empty;
                        line.BillingDate = billingLine.Invoice.BillingDate.Value;
                        line.ServiceName = billingLine.MasterService.ServiceName;
                        line.ServiceChargeType = billingLine.MasterServiceChargeType.Name;
                        line.ConsumedUnit = billingLine.ConsumedUnit;
                        line.Rate = billingLine.Rate ?? 0;
                        line.Discount = billingLine.Discount??0;
                        line.SubTotal = billingLine.ChargedAmount - billingLine.Tax;
                        line.Tax = billingLine.Tax;
                        line.Total = billingLine.ChargedAmount;
                        detailJournalData.Add(line);
                    }
                }
                return detailJournalData;    
            }
        }

    }

    [DataContract]
    public class InvoiceLineJournalData
    {
        [DataMember]
        public long InvoiceID { get; set; }
        [DataMember]
        public string PolicyID { get; set; }
        [DataMember]
        public DateTime BillingDate { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string ServiceChargeType { get; set; }
        [DataMember]
        public int ConsumedUnit { get; set; }
        [DataMember]
        public double Rate { get; set; }
        [DataMember]
        public decimal Discount { get; set; }
        [DataMember]
        public decimal SubTotal { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public decimal Total { get; set; }
    }
}
