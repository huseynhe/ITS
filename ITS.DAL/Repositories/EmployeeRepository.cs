using ITS.DAL.DTO;
using ITS.DAL.Objects;
using ITS.UTILITY;
using ITS.UTILITY.Custom;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL.Repositories
{
    public class EmployeeRepository
    {


        public List<PersonDTO> SW_GetEmployees(Search search)
        {
            var result = new List<PersonDTO>();
            string queryEnd = "";
            string head = "";


            head = @" emp.[ID]
                       ,emp.[FirstName]
                       ,emp.[LastName]
                       ,emp.[FatherName]
                       ,emp.[GenderType]
	                   ,(select t.Name from dbo.tbl_Type t where t.ID=emp.GenderType) as GenderTypeDesc";


            StringBuilder allQuery = new StringBuilder();

            var query = @"SELECT " + head + @" from [dbo].[tbl_Employee] emp   where  emp.Status=1   ";
            allQuery.Append(query);


            var nameQuery = @" and emp.FirstName like N'%' + @P_FirstName + '%'";
            if (!string.IsNullOrEmpty(search.Name))
            {
                allQuery.Append(nameQuery);
            }

            var surnameQuery = @" and emp.LastName like N'%' + @P_LastName + '%'";
            if (!string.IsNullOrEmpty(search.UserName))
            {
                allQuery.Append(surnameQuery);
            }

            allQuery.Append(queryEnd);


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(allQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@P_FirstName", search.Name.GetStringOrEmptyData());
                    command.Parameters.AddWithValue("@P_LastName", search.SurName.GetStringOrEmptyData());

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        result.Add(new PersonDTO()
                        {
                            ID = reader.GetInt64OrDefaultValue(0),
                            FirstName = reader.GetStringOrEmpty(1),
                            LastName = reader.GetStringOrEmpty(2),
                            FatherName = reader.GetStringOrEmpty(3),
                            GenderType = reader.GetInt32OrDefaultValue(4),
                            GenderTypeDesc = reader.GetStringOrEmpty(5)
                        });


                    }
                }
                connection.Close();
            }

            return result;
        }


    }
}