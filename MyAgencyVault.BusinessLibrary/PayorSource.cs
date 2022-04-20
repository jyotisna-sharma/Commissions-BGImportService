using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class PayorSource
    {
        [DataMember]
        public Guid LicenseeId { get; set; }
        [DataMember]
        public Guid PayorId { get; set; }
        [DataMember]
        public bool IsWebsite { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public string ConfigNotes { get; set; }

        public void AddUpdate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                bool AddCase = false;
                DLinq.PayorSource source = DataModel.PayorSources.FirstOrDefault(s => s.LicenseeId == LicenseeId && s.PayorId == PayorId);
                if (source == null)
                {
                    source = new DLinq.PayorSource();
                    AddCase = true;
                }

                source.LicenseeId = LicenseeId;
                source.Licensee = DataModel.Licensees.FirstOrDefault(s => s.LicenseeId == LicenseeId);
                source.PayorId = PayorId;
                source.Payor = DataModel.Payors.FirstOrDefault(s => s.PayorId == PayorId);
                source.SourceType = IsWebsite;
                source.Notes = Notes;               

                if(AddCase)
                {
                    DataModel.AddToPayorSources(source);
                }

                DataModel.SaveChanges();

            }
        }

        public void AddUpdateConfigNotes()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                bool AddCase = false;
                DLinq.PayorSource source = DataModel.PayorSources.FirstOrDefault(s => s.LicenseeId == LicenseeId && s.PayorId == PayorId);
                if (source == null)
                {
                    source = new DLinq.PayorSource();
                    AddCase = true;
                }

                source.LicenseeId = LicenseeId;
                source.Licensee = DataModel.Licensees.FirstOrDefault(s => s.LicenseeId == LicenseeId);
                source.PayorId = PayorId;
                source.Payor = DataModel.Payors.FirstOrDefault(s => s.PayorId == PayorId);
                source.SourceType = IsWebsite;               
                source.ConfigNotes = ConfigNotes;

                if (AddCase)
                {
                    DataModel.AddToPayorSources(source);
                }

                DataModel.SaveChanges();
            }
        }

        public PayorSource GetPayorSource()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.PayorSource source = DataModel.PayorSources.FirstOrDefault(s => s.LicenseeId == LicenseeId && s.PayorId == PayorId);
                if (source == null)
                {
                    AddUpdate();
                    PayorSource payorsource = new PayorSource()
                    {
                        LicenseeId = this.LicenseeId,
                        PayorId = this.PayorId,
                        IsWebsite = this.IsWebsite,
                        Notes = this.Notes,
                        ConfigNotes = this.ConfigNotes,
                    };
                    return payorsource;
                }
                else
                {
                    PayorSource payorsource = new PayorSource()
                    {
                        LicenseeId = source.LicenseeId,
                        PayorId = source.PayorId,
                        IsWebsite = source.SourceType,
                        Notes = source.Notes,
                        ConfigNotes = source.ConfigNotes,
                    };
                    return payorsource;
                }
            }
        }
        
        public static void UpdateFollowUpDateAndserviceStatus(string Name, string Value)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.MasterSystemConstant sytemConstant = new DLinq.MasterSystemConstant();

                var _MasterSystemConstant = (from p in DataModel.MasterSystemConstants where (p.Name == Name) select p).FirstOrDefault();

                if (_MasterSystemConstant != null)
                {
                    if (_MasterSystemConstant.Name == Name)
                    {
                        _MasterSystemConstant.Value = Value;

                        DataModel.SaveChanges();
                    }
                }

            }
        }
        
    }
}
