using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using MyAgencyVault.BusinessLibrary.Base;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class CompType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? IncomingPaymentTypeID { get; set; }

        [DataMember]
        public string PaymentTypeName { get; set; }

        [DataMember]
        public string Names { get; set; }

        public List<CompType> GetAllComptype(string agencyName  = "")
        {
            List<CompType> lstCompType = null;
            try
            {
                //ActionLogger.Logger.WriteImportPolicyLog("in GetAllComptype", true);
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    lstCompType = (from p in DataModel.IncomingCompTypes
                                   select new CompType
                                   {
                                       Id = p.Id,
                                       IncomingPaymentTypeID = p.IncomingPaymentTypeID,
                                       PaymentTypeName = p.PaymentTypeName,
                                       Names = p.Names
                                   }).ToList();

                }
            }
            catch(Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("Exception getting comptype: " + ex.Message, true, agencyName);
            }

            return lstCompType;
        }

        public void AddUpdateCompType(CompType objCompType)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.IncomingCompType ObjValue = (from p in DataModel.IncomingCompTypes.Where(b => b.Id == objCompType.Id) select p).FirstOrDefault();
                    //insert
                    if (ObjValue == null)
                    {
                        var CompValue = new DLinq.IncomingCompType
                        {
                            IncomingPaymentTypeID = objCompType.IncomingPaymentTypeID,
                            PaymentTypeName = objCompType.PaymentTypeName,
                            Names = objCompType.Names
                        };

                        DataModel.AddToIncomingCompTypes(CompValue);
                    }
                    //update 
                    else
                    {
                        ObjValue.IncomingPaymentTypeID = objCompType.IncomingPaymentTypeID;
                        ObjValue.PaymentTypeName = objCompType.PaymentTypeName;
                        ObjValue.Names = objCompType.Names;
                    }

                    DataModel.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public bool DeleteCompType(CompType objCompType)
        {
            bool bValue = true;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.IncomingCompType ObjIncomingCompType = DataModel.IncomingCompTypes.FirstOrDefault(p => p.Id == objCompType.Id);
                    if (ObjIncomingCompType != null)
                    {
                        DataModel.DeleteObject(ObjIncomingCompType);
                        DataModel.SaveChanges();
                        bValue = true;
                    }
                }
            }
            catch
            {
                bValue = false;
            }

            return bValue;
        }

        public bool FindCompTypeName(string strName)
        {
            bool bValue = false;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.IncomingCompType ObjIncomingCompType = DataModel.IncomingCompTypes.FirstOrDefault(p => p.Names == strName);
                    if (ObjIncomingCompType != null)
                    {
                        bValue = true;
                    }
                }
            }
            catch
            {
                bValue = false;
            }

            return bValue;
        }

        public static int?  GetCompTypeName(string strName)
        {
            int? intCompType =null;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.IncomingCompType ObjIncomingCompType = DataModel.IncomingCompTypes.FirstOrDefault(p => p.Names.ToLower() == strName.ToLower());
                    if (ObjIncomingCompType != null)
                    {
                        intCompType = ObjIncomingCompType.IncomingPaymentTypeID;
                    }
                }
            }
            catch
            {
            }
            return intCompType;
        }
    }

}
