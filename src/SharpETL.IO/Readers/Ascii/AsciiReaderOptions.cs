using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers.Ascii
{
    public class AsciiReaderOptions : FileOptionsSet
    {
        public const string FIELD_TERM_NAME = "FieldsTerminatedBy";
        public const string ENCLOSED_BY_NAME = "EnclosedBy";
        public const string ENCLOSING_OPTIONAL_NAME = "EnslosingOptional";
        public const string RECORD_DELIMITER_NAME = "RecordDelimiter";

        public const string FIELD_TERM_DEFAULT = ";";
        public const string ENCLOSED_BY_DEFAULT = "\"";
        public const string RECORD_DELIMITER_DEFAULT = "\n";

        public AsciiReaderOptions(string path) : base(path) { }
        public AsciiReaderOptions(IOptionsSet other) : base(other) { }

        public string FieldsTerminatedBy
        {
            get { return Get<string>(FIELD_TERM_NAME) ?? FIELD_TERM_DEFAULT; }
            set { Set(FIELD_TERM_NAME, value); }
        }

        public string EnclosedBy
        {
            get { return Get<string>(ENCLOSED_BY_NAME) ?? ENCLOSED_BY_DEFAULT; }
            set { Set(ENCLOSED_BY_NAME, value); }
        }

        public bool EnclosingOptional
        {
            get { return Get<bool>(ENCLOSING_OPTIONAL_NAME); }
            set { Set(ENCLOSING_OPTIONAL_NAME, value); }
        }

        public string RecordDelimiter
        {
            get { return Get<string>(RECORD_DELIMITER_NAME) ?? RECORD_DELIMITER_DEFAULT; }
            set { Set(RECORD_DELIMITER_NAME, value); }
        }
    }
}
