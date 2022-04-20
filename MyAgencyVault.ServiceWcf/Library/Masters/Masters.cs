using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAgencyVault.BusinessLibrary.Masters;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using System.Runtime.Serialization;   


namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IMasters
    {
        [OperationContract]
         List<FileType> GetSupportedFileTypeList();
        [OperationContract]
        List<BatchDownloadStatus> GetBatchDownloadStatus();
        [OperationContract]
        List<LicenseeStatus> GetLicenseeStatusList();
        [OperationContract]
         List<PayorToolIncomingFieldType> GetPayorToolIncomingFieldList();
        [OperationContract]
        List<PayorToolLearnedlFieldType> GetPayorToolLearnedFieldList();
        [OperationContract]
        List<PolicyIncomingPaymentType> GetPolicyIncomingPaymentTypeList();
        [OperationContract]
        List<PolicyIncomingScheduleType> GetPolicyIncomingScheduleTypeList();
        [OperationContract]
        List<PolicyMode> GetPolicyModeList();
        [OperationContract]
        List<PolicyMode> GetPolicyModeListWithBlankAdded();
        [OperationContract]
        PolicyMode GetPolicyModeByID(int ModeID);
        [OperationContract]
         List<PolicyOutgoingScheduleType> GetPolicyOutgoingScheduleTypeList();
        [OperationContract]
        List<PolicyStatus> GetPolicyStatusList();
        [OperationContract]
        List<PolicyTerminationReason> GetTerminationReasonList();
        [OperationContract]
        List<PolicyTerminationReason> GetTerminationReasonListWithBlankAdded();
        [OperationContract]
        List<Question> GetQuestions();
        [OperationContract]
        List<Region> GetRegionList();
        [OperationContract]
        List<SystemConstant> GetSystemConstants();
        [OperationContract]
        List<Zip> GetZipList();
        [OperationContract]
        Zip GetZip(string zipcode);
        [OperationContract]
        List<PayorToolMaskedFieldType> GetPayorToolMaskedFieldList();
        [OperationContract]
        string GetMaskName(int id);
        [OperationContract]
        string GetSystemConstantKeyValue(string key);
        [OperationContract]
        PolicyDetailMasterData GetPolicyDetailMasterData();
   
        [OperationContract]
        void AddLog(string message);
        
        [OperationContract]
        TempFolderDetails GetTempFolderSetting();
    }


    public partial class MavService :IMasters  
    {

        public TempFolderDetails GetTempFolderSetting()
        {
            TempFolderDetails obj = new TempFolderDetails();
            obj.AllowDelete = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["AllowTempFolderDelete"]);
            obj.FileSizeToBeDeleted = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["TempFolderDeleteSizeInGB"]);
            return obj;
        }

        public void AddLog(string message)
        {
            ActionLogger.Logger.WriteImportPolicyLog(message, true); 
        }

        public PolicyDetailMasterData GetPolicyDetailMasterData()
        {
            return Policy.GetPolicyDetailMasterData();
        }

        public List<FileType> GetSupportedFileTypeList()
        {
           return FileType.GetSupportedFileTypeList();
        }

        public List<LicenseeStatus> GetLicenseeStatusList()
        {
          return LicenseeStatus.GetLicenseeStatusList() ;
        }

        public List<PayorToolIncomingFieldType> GetPayorToolIncomingFieldList()
        {
            return PayorToolIncomingFieldType.GetFieldList();  
        }

        public List<BatchDownloadStatus> GetBatchDownloadStatus()
        {
            return BatchDownloadStatus.GetBatchDownloadStatus();
        
        }

        public List<PayorToolLearnedlFieldType> GetPayorToolLearnedFieldList()
        {
            return PayorToolLearnedlFieldType.GetFieldList();
        }

        public List<PolicyIncomingPaymentType> GetPolicyIncomingPaymentTypeList()
        {
            return PolicyIncomingPaymentType.GetIncomingPaymentTypeList();
        }

        public List<PolicyIncomingScheduleType> GetPolicyIncomingScheduleTypeList()
        {
           return PolicyIncomingScheduleType.GetIncomingScheduleTypeList();
        }

        public List<PolicyMode> GetPolicyModeList()
        {
            return PolicyMode.GetPolicyModeList(); 
        }

        public List<PolicyMode> GetPolicyModeListWithBlankAdded()
        {
            return PolicyMode.GetPolicyModeListWithBlankAdded(); 
        }

        public PolicyMode GetPolicyModeByID(int ModeID)
        {
            return PolicyMode.GetPolicyModeByID(ModeID);
        }

        public List<PolicyOutgoingScheduleType> GetPolicyOutgoingScheduleTypeList()
        {
            return PolicyOutgoingScheduleType.GetOutgoingScheduleTypeList();
        }

        public List<PolicyStatus> GetPolicyStatusList()
        {
            return PolicyStatus.GetPolicyStatusList();
        }

        public List<PolicyTerminationReason> GetTerminationReasonList()
        {
            return PolicyTerminationReason.GetTerminationReasonList();
        }

        public List<PolicyTerminationReason> GetTerminationReasonListWithBlankAdded()
        {
            return PolicyTerminationReason.GetTerminationReasonListWithBlankAdded();
        }

        public List<Question> GetQuestions()
        {
            return Question.GetQuestions() ;
        }

        public List<Region> GetRegionList()
        {
            return Region.GetRegionList();            
        }
        
        public List<SystemConstant> GetSystemConstants()
        {
            return SystemConstant.GetSystemConstants();
        }

        public List<Zip> GetZipList()
        {
          return Zip.GetZipList();
        }

        public List<PayorToolMaskedFieldType> GetPayorToolMaskedFieldList()
        {
            return PayorToolMaskedFieldType.GetFieldList();
        }

        public string GetMaskName(int id)
        {
            return PayorToolMaskedFieldType.GetMaskName(id);
        }
        public string GetSystemConstantKeyValue(string key)
        {
            return SystemConstant.GetKeyValue(key);
        }

        #region IMasters Members


        public Zip GetZip(string zipcode)
        {
            return Zip.GetZip(zipcode);
        }

        #endregion
    }

    [DataContract]
    public class TempFolderDetails
    {

        bool allowDelete;
        [DataMember]
        public bool AllowDelete
        {
            get { return allowDelete; }
            set { allowDelete = value; }
        }

        string fileSizeToBeDeleted;
        [DataMember]
        public string FileSizeToBeDeleted
        {
            get { return fileSizeToBeDeleted; }
            set { fileSizeToBeDeleted = value; }
        }

    }
}