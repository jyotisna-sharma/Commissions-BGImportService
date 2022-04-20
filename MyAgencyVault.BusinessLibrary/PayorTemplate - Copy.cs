using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Data.SqlClient;
using System.Data;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PayorTemplate
    {
        [DataMember]
        public Guid PayorId { get; set; }
        [DataMember]
        public string XlsColumnList { get; set; }
        [DataMember]
        public string SheetName { get; set; }
        [DataMember]
        public int DataStartIndex { get; set; }
        [DataMember]
        public int LastRowSkipCount { get; set; }
        [DataMember]
        public List<MappedField> MappedFields { get; set; }

        public void AddUpdatePayorTemplate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.Template temp = DataModel.Templates.FirstOrDefault(s => s.PayorID == this.PayorId);
                if (temp == null)
                {
                    temp = new DLinq.Template();
                    temp.PayorID = this.PayorId;
                    temp.LastModified = DateTime.Now;
                    temp.ExcelColumnList = this.XlsColumnList;
                    temp.SheetName = this.SheetName;
                    temp.IsDeleted = false;
                    temp.DataStartIndex = this.DataStartIndex;
                    temp.LastRowsToSkip = this.LastRowSkipCount;
                    DataModel.Templates.AddObject(temp);
                }
                else
                {
                    temp.LastModified = DateTime.Now;
                    temp.SheetName = this.SheetName;
                    temp.DataStartIndex = this.DataStartIndex;
                    temp.LastRowsToSkip = this.LastRowSkipCount;

                    if (!string.IsNullOrEmpty(this.XlsColumnList))
                        temp.ExcelColumnList = this.XlsColumnList;

                    if (temp != null && temp.FieldMappings != null && temp.FieldMappings.Count != 0)
                    {
                        for (int index = 0; index < temp.FieldMappings.Count; index++)
                            DataModel.FieldMappings.DeleteObject(temp.FieldMappings.FirstOrDefault());
                    }
                }

                DataModel.SaveChanges();

                foreach (MappedField mappedField in this.MappedFields)
                {
                    DLinq.FieldMapping mapping = new DLinq.FieldMapping { DBFieldName = mappedField.DBField, ExcelFieldNo = int.Parse(mappedField.ExcelField), ExcelFieldName = mappedField.ExcelFieldName, ExcelFieldFormat = mappedField.Format };
                    temp.FieldMappings.Add(mapping);
                }
                DataModel.SaveChanges();
            }
        }

        public static void DaletePayorTemplate(Guid PayorId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.Template temp = DataModel.Templates.FirstOrDefault(s => s.PayorID == PayorId);
                if (temp != null)
                {
                    temp.IsDeleted = true;
                }
            }
        }

        public void DeletePhrase(int intID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.ImportToolPayorPhrase temp = DataModel.ImportToolPayorPhrases.FirstOrDefault(s => s.ID == intID);
                if (temp != null)
                {
                    DataModel.DeleteObject(temp);
                    DataModel.SaveChanges();
                }
            }
        }

        public void UpdatePhrase(int intID, string strPhrase)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.ImportToolPayorPhrase temp = DataModel.ImportToolPayorPhrases.FirstOrDefault(s => s.ID == intID);
                if (temp != null)
                {
                    temp.PayorPhrases = strPhrase;
                    DataModel.SaveChanges();
                }
            }
        }

        public static List<PayorTemplate> getPayorTemplates()
        {
            return new List<PayorTemplate>();
        }

        public static PayorTemplate getPayorTemplate(Guid PayorId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.Template template = DataModel.Templates.FirstOrDefault(s => s.PayorID == PayorId);
                PayorTemplate payorTemplate = new PayorTemplate();

                if (template != null)
                {
                    payorTemplate.XlsColumnList = template.ExcelColumnList;
                    payorTemplate.SheetName = template.SheetName;
                    payorTemplate.DataStartIndex = template.DataStartIndex ?? 1;
                    payorTemplate.LastRowSkipCount = template.LastRowsToSkip ?? 0;
                }

                List<DLinq.FieldMapping> mapFields = DataModel.FieldMappings.Where(s => s.PayorID == PayorId && s.IsDeleted == false).ToList();
                if (mapFields != null)
                {
                    if (payorTemplate.MappedFields == null)
                        payorTemplate.MappedFields = new List<MappedField>();

                    foreach (DLinq.FieldMapping field in mapFields)
                    {
                        MappedField mpField = new MappedField();
                        mpField.DBField = field.DBFieldName;
                        mpField.ExcelField = field.ExcelFieldNo.ToString();
                        mpField.ExcelFieldName = field.ExcelFieldName;
                        mpField.Format = field.ExcelFieldFormat;
                        payorTemplate.MappedFields.Add(mpField);
                    }
                }

                return payorTemplate;
            }
        }

















        //Added on 27/06/2013
        //Add and update payor tool template
        #region"Import tool functionality"

        /// <summary>
        /// CreatedBY:Ankit khandelwal
        /// CreatedOn:26-march-2020
        /// Purpose:Get the Import tool payor template details
        /// </summary>
        /// <param name="selectedPayorId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static TempalateDetails GetImportToolTemplateDetails(Guid selectedPayorId, Guid? templateId)
        {
            ActionLogger.Logger.WriteImportPolicyLog("GetImportToolTemplateDetails: Processing begins with payorId: " + selectedPayorId + " " + templateId, true);
            TempalateDetails templateDetails = new TempalateDetails();
            try
            {
                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GetTemplateDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@payorId", selectedPayorId);
                        cmd.Parameters.AddWithValue("@templateId", templateId);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            templateDetails.FormatType = string.IsNullOrEmpty(reader["FormatType"].ToString()) ? "Tab" : Convert.ToString(reader["FormatType"]);
                            templateDetails.FileType = string.IsNullOrEmpty(reader["FileType"].ToString()) ? "xlsx" : reader["FileType"].ToString().ToLower();
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetImportToolTemplateDetails: Exception occurs while processing with templateId: " + templateId + " " + ex.Message, true);
                throw ex;
            }
            return templateDetails;
        }
        /// <summary>
        /// ModifiedBy:Ankit Khandelwal
        /// ModifiedOn:April05,2020
        /// purpose:Adding a save template call on template Creation
        /// </summary>
        /// <param name="SelectedPayorID"></param>
        /// <param name="SelectedTempID"></param>
        /// <param name="strTempName"></param>
        /// <param name="isDeleted"></param>
        /// <param name="isForceImport"></param>
        /// <param name="strCommandType"></param>
        /// <returns></returns>
        public bool AddUpdateImportToolPayorTemplate(Guid SelectedPayorID, Guid SelectedTempID, string strTempName, bool isDeleted, bool isForceImport, string strCommandType)
        {
            ActionLogger.Logger.WriteImportPolicyLog("AddUpdateImportToolPayorTemplate:processing  begins with SelectedPayorID" + SelectedPayorID + "" + SelectedTempID, true);
            bool bValue = true;
            bool isNewTemplateAdded = false;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.ImportToolPayorTemplate temp = DataModel.ImportToolPayorTemplates.FirstOrDefault(s => s.PayorID == SelectedPayorID && s.TemplateID == SelectedTempID && isDeleted == false);
                    if (strCommandType == "Add")
                    {
                        isNewTemplateAdded = true;
                        //Add 
                        temp = new DLinq.ImportToolPayorTemplate();
                        temp.TemplateID = Guid.NewGuid();
                        temp.PayorID = SelectedPayorID;
                        temp.TemplateName = strTempName;
                        temp.IsDeleted = isDeleted;
                        temp.IsForceImport = isForceImport;
                        DataModel.AddToImportToolPayorTemplates(temp);

                    }
                    else
                    {
                        //Update
                        temp.TemplateName = strTempName;
                        temp.IsForceImport = isForceImport;
                    }
                    DataModel.SaveChanges();
                    bValue = true;
                }
                if (isNewTemplateAdded == true)
                {
                    PayorTemplate.SaveImportToolTemplateDetails(SelectedPayorID, SelectedTempID, "Tab", "xlsx");
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdateImportToolPayorTemplate:Exception occurs while processing with SelectedPayorID" + SelectedPayorID + "" + ex.Message, true);
                bValue = false;
                throw ex;
            }

            return bValue;
        }

        public bool ValidateTemplateName(Guid SelectedPayorID, Guid SelectedTempID, string strTempName, bool isDeleted, bool isForceImport)
        {
            bool bValue = false;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.ImportToolPayorTemplate temp = DataModel.ImportToolPayorTemplates.FirstOrDefault(s => s.PayorID == SelectedPayorID && s.TemplateName.ToLower() == strTempName.ToLower() && s.IsDeleted == false);
                    if (temp != null)
                    {
                        bValue = true;
                    }
                    else
                    {
                        bValue = false;
                    }

                }
            }
            catch
            {
                bValue = false;
            }

            return bValue;

        }

        public List<ImportToolPayorTemplate> GetImportToolTemplateValue(Guid SelectedPayorID, Guid SelectedTempID)
        {
            List<ImportToolPayorTemplate> tempPayor = new List<ImportToolPayorTemplate>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    tempPayor = (from p in DataModel.ImportToolPayorTemplates
                                 where p.PayorID == SelectedPayorID && p.TemplateID == SelectedTempID && p.IsDeleted == false
                                 select new ImportToolPayorTemplate
                                 {
                                     ID = p.ID,
                                     TemplateID = (Guid)p.TemplateID,
                                     TemplateName = p.TemplateName,
                                     IsForceImport = p.IsForceImport,
                                     PayorID = p.PayorID
                                 }).ToList();

                }
            }
            catch
            {
            }
            return tempPayor;
        }

        public List<Tempalate> GetImportToolPayorTemplate(Guid SelectedPayorID)
        {
            List<Tempalate> tempPayor = new List<Tempalate>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    tempPayor = (from p in DataModel.ImportToolPayorTemplates
                                 where p.PayorID == SelectedPayorID && p.IsDeleted == false
                                 select new Tempalate
                                 {
                                     ID = p.ID,
                                     TemplateID = (Guid)p.TemplateID,
                                     TemplateName = p.TemplateName,


                                 }).ToList();

                }
            }
            catch
            {
            }
            return tempPayor;
        }

        public List<Tempalate> GetAllPayorTemplate(Guid? SelectedPayorID)
        {
            List<Tempalate> tempPayor = new List<Tempalate>();


            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {

                    if (SelectedPayorID != null)
                    {
                        tempPayor = (from p in DataModel.ImportToolPayorTemplates
                                     where p.PayorID == SelectedPayorID && p.IsDeleted == false
                                     select new Tempalate
                                     {
                                         ID = p.ID,
                                         TemplateID = (Guid)p.TemplateID,
                                         TemplateName = p.TemplateName,

                                     }).ToList();
                    }
                    else
                    {
                        tempPayor = (from p in DataModel.ImportToolPayorTemplates
                                     where p.IsDeleted == false
                                     select new Tempalate
                                     {
                                         ID = p.ID,
                                         TemplateID = (Guid)p.TemplateID,
                                         TemplateName = p.TemplateName,

                                     }).ToList();


                        Tempalate tempEmpty = new Tempalate();
                        tempEmpty.TemplateName = "--All--";

                        tempPayor.Insert(0, tempEmpty);


                    }

                }
            }
            catch
            {
            }
            return tempPayor;

        }

        public bool deleteImportToolPayorTemplate(Tempalate SelectedPayortempalate)
        {
            bool bValue = false;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {

                    DLinq.ImportToolPayorTemplate temp = DataModel.ImportToolPayorTemplates.FirstOrDefault(s => s.ID == SelectedPayortempalate.ID);
                    if (temp != null)
                    {
                        temp.IsDeleted = true;
                        bValue = true;
                        DataModel.SaveChanges();
                    }
                }
            }
            catch
            {
                bValue = false;
            }

            return bValue;
        }

        public bool AddUpdateImportToolPayorPhrase(ImportToolPayorPhrase objImportToolPayorPhrase)
        {
            bool bValue = false;

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.ImportToolPayorPhrase temp = DataModel.ImportToolPayorPhrases.FirstOrDefault(s => s.PayorPhrases.ToLower() == objImportToolPayorPhrase.PayorPhrases.ToLower() && s.PayorID == objImportToolPayorPhrase.PayorID && s.TemplateID == objImportToolPayorPhrase.TemplateID);
                    if (temp == null)
                    {
                        temp = new DLinq.ImportToolPayorPhrase();

                        temp.TemplateID = objImportToolPayorPhrase.TemplateID;
                        temp.TemplateName = objImportToolPayorPhrase.TemplateName;
                        temp.PayorID = objImportToolPayorPhrase.PayorID;
                        temp.PayorName = objImportToolPayorPhrase.PayorName;
                        temp.FileType = objImportToolPayorPhrase.FileType;
                        temp.FileFormat = objImportToolPayorPhrase.FileFormat;
                        temp.FixedRowLocation = objImportToolPayorPhrase.FixedRowLocation;
                        temp.FixedColLocation = objImportToolPayorPhrase.FixedColLocation;
                        temp.RelativeSearchText = objImportToolPayorPhrase.RelativeSearchText;
                        temp.RelativeRowLocation = objImportToolPayorPhrase.RelativeRowLocation;
                        temp.RelativeColLocation = objImportToolPayorPhrase.RelativeColLocation;
                        temp.PayorPhrases = objImportToolPayorPhrase.PayorPhrases;

                        DataModel.AddToImportToolPayorPhrases(temp);

                        bValue = true;
                    }

                    DataModel.SaveChanges();
                    // Added By Ankit for Update Save FileType and FileFormat Update in  ImportToolTemplate Table
                    PayorTemplate.SaveImportToolTemplateDetails(objImportToolPayorPhrase.PayorID, objImportToolPayorPhrase.TemplateID, objImportToolPayorPhrase.FileFormat, objImportToolPayorPhrase.FileType);
                }

            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdateImportToolPayorPhrase:Exception occurs while processing" + objImportToolPayorPhrase.PayorID + " " + objImportToolPayorPhrase.TemplateID + "" + ex.Message, true);
                bValue = false;
                throw ex;
            }

            return bValue;
        }

        public List<ImportToolPayorPhrase> CheckAvailability(string strPhrase)
        {
            List<ImportToolPayorPhrase> objImportToolPhrase = new List<ImportToolPayorPhrase>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    objImportToolPhrase = (from p in DataModel.ImportToolPayorPhrases
                                           where (p.PayorPhrases == strPhrase)
                                           select new ImportToolPayorPhrase
                                           {
                                               ID = p.ID,
                                               PayorID = (Guid)p.PayorID,
                                               TemplateID = (Guid)p.TemplateID,
                                               PayorPhrases = p.PayorPhrases,
                                               PayorName = p.PayorName,
                                               TemplateName = p.TemplateName
                                           }).ToList();

                }
            }
            catch
            {
            }

            foreach (var item in objImportToolPhrase)
            {
                string strValue = Phrase(item.PayorID, item.TemplateID);
                item.PayorPhrases = strValue;
            }

            return objImportToolPhrase;
        }

        public string ValidatePhraseAvailbility(string strPhrase)
        {
            string strValue = string.Empty;
            try
            {
                List<ImportToolPayorPhrase> objImportToolPhrase = new List<ImportToolPayorPhrase>();
                objImportToolPhrase = CheckAvailability(strPhrase);

                if (objImportToolPhrase.Count > 0)
                {
                    List<ImportToolPayorPhrase> objNewImportToolPhrase = GetAllTemplatePhraseOnTemplate();

                    foreach (var itemPhrase in objNewImportToolPhrase)
                    {
                        strValue = CombinedPhrase(itemPhrase.PayorID, itemPhrase.TemplateID, objNewImportToolPhrase);
                        strValue = strValue + "," + strPhrase;
                        foreach (var item in objImportToolPhrase)
                        {
                            if (CompareString(item.PayorPhrases, strValue))
                            {
                                return strValue;
                            }
                        }
                        strValue = string.Empty;

                    }
                    strValue = string.Empty;
                }
                else
                {
                    return strValue;
                }
            }
            catch
            {
            }
            return strValue;
        }

        private bool CompareString(string Sourc1, string source2)
        {
            bool bValue = false;
            try
            {
                string[] Source1Array = Sourc1.Split(',');
                string[] Source2Array = source2.Split(',');

                if (Source1Array.Length == Source2Array.Length)
                {
                    foreach (string str in Source1Array)
                    {
                        //if the string is present in textbox2's array
                        if (Source2Array.Contains(str))
                        {
                            bValue = true;
                        }
                        else
                        {
                            bValue = false;
                            break;
                        }
                    }
                }
                else
                {
                    bValue = false;
                }

            }
            catch
            {
                bValue = false;
            }

            return bValue;
        }

        public string Phrase(Guid PayorID, Guid TemplateID)
        {
            string strPhrase = string.Empty;

            List<ImportToolPayorPhrase> objImportToolPhrase = new List<ImportToolPayorPhrase>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    objImportToolPhrase = (from p in DataModel.ImportToolPayorPhrases
                                           where (p.PayorID == PayorID && p.TemplateID == TemplateID)
                                           select new ImportToolPayorPhrase
                                           {
                                               PayorPhrases = p.PayorPhrases,

                                           }).ToList();

                }

                foreach (var item in objImportToolPhrase)
                {
                    if (!string.IsNullOrEmpty(item.PayorPhrases))
                    {
                        strPhrase += item.PayorPhrases + ",";
                    }

                }
                strPhrase = strPhrase.Remove(strPhrase.Length - 1, 1);
            }
            catch
            {
            }
            return strPhrase;
        }

        public bool AddUpdateImportToolStatementDataSettings(ImportToolStatementDataSettings objImportToolStatementDataSettings)
        {
            bool bValue = false;

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.ImportToolStatementDataSetting temp = DataModel.ImportToolStatementDataSettings.FirstOrDefault(s => s.PayorID == objImportToolStatementDataSettings.PayorID && s.TemplateID == objImportToolStatementDataSettings.TemplateID && s.MasterStatementDataID == objImportToolStatementDataSettings.MasterStatementDataID);

                    if (temp == null)
                    {
                        temp = new DLinq.ImportToolStatementDataSetting();

                        temp.FixedRowsLocation = objImportToolStatementDataSettings.FixedRowLocation;
                        temp.FixedColsLocation = objImportToolStatementDataSettings.FixedColLocation;
                        temp.RelativeSearch = Convert.ToString(objImportToolStatementDataSettings.RelativeSearch);
                        temp.RelativeRowsLocation = objImportToolStatementDataSettings.RelativeRowLocation;
                        temp.RelativeColsLocation = objImportToolStatementDataSettings.RelativeColLocation;

                        temp.MasterStatementDataID = objImportToolStatementDataSettings.MasterStatementDataID;
                        temp.PayorID = objImportToolStatementDataSettings.PayorID;
                        temp.TemplateID = (Guid)objImportToolStatementDataSettings.TemplateID;
                        temp.IsBlankFieldsIndicatorAvailable = (bool)objImportToolStatementDataSettings.IsBlankFieldsIndicatorAvailable;
                        temp.BlankFieldsIndicator = objImportToolStatementDataSettings.BlankFieldsIndicator;

                        DataModel.AddToImportToolStatementDataSettings(temp);
                        bValue = true;
                    }
                    else
                    {
                        temp.FixedRowsLocation = objImportToolStatementDataSettings.FixedRowLocation;
                        temp.FixedColsLocation = objImportToolStatementDataSettings.FixedColLocation;
                        temp.RelativeSearch = Convert.ToString(objImportToolStatementDataSettings.RelativeSearch);
                        temp.RelativeRowsLocation = objImportToolStatementDataSettings.RelativeRowLocation;
                        temp.RelativeColsLocation = objImportToolStatementDataSettings.RelativeColLocation;
                        temp.IsBlankFieldsIndicatorAvailable = (bool)objImportToolStatementDataSettings.IsBlankFieldsIndicatorAvailable;
                        temp.BlankFieldsIndicator = objImportToolStatementDataSettings.BlankFieldsIndicator;

                        bValue = true;
                    }

                    DataModel.SaveChanges();
                }

            }
            catch
            {
                bValue = false;
            }

            return bValue;
        }

        public List<ImportToolStatementDataSettings> GetAllImportToolStatementDataSettings(Guid GuidPayorID, Guid guidTemplateID)
        {
            var tempList = new List<ImportToolStatementDataSettings>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    tempList = (from p in DataModel.ImportToolStatementDataSettings
                                where (p.PayorID == GuidPayorID && p.TemplateID == guidTemplateID)
                                select new ImportToolStatementDataSettings
                                {
                                    ID = p.ID,
                                    PayorID = p.PayorID,
                                    TemplateID = p.TemplateID,
                                    MasterStatementDataID = p.MasterStatementDataID,
                                    FixedColLocation = p.FixedColsLocation,
                                    FixedRowLocation = p.FixedRowsLocation,
                                    RelativeSearch = p.RelativeSearch,
                                    RelativeRowLocation = p.RelativeRowsLocation,
                                    RelativeColLocation = p.RelativeColsLocation,
                                    IsBlankFieldsIndicatorAvailable = (bool)p.IsBlankFieldsIndicatorAvailable,
                                    BlankFieldsIndicator = p.BlankFieldsIndicator

                                }).ToList();

                }

            }
            catch
            {
            }
            return tempList;
        }

        public List<ImportToolPayorPhrase> GetAllTemplatePhraseOnTemplate()
        {
            List<ImportToolPayorPhrase> objList = new List<ImportToolPayorPhrase>();

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    objList = (from p in DataModel.ImportToolPayorPhrases
                               select new ImportToolPayorPhrase
                               {
                                   ID = p.ID,
                                   PayorID = (Guid)p.PayorID,
                                   PayorName = p.PayorName,
                                   TemplateID = (Guid)p.TemplateID,
                                   TemplateName = p.TemplateName,
                                   PayorPhrases = p.PayorPhrases

                               }).ToList();
                }

            }
            catch
            {
            }
            return objList;
        }

        private string CombinedPhrase(Guid payorID, Guid templateID, List<ImportToolPayorPhrase> objList)
        {
            string strValue = string.Empty;

            List<ImportToolPayorPhrase> objListNewList = new List<ImportToolPayorPhrase>();

            objListNewList = new List<ImportToolPayorPhrase>(objList.Where(s => s.PayorID == payorID && s.TemplateID == templateID).ToList());

            foreach (var item in objListNewList)
            {
                if (!string.IsNullOrEmpty(item.PayorPhrases))
                {
                    strValue += item.PayorPhrases + ",";
                }

            }
            strValue = strValue.Remove(strValue.Length - 1, 1);

            return strValue;
        }

        public List<MaskFieldTypes> AllMaskType()
        {
            List<MaskFieldTypes> objList = new List<MaskFieldTypes>();

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                objList = (from p in DataModel.MasterPayorToolMaskFieldTypes
                           select new MaskFieldTypes
                           {
                               PTMaskFieldTypeId = p.PTMaskFieldTypeId,
                               Name = p.Name,
                               Description = p.Description,
                               Type = (int)p.Type

                           }).ToList();

                return objList;

            }
        }

        public List<TranslatorTypes> AllTranslatorType()
        {
            List<TranslatorTypes> objList = new List<TranslatorTypes>();

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                objList = (from p in DataModel.ImportToolCommTranslators
                           select new TranslatorTypes
                           {
                               TransID = p.TransID,
                               Name = p.Name,
                               Description = p.Description,
                               Type = (int)p.Type

                           }).ToList();

                return objList;

            }
        }

        public List<ImportToolStatementDataSettings> LoadImportToolStatementDataSetting()
        {
            List<ImportToolStatementDataSettings> objImportToolStatementDataSettings = new List<ImportToolStatementDataSettings>();

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    objImportToolStatementDataSettings = (from p in DataModel.ImportToolStatementDataSettings
                                                          select new ImportToolStatementDataSettings
                                                          {
                                                              ID = p.ID,
                                                              PayorID = (Guid)p.PayorID,
                                                              TemplateID = (Guid)p.TemplateID,
                                                              MasterStatementDataID = p.MasterStatementDataID,
                                                              FixedRowLocation = p.FixedRowsLocation,
                                                              FixedColLocation = p.FixedColsLocation,
                                                              RelativeSearch = p.RelativeSearch,
                                                              RelativeRowLocation = p.RelativeRowsLocation,
                                                              RelativeColLocation = p.RelativeColsLocation,
                                                              IsBlankFieldsIndicatorAvailable = p.IsBlankFieldsIndicatorAvailable,
                                                              BlankFieldsIndicator = p.BlankFieldsIndicator
                                                          }).ToList();
                }
            }
            catch
            {
            }

            return objImportToolStatementDataSettings;
        }

        public void AddUpdatePaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.ImportToolSeletedPaymentData temp = DataModel.ImportToolSeletedPaymentDatas.FirstOrDefault(s => s.PayorID == objImportToolSeletedPaymentData.PayorID && s.TemplateID == objImportToolSeletedPaymentData.TemplateID && s.FieldID == objImportToolSeletedPaymentData.FieldID);
                    if (temp == null)
                    {
                        temp = new DLinq.ImportToolSeletedPaymentData();
                        temp.PayorID = objImportToolSeletedPaymentData.PayorID;
                        temp.TemplateID = objImportToolSeletedPaymentData.TemplateID;
                        temp.PayorToolAvailableFieldId = objImportToolSeletedPaymentData.PayorToolAvailableFeildsID;
                        temp.FieldID = objImportToolSeletedPaymentData.FieldID;
                        temp.FieldName = objImportToolSeletedPaymentData.FieldName;
                        DataModel.AddToImportToolSeletedPaymentDatas(temp);
                    }

                    DataModel.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public void DuplicateSelectedPaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData)
        {
            try
            {
                DeleteDuplicatePaymentDataFieldsSettings(objImportToolSeletedPaymentData);
                DeleteDuplicateSeletedPaymentData(objImportToolSeletedPaymentData);
                AddUpdatePaymentData(objImportToolSeletedPaymentData);
            }
            catch
            {
            }

        }

        private static void DeleteDuplicateSeletedPaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.ImportToolSeletedPaymentData temp = DataModel.ImportToolSeletedPaymentDatas.FirstOrDefault(s => s.PayorID == objImportToolSeletedPaymentData.PayorID && s.TemplateID == objImportToolSeletedPaymentData.TemplateID);
                    if (temp != null)
                    {
                        DataModel.DeleteObject(temp);
                        DataModel.SaveChanges();
                    }
                }
            }
            catch
            {
            }
        }

        public void DeleteDuplicatePaymentDataFieldsSettings(ImportToolSeletedPaymentData objImportToolSeletedPaymentData)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.ImportToolPaymentDataFieldsSetting temp = DataModel.ImportToolPaymentDataFieldsSettings.FirstOrDefault(s => s.PayorID == objImportToolSeletedPaymentData.PayorID && s.TemplateID == objImportToolSeletedPaymentData.TemplateID);
                    if (temp != null)
                    {
                        DataModel.DeleteObject(temp);
                        DataModel.SaveChanges();
                    }
                }
            }
            catch
            {
            }
        }

        public void DeletePaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData)
        {
            try
            {
                //Calll to delete payment datas etting
                DeletePaymentDataFieldsSettings(objImportToolSeletedPaymentData);

                DeleteSeletedPaymentData(objImportToolSeletedPaymentData);
            }
            catch
            {
            }
        }

        private static void DeleteSeletedPaymentData(ImportToolSeletedPaymentData objImportToolSeletedPaymentData)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.ImportToolSeletedPaymentData temp = DataModel.ImportToolSeletedPaymentDatas.FirstOrDefault(s => s.PayorID == objImportToolSeletedPaymentData.PayorID && s.TemplateID == objImportToolSeletedPaymentData.TemplateID && s.FieldID == objImportToolSeletedPaymentData.FieldID);
                    if (temp != null)
                    {
                        DataModel.DeleteObject(temp);
                        DataModel.SaveChanges();
                    }
                }
            }
            catch
            {
            }
        }

        public void DeletePaymentDataFieldsSettings(ImportToolSeletedPaymentData objImportToolSeletedPaymentData)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.ImportToolPaymentDataFieldsSetting temp = DataModel.ImportToolPaymentDataFieldsSettings.FirstOrDefault(s => s.PayorID == objImportToolSeletedPaymentData.PayorID && s.TemplateID == objImportToolSeletedPaymentData.TemplateID && s.FieldsID == objImportToolSeletedPaymentData.FieldID);
                    if (temp != null)
                    {
                        DataModel.DeleteObject(temp);
                        DataModel.SaveChanges();
                    }
                }
            }
            catch
            {
            }
        }

        public List<ImportToolSeletedPaymentData> LoadImportToolSeletedPaymentData(Guid PayorID, Guid TemplateID)
        {
            List<ImportToolSeletedPaymentData> objIImportToolSeletedPaymentData = new List<ImportToolSeletedPaymentData>();

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    objIImportToolSeletedPaymentData = (from p in DataModel.ImportToolSeletedPaymentDatas.Where(p => p.PayorID == PayorID && p.TemplateID == TemplateID)
                                                        select new ImportToolSeletedPaymentData
                                                        {
                                                            ID = p.ID,
                                                            PayorID = (Guid)p.PayorID,
                                                            TemplateID = (Guid)p.TemplateID,
                                                            PayorToolAvailableFeildsID = (int)(p.PayorToolAvailableFieldId),
                                                            FieldID = (int)(p.FieldID),
                                                            FieldName = p.FieldName,

                                                        }).ToList();
                }
            }
            catch
            {
            }

            return objIImportToolSeletedPaymentData;
        }

        public void AddUpdatePaymentDataFieldsSetting(ImportToolPaymentDataFieldsSettings objImportToolPaymentDataFieldsSettings)
        {
            try
            {
                //Afert delete data then add 
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.ImportToolPaymentDataFieldsSetting temp = DataModel.ImportToolPaymentDataFieldsSettings.FirstOrDefault(s => s.PayorID == objImportToolPaymentDataFieldsSettings.PayorID && s.TemplateID == objImportToolPaymentDataFieldsSettings.TemplateID && s.PayorToolAvailableFeildsID == objImportToolPaymentDataFieldsSettings.PayorToolAvailableFeildsID);
                    if (temp == null)
                    {
                        temp = new DLinq.ImportToolPaymentDataFieldsSetting();
                        temp.PayorID = objImportToolPaymentDataFieldsSettings.PayorID;
                        temp.TemplateID = objImportToolPaymentDataFieldsSettings.TemplateID;
                        temp.PayorToolAvailableFeildsID = objImportToolPaymentDataFieldsSettings.PayorToolAvailableFeildsID;
                        temp.FieldsID = objImportToolPaymentDataFieldsSettings.FieldsID;
                        temp.FieldsName = objImportToolPaymentDataFieldsSettings.FieldsName;
                        temp.FixedRowLocation = objImportToolPaymentDataFieldsSettings.FixedRowLocation;
                        temp.FixedColLocation = objImportToolPaymentDataFieldsSettings.FixedColLocation;
                        temp.HeaderSearch = objImportToolPaymentDataFieldsSettings.HeaderSearch;
                        temp.RelativeRowLocation = objImportToolPaymentDataFieldsSettings.RelativeRowLocation;
                        temp.RelativeColLocation = objImportToolPaymentDataFieldsSettings.RelativeColLocation;
                        temp.PartOfPrimaryKey = objImportToolPaymentDataFieldsSettings.PartOfPrimaryKey;
                        temp.CalculatedFields = objImportToolPaymentDataFieldsSettings.CalculatedFields;
                        temp.FormulaExpression = objImportToolPaymentDataFieldsSettings.FormulaExpression;
                        temp.PayorToolMaskFieldTypeId = objImportToolPaymentDataFieldsSettings.PayorToolMaskFieldTypeId;

                        temp.StartColLocation = objImportToolPaymentDataFieldsSettings.selectedPaymentDataStartColValue;
                        temp.StartRowLocation = objImportToolPaymentDataFieldsSettings.selectedPaymentDataStartRowValue;
                        temp.EndColLocation = objImportToolPaymentDataFieldsSettings.selectedPaymentDataEndColValue;
                        temp.EndRowLocation = objImportToolPaymentDataFieldsSettings.selectedPaymentDataEndRowValue;
                        //Newly Added
                        temp.TransID = objImportToolPaymentDataFieldsSettings.TransID;
                        temp.TransName = objImportToolPaymentDataFieldsSettings.TransName;
                        temp.DefaultText = objImportToolPaymentDataFieldsSettings.strDefaultText;

                        DataModel.AddToImportToolPaymentDataFieldsSettings(temp);

                    }
                    else
                    {
                        temp.FixedRowLocation = objImportToolPaymentDataFieldsSettings.FixedRowLocation;
                        temp.FixedColLocation = objImportToolPaymentDataFieldsSettings.FixedColLocation;
                        temp.HeaderSearch = objImportToolPaymentDataFieldsSettings.HeaderSearch;
                        temp.RelativeRowLocation = objImportToolPaymentDataFieldsSettings.RelativeRowLocation;
                        temp.RelativeColLocation = objImportToolPaymentDataFieldsSettings.RelativeColLocation;
                        temp.PartOfPrimaryKey = objImportToolPaymentDataFieldsSettings.PartOfPrimaryKey;
                        temp.CalculatedFields = objImportToolPaymentDataFieldsSettings.CalculatedFields;
                        temp.FormulaExpression = objImportToolPaymentDataFieldsSettings.FormulaExpression;
                        temp.PayorToolMaskFieldTypeId = objImportToolPaymentDataFieldsSettings.PayorToolMaskFieldTypeId;

                        temp.StartColLocation = objImportToolPaymentDataFieldsSettings.selectedPaymentDataStartColValue;
                        temp.StartRowLocation = objImportToolPaymentDataFieldsSettings.selectedPaymentDataStartRowValue;
                        temp.EndColLocation = objImportToolPaymentDataFieldsSettings.selectedPaymentDataEndColValue;
                        temp.EndRowLocation = objImportToolPaymentDataFieldsSettings.selectedPaymentDataEndRowValue;
                        //Newly Added
                        temp.TransID = objImportToolPaymentDataFieldsSettings.TransID;
                        temp.TransName = objImportToolPaymentDataFieldsSettings.TransName;

                        //Added default text
                        temp.DefaultText = objImportToolPaymentDataFieldsSettings.strDefaultText;

                    }

                    DataModel.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public List<ImportToolPaymentDataFieldsSettings> LoadPaymentDataFieldsSetting(Guid PayorID, Guid TemplateID)
        {
            List<ImportToolPaymentDataFieldsSettings> objImportToolPaymentDataFieldsSetting = new List<ImportToolPaymentDataFieldsSettings>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    objImportToolPaymentDataFieldsSetting = (from p in DataModel.ImportToolPaymentDataFieldsSettings.Where(p => p.PayorID == PayorID && p.TemplateID == TemplateID)
                                                             select new ImportToolPaymentDataFieldsSettings
                                                             {
                                                                 ID = p.ID,
                                                                 PayorID = (Guid)p.PayorID,
                                                                 TemplateID = (Guid)p.TemplateID,
                                                                 PayorToolAvailableFeildsID = (int)p.PayorToolAvailableFeildsID,
                                                                 FieldsID = (int)p.FieldsID,
                                                                 FieldsName = p.FieldsName,
                                                                 FixedColLocation = p.FixedColLocation,
                                                                 FixedRowLocation = p.FixedRowLocation,
                                                                 HeaderSearch = p.HeaderSearch,
                                                                 RelativeColLocation = p.RelativeColLocation,
                                                                 RelativeRowLocation = p.RelativeRowLocation,
                                                                 PartOfPrimaryKey = (bool)p.PartOfPrimaryKey,
                                                                 CalculatedFields = (bool)p.CalculatedFields,
                                                                 FormulaExpression = p.FormulaExpression,
                                                                 PayorToolMaskFieldTypeId = (int)p.PayorToolMaskFieldTypeId,
                                                                 selectedPaymentDataStartColValue = p.StartColLocation,
                                                                 selectedPaymentDataStartRowValue = p.StartRowLocation,
                                                                 selectedPaymentDataEndColValue = p.EndColLocation,
                                                                 selectedPaymentDataEndRowValue = p.EndRowLocation,
                                                                 TransID = p.TransID,
                                                                 TransName = p.TransName,
                                                                 strDefaultText = p.DefaultText

                                                             }).ToList();



                }
            }
            catch
            {
            }
            return objImportToolPaymentDataFieldsSetting;
        }



        #endregion

        #region new Methods for web version compatibility
        /// <summary>
        /// Createdby:Ankit khandelwal
        /// CreatedOn:April04,2020
        /// Purpose:Save the details of FileType and FielFormat on template Level
        /// </summary>
        /// <param name="selectedPayorId"></param>
        /// <param name="templateId"></param>
        /// <param name="templateData"></param>
        public static void SaveImportToolTemplateDetails(Guid selectedPayorId, Guid? templateId, string FormatType, string FileType)
        {
            ActionLogger.Logger.WriteImportPolicyLog("SaveImportToolTemplateDetails: Processing begins with payorId: " + selectedPayorId + " " + templateId, true);
            try
            {

                using (SqlConnection con = new SqlConnection(DBConnection.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_SaveImportPayorTemplateDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@payorId", selectedPayorId);
                        cmd.Parameters.AddWithValue("@templateId", templateId);
                        cmd.Parameters.AddWithValue("@FormatType", FormatType);
                        cmd.Parameters.AddWithValue("@FileType", FileType);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetImportToolTemplateDetails: Exception occurs while processing with templateId: " + templateId + " " + ex.Message, true);
                throw ex;
            }
        }

    }

    [DataContract]
    public class ImportToolPayorTemplate
    {
        [DataMember]
        public int? ID { get; set; }
        [DataMember]
        public Guid? TemplateID { get; set; }
        [DataMember]
        public Guid? PayorID { get; set; }
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public bool? IsForceImport { get; set; }
    }

    [DataContract]
    public class ImportToolStatementDataSettings
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public Guid? PayorID { get; set; }

        [DataMember]
        public Guid? TemplateID { get; set; }

        [DataMember]
        public int? MasterStatementDataID { get; set; }

        [DataMember]
        public string FixedRowLocation { get; set; }

        [DataMember]
        public string FixedColLocation { get; set; }

        [DataMember]
        public string RelativeSearch { get; set; }

        [DataMember]
        public string RelativeRowLocation { get; set; }

        [DataMember]
        public string RelativeColLocation { get; set; }

        [DataMember]
        public bool? IsBlankFieldsIndicatorAvailable { get; set; }

        [DataMember]
        public string BlankFieldsIndicator { get; set; }

    }

    [DataContract]
    public class MappedField
    {
        [DataMember]
        public string ExcelField { get; set; }
        [DataMember]
        public string DBField { get; set; }
        [DataMember]
        public string ExcelFieldName { get; set; }
        [DataMember]
        public string Format { get; set; }
    }

    [DataContract]
    public class ImportToolPayorPhrase
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public Guid PayorID { get; set; }

        [DataMember]
        public string PayorName { get; set; }

        [DataMember]
        public Guid TemplateID { get; set; }

        [DataMember]
        public string TemplateName { get; set; }

        [DataMember]
        public string FileType { get; set; }

        [DataMember]
        public string FileFormat { get; set; }

        [DataMember]
        public string FixedRowLocation { get; set; }

        [DataMember]
        public string FixedColLocation { get; set; }

        [DataMember]
        public string RelativeSearchText { get; set; }

        [DataMember]
        public string RelativeRowLocation { get; set; }

        [DataMember]
        public string RelativeColLocation { get; set; }

        [DataMember]
        public string PayorPhrases { get; set; }

        [DataMember]
        public int? intPhraseCount { get; set; }

    }

    [DataContract]
    public enum AvailableStatementData
    {
        [EnumMember]
        CheckAmt = 1,

        [EnumMember]
        BalforAdj = 2,

        [EnumMember]
        NetCheck = 3,

        [EnumMember]
        StatementDate = 4,

        [EnumMember]
        StartData = 5,

        [EnumMember]
        EndData = 6

    }

    [DataContract]
    public class MaskFieldTypes
    {
        [DataMember]
        public int PTMaskFieldTypeId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Type { get; set; }
    }

    [DataContract]
    public class TranslatorTypes
    {
        [DataMember]
        public int TransID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Type { get; set; }
    }

    [DataContract]
    public class ImportToolPaymentDataFieldsSettings
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public Guid PayorID { get; set; }

        [DataMember]
        public Guid TemplateID { get; set; }

        [DataMember]
        public int PayorToolAvailableFeildsID { get; set; }

        [DataMember]
        public int FieldsID { get; set; }

        [DataMember]
        public string FieldsName { get; set; }

        [DataMember]
        public string FixedRowLocation { get; set; }

        [DataMember]
        public string FixedColLocation { get; set; }

        [DataMember]
        public string HeaderSearch { get; set; }

        [DataMember]
        public string RelativeRowLocation { get; set; }

        [DataMember]
        public string RelativeColLocation { get; set; }

        [DataMember]
        public bool PartOfPrimaryKey { get; set; }

        [DataMember]
        public bool CalculatedFields { get; set; }

        [DataMember]
        public string FormulaExpression { get; set; }

        [DataMember]
        public int PayorToolMaskFieldTypeId { get; set; }

        [DataMember]
        public string selectedPaymentDataStartColValue { get; set; }

        [DataMember]
        public string selectedPaymentDataStartRowValue { get; set; }

        [DataMember]
        public string selectedPaymentDataEndColValue { get; set; }

        [DataMember]
        public string selectedPaymentDataEndRowValue { get; set; }

        [DataMember]
        public int? TransID { get; set; }

        [DataMember]
        public string TransName { get; set; }

        [DataMember]
        public string strDefaultText { get; set; }

    }

    [DataContract]
    public class ImportToolSeletedPaymentData
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public Guid PayorID { get; set; }

        [DataMember]
        public Guid TemplateID { get; set; }

        [DataMember]
        public int PayorToolAvailableFeildsID { get; set; }

        [DataMember]
        public int FieldID { get; set; }

        [DataMember]
        public string FieldName { get; set; }

    }

    #endregion
}

