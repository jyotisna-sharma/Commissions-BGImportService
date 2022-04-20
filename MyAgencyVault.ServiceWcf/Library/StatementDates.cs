using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface IStatementDates
    {
        [OperationContract]
        void AddUpdateStatementDates(List<StatementDates> StatementDate);

        [OperationContract]
        void DeleteStatementDates(List<StatementDates> StatementDate);

        [OperationContract]
        List<StatementDates> GetStatementDates();

        [OperationContract]
        List<StatementDates> GetActiveStatementDates();

        [OperationContract]
        void MarkAsBatchGenerated(List<StatementDates> Dates);

    }

    public partial class MavService : IStatementDates
    {
        public void AddUpdateStatementDates(List<StatementDates> StatementDate)
        {
            StatementDates.AddUpdate(StatementDate);
        }

        public void DeleteStatementDates(List<StatementDates> StatementDate)
        {
            StatementDates.Delete(StatementDate);
        }

        public List<StatementDates> GetStatementDates()
        {
            return StatementDates.GetStatementDate();
        }

        public List<StatementDates> GetActiveStatementDates()
        {
            return StatementDates.GetActiveStatementDates();
        }

        public void MarkAsBatchGenerated(List<StatementDates> Dates)
        {
            StatementDates.MarkAsBatchGenerated(Dates);
        }
    }
}
