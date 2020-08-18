using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.UTILITY.Custom
{
    public static class StripTag
    {
        public static string strSqlBlocker(string StrValue)
        {
            string[] BadCharacters = { "*", "#", ">", "<", "=", "&", "$", "%", "(", ")", "@", "!", /*",",*/ "'", "^", "||", "&", ":", @"\", "from", "select", "drop", "update", "all", "print", "md5", ".ini", ".exe", ".bat", "system32" };
           // string[] BadCharacters = { "*", "#", ">", "<", "=", "&", "$", "%", "(", ")", "@", "!", /*",",*/ "'", "^", "||", "&", ":", "/", @"\", "from", "select", "drop", "update", "all", "print", "md5", ".ini", ".exe", ".bat", "system32" };
            int i;
            for (i = 0; i < BadCharacters.Length; i++)
            {
                StrValue = StrValue.Replace(BadCharacters[i], "");
            }

            StrValue = StrValue.Replace("İ", "@");
            StrValue = StrValue.ToLower();
            StrValue = StrValue.Replace("@", "İ");

            return StrValue;
        }
    }
}
