using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class PayorToolAvailablelFieldType
    {
        #region "Datamembers aka -- public properties."
        
        [DataMember]
        public int FieldID { get; set; }
        [DataMember]
        public string FieldName { get; set; }
        [DataMember]
        public string FieldDiscription { get; set; }
        [DataMember]
        public bool IsUsed { get; set; }
        [DataMember]
        public string EquivalentIncomingField { get; set; }
        [DataMember]
        public string EquivalentLearnedField { get; set; }
        [DataMember]
        public string EquivalentDeuField { get; set; }
        [DataMember]
        public bool canDeleted { get; set; }
        #endregion

        public int AddUpdate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.MasterPayorToolAvailableField Obj = null;
                Obj = (from p in DataModel.MasterPayorToolAvailableFields where p.Name == this.FieldName select p).FirstOrDefault();

                if (Obj == null)
                {
                    Obj = new DLinq.MasterPayorToolAvailableField();
                    Obj.Name = this.FieldName;
                    Obj.Description = this.FieldDiscription;
                    Obj.EquivalentDeuField = this.EquivalentDeuField;
                    Obj.EquivalentIncomingField = this.EquivalentIncomingField;
                    Obj.EquivalentLearnedField = this.EquivalentLearnedField;
                    Obj.IsDeleted = false;
                    Obj.IsDeletable = true;
                    DataModel.AddToMasterPayorToolAvailableFields(Obj);
                    DataModel.SaveChanges();
                }

                return Obj.PTAvailableFieldId;
            }
        }

        public bool Delete()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                bool isDeleted = false;
                //old
                //DLinq.MasterPayorToolAvailableField _field = DataModel.MasterPayorToolAvailableFields.FirstOrDefault(s => s.Name == this.FieldName);
                //new
                //Check custom field available in any other fields
                DLinq.PayorToolField pfield = DataModel.PayorToolFields.FirstOrDefault(s => s.PTAvailableFieldId==this.FieldID && s.IsDeleted==false);
                if (pfield != null)
                {
                    isDeleted = false;
                }
                else
                {
                    DLinq.MasterPayorToolAvailableField _field = DataModel.MasterPayorToolAvailableFields.FirstOrDefault(s => s.Name == this.FieldName && s.IsDeleted == false);
                    _field.IsDeleted = true;
                    isDeleted = true;
                    DataModel.SaveChanges();
                }
                
                return isDeleted;
            }
        }

        public static List<PayorToolAvailablelFieldType> GetFieldList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<PayorToolAvailablelFieldType> availableFields = (from cf in DataModel.MasterPayorToolAvailableFields
                                                                      where cf.IsDeleted == false || cf.IsDeleted == null
                                                                      select new PayorToolAvailablelFieldType
                                                                      {
                                                                          FieldName = cf.Name,
                                                                          FieldID = cf.PTAvailableFieldId,
                                                                          canDeleted = cf.IsDeletable,
                                                                          EquivalentIncomingField = cf.EquivalentIncomingField,
                                                                          EquivalentDeuField = cf.EquivalentDeuField,
                                                                          EquivalentLearnedField = cf.EquivalentLearnedField
                                                                      }
                    ).ToList();

                foreach (PayorToolAvailablelFieldType field in availableFields)
                {
                    bool isUsed = DataModel.PayorToolFields.Any(s => s.IsDeleted == false && s.PTAvailableFieldId == field.FieldID);
                    field.IsUsed = isUsed;
                }

                return availableFields;
            }
        }

        public static List<PayorToolAvailablelFieldType> GetImportToolList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<PayorToolAvailablelFieldType> availableFields = (from cf in DataModel.ImportToolAvailableFields
                                                                      where cf.IsDeleted == false || cf.IsDeleted == null
                                                                      select new PayorToolAvailablelFieldType
                                                                      {
                                                                          FieldName = cf.Name,
                                                                          FieldID = cf.PTAvailableFieldId,
                                                                          canDeleted = cf.IsDeletable,
                                                                          EquivalentIncomingField = cf.EquivalentIncomingField,
                                                                          EquivalentDeuField = cf.EquivalentDeuField,
                                                                          EquivalentLearnedField = cf.EquivalentLearnedField
                                                                      }
                    ).ToList();

                foreach (PayorToolAvailablelFieldType field in availableFields)
                {
                    bool isUsed = DataModel.PayorToolFields.Any(s => s.IsDeleted == false && s.PTAvailableFieldId == field.FieldID);
                    field.IsUsed = isUsed;
                }

                return availableFields;
            }
        }
        
    }
}
