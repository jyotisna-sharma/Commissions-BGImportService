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
    public class IssueCategory :IEditable<IssueCategory>
    {

        #region "Datamembers aka - public properties"
        [DataMember]
        public int CategoryID { get; set; }

        [DataMember]
        public string CategoryName { get; set; }
        #endregion
       // public static List<IssueCategory> GetCategory(int CategoryID)
        public static IssueCategory GetCategory(int CategoryID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.MasterIssueCategories
                        where f.IssueCategoryId == CategoryID
                        select new IssueCategory
                        {
                          CategoryID=f.IssueCategoryId,
                          CategoryName=f.Name,
                        }).FirstOrDefault();
            }
        }
        public static List<IssueCategory> GetAllCategory()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.MasterIssueCategories                       
                        select new IssueCategory
                        {
                            CategoryID = f.IssueCategoryId,
                            CategoryName = f.Name,
                        }).ToList();
            }
        }




        public void AddUpdate()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public IssueCategory GetOfID()
        {
            throw new NotImplementedException();
        }
    }
}
