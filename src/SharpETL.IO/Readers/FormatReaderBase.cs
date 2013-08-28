using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using SharpETL.IO.Readers.Options;

namespace SharpETL.IO.Readers
{
    public abstract class FormatReaderBase<T> : IFormatReader<T> where T : IOptionsSet
    {
        public T Options { get; private set; }

        public FormatReaderBase(T options)
        {
            if (options == null) throw new ArgumentNullException("options");
            Options = options;
        }

        public abstract DataSet ToDataSet();

        protected bool IsFilePathSet()
        {
            IFileOptionsSet fos = Options as IFileOptionsSet;
            return fos != null && !String.IsNullOrEmpty(fos.FilePath);
        }

        protected string GetValidFilePath()
        {
            IFileOptionsSet fos = Options as IFileOptionsSet;
            if (fos == null) throw new InvalidOperationException("Options != IFileOptionsSet");
            string filePath = fos.FilePath;
            if (String.IsNullOrEmpty(filePath)) throw new InvalidOperationException("Не задано имя файла");
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
            return filePath;
        }
    }
}
