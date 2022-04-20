using System;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;
using System.Collections.Generic;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPayorTemplate
    {
        [OperationContract]
        void AddUpdateTemplate(PayorTemplate template);

        [OperationContract]
        void DeleteTemplate(Guid PayorId);

        [OperationContract]
        List<PayorTemplate> getPayorTemplates();

        [OperationContract]
        PayorTemplate getPayorTemplate(Guid PayorId);

        [OperationContract]
        bool AddUpdateImportToolPayorTemplate(Guid SelectedPayorID, Guid SelectedtempID, string strTemName, bool isDeleted, bool isForceImport, string strCommandType);

        [OperationContract]
        bool ValidateTemplateName(Guid SelectedPayorID, Guid SelectedTempID, string strTempName, bool isDeleted, bool isForceImport);


        [OperationContract]
        List<Tempalate> GetImportToolPayorTemplate(Guid SelectedPayorID);

        [OperationContract]
        bool deleteImportToolPayorTemplate(Tempalate SelectedPayortempalate);

        [OperationContract]
        bool AddUpdateImportToolPayorPhrase(ImportToolPayorPhrase objImportToolPayorPhrase);

        [OperationContract]
        List<ImportToolPayorPhrase> CheckAvailability(string strPhrase);

        [OperationContract]
        bool AddUpdateImportToolStatementDataSettings(ImportToolStatementDataSettings objImportToolStatementDataSettings);

        [OperationContract]
        List<ImportToolStatementDataSettings> GetAllImportToolStatementDataSettings(Guid PayorID, Guid templateID);

        [OperationContract]
        string ValidatePhraseAvailbility(string strPhrase);

        [OperationContract]
        List<MaskFieldTypes> AllMaskType();

        //Add to fetch translator type
        [OperationContract]
        List<TranslatorTypes> AllTranslatorType();

        [OperationContract]
        void AddUpdatePaymentDataFieldsSetting(ImportToolPaymentDataFieldsSettings objImportToolPaymentDataFieldsSettings);

        [OperationContract]
        List<ImportToolStatementDataSettings> LoadImportToolStatementDataSetting();

        [OperationContract]
        void AddUpdatePaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData);

        [OperationContract]
        void DeletePaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData);

        [OperationContract]
        List<ImportToolSeletedPaymentData> LoadImportToolSeletedPaymentData(Guid PayorID, Guid TemplateID);

        [OperationContract]
        List<ImportToolPaymentDataFieldsSettings> LoadPaymentDataFieldsSetting(Guid PayorID, Guid TemplateID);

        [OperationContract]
        void DuplicateSelectedPaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData);

        [OperationContract]
        List<ImportToolPayorPhrase> GetAllTemplatePhraseOnTemplate();

        [OperationContract]
        List<Tempalate> GetAllPayorTemplate(Guid? SelectedPayorID);

        [OperationContract]
        void DeletePhrase(int intID);

        [OperationContract]
        void UpdatePhrase(int intID, string Phrase);


    }

    public partial class MavService : IPayorTemplate
    {
        public void AddUpdateTemplate(PayorTemplate template)
        {
            template.AddUpdatePayorTemplate();
        }

        public void DeleteTemplate(Guid PayorId)
        {
            PayorTemplate.DaletePayorTemplate(PayorId);
        }

        public List<PayorTemplate> getPayorTemplates()
        {
            return PayorTemplate.getPayorTemplates();
        }

        public PayorTemplate getPayorTemplate(Guid PayorId)
        {
            return PayorTemplate.getPayorTemplate(PayorId);
        }

        public bool AddUpdateImportToolPayorTemplate(Guid SelectedPayorID,Guid SelectedtempID, string strTemName, bool isDeleted, bool isForceImport,string strCommandType)
        {
            PayorTemplate objPayorTemplate = new PayorTemplate();
            return objPayorTemplate.AddUpdateImportToolPayorTemplate(SelectedPayorID, SelectedtempID, strTemName, isDeleted,  isForceImport, strCommandType);
        }

        public bool ValidateTemplateName(Guid SelectedPayorID, Guid SelectedTempID, string strTempName, bool isDeleted, bool isForceImport)
        {
            PayorTemplate objPayorTemplate = new PayorTemplate();
            return objPayorTemplate.ValidateTemplateName(SelectedPayorID, SelectedTempID, strTempName, isDeleted, isForceImport);
        }

        public List<Tempalate> GetImportToolPayorTemplate(Guid SelectedPayorID)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.GetImportToolPayorTemplate(SelectedPayorID);
        }

        public bool deleteImportToolPayorTemplate(Tempalate SelectedPayortempalate)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.deleteImportToolPayorTemplate(SelectedPayortempalate);
        }

        public bool AddUpdateImportToolPayorPhrase(ImportToolPayorPhrase objImportToolPayorPhrase)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.AddUpdateImportToolPayorPhrase(objImportToolPayorPhrase);
        }

        public List<ImportToolPayorPhrase> CheckAvailability(string strPhrase)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.CheckAvailability(strPhrase);
        }

        public bool AddUpdateImportToolStatementDataSettings(ImportToolStatementDataSettings objImportToolStatementDataSettings)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.AddUpdateImportToolStatementDataSettings(objImportToolStatementDataSettings);
        }

        public List<ImportToolStatementDataSettings> GetAllImportToolStatementDataSettings(Guid PayorID, Guid templateID)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.GetAllImportToolStatementDataSettings(PayorID, templateID);
        }

        public string ValidatePhraseAvailbility(string strPhrase)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.ValidatePhraseAvailbility(strPhrase);
        }

        public List<MaskFieldTypes> AllMaskType()
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.AllMaskType();
        }

        public List<TranslatorTypes> AllTranslatorType()
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.AllTranslatorType();
        }

        public void AddUpdatePaymentDataFieldsSetting(ImportToolPaymentDataFieldsSettings objImportToolPaymentDataFieldsSettings)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            objPayortemplate.AddUpdatePaymentDataFieldsSetting(objImportToolPaymentDataFieldsSettings);
        }

        public List<ImportToolStatementDataSettings> LoadImportToolStatementDataSetting()
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.LoadImportToolStatementDataSetting();
        }

        public void AddUpdatePaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            objPayortemplate.AddUpdatePaymentData(objImportToolSeletedPaymentData);
        }

        public void DeletePaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            objPayortemplate.DeletePaymentData(objImportToolSeletedPaymentData);
        }

        public List<ImportToolSeletedPaymentData> LoadImportToolSeletedPaymentData(Guid PayorID, Guid TemplateID)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.LoadImportToolSeletedPaymentData(PayorID, TemplateID);
        }

        public List<ImportToolPaymentDataFieldsSettings> LoadPaymentDataFieldsSetting(Guid PayorID, Guid TemplateID)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.LoadPaymentDataFieldsSetting(PayorID, TemplateID);
        }
        
        public void DuplicateSelectedPaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            objPayortemplate.DuplicateSelectedPaymentData(objImportToolSeletedPaymentData);
        }

        public List<ImportToolPayorPhrase> GetAllTemplatePhraseOnTemplate()
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.GetAllTemplatePhraseOnTemplate();
        }

        public List<Tempalate> GetAllPayorTemplate(Guid? SelectedPayorID)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            return objPayortemplate.GetAllPayorTemplate(SelectedPayorID);
        }
        public void DeletePhrase(int intID)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            objPayortemplate.DeletePhrase(intID);
        }

        public void UpdatePhrase(int intID, string strPhrase)
        {
            PayorTemplate objPayortemplate = new PayorTemplate();
            objPayortemplate.UpdatePhrase(intID, strPhrase);
        }
    }
}