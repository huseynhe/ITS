using ITS.DAL.DTO;
using ITS.DAL.Model;
using ITS.UTILITY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL.Repositories
{
   public class LoginRepository
    {
        public string DoLogin(string UserName, string Password, string IPAddress)
        {
            string result = "";
            SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand("sp_DoLogin", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new SqlParameter("@UserName", UserName));
            cmd.Parameters.Add(new SqlParameter("@Password", Password));
            cmd.Parameters.Add(new SqlParameter("@IPaddress", IPAddress));
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = reader.GetString(0);
            }
            connection.Close();
            cmd.Dispose();
            return result;
        }


    }
}
