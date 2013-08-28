using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using SharpETL.IO.Readers.Ascii;
using SharpETL.IO.Readers.Dbf;
using SharpETL.IO.Readers.Xls;
using SharpETL.IO.Readers.Options;
using SharpETL.IO.Readers.Db;

namespace SharpETL.IO.Readers
{
    public class UnimplementedReaderTypeException : Exception
    {
        private const string message = "Unsupported format '{0}'.";
        private static string GetMessage(ReaderType type) { return String.Format(message, type); }
        
        public UnimplementedReaderTypeException(ReaderType type) : base(GetMessage(type)) { }
        public UnimplementedReaderTypeException(ReaderType type, Exception innerException) : base(GetMessage(type), innerException) { }
    }

    public class ReaderFactory : IReaderFactory
    {
        public IFormatReader<XlsReaderOptions> CreateXlsReader(string path, bool useFirstRowAsHeader = false)
        {
            return new XlsReader(path, useFirstRowAsHeader);
        }

        public IFormatReader<XlsReaderOptions> CreateXlsReader(XlsReaderOptions options)
        {
            return new XlsReader(options);
        }

        public IFormatReader<DbfReaderOptions> CreateDbfReader(string path)
        {
            return new DbfReader(path);
        }

        public IFormatReader<DbfReaderOptions> CreateDbfReader(DbfReaderOptions options)
        {
            return new DbfReader(options);
        }

        public IFormatReader<AsciiReaderOptions> CreateAsciiReader(string path)
        {
            return new AsciiReader(path);
        }

        public IFormatReader<AsciiReaderOptions> CreateAsciiReader(AsciiReaderOptions options)
        {
            return new AsciiReader(options);
        }

        public IFormatReader<IFileOptionsSet> CreateReader(ReaderType type, string path, IOptionsSet baseOptions = null)
        {
            IFormatReader<IFileOptionsSet> result = null;
            FileOptionsSet options = new FileOptionsSet(baseOptions ?? OptionsSet.Empty) { FilePath = path };
            switch (type) {
            case ReaderType.Ascii:
                result = new AsciiReader(options);
                break;
            case ReaderType.Excel:
            case ReaderType.Excel2007:
                result = new XlsReader(options);
                break;
            case ReaderType.Dbf:
                result = new DbfReader(options);
                break;
            case ReaderType.Db:
                throw new InvalidOperationException("use CreateDbReader()");
            default:
                throw new UnimplementedReaderTypeException(type);
            }
            return result;
        }

        public IFormatReader<IFileOptionsSet> CreateReaderByExtension(string path, IOptionsSet baseOptions = null)
        {
            var extToTypeMap = new Dictionary<string, ReaderType>() {
                { ".xls", ReaderType.Excel },
                { ".xlm", ReaderType.Excel },
                { ".xlsx", ReaderType.Excel2007 },
                { ".xlsm", ReaderType.Excel2007 },
                { ".dbf", ReaderType.Dbf },
                { ".csv", ReaderType.Ascii }
            };
            string ext = Path.GetExtension(path).ToLower();
            if (extToTypeMap.ContainsKey(ext)) {
                return CreateReader(extToTypeMap[ext], path, baseOptions);
            }
            return null;
        }

        public IEnumerable<IFormatReader<IFileOptionsSet>> CreateDirectoryReader(string path, string pattern = null, IOptionsSet baseOptions = null)
        {
            if (String.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (Path.HasExtension(path)) {
                path = Path.GetDirectoryName(path);
            }
            Regex re = (String.IsNullOrEmpty(pattern) ? null : new Regex(pattern, RegexOptions.IgnoreCase));
            var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var filePath in files)
            {
                if (re == null || re.IsMatch(filePath)) {
                    var reader = CreateReaderByExtension(filePath, baseOptions);
                    if (reader != null) {
                        yield return reader;
                    }
                }
            }
        }

        public IFormatReader<DbReaderOptions> CreateDbReader(string id, string connectionString, string name, string query)
        {
            return CreateDbReader(id, connectionString, new Dictionary<string, string>() { { name, query } });
        }

        public IFormatReader<DbReaderOptions> CreateDbReader(string id, string connectionString, IDictionary<string, string> namedQueries)
        {
            return new DbReader(id, connectionString, namedQueries);
        }

        public IFormatReader<DbReaderOptions> CreateDbReaderEx(string id, string connectionString)
        {
            return new DbReaderEx(id, connectionString);
        }

        public IFormatReader<DbReaderOptions> CreateDbReaderEx(string id, string connectionString, string name, string query)
        {
            return CreateDbReaderEx(id, connectionString, new Dictionary<string, string>() { { name, query } });
        }

        public IFormatReader<DbReaderOptions> CreateDbReaderEx(string id, string connectionString, IDictionary<string, string> namedQueries)
        {
            return new DbReaderEx(id, connectionString, namedQueries);
        }
    }
}
