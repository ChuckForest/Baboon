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
    public enum EntityExemptionType
    {
        GeneralFailure,
        SqlServerFailure,
    }

    public static partial  class CRUDActions
    {
        private static Stopwatch Watch = new Stopwatch();

        private static void StartWatch(string text)
        {
            Watch.Reset();
            Watch.Start();
        }

        private static void StopWatch(string text)
        {
            Watch.Stop();
            //getLogger().Info(String.Format("------------------------------------------------------ {1} took {0} Milliseconds", Watch.ElapsedMilliseconds, text));
        }

        private static string GetConnectionString(string name, string collectionName)
        {
            string connectionString = String.Empty;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch (System.NullReferenceException ex)
            {
                System.Exception nEx = new System.Exception(String.Format("Could not find ConntectingSring '{0}'", name), ex);
                throw nEx;

            }
            return connectionString;
        }

        private static string GetConnectionString(IEntity entity)
        {
            string connectionString = String.Empty;
            if (entity.ConnectionString.Equals(String.Empty))
                connectionString = GetConnectionString(entity.GetType().Namespace, entity.EntityName);
            else
                connectionString = entity.ConnectionString;
            entity.ConnectionString = connectionString;
            return connectionString;
        }

        private static string GetConnectionString<T>(Collection<T> collection) where T : IEntity
        {
            string connectionString = String.Empty;

            if (collection.ConnectionString.Equals(String.Empty))
                connectionString = GetConnectionString("SideShow_Library", collection.CollectionName);
            else
                connectionString = collection.ConnectionString;
            collection.ConnectionString = connectionString;
            return connectionString;
        }

        private static void SetData(SqlDataReader reader, ElementBaseData element)
        {
            if (HasColumn(reader, element.Name))
            {
                var index = reader.GetOrdinal(element.Name);
                //element.Data = reader.GetValue(reader.GetOrdinal(element.Name));
                element.Data = reader.GetFieldType(index).Name == "Boolean" ? reader.GetBoolean(index) : reader.GetValue(index);
                //getLogger().Trace(String.Format("SetData Name: {0} Value: '{1}'", element.Name, element.Data));
            }
            else
            {
                //getLogger().Debug(String.Format("---------------------------------- element '{0}' not found in data reader", element.Name));

            }
        }

        private static bool HasColumn(this IDataRecord dr, string columnName) //in IDataRecord
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    //getLogger().Trace(String.Format("HasColumn Name: {0} found at index {1}", columnName, i));
                    return true;
                }
            }
            return false;
        }

        private static SqlParameter[] BuildSqlParameters(List<ElementBaseData> elements)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            elements.ForEach(e => parameters.Add(new SqlParameter(e.Name, e.Data)));

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
        
        private static SqlParameter[] BuildSqlParameters<T>(IEntity entity) where T : IEntity
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (KeyValuePair<string, string> kvp in entity.Parameters)
            {
                parameters.Add(new SqlParameter(kvp.Key, kvp.Value));
            }

            return parameters.ToArray();
        }

        private static void ConvertElementToSqlParameterIn(List<SqlParameter> parameters, ElementBaseData ebd)
        {
            //if (ebd.IsEntity)
            //{
            //    Type elementType = ebd.Data.GetType();
            //    IEntity entityInstance = (IEntity)Activator.CreateInstance(elementType);

            //    List<ElementBaseData> childElements = entityInstance.Elements.Where(i => (ebd.ChildParametersIn & i.Bitwise) = i.Bitwise);
            //}
            //else
                parameters.Add(new SqlParameter(ebd.Name, ebd.Data));
        }
        
        private static SqlParameter[] BuildSqlParametersIn(List<ElementBaseData> elements)
        {
            List<SqlParameter> ret = new List<SqlParameter>();

            elements.ForEach(i => ConvertElementToSqlParameterIn(ret, i));

            //for (int i = 0; i < elements.Count; i++)
            //{
            //    ret.Add(new SqlParameter(elements[i].Name, elements[i].Data));
            //}

            return ret.ToArray();
        }

        private static SqlParameter[] BuildSqlParametersOut(List<ElementBaseData> elements)
        {
            SqlParameter[] ret = new SqlParameter[elements.Count];

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].SqlSize == null)
                    ret[i] = new SqlParameter(elements[i].Name, (SqlDbType)elements[i].SqlDataType) { Direction = ParameterDirection.Output };
                else
                    ret[i] = new SqlParameter(elements[i].Name, (SqlDbType)elements[i].SqlDataType, (int)elements[i].SqlSize) { Direction = ParameterDirection.Output };
            }

            return ret;
        }

        private static SqlParameter[] BuildSqlParametersInOut(List<ElementBaseData> elements)
        {
            SqlParameter[] ret = new SqlParameter[elements.Count];

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].SqlSize == null)
                    ret[i] = new SqlParameter(elements[i].Name, elements[i].SqlDataType) { Direction = ParameterDirection.InputOutput };
                else
                    ret[i] = new SqlParameter(elements[i].Name, elements[i].SqlDataType.Value, (int)elements[i].SqlSize) { Direction = ParameterDirection.InputOutput };

                ret[i].Value = elements[i].Data;
            }

            return ret;
        }

        //private static DataTable BuildDataTable<T>(Collection<T> entities) where T : IEntity
        //{
        //    var dt = new DataTable();

        //    IEntity entityInstance = (IEntity)Activator.CreateInstance(typeof(T));

        //    foreach (var element in entityInstance.Elements)
        //    {
        //        dt.Columns.Add(element.Name);
        //    }

        //    foreach (var entity in entities.ToList())
        //    {
        //        var dr = dt.NewRow();
        //        foreach (var element in entity.Elements)
        //        {
        //            dr[element.Name] = element.Data;
        //        }
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}

        private static void HydrateEntity(IEntity entity, SqlParameter[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                HydrateElement(entity.__Elements.Find(e => e.Name == parameters[i].ParameterName), parameters[i]);
            }
        }

        private static void HydrateElement(ElementBaseData element, SqlParameter parameter)
        {
            if (parameter.SqlDbType.Equals(SqlDbType.Bit))
                element.Data = bool.Parse(parameter.Value.ToString());
            else
                element.Data = parameter.Value;
        }

        //private static void PopulateChildElement<T>(SqlDataReader reader, ElementBaseData element) where T : IEntity
        //{
        //    if (reader.NextResult())
        //    {
        //        if (element.IsCollection)
        //        {
        //            Type T2 = typeof(T).GetProperties().ToList().Find(p => p.Name.Equals(element.Name)).PropertyType.GetGenericArguments()[0];
        //            //getLogger().Info(string.Format("----------- {0}--------------", T2.FullName));
        //            MethodInfo method = typeof(CRUDActions).GetMethod("PopulateChildElementCollection", BindingFlags.NonPublic | BindingFlags.Static);
        //            MethodInfo generic = method.MakeGenericMethod(T2);
        //            object[] arguments = { reader, element };
        //            generic.Invoke(null, arguments);
        //        }
        //        else
        //        {
        //            if (reader.Read())
        //                (element.Data as IEntity).Elements.Where(el => (el.MultipleResultSetIndex == null && el.IsCollection == false)).ToList<ElementBaseData>().ForEach(e => SetData(reader, e));
        //            else
        //                (element.Data as IEntity).Notifications.Add(new Notification() { Severity = Notification.NoticeType.Warning, Message = "Record not found. for " });
        //        }
        //    }
        //}

        //private static void PopulateChildElementCollection<T2>(SqlDataReader reader, ElementBaseData element)
        //{
        //    (element.Data as Collection<T2>).AddRange(reader);
        //}

        private static void PopulateChildElement<T>(SqlDataReader reader, ElementBaseData element) where T : IEntity
        {
            //if (reader.NextResult())
            //{
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
                    if (reader.Read())
                        (element.Data as IEntity).__Elements.Where(el => (el.MultipleResultSetIndex == null && el.IsCollection == false)).ToList<ElementBaseData>().ForEach(e => SetData(reader, e));
                    else
                        (element.Data as IEntity).Notifications.Add(new Notification() { Severity = Notification.NoticeType.Warning, Message = "Record not found. for " });
                }
            //}
        }

        private static void PopulateChildElementCollection<T2>(SqlDataReader reader, ElementBaseData element) where T2 : IEntity
        {
            Collection<T2> col = new Collection<T2>();
            col.AddRange(reader);

            col.ToList().ForEach(i => (element.Data as List<T2>).Add(i));
            //(element.Data as Collection<T2>).AddRange(reader);
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
        


        //private static void HydrateEntity(IEntity entity, SqlParameter[] parameters)
        //{
        //    for (int i = 0; i < parameters.Length; i++)
        //    {
        //        getLogger().Trace(String.Format("HydrateEntity {0} Name: {1} Value: '{2}'", i, parameters[i].ParameterName, parameters[i].Value));
        //        HydrateElement(entity.Elements.Find(e => e.Name == parameters[i].ParameterName), parameters[i]);
        //    }
        //}

        //private static void HydrateElement(ElementBaseData element, SqlParameter parameter)
        //{
        //    getLogger().Trace(String.Format("HydrateEntity Name: {0} Value: '{1}' to {2}", parameter.ParameterName, parameter.Value, element.Name));
        //    if (parameter.SqlDbType.Equals(SqlDbType.Bit))
        //        element.Data = bool.Parse(parameter.Value.ToString());
        //    else
        //        element.Data = parameter.Value;
        //}
    }
}
