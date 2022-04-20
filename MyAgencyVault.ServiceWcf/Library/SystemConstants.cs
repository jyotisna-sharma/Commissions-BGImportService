using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Runtime.Serialization;

namespace MyAgencyVault.WcfService.Library
{
    [ServiceContract]
    interface ISystemConstant
    {
        [OperationContract]
        List<SystemConstant> getSystemConstants();

        [OperationContract]
        string GetKeyValue(string Key);

 
    }

    public partial class MavService : ISystemConstant
    {
        public List<SystemConstant> getSystemConstants()
        {
            return SystemConstant.GetSystemConstants();
        }

        public string GetKeyValue(string Key)
        {
            return SystemConstant.GetKeyValue(Key);
        }

    }
}