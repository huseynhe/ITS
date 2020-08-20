using ITS.DAL.DTO;
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
    public class ReportRepository
    {
        private int pageNumber = 1;
        private int pageSize = 1000000;
        public List<ReportDTO> GetMachineGroupReports()
        {
            var result = new List<ReportDTO>();
            ReportDTO reportDTO = null;
            var query = @"select 
                            CASE
                                WHEN ISNULL(mg.Name,'')='' THEN N'Digər'
                                ELSE mg.Name
                            END as Name
                            ,COUNT(*) as say from dbo.tbl_Machine m 
                              left join dbo.tbl_MachineGroup mg on mg.ID=m.MachineGroupID and mg.Status=1
                               where m.Status=1
                               group by mg.Name ";


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query.ToString(), connection))
                {
                    //command.Parameters.AddWithValue("@P_PersonID", personID);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        reportDTO = new ReportDTO()
                        {
                            name = reader.GetStringOrEmpty(0),
                            count = reader.GetInt32OrDefaultValue(1)

                        };

                        result.Add(reportDTO);
                    }
                }
                connection.Close();
            }

            return result;

        }

        public List<ReportDTO> GetMachineReports()
        {
            var result = new List<ReportDTO>();
            ReportDTO reportDTO = null;
            var query = @" select Name,Quantity from tbl_Machine m where m.Status=1";


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query.ToString(), connection))
                {
                    //command.Parameters.AddWithValue("@P_PersonID", personID);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        reportDTO = new ReportDTO()
                        {
                            name = reader.GetStringOrEmpty(0),
                            count = reader.GetInt32OrDefaultValue(1)

                        };

                        result.Add(reportDTO);
                    }
                }
                connection.Close();
            }

            return result;

        }
    }
}
