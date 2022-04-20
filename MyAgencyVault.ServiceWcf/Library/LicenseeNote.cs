using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ILicenseeNote
    {
        [OperationContract]
        LicenseeNote AddUpdateLicenseeNote(LicenseeNote LNote);
        [OperationContract]
        void DeleteLicenseeNote(LicenseeNote LNote);
        [OperationContract]
        bool IsValidLicenseeNote(LicenseeNote LNote);
        [OperationContract]
        LicenseeNote GetLicenseeNoteOfID(Guid Id);

        [OperationContract]
        List<LicenseeNote> GetLicenseeNotes(Guid lincenseeID);
    }
    public partial class MavService : ILicenseeNote
    {


        #region ILicenseeNote Members

        public LicenseeNote AddUpdateLicenseeNote(LicenseeNote LNote)
        {
            return LNote.AddUpdate();
        }

        public void DeleteLicenseeNote(LicenseeNote LNote)
        {
            LNote.Delete();
        }

        public bool IsValidLicenseeNote(LicenseeNote LNote)
        {
            return LNote.IsValid();
        }

        public LicenseeNote GetLicenseeNoteOfID(Guid Id)
        {
            return LicenseeNote.GetOfID(Id);
        }

        public List<LicenseeNote> GetLicenseeNotes(Guid lincenseeID)
        {
            return LicenseeNote.GetLicenseeNotes(lincenseeID);
        }
        #endregion
    }
}