using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace DataAccessLayer.LinqtoEntity
{
    public partial class CommissionDepartmentEntities
    {
        public int CalculatePMC1(Nullable<global::System.Guid> policyID, ObjectParameter pMC)
        {
            ObjectParameter policyIDParameter;
            if (policyID.HasValue)
            {
                policyIDParameter = new ObjectParameter("PolicyID", policyID);
            }
            else
            {
                policyIDParameter = new ObjectParameter("PolicyID", typeof(global::System.Guid));
            }

            return base.ExecuteFunction("CalculatePMC", policyIDParameter, pMC);
        }
    }
}
