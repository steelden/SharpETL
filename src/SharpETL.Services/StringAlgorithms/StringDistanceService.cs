using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SharpETL.Services.StringAlgorithms
{
    public sealed class StringDistanceService : IStringDistanceService
    {
        private int _addCost;
        private int _removeCost;
        private int _changeCost;

        public StringDistanceService()
        {
            SetCosts(1, 1, 1);
        }

        public void SetCosts(int addCost, int removeCost, int changeCost)
        {
            _addCost = addCost;
            _removeCost = removeCost;
            _changeCost = changeCost;
        }

        public int CalcDistance(string s1, string s2)
        {
            if (String.IsNullOrEmpty(s1)) {
                if (String.IsNullOrEmpty(s2)) return 0;
                return s2.Length;
            } else if (String.IsNullOrEmpty(s2)) {
                return s1.Length;
            }
            return s1.Length < s2.Length ? CalcDamerauLevenshteinDistance(s1, s2) : CalcDamerauLevenshteinDistance(s2, s1);
        }

        public Tuple<string, int> FindSmallestDistance(string origin, IEnumerable<string> matches)
        {
            int minDistance = Int32.MaxValue;
            string minString = String.Empty;
            foreach (string s in matches) {
                int distance = CalcDistance(origin, s);
                if (distance < minDistance) {
                    minString = s;
                    minDistance = distance;
                }
            }
            return new Tuple<string,int>(minString, minDistance);
        }

        private int CalcDamerauLevenshteinDistance(string s1, string s2)
        {
            throw new NotImplementedException();
        }
    }
}
