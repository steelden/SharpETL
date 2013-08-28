using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers.Db
{
    public class DbReaderOptions : OptionsSet
    {
        public const string CONNECTION_STRING_NAME = "ConnectionString";
        public const string NAMED_QUERIES_NAME = "NamedQueries";

        public DbReaderOptions(IOptionsSet other) : base(other) { }
        public DbReaderOptions(string connectionString, IDictionary<string, string> namedQueries)
        {
            ConnectionString = connectionString;
            NamedQueries = namedQueries;
        }

        public string ConnectionString
        {
            get { return Get<string>(CONNECTION_STRING_NAME) ?? String.Empty; }
            set { Set(CONNECTION_STRING_NAME, value); }
        }

        public IDictionary<string, string> NamedQueries
        {
            get { return Get<IDictionary<string, string>>(NAMED_QUERIES_NAME); }
            set { Set(NAMED_QUERIES_NAME, value); }
        }
    }
}
