using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Components
{
    [Serializable]
    public class Element : Element<object>
    {
        public static readonly IElement Empty = new Element();

        protected Element() { }
        public Element(string name, string id, object data)
            : base(name, id, data)
        {
        }
    }

    [Serializable]
    public class Element<T> : IElement, IElement<T>
    {
        private string _name;
        private string _id;
        private T _data;

        //public static readonly IElement<T> Empty = new Element<T>();

        protected Element() { }
        public Element(string name, string id, T data)
        {
            _name = name;
            _id = id;
            _data = data;
        }

        public string Name { get { return _name; } }
        public string Id { get { return _id; } }
        public T Data { get { return _data; } }
        object IElement.Data { get { return _data; } }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            sb.AppendFormat("\"{0}\", \"{1}\", [", Name, Id);
            sb.AppendFormat("{0}", Data);
            sb.Append("] }");
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IElement<T>);
        }

        public bool Equals(IElement<T> obj)
        {
            if (Object.ReferenceEquals(obj, null)) return false;
            if (Object.ReferenceEquals(obj, this)) return true;
            return obj.Name == this.Name && obj.Id == this.Id;
        }

        public override int GetHashCode()
        {
            unchecked {
                int code = 17;
                code = code * 31 + (_name != null ? _name.GetHashCode() : 0);
                code = code * 31 + (_id != null ? _id.GetHashCode() : 0);
                return code;
            }
        }
    }
}
