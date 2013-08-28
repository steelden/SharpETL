using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Configuration;

namespace SharpETL.Planning
{
    public interface IPlanner
    {
        IPlan GetPlan(IEngineConfiguration config);
    }
}
