using System;
using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public partial class Field : Base
	{
		#region "Enumerators"

		private Dictionary<string, int> FieldFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public Field()
			: base()
		{
			Initialize();
		}

		public Field(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Field(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Field";

			ElementBaseData ebdDisplayName = new ElementBaseData(FieldElementIndex.DisplayName)
			{
				Data = String.Empty
			};
			ElementBaseData ebdRules = new ElementBaseData(FieldElementIndex.Rules)
			{
				Data = new List<Rule>()
			};
			ElementBaseData ebdDataType = new ElementBaseData(FieldElementIndex.DataType)
			{
				Data = new DataType()
			};
			ElementBaseData ebdAttributes = new ElementBaseData(FieldElementIndex.Attributes)
			{
				Data = new List<Attribute>()
			};
			ElementBaseData ebdDefaultValue = new ElementBaseData(FieldElementIndex.DefaultValue)
			{
				Data = String.Empty
			};
			ElementBaseData ebdFieldType = new ElementBaseData(FieldElementIndex.FieldType)
			{
				Data = new FieldType()
			};
			ElementBaseData ebdIsNodeAttribute = new ElementBaseData(FieldElementIndex.IsNodeAttribute)
			{
				Data = false
			};
			ElementBaseData ebdIsNodeValue = new ElementBaseData(FieldElementIndex.IsNodeValue)
			{
				Data = false
			};
			ElementBaseData ebdIsDateOnly = new ElementBaseData(FieldElementIndex.IsDateOnly)
			{
				Data = false
			};
			ElementBaseData ebdIsTimeOnly = new ElementBaseData(FieldElementIndex.IsTimeOnly)
			{
				Data = false
			};
			ElementBaseData ebdUseForMatching = new ElementBaseData(FieldElementIndex.UseForMatching)
			{
				Data = false
			};
			ElementBaseData ebdIsEntity = new ElementBaseData(FieldElementIndex.IsEntity)
			{
				Data = false
			};
            ElementBaseData ebdDataChangeNotification = new ElementBaseData(FieldElementIndex.DataChangeNotification)
            {
                Data = false
            };
            ElementBaseData ebdDependant = new ElementBaseData(FieldElementIndex.Dependant)
            {
                Data = false
            };
            ElementBaseData ebdHideOnAdd = new ElementBaseData(FieldElementIndex.HideOnAdd)
            {
                Data = false
            };
            ElementBaseData ebdHideOnEdit = new ElementBaseData(FieldElementIndex.HideOnEdit)
            {
                Data = false
            };
            ElementBaseData ebdHideOnSummary = new ElementBaseData(FieldElementIndex.HideOnSummary)
            {
                Data = false
            };
            ElementBaseData ebdRedact = new ElementBaseData(FieldElementIndex.Redact)
            {
                Data = false
            };
            ElementBaseData ebdConfirm = new ElementBaseData(FieldElementIndex.Confirm)
            {
                Data = false
            };
            ElementBaseData ebdFieldPresentations = new ElementBaseData(FieldElementIndex.FieldPresentations)
            {
                Data = new List<FieldPresentation>()
            };

			FieldFields.Add("DisplayName", AddField(ebdDisplayName));
			FieldFields.Add("Rules", AddField(ebdRules));
			FieldFields.Add("DataType", AddField(ebdDataType));
			FieldFields.Add("Attributes", AddField(ebdAttributes));
			FieldFields.Add("DefaultValue", AddField(ebdDefaultValue));
			FieldFields.Add("FieldType", AddField(ebdFieldType));
			FieldFields.Add("IsNodeAttribute", AddField(ebdIsNodeAttribute));
			FieldFields.Add("IsNodeValue", AddField(ebdIsNodeValue));
			FieldFields.Add("IsDateOnly", AddField(ebdIsDateOnly));
			FieldFields.Add("IsTimeOnly", AddField(ebdIsTimeOnly));
			FieldFields.Add("UseForMatching", AddField(ebdUseForMatching));
			FieldFields.Add("IsEntity", AddField(ebdIsEntity));
            FieldFields.Add("DataChangeNotification", AddField(ebdDataChangeNotification));
            FieldFields.Add("Dependant", AddField(ebdDependant));
            FieldFields.Add("HideOnAdd", AddField(ebdHideOnAdd));
            FieldFields.Add("HideOnEdit", AddField(ebdHideOnEdit));
            FieldFields.Add("HideOnSummary", AddField(ebdHideOnSummary));
            FieldFields.Add("Redact", AddField(ebdRedact));
            FieldFields.Add("Confirm", AddField(ebdConfirm));
            FieldFields.Add("FieldPresentations", AddField(ebdFieldPresentations));

			AttachedCollections.Add(ebdRules);
			AttachedCollections.Add(ebdDataType);
			AttachedCollections.Add(ebdAttributes);
			AttachedCollections.Add(ebdFieldType);
            AttachedCollections.Add(ebdFieldPresentations);
		}

		#endregion

		#region "Properties"

		[DataMember]
        [Category("Presentation")]
		public string DisplayName
		{
			get { return __Elements[(int)FieldFields["DisplayName"]].Data.ToString(); }
			set { __Elements[(int)FieldFields["DisplayName"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public List<Rule> Rules
		{
			get { return __Elements[(int)FieldFields["Rules"]].Data as List<Rule>; }
			set { __Elements[(int)FieldFields["Rules"]].Data = value; }
		}
		[DataMember]
        [Category("Typing")]
		public DataType DataType
		{
			get { return __Elements[(int)FieldFields["DataType"]].Data as DataType; }
			set { __Elements[(int)FieldFields["DataType"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public List<Attribute> Attributes
		{
			get { return __Elements[(int)FieldFields["Attributes"]].Data as List<Attribute>; }
			set { __Elements[(int)FieldFields["Attributes"]].Data = value; }
		}
		[DataMember]
        [Category("Presentation")]
		public string DefaultValue
		{
			get { return __Elements[(int)FieldFields["DefaultValue"]].Data.ToString(); }
			set { __Elements[(int)FieldFields["DefaultValue"]].Data = value; }
		}
		[DataMember]
        [Category("Typing")]
		public FieldType FieldType
		{
			get { return __Elements[(int)FieldFields["FieldType"]].Data as FieldType; }
			set { __Elements[(int)FieldFields["FieldType"]].Data = value; }
		}
		[DataMember]
        [Category("Serializing")]
		public bool IsNodeAttribute
		{
			get
			{
				if (__Elements[(int)FieldFields["IsNodeAttribute"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["IsNodeAttribute"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)FieldFields["IsNodeAttribute"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)FieldFields["IsNodeAttribute"]].Data.ToString());
			}
			set { __Elements[(int)FieldFields["IsNodeAttribute"]].Data = value; }
		}
		[DataMember]
        [Category("Serializing")]
		public bool IsNodeValue
		{
			get
			{
				if (__Elements[(int)FieldFields["IsNodeValue"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["IsNodeValue"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)FieldFields["IsNodeValue"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)FieldFields["IsNodeValue"]].Data.ToString());
			}
			set { __Elements[(int)FieldFields["IsNodeValue"]].Data = value; }
		}
		[DataMember]
        [Category("Presentation")]
		public bool IsDateOnly
		{
			get
			{
				if (__Elements[(int)FieldFields["IsDateOnly"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["IsDateOnly"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)FieldFields["IsDateOnly"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)FieldFields["IsDateOnly"]].Data.ToString());
			}
			set { __Elements[(int)FieldFields["IsDateOnly"]].Data = value; }
		}
		[DataMember]
        [Category("Presentation")]
		public bool IsTimeOnly
		{
			get
			{
				if (__Elements[(int)FieldFields["IsTimeOnly"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["IsTimeOnly"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)FieldFields["IsTimeOnly"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)FieldFields["IsTimeOnly"]].Data.ToString());
			}
			set { __Elements[(int)FieldFields["IsTimeOnly"]].Data = value; }
		}
		[DataMember]
        [Category("Presentation")]
		public bool UseForMatching
		{
			get
			{
				if (__Elements[(int)FieldFields["UseForMatching"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["UseForMatching"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)FieldFields["UseForMatching"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)FieldFields["UseForMatching"]].Data.ToString());
			}
			set { __Elements[(int)FieldFields["UseForMatching"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public bool IsEntity
		{
			get
			{
				if (__Elements[(int)FieldFields["IsEntity"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["IsEntity"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)FieldFields["IsEntity"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)FieldFields["IsEntity"]].Data.ToString());
			}
			set { __Elements[(int)FieldFields["IsEntity"]].Data = value; }
		}
        [DataMember]
        [Category("Handlers")]
        public bool DataChangeNotification
        {
            get
            {
                if (__Elements[(int)FieldFields["DataChangeNotification"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["DataChangeNotification"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldFields["DataChangeNotification"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldFields["DataChangeNotification"]].Data.ToString());
            }
            set { __Elements[(int)FieldFields["DataChangeNotification"]].Data = value; }
        }
        [DataMember]
        [Category("Behavior")]
        public bool Dependant
        {
            get
            {
                if (__Elements[(int)FieldFields["Dependant"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["Dependant"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldFields["Dependant"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldFields["Dependant"]].Data.ToString());
            }
            set { __Elements[(int)FieldFields["Dependant"]].Data = value; }
        }
        [DataMember]
        [Category("Presentation")]
        public bool HideOnAdd
        {
            get
            {
                if (__Elements[(int)FieldFields["HideOnAdd"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["HideOnAdd"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldFields["HideOnAdd"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldFields["HideOnAdd"]].Data.ToString());
            }
            set { __Elements[(int)FieldFields["HideOnAdd"]].Data = value; }
        }
        [DataMember]
        [Category("Presentation")]
        public bool HideOnEdit
        {
            get
            {
                if (__Elements[(int)FieldFields["HideOnEdit"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["HideOnEdit"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldFields["HideOnEdit"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldFields["HideOnEdit"]].Data.ToString());
            }
            set { __Elements[(int)FieldFields["HideOnEdit"]].Data = value; }
        }
        [DataMember]
        [Category("Presentation")]
        public bool HideOnSummary
        {
            get
            {
                if (__Elements[(int)FieldFields["HideOnSummary"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["HideOnSummary"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldFields["HideOnSummary"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldFields["HideOnSummary"]].Data.ToString());
            }
            set { __Elements[(int)FieldFields["HideOnSummary"]].Data = value; }
        }
        [DataMember]
        [Category("Presentation")]
        public bool Redact
        {
            get
            {
                if (__Elements[(int)FieldFields["Redact"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["Redact"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldFields["Redact"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldFields["Redact"]].Data.ToString());
            }
            set { __Elements[(int)FieldFields["Redact"]].Data = value; }
        }
        [DataMember]
        public bool Confirm
        {
            get
            {
                if (__Elements[(int)FieldFields["Confirm"]].Data.ToString().Equals("0") || __Elements[(int)FieldFields["Confirm"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldFields["Confirm"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldFields["Confirm"]].Data.ToString());
            }
            set { __Elements[(int)FieldFields["Confirm"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public List<FieldPresentation> FieldPresentations
        {
            get { return __Elements[(int)FieldFields["FieldPresentations"]].Data as List<FieldPresentation>; }
            set { __Elements[(int)FieldFields["FieldPresentations"]].Data = value; }
        }

		#endregion

		#region "Methods"

		[OnDeserializing]
		void OnDeserializing(StreamingContext ctx)
		{
			Initialize();
		}

		public virtual void Load()
		{
			CRUDFunctions.Load<Field>(this);
			Rules.ForEach(e => e.Load());
			Attributes.ForEach(e => e.Load());
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Field>(this);
			base.Save<Field>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Field>(this);
			base.Save<Field>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Field> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Fields")
		{
			Collection<Field> collection = new Collection<Field>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Field>(collection);
			return collection;
		}

		public static Field Get(string ConnectionString)
		{
			Field entity = new Field();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class FieldElementIndex
	{
		public readonly static ElementBase DisplayName = new ElementBase()
		{
			Name = "DisplayName",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 255
		};

		public readonly static ElementBase Rules = new ElementBase()
		{
			Name = "Rules",
			TypeName = "Rule",
			IsCollection = true,
			MultipleResultSetIndex = 1
		};

		public readonly static ElementBase DataType = new ElementBase()
		{
			Name = "DataType",
			TypeName = "DataType",
			IsEntity = true,
			MultipleResultSetIndex = 2
		};

		public readonly static ElementBase Attributes = new ElementBase()
		{
			Name = "Attributes",
			TypeName = "Attribute",
			IsCollection = true,
			MultipleResultSetIndex = 3
		};

		public readonly static ElementBase DefaultValue = new ElementBase()
		{
			Name = "DefaultValue",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 255
		};

		public readonly static ElementBase FieldType = new ElementBase()
		{
			Name = "FieldType",
			TypeName = "FieldType",
			IsEntity = true,
			MultipleResultSetIndex = 4
		};

		public readonly static ElementBase IsNodeAttribute = new ElementBase()
		{
			Name = "IsNodeAttribute",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

		public readonly static ElementBase IsNodeValue = new ElementBase()
		{
			Name = "IsNodeValue",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

		public readonly static ElementBase IsDateOnly = new ElementBase()
		{
			Name = "IsDateOnly",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

		public readonly static ElementBase IsTimeOnly = new ElementBase()
		{
			Name = "IsTimeOnly",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

		public readonly static ElementBase UseForMatching = new ElementBase()
		{
			Name = "UseForMatching",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

		public readonly static ElementBase IsEntity = new ElementBase()
		{
			Name = "IsEntity",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

        public readonly static ElementBase DataChangeNotification = new ElementBase()
        {
            Name = "DataChangeNotification",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

        public readonly static ElementBase Dependant = new ElementBase()
        {
            Name = "Dependant",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

        public readonly static ElementBase HideOnAdd = new ElementBase()
        {
            Name = "HideOnAdd",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

        public readonly static ElementBase HideOnEdit = new ElementBase()
        {
            Name = "HideOnEdit",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

        public readonly static ElementBase HideOnSummary = new ElementBase()
        {
            Name = "HideOnSummary",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

        public readonly static ElementBase Redact = new ElementBase()
        {
            Name = "Redact",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

        public readonly static ElementBase Confirm = new ElementBase()
        {
            Name = "Confirm",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit,
            SqlSize = 50
        };

        public readonly static ElementBase FieldPresentations = new ElementBase()
        {
            Name = "FieldPresentations",
            TypeName = "FieldPresentation",
            IsCollection = true,
            MultipleResultSetIndex = 5
        };

	}

	#endregion
}

