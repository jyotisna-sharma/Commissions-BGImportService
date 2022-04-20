using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Drawing;
using System.Runtime.Serialization;

using DLinq = DataAccessLayer.LinqtoEntity;
using DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PayorToolField
    {

        #region IEditable<PayorToolField> Members

        //public static void AddUpdate(PayorToolField payorToolField)
        //{
        //    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //    {
        //        DLinq.PayorToolField PayorFields = null;
        //        PayorFields = (from p in DataModel.PayorToolFields
        //                       where p.PayorToolFieldId == payorToolField.PayorFieldID
        //                       select p).FirstOrDefault();
        //        if (PayorFields != null)
        //        {
        //            PayorFields.AllignedDirection = payorToolField.AllignedDirection;
        //            PayorFields.DefaultNumeric = payorToolField.DefaultValue;
        //            PayorFields.EquivalentIncomingField = payorToolField.EquivalentIncomingField;
        //            PayorFields.EquivalentLearnedField = payorToolField.EquivalentLearnedField;
        //            PayorFields.EquivalentDeuField = payorToolField.EquivalentDeuField;
        //            PayorFields.FieldHeight = Convert.ToInt16(payorToolField.ControlHeight);
        //            PayorFields.FieldOrder = payorToolField.FieldOrder;
        //            PayorFields.FieldPositionX = (int)payorToolField.ControlX;
        //            PayorFields.FieldPositionY = (int)payorToolField.ControlY;
        //            PayorFields.FieldStatus = payorToolField.FieldStatusValue;
        //            PayorFields.FieldWidth = Convert.ToInt16(payorToolField.ControlWidth);
        //            PayorFields.FormulaId = payorToolField.FormulaId;
        //            PayorFields.HelpText = payorToolField.HelpText;
        //            PayorFields.IsCalculatedField = payorToolField.IsCalculatedField;
        //            PayorFields.IsOorBlankAllowed = payorToolField.IsZeroorBlankAllowed;
        //            PayorFields.IsOverrideOfCalcAllowed = payorToolField.IsOverrideOfCalcAllowed;
        //            PayorFields.IsPopulatedIfLinked = payorToolField.IsPopulateIfLinked;
        //            PayorFields.IsPartOfPrimary = payorToolField.IsPartOfPrimaryKey;
        //            PayorFields.IsTabbedToNextFieldIfLinked = payorToolField.IsTabbedToNextFieldIfLinked;
        //            PayorFields.LabelOnImage = payorToolField.LabelOnField;
        //            PayorFields.MasterPayorToolAvailableFieldReference.Value = ReferenceMaster.GetreferencedPayorToolAvailableField(payorToolField.PTAvailableFieldId, DataModel);
        //            PayorFields.MasterPayorToolMaskFieldType = ReferenceMaster.GetreferencedMaskFieldType(payorToolField.MaskFieldTypeId, DataModel);
        //            PayorFields.PayorTool = ReferenceMaster.GetreferencedPayorTool(payorToolField.PayorToolId, DataModel);
        //            PayorFields.MaskFieldTypeId = PayorFields.MasterPayorToolMaskFieldType.PTMaskFieldTypeId;
        //        }
        //        else
        //        {
        //            PayorFields = new DLinq.PayorToolField();
        //            PayorFields.PayorToolFieldId = payorToolField.PayorFieldID;

        //            PayorFields.AllignedDirection = payorToolField.AllignedDirection;
        //            PayorFields.DefaultNumeric = payorToolField.DefaultValue;
        //            PayorFields.EquivalentIncomingField = payorToolField.EquivalentIncomingField;
        //            PayorFields.EquivalentLearnedField = payorToolField.EquivalentLearnedField;
        //            PayorFields.EquivalentDeuField = payorToolField.EquivalentDeuField;
        //            PayorFields.FieldHeight = Convert.ToInt16(payorToolField.ControlHeight);
        //            PayorFields.FieldOrder = payorToolField.FieldOrder;
        //            PayorFields.FieldPositionX = (int)payorToolField.ControlX;
        //            PayorFields.FieldPositionY = (int)payorToolField.ControlY;
        //            PayorFields.FieldStatus = payorToolField.FieldStatusValue;
        //            PayorFields.FieldWidth = Convert.ToInt16(payorToolField.ControlWidth);
        //            PayorFields.FormulaId = payorToolField.FormulaId;
        //            PayorFields.HelpText = payorToolField.HelpText;
        //            PayorFields.IsCalculatedField = payorToolField.IsCalculatedField;
        //            PayorFields.IsOorBlankAllowed = payorToolField.IsZeroorBlankAllowed;
        //            PayorFields.IsOverrideOfCalcAllowed = payorToolField.IsOverrideOfCalcAllowed;
        //            PayorFields.IsPopulatedIfLinked = payorToolField.IsPopulateIfLinked;
        //            PayorFields.IsPartOfPrimary = payorToolField.IsPartOfPrimaryKey;
        //            PayorFields.IsTabbedToNextFieldIfLinked = payorToolField.IsTabbedToNextFieldIfLinked;
        //            PayorFields.LabelOnImage = payorToolField.LabelOnField;
        //            PayorFields.MasterPayorToolAvailableFieldReference.Value = ReferenceMaster.GetreferencedPayorToolAvailableField(payorToolField.PTAvailableFieldId, DataModel);
        //            PayorFields.MasterPayorToolMaskFieldTypeReference.Value = ReferenceMaster.GetreferencedMaskFieldType(payorToolField.MaskFieldTypeId, DataModel);
        //            if (PayorFields.MasterPayorToolMaskFieldTypeReference.Value ==null)
        //            {
        //                PayorFields.MaskFieldTypeId = 1;
        //            }
        //            PayorFields.PayorToolReference.Value = ReferenceMaster.GetreferencedPayorTool(payorToolField.PayorToolId, DataModel);
        //            DataModel.AddToPayorToolFields(PayorFields);

        //        }

        //        if (payorToolField.CalculationFormula != null)
        //        {
        //            PayorFields.Formula = Formula.AddUpdate(payorToolField.CalculationFormula, DataModel);
        //        }
        //        else
        //        {
        //            if (payorToolField.FormulaId != null)
        //            {
        //                Formula.Delete(payorToolField.FormulaId.Value, DataModel);
        //                PayorFields.Formula = null;
        //                PayorFields.FormulaId = null;
        //            }
        //        }

        //        DataModel.SaveChanges();
        //    }
        //}
        /// <summary>
        /// check if already in use, if yes no delete allow
        /// </summary>
        /// 
        public static void AddUpdate(PayorToolField payorToolField)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorToolField PayorFields = null;
                if (payorToolField.TemplateID == null)
                {
                    PayorFields = (from p in DataModel.PayorToolFields
                                   where p.PayorToolFieldId == payorToolField.PayorFieldID && p.TemplateID == null
                                   select p).FirstOrDefault();
                }
                else
                {
                    PayorFields = (from p in DataModel.PayorToolFields
                                   where p.PayorToolFieldId == payorToolField.PayorFieldID && p.TemplateID == payorToolField.TemplateID
                                   select p).FirstOrDefault();
                }
                if (PayorFields != null)
                {
                    PayorFields.AllignedDirection = payorToolField.AllignedDirection;
                    PayorFields.DefaultNumeric = payorToolField.DefaultValue;
                    PayorFields.EquivalentIncomingField = payorToolField.EquivalentIncomingField;
                    PayorFields.EquivalentLearnedField = payorToolField.EquivalentLearnedField;
                    PayorFields.EquivalentDeuField = payorToolField.EquivalentDeuField;
                    PayorFields.FieldHeight = Convert.ToInt16(payorToolField.ControlHeight);
                    PayorFields.FieldOrder = payorToolField.FieldOrder;
                    PayorFields.FieldPositionX = (int)payorToolField.ControlX;
                    PayorFields.FieldPositionY = (int)payorToolField.ControlY;
                    PayorFields.FieldStatus = payorToolField.FieldStatusValue;
                    PayorFields.FieldWidth = Convert.ToInt16(payorToolField.ControlWidth);
                    PayorFields.FormulaId = payorToolField.FormulaId;
                    PayorFields.HelpText = payorToolField.HelpText;
                    PayorFields.IsCalculatedField = payorToolField.IsCalculatedField;
                    PayorFields.IsOorBlankAllowed = payorToolField.IsZeroorBlankAllowed;
                    PayorFields.IsOverrideOfCalcAllowed = payorToolField.IsOverrideOfCalcAllowed;
                    PayorFields.IsPopulatedIfLinked = payorToolField.IsPopulateIfLinked;
                    PayorFields.IsPartOfPrimary = payorToolField.IsPartOfPrimaryKey;
                    PayorFields.IsTabbedToNextFieldIfLinked = payorToolField.IsTabbedToNextFieldIfLinked;
                    PayorFields.LabelOnImage = payorToolField.LabelOnField;
                    //added
                    PayorFields.TemplateID = payorToolField.TemplateID;

                    PayorFields.MasterPayorToolAvailableFieldReference.Value = ReferenceMaster.GetreferencedPayorToolAvailableField(payorToolField.PTAvailableFieldId, DataModel);
                    PayorFields.MasterPayorToolMaskFieldType = ReferenceMaster.GetreferencedMaskFieldType(payorToolField.MaskFieldTypeId, DataModel);
                    PayorFields.PayorTool = ReferenceMaster.GetreferencedPayorTool(payorToolField.PayorToolId, DataModel);
                    PayorFields.MaskFieldTypeId = PayorFields.MasterPayorToolMaskFieldType.PTMaskFieldTypeId;
                }
                else
                {
                    PayorFields = new DLinq.PayorToolField();
                    PayorFields.PayorToolFieldId = payorToolField.PayorFieldID;

                    PayorFields.AllignedDirection = payorToolField.AllignedDirection;
                    PayorFields.DefaultNumeric = payorToolField.DefaultValue;
                    PayorFields.EquivalentIncomingField = payorToolField.EquivalentIncomingField;
                    PayorFields.EquivalentLearnedField = payorToolField.EquivalentLearnedField;
                    PayorFields.EquivalentDeuField = payorToolField.EquivalentDeuField;
                    PayorFields.FieldHeight = Convert.ToInt16(payorToolField.ControlHeight);
                    PayorFields.FieldOrder = payorToolField.FieldOrder;
                    PayorFields.FieldPositionX = (int)payorToolField.ControlX;
                    PayorFields.FieldPositionY = (int)payorToolField.ControlY;
                    PayorFields.FieldStatus = payorToolField.FieldStatusValue;
                    PayorFields.FieldWidth = Convert.ToInt16(payorToolField.ControlWidth);
                    PayorFields.FormulaId = payorToolField.FormulaId;
                    PayorFields.HelpText = payorToolField.HelpText;
                    PayorFields.IsCalculatedField = payorToolField.IsCalculatedField;
                    PayorFields.IsOorBlankAllowed = payorToolField.IsZeroorBlankAllowed;
                    PayorFields.IsOverrideOfCalcAllowed = payorToolField.IsOverrideOfCalcAllowed;
                    PayorFields.IsPopulatedIfLinked = payorToolField.IsPopulateIfLinked;
                    PayorFields.IsPartOfPrimary = payorToolField.IsPartOfPrimaryKey;
                    PayorFields.IsTabbedToNextFieldIfLinked = payorToolField.IsTabbedToNextFieldIfLinked;
                    PayorFields.LabelOnImage = payorToolField.LabelOnField;

                    //added
                    PayorFields.TemplateID = payorToolField.TemplateID;

                    PayorFields.MasterPayorToolAvailableFieldReference.Value = ReferenceMaster.GetreferencedPayorToolAvailableField(payorToolField.PTAvailableFieldId, DataModel);
                    PayorFields.MasterPayorToolMaskFieldTypeReference.Value = ReferenceMaster.GetreferencedMaskFieldType(payorToolField.MaskFieldTypeId, DataModel);
                    if (PayorFields.MasterPayorToolMaskFieldTypeReference.Value == null)
                    {
                        PayorFields.MaskFieldTypeId = 1;
                    }
                    PayorFields.PayorToolReference.Value = ReferenceMaster.GetreferencedPayorTool(payorToolField.PayorToolId, DataModel);
                    DataModel.AddToPayorToolFields(PayorFields);

                }

                if (payorToolField.CalculationFormula != null)
                {
                    PayorFields.Formula = Formula.AddUpdate(payorToolField.CalculationFormula, DataModel);
                }
                else
                {
                    if (payorToolField.FormulaId != null)
                    {
                        Formula.Delete(payorToolField.FormulaId.Value, DataModel);
                        PayorFields.Formula = null;
                        PayorFields.FormulaId = null;
                    }
                }

                DataModel.SaveChanges();
            }
        }

        public static void UpdateDuplicatePayor(PayorToolField payorToolField, Guid PayorToolID, Guid? TemplateID)
        {

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorToolField PayorFields = null;

                PayorFields = new DLinq.PayorToolField();
                PayorFields.PayorToolFieldId = Guid.NewGuid();

                PayorFields.AllignedDirection = payorToolField.AllignedDirection;
                PayorFields.DefaultNumeric = payorToolField.DefaultValue;
                PayorFields.EquivalentIncomingField = payorToolField.EquivalentIncomingField;
                PayorFields.EquivalentLearnedField = payorToolField.EquivalentLearnedField;
                PayorFields.EquivalentDeuField = payorToolField.EquivalentDeuField;
                PayorFields.FieldHeight = Convert.ToInt16(payorToolField.ControlHeight);
                PayorFields.FieldOrder = payorToolField.FieldOrder;
                PayorFields.FieldPositionX = (int)payorToolField.ControlX;
                PayorFields.FieldPositionY = (int)payorToolField.ControlY;
                PayorFields.FieldStatus = payorToolField.FieldStatusValue;
                PayorFields.FieldWidth = Convert.ToInt16(payorToolField.ControlWidth);
                PayorFields.FormulaId = payorToolField.FormulaId;
                PayorFields.HelpText = payorToolField.HelpText;
                PayorFields.IsCalculatedField = payorToolField.IsCalculatedField;
                PayorFields.IsOorBlankAllowed = payorToolField.IsZeroorBlankAllowed;
                PayorFields.IsOverrideOfCalcAllowed = payorToolField.IsOverrideOfCalcAllowed;
                PayorFields.IsPopulatedIfLinked = payorToolField.IsPopulateIfLinked;
                PayorFields.IsPartOfPrimary = payorToolField.IsPartOfPrimaryKey;
                PayorFields.IsTabbedToNextFieldIfLinked = payorToolField.IsTabbedToNextFieldIfLinked;
                PayorFields.LabelOnImage = payorToolField.LabelOnField;

                //added
                PayorFields.TemplateID = payorToolField.TemplateID;

                PayorFields.MasterPayorToolAvailableFieldReference.Value = ReferenceMaster.GetreferencedPayorToolAvailableField(payorToolField.PTAvailableFieldId, DataModel);
                PayorFields.MasterPayorToolMaskFieldTypeReference.Value = ReferenceMaster.GetreferencedMaskFieldType(payorToolField.MaskFieldTypeId, DataModel);
                if (PayorFields.MasterPayorToolMaskFieldTypeReference.Value == null)
                {
                    PayorFields.MaskFieldTypeId = 1;
                }
                PayorFields.PayorToolReference.Value = ReferenceMaster.GetreferencedPayorTool(payorToolField.PayorToolId, DataModel);
                DataModel.AddToPayorToolFields(PayorFields);
                
                if (payorToolField.CalculationFormula != null)
                {
                    PayorFields.Formula = Formula.AddUpdate(payorToolField.CalculationFormula, DataModel);
                }
                else
                {
                    if (payorToolField.FormulaId != null)
                    {
                        Formula.Delete(payorToolField.FormulaId.Value, DataModel);
                        PayorFields.Formula = null;
                        PayorFields.FormulaId = null;
                    }
                }

                DataModel.SaveChanges();
            }
        }

        public static void Delete(PayorToolField payorToolField)
        {
            Delete(payorToolField.PayorFieldID);
        }

        public static void Delete(Guid payorToolFieldId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorToolField Pobj = (from p in DataModel.PayorToolFields
                                             where p.PayorToolFieldId == payorToolFieldId && p.IsDeleted == false
                                             select p).FirstOrDefault();
                if (Pobj != null)
                {
                    //Pobj.IsDeleted = true;
                    DataModel.DeleteObject(Pobj);
                    DataModel.SaveChanges();
                }
            }
        }

        public static void DeletePayorToolFiledID(Guid payorToolFieldId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorToolField Pobj = (from p in DataModel.PayorToolFields
                                             where p.PayorToolFieldId == payorToolFieldId && p.IsDeleted == false
                                             select p).FirstOrDefault();
                if (Pobj != null)
                {                   
                    DataModel.DeleteObject(Pobj);
                    DataModel.SaveChanges();
                }
            }
        }

//        public static List<PayorToolField> GetPayorToolFields(Guid PayorToolId)
//        {
//            CommissionDepartmentEntities DataModel = Entity.DataModel;
//            List<PayorToolField> Pobj = (from p in DataModel.PayorToolFields
//                                         where p.PayorToolId == PayorToolId && p.IsDeleted == false
//                                         select new PayorToolField
//                                         {
                                             
//PayorToolId =p.PayorToolId,
//         PayorFieldID =p.PayorToolFieldId,
//         LabelOnField =p.LabelOnImage,
//         FieldStatusValue =p.FieldStatus,
//        FieldOrder =p.FieldOrder,
//         IsPartOfPrimaryKey =p.IsPartOfPrimary,
//         IsPopulateIfLinked =p.IsPopulatedIfLinked,
//         IsTabbedToNextFieldIfLinked =p.IsTabbedToNextFieldIfLinked,
//         IsCalculatedField =p.IsCalculatedField,
//         IsOverrideOfCalcAllowed =p.IsOverrideOfCalcAllowed,
//         DefaultValue =p.DefaultNumeric,
//         IsZeroorBlankAllowed =p.IsOorBlankAllowed,
//         AllignedDirection =p.AllignedDirection,
//        CalculationFormula =p.Formula,
//         HelpText =p.HelpText,
//         IsDeleted =p.IsDeleted,
        
//         ControlHeight =p.FieldHeight,
//         ControlWidth =p.FieldWidth,
//        PTAvailableFieldId =p.PTAvailableFieldId,
//         AvailableFieldName =p.MasterPayorToolAvailableField.Name,
//         MaskText =p.MasterPayorToolMaskFieldType.Name,
//        MaskFieldTypeId =p.MaskFieldTypeId,
//        MaskFieldType =p.MasterPayorToolMaskFieldType.PTMaskFieldTypeId,
//         ControlX =p.FieldPositionX,
//          ControlY =p.FieldPositionY,
//         FieldValue 
//         EquivalentIncomingField  
//         EquivalentLearnedField 
//         EquivalentDeuField 
//        FormulaId 
//         IsNotVisible
//                                         }
//                                         ).ToList<PayorToolField>();

//            return Pobj;
//        }
        #endregion
        #region "Data members aka- public properties"
        [DataMember]
        public Guid PayorToolId { get; set; }
        [DataMember]
        public Guid PayorFieldID { get; set; }
        [DataMember]
        public string LabelOnField { get; set; }
        [DataMember]
        public string FieldStatusValue { get; set; }
        [DataMember]
        public int FieldOrder { get; set; }
        [DataMember]
        public bool IsPartOfPrimaryKey { get; set; }
        [DataMember]
        public bool IsPopulateIfLinked { get; set; }
        [DataMember]
        public bool IsTabbedToNextFieldIfLinked { get; set; }
        [DataMember]
        public bool IsCalculatedField { get; set; }
        [DataMember]
        public bool IsOverrideOfCalcAllowed { get; set; }
        [DataMember]
        public string DefaultValue { get; set; }
        [DataMember]
        public bool IsZeroorBlankAllowed { get; set; }
        [DataMember]
        public string AllignedDirection { get; set; }
        [DataMember]
        public Formula CalculationFormula { get; set; }
        [DataMember]
        public string HelpText { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
      
        [DataMember]
        public double ControlHeight { get; set; }
        [DataMember]
        public double ControlWidth { get; set; }
        [DataMember]
        public int PTAvailableFieldId { get; set; }
        [DataMember]
        public string AvailableFieldName { get; set; }
        [DataMember]
        public string MaskText { get; set; }
        [DataMember]
        public int MaskFieldTypeId { get; set; }
        [DataMember]
        public byte MaskFieldType { get; set; }
        [DataMember]
        public double ControlX { get; set; }
        [DataMember]
        public double ControlY { get; set; }
        [DataMember]
        public string FieldValue { get; set; }
        [DataMember]
        public string EquivalentIncomingField { get; set; } //Gaurav Needs to be changed
        [DataMember]
        public string EquivalentLearnedField { get; set; } //Gaurav Needs to be changed
        [DataMember]
        public string EquivalentDeuField { get; set; } //Gaurav Needs to be changed
        [DataMember]
        public Guid? FormulaId { get; set; }
        [DataMember]
        public bool IsNotVisible { get; set; }

        [DataMember]
        public Guid? TemplateID { get; set; }
        #endregion
    }

}
