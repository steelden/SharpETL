using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers.Ascii
{
    public class AsciiReader : FormatReaderBase<AsciiReaderOptions>
    {
        public AsciiReader(string path) : this(new AsciiReaderOptions(path)) { }
        public AsciiReader(IOptionsSet options) : base(new AsciiReaderOptions(options)) { }
        public override DataSet ToDataSet()
        {
            throw new NotImplementedException();
        }
    }
}
