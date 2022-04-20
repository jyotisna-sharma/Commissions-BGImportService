using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PolicyLocking
    {       
        public static bool LockPolicy(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                System.Data.Objects.ObjectParameter objParam = new System.Data.Objects.ObjectParameter("IsLockObtained", typeof(bool));
                DataModel.LockPolicy(PolicyId, objParam);
                return (bool)objParam.Value;
            }
        }

        public static bool UnlockPolicy(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                System.Data.Objects.ObjectParameter objParam = new System.Data.Objects.ObjectParameter("UnlockSuccessfull", typeof(bool));
                DataModel.UnlockPolicy(PolicyId, objParam);
                return (bool)objParam.Value;
            }
        }
    }
}
