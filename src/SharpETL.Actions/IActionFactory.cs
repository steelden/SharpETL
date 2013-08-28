using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Actions;
using SharpETL.Actions.Specialized;
using SharpETL.Actions.Db;
using SharpETL.Actions.Script;

namespace SharpETL.Configuration
{
    public interface IActionFactory
    {
        IScriptAction CreateScriptAction(string id, ISource source, IScript script);
        IAction CreateFilterAction(string id, Func<IElement, bool> predicate);
        IAction CreateNullAction(string id);
        IAction CreateDistinctAction(string id);
        IAction CreateDebugAction(string id);
        IEnumerable<IAction> CreateMultipleSourcesScriptActions(string id, IEnumerable<ISource> sourcesList, IScript script);
        IAction CreateSchemaValidationAction(string id, string schemaPath, Func<SchemaValidationError, SchemaValidationDecision> errorHandler);
        IAction CreateMapNameToIdAction(string id, string schemaPath, string connectionString, Action<ResolveReferencesError> errorHandler = null);
        IAction CreateMapIdToNameAction(string id, string schemaPath, string connectionString, Action<ResolveReferencesError> errorHandler = null);
        IAction CreateSqlInsertTextAction(string id);
        IAction CreateSqlPreparationAction(string id, string schemaPath, string connectionString, bool updateOnlyIfNewValueIsNotNull = false);
        IAction CreateSqlExecutionAction(string id, string connectionString, SqlExecuteTarget target);
        IAction CreateSourceAction(string id, ISource source);
        IAction CreateBinarySerializeAction(string id, string path, bool overwrite = false);
        IAction CreateBinaryDeserializeAction(string id, string path);
        IAction CreateReorderByReferenceAction(string id, string schemaPath);
        IAction CreateXlsxTarget(string id, string path, bool overwrite = false);
        IAction CreateAsciiTarget(string id,
            string path, string extension, bool addHeader,
            string columnSeparator, string recordSeparator,
            string quoteChar, string quoteCharReplacement);
        IAction CreateCsvTarget(string id, string path);
        IAction CreatePlainTextTarget(string id, string path, string separator = ";");
    }
}
