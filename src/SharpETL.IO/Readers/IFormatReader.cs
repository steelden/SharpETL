using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers
{
    public interface IFormatReader
    {
        DataSet ToDataSet();
    }

    public interface IFormatReader<out T> : IFormatReader where T : IOptionsSet
    {
        T Options { get; }
    }
}
