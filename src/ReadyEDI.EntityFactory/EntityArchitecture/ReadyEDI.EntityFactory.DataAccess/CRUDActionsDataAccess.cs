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
        private static void ExecuteCommand<T>(IEntity entity, SqlConnection sqlConnection, SqlCommand cmd) where T : IEntity
        {
            //using (SqlCommand cmd = sqlConnection.CreateCommand())
            //{
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = String.Concat(entity.EntityName, entity.GetActionName());
                cmd.CommandText = String.Concat(
                    String.Format("[{0}].", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                            , entity.EntityName, entity.GetActionName());
                cmd.Parameters.AddRange(BuildSqlParametersIn(entity.__Elements.Where(e => (e.ParameterIn & entity.CurrentAction) == entity.CurrentAction).ToList()));
                cmd.Parameters.AddRange(BuildSqlParameters<T>(entity));
                SqlParameter[] parametersOut = BuildSqlParametersOut(entity.__Elements.Where(e => (e.ParameterOut & entity.CurrentAction) == entity.CurrentAction && !e.IsCollection && !e.IsEntity).ToList());
                cmd.Parameters.AddRange(parametersOut);
                SqlParameter[] parametersInOut = BuildSqlParametersInOut(entity.__Elements.Where(e => (e.ParameterInOut & entity.CurrentAction) == entity.CurrentAction).ToList());
                cmd.Parameters.AddRange(parametersInOut);
                List<ElementBaseData> entityParameters = entity.__Elements.FindAll(e => (e.ParameterOut & entity.CurrentAction) == entity.CurrentAction && (e.IsCollection || e.IsEntity));
                //conn.Open();
                try
                {
                    if (entityParameters.Count > 0)
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                (entityParameters[0].Data as Collection<IEntity>).AddRange(reader);
                                reader.Close();
                            }
                        }
                    }
                    else
                    {
                        cmd.ExecuteNonQuery();
                    }
                    HydrateEntity(entity, parametersOut);
                    HydrateEntity(entity, parametersInOut);
                }
                catch (SqlException ex)
                {
                    if (ex.State > 99)
                    {
                        ProcessNotification(entity, ex, Notification.NoticeType.Error);

                    }
                    else
                    {
                        ProcessException(EntityExemptionType.SqlServerFailure, entity, ex);
                    }
                }
                finally
                {
                    cmd.Dispose();
                    //if (conn.State == ConnectionState.Open) conn.Close();
                }
            //}
        }

        public static void Execute<T>(IEntity entity) where T : IEntity
        {
            try
            {
                if (entity.SqlConnection == null)
                {
                    using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                    {
                        conn.Open();
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            ExecuteCommand<T>(entity, conn, cmd);
                        }
                        conn.Close();
                    }
                }
                else
                {
                    using (SqlCommand cmd = entity.SqlConnection.CreateCommand())
                    {
                        cmd.Transaction = entity.SqlTransaction;
                        ExecuteCommand<T>(entity, entity.SqlConnection, cmd);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
        }

        public static bool ExecuteScalar<T>(IEntity entity) where T : IEntity
        {
            bool ret = false;
            try
            {
                List<ElementBaseData> elements = entity.__Elements.Where(e => (e.ParameterIn & entity.CurrentAction) == entity.CurrentAction).ToList();

                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        List<string> args = new List<string>();
                        elements.ForEach(e => args.Add(e.QualifySqlArgument()));
                        cmd.CommandText = String.Format("SELECT dbo.{0}{1}({2}) as functionResult", entity.EntityName, entity.GetActionName(), String.Join(",", args));
                        conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            try
                            {
                                dr.Read();
                                ret = dr["functionResult"].ToString().Equals("True");
                            }
                            catch (SqlException ex)
                            {
                                if (ex.State > 99)
                                {
                                    ProcessNotification(entity, ex, Notification.NoticeType.Error);
                                }
                                else
                                {
                                    ProcessException(EntityExemptionType.SqlServerFailure, entity, ex);
                                }
                            }
                            finally
                            {
                                dr.Close();
                            }
                        }
                        cmd.Dispose();
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
            return ret;
        }

        public static bool ExecuteScalar<T>(IEntity entity, ref object result) where T : IEntity
        {
            bool ret = false;
            try
            {
                List<ElementBaseData> elements = entity.__Elements.Where(e => (e.ParameterIn & entity.CurrentAction) == entity.CurrentAction).ToList();

                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        List<string> args = new List<string>();
                        elements.ForEach(e => args.Add(e.QualifySqlArgument()));
                        cmd.CommandText = String.Format("SELECT dbo.{0}{1}({2}) as functionResult", entity.EntityName, entity.GetActionName(), String.Join(",", args));
                        conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            try
                            {
                                dr.Read();
                                result = dr["functionResult"];
                                ret = true;
                            }
                            catch (SqlException ex)
                            {
                                if (ex.State > 99)
                                {
                                    ProcessNotification(entity, ex, Notification.NoticeType.Error);
                                }
                                else
                                {
                                    ProcessException(EntityExemptionType.SqlServerFailure, entity, ex);
                                }
                            }
                            finally
                            {
                                dr.Close();
                            }
                        }
                        cmd.Dispose();
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
            return ret;
        }

        public static void Create<T>(IEntity entity) where T : IEntity
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Concat(
                            String.Format("[{0}].", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                            , entity.EntityName, "Create");
                        SqlParameter[] parametersOut = BuildSqlParametersInOut(entity.__Elements.Where(e => e.UseForMatching).ToList());
                        cmd.Parameters.AddRange(parametersOut);
                        cmd.Parameters.AddRange(BuildSqlParameters(entity.__Elements.Where(e => !e.IsCollection && !e.UseForMatching).ToList()));

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            HydrateEntity(entity, parametersOut);
                        }
                        catch (SqlException ex)
                        {
                            if (ex.State > 99)
                            {
                                ProcessNotification(entity, ex, Notification.NoticeType.Error);

                            }
                            else
                            {
                                ProcessException(EntityExemptionType.SqlServerFailure, entity, ex);
                            }
                        }
                        finally
                        {
                            if (conn.State == ConnectionState.Open) conn.Close();
                        }
                        cmd.Dispose();
                    }

                    entity.AttachedCollections.ForEach(ac => UpdateAttachedCollection<T>(conn, entity, ac));

                    if (conn.State == ConnectionState.Open) conn.Close();

                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }

        }

        public static void Retrieve<T>(IEntity entity) where T : IEntity
        {
            try
            {
                //Stopwatch watch = new Stopwatch();
                //watch.Start();
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    //watch.Stop();
                    //watch.Restart();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        //watch.Stop();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Concat(
                            String.Format("[{0}].", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                            , entity.EntityName, "Retrieve");
                        cmd.Parameters.AddRange(BuildSqlParameters<T>(entity));
                        cmd.Parameters.AddRange(BuildSqlParameters(entity.__Elements.Where(e => e.UseForMatching).ToList()));
                        //cmd.Parameters.AddRange(BuildSqlParametersIn(entity.Elements.Where(e => (e.ParameterIn & entity.Action) == entity.Action).ToList()));
                        //cmd.Parameters.AddRange(BuildSqlParametersOut(entity.Elements.Where(e => (e.ParameterOut & entity.Action) == entity.Action).ToList()));
                        if(entity.LazyLoad == 0)
                        {
                            //cmd.Parameters.Add(new SqlParameter() { ParameterName = "LazyLoad", Value = true });
                        }
                        try
                        {
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    entity.__Elements.Where(el => (el.MultipleResultSetIndex == null && el.IsCollection == false)).ToList<ElementBaseData>().ForEach(e => SetData(reader, e));

                                    if (entity.LazyLoad == 0)
                                    {
                                        // lets deal with multiple resultsets
                                        List<ElementBaseData> elements = entity.__Elements.Where(e => e.MultipleResultSetIndex != null).ToList();
                                        if (elements.Count > 0)
                                        {
                                            elements = elements.OrderBy(e => e.MultipleResultSetIndex).ToList();
                                            reader.NextResult();
                                            elements.ForEach(e =>
                                                    {
                                                        PopulateChildElement<T>(reader, e);
                                                    }
                                                );
                                        }
                                    }
                                }
                                else
                                {
                                    ProcessNotification(entity, new System.Exception("Record not found."), Notification.NoticeType.Warning);
                                }
                                reader.Close();
                            }
                        }
                        catch (SqlException ex)
                        {
                            if (ex.State > 99)
                            {
                                entity.Notifications.Add(new Notification() { Severity = Notification.NoticeType.Error, Message = ex.Message });
                            }
                            else
                            {
                                ProcessException(EntityExemptionType.SqlServerFailure, entity, ex);
                            }
                        }
                        finally
                        {
                        }
                        cmd.Dispose();
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }

        }

        public static void Retrieve<T>(Collection<T> entities) where T : IEntity
        {
            try
            {
                //if (!entities.Refresh && entities.DataTable != null)
                //{
                //    return;
                //}
                //else
                //{
                    using (SqlConnection conn = new SqlConnection(GetConnectionString<T>(entities)))
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = String.Concat(
                            String.Format("[{0}].", String.IsNullOrWhiteSpace(entities.SchemaName) ? "dbo" : entities.SchemaName)
                                , entities.CollectionName, "Retrieve");
                            cmd.Parameters.AddRange(BuildSqlParameters<T>(entities));
                            try
                            {
                                conn.Open();
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    entities.AddRange(reader);
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
                //}
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entities, ex);
            }
        }

        public static void Update<T>(IEntity entity, long fieldsToSave) where T : IEntity
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Concat(
                            String.Format("[{0}].", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                            , entity.EntityName, "Update");
                        if (fieldsToSave > 0)
                        {
                            SqlParameter[] para = BuildSqlParameters(entity.__Elements.Where(e => e.UseForMatching || (fieldsToSave & e.Bitwise) == e.Bitwise).ToList());
                            para = para.Distinct().ToArray();
                            cmd.Parameters.AddRange(para);
                        }
                        else
                        {
                            SqlParameter[] para = BuildSqlParametersIn(entity.__Elements.Where(e => (e.UseForMatching || (e.ParameterIn & entity.CurrentAction) == e.Bitwise)).ToList());
                            para = para.Distinct().ToArray();
                            cmd.Parameters.AddRange(para);
                        }

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            if (ex.State > 99)
                            {
                                ProcessNotification(entity, ex, Notification.NoticeType.Exception);
                            }
                            else
                            {
                                ProcessException(EntityExemptionType.SqlServerFailure, entity, ex);
                            }
                        }
                        cmd.Dispose();
                    }
                  
                    entity.AttachedCollections.ForEach(ac => UpdateAttachedCollection<T>(conn, entity, ac));

                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }

        }

        private static void UpdateAttachedCollection<T>(SqlConnection conn, IEntity entity, ElementBaseData attachedCollection) where T : IEntity
        {
            Type T2 = typeof(T).GetProperties().ToList().Find(p => p.Name.Equals(attachedCollection.Name)).PropertyType.GetGenericArguments()[0];
            MethodInfo method = typeof(CRUDActions).GetMethod("UpdateAttachedCollectionType", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo generic = method.MakeGenericMethod(T2);
            object[] arguments = { conn, entity, attachedCollection };
            generic.Invoke(null, arguments);
        }

        private static void UpdateAttachedCollectionType<T2>(SqlConnection conn, IEntity entity, ElementBaseData attachedCollection) where T2 : IEntity
        {
            SqlParameter[] para = BuildSqlParameters(entity.__Elements.Where(e => e.UseForMatching).ToList());
            para = para.Distinct().ToArray();

            if (conn.State.Equals(ConnectionState.Closed))
                conn.Open();

            (attachedCollection.Data as List<T2>).ForEach(e => UpdateAttachedCollectionItem<T2>(conn, entity, para, e, attachedCollection.Name));
        }

        private static void UpdateAttachedCollectionItem<T2>(SqlConnection conn, IEntity entity, SqlParameter[] para, IEntity attachedEntity, string attachedCollectionName) where T2 : IEntity
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = String.Concat(
                    String.Format("[{0}].", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                    , entity.EntityName, attachedCollectionName, "Update");

                cmd.Parameters.AddRange(para);
                SqlParameter[] para2 = BuildSqlParameters(attachedEntity.__Elements.Where(e => e.UseForMatching || e.IsFlag).ToList());
                para2 = para2.Distinct().ToArray();
                cmd.Parameters.AddRange(para2);

                try
                {
                    //conn.Open();
                    cmd.ExecuteNonQuery();

                    attachedEntity.AttachedCollections.ForEach(ac => UpdateAttachedCollection<T2>(conn, attachedEntity, ac));
                }
                catch (SqlException ex)
                {
                    if (ex.State > 99)
                    {
                        ProcessNotification(entity, ex, Notification.NoticeType.Exception);
                    }
                    else
                    {
                        ProcessException(EntityExemptionType.SqlServerFailure, entity, ex);
                    }
                }
                finally
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }
        }
        
        public static void Delete<T>(Collection<T> entities) where T : IEntity
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entities)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Concat(
                            String.Format("[{0}].", String.IsNullOrWhiteSpace(entities.SchemaName) ? "dbo" : entities.SchemaName)
                            , entities.CollectionName, "Delete");
                        cmd.Parameters.AddRange(BuildSqlParameters<T>(entities));
                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
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

        public static void Delete(IEntity entity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Concat(
                            String.Format("[{0}].", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                            , entity.EntityName, "Delete");
                        cmd.Parameters.AddRange(BuildSqlParameters(entity.__Elements.Where(e => e.UseForMatching).ToList()));
                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            if (ex.State > 99)
                            {
                                ProcessNotification(entity, ex, Notification.NoticeType.Exception);
                            }
                            else
                            {
                                ProcessException(EntityExemptionType.SqlServerFailure, entity, ex);
                            }
                        }
                        finally
                        {
                            if (conn.State == ConnectionState.Open) conn.Close();
                        }
                        cmd.Dispose();
                    }
                    if (conn.State == ConnectionState.Open) conn.Close();

                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
        }

        public static void Update<T>(Collection<T> entities, bool throwExceptions = false) where T : IEntity
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString<T>(entities)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Concat(
                            String.Format("[{0}].", String.IsNullOrWhiteSpace(entities.SchemaName) ? "dbo" : entities.SchemaName)
                        , entities.CollectionName, "CollectionUpdate");
                        cmd.Parameters.AddWithValue("@Entities", BuildDataTable<T>(entities));
                        cmd.Parameters.AddRange(BuildSqlParameters<T>(entities));
                        //logActions(cmd);
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

    }
}
