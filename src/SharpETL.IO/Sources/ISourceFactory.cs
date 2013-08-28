using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.IO.Readers.Dbf;
using SharpETL.IO.Readers.Xls;
using SharpETL.IO.Readers.Ascii;

namespace SharpETL.IO.Sources
{
    public enum SourceType
    {
        DataSet,
        Nullable
    }

    public interface ISourceFactory
    {
        ISource CreateNullSource(string id);
        ISource CreateXlsSource(string path, bool useFirstRowAsHeader = false);
        ISource CreateXlsSource(XlsReaderOptions options);
        ISource CreateDbfSource(string path);
        ISource CreateDbfSource(DbfReaderOptions options);
        ISource CreateAsciiReader(string path);
        ISource CreateAsciiReader(AsciiReaderOptions options);
        ISource CreateSourceByExtension(string path);
        ISource CreateDbSource(string id, string connectionString, string name, string query);
        ISource CreateDbSource(string id, string connectionString, IDictionary<string, string> namedQueries);
        ISource CreateDbSourceEx(string id, string connectionString);
        ISource CreateDbSourceEx(string id, string connectionString, string name, string query);
        ISource CreateDbSourceEx(string id, string connectionString, IDictionary<string, string> namedQueries);
        IEnumerable<ISource> CreateDirectorySource(string path, string pattern = null);
    }
}
