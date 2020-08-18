using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.UTILITY
{
    public class ConnectionStrings
    {
        public static String ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ITSConnectionString"].ConnectionString;
            }
        }
       
        public static String ConnectionString_Log
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ITS_LOGConnectionString"].ConnectionString;
            }
        }
    }
}
