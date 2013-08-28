using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Data.OleDb;
using System.Collections;
using Excel;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers.Xls
{
    public class XlsReader : FormatReaderBase<XlsReaderOptions>
    {
        public XlsReader(string path) : this(new XlsReaderOptions(path)) { }
        public XlsReader(string path, bool isFirstRowAsHeader) : this(new XlsReaderOptions(path, isFirstRowAsHeader)) { }
        public XlsReader(IOptionsSet options) : base(new XlsReaderOptions(options)) { }

        public override DataSet ToDataSet()
        {
            DataSet dataSet = new DataSet();
            string path = GetValidFilePath();
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                IExcelDataReader excelReader = GetDataReader(path, stream);
                excelReader.IsFirstRowAsColumnNames = Options.UseFirstRowAsHeader;
                dataSet = excelReader.AsDataSet(true).ApplyRules(Options);
                dataSet.DataSetName = stream.Name;
                excelReader.Close();
            }
            return dataSet;
        }

        private IExcelDataReader GetDataReader(string path, Stream stream)
        {
            IExcelDataReader reader = null;
            if (path.EndsWith(".xlsx") || path.EndsWith(".xlsm")) {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            } else if (path.EndsWith(".xls") || path.EndsWith(".xlm")) {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            } else {
                try {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                catch {
                    try {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    catch (Exception ex) {
                        throw new UnimplementedReaderTypeException(ReaderType.Excel, ex);
                    }
                }
            }
            return reader;
        }
    }
}
