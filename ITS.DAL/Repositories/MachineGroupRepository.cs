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
    public class MachineGroupRepository
    {
        private int pageNumber = 1;
        private int pageSize = 1000000;


        private List<tbl_MachineGroup> GetMachineGroups(Search search, out int _count)
        {
            _count = 0;
            var result = new List<tbl_MachineGroup>();
            string queryEnd = "";
            string head = "";

            if (search.isCount == false)
            {
                head = @" mg.ID,mg.Name,mg.Description ";
            }
            else
            {
                head = @"  count(*) as totalcount ";
            }


            StringBuilder allQuery = new StringBuilder();

            var query = @"SELECT " + head + @"  from dbo.tbl_MachineGroup mg where mg.Status=1   ";
            allQuery.Append(query);

            string queryName = @" and  mg.Name like N'%'+@P_Name+'%'";


            if (!string.IsNullOrEmpty(search.Name))
            {
                allQuery.Append(queryName);
            }

       


            if (search.isCount == false)
            {
                queryEnd = @" order by   mg.ID desc OFFSET ( @PageNo - 1 ) * @RecordsPerPage ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";
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
                            result.Add(new tbl_MachineGroup()
                            {
                                //[ID],[Code],[Name],[N_Name],[Sort]
                                ID = reader.GetInt32OrDefaultValue(0),
                                Name = reader.GetStringOrEmpty(1),
                                Description = reader.GetStringOrEmpty(2)
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

        public IList<tbl_MachineGroup> SW_GetMachineGroups(Search search)
        {
            int _count = 0;
            if (search.pageNumber <= 0 || search.pageSize <= 0)
            {
                search.pageNumber = pageNumber;
                search.pageSize = pageSize;
            }
            search.isCount = false;

            IList<tbl_MachineGroup> slist = GetMachineGroups(search, out _count);
            return slist;
        }
        public int SW_GetMachineGroupsCount(Search search)
        {
            search.isCount = true;
            int _count = 0;
            GetMachineGroups(search, out _count);
            return _count;
        }
    }
}
