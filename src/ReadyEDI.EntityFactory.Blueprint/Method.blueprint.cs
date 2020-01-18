using System;
using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;
using System.Drawing.Design;
using System.ComponentModel.Design;

namespace ReadyEDI.EntityFactory.Blueprint
{
	public partial class Method : Base
	{
		#region "Enumerators"

		private Dictionary<string, int> MethodFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public Method()
			: base()
		{
			Initialize();
		}

		public Method(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Method(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Method";

			ElementBaseData ebdArguments = new ElementBaseData(MethodElementIndex.Arguments)
			{
				Data = new List<Argument>()
			};
			ElementBaseData ebdCode = new ElementBaseData(MethodElementIndex.Code)
			{
				Data = String.Empty
			};
			ElementBaseData ebdReturnType = new ElementBaseData(MethodElementIndex.ReturnType)
			{
				Data = new FieldType()
			};
			ElementBaseData ebdFramework = new ElementBaseData(MethodElementIndex.Framework)
			{
				Data = String.Empty
			};

			MethodFields.Add("Arguments", AddField(ebdArguments));
			MethodFields.Add("Code", AddField(ebdCode));
			MethodFields.Add("ReturnType", AddField(ebdReturnType));
			MethodFields.Add("Framework", AddField(ebdFramework));

			AttachedCollections.Add(ebdArguments);
			AttachedCollections.Add(ebdReturnType);
		}

		#endregion

		#region "Properties"

		[DataMember]
		public List<Argument> Arguments
		{
			get { return __Elements[(int)MethodFields["Arguments"]].Data as List<Argument>; }
			set { __Elements[(int)MethodFields["Arguments"]].Data = value; }
		}
		[DataMember]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		public string Code
		{
			get { return __Elements[(int)MethodFields["Code"]].Data.ToString(); }
			set { __Elements[(int)MethodFields["Code"]].Data = value; }
		}
		[DataMember]
		public FieldType ReturnType
		{
			get { return __Elements[(int)MethodFields["ReturnType"]].Data as FieldType; }
			set { __Elements[(int)MethodFields["ReturnType"]].Data = value; }
		}
		[DataMember]
		public string Framework
		{
			get { return __Elements[(int)MethodFields["Framework"]].Data.ToString(); }
			set { __Elements[(int)MethodFields["Framework"]].Data = value; }
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
			CRUDFunctions.Load<Method>(this);
			Arguments.ForEach(e => e.Load());
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Method>(this);
			base.Save<Method>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Method>(this);
			base.Save<Method>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Method> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Methods")
		{
			Collection<Method> collection = new Collection<Method>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Method>(collection);
			return collection;
		}

		public static Method Get(string ConnectionString)
		{
			Method entity = new Method();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class MethodElementIndex
	{
		public readonly static ElementBase Arguments = new ElementBase()
		{
			Name = "Arguments",
			TypeName = "Argument",
			IsCollection = true,
			MultipleResultSetIndex = 1
		};

		public readonly static ElementBase Code = new ElementBase()
		{
			Name = "Code",
			TypeName = "string",
			SqlDataType = SqlDbType.Text
		};

		public readonly static ElementBase ReturnType = new ElementBase()
		{
			Name = "ReturnType",
			TypeName = "FieldType",
			IsEntity = true,
			MultipleResultSetIndex = 2
		};

		public readonly static ElementBase Framework = new ElementBase()
		{
			Name = "Framework",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 50
		};

	}

	#endregion
}

