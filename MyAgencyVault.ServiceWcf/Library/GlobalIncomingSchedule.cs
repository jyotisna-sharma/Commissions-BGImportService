using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IGlobalIncomingSchedule
    {
        [OperationContract]
        void AddUpdateGlobalIncomingSchedule(GlobalIncomingSchedule IncomingSch);

        [OperationContract]
        GlobalIncomingSchedule GetGlobalIncomingSchedule(Guid carrierId, Guid coverageId);

        [OperationContract]
        void ChangeScheduleType(Guid carrierId, Guid coverageId, int scheduleType);

        [OperationContract]
        void SavePayorSchedules(PayorIncomingSchedule schedule, int overwrite=0);

        [OperationContract]
        void DeleteSchedule(Guid ScheduleID);

        [OperationContract]
        List<PayorIncomingSchedule> GetAllSchedules(Guid LicenseeID);

        [OperationContract]
        PayorIncomingSchedule GetPayorScheduleDetails(Guid PayorID, Guid CarrierID, Guid CoverageID, Guid LicenseeID, string ProductType, int IncomingPaymentTypeID);
    }

    public partial class MavService : IGlobalIncomingSchedule
    {
        #region IGlobalIncomingSchedule Members


        public void AddUpdateGlobalIncomingSchedule(GlobalIncomingSchedule IncomingSch)
        {
            IncomingSchedule.AddUpdateGlobalSchedule(IncomingSch);
        }

        public void ChangeScheduleType(Guid carrierId, Guid coverageId, int scheduleType)
        {
            IncomingSchedule.ChangeScheduleType(carrierId, coverageId, scheduleType);
        }

        public GlobalIncomingSchedule GetGlobalIncomingSchedule(Guid carrierId, Guid coverageId)
        {
            return IncomingSchedule.GetGlobalIncomingSchedule(carrierId, coverageId);
        }

        #endregion

        #region Payor Schedules
        public void SavePayorSchedules(PayorIncomingSchedule schedule, int overwrite = 0)
        {
            PayorIncomingSchedule.SaveSchedule(schedule, overwrite);
        }

        public void DeleteSchedule(Guid ScheduleID)
        {
            PayorIncomingSchedule.DeleteSchedule(ScheduleID);
        }

        public List<PayorIncomingSchedule> GetAllSchedules(Guid LicenseeID)
        {
            return PayorIncomingSchedule.GetAllSchedules(LicenseeID);
        }

        public PayorIncomingSchedule GetPayorScheduleDetails(Guid PayorID, Guid CarrierID, Guid CoverageID, Guid LicenseeID, string ProductType, int IncomingPaymentTypeID)
        {
            return PayorIncomingSchedule.GetPayorScheduleDetails(PayorID, CarrierID, CoverageID,LicenseeID, ProductType, IncomingPaymentTypeID);
        }
        #endregion
    }
}