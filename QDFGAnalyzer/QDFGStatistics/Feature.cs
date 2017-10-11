using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace QDFGGraphManager.QDFGStatistics
{
    internal sealed class Feature
    {
        private String name;
        private Double valueDBL;

        public Feature(String name, Double value)
        {
            this.name = name;
            this.valueDBL = value;
        }

        public String Name
        {
            get { return this.name; }
        }

        public Double Value
        {
            get { return this.valueDBL; }
        }

        public String ValueAsString
        {
            get { return this.valueDBL.ToString(Settings.NUMBER_FORMAT, CultureInfo.InvariantCulture); }
        }
    }
}
