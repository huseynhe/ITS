using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.UTILITY
{
    public class MyTypes
    {
        public struct ControllersList
        {
            public string ControllerName { get; set; }
            public string Description { get; set; }
        }

        public struct ActionsList
        {
            public string ActionName { get; set; }
            public string Description { get; set; }
        }

        public struct DataChangeLog
        {
            public string ColumnName { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
        }

    }
}
