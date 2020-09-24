using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.UTILITY.Custom
{
    public static class DataReaderExtensions
    {
        public static string GetStringOrEmpty(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? String.Empty : reader.GetString(ordinal);
        }

        public static string GetStringOrEmpty(this IDataReader reader, string columnName)
        {
            return reader.GetStringOrEmpty(reader.GetOrdinal(columnName));
        }


        public static DateTime? GetDateTimeOrEmpty(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? (DateTime?)null : reader.GetDateTime(ordinal);
        }

        public static DateTime? GetDateTimeOrEmpty(this IDataReader reader, string columnName)
        {
            return reader.GetDateTimeOrEmpty(reader.GetOrdinal(columnName));
        }

        public static DateTime GetDateTimeOrNow(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? DateTime.Now : reader.GetDateTime(ordinal);
        }

      

        public static Byte? GetByteOrEmpty(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? (Byte?)null : reader.GetByte(ordinal);
        }

        public static Byte? GetByteOrEmpty(this IDataReader reader, string columnName)
        {
            return reader.GetByteOrEmpty(reader.GetOrdinal(columnName));
        }

        public static Boolean GetBoolOrFalse(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ?false : reader.GetBoolean(ordinal);
        }
        public static Boolean? GetBoolOrEmpty(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? (Boolean?)null : reader.GetBoolean(ordinal);
        }

        public static Boolean? GetBoolOrEmpty(this IDataReader reader, string columnName)
        {
            return reader.GetBoolOrEmpty(reader.GetOrdinal(columnName));
        }
        public static Int64 GetInt64OrDefaultValue(this IDataReader reader, int ordinal = 0)
        {
            try
            {
                return reader.IsDBNull(ordinal) ? 0 : reader.GetInt64(ordinal);
            }
            catch
            {
                return 0;
            }

        }
        public static Int32 GetInt32OrDefaultValue(this IDataReader reader, int ordinal = 0)
        {
            try
            {
                return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
            }
            catch
            {
                return 0;
            }

        }

        public static Int32 GetInt32OrDefaultValue(this IDataReader reader, string columnName)
        {
            return reader.GetInt32OrDefaultValue(reader.GetOrdinal(columnName));
        }


        public static decimal GetDecimalOrDefaultValue(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? 0 : reader.GetDecimal(ordinal);
        }

        public static decimal? GetDecimalOrDefaultValue2(this IDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return reader.GetDecimal(ordinal);
            }

        }
        public static decimal GetDecimalOrDefaultValue(this IDataReader reader, string columnName)
        {
            return reader.GetDecimalOrDefaultValue(reader.GetOrdinal(columnName));
        }

        public static float GetFloatOrDefaultValue(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? 0 : reader.GetFloat(ordinal);
        }

        public static float GetFloatOrDefaultValue(this IDataReader reader, string columnName)
        {
            return reader.GetFloatOrDefaultValue(reader.GetOrdinal(columnName));
        }

        public static double GetDoubleOrDefaultValue(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? 0 : reader.GetDouble(ordinal);
        }

        public static double GetDoubleOrDefaultValue(this IDataReader reader, string columnName)
        {
            return reader.GetDoubleOrDefaultValue(reader.GetOrdinal(columnName));
        }
    }

}
