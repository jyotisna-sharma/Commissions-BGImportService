using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class PayorToolLearnedlFieldType
    {
        #region "Data Members aka - public properties"
        [DataMember]
        public int learnFieldID { get; set; }
        [DataMember]
        public string learnFieldName { get; set; }
        #endregion 
        public static List<PayorToolLearnedlFieldType> GetFieldList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from s in DataModel.MasterPayorToolLearnedFields
                        select new PayorToolLearnedlFieldType
                        {
                            learnFieldID = s.PTLearnedFieldId,
                            learnFieldName = s.Name
                        }).ToList();
            }
        }
    }
}
