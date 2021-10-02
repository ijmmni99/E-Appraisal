using System;
using System.Collections.Generic;
using System.Linq;
using ExcelDataReader;
using System.Web;
using System.IO;
using System.Data;

namespace E_Appraisal.Models
{
    public class ExcelData
    {
        string _path;
        public ExcelData(string path)
        {
            _path = path;
        }

        public IExcelDataReader getExcelReader()
        {
            // ExcelDataReader works with the binary Excel file, so it needs a FileStream
            // to get started. This is how we avoid dependencies on ACE or Interop:
            FileStream stream = File.Open(_path, FileMode.Open, FileAccess.Read);
            //
            // We return the interface, so that
            IExcelDataReader reader = null;
            try
            {
                if (_path.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                if (_path.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                if (_path.EndsWith(".ods"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                return reader;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<string> getWorksheetNames()
        {
            var reader = this.getExcelReader();
            var workbook = reader.AsDataSet();
            var sheets = from DataTable sheet in workbook.Tables select sheet.TableName;
            return sheets;
        }
        public IEnumerable<DataRow> getData(string sheet, bool firstRowIsColumnNames = true)
        {
            var reader = this.getExcelReader();
            reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = firstRowIsColumnNames
                }
            });
            //string SheetName = getWorksheetNames().FirstOrDefault();
            var workSheet = reader.AsDataSet().Tables[sheet];
            var filteredRows = workSheet.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is System.DBNull)));
            // TTliteUtil.Util.WriteToEventLog("Sheet read: "+workSheet.ToString());
            var rows = from DataRow a in filteredRows select a;
            reader.Close();
            return rows;
        }
    }
}