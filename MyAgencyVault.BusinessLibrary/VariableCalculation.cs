using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Threading;
using System.Data;
using System.IO;
using System.Globalization;

namespace MyAgencyVault.BusinessLibrary
{

  [DataContract]
  public enum ExportBatchFile
  {
    [EnumMember]
    ExportCardPayee,
    [EnumMember]
    ExportCheckPayee,
    [EnumMember]
    Refresh
  }

  [DataContract]
  public class LicenseeVariableInputDetail
  {
    [DataMember]
    public ExportBatchFile BatchFileInfo { get; set; }
    [DataMember]
    public List<Guid> Licensees { get; set; }
    [DataMember]
    public string selectedInvoiceMonth { get; set; }
  }

  [DataContract]
  public class VariableCollection
  {
    [DataMember]
    public int PolicyCount { get; set; }
    [DataMember]
    public int TrackableMonthCount { get; set; }
    [DataMember]
    public int UnbilledTrackableMonthCount { get; set; }
    [DataMember]
    public int UsersCount { get; set; }
    [DataMember]
    public int PayorsCount { get; set; }
    [DataMember]
    public int UnbilledEntrieCount { get; set; }
    [DataMember]
    public int UnbilledAdjustmentCount { get; set; }
    [DataMember]
    public int UnbilledWebStatementCount { get; set; }
    [DataMember]
    public int UnbilledEDICount { get; set; }
    [DataMember]
    public List<ServiceCharge> ServiceCharges { get; set; }

    public VariableCollection()
    {
      ServiceCharges = new List<ServiceCharge>();
    }
  }

  [DataContract]
  public class LicenseeVariableOutputDetail
  {
    [DataMember]
    public Dictionary<Guid, VariableCollection> LicenseesValueDictionary;
    [DataMember]
    public ExportBatchFile BatchFileType;

    public LicenseeVariableOutputDetail()
    {
      LicenseesValueDictionary = new Dictionary<Guid, VariableCollection>();
    }

    public LicenseeVariableOutputDetail(Guid liceId)
    {
      LicenseesValueDictionary = new Dictionary<Guid, VariableCollection>();
      LicenseesValueDictionary.Add(liceId, new VariableCollection());
    }

    internal void setPolicyCount(Guid id, int value)
    {
      AddVariable(id);
      LicenseesValueDictionary[id].PolicyCount += value;
    }

    internal void setTrackableMonthCount(Guid id, int value)
    {
      AddVariable(id);
      LicenseesValueDictionary[id].TrackableMonthCount += value;
    }

    internal void setUnbilledTrackableMonthCount(Guid id, int value)
    {
      AddVariable(id);
      LicenseesValueDictionary[id].UnbilledTrackableMonthCount += value;
    }

    internal void setUsersCount(Guid id, int value)
    {
      AddVariable(id);
      LicenseesValueDictionary[id].UsersCount += value;
    }

    internal void setPayorsCount(Guid id, int value)
    {
      AddVariable(id);
      LicenseesValueDictionary[id].PayorsCount += value;
    }

    internal void setUnbilledEntrieCount(Guid id, int value)
    {
      AddVariable(id);
      LicenseesValueDictionary[id].UnbilledEntrieCount += value;
    }

    internal void setUnbilledAdjustmentCount(Guid id, int value)
    {
      AddVariable(id);
      LicenseesValueDictionary[id].UnbilledAdjustmentCount += value;
    }

    internal void setUnbilledWebStatementCount(Guid id, int value)
    {
      AddVariable(id);
      LicenseesValueDictionary[id].UnbilledWebStatementCount += value;
    }

    internal void setUnbilledEDICount(Guid id, int value)
    {
      AddVariable(id);
      LicenseesValueDictionary[id].UnbilledEDICount += value;
    }

    internal void setServiceCharges(Guid id, List<ServiceCharge> charges)
    {
      AddVariable(id);
      LicenseesValueDictionary[id].ServiceCharges = charges;
    }

    internal void AddVariable(Guid id)
    {
      if (!LicenseesValueDictionary.ContainsKey(id))
        LicenseesValueDictionary.Add(id, new VariableCollection());
    }
  }

  public class VariableCalculation
  {
    private LicenseeVariableOutputDetail _licVariableInfo;
    private LicenseeVariableInputDetail _licInputInfo;

    public LicenseeVariableOutputDetail LicenseeVariableInfo
    {
      get { return _licVariableInfo; }
      set { _licVariableInfo = value; }
    }

    public LicenseeVariableInputDetail LicenseeInputInfo
    {
      get { return _licInputInfo; }
      set { _licInputInfo = value; }
    }

    public void StartVariableCalculation()
    {
      if (_licInputInfo != null)
      {
        List<ExportCardPayeeInfo> exportCardPayees = new List<ExportCardPayeeInfo>();
        ExportCardPayeeInfo exportCardPaye = null;

        ExportDate lastExportData = ExportDate.getExportDate();

        DateTime lastExportedDate = DateTime.Today;
        DateTime currentExportedDate = DateTime.Today;

        bool isCardPayee = false;

        switch (_licInputInfo.BatchFileInfo)
        {
          case ExportBatchFile.ExportCardPayee:
            {
              _licVariableInfo.BatchFileType = ExportBatchFile.ExportCardPayee;

              using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
              {
                _licInputInfo.Licensees = (from se in DataModel.Licensees
                                           where se.LicensePaymentModeId != 1 && (se.IsDeleted == false || se.IsDeleted == null) && se.LicenseStatusId == 0
                                           select se.LicenseeId).ToList();

                if (!lastExportData.CardPayeeExportDate.HasValue)
                {
                  lastExportedDate = DataModel.ServiceLines.Select(s => s.StartDate).Min().Value;
                  lastExportedDate = lastExportedDate.AddMonths(-1);
                }
                else
                  lastExportedDate = lastExportData.CardPayeeExportDate.Value;
              }

              currentExportedDate = DateTime.ParseExact(_licInputInfo.selectedInvoiceMonth, "MMM-yyyy", DateTimeFormatInfo.InvariantInfo);
              isCardPayee = true;
            }
            break;
          case ExportBatchFile.ExportCheckPayee:
            {
              _licVariableInfo.BatchFileType = ExportBatchFile.ExportCheckPayee;

              using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
              {
                _licInputInfo.Licensees = (from se in DataModel.Licensees
                                           where se.LicensePaymentModeId == 1 && (se.IsDeleted == false || se.IsDeleted == null) && se.LicenseStatusId == 0
                                           select se.LicenseeId).ToList();

                if (!lastExportData.CheckPayeeExportDate.HasValue)
                {
                  lastExportedDate = DataModel.ServiceLines.Select(s => s.StartDate).Min().Value;
                  lastExportedDate = lastExportedDate.AddMonths(-1);
                }
                else
                  lastExportedDate = lastExportData.CheckPayeeExportDate.Value;
              }

              currentExportedDate = DateTime.ParseExact(_licInputInfo.selectedInvoiceMonth, "MMM-yyyy", DateTimeFormatInfo.InvariantInfo);
              isCardPayee = false;
            }
            break;
          #region Refresh
          case ExportBatchFile.Refresh:
            {
              _licVariableInfo.BatchFileType = ExportBatchFile.Refresh;

              using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
              {
                DLinq.Licensee lic = null;
                if (_licInputInfo.Licensees != null && _licInputInfo.Licensees.Count != 0)
                {
                  Guid id = new Guid(_licInputInfo.Licensees[0].ToString());
                  lic = (from m in DataModel.Licensees where id == m.LicenseeId select m).FirstOrDefault();
                  _licVariableInfo.AddVariable(id);
                }

                if (lic != null && lic.MasterLicensePaymentMode.LicensePaymentModeId == 1)
                  if (!lastExportData.CheckPayeeExportDate.HasValue)
                  {
                    lastExportedDate = DataModel.ServiceLines.Select(s => s.StartDate).Min().Value;
                    lastExportedDate = lastExportedDate.AddMonths(-1);
                  }
                  else
                    lastExportedDate = lastExportData.CardPayeeExportDate.Value;
                else
                  if (!lastExportData.CardPayeeExportDate.HasValue)
                  {
                    lastExportedDate = DataModel.ServiceLines.Select(s => s.StartDate).Min().Value;
                    lastExportedDate = lastExportedDate.AddMonths(-1);
                  }
                  else
                    lastExportedDate = lastExportData.CardPayeeExportDate.Value;

                currentExportedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                isCardPayee = false;
                CalculateVariablesOnly(_licInputInfo.Licensees[0], lastExportedDate, currentExportedDate, DataModel);
              }
            }
            break;
          #endregion
        }

        if (_licInputInfo.BatchFileInfo == ExportBatchFile.Refresh)
          return;

        int startMonth = lastExportedDate.Month;
        int endMonth = currentExportedDate.Month;
        int startYear = lastExportedDate.Year;
        int endYear = currentExportedDate.Year;

        using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        {
          if (DataModel.Connection.State != ConnectionState.Open)
            DataModel.Connection.Open();

          IDbTransaction transaction = DataModel.Connection.BeginTransaction();

          try
          {
            List<DLinq.Invoice> savedInvoices = new List<DLinq.Invoice>();
            DateTime? startDate = null;
            DateTime? endDate = null;
            foreach (Guid licenseeId in _licInputInfo.Licensees)
            {
              List<BillingLinePlusPolicyData> BillingServiceLines = new List<BillingLinePlusPolicyData>();
              exportCardPaye = null;

              List<DLinq.ServiceLine> serviceLines = getApplicableServiceLines(licenseeId, currentExportedDate, DataModel);
              int varValue = 0;

              var distinctChargeTypes = (from m in serviceLines select m.MasterServiceChargeType.Name).Distinct();
              Calculation VariableCalculation = new Calculation();
              VariableCalculation.CalculateVariablesforLicensee(licenseeId, lastExportedDate, currentExportedDate, DataModel);
              setVariablesValue(licenseeId, VariableCalculation);
              foreach (DLinq.ServiceLine serviceLine in serviceLines)
              {
                VariableCalculation.PolicyLevelData = new List<PolicyData>();
                switch (serviceLine.ServiceChargeTypeId.ToString())
                {
                  case "1"://Flat Charge
                    varValue = 1;
                    break;
                  case "2"://Per Trackable Month
                    varValue = VariableCalculation.NumberOfTrackableMonth - VariableCalculation.NumberOfBilledTrackableMonth;
                    VariableCalculation.PolicyLevelData = VariableCalculation.TrackableMonthsPolicyLevelData;
                    break;
                  case "3"://Per Adjustment
                    varValue = VariableCalculation.NumberOfAdjustments;
                    VariableCalculation.PolicyLevelData = VariableCalculation.AdjustmentsPolicyLevelData;
                    break;
                  case "4"://Per Entry
                    varValue = VariableCalculation.NumberOfTotalEntries;
                    VariableCalculation.PolicyLevelData = VariableCalculation.TotalentriesPolicyLevelData;
                    break;
                  case "5"://Per Web Statement
                    varValue = VariableCalculation.NumberOfWebStatements;
                    break;
                  case "6"://Per EDI
                    varValue = VariableCalculation.TotalEDI;
                    break;
                  case "7"://Per policy
                    varValue = VariableCalculation.NumberOfActivePolicies;
                    break;
                  case "8"://Per Agent
                    varValue = VariableCalculation.NumberOfAgents;
                    break;
                  case "9"://Per Payee
                    varValue = VariableCalculation.NumberOfPayees;
                    break;
                }
                convertServiceLineToBillingLine(serviceLine, VariableCalculation.PolicyLevelData, varValue, BillingServiceLines, DataModel);
              }


              if (BillingServiceLines != null && BillingServiceLines.Count != 0)
              {
                DateTime NextToInvoiceMonth = currentExportedDate.AddMonths(1);
                decimal InvoiceAmount = 0;

                foreach (BillingLinePlusPolicyData billLine in BillingServiceLines)
                  InvoiceAmount += billLine.BillingLine.ChargedAmount;

                DLinq.Invoice invoice = new DLinq.Invoice
                {
                  BillingDate = new DateTime(NextToInvoiceMonth.Year, NextToInvoiceMonth.Month, 1),
                  BillingStartDate = startDate,
                  BillingEndDate = endDate,
                  InvoiceAmount = InvoiceAmount,
                  DueBalance = InvoiceAmount,
                  LicenseeId = licenseeId,
                  InvoiceGeneratedOn = DateTime.Today
                };

              
                foreach (BillingLinePlusPolicyData billingline in BillingServiceLines)
                {
                  billingline.BillingLine.Invoice = invoice;
                  if (billingline.PolicyData != null)
                  {
                    foreach (PolicyData policyData in billingline.PolicyData)
                    {
                      DLinq.PolicyLevelBillingDetail polLine = new DLinq.PolicyLevelBillingDetail();
                      polLine.TotalUnits = policyData.Value;
                      polLine.PolicyId = policyData.PolicyID;
                      polLine.InvoiceBillingLineDetail = billingline.BillingLine;
                    }
                  }
                }

                DataModel.AddToInvoices(invoice);

                DLinq.Licensee licensee = DataModel.Licensees.FirstOrDefault(s => s.LicenseeId == licenseeId);
                if (licensee != null)
                  licensee.DueBalance += InvoiceAmount;

                savedInvoices.Add(invoice);
              }
            }

            if (savedInvoices.Count != 0)
            {
              DLinq.ExportBatchFile expBatchFile = new DLinq.ExportBatchFile();
              expBatchFile.ExportBatchId = Guid.NewGuid();

              string fileName = string.Empty;
              if (_licInputInfo.BatchFileInfo == ExportBatchFile.ExportCardPayee)
                fileName = "Card-Export-" + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
              else
                fileName = "Check-Export-" + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";


              expBatchFile.FileName = fileName;
              expBatchFile.CreatedOn = DateTime.Today;
              expBatchFile.IsFileImported = false;
              expBatchFile.InvoiceDate = currentExportedDate;
              DataModel.AddToExportBatchFiles(expBatchFile);

              foreach (DLinq.Invoice inc in savedInvoices)
                inc.ExportBatchFile = expBatchFile;

              DataModel.SaveChanges();

              foreach (DLinq.Invoice inc in savedInvoices)
              {
                exportCardPaye = ExportCardPayeeInfo.fillExportCardPayeeInfo(inc.InvoiceId, isCardPayee, DataModel);
                exportCardPayees.Add(exportCardPaye);
              }

              if (ExportCardPayeeInfo.CreateExportFile(exportCardPayees, fileName))
              {
                string KeyValue = SystemConstant.GetKeyValue("ServerWebDevPath");
                string[] Keys = KeyValue.Split(';');
                FileUtility ObjUpload = null;

                if (Keys.Length == 4)
                  ObjUpload = FileUtility.CreateClient(Keys[0], Keys[1], Keys[2], Keys[3]);
                else
                  ObjUpload = FileUtility.CreateClient(Keys[0], Keys[1], Keys[2], null);

                AutoResetEvent autoResetEvent = new AutoResetEvent(false);
                ObjUpload.UploadComplete += (i, j) =>
                {
                  autoResetEvent.Set();
                };
                ObjUpload.Upload(Path.GetTempPath() + fileName, @"/UploadBatch/" + fileName);
                autoResetEvent.WaitOne();
              }
              savedInvoices.Clear();
            }

            DLinq.MasterSystemConstant sysConst = null;
            if (_licInputInfo.BatchFileInfo == ExportBatchFile.ExportCardPayee)
            {
              sysConst = (from m in DataModel.MasterSystemConstants where m.Name == "CardPayeeDate" select m).FirstOrDefault();
              if (sysConst != null)
                sysConst.Value = currentExportedDate.ToString("MMM-yyyy");
            }
            else
            {
              sysConst = (from m in DataModel.MasterSystemConstants where m.Name == "CheckPayeeDate" select m).FirstOrDefault();
              if (sysConst != null)
                sysConst.Value = currentExportedDate.ToString("MMM-yyyy");
            }

            DataModel.SaveChanges();
            transaction.Commit();
          }
          catch
          {
            transaction.Rollback();
          }
        }
      }
    }
    private void setVariablesValue(Guid licenseeId, Calculation Variables)
    {
      _licVariableInfo.setPolicyCount(licenseeId, Variables.NumberOfActivePolicies);
      _licVariableInfo.setTrackableMonthCount(licenseeId, Variables.NumberOfTrackableMonth);
      _licVariableInfo.setUnbilledTrackableMonthCount(licenseeId, Variables.NumberOfTrackableMonth - Variables.NumberOfBilledTrackableMonth);
      _licVariableInfo.setUsersCount(licenseeId, Variables.NumberOfAgents);
      _licVariableInfo.setPayorsCount(licenseeId, Variables.NumberOfPayees);
      _licVariableInfo.setUnbilledEntrieCount(licenseeId, Variables.NumberOfTotalEntries);
      _licVariableInfo.setUnbilledWebStatementCount(licenseeId, Variables.NumberOfWebStatements);
      _licVariableInfo.setUnbilledEDICount(licenseeId, Variables.TotalEDI);
      _licVariableInfo.setUnbilledAdjustmentCount(licenseeId, Variables.NumberOfAdjustments);
    }
    private void CalculateVariablesOnly(Guid licenseeId, DateTime LastExportDate, DateTime CurrentExportDate, DLinq.CommissionDepartmentEntities DataModel)
    {
      List<ServiceCharge> ServiceChargeList = new List<ServiceCharge>();
      ServiceCharge serviceCharge;

      List<DLinq.ServiceLine> serviceLines = getApplicableServiceLines(licenseeId, CurrentExportDate, DataModel);
      var distinctChargeTypes = (from m in serviceLines select m.MasterServiceChargeType.Name).Distinct();
      Calculation Variables = new Calculation();
      Variables.CalculateVariablesforLicensee(licenseeId, LastExportDate, CurrentExportDate, DataModel);

      setVariablesValue(licenseeId, Variables);

      foreach (DLinq.ServiceLine serviceLine in serviceLines)
      {
        double? EstimatedServiceCharge = 0.0;
        switch (serviceLine.ServiceChargeTypeId.ToString())
        {

          case "1"://Flat Charge
            serviceCharge = new ServiceCharge();
            serviceCharge.ServiceId = serviceLine.ServiceId;
            serviceCharge.Charge = Convert.ToDecimal(serviceLine.Rate);
            ServiceChargeList.Add(serviceCharge);
            break;
          case "2"://Per Trackable Month
            int unBilledMonths = _licVariableInfo.LicenseesValueDictionary[licenseeId].UnbilledTrackableMonthCount;
            if (unBilledMonths < serviceLine.Range1)
            {
              EstimatedServiceCharge = 0.0;
            }
            else
            {
              if (unBilledMonths > serviceLine.Range2)
              {
                EstimatedServiceCharge = serviceLine.Range2 * serviceLine.Rate;
              }
              else
              {
                EstimatedServiceCharge = (unBilledMonths - serviceLine.Range1 + 1) * serviceLine.Rate;
              }
            }
            CalculateTaxAndDiscount(EstimatedServiceCharge, ServiceChargeList, serviceLine);
            break;
          case "3"://Per Adjustment
            int AdjustmentsSinceLastBilling = _licVariableInfo.LicenseesValueDictionary[licenseeId].UnbilledAdjustmentCount;
            if (AdjustmentsSinceLastBilling > serviceLine.Range2)
            {
              EstimatedServiceCharge = serviceLine.Range2 * serviceLine.Rate;
            }
            else
            {
              EstimatedServiceCharge = AdjustmentsSinceLastBilling * serviceLine.Rate;
            }
            CalculateTaxAndDiscount(EstimatedServiceCharge, ServiceChargeList, serviceLine);
            break;
          case "4"://Per Entry
            int EntriesSinceLastBilling = _licVariableInfo.LicenseesValueDictionary[licenseeId].UnbilledEntrieCount;
            if (EntriesSinceLastBilling > serviceLine.Range2)
            {
              EstimatedServiceCharge = serviceLine.Range2 * serviceLine.Rate;
            }
            else
            {
              EstimatedServiceCharge = EntriesSinceLastBilling * serviceLine.Rate;
            }
            CalculateTaxAndDiscount(EstimatedServiceCharge, ServiceChargeList, serviceLine);
            break;
          case "5"://Per Web Statement
            int WebStatementsSinceLastBilling = _licVariableInfo.LicenseesValueDictionary[licenseeId].UnbilledWebStatementCount;
            if (WebStatementsSinceLastBilling > serviceLine.Range2)
            {
              EstimatedServiceCharge = serviceLine.Range2 * serviceLine.Rate;
            }
            else
            {
              EstimatedServiceCharge = WebStatementsSinceLastBilling * serviceLine.Rate;
            }
            CalculateTaxAndDiscount(EstimatedServiceCharge, ServiceChargeList, serviceLine);
            break;
          case "6"://Per EDI
            int EDISinceLastbilling = _licVariableInfo.LicenseesValueDictionary[licenseeId].UnbilledEDICount;
            if (EDISinceLastbilling > serviceLine.Range2)
            {
              EstimatedServiceCharge = serviceLine.Range2 * serviceLine.Rate;
            }
            else
            {
              EstimatedServiceCharge = EDISinceLastbilling * serviceLine.Rate;
            }
            CalculateTaxAndDiscount(EstimatedServiceCharge, ServiceChargeList, serviceLine);
            break;
          case "7"://Per policy
            int NumberofPolicies = _licVariableInfo.LicenseesValueDictionary[licenseeId].PolicyCount;

            if (NumberofPolicies > serviceLine.Range2)
            {
              EstimatedServiceCharge = serviceLine.Range2 * serviceLine.Rate;
            }
            else
            {
              EstimatedServiceCharge = NumberofPolicies * serviceLine.Rate;
            }
            if (EstimatedServiceCharge < serviceLine.Min)
            {
              EstimatedServiceCharge = serviceLine.Min;
            }
            CalculateTaxAndDiscount(EstimatedServiceCharge, ServiceChargeList, serviceLine);

            break;
          case "8"://Per Agent
            int numberOfAgents = _licVariableInfo.LicenseesValueDictionary[licenseeId].UsersCount;
            if (numberOfAgents > serviceLine.Range2)
            {
              EstimatedServiceCharge = serviceLine.Range2 * serviceLine.Rate;
            }
            else
            {
              EstimatedServiceCharge = numberOfAgents * serviceLine.Rate;
            }
            CalculateTaxAndDiscount(EstimatedServiceCharge, ServiceChargeList, serviceLine);
            break;
          case "9"://Per Payee
            int NumberOfPayees = _licVariableInfo.LicenseesValueDictionary[licenseeId].PayorsCount;
            if (NumberOfPayees > serviceLine.Range2)
            {
              EstimatedServiceCharge = serviceLine.Range2 * serviceLine.Rate;
            }
            else
            {
              EstimatedServiceCharge = NumberOfPayees * serviceLine.Rate;
            }
            CalculateTaxAndDiscount(EstimatedServiceCharge, ServiceChargeList, serviceLine);
            break;
        }
      }

      #region commentedCode
      //int startMonth = LastExpDate.Month;
      //int endMonth = CurrentExpDate.Month;
      //int startYear = LastExpDate.Year;
      //int endYear = CurrentExpDate.Year;
      //List<ServiceCharge> ServiceCharges = new List<ServiceCharge>();

      //for (int year = startYear; year <= endYear; year++)
      //{
      //  endMonth = 12;
      //  startMonth = 1;

      //  if (year == endYear)
      //    endMonth = CurrentExpDate.Month;

      //  if (year == startYear)
      //    startMonth = LastExpDate.Month + 1;

      //  for (int month = startMonth; month <= endMonth; month++)
      //  {
      //    List<DLinq.ServiceLine> serviceLines = getApplicableServiceLines(licenseeId, month, year, DataModel);
      //    DLinq.ServiceLine tempServLine = null;
      //    int varValue = 0;

      //    if (serviceLines != null && serviceLines.Count != 0)
      //    {
      //      var slines = (from m in serviceLines where m.MasterServiceChargeType.SCTypeId == 1 select m).ToList();

      //      foreach (DLinq.ServiceLine servLine in slines)
      //        calculateServiceCharge(servLine, 1, ServiceCharges, DataModel);

      //      var distinctChargeTypes = (from m in serviceLines select m.MasterServiceChargeType.Name).Distinct();

      //      foreach (string chargeType in distinctChargeTypes)
      //      {
      //        switch (chargeType)
      //        {
      //          case "Per Trackable Month":
      //            if (year == endYear && month == endMonth)
      //            {
      //              PerTrackableMonth perMonth = new PerTrackableMonth { LicenseeID = licenseeId, Month = month, Year = year, DataModel = DataModel };
      //              varValue = perMonth.Calculate();

      //              if (varValue != 0)
      //              {
      //                _licVariableInfo.setPolicyCount(licenseeId, perMonth.NoOfPolicy);
      //                _licVariableInfo.setTrackableMonthCount(licenseeId, perMonth.NoOfTrackableMonth);
      //                _licVariableInfo.setUnbilledTrackableMonthCount(licenseeId, perMonth.NoOfTrackableMonth - perMonth.NoOfBilledTrackableMonth);

      //                tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Trackable Month" && m.Range1.Value <= varValue && m.Range2.Value >= varValue select m).First();
      //              }
      //              else
      //                tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Trackable Month" select m).First();

      //              calculateServiceCharge(tempServLine, varValue, ServiceCharges, DataModel);
      //            }
      //            break;
      //          case "Per Adjustment":
      //            PerAdjustment perAdjustment = new PerAdjustment { LicenseeID = licenseeId, Month = month, Year = year, DataModel = DataModel };
      //            varValue = perAdjustment.Calculate();

      //            if (varValue != 0)
      //            {
      //              _licVariableInfo.setUnbilledAdjustmentCount(licenseeId, varValue);
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Adjustment" && m.Range1.Value <= varValue && m.Range2.Value >= varValue select m).First();
      //            }
      //            else
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Adjustment" select m).First();

      //            calculateServiceCharge(tempServLine, varValue, ServiceCharges, DataModel);
      //            break;
      //          case "Per Web Statement":
      //            PerWebstatement perWebstatement = new PerWebstatement { LicenseeID = licenseeId, Month = month, Year = year, DataModel = DataModel };
      //            varValue = perWebstatement.Calculate();

      //            if (varValue != 0)
      //            {
      //              _licVariableInfo.setUnbilledWebStatementCount(licenseeId, varValue);
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Web Statement" && m.Range1.Value <= varValue && m.Range2.Value >= varValue select m).First();
      //            }
      //            else
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Web Statement" select m).First();

      //            calculateServiceCharge(tempServLine, varValue, ServiceCharges, DataModel);

      //            break;
      //          case "Per Entry":
      //            PerEntry perEntry = new PerEntry { LicenseeID = licenseeId, Month = month, Year = year, DataModel = DataModel };
      //            varValue = perEntry.Calculate();

      //            if (varValue != 0)
      //            {
      //              _licVariableInfo.setUnbilledEntrieCount(licenseeId, varValue);
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Entry" && m.Range1.Value <= varValue && m.Range2.Value >= varValue select m).First();
      //            }
      //            else
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Entry" select m).First();

      //            calculateServiceCharge(tempServLine, varValue, ServiceCharges, DataModel);
      //            break;
      //          case "Per Agent":
      //            PerAgent perAgent = new PerAgent { LicenseeID = licenseeId, Month = month, Year = year, DataModel = DataModel };
      //            varValue = perAgent.Calculate();

      //            if (varValue != 0)
      //            {
      //              _licVariableInfo.setUsersCount(licenseeId, varValue);
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Agent" && m.Range1.Value <= varValue && m.Range2.Value >= varValue select m).First();
      //            }
      //            else
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Agent" select m).First();

      //            calculateServiceCharge(tempServLine, varValue, ServiceCharges, DataModel);
      //            break;
      //          case "Per Payee":
      //            PerPayee perPayee = new PerPayee { LicenseeID = licenseeId, Month = month, Year = year, DataModel = DataModel };
      //            varValue = perPayee.Calculate();

      //            if (varValue != 0)
      //            {
      //              _licVariableInfo.setPayorsCount(licenseeId, varValue);
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Payee" && m.Range1.Value <= varValue && m.Range2.Value >= varValue select m).First();
      //            }
      //            else
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Payee" select m).First();

      //            calculateServiceCharge(tempServLine, varValue, ServiceCharges, DataModel);
      //            break;
      //          case "Per Active Policy":
      //            PerPayee perActivePolicy = new PerPayee { LicenseeID = licenseeId, Month = month, Year = year, DataModel = DataModel };
      //            varValue = perActivePolicy.Calculate();

      //            if (varValue != 0)
      //            {
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Active Policy" && m.Range1.Value <= varValue && m.Range2.Value >= varValue select m).First();
      //            }
      //            else
      //              tempServLine = (from m in serviceLines where m.MasterServiceChargeType.Name == "Per Active Policy" select m).First();

      //            calculateServiceCharge(tempServLine, varValue, ServiceCharges, DataModel);
      //            break;
      //        }
      //      }
      //    }
      //  }
      //}
      #endregion commentedCode
      _licVariableInfo.setServiceCharges(licenseeId, ServiceChargeList);
    }
    private void CalculateTaxAndDiscount(double? EstimatedServiceCharge, List<ServiceCharge> ServiceChargeList, DLinq.ServiceLine serviceLine)
    {
      double? MinimumCharge = serviceLine.Min == null ? 0.0 : serviceLine.Min;

      EstimatedServiceCharge = EstimatedServiceCharge > MinimumCharge ? EstimatedServiceCharge : MinimumCharge;
      double taxAmount = serviceLine.Licensee.TaxRate == null ? 0.0 : (serviceLine.Licensee.TaxRate.Value / 100);
      double discount = EstimatedServiceCharge * serviceLine.Discount == null ? 0.0 : (serviceLine.Discount.Value / 100);
      EstimatedServiceCharge = EstimatedServiceCharge - discount + taxAmount;

      ServiceCharge serviceCharge = new ServiceCharge();
      serviceCharge.ServiceId = serviceLine.ServiceId;
      serviceCharge.Charge = Convert.ToDecimal(EstimatedServiceCharge);
      ServiceChargeList.Add(serviceCharge);
    }
    private void calculateServiceCharge(DLinq.ServiceLine serviceLine, int value, List<ServiceCharge> serviceCharges, DLinq.CommissionDepartmentEntities DataModel)
    {
      ServiceCharge serviceCharge = serviceCharges.FirstOrDefault(s => s.ServiceId == serviceLine.ServiceId);
      if (serviceCharge == null)
      {
        serviceCharge = new ServiceCharge();
        serviceCharge.ServiceId = serviceLine.ServiceId;

        decimal amount = 0;
        decimal ChargedAmount = 0;

        if (value == 0)
          amount = Convert.ToDecimal(serviceLine.Min);
        else
          amount = Convert.ToDecimal(serviceLine.Rate * value);

        if (amount != 0)
        {
          decimal discount = amount * Convert.ToDecimal(serviceLine.Discount == null ? 0.0 : (serviceLine.Discount.Value / 100));
          decimal tax = 0;

          if (serviceLine.IsTaxable)
          {
            tax = (amount - discount) * Convert.ToDecimal(serviceLine.Licensee.TaxRate == null ? 0.0 : (serviceLine.Licensee.TaxRate.Value / 100));
          }

          ChargedAmount = amount - discount + tax;
        }
        else
        {
          ChargedAmount = 0;
        }

        serviceCharge.Charge = ChargedAmount;
        serviceCharges.Add(serviceCharge);
      }
      else
      {
        decimal amount = 0;
        decimal ChargedAmount = 0;

        if (value == 0)
          amount = Convert.ToDecimal(serviceLine.Min);
        else
          amount = Convert.ToDecimal(serviceLine.Rate * value);

        if (amount != 0)
        {
          decimal discount = amount * Convert.ToDecimal(serviceLine.Discount == null ? 0.0 : (serviceLine.Discount.Value / 100));
          decimal tax = 0;

          if (serviceLine.IsTaxable)
          {
            tax = (amount - discount) * Convert.ToDecimal(serviceLine.Licensee.TaxRate == null ? 0.0 : (serviceLine.Licensee.TaxRate.Value / 100));
          }

          ChargedAmount = amount - discount + tax;
        }
        else
        {
          ChargedAmount = 0;
        }
        serviceCharge.Charge += ChargedAmount;
      }
    }

    private void convertServiceLineToBillingLine(DLinq.ServiceLine serviceLine, List<PolicyData> policyData, int value, List<BillingLinePlusPolicyData> billingPolicyData, DLinq.CommissionDepartmentEntities DataModel)
    {
      BillingLinePlusPolicyData billingAndPolicyData = billingPolicyData.FirstOrDefault(s => s.BillingLine.ServiceId == serviceLine.ServiceId);

      if (billingAndPolicyData == null)
      {
        billingAndPolicyData = new BillingLinePlusPolicyData();
        DLinq.InvoiceBillingLineDetail variable = new DLinq.InvoiceBillingLineDetail
        {
          BillingLineId = Guid.NewGuid(),
          Min = Convert.ToDecimal(serviceLine.Min),
          Range1 = serviceLine.Range1,
          Range2 = serviceLine.Range2,
          Rate = serviceLine.Rate,
          StartDate = serviceLine.StartDate,
          EndDate = serviceLine.EndDate
        };

        variable.MasterService = serviceLine.MasterService;
        variable.MasterServiceChargeType = serviceLine.MasterServiceChargeType;

        variable.ConsumedUnit = value;

        decimal amount = 0;
        if (value == 0)
          amount = Convert.ToDecimal(variable.Min);
        else
          amount = Convert.ToDecimal(serviceLine.Rate * value);

        if (amount != 0)
        {
          decimal discount = amount * Convert.ToDecimal(serviceLine.Discount == null ? 0.0 : (serviceLine.Discount.Value / 100));
          variable.Discount = discount;

          decimal tax = 0;
          if (serviceLine.IsTaxable)
          {
            tax = (amount - discount) * Convert.ToDecimal(serviceLine.Licensee.TaxRate == null ? 0.0 : (serviceLine.Licensee.TaxRate.Value / 100));
          }
          variable.Tax = tax;

          variable.ChargedAmount = amount - discount + tax;
        }
        else
        {
          variable.ChargedAmount = 0;
        }

        billingAndPolicyData.BillingLine = variable;
        billingAndPolicyData.PolicyData = policyData;

        billingPolicyData.Add(billingAndPolicyData);
      }
      else
      {
        DLinq.InvoiceBillingLineDetail variable = billingAndPolicyData.BillingLine;
        variable.ConsumedUnit += value;

        decimal amount = 0;
        if (value == 0)
          amount = Convert.ToDecimal(variable.Min);
        else
          amount = Convert.ToDecimal(serviceLine.Rate * value);

        if (amount != 0)
        {
          decimal discount = amount * Convert.ToDecimal(serviceLine.Discount == null ? 0.0 : (serviceLine.Discount.Value / 100));
          variable.Discount += discount;

          decimal tax = 0;
          if (serviceLine.IsTaxable)
          {
            tax = (amount - discount) * Convert.ToDecimal(serviceLine.Licensee.TaxRate == null ? 0.0 : (serviceLine.Licensee.TaxRate.Value / 100));
          }

          variable.Tax += tax;
          variable.ChargedAmount += amount - discount + tax;
        }
        else
        {
          variable.ChargedAmount += 0;
        }
      }
    }

    private List<DLinq.ServiceLine> getApplicableServiceLines(Guid licenseeId, DateTime BillingDate, DLinq.CommissionDepartmentEntities DataModel)
    {
      int month = BillingDate.Month;
      int year = BillingDate.Year;
      return getApplicableServiceLines(licenseeId, month, year, DataModel);
    }

    private List<DLinq.ServiceLine> getApplicableServiceLines(Guid licenseeId, int month, int year, DLinq.CommissionDepartmentEntities DataModel)
    {
      DateTime? billingDate = new DateTime(year, month, 1).AddMonths(1);
      List<DLinq.ServiceLine> applicableServiceLines = new List<DLinq.ServiceLine>();


      var serviceLines = (from m in DataModel.ServiceLines
                          where m.Licensee.LicenseeId == licenseeId && m.StartDate <= billingDate && m.EndDate >= billingDate
                          select m).ToList();

      foreach (DLinq.ServiceLine servLine in serviceLines)
      {
        applicableServiceLines.Add(servLine);

        if (!servLine.MasterServiceReference.IsLoaded)
          servLine.MasterServiceReference.Load();

        if (!servLine.MasterServiceChargeTypeReference.IsLoaded)
          servLine.MasterServiceChargeTypeReference.Load();

        if (!servLine.LicenseeReference.IsLoaded)
          servLine.LicenseeReference.Load();
      }
      return applicableServiceLines;
    }
  }

  public class BillingLinePlusPolicyData
  {
    public DLinq.InvoiceBillingLineDetail BillingLine { get; set; }
    public List<PolicyData> PolicyData { get; set; }
  }

  public class ServiceCharge
  {
    public int ServiceId { get; set; }
    public decimal Charge { get; set; }
  }


}
