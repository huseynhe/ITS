using ITS.DAL.Model;
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
    public class RegionRepository
    {

     
        public tbl_Region SV_GetSQLRegionsById(int id)
        {
            DataOperations sqlDataOperation = new DataOperations();
            tbl_Region region = sqlDataOperation.GetRegionById(id);
            return region;
        }
        public List<tbl_Region> SV_GetSQLRegions(int Parent)
        {

            var result = new List<tbl_Region>();
            StringBuilder allQuery = new StringBuilder();
            var query = @"select r.ID,r.Name,r.Parent from [dbo].[tbl_Region] r where r.Parent=@P_Parent and r.Status=1";

            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@P_Parent", Parent);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        result.Add(new tbl_Region()
                        {
                            ID = reader.GetInt32OrDefaultValue(0),
                            Name = reader.GetStringOrEmpty(1),
                            Parent = reader.GetInt32OrDefaultValue(2),

                        });
                    }
                }
                connection.Close();
            }

            return result;
        }

        public List<tbl_Region> SV_GetSQLRegionsByChild(int childId)
        {

            var result = new List<tbl_Region>();
            StringBuilder allQuery = new StringBuilder();
            var query = @"WITH UserCTE AS (
                          SELECT  Id, name, parent,0 AS steps
                          FROM [dbo].[tbl_Region]
                          WHERE Id =@P_ChildID and Status=1  
                          UNION ALL  
                          SELECT mgr.Id, mgr.name, mgr.parent, usr.steps +1 AS steps
                          FROM UserCTE AS usr
                            INNER JOIN  [dbo].[tbl_Region] AS mgr
                              ON usr.Parent = mgr.Id
                        )
                        SELECT u.Id,u.Name,u.Parent,u.steps FROM UserCTE AS u order by u.steps desc";

            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@P_ChildID", childId);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        result.Add(new tbl_Region()
                        {
                            ID = reader.GetInt32OrDefaultValue(0),
                            Name = reader.GetStringOrEmpty(1),
                            Parent = reader.GetInt32OrDefaultValue(2),

                        });
                    }
                }
                connection.Close();
            }

            return result;
        }
    }
}
