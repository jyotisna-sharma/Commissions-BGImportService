using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class IssueReasons
    {
        #region "Datamembers aka - public properties"
        [DataMember]
        public int ReasonsID { get; set; }

        [DataMember]
        public string ReasonsName { get; set; }
        #endregion
        //public static List<IssueReasons> GetReasons(int ReasonsID)
        //{

        //    using (DLinq.CommissionDepartmentEntities DataModel = new DLinq.CommissionDepartmentEntities())
        //    {
        //        return (from f in DataModel.MasterIssueReasons
        //                where f.IssueReasonId == ReasonsID
        //                select new IssueReasons
        //                {
        //                    ReasonsID = f.IssueReasonId,
        //                    ReasonsName = f.Name,
        //                }).ToList();
        //    }
        //}
        public static IssueReasons GetReasons(int ReasonsID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.MasterIssueReasons
                        where f.IssueReasonId == ReasonsID
                        select new IssueReasons
                        {
                            ReasonsID = f.IssueReasonId,
                            ReasonsName = f.Name,
                        }).FirstOrDefault();
            }
        }
        public static List<IssueReasons> GetAllReason()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.MasterIssueReasons
                        select new IssueReasons
                        {
                            ReasonsID = f.IssueReasonId,
                            ReasonsName = f.Name,
                        }).ToList();
            }
        }
    }
}
