using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLinq = DataAccessLayer.LinqtoEntity;

namespace MyAgencyVault.BusinessLibrary
{
    public static class BLHelper
    {
        public static string CorrectPolicyNo(string value)
        {
            value = value.Trim();
            value = value.Replace(" ", "");

            bool IsCharAddedToStringBuilder = false;
            StringBuilder stringBuilder = new StringBuilder(50);

            foreach (char c in value)
            {
                if (!IsCharAddedToStringBuilder && c == '0')
                {
                    continue;
                }

                if (char.IsLetterOrDigit(c))
                {
                    stringBuilder.Append(c);
                    IsCharAddedToStringBuilder = true;
                }
            }

            return stringBuilder.ToString();
        }

        public static int GetPolicyMode(string value)
        {
            try
            {

                int PolicyMode = 0;
                string firstCharachterValue = value.Substring(0, 1);
                value = value.Trim();
                if (firstCharachterValue.ToUpper() == "M" || value == "01" || value == "1")
                    PolicyMode = 0;
                else if (firstCharachterValue.ToUpper() == "Q" || value == "03" || value == "3")
                    PolicyMode = 1;
                else if (firstCharachterValue.ToUpper() == "S" || value == "06" || value == "6")
                    PolicyMode = 2;
                else if (firstCharachterValue.ToUpper() == "A" || value == "12")
                    PolicyMode = 3;
                else if (firstCharachterValue.ToUpper() == "O")
                    PolicyMode = 4;

                return PolicyMode;
            }
            catch
            {
                return 0;
            }
        }

        public static int getCompTypeId(string value)
        {
            int CompTypeId = 0;
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim().ToUpper();

                    if (value == "C")
                        CompTypeId = 1;
                    else if (value == "O")
                        CompTypeId = 2;
                    else if (value == "B")
                        CompTypeId = 3;
                    else if (value == "F")
                        CompTypeId = 4;
                }
            }
            catch
            {
            }
            return CompTypeId;
        }

        public static int getCompTypeIdByName(string value)
        {
            ActionLogger.Logger.WriteImportPolicyLog("getCompTypeIdByName req: " + value, true);

            //Pending status
            int CompTypeId = 5;
            try
            {
                if (string.IsNullOrEmpty(value))
                {   //Default
                    return CompTypeId = 5;
                }
                CompType objCompType = new CompType();
                List<CompType> onjList = objCompType.GetAllComptype();

                ActionLogger.Logger.WriteImportPolicyLog("getCompTypeIdByName onjList- " + onjList.ToStringDump(), true);

                CompType objComp = onjList.Where(p => p.Names.ToLower() == value.Trim().ToLower()).FirstOrDefault();
                if (objComp != null)
                {
                    ActionLogger.Logger.WriteImportPolicyLog("getCompTypeIdByName object found: " + objComp.ToStringDump(), true);
                    if (objComp.IncomingPaymentTypeID != null)
                    {
                        CompTypeId = Convert.ToInt32(objComp.IncomingPaymentTypeID);
                    }
                }
                ActionLogger.Logger.WriteImportPolicyLog("getCompTypeIdByName result: " + CompTypeId, true);

            }

            catch (Exception ex)
            {
                ActionLogger.Logger.WriteImportPolicyLog("getCompTypeIdByName ex: " + ex.Message, true);
            }

            return CompTypeId;
        }

        public static Guid? GetClientId(string name, Guid licId)
        {
            DLinq.Client client = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                client = DataModel.Clients.FirstOrDefault(s => s.Name.ToUpper() == name.ToUpper() && s.LicenseeId == licId && s.IsDeleted == false);

                if (client != null)
                    return client.ClientId;
                else
                    return null;
            }
        }

        public static Guid? GetCarrierId(string nickName, Guid payorId)
        {
            DLinq.CarrierNickName CarrierNickNameRow = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                CarrierNickNameRow = DataModel.CarrierNickNames.FirstOrDefault(s => s.NickName.ToUpper() == nickName.ToUpper() && s.PayorId == payorId && s.IsDeleted == false);

                if (CarrierNickNameRow != null)
                    return CarrierNickNameRow.CarrierId;
                else
                    return null;
            }
        }

        public static Guid? GetProductId(string nickName, Guid payorId, Guid? carrierId)
        {
            if (carrierId == null)
                return null;

            DLinq.CoverageNickName ProductNickNameRow = null;
            using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
            {
                ProductNickNameRow = DataModel.CoverageNickNames.FirstOrDefault(s => s.NickName.ToUpper() == nickName.ToUpper() && s.PayorId == payorId && s.CarrierId == carrierId && s.IsDeleted == false);

                if (ProductNickNameRow != null)
                    return ProductNickNameRow.CoverageId;
                else
                    return null;
            }
        }

        public static Guid? GetProductId(string nickName, Guid payorId)
        {

            DLinq.CoverageNickName ProductNickNameRow = null;
            Guid? ProductId = null;

            if (string.IsNullOrEmpty(nickName))
            {
                return ProductId;
            }

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    ProductNickNameRow = DataModel.CoverageNickNames.FirstOrDefault(s => s.NickName.ToUpper() == nickName.ToUpper() && s.PayorId == payorId && s.IsDeleted == false);

                    if (ProductNickNameRow != null)
                        ProductId = ProductNickNameRow.CoverageId;
                    else
                        ProductId = null;
                }
            }
            catch
            {
            }
            return ProductId;
        }

        public static Guid? GetProductIdByProductType(string strProductType, Guid payorId, string policyNumber)
        {

            DLinq.PolicyLearnedField PolicyLearnedProductType = null;
            Guid? ProductId = null;

            if (string.IsNullOrEmpty(strProductType))
            {
                return ProductId;
            }

            if (string.IsNullOrEmpty(policyNumber))
            {
                return ProductId;
            }

            try
            {
                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    PolicyLearnedProductType = DataModel.PolicyLearnedFields.FirstOrDefault(s => s.ProductType != null && s.ProductType.ToUpper() == strProductType.ToUpper() && s.PolicyNumber != null && s.PolicyNumber.ToUpper() == policyNumber && s.PayorId == payorId);
                    if (PolicyLearnedProductType != null)
                    {
                        ProductId = PolicyLearnedProductType.CoverageId;
                    }
                    else
                    {
                        ProductId = null;
                    }                    
                }
            }
            catch
            {
            }
            return ProductId;
        }

        public static Guid? GetProductIdByCoverageNickName(string strProductType, Guid payorId, string policyNumber)
        {
            DLinq.PolicyLearnedField PolicyLearnedCoverageNickName = null;
            Guid? ProductId = null;

            if (string.IsNullOrEmpty(strProductType))
            {
                return ProductId;
            }

            if (string.IsNullOrEmpty(policyNumber))
            {
                return ProductId;
            }

            try
            {

                using (DLinq.CommissionDepartmentEntities DataModel = Entity.DataModel)
                {
                    PolicyLearnedCoverageNickName = DataModel.PolicyLearnedFields.FirstOrDefault(s => s.CoverageNickName != null && s.CoverageNickName.ToUpper() == strProductType.ToUpper() && s.PolicyNumber != null && s.PolicyNumber.ToUpper() == policyNumber && s.PayorId == payorId);
                    if (PolicyLearnedCoverageNickName != null)
                    {
                        ProductId = PolicyLearnedCoverageNickName.CoverageId;
                    }
                    else
                    {
                        ProductId = null;
                    }
                }
            }
            catch
            {
            }

            return ProductId;
        }
    }
}
