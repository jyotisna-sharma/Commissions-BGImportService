using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{


    [DataContract]
    public class PolicyNotes : Note
    {
        public override void AddUpdate()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _PolicyNote = (from pn in DataModel.PolicyNotes where pn.PolicyNoteId == this.NoteID select pn).FirstOrDefault();
              
                if (_PolicyNote == null)
                {
                    _PolicyNote = new DLinq.PolicyNote
                    {

                        PolicyNoteId = this.NoteID,
                        Note = this.Content,
                        LastModifiedOn = this.LastModifiedDate,
                        CreatedOn = this.CreatedDate
                    };
                    _PolicyNote.PolicyReference.Value = (from pid in DataModel.Policies where pid.PolicyId == this.PolicyID select pid).FirstOrDefault();
                    DataModel.AddToPolicyNotes(_PolicyNote);
                }

                else
                {
                    _PolicyNote.Note = this.Content;
                    _PolicyNote.LastModifiedOn = DateTime.Today;
                   // _PolicyNote.CreatedOn = this.CreatedDate;
                }
                DataModel.SaveChanges();

            }
        }

        public override void Delete()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policyNote = (from po in DataModel.PolicyNotes where po.PolicyNoteId == this.NoteID select po).FirstOrDefault();
                if (_policyNote == null) return;
                DataModel.DeleteObject(_policyNote);
                DataModel.SaveChanges();
            }

        }
        public static void DeleteNoteByPolicyId(Guid PolicyId)
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                var _policyNote = (from po in DataModel.PolicyNotes where po.PolicyId == PolicyId select po).FirstOrDefault();
                if (_policyNote == null) return;
                DataModel.DeleteObject(_policyNote);
                DataModel.SaveChanges();
            }

        }
        public static List<PolicyNotes> GetNotes()
        {
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                return (from gn in DataModel.PolicyNotes

                        select new PolicyNotes
                       {
                           Content = gn.Note,
                           LastModifiedDate = gn.LastModifiedOn,
                           CreatedDate = gn.CreatedOn,
                           PolicyID = gn.Policy.PolicyId,
                           NoteID = gn.PolicyNoteId
                       }
                    ).ToList();


            }
        }

        //public static List<PolicyNotes> GetNotesPolicyWise(Guid PolicyId)
        //{
        //    List<PolicyNotes> _PolicyNotes = GetNotes().Where(p => p.PolicyID == PolicyId).OrderByDescending(p=>p.CreatedDate).ToList();
        //    return _PolicyNotes;

        //}


        public static List<PolicyNotes> GetNotesPolicyWise(Guid PolicyId)
        {
            List<PolicyNotes> _PolicyNotes = new List<PolicyNotes>();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    _PolicyNotes = (from pn in DataModel.PolicyNotes.Where(p => p.PolicyId == PolicyId)
                                    select new PolicyNotes
                                    {
                                        Content = pn.Note,
                                        LastModifiedDate = pn.LastModifiedOn,
                                        PolicyID = PolicyId,
                                        CreatedDate = pn.CreatedOn,
                                        NoteID = pn.PolicyNoteId
                                    }).ToList();
                }
            }
            catch(Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog(ex.StackTrace.ToString(), true);
            }
            return _PolicyNotes;

        }


        [DataMember]
        public Guid PolicyID { get; set; }

    }
}
