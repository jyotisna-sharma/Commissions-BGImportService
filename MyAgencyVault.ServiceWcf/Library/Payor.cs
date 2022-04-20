using System.Collections.Generic;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;
using System;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IPayor
    {
        [OperationContract]
        ReturnStatus AddUpdateDeletePayor(Payor pObj, Operation operationType);

        [OperationContract]
        ReturnStatus DeletePayor(Guid PayorId);

        [OperationContract]
        List<Payor> GetPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo);

        [OperationContract]
        int GetPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo);

        [OperationContract]
        List<Payor> GetPayorsInChunk(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take);

        [OperationContract]
        Payor GetPayorByID(Guid PayorId);

        [OperationContract]
        Payor GetPayorIDbyNickName(string nickName);

        [OperationContract]
        ReturnStatus ValdateLocalPayor(Payor pObj, string strPayorName, string strNickName);
    }

    [ServiceContract]
    interface IDisplayPayor
    {
        [OperationContract]
        List<DisplayedPayor> GetDisplayPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo);

        [OperationContract]
        int GetDisplayPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo);

        [OperationContract]
        List<DisplayedPayor> GetDisplayPayorsInChunk(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take);
    }

    [ServiceContract]
    interface IConfigDisplayPayor
    {
        [OperationContract]
        List<ConfigDisplayedPayor> GetConfigDisplayPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo);

        [OperationContract]
        int GetConfigDisplayPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo);

        [OperationContract]
        List<ConfigDisplayedPayor> GetConfigDisplayPayorsInChunk(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take);
    }

    [ServiceContract]
    interface ISettingDisplayPayor
    {
        [OperationContract]
        List<SettingDisplayedPayor> GetSettingDisplayPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo);

        [OperationContract]
        int GetSettingDisplayPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo);

        [OperationContract]
        List<SettingDisplayedPayor> GetSettingDisplayPayorsInChunk(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take);
    }

    public partial class MavService : IPayor, IDisplayPayor, IConfigDisplayPayor,ISettingDisplayPayor
    {
        #region Payor

        public ReturnStatus AddUpdateDeletePayor(Payor pObj,Operation operationType)
        {
            return pObj.AddUpdateDelete(operationType);
        }

        public ReturnStatus ValdateLocalPayor(Payor pObj, string strPayorName, string strNickName)
        {
            return pObj.ValdateLocalPayor(strPayorName, strNickName);
        }

        public ReturnStatus DeletePayor(Guid PayorId)
        {
            return Payor.DeletePayor(PayorId);
        }

        public List<Payor> GetPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            return Payor.GetPayors(LicenseeId, PayerfillInfo);
        }

        public int GetPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            return Payor.GetPayorCount(LicenseeId, PayerfillInfo);
        }

        public List<Payor> GetPayorsInChunk(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take)
        {
            return Payor.GetPayors(LicenseeId, PayerfillInfo, skip, take);
        }

        public Payor GetPayorByID(Guid PayorId)
        {
            return Payor.GetPayorByID(PayorId);
        }

        public Payor GetPayorIDbyNickName(string nickName)
        {
            return Payor.GetPayorIDbyNickName(nickName);
        }

        #endregion

        #region DisplayPayor

        public List<DisplayedPayor> GetDisplayPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            return DisplayedPayor.GetPayors(LicenseeId, PayerfillInfo);
        }

        public int GetDisplayPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            return DisplayedPayor.GetPayorCount(LicenseeId, PayerfillInfo);
        }

        public List<DisplayedPayor> GetDisplayPayorsInChunk(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take)
        {
            return DisplayedPayor.GetPayors(LicenseeId, PayerfillInfo, skip, take);
        }

        #endregion

        #region ConfigDisplayPayor

        public List<ConfigDisplayedPayor> GetConfigDisplayPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            return ConfigDisplayedPayor.GetPayors(LicenseeId, PayerfillInfo);
        }

        public int GetConfigDisplayPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            return ConfigDisplayedPayor.GetPayorCount(LicenseeId, PayerfillInfo);
        }

        public List<ConfigDisplayedPayor> GetConfigDisplayPayorsInChunk(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take)
        {
            return ConfigDisplayedPayor.GetPayors(LicenseeId, PayerfillInfo,skip,take);
        }

        #endregion

        #region SettingDisplayPayor

        public List<SettingDisplayedPayor> GetSettingDisplayPayors(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            return SettingDisplayedPayor.GetPayors(LicenseeId, PayerfillInfo);
        }

        public int GetSettingDisplayPayorCount(Guid? LicenseeId, PayorFillInfo PayerfillInfo)
        {
            return SettingDisplayedPayor.GetPayorCount(LicenseeId, PayerfillInfo);
        }

        public List<SettingDisplayedPayor> GetSettingDisplayPayorsInChunk(Guid? LicenseeId, PayorFillInfo PayerfillInfo, int skip, int take)
        {
            return SettingDisplayedPayor.GetPayors(LicenseeId, PayerfillInfo,skip,take);
        }

        #endregion
    }
}
