using ReadyEDI.EntityFactory.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace ReadyEDI.EntityFactory
{
    [DataContract]
    [Serializable]
    public abstract class EntityAbstract : IEntity, ICloneable
    {
        [NonSerialized]
        private string _entityName = String.Empty;
        [NonSerialized]
        private List<ElementBaseData> _elements = new List<ElementBaseData>();
        [NonSerialized]
        private long _currentAction = 0;
        //private string _actionName = String.Empty;
        [NonSerialized]
        private List<Action> _actions = new List<Action>();
        [NonSerialized]
        private List<Notification> _notifications = new List<Notification>();
        [NonSerialized]
        private CRUDConflict _crudConflict = new CRUDConflict();
        //[NonSerialized]
        //private int _id = 0;
        [NonSerialized]
        private DataProxyCollection _memento = new DataProxyCollection();
        [NonSerialized]
        private List<EntityException> _exceptions = new List<EntityException>();
        [NonSerialized]
        private string _connectionString = String.Empty;
        [NonSerialized]
        private Guid _queueId = Guid.Empty;
        [NonSerialized]
        private Dictionary<string, string> _paramerters = new Dictionary<string, string>();
        [NonSerialized]
        private string _DBName = String.Empty;
        [NonSerialized]
        private string _SchemaName = String.Empty;
        
        [NonSerialized]
        private SqlConnection _sqlConnection = null;
        [NonSerialized]
        private SqlTransaction _sqlTransaction = null;
        [NonSerialized]
        private long _lazyLoad = 0;
        [NonSerialized]
        private bool _populated = false;
        [NonSerialized]
        private List<ElementBaseData> _attachedCollections = new List<ElementBaseData>();
        [NonSerialized]
        private bool _markForDeletion = false;

        [OnDeserializing]
        void OnDeserializing(StreamingContext ctx)
        {
            _entityName = String.Empty;
            _elements = new List<ElementBaseData>();
            _currentAction = 0;
            _actions = new List<Action>();
            _notifications = new List<Notification>();
            _crudConflict = new CRUDConflict();
            //_id = 0;
            _memento = new DataProxyCollection();
            _exceptions = new List<EntityException>();
            _connectionString = String.Empty;
            _queueId = Guid.Empty;
            _paramerters = new Dictionary<string, string>();
            _DBName = String.Empty;
            _SchemaName = String.Empty;
            _lazyLoad = 0;
        }

        //private bool _isDirty = false;
        [XmlIgnore]
        [Browsable(false)]
        public string DBName
        {
            get { return String.IsNullOrWhiteSpace(_DBName) ? "" : String.Concat("[", _DBName.Trim(), "]."); }
            set { _DBName = value; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public string SchemaName
        {
            get { return String.IsNullOrWhiteSpace(_SchemaName) ? "dbo" : _SchemaName.Trim(); }
            set { _SchemaName = value; }
        }
        [XmlIgnore]
        [Browsable(false)]
        public SqlConnection SqlConnection
        {
            get { return _sqlConnection; }
            set { _sqlConnection = value; }
        }
        [XmlIgnore]
        [Browsable(false)]
        public SqlTransaction SqlTransaction
        {
            get { return _sqlTransaction; }
            set { _sqlTransaction = value; }
        }
        [XmlIgnore]
        [Browsable(false)]
        public string __EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }
        [XmlIgnore]
        [Browsable(false)]
        public string EntityName
        {
            get { return _entityName; }
            set { _entityName = value; }
        }
        [XmlIgnore]
        [Browsable(false)]
        public List<ElementBaseData> __Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }
        [XmlIgnore]
        [Browsable(false)]
        public long CurrentAction
        {
            get { return _currentAction; }
            set { _currentAction = value; }
        }

        public string GetActionName()
        {
            return _actions.Find(a => a.Bitwise == _currentAction).Name;
            //return _actions[Convert.ToInt32(Math.Log(_action, 2))];
            //return Enum.GetNames(_actions)[Convert.ToInt32(Math.Log(_action, 2))];
            //return ((Enum)_actions).GetEnumerator().ToString();
            //_actions.GetType().Cast<T>().GetEnumerator();
            //Actions.GetName
            //Actions.GetType() as Action
        }

        [XmlIgnore]
        [Browsable(false)]
        public List<Action> __Actions
        {
            get { return _actions; }
            set { _actions = value; }
        }

        //public string ActionName
        //{
        //    get { return _actionName; }
        //    set { _actionName = value; }
        //}

        [XmlIgnore]
        //[DataMember]
        [Browsable(false)]
        public List<Notification> Notifications
        {
            get { return _notifications; }
            set { _notifications = value; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public CRUDConflict CRUDConflict
        {
            get { return _crudConflict; }
            set { _crudConflict = value; }
        }

        //public int _Id
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

        [XmlIgnore]
        [Browsable(false)]
        [HiddenInput(DisplayValue = false)]
        [ScaffoldColumn(false)]
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public Guid QueueId
        {
            get { return _queueId; }
            set { _queueId = value; }
        }

        //public bool IsDirty
        //{
        //    get { return _isDirty; }
        //    set { _isDirty = value; }
        //}

        [XmlIgnore]
        //[DataMember]
        [Browsable(false)]
        public DataProxyCollection __Memento
        {
            get { return _memento; }
            set { _memento = value; }
        }

        [XmlIgnore]
        //[DataMember]
        [Browsable(false)]
        public List<EntityException> Exceptions
        {
            get { return _exceptions; }
            set { _exceptions = value; }
        }
        [XmlIgnore]
        [Browsable(false)]
        public Dictionary<string, string> Parameters
        {
            get { return _paramerters; }
            set { _paramerters = value; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public long LazyLoad
        {
            get { return _lazyLoad; }
            set { _lazyLoad = value; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public bool Populated
        {
            get { return _populated; }
            set { _populated = value; }
        }
        [XmlIgnore]
        [Browsable(false)]
        public List<ElementBaseData> AttachedCollections
        {
            get { return _attachedCollections; }
            set { _attachedCollections = value; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public bool MarkForDeletion
        {
            get { return _markForDeletion; }
            set { _markForDeletion = value; }
        }

        public virtual object Clone()
        {
            IEntity entity = this.MemberwiseClone() as IEntity;

            List<ElementBaseData> elements = new List<ElementBaseData>();
            _elements.ForEach(e => elements.Add(e.Clone() as ElementBaseData));
            entity.__Elements = elements;

            return entity;
        }

        public virtual bool Validate()
        {
            Notifications.Where(n => n.Severity == Notification.NoticeType.Error).ToList().Clear();

            return __Elements.FindAll(e => !Validate(e)).Count == 0;
        }

        public bool IsDirty()
        {
            for (int i = 0; i < __Elements.Count(); i++)
            {
                if (!__Elements[i].IsCollection && !__Elements[i].IsEntity && __Elements[i].Data != null && !__Elements[i].Data.Equals(__Memento[i].Data))
                    return true;
            }

            return false;
        }

        private bool Validate(IBaseData elementBaseData)
        {
            List<Notification> elementExceptions = null;//cjf elementBaseData.Validate();
            _notifications.AddRange(elementExceptions);

            return elementExceptions.Count == 0;
        }

        public List<string> GetErrorMessages()
        {
            List<string> messages = new List<string>();

            _notifications.ForEach(n =>
            {
                bool ruleMessage = false;

                foreach(ElementBaseData ebd in __Elements.Where(el => el.Rules.Count > 0))
                {
                    Rule rule = ebd.Rules.Find(r => r.RuleGuid.Equals(n.RuleGuid));
                    if (rule != null)
                    {
                        messages.Add(rule.Message);
                        ruleMessage = true;
                        break;
                    }
                }

                if (!ruleMessage)
                {
                    messages.Add(n.Message);
                }
            });

            return messages;
        }

        public virtual long Compare()
        {
            long ret = 0;

            _elements.Where(e => !e.IsCollection).ToList().ForEach(e => 
                ret += CompareFieldData(e, _memento.Find(t => t.ElementId == e.ID))
            );

            return ret;
        }

        public virtual long Compare(DataProxyCollection data)
        {
            long ret = 0;

            _elements.Where(e => !e.IsCollection).ToList().ForEach(e => ret += CompareFieldData(e, data.Find(t => t.ElementId == e.ID)));

            return ret;
        }

        public virtual long Compare(IEntity to)
        {
            long ret = 0;

            _elements.Where(e => !e.IsCollection).ToList().ForEach(e => ret += CompareFieldData(e, to.__Elements.Find(t => t.ID == e.ID)));

            return ret;
        }

        private long CompareFieldData(IBaseData e1, DataProxy e2)
        {
            long ret = 0;

            if (e2 == null)
            {
                ret = e1.Bitwise ?? default(long);
                return ret;
            }
            else if (e2.Data == null)
                return ret;

            if (!e1.Data.Equals(e2.Data))
                ret = e1.Bitwise ?? default(long);

            return ret;
        }

        private long CompareFieldData(IBaseData e1, IBaseData e2)
        {
            long ret = 0;

            if (e1.Data != null && !e1.Data.Equals(e2.Data))
                ret = e1.Bitwise ?? default(long);

            return ret;
        }

        public DataProxyCollection GetData()
        {
            DataProxyCollection data = new DataProxyCollection();

            //data.Add(new DataProxy() { ElementId = -1, Data = _id });
            _elements.ForEach(e => GetData(data, e));// data.Add(new DataProxy() { ElementId = e.ID ?? default(int), Data = e.Data }));

            return data;
        }

        private void GetData(DataProxyCollection proxies, ElementBaseData element)
        {
            if (!element.IsCollection)//(!element.Data.GetType().Name.Equals("Collection`1"))
                proxies.Add(new DataProxy() { ElementId = element.ID ?? default(int), Data = element.Data });
        }

        public virtual void Load()
        {
            _memento = GetData();
        }

        public void Load(DataProxyCollection data)
        {
            ////DataProxy idProxy = data.Find(d => d.ElementId == -1);
            ////if (idProxy != null)
            ////{
            ////    _id = int.Parse(idProxy.Data.ToString());
            ////    data.Remove(idProxy);
            ////}
            data.ForEach(d => __Elements[d.ElementId].Data = d.Data);

            _memento = GetData();
        }

        public void UpdateFieldValue(DataProxy proxy)
        {
            ////if (proxy.ElementId == -1)
            ////    _id = int.Parse(proxy.Data.ToString());
            ////else
                _elements[proxy.ElementId].Data = proxy.Data;
        }

        public virtual void Save<T>() where T : IEntity
        {
            _memento = GetData();
        }

        public void RevertConflicts()
        {
            _crudConflict.Fields.ForEach(f => RevertField(f));
        }

        private void RevertField(int elementId)
        {
            _elements.Find(e => e.ID == elementId).Data = _memento.Find(m => m.ElementId == elementId);
        }

        public virtual void RefreshCollections()
        {

        }

        public List<string> GetMatchDataValues()
        {
            List<string> ret = new List<string>();
            _elements.Where(e => e.UseForMatching).ToList().ForEach(e => ret.Add(e.Data.ToString()));

            return ret;
        }

        public void Merge(IEntity entity)
        {
            __Elements.FindAll(e => !e.IsCollection).ForEach(e => e.Data = entity.__Elements.Find(p => p.ID == e.ID).Data);
        }

        public int AddField(ElementBaseData element)
        {
            int ret = __Elements.Count;
            element.ID = ret;
            element.Bitwise = Convert.ToInt64(Math.Pow(2, ret));

            __Elements.Add(element);
            _memento.Add(new DataProxy() { ElementId = ret, Data = element.Data });

            return ret;
        }

        //public long AddAction(string actionName, long inParams, long outParams)
        //{
        //    _actions.Add(new Action() { Name = actionName });

            

        //    return 0;
        //}

        public virtual void RegisterActions(Type actionEnum)
        {
            int initialCount = _actions.Count;
            string[] enumNames = Enum.GetNames(actionEnum);
            int endCount = initialCount + enumNames.Length;
            
            for (int i = initialCount; i < endCount; i++)
            {
                _actions.Add(new Action() { Name = enumNames[i - initialCount], Id = i + 1, Bitwise = Convert.ToInt64(Math.Pow(2, i)) });
            }
        }

        //public virtual bool Save<T>(bool bypassConflicts)
        //{
        //    CRUDFunctions.Save<Profile>(this, bypassConflicts);
        //    _memento = GetData();

        //    return true;
        //}

        public string JsonStringify()
        {
            List<string> FieldsAndValues = new List<string>();

            _elements.ForEach(e => FieldsAndValues.Add(JsonStringify(e)));

            //return String.Format("{{ {0} }}", String.Join(", ", FieldsAndValues));
            if (this.Notifications.Count == 0)
            {
                FieldsAndValues.Add("\"_notifications\":[]");
            }
            else
            {
                FieldsAndValues.Add(string.Format("\"{0}\":{1}", "\"Notifications\"", new JavaScriptSerializer().Serialize(this.Notifications)));
            }

            if (this.Exceptions.Count == 0)
            {
                FieldsAndValues.Add("\"_exceptions\":[]");
            }
            else
            {
                FieldsAndValues.Add(string.Format("\"{0}\":{1}", "\"Exceptions\"", new JavaScriptSerializer().Serialize(this.Exceptions)));
            }

            return String.Format("{{ {0} }}", String.Join(", ", FieldsAndValues.Where(x => !string.IsNullOrEmpty(x))));
        }

        private string JsonStringify(ElementBaseData element)
        {
            /*string ret = String.Empty;

            if (element is IEntity)
            {
                ret = String.Format("\"{0}\": \"{1}\"", element.Name, (element.Data as IEntity).JsonStringify());
            }
            else if (element.IsCollection)
            {
                (element.Data as IEnumerable<IEntity>).ToList().ForEach(
                    e => ret += e.JsonStringify()
                );
                ret = String.Format("\"{0}\": \"{1}\"", element.Name, ret);
            }
            else
            {
                ret = String.Format("\"{0}\": \"{1}\"", element.Name, element.Data.ToString());
            }

            return ret;*/

            string ret = String.Empty;
            float f;

            if (element is IEntity)
            {
                ret = element.Data != null ? String.Format("\"{0}\": \"{1}\"", element.Name, (element.Data as IEntity).JsonStringify()) : "";
            }
            else if (element.IsCollection)
            {
                if (element.Data != null)
                {
                    if (element.Data.ToString() == "")
                    {
                        ret = String.Format("\"{0}\": []", element.Name);
                    }
                    else
                    {
                        (element.Data as IEnumerable<IEntity>).ToList().ForEach(
                            e => ret += e.JsonStringify()
                        );
                        ret = String.Format("\"{0}\": \"{1}\"", element.Name, ret);
                    }
                }
                else
                {
                    ret = "";
                }
            }
            else if (element.Data != null && float.TryParse(element.Data.ToString(), out f))
            {
                ret = element.Data != null ? String.Format("\"{0}\": {1}", element.Name, element.Data.ToString()) : "";
            }
            else if (element.Data != null && (element.TypeName.Equals("bool") || element.TypeName.Equals("Boolean")))
            {
                ret = element.Data != null ? String.Format("\"{0}\": {1}", element.Name, element.Data.ToString().ToLower()) : "";
            }
            else if (element.IsDateTime)
            {
                ret = element.Data != null ? String.Format("\"{0}\": \"\\/Date({1})\\/\"", element.Name, (Convert.ToDateTime(element.Data).Ticks / 10000).ToString()) : "";
            }
            else
            {
                ret = element.Data != null ? String.Format("\"{0}\": \"{1}\"", element.Name, element.Data.ToString()) : "";
            }

            return ret;
        }

        public void LoadFromJson2(string jsonData)
        {
            List<string> nvps = Regex.Split(jsonData, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)").ToList();

            int childIndex = -1;
            List<ElementBaseData> ebdChildren = new List<ElementBaseData>();
            List<IEntity> children = new List<IEntity>();

            bool arrayStarted = false;
            bool arrayEnded = false;
            List<string> ebdArray = null;

            int counter = 0;

            foreach (string nvp in nvps)
            {
                counter++;

                string[] nvpArr = Regex.Split(nvp, ":(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                string fieldName = nvpArr[0].Replace("{", String.Empty).Replace("\"", String.Empty).Trim();

                int endFancyCount = nvpArr[1].Count(c => c.Equals('}'));
                if (nvpArr[1].Replace(" ", String.Empty).StartsWith("{}"))
                    endFancyCount--;
                if (counter == nvps.Count)
                    endFancyCount--;

                arrayEnded = endFancyCount > 0 && nvpArr[1].EndsWith("]");
                if (arrayEnded)
                    nvpArr[1] = nvpArr[1].Substring(0, nvpArr[1].Length - 1);
                nvpArr[1] = nvpArr[1].Replace("}", String.Empty);
                
                ElementBaseData ebd = null;
                if (childIndex == -1)
                    ebd = __Elements.Find(e => e.Name.Equals(fieldName));
                else
                {
                    IEntity entity = children.Last();
                    if (entity != null)
                        ebd = children.Last().__Elements.Find(e => e.Name.Equals(fieldName));
                }

                if (ebd == null)
                {
                    if (nvpArr[1].StartsWith("[") && !nvpArr[1].Replace(" ", String.Empty).Equals("[]"))
                    {
                        if (ebdChildren.Last() != null)
                        {
                            childIndex++;
                            ebdChildren.Add(null);
                            children.Add(null);
                        }
                    }
                }
                else
                {
                    if (nvp != nvps.First() && nvp.StartsWith("{"))
                    {
                        Type gType = ebdChildren.Last().Data.GetType().GetGenericArguments().First();

                        IEntity entity = Activator.CreateInstance(gType) as IEntity; //next entity
                        ElementBaseData ebdEntity = entity.__Elements.Find(e => e.Name.Equals(fieldName)); //first ebd in next entity

                        if (ebdEntity != null)
                        {

                            ebdEntity.Data = nvpArr[1].Replace("}", String.Empty).Replace("]", String.Empty).Replace("\"", String.Empty); //set value of first ebd
                        }

                        children.RemoveAt(children.Count - 1);
                        children.Add(entity);
                    }
                    else
                    {
                        if (ebd.IsCollection)
                        {
                            if (!nvpArr[1].Replace(" ", String.Empty).Equals("[]"))
                            {
                                Type gType = ebd.Data.GetType().GetGenericArguments().First();

                                string childFieldName = nvpArr[1].Replace("[", String.Empty).Replace("{", String.Empty).Replace("\"", String.Empty);

                                IEntity entity = Activator.CreateInstance(gType) as IEntity; //first entity
                                ElementBaseData ebdEntity = entity.__Elements.Find(e => e.Name.Equals(childFieldName)); //first ebd in first entity

                                if (ebdEntity != null)
                                {

                                    ebdEntity.Data = nvpArr[2].Replace("}", String.Empty).Replace("]", String.Empty).Replace("\"", String.Empty); //set value of first ebd
                                }

                                childIndex++;
                                ebdChildren.Add(ebd);
                                children.Add(entity);

                            }

                        }
                        else if (ebd.IsEntity)
                        {
                            if (!nvpArr[1].Replace(" ", String.Empty).Equals("{}"))
                            {
                                Type gType = ebd.Data.GetType();

                                string childFieldName = nvpArr[1].Replace("{", String.Empty).Replace("\"", String.Empty);

                                IEntity entity = Activator.CreateInstance(gType) as IEntity;
                                ElementBaseData ebdEntity = entity.__Elements.Find(e => e.Name.Equals(childFieldName));

                                if (ebdEntity != null)
                                    ebdEntity.Data = nvpArr[2].Replace("}", String.Empty).Replace("\"", String.Empty);

                                if (nvpArr[2].Contains("}"))
                                {
                                    ebd.Data = entity;
                                }
                                else
                                {
                                    childIndex++;
                                    ebdChildren.Add(ebd);
                                    children.Add(entity);
                                }
                            }
                        }
                        else
                        {
                            if (nvpArr[1].Equals("null"))
                                ebd.Data = null;
                            else if (nvpArr[1].StartsWith("["))
                            {
                                if (!nvpArr[1].Replace(" ", String.Empty).Equals("[]"))
                                {
                                    if (nvpArr[1].Contains("]"))
                                    {
                                        (ebd.Data as List<string>).Add(nvpArr[1].Replace("[", String.Empty).Replace("\"", String.Empty).Replace("]", String.Empty));
                                    }
                                    else
                                    {
                                        arrayStarted = true;
                                        ebdArray = ebd.Data as List<string>;
                                    }
                                }
                            }
                            else
                                ebd.Data = nvpArr[1].Replace("\"", String.Empty).Trim();


                        }
                    }
                }

                for (int i = 0; i < endFancyCount; i++)
                {
                    if (ebdChildren.Count == 0)
                        continue;

                    ElementBaseData ebdLast = ebdChildren.Last();
                    if (ebdLast == null)
                    {
                        ebdChildren.RemoveAt(ebdChildren.Count - 1);
                        children.RemoveAt(children.Count - 1);
                        childIndex--;
                    }
                    else
                    {
                        IEntity entityLast = children.Last();
                        if (ebdLast.IsCollection)
                        {
                            MethodInfo method = ebdLast.Data.GetType().GetMethod("Add");
                            method.Invoke(ebdLast.Data, new object[] { entityLast });

                            if (arrayEnded)
                            {
                                ebdChildren.Remove(ebdLast);
                                children.Remove(entityLast);
                                childIndex--;
                            }

                        }
                        else
                        {
                            ebdLast.Data = entityLast;
                            ebdChildren.Remove(ebdLast);
                            children.Remove(entityLast);
                            childIndex--;
                        }
                    }
                }
            }
        }

        public void LoadFromJson(string jsonData)
        {
            int startQualifier = jsonData.IndexOf("\"") + 1;
            int endQualifier = startQualifier + jsonData.Substring(startQualifier).IndexOf("\"") + 1;
            int startPos = endQualifier + jsonData.Substring(endQualifier).IndexOf(":");

            while (startPos > -1)
            {
                string fieldName = GetFieldName(jsonData.Substring(0, startPos));

                if (fieldName.Equals("refunds") && !jsonData.Substring(12, 2).Equals("[]"))
                {
                    int x = 1;
                }

                int endPos;
                bool isArray;
                string fieldValue = GetFieldValue(jsonData.Substring(startPos + 1).Trim(), out endPos, out isArray);

                ElementBaseData ebd = __Elements.Find(e => e.Name.Equals(fieldName));
                if (ebd != null)
                {
                    if (ebd.IsCollection)
                    {
                        Type type = this.GetType();
                        PropertyInfo prop = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
                        Type gType = prop.PropertyType.GetGenericArguments().First();
                        List<string> fieldValueList = GetFieldValueList(fieldValue);
                        fieldValueList.ForEach(v => AddNewEntity(gType, ebd, v));                       
                    }
                    else if (ebd.IsEntity)
                    {
                        Type type = this.GetType();
                        PropertyInfo prop = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
                        Type gType = prop.PropertyType;
                        CreateNewEntity(gType, ebd, fieldValue);
                    }
                    else
                    {
                        if (fieldValue.Equals("null"))
                            ebd.Data = null;
                        else if (isArray)
                            ebd.Data = new string[] { fieldValue.Replace("\"", String.Empty) }.ToList();
                        else
                            ebd.Data = fieldValue.Replace("\"", String.Empty);
                    }
                };

                int i = endPos + startPos + 2;
                if (i > jsonData.Length)
                    i = jsonData.Length;
                jsonData = jsonData.Substring(i);
                startQualifier = jsonData.IndexOf("\"") + 1;
                endQualifier = startQualifier + jsonData.Substring(startQualifier).IndexOf("\"") + 1;
                startPos = endQualifier + jsonData.Substring(endQualifier).IndexOf(":");
            }
        }

        private List<string> GetFieldValueList(string fieldValue)
        {
            List<string> ret = new List<string>();

            if (fieldValue.Equals(String.Empty))
            return ret;

            List<string> start = Regex.Split(fieldValue, "},").ToList();//fieldValue.Split(new char[] {'}', ','}).ToList();
            //start.Remove(start.Last());
            //start.ForEach(s => ret.Add(RemoveFancy(s)));
            string str = String.Empty;
            foreach (string s in start)
            {
                if (s.StartsWith("{"))
                {
                    if (!str.Equals(String.Empty))
                        ret.Add(str);

                    str = s;
        }
                else
                {
                    str += string.Format("}}{0}", s);
                }
            }

            if (!str.Equals(String.Empty))
                ret.Add(str);

            List<string> ret2 = new List<string>();
            ret.ForEach(s => ret2.Add(s.Substring(s.IndexOf("{") + 1, s.Length - 1)));

            return ret2;
        }

        private string RemoveFancy(string s)
        {
            string ret = String.Empty;

            int startPos = s.IndexOf("{") + 1;
            ret = s.Substring(startPos);

            return ret;
        }

        private void AddNewEntity(Type gType, ElementBaseData ebd, string fieldValue)
            {
            object o = Activator.CreateInstance(gType);
            (o as IEntity).LoadFromJson(fieldValue);

            MethodInfo method = ebd.Data.GetType().GetMethod("Add");
            method.Invoke(ebd.Data, new object[] { o });
            }

        private void CreateNewEntity(Type gType, ElementBaseData ebd, string fieldValue)
        {
            object o = Activator.CreateInstance(gType);
            (o as IEntity).LoadFromJson(fieldValue);

            ebd.Data = o;
        }

        private string GetFieldValue(string jsonData, out int endPos, out bool isArray)
        {
            string ret = String.Empty;
            endPos = 0;
            isArray = false;

            if (jsonData.First().Equals('['))
            {
                isArray = true;
                endPos = jsonData.Substring(1).IndexOf("[");
                if (endPos == -1) //no other array
                {
                    endPos = jsonData.Substring(1).IndexOf("]");
                    if (endPos == -1)
                    {
                        ret = jsonData.Substring(1) + "}";
                        endPos = ret.Length;
                    }
                    else
                    {
                        ret = jsonData.Substring(1, endPos);
                    }
                }
                else
                {
                    int inBetween = jsonData.Substring(1, endPos).IndexOf("]");
                    if (inBetween == -1)
                    {
                        //end of inner array
                        endPos = endPos + jsonData.Substring(endPos).IndexOf("]") + 1;

                        if (jsonData.Substring(endPos, 1) != "}")
                        {
                            while (jsonData.Substring(endPos + 1, 1) != "]")
                            {
                                endPos = endPos + jsonData.Substring(endPos + 1).IndexOf("[");
                                endPos = endPos + jsonData.Substring(endPos).IndexOf("]") + 1;
                            }

                            //endPos = endPos + jsonData.Substring(endPos + 1).IndexOf("[");
                            //endPos = endPos + jsonData.Substring(endPos).IndexOf("]") + 1;

                            //endPos = endPos + jsonData.Substring(endPos).IndexOf("]") + 1;
                        }

                        
                        //endPos = jsonData.IndexOf("}");
                        ret = jsonData.Substring(1, endPos);
                        endPos++;
                        //endPos = endPos + jsonData.Substring(endPos).IndexOf("]") + 1;
                        ////endPos = endPos + jsonData.Substring(endPos).IndexOf("[");

                        ////endPos = endPos + jsonData.Substring(endPos).IndexOf("]");
                        ////endPos = endPos + jsonData.Substring(endPos).IndexOf("[");

                        ////endPos = endPos + jsonData.Substring(endPos).IndexOf("]");
                        ////endPos = endPos + 1;
                        //int endPos2 = endPos + jsonData.Substring(endPos).IndexOf("[");
                        //if (endPos2 == -1) //no other array
                        //{
                        //    endPos = jsonData.Substring(endPos).IndexOf("]");
                        //    ret = jsonData.Substring(1, endPos);
                        //}
                        //else
                        //{
                        //    int inBetween2 = jsonData.Substring(endPos, endPos2).IndexOf("]");
                        //    if (inBetween2 == -1)
                        //    {
                        //        endPos2 = endPos2 + jsonData.Substring(endPos2).IndexOf("]") + 1;
                        //        int endPos3 = jsonData.Substring(endPos2).IndexOf("[");
                        //        if (endPos3 == -1) //no other array
                        //        {
                        //            endPos = jsonData.Substring(endPos2).IndexOf("]");
                        //            ret = jsonData.Substring(1, endPos);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        endPos = endPos2 + jsonData.Substring(endPos2).IndexOf("}");
                        //        ret = jsonData.Substring(1, endPos);
                        //    }
                        //}
                    }
                    else //no inner array
                    {
                        endPos = jsonData.Substring(1).IndexOf("]");
                        ret = jsonData.Substring(1, endPos);
                    }
                }
            }
            else if (jsonData.First().Equals('{'))
            {
                endPos = jsonData.Substring(1).IndexOf("{");
                if (endPos == -1) //no other objects
                {
                    endPos = jsonData.Substring(1).IndexOf("}");
                    if (endPos > -1)
                        ret = jsonData.Substring(1, endPos);
                }
                else
                {
                    int inBetween = jsonData.Substring(1, endPos).IndexOf("}");
                    if (inBetween == -1)
                    {
                        //end of inner object
                        endPos = endPos + jsonData.Substring(endPos).IndexOf("}") + 1;
                        endPos = endPos + jsonData.Substring(endPos).IndexOf("}") - 1;
                        ret = jsonData.Substring(1, endPos);
                    }
                    else //no inner object
                    {
                        endPos = jsonData.Substring(1).IndexOf("}");
                        ret = jsonData.Substring(1, endPos);
                    }
                }
            }
            else
        {
                endPos = jsonData.IndexOf(",");
                if (endPos == -1)
                    endPos = jsonData.Length;
                ret = jsonData.Substring(0, endPos);
            }

            return ret;

        }

        private string GetFieldName(string jsonData)
        {
            int startPos = jsonData.IndexOf("\"") + 1;
            int endPos = jsonData.LastIndexOf("\"");

            return jsonData.Substring(startPos, endPos - startPos);
        }

        //public NLog.Logger getLogger()
        //{
        //    if(logger == null)
        //    {
        //        logger = NLog.LogManager.GetLogger(EntityName);
        //    }
        //    return logger;
        //}

        //public void logElements()
        //{
        //    this.Elements.ForEach(e =>
        //        this.getLogger().Info(String.Format("{0}.Elements ID[{1}] Name:{2} Data: '{3}' Bitwise:{4}", this.EntityName, e.ID, e.Name, e.Data, e.Bitwise))
        //    );

        //}

        //public void logMemento()
        //{
        //    this._memento.ForEach(m =>
        //        this.getLogger().Info(String.Format("{0}._memento ElementId: {1} Data: '{2}', ElmentName: {3}", m.ElementId, this.EntityName, m.Data, getElementForProxy(m).Name)
        //    ));
        //}

        private ElementBaseData getElementForProxy(DataProxy dataProxy)
        {
            ElementBaseData elementBaseData;
            elementBaseData = this.__Elements.Find(e => e.ID == dataProxy.ElementId);
            if (elementBaseData == null)
            {
                elementBaseData = new ElementBaseData();
            }
            return elementBaseData;
        }

        public void logMementoAndElements()
        {
            //logMemento();
            //logElements();
        }

        public string GetDisplayName(string FieldName)
        {
            return this.__Elements.Find(e => e.Name == FieldName).DisplayName ?? this.__Elements.Find(e => e.Name == FieldName).Name;
        }

        private void LoadElementFormData(System.Collections.Specialized.NameValueCollection nvc, ElementBaseData ebd)
        {
            if (nvc.AllKeys.Contains(ebd.Name))
            {
                if (this.GetType().GetProperty(ebd.Name).PropertyType.Name.Equals("Boolean"))
                {
                    if (nvc.Get(ebd.Name).StartsWith("true"))
                        ebd.Data = true;
                    else
                        ebd.Data = false;
                }
                else if (this.GetType().GetProperty(ebd.Name).PropertyType.Name.Equals("String"))
                {
                    ebd.Data = nvc.Get(ebd.Name).ToString().Trim().Trim('"');
                }
                else
                {
                    //e.Data = nvc.Get(e.Name);
                    this.GetType().GetProperty(ebd.Name).SetValue(
                        this, Convert.ChangeType(nvc.Get(ebd.Name) ?? ebd.Data, this.GetType().GetProperty(ebd.Name).PropertyType), null
                    );
                }

            }
        }

        public void LoadFormData(System.Collections.Specialized.NameValueCollection nvc)
        {
            if (!nvc.HasKeys()) return;
            __Elements.ForEach(e => LoadElementFormData(nvc, e));
        }

        public void LoadEntities()
        {
            foreach (ElementBaseData e in __Elements.FindAll(e => (e.IsEntity.Equals(true) && e.LazyLoad.Equals(false))))
            {
                EntityAbstract entity = (EntityAbstract)Activator.CreateInstance(e.Data.GetType());
                entity.ConnectionString = ConnectionString;
                entity.__Elements.Find(el => el.UseForMatching = true).Data = __Elements.Find(x => x.Name == e.ForeignKey).Data;
                entity.Load();
                e.Data = entity;
            }
        }

        public void LoadCollections()
        {
            foreach (ElementBaseData e in __Elements.FindAll(e2 => (e2.IsCollection.Equals(true) && e2.LazyLoad.Equals(false))))
            {
                Type EntityType = e.Data.GetType().GetGenericArguments().Single();
                if (EntityType != null)
                {
                    System.Reflection.MethodInfo method = EntityType.GetMethod(e.Name);
                    if (method != null)
                    {
                        method.Invoke(this, new object[] { this as EntityAbstract });
                    }
                }
            }
        }
        //public EntityException BuildEntityException()
        //{
        //    //int errorCount = Notifications.Count + Exceptions.Count;
        //    //if (errorCount == 0)
        //    //    return null;

        //    //EntityException entityException = new EntityException();


        //}

        public string XmlSerialize(string dateFormat, string timeFormat)
        {
            //string ret = String.Empty;

            XmlDocument xml = new XmlDocument();
            //XNamespace aw = "http://www.adventure-works.com";
            //CreateXmlRoot(xml, null, this, dateFormat, timeFormat);


            XmlElement root = xml.CreateElement(_entityName);/*xml.CreateElement("bo", _entityName, "http://ACORD.org/Standards/Life/2");
            root.SetAttribute("xmlns:bo", "http://ACORD.org/Standards/Life/2");
            root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            root.SetAttribute("xsi:schemaLocation", "http://ACORD.org/Standards/Life/2 TXLife2.10.00.XSD");*/
            _elements.ForEach(e => CreateXmlElement(xml, root, e, dateFormat, timeFormat));
            xml.AppendChild(root);

            return xml.OuterXml;
        }

        private void CreateXmlRoot(XmlDocument xml, XmlNode parent, IEntity entity, string dateFormat, string timeFormat)
        {
            //XmlNode root = xml.CreateElement(_entityName);
            XmlElement root = xml.CreateElement(entity.__EntityName);//xml.CreateElement("bo", entity.EntityName, "http://ACORD.org/Standards/Life/2");
            entity.__Elements.ForEach(e => CreateXmlElement(xml, root, e, dateFormat, timeFormat));

            parent.AppendChild(root);
        }

        private void CreateXmlElement(XmlDocument xml, XmlNode parent, ElementBaseData ebd, string dateFormat, string timeFormat)
        {
            if (ebd.IsEntity)
            {
                XmlElement node = xml.CreateElement(ebd.Name);//xml.CreateElement("bo", ebd.Name, "http://ACORD.org/Standards/Life/2");
                (ebd.Data as IEntity).__Elements.ForEach(e => CreateXmlElement(xml, node, e, dateFormat, timeFormat));
                parent.AppendChild(node);
            }
            else if (ebd.IsCollection)
            {
                (ebd.Data as IEnumerable<IEntity>).ToList().ForEach(e => CreateXmlRoot(xml, parent, e, dateFormat, timeFormat));
            }
            else
            {
                if (ebd.IsNodeAttribute)
                {
                    XmlAttribute node = xml.CreateAttribute(ebd.Name);
                    node.Value = ebd.Data.ToString();
                    parent.Attributes.Append(node);
                }
                else if (ebd.IsNodeValue)
                {
                    parent.InnerText = ebd.Data.ToString();
                }
                else
                {
                    XmlElement node = xml.CreateElement(ebd.Name);//xml.CreateElement("bo", ebd.Name, "http://ACORD.org/Standards/Life/2");
                    if (ebd.Data.GetType().Equals(typeof(System.DateTime)))
                    {
                        if (ebd.IsDateOnly)
                            node.InnerText = DateTime.Parse(ebd.Data.ToString()).ToString(dateFormat);
                        else if (ebd.IsTimeOnly)
                            node.InnerText = DateTime.Parse(ebd.Data.ToString()).ToString(timeFormat);
                        else
                            node.InnerText = DateTime.Parse(ebd.Data.ToString()).ToString(String.Format("{0} {1}", dateFormat, timeFormat));
                    }
                    else
                        node.InnerText = ebd.Data.ToString();
                    parent.AppendChild(node);
                }
            }
        }

    }
}

////////using ReadyEDI.EntityFactory.Elements;
////////using System;
////////using System.Collections.Generic;
////////using System.Data.SqlClient;
////////using System.Linq;
////////using System.Reflection;
////////using System.Runtime.Serialization;
////////using System.Text;
////////using System.Text.RegularExpressions;
////////using System.Threading.Tasks;
////////using System.Web.Script.Serialization;
////////using System.Xml;

////////namespace ReadyEDI.EntityFactory
////////{
////////    [Serializable]
////////    public abstract class EntityAbstract : IEntity, ICloneable
////////    {
////////        [NonSerialized]
////////        private string _entityName = String.Empty;
////////        [NonSerialized]
////////        private List<ElementBaseData> _elements = new List<ElementBaseData>();
////////        [NonSerialized]
////////        private long _action = 0;
////////        //private string _actionName = String.Empty;
////////        [NonSerialized]
////////        private List<Action> _actions = new List<Action>();
////////        private List<Notification> _notifications = new List<Notification>();
////////        [NonSerialized]
////////        private CRUDConflict _crudConflict = new CRUDConflict();
////////        [NonSerialized]
////////        private int _id = 0;    
////////        [NonSerialized]
////////        private DataProxyCollection _memento = new DataProxyCollection();
////////        private List<EntityException> _exceptions = new List<EntityException>();
////////        [NonSerialized]
////////        private string _connectionString = String.Empty;
////////        [NonSerialized]
////////        private Guid _queueId = Guid.Empty;
////////        [NonSerialized]
////////        private Dictionary<string, string> _paramerters = new Dictionary<string, string>();
////////        private string _DBName = "";
////////        private string _SchemaName = "";

////////        [NonSerialized]
////////        private SqlConnection _sqlConnection = null;
////////        [NonSerialized]
////////        private SqlTransaction _sqlTransaction = null;
////////        [NonSerialized]
////////        private long _lazyLoad = 0;


////////        //private bool _isDirty = false;
////////        public string DBName
////////        {
////////            get { return String.IsNullOrWhiteSpace(_DBName) ? "" : String.Concat("[", _DBName.Trim(), "]."); }
////////            set { _DBName = value; }
////////        }

////////        public string SchemaName
////////        {
////////            get { return String.IsNullOrWhiteSpace(_SchemaName) ? "dbo" : _SchemaName.Trim(); }
////////            set { _SchemaName = value; }
////////        }

////////        public SqlConnection SqlConnection
////////        {
////////            get { return _sqlConnection; }
////////            set { _sqlConnection = value; }
////////        }
////////        public SqlTransaction SqlTransaction
////////        {
////////            get { return _sqlTransaction; }
////////            set { _sqlTransaction = value; }
////////        }
////////        public string EntityName
////////        {
////////            get { return _entityName; }
////////            set { _entityName = value; }
////////        }

////////        public List<ElementBaseData> Elements
////////        {
////////            get { return _elements; }
////////            set { _elements = value; }
////////        }
////////        public long Action
////////        {
////////            get { return _action; }
////////            set { _action = value; }
////////        }

////////        public string GetActionName()
////////        {
////////            return _actions.Find(a => a.Bitwise == _action).Name;
////////            //return _actions[Convert.ToInt32(Math.Log(_action, 2))];
////////            //return Enum.GetNames(_actions)[Convert.ToInt32(Math.Log(_action, 2))];
////////            //return ((Enum)_actions).GetEnumerator().ToString();
////////            //_actions.GetType().Cast<T>().GetEnumerator();
////////            //Actions.GetName
////////            //Actions.GetType() as Action
////////        }

////////        public List<Action> Actions
////////        {
////////            get { return _actions; }
////////            set { _actions = value; }
////////        }

////////        //public string ActionName
////////        //{
////////        //    get { return _actionName; }
////////        //    set { _actionName = value; }
////////        //}

////////        public List<Notification> Notifications
////////        {
////////            get { return _notifications; }
////////            set { _notifications = value; }
////////        }

////////        public CRUDConflict CRUDConflict
////////        {
////////            get { return _crudConflict; }
////////            set { _crudConflict = value; }
////////        }

////////        public int Id
////////        {
////////            get { return _id; }
////////            set { _id = value; }
////////        }

////////        public string ConnectionString
////////        {
////////            get { return _connectionString; }
////////            set { _connectionString = value; }
////////        }

////////        public Guid QueueId
////////        {
////////            get { return _queueId; }
////////            set { _queueId = value; }
////////        }

////////        //public bool IsDirty
////////        //{
////////        //    get { return _isDirty; }
////////        //    set { _isDirty = value; }
////////        //}

////////        public DataProxyCollection Memento
////////        {
////////            get { return _memento; }
////////            set { _memento = value; }
////////        }

////////        public List<EntityException> Exceptions
////////        {
////////            get { return _exceptions; }
////////            set { _exceptions = value; }
////////        }

////////        public Dictionary<string, string> Parameters
////////        {
////////            get { return _paramerters; }
////////            set { _paramerters = value; }
////////        }

////////        public long LazyLoad
////////        {
////////            get { return _lazyLoad; }
////////            set { _lazyLoad = value; }
////////        }

////////        public virtual object Clone()
////////        {
////////            IEntity entity = this.MemberwiseClone() as IEntity;

////////            List<ElementBaseData> elements = new List<ElementBaseData>();
////////            _elements.ForEach(e => elements.Add(e.Clone() as ElementBaseData));
////////            entity.Elements = elements;

////////            return entity;
////////        }

////////        public virtual bool Validate()
////////        {
////////            Notifications.Where(n => n.Severity == Notification.NoticeType.Error).ToList().Clear();

////////            return Elements.FindAll(e => !Validate(e)).Count == 0;
////////        }

////////        private bool Validate(IBaseData elementBaseData)
////////        {
////////            //List<Notification> elementExceptions = elementBaseData.Validate();
////////            //elementExceptions.ForEach(ex => _notifications.Add(ex));

////////            //return elementExceptions.Count == 0;
////////            return true;
////////        }

////////        public virtual long Compare()
////////        {
////////            long ret = 0;

////////            _elements.Where(e => !e.IsCollection).ToList().ForEach(e => 
////////                ret += CompareFieldData(e, _memento.Find(t => t.ElementId == e.ID))
////////            );

////////            return ret;
////////        }

////////        public virtual long Compare(DataProxyCollection data)
////////        {
////////            long ret = 0;

////////            _elements.Where(e => !e.IsCollection).ToList().ForEach(e => ret += CompareFieldData(e, data.Find(t => t.ElementId == e.ID)));

////////            return ret;
////////        }

////////        public virtual long Compare(IEntity to)
////////        {
////////            long ret = 0;

////////            _elements.Where(e => !e.IsCollection).ToList().ForEach(e => ret += CompareFieldData(e, to.Elements.Find(t => t.ID == e.ID)));

////////            return ret;
////////        }

////////        private long CompareFieldData(IBaseData e1, DataProxy e2)
////////        {
////////            long ret = 0;

////////            if (e2 == null)
////////            {
////////                ret = e1.Bitwise ?? default(long);
////////                return ret;
////////            }
////////            if (!e1.Data.Equals(e2.Data))
////////                ret = e1.Bitwise ?? default(long);

////////            return ret;
////////        }

////////        private long CompareFieldData(IBaseData e1, IBaseData e2)
////////        {
////////            long ret = 0;

////////            if (!e1.Data.Equals(e2.Data))
////////                ret = e1.Bitwise ?? default(long);

////////            return ret;
////////        }

////////        public DataProxyCollection GetData()
////////        {
////////            DataProxyCollection data = new DataProxyCollection();

////////            data.Add(new DataProxy() { ElementId = -1, Data = _id });
////////            _elements.ForEach(e => GetData(data, e));// data.Add(new DataProxy() { ElementId = e.ID ?? default(int), Data = e.Data }));

////////            return data;
////////        }

////////        private void GetData(DataProxyCollection proxies, ElementBaseData element)
////////        {
////////            if (!element.IsCollection)//(!element.Data.GetType().Name.Equals("Collection`1"))
////////                proxies.Add(new DataProxy() { ElementId = element.ID ?? default(int), Data = element.Data });
////////        }

////////        public virtual void Load()
////////        {
////////            _memento = GetData();
////////        }

////////        public void Load(DataProxyCollection data)
////////        {
////////            DataProxy idProxy = data.Find(d => d.ElementId == -1);
////////            if (idProxy != null)
////////            {
////////                _id = int.Parse(idProxy.Data.ToString());
////////                data.Remove(idProxy);
////////            }
////////            data.ForEach(d => Elements[d.ElementId].Data = d.Data);

////////            _memento = GetData();
////////        }

////////        public void UpdateFieldValue(DataProxy proxy)
////////        {
////////            if (proxy.ElementId == -1)
////////                _id = int.Parse(proxy.Data.ToString());
////////            else
////////                _elements[proxy.ElementId].Data = proxy.Data;
////////        }

////////        public virtual void Save<T>() where T : IEntity
////////        {
////////            _memento = GetData();
////////        }

////////        public void RevertConflicts()
////////        {
////////            _crudConflict.Fields.ForEach(f => RevertField(f));
////////        }

////////        private void RevertField(int elementId)
////////        {
////////            _elements.Find(e => e.ID == elementId).Data = _memento.Find(m => m.ElementId == elementId);
////////        }

////////        public virtual void RefreshCollections()
////////        {

////////        }

////////        public List<string> GetMatchDataValues()
////////        {
////////            List<string> ret = new List<string>();
////////            _elements.Where(e => e.UseForMatching).ToList().ForEach(e => ret.Add(e.Data.ToString()));

////////            return ret;
////////        }

////////        public void Merge(IEntity entity)
////////        {
////////            Elements.FindAll(e => !e.IsCollection).ForEach(e => e.Data = entity.Elements.Find(p => p.ID == e.ID).Data);
////////        }

////////        public int AddField(ElementBaseData element)
////////        {
////////            int ret = Elements.Count;
////////            element.ID = ret;
////////            element.Bitwise = Convert.ToInt64(Math.Pow(2, ret));

////////            Elements.Add(element);
////////            _memento.Add(new DataProxy() { ElementId = ret, Data = element.Data });

////////            return ret;
////////        }

////////        //public long AddAction(string actionName, long inParams, long outParams)
////////        //{
////////        //    _actions.Add(new Action() { Name = actionName });

            

////////        //    return 0;
////////        //}

////////        public virtual void RegisterActions(Type actionEnum)
////////        {
////////            int initialCount = _actions.Count;
////////            string[] enumNames = Enum.GetNames(actionEnum);
////////            int endCount = initialCount + enumNames.Length;
            
////////            for (int i = initialCount; i < endCount; i++)
////////            {
////////                _actions.Add(new Action() { Name = enumNames[i - initialCount], Id = i + 1, Bitwise = Convert.ToInt64(Math.Pow(2, i)) });
////////            }
////////        }

////////        //public virtual bool Save<T>(bool bypassConflicts)
////////        //{
////////        //    CRUDFunctions.Save<Profile>(this, bypassConflicts);
////////        //    _memento = GetData();

////////        //    return true;
////////        //}

////////        public string JsonStringify()
////////        {
////////            List<string> FieldsAndValues = new List<string>();

////////            _elements.ForEach(e => FieldsAndValues.Add(JsonStringify(e)));

////////            //if (this.Notifications.Count == 0)
////////            //{
////////            //    FieldsAndValues.Add("\"Notifications\":[]");
////////            //}
////////            //else
////////            //{
////////            //    FieldsAndValues.Add(string.Format("\"{0}\":{1}", "\"Notifications\"", new JavaScriptSerializer().Serialize(this.Notifications)));
////////            //}

////////            //if (this.Exceptions.Count == 0)
////////            //{
////////            //    FieldsAndValues.Add("\"Exceptions\":[]");
////////            //}
////////            //else
////////            //{
////////            //    FieldsAndValues.Add(string.Format("\"{0}\":{1}", "\"Exceptions\"", new JavaScriptSerializer().Serialize(this.Exceptions)));
////////            //}

////////            return String.Format("{{ {0} }}", String.Join(", ", FieldsAndValues.Where(x => !string.IsNullOrEmpty(x))));
////////        }

////////        private string JsonStringify(ElementBaseData element)
////////        {
////////            string ret = String.Empty;
////////            float f;

////////            if (element is IEntity)
////////            {
////////                ret = element.Data != null ? String.Format("\"{0}\": \"{1}\"", element.Name, (element.Data as IEntity).JsonStringify()) : "";
////////            }
////////            else if (element.IsCollection)
////////            {
////////                if (element.Data != null)
////////                {
////////                    if (element.Data.ToString() == "")
////////                    {
////////                        ret = String.Format("\"{0}\":[]", element.Name);
////////                    }
////////                    else
////////                    {
////////                        if ((element.Data as IEnumerable<IEntity>) != null)
////////                        {
////////                (element.Data as IEnumerable<IEntity>).ToList().ForEach(
////////                    e => ret += e.JsonStringify()
////////                );
////////                            ret = String.Format("\"{0}\":\"{1}\"", element.Name, ret);
////////                        }
////////                    }
////////                }
////////                else
////////                {
////////                    ret = "";
////////                }
////////            }
////////            else if (element.Data != null && float.TryParse(element.Data.ToString(), out f))
////////            {
////////                ret = element.Data != null ? String.Format("\"{0}\":{1}", element.Name, element.Data.ToString()) : "";
////////            }
////////            else if (element.Data != null && (element.Data.ToString().ToLower() == "true" || element.Data.ToString().ToLower() == "false"))
////////            {
////////                ret = element.Data != null ? String.Format("\"{0}\":{1}", element.Name, element.Data.ToString().ToLower()) : "";
////////            }
////////            else if (element.IsDateTime)
////////            {
////////                //ret = element.Data != null ? String.Format("\"{0}\": \"\\/Date({1})\\/\"", element.Name, (Convert.ToDateTime(element.Data).Ticks / (long)10000).ToString()) : "";
////////                ret = element.Data != null ? String.Format("\"{0}\":{1}", element.Name, (new JavaScriptSerializer().Serialize(Convert.ToDateTime(element.Data).ToLocalTime()))) : "";
////////            }
////////            else
////////            {
////////                ret = element.Data != null ? String.Format("\"{0}\":\"{1}\"", element.Name, element.Data.ToString()) : "";
////////            }

////////            return ret;
////////        }

////////        //private string JsonStringify(ElementBaseData element)
////////        //{
////////        //    string ret = String.Empty;
////////        //    float f;

////////        //    if (element is IEntity)
////////        //    {
////////        //        ret = element.Data != null ? String.Format("\"{0}\": \"{1}\"", element.Name, (element.Data as IEntity).JsonStringify()) : "";
////////        //    }
////////        //    else if (element.IsCollection)
////////        //    {
////////        //        if (element.Data != null)
////////        //        {
////////        //            if (element.Data.ToString() == "")
////////        //            {
////////        //                ret = String.Format("\"{0}\":[]", element.Name);
////////        //            }
////////        //            else
////////        //            {
////////        //                (element.Data as IEnumerable<IEntity>).ToList().ForEach(
////////        //                    e => ret += e.JsonStringify()
////////        //                );
////////        //                ret = String.Format("\"{0}\":\"{1}\"", element.Name, ret);
////////        //            }
////////        //        }
////////        //        else
////////        //        {
////////        //            ret = "";
////////        //        }
////////        //    }
////////        //    else if (element.Data != null && float.TryParse(element.Data.ToString(), out f))
////////        //    {
////////        //        ret = element.Data != null ? String.Format("\"{0}\":{1}", element.Name, element.Data.ToString()) : "";
////////        //    }
////////        //    else if (element.Data != null && (element.Data.ToString().ToLower() =="true" || element.Data.ToString().ToLower() =="false")) 
////////        //    {
////////        //        ret = element.Data != null ? String.Format("\"{0}\":{1}", element.Name, element.Data.ToString().ToLower()) : "";
////////        //    }
////////        //    else if (element.IsDateTime)
////////        //    {
////////        //        //ret = element.Data != null ? String.Format("\"{0}\": \"\\/Date({1})\\/\"", element.Name, (Convert.ToDateTime(element.Data).Ticks / (long)10000).ToString()) : "";
////////        //        ret = element.Data != null ? String.Format("\"{0}\":{1}", element.Name, (new JavaScriptSerializer().Serialize(Convert.ToDateTime(element.Data).ToLocalTime()))) : "";
////////        //    }
////////        //    else
////////        //    {
////////        //        ret = element.Data != null ? String.Format("\"{0}\":\"{1}\"", element.Name, element.Data.ToString()) : "";
////////        //    }

////////        //    return ret;
////////        //}

////////        public void LoadFromJson2(string jsonData)
////////        {
////////            List<string> nvps = Regex.Split(jsonData, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)").ToList();

////////            List<ElementBaseData> ebdChildren = new List<ElementBaseData>();
////////            List<IEntity> children = new List<IEntity>();

////////            bool arrayStarted = false;
////////            bool arrayEnded = false;
////////            List<string> ebdArray = null;

////////            int counter = 0;

////////            foreach (string nvp in nvps)
////////            {
////////                int childIndex = -1;
////////                counter++;

////////                string[] nvpArr = Regex.Split(nvp, ":(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
////////                string fieldName = nvpArr[0].Replace("{", String.Empty).Replace("\"", String.Empty);

////////                int endFancyCount = nvpArr[1].Count(c => c.Equals('}'));
////////                if (nvpArr[1].Replace(" ", String.Empty).StartsWith("{}"))
////////                    endFancyCount--;
////////                if (counter == nvps.Count)
////////                    endFancyCount--;

////////                arrayEnded = endFancyCount > 0 && nvpArr[1].EndsWith("]");
////////                if (arrayEnded)
////////                    nvpArr[1] = nvpArr[1].Substring(0, nvpArr[1].Length - 1);
////////                nvpArr[1] = nvpArr[1].Replace("}", String.Empty);

////////                ElementBaseData ebd = null;
////////                if (childIndex == -1)
////////                    ebd = Elements.Find(e => e.Name.Equals(fieldName));
////////                else
////////                {
////////                    IEntity entity = children.Last();
////////                    if (entity != null)
////////                        ebd = children.Last().Elements.Find(e => e.Name.Equals(fieldName));
////////                }

////////                if (ebd == null)
////////                {
////////                    if (nvpArr[1].StartsWith("[") && !nvpArr[1].Replace(" ", String.Empty).Equals("[]"))
////////                    {
////////                        if (ebdChildren.Last() != null)
////////                        {
////////                            childIndex++;
////////                            ebdChildren.Add(null);
////////                            children.Add(null);
////////                        }
////////                    }
////////                }
////////                else
////////                {
////////                    if (nvp != nvps.First() && nvp.StartsWith("{"))
////////                    {
////////                        Type gType = ebdChildren.Last().Data.GetType().GetGenericArguments().First();

////////                        IEntity entity = Activator.CreateInstance(gType) as IEntity; //next entity
////////                        ElementBaseData ebdEntity = entity.Elements.Find(e => e.Name.Equals(fieldName)); //first ebd in next entity

////////                        if (ebdEntity != null)
////////                        {

////////                            ebdEntity.Data = nvpArr[1].Replace("}", String.Empty).Replace("]", String.Empty).Replace("\"", String.Empty); //set value of first ebd
////////                        }

////////                        children.RemoveAt(children.Count - 1);
////////                        children.Add(entity);
////////                    }
////////                    else
////////                    {
////////                        if (ebd.IsCollection)
////////                        {
////////                            if (!nvpArr[1].Replace(" ", String.Empty).Equals("[]"))
////////                            {
////////                                Type gType = ebd.Data.GetType().GetGenericArguments().First();

////////                                string childFieldName = nvpArr[1].Replace("[", String.Empty).Replace("{", String.Empty).Replace("\"", String.Empty);

////////                                IEntity entity = Activator.CreateInstance(gType) as IEntity; //first entity
////////                                ElementBaseData ebdEntity = entity.Elements.Find(e => e.Name.Equals(childFieldName)); //first ebd in first entity

////////                                if (ebdEntity != null)
////////                                {

////////                                    ebdEntity.Data = nvpArr[2].Replace("}", String.Empty).Replace("]", String.Empty).Replace("\"", String.Empty); //set value of first ebd
////////                                }

////////                                childIndex++;
////////                                ebdChildren.Add(ebd);
////////                                children.Add(entity);

////////                            }

////////                        }
////////                        else if (ebd.IsEntity)
////////                        {
////////                            if (!nvpArr[1].Replace(" ", String.Empty).Equals("{}"))
////////                            {
////////                                Type gType = ebd.Data.GetType();

////////                                string childFieldName = nvpArr[1].Replace("{", String.Empty).Replace("\"", String.Empty);

////////                                IEntity entity = Activator.CreateInstance(gType) as IEntity;
////////                                ElementBaseData ebdEntity = entity.Elements.Find(e => e.Name.Equals(childFieldName));

////////                                if (ebdEntity != null)
////////                                    ebdEntity.Data = nvpArr[2].Replace("}", String.Empty).Replace("\"", String.Empty);

////////                                if (nvpArr[2].Contains("}"))
////////                                {
////////                                    ebd.Data = entity;
////////                                }
////////                                else
////////                                {
////////                                    childIndex++;
////////                                    ebdChildren.Add(ebd);
////////                                    children.Add(entity);
////////                                }
////////                            }
////////                        }
////////                        else if (ebd.IsDateTime)
////////                        {
////////                            if (nvpArr[1].Equals("null"))
////////                                ebd.Data = null;
////////                            else if (string.IsNullOrWhiteSpace(nvpArr[1]))
////////                            {
////////                                // do nothing here -- we'll pick up the default value
////////                            }
////////                            else
////////                            {
////////                                // the number of .net ticks at the unix epoch
////////                                const long epochTicks = 621355968000000000;
////////                                // there are 10000 .net ticks per millisecond
////////                                const long ticksPerMillisecond = 10000;
////////                                // calculate the total number of .net ticks for your date
////////                                var millisecondsSince1970 = long.Parse(nvpArr[1].Split("()-+".ToCharArray())[1]);
////////                                var ticks = epochTicks + (millisecondsSince1970 * ticksPerMillisecond);
////////                                ebd.Data = new DateTime(ticks);
////////                            }
////////                        }
////////                        else
////////                        {
////////                            if (nvpArr[1].Equals("null"))
////////                                ebd.Data = null;
////////                            else if (nvpArr[1].StartsWith("["))
////////                            {
////////                                if (!nvpArr[1].Replace(" ", String.Empty).Equals("[]"))
////////                                {
////////                                    if (nvpArr[1].Contains("]"))
////////                                    {
////////                                        (ebd.Data as List<string>).Add(nvpArr[1].Replace("[", String.Empty).Replace("\"", String.Empty).Replace("]", String.Empty));
////////                                    }
////////                                    else
////////                                    {
////////                                        arrayStarted = true;
////////                                        ebdArray = ebd.Data as List<string>;
////////                                    }
////////                                }
////////                            }
////////                            else
////////                                ebd.Data = nvpArr[1].Replace("\"", String.Empty);


////////                        }
////////                    }
////////                }

////////                for (int i = 0; i < endFancyCount; i++)
////////                {
////////                    if (ebdChildren.Count == 0)
////////                        continue;

////////                    ElementBaseData ebdLast = ebdChildren.Last();
////////                    if (ebdLast == null)
////////                    {
////////                        ebdChildren.RemoveAt(ebdChildren.Count - 1);
////////                        children.RemoveAt(children.Count - 1);
////////                        childIndex--;
////////                    }
////////                    else
////////                    {
////////                        IEntity entityLast = children.Last();
////////                        if (ebdLast.IsCollection)
////////                        {
////////                            MethodInfo method = ebdLast.Data.GetType().GetMethod("Add");
////////                            method.Invoke(ebdLast.Data, new object[] { entityLast });

////////                            if (arrayEnded)
////////                            {
////////                                ebdChildren.Remove(ebdLast);
////////                                children.Remove(entityLast);
////////                                childIndex--;
////////                            }

////////                        }
////////                        else
////////                        {
////////                            ebdLast.Data = entityLast;
////////                            ebdChildren.Remove(ebdLast);
////////                            children.Remove(entityLast);
////////                            childIndex--;
////////                        }
////////                    }
////////                }
////////            }
////////        }

////////        public void LoadFromJson(string jsonData)
////////        {
////////            int startQualifier = jsonData.IndexOf("\"") + 1;
////////            int endQualifier = startQualifier + jsonData.Substring(startQualifier).IndexOf("\"") + 1;
////////            int startPos = endQualifier + jsonData.Substring(endQualifier).IndexOf(":");

////////            while (startPos > -1)
////////            {
////////                string fieldName = GetFieldName(jsonData.Substring(0, startPos));

////////                if (fieldName.Equals("refunds") && !jsonData.Substring(12, 2).Equals("[]"))
////////                {
////////                    int x = 1;
////////                }

////////                int endPos;
////////                bool isArray;
////////                string fieldValue = GetFieldValue(jsonData.Substring(startPos + 1).Trim(), out endPos, out isArray);

////////                ElementBaseData ebd = Elements.Find(e => e.Name.Equals(fieldName));
////////                if (ebd != null)
////////                {
////////                    if (ebd.IsCollection)
////////                    {
////////                        Type type = this.GetType();
////////                        PropertyInfo prop = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
////////                        Type gType = prop.PropertyType.GetGenericArguments().First();
////////                        List<string> fieldValueList = GetFieldValueList(fieldValue);
////////                        fieldValueList.ForEach(v => AddNewEntity(gType, ebd, v));
////////                    }
////////                    else if (ebd.IsEntity)
////////                    {
////////                        Type type = this.GetType();
////////                        PropertyInfo prop = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
////////                        Type gType = prop.PropertyType;
////////                        CreateNewEntity(gType, ebd, fieldValue);
////////                    }
////////                    else
////////                    {
////////                        if (fieldValue.Equals("null"))
////////                            ebd.Data = null;
////////                        else if (isArray)
////////                            ebd.Data = new string[] { fieldValue.Replace("\"", String.Empty) }.ToList();
////////                        else
////////                            ebd.Data = fieldValue.Replace("\"", String.Empty);
////////                    }
////////                };

////////                int i = endPos + startPos + 2;
////////                if (i > jsonData.Length)
////////                    i = jsonData.Length;
////////                jsonData = jsonData.Substring(i);
////////                startQualifier = jsonData.IndexOf("\"") + 1;
////////                endQualifier = startQualifier + jsonData.Substring(startQualifier).IndexOf("\"") + 1;
////////                startPos = endQualifier + jsonData.Substring(endQualifier).IndexOf(":");
////////            }
////////        }

////////        private List<string> GetFieldValueList(string fieldValue)
////////        {
////////            List<string> ret = new List<string>();

////////            if (fieldValue.Equals(String.Empty))
////////                return ret;

////////            List<string> start = Regex.Split(fieldValue, "},").ToList();//fieldValue.Split(new char[] {'}', ','}).ToList();
////////            //start.Remove(start.Last());
////////            //start.ForEach(s => ret.Add(RemoveFancy(s)));
////////            string str = String.Empty;
////////            foreach (string s in start)
////////            {
////////                if (s.StartsWith("{"))
////////                {
////////                    if (!str.Equals(String.Empty))
////////                        ret.Add(str);

////////                    str = s;
////////            }
////////            else
////////            {
////////                    str += string.Format("}}{0}", s);
////////                }
////////            }

////////            if (!str.Equals(String.Empty))
////////                ret.Add(str);

////////            List<string> ret2 = new List<string>();
////////            ret.ForEach(s => ret2.Add(s.Substring(s.IndexOf("{") + 1, s.Length - 1)));

////////            return ret2;
////////            }

////////        private string RemoveFancy(string s)
////////        {
////////            string ret = String.Empty;

////////            int startPos = s.IndexOf("{") + 1;
////////            ret = s.Substring(startPos);

////////            return ret;
////////        }

////////        private void AddNewEntity(Type gType, ElementBaseData ebd, string fieldValue)
////////        {
////////            object o = Activator.CreateInstance(gType);
////////            (o as IEntity).LoadFromJson(fieldValue);

////////            MethodInfo method = ebd.Data.GetType().GetMethod("Add");
////////            method.Invoke(ebd.Data, new object[] { o });
////////        }

////////        private void CreateNewEntity(Type gType, ElementBaseData ebd, string fieldValue)
////////        {
////////            object o = Activator.CreateInstance(gType);
////////            (o as IEntity).LoadFromJson(fieldValue);

////////            ebd.Data = o;
////////        }

////////        private string GetFieldValue(string jsonData, out int endPos, out bool isArray)
////////        {
////////            string ret = String.Empty;
////////            endPos = 0;
////////            isArray = false;

////////            if (jsonData.First().Equals('['))
////////            {
////////                isArray = true;
////////                endPos = jsonData.Substring(1).IndexOf("[");
////////                if (endPos == -1) //no other array
////////                {
////////                    endPos = jsonData.Substring(1).IndexOf("]");
////////                    if (endPos == -1)
////////                    {
////////                        ret = jsonData.Substring(1) + "}";
////////                        endPos = ret.Length;
////////                    }
////////                    else
////////                    {
////////                        ret = jsonData.Substring(1, endPos);
////////                    }
////////                }
////////                else
////////                {
////////                    int inBetween = jsonData.Substring(1, endPos).IndexOf("]");
////////                    if (inBetween == -1)
////////                    {
////////                        //end of inner array
////////                        endPos = endPos + jsonData.Substring(endPos).IndexOf("]") + 1;

////////                        if (jsonData.Substring(endPos, 1) != "}")
////////        {
////////                            while (jsonData.Substring(endPos + 1, 1) != "]")
////////            {
////////                                endPos = endPos + jsonData.Substring(endPos + 1).IndexOf("[");
////////                                endPos = endPos + jsonData.Substring(endPos).IndexOf("]") + 1;
////////            }

////////                            //endPos = endPos + jsonData.Substring(endPos + 1).IndexOf("[");
////////                            //endPos = endPos + jsonData.Substring(endPos).IndexOf("]") + 1;

////////                            //endPos = endPos + jsonData.Substring(endPos).IndexOf("]") + 1;
////////        }


////////                        //endPos = jsonData.IndexOf("}");
////////                        ret = jsonData.Substring(1, endPos);
////////                        endPos++;
////////                        //endPos = endPos + jsonData.Substring(endPos).IndexOf("]") + 1;
////////                        ////endPos = endPos + jsonData.Substring(endPos).IndexOf("[");

////////                        ////endPos = endPos + jsonData.Substring(endPos).IndexOf("]");
////////                        ////endPos = endPos + jsonData.Substring(endPos).IndexOf("[");

////////                        ////endPos = endPos + jsonData.Substring(endPos).IndexOf("]");
////////                        ////endPos = endPos + 1;
////////                        //int endPos2 = endPos + jsonData.Substring(endPos).IndexOf("[");
////////                        //if (endPos2 == -1) //no other array
////////                        //{
////////                        //    endPos = jsonData.Substring(endPos).IndexOf("]");
////////                        //    ret = jsonData.Substring(1, endPos);
////////                        //}
////////                        //else
////////                        //{
////////                        //    int inBetween2 = jsonData.Substring(endPos, endPos2).IndexOf("]");
////////                        //    if (inBetween2 == -1)
////////                        //    {
////////                        //        endPos2 = endPos2 + jsonData.Substring(endPos2).IndexOf("]") + 1;
////////                        //        int endPos3 = jsonData.Substring(endPos2).IndexOf("[");
////////                        //        if (endPos3 == -1) //no other array
////////                        //        {
////////                        //            endPos = jsonData.Substring(endPos2).IndexOf("]");
////////                        //            ret = jsonData.Substring(1, endPos);
////////                        //        }
////////                        //    }
////////                        //    else
////////                        //    {
////////                        //        endPos = endPos2 + jsonData.Substring(endPos2).IndexOf("}");
////////                        //        ret = jsonData.Substring(1, endPos);
////////                        //    }
////////                        //}
////////                    }
////////                    else //no inner array
////////                    {
////////                        endPos = jsonData.Substring(1).IndexOf("]");
////////                        ret = jsonData.Substring(1, endPos);
////////                    }
////////                }
////////            }
////////            else if (jsonData.First().Equals('{'))
////////            {
////////                endPos = jsonData.Substring(1).IndexOf("{");
////////                if (endPos == -1) //no other objects
////////                {
////////                    endPos = jsonData.Substring(1).IndexOf("}");
////////                    if (endPos > -1)
////////                        ret = jsonData.Substring(1, endPos);
////////                }
////////                else
////////                {
////////                    int inBetween = jsonData.Substring(1, endPos).IndexOf("}");
////////                    if (inBetween == -1)
////////                    {
////////                        //end of inner object
////////                        endPos = endPos + jsonData.Substring(endPos).IndexOf("}") + 1;
////////                        endPos = endPos + jsonData.Substring(endPos).IndexOf("}") - 1;
////////                        ret = jsonData.Substring(1, endPos);
////////                    }
////////                    else //no inner object
////////                    {
////////                        endPos = jsonData.Substring(1).IndexOf("}");
////////                        ret = jsonData.Substring(1, endPos);
////////                    }
////////                }
////////            }
////////            else
////////        {
////////                endPos = jsonData.IndexOf(",");
////////                if (endPos == -1)
////////                    endPos = jsonData.Length;
////////                ret = jsonData.Substring(0, endPos);
////////            }

////////            return ret;

////////        }

////////        private string GetFieldName(string jsonData)
////////        {
////////            int startPos = jsonData.IndexOf("\"") + 1;
////////            int endPos = jsonData.LastIndexOf("\"");

////////            return jsonData.Substring(startPos, endPos - startPos);
////////        }

////////        //public NLog.Logger getLogger()
////////        //{
////////        //    if(logger == null)
////////        //    {
////////        //        logger = NLog.LogManager.GetLogger(EntityName);
////////        //    }
////////        //    return logger;
////////        //}

////////        //public void logElements()
////////        //{
////////        //    this.Elements.ForEach(e =>
////////        //        this.getLogger().Info(String.Format("{0}.Elements ID[{1}] Name:{2} Data: '{3}' Bitwise:{4}", this.EntityName, e.ID, e.Name, e.Data, e.Bitwise))
////////        //    );

////////        //}

////////        //public void logMemento()
////////        //{
////////        //    this._memento.ForEach(m =>
////////        //        this.getLogger().Info(String.Format("{0}._memento ElementId: {1} Data: '{2}', ElmentName: {3}", m.ElementId, this.EntityName, m.Data, getElementForProxy(m).Name)
////////        //    ));
////////        //}

////////        private ElementBaseData getElementForProxy(DataProxy dataProxy)
////////        {
////////            ElementBaseData elementBaseData;
////////            elementBaseData = this.Elements.Find(e => e.ID == dataProxy.ElementId);
////////            if (elementBaseData == null)
////////            {
////////                elementBaseData = new ElementBaseData();
////////            }
////////            return elementBaseData;
////////        }

////////        public void logMementoAndElements()
////////        {
////////            //logMemento();
////////            //logElements();
////////        }

////////        public string GetDisplayName(string FieldName)
////////        {
////////            return this.Elements.Find(e => e.Name == FieldName).DisplayName ?? this.Elements.Find(e => e.Name == FieldName).Name;
////////        }

////////        public void LoadFormData(System.Collections.Specialized.NameValueCollection nvc)
////////        {
////////            if (nvc.Keys.Count == 0) return;
////////            Elements.ForEach(e =>
////////                    this.GetType().GetProperty(e.Name).SetValue(this, Convert.ChangeType(nvc.Get(e.Name) ?? e.Data, this.GetType().GetProperty(e.Name).PropertyType), null)
////////                        );
////////                    }
////////        //public EntityException BuildEntityException()
////////        //{
////////        //    //int errorCount = Notifications.Count + Exceptions.Count;
////////        //    //if (errorCount == 0)
////////        //    //    return null;

////////        //    //EntityException entityException = new EntityException();


////////        //}

////////        public string XmlSerialize(string dateFormat, string timeFormat)
////////        {
////////            //string ret = String.Empty;

////////            XmlDocument xml = new XmlDocument();
////////            //XNamespace aw = "http://www.adventure-works.com";
////////            //CreateXmlRoot(xml, null, this, dateFormat, timeFormat);
////////            XmlElement root = xml.CreateElement("bo", _entityName, "http://ACORD.org/Standards/Life/2");
////////            root.SetAttribute("xmlns:bo", "http://ACORD.org/Standards/Life/2");
////////            root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
////////            root.SetAttribute("xsi:schemaLocation", "http://ACORD.org/Standards/Life/2 TXLife2.10.00.XSD");
////////            _elements.ForEach(e => CreateXmlElement(xml, root, e, dateFormat, timeFormat));
////////            xml.AppendChild(root);

////////            return xml.OuterXml;
////////                }

////////        private void CreateXmlRoot(XmlDocument xml, XmlNode parent, IEntity entity, string dateFormat, string timeFormat)
////////        {
////////            //XmlNode root = xml.CreateElement(_entityName);
////////            XmlElement root = xml.CreateElement("bo", entity.EntityName, "http://ACORD.org/Standards/Life/2");
////////            entity.Elements.ForEach(e => CreateXmlElement(xml, root, e, dateFormat, timeFormat));

////////            parent.AppendChild(root);
////////        }

////////        private void CreateXmlElement(XmlDocument xml, XmlNode parent, ElementBaseData ebd, string dateFormat, string timeFormat)
////////        {
////////            if (ebd.IsEntity)
////////            {
////////                XmlElement node = xml.CreateElement("bo", ebd.Name, "http://ACORD.org/Standards/Life/2");
////////                (ebd.Data as IEntity).Elements.ForEach(e => CreateXmlElement(xml, node, e, dateFormat, timeFormat));
////////                parent.AppendChild(node);
////////            }
////////            else if (ebd.IsCollection)
////////            {
////////                (ebd.Data as IEnumerable<IEntity>).ToList().ForEach(e => CreateXmlRoot(xml, parent, e, dateFormat, timeFormat));
////////        }
////////            else
////////            {
////////                if (ebd.IsNodeAttribute)
////////        {
////////                    XmlAttribute node = xml.CreateAttribute(ebd.Name);
////////                    node.Value = ebd.Data.ToString();
////////                    parent.Attributes.Append(node);
////////                }
////////                else if (ebd.IsNodeValue)
////////            {
////////                    parent.InnerText = ebd.Data.ToString();
////////                }
////////                else
////////                {
////////                    XmlElement node = xml.CreateElement("bo", ebd.Name, "http://ACORD.org/Standards/Life/2");
////////                    if (ebd.Data.GetType().Equals(typeof(System.DateTime)))
////////                    {
////////                        if (ebd.IsDateOnly)
////////                            node.InnerText = DateTime.Parse(ebd.Data.ToString()).ToString(dateFormat);
////////                        else if (ebd.IsTimeOnly)
////////                            node.InnerText = DateTime.Parse(ebd.Data.ToString()).ToString(timeFormat);
////////                        else
////////                            node.InnerText = DateTime.Parse(ebd.Data.ToString()).ToString(String.Format("{0} {1}", dateFormat, timeFormat));
////////                    }
////////                    else
////////                        node.InnerText = ebd.Data.ToString();
////////                    parent.AppendChild(node);
////////                }
////////            }
////////        }

////////    }
////////}
