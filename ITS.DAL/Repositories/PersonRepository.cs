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
    public class PersonRepository
    {
        private int pageNumber = 1;
        private int pageSize = 1000000;
        RegionRepository regionRepository = new RegionRepository();

        private List<PersonDTO> GetPersons(Search search, out int _count)
        {
            _count = 0;
            var result = new List<PersonDTO>();
            string queryEnd = "";
            string head = "";

            if (search.isCount == false)
            {
                head = @"  person.[ID] as PersonID
                          ,person.[PIN]
                          ,person.[Name]
                          ,person.[Surname]
                          ,person.[Fathername]
                          ,person.[Gender]
	                      ,(select t.Name from dbo.tbl_Type t where t.ID=person.Gender) as GenderDesc
                          ,[Address]
                          ,[Description]
                          ,[PersonType]
                          ,(select t.Name from dbo.tbl_Type t where t.ID=person.PersonType) as PersonTypeDesc";
            }
            else
            {
                head = @"  count(*) as totalcount ";
            }


            StringBuilder allQuery = new StringBuilder();

            var query = @"SELECT " + head + @" FROM [dbo].[tbl_Person] person  where person.Status=1   ";
            allQuery.Append(query);

            string queryPIN = @" and  person.PIN like N'%'+@P_PIN+'%'";
            if (!string.IsNullOrEmpty(search.Code))
            {
                allQuery.Append(queryPIN);
            }

            string queryName = @" and  person.Name like N'%'+@P_Name+'%'";
            if (!string.IsNullOrEmpty(search.Name))
            {
                allQuery.Append(queryName);
            }

            string querySurname = @" and  person.Surname like N'%'+@P_Surname+'%'";
            if (!string.IsNullOrEmpty(search.SurName))
            {
                allQuery.Append(querySurname);
            }



            if (search.isCount == false)
            {
                queryEnd = @" order by   person.UpdateDate desc OFFSET ( @PageNo - 1 ) * @RecordsPerPage ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";
            }


            allQuery.Append(queryEnd);


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(allQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@PageNo", search.pageNumber);
                    command.Parameters.AddWithValue("@RecordsPerPage", search.pageSize);
                    command.Parameters.AddWithValue("@P_PIN", search.Code.GetStringOrEmptyData());
                    command.Parameters.AddWithValue("@P_Name", search.Name.GetStringOrEmptyData());
                    command.Parameters.AddWithValue("@P_Surname", search.SurName.GetStringOrEmptyData());

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (search.isCount == false)
                        {
                            PersonDTO personDTO = new PersonDTO()
                            {
                                //[ID],[Code],[Name],[N_Name],[Sort]
                                ID = reader.GetInt32OrDefaultValue(0),
                                PIN = reader.GetStringOrEmpty(1),
                                FirstName = reader.GetStringOrEmpty(2),
                                LastName = reader.GetStringOrEmpty(3),
                                FatherName = reader.GetStringOrEmpty(4),
                                GenderType = reader.GetInt32OrDefaultValue(5),
                                GenderTypeDesc = reader.GetStringOrEmpty(6),
                                Address = reader.GetStringOrEmpty(7),
                                Description = reader.GetStringOrEmpty(8),
                                PersonType = reader.GetInt32OrDefaultValue(9),
                                PersonTypeDesc = reader.GetStringOrEmpty(10),


                            };
                            result.Add(personDTO);



                        }
                        else
                        {

                            _count = reader.GetInt32OrDefaultValue(0);

                        }
                    }
                }
                connection.Close();
            }

            return result;
        }

        public IList<PersonDTO> SW_GetPersons(Search search)
        {
            int _count = 0;
            if (search.pageNumber <= 0 || search.pageSize <= 0)
            {
                search.pageNumber = pageNumber;
                search.pageSize = pageSize;
            }
            search.isCount = false;

            IList<PersonDTO> slist = GetPersons(search, out _count);
            return slist;
        }
        public int SW_GetPersonsCount(Search search)
        {
            search.isCount = true;
            int _count = 0;
            GetPersons(search, out _count);
            return _count;
        }

        public PersonDTO GetPersonById(Int64 personID)
        {
            PersonDTO personDTO = null;
            var query = @" select person.[ID] as PersonID
                          ,person.[PIN]
                          ,person.[Name]
                          ,person.[Surname]
                          ,person.[Fathername]
                          ,person.[Gender]
	                      ,(select t.Name from dbo.tbl_Type t where t.ID=person.Gender) as GenderDesc
                          ,person.[Address]	
                          ,person.[Description]
                          ,[PersonType]
                          ,(select t.Name from dbo.tbl_Type t where t.ID=person.PersonType) as PersonTypeDesc
                          from dbo.tbl_Person person
						  where person.Status=1 and person.ID=@P_ID; ";


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@P_ID", personID);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        personDTO = new PersonDTO()
                        {
                            //[ID],[Code],[Name],[N_Name],[Sort]
                            ID = reader.GetInt32OrDefaultValue(0),
                            PIN = reader.GetStringOrEmpty(1),
                            FirstName = reader.GetStringOrEmpty(2),
                            LastName = reader.GetStringOrEmpty(3),
                            FatherName = reader.GetStringOrEmpty(4),
                            GenderType = reader.GetInt32OrDefaultValue(5),
                            GenderTypeDesc = reader.GetStringOrEmpty(6),                
                            Address = reader.GetStringOrEmpty(7),
                            Description = reader.GetStringOrEmpty(8),
                            PersonType = reader.GetInt32OrDefaultValue(9),
                            PersonTypeDesc = reader.GetStringOrEmpty(10),
                        };


                    }
                }
                connection.Close();
            }

            return personDTO;
        }
    }
}
