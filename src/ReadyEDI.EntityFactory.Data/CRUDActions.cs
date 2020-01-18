using ReadyEDI.EntityFactory.Elements;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Messaging;
using System.Reflection;
using System.Text;

namespace ReadyEDI.EntityFactory.Data
{
    public static class CRUDActions
    {
        private static string dataObjectNameDelimitor = "_";
        private static string splitEncoding = "&#59;";

        #region "Enumerators"

        public enum EntityExemptionType
        {
            GeneralFailure,
            SqlServerFailure,
        }

        #endregion

        #region "ConnectionString"

        private static string GetConnectionString(string name)
        {
            string connectionString = String.Empty;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch (System.NullReferenceException ex)
            {
                System.Exception nEx = new System.Exception(String.Format("Could not find ConnectionString '{0}'", name), ex);
                throw nEx;

            }
            return connectionString;
        }

        private static string GetConnectionString(IEntity entity)
        {
            string connectionString = String.Empty;
            if (entity.ConnectionString.Equals(String.Empty))
                connectionString = GetConnectionString(entity.GetType().Namespace);
            else
                connectionString = entity.ConnectionString;
            entity.ConnectionString = connectionString;
            return connectionString;
        }

        private static string GetConnectionString<T>(Collection<T> collection) where T : IEntity
        {
            string connectionString = String.Empty;

            if (collection.ConnectionString.Equals(String.Empty))
                connectionString = GetConnectionString(typeof(T).Namespace);
            else
                connectionString = collection.ConnectionString;
            collection.ConnectionString = connectionString;
            return connectionString;
        }

        #endregion

        #region "Parameters"

        private static SqlParameter[] BuildSqlParameters(List<ElementBaseData> elements, ParameterDirection direction)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (direction.Equals(ParameterDirection.Input))
                elements.ForEach(e =>
                {
                    if (e.IsEntity)
                    {
                        if (e.Data != null)
                            (e.Data as IEntity).__Elements.Where(elem => elem.SqlDbIsPrimaryKey).ToList().ForEach(elem =>
                            {
                                //if (elem.Name == "Id")
                                    parameters.Add(new SqlParameter(string.Concat(e.Name, "_", elem.Name), elem.Data));
                                //else
                                //    parameters.Add(new SqlParameter(elem.Name, elem.Data));
                            });
                    }
                    else
                    {
                        var parameter = new SqlParameter(e.Name, e.Data == null
                            ? DBNull.Value : e.Data);
                        if (e.SqlDataType.Equals(System.Data.SqlDbType.DateTime2))
                            parameter.SqlDbType = SqlDbType.DateTime2;
                        else if (e.SqlDataType.Equals(System.Data.SqlDbType.VarBinary))
                            parameter.SqlDbType = SqlDbType.VarBinary;
                        parameters.Add(parameter);
                    }
                });
            else
            {
                elements.ForEach(e =>
                {
                    SqlParameter param;

                    if (e.SqlSize != null)
                        param = new SqlParameter(e.Name, (SqlDbType)e.SqlDataType, (int)e.SqlSize) { Direction = direction };
                    else if (e.SqlScale != null)
                    {
                        param = new SqlParameter(e.Name, (SqlDbType)e.SqlDataType) { Direction = direction, Scale = Convert.ToByte((int)e.SqlScale) };
                        if (e.SqlPrecision != null)
                            param.Precision = Convert.ToByte((int)e.SqlPrecision);
                    }
                    else
                        param = new SqlParameter(e.Name, (SqlDbType)e.SqlDataType) { Direction = direction };

                    if (direction == ParameterDirection.InputOutput)
                        param.Value = e.Data == null ? DBNull.Value : e.Data;

                    parameters.Add(param);
                });
            }

            return parameters.ToArray();
        }

        private static SqlParameter[] BuildIntrinsicSqlParameters(List<ElementBaseData> elements)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            elements.ForEach(e => parameters.Add(new SqlParameter(e.Name, (e.Data as List<string>)[0])));

            return parameters.ToArray();
        }

        private static SqlParameter[] BuildFKSqlParameters(List<ElementBaseData> elements)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            elements.ForEach(e => parameters.Add(new SqlParameter(String.Concat(e.Name, "Id"), e.Data)));

            return parameters.ToArray();
        }

        private static SqlParameter[] BuildSqlParametersForAttachment(string entityName, List<ElementBaseData> elements)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            elements.ForEach(e => parameters.Add(new SqlParameter(String.Format("{0}_{1}", entityName, e.Name), e.Data == null ? DBNull.Value : e.Data)));

            return parameters.ToArray();
        }

        private static SqlParameter[] BuildSqlParameters<T>(IEntity entity) where T : IEntity
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (KeyValuePair<string, string> kvp in entity.Parameters)
            {
                parameters.Add(new SqlParameter(kvp.Key, kvp.Value));
            }

            return parameters.ToArray();
        }

        private static SqlParameter[] BuildSqlParameters<T>(Collection<T> entities) where T : IEntity
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (CriteriaParameter kvp in entities.Parameters)
            {
                parameters.Add(new SqlParameter(kvp.ColumnName, kvp.Constraint));
            }

            return parameters.ToArray();
        }

        private static DataTable BuildDataTable<T>(Collection<T> entities) where T : IEntity
        {
            var dt = new DataTable();

            IEntity entityInstance = (IEntity)Activator.CreateInstance(typeof(T));

            foreach (var element in entityInstance.__Elements)
            {
                dt.Columns.Add(element.Name);
            }

            foreach (var entity in entities.ToList())
            {
                var dr = dt.NewRow();
                foreach (var element in entity.__Elements)
                {
                    dr[element.Name] = element.Data;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        #endregion

        #region "Populate"

        private static void SetData(SqlDataReader reader, ElementBaseData element)
        {
            if (HasColumn(reader, element.Name))
            {
                int index = reader.GetOrdinal(element.Name);
                if (reader.IsDBNull(index))
                    element.Data = null;
                else if (element.IsCollection && element.TypeName.ToLower().Equals("string"))
                    element.Data = HydrateComponentElement(reader.GetValue(index).ToString());
                else
                    element.Data = reader.GetFieldType(index).Name == "Boolean" ? reader.GetBoolean(index) : reader.GetValue(index);
            }
        }

        private static bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        //private static void HydrateSummary(IEntity entity, SqlDataReader reader)
        //{
        //    while
        //}

        private static void HydrateEntity(IEntity entity, SqlParameter[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                HydrateElement(entity.__Elements.Find(e => e.Name == parameters[i].ParameterName), parameters[i]);
            }
        }

        private static List<string> HydrateComponentElement(string value)
        {
            var list = new List<string>();

            value.Split(';').ToList().ForEach(v =>
            {
                list.Add(v.Replace(splitEncoding, ";"));
            });

            return list;
        }

        private static void HydrateElement(ElementBaseData element, SqlParameter parameter)
        {
            if (parameter.SqlDbType.Equals(SqlDbType.Bit))
                element.Data = bool.Parse(parameter.Value.ToString());
            else if (element.IsCollection && element.TypeName.ToLower().Equals("string"))
                element.Data = HydrateComponentElement(parameter.Value.ToString());
            else
                element.Data = parameter.Value;
        }

        private static void PopulateChildElement<T>(SqlDataReader reader, ElementBaseData element) where T : IEntity
        {
            if (element.IsCollection)
            {
                Type T2 = typeof(T).GetProperties().ToList().Find(p => p.Name.Equals(element.Name)).PropertyType.GetGenericArguments()[0];
                MethodInfo method = typeof(CRUDActions).GetMethod("PopulateChildElementCollection", BindingFlags.NonPublic | BindingFlags.Static);
                MethodInfo generic = method.MakeGenericMethod(T2);
                object[] arguments = { reader, element };
                generic.Invoke(null, arguments);
            }
            else
            {
                if (reader.HasRows)
                {
                    if (element.Data == null)
                    {
                        Type T2 = typeof(T).GetProperties().ToList().Find(p => p.Name.Equals(element.Name)).PropertyType;
                        element.Data = (IEntity)Activator.CreateInstance(T2);
                    }

                    if (reader.Read())
                        (element.Data as IEntity).__Elements.Where(el => (el.MultipleResultSetIndex == null && el.IsCollection == false)).ToList<ElementBaseData>().ForEach(e => SetData(reader, e));
                    else
                        (element.Data as IEntity).Notifications.Add(new Notification() { Severity = Notification.NoticeType.Warning, Message = "Record not found. for " });
                }

                reader.NextResult();
            }
        }

        private static void PopulateChildElementCollection<T2>(SqlDataReader reader, ElementBaseData element) where T2 : IEntity
        {
            Collection<T2> col = new Collection<T2>();
            col.AddRange(reader);

            col.ToList().ForEach(i => (element.Data as List<T2>).Add(i));
        }

        private static void EncodeSplit(IEntity entity)
        {
            entity.__Elements.ForEach(e =>
            {
                if (e.IsCollection && e.TypeName.ToLower().Equals("string"))
                {
                    if (e.Data == null || e.Data.Equals(string.Empty))
                        e.Data = string.Empty;
                    else
                    {
                        var list = (e.Data as List<string>);
                        if (list != null)
                        {
                            list.ForEach(v => v.Replace(";", splitEncoding));
                            list.RemoveAll(v => v.Equals(string.Empty));
                            e.Data = string.Join(";", list);
                        }
                    }
                }
            });
        }

        private static void DecodeSplit(IEntity entity)
        {
            entity.__Elements.ForEach(e =>
            {
                if (e.IsCollection && e.TypeName.ToLower().Equals("string"))
                {
                    var list = HydrateComponentElement(e.Data.ToString());
                    e.Data = list;
                }
            });
        }

        #endregion

        #region "Actions"

        private static void Report(SqlCommand command)
        {
            //var queuePath = string.Empty;
            ////push queue
            //if (!MessageQueue.Exists(queuePath))
            //    MessageQueue.Create(queuePath);

            //using (var myQueue = new MessageQueue(queuePath))
            //{
            //    myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(SqlCommand) });
            //    myQueue.Send(command);
            //}
        }

        public static void Drop(IEntity entity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = string.Format("DROP TABLE [{0}].[(1)]", entity.SchemaName, entity.EntityName);
                        cmd.ExecuteNonQuery();
                    }

                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }        
        }

        public static void Create<T>(IEntity entity) where T : IEntity
        {
            try
            {
                int exceptionCount = entity.Exceptions.Count;
                int notificationCount = entity.Notifications.Count;

                EncodeSplit(entity);

                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.CommandText = String.Concat(
                        //    String.Format("[{0}].", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                        //    , entity.__EntityName, "_Create");
                        cmd.CommandText = String.Format("[{0}].{1}_Create", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName, entity.__EntityName);
                        SqlParameter[] parametersOut = BuildSqlParameters(entity.__Elements.Where(e => /*e.UseForMatching || */e.SqlDbIsPrimaryKey).ToList(), ParameterDirection.InputOutput);
                        cmd.Parameters.AddRange(parametersOut);
                        cmd.Parameters.AddRange(BuildSqlParameters(entity.__Elements.Where(e => (!e.IsCollection || e.TypeName.ToLower().Equals("string")) /*&& !e.IsEntity && !e.UseForMatching*/ && !e.SqlDbIsPrimaryKey && !e.DoNotPersist).ToList(), ParameterDirection.Input));

                        try
                        {
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
                        //finally
                        //{
                        //    if (conn.State == ConnectionState.Open) conn.Close();
                        //}

                        //cmd.Dispose();
                        if (!entity.AuditBypass) Report(cmd);
                    }

                    if (entity.AttachedCollections != null)
                        entity.AttachedCollections.ForEach(ac => UpdateAttachedCollection<T>(conn, tran, entity, ac));

                    if (entity.Exceptions.Count > exceptionCount || entity.Notifications.Count > notificationCount)
                        tran.Rollback();
                    else
                        tran.Commit();

                    if (conn.State == ConnectionState.Open) conn.Close();
                }

                
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }

        }

        public static void Match(IEntity entity)
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
                            , entity.__EntityName, "_Match");
                        SqlParameter[] parametersOut = BuildSqlParameters(entity.__Elements.Where(e => e.SqlDbIsPrimaryKey).ToList(), ParameterDirection.Output);
                        cmd.Parameters.AddRange(parametersOut);
                        cmd.Parameters.AddRange(BuildSqlParameters(entity.__Elements.Where(e => e.UseForMatching).ToList(), ParameterDirection.Input));
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

        public static void Retrieve<T>(IEntity entity) where T : IEntity
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
                            , entity.__EntityName, "_Retrieve");
                        cmd.Parameters.AddRange(BuildSqlParameters<T>(entity));
                        cmd.Parameters.AddRange(BuildSqlParameters(entity.__Elements.Where(e => e.SqlDbIsPrimaryKey).ToList(), ParameterDirection.Input));
                        if (entity.LazyLoad == 0)
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
                                    var fields = entity.__Elements.Where(el => (el.MultipleResultSetIndex == null && el.IsCollection == false && el.IsEntity == false)).ToList<ElementBaseData>();
                                    fields.ForEach(e => SetData(reader, e));

                                    //if (entity.LazyLoad == 0)
                                    //{
                                        List<ElementBaseData> elements = entity.__Elements.Where(e => e.MultipleResultSetIndex != null).ToList();
                                        if (elements.Count > 0)
                                        {
                                            elements = elements.OrderBy(e => e.MultipleResultSetIndex).ToList();
                                            //if (fields.Where(f => !f.SqlDbIsPrimaryKey).Any())
                                                reader.NextResult();
                                            elements.ForEach(e =>
                                            {
                                                PopulateChildElement<T>(reader, e);
                                            });
                                        }
                                    //}
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
                using (SqlConnection conn = new SqlConnection(GetConnectionString<T>(entities)))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (entities.CollectionName.Equals(String.Empty))
                            cmd.CommandText = String.Concat(String.Format("[{0}].", String.IsNullOrWhiteSpace(entities.SchemaName) ? "dbo" : entities.SchemaName)
                                , typeof(T).Name, "_Summary");
                        else
                            cmd.CommandText = String.Concat(String.Format("[{0}].", String.IsNullOrWhiteSpace(entities.SchemaName) ? "dbo" : entities.SchemaName)
                                , entities.CollectionName, "_Retrieve");
                        
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

                    if (!entities.CollectionName.Equals(String.Empty) && !entities.LazyLoad)
                    {
                        entities.ToList().ForEach(e => e.Load());
                    }

                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entities, ex);
            }
        }

        /*public static void Summary<T>(Collection<T> entities) where T : IEntity
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
                            , entities.__EntityName, "_Summary");

                        try
                        {
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    ret.AddHydrateSummary(entity, reader);
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
                                ProcessNotification(entity, ex, Notification.NoticeType.Error);
                            }
                            else
                            {
                                ProcessException(EntityExemptionType.SqlServerFailure, entity, ex);
                            }
                        }
                        cmd.Dispose();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
        }*/

        public static void Update<T>(IEntity entity) where T : IEntity
        { 
            try
            {
                int exceptionCount = entity.Exceptions.Count;
                int notificationCount = entity.Notifications.Count;

                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Concat(
                            String.Format("[{0}].", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                            , entity.__EntityName, "_Update");
                        SqlParameter[] para = BuildSqlParameters(entity.__Elements.Where(e => !e.IsCollection/* && !e.IsEntity*/ && !e.DoNotPersist).ToList(), ParameterDirection.Input);
                        para = para.Distinct().ToArray();
                        cmd.Parameters.AddRange(para);

                        try
                        {
                            EncodeSplit(entity);
                            cmd.ExecuteNonQuery();
                            DecodeSplit(entity);
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

                    entity.AttachedCollections.ForEach(ac => UpdateAttachedCollection<T>(conn, tran, entity, ac));

                    if (entity.Exceptions.Count > exceptionCount || entity.Notifications.Count > notificationCount)
                        tran.Rollback();
                    else
                        tran.Commit();

                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
        }

        public static void Merge<T>(IEntity entity, long fieldsToSave) where T : IEntity
        {
            try
            {
                int exceptionCount = entity.Exceptions.Count;
                int notificationCount = entity.Notifications.Count;

                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Concat(
                            String.Format("[{0}].", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                            , entity.__EntityName, "_Merge");
                        //if (fieldsToSave > 0)
                        //{
                            SqlParameter[] para = BuildSqlParameters(entity.__Elements.Where(e => /*e.UseForMatching || */(fieldsToSave & e.Bitwise) == e.Bitwise).ToList(), ParameterDirection.Input);
                            para = para.Distinct().ToArray();
                            cmd.Parameters.AddRange(para);
                        //}
                        //else
                        //{
                        //    SqlParameter[] para = BuildSqlParameters(entity.__Elements.Where(e => (e.UseForMatching || (e.ParameterIn & entity.CurrentAction) == e.Bitwise)).ToList(), ParameterDirection.Input);
                        //    para = para.Distinct().ToArray();
                        //    cmd.Parameters.AddRange(para);
                        //}

                        try
                        {
                            EncodeSplit(entity);
                            cmd.ExecuteNonQuery();
                            DecodeSplit(entity);
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

                    entity.AttachedCollections.ForEach(ac => UpdateAttachedCollection<T>(conn, tran, entity, ac));

                    if (entity.Exceptions.Count > exceptionCount || entity.Notifications.Count > notificationCount)
                        tran.Rollback();
                    else
                        tran.Commit();

                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }

        }

        /*public static void Update<T>(Collection<T> entities, bool throwExceptions = false) where T : IEntity
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
        }*/

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
                            , entity.__EntityName, "_Delete");
                        cmd.Parameters.AddRange(BuildSqlParameters(entity.__Elements.Where(e => e.SqlDbIsPrimaryKey).ToList(), ParameterDirection.Input));
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

        public static void Slice(IEntity entity, int field, IEnumerable<IEntity> remove)
        {
            if (!remove.Any())
                return;

            try
            {
                var fieldName = entity.__Elements.FindLast(elem => (long?)Math.Pow(2, field) == elem.Bitwise).Name;

                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = string.Concat(
                            string.Format("[{0}].", string.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                            , entity.__EntityName, "_", fieldName, "_Delete");

                        var staticParameter = new List<SqlParameter>();
                        entity.__Elements.Where(elem => elem.SqlDbIsPrimaryKey).ToList().ForEach(elem =>
                        {
                            staticParameter.Add(new SqlParameter(string.Concat(entity.EntityName, "_", elem.Name), elem.Data));
                        });

                        remove.ToList().ForEach(r =>
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(staticParameter.ToArray());

                            r.__Elements.Where(elem => elem.SqlDbIsPrimaryKey).ToList().ForEach(elem =>
                            {
                                cmd.Parameters.Add(new SqlParameter(string.Concat(fieldName, "_", elem.Name), elem.Data));
                            });

                            try
                            {
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
                        });
                    }

                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }
        }

        /*public static Guid GetMapGuid<T>(IEntity entity, int field, int id) where T : IEntity
        {
            try
            {
                var fieldName = entity.__Elements.FindLast(elem => (long?)Math.Pow(2, field) == elem.Bitwise).Name;
                var fieldType = entity.__Elements.FindLast(elem => (long?)Math.Pow(2, field) == elem.Bitwise).TypeName;

                using (SqlConnection conn = new SqlConnection(GetConnectionString(entity)))
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = string.Concat(
                            string.Format("[{0}].", string.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName)
                            , entity.__EntityName, "_", fieldName, "_Match");


                        //cmd.Parameters.Add(new SqlParameter(string.Concat(entity.EntityName, "_Id"), entity.__Elements))

                        var staticParameter = new List<SqlParameter>();
                        entity.__Elements.Where(elem => elem.SqlDbIsPrimaryKey).ToList().ForEach(elem =>
                        {
                            staticParameter.Add(new SqlParameter(string.Concat(entity.EntityName, "_", elem.Name), elem.Data));
                        });
                        staticParameter.Add(new SqlParameter(string.Concat(fieldName, "_Id"), elem.Data));

                        try
                        {
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

                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (System.Exception ex)
            {
                ProcessException(EntityExemptionType.GeneralFailure, entity, ex);
            }

            return Guid.NewGuid();
        }*/

        /*public static void Delete<T>(Collection<T> entities) where T : IEntity
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
        }*/

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
                        cmd.CommandText = String.Format("SELECT dbo.{0}{3}{1}({2}) as functionResult", entity.__EntityName, entity.GetActionName(), String.Join(",", args), dataObjectNameDelimitor);
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
                        cmd.CommandText = String.Format("SELECT dbo.{0}{3}{1}({2}) as functionResult", entity.__EntityName, entity.GetActionName(), String.Join(",", args), dataObjectNameDelimitor);
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

        private static void ExecuteCommand<T>(IEntity entity, SqlConnection sqlConnection, SqlCommand cmd) where T : IEntity
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = String.Format("[{0}].{1}{3}{2}", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName, entity.__EntityName, entity.GetActionName(), dataObjectNameDelimitor);
            cmd.Parameters.AddRange(BuildSqlParameters(entity.__Elements.Where(e => (e.ParameterIn & entity.CurrentAction) == entity.CurrentAction).ToList(), ParameterDirection.Input));
            cmd.Parameters.AddRange(BuildSqlParameters<T>(entity));
            SqlParameter[] parametersOut = BuildSqlParameters(entity.__Elements.Where(e => (e.ParameterOut & entity.CurrentAction) == entity.CurrentAction && !e.IsCollection && !e.IsEntity).ToList(), ParameterDirection.Output);
            cmd.Parameters.AddRange(parametersOut);
            SqlParameter[] parametersInOut = BuildSqlParameters(entity.__Elements.Where(e => (e.ParameterInOut & entity.CurrentAction) == entity.CurrentAction).ToList(), ParameterDirection.InputOutput);
            cmd.Parameters.AddRange(parametersInOut);
            List<ElementBaseData> entityParameters = entity.__Elements.FindAll(e => (e.ParameterOut & entity.CurrentAction) == entity.CurrentAction && (e.IsCollection || e.IsEntity));
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
            }
        }

        private static void UpdateAttachedCollection<T>(SqlConnection conn, SqlTransaction tran, IEntity entity, ElementBaseData attachedCollection) where T : IEntity
        {
            if (attachedCollection.Data == null)
                return;

            Type T2;
            if (attachedCollection.IsCollection)
            {
                T2 = typeof(T).GetProperties().ToList().Find(p => p.Name.Equals(attachedCollection.Name)).PropertyType.GetGenericArguments()[0];
            }
            else
                T2 = typeof(T).GetProperties().ToList().Find(p => p.Name.Equals(attachedCollection.Name)).PropertyType;

            //get the exended entity that contains the property
            /*var types = new List<Type>();
            var type = typeof(T);
            while (type.BaseType != null)
            {
                var e = type.BaseType as IEntity;
                if (e.__Elements.Where(el => el.Name.Equals(attachedCollection.Name)).Any())
                    entity = e;

                type = type.BaseType;
            }*/
            //entity = type as IEntity;

            
            var type = typeof(T);
            var entityName = type.Name;
            while (type != null && !type.GetTypeInfo().DeclaredProperties.Where(el => el.Name.Equals(attachedCollection.Name)).Any())
            {
                type = type.BaseType;
                if (type != null)
                    entityName = type.Name;
            }

            MethodInfo method = typeof(CRUDActions).GetMethod("UpdateAttachedCollectionType", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo generic = method.MakeGenericMethod(T2);
            object[] arguments = { conn, tran, entity, attachedCollection, entityName };
            generic.Invoke(null, arguments);
        }

        private static void UpdateAttachedCollectionType<T2>(SqlConnection conn, SqlTransaction tran, IEntity entity, ElementBaseData attachedCollection, string entityName) where T2 : IEntity
        {
            //EncodeSplit(entity);
            

            //if (conn.State.Equals(ConnectionState.Closed))
            //    conn.Open();

            if (attachedCollection.IsCollection && !attachedCollection.TypeName.ToLower().Equals("string"))
                (attachedCollection.Data as List<T2>).ForEach(e => UpdateAttachedCollectionItem<T2>(conn, tran, entity, /*para, */e, attachedCollection.Name/*, isNew*/, entityName));
            else
                UpdateAttachedCollectionItem<T2>(conn, tran, entity, /*para, */attachedCollection.Data as IEntity, attachedCollection.Name/*, isNew*/, entityName);

        }

        private static void UpdateAttachedCollectionItem<T2>(SqlConnection conn, SqlTransaction tran, IEntity entity, /*SqlParameter[] para, */IEntity attachedEntity, string attachedCollectionName/*, bool isNew*/, string entityName) where T2 : IEntity
        {
            try
            {

                if (attachedEntity == null)
                    return;

                EncodeSplit(attachedEntity);

                bool isNew = false;
                attachedEntity.__Elements.Where(ebd => /*ebd.UseForMatching || */ebd.SqlDbIsPrimaryKey).ToList().ForEach(ebd =>
                {
                    if ((ebd.TypeName.Equals("int") && int.Parse(ebd.Data.ToString()) == 0) || (ebd.TypeName.Equals("Guid") && Guid.Parse(ebd.Data.ToString()).Equals(Guid.Empty)))
                        isNew = true;
                });

                if (attachedEntity.MarkForDeletion)
                {
                    if (!isNew)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = String.Format("[{0}].{1}{3}{2}_Delete", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName, entity.__EntityName, attachedCollectionName, dataObjectNameDelimitor);

                            cmd.Parameters.AddRange(BuildSqlParametersForAttachment(entity.__EntityName, entity.__Elements.Where(e => /*e.UseForMatching || */e.SqlDbIsPrimaryKey || e.IsFlag).ToList()));
                            cmd.Parameters.AddRange(BuildSqlParametersForAttachment(attachedCollectionName, attachedEntity.__Elements.Where(e => /*e.UseForMatching || */e.SqlDbIsPrimaryKey || e.IsFlag).ToList()));

                            try
                            {
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
                                cmd.Parameters.Clear();
                                cmd.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    //EncodeSplit(attachedEntity);
                    //EncodeSplit(entity);
/*
                    var entityName = entity.__EntityName;
                    var elements = entity.__Elements;
                    var type = typeof(T2);
                    while (type != null && !elements.Where(el => el.Name.Equals(attachedCollectionName)).Any())
                    {       
                        type = type.DeclaringType;
                        if (type != null)
                        {
                            var iEntity = type as IEntity;
                            entityName = iEntity.__EntityName;
                            elements = iEntity.__Elements;
                        }
                    }*/
                    /*
                    var entityName = entity.__EntityName;
                    var type = typeof(T2);
                    while (type != null && !type.GetProperties().Where(p => p.Name.Equals(attachedCollectionName)).Any())
                    {
                        type = type.DeclaringType;
                        if (type != null)
                            entityName = (type as IEntity).__EntityName;
                    }
                    */
                    SqlParameter[] para;
                    if (isNew)
                        para = BuildSqlParameters(attachedEntity.__Elements.Where(e => /*e.UseForMatching || */e.SqlDbIsPrimaryKey).ToList(), ParameterDirection.InputOutput);
                    else
                        para = BuildSqlParameters(attachedEntity.__Elements.Where(e => /*e.UseForMatching || */e.SqlDbIsPrimaryKey).ToList(), ParameterDirection.Input);

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        //if (entityName == "Contact")
                        //    System.Diagnostics.Debugger.Break();

                        cmd.Transaction = tran;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = String.Format("[{0}].{1}{4}{2}{4}{3}", String.IsNullOrWhiteSpace(entity.SchemaName) ? "dbo" : entity.SchemaName, entityName, attachedCollectionName, isNew ? "Create" : "Update", dataObjectNameDelimitor);

                        SqlParameter[] parametersOut = para;
                        cmd.Parameters.AddRange(parametersOut);                       
                        SqlParameter[] para2 = BuildSqlParametersForAttachment(entityName, entity.__Elements.Where(e => /*e.UseForMatching || */e.SqlDbIsPrimaryKey || e.IsFlag).ToList());
                        cmd.Parameters.AddRange(para2);
                        cmd.Parameters.AddRange(BuildSqlParameters(attachedEntity.__Elements.Where(e => (!e.IsCollection || e.TypeName.ToLower().Equals("string")) && !e.IsEntity && /*!e.UseForMatching && */!e.SqlDbIsPrimaryKey && !e.DoNotPersist).ToList(), ParameterDirection.Input));
                        //cmd.Parameters.AddRange(BuildFKSqlParameters(attachedEntity.__Elements.Where(e => !e.IsCollection && e.IsEntity).ToList()));
                        //cmd.Parameters.AddRange(BuildIntrinsicSqlParameters(attachedEntity.__Elements.Where(e => e.IsCollection && e.TypeName.ToLower().Equals("string")).ToList()));

                        try
                        {
                            cmd.ExecuteNonQuery();
                            HydrateEntity(attachedEntity, parametersOut);

                            if (attachedEntity.LazyLoad >= 0)
                                attachedEntity.AttachedCollections.ForEach(ac => UpdateAttachedCollection<T2>(conn, tran, attachedEntity, ac));
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
                        catch (System.Exception ex)
                        {
                            ProcessException(EntityExemptionType.SqlServerFailure, entity, ex);
                        }
                        finally
                        {
                            cmd.Parameters.Clear();
                            cmd.Dispose();
                        }
                    }
                }
            }
            catch(System.Exception ex)
            {

            }
        }

        #endregion

        #region "Messaging"

        private static void ProcessNotification(IEntity entity, System.Exception ex, Notification.NoticeType severity)
        {
            Notification notification = new Notification() { Severity = severity, Message = ex.Message };
            entity.Notifications.Add(notification);
        }

        private static void ProcessNotification<T>(Collection<T> entities, System.Exception ex, Notification.NoticeType severity) where T : IEntity
        {
            Notification notification = new Notification() { Severity = severity, Message = ex.Message };
            entities.Notifications.Add(notification);
        }

        private static void ProcessException(EntityExemptionType exType, IEntity entity, System.Exception ex)
        {
            EntityException entityEx = new EntityException((int)exType, String.Format("{0} generated a system failure.", entity.GetType()), ex);
            List<string> variables = new List<string>();
            entity.Exceptions.Add(entityEx);
        }

        private static void ProcessException<T>(EntityExemptionType exType, Collection<T> entities, System.Exception ex) where T : IEntity
        {
            EntityException entityEx = new EntityException((int)exType, String.Format("{0} generated a system failure.", entities.GetType()), ex);
            List<string> variables = new List<string>();
            entities.Exceptions.Add(entityEx);
        }

        #endregion

    }
}
