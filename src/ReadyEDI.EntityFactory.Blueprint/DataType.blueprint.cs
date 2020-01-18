using System;
using ReadyEDI.EntityFactory;
using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public partial class DataType : Base
	{
		#region "Enumerators"

		private Dictionary<string, int> DataTypeFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public DataType()
			: base()
		{
			Initialize();
		}

		public DataType(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public DataType(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "DataType";

			ElementBaseData ebdSize = new ElementBaseData(DataTypeElementIndex.Size)
			{
				Data = null
			};
			ElementBaseData ebdPrecision = new ElementBaseData(DataTypeElementIndex.Precision)
			{
				Data = null
			};
			ElementBaseData ebdScale = new ElementBaseData(DataTypeElementIndex.Scale)
			{
				Data = null
			};
			ElementBaseData ebdIsIdentity = new ElementBaseData(DataTypeElementIndex.IsIdentity)
			{
				Data = false
			};
			ElementBaseData ebdColumnType = new ElementBaseData(DataTypeElementIndex.ColumnType)
			{
				Data = String.Empty
			};
			ElementBaseData ebdDefaultValue = new ElementBaseData(DataTypeElementIndex.DefaultValue)
			{
				Data = String.Empty
			};
            ElementBaseData ebdIsPrimaryKey = new ElementBaseData(DataTypeElementIndex.IsPrimaryKey)
            {
                Data = false
            };
            ElementBaseData ebdMultipleResultSetIndex = new ElementBaseData(DataTypeElementIndex.MultipleResultSetIndex)
            {
                Data = 0
            };
            ElementBaseData ebdIsIdentifier = new ElementBaseData(DataTypeElementIndex.IsIdentifier)
            {
                Data = false
            };

			DataTypeFields.Add("Size", AddField(ebdSize));
			DataTypeFields.Add("Precision", AddField(ebdPrecision));
			DataTypeFields.Add("Scale", AddField(ebdScale));
			DataTypeFields.Add("IsIdentity", AddField(ebdIsIdentity));
			DataTypeFields.Add("ColumnType", AddField(ebdColumnType));
			DataTypeFields.Add("DefaultValue", AddField(ebdDefaultValue));
            DataTypeFields.Add("IsPrimaryKey", AddField(ebdIsPrimaryKey));
            DataTypeFields.Add("MultipleResultSetIndex", AddField(ebdMultipleResultSetIndex));
            DataTypeFields.Add("IsIdentifier", AddField(ebdIsIdentifier));
		}

		#endregion

		#region "Properties"

		[DataMember]
		public int? Size
		{
			get
			{
				if (__Elements[(int)DataTypeFields["Size"]].Data == null)
					return null;
				else
					return int.Parse(__Elements[(int)DataTypeFields["Size"]].Data.ToString());
			}
			set { __Elements[(int)DataTypeFields["Size"]].Data = value; }
		}
		[DataMember]
		public int? Precision
		{
			get
			{
				if (__Elements[(int)DataTypeFields["Precision"]].Data == null)
					return null;
				else
					return int.Parse(__Elements[(int)DataTypeFields["Precision"]].Data.ToString());
			}
			set { __Elements[(int)DataTypeFields["Precision"]].Data = value; }
		}
		[DataMember]
		public int? Scale
		{
			get
			{
				if (__Elements[(int)DataTypeFields["Scale"]].Data == null)
					return null;
				else
					return int.Parse(__Elements[(int)DataTypeFields["Scale"]].Data.ToString());
			}
			set { __Elements[(int)DataTypeFields["Scale"]].Data = value; }
		}
		[DataMember]
		public bool IsIdentity
		{
			get
			{
				if (__Elements[(int)DataTypeFields["IsIdentity"]].Data.ToString().Equals("0") || __Elements[(int)DataTypeFields["IsIdentity"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)DataTypeFields["IsIdentity"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)DataTypeFields["IsIdentity"]].Data.ToString());
			}
			set { __Elements[(int)DataTypeFields["IsIdentity"]].Data = value; }
		}
		[DataMember]
		public string ColumnType
		{
			get { return __Elements[(int)DataTypeFields["ColumnType"]].Data.ToString(); }
			set { __Elements[(int)DataTypeFields["ColumnType"]].Data = value; }
		}
		[DataMember]
		public string DefaultValue
		{
			get { return __Elements[(int)DataTypeFields["DefaultValue"]].Data.ToString(); }
			set { __Elements[(int)DataTypeFields["DefaultValue"]].Data = value; }
		}
        [DataMember]
        public bool? IsPrimaryKey
        {
            get
            {
                if (__Elements[(int)DataTypeFields["IsPrimaryKey"]].Data == null || __Elements[(int)DataTypeFields["IsPrimaryKey"]].Data.ToString().Equals("0") || __Elements[(int)DataTypeFields["IsPrimaryKey"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)DataTypeFields["IsPrimaryKey"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)DataTypeFields["IsPrimaryKey"]].Data.ToString());
            }
            set { __Elements[(int)DataTypeFields["IsPrimaryKey"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public int MultipleResultSetIndex
        {
            get { return __Elements[(int)DataTypeFields["MultipleResultSetIndex"]].Data == null ? 0 : int.Parse(__Elements[(int)DataTypeFields["MultipleResultSetIndex"]].Data.ToString()); }
            set { __Elements[(int)DataTypeFields["MultipleResultSetIndex"]].Data = value; }
        }
        [DataMember]
        public bool? IsIdentifier
        {
            get
            {
                if (__Elements[(int)DataTypeFields["IsIdentifier"]].Data == null || __Elements[(int)DataTypeFields["IsIdentifier"]].Data.ToString().Equals("0") || __Elements[(int)DataTypeFields["IsIdentifier"]].Data.Equals(String.Empty))
                    return false;
                if (__Elements[(int)DataTypeFields["IsIdentifier"]].Data.ToString().Equals("1"))
                    return true;
                return bool.Parse(__Elements[(int)DataTypeFields["IsIdentifier"]].Data.ToString());
            }
            set { __Elements[(int)DataTypeFields["IsIdentifier"]].Data = value; }
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
			CRUDFunctions.Load<DataType>(this);
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<DataType>(this);
			base.Save<DataType>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<DataType>(this);
			base.Save<DataType>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<DataType> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "DataTypes")
		{
			Collection<DataType> collection = new Collection<DataType>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<DataType>(collection);
			return collection;
		}

		public static DataType Get(string ConnectionString)
		{
			DataType entity = new DataType();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class DataTypeElementIndex
	{
		public readonly static ElementBase Size = new ElementBase()
		{
			Name = "Size",
			TypeName = "int",
			SqlDataType = SqlDbType.Int
		};

		public readonly static ElementBase Precision = new ElementBase()
		{
			Name = "Precision",
			TypeName = "int",
			SqlDataType = SqlDbType.Int
		};

		public readonly static ElementBase Scale = new ElementBase()
		{
			Name = "Scale",
			TypeName = "int",
			SqlDataType = SqlDbType.Int
		};

		public readonly static ElementBase IsIdentity = new ElementBase()
		{
			Name = "IsIdentity",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

		public readonly static ElementBase ColumnType = new ElementBase()
		{
			Name = "ColumnType",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 25
		};

		public readonly static ElementBase DefaultValue = new ElementBase()
		{
			Name = "DefaultValue",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 255
		};

        public readonly static ElementBase IsPrimaryKey = new ElementBase()
        {
            Name = "IsPrimaryKey",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };

        public readonly static ElementBase MultipleResultSetIndex = new ElementBase()
        {
            Name = "MultipleResultSetIndex",
            TypeName = "int",
            SqlDataType = SqlDbType.Int
        };

        public readonly static ElementBase IsIdentifier = new ElementBase()
        {
            Name = "IsIdentifier",
            TypeName = "bool",
            SqlDataType = SqlDbType.Bit
        };
	}

	#endregion
}

