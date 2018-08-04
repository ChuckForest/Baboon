using ReadyEDI.EntityFactory.Elements;
using ReadyEDI.EntityFactory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace ReadyEDI.EntityFactory.DataAccess
{
    public static partial class CRUDActions
    {
        public static void CrudSelect(IEntity entity)
        {
            //logActions("CrudSelect " + entity.EntityName);
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlParameter[] paras;
                        paras = BuildSqlParametersIn(entity.__Elements.Where(e => (e.UseForMatching || e.SqlDbIsPrimaryKey || (e.ParameterIn & entity.CurrentAction) == e.Bitwise)).ToList());
                        paras = paras.Distinct().ToArray();
                        StringBuilder Sql = new StringBuilder();
                        Sql.AppendFormat("select ([{3}]) from {0}[{1}].[{2}]", entity.DBName, entity.SchemaName, entity.EntityName, String.Join("], [", from paras2 in paras select (paras2.ParameterName)));
                        Sql.AppendFormat(" where {0} = {1} ", "1", "1");
                        //logActions(cmd);
                        try
                        {
                            cmd.CommandText = Sql.ToString();
                            //nLogger.Info(Sql);
                            conn.Open();
                            //StartWatch("Select");
                            cmd.ExecuteReader();
                            //StopWatch("Select");
                        }
                        catch (SqlException ex)
                        {
                            ProcessNotification(entity, ex, Notification.NoticeType.Exception);
                        }
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
        }

        public static void CrudUpdate(IEntity entity, long fieldsToSave)
        {
            //logActions("CrudUpdate " + entity.EntityName);
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlParameter[] paras;
                        if (fieldsToSave > 0)
                        {
                            paras = BuildSqlParameters(entity.__Elements.Where(e => e.UseForMatching || e.SqlDbIsPrimaryKey || (fieldsToSave & e.Bitwise) == e.Bitwise).ToList());
                        }
                        else
                        {
                            paras = BuildSqlParametersIn(entity.__Elements.Where(e => (e.UseForMatching || e.SqlDbIsPrimaryKey || (e.ParameterIn & entity.CurrentAction) == e.Bitwise)).ToList());
                        }
                        paras = paras.Distinct().ToArray();
                        StringBuilder Sql = new StringBuilder();
                        Sql.AppendFormat("Update {0}[{1}].[{2}] set ", entity.DBName, entity.SchemaName, entity.EntityName, "", "");

                        foreach (SqlParameter para in paras)
                        {
                            //para.SqlDbType
                            Sql.AppendFormat("\n[{0}] = '{1}' -- {2}", para.ParameterName, para.Value, para.DbType);
                        }
                        Sql.AppendFormat(" where {0}", "");

                        //logActions(cmd);
                        try
                        {
                            cmd.CommandText = Sql.ToString();
                            //nLogger.Info(Sql);
                            conn.Open();
                            //StartWatch("Update ExecuteNonQuery");
                            cmd.ExecuteNonQuery();
                            //StopWatch("Update ExecuteNonQuery");
                        }
                        catch (SqlException ex)
                        {
                            ProcessNotification(entity, ex, Notification.NoticeType.Exception);
                        }
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
        }

        public static void CrudCreate(IEntity entity)
        {
            //logActions("CrudCreate " + entity.EntityName);
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlParameter[] paras;
                        paras = BuildSqlParametersIn(entity.__Elements.Where(e => (e.UseForMatching || e.SqlDbIsPrimaryKey || (e.ParameterIn & entity.CurrentAction) == e.Bitwise)).ToList());
                        paras = paras.Distinct().ToArray();
                        StringBuilder Sql = new StringBuilder();
                        Sql.AppendFormat("Insert into {0}[{1}].[{2}] ([{3}]) ", entity.DBName, entity.SchemaName, entity.EntityName, String.Join("], [", from paras2 in paras select (paras2.ParameterName)));

                        foreach (SqlParameter para in paras)
                        {
                            Sql.AppendFormat(" '{0}' ", String.Join("', '", from paras2 in paras select (paras2.Value)));
                        }

                        //logActions(cmd);
                        try
                        {
                            cmd.CommandText = Sql.ToString();
                            //nLogger.Info(Sql);
                            conn.Open();
                            //StartWatch("Create");
                            cmd.ExecuteNonQuery();
                            //StopWatch("Create");
                        }
                        catch (SqlException ex)
                        {
                            ProcessNotification(entity, ex, Notification.NoticeType.Exception);
                        }
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
        }

        public static void CrudDelete(IEntity entity)
        {
            //logActions("CrudDelete " + entity.EntityName);
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlParameter[] paras;
                        paras = BuildSqlParametersIn(entity.__Elements.Where(e => (e.UseForMatching || e.SqlDbIsPrimaryKey || (e.ParameterIn & entity.CurrentAction) == e.Bitwise)).ToList());
                        paras = paras.Distinct().ToArray();
                        StringBuilder Sql = new StringBuilder();
                        Sql.AppendFormat("delete from {0}[{1}].[{2}] where {3} = {4}", entity.DBName, entity.SchemaName, entity.EntityName, "", "");
                        
                        //logActions(cmd);
                        try
                        {
                            cmd.CommandText = Sql.ToString();
                            //nLogger.Info(Sql);
                            conn.Open();
                            //StartWatch("Delete");
                            cmd.ExecuteNonQuery();
                            //StopWatch("Delete");
                        }
                        catch (SqlException ex)
                        {
                            ProcessNotification(entity, ex, Notification.NoticeType.Exception);
                        }
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
        }

        //TODO
        public static void CrudRetrieve<T>(Collection<T> entities) where T : IEntity
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString<T>(entities)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Concat(entities.DBName, entities.SchemaName, entities.CollectionName, "Retrive");
                        cmd.Parameters.AddRange(BuildSqlParameters<T>(entities));
                        logActions(cmd);
                        try
                        {
                            conn.Open();
                            StartWatch("Retrieve<T>(entities) ExecuteReader");
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                StopWatch("Retrieve<T>(entities) ExecuteReader");
                                entities.AddRange(reader);
                                //CRUDActions.getLogger().Info(string.Format("got {0} rows", entities.Count.ToString()));
                                reader.Close();
                            }
                        }
                        catch (SqlException ex)
                        {
                            if (ex.State > 99)
                            {
                                ProcessNotification(entities, ex, Notification.NoticeType.Error);
                            }
                            else
                            {
                                ProcessException(EntityExemptionType.SqlServerFailure, entities, ex);
                            }
                        }
                        cmd.Dispose();
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();

                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entities, ex);
            }
        }
        
        //TODO
        public static void CrudUpdate<T>(Collection<T> entities, bool throwExceptions = false) where T : IEntity
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString<T>(entities)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = String.Concat(entities.DBName, entities.SchemaName, entities.CollectionName, "CollectionUpdate");
                        //cmd.Parameters.AddWithValue("@Entities", BuildDataTable<T>(entities));
                        cmd.Parameters.AddRange(BuildSqlParameters<T>(entities));
                        logActions(cmd);
                        try
                        {
                            conn.Open();
                            StartWatch("Update<T> ExecuteNonQuery");
                            cmd.ExecuteNonQuery();
                            StopWatch("Update<T> ExecuteNonQuery");
                        }
                        catch (SqlException ex)
                        {
                            if (ex.State > 99)
                            {
                                ProcessNotification(entities, ex, Notification.NoticeType.Error);
                            }
                            else
                            {
                                ProcessException(EntityExemptionType.SqlServerFailure, entities, ex);
                                if (throwExceptions)
                                {
                                    throw ex.InnerException;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entities, ex);
                if (throwExceptions)
                {
                    throw ex.InnerException;
                }
            }
        }
        
        //TODO
        public static void CrudDelete<T>(Collection<T> entities) where T : IEntity
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entities)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Concat(entities.DBName, entities.SchemaName, entities.CollectionName, "Delete");
                        cmd.Parameters.AddRange(BuildSqlParameters<T>(entities));
                        try
                        {
                            conn.Open();
                            StartWatch("Delete<T> ExecuteNonQuery");
                            cmd.ExecuteNonQuery();
                            StopWatch("Delete<T> ExecuteNonQuery");
                        }
                        catch (SqlException ex)
                        {
                            if (ex.State > 99)
                            {
                                ProcessNotification(entities, ex, Notification.NoticeType.Error);
                            }
                            else
                            {
                                ProcessException(EntityExemptionType.SqlServerFailure, entities, ex);
                            }
                        }
                        finally
                        {
                            if (conn.State == ConnectionState.Open) conn.Close();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entities, ex);
            }
        }
    }
}

//BigInt
//Bit
//Decimal
//Float
//Int
//Money
//Real
//SmallInt
//SmallMoney
//TinyInt
