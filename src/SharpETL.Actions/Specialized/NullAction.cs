using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;
using SharpETL.Actions.Binding;
using SharpETL.Configuration;

namespace SharpETL.Actions.Specialized
{
    public sealed class NullAction : ActionBase, IProvideConfigurationData<NullAction>
    {
        public NullAction(string id)
            : base(id)
        {
        }

        protected override IConfigurableBindingStrategy OnCreateStrategy()
        {
            return new NullStrategy();
        }

        protected override void OnConfigureStrategy(IConfigurableBindingStrategy strategy)
        {
        }

        public IConfigurationData<NullAction> GetConfiguration()
        {
            return new NullActionConfigurationData() { Id = this.Id };
        }
    }

    [Serializable]
    public sealed class NullActionConfigurationData : ConfigurationDataBase<NullAction>
    {
        public override NullAction CreateObject()
        {
            ValidateThrow();
            return new NullAction(Id);
        }

        public override string GetName()
        {
            return "NullAction";
        }

        public override IDictionary<string, object> GetData()
        {
            return new Dictionary<string, object>() { { "Id", Id } };
        }
    }
}
