using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ITS.UI.Attributes

{
    public static class Extensions
    {
        public static string GetDisplayName(this MemberInfo target)
        {
            return target.GetCustomAttributes(typeof(DescriptionAttribute), true)
                .Cast<DescriptionAttribute>().Select(d => d.Title)
                .SingleOrDefault() ?? target.Name;
        }
    }
}