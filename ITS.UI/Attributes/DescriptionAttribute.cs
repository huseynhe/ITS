using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITS.UI.Attributes

{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class DescriptionAttribute : Attribute
    {
        private readonly string _title;
        public string Title
        {
            get { return _title; }
        }

        public DescriptionAttribute(string title)
        {
            _title = title;
        }
    }
}