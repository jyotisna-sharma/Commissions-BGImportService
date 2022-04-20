using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary.Masters;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPayorToolAvailablelFieldType
    {
      [OperationContract]
      int AddUpdatePayorToolAvailablelFieldType(PayorToolAvailablelFieldType PyrToolAvalableFields);       
      [OperationContract]
      bool DeletePayorToolAvailablelFieldType(PayorToolAvailablelFieldType PyrToolAvalableFields);       
      [OperationContract] 
      List<PayorToolAvailablelFieldType> GetFieldList();

      [OperationContract]
      List<PayorToolAvailablelFieldType> GetImportToolList();
        
    }
    public partial class MavService : IPayorToolAvailablelFieldType 
    {

       #region IPayorToolAvailablelFieldType Members

        public int AddUpdatePayorToolAvailablelFieldType(PayorToolAvailablelFieldType PyrToolAvalableFields)
        {
           return PyrToolAvalableFields.AddUpdate();
        }
        public bool DeletePayorToolAvailablelFieldType(PayorToolAvailablelFieldType PyrToolAvalableFields)
        {
           return PyrToolAvalableFields.Delete();
        }

        public List<PayorToolAvailablelFieldType> GetFieldList()
        {
            return PayorToolAvailablelFieldType.GetFieldList();  
        }

        public List<PayorToolAvailablelFieldType> GetImportToolList()
        {
            return PayorToolAvailablelFieldType.GetImportToolList();  
        }

        #endregion
    }
}