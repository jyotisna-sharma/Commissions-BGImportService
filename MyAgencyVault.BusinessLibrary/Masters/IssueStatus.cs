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
   public class IssueStatus
    {
        #region "Datamembers aka - public properties"
        [DataMember]
        public int StatusID { get; set; }

        [DataMember]
        public string StatusName { get; set; }
        #endregion
        //public static List<IssueStatus> GetStatus(int StatusID)
        //{

        //    using (DLinq.CommissionDepartmentEntities DataModel = new DLinq.CommissionDepartmentEntities())
        //    {
        //        return (from f in DataModel.MasterIssueStatus
        //                where f.IssueStatusId == StatusID
        //                select new IssueStatus
        //                {
        //                    StatusID = f.IssueStatusId,
        //                    StatusName = f.Name,
        //                }).ToList();
        //    }
        //}
        public static IssueStatus GetStatus(int StatusID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.MasterIssueStatus
                        where f.IssueStatusId == StatusID
                        select new IssueStatus
                        {
                            StatusID = f.IssueStatusId,
                            StatusName = f.Name,
                        }).FirstOrDefault();
            }
        }
        public static List<IssueStatus> GetAllStatus()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.MasterIssueStatus
                        select new IssueStatus
                        {
                            StatusID = f.IssueStatusId,
                            StatusName = f.Name,
                        }).ToList();
            }
        }
    }
}
