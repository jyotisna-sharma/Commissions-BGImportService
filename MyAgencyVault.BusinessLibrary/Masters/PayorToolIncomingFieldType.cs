using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class PayorToolIncomingFieldType
    {
        #region "data members aka - public properties"
        [DataMember]
        public int IncomingFieldID { get; set; }
        [DataMember]
        public string incomingFieldName { get; set; }
        #endregion 
        public static List<PayorToolIncomingFieldType> GetFieldList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from s in DataModel.MasterPayorToolIncomingFields
                        select new PayorToolIncomingFieldType
                        {
                            IncomingFieldID = s.PTIncomingFieldId,
                            incomingFieldName = s.Name

                        }).ToList();
            }
        }
    }
}
