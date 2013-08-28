using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers.Dbf
{
    public class DbfReader : FormatReaderBase<DbfReaderOptions>
    {
        public DbfReader(string path) : this(new DbfReaderOptions(path)) { }
        public DbfReader(IOptionsSet options) : base(new DbfReaderOptions(options)) { }

        public override DataSet ToDataSet()
        {
            string name = GetValidFilePath().ToLower();
            DataSet dataSet = new DataSet(name);
            string query = string.Format("select * from {0}", Path.GetFileName(name));
            using (OleDbConnection conn = new OleDbConnection(GetConnectionString()))
            using (OleDbCommand command = new OleDbCommand(query, conn))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command)) {
                adapter.Fill(dataSet, Guid.NewGuid().ToString());
            }
            return dataSet;
        }

        private string GetConnectionString()
        {
            string path = GetValidFilePath();
            return String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=dBASE IV;", Path.GetDirectoryName(path));
        }
    }
}
