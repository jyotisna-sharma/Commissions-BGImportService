using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary.Masters
{
    [DataContract]
    public class Zip
    {
        #region "Data Members"
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        #endregion 

        public static bool IsZipCodeExist(string zipCode)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                bool zipExist = DataModel.MasterZipCodes.Any(s => s.ZipCode.ToString("D5") == zipCode);
                return zipExist;
            }
        }

        public static void AddZipDate(string zipCode, string city, string state)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.MasterZipCode zipData = new DLinq.MasterZipCode();

                zipData.ZipCode = long.Parse(zipCode);
                zipData.State = state;
                zipData.City = city;

                DataModel.MasterZipCodes.AddObject(zipData);
                DataModel.SaveChanges();
            }
        }

        public static Zip GetZip(string zipcode)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                long? zipCd = zipcode.CustomParseToLong();

                Zip Info = (from z in DataModel.MasterZipCodes
                            where z.ZipCode == zipCd
                            select new Zip()
                            {
                                City = z.City,
                                State = z.State
                            }).FirstOrDefault();

                if (Info != null)
                    Info.ZipCode = zipcode;

                return Info;
            }
        }

        public static List<Zip> GetZipList()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<Zip> ZipCodes = (from z in DataModel.MasterZipCodes
                                      select new Zip
                                      {
                                          City = z.City,
                                          State = z.State,
                                      }).ToList();

                foreach (Zip zp in ZipCodes)
                {
                    DLinq.MasterZipCode code = DataModel.MasterZipCodes.FirstOrDefault(s => s.City == zp.City && s.State == zp.State);
                    if (code != null)
                        zp.ZipCode = code.ZipCode.ToString("D5");
                }
                return ZipCodes;
            }
        }
    }
}
