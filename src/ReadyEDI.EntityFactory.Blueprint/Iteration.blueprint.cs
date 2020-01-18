using System;
using ReadyEDI.EntityFactory;
using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public partial class Iteration : Base
	{
		#region "Enumerators"

		private Dictionary<string, int> IterationFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public Iteration()
			: base()
		{
			Initialize();
		}

		public Iteration(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Iteration(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Iteration";

			ElementBaseData ebdValue = new ElementBaseData(IterationElementIndex.Value)
			{
				Data = String.Empty
			};

			IterationFields.Add("Value", AddField(ebdValue));
		}

		#endregion

		#region "Properties"

		[DataMember]
		public string Value
		{
			get { return __Elements[(int)IterationFields["Value"]].Data.ToString(); }
			set { __Elements[(int)IterationFields["Value"]].Data = value; }
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
			CRUDFunctions.Load<Iteration>(this);
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Iteration>(this);
			base.Save<Iteration>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Iteration>(this);
			base.Save<Iteration>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Iteration> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Iterations")
		{
			Collection<Iteration> collection = new Collection<Iteration>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Iteration>(collection);
			return collection;
		}

		public static Iteration Get(string ConnectionString)
		{
			Iteration entity = new Iteration();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class IterationElementIndex
	{
		public readonly static ElementBase Value = new ElementBase()
		{
			Name = "Value",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 255
		};

	}

	#endregion
}

