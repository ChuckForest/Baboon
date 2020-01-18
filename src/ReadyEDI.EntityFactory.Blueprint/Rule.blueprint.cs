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
	public partial class Rule : Base
	{
		#region "Enumerators"

		private Dictionary<string, int> RuleFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public Rule()
			: base()
		{
			Initialize();
		}

		public Rule(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public Rule(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "Rule";

			ElementBaseData ebdRuleType = new ElementBaseData(RuleElementIndex.RuleType)
			{
				Data = String.Empty
			};
			ElementBaseData ebdMessage = new ElementBaseData(RuleElementIndex.Message)
			{
				Data = String.Empty
			};
			ElementBaseData ebdValue = new ElementBaseData(RuleElementIndex.Value)
			{
				Data = String.Empty
			};
			ElementBaseData ebdIsString = new ElementBaseData(RuleElementIndex.IsString)
			{
				Data = false
			};

			RuleFields.Add("RuleType", AddField(ebdRuleType));
			RuleFields.Add("Message", AddField(ebdMessage));
			RuleFields.Add("Value", AddField(ebdValue));
			RuleFields.Add("IsString", AddField(ebdIsString));
		}

		#endregion

		#region "Properties"

		[DataMember]
        [Category("Contraint")]
		public string RuleType
		{
			get { return __Elements[(int)RuleFields["RuleType"]].Data.ToString(); }
			set { __Elements[(int)RuleFields["RuleType"]].Data = value; }
		}
		[DataMember]
        [Category("Contraint")]
		public string Message
		{
			get { return __Elements[(int)RuleFields["Message"]].Data.ToString(); }
			set { __Elements[(int)RuleFields["Message"]].Data = value; }
		}
		[DataMember]
        [Category("Contraint")]
		public string Value
		{
			get { return __Elements[(int)RuleFields["Value"]].Data.ToString(); }
			set { __Elements[(int)RuleFields["Value"]].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public bool IsString
		{
			get
			{
				if (__Elements[(int)RuleFields["IsString"]].Data.ToString().Equals("0") || __Elements[(int)RuleFields["IsString"]].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)RuleFields["IsString"]].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)RuleFields["IsString"]].Data.ToString());
			}
			set { __Elements[(int)RuleFields["IsString"]].Data = value; }
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
			CRUDFunctions.Load<Rule>(this);
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Rule>(this);
			base.Save<Rule>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Rule>(this);
			base.Save<Rule>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Rule> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Rules")
		{
			Collection<Rule> collection = new Collection<Rule>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Rule>(collection);
			return collection;
		}

		public static Rule Get(string ConnectionString)
		{
			Rule entity = new Rule();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class RuleElementIndex
	{
		public readonly static ElementBase RuleType = new ElementBase()
		{
			Name = "RuleType",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 255
		};

		public readonly static ElementBase Message = new ElementBase()
		{
			Name = "Message",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 1000
		};

		public readonly static ElementBase Value = new ElementBase()
		{
			Name = "Value",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 1000
		};

		public readonly static ElementBase IsString = new ElementBase()
		{
			Name = "IsString",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit
		};

	}

	#endregion
}

