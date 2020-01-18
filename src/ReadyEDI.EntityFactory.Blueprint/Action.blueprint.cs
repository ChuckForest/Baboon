using System;
using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public partial class Action : Base
	{
		#region "Enumerators"

		private Dictionary<string, int> ActionFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public Action()
			: base()
		{
			Initialize();
		}

		public Action(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Action(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Action";

			ElementBaseData ebdParametersIn = new ElementBaseData(ActionElementIndex.ParametersIn)
			{
				Data = new List<Field>()
			};
			ElementBaseData ebdParametersOut = new ElementBaseData(ActionElementIndex.ParametersOut)
			{
				Data = new List<Field>()
			};
			ElementBaseData ebdParametersInOut = new ElementBaseData(ActionElementIndex.ParametersInOut)
			{
				Data = new List<Field>()
			};
            ElementBaseData ebdScript = new ElementBaseData(ActionElementIndex.Script)
            {
                Data = String.Empty
            };

			ActionFields.Add("ParametersIn", AddField(ebdParametersIn));
			ActionFields.Add("ParametersOut", AddField(ebdParametersOut));
			ActionFields.Add("ParametersInOut", AddField(ebdParametersInOut));
            ActionFields.Add("Script", AddField(ebdScript));

			AttachedCollections.Add(ebdParametersIn);
			AttachedCollections.Add(ebdParametersOut);
			AttachedCollections.Add(ebdParametersInOut);
		}

		#endregion

		#region "Properties"

		[DataMember]
		public List<Field> ParametersIn
		{
			get { return __Elements[(int)ActionFields["ParametersIn"]].Data as List<Field>; }
			set { __Elements[(int)ActionFields["ParametersIn"]].Data = value; }
		}
		[DataMember]
		public List<Field> ParametersOut
		{
			get { return __Elements[(int)ActionFields["ParametersOut"]].Data as List<Field>; }
			set { __Elements[(int)ActionFields["ParametersOut"]].Data = value; }
		}
		[DataMember]
		public List<Field> ParametersInOut
		{
			get { return __Elements[(int)ActionFields["ParametersInOut"]].Data as List<Field>; }
			set { __Elements[(int)ActionFields["ParametersInOut"]].Data = value; }
		}
        [DataMember]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Script
        {
            get { return __Elements[(int)ActionFields["Script"]].Data.ToString(); }
            set { __Elements[(int)ActionFields["Script"]].Data = value; }
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
			CRUDFunctions.Load<Action>(this);
			ParametersIn.ForEach(e => e.Load());
			ParametersOut.ForEach(e => e.Load());
			ParametersInOut.ForEach(e => e.Load());
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Action>(this);
			base.Save<Action>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Action>(this);
			base.Save<Action>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Action> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Actions")
		{
			Collection<Action> collection = new Collection<Action>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Action>(collection);
			return collection;
		}

		public static Action Get(string ConnectionString)
		{
			Action entity = new Action();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class ActionElementIndex
	{
		public readonly static ElementBase ParametersIn = new ElementBase()
		{
			Name = "ParametersIn",
			TypeName = "Field",
			IsCollection = true,
			MultipleResultSetIndex = 1
		};

		public readonly static ElementBase ParametersOut = new ElementBase()
		{
			Name = "ParametersOut",
			TypeName = "Field",
			IsCollection = true,
			MultipleResultSetIndex = 2
		};

		public readonly static ElementBase ParametersInOut = new ElementBase()
		{
			Name = "ParametersInOut",
			TypeName = "Field",
			IsCollection = true,
			MultipleResultSetIndex = 3
		};

        public readonly static ElementBase Script = new ElementBase()
        {
            Name = "Script",
            TypeName = "string",
            SqlDataType = SqlDbType.Text
        };

	}

	#endregion
}

