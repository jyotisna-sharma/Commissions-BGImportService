using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLinq = DataAccessLayer.LinqtoEntity;
using System.Runtime.Serialization;
using MyAgencyVault.BusinessLibrary.Masters;
namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class LicenseeNote
    {
        [DataMember]
        public Guid LicenseeId { get; set; }
        [DataMember]
        public Guid NoteID { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public DateTime? LastModifiedDate { get; set; }

        public LicenseeNote AddUpdate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.LicenseeNote _lincesNote =
                    (from N in DataModel.LicenseeNotes
                     where (N.LicenseeNoteId == this.NoteID)
                     select N).FirstOrDefault();

                if (_lincesNote == null)
                {
                    _lincesNote = new DLinq.LicenseeNote
                    {
                        LicenseeNoteId = this.NoteID,
                        Note = this.Content,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    };
                    DLinq.Licensee _license = ReferenceMaster.GetReferencedLicensee(this.LicenseeId, DataModel);
                    _lincesNote.Licensee = _license;
                    DataModel.AddToLicenseeNotes(_lincesNote);
                }
                else
                {
                    _lincesNote.Note = this.Content;
                    _lincesNote.CreatedDate = this.CreatedDate;
                    _lincesNote.LastModifiedDate = DateTime.Now;
                }
                this.CreatedDate = _lincesNote.CreatedDate;
                this.LastModifiedDate = _lincesNote.LastModifiedDate;

                DataModel.SaveChanges();

                return this;
            }
        }
        public void Delete()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.LicenseeNote _note = (from n in DataModel.LicenseeNotes
                                            where (n.LicenseeNoteId == this.NoteID)
                                            select n).FirstOrDefault();
                DataModel.DeleteObject(_note);
                DataModel.SaveChanges();
            }
        }
        public bool IsValid()
        {
            throw new NotImplementedException();
        }
        public static LicenseeNote GetOfID(Guid id)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                DLinq.LicenseeNote _note = (from n in DataModel.LicenseeNotes
                                            where (n.LicenseeNoteId == id)
                                            select n).First();
                return new LicenseeNote { Content = _note.Note, NoteID = _note.LicenseeNoteId, CreatedDate = _note.CreatedDate, LastModifiedDate = _note.LastModifiedDate, LicenseeId = _note.Licensee.LicenseeId };
            }
        }

        public static List<LicenseeNote> GetLicenseeNotes(Guid lincenseeID)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var licNotes = (from se in DataModel.LicenseeNotes where (se.LicenseeId==lincenseeID)
                                select new LicenseeNote
                                {
                                    Content = se.Note,
                                    CreatedDate = se.CreatedDate,
                                    LastModifiedDate = se.LastModifiedDate,
                                }).ToList();

                return licNotes;
            }

        }
    }
}
