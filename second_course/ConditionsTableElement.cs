using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace second_course
{
    class ConditionsTableElement
    {
        public string ColumnName { get; set; }
        public string Operator { get; set; }
        public List<string> OperatorsList { get; set; }
        public string ComparisonValue { get; set; }

        public ConditionsTableElement() { }
    }
}
