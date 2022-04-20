using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Collections.ObjectModel;
using System.IO;
namespace MyAgencyVault.BusinessLibrary
{

    #region"add template"
    public class Tempalate
    {
        [DataMember]
        public int? ID { get; set; }
        [DataMember]
        public Guid? TemplateID { get; set; }
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }

    }
    #endregion

    [DataContract]
    public class PayorTool
    {
        #region IEditable<PayorTool> Members
        //public static void AddUpdate(PayorTool payorTool)
        //{
        //    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //    {
        //        DLinq.PayorTool ObjPayor = null;
        //        ObjPayor = (from p in DataModel.PayorTools
        //                    where p.PayorToolId == payorTool.PayorToolId
        //                    select p).FirstOrDefault();

        //        if (ObjPayor != null)
        //        {
        //            if (!string.IsNullOrEmpty(payorTool.StatementImageFilePath))
        //            {
        //                if (!payorTool.StatementImageFilePath.EndsWith(".tmp"))
        //                {
        //                    ObjPayor.StatementImageFile = payorTool.WebDevStatementImageFilePath;
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(payorTool.ChequeImageFilePath))
        //            {
        //                if (!payorTool.ChequeImageFilePath.EndsWith(".tmp"))
        //                {
        //                    ObjPayor.ChequeImageFile = payorTool.WebDevChequeImageFilePath;
        //                }
        //            }

        //        }
        //        else
        //        {
        //            ObjPayor = new DLinq.PayorTool();
        //            ObjPayor.StatementImageFile = payorTool.WebDevStatementImageFilePath;
        //            ObjPayor.ChequeImageFile = payorTool.WebDevChequeImageFilePath;
        //            ObjPayor.PayorToolId = payorTool.PayorToolId;
        //            ObjPayor.PayorId = payorTool.PayorID;
        //            DataModel.AddToPayorTools(ObjPayor);

        //        }
        //        DataModel.SaveChanges();
        //        AddUpdatePayorToolFields(payorTool);
        //    }
        //}
        ///// <summary>
        /// Add/Update payor Tool fields
        /// </summary>
        /// 
        public static void AddUpdate(PayorTool payorTool)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorTool ObjPayor = null;

                if (payorTool.TemplateID == null)
                {
                    ObjPayor = (from p in DataModel.PayorTools
                                where p.PayorId == payorTool.PayorID && p.TemplateID == null
                                select p).FirstOrDefault();
                }
                else
                {
                    ObjPayor = (from p in DataModel.PayorTools
                                where p.PayorId == payorTool.PayorID && p.TemplateID == payorTool.TemplateID
                                select p).FirstOrDefault();
                }

                if (ObjPayor != null)
                {
                    if (!string.IsNullOrEmpty(payorTool.StatementImageFilePath))
                    {
                        if (!payorTool.StatementImageFilePath.EndsWith(".tmp"))
                        {
                            ObjPayor.StatementImageFile = payorTool.WebDevStatementImageFilePath;
                        }
                    }

                    if (!string.IsNullOrEmpty(payorTool.ChequeImageFilePath))
                    {
                        if (!payorTool.ChequeImageFilePath.EndsWith(".tmp"))
                        {
                            ObjPayor.ChequeImageFile = payorTool.WebDevChequeImageFilePath;
                        }
                    }

                }
                else
                {
                    ObjPayor = new DLinq.PayorTool();
                    ObjPayor.StatementImageFile = payorTool.WebDevStatementImageFilePath;
                    ObjPayor.ChequeImageFile = payorTool.WebDevChequeImageFilePath;
                    ObjPayor.PayorToolId = payorTool.PayorToolId;
                    ObjPayor.PayorId = payorTool.PayorID;
                    //added New
                    ObjPayor.TemplateID = payorTool.TemplateID;
                    DataModel.AddToPayorTools(ObjPayor);

                }
                DataModel.SaveChanges();
                AddUpdatePayorToolFields(payorTool);
            }
        }

        #region "duplicate"
        //duplicate payor tools
        public static bool UpdateDulicatePayorTool(Guid SourcePayorID, Guid? SourceTempID, Guid DestinationPayorID, Guid? DestiTempID)
        {
            bool bValue = true;

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.PayorTool ObjPayorSource = null;
                    DLinq.PayorTool ObjPayorTarget = null;

                    if (SourceTempID == null)
                    {
                        ObjPayorSource = (from p in DataModel.PayorTools
                                    where p.PayorId == SourcePayorID && p.TemplateID == null
                                    select p).FirstOrDefault();
                    }
                    else
                    {
                        ObjPayorSource = (from p in DataModel.PayorTools
                                    where p.PayorId == SourcePayorID && p.TemplateID == SourceTempID
                                    select p).FirstOrDefault();
                    }
                                                           

                    if (DestiTempID == null)
                    {
                        ObjPayorTarget = (from p in DataModel.PayorTools
                                          where p.PayorId == DestinationPayorID && p.TemplateID == null
                                          select p).FirstOrDefault();
                    }
                    else
                    {
                        ObjPayorTarget = (from p in DataModel.PayorTools
                                          where p.PayorId == DestinationPayorID && p.TemplateID == DestiTempID
                                          select p).FirstOrDefault();
                    }



                    if (ObjPayorTarget != null)
                    {
                        PayorTool SourcePayorTool = GetPayorTool(SourcePayorID, SourceTempID);

                        PayorTool TargetPayorTool = GetPayorTool(DestinationPayorID, DestiTempID);
                        //Update Image
                        ObjPayorTarget.StatementImageFile = SourcePayorTool.WebDevStatementImageFilePath;
                        ObjPayorTarget.ChequeImageFile = SourcePayorTool.WebDevChequeImageFilePath;
                        DataModel.SaveChanges();
                        UpdatePayorToolFields(SourcePayorTool, TargetPayorTool);

                        return bValue;

                    }
                    else
                    {
                        PayorTool SourcePayorTool = GetPayorTool(SourcePayorID, SourceTempID);
                        DLinq.PayorTool ObjPayor = null;
                        //Update Image
                        ObjPayor = new DLinq.PayorTool();
                        ObjPayor.StatementImageFile = SourcePayorTool.WebDevStatementImageFilePath;
                        ObjPayor.ChequeImageFile = SourcePayorTool.WebDevChequeImageFilePath;
                        ObjPayor.PayorToolId =Guid.NewGuid();
                        ObjPayor.PayorId =DestinationPayorID;                       
                        ObjPayor.TemplateID = DestiTempID;

                        DataModel.AddToPayorTools(ObjPayor);

                        DataModel.SaveChanges();
                                                
                        PayorTool TargetPayorTool = GetPayorTool(DestinationPayorID, DestiTempID);
                        if (TargetPayorTool == null)
                        {
                            SourcePayorTool.PayorToolId = DestinationPayorID;
                            SourcePayorTool.TemplateID = DestiTempID;
                            AddUpdatePayorToolFields(SourcePayorTool);
                        }
                        else
                        {
                            UpdatePayorToolFields(SourcePayorTool, TargetPayorTool);
                        }

                        
                    }

                }
            }
            catch
            {
                 bValue = false;
            }
            return bValue;
        }
        //duplicate payor tools
        public static PayorTool GetPayorTool(Guid PayorID,Guid? templatID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                PayorTool pTool = null;

                if (templatID == null)
                {
                    pTool = (from p in DataModel.PayorTools
                             where p.Payor.PayorId == PayorID && p.TemplateID == null && p.IsDeleted == false
                             select new PayorTool
                             {
                                 ChequeImageFilePath = p.ChequeImageFile,
                                 StatementImageFilePath = p.StatementImageFile,
                                 WebDevChequeImageFilePath = p.ChequeImageFile,
                                 WebDevStatementImageFilePath = p.StatementImageFile,
                                 PayorToolId = p.PayorToolId,
                                 PayorID = p.Payor.PayorId,
                                TemplateID=p.TemplateID,
                             }).FirstOrDefault();
                }
                else
                {
                    pTool = (from p in DataModel.PayorTools
                             where p.Payor.PayorId == PayorID && p.TemplateID == templatID && p.IsDeleted == false
                             select new PayorTool
                             {
                                 ChequeImageFilePath = p.ChequeImageFile,
                                 StatementImageFilePath = p.StatementImageFile,
                                 WebDevChequeImageFilePath = p.ChequeImageFile,
                                 WebDevStatementImageFilePath = p.StatementImageFile,
                                 PayorToolId = p.PayorToolId,
                                 PayorID = p.Payor.PayorId,  
                                 TemplateID=p.TemplateID,
                             }).FirstOrDefault();
                }

                if (pTool != null)
                {
                    pTool.ToolFields =
                     (from p1 in DataModel.PayorToolFields
                      where p1.PayorTool.PayorToolId == pTool.PayorToolId && p1.IsDeleted == false
                      select new PayorToolField
                      {
                          AllignedDirection = p1.AllignedDirection ?? "Left",
                          DefaultValue = p1.DefaultNumeric,
                          EquivalentIncomingField = p1.EquivalentIncomingField,
                          EquivalentLearnedField = p1.EquivalentLearnedField,
                          EquivalentDeuField = p1.EquivalentDeuField,
                          ControlHeight = p1.FieldHeight ?? 0,
                          FieldOrder = p1.FieldOrder ?? 0,
                          ControlX = p1.FieldPositionX.Value,
                          ControlY = p1.FieldPositionY.Value,
                          FieldStatusValue = p1.FieldStatus,
                          ControlWidth = p1.FieldWidth ?? 0,
                          FormulaId = p1.FormulaId,
                          HelpText = p1.HelpText,
                          IsCalculatedField = p1.IsCalculatedField,
                          IsZeroorBlankAllowed = p1.IsOorBlankAllowed,
                          IsOverrideOfCalcAllowed = p1.IsOverrideOfCalcAllowed,
                          IsPopulateIfLinked = p1.IsPopulatedIfLinked,
                          IsPartOfPrimaryKey = p1.IsPartOfPrimary,
                          IsTabbedToNextFieldIfLinked = p1.IsTabbedToNextFieldIfLinked,
                          LabelOnField = p1.LabelOnImage,
                          MaskFieldTypeId = p1.MasterPayorToolMaskFieldType.PTMaskFieldTypeId == null ? 0 : p1.MasterPayorToolMaskFieldType.PTMaskFieldTypeId,
                          MaskFieldType = p1.MasterPayorToolMaskFieldType.Type.Value,
                          MaskText = p1.MasterPayorToolMaskFieldType.Name,
                          PTAvailableFieldId = p1.MasterPayorToolAvailableField.PTAvailableFieldId == null ? 0 : p1.MasterPayorToolAvailableField.PTAvailableFieldId,
                          AvailableFieldName = p1.MasterPayorToolAvailableField.PTAvailableFieldId == null ? "" : p1.MasterPayorToolAvailableField.Name,
                          PayorFieldID = p1.PayorToolFieldId,
                          PayorToolId = p1.PayorTool.PayorToolId
                      }).ToList();

                    foreach (PayorToolField field in pTool.ToolFields)
                    {
                        if (field.FormulaId != null)
                        {
                            DLinq.Formula formula = DataModel.Formulas.FirstOrDefault(s => s.FormulaId == field.FormulaId);
                            if (formula != null)
                            {
                                field.CalculationFormula = new Formula();
                                field.CalculationFormula.FormulaExpression = formula.FormulaExpression;
                                field.CalculationFormula.FormulaTtitle = formula.FormulaTtitle;
                                field.CalculationFormula.FormulaID = formula.FormulaId;
                            }
                        }
                    }
                }

                return pTool;
            }
        }
        //duplicate payor tools
        private static void UpdatePayorToolFields(PayorTool SourcepayorTool, PayorTool TargetpayorTool)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.PayorToolField> payorToolFields = DataModel.PayorToolFields.Where(s => s.PayorToolId == TargetpayorTool.PayorToolId && s.IsDeleted == false).ToList();
                if (payorToolFields != null && payorToolFields.Count != 0)
                {
                    foreach (DLinq.PayorToolField field in payorToolFields)                       
                        PayorToolField.DeletePayorToolFiledID(field.PayorToolFieldId);
                }

                foreach (PayorToolField Field in SourcepayorTool.ToolFields)
                {
                    Field.PayorToolId = TargetpayorTool.PayorToolId;
                    Field.TemplateID = TargetpayorTool.TemplateID;                   
                    PayorToolField.UpdateDuplicatePayor(Field, TargetpayorTool.PayorToolId, TargetpayorTool.TemplateID);
                }
            }
        }
        //duplicate payor tools
        public static bool IsAvailablePayorTempalate(Guid SourcePayorID, Guid? SourceTempID, Guid DestinationPayorID, Guid? DestiTempID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorTool ObjPayorTemplate = null;

                if (DestiTempID == null)
                {
                    ObjPayorTemplate = (from p in DataModel.PayorTools
                                        where p.PayorId == DestinationPayorID && p.TemplateID == null
                                        select p).FirstOrDefault();
                }
                else
                {
                    ObjPayorTemplate = (from p in DataModel.PayorTools
                                        where p.PayorId == DestinationPayorID && p.TemplateID == DestiTempID
                                        select p).FirstOrDefault();
                }

                if (ObjPayorTemplate != null)
                {
                    return true;
                }
                else
                    return false;


            }
        }

        #endregion

        private static void AddUpdatePayorToolFields(PayorTool payorTool)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<DLinq.PayorToolField> payorToolFields = DataModel.PayorToolFields.Where(s => s.PayorToolId == payorTool.PayorToolId && s.IsDeleted == false).ToList();
                if (payorToolFields != null && payorToolFields.Count != 0)
                {
                    payorToolFields = payorToolFields.Where(s => !payorTool.ToolFields.Exists(p => p.PayorFieldID == s.PayorToolFieldId)).ToList();
                    foreach (DLinq.PayorToolField field in payorToolFields)
                        PayorToolField.Delete(field.PayorToolFieldId);
                }

                foreach (PayorToolField Field in payorTool.ToolFields)
                {
                    Field.PayorToolId = payorTool.PayorToolId;
                    Field.TemplateID = payorTool.TemplateID;
                    PayorToolField.AddUpdate(Field);
                }
            }
        }

        public static bool DeletePayorToolTemplate(PayorTool payorTool, Guid? tempID)
        {
            bool bValue = true;

            try
            {
                Delete(payorTool, tempID);
            }
            catch
            {
                bValue = false;
            }
            return bValue;

        }

        public static void Delete(PayorTool payorTool, Guid? tempID)
        {
            try
            {
                //Delete all payor toll fieds
                DeletePayorToolField(payorTool);

                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {

                    DLinq.PayorTool ObjPayorTool = null;
                    if (tempID == null)
                    {
                        ObjPayorTool = (from p in DataModel.PayorTools
                                        where p.PayorToolId == payorTool.PayorToolId && p.TemplateID == null
                                        select p).FirstOrDefault();
                    }
                    else
                    {
                        ObjPayorTool = (from p in DataModel.PayorTools
                                        where p.PayorToolId == payorTool.PayorToolId && p.TemplateID == tempID
                                        select p).FirstOrDefault();
                    }

                    if (ObjPayorTool != null)
                    {
                        //Delete Payor tool
                        try
                        {
                            DataModel.DeleteObject(ObjPayorTool);
                            DataModel.SaveChanges();
                        }
                        catch
                        {
                            ObjPayorTool.IsDeleted = true;
                            DataModel.SaveChanges();
                            //delete payor template
                            deleteTemplate(tempID);
                        }

                    }

                    //delete payor template
                    deleteTemplate(tempID);

                }
            }
            catch
            {
            }
        }

        private static void DeletePayorToolField(PayorTool payorTool)
        {
            if (payorTool.ToolFields != null)
            {
                foreach (PayorToolField Field in payorTool.ToolFields)
                {
                    PayorToolField.Delete(Field);
                }
            }
        }

        private static void deleteTemplate(Guid? tempID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorTemplate temp = DataModel.PayorTemplates.FirstOrDefault(s => s.TemplateID == tempID);

                if (temp != null)
                {

                    DataModel.DeleteObject(temp);
                    DataModel.SaveChanges();
                }

               
            }
        }

        public static PayorTool GetPayorToolMgr(Guid PayorID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                PayorTool pTool = (from p in DataModel.PayorTools
                                   where p.Payor.PayorId == PayorID && p.TemplateID==null &&  p.IsDeleted == false
                                   select new PayorTool
                                   {
                                       ChequeImageFilePath = string.Empty,
                                       StatementImageFilePath = string.Empty,
                                       WebDevChequeImageFilePath = p.ChequeImageFile,
                                       WebDevStatementImageFilePath = p.StatementImageFile,
                                       PayorToolId = p.PayorToolId,
                                       PayorID = p.Payor.PayorId,
                                   }).FirstOrDefault();

                if (pTool != null)
                {
                    pTool.ToolFields =
                     (from p1 in DataModel.PayorToolFields
                      where p1.PayorTool.PayorToolId == pTool.PayorToolId && p1.IsDeleted == false
                      select new PayorToolField
                      {
                          AllignedDirection = p1.AllignedDirection ?? "Left",
                          DefaultValue = p1.DefaultNumeric,
                          EquivalentIncomingField = p1.EquivalentIncomingField,
                          EquivalentLearnedField = p1.EquivalentLearnedField,
                          EquivalentDeuField = p1.EquivalentDeuField,
                          ControlHeight = p1.FieldHeight ?? 0,
                          FieldOrder = p1.FieldOrder ?? 0,
                          ControlX = p1.FieldPositionX.Value,
                          ControlY = p1.FieldPositionY.Value,
                          FieldStatusValue = p1.FieldStatus,
                          ControlWidth = p1.FieldWidth ?? 0,
                          FormulaId = p1.FormulaId,
                          HelpText = p1.HelpText,
                          IsCalculatedField = p1.IsCalculatedField,
                          IsZeroorBlankAllowed = p1.IsOorBlankAllowed,
                          IsOverrideOfCalcAllowed = p1.IsOverrideOfCalcAllowed,
                          IsPopulateIfLinked = p1.IsPopulatedIfLinked,
                          IsPartOfPrimaryKey = p1.IsPartOfPrimary,
                          IsTabbedToNextFieldIfLinked = p1.IsTabbedToNextFieldIfLinked,
                          LabelOnField = p1.LabelOnImage,
                          MaskFieldTypeId = p1.MasterPayorToolMaskFieldType.PTMaskFieldTypeId == null ? 0 : p1.MasterPayorToolMaskFieldType.PTMaskFieldTypeId,
                          MaskFieldType = p1.MasterPayorToolMaskFieldType.Type.Value,
                          MaskText = p1.MasterPayorToolMaskFieldType.Name,
                          PTAvailableFieldId = p1.MasterPayorToolAvailableField.PTAvailableFieldId == null ? 0 : p1.MasterPayorToolAvailableField.PTAvailableFieldId,
                          AvailableFieldName = p1.MasterPayorToolAvailableField.PTAvailableFieldId == null ? "" : p1.MasterPayorToolAvailableField.Name,
                          PayorFieldID = p1.PayorToolFieldId,
                          PayorToolId = p1.PayorTool.PayorToolId
                      }).ToList();

                    foreach (PayorToolField field in pTool.ToolFields)
                    {
                        if (field.FormulaId != null)
                        {
                            DLinq.Formula formula = DataModel.Formulas.FirstOrDefault(s => s.FormulaId == field.FormulaId);
                            if (formula != null)
                            {
                                field.CalculationFormula = new Formula();
                                field.CalculationFormula.FormulaExpression = formula.FormulaExpression;
                                field.CalculationFormula.FormulaTtitle = formula.FormulaTtitle;
                                field.CalculationFormula.FormulaID = formula.FormulaId;
                            }
                        }
                    }
                }
                return pTool;
            }
        }

        public static Guid GetPayorToolId(Guid PayorId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorTool payorTool = DataModel.PayorTools.FirstOrDefault(s => s.PayorId == PayorId && s.IsDeleted == false);
                if (payorTool != null)
                    return payorTool.PayorToolId;
                else
                    return Guid.NewGuid();
            }
        }

        public static PayorTool GetPayorToolMgr(Guid PayorID, Guid? TemplateID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                PayorTool pTool = new PayorTool();

                if (TemplateID == null)
                {
                    pTool = (from p in DataModel.PayorTools
                             where p.Payor.PayorId == PayorID && p.TemplateID == null && p.IsDeleted == false
                             select new PayorTool
                             {
                                 ChequeImageFilePath = string.Empty,
                                 StatementImageFilePath = string.Empty,
                                 WebDevChequeImageFilePath = p.ChequeImageFile,
                                 WebDevStatementImageFilePath = p.StatementImageFile,
                                 PayorToolId = p.PayorToolId,
                                 PayorID = p.Payor.PayorId,
                                 TemplateID=p.TemplateID,
                             }).FirstOrDefault();
                }
                else
                {
                    pTool = (from p in DataModel.PayorTools
                             where p.Payor.PayorId == PayorID && p.TemplateID == TemplateID && p.IsDeleted == false
                             select new PayorTool
                             {
                                 ChequeImageFilePath = string.Empty,
                                 StatementImageFilePath = string.Empty,
                                 WebDevChequeImageFilePath = p.ChequeImageFile,
                                 WebDevStatementImageFilePath = p.StatementImageFile,
                                 PayorToolId = p.PayorToolId,
                                 PayorID = p.Payor.PayorId,
                                 TemplateID=p.TemplateID,
                             }).FirstOrDefault();
                }

                if (pTool != null)
                {
                    pTool.ToolFields =
                     (from p1 in DataModel.PayorToolFields
                      where p1.PayorTool.PayorToolId == pTool.PayorToolId && p1.IsDeleted == false
                      select new PayorToolField
                      {
                          AllignedDirection = p1.AllignedDirection ?? "Left",
                          DefaultValue = p1.DefaultNumeric,
                          EquivalentIncomingField = p1.EquivalentIncomingField,
                          EquivalentLearnedField = p1.EquivalentLearnedField,
                          EquivalentDeuField = p1.EquivalentDeuField,
                          ControlHeight = p1.FieldHeight ?? 0,
                          FieldOrder = p1.FieldOrder ?? 0,
                          ControlX = p1.FieldPositionX.Value,
                          ControlY = p1.FieldPositionY.Value,
                          FieldStatusValue = p1.FieldStatus,
                          ControlWidth = p1.FieldWidth ?? 0,
                          FormulaId = p1.FormulaId,
                          HelpText = p1.HelpText,
                          IsCalculatedField = p1.IsCalculatedField,
                          IsZeroorBlankAllowed = p1.IsOorBlankAllowed,
                          IsOverrideOfCalcAllowed = p1.IsOverrideOfCalcAllowed,
                          IsPopulateIfLinked = p1.IsPopulatedIfLinked,
                          IsPartOfPrimaryKey = p1.IsPartOfPrimary,
                          IsTabbedToNextFieldIfLinked = p1.IsTabbedToNextFieldIfLinked,
                          LabelOnField = p1.LabelOnImage,
                          MaskFieldTypeId = p1.MasterPayorToolMaskFieldType.PTMaskFieldTypeId == null ? 0 : p1.MasterPayorToolMaskFieldType.PTMaskFieldTypeId,
                          MaskFieldType = p1.MasterPayorToolMaskFieldType.Type.Value,
                          MaskText = p1.MasterPayorToolMaskFieldType.Name,
                          PTAvailableFieldId = p1.MasterPayorToolAvailableField.PTAvailableFieldId == null ? 0 : p1.MasterPayorToolAvailableField.PTAvailableFieldId,
                          AvailableFieldName = p1.MasterPayorToolAvailableField.PTAvailableFieldId == null ? "" : p1.MasterPayorToolAvailableField.Name,
                          PayorFieldID = p1.PayorToolFieldId,
                          PayorToolId = p1.PayorTool.PayorToolId


                      }).ToList();

                    foreach (PayorToolField field in pTool.ToolFields)
                    {
                        if (field.FormulaId != null)
                        {
                            DLinq.Formula formula = DataModel.Formulas.FirstOrDefault(s => s.FormulaId == field.FormulaId);
                            if (formula != null)
                            {
                                field.CalculationFormula = new Formula();
                                field.CalculationFormula.FormulaExpression = formula.FormulaExpression;
                                field.CalculationFormula.FormulaTtitle = formula.FormulaTtitle;
                                field.CalculationFormula.FormulaID = formula.FormulaId;
                            }
                        }
                    }
                }
                return pTool;
            }
        }

        public static bool AddUpdatePayorToolTemplate(Guid tempID, string strTemName, bool isDeleted, Guid SelectedPayorID)
        {
            bool bValue = true;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.PayorTemplate temp = DataModel.PayorTemplates.FirstOrDefault(s => s.TemplateID == tempID);
                    if (temp == null)
                    {
                        temp = new DLinq.PayorTemplate();
                        temp.TemplateID = Guid.NewGuid();
                        temp.PayorId = SelectedPayorID;
                        temp.TemplateName = strTemName;
                        temp.IsDeleted = isDeleted;
                        DataModel.AddToPayorTemplates(temp);
                    }
                    else
                    {
                        temp.TemplateName = strTemName;
                    }

                    DataModel.SaveChanges();

                    bValue = true;

                }
            }
            catch
            {
                bValue = false;
            }

            return bValue;

        }

        public static List<Tempalate> GetPayorToolTemplate(Guid SelectedPayorID)
        {
            List<Tempalate> tempPayor = new List<Tempalate>();
            List<PayorTool> tempPayor1 = new List<PayorTool>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    tempPayor = (from p in DataModel.PayorTemplates
                                 where p.PayorId == SelectedPayorID && p.IsDeleted == false
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

            //Add default value

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                tempPayor1 = (from p in DataModel.PayorTools
                              where p.PayorId == SelectedPayorID && p.TemplateID == null && p.IsDeleted == false
                              select new PayorTool
                              {
                                  PayorToolId = (Guid)p.PayorToolId,

                              }).ToList();

            }


            if (tempPayor1.Count > 0)
            {
                tempPayor = new List<Tempalate>(tempPayor.OrderBy(p => p.TemplateName));

                Tempalate tempDefault = new Tempalate();
                tempDefault.ID = 0;
                //tempDefault.TemplateID = new Guid();
                tempDefault.TemplateName = "Default";
                tempPayor.Insert(0, tempDefault);
            }

            return tempPayor;

        }

        #endregion

        #region
        [DataMember]
        public Guid PayorToolId { get; set; }
        [DataMember]
        public string ChequeImageFilePath { get; set; }
        [DataMember]
        public string StatementImageFilePath { get; set; }
        [DataMember]
        public string WebDevChequeImageFilePath { get; set; }
        [DataMember]
        public string WebDevStatementImageFilePath { get; set; }
        [DataMember]
        public List<PayorToolField> ToolFields { get; set; }
        [DataMember]
        public Guid PayorID { get; set; }

        [DataMember]
        public Guid? TemplateID { get; set; }

        #endregion
    }
}
