using System;
using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
    public partial class FieldType : Base, Interfaces.IGeneratable
	{
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

		#region "Enumerators"

		private Dictionary<string, int> FieldTypeFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public FieldType()
			: base()
		{
			Initialize();
		}

		public FieldType(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public FieldType(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "FieldType";

			ElementBaseData ebdFramework = new ElementBaseData(FieldTypeElementIndex.Framework)
			{
				Data = String.Empty
			};
			ElementBaseData ebdIsNullable = new ElementBaseData(FieldTypeElementIndex.IsNullable)
			{
				Data = false
			};
			ElementBaseData ebdIsCollection = new ElementBaseData(FieldTypeElementIndex.IsCollection)
			{
				Data = false
			};
			ElementBaseData ebdCollectionType = new ElementBaseData(FieldTypeElementIndex.CollectionType)
			{
				Data = String.Empty
			};
			ElementBaseData ebdDefaultValue = new ElementBaseData(FieldTypeElementIndex.DefaultValue)
			{
				Data = String.Empty
			};
			ElementBaseData ebdTextEncoding = new ElementBaseData(FieldTypeElementIndex.TextEncoding)
			{
				Data = String.Empty
			};
			ElementBaseData ebdIsFactoryEntity = new ElementBaseData(FieldTypeElementIndex.IsFactoryEntity)
			{
				Data = false
			};
            ElementBaseData ebdFieldTypeOption = new ElementBaseData(FieldTypeElementIndex.FieldTypeOption)
            {
                Data = String.Empty
            };
            ElementBaseData ebdIsInterface = new ElementBaseData(FieldTypeElementIndex.IsInterface)
            {
                Data = false
            };
            ElementBaseData ebdNamespace = new ElementBaseData(FieldTypeElementIndex.Namespace)
            {
                Data = String.Empty
            };
            ElementBaseData ebdFieldTypeName = new ElementBaseData(FieldTypeElementIndex.FieldTypeName)
            {
                Data = String.Empty
            };
            ElementBaseData ebdIsAttached = new ElementBaseData(FieldTypeElementIndex.IsAttached)
            {
                Data = false
            };
            ElementBaseData ebdDoNotPersist = new ElementBaseData(FieldTypeElementIndex.DoNotPersist)
            {
                Data = false
            };
            ElementBaseData ebdSqlDataType = new ElementBaseData(FieldTypeElementIndex.SqlDataType)
            {
                Data = String.Empty
            };
            ElementBaseData ebdBluePrintGuid = new ElementBaseData(FieldTypeElementIndex.BluePrintGuid)
            {
                Data = Guid.Empty
            };
            ElementBaseData ebdVersion = new ElementBaseData(FieldTypeElementIndex.Version)
            {
                Data = String.Empty
            };
            ElementBaseData ebdIsOneToMany = new ElementBaseData(FieldTypeElementIndex.IsOneToMany)
            {
                Data = false
            };

			FieldTypeFields.Add("Framework", AddField(ebdFramework));
			FieldTypeFields.Add("IsNullable", AddField(ebdIsNullable));
			FieldTypeFields.Add("IsCollection", AddField(ebdIsCollection));
			FieldTypeFields.Add("CollectionType", AddField(ebdCollectionType));
			FieldTypeFields.Add("DefaultValue", AddField(ebdDefaultValue));
			FieldTypeFields.Add("TextEncoding", AddField(ebdTextEncoding));
			FieldTypeFields.Add("IsFactoryEntity", AddField(ebdIsFactoryEntity));
            FieldTypeFields.Add("FieldTypeOption", AddField(ebdFieldTypeOption));
            FieldTypeFields.Add("IsInterface", AddField(ebdIsInterface));
            FieldTypeFields.Add("Namespace", AddField(ebdNamespace));
            FieldTypeFields.Add("FieldTypeName", AddField(ebdFieldTypeName));
            FieldTypeFields.Add("IsAttached", AddField(ebdIsAttached));
            FieldTypeFields.Add("DoNotPersist", AddField(ebdDoNotPersist));
            FieldTypeFields.Add("SqlDataType", AddField(ebdSqlDataType));
            FieldTypeFields.Add("BluePrintGuid", AddField(ebdBluePrintGuid));
            FieldTypeFields.Add("Version", AddField(ebdVersion));
            FieldTypeFields.Add("IsOneToMany", AddField(ebdIsOneToMany));
		}

		#endregion

		#region "Properties"

		[DataMember]
		public string Framework
		{
			get { return __Elements[(int)FieldTypeFields["Framework"]].Data.ToString(); }
			set { __Elements[(int)FieldTypeFields["Framework"]].Data = value; }
		}
		[DataMember]
		public bool IsNullable
		{
			get
			{
				if (__Elements[(int)FieldTypeFields["IsNullable"]].Data.ToString().Equals("0") || __Elements[(int)FieldTypeFields["IsNullable"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)FieldTypeFields["IsNullable"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)FieldTypeFields["IsNullable"]].Data.ToString());
			}
			set { __Elements[(int)FieldTypeFields["IsNullable"]].Data = value; }
		}
		[DataMember]
		public bool IsCollection
		{
			get
			{
				if (__Elements[(int)FieldTypeFields["IsCollection"]].Data.ToString().Equals("0") || __Elements[(int)FieldTypeFields["IsCollection"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)FieldTypeFields["IsCollection"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)FieldTypeFields["IsCollection"]].Data.ToString());
			}
			set { __Elements[(int)FieldTypeFields["IsCollection"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public string CollectionType
		{
			get { return __Elements[(int)FieldTypeFields["CollectionType"]].Data.ToString(); }
			set { __Elements[(int)FieldTypeFields["CollectionType"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public string DefaultValue
		{
			get { return __Elements[(int)FieldTypeFields["DefaultValue"]].Data.ToString(); }
			set
            {
                if (value != __Elements[(int)FieldTypeFields["DefaultValue"]].Data.ToString())
                {
                    __Elements[(int)FieldTypeFields["DefaultValue"]].Data = value;
                    OnPropertyChanged("DefaultValue");
                }
            }
		}
		[DataMember]
        [Browsable(false)]
		public string TextEncoding
		{
			get { return __Elements[(int)FieldTypeFields["TextEncoding"]].Data.ToString(); }
			set { __Elements[(int)FieldTypeFields["TextEncoding"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public bool IsFactoryEntity
		{
			get
			{
				if (__Elements[(int)FieldTypeFields["IsFactoryEntity"]].Data.ToString().Equals("0") || __Elements[(int)FieldTypeFields["IsFactoryEntity"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)FieldTypeFields["IsFactoryEntity"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)FieldTypeFields["IsFactoryEntity"]].Data.ToString());
			}
			set { __Elements[(int)FieldTypeFields["IsFactoryEntity"]].Data = value; }
		}
        [DataMember]
        [Browsable(false)]
        public string FieldTypeOption
        {
            get { return __Elements[(int)FieldTypeFields["FieldTypeOption"]].Data.ToString(); }
            set
            {
                if (value != __Elements[(int)FieldTypeFields["FieldTypeOption"]].Data.ToString())
                {
                    __Elements[(int)FieldTypeFields["FieldTypeOption"]].Data = value;
                    OnPropertyChanged("FieldTypeOption");
                }
            }
        }
        [DataMember]
        public bool IsInterface
        {
            get
            {
                if (__Elements[(int)FieldTypeFields["IsInterface"]].Data.ToString().Equals("0") || __Elements[(int)FieldTypeFields["IsInterface"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldTypeFields["IsInterface"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldTypeFields["IsInterface"]].Data.ToString());
            }
            set { __Elements[(int)FieldTypeFields["IsInterface"]].Data = value; }
        }
        [DataMember]
        public string Namespace
        {
            get { return __Elements[(int)FieldTypeFields["Namespace"]].Data.ToString(); }
            set { __Elements[(int)FieldTypeFields["Namespace"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public string FieldTypeName
        {
            get { return __Elements[(int)FieldTypeFields["FieldTypeName"]].Data.ToString(); }
            set { __Elements[(int)FieldTypeFields["FieldTypeName"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public bool IsAttached
        {
            get
            {
                if (__Elements[(int)FieldTypeFields["IsAttached"]].Data.ToString().Equals("0") || __Elements[(int)FieldTypeFields["IsAttached"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldTypeFields["IsAttached"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldTypeFields["IsAttached"]].Data.ToString());
            }
            set { __Elements[(int)FieldTypeFields["IsAttached"]].Data = value; }
        }
        [DataMember]
        public bool DoNotPersist
        {
            get
            {
                if (__Elements[(int)FieldTypeFields["DoNotPersist"]].Data.ToString().Equals("0") || __Elements[(int)FieldTypeFields["DoNotPersist"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldTypeFields["DoNotPersist"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldTypeFields["DoNotPersist"]].Data.ToString());
            }
            set { __Elements[(int)FieldTypeFields["DoNotPersist"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public string SqlDataType
        {
            get { return __Elements[(int)FieldTypeFields["SqlDataType"]].Data.ToString(); }
            set
            {
                if (value != __Elements[(int)FieldTypeFields["SqlDataType"]].Data.ToString())
                {
                    __Elements[(int)FieldTypeFields["SqlDataType"]].Data = value;
                    OnPropertyChanged("SqlDataType");
                }
            }
        }
        [DataMember]
        [Browsable(false)]
        public Guid BluePrintGuid
        {
            get { return Guid.Parse(__Elements[(int)FieldTypeFields["BluePrintGuid"]].Data.ToString()); }
            set { __Elements[(int)FieldTypeFields["BluePrintGuid"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public string Version
        {
            get { return __Elements[(int)FieldTypeFields["Version"]].Data.ToString(); }
            set { __Elements[(int)FieldTypeFields["Version"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public bool IsOneToMany
        {
            get
            {
                if (__Elements[(int)FieldTypeFields["IsOneToMany"]].Data.ToString().Equals("0") || __Elements[(int)FieldTypeFields["IsOneToMany"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)FieldTypeFields["IsOneToMany"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)FieldTypeFields["IsOneToMany"]].Data.ToString());
            }
            set { __Elements[(int)FieldTypeFields["IsOneToMany"]].Data = value; }
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
			CRUDFunctions.Load<FieldType>(this);
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<FieldType>(this);
			base.Save<FieldType>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<FieldType>(this);
			base.Save<FieldType>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<FieldType> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "FieldTypes")
		{
			Collection<FieldType> collection = new Collection<FieldType>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<FieldType>(collection);
			return collection;
		}

		public static FieldType Get(string ConnectionString)
		{
			FieldType entity = new FieldType();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class FieldTypeElementIndex
	{
		public readonly static ElementBase Framework = new ElementBase()
		{
			Name = "Framework",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 50
		};

		public readonly static ElementBase IsNullable = new ElementBase()
		{
			Name = "IsNullable",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

		public readonly static ElementBase IsCollection = new ElementBase()
		{
			Name = "IsCollection",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

		public readonly static ElementBase CollectionType = new ElementBase()
		{
			Name = "CollectionType",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 50
		};

		public readonly static ElementBase DefaultValue = new ElementBase()
		{
			Name = "DefaultValue",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 255
		};

		public readonly static ElementBase TextEncoding = new ElementBase()
		{
			Name = "TextEncoding",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 15
		};

		public readonly static ElementBase IsFactoryEntity = new ElementBase()
		{
			Name = "IsFactoryEntity",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

        public readonly static ElementBase FieldTypeOption = new ElementBase()
        {
            Name = "FieldTypeOption",
            TypeName = "string",
            SqlDataType = SqlDbType.VarChar,
            SqlSize = 25
        };

        public readonly static ElementBase IsInterface = new ElementBase()
        {
            Name = "IsInterface",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

        public readonly static ElementBase Namespace = new ElementBase()
        {
            Name = "Namespace",
            TypeName = "string",
            SqlDataType = SqlDbType.VarChar,
            SqlSize = 255
        };

        public readonly static ElementBase FieldTypeName = new ElementBase()
        {
            Name = "FieldTypeName",
            TypeName = "string",
            SqlDataType = SqlDbType.VarChar,
            SqlSize = 255
        };

        public readonly static ElementBase IsAttached = new ElementBase()
        {
            Name = "IsAttached",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

        public readonly static ElementBase DoNotPersist = new ElementBase()
        {
            Name = "DoNotPersist",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

        public readonly static ElementBase SqlDataType = new ElementBase()
        {
            Name = "SqlDataType",
            TypeName = "string",
            SqlDataType = SqlDbType.VarChar,
            SqlSize = 255
        };

        public readonly static ElementBase BluePrintGuid = new ElementBase()
        {
            Name = "BluePrintGuid",
            TypeName = "Guid",
            SqlDataType = SqlDbType.UniqueIdentifier
        };

        public readonly static ElementBase Version = new ElementBase()
        {
            Name = "Version",
            TypeName = "string",
            SqlDataType = SqlDbType.VarChar,
            SqlSize = 15
        };

        public readonly static ElementBase IsOneToMany = new ElementBase()
        {
            Name = "IsOneToMany",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

	}

	#endregion
}

