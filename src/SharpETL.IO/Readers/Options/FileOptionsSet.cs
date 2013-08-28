using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.IO.Readers.Options
{
    public class FileOptionsSet : OptionsSet, IFileOptionsSet
    {
        public const string FILE_PATH_OPTION_NAME = "FilePath";
        public string FilePath
        {
            get { return Get<string>(FILE_PATH_OPTION_NAME); }
            set { Set(FILE_PATH_OPTION_NAME, value); }
        }

        public FileOptionsSet() : base() { }
        public FileOptionsSet(IOptionsSet other) : base(other) { }

        public FileOptionsSet(string path)
            : this()
        {
            FilePath = path;
        }
    }
}
