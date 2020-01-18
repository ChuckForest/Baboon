using ReadyEDI.EntityFactory.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public partial class Attribute : Base
	{
		#region "Enumerators"

		private Dictionary<string, int> AttributeFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public Attribute()
			: base()
		{
			Initialize();
		}

		public Attribute(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Attribute(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Attribute";


		}

		#endregion

		#region "Properties"


		#endregion

		#region "Methods"

		[OnDeserializing]
		void OnDeserializing(StreamingContext ctx)
		{
			Initialize();
		}

		public virtual void Load()
		{
			CRUDFunctions.Load<Attribute>(this);
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Attribute>(this);
			base.Save<Attribute>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Attribute>(this);
			base.Save<Attribute>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Attribute> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Attributes")
		{
			Collection<Attribute> collection = new Collection<Attribute>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Attribute>(collection);
			return collection;
		}

		public static Attribute Get(string ConnectionString)
		{
			Attribute entity = new Attribute();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class AttributeElementIndex
	{
	}

	#endregion
}

