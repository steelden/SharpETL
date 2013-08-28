using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.IO.Readers;
using SharpETL.IO.Readers.Db;
using SharpETL.IO.Readers.Xls;
using SharpETL.IO.Readers.Dbf;
using SharpETL.IO.Readers.Ascii;

namespace SharpETL.IO.Sources
{
    public class SourceFactory : ISourceFactory
    {
        private IReaderFactory _readerFactory;

        public SourceFactory(IReaderFactory readerFactory)
        {
            _readerFactory = readerFactory;
        }

        public ISource CreateXlsSource(string path, bool useFirstRowAsHeader = false)
        {             
            var reader = _readerFactory.CreateXlsReader(path, useFirstRowAsHeader);
            return CreateSourceFromReader(reader);
        }

        public ISource CreateXlsSource(XlsReaderOptions options)
        {
            var reader = _readerFactory.CreateXlsReader(options);
            return CreateSourceFromReader(reader);
        }

        public ISource CreateDbfSource(string path)
        {
            var reader = _readerFactory.CreateDbfReader(path);
            return CreateSourceFromReader(reader);
        }

        public ISource CreateDbfSource(DbfReaderOptions options)
        {
            var reader = _readerFactory.CreateDbfReader(options);
            return CreateSourceFromReader(reader);
        }

        public ISource CreateNullSource(string id)
        {
            return new NullSource(id);
        }

        public ISource CreateAsciiReader(string path)
        {
            var reader = _readerFactory.CreateAsciiReader(path);
            return CreateSourceFromReader(reader);
        }

        public ISource CreateAsciiReader(AsciiReaderOptions options)
        {
            var reader = _readerFactory.CreateAsciiReader(options);
            return CreateSourceFromReader(reader);
        }

        public ISource CreateSourceByExtension(string path)
        {
            var reader = _readerFactory.CreateReaderByExtension(path);
            return CreateSourceFromReader(reader);
        }

        public IEnumerable<ISource> CreateDirectorySource(string path, string pattern = null)
        {
            var readers = _readerFactory.CreateDirectoryReader(path, pattern);
            return readers.Select(x => CreateSourceFromReader(x));
        }

        public ISource CreateDbSource(string id, string connectionString, string name, string query)
        {
            return CreateSourceFromReader(_readerFactory.CreateDbReader(id, connectionString, name, query));
        }

        public ISource CreateDbSource(string id, string connectionString, IDictionary<string, string> namedQueries)
        {
            return CreateSourceFromReader(_readerFactory.CreateDbReader(id, connectionString, namedQueries));
        }

        public ISource CreateDbSourceEx(string id, string connectionString)
        {
            DbReaderEx reader = (DbReaderEx)_readerFactory.CreateDbReaderEx(id, connectionString);
            return new DeferredSource(() => new DbReaderExSource(reader));
        }

        public ISource CreateDbSourceEx(string id, string connectionString, string name, string query)
        {
            return CreateDbSourceEx(id, connectionString, new Dictionary<string, string>() { { name, query } });
        }

        public ISource CreateDbSourceEx(string id, string connectionString, IDictionary<string, string> namedQueries)
        {
            DbReaderEx reader = (DbReaderEx)_readerFactory.CreateDbReaderEx(id, connectionString, namedQueries);
            return new DeferredSource(() => new DbReaderExSource(reader));
        }

        private ISource CreateSourceFromReader(IFormatReader reader)
        {
            return new DeferredSource(() => new DataSetSource(reader.ToDataSet()));
        }
    }
}
