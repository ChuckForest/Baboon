using System;
using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
    public partial class Extension : Base, Interfaces.IGeneratable
	{
		#region "Enumerators"

		private Dictionary<string, int> ExtensionFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public Extension()
			: base()
		{
			Initialize();
		}

		public Extension(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Extension(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Extension";

			ElementBaseData ebdNamespace = new ElementBaseData(ExtensionElementIndex.Namespace)
			{
				Data = String.Empty
			};
            ElementBaseData ebdBluePrintGuid = new ElementBaseData(ExtensionElementIndex.BluePrintGuid)
            {
                Data = Guid.Empty
            };
            ElementBaseData ebdVersion = new ElementBaseData(ExtensionElementIndex.Version)
            {
                Data = String.Empty
            };

			ExtensionFields.Add("Namespace", AddField(ebdNamespace));
            ExtensionFields.Add("BluePrintGuid", AddField(ebdBluePrintGuid));
            ExtensionFields.Add("Version", AddField(ebdVersion));
		}

		#endregion

		#region "Properties"

		[DataMember]
		public string Namespace
		{
			get { return __Elements[(int)ExtensionFields["Namespace"]].Data.ToString(); }
			set { __Elements[(int)ExtensionFields["Namespace"]].Data = value; }
		}
        [DataMember]
        [Browsable(false)]
        public Guid BluePrintGuid
        {
            get { return Guid.Parse(__Elements[(int)ExtensionFields["BluePrintGuid"]].Data.ToString()); }
            set { __Elements[(int)ExtensionFields["BluePrintGuid"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public string Version
        {
            get { return __Elements[(int)ExtensionFields["Version"]].Data.ToString(); }
            set { __Elements[(int)ExtensionFields["Version"]].Data = value; }
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
			CRUDFunctions.Load<Extension>(this);
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Extension>(this);
			base.Save<Extension>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Extension>(this);
			base.Save<Extension>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Extension> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Extensions")
		{
			Collection<Extension> collection = new Collection<Extension>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Extension>(collection);
			return collection;
		}

		public static Extension Get(string ConnectionString)
		{
			Extension entity = new Extension();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class ExtensionElementIndex
	{
		public readonly static ElementBase Namespace = new ElementBase()
		{
			Name = "Namespace",
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

	}

	#endregion
}

