using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;


namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class LearnedToPolicyPost : PolicyDetailsData
    {
        public static void AddUpdateLearnedToPolicy(Guid PolicyId)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    //var _PolicyLearned = (from p in DataModel.PolicyLearnedFields where (p.PolicyId == PolicyId) select p).FirstOrDefault();
                    PolicyLearnedFieldData _PolicyLearned = PolicyLearnedField.GetPolicyLearnedFieldsPolicyWise(PolicyId);
                    var _Policy = (from p in DataModel.Policies where (p.PolicyId == PolicyId) select p).FirstOrDefault();

                    if (_Policy == null)
                    {
                        return;
                    }

                    if (_Policy.PolicyClientId == null || _Policy.PolicyClientId == Guid.Empty)
                        _Policy.PolicyClientId = _PolicyLearned.ClientID;

                    if (string.IsNullOrEmpty(_Policy.Insured))
                        _Policy.Insured = _PolicyLearned.Insured;

                    if (string.IsNullOrEmpty(_Policy.PolicyNumber))
                        _Policy.PolicyNumber = _PolicyLearned.PolicyNumber;

                    if (_Policy.OriginalEffectiveDate == null)
                        _Policy.OriginalEffectiveDate = _PolicyLearned.Effective;

                    if (string.IsNullOrEmpty(_Policy.Enrolled))
                        _Policy.Enrolled = _PolicyLearned.Enrolled;

                    if (string.IsNullOrEmpty(_Policy.Eligible))
                        _Policy.Eligible = _PolicyLearned.Eligible;

                    if (_Policy.SplitPercentage == null)
                        _Policy.SplitPercentage = Convert.ToDouble(_PolicyLearned.Link2);


                    _Policy.IncomingPaymentTypeId = _PolicyLearned.CompTypeId;

                    if (_Policy.PolicyModeId == null)
                        _Policy.PolicyModeId = _PolicyLearned.PolicyModeId;

                    if (_Policy.CarrierId == null || _Policy.CarrierId == Guid.Empty)
                        _Policy.CarrierId = _PolicyLearned.CarrierId;

                    if (_Policy.CoverageId == null || _Policy.CoverageId == Guid.Empty)
                        _Policy.CoverageId = _PolicyLearned.CoverageId;

                    if (_Policy.TrackFromDate == null)
                        _Policy.TrackFromDate = _PolicyLearned.TrackFrom;

                    if (_Policy.MonthlyPremium == null)
                        _Policy.MonthlyPremium = _PolicyLearned.ModalAvgPremium;


                    DataModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdateLearnedToPolicy ex.InnerException :" + ex.InnerException.ToString(), true);
             //   throw ex;
            }
        }
    }
}
