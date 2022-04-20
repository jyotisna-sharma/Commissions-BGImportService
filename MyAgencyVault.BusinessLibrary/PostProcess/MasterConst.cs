using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class MessageConst
    {
        [DataMember]
        public const string LockErrorMessage = "Policy is in use";
    }
    [DataContract]
    public enum MasterFollowUpProcCalledModule
    {
        Policy,
        PostProcess,
        CommissiondashBoard,
        LinkPaymentPolicy,
        DEU,
        FollowUpmgr,
        OWN,

    }

    public enum MasterSystemConst
    {
        WebDevPath=1,
        CardPayeeDate,
        CheckPayeeDate,
        MailServerName,
        MailServerEMail,
        MailServerUserName,
        MailServerPassword,
        MailServerPortNo,
        AlertCommissionDeptMailId,
        TruncationError,
        ReportServerUrl,
        CommDeptMailId,
        CommDeptFax,
        CommDeptPhoneNumber,
        MailLogoPath,
        FaxAgency,
        FaxPayor,
        MailScanFaxEmail,
        MailScanErrorEmail,
        MailScanErrorEmailPassword,
        NextFollowUpRunDaysCount,
        IsFollowUpRun,
    }
    public enum FirstYrRenewalYr
    {
        FirstYear,
        Renewal,
        None,
    }
    public enum FollowUpSearchFilterStatus
    {
        Open = 1,
        PaymentPending = 6,
        Closed = 2,
        All = 0,

    }
    public enum MasterPolicyMode
    {
        Monthly = 0,
        Quarterly = 1,
        HalfYearly = 2,
        Annually = 3,
        OneTime = 4,
        Random = 5,
    }
    /// <summary>
    /// For NotAnyIssue do nothing
    /// </summary>
    public enum FollowUpProcessType
    {
        MissingPaymentType,
        MissingScheduleType,

    }
    public enum FollowUpIssueCategory
    {
        MissFirst = 1,
        MissInv = 2,
        VarSchedule = 3,
        VarCompDue = 4,
        NotAnyIssue = 5,
    }
    public enum FollowUpIssueReason
    {
        Pending = 1,
        NoPremium = 2,
        PolicyCredit,
        PolicyTerm,
        PayNotDue,
        NonCommiss,
        BrkLicExp,
        BrkNotAppt,
        BrkNotBOR,
        BrkLostBOR,
        PaymentDue,
        Other,
        BadTrmDate,
    }
    public enum FollowUpIssueStatus
    {
        Open = 1,
        Closed,
        BrkInfoReq,
        CarrierReview,
        LicenseExp,
        PaymentDue,
    }

    public enum FollowUpResult
    {
        Resolved_Brk = 1,//Resolved by Broker
        Resolved_Carr,
        Resolved_CD,//Payment Received
        Pending,
    }
    [DataContract]
    public enum _PolicyStatus
    {
        [EnumMember]
        Active = 0,
        [EnumMember]
        Terminated = 1,
        [EnumMember]
        Pending = 2,
        [EnumMember]
        Any = 3,
        [EnumMember]
        Delete = 4,
    }

    public enum PaymentMode
    {
        Monthly = 1,
        Quarterly = 3,
        HalfYearly = 6,
        Yearly = 12,
        OneTime,
        Random,


    }

    public enum PostStatus
    {
        NoLink = 1,
        NoAgency = 2,
        Linked_Agency = 3,
        Ag_NoSplits = 4,
        Ag_NoMSplits = 5,
        Linked_NoAg = 6,
    }
    public enum PayorToolIncomingDEUFields
    {
        None = 1,
        PolicyNumber = 2,
        Insured = 3,
        OriginalEffDate = 4,
        InvoiceDate = 5,
        PaymentReceived = 6,
        BilledAmount = 7,
        CommissionPercentage = 8,
        CommissionPaid = 9,

        Renewal = 10,
        Enrolled = 11,
        Eligible = 12,
        Link1 = 13,
        SplitPer = 14,
        PolicyMode = 15,
        Carrier = 16,
        Coverage = 17,
        PayorSysID = 18,
        CompScheduleType = 19,
        CompType = 20,
        Client = 21,
        NumberOfUnits = 22,
        DollerPerUnit = 23,
        Fee = 24,
        Bonus = 25,
        CommissionPaid1 = 26,
        CommissionPaid2 = 27,
        CommissionTotal = 28
    }
    public enum PayorToolLearnedFields
    {
        Insured = 1,
        PolicyNumber = 2,
        Effective = 3,
        InvoiceDate = 4,
        Renewal = 5,
        Carrier = 6,
        Product = 7,
        ModelAvgPremium = 8,
        PolicyMode = 9,
        Enrolled = 10,
        Eligible = 11,
        Link1 = 12,
        SplitPer = 13,
        Client = 14
    }
    public enum MasterAdvanceScheduleType
    {
        PercentageofPremium_scale = 1,
        PercentageofPremium_target = 2,
        PerHeadFee_scale = 3,
        PerHeadFee_target = 4,
        FlatDollar_modal = 5,
    }
    public enum MasterBasicIncomingSchedule
    {

        PercentageOfPremium = 1,
        PerHead = 2,

    }
    public enum MasterBasicOutgoinSchedule
    {
        PercentageOfPremium = 1,
        PercentageOfCommission = 2,
    }
    public enum PolicySchedule
    {
        Basic,
        Advance,
        None,
    }
    public enum MasterPayorTypes
    {
        SingleCarrier = 0,
        GeneralAgent = 1,

    }
    public enum TypeOFIncomingPolicySchedule
    {
        PercentageOfPremium = 1,
        PerHead = 2,
        Advance = 3,

    }
    [DataContract]
    public enum PostEntryProcess
    {
        [EnumMember]
        FirstPost = 1,
        [EnumMember]
        RePost = 2,
        [EnumMember]
        Delete = 3,
    }
    [DataContract]
    public enum PostCompleteStatusEnum
    {
        [EnumMember]
        NotStarted = 0,
        [EnumMember]
        InProgress = 1,
        [EnumMember]
        Unsuccessful = 2,
        [EnumMember]
        Successful = 3,
    }
    public enum MasterIncoimgPaymentType
    {
        other = 0,
        commission = 1,
        overide = 2,
        bonus = 3,
        fee = 4,
    }
    public enum FollowUpRunModules
    {
        PaymentEntered = 1,
        PaymentDeleted = 2,
        IncomingScheduleChange = 3,
        PolicyDetailChange = 4,
        ResolveIssue = 5,
    }
}
