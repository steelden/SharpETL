using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Components
{
    public interface IElement
    {
        string Name { get; }
        string Id { get; }
        object Data { get; }
    }

    public interface IElement<out T> : IElement
    {
        new T Data { get; }
    }
}
