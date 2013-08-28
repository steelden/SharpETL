using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using SharpETL.Components;
using SharpETL.Actions.Specialized;
using SharpETL.Actions.Db;
using SharpETL.Actions.Script;
using SharpETL.Actions.Targets;

namespace SharpETL.Actions
{
    public sealed class ActionFactory : IActionFactory
    {
        public IScriptAction CreateScriptAction(string id, ISource source, IScript script)
        {
            return new ScriptAction(id, source, script);
        }

        public IAction CreateFilterAction(string id, Func<IElement, bool> predicate)
        {
            return new FilterAction(id, predicate);
        }

        public IAction CreateNullAction(string id)
        {
            return new NullAction(id);
        }

        public IAction CreateDistinctAction(string id)
        {
            return new DistinctAction(id);
        }

        public IAction CreateDebugAction(string id)
        {
            return new DebugAction(id);
        }

        public IEnumerable<IAction> CreateMultipleSourcesScriptActions(string id, IEnumerable<ISource> sourcesList, IScript script)
        {
            Func<ISource, string> getId = x => String.Format("{0}_{1}", id, x.Id);
            return sourcesList.Select(x => CreateScriptAction(getId(x), x, script)).ToArray();
        }

        public IAction CreateSchemaValidationAction(string id, string schemaPath, Func<SchemaValidationError, SchemaValidationDecision> errorHandler)
        {
            return new SchemaValidationAction(id, schemaPath, errorHandler);
        }

        public IAction CreateMapNameToIdAction(string id, string schemaPath, string connectionString, Action<ResolveReferencesError> errorHandler = null)
        {
            return new MapNameToIdAction(id, schemaPath, connectionString, errorHandler);
        }

        public IAction CreateMapIdToNameAction(string id, string schemaPath, string connectionString, Action<ResolveReferencesError> errorHandler = null)
        {
            return new MapIdToNameAction(id, schemaPath, connectionString, errorHandler);
        }

        public IAction CreateSqlInsertTextAction(string id)
        {
            return new SqlInsertTextAction(id);
        }

        public IAction CreateSqlPreparationAction(string id, string schemaPath, string connectionString, bool updateOnlyIfNewValueIsNotNull = false)
        {
            return new SqlPreparationAction(id, schemaPath, connectionString, updateOnlyIfNewValueIsNotNull);
        }

        public IAction CreateSqlExecutionAction(string id, string connectionString, SqlExecuteTarget target)
        {
            return new SqlExecutionAction(id, connectionString, target);
        }

        public IAction CreateSourceAction(string id, ISource source)
        {
            return new SourceAction(id, source);
        }

        public IAction CreateBinarySerializeAction(string id, string path, bool overwrite = false)
        {
            return new BinarySerializeAction(id, path, overwrite);
        }

        public IAction CreateBinaryDeserializeAction(string id, string path)
        {
            return new BinaryDeserializeAction(id, path);
        }

        public IAction CreateReorderByReferenceAction(string id, string schemaPath)
        {
            return new ReorderByReferenceAction(id, schemaPath);
        }

        public IAction CreateXlsxTarget(string id, string path, bool overwrite = false)
        {
            return new XlsxTargetAction(id, path, overwrite);
        }

        public IAction CreateAsciiTarget(string id,
            string path, string extension, bool addHeader,
            string columnSeparator, string recordSeparator,
            string quoteChar, string quoteCharReplacement)
        {
            return new AsciiTargetAction(id, path, extension, addHeader, columnSeparator, recordSeparator, quoteChar, quoteCharReplacement);
        }

        public IAction CreateCsvTarget(string id, string path)
        {
            return new AsciiTargetAction(id, path, "csv", true, ";", Environment.NewLine, "\"", "\"\"");
        }

        public IAction CreatePlainTextTarget(string id, string path, string separator = ";")
        {
            return new AsciiTargetAction(id, path, "txt", false, separator, Environment.NewLine, null, null);
        }
    }
}
