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
                            (select t.Name from dbo.tbl_Type t where t.ID=ct.CareTeamType and t.Status=1) as CareTeamTypeDesc
                           ";
            }
            else
            {
                head = @"  count(*) as totalcount ";
            }


            StringBuilder allQuery = new StringBuilder();

            var query = @"SELECT " + head + @"   from dbo.tbl_CareTracking ct
	                                               left join dbo.tbl_CareTrackingDetail ctd on ct.ID =ctd.CareTrackingID  and ctd.Status=1
                                                    left join dbo.tbl_BusinessCenter bc on ct.BusinessCenterID=bc.ID and bc.Status=1
                                                    left join dbo.tbl_MachineGroup mg on ct.MachineGroupID=mg.ID and mg.Status=1
                                                    left join dbo.tbl_Machine  m on ct.MachineID=m.ID and m.Status=1
                                                  where ct.Status=1 and  (ctd.ResultType is null or  ctd.ResultType!=5)";
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
                queryEnd = @" order by ct.CareDate asc OFFSET ( @PageNo - 1 ) * @RecordsPerPage ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";
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
                                CareTeamTypeDesc = reader.GetStringOrEmpty(14)
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

        public CareTrackingDTO GetCareTrackingByID(int id)
        {

            CareTrackingDTO careTrackingDTO = null;
            var query = @"SELECT ct.ID as CareTrackingID,
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
                            (select t.Name from dbo.tbl_Type t where t.ID=ct.CareTeamType and t.Status=1) as CareTeamTypeDesc
                            from dbo.tbl_CareTracking ct
                                                    left join dbo.tbl_BusinessCenter bc on ct.BusinessCenterID=bc.ID and bc.Status=1
                                                    left join dbo.tbl_MachineGroup mg on ct.MachineGroupID=mg.ID and mg.Status=1
                                                    left join dbo.tbl_Machine  m on ct.MachineID=m.ID and m.Status=1
                                                  where ct.Status=1 and ct.ID=@P_ID";


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@P_ID", id);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        careTrackingDTO = new CareTrackingDTO()
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
                        };
                    }
                }
                connection.Close();
            }

            return careTrackingDTO;
        }
        #region CareTrackingDetailDTO
        public List<CareTrackingDetailDTO> GetCareTrackingDetailsByCTID(Int64 ctID)
        {
            var result = new List<CareTrackingDetailDTO>();
            CareTrackingDetailDTO careTrackingDetailDTO = null;
            var query = @"select ctd.ID,ctd.CareTrackingID,ctd.StartDate,ctd.StartTime, ctd.EndDate,ctd.EndTime,
                              ctd.Description
							  ,ctd.MechanicID
	                         ,(select p.Surname +' '+p.Name+' '+p.Fathername from dbo.tbl_Person p where p.ID=ctd.MechanicID and p.Status=1) as MechanicSAA 
                             ,ctd.ReceivingPersonID
	                        ,(select p.Surname +' '+p.Name+' '+p.Fathername from dbo.tbl_Person p where p.ID=ctd.ReceivingPersonID and p.Status=1) as ReceivingPersonSAA,
                            ctd.ResultType,
                            (select t.Name from dbo.tbl_Type t where t.ID=ctd.ResultType and t.Status=1) as ResultTypeDesc,
                            ctd.ResultDescription 
							 from dbo.tbl_CareTrackingDetail ctd
	                           where ctd.Status=1 and ctd.CareTrackingID=@P_CareTrackingID";


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@P_CareTrackingID", ctID);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        careTrackingDetailDTO = new CareTrackingDetailDTO()
                        {
                            CareTrackingDetailID = reader.GetInt32OrDefaultValue(0),
                            CareTrackingID = reader.GetInt32OrDefaultValue(1),
                            StartDate = reader.GetDateTimeOrEmpty(2),

                            HasStartTime = reader.IsDBNull(3),
                            EndDate = reader.GetDateTimeOrEmpty(4),

                            HasEndTime = reader.IsDBNull(5),
                            Description = reader.GetStringOrEmpty(6),
                            MechanicID = reader.GetInt32OrDefaultValue(7),
                            MechanicSAA = reader.GetStringOrEmpty(8),
                            ReceivingPersonID = reader.GetInt32OrDefaultValue(9),
                            ReceivingPersonSAA = reader.GetStringOrEmpty(10),

                            ResultType = reader.GetInt32OrDefaultValue(11),
                            ResultTypeDesc = reader.GetStringOrEmpty(12),
                            ResultDescription = reader.GetStringOrEmpty(13),
                        };
                        if (!careTrackingDetailDTO.HasStartTime)
                        {

                            careTrackingDetailDTO.StartTime = reader.GetTimeSpan(3);
                            if (careTrackingDetailDTO.StartDate != null)
                            {
                                careTrackingDetailDTO.FullStartDate = ((DateTime)careTrackingDetailDTO.StartDate) + careTrackingDetailDTO.StartTime;
                            }
                        }
                        if (!careTrackingDetailDTO.HasEndTime)
                        {

                            careTrackingDetailDTO.EndTime = reader.GetTimeSpan(5);
                            if (careTrackingDetailDTO.EndDate != null)
                            {
                                careTrackingDetailDTO.FullEndDate = ((DateTime)careTrackingDetailDTO.EndDate) + careTrackingDetailDTO.EndTime;
                            }
                        }


                        TimeSpan span = careTrackingDetailDTO.FullEndDate.Subtract(careTrackingDetailDTO.FullStartDate);
                        StringBuilder durationTime = new StringBuilder();
                        careTrackingDetailDTO.DurationTime = String.Format("{0} saat", span.Hours);
                        if (span.Days > 0)
                        {
                            durationTime.Append(String.Format("{0} Gün ", span.Days));

                        }
                        if (span.Hours > 0)
                        {
                            durationTime.Append(String.Format("{0} Saat ", span.Hours));

                        }
                        if (span.Minutes > 0)
                        {
                            durationTime.Append(String.Format("{0} Dəqiqə ", span.Minutes));

                        }
                        if (durationTime.Length != 0)
                        {
                            careTrackingDetailDTO.DurationTime = durationTime.ToString();
                        }

                        result.Add(careTrackingDetailDTO);
                    }
                }
                connection.Close();
            }

            return result;

        }
        #endregion

        #region CareTrackingDTO
        private List<CareTrackingDTO> GetCareTrackingsForExcel(Search search, out int _count)
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
							 ctd.ID,ctd.CareTrackingID,ctd.StartDate,ctd.StartTime, ctd.EndDate,ctd.EndTime,
                             ctd.Description
							 ,ctd.MechanicID
	                         ,(select p.Surname +' '+p.Name+' '+p.Fathername from dbo.tbl_Person p where p.ID=ctd.MechanicID and p.Status=1) as MechanicSAA 
                             ,ctd.ReceivingPersonID
	                        ,(select p.Surname +' '+p.Name+' '+p.Fathername from dbo.tbl_Person p where p.ID=ctd.ReceivingPersonID and p.Status=1) as ReceivingPersonSAA,
                            ctd.ResultType,
                            (select t.Name from dbo.tbl_Type t where t.ID=ctd.ResultType and t.Status=1) as ResultTypeDesc,
                            ctd.ResultDescription
                           ";
            }
            else
            {
                head = @"  count(*) as totalcount ";
            }


            StringBuilder allQuery = new StringBuilder();

            var query = @"SELECT " + head + @"  from [dbo].[tbl_CareTracking] ct
					 left join dbo.tbl_CareTrackingDetail ctd on ct.ID =ctd.CareTrackingID  and ctd.Status=1
					 left join dbo.tbl_BusinessCenter bc on ct.BusinessCenterID=bc.ID and bc.Status=1
					 left join dbo.tbl_MachineGroup mg on ct.MachineGroupID=mg.ID and mg.Status=1
					 left join dbo.tbl_Machine  m on ct.MachineID=m.ID and m.Status=1
					where ct.Status=1 ";
            allQuery.Append(query);

            string queryID = @" and  ct.ID =@P_ID";
            if (search.Id>0)
            {
                allQuery.Append(queryID);
            }


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
                queryEnd = @" order by   ct.CareDate asc";
            }


            allQuery.Append(queryEnd);


            using (var connection = new SqlConnection(ConnectionStrings.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(allQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@P_ID", search.Id);
                    command.Parameters.AddWithValue("@P_BusinessCenterName", search.BusinessCenterName.GetStringOrEmptyData());
                    command.Parameters.AddWithValue("@P_MachineGroupName", search.MachineGroupName.GetStringOrEmptyData());
                    command.Parameters.AddWithValue("@P_MachineName", search.MachineName.GetStringOrEmptyData());
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (search.isCount == false)
                        {
                            CareTrackingDTO careTrackingDTO = new CareTrackingDTO()
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
                                careTrackingDetailDTO = new CareTrackingDetailDTO()
                                {
                                    CareTrackingDetailID = reader.GetInt32OrDefaultValue(15),
                                    CareTrackingID = reader.GetInt32OrDefaultValue(16),
                                    StartDate = reader.GetDateTimeOrEmpty(17),
                                    HasStartTime = reader.IsDBNull(18),
                                    EndDate = reader.GetDateTimeOrEmpty(19),
                                    HasEndTime = reader.IsDBNull(20),
                                    Description = reader.GetStringOrEmpty(21),
                                    MechanicID = reader.GetInt32OrDefaultValue(22),
                                    MechanicSAA = reader.GetStringOrEmpty(23),
                                    ReceivingPersonID = reader.GetInt32OrDefaultValue(24),
                                    ReceivingPersonSAA = reader.GetStringOrEmpty(25),

                                    ResultType = reader.GetInt32OrDefaultValue(26),
                                    ResultTypeDesc = reader.GetStringOrEmpty(27),
                                    ResultDescription = reader.GetStringOrEmpty(28),
                                }
                            };
                            if (careTrackingDTO.careTrackingDetailDTO.CareTrackingDetailID>0)
                            {
                                if (!careTrackingDTO.careTrackingDetailDTO.HasStartTime)
                                {
                                    careTrackingDTO.careTrackingDetailDTO.StartTime = reader.GetTimeSpan(18);
                                    careTrackingDTO.careTrackingDetailDTO.StartTimeDesc = reader.GetTimeSpan(18).ToString(@"hh\:mm"); ;
                                    if (careTrackingDTO.careTrackingDetailDTO.StartDate != null)
                                    {
                                        careTrackingDTO.careTrackingDetailDTO.FullStartDate = ((DateTime)careTrackingDTO.careTrackingDetailDTO.StartDate) + careTrackingDTO.careTrackingDetailDTO.StartTime;
                                    }
                                }
                                if (!careTrackingDTO.careTrackingDetailDTO.HasEndTime)
                                {
                                    careTrackingDTO.careTrackingDetailDTO.EndTime = reader.GetTimeSpan(20);
                                    careTrackingDTO.careTrackingDetailDTO.EndTimeDesc = reader.GetTimeSpan(20).ToString(@"hh\:mm"); ;
                                    if (careTrackingDTO.careTrackingDetailDTO.EndDate != null)
                                    {
                                        careTrackingDTO.careTrackingDetailDTO.FullEndDate = ((DateTime)careTrackingDTO.careTrackingDetailDTO.EndDate) + careTrackingDTO.careTrackingDetailDTO.EndTime;
                                    }
                                }
                                TimeSpan span = careTrackingDTO.careTrackingDetailDTO.FullEndDate.Subtract(careTrackingDTO.careTrackingDetailDTO.FullStartDate);
                                StringBuilder durationTime = new StringBuilder();
                                careTrackingDTO.careTrackingDetailDTO.DurationTime = String.Format("{0} saat", span.Hours);
                                if (span.Days > 0)
                                {
                                    durationTime.Append(String.Format("{0} Gün ", span.Days));

                                }
                                if (span.Hours > 0)
                                {
                                    durationTime.Append(String.Format("{0} Saat ", span.Hours));

                                }
                                if (span.Minutes > 0)
                                {
                                    durationTime.Append(String.Format("{0} Dəqiqə ", span.Minutes));

                                }
                                if (durationTime.Length != 0)
                                {
                                    careTrackingDTO.careTrackingDetailDTO.DurationTime = durationTime.ToString();
                                }
                            }
                            result.Add(careTrackingDTO);
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

        public IList<CareTrackingDTO> SW_GetCareTrackingsForExcel(Search search)
        {
            int _count = 0;
            if (search.pageNumber <= 0 || search.pageSize <= 0)
            {
                search.pageNumber = pageNumber;
                search.pageSize = pageSize;
            }
            search.isCount = false;

            IList<CareTrackingDTO> slist = GetCareTrackingsForExcel(search, out _count);
            return slist;
        }
        public int SW_GetCareTrackingsCountForExcel(Search search)
        {
            search.isCount = true;
            int _count = 0;
            GetCareTrackingsForExcel(search, out _count);
            return _count;
        }
        #endregion
    }
}
