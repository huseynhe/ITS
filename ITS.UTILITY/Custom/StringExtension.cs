using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.UTILITY.Custom
{
    public static class StringExtension
    {
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
        public static string GetStringOrEmptyData(this String data)
        {
            return String.IsNullOrEmpty(data) ? String.Empty : data;
        }

        public static Byte[] StringToByteArray(String str)
        {
            string[] bp = str.Split(',').ToArray();
            byte[] bt = new byte[bp.Length];

            for (int i = 0; i < bp.Length; i++)
            {
                bt[i] = Convert.ToByte(bp[i]);
            }

            return bt;
        }


    }
}
