using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class FileType
    {
        #region "Data members aka - public properties."
        [DataMember]
        public int FileTypeId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        #endregion 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<FileType> GetSupportedFileTypeList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from f in DataModel.MasterFileTypes
                        select new FileType
                        {
                            FileTypeId = f.FileTypeId,
                            Name = f.Name,
                            Description = f.Description
                        }).ToList();
            }
        }
    }
}
