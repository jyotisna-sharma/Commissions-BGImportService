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
   public class IssueResults
    {
        #region "Datamembers aka - public properties"
        [DataMember]
        public int ResultsID { get; set; }

        [DataMember]
        public string ResultsName { get; set; }
        #endregion
        //public static List<IssueResults> GetResults(int ResultsID)
        //{

        //    using (DLinq.CommissionDepartmentEntities DataModel = new DLinq.CommissionDepartmentEntities())
        //    {
        //        return (from f in DataModel.MasterIssueResults
        //                where f.IssueResultId == ResultsID
        //                select new IssueResults
        //                {
        //                    ResultsID = f.IssueResultId,
        //                    ResultsName = f.Name,
        //                }).ToList();
        //    }
        //}
        public static IssueResults GetResults(int ResultsID)
        {

            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.MasterIssueResults
                        where f.IssueResultId == ResultsID
                        select new IssueResults
                        {
                            ResultsID = f.IssueResultId,
                            ResultsName = f.Name,
                        }).FirstOrDefault();
            }
        }
        public static List<IssueResults> GetAllResults()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.MasterIssueResults
                        select new IssueResults
                        {
                            ResultsID = f.IssueResultId,
                            ResultsName = f.Name,
                        }).ToList();
            }
        }
    }
}
