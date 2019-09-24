using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOEDriverTool.Attribute
{
    public class StringValue: System.Attribute
    {
        private readonly string _value;
        private readonly string _description;

        public StringValue(string value, string description = "")
        {
            _value = value;
            _description = description;
        }

        public string Value
        {
            get { return _value; }
        }

        public string Description
        {
            get { return _description; }
        }
    }
}
