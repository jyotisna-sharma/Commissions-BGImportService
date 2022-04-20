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
    public class ServiceChargeType : IEditable<ServiceChargeType>
    {
        #region
        [DataMember]
        public int ServiceChargeTypeID { get; set; }
        [DataMember]
        public string ServiceChargeName { get; set; }
        [DataMember]
        public string ServiceChargeDescription{ get; set; }
        #endregion 
        public static List<ServiceChargeType> GetAllServiceChargeType()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from sct in DataModel.MasterServiceChargeTypes
                        select new ServiceChargeType
                        {
                            ServiceChargeTypeID = sct.SCTypeId,
                            ServiceChargeName = sct.Name,
                            ServiceChargeDescription = sct.Description,
                        }).ToList();
            }
        }

        public override string ToString()
        {
            return ServiceChargeName;
        }

        public void AddUpdate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _servicechargetype = (from sct in DataModel.MasterServiceChargeTypes
                                          where sct.SCTypeId == this.ServiceChargeTypeID
                                          select sct).FirstOrDefault();
                if (_servicechargetype == null)//New
                {
                    _servicechargetype = new DLinq.MasterServiceChargeType
                    {
                        Name = this.ServiceChargeName,
                        Description = this.ServiceChargeDescription,
                    };

                    DataModel.AddToMasterServiceChargeTypes(_servicechargetype);
                }
                else
                {
                    _servicechargetype.Name = this.ServiceChargeName;

                }
                DataModel.SaveChanges();
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public ServiceChargeType GetOfID()
        {
            throw new NotImplementedException();
        }
    }
}
