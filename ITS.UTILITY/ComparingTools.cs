using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ITS.UTILITY
{
    public class ComparingTools
    {
        public static List<MyTypes.DataChangeLog> ColumnChanges(object source, object destination)
        {
            List<MyTypes.DataChangeLog> result = new List<MyTypes.DataChangeLog>();
            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            if (sourceType == destinationType)
            {
                PropertyInfo[] sourceProperties = sourceType.GetProperties();
                foreach (PropertyInfo pi in sourceProperties)
                {
                    try
                    {
                        object svalue = pi.Name.IndexOf("tbl_") == -1 ? sourceType.GetProperty(pi.Name).GetValue(source, null) : null;
                        object dvalue = pi.Name.IndexOf("tbl_") == -1 ? destinationType.GetProperty(pi.Name).GetValue(destination, null) : null;
                        if (svalue != null && dvalue != null && svalue.ToString() != dvalue.ToString())
                        {
                            result.Add(new MyTypes.DataChangeLog()
                            {
                                ColumnName = pi.Name,
                                OldValue = sourceType.GetProperty(pi.Name).GetValue(source, null).ToString(),
                                NewValue = destinationType.GetProperty(pi.Name).GetValue(destination, null).ToString()
                            });
                        }
                    }
                    catch
                    {
                        throw new Exception("Müqayisə olunan sütunlar mövcud olmayan obyektlərdir!");
                    }
                }
            }
            else
            {
                throw new ArgumentException("Sütunları müqayisə olunan obyektlərin tipləri eyni deyil!");
            }

            return result;
        }
    }
}
