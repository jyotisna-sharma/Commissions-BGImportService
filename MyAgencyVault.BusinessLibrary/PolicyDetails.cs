using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public partial class PolicyDetailPreviousData
    {
        [DataMember]
        public int? PolicyModeId { get; set; }

        [DataMember]
        public DateTime? OriginalEffectiveDate { get; set; }

        [DataMember]
        public DateTime? TrackFromDate { get; set; }

        [DataMember]
        public DateTime? PolicyTermdateDate { get; set; }

        [DataMember]
        public bool IsTrackMissingMonth { get; set; }

        [DataMember]
        public bool IsTrackIncomingPercentage { get; set; }


    }
    [DataContract]
    public partial class PolicyDetailsData
    {
        #region "Datamembers aka public properties."
        [DataMember]
        public Guid PolicyId { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public int? PolicyStatusId { get; set; }
        [DataMember]
        public string PolicyStatusName { get; set; }
        [DataMember]
        public string PolicyType { get; set; }
        [DataMember]
        public Guid? PolicyLicenseeId { get; set; }
        [DataMember]
        public string Insured { get; set; }
        [DataMember]
        public DateTime? OriginalEffectiveDate { get; set; }
        [DataMember]
        public DateTime? TrackFromDate { get; set; }
        [DataMember]
        public int? PolicyModeId { get; set; }
        [DataMember]
        public decimal? ModeAvgPremium { get; set; }
        [DataMember]
        public string SubmittedThrough { get; set; }
        [DataMember]
        public string Enrolled { get; set; }
        [DataMember]
        public string Eligible { get; set; }
        [DataMember]
        public DateTime? PolicyTerminationDate { get; set; }
        [DataMember]
        public int? TerminationReasonId { get; set; }
        [DataMember]
        public bool IsTrackMissingMonth { get; set; }
        [DataMember]
        public bool IsTrackIncomingPercentage { get; set; }
        [DataMember]
        public bool IsTrackPayment { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public Guid OldPolicyId { get; set; }
        [DataMember]
        public Guid? CarrierID { get; set; }
        [DataMember]
        public string CarrierName { get; set; }
        [DataMember]
        public Guid? CoverageId { get; set; }
        [DataMember]
        public string CoverageName { get; set; }
        [DataMember]
        public Guid? ClientId { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public Guid? ReplacedBy { get; set; }
        [DataMember]
        public Guid? DuplicateFrom { get; set; }
        [DataMember]
        public bool? IsIncomingBasicSchedule { get; set; }
        [DataMember]
        public bool? IsOutGoingBasicSchedule { get; set; }
        [DataMember]
        public string PayorNickName { get; set; }//Recently Added
        [DataMember]
        public Guid? PayorId { get; set; }//Recently Added
        [DataMember]
        public string PayorName { get; set; }//Recently Added
        [DataMember]
        public double? SplitPercentage { get; set; }//Recently added

        [DataMember]
        public Int32? Advance { get; set; }//Recently 27/08/2013 added        

        [DataMember]
        public int? IncomingPaymentTypeId { get; set; }

        [DataMember]
        public DateTime? LastFollowUpRuns { get; set; }

        [DataMember]
        public string PolicyIncomingPayType { get; set; } //added by vinod Eric new Enhancement

        [DataMember]
        public DateTime? CreatedOn { get; set; }
        [DataMember]
        public Guid CreatedBy { get; set; }
        [DataMember]
        public bool IsSavedPolicy { get; set; }
        [DataMember]
        public int? CompType { get; set; }
        [DataMember]
        public string CompSchuduleType { get; set; }

        [DataMember]
        public PolicyDetailPreviousData PolicyPreviousData { get; set; }
        [DataMember]
        public Byte[] RowVersion { get; set; }
        [DataMember]
        public PolicyLearnedFieldData LearnedFields { get; set; }
        [DataMember]
        public string RenewalPercentage { get; set; }
        [DataMember]
        public List<PolicyPaymentEntriesPost> policyPaymentEntries { get;set; }
        [DataMember]
        public List<PolicyOutgoingSchedule> PolicyOutGoingSchedules { get; set; }
        [DataMember]
        public List<PolicyNotes> PolicyNotes { get; set; }
        [DataMember]
        public List<PolicyIncomingSchedule> PolicyIncomingSchedules { get; set; }

        //Added new property
        //Seleted product type for policy manager
        [DataMember]
        public string ProductType { get; set; }

        [DataMember]
        public Guid? UserCredentialId { get; set; }

        [DataMember]
        public string AccoutExec { get; set; }

        [DataMember]
        public bool? IsCustomBasicSchedule { get; set; }
        [DataMember]
        public string CustomDateType { get; set; }

        [DataMember]
        public Guid? PrimaryAgent { get; set; }
        [DataMember]
        public bool? IsTieredSchedule { get; set; }
        [DataMember]
        public bool? IsCreatedFromWeb { get; set; }
        [DataMember]
        public bool? IsLastModifiedFromWeb { get; set; }
        [DataMember]
        public DateTime? LastModifiedOn { get; set; }
        [DataMember]
        public Guid? LastModifiedBy { get; set; }
        #endregion
    }

    [DataContract]
    public partial class PolicySetting
    {
        [DataMember]
        public bool IsTrackMissingMonth { get; set; }
        [DataMember]
        public bool IsTrackIncomingPercentage { get; set; }       
       
    }
}
