using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    
    [ServiceContract]
    interface IPayorTool 
    {
        #region IEditable<PayorTool> Members
        [OperationContract]
        void AddUpdatePayorToolMgr(PayorTool PyorTool);
        [OperationContract]
        void DeletePayorToolMgr(PayorTool PyorTool,Guid? TemplateID);
        [OperationContract]
        PayorTool GetPayorToolMgr(Guid PayorID);
        [OperationContract]
        Guid GetPayorToolId(Guid PayorId);

        [OperationContract]
        bool AddUpdatePayorToolTemplate(Guid tempID, string strTemName, bool isDeleted, Guid PayorID);

        [OperationContract]
        bool DeletePayorToolTemplate(PayorTool PyorTool,Guid? GuidID);

        [OperationContract]
        List<Tempalate> GetPayorToolTemplate(Guid PayorId);

        [OperationContract]
        PayorTool GetPayorToolMgrWithTemplate(Guid PayorID, Guid? TempID);

        [OperationContract]
        bool IsAvailablePayorTempalate(Guid SourcePayorID, Guid? SourceTempID, Guid DestinationPayorID, Guid? DestiTempID);

        [OperationContract]
        bool UpdateDulicatePayorTool(Guid SourcePayorID, Guid? SourceTempID, Guid DestinationPayorID, Guid? DestiTempID);
        

        #endregion


    }
    public partial class MavService : IPayorTool
    {

        #region IPayorTool Members

        public void AddUpdatePayorToolMgr(PayorTool PyorTool)
        {
            PayorTool.AddUpdate(PyorTool);
        }

        public void DeletePayorToolMgr(PayorTool PyorTool,Guid? TemplateID)
        {
            PayorTool.Delete(PyorTool, TemplateID);
        }      

        public PayorTool GetPayorToolMgr(Guid PayorID)
        {
            return PayorTool.GetPayorToolMgr(PayorID);
        }
        
        public Guid GetPayorToolId(Guid PayorId)
        {
            return PayorTool.GetPayorToolId(PayorId);
        }

        public bool AddUpdatePayorToolTemplate(Guid tempID, string strTemName, bool isDeleted, Guid PayorID)
        {
            return PayorTool.AddUpdatePayorToolTemplate(tempID, strTemName, isDeleted, PayorID);
        }


        public bool DeletePayorToolTemplate(PayorTool PyorTool,Guid? GuidID)
        {
            return PayorTool.DeletePayorToolTemplate(PyorTool,GuidID);
        }

        public List<Tempalate> GetPayorToolTemplate(Guid PayorId)
        {
            return PayorTool.GetPayorToolTemplate(PayorId);
        }

        public PayorTool GetPayorToolMgrWithTemplate(Guid PayorID, Guid? TempID)
        {
            return PayorTool.GetPayorToolMgr(PayorID, TempID);
        }
        
        public bool IsAvailablePayorTempalate(Guid SourcePayorID, Guid? SourceTempID, Guid DestinationPayorID, Guid? DestiTempID)
        {
            return PayorTool.IsAvailablePayorTempalate(SourcePayorID, SourceTempID, DestinationPayorID, DestiTempID);
        }

        public bool UpdateDulicatePayorTool(Guid SourcePayorID, Guid? SourceTempID, Guid DestinationPayorID, Guid? DestiTempID)
        {
            return PayorTool.UpdateDulicatePayorTool(SourcePayorID, SourceTempID, DestinationPayorID, DestiTempID);
        }
        #endregion
    }
}
