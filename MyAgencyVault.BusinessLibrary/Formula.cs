using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{
    [DataContract]
    public class Formula
    {
        public static DLinq.Formula AddUpdate(Formula formula, CommissionDepartmentEntities DataModel)
        {

            var tempFormula = (from se in DataModel.Formulas
                               where formula.FormulaID == se.FormulaId
                               select se).FirstOrDefault();

            if (tempFormula != null)
            {
                tempFormula.FormulaExpression = formula.FormulaExpression;
                tempFormula.FormulaTtitle = formula.FormulaTtitle;
                tempFormula.IsDeleted = false;
            }
            else
            {
                DLinq.Formula formulaRow = new DLinq.Formula();
                if (formula.FormulaID == Guid.Empty)
                    formulaRow.FormulaId = Guid.NewGuid();
                else
                    formulaRow.FormulaId = formula.FormulaID;

                formulaRow.FormulaExpression = formula.FormulaExpression;
                formulaRow.FormulaTtitle = formula.FormulaTtitle;
                formulaRow.IsDeleted = false;

                DataModel.AddToFormulas(formulaRow);
                tempFormula = formulaRow;
            }
            DataModel.SaveChanges();
            return tempFormula;
        }

        public static void Delete(Guid formulaId, CommissionDepartmentEntities DataModel)
        {
            var tempFormula = (from se in DataModel.Formulas
                               where formulaId == se.FormulaId
                               select se).FirstOrDefault();

            if (tempFormula != null)
            {
                tempFormula.IsDeleted = true;
                DataModel.SaveChanges();
            }

        }

        public static Formula GetFormula()
        {
            return null;
        }

        #region  "Data Members aka- public properties"
        [DataMember]
        public Guid FormulaID { get; set; }
        [DataMember]
        public string FormulaTtitle { get; set; }
        [DataMember]
        public string FormulaExpression { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }             
        #endregion 
    }
}
