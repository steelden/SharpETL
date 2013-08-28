using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions;
using SharpETL.Actions.Binding;
using SharpETL.Components;
using SharpETL.Extensions;
using OfficeOpenXml;
using System.IO;

namespace SharpETL.Actions.Targets
{
    public sealed class XlsxTargetAction : BindedActionBase
    {
        private ExcelPackage _pkg;
        private IDictionary<string, int> _currentRows;
        private IDictionary<string, IDictionary<string, int>> _schemas;
        private object _gate;

        public XlsxTargetAction(string id, string path, bool overwrite)
            : base(id)
        {
            FileInfo info = new FileInfo(path);
            if (info.Exists) {
                if (overwrite) {
                    info.Delete();
                } else {
                    throw new InvalidOperationException("File already exists.");
                }
            }

            _pkg = new ExcelPackage(info);
            _currentRows = new Dictionary<string, int>();
            _schemas = new Dictionary<string, IDictionary<string, int>>();
            _gate = new object();
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.Element | ActionEvents.Completed | ActionEvents.Finally;
        }

        protected override bool OnElement(IElement element, IObserver<IElement> sink)
        {
            var data = element.Data as DataElementValues;
            if (data == null) {
                return true;
            }
            lock (_gate) {
                string name = element.Name;
                ExcelWorksheet sheet = _pkg.Workbook.Worksheets[name];
                if (sheet == null) {
                    sheet = _pkg.Workbook.Worksheets.Add(name);
                }
                int currentRow;
                if (!_currentRows.TryGetValue(name, out currentRow)) {
                    currentRow = AddHeaderRow(data.Schema, sheet);
                    _currentRows.Add(name, currentRow);
                    if (data.Schema != null) {
                        _schemas.Add(name, data.Schema);
                    }
                }
                int i = 1;
                foreach (var obj in data.Values) { 
                    sheet.SetValue(currentRow, i, obj);
                    if (obj != null && typeof(DateTime).IsAssignableFrom(obj.GetType())) {
                        sheet.Cells[currentRow, i].Style.Numberformat.Format = @"dd/mm/yyy hh:mm";
                    }
                    i++;
                }
                _currentRows[name] = currentRow + 1;
            }
            return true;
        }

        protected override bool OnCompleted(IObserver<IElement> sink)
        {
            if (_pkg != null) {
                SetAutoFit(_pkg.Workbook);
                _pkg.Save();
            }
            return true;
        }

        protected override void OnFinally()
        {
            if (_pkg != null) {
                _pkg.Dispose();
                _pkg = null;
            }
        }

        private int AddHeaderRow(IDictionary<string, int> schema, ExcelWorksheet sheet)
        {
            if (schema != null) {
                foreach (var item in schema) {
                    sheet.SetValue(1, item.Value + 1, item.Key);
                }
                return 2;
            }
            return 1;
        }

        private void SetAutoFit(ExcelWorkbook book)
        {
            foreach (var sheet in book.Worksheets) {
                IDictionary<string, int> schema;
                if (_schemas.TryGetValue(sheet.Name, out schema)) {
                    foreach (var item in schema) {
                        sheet.Column(item.Value + 1).AutoFit();
                    }
                }
            }
        }
    }
}
