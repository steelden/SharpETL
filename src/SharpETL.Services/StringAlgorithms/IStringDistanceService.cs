using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpETL.Components;

namespace SharpETL.Services
{
    public interface IStringDistanceService : IService
    {
        void SetCosts(int addCost, int removeCost, int changeCost);
        int CalcDistance(string origin, string match);
        Tuple<string, int> FindSmallestDistance(string origin, IEnumerable<string> matches);
    }
}
