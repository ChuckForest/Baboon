using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public partial class Enumeration : Base
	{
		#region "Enumerators"

		private Dictionary<string, int> EnumerationFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public Enumeration()
			: base()
		{
			Initialize();
		}

		public Enumeration(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Enumeration(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Enumeration";

			ElementBaseData ebdIterations = new ElementBaseData(EnumerationElementIndex.Iterations)
			{
				Data = new List<Iteration>()
			};
			ElementBaseData ebdFieldType = new ElementBaseData(EnumerationElementIndex.FieldType)
			{
				Data = new FieldType()
			};

			EnumerationFields.Add("Iterations", AddField(ebdIterations));
			EnumerationFields.Add("FieldType", AddField(ebdFieldType));

			AttachedCollections.Add(ebdIterations);
			AttachedCollections.Add(ebdFieldType);
		}

		#endregion

		#region "Properties"

		[DataMember]
		public List<Iteration> Iterations
		{
			get { return __Elements[(int)EnumerationFields["Iterations"]].Data as List<Iteration>; }
			set { __Elements[(int)EnumerationFields["Iterations"]].Data = value; }
		}
		[DataMember]
		public FieldType FieldType
		{
			get { return __Elements[(int)EnumerationFields["FieldType"]].Data as FieldType; }
			set { __Elements[(int)EnumerationFields["FieldType"]].Data = value; }
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
			CRUDFunctions.Load<Enumeration>(this);
			Iterations.ForEach(e => e.Load());
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Enumeration>(this);
			base.Save<Enumeration>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Enumeration>(this);
			base.Save<Enumeration>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Enumeration> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Enumerations")
		{
			Collection<Enumeration> collection = new Collection<Enumeration>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Enumeration>(collection);
			return collection;
		}

		public static Enumeration Get(string ConnectionString)
		{
			Enumeration entity = new Enumeration();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class EnumerationElementIndex
	{
		public readonly static ElementBase Iterations = new ElementBase()
		{
			Name = "Iterations",
			TypeName = "Iteration",
			SqlDataType = SqlDbType.Int,
			IsCollection = true,
			MultipleResultSetIndex = 1
		};

		public readonly static ElementBase FieldType = new ElementBase()
		{
			Name = "FieldType",
			TypeName = "FieldType",
			IsEntity = true,
			MultipleResultSetIndex = 2
		};

	}

	#endregion
}

