using System;
using System.Collections.Generic;
using System.ServiceModel;
using MyAgencyVault.BusinessLibrary;
using MyAgencyVault.BusinessLibrary.Masters;
using System.Threading;

namespace MyAgencyVault.WcfService
{
    [ServiceContract]
    interface ICalculateVariableService
    {
        [OperationContract]
        LicenseeVariableOutputDetail StartCalculation(LicenseeVariableInputDetail inputData);
    }

    public partial class MavService : ICalculateVariableService
    {
        public LicenseeVariableOutputDetail StartCalculation(LicenseeVariableInputDetail inputData)
        {
            VariableCalculation VarCalc = new VariableCalculation();
            VarCalc.LicenseeInputInfo = inputData;
            VarCalc.LicenseeVariableInfo = new LicenseeVariableOutputDetail();
            VarCalc.StartVariableCalculation();

            return VarCalc.LicenseeVariableInfo;
        }
    }
}