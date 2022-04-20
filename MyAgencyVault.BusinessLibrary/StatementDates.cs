using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class StatementDates
    {

        #region IEditable<StatementDates> Members

        public static void AddUpdate(List<StatementDates> StatementDate)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                foreach (StatementDates _StatementDate in StatementDate)
                {
                    DLinq.GlobalPayorStatementDate PayorStatementDate = null;
                    if (_StatementDate.IsDeleted)
                    {
                        PayorStatementDate = (from e in DataModel.GlobalPayorStatementDates
                                              where e.PayorStatementDateID == _StatementDate.PayorStatementDateID
                                              select e).FirstOrDefault();
                        DataModel.DeleteObject(PayorStatementDate);
                    }
                    else if (_StatementDate.IsNew)
                    {
                        PayorStatementDate = new DLinq.GlobalPayorStatementDate();
                        PayorStatementDate.PayorID = _StatementDate.PayorID;
                        PayorStatementDate.IsBatchCreated = false;
                        PayorStatementDate.PayorStatementDateID = _StatementDate.PayorStatementDateID;
                        PayorStatementDate.StatementDate = _StatementDate.StatementDate;

                        DataModel.AddToGlobalPayorStatementDates(PayorStatementDate);
                    }
                }

                DataModel.SaveChanges();
            }
        }

        public static void Delete(List<StatementDates> StatementDate)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                foreach (StatementDates _StatementDate in StatementDate)
                {
                    DLinq.GlobalPayorStatementDate _PayorStatementDate = (from n in DataModel.GlobalPayorStatementDates
                                                                          where (n.PayorStatementDateID == _StatementDate.PayorStatementDateID)
                                                                          select n).FirstOrDefault();
                    DataModel.DeleteObject(_PayorStatementDate);
                    DataModel.SaveChanges();
                }
            }
        }

        public static List<StatementDates> GetStatementDate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from hd in DataModel.GlobalPayorStatementDates

                        select new StatementDates
                        {
                            PayorID = hd.Payor.PayorId,
                            PayorStatementDateID = hd.PayorStatementDateID,
                            StatementDate = hd.StatementDate
                        }).ToList();
            }
        }

        public static void MarkAsBatchGenerated(List<StatementDates> Dates)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                foreach (StatementDates date in Dates)
                {
                    DLinq.GlobalPayorStatementDate Dts = DataModel.GlobalPayorStatementDates.Where(s => s.PayorStatementDateID == date.PayorStatementDateID).First();
                    Dts.IsBatchCreated = true;
                }

                DataModel.SaveChanges();
            }
        }

        public static List<StatementDates> GetActiveStatementDates()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from hd in DataModel.GlobalPayorStatementDates
                        where hd.IsBatchCreated == false && hd.StatementDate <= DateTime.Now
                        select new StatementDates
                        {
                            PayorID = hd.Payor.PayorId,
                            PayorStatementDateID = hd.PayorStatementDateID,
                            StatementDate = hd.StatementDate
                        }).ToList();

                //return (from hd in DataModel.GlobalPayorStatementDates                       
                //        select new StatementDates
                //        {
                //            PayorID = hd.Payor.PayorId,
                //            PayorStatementDateID = hd.PayorStatementDateID,
                //            StatementDate = hd.StatementDate
                //        }).ToList();

               
            }
        }

        #endregion

        #region "public properties"
        [DataMember]
        public Guid PayorStatementDateID { get; set; }
        [DataMember]
        public Guid PayorID { get; set; }
        [DataMember]
        public DateTime StatementDate { get; set; }
        [DataMember]
        public bool IsNew { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }

        #endregion
    }

}
