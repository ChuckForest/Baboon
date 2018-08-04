using ReadyEDI.EntityFactory.Elements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReadyEDI.EntityFactory
{
    [Serializable]
    public class Collection<T> : ICollection<T> where T : IEntity// : List<IEntity>
    {
        //private static NLog.Logger nLogger;
        //private static NLog.Logger getLogger()
        //{
        //    if (nLogger == null)
        //    {
        //        nLogger = NLog.LogManager.GetLogger("CRUDActions");
        //    }
        //    return nLogger;
        //}
        [NonSerialized]
        private List<T> _list = new List<T>();
        [NonSerialized]
        private string _collectionName = String.Empty;
        [NonSerialized]
        private List<CriteriaParameter> _parameters = new List<CriteriaParameter>();
        [NonSerialized]
        private int _defaultItemIndex = 0;
        [NonSerialized]
        private string _connectionString = String.Empty;
        [NonSerialized]
        private List<Notification> _notifications = new List<Notification>();
        [NonSerialized]
        private List<EntityException> _exceptions = new List<EntityException>();
        [NonSerialized]
        private string _DBName = "";
        [NonSerialized]
        private string _SchemaName = "";

        public string DBName
        {
            get { return String.IsNullOrWhiteSpace(_DBName) ? "" : String.Concat("[", _DBName.Trim(), "]."); }
            set { _DBName = value; }
        }
        [NonSerialized]
        private int _paginate = 0;
        [NonSerialized]
        private int _page = 1;
        [NonSerialized]
        private DataTable _dataTable = new DataTable();
        [NonSerialized]
        private bool _refresh = false;
        [NonSerialized]
        private bool _lazyLoad = true;
        [NonSerialized]
        private string _filter = String.Empty;
        [NonSerialized]
        private string _orderBy = String.Empty;

        [XmlIgnore]
        public string SchemaName
        {
            get { return String.IsNullOrWhiteSpace(_SchemaName) ? "dbo" : _SchemaName.Trim(); }
            set { _SchemaName = value; }
        }
        public Collection()
        {

        }

        public Collection(List<T> list)
        {
            _list = list;
        }

        [XmlIgnore]
        public string CollectionName
        {
            get { return _collectionName; }
            set { _collectionName = value; }
        }

        [XmlIgnore]
        public List<Notification> Notifications
        {
            get { return _notifications; }
            set { _notifications = value; }
        }
        [XmlIgnore]
        public List<EntityException> Exceptions
        {
            get { return _exceptions; }
            set { _exceptions = value; }
        }
        [XmlIgnore]
        public List<CriteriaParameter> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
        [XmlIgnore]
        public int DefaultItemIndex
        {
            get { return _defaultItemIndex; }
            set { _defaultItemIndex = value; }
        }
        [XmlIgnore]
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        [XmlIgnore]
        public int Paginate
        {
            get { return _paginate; }
            set { _paginate = value; }
        }
        [XmlIgnore]
        public int Page
        {
            get { return _page; }
            set { _page = value; }
        }
        [XmlIgnore]
        public DataTable DataTable
        {
            get { return _dataTable; }
            set { _dataTable = value; }
        }
        [XmlIgnore]
        public bool Refresh
        {
            get { return _refresh; }
            set { _refresh = value; }
        }
        [XmlIgnore]
        public bool LazyLoad
        {
            get { return _lazyLoad; }
            set { _lazyLoad = value; }
        }
        [XmlIgnore]
        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }
        [XmlIgnore]
        public string OrderBy
        {
            get { return _orderBy; }
            set { _orderBy = value; }
        }

        public void Save()
        {
            _list.ForEach(i => i.Save<T>());
        }

        public void SaveChanges()
        {
            //_list.ForEach(i => i.SaveChanges());
        }

        public void AddRange(Collection<T> collection)
        {
            _list.AddRange(collection.ToList());
        }

        public void AddRange(List<T> collection)
        {
            _list.AddRange(collection);
        }

        public void AddRow(DataRow row)
        {
            IEntity entity = (IEntity)Activator.CreateInstance(typeof(T), new object[] { });
            entity.ConnectionString = _connectionString;
            entity.__Elements.Where(el => /*el.MultipleResultSetIndex == null && */!el.IsCollection/* && !el.IsEntity*/).ToList<ElementBaseData>().ForEach(e => ProcessRow(entity, e, row));
            if (!_lazyLoad)
                entity.RefreshCollections();

            _list.Add((T)entity);
        }

        public void AddRange(SqlDataReader reader)
        {
            //getLogger().Trace(string.Format("Collection<{1}>->AddRange(reader)", this.ElementType.Name, this.ElementType.FullName));
            //if (getLogger().IsTraceEnabled)
            //{
            //    for (int i = 0; i < reader.VisibleFieldCount; i++)
            //    {
            //        getLogger().Trace(string.Format("Collection->AddRange->Reader->ColumnName {0} {1}", i.ToString(), reader.GetName(i)));
            //    }
            //}
            _dataTable.Clear();
            _dataTable.Load(reader);

            DataView dv = new DataView(_dataTable)
            {
                RowFilter = _filter,
                Sort = _orderBy
            };

            _list.Capacity = dv.Count;

            DataTable returnTable = dv.ToTable();

            List<DataRow> rows = null;
            if (_paginate > 0)
                rows = returnTable.AsEnumerable().ToList().Skip((_page - 1) * _paginate).Take(_paginate).ToList();
                //rows = returnTable.AsEnumerable().ToList().Take(_paginate * _page).ToList();
            else
                rows = returnTable.AsEnumerable().ToList();

            rows.ForEach(r => AddRow(r));

            //foreach (DataRow row in _dataTable.Rows)
            //{
            //    counter++;
            //    IEntity entity = (IEntity)Activator.CreateInstance(typeof(T), new object[] { });
            //    entity.Elements.Where(el => (el.MultipleResultSetIndex == null && el.IsCollection == false)).ToList<ElementBaseData>().ForEach(e => ProcessReader(e, reader));
            //    entity.RefreshCollections();
            //    _list.Add((T)entity);
            //    if (readEnd > 0 && counter == readEnd)
            //        continue;
            //}

            //while (reader.Read())
            //{
            //    //if (counter == readStart)
            //    //{
            //        counter++;
            //        IEntity entity = (IEntity)Activator.CreateInstance(typeof(T), new object[] { });
            //        entity.Elements.Where(el => (el.MultipleResultSetIndex == null && el.IsCollection == false)).ToList<ElementBaseData>().ForEach(e => ProcessReader(e, reader));
            //        entity.RefreshCollections();
            //        _list.Add((T)entity);
            //        if (readEnd > 0 && counter == readEnd)
            //            continue;
            //    //}
            //    //else
            //    //{
            //    //    counter++;
            //    //}
            //}
            ////getLogger().Trace(string.Format("-------- {0} rows ----------", counter));
        }


        public Collection<T> GetRange(int index, int count)
        {
            return _list.GetRange(index, count) as Collection<T>;
        }

        public void Reverse()
        {
            _list.Reverse();
        }

        public List<T> ToList()
        {
            List<T> ret = new List<T>();

            _list.ForEach(t => ret.Add((T)t));
            ret.Capacity = _list.Capacity;

            return ret;
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public void Add(IEntity item)
        {
            _list.Add((T)item);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public Collection<T> Except(Collection<T> second)
        {
            List<T> list = _list.Except(second.ToList()).ToList();
            Collection<T> ret = new Collection<T>(list);

            return ret;
        }

        public Collection<IEntity> ToInterface()
        {
            Collection<IEntity> ret = new Collection<IEntity>();

            _list.ForEach(t => ret.Add(t as IEntity));

            return ret;
        }

        private void ProcessPropertyEntityRow(IEntity entity, ElementBaseData element, DataRow row, List<string> parentPropertyEntityNames)
        {
            IEntity propertyEntity;
            if (element.Data == null)
                propertyEntity = (IEntity)Activator.CreateInstance(entity.GetType().AssemblyQualifiedName.Split(',')[1].Trim(), String.Format("{0}.{1}", entity.GetType().Namespace, element.TypeName)).Unwrap();
            else
                propertyEntity = (IEntity)element.Data;

            parentPropertyEntityNames.Add(propertyEntity.__EntityName);

            propertyEntity.__Elements.Where(ebd => !ebd.IsEntity).ToList().ForEach(ebd =>
            {
                string rowName = String.Format("{0}_{1}", String.Join("_", parentPropertyEntityNames), ebd.Name);
                if (row.Table.Columns.Contains(rowName))
                    ebd.Data = row[rowName];
            });

            propertyEntity.__Elements.Where(ebd => ebd.IsEntity).ToList().ForEach(ebd =>
            {
                ProcessPropertyEntityRow(propertyEntity, ebd, row, parentPropertyEntityNames);
            });

            //foreach (ElementBaseData ebd in propertyEntity.__Elements.Where())
            //{
            //    if (ebd.IsEntity)
            //    {
            //        ProcessPropertyEntityRow(propertyEntity, ebd, row, parentPropertyEntityNames);
            //    }
            //    else if (row.Table.Columns.Contains(propertyEntity.__EntityName + "_" + ebd.Name))
            //    {
            //        ebd.Data = row[propertyEntity.__EntityName + "_" + ebd.Name];
            //    }
            //}

            element.Data = propertyEntity;
        }

        private void ProcessRow(IEntity entity, ElementBaseData element, DataRow row)
        {
            if (element.IsEntity)
            {
                ProcessPropertyEntityRow(entity, element, row, new List<string>());
                //IEntity propertyEntity;
                //if (element.Data == null)
                //    propertyEntity = (IEntity)Activator.CreateInstance(entity.GetType().AssemblyQualifiedName.Split(',')[1].Trim(), String.Format("{0}.{1}", entity.GetType().Namespace, element.TypeName)).Unwrap();
                //else
                //    propertyEntity = (IEntity)element.Data;

                //foreach (ElementBaseData ebd in propertyEntity.__Elements)
                //{
                //    if (row.Table.Columns.Contains(propertyEntity.__EntityName + "_" + ebd.Name))
                //    {
                //        ebd.Data = row[propertyEntity.__EntityName + "_" + ebd.Name];
                //    }
                //}

                //element.Data = propertyEntity;
            }
            else
            {
                if (row.Table.Columns.Contains(element.Name))
                {
                    element.Data = row[element.Name];
                    //var index = reader.GetOrdinal(element.Name);
                    ////getLogger().Trace(string.Format("Collection->AddRange->ProcessReader {0} {1}", element.Name, reader.GetValue(reader.GetOrdinal(element.Name))));
                    //element.Data = reader.GetFieldType(index).Name == "Boolean" ? reader.GetBoolean(index) : reader.GetValue(index);
                }
                else
                {
                    //getLogger().Debug(String.Format("---------------------------------- element '{0}' not found in data reader", element.Name));
                }
            }
        }

        //private bool HasColumn(DataRow row, string columnName)
        //{
        //    return row.Table.Columns.Contains(columnName);
        //}

        private void ProcessReader(ElementBaseData element, SqlDataReader reader)
        {
            if (HasColumn(reader, element.Name))
            {
                var index = reader.GetOrdinal(element.Name);
                //getLogger().Trace(string.Format("Collection->AddRange->ProcessReader {0} {1}", element.Name, reader.GetValue(reader.GetOrdinal(element.Name))));
                element.Data = reader.GetFieldType(index).Name == "Boolean" ? reader.GetBoolean(index) : reader.GetValue(index);
            }
            else
            {
                //getLogger().Debug(String.Format("---------------------------------- element '{0}' not found in data reader", element.Name));
            }
        }

        private bool HasColumn(IDataRecord dr, string columnName)
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

        public Type ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        public void LoadFromJson(string jsonData)
        {

        }

        public void Clear()
        {
            _list.Clear();
        }

    }
}
////////using ReadyEDI.EntityFactory.Elements;
////////using System;
////////using System.Collections.Generic;
////////using System.Data;
////////using System.Data.SqlClient;
////////using System.Linq;
////////using System.Reflection;
////////using System.Runtime.Serialization;
////////using System.Text;
////////using System.Threading.Tasks;

////////namespace ReadyEDI.EntityFactory
////////{
////////    [Serializable]
////////    public class Collection<T> : ICollection<T> where T : IEntity// : List<IEntity>
////////    {
////////        //private static NLog.Logger nLogger;
////////        //private static NLog.Logger getLogger()
////////        //{
////////        //    if (nLogger == null)
////////        //    {
////////        //        nLogger = NLog.LogManager.GetLogger("CRUDActions");
////////        //    }
////////        //    return nLogger;
////////        //}
////////        private List<T> _list = new List<T>();

////////        private string _collectionName = String.Empty;
////////        [NonSerialized]
////////        private Dictionary<string, string> _parameters = new Dictionary<string, string>();
////////        private int _defaultItemIndex = 0;
////////        private string _connectionString = String.Empty;
////////        [NonSerialized]
////////        private List<Notification> _notifications = new List<Notification>();
////////        [NonSerialized]
////////        private List<EntityException> _exceptions = new List<EntityException>();
////////        private string _DBName = "";
////////        private string _SchemaName = "";

////////        public string DBName
////////        {
////////            get { return String.IsNullOrWhiteSpace(_DBName) ? "" : String.Concat("[", _DBName.Trim(), "]."); }
////////            set { _DBName = value; }
////////        }
////////        private int _paginate = 0;
////////        private int _page = 1;
////////        [NonSerialized]
////////        private DataTable _dataTable = new DataTable();
////////        private bool _refresh = false;
////////        private bool _lazyLoad = false;
////////        private string _filter = String.Empty;
////////        private string _orderBy = String.Empty;

////////        public string SchemaName
////////        {
////////            get { return String.IsNullOrWhiteSpace(_SchemaName) ? "dbo" : _SchemaName.Trim(); }
////////            set { _SchemaName = value; }
////////        }
////////        public Collection()
////////        {

////////        }

////////        public Collection(List<T> list)
////////        {
////////            _list = list;
////////        }

////////        public string CollectionName
////////        {
////////            get { return _collectionName; }
////////            set { _collectionName = value; }
////////        }

////////        public List<Notification> Notifications
////////        {
////////            get { return _notifications; }
////////            set { _notifications = value; }
////////        }
////////        public List<EntityException> Exceptions
////////        {
////////            get { return _exceptions; }
////////            set { _exceptions = value; }
////////        }

////////        public Dictionary<string, string> Parameters
////////        {
////////            get { return _parameters; }
////////            set { _parameters = value; }
////////        }

////////        public int DefaultItemIndex
////////        {
////////            get { return _defaultItemIndex; }
////////            set { _defaultItemIndex = value; }
////////        }

////////        public string ConnectionString
////////        {
////////            get { return _connectionString; }
////////            set { _connectionString = value; }
////////        }

////////        public int Paginate
////////        {
////////            get { return _paginate; }
////////            set { _paginate = value; }
////////        }

////////        public int Page
////////            {
////////            get { return _page; }
////////            set { _page = value; }
////////        }

////////        public DataTable DataTable
////////                {
////////            get { return _dataTable; }
////////            set { _dataTable = value; }
////////                }

////////        public bool Refresh
////////            {
////////            get { return _refresh; }
////////            set { _refresh = value; }
////////        }

////////        public bool LazyLoad
////////        {
////////            get { return _lazyLoad; }
////////            set { _lazyLoad = value; }
////////        }

////////        public string Filter
////////        {
////////            get { return _filter; }
////////            set { _filter = value; }
////////        }

////////        public string OrderBy
////////        {
////////            get { return _orderBy; }
////////            set { _orderBy = value; }
////////        }

////////        public void Save()
////////        {
////////            _list.ForEach(i => i.Save<T>());
////////        }

////////        public void SaveChanges()
////////        {
////////            //_list.ForEach(i => i.SaveChanges());
////////        }

////////        public void AddRange(Collection<T> collection)
////////        {
////////            _list.AddRange(collection.ToList());
////////        }

////////        public void AddRow(DataRow row)
////////        {
////////                IEntity entity = (IEntity)Activator.CreateInstance(typeof(T), new object[] { });
////////            entity.Elements.Where(el => (el.MultipleResultSetIndex == null && el.IsCollection == false)).ToList<ElementBaseData>().ForEach(e => ProcessRow(e, row));
////////                entity.RefreshCollections();
////////            _list.Add((T)entity);
////////            }

////////        public void AddRange(SqlDataReader reader)
////////        {
////////            //getLogger().Trace(string.Format("Collection<{1}>->AddRange(reader)", this.ElementType.Name, this.ElementType.FullName));
////////            //if (getLogger().IsTraceEnabled)
////////            //{
////////            //    for (int i = 0; i < reader.VisibleFieldCount; i++)
////////            //    {
////////            //        getLogger().Trace(string.Format("Collection->AddRange->Reader->ColumnName {0} {1}", i.ToString(), reader.GetName(i)));
////////            //    }
////////            //}
////////            _dataTable.Load(reader);

////////            DataView dv = new DataView(_dataTable)
////////            {
////////                RowFilter = _filter,
////////                Sort = _orderBy
////////            };

////////            DataTable returnTable = dv.ToTable();

////////            List<DataRow> rows = null;
////////            if (_paginate > 0)
////////                rows = returnTable.AsEnumerable().ToList().Skip((_page - 1) * _paginate).Take(_paginate).ToList();
////////            else
////////                rows = returnTable.AsEnumerable().ToList();

////////            rows.ForEach(r => AddRow(r));

////////            //foreach (DataRow row in _dataTable.Rows)
////////            //{
////////            //    counter++;
////////            //    IEntity entity = (IEntity)Activator.CreateInstance(typeof(T), new object[] { });
////////            //    entity.Elements.Where(el => (el.MultipleResultSetIndex == null && el.IsCollection == false)).ToList<ElementBaseData>().ForEach(e => ProcessReader(e, reader));
////////            //    entity.RefreshCollections();
////////            //    _list.Add((T)entity);
////////            //    if (readEnd > 0 && counter == readEnd)
////////            //        continue;
////////            //}

////////            //while (reader.Read())
////////            //{
////////            //    //if (counter == readStart)
////////            //    //{
////////            //        counter++;
////////            //        IEntity entity = (IEntity)Activator.CreateInstance(typeof(T), new object[] { });
////////            //        entity.Elements.Where(el => (el.MultipleResultSetIndex == null && el.IsCollection == false)).ToList<ElementBaseData>().ForEach(e => ProcessReader(e, reader));
////////            //        entity.RefreshCollections();
////////            //        _list.Add((T)entity);
////////            //        if (readEnd > 0 && counter == readEnd)
////////            //            continue;
////////            //    //}
////////            //    //else
////////            //    //{
////////            //    //    counter++;
////////            //    //}
////////            //}
////////            ////getLogger().Trace(string.Format("-------- {0} rows ----------", counter));
////////        }


////////        public Collection<T> GetRange(int index, int count)
////////        {
////////            return _list.GetRange(index, count) as Collection<T>;
////////        }

////////        public void Reverse()
////////        {
////////            _list.Reverse();
////////        }

////////        public List<T> ToList()
////////        {
////////            List<T> ret = new List<T>();

////////            _list.ForEach(t => ret.Add((T)t));

////////            return ret;
////////        }

////////        public void RemoveAt(int index)
////////        {
////////            _list.RemoveAt(index);
////////        }

////////        public void Add(IEntity item)
////////        {
////////            _list.Add((T)item);
////////        }

////////        public int Count
////////        {
////////            get { return _list.Count; }
////////        }

////////        public Collection<T> Except(Collection<T> second)
////////        {
////////            List<T> list = _list.Except(second.ToList()).ToList();
////////            Collection<T> ret = new Collection<T>(list);

////////            return ret;
////////        }

////////        public Collection<IEntity> ToInterface()
////////        {
////////            Collection<IEntity> ret = new Collection<IEntity>();

////////            _list.ForEach(t => ret.Add(t as IEntity));

////////            return ret;
////////        }

////////        private void ProcessRow(ElementBaseData element, DataRow row)
////////        {
////////            if (row.Table.Columns.Contains(element.Name))
////////            {
////////                element.Data = row[element.Name];
////////                //var index = reader.GetOrdinal(element.Name);
////////                ////getLogger().Trace(string.Format("Collection->AddRange->ProcessReader {0} {1}", element.Name, reader.GetValue(reader.GetOrdinal(element.Name))));
////////                //element.Data = reader.GetFieldType(index).Name == "Boolean" ? reader.GetBoolean(index) : reader.GetValue(index);
////////            }
////////            else
////////            {
////////                //getLogger().Debug(String.Format("---------------------------------- element '{0}' not found in data reader", element.Name));
////////            }
////////        }

////////        //private bool HasColumn(DataRow row, string columnName)
////////        //{
////////        //    return row.Table.Columns.Contains(columnName);
////////        //}

////////        private void ProcessReader(ElementBaseData element, SqlDataReader reader)
////////        {
////////            if (HasColumn(reader, element.Name))
////////            {
////////                var index = reader.GetOrdinal(element.Name);
////////                //getLogger().Trace(string.Format("Collection->AddRange->ProcessReader {0} {1}", element.Name, reader.GetValue(reader.GetOrdinal(element.Name))));
////////                element.Data = reader.GetFieldType(index).Name == "Boolean" ? reader.GetBoolean(index) : reader.GetValue(index);
////////            }
////////            else
////////            {
////////                //getLogger().Debug(String.Format("---------------------------------- element '{0}' not found in data reader", element.Name));
////////            }
////////        }

////////        private bool HasColumn(IDataRecord dr, string columnName)
////////        {
////////            for (int i = 0; i < dr.FieldCount; i++)
////////            {
////////                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
////////                {
////////                    //getLogger().Trace(String.Format("HasColumn Name: {0} found at index {1}", columnName, i));
////////                    return true;
////////                }
////////            }
////////            return false;
////////        }

////////        public Type ElementType
////////        {
////////            get
////////            {
////////                return typeof(T);
////////            }
////////        }

////////        public void LoadFromJson(string jsonData)
////////        {

////////        }

////////        public void Clear()
////////        {
////////            _list.Clear();
////////        }

////////    }
////////}
