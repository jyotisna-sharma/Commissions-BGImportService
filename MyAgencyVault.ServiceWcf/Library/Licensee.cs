using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using MyAgencyVault.BusinessLibrary;
using System.Collections;
using System.Data;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary.Masters;
using MyAgencyVault.BusinessLibrary.Base;

namespace MyAgencyVault.WcfService
{
    [ServiceContract] 
    interface ILicensee
    {
        #region IEditable<Licensee> Members
        [OperationContract]
        void AddUpdateLicensee(LicenseeDisplayData License);
        [OperationContract]
        void DeleteLicensee(LicenseeDisplayData License);
        
        #endregion

                /// <summary>
        /// call to save all the notes in this licensee.
        /// </summary>
        void SaveAllNotes(LicenseeDisplayData License);
        
        /// <summary>
        /// need to look forward the requirement of this function in this class.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<LicenseeDisplayData> GetLicenseeList(LicenseeStatusEnum status, Guid licenseeId);

        /// <summary>
        /// compile and create Licensee wise batch files (all for Card payee) to be sent to Licensees given in the parameter, 
        /// for the given From and To Date.
        /// <param name="From Date"/>
        /// <param name="To Date"/>        
        /// <param name="LicenseeList"/>        
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool ExportCardPayees();
        

        /// <summary>
        /// compile and create Licensee wise batch files (all for Check payee) to be sent to Licensees given in the parameter, 
        /// for the given From and To Date.
        /// <param name="From Date"/>
        /// <param name="To Date"/>        
        /// <param name="LicenseeList"/>        
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool ExportCheckPayees();
       

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool ImportDataFromFile();

        [OperationContract]
        LicenseeDisplayData GetLicenseeByID(Guid id);
        [OperationContract]
        List<string> getPaymentTypes();
        [OperationContract]
        List<LicenseeDisplayData> GetDisplayedLicenseeList(Guid id);
        [OperationContract]
        void SetLastLoginTime(Guid LicenseeId);
        [OperationContract]
        void SetLastUploadTime(Guid LicenseeId);
        [OperationContract]
        List<LicenseeBalance> getLicenseesBalance();

        [OperationContract]
        List<LicenseeDisplayData> GetDisplayedLicenseeListPolicyManger(Guid id);
        
    }
    public partial class MavService : ILicensee
    {

        #region ILicensee Members

      public void AddUpdateLicensee(LicenseeDisplayData licensee)
        {
            Licensee.AddUpdate(licensee);
        }

      public void DeleteLicensee(LicenseeDisplayData licensee)
        {
            Licensee.Delete(licensee);  
        }


      public void SaveAllNotes(LicenseeDisplayData licensee)
        {
            Licensee.SaveAllNotes(licensee);
        }

      public List<LicenseeDisplayData> GetLicenseeList(LicenseeStatusEnum status, Guid licenseeId)
        {
            return Licensee.GetLicenseeList(status, licenseeId);
        }

        public bool ExportCardPayees()
        {
            return Licensee.ExportCardPayees();
        }

        public bool ExportCheckPayees()
        {
            return Licensee.ExportCheckPayees();
        }

        public bool ImportDataFromFile()
        {
            return Licensee.ImportDataFromFile();
        }

        public LicenseeDisplayData GetLicenseeByID(Guid id)
        {
            return Licensee.GetLicenseeByID(id);
        }

        public List<string> getPaymentTypes()
        {
            return Licensee.getPaymentTypes();
        }

        public List<LicenseeDisplayData> GetDisplayedLicenseeList(Guid id)
        {
            return LicenseeDisplayData.GetDisplayedLicenseeList(id);
        }

        public List<LicenseeDisplayData> GetDisplayedLicenseeListPolicyManger(Guid id)
        {
            return LicenseeDisplayData.GetDisplayedLicenseeListPolicyManger(id);
        }

        public void SetLastLoginTime(Guid LicenseeId)
        {
            Licensee.SetLastLoginTime(LicenseeId);
        }

        public void SetLastUploadTime(Guid LicenseeId)
        {
            Licensee.SetLastUploadTime(LicenseeId);
        }

        public List<LicenseeBalance> getLicenseesBalance()
        {
            return Licensee.getLicenseesBalance();
        }

        #endregion
    }
}
