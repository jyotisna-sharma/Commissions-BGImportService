using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class BatchDownloadStatus
    {

        #region "Data Members aka - |Public properties"
        [DataMember]
        public int BatchDownloadStatusId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Desc { get; set; }
        #endregion

        public static List<BatchDownloadStatus> GetBatchDownloadStatus()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from dv in DataModel.MasterBatchDownloadStatus

                        select new BatchDownloadStatus
                        {
                            BatchDownloadStatusId = dv.BatchDownloadStatusId,
                            Desc = dv.Description,
                            Name = dv.Name
                        }).ToList();
            }
        }

    }
}
