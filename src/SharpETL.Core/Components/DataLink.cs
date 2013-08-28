using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;
using System.Xml.Linq;

namespace SharpETL.Components
{
    public class DataLink : ILink, IProvideConfigurationData<DataLink>
    {
        private IAction _from;
        private IAction _to;

        public DataLink(ILink link)
            : this(link.From, link.To)
        {
        }

        public DataLink(IAction from, IAction to)
        {
            _from = from;
            _to = to;
        }

        public IAction From { get { return _from; } }
        public IAction To { get { return _to; } }

        public IConfigurationData<DataLink> GetConfiguration()
        {
            return new LinkConfigurationData() { From = this.From, To = this.To };
        }
    }

    [Serializable]
    public sealed class LinkConfigurationData : ConfigurationDataBase<DataLink>
    {
        public IAction From { get; set; }
        public IAction To { get; set; }

        public override IDictionary<string, object> GetData()
        {
            return new Dictionary<string, object>() { { "From", From }, { "To", To } };
        }

        public override DataLink CreateObject()
        {
            ValidateThrow();
            return new DataLink(From, To);
        }
    }
}
