using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class PayorToolMaskedFieldType
    {
        #region "Data member aka- public properties"
        [DataMember]
        public int maskFieldID { get; set; }
        [DataMember]
        public string MaskName { get; set; }
        [DataMember]
        public byte Type { get; set; }

        #endregion 
        public static List<PayorToolMaskedFieldType> GetFieldList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from e in DataModel.MasterPayorToolMaskFieldTypes
                        select new PayorToolMaskedFieldType
                        {
                            maskFieldID = e.PTMaskFieldTypeId,
                            MaskName = e.Name,
                            Type = e.Type.Value
                        }).ToList();
            }
        }
        public static string GetMaskName(int MaskID)
        {
            if (MaskID == 8)
                return string.Empty;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                PayorToolMaskedFieldType Mask = (from e in DataModel.MasterPayorToolMaskFieldTypes
                                                 where e.PTMaskFieldTypeId == MaskID
                                                 select new PayorToolMaskedFieldType
                                                 {
                                                     maskFieldID = e.PTMaskFieldTypeId,
                                                     Type = e.Type.Value,
                                                     MaskName = e.Name
                                                 }).FirstOrDefault();
                return Mask.MaskName ?? string.Empty;
            }

        }

    }
}
