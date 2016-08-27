using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADV
{
    public class ExportImport
    {
        public string Import()
        {
            string strError = string.Empty;
            try
            {
                byte[] file = File.ReadAllBytes(string.Format(@"{0}\content\files\.xlsx"));
                MemoryStream ms = new MemoryStream(file);
                using (ExcelPackage package = new ExcelPackage(ms))
                {
                    if (package.Workbook.Worksheets.Count == 0)
                        strError = "Your Excel file does not contain any work sheets";
                    else
                    {
                        foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
                        {
                        }
                    }
                }
            }
            catch (Exception error)
            {
                return error.Message;
            }

            return string.Empty;
        }
    }
}
