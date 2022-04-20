using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract] 
    interface INote
    {

        #region IEditable<Note> Members
        [OperationContract]
        void AddUpdateNote(PolicyNotes Not);
       [OperationContract]
        void DeleteNote(PolicyNotes Not);
       [OperationContract]
       List<PolicyNotes> GetNote();
       //[OperationContract]
       //bool IsValidNote(PolicyNotes Not);
       [OperationContract]
       List<PolicyNotes> GetNotesPolicyWise(Guid PolicyId);
     
        #endregion
    }
    public partial class MavService : INote
    {

        #region INote Members

        public void AddUpdateNote(PolicyNotes Not)
        {
            Not.AddUpdate();
        }

        public void DeleteNote(PolicyNotes Not)
        {
            Not.Delete();
        }

        public List<PolicyNotes> GetNote()
        {
            return PolicyNotes.GetNotes();
        }

        //public bool IsValidNote(Note Not)
        //{
        //    return Not.IsValid();  
        //}

        #endregion

        #region INote Members


        public List<PolicyNotes> GetNotesPolicyWise(Guid PolicyId)
        {
            return PolicyNotes.GetNotesPolicyWise(PolicyId);
        }

        #endregion
    }
}
