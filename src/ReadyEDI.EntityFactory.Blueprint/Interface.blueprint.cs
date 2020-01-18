using System;
using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
    public partial class Interface : Base, Interfaces.IFieldsContainer
	{
		#region "Enumerators"

		private Dictionary<string, int> InterfaceFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public Interface()
			: base()
		{
			Initialize();
		}

		public Interface(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Interface(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Interface";

			ElementBaseData ebdFields = new ElementBaseData(InterfaceElementIndex.Fields)
			{
				Data = new List<Field>()
			};
            ElementBaseData ebdNamespace = new ElementBaseData(InterfaceElementIndex.Namespace)
            {
                Data = String.Empty
            };
            ElementBaseData ebdBluePrintGuid = new ElementBaseData(InterfaceElementIndex.BluePrintGuid)
            {
                Data = Guid.Empty
            };
            ElementBaseData ebdVersion = new ElementBaseData(InterfaceElementIndex.Version)
            {
                Data = String.Empty
            };
            ElementBaseData ebdExtended = new ElementBaseData(InterfaceElementIndex.Extended)
            {
                Data = new Extension()
            };
            ElementBaseData ebdFieldContainerGuid = new ElementBaseData(InterfaceElementIndex.FieldContainerGuid)
            {
                Data = Guid.NewGuid()
            };

			InterfaceFields.Add("Fields", AddField(ebdFields));
            InterfaceFields.Add("Namespace", AddField(ebdNamespace));
            InterfaceFields.Add("BluePrintGuid", AddField(ebdBluePrintGuid));
            InterfaceFields.Add("Version", AddField(ebdVersion));
            InterfaceFields.Add("Extended", AddField(ebdExtended));
            InterfaceFields.Add("FieldContainerGuid", AddField(ebdFieldContainerGuid));

			AttachedCollections.Add(ebdFields);
            AttachedCollections.Add(ebdExtended);
		}

		#endregion

		#region "Properties"

		[DataMember]
        [Browsable(false)]
		public List<Field> Fields
		{
			get { return __Elements[(int)InterfaceFields["Fields"]].Data as List<Field>; }
			set { __Elements[(int)InterfaceFields["Fields"]].Data = value; }
		}
        [DataMember]
        public string Namespace
        {
            get { return __Elements[(int)InterfaceFields["Namespace"]].Data.ToString(); }
            set { __Elements[(int)InterfaceFields["Namespace"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public Guid BluePrintGuid
        {
            get { return Guid.Parse(__Elements[(int)InterfaceFields["BluePrintGuid"]].Data.ToString()); }
            set { __Elements[(int)InterfaceFields["BluePrintGuid"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public string Version
        {
            get { return __Elements[(int)InterfaceFields["Version"]].Data.ToString(); }
            set { __Elements[(int)InterfaceFields["Version"]].Data = value; }
        }
        [DataMember]
        public Extension Extended
        {
            get { return __Elements[(int)InterfaceFields["Extended"]].Data as Extension; }
            set { __Elements[(int)InterfaceFields["Extended"]].Data = value; }
        }
        [DataMember]
        public Guid FieldContainerGuid
        {
            get { return Guid.Parse(__Elements[(int)InterfaceFields["FieldContainerGuid"]].Data.ToString()); }
            set { __Elements[(int)InterfaceFields["FieldContainerGuid"]].Data = value; }
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
			CRUDFunctions.Load<Interface>(this);
			Fields.ForEach(e => e.Load());
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Interface>(this);
			base.Save<Interface>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Interface>(this);
			base.Save<Interface>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Interface> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Interfaces")
		{
			Collection<Interface> collection = new Collection<Interface>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Interface>(collection);
			return collection;
		}

		public static Interface Get(string ConnectionString)
		{
			Interface entity = new Interface();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class InterfaceElementIndex
	{
		public readonly static ElementBase Fields = new ElementBase()
		{
			Name = "Fields",
			TypeName = "Field",
			IsCollection = true,
			MultipleResultSetIndex = 1
		};

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

        public readonly static ElementBase Extended = new ElementBase()
        {
            Name = "Extended",
            TypeName = "Extension",
            SqlDataType = SqlDbType.VarChar,
            IsEntity = true,
            MultipleResultSetIndex = 2
        };

        public readonly static ElementBase FieldContainerGuid = new ElementBase()
        {
            Name = "FieldContainerGuid",
            TypeName = "Guid",
            SqlDataType = SqlDbType.UniqueIdentifier
        };

	}

	#endregion
}

