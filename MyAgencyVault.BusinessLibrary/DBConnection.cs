using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data;

namespace MyAgencyVault.BusinessLibrary
{
    public static class DBConnection
    {
        public static string GetConnectionString()
        {
            DLinq.CommissionDepartmentEntities ctx = new DLinq.CommissionDepartmentEntities(); //create your entity object here
            EntityConnection ec = (EntityConnection)ctx.Connection;
            SqlConnection sc = (SqlConnection)ec.StoreConnection; //get the SQLConnection that your entity object would use
            return sc.ConnectionString.ToString();
        }
    }
}
