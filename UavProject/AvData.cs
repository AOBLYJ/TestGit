using ControlUtils.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UavProject
{
    class AvData : ViewModel
    {
        public AvData(string fieldname, string field_offset)
        {
            Field = fieldname;
            FieldOffset = field_offset;
            //Offset

            //sdfs
        }


        public string Field
        {
            get;
            private set;
        }

        public string FieldOffset
        {
            get;
            private set;
        }
        public string Value
        {
            get
            {
                if (!_props.ContainsKey("Value")) _props.Add("Value", new NotifyingProperty("Value", typeof(string), string.Empty));
                return (string)GetValue(_props["Value"]);
            }
            set
            {
                if (!_props.ContainsKey("Value")) _props.Add("Value", new NotifyingProperty("Value", typeof(string), string.Empty));
                SetValue(_props["Value"], value);
            }
        }
    }
}
