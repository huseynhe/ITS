using ITS.DAL.Model;
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
    public class BusinessCenterRepository
    {
        private int pageNumber = 1;
        private int pageSize = 1000000;


        private List<tbl_BusinessCenter> GetBusinessCenters(Search search, out int _count)
        {
            _count = 0;
            var result = new List<tbl_BusinessCenter>();
            string queryEnd = "";
            string head = "";

            if (search.isCount == false)
            {
                head = @" bc.ID,bc.Name,bc.Description,bc.Address ";
            }
            else
            {
                head = @"  count(*) as totalcount ";
            }


            StringBuilder allQuery = new StringBuilder();

            var query = @"SELECT " + head + @"  from dbo.tbl_BusinessCenter bc where bc.Status=1 ";
            allQuery.Append(query);

            string queryName = @" and  bc.Name like N'%'+@P_Name+'%'";


            if (!string.IsNullOrEmpty(search.Name))
            {
                allQuery.Append(queryName);
            }




            if (search.isCount == false)
            {
                queryEnd = @" order by   bc.ID desc OFFSET ( @PageNo - 1 ) * @RecordsPerPage ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";
            }


            allQuery.Append(queryEnd);


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(allQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@PageNo", search.pageNumber);
                    command.Parameters.AddWithValue("@RecordsPerPage", search.pageSize);
                    command.Parameters.AddWithValue("@P_Name", search.Name.GetStringOrEmptyData());

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (search.isCount == false)
                        {
                            result.Add(new tbl_BusinessCenter()
                            {
                                //[ID],[Code],[Name],[N_Name],[Sort]
                                ID = reader.GetInt32OrDefaultValue(0),
                                Name = reader.GetStringOrEmpty(1),
                                Description = reader.GetStringOrEmpty(2),
                                Address=reader.GetStringOrEmpty(3)
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

        public IList<tbl_BusinessCenter> SW_GetBusinessCenters(Search search)
        {
            int _count = 0;
            if (search.pageNumber <= 0 || search.pageSize <= 0)
            {
                search.pageNumber = pageNumber;
                search.pageSize = pageSize;
            }
            search.isCount = false;

            IList<tbl_BusinessCenter> slist = GetBusinessCenters(search, out _count);
            return slist;
        }
        public int SW_GetBusinessCentersCount(Search search)
        {
            search.isCount = true;
            int _count = 0;
            GetBusinessCenters(search, out _count);
            return _count;
        }
    }
}
