using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ICompType
    {
        [OperationContract]
        List<CompType> GetAllComptype();

        [OperationContract]
        void AddUpdateCompType(CompType objCompType);

        [OperationContract]
        bool DeleteCompType(CompType objCompType);

        [OperationContract]
        bool FindCompTypeName(string strName);


    }

    public partial class MavService : ICompType
    {
        public List<CompType> GetAllComptype()
        {
            CompType objCompType = new CompType();
            return objCompType.GetAllComptype();
        }

        public void AddUpdateCompType(CompType CompType)
        {
            CompType objCompType = new CompType();
            objCompType.AddUpdateCompType(CompType);
        }

        public bool DeleteCompType(CompType CompType)
        {
            CompType objCompType = new CompType();
            return objCompType.DeleteCompType(CompType);
        }

        public bool FindCompTypeName(string strName)
        {
            CompType objCompType = new CompType();
            return objCompType.FindCompTypeName(strName);
        }
    }
}