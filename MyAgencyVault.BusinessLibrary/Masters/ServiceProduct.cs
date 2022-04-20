using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using MyAgencyVault.BusinessLibrary.Base;
using DLinq = DataAccessLayer.LinqtoEntity;
namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
  public  class ServiceProduct : IEditable<ServiceProduct>
    { 
        #region
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public string ServiceDescription{ get; set; }
        #endregion 
        public static List<ServiceProduct> GetAllServiceProduct()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from sct in DataModel.MasterServiceChargeTypes
                        select new ServiceProduct
                        {
                            ServiceID = sct.SCTypeId,
                            ServiceName = sct.Name,
                            ServiceDescription = sct.Description,
                        }).ToList();
            }
        }

        public override string ToString()
        {
            return this.ServiceName;
        }

        public void AddUpdate()
        {
            //try
            //{
            //    using (DLinq.CommissionDepartmentEntities DataModel = new DLinq.CommissionDepartmentEntities())
            //    {
            //        var _servicec = (from se in DataModel.Services
            //                                  where se.SCId == this.ServiceID
            //                                  select se).FirstOrDefault();
            //        if (_servicec == null)//New
            //        {
            //            _servicec = new DLinq.Services
            //            {
            //                Name = this.ServiceName,
            //                Description = this.ServiceDescription,
            //            };

            //            DataModel.AddToMasterServiceChargeTypes(_servicec);
            //        }
            //        else
            //        {
            //            _servicec.Name = this.ServiceDescription;

            //        }
            //        DataModel.SaveChanges();
            //    }
            //}
            //catch
            //{

            //}
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public ServiceProduct GetOfID()
        {
            throw new NotImplementedException();
        }
    }
}
