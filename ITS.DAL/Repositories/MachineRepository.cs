using ITS.DAL.DTO;
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
    public class MachineRepository
    {
        private int pageNumber = 1;
        private int pageSize = 1000000;


        private List<MachineDTO> GetMachines(Search search, out int _count)
        {
            _count = 0;
            var result = new List<MachineDTO>();
            string queryEnd = "";
            string head = "";

            if (search.isCount == false)
            {
                head = @" m.ID,m.Code,m.Name,m.Description,m.Quantity,m.MachineGroupID,mg.Name as MachineGroupName ";
            }
            else
            {
                head = @"  count(*) as totalcount ";
            }


            StringBuilder allQuery = new StringBuilder();

            var query = @"SELECT " + head + @"  from dbo.tbl_Machine m 
                                                left join [dbo].[tbl_MachineGroup] mg on m.MachineGroupID=mg.ID and mg.Status=1
                                                where m.Status=1  ";
            allQuery.Append(query);

            string queryName = @" and  m.Name like N'%'+@P_Name+'%'";


            if (!string.IsNullOrEmpty(search.Name))
            {
                allQuery.Append(queryName);
            }

            string queryCode = @" and  m.Code like N'%'+@P_Code+'%'";

            if (!string.IsNullOrEmpty(search.Code))
            {
                allQuery.Append(queryCode);
            }



            if (search.isCount == false)
            {
                queryEnd = @" order by   m.ID desc OFFSET ( @PageNo - 1 ) * @RecordsPerPage ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";
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
                    command.Parameters.AddWithValue("@P_Code", search.Code.GetStringOrEmptyData());

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (search.isCount == false)
                        {
                            result.Add(new MachineDTO()
                            {
                                //[ID],[Code],[Name],[N_Name],[Sort]
                                ID = reader.GetInt32OrDefaultValue(0),
                                Code = reader.GetStringOrEmpty(1),
                                Name = reader.GetStringOrEmpty(2),
                                Description = reader.GetStringOrEmpty(3),
                                Quantity = reader.GetInt32OrDefaultValue(4),
                                MachineGroupID = reader.GetInt32OrDefaultValue(5),
                                MachineGroupDesc = reader.GetStringOrEmpty(6)

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

        public IList<MachineDTO> SW_GetMachines(Search search)
        {
            int _count = 0;
            if (search.pageNumber <= 0 || search.pageSize <= 0)
            {
                search.pageNumber = pageNumber;
                search.pageSize = pageSize;
            }
            search.isCount = false;

            IList<MachineDTO> slist = GetMachines(search, out _count);
            return slist;
        }
        public int SW_GetMachinesCount(Search search)
        {
            search.isCount = true;
            int _count = 0;
            GetMachines(search, out _count);
            return _count;
        }
    }
}
