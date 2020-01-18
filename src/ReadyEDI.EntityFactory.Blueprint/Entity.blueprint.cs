using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
    public partial class Entity : Base, Interfaces.IFieldsContainer
	{
		#region "Enumerators"

		private Dictionary<string, int> EntityFields = new Dictionary<string, int>();

        public enum DeletionTypeOption
        {
            Delete,
            Deactivate
        }

		#endregion

		#region "Constructors"

		public Entity()
			: base()
		{
			Initialize();
		}

		public Entity(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Entity(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Entity";

			ElementBaseData ebdFields = new ElementBaseData(EntityElementIndex.Fields)
			{
				Data = new List<Field>()
			};
			ElementBaseData ebdActions = new ElementBaseData(EntityElementIndex.Actions)
			{
				Data = new List<Action>()
			};
			ElementBaseData ebdEnumerators = new ElementBaseData(EntityElementIndex.Enumerators)
			{
				Data = new List<Enumeration>()
			};
			ElementBaseData ebdAttributes = new ElementBaseData(EntityElementIndex.Attributes)
			{
				Data = new List<Attribute>()
			};
			ElementBaseData ebdInterfaces = new ElementBaseData(EntityElementIndex.Interfaces)
			{
				Data = new List<Interface>()
			};
			ElementBaseData ebdExtended = new ElementBaseData(EntityElementIndex.Extended)
			{
				Data = new Extension()
			};
			ElementBaseData ebdEntityId = new ElementBaseData(EntityElementIndex.EntityId)
			{
				Data = 0
			};
			ElementBaseData ebdEntityName = new ElementBaseData(EntityElementIndex.EntityName)
			{
				Data = String.Empty
			};
            ElementBaseData ebdBluePrintGuid = new ElementBaseData(EntityElementIndex.BluePrintGuid)
            {
                Data = Guid.Empty
            };
            ElementBaseData ebdVersion = new ElementBaseData(EntityElementIndex.Version)
            {
                Data = String.Empty
            };
            ElementBaseData ebdDeletionType = new ElementBaseData(EntityElementIndex.DeletionType)
            {
                Data = DeletionTypeOption.Delete
            };
            ElementBaseData ebdIcon = new ElementBaseData(EntityElementIndex.Icon)
            {
                Data = String.Empty
            };
            ElementBaseData ebdMethods = new ElementBaseData(EntityElementIndex.Methods)
            {
                Data = new List<Method>()
            };
            ElementBaseData ebdFieldContainerGuid = new ElementBaseData(EntityElementIndex.FieldContainerGuid)
            {
                Data = Guid.NewGuid()
            };

			EntityFields.Add("Fields", AddField(ebdFields));
			EntityFields.Add("Actions", AddField(ebdActions));
			EntityFields.Add("Enumerators", AddField(ebdEnumerators));
			EntityFields.Add("Attributes", AddField(ebdAttributes));
			EntityFields.Add("Interfaces", AddField(ebdInterfaces));
			EntityFields.Add("Extended", AddField(ebdExtended));
			EntityFields.Add("EntityId", AddField(ebdEntityId));
			EntityFields.Add("EntityName", AddField(ebdEntityName));
            EntityFields.Add("BluePrintGuid", AddField(ebdBluePrintGuid));
            EntityFields.Add("Version", AddField(ebdVersion));
            EntityFields.Add("DeletionType", AddField(ebdDeletionType));
            EntityFields.Add("Icon", AddField(ebdIcon));
            EntityFields.Add("Methods", AddField(ebdMethods));
            EntityFields.Add("FieldContainerGuid", AddField(ebdFieldContainerGuid));

			AttachedCollections.Add(ebdFields);
			AttachedCollections.Add(ebdActions);
			AttachedCollections.Add(ebdEnumerators);
			AttachedCollections.Add(ebdAttributes);
			AttachedCollections.Add(ebdInterfaces);
            AttachedCollections.Add(ebdExtended);
            AttachedCollections.Add(ebdMethods);
		}

		#endregion

		#region "Properties"

		[DataMember]
        [Browsable(false)]
		public List<Field> Fields
		{
			get { return __Elements[(int)EntityFields["Fields"]].Data as List<Field>; }
			set { __Elements[(int)EntityFields["Fields"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public List<Action> Actions
		{
			get { return __Elements[(int)EntityFields["Actions"]].Data as List<Action>; }
			set { __Elements[(int)EntityFields["Actions"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public List<Enumeration> Enumerators
		{
			get { return __Elements[(int)EntityFields["Enumerators"]].Data as List<Enumeration>; }
			set { __Elements[(int)EntityFields["Enumerators"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public List<Attribute> Attributes
		{
			get { return __Elements[(int)EntityFields["Attributes"]].Data as List<Attribute>; }
			set { __Elements[(int)EntityFields["Attributes"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public List<Interface> Interfaces
		{
			get { return __Elements[(int)EntityFields["Interfaces"]].Data as List<Interface>; }
			set { __Elements[(int)EntityFields["Interfaces"]].Data = value; }
		}
		[DataMember]
        [Category("Naming")]
		public Extension Extended
		{
			get { return __Elements[(int)EntityFields["Extended"]].Data as Extension; }
			set { __Elements[(int)EntityFields["Extended"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public int EntityId
		{
			get { return int.Parse(__Elements[(int)EntityFields["EntityId"]].Data.ToString()); }
			set { __Elements[(int)EntityFields["EntityId"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public string EntityName
		{
			get { return __Elements[(int)EntityFields["EntityName"]].Data.ToString(); }
			set { __Elements[(int)EntityFields["EntityName"]].Data = value; }
		}
        [DataMember]
        [Browsable(false)]
        public Guid BluePrintGuid
        {
            get { return Guid.Parse(__Elements[(int)EntityFields["BluePrintGuid"]].Data.ToString()); }
            set { __Elements[(int)EntityFields["BluePrintGuid"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public string Version
        {
            get { return __Elements[(int)EntityFields["Version"]].Data.ToString(); }
            set { __Elements[(int)EntityFields["Version"]].Data = value; }
        }
        [DataMember]
        [Category("Behavior")]
        public DeletionTypeOption DeletionType
        {
            get { return (DeletionTypeOption)__Elements[(int)EntityFields["DeletionType"]].Data; }
            set { __Elements[(int)EntityFields["DeletionType"]].Data = value; }
        }
        [DataMember]
        [Category("Presentation")]
        public string Icon
        {
            get { return __Elements[(int)EntityFields["Icon"]].Data.ToString(); }
            set { __Elements[(int)EntityFields["Icon"]].Data = value; }
        }
        [DataMember]
        [Browsable(false)]
        public List<Method> Methods
        {
            get { return __Elements[(int)EntityFields["Methods"]].Data as List<Method>; }
            set { __Elements[(int)EntityFields["Methods"]].Data = value; }
        }
        [DataMember]
        public Guid FieldContainerGuid
        {
            get { return Guid.Parse(__Elements[(int)EntityFields["FieldContainerGuid"]].Data.ToString()); }
            set { __Elements[(int)EntityFields["FieldContainerGuid"]].Data = value; }
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
			CRUDFunctions.Load<Entity>(this);
			Fields.ForEach(e => e.Load());
			Actions.ForEach(e => e.Load());
			Enumerators.ForEach(e => e.Load());
			Attributes.ForEach(e => e.Load());
			Interfaces.ForEach(e => e.Load());
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Entity>(this);
			base.Save<Entity>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Entity>(this);
			base.Save<Entity>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Entity> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Entities")
		{
			Collection<Entity> collection = new Collection<Entity>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Entity>(collection);
			return collection;
		}

		public static Entity Get(string ConnectionString)
		{
			Entity entity = new Entity();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class EntityElementIndex
	{
		public readonly static ElementBase Fields = new ElementBase()
		{
			Name = "Fields",
			TypeName = "Field",
			IsCollection = true,
			MultipleResultSetIndex = 1
		};

		public readonly static ElementBase Actions = new ElementBase()
		{
			Name = "Actions",
			TypeName = "Action",
			IsCollection = true,
			MultipleResultSetIndex = 3
		};

		public readonly static ElementBase Enumerators = new ElementBase()
		{
			Name = "Enumerators",
			TypeName = "Enumeration",
			IsCollection = true,
			MultipleResultSetIndex = 4
		};

		public readonly static ElementBase Attributes = new ElementBase()
		{
			Name = "Attributes",
			TypeName = "Attribute",
			IsCollection = true,
			MultipleResultSetIndex = 5
		};

		public readonly static ElementBase Interfaces = new ElementBase()
		{
			Name = "Interfaces",
			TypeName = "Interface",
			IsCollection = true,
			MultipleResultSetIndex = 6
		};

		public readonly static ElementBase Extended = new ElementBase()
		{
			Name = "Extended",
			TypeName = "Extension",
			SqlDataType = SqlDbType.VarChar,
            IsEntity = true,
            MultipleResultSetIndex = 2
		};

		public readonly static ElementBase EntityId = new ElementBase()
		{
			Name = "EntityId",
			TypeName = "int",
			SqlDataType = SqlDbType.Int
		};

		public readonly static ElementBase EntityName = new ElementBase()
		{
			Name = "EntityName",
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

        public readonly static ElementBase DeletionType = new ElementBase()
        {
            Name = "DeletionType",
            TypeName = "int",
            SqlDataType = SqlDbType.Int
        };

        public readonly static ElementBase Icon = new ElementBase()
        {
            Name = "Icon",
            TypeName = "string",
            SqlDataType = SqlDbType.VarChar,
            SqlSize = 255
        };

        public readonly static ElementBase Methods = new ElementBase()
        {
            Name = "Methods",
            TypeName = "Method",
            IsCollection = true,
            MultipleResultSetIndex = 7
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

