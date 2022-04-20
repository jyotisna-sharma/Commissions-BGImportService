using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using Masters = MyAgencyVault.BusinessLibrary.Masters;
using DataAccessLayer.LinqtoEntity;
using System.ComponentModel.DataAnnotations;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class GlobalPayorContact : IEditable<GlobalPayorContact>
    {
        #region "DataMember aka - public properties "
        [DataMember]
        public Guid PayorContactId { get; set; }
        [DataMember]
        public Guid GlobalPayorId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string OfficePhone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public int? Priority { get; set; }
        [DataMember]
        public string ContactPref { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        #endregion 
        #region IEditable<GlobalPayorContact> Members

        public void AddUpdate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {

                DLinq.GlobalPayorContact _contact = (from c in DataModel.GlobalPayorContacts
                                                     where c.PayorContactId == this.PayorContactId
                                                     select c).FirstOrDefault();
                if (_contact == null)
                {
                    _contact = new DLinq.GlobalPayorContact
                    {
                        PayorContactId = this.PayorContactId,
                        FirstName = this.FirstName,
                        LastName = this.LastName,
                        ZipCode = this.Zip.CustomParseToLong(),
                        city = this.City,
                        state = this.State,
                        email = this.Email,
                        OfficePhone = this.OfficePhone,
                        Fax = this.Fax,
                        Priority = this.Priority,
                        ContactPref = this.ContactPref,
                        IsDeleted = this.IsDeleted,

                    };
                    _contact.Payor = Masters.ReferenceMaster.GetReferencedPayor(this.GlobalPayorId, DataModel);
                    DataModel.AddToGlobalPayorContacts(_contact);
                }
                else
                {
                    _contact.FirstName = this.FirstName;
                    _contact.LastName = this.LastName;
                    _contact.ZipCode = this.Zip.CustomParseToLong();
                    _contact.city = this.City;
                    _contact.state = this.State;
                    _contact.email = this.Email;
                    _contact.OfficePhone = this.OfficePhone;
                    _contact.Fax = this.Fax;
                    _contact.Priority = this.Priority;
                    _contact.ContactPref = this.ContactPref;
                    _contact.IsDeleted = this.IsDeleted;

                }
                DataModel.SaveChanges();
            }
        }

        public void Delete()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var PayorContact = (from c in DataModel.GlobalPayorContacts
                                    where c.PayorContactId == this.PayorContactId
                                    select c).FirstOrDefault();
                PayorContact.IsDeleted = true;
                DataModel.SaveChanges();
            }
        }

        public static List<GlobalPayorContact> getContacts(Guid PayorId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                List<GlobalPayorContact> contacts = (from r in DataModel.GlobalPayorContacts
                                                     where (r.Payor.PayorId == PayorId) && (r.IsDeleted == false)
                                                     select new GlobalPayorContact
                                                     {
                                                         PayorContactId = r.PayorContactId,
                                                         FirstName = r.FirstName,
                                                         LastName = r.LastName,
                                                         City = r.city,
                                                         State = r.state,
                                                         ContactPref = r.ContactPref,
                                                         Email = r.email,
                                                         Fax = r.Fax,
                                                         GlobalPayorId = r.Payor.PayorId,
                                                         IsDeleted = r.IsDeleted,
                                                         OfficePhone = r.OfficePhone,
                                                         Priority = r.Priority
                                                     }).ToList();

                foreach (GlobalPayorContact contact in contacts)
                {
                    DLinq.GlobalPayorContact cnt = DataModel.GlobalPayorContacts.First(s => s.PayorContactId == contact.PayorContactId);
                    if (cnt.ZipCode != null)
                        contact.Zip = cnt.ZipCode.Value.ToString("D5");
                    else
                        contact.Zip = null;
                }
                return contacts;
            }
        }

        public GlobalPayorContact GetOfID()
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
