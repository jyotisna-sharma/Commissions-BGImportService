using System;
using System.Collections.Generic;
using System.Linq;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DataAccessLayer.LinqtoEntity;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Collections.ObjectModel;
using MyAgencyVault.EmailFax;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class DisplayBrokerCode
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public Guid? licenseeID { get; set; }
        [DataMember]
        public Guid? payorID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public DateTime? dtCreatedDate { get; set; }
        [DataMember]
        public DateTime? dtModifiedDate { get; set; }

    }

    [DataContract]
    public class ImportToolBrokerSetting
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FixedRows { get; set; }
        [DataMember]
        public string FixedColumns { get; set; }
        [DataMember]
        public string RelativeSearchtext { get; set; }
        [DataMember]
        public string RelativeRows { get; set; }
        [DataMember]
        public string RelativeColumns { get; set; }

    }

    [DataContract]
    public class PayorPhrase
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int? StatementDataSettingsID { get; set; }
        [DataMember]
        public int? MasterStatementDataID { get; set; }
    }

    [DataContract]
    public class TemplatePhrase
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int? StatementDataSettingsID { get; set; }
        [DataMember]
        public int? MasterStatementDataID { get; set; }
    }

    [DataContract]
    public class ImportToolMasterStatementData
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Statementdata { get; set; }
        [DataMember]
        public string StatementDataType { get; set; }
    }

    public class Brokercode
    {
        public void AddBrokerCode(DisplayBrokerCode objBrokerCode)
        {
            try
            {
                //var BrokerValue = null;
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var BrokerValue = new DLinq.BrokerCode
                     {
                         LicenseeId = (Guid)objBrokerCode.licenseeID,
                         PayorId = objBrokerCode.payorID,
                         Code = objBrokerCode.Code,
                         Createddate = System.DateTime.Now,
                         LastModifiedDate = System.DateTime.Now
                     };

                    DataModel.AddToBrokerCodes(BrokerValue);
                    DataModel.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public void AddImportToolBrokerSettings(ImportToolBrokerSetting objImportToolBrokerSetting)
        {
            try
            {
                //var BrokerValue = null;
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var BrokerValue = new DLinq.ImportToolBrokersSetting
                    {
                        FixedRows = objImportToolBrokerSetting.FixedRows,
                        FixedColumns = objImportToolBrokerSetting.FixedColumns,
                        RelativeSearchtext = objImportToolBrokerSetting.RelativeSearchtext,
                        RelativeRows = objImportToolBrokerSetting.RelativeRows,
                        RelativeColumns = objImportToolBrokerSetting.RelativeColumns
                    };

                    DataModel.AddToImportToolBrokersSettings(BrokerValue);
                    DataModel.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public void UpdateBrokerCode(DisplayBrokerCode objBrokerCode)
        {
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    DLinq.BrokerCode BrokerValue = (from p in DataModel.BrokerCodes.Where(b => b.Id == objBrokerCode.Id) select p).FirstOrDefault();

                    if (BrokerValue != null) //Update record 
                    {
                        BrokerValue.LicenseeId = (Guid)objBrokerCode.licenseeID;
                        BrokerValue.PayorId = objBrokerCode.payorID;
                        BrokerValue.Code = objBrokerCode.Code;
                        BrokerValue.LastModifiedDate = System.DateTime.Now;
                    }
                    DataModel.SaveChanges();
                }
            }
            catch
            {
            }
        }

        public bool ValidateBrokerCode(string strBrokerCode)
        {
            DisplayBrokerCode objBrokers = new DisplayBrokerCode();
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    var value = DataModel.BrokerCodes.Where(b => b.Code.ToLower() == strBrokerCode.ToLower()).FirstOrDefault();
                    if (value != null)
                    {
                        objBrokers.licenseeID = value.LicenseeId;
                        objBrokers.Code = value.Code;
                        objBrokers.payorID = value.PayorId;
                        objBrokers.dtCreatedDate = value.Createddate;
                        objBrokers.dtModifiedDate = value.LastModifiedDate;
                    }
                }
            }
            catch
            {
            }
            if (string.IsNullOrEmpty(objBrokers.Code))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public List<DisplayBrokerCode> LoadBrokerCode(Guid? lincessID)
        {
            List<DisplayBrokerCode> lstBrokerCode = null;

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    lstBrokerCode = (from p in DataModel.BrokerCodes
                                     where (p.LicenseeId == lincessID)
                                     select new DisplayBrokerCode
                                     {
                                         Id = p.Id,
                                         licenseeID = p.LicenseeId,
                                         payorID = p.PayorId,
                                         Code = p.Code,
                                         dtCreatedDate = p.Createddate,
                                         dtModifiedDate = p.LastModifiedDate,

                                     }).ToList();


                }
            }
            catch
            {
            }
            return lstBrokerCode;
        }

        public List<DisplayBrokerCode> GetBrokerCode(string strBrokerCode)
        {
            List<DisplayBrokerCode> lstBrokerCode = null;          

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    lstBrokerCode = (from p in DataModel.BrokerCodes
                                     where (p.Code.ToLower() == strBrokerCode.ToLower())
                                     select new DisplayBrokerCode
                                     {
                                         Id = p.Id,
                                         licenseeID = p.LicenseeId,
                                         payorID = p.PayorId,
                                         Code = p.Code,
                                         dtCreatedDate = p.Createddate,
                                         dtModifiedDate = p.LastModifiedDate,

                                     }).ToList();


                }
            }
            catch
            {
            }
            return lstBrokerCode;
        }

        public List<DisplayBrokerCode> GetBrokerCodeByBrokerName(string strBrokerCode)
        {
            List<DisplayBrokerCode> lstBrokerCode = null;

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    lstBrokerCode = (from p in DataModel.BrokerCodes
                                     where (p.Code.ToLower() == strBrokerCode.ToLower())
                                     select new DisplayBrokerCode
                                     {
                                         Id = p.Id,
                                         licenseeID = p.LicenseeId,
                                         payorID = p.PayorId,
                                         Code = p.Code,
                                         dtCreatedDate = p.Createddate,
                                         dtModifiedDate = p.LastModifiedDate,

                                     }).ToList();


                }
            }
            catch
            {
            }
            return lstBrokerCode;
        }

        public List<ImportToolBrokerSetting> LoadImportToolBrokerSetting()
        {
            List<ImportToolBrokerSetting> objImportToolBrokerSetting = new List<ImportToolBrokerSetting>();

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    objImportToolBrokerSetting = (from p in DataModel.ImportToolBrokersSettings
                                                  select new ImportToolBrokerSetting
                                                  {
                                                      Id = p.ID,
                                                      FixedRows = p.FixedRows,
                                                      FixedColumns = p.FixedColumns,
                                                      RelativeSearchtext = p.RelativeSearchtext,
                                                      RelativeRows = p.RelativeRows,
                                                      RelativeColumns = p.RelativeColumns

                                                  }).ToList();
                }
            }
            catch(Exception)
            {
                
            }
            return objImportToolBrokerSetting;
        }

        public bool DeleteBrokerCode(DisplayBrokerCode ObjBrokerCode)
        {
            bool bValue = false;
            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    if (ObjBrokerCode != null)
                    {
                        DLinq.BrokerCode delObject = DataModel.BrokerCodes.FirstOrDefault(p => p.Id == ObjBrokerCode.Id);

                        if (delObject != null)
                        {
                            DataModel.DeleteObject(delObject);
                            DataModel.SaveChanges();
                            bValue = true;
                        }
                    }
                }
            }
            catch
            {
            }
            return bValue;
        }

        public void NotifyMail(MailData _MailData, string strSubject, string strMailBody)
        {
            _MailData.CommDeptMail = Masters.SystemConstant.GetKeyValue("CommDeptMailId");
            _MailData.HostName = Masters.SystemConstant.GetKeyValue("MailServerName");
            _MailData.Port = Masters.SystemConstant.GetKeyValue("MailServerPortNo");
            _MailData.Password = Masters.SystemConstant.GetKeyValue("MailServerPassword");
            _MailData.UserName = Masters.SystemConstant.GetKeyValue("MailServerUserName");
            new EmailFax.OutLookEmailFax(_MailData).SendNotificationMail(_MailData, strSubject, strMailBody);
        }

        public List<ImportToolMasterStatementData> LoadImportToolMasterStatementData()
        {
            List<ImportToolMasterStatementData> objImportToolMasterStatementData = new List<ImportToolMasterStatementData>();

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    objImportToolMasterStatementData = (from p in DataModel.ImportToolMasterStatementDatas
                                                        select new ImportToolMasterStatementData
                                                        {
                                                            ID = p.ID,
                                                            Statementdata = p.Statementdata,
                                                            StatementDataType = p.StatementDataType
                                                        }).ToList();
                }
            }
            catch
            {
            }

            return objImportToolMasterStatementData;
        }
    }
}
