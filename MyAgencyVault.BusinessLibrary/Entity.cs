using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{
    public static class Entity
    {
        public static CommissionDepartmentEntities DataModel
        {
            get
            {
                return new CommissionDepartmentEntities();
            }
        }
    }
}
