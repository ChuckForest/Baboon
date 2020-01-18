using System;
using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public partial class Argument : Base
	{
		#region "Enumerators"

		private Dictionary<string, int> ArgumentFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public Argument()
			: base()
		{
			Initialize();
		}

		public Argument(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Argument(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Argument";

			ElementBaseData ebdFieldType = new ElementBaseData(ArgumentElementIndex.FieldType)
			{
				Data = new FieldType()
			};
			ElementBaseData ebdIsEntity = new ElementBaseData(ArgumentElementIndex.IsEntity)
			{
				Data = false
			};

			ArgumentFields.Add("FieldType", AddField(ebdFieldType));
			ArgumentFields.Add("IsEntity", AddField(ebdIsEntity));

			AttachedCollections.Add(ebdFieldType);
		}

		#endregion

		#region "Properties"

		[DataMember]
		public FieldType FieldType
		{
			get { return __Elements[(int)ArgumentFields["FieldType"]].Data as FieldType; }
			set { __Elements[(int)ArgumentFields["FieldType"]].Data = value; }
		}
		[DataMember]
		public bool IsEntity
		{
			get
			{
				if (__Elements[(int)ArgumentFields["IsEntity"]].Data.ToString().Equals("0") || __Elements[(int)ArgumentFields["IsEntity"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)ArgumentFields["IsEntity"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)ArgumentFields["IsEntity"]].Data.ToString());
			}
			set { __Elements[(int)ArgumentFields["IsEntity"]].Data = value; }
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
			CRUDFunctions.Load<Argument>(this);
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Argument>(this);
			base.Save<Argument>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Argument>(this);
			base.Save<Argument>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Argument> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Arguments")
		{
			Collection<Argument> collection = new Collection<Argument>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Argument>(collection);
			return collection;
		}

		public static Argument Get(string ConnectionString)
		{
			Argument entity = new Argument();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class ArgumentElementIndex
	{
		public readonly static ElementBase FieldType = new ElementBase()
		{
			Name = "FieldType",
			TypeName = "FieldType",
			IsEntity = true,
			MultipleResultSetIndex = 1
		};

		public readonly static ElementBase IsEntity = new ElementBase()
		{
			Name = "IsEntity",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

	}

	#endregion
}

