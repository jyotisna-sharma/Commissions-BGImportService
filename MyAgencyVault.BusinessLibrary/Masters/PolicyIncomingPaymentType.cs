using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class PolicyIncomingPaymentType
    {
        #region "Data Members"
        [DataMember]
        public int PaymentTypeId { get; set; }
        [DataMember]
        public string PaymenProcedureName { get; set; }
        #endregion 

        public static List<PolicyIncomingPaymentType> GetIncomingPaymentTypeList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from s in DataModel.MasterIncomingPaymentTypes
                        orderby s.Name
                        select new PolicyIncomingPaymentType
                        {
                            PaymentTypeId = s.IncomingPaymentTypeId,
                            PaymenProcedureName = s.Name,

                        }).ToList();
            }
        }
    }
}
