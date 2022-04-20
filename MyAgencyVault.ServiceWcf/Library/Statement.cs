using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary;
using System.ServiceModel;

namespace MyAgencyVault.WcfService
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    interface IStatement
    {
        [OperationContract]
        void PostStatement(Statement Stment);

        [OperationContract]
        bool CloseStatement(Statement Stment);

        [OperationContract]
        bool CloseStatementFromDeu(Statement Stment);

        [OperationContract]
        bool OpenStatement(Statement Stment);

        [OperationContract]
        Statement GetFindStatement(int StatementNumber);

        [OperationContract]
        int AddUpdateStatement(Statement Stment);

        [OperationContract]
        bool DeleteStatement(Guid StatementId, UserRole _UserRole, string OpertaionType);

        [OperationContract]
        Statement GetStatement(Guid StatementID);

        [OperationContract]
        ModifiableStatementData UpdateCheckAmount(Guid statementId, decimal checkAmount, decimal dcNetAmount, decimal adjustment);

        [OperationContract]
        void UpdateStatementDate(Guid StatementID, DateTime NewDate);
    }

    public partial class MavService : IStatement
    {

        public void PostStatement(Statement Stment)
        {
            Stment.PostStatement();
        }

        public bool CloseStatement(Statement Stment)
        {
            return Stment.CloseStatement();
        }

        public bool OpenStatement(Statement Stment)
        {
            return Stment.OpenStatement();
        }

        public Statement GetFindStatement(int StatementNumber)
        {
            return Statement.GetFindStatement(StatementNumber);
        }

        public int AddUpdateStatement(Statement Stment)
        {
            return Stment.AddUpdate();
        }

        public bool DeleteStatement(Guid StatementId, UserRole _UserRole, string OpertaionType)
        {
            return Statement.DeleteStatement(StatementId, _UserRole, OpertaionType);
        }

        public Statement GetStatement(Guid StatementID)
        {
            return Statement.GetStatement(StatementID);
        }

        public ModifiableStatementData UpdateCheckAmount(Guid statementId, decimal checkAmount, decimal dcNetAmount, decimal adjustment)
        {
            return Statement.UpdateCheckAmount(statementId, checkAmount, dcNetAmount, adjustment);
        }

        public bool CloseStatementFromDeu(Statement objStatement)
        {
            return objStatement.CloseStatementFromDeu(objStatement);
        }

        public void UpdateStatementDate(Guid StatementID, DateTime NewDate)
        {
            Statement.UpdateStatementDateFromDEU(StatementID, NewDate);
        }
    }
}
