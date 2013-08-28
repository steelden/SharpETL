using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharpETL.Services.Schema;
using SharpETL.Components;

namespace SharpETL.Services
{
    public interface ISchemaService : IService
    {
        ISimpleDbSchema GetSimpleSchemaXml(string xml);
        ISimpleDbSchema LoadSimpleSchemaXml(string path);
    }
}
