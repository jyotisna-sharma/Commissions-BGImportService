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
using System.Linq.Expressions;

namespace MyAgencyVault.BusinessLibrary
{
    public class Calculation
    {
        #region Properties
        private List<PolicyDetailsData> _LicenseePolicies;
        private List<PolicyDetailsData> LicenseePolicies
        {
            get
            {
                return _LicenseePolicies;
            }
            set
            {
                _LicenseePolicies = value;
            }
        }
        public int NumberOfAgents { get; set; }
        public int NumberOfPayees { get; set; }
        List<PolicyData> _PolicyLevelData;
        public List<PolicyData> PolicyLevelData
        {
            get
            {
                if (_PolicyLevelData == null)
                {
                    _PolicyLevelData = new List<PolicyData>();
                }
                return _PolicyLevelData;
            }
            set
            {
                _PolicyLevelData = value;
            }
        }
        List<PolicyData> _TotalentriesPolicyLevelData;
        public List<PolicyData> TotalentriesPolicyLevelData
        {
            get
            {
                if (_TotalentriesPolicyLevelData == null)
                {
                    _TotalentriesPolicyLevelData = new List<PolicyData>();
                }
                return _TotalentriesPolicyLevelData;
            }
            set
            {
                _TotalentriesPolicyLevelData = value;
            }
        }
        List<PolicyData> _AdjustmentsPolicyLevelData;
        public List<PolicyData> AdjustmentsPolicyLevelData
        {
            get
            {
                if (_AdjustmentsPolicyLevelData == null)
                {
                    _AdjustmentsPolicyLevelData = new List<PolicyData>();
                }
                return _AdjustmentsPolicyLevelData;
            }
            set
            {
                _AdjustmentsPolicyLevelData = value;
            }
        }

        List<PolicyData> _TrackableMonthsPolicyLevelData;
        public List<PolicyData> TrackableMonthsPolicyLevelData
        {
            get
            {
                if (_TrackableMonthsPolicyLevelData == null)
                {
                    _TrackableMonthsPolicyLevelData = new List<PolicyData>();
                }
                return _TrackableMonthsPolicyLevelData;
            }
            set
            {
                _TrackableMonthsPolicyLevelData = value;
            }
        }
        public int NumberOfActivePolicies { get; set; }
        public int NumberOfTrackableMonth { get; set; }
        public int NumberOfBilledTrackableMonth { get; set; }
        public int NumberOfAdjustments { get; set; }
        public int NumberOfWebStatements { get; set; }
        public int NumberOfTotalEntries { get; set; }
        public int TotalEDI { get; set; }
        public DateTime variablePreviousExportDate { get; set; }
        public DateTime variableCurrentExportDate { get; set; }
        #endregion Properties
        private Guid _LicenseeID { get; set; }
        #region PrivateMethods
        private DateTime calculateBillingDate()
        {
            DateTime billingDate = new DateTime(variableCurrentExportDate.Year, variableCurrentExportDate.Month, 1);
            return billingDate;
        }
        private DateTime? calculateBillingTerminationDate(PolicyDetailsData policy, DLinq.CommissionDepartmentEntities DataModel)
        {
            DateTime? polTerminationDate = policy.PolicyTerminationDate;
            DateTime? maxInvoiceDate = null;
            DateTime? maxInvoiceDateNextMonth = null;

            if (polTerminationDate.HasValue)
            {
                maxInvoiceDate = DataModel.PolicyPaymentEntries.Where(p => p.PolicyId == policy.PolicyId).Select(s => s.InvoiceDate).Max();
                if (maxInvoiceDate != null)
                {
                    maxInvoiceDateNextMonth = maxInvoiceDate.Value.AddMonths(1);
                    maxInvoiceDateNextMonth = new DateTime(maxInvoiceDateNextMonth.Value.Year, maxInvoiceDateNextMonth.Value.Month, 1);
                }
            }

            //DLinq.PolicyLearnedField policyLearnedField = policy.LearnedFields;
            DateTime? autoTerminationDate = null;

            if (policy.LearnedFields != null)
                autoTerminationDate = policy.LearnedFields.AutoTerminationDate;
            else
                autoTerminationDate = DataModel.PolicyLearnedFields.Where(p => p.PolicyId == policy.PolicyId).FirstOrDefault().AutoTerminationDate;

            DateTime?[] dates = new DateTime?[] { polTerminationDate, maxInvoiceDateNextMonth, autoTerminationDate };
            DateTime? terDate = dates.Max();
            return terDate;
        }
        private int CalculateBilledTrackableMonthCount(Guid policyId, DLinq.CommissionDepartmentEntities DataModel)
        {
            int billedTrackableMonth = 0;
            List<DLinq.PolicyLevelBillingDetail> policyLevelBillingDetails = DataModel.PolicyLevelBillingDetails.Where(s => s.PolicyId == policyId && s.InvoiceBillingLineDetail.MasterServiceChargeType.SCTypeId == 2).ToList();
            billedTrackableMonth = policyLevelBillingDetails.Sum(s => s.TotalUnits ?? 0);

            return billedTrackableMonth;
        }
        private int CalculateTrackableMonthCount(PolicyDetailsData policy, DLinq.CommissionDepartmentEntities DataModel)
        {
            DateTime? TrackDate;
            if (policy.LearnedFields != null)
            {
                TrackDate = policy.LearnedFields.TrackFrom;//(from m in DataModel.PolicyLearnedFields where m.Policy.PolicyId == policyId select m.TrackFrom).FirstOrDefault();
            }
            else
            {
                TrackDate = DataModel.PolicyLearnedFields.Where(p => p.PolicyId == policy.PolicyId).FirstOrDefault().TrackFrom;
            }
            DateTime date = calculateBillingDate();
            int NoOfTrackableMonths = 0;

            if (TrackDate.HasValue)
            {
                DateTime? AutoTermDate = null;
                if (policy.LearnedFields == null)
                {
                    AutoTermDate = DataModel.PolicyLearnedFields.Where(p => p.PolicyId == policy.PolicyId).FirstOrDefault().AutoTerminationDate;
                }
                else
                {
                    AutoTermDate = policy.LearnedFields.AutoTerminationDate;
                }
                if (AutoTermDate.HasValue)
                {
                    NoOfTrackableMonths = (AutoTermDate.Value.Year - TrackDate.Value.Year) * 12 + (AutoTermDate.Value.Month - TrackDate.Value.Month);
                }
                else
                {
                    NoOfTrackableMonths = (date.Year - TrackDate.Value.Year) * 12 + (date.Month - TrackDate.Value.Month);
                }
            }

            return NoOfTrackableMonths;
        }
        private int calculateDistinctInvoiceDate(PolicyDetailsData policy, DLinq.CommissionDepartmentEntities DataModel)
        {
            int DisinctInvoiceDate = DataModel.PolicyPaymentEntries.Where(s => s.PolicyId == policy.PolicyId && s.InvoiceDate.Value > variablePreviousExportDate && s.InvoiceDate.Value <= variableCurrentExportDate).Select(s => s.InvoiceDate).Distinct().Count();
            return DisinctInvoiceDate;
        }
        private int calculateTotalEntry(PolicyDetailsData policy, DLinq.CommissionDepartmentEntities DataModel)
        {
            int Entries = DataModel.PolicyPaymentEntries.Where(s => s.PolicyId == policy.PolicyId && s.InvoiceDate.Value > variablePreviousExportDate && s.InvoiceDate.Value <= variableCurrentExportDate).Count();
            return Entries;
        }
        #endregion

        #region PublicMethods

        private void CalculateAdjustments(DLinq.CommissionDepartmentEntities DataModel)
        {
            int adjustments = 0;

            foreach (PolicyDetailsData policy in LicenseePolicies)
            {
                int totalEntry = calculateTotalEntry(policy, DataModel);
                int distInvoiceDates = calculateDistinctInvoiceDate(policy, DataModel);
                int adj = totalEntry - distInvoiceDates;

                PolicyData pDate = new PolicyData { PolicyID = policy.PolicyId, Value = adj };
                AdjustmentsPolicyLevelData.Add(pDate);

                adjustments += adj;
            }
            NumberOfAdjustments = adjustments;
        }
        private void CalculateTrackableUntrackableMonths(DLinq.CommissionDepartmentEntities DataModel)
        {
            NumberOfActivePolicies = NumberOfTrackableMonth = NumberOfBilledTrackableMonth = 0;
            int unbilledTrackableMonths = 0;

            DateTime? billingTerminationDate;
            DateTime billingCycleDate;

            foreach (PolicyDetailsData policy in LicenseePolicies)
            {
                billingTerminationDate = calculateBillingTerminationDate(policy, DataModel);
                billingCycleDate = calculateBillingDate();

                //DateTime? TrackFromDate = DataModel.PolicyLearnedFields.Where(s => s.PolicyId == policy.PolicyId).Select(s => s.TrackFrom).FirstOrDefault();
                DateTime? TrackFromDate;
                if (policy.LearnedFields == null)
                {
                    TrackFromDate = DataModel.PolicyLearnedFields.Where(s => s.PolicyId == policy.PolicyId).Select(s => s.TrackFrom).FirstOrDefault();
                }
                else
                {
                    TrackFromDate = policy.LearnedFields.TrackFrom;
                }
                int unbilledMonths = 0;
                if (billingCycleDate > TrackFromDate)
                {
                    if (billingTerminationDate == null || (billingTerminationDate.Value > variableCurrentExportDate) || billingTerminationDate.Value < billingCycleDate)
                    {
                        int trackableMonths = CalculateTrackableMonthCount(policy, DataModel);
                        int billedMonths = CalculateBilledTrackableMonthCount(policy.PolicyId, DataModel);
                        unbilledMonths = trackableMonths - billedMonths;
                        PolicyData pDate = new PolicyData { PolicyID = policy.PolicyId, Value = unbilledMonths };
                        TrackableMonthsPolicyLevelData.Add(pDate);
                        NumberOfTrackableMonth += trackableMonths;
                        NumberOfBilledTrackableMonth += billedMonths;
                    }
                }
            }
        }
        private void CalculateWebStatements(DLinq.CommissionDepartmentEntities DataModel)
        {
            int entryCount = 0;
            entryCount = (from m in DataModel.Batches
                          where m.UploadStatusId == 3 && m.LicenseeId == _LicenseeID
                          select m).Count();

            NumberOfWebStatements = entryCount;
        }
        private void CalculateTotalEntries(DLinq.CommissionDepartmentEntities DataModel)
        {
            int entryCount = 0;
            foreach (PolicyDetailsData policy in LicenseePolicies)
            {
                int count = (from m in DataModel.PolicyPaymentEntries where m.InvoiceDate.Value > variablePreviousExportDate && m.InvoiceDate.Value <= variableCurrentExportDate && m.PolicyId == policy.PolicyId select m).Count();
                PolicyData pDate = new PolicyData { PolicyID = policy.PolicyId, Value = count };
                TotalentriesPolicyLevelData.Add(pDate);
                entryCount += count;
            }
            NumberOfTotalEntries = entryCount;
        }
        private void CalculateActivePolicies()
        {
            int entryCount = 0;

            entryCount = LicenseePolicies.Where(lp => lp.PolicyStatusId != 1).Count();
            NumberOfActivePolicies = entryCount;
        }
        private void CalculateTotalEDI()
        {
            TotalEDI = 0;
        }
        private void CalculateAgents(DLinq.CommissionDepartmentEntities DataModel)
        {
            int entryCount = 0;
            entryCount = (from m in DataModel.UserCredentials where m.CreatedOn.Value > variablePreviousExportDate && m.CreatedOn.Value <= variableCurrentExportDate && m.Licensee.LicenseeId == _LicenseeID select m).Count();
            NumberOfAgents = entryCount;
        }
        private void CalculatePayees(DLinq.CommissionDepartmentEntities DataModel)
        {
            int entryCount = 0;
            entryCount = (from m in DataModel.UserDetails where m.AddPayeeOn.Value > variablePreviousExportDate && m.AddPayeeOn.Value <= variableCurrentExportDate && m.UserCredential.LicenseeId == _LicenseeID select m).Count();
            NumberOfPayees = entryCount;
        }

        public void CalculateVariablesforLicensee(Guid LicenseeID, DateTime PreviousExportDate, DateTime CurrentExportDate, DLinq.CommissionDepartmentEntities DataModel)
        {
            _LicenseeID = LicenseeID;
            variableCurrentExportDate = CurrentExportDate;
            variablePreviousExportDate = PreviousExportDate;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PolicyLicenseeId", LicenseeID);
            parameters.Add("IsDeleted", false);
            Expression<Func<DLinq.Policy, bool>> parameterexpression = p => p.PolicyStatusId != 1;

            LicenseePolicies = Policy.GetPolicyData(parameters, parameterexpression);

            LicenseePolicies.ForEach(p => p.LearnedFields = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(p.PolicyId));

            CalculateAdjustments(DataModel);
            CalculateTrackableUntrackableMonths(DataModel);
            CalculateWebStatements(DataModel);
            CalculateTotalEntries(DataModel);
            CalculateActivePolicies();
            CalculateTotalEDI();
            CalculateAgents(DataModel);
            CalculatePayees(DataModel);
        }

        #endregion
    }

    public class PolicyData
    {
        public Guid PolicyID { get; set; }
        public int Value { get; set; }
    }

}
