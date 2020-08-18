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
    public class TypeRepository
    {
        private int pageNumber = 1;
        private int pageSize = 1000000;
        DataOperations dataOperations = new DataOperations();
        internal List<TypeDTO> GetSQLTypeDTO(Search search, out int _count)
        {
            var result = new List<TypeDTO>();
            _count = 0;
            string queryEnd = "";
            string head = "";

            string parent = "";

            if (search.isCount == false)
            {
                head = @" ty.Name as ParentName, cte.ID, cte.ParentID, cte.Name, cte.Description,cte.Code, CONVERT(varchar(max),cte.Level,2) as Level";
            }
            else
            {
                head = @" count(cte.ID) ";
            }


            if (search.ParentId > 0)
            {
                parent = "WHERE ParentID=@ParentId";
            }
            else
            {
                parent = "WHERE ParentID=0";
            }

            StringBuilder allQuery = new StringBuilder();

            var query = @"

                   WITH cte AS
                    (
                    SELECT ID,Name,Description,ParentID,Status,Code,
                    CAST(ID AS varbinary(max)) AS Level
                    FROM tbl_Type
                    " + parent + @"
                    UNION ALL
                    SELECT 
                    m.ID,m.Name,m.Description,m.ParentId,m.Status,m.Code,
                    Level + CAST(m.ID AS varbinary(max)) AS Level
                    FROM tbl_Type m
                    INNER JOIN cte c
                    ON c.Id = m.ParentID
                    )

                    SELECT " + head + @" FROM cte

                    LEFT JOIN tbl_Type ty on ty.ID=cte.ParentID

                    where cte.Status=1 ";

            allQuery.Append(query);

            var typeCode = @" and cte.Code like N'%' + @TypeCode + '%'";

            if (!string.IsNullOrEmpty(search.Code))
            {
                allQuery.Append(typeCode);
            }

            var typeName = @" and cte.Name like N'%' + @TypeName + '%'";

            if (!string.IsNullOrEmpty(search.Name))
            {
                allQuery.Append(typeName);
            }



            if (search.isCount == false)
            {
                queryEnd = @" ORDER BY Level OFFSET ( @PageNo - 1 ) * @RecordsPerPage ROWS
                              FETCH NEXT @RecordsPerPage ROWS ONLY";
            }

            allQuery.Append(queryEnd);

            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(allQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@PageNo", search.pageNumber);
                    command.Parameters.AddWithValue("@RecordsPerPage", search.pageSize);
                    command.Parameters.AddWithValue("@ParentId", search.ParentId);
                    command.Parameters.AddWithValue("@TypeName", search.Name.GetStringOrEmptyData());
                    command.Parameters.AddWithValue("@TypeCode", search.Code.GetStringOrEmptyData());
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (search.isCount == false)
                        {
                            result.Add(new TypeDTO()
                            {
                                ParentTypeName = reader.GetStringOrEmpty(0),
                                ID = reader.GetInt32OrDefaultValue(1),
                                ParentID = reader.GetInt32OrDefaultValue(2),
                                Name = reader.GetStringOrEmpty(3),
                                Description = reader.GetStringOrEmpty(4),
                                Code = reader.GetStringOrEmpty(5),
                                Level = reader.GetStringOrEmpty(6),
                            });
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

        public IList<TypeDTO> SW_GetTypes(Search search)
        {
            int _count = 0;
            if (search.pageNumber <= 0 || search.pageSize <= 0)
            {
                search.pageNumber = pageNumber;
                search.pageSize = pageSize;
            }
            search.isCount = false;

            IList<TypeDTO> slist = GetSQLTypeDTO(search, out _count);
            return slist;
        }
        public int SW_GetTypesCount(Search search)
        {
            search.isCount = true;
            int _count = 0;
            GetSQLTypeDTO(search, out _count).FirstOrDefault();

            return _count;
        }


        public TypeDTO GetTypeDTOByID(int typeID)
        {
            TypeDTO typeDTO = new TypeDTO();


            var query = @"select t1.Name as ParentName,t.ID,t.Name ,t.ParentID,t.Description,t.Code from dbo.[tbl_Type] t 
                           left join [tbl_Type] t1 on t.ParentID =t1.ID
                           where  t.Status=1 and
                           t.id=@T_TypeId";


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@T_TypeId", typeID);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        typeDTO.ParentTypeName = reader.GetStringOrEmpty(0);
                        typeDTO.ID = reader.GetInt32OrDefaultValue(1);
                        typeDTO.Name = reader.GetStringOrEmpty(2);
                        typeDTO.ParentID = reader.GetInt32OrDefaultValue(3);
                        typeDTO.Description = reader.GetStringOrEmpty(4);
                        typeDTO.Code = reader.GetStringOrEmpty(5);
                    }
                }
                connection.Close();
            }

            return typeDTO;
        }
    }
}
