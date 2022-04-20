using System.Collections.Generic;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;
using System;
using MyAgencyVault.EmailFax;


namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IBrokerCode
    {
        [OperationContract]
        void AddBrokerCode(DisplayBrokerCode objDisplayBrokerCode);

        [OperationContract]
        void UpdateBrokerCode(DisplayBrokerCode objDisplayBrokerCode);

        [OperationContract]
        bool ValidateBrokerCode(string strBrokerCode);

        [OperationContract]
        List<DisplayBrokerCode> LoadBrokerCode(Guid? licID);

        [OperationContract]
        bool DeleteBrokerCode(DisplayBrokerCode objDisplayBrokerCode);

        [OperationContract]
        void NotifyMail(MailData _MailData, string strSubject, string strMailBody);

        [OperationContract]
        void AddImportToolBrokerSettings(ImportToolBrokerSetting objImportToolBrokerSetting);

        [OperationContract]         
        List<ImportToolBrokerSetting> LoadImportToolBrokerSetting();

        [OperationContract]
        List<ImportToolMasterStatementData> LoadImportToolMasterStatementData();
    }

    public partial class MavService : IBrokerCode
    {
        public void AddBrokerCode(DisplayBrokerCode objDisplayBrokerCode)
        {
            Brokercode objBrokercode = new Brokercode();
            objBrokercode.AddBrokerCode(objDisplayBrokerCode);
        }

        public void UpdateBrokerCode(DisplayBrokerCode objDisplayBrokerCode)
        {
            Brokercode objBrokercode = new Brokercode();
            objBrokercode.UpdateBrokerCode(objDisplayBrokerCode);
        }

        public bool ValidateBrokerCode(string strBrokerCode)
        {
            Brokercode objBrokercode = new Brokercode();
            return objBrokercode.ValidateBrokerCode(strBrokerCode);
        }

        public List<DisplayBrokerCode> LoadBrokerCode(Guid? licID)
        {
            Brokercode objBrokercode = new Brokercode();
            return objBrokercode.LoadBrokerCode(licID);
        }

        public bool DeleteBrokerCode(DisplayBrokerCode ObjBrokerCode)
        {
            Brokercode objBrokercode = new Brokercode();
            return objBrokercode.DeleteBrokerCode(ObjBrokerCode);
        }

        public void NotifyMail(MailData _MailData, string strSubject, string strMailBody)
        {
            Brokercode objBrokercode = new Brokercode();
            objBrokercode.NotifyMail(_MailData, strSubject, strMailBody);
        }

        public void AddImportToolBrokerSettings(ImportToolBrokerSetting objImportToolBrokerSetting)
        {
            Brokercode objBrokercode = new Brokercode();
            objBrokercode.AddImportToolBrokerSettings(objImportToolBrokerSetting);
        }

        public List<ImportToolBrokerSetting> LoadImportToolBrokerSetting()
        {
            Brokercode objBrokercode = new Brokercode();           
            return objBrokercode.LoadImportToolBrokerSetting();
        }

        public List<ImportToolMasterStatementData> LoadImportToolMasterStatementData()
        {
            Brokercode objBrokercode = new Brokercode();
            return objBrokercode.LoadImportToolMasterStatementData();
        }     
    }
}