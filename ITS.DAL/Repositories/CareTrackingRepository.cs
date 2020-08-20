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
    public class CareTrackingRepository
    {
        private int pageNumber = 1;
        private int pageSize = 1000000;


        private List<CareTrackingDTO> GetCareTrackings(Search search, out int _count)
        {
            _count = 0;
            var result = new List<CareTrackingDTO>();
            string queryEnd = "";
            string head = "";

            if (search.isCount == false)
            {
                head = @"  ct.ID as CareTrackingID,
                            ct.CareDate,
                            ct.BusinessCenterID,
                            bc.Name as BusinessCenterName,
                            ct.MachineGroupID,
                            mg.Name as MachineGroupName,
                            ct.MachineID,
                            m.Name as MachineName,
                            ct.CareDescription,
                            ct.CareType,
                            (select t.Name from dbo.tbl_Type t where t.ID=ct.CareType and t.Status=1) as CareTypeDesc,
                            ct.PlanedCareType,
                            (select t.Name from dbo.tbl_Type t where t.ID=ct.PlanedCareType and t.Status=1) as PlanedCareTypeDesc,
                            ct.CareTeamType,
                            (select t.Name from dbo.tbl_Type t where t.ID=ct.CareTeamType and t.Status=1) as CareTeamTypeDesc,
                            ct.ResultType,
                            (select t.Name from dbo.tbl_Type t where t.ID=ct.ResultType and t.Status=1) as ResultTypeDesc,
                            ct.ResultDescription ";
            }
            else
            {
                head = @"  count(*) as totalcount ";
            }


            StringBuilder allQuery = new StringBuilder();

            var query = @"SELECT " + head + @"   from dbo.tbl_CareTracking ct
                                                    left join dbo.tbl_BusinessCenter bc on ct.BusinessCenterID=bc.ID and bc.Status=1
                                                    left join dbo.tbl_MachineGroup mg on ct.MachineGroupID=mg.ID and mg.Status=1
                                                    left join dbo.tbl_Machine  m on ct.MachineID=m.ID and m.Status=1
                                                  where ct.Status=1";
            allQuery.Append(query);

            string queryNameBC = @" and  bc.Name like N'%'+@P_BusinessCenterName+'%'";
            if (!string.IsNullOrEmpty(search.BusinessCenterName))
            {
                allQuery.Append(queryNameBC);
            }

            string queryNameMG = @" and  mg.Name like N'%'+@P_MachineGroupName+'%'";
            if (!string.IsNullOrEmpty(search.MachineGroupName))
            {
                allQuery.Append(queryNameMG);
            }

            string queryNameM = @" and  m.Name like N'%'+@P_MachineName+'%'";
            if (!string.IsNullOrEmpty(search.MachineName))
            {
                allQuery.Append(queryNameM);
            }

            if (search.isCount == false)
            {
                queryEnd = @" order by   ct.ID desc OFFSET ( @PageNo - 1 ) * @RecordsPerPage ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";
            }


            allQuery.Append(queryEnd);


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(allQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@PageNo", search.pageNumber);
                    command.Parameters.AddWithValue("@RecordsPerPage", search.pageSize);
                    command.Parameters.AddWithValue("@P_BusinessCenterName", search.BusinessCenterName.GetStringOrEmptyData());
                    command.Parameters.AddWithValue("@P_MachineGroupName", search.MachineGroupName.GetStringOrEmptyData());
                    command.Parameters.AddWithValue("@P_MachineName", search.MachineName.GetStringOrEmptyData());
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (search.isCount == false)
                        {
                            result.Add(new CareTrackingDTO()
                            {
                                CareTrackingID = reader.GetInt32OrDefaultValue(0),
                                CareDate = reader.GetDateTimeOrNow(1),
                                BusinessCenterID = reader.GetInt32OrDefaultValue(2),
                                BusinessCenterName = reader.GetStringOrEmpty(3),
                                MachineGroupID = reader.GetInt32OrDefaultValue(4),
                                MachineGroupName = reader.GetStringOrEmpty(5),
                                MachineID = reader.GetInt32OrDefaultValue(6),
                                MachineName = reader.GetStringOrEmpty(7),
                                CareDescription = reader.GetStringOrEmpty(8),
                                CareType = reader.GetInt32OrDefaultValue(9),
                                CareTypeDesc = reader.GetStringOrEmpty(10),
                                PlanedCareType = reader.GetInt32OrDefaultValue(11),
                                PlanedCareTypeDesc = reader.GetStringOrEmpty(12),
                                CareTeamType = reader.GetInt32OrDefaultValue(13),
                                CareTeamTypeDesc = reader.GetStringOrEmpty(14),
                                ResultType = reader.GetInt32OrDefaultValue(15),
                                ResultTypeDesc = reader.GetStringOrEmpty(16),
                                ResultDescription = reader.GetStringOrEmpty(17),
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

        public IList<CareTrackingDTO> SW_GetCareTrackings(Search search)
        {
            int _count = 0;
            if (search.pageNumber <= 0 || search.pageSize <= 0)
            {
                search.pageNumber = pageNumber;
                search.pageSize = pageSize;
            }
            search.isCount = false;

            IList<CareTrackingDTO> slist = GetCareTrackings(search, out _count);
            return slist;
        }
        public int SW_GetCareTrackingsCount(Search search)
        {
            search.isCount = true;
            int _count = 0;
            GetCareTrackings(search, out _count);
            return _count;
        }
    }
}
