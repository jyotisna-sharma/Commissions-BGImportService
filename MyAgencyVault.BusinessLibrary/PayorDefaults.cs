using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq=DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.ComponentModel;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PayorDefaults : IEditable<PayorDefaults>
    {
        #region IEditable<PayorDefaults> Members

        public void AddUpdate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.GlobalPayorDefault payorDefaultDetail = (from e in DataModel.GlobalPayorDefaults
                                                                   where e.PayorDefaultSettingsId == this.PayorDefaultSettingsId
                                                                   select e).FirstOrDefault();

                if (payorDefaultDetail == null)
                {
                    payorDefaultDetail = new DLinq.GlobalPayorDefault
                    {

                        PayorDefaultSettingsId = this.PayorDefaultSettingsId,

                        FieldNamesRow = this.FieldNamesRow,
                        FirstPaymentOnRow = this.FirstPaymentOnRow,

                        IsTotalExists = this.IsTotalExists,
                        LocationColumn = this.LocationRow,
                        LocationRow = this.LocationRow,
                        NavigationInstructions = this.NavigationInstructions,
                        FileTypeId = this.FileTypeId,
                        LoginControl = this.LoginControl,
                        PasswordControl = this.PasswordControl,
                        WebSiteUrl = this.WebSiteUrl,

                    };
                    payorDefaultDetail.PayorReference.Value = ReferenceMaster.GetReferencedPayor(this.GlobalPayorId, DataModel);
                    payorDefaultDetail.MasterFileTypeReference.Value = ReferenceMaster.GetReferencedFileType(this.FileTypeId,DataModel);
                    DataModel.AddToGlobalPayorDefaults(payorDefaultDetail);
                }
                else
                {
                    payorDefaultDetail.FieldNamesRow = this.FieldNamesRow;
                    payorDefaultDetail.FirstPaymentOnRow = this.FirstPaymentOnRow;

                    payorDefaultDetail.IsTotalExists = this.IsTotalExists;
                    payorDefaultDetail.LocationColumn = this.LocationRow;
                    payorDefaultDetail.LocationRow = this.LocationRow;
                    payorDefaultDetail.NavigationInstructions = this.NavigationInstructions;
                    payorDefaultDetail.LoginControl = this.LoginControl;
                    payorDefaultDetail.PasswordControl = this.PasswordControl;
                    payorDefaultDetail.WebSiteUrl = this.WebSiteUrl;
                    payorDefaultDetail.FileTypeId = this.FileTypeId;
                    payorDefaultDetail.MasterFileTypeReference.Value = ReferenceMaster.GetReferencedFileType(this.FileTypeId,DataModel);
                }
                DataModel.SaveChanges();
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public PayorDefaults GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region "DAta members aka - public properties."
        
        [DataMember]
        public StatementDates PayorDefaultStatementDates
        {
            get
            {               
                StatementDates stDates = new StatementDates();
                return stDates;
            }
            set
            {
            }
        }

        public static PayorDefaults GetPayorDefault(Guid globalPayerId)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    return (from hd in DataModel.GlobalPayorDefaults
                            where ((hd.GlobalPayorId != null) && (hd.GlobalPayorId.Value == globalPayerId))
                            select new PayorDefaults
                            {
                                LocationColumn = hd.LocationColumn,
                                LocationRow = hd.LocationRow,
                                LoginControl = hd.LoginControl,
                                FieldNamesRow = hd.FieldNamesRow,
                                FirstPaymentOnRow = hd.FirstPaymentOnRow,
                                IsTotalExists = hd.IsTotalExists,
                                PasswordControl = hd.PasswordControl,
                                GlobalPayorId = hd.Payor.PayorId,
                                WebSiteUrl = hd.WebSiteUrl,
                                NavigationInstructions = hd.NavigationInstructions,
                                PayorDefaultSettingsId = hd.PayorDefaultSettingsId,
                                FileTypeId = hd.MasterFileType.FileTypeId
                            }).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<PayorDefaults> GetPayorDefault()
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    return (from hd in DataModel.GlobalPayorDefaults
                            select new PayorDefaults
                            {
                                LocationColumn = hd.LocationColumn,
                                LocationRow = hd.LocationRow,
                                LoginControl = hd.LoginControl,
                                FieldNamesRow = hd.FieldNamesRow,
                                FirstPaymentOnRow = hd.FirstPaymentOnRow,
                                IsTotalExists = hd.IsTotalExists,
                                PasswordControl = hd.PasswordControl,
                                GlobalPayorId = hd.Payor.PayorId,
                                WebSiteUrl = hd.WebSiteUrl,
                                NavigationInstructions = hd.NavigationInstructions,
                                PayorDefaultSettingsId = hd.PayorDefaultSettingsId,
                                FileTypeId = hd.MasterFileType.FileTypeId
                            }).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        [DataMember]
        public Guid PayorDefaultSettingsId { get; set; }
        [DataMember]
        public Guid GlobalPayorId { get; set; }
        [DataMember]
        public decimal? FirstPaymentOnRow { get; set; }
        [DataMember]
        public decimal? FieldNamesRow { get; set; }
        [DataMember]
        public decimal? LocationColumn { get; set; }
        [DataMember]
        public decimal? LocationRow { get; set; }
        [DataMember]
        public bool IsTotalExists { get; set; }
        [DataMember]
        public int FileTypeId { get; set; }
        [DataMember]
        public string WebSiteUrl { get; set; }
        [DataMember]
        public string NavigationInstructions { get; set; }
        [DataMember]
        public string LoginControl { get; set; }
        [DataMember]
        public string PasswordControl { get; set; }
        #endregion 

        /// <summary>
        /// update the related payor's default statement dates.
        /// </summary>
        //public void AddUpdateStatementDates()
        //{
        //    using (Entity.DataModel)
        //    {
        //        foreach (DateTime dt in this.PayorDefaultStatementDates.Months)
        //        {
        //            var existingStatementDate = (from e in Entity.DataModel.GlobalPayorStatementDates
        //                                         where (e.Payor.PayorId == this.GlobalPayorId && e.StatementDate.Value.Month == dt.Month)
        //                                         select e).FirstOrDefault();

        //            if (existingStatementDate == null)
        //            {
        //                existingStatementDate = new DLinq.GlobalPayorStatementDate
        //                {
        //                    PayorStatementDateID = Guid.NewGuid(),
        //                    StatementDate = dt
        //                };
        //                existingStatementDate.PayorReference.Value = (from l in Entity.DataModel.Payors where l.PayorId == this.GlobalPayorId select l).FirstOrDefault();

        //                Entity.DataModel .AddToGlobalPayorStatementDates(existingStatementDate);
        //            }
        //            else
        //            {
        //                existingStatementDate.StatementDate = dt;
        //            }
        //            Entity.DataModel.SaveChanges();
        //        }
        //    }
        //}

       

    }
}
