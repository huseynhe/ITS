using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ITS.UTILITY;
using ITS.DAL.DTO;
using ITS.DAL.Objects;
using ITS.UTILITY.Custom;
using ITS.DAL.Model;

namespace ITS.DAL.Repositories
{
    public class AccessRightsRepository
    {
        private int pageNumber = 1;
        private int pageSize = 1000000;
        DataOperations dataOperations = new DataOperations();

        public bool UserAccessCheck(int UserId, string ControllerName, string ActionName)
        {
            int result = 0;
            SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand("sp_UserAccessCheck", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
            cmd.Parameters.Add(new SqlParameter("@ControllerName", ControllerName));
            cmd.Parameters.Add(new SqlParameter("@ActionName", ActionName));
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result = reader.GetInt32OrDefaultValue(0);
            }
            connection.Close();
            cmd.Dispose();
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckAccessRights(int Id, int UserId, string ControllerName, string ActionName, int HasAccess)
        {
            DataOperations DO = new DataOperations();
            List<tbl_AccessRight> ARS = DO.GetAccessRights();
            int cnt;
            if (Id > 0 && HasAccess != 0)
            {
                cnt = ARS.Where(ar => ar.ID != Id && ar.UserId == UserId && ar.Controller == ControllerName && ar.Action == ActionName && ar.HasAccess == HasAccess).Count();
            }
            else if (Id > 0)
            {
                cnt = ARS.Where(ar => ar.ID != Id && ar.UserId == UserId && ar.Controller == ControllerName && ar.Action == ActionName).Count();
            }
            else
            {
                cnt = ARS.Where(ar => ar.UserId == UserId && ar.Controller == ControllerName && ar.Action == ActionName).Count();
            }
            bool result = cnt > 0 ? true : false;
            return result;
        }

        public AccessRightDTO GetAccessRightsComponent(int Id)
        {
            DataOperations DO = new DataOperations();
            AccessRightDTO result = new AccessRightDTO
            {
                AccessRightObj = DO.GetAccessRight(Id)
            };
            result.UserObj = DO.GetUser(result.AccessRightObj.UserId);
            return result;
        }

        public List<AccessRightDTO> GetAccessRightsComponents(int UserId, int pn, int ps)
        {
            var result = new List<AccessRightDTO>();
            var query = @"SELECT Id
                        FROM [dbo].[tbl_AccessRight] ar
                        WHERE 
	                        Status=1 AND 
	                        UserId=@UserId
                         ORDER BY Controller, Action
                         OFFSET(@pn - 1) * @ps
                         ROWS FETCH NEXT @ps ROWS ONLY";
            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(query.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@ps", ps);
                    cmd.Parameters.AddWithValue("@pn", pn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //AccessRightDTO ARC = GetAccessRightsComponent(reader.GetInt32OrDefaultValue(0));
                        //result.Add(ARC);
                    }
                }
                connection.Close();
            }
            return result;
        }


        private List<AccessRightDTO> GetAccessRightsDTO(Search search, out int _count)
        {
            _count = 0;
            var result = new List<AccessRightDTO>();
            string queryEnd = "";
            string head = "";

            if (search.isCount == false)
            {
                head = @" ar.Id  ";
            }
            else
            {
                head = @" count(ar.ID) ";
            }


            StringBuilder allQuery = new StringBuilder();

            var query = @"SELECT " + head + @" from tbl_AccessRight ar
                          where  ar.Status=1 AND 
	                        ar.UserId=@P_UserId";


            allQuery.Append(query);

            var contName = @" and ar.ControllerDesc like N'%' + @P_ContName + '%'";

            if (!string.IsNullOrEmpty(search.ControllerName))
            {
                allQuery.Append(contName);
            }

            var actionName = @" and ar.ActionDesc like N'%' + @P_ActionName + '%'";
            if (!string.IsNullOrEmpty(search.ActionName))
            {
                allQuery.Append(actionName);
            }


            if (search.isCount == false)
            {
                queryEnd = @" order by ar.InsertDate desc OFFSET ( @PageNo - 1 ) * @RecordsPerPage ROWS
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
                    command.Parameters.AddWithValue("@P_UserId", search.UserId);
                    command.Parameters.AddWithValue("@P_ContName", search.ControllerName.GetStringOrEmptyData());
                    command.Parameters.AddWithValue("@P_ActionName", search.ActionName.GetStringOrEmptyData());
                    //command.Parameters.AddWithValue("@id", id);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (search.isCount == false)
                        {
                            AccessRightDTO ARC = GetAccessRightsComponent(reader.GetInt32OrDefaultValue(0));
                            result.Add(ARC);
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
        public List<AccessRightDTO> SW_GetAccessRightsDTO(Search search)
        {
            int _count = 0;
            if (search.pageNumber <= 0 || search.pageSize <= 0)
            {
                search.pageNumber = pageNumber;
                search.pageSize = pageSize;
            }
            search.isCount = false;

            List<AccessRightDTO> slist = GetAccessRightsDTO(search, out _count);
            return slist;
        }
        public int SW_GetAccessRightsDTOCount(Search search)
        {
            search.isCount = true;
            int _count = 0;
            GetAccessRightsDTO(search, out _count).FirstOrDefault();

            return _count;
        }
    }
}
