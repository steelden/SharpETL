using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers.Xls
{
    public class XlsReaderOptions : FileOptionsSet
    {        
        public const string SHEET_NAME_AS_TABLE_NAME = "SetTableNameAsSheetName";
        public const string USED_SYMBOL_TO_COMMENT = "CommentSymbol";
        public const string SHEET_NAMES = "SheetNames";
        public const string FIRST_ROW_AS_HEADER = "IsFirstRowAsColumnNames";

        public XlsReaderOptions(IOptionsSet other) : base(other) { }
        public XlsReaderOptions() : this(String.Empty) { }
        public XlsReaderOptions(string path) : this(path, false) { }

        public XlsReaderOptions(string path, bool useFirstRowAsHeader)
            : base(path)
        {
            Set(FIRST_ROW_AS_HEADER, useFirstRowAsHeader);
        }

        public string CommentSymbol
        {
            get { return Get<string>(USED_SYMBOL_TO_COMMENT) ?? String.Empty; }
            set { Set(USED_SYMBOL_TO_COMMENT, value); }
        }

        public string SheetNames
        {
            get { return Get<string>(SHEET_NAMES) ?? String.Empty; }
            set { Set(SHEET_NAMES, value); }
        }

        public bool UseSheetNameAsTableName
        {
            get { return Get<bool>(SHEET_NAME_AS_TABLE_NAME); }
            set { Set(SHEET_NAME_AS_TABLE_NAME, value); }
        }

        public bool UseFirstRowAsHeader
        {
            get { return Get<bool>(FIRST_ROW_AS_HEADER); }
            set { Set(FIRST_ROW_AS_HEADER, value); }
        }
    }
}

