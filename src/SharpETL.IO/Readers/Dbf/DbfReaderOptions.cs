using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers.Dbf
{
    public class DbfReaderOptions : FileOptionsSet
    {
         /// <summary>
        /// Default set FIRST_ROW_AS_HEADER false
        /// </summary>
        /// <param name="path"></param>
        public DbfReaderOptions(string path) : base(path) { }
        public DbfReaderOptions(IOptionsSet other) : base(other) { }
    }
}
