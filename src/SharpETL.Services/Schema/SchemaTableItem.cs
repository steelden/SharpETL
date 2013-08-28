using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Services.Schema
{
    [Serializable]
    public sealed class SchemaTableItem
    {
        public SchemaTableItem() { }
        public SchemaTableItem(int id, string name, string description, bool insertable)
        {
            Id = id;
            Name = name;
            Description = description;
            Insertable = insertable;
        }

        public readonly int Id;
        public readonly string Name;
        public readonly string Description;
        public readonly bool Insertable;
    }
}
