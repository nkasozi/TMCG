using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TcmpTestCore
{
    public class Chart
    {
        public List<string> XAxisValues = new List<string>();
        public List<string> YAxisValues = new List<string>();
        public string lblXAxis { get; set; }
        public string lblYAxis { get; set; }

        public string GetXAxisValuesAsCommaSeparatedString()
        {
            string result = "";
            foreach(var item in XAxisValues)
            {
                result += $"\"{item}\",";
            }
            result = result.Trim(',');
            return result;
        }

        public string GetYAxisValuesAsCommaSeparatedString()
        {
            string result = "";
            foreach (var item in YAxisValues)
            {
                result += $"{item},";
            }
            result = result.Trim(',');
            return result;
        }
    }
}