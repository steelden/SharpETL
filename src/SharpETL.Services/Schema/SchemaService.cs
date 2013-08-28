using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharpETL.Services.Schema
{
    public sealed class SchemaService : ISchemaService
    {
        private IDictionary<string, WeakReference> _schemaCache;

        public SchemaService()
        {
            _schemaCache = new Dictionary<string, WeakReference>();
        }

        public ISimpleDbSchema GetSimpleSchemaXml(string xml)
        {
            return SimpleDbSchema.FromXml(xml);
        }

        public ISimpleDbSchema LoadSimpleSchemaXml(string path)
        {
            string fullPath = Path.GetFullPath(path);
            WeakReference result;
            if (_schemaCache.TryGetValue(fullPath, out result) && result.IsAlive) {
                return (ISimpleDbSchema)result.Target;
            }
            using (var stream = File.OpenRead(fullPath)) {
                var schema = SimpleDbSchema.FromXml(stream);
                _schemaCache[fullPath] = new WeakReference(schema);
                return schema;
            }
        }
    }
}
