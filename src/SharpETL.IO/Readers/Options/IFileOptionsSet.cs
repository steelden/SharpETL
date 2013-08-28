using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.IO.Readers.Options
{
    public interface IFileOptionsSet : IOptionsSet
    {
        string FilePath { get; }
    }
}
