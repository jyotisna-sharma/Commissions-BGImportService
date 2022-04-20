using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAgencyVault.BusinessLibrary.Base;
using System.Runtime.Serialization;
using DLinq = DataAccessLayer.LinqtoEntity;
using DataAccessLayer.LinqtoEntity;
using MyAgencyVault.BusinessLibrary.Masters;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;


namespace MyAgencyVault.BusinessLibrary
{
    public class ExcelUtility : IDisposable
    {
        private Excel.Application xlApp;
        private Excel.Workbook xlWorkBook;
        private Excel.Worksheet xlWorkSheet;
        private Template template;
        List<FieldMapping> OrderedFields;
        private int StartDataRow;
        private int EndDataRow;
        private int StartDataColumn;
        private int EndDataColumn;
        public bool IsValidFile = false;

        public ExcelUtility(string FileName,Template template)
        {
            this.template = template;
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkBook = xlApp.Workbooks.Open(FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = null;

            for (int count = 1; count <= xlWorkBook.Worksheets.Count; count++)
            {
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(count);
                if (xlWorkSheet.Name == template.SheetName)
                    break;
            }
            OrderedFields = template.FieldMappings.ToList();

            StartDataColumn = getStartDataColumn();
            EndDataColumn = getEndDataColumn();

            StartDataRow = getDataStartRow();
            EndDataRow = getDataEndRow();
           
            IsValidFile = ValidateAgainstTemplate();
        }

        private int getStartDataColumn()
        {
            string value = null;
            int StartIndex = 1;
            string []Fields = template.ExcelColumnList.Split(',');

            for (;StartIndex < 100 ;StartIndex++)
            {
                if ((xlWorkSheet.Cells[template.DataStartIndex, StartIndex]).Value != null)
                {
                    value = (xlWorkSheet.Cells[template.DataStartIndex, StartIndex]).Value;
                    if (value == Fields[0])
                        break;
                }
            }

            if (StartIndex == 100)
                return -1;
            else
                return StartIndex;
        }

        private int getEndDataColumn()
        {
            string value = null;
            int StartIndex = 1;
            string[] Fields = template.ExcelColumnList.Split(',');

            for (; StartIndex < 100; StartIndex++)
            {
                if ((xlWorkSheet.Cells[template.DataStartIndex, StartIndex]).Value != null)
                {
                    value = (xlWorkSheet.Cells[template.DataStartIndex, StartIndex]).Value;
                    if (value == Fields[Fields.Length - 1])
                        break;
                }
            }

            if (StartIndex == 100)
                return -1;
            else
                return StartIndex;
        }

        public bool ValidateAgainstTemplate()
        {
            string value = string.Empty;
            for (int columIndex = StartDataColumn; columIndex <= EndDataColumn; columIndex++)
            {
                value += xlWorkSheet.Cells[template.DataStartIndex, columIndex].Value2.ToString() + ",";
            }
            value = value.Remove(value.Length - 1, 1);

            if (value == template.ExcelColumnList)
                return true;
            else
                return false;
        }

        public int getDataStartRow()
        {
            return template.DataStartIndex.Value + 1;
        }

        public int getDataEndRow()
        {
            int firstRow = getDataStartRow();
            int rowIndex = firstRow;
            int lastBlankRowCount = 0;

            for (;; rowIndex++)
            {
                bool DataFound = false;
                for (int StartColumn = StartDataColumn; StartColumn <= EndDataColumn;StartColumn++)
                {
                    string value = null;
                    if ((xlWorkSheet.Cells[rowIndex, StartColumn]).Value != null)
                        value = (string)(xlWorkSheet.Cells[rowIndex, StartColumn]).Value.ToString();

                    if (!string.IsNullOrEmpty(value))
                    {
                        DataFound = true;
                        break;
                    }
                }

                if (DataFound == false)
                {
                    if (lastBlankRowCount != 5)
                        lastBlankRowCount++;
                    else
                        break;
                }
                else
                    lastBlankRowCount = 0; 
            }
            return rowIndex - template.LastRowsToSkip.Value - lastBlankRowCount - 1;
        }

        private string GetExcelFileData(int Row)
        {
            string DataList = string.Empty;
            Range range = xlWorkSheet.UsedRange;

            foreach (FieldMapping field in OrderedFields)
            {
                switch (field.DBFieldName)
                {
                    case "PolicyNumber":
                    case "Insured":
                    case "PolicyMode":
                    case "Carrier":
                    case "Product":
                    case "PayorSysId":
                    case "CompScheduleType":
                    case "Client":
                        string stringValue = (range.Cells[Row, field.ExcelFieldNo] as Range).Value;
                        DataList += stringValue + "#";
                        break;

                    case "OriginalEffectiveDate":
                    case "InvoiceDate":
                        DateTime conv = (range.Cells[Row, field.ExcelFieldNo] as Range).Value;
                        DataList += conv.ToString("MM/dd/yyyy") + "#";
                        break;

                    case "CommissionPercentage":
                    case "SplitPercentage":
                    case "Renewal":
                        double? dblValue = (range.Cells[Row, field.ExcelFieldNo] as Range).Value as double?;
                        if (dblValue.HasValue)
                        {
                            DataList += (dblValue.Value * 100).ToString() + "#";
                        }
                        else
                        {
                            string strValue = (range.Cells[Row, field.ExcelFieldNo] as Range).Value as string;
                            double dbl = double.Parse(strValue);
                            DataList += dbl.ToString() + "#";
                        }
                        break;

                    case "PaymentReceived":
                    case "Fee":
                    case "Bonus":
                    case "CommissionTotal":
                    case "DollerPerUnit":
                        decimal? decimalValue = (range.Cells[Row, field.ExcelFieldNo] as Range).Value as decimal?;
                        if (decimalValue.HasValue)
                        {
                            DataList += decimalValue.Value.ToString() + "#";
                        }
                        else
                        {
                            string strValue = (range.Cells[Row, field.ExcelFieldNo] as Range).Value as string;
                            decimal dbl = decimal.Parse(strValue);
                            DataList += dbl.ToString() + "#";
                        }
                        
                        break;

                    case "Enrolled":
                    case "Eligible":
                    case "NumberOfUnits":
                        double? dblValue1 = (range.Cells[Row, field.ExcelFieldNo] as Range).Value as double?;
                        if (dblValue1.HasValue)
                        {
                            int intValue = (int)dblValue1.Value;
                            DataList += intValue.ToString() + "#";
                        }
                        else
                        {
                            string strValue = (range.Cells[Row, field.ExcelFieldNo] as Range).Value as string;
                            int intVl = int.Parse(strValue);
                            DataList += intVl.ToString() + "#";
                        }
                        break;
                    
                    //case "CompType":
                    //    if (string.IsNullOrEmpty(Data[DataIndex]))
                    //        DeuEntry.CompTypeID = null;
                    //    else
                    //        DeuEntry.CompTypeID = DEU.getCompTypeId(field.DeuFieldValue);
                    //    deuData.CompTypeID = DeuEntry.CompTypeID;
                    //    break;
                }
            }

            DataList = DataList.Remove(DataList.Length - 1,1);
            return DataList;
        }

        private int CurrentDataRow;
        public void Reset()
        {
            CurrentDataRow = getDataStartRow();
        }

        public string Read()
        {
            if (CurrentDataRow > EndDataRow)
                return string.Empty;

            string data = GetExcelFileData(CurrentDataRow);
            CurrentDataRow++;
            return data;
        }

        public void Dispose()
        {
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
        }
    }
}
