using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Components
{
    public interface ILink
    {
        IAction From { get; }
        IAction To { get; }
    }
}
