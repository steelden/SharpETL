using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Actions.Binding;
using SharpETL.Components;
using SharpETL.Services.Schema;
using SharpETL.Services;
using System.Globalization;

namespace SharpETL.Actions.Db
{
    public enum SchemaValidationErrorType
    {
        None = 0,
        MissingField = 1,
        UnknownField = 2,
        InvalidFieldType = 3,
        LessThenMinNumber = 4,
        GreaterThenMaxNumber = 5,
        LessThenMinDate = 6,
        GreaterThenMaxDate = 7,
        NullValueNotNullField = 8,
        Other = 100
    }

    public sealed class SchemaValidationException : Exception
    {
        public SchemaValidationException(SchemaValidationErrorType errorType) : base(GetMessage(errorType)) { }
        public SchemaValidationException(SchemaValidationErrorType errorType, Exception innerException) : base(GetMessage(errorType), innerException) { }
        private static readonly string ERROR_TEXT = "Validation Error: {0}";
        private static string GetMessage(SchemaValidationErrorType errorType) { return String.Format(ERROR_TEXT, errorType); }
    }

    public sealed class SchemaValidationError
    {
        public SchemaValidationError(IAction action, DataElement element, SchemaValidationErrorType errorType, string fieldName)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (element == null) throw new ArgumentNullException("element");
            Action = action;
            Element = element;
            ErrorType = errorType;
            FieldName = fieldName;
        }
        public readonly IAction Action;
        public readonly DataElement Element;
        public readonly SchemaValidationErrorType ErrorType;
        public readonly string FieldName;
    }

    public enum SchemaValidationDecision
    {
        Default = 0,
        IgnoreError = 0,
        PassElement = 1,
        SkipElement = 2,
        DoComplete = 3,
        DoError = 4
    }
        

    public sealed class SchemaValidationAction : BindedActionBase
    {
        private string _path;
        private ISimpleDbSchema _schema;
        private Func<SchemaValidationError, SchemaValidationDecision> _errorHandler;

        public SchemaValidationAction(string id, string schemaPath, Func<SchemaValidationError, SchemaValidationDecision> errorHandler)
            : base(id)
        {
            if (String.IsNullOrEmpty(schemaPath)) throw new ArgumentNullException("schemaPath");
            if (errorHandler == null) throw new ArgumentNullException("errorHandler");
            _path = schemaPath;
            _errorHandler = errorHandler;
        }

        protected override ActionEvents OnGetExpectedEvents()
        {
            return ActionEvents.Element;
        }

        protected override void OnSetEngine()
        {
            base.OnSetEngine();
            var sservice = Engine.GetService<ISchemaService>();
            _schema = sservice.LoadSimpleSchemaXml(_path);
        }

        protected override bool OnElement(IElement element, IObserver<IElement> sink)
        {
            var de = element as DataElement;
            if (de != null && de.Data != null && de.Data.Schema != null) {
                return ValidateValues(de, sink);
            }
            return true;
        }

        private SchemaValidationDecision OnHandleError(DataElement element,
                                                                 SchemaValidationErrorType errorType,
                                                                 string fieldName)
        {
            if (_errorHandler != null) {
                var error = new SchemaValidationError(this, element, errorType, fieldName);
                return _errorHandler(error);
            }
            return SchemaValidationDecision.Default;
        }

        private bool ValidateValues(DataElement element, IObserver<IElement> sink)
        {
            var fields = _schema.GetTableSchemaDictionary(element.Name);
            var data = element.Data.Schema.ToDictionary(x => x.Key, x => element.Data.Values[x.Value]);
            var names = fields.Keys.Concat(data.Keys).Distinct();
            foreach (string name in names) {
                SchemaValidationErrorType error = SchemaValidationErrorType.None;

                if (!fields.ContainsKey(name)) {
                    error = SchemaValidationErrorType.UnknownField;
                } else if (!data.ContainsKey(name)) {
                    error = SchemaValidationErrorType.MissingField;
                } else {
                    error = CheckValue(fields[name], data[name]);
                }
                if (error != SchemaValidationErrorType.None) {
                    SchemaValidationDecision decision = OnHandleError(element, error, name);
                    switch (decision) {
                    case SchemaValidationDecision.PassElement: return true;
                    case SchemaValidationDecision.SkipElement: return false;
                    case SchemaValidationDecision.DoComplete: sink.OnCompleted(); return false;
                    case SchemaValidationDecision.DoError: sink.OnError(new SchemaValidationException(error)); return false;
                    case SchemaValidationDecision.IgnoreError:
                    default:
                        break;
                    }
                }
            }
            return true;
        }

        private SchemaValidationErrorType CheckValue(SchemaFieldItem field, object value)
        {
            SchemaValidationErrorType result = SchemaValidationErrorType.None;
            if (value == null || value == DBNull.Value) {
                if (field.IsNotNull) {
                    result = SchemaValidationErrorType.NullValueNotNullField;
                }
            } else {
                result = SchemaValidationErrorType.InvalidFieldType;
                SFIType? fieldType = TryGetFieldType(value.GetType());
                if (fieldType.HasValue && (int)fieldType.Value == field.FieldType) {
                    result = CheckMinMax(field, (IConvertible)value);
                }
            }
            return result;
        }

        private SchemaValidationErrorType CheckMinMax(SchemaFieldItem field, IConvertible value)
        {
            SchemaValidationErrorType result = SchemaValidationErrorType.None;
            if (field.FieldType == (int)SFIType.Number) {
                decimal dvalue = value.ToDecimal(CultureInfo.InvariantCulture);
                if (field.MinValue.HasValue && dvalue < field.MinValue.Value) {
                    result = SchemaValidationErrorType.LessThenMinNumber;
                }
                if (field.MaxValue.HasValue && dvalue > field.MaxValue.Value) {
                    result = SchemaValidationErrorType.GreaterThenMaxNumber;
                }
            } else if (field.FieldType == (int)SFIType.Date) {
                DateTime dtvalue = value.ToDateTime(CultureInfo.InvariantCulture);
                if (field.MinDateValue.HasValue && dtvalue < field.MinDateValue.Value) {
                    result = SchemaValidationErrorType.LessThenMinDate;
                }
                if (field.MaxDateValue.HasValue && dtvalue > field.MaxDateValue.Value) {
                    result = SchemaValidationErrorType.GreaterThenMaxDate;
                }
            }
            return result;
        }

        private readonly IDictionary<Type, SFIType> _typeToFieldTypeMap = new Dictionary<Type, SFIType>() {
            { typeof(String), SFIType.String },
            { typeof(Decimal), SFIType.Number },
            { typeof(Int32), SFIType.Number },
            { typeof(Int64), SFIType.Number },
            { typeof(Byte), SFIType.Number },
            { typeof(Single), SFIType.Number },
            { typeof(Double), SFIType.Number },
            { typeof(DateTime), SFIType.Date },
            { typeof(DateTimeOffset), SFIType.Date },
        };

        private readonly IDictionary<Type, SFIType?> _typeToFieldTypeCache = new Dictionary<Type, SFIType?>();

        private SFIType? TryGetFieldType(Type type)
        {
            SFIType? fieldType;
            if (!_typeToFieldTypeCache.TryGetValue(type, out fieldType)) {
                foreach (var typeData in _typeToFieldTypeMap) {
                    if (typeData.Key.IsAssignableFrom(type)) {
                        _typeToFieldTypeCache[type] = typeData.Value;
                        return typeData.Value;
                    }
                }
                _typeToFieldTypeCache[type] = null;
                return null;
            }
            return fieldType;
        }
    }
}
