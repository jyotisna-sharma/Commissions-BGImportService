using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using MyAgencyVault.BusinessLibrary;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ISettings
    {
        [OperationContract]
        List<ReportCustomFieldSettings> GetReportCustomFieldSettings(Guid licenseeId, Guid reportID);

        [OperationContract]
        void AddUpdate(Guid licenseeId, Guid reportID, string fields);

        [OperationContract]
        void AddUpdateNamedSchedule(PayorIncomingSchedule schedule);
        [OperationContract]
        PayorIncomingSchedule IsNamedScheduleExist(string scheduleName, Guid? incomingScheduleId,Guid licenseeId,out bool isExist);
        [OperationContract]
        List<PayorIncomingSchedule> GetNamedScheduleList(Guid LicenseeId);
        [OperationContract]
        bool CheckNamedscheduleExist(Guid licenseeId);
    }
    public partial class MavService : ISettings
    {
        public List<ReportCustomFieldSettings> GetReportCustomFieldSettings(Guid licenseeId, Guid reportID)
        {
            List<ReportCustomFieldSettings> lst = new List<ReportCustomFieldSettings>();
            try
            {
                lst = Settings.getData(licenseeId, reportID);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetReportCustomFieldSettings Settings:" + ex.Message, true);
            }
            return lst;
        }

        public void AddUpdate(Guid licenseeId, Guid reportID, string fields)
        {
            try
            {
                Settings.AddUpdate(licenseeId, reportID, fields);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdate Settings:" + ex.Message, true);
            }
        }
        public void AddUpdateNamedSchedule(PayorIncomingSchedule schedule)
        {
            try
            {
                Settings.SaveNamedSchedule(schedule);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("AddUpdateNamedSchedule Settings:" + ex.Message, true);
            }
        }
        public PayorIncomingSchedule IsNamedScheduleExist(string scheduleName, Guid? incomingScheduleId,Guid licenseeId, out bool isExist)
        {
            PayorIncomingSchedule schedule = new PayorIncomingSchedule();
            isExist = true;
            try
            {
                schedule = Settings.IsNamedScheduleExist(scheduleName, incomingScheduleId, licenseeId, out bool isrecordExist);
                isExist = isrecordExist;
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("IsNamedScheduleExist Settings:" + ex.Message, true);
            }
            return schedule;
        }
        public List<PayorIncomingSchedule> GetNamedScheduleList(Guid LicenseeId)
        {
            List<PayorIncomingSchedule> list = null;
            try
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetNamedScheduleList LicenseeId:" + LicenseeId, true);
                list = Settings.GetNamedScheduleList(LicenseeId);
            }
            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("GetNamedScheduleList Settings:" + ex.Message, true);
            }
            return list;
        }
        public bool CheckNamedscheduleExist(Guid licenseeId)
        {
            bool isExist = false;
            try
            {
                isExist = Settings.CheckNamedscheduleExist(licenseeId);
            }
           catch(Exception ex)
            {

                ActionLogger.Logger.WriteImportPolicyLog("CheckNamedscheduleExist Settings:" + ex.Message, true);
            }
            return isExist;
        }

    }
}