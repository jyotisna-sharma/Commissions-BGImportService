using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public abstract class Note : IEditable<Note>
    {
        #region IEditable<Note> Members
        public abstract void AddUpdate();        
        public abstract void Delete();
        public Note GetOfID()
        {
            throw new NotImplementedException();
        }
        //public abstract bool IsValid();        
        #endregion
        #region "data members aka - public properties."
        [DataMember]
        public Guid NoteID { get; set; }        
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public DateTime? LastModifiedDate { get; set; }
        #endregion 
    }
}
