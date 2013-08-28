using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SharpETL.IO.Readers.Options;
using SharpETL.IO.Readers.Xls;

namespace SharpETL.IO.Readers
{
    public static class ExtensionsDataSet
    {
        public static DataSet ApplyRules(this DataSet ds, XlsReaderOptions options)
        {
            var newDs = ds.Copy();

            if (!String.IsNullOrEmpty(options.CommentSymbol)) {
                var _commentSymbol = options.CommentSymbol;
                foreach (DataTable table in ds.Tables) {
                    if (table.TableName.StartsWith(_commentSymbol)) {
                        newDs.Tables.Remove(table.TableName);
                    } else {
                        foreach (DataColumn column in table.Columns) {
                            if (column.ColumnName.StartsWith(_commentSymbol)) {
                                newDs.Tables[table.TableName].Columns.Remove(newDs.Tables[table.TableName].Columns[column.ColumnName]);
                            }
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(options.SheetNames)) {
                var _sheets = options.SheetNames.Split(',').Select(x => x.Trim()).ToList();
                foreach (DataTable table in ds.Tables) {
                    if (_sheets != null && !_sheets.Contains(table.TableName)) {
                        newDs.Tables.Remove(table.TableName);
                    }
                }
            }
            return newDs;
        }
    }
}
