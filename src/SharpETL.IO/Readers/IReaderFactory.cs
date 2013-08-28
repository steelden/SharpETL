using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharpETL.IO.Readers.Options;
using SharpETL.IO.Readers.Xls;
using SharpETL.IO.Readers.Dbf;
using SharpETL.IO.Readers.Ascii;
using SharpETL.IO.Readers.Db;

namespace SharpETL.IO.Readers
{
    public enum ReaderType
    {
        None = 0,
        Excel = 1,
        Dbf = 2,
        Ascii = 3,
        Excel2007 = 4,
        Db = 5
    }

    public interface IReaderFactory
    {
        IFormatReader<XlsReaderOptions> CreateXlsReader(string path, bool useFirstRowAsHeader = false);
        IFormatReader<XlsReaderOptions> CreateXlsReader(XlsReaderOptions options);
        IFormatReader<DbfReaderOptions> CreateDbfReader(string path);
        IFormatReader<DbfReaderOptions> CreateDbfReader(DbfReaderOptions options);
        IFormatReader<AsciiReaderOptions> CreateAsciiReader(string path);
        IFormatReader<AsciiReaderOptions> CreateAsciiReader(AsciiReaderOptions options);
        IFormatReader<DbReaderOptions> CreateDbReader(string id, string connectionString, string name, string query);
        IFormatReader<DbReaderOptions> CreateDbReader(string id, string connectionString, IDictionary<string, string> namedQueries);
        IFormatReader<DbReaderOptions> CreateDbReaderEx(string id, string connectionString);
        IFormatReader<DbReaderOptions> CreateDbReaderEx(string id, string connectionString, string name, string query);
        IFormatReader<DbReaderOptions> CreateDbReaderEx(string id, string connectionString, IDictionary<string, string> namedQueries);
        IFormatReader<IFileOptionsSet> CreateReader(ReaderType type, string path, IOptionsSet baseOptions = null);
        IFormatReader<IFileOptionsSet> CreateReaderByExtension(string path, IOptionsSet baseOptions = null);
        IEnumerable<IFormatReader<IFileOptionsSet>> CreateDirectoryReader(string path, string pattern = null, IOptionsSet baseOptions = null);
    }
}
