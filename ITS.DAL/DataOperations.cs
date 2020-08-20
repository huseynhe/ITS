using ITS.DAL.Repositories;
using ITS.DAL.Model;
using ITS.UTILITY;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL
{
   public class DataOperations
    {
        #region Users
        public tbl_User GetUser(int Id)
        {
            ITSEntities entities = new ITSEntities();

            tbl_User item = entities.tbl_User.Where(x => x.Status == 1 && x.ID == Id).FirstOrDefault();
            entities.Dispose();
            return item;
        }
        public tbl_User GetUserByUserName(string userName)
        {
            ITSEntities entities = new ITSEntities();
            tbl_User item = entities.tbl_User.Where(x => x.Status == 1 && x.UserName == userName).FirstOrDefault();
            entities.Dispose();
            return item;
        }
        public List<tbl_User> GetUsers()
        {
            ITSEntities entities = new ITSEntities();
            List<tbl_User> itemlist = entities.tbl_User.Where(x => x.Status == 1).ToList();
            entities.Dispose();
            return itemlist;
        }
        public bool AddUser(tbl_User u, out string ErrorMessage)
        {
            try
            {
                using (var context = new ITSEntities())
                {
                    UsersRepository UR = new UsersRepository();
                    bool namecheck = UR.CheckForUserName(0, u.UserName);
                    if (namecheck)
                    {
                        ErrorMessage = "Bu istifadəçi adı artıq daxil edilib!";
                        return false;
                    }
                    u.Status = 1;
                    u.InsertDate = DateTime.Now;
                    context.tbl_User.Add(u);
                    if (context.SaveChanges() == 0)
                    {
                        ErrorMessage = "İstifadəçini əlavə edərkən xəta baş verdi. Zəhmət olmasa yenidən cəhd edin.";
                        return false;
                    }
                    else
                    {
                        ErrorMessage = "Məlumatlar uğurla əlavə olundu.";
                        return true;
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateUser(tbl_User u, out string ErrorMessage)
        {
            ITSEntities entities = new ITSEntities();
            tbl_User user = entities.tbl_User.Where(x => x.Status == 1 && x.ID == u.ID).FirstOrDefault();
            try
            {
                UsersRepository UR = new UsersRepository();
                bool namecheck = UR.CheckForUserName(u.ID, u.UserName);
                if (namecheck)
                {
                    ErrorMessage = "Bu istifadəçi adı artıq daxil edilib.";
                    return false;
                }
                user.UserName = u.UserName;
                user.EmployeeID = u.EmployeeID;
                user.Password = u.Password;
                user.AccountLocked = u.AccountLocked;
                user.UpdateDate = DateTime.Now;
                user.UpdateUser = u.UpdateUser;
                entities.tbl_User.Attach(user);
                entities.Entry(user).State = System.Data.Entity.EntityState.Modified;
                if (entities.SaveChanges() == 0)
                {
                    ErrorMessage = "İstifadəçini redaktə edərkən xəta baş verdi. Zəhmət olmasa yenidən cəhd edin.";
                    return false;
                }
                ErrorMessage = "Məlumatlar uğurla dəyişdirilmiştir";
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                entities.Dispose();
            }

        }
        public bool DeleteUser(int Id, int userID, out string ErrorMessage)
        {
            try
            {
                tbl_User ou;
                using (var context = new ITSEntities())
                {
                    ou = (from u in context.tbl_User
                          where u.ID == Id
                          select u).FirstOrDefault();
                }
                if (ou != null)
                {
                    using (var context = new ITSEntities())
                    {
                        UsersRepository UR = new UsersRepository();
                        if (UR.GetRelatedUsersCounts(Id))
                        {
                            ErrorMessage = "Bu istifadəçi ilə bağlı digər cədvəllərdə məlumat olduğu üçün silinə bilməz.";
                            return false;
                        }
                        else
                        {
                            ou.Status = 0;
                            ou.UpdateUser = userID;
                            ou.UpdateDate = DateTime.Now;
                            context.tbl_User.Attach(ou);
                            context.Entry(ou).State = System.Data.Entity.EntityState.Modified;
                            if (context.SaveChanges() == 0)
                            {
                                ErrorMessage = "İstifadəçini silərkən xəta baş verdi. Zəhmət olmasa yenidən cəhd edin";
                                return false;
                            }
                            ErrorMessage = "Məlumatlar uğurla silindi";
                            return true;
                        }
                    }
                }
                else
                {
                    Exception e = new Exception(Id.ToString() + " İD-li istifadəçi yoxdur!");
                    throw e;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region AccessRights
        public bool AddAccessRights(tbl_AccessRight ar, out string ErrorMessage)
        {
            try
            {
                using (var context = new ITSEntities())
                {
                    AccessRightsRepository ARR = new AccessRightsRepository();
                    bool accessrightscheck = ARR.CheckAccessRights(0, ar.UserId, ar.Controller, ar.Action, 0);
                    if (accessrightscheck)
                    {
                        ErrorMessage = "Bu icazə artıq daxil edilib.";
                        return false;
                    }
                    ar.Status = 1;
                    context.tbl_AccessRight.Add(ar);
                    if (context.SaveChanges() == 0)
                    {
                        ErrorMessage = "İcazəni əlavə edərkən xəta baş verdi. Zəhmət olmasa yenidən cəhd edin.";
                        return false;
                    }
                    ErrorMessage = "İcazəni uğurla əlavə edilmiştir";
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateAccessRights(tbl_AccessRight ar, out string ErrorMessage)
        {
            ITSEntities entities = new ITSEntities();
            tbl_AccessRight uar = entities.tbl_AccessRight.Where(x => x.Status == 1 && x.ID == ar.ID).FirstOrDefault();
            try
            {
                AccessRightsRepository ARR = new AccessRightsRepository();
                bool accessrightscheck = ARR.CheckAccessRights(ar.ID, ar.UserId, ar.Controller, ar.Action, ar.HasAccess);
                if (accessrightscheck)
                {
                    ErrorMessage = "Bu icazə adı artıq daxil edilib.";
                    return false;
                }
                uar.UserId = ar.UserId;
                uar.Controller = ar.Controller;
                uar.ControllerDesc = ar.ControllerDesc;
                uar.ActionDesc = ar.ActionDesc;
                uar.Action = ar.Action;
                uar.HasAccess = ar.HasAccess;
                entities.tbl_AccessRight.Attach(uar);
                entities.Entry(uar).State = System.Data.Entity.EntityState.Modified;
                if (entities.SaveChanges() == 0)
                {
                    ErrorMessage = "İstifadəçi icazəsini redaktə edərkən xəta baş verdi. Zəhmət olmasa yenidən cəhd edin.";
                    return false;
                }
                ErrorMessage = "İcazə uğurla redaktə edilmişdir";
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                entities.Dispose();
            }
        }
        public bool DeleteAccessRights(int Id, int userId, out string ErrorMessage)
        {
            try
            {
                tbl_AccessRight oar;
                using (var context = new ITSEntities())
                {
                    oar = (from ar in context.tbl_AccessRight
                           where ar.ID == Id
                           select ar).FirstOrDefault();
                }
                if (oar != null)
                {
                    using (var context = new ITSEntities())
                    {
                        oar.Status = 0;
                        oar.UpdateDate = DateTime.Now;
                        oar.UpdateUser = userId;
                        context.tbl_AccessRight.Attach(oar);
                        context.Entry(oar).State = System.Data.Entity.EntityState.Modified;
                        if (context.SaveChanges() == 0)
                        {
                            ErrorMessage = "İstifadəçi icazəsini silərkən xəta baş verdi. Zəhmət olmasa yenidən cəhd edin";
                            return false;
                        }
                        ErrorMessage = "Icazə uğurla silinmişdir";
                        return true;
                    }
                }
                else
                {
                    Exception e = new Exception(Id.ToString() + " İD-li istifadəçi icazəsi yoxdur!");
                    throw e;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public tbl_AccessRight GetAccessRight(int Id)
        {
            ITSEntities entities = new ITSEntities();
            tbl_AccessRight ar = entities.tbl_AccessRight.Where(x => x.Status == 1 && x.ID == Id).FirstOrDefault();
            entities.Dispose();
            return ar;
        }
        public List<tbl_AccessRight> GetAccessRights()
        {
            ITSEntities entities = new ITSEntities();
            List<tbl_AccessRight> ARL = entities.tbl_AccessRight.Where(x => x.Status == 1).ToList();
            entities.Dispose();
            return ARL;
        }
        #endregion

        #region DataChange Log
        public bool SaveToLog_DataChange(int UserId, int OperationType, string TableName, int OriginalId, List<MyTypes.DataChangeLog> Changes)
        {
            SqlConnection connection = new SqlConnection(ConnectionStrings.ConnectionString_Log);
            connection.Open();
            string query;
            try
            {
                if (OperationType == 1 | OperationType == 3)
                {
                    query = @"INSERT INTO [dbo].[log_dataChange] (UserId, OperationType, TableName, OriginalId, OperationDateTime)
                            VALUES(@UserId, @OperationType, @TableName, @OriginalId, GETDATE())";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@OperationType", OperationType);
                    cmd.Parameters.AddWithValue("@TableName", TableName);
                    cmd.Parameters.AddWithValue("@OriginalId", OriginalId);
                    cmd.CommandType = System.Data.CommandType.Text;
                    int result = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    bool r = true;
                    foreach (MyTypes.DataChangeLog Change in Changes)
                    {
                        query = @"INSERT INTO [dbo].[log_dataChange] (UserId, OperationType, TableName, OriginalId, ColumnName, OldValue, NewValue, OperationDateTime)
                                VALUES(@UserId, @OperationType, @TableName, @OriginalId, @ColumnName, @OldValue, @NewValue, GETDATE())";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        cmd.Parameters.AddWithValue("@OperationType", OperationType);
                        cmd.Parameters.AddWithValue("@TableName", TableName);
                        cmd.Parameters.AddWithValue("@OriginalId", OriginalId);
                        cmd.Parameters.AddWithValue("@ColumnName", Change.ColumnName);
                        cmd.Parameters.AddWithValue("@OldValue", Change.OldValue);
                        cmd.Parameters.AddWithValue("@NewValue", Change.NewValue);
                        int eachresult = (int)cmd.ExecuteNonQuery();
                        if (eachresult == 0)
                        {
                            r = false;
                        }
                        cmd.Dispose();
                    }
                    return r;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        #region TblRegion
        public tbl_Region AddRegion(tbl_Region item)
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    item.Status = 1;
                    item.InsertDate = DateTime.Now;
                    context.tbl_Region.Add(item);
                    context.SaveChanges();
                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public tbl_Region DeleteRegion(int id, int userId)
        {

            try
            {
                tbl_Region oldItem;
                using (var context = new ITSEntities())
                {

                    oldItem = (from p in context.tbl_Region
                               where p.ID == id && p.Status == 1
                               select p).FirstOrDefault();

                }

                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {
                        oldItem.Status = 0;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = userId;
                        context.tbl_Region.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }

                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }
                return oldItem;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<tbl_Region> GetRegions()
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    var items = (from p in context.tbl_Region
                                 where p.Status == 1
                                 select p);

                    return items.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public tbl_Region GetRegionById(Int64 Id)
        {

            try
            {
                using (var context = new ITSEntities())
                {


                    var item = (from p in context.tbl_Region
                                where p.ID == Id && p.Status == 1
                                select p).FirstOrDefault();

                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<tbl_Region> GetRegionByParentId(Int64 parentId)
        {

            try
            {
                using (var context = new ITSEntities())
                {


                    var items = (from p in context.tbl_Region
                                 where p.ParentId == parentId && p.Status == 1
                                 select p).ToList();

                    return items;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public tbl_Region UpdateRegion(tbl_Region item)
        {
            try
            {
                tbl_Region oldItem;
                using (var context = new ITSEntities())
                {
                    oldItem = (from p in context.tbl_Region
                               where p.ID == item.ID && p.Status == 1
                               select p).FirstOrDefault();

                }
                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {


                        oldItem.Name = item.Name;
                        oldItem.ParentId = item.ParentId;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = item.UpdateUser;
                        context.tbl_Region.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return oldItem;
                    }
                }
                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }


            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region TblType
        public tbl_Type AddType(tbl_Type item)
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    item.Status = 1;
                    item.InsertDate = DateTime.Now;
                    context.tbl_Type.Add(item);
                    context.SaveChanges();
                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public tbl_Type DeleteType(int id, int userId)
        {

            try
            {
                tbl_Type oldItem;
                using (var context = new ITSEntities())
                {

                    oldItem = (from p in context.tbl_Type
                               where p.ID == id && p.Status == 1
                               select p).FirstOrDefault();

                }

                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {
                        oldItem.Status = 0;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = userId;
                        context.tbl_Type.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }

                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }
                return oldItem;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<tbl_Type> GetTypes()
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    var items = (from p in context.tbl_Type
                                 where p.Status == 1
                                 select p);

                    return items.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public tbl_Type GetTypeById(Int64 Id)
        {

            try
            {
                using (var context = new ITSEntities())
                {


                    var item = (from p in context.tbl_Type
                                where p.ID == Id && p.Status == 1
                                select p).FirstOrDefault();

                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<tbl_Type> GetTypeByParentId(Int64 parentId)
        {

            try
            {
                using (var context = new ITSEntities())
                {


                    var items = (from p in context.tbl_Type
                                 where p.ParentID == parentId && p.Status == 1
                                 select p).ToList();

                    return items;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public tbl_Type UpdateType(tbl_Type item)
        {
            try
            {
                tbl_Type oldItem;
                using (var context = new ITSEntities())
                {
                    oldItem = (from p in context.tbl_Type
                               where p.ID == item.ID && p.Status == 1
                               select p).FirstOrDefault();

                }
                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {


                        oldItem.Name = item.Name;
                        oldItem.Code = item.Code;
                        oldItem.Description = item.Description;
                        oldItem.ParentID = item.ParentID;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = item.UpdateUser;
                        context.tbl_Type.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return oldItem;
                    }
                }
                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }


            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region tbl_Employee
        public tbl_Employee AddEmployee(tbl_Employee item)
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    item.Status = 1;
                    item.InsertDate = DateTime.Now;
                    item.UpdateDate = DateTime.Now;
                    context.tbl_Employee.Add(item);
                    context.SaveChanges();
                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public tbl_Employee DeleteEmployee(int id, int userId)
        {

            try
            {
                tbl_Employee oldItem;
                using (var context = new ITSEntities())
                {

                    oldItem = (from p in context.tbl_Employee
                               where p.ID == id && p.Status == 1
                               select p).FirstOrDefault();

                }

                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {
                        oldItem.Status = 0;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = userId;
                        context.tbl_Employee.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }

                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }
                return oldItem;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<tbl_Employee> GetEmployees()
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    var items = (from p in context.tbl_Employee
                                 where p.Status == 1
                                 select p);

                    return items.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public tbl_Employee GetEmployeeById(Int64 Id)
        {

            try
            {
                using (var context = new ITSEntities())
                {


                    var item = (from p in context.tbl_Employee
                                where p.ID == Id && p.Status == 1
                                select p).FirstOrDefault();

                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public tbl_Employee UpdateEmployee(tbl_Employee item)
        {
            try
            {
                tbl_Employee oldItem;
                using (var context = new ITSEntities())
                {
                    oldItem = (from p in context.tbl_Employee
                               where p.ID == item.ID && p.Status == 1
                               select p).FirstOrDefault();

                }
                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {


                        oldItem.FirstName = item.FirstName;
                        oldItem.LastName = item.LastName;
                        oldItem.FatherName = item.FatherName;
                        oldItem.GenderType = item.GenderType;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = item.UpdateUser;
                        context.tbl_Employee.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return oldItem;
                    }
                }
                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }


            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region tbl_Country
        public tbl_Country AddCountry(tbl_Country item)
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    item.Status = 1;
                    item.InsertDate = DateTime.Now;
                    item.UpdateDate = DateTime.Now;
                    context.tbl_Country.Add(item);
                    context.SaveChanges();
                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public tbl_Country DeleteCountry(int id, int userId)
        {

            try
            {
                tbl_Country oldItem;
                using (var context = new ITSEntities())
                {

                    oldItem = (from p in context.tbl_Country
                               where p.ID == id && p.Status == 1
                               select p).FirstOrDefault();

                }

                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {
                        oldItem.Status = 0;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = userId;
                        context.tbl_Country.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }

                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }
                return oldItem;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<tbl_Country> GetCountries()
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    var items = (from p in context.tbl_Country
                                 where p.Status == 1
                                 select p);

                    return items.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public tbl_Country GetCountryById(Int64 Id)
        {

            try
            {
                using (var context = new ITSEntities())
                {


                    var item = (from p in context.tbl_Country
                                where p.ID == Id && p.Status == 1
                                select p).FirstOrDefault();

                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public tbl_Country UpdateCountry(tbl_Country item)
        {
            try
            {
                tbl_Country oldItem;
                using (var context = new ITSEntities())
                {
                    oldItem = (from p in context.tbl_Country
                               where p.ID == item.ID && p.Status == 1
                               select p).FirstOrDefault();

                }
                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {


                        oldItem.NAME = item.NAME;
                        oldItem.USER_NAME = item.USER_NAME;
                        oldItem.CODE = item.CODE;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = item.UpdateUser;
                        context.tbl_Country.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return oldItem;
                    }
                }
                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }


            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region tbl_Machine
        public tbl_Machine AddMachine(tbl_Machine item)
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    item.Status = 1;
                    item.InsertDate = DateTime.Now;
                    item.UpdateDate = DateTime.Now;
                    context.tbl_Machine.Add(item);
                    context.SaveChanges();
                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public tbl_Machine DeleteMachine(int id, int userId)
        {

            try
            {
                tbl_Machine oldItem;
                using (var context = new ITSEntities())
                {

                    oldItem = (from p in context.tbl_Machine
                               where p.ID == id && p.Status == 1
                               select p).FirstOrDefault();

                }

                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {
                        oldItem.Status = 0;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = userId;
                        context.tbl_Machine.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }

                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }
                return oldItem;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<tbl_Machine> GetMachines()
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    var items = (from p in context.tbl_Machine
                                 where p.Status == 1
                                 select p);

                    return items.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public tbl_Machine GetMachineById(Int64 Id)
        {

            try
            {
                using (var context = new ITSEntities())
                {


                    var item = (from p in context.tbl_Machine
                                where p.ID == Id && p.Status == 1
                                select p).FirstOrDefault();

                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public tbl_Machine UpdateMachine(tbl_Machine item)
        {
            try
            {
                tbl_Machine oldItem;
                using (var context = new ITSEntities())
                {
                    oldItem = (from p in context.tbl_Machine
                               where p.ID == item.ID && p.Status == 1
                               select p).FirstOrDefault();

                }
                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {

                        oldItem.Code = item.Code;
                        oldItem.Name = item.Name;
                        oldItem.Description = item.Description;
                        oldItem.Quantity = item.Quantity;
                        oldItem.MachineGroupID = item.MachineGroupID;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = item.UpdateUser;
                        context.tbl_Machine.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return oldItem;
                    }
                }
                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }


            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
        #region tbl_MachineGroup
        public tbl_MachineGroup AddMachineGroup(tbl_MachineGroup item)
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    item.Status = 1;
                    item.InsertDate = DateTime.Now;
                    item.UpdateDate = DateTime.Now;
                    context.tbl_MachineGroup.Add(item);
                    context.SaveChanges();
                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public tbl_MachineGroup DeleteMachineGroup(int id, int userId)
        {

            try
            {
                tbl_MachineGroup oldItem;
                using (var context = new ITSEntities())
                {

                    oldItem = (from p in context.tbl_MachineGroup
                               where p.ID == id && p.Status == 1
                               select p).FirstOrDefault();

                }

                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {
                        oldItem.Status = 0;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = userId;
                        context.tbl_MachineGroup.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }

                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }
                return oldItem;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<tbl_MachineGroup> GetMachineGroups()
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    var items = (from p in context.tbl_MachineGroup
                                 where p.Status == 1
                                 select p);

                    return items.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public tbl_MachineGroup GetMachineGroupById(Int64 Id)
        {

            try
            {
                using (var context = new ITSEntities())
                {


                    var item = (from p in context.tbl_MachineGroup
                                where p.ID == Id && p.Status == 1
                                select p).FirstOrDefault();

                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public tbl_MachineGroup UpdateMachineGroup(tbl_MachineGroup item)
        {
            try
            {
                tbl_MachineGroup oldItem;
                using (var context = new ITSEntities())
                {
                    oldItem = (from p in context.tbl_MachineGroup
                               where p.ID == item.ID && p.Status == 1
                               select p).FirstOrDefault();

                }
                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {

                        oldItem.Name = item.Name;
                        oldItem.Description = item.Description;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = item.UpdateUser;
                        context.tbl_MachineGroup.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return oldItem;
                    }
                }
                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }


            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region tbl_BusinessCenter
        public tbl_BusinessCenter AddBusinessCenter(tbl_BusinessCenter item)
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    item.Status = 1;
                    item.InsertDate = DateTime.Now;
                    item.UpdateDate = DateTime.Now;
                    context.tbl_BusinessCenter.Add(item);
                    context.SaveChanges();
                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public tbl_BusinessCenter DeleteBusinessCenter(int id, int userId)
        {

            try
            {
                tbl_BusinessCenter oldItem;
                using (var context = new ITSEntities())
                {

                    oldItem = (from p in context.tbl_BusinessCenter
                               where p.ID == id && p.Status == 1
                               select p).FirstOrDefault();

                }

                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {
                        oldItem.Status = 0;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = userId;
                        context.tbl_BusinessCenter.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }

                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }
                return oldItem;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<tbl_BusinessCenter> GetBusinessCenters()
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    var items = (from p in context.tbl_BusinessCenter
                                 where p.Status == 1
                                 select p);

                    return items.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public tbl_BusinessCenter GetBusinessCenterById(Int64 Id)
        {

            try
            {
                using (var context = new ITSEntities())
                {


                    var item = (from p in context.tbl_BusinessCenter
                                where p.ID == Id && p.Status == 1
                                select p).FirstOrDefault();

                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public tbl_BusinessCenter UpdateBusinessCenter(tbl_BusinessCenter item)
        {
            try
            {
                tbl_BusinessCenter oldItem;
                using (var context = new ITSEntities())
                {
                    oldItem = (from p in context.tbl_BusinessCenter
                               where p.ID == item.ID && p.Status == 1
                               select p).FirstOrDefault();

                }
                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {

                        oldItem.Name = item.Name;
                        oldItem.Description = item.Description;
                        oldItem.Address = item.Address;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = item.UpdateUser;
                        context.tbl_BusinessCenter.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return oldItem;
                    }
                }
                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }


            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region tbl_CareTracking
        public tbl_CareTracking AddCareTracking(tbl_CareTracking item)
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    item.Status = 1;
                    item.InsertDate = DateTime.Now;
                    item.UpdateDate = DateTime.Now;
                    context.tbl_CareTracking.Add(item);
                    context.SaveChanges();
                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public tbl_CareTracking DeleteCareTracking(int id, int userId)
        {

            try
            {
                tbl_CareTracking oldItem;
                using (var context = new ITSEntities())
                {

                    oldItem = (from p in context.tbl_CareTracking
                               where p.ID == id && p.Status == 1
                               select p).FirstOrDefault();

                }

                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {
                        oldItem.Status = 0;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = userId;
                        context.tbl_CareTracking.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                }

                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }
                return oldItem;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public List<tbl_CareTracking> GetCareTrackings()
        {

            try
            {
                using (var context = new ITSEntities())
                {
                    var items = (from p in context.tbl_CareTracking
                                 where p.Status == 1
                                 select p);

                    return items.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public tbl_CareTracking GetCareTrackingById(Int64 Id)
        {

            try
            {
                using (var context = new ITSEntities())
                {


                    var item = (from p in context.tbl_CareTracking
                                where p.ID == Id && p.Status == 1
                                select p).FirstOrDefault();

                    return item;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public tbl_CareTracking UpdateCareTracking(tbl_CareTracking item)
        {
            try
            {
                tbl_CareTracking oldItem;
                using (var context = new ITSEntities())
                {
                    oldItem = (from p in context.tbl_CareTracking
                               where p.ID == item.ID && p.Status == 1
                               select p).FirstOrDefault();

                }
                if (oldItem != null)
                {
                    using (var context = new ITSEntities())
                    {
                        oldItem.CareDate = item.CareDate;
                        oldItem.BusinessCenterID = item.BusinessCenterID;
                        oldItem.MachineGroupID = item.MachineGroupID;
                        oldItem.MachineID = item.MachineID;
                        oldItem.CareDescription = item.CareDescription;
                        oldItem.CareType = item.CareType;
                        oldItem.PlanedCareType = item.PlanedCareType;
                        oldItem.CareTeamType = item.CareTeamType;
                        oldItem.ResultType = item.ResultType;
                        oldItem.ResultDescription = item.ResultDescription;
                        oldItem.UpdateDate = DateTime.Now;
                        oldItem.UpdateUser = item.UpdateUser;
                        context.tbl_CareTracking.Attach(oldItem);
                        context.Entry(oldItem).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return oldItem;
                    }
                }
                else
                {
                    Exception ex = new Exception("Bu nomrede setir recor yoxdur");
                    throw ex;
                }


            }

            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
    }
}
