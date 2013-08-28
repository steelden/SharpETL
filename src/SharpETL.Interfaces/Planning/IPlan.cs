using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpETL.Planning
{
    public interface IPlan
    {
        void Execute();
        void BeginExecute();
        void EndExecute();
    }
}
