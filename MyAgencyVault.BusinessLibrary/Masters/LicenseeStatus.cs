using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class LicenseeStatus
    {
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Status { get; set; }
        
        /// <summary>
        /// to get list of all licensee status defined in the system.
        /// </summary>
        /// <returns></returns>
        public static List<LicenseeStatus> GetLicenseeStatusList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from s in DataModel.MasterLicenseStatus
                        select new LicenseeStatus
                        {
                            StatusId = s.LicenseStatusId,
                            Status = s.Name
                        }).ToList();
            }
        }
    }
}
