using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class SystemConstant
    {
        #region "Data Members aka - public properties."
        [DataMember]
        public int Key;
        [DataMember]
        public string Value;
        #endregion

        public static List<SystemConstant> GetSystemConstants()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from s in DataModel.MasterSystemConstants
                        select new SystemConstant
                        {
                            Key = s.SystemConstantId,
                            Value = s.Value

                        }).ToList();
            }
        }

         
        public static string GetKeyValue(string Key)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DataModel.CommandTimeout = 360000;
                SystemConstant Constant = (from s in DataModel.MasterSystemConstants
                                           where s.Name == Key
                                           select new SystemConstant
                                           {
                                               Value = s.Value
                                           }).FirstOrDefault();
                return Constant.Value;
            }
        }
                
        public static string IsFollowUuRuns(string Key)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                SystemConstant Constant = (from s in DataModel.MasterSystemConstants
                                           where s.Name == Key
                                           select new SystemConstant
                                           {
                                               Value = s.Value
                                           }).FirstOrDefault();
                return Constant.Value;
            }
        }


        public static void AddNameValue(string Name, string Value)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.MasterSystemConstant sytemConstant = new DLinq.MasterSystemConstant();
                sytemConstant.Name = Name;
                sytemConstant.Value = Value;

                int Id = DataModel.MasterSystemConstants.Select(s => s.SystemConstantId).Max();
                sytemConstant.SystemConstantId = Id + 1;

                DataModel.AddToMasterSystemConstants(sytemConstant);
                DataModel.SaveChanges();
            }
        }

        //public static void UpdateFollowUpDateAndserviceStatus(string Name, string Value)
        //{
        //    using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
        //    {
        //        DLinq.MasterSystemConstant sytemConstant = new DLinq.MasterSystemConstant();

        //        var _MasterSystemConstant = (from p in DataModel.MasterSystemConstants where (p.Name == Name) select p).FirstOrDefault();

        //        if (_MasterSystemConstant != null)
        //        {
        //            if (_MasterSystemConstant.Name == Name)
        //            {
        //                _MasterSystemConstant.Value = Value;

        //                DataModel.SaveChanges();
        //            }
        //        }

        //    }
        //}
    }
}
