using ReadyEDI.EntityFactory.Elements;
using ReadyEDI.EntityFactory.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
	[DataContract]
	public partial class FieldPresentation : Base
	{
		#region "Enumerators"

		private Dictionary<string, int> FieldPresentationFields = new Dictionary<string, int>();

		#endregion

		#region "Constructors"

		public FieldPresentation()
			: base()
		{
			Initialize();
		}

		public FieldPresentation(DataProxyCollection data)
		{
			Initialize();
			Load(data);
		}

		public FieldPresentation(Base extendedBase)
		{
			base.__Elements = extendedBase.__Elements;
			base.__Actions = extendedBase.__Actions;
			base.__Memento = extendedBase.__Memento;

			Initialize();
		}

		private void Initialize()
		{
			__EntityName = "FieldPresentation";

			ElementBaseData ebdDefaultValue = new ElementBaseData(FieldPresentationElementIndex.DefaultValue)
			{
				Data = String.Empty
			};
			ElementBaseData ebdDisplayName = new ElementBaseData(FieldPresentationElementIndex.DisplayName)
			{
				Data = String.Empty
			};
			ElementBaseData ebdHideOnAdd = new ElementBaseData(FieldPresentationElementIndex.HideOnAdd)
			{
				Data = false
			};
			ElementBaseData ebdHideOnEdit = new ElementBaseData(FieldPresentationElementIndex.HideOnEdit)
			{
				Data = false
			};
			ElementBaseData ebdHideOnSummary = new ElementBaseData(FieldPresentationElementIndex.HideOnSummary)
			{
				Data = false
			};
			ElementBaseData ebdIsDateOnly = new ElementBaseData(FieldPresentationElementIndex.IsDateOnly)
			{
				Data = false
			};
			ElementBaseData ebdIsTimeOnly = new ElementBaseData(FieldPresentationElementIndex.IsTimeOnly)
			{
				Data = false
			};
			ElementBaseData ebdRedact = new ElementBaseData(FieldPresentationElementIndex.Redact)
			{
				Data = false
			};
			ElementBaseData ebdConfirm = new ElementBaseData(FieldPresentationElementIndex.Confirm)
			{
				Data = false
			};
			ElementBaseData ebdFieldContainerGuid = new ElementBaseData(FieldPresentationElementIndex.FieldContainerGuid)
			{
				Data = Guid.Empty
			};

			FieldPresentationFields.Add("DefaultValue", AddField(ebdDefaultValue));
			FieldPresentationFields.Add("DisplayName", AddField(ebdDisplayName));
			FieldPresentationFields.Add("HideOnAdd", AddField(ebdHideOnAdd));
			FieldPresentationFields.Add("HideOnEdit", AddField(ebdHideOnEdit));
			FieldPresentationFields.Add("HideOnSummary", AddField(ebdHideOnSummary));
			FieldPresentationFields.Add("IsDateOnly", AddField(ebdIsDateOnly));
			FieldPresentationFields.Add("IsTimeOnly", AddField(ebdIsTimeOnly));
			FieldPresentationFields.Add("Redact", AddField(ebdRedact));
			FieldPresentationFields.Add("Confirm", AddField(ebdConfirm));
			FieldPresentationFields.Add("FieldContainerGuid", AddField(ebdFieldContainerGuid));
		}

		#endregion

		#region "Properties"

		[DataMember]
		public string DefaultValue
		{
			get { return __Elements[(int)FieldPresentationFields["DefaultValue"]].Data.ToString(); }
			set { __Elements[(int)FieldPresentationFields["DefaultValue"]].Data = value; }
		}
		[DataMember]
		public string DisplayName
		{
			get { return __Elements[(int)FieldPresentationFields["DisplayName"]].Data.ToString(); }
			set { __Elements[(int)FieldPresentationFields["DisplayName"]].Data = value; }
		}
		[DataMember]
		public bool HideOnAdd
		{
			get { return bool.Parse(__Elements[(int)FieldPresentationFields["HideOnAdd"]].Data.ToString()); }
			set { __Elements[(int)FieldPresentationFields["HideOnAdd"]].Data = value; }
		}
		[DataMember]
		public bool HideOnEdit
		{
			get { return bool.Parse(__Elements[(int)FieldPresentationFields["HideOnEdit"]].Data.ToString()); }
			set { __Elements[(int)FieldPresentationFields["HideOnEdit"]].Data = value; }
		}
		[DataMember]
		public bool HideOnSummary
		{
			get { return bool.Parse(__Elements[(int)FieldPresentationFields["HideOnSummary"]].Data.ToString()); }
			set { __Elements[(int)FieldPresentationFields["HideOnSummary"]].Data = value; }
		}
		[DataMember]
		public bool IsDateOnly
		{
			get { return bool.Parse(__Elements[(int)FieldPresentationFields["IsDateOnly"]].Data.ToString()); }
			set { __Elements[(int)FieldPresentationFields["IsDateOnly"]].Data = value; }
		}
		[DataMember]
		public bool IsTimeOnly
		{
			get { return bool.Parse(__Elements[(int)FieldPresentationFields["IsTimeOnly"]].Data.ToString()); }
			set { __Elements[(int)FieldPresentationFields["IsTimeOnly"]].Data = value; }
		}
		[DataMember]
		public bool Redact
		{
			get { return bool.Parse(__Elements[(int)FieldPresentationFields["Redact"]].Data.ToString()); }
			set { __Elements[(int)FieldPresentationFields["Redact"]].Data = value; }
		}
		[DataMember]
		public bool Confirm
		{
			get { return bool.Parse(__Elements[(int)FieldPresentationFields["Confirm"]].Data.ToString()); }
			set { __Elements[(int)FieldPresentationFields["Confirm"]].Data = value; }
		}
		[DataMember]
		public Guid FieldContainerGuid
		{
			get { return Guid.Parse(__Elements[(int)FieldPresentationFields["FieldContainerGuid"]].Data.ToString()); }
			set { __Elements[(int)FieldPresentationFields["FieldContainerGuid"]].Data = value; }
		}

		#endregion

		[OnDeserializing]
		void OnDeserializing(StreamingContext ctx)
		{
			Initialize();
		}

		public override void Load()
		{
			CRUDFunctions.Load<FieldPresentation>(this);
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<FieldPresentation>(this);
			base.Save<FieldPresentation>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<FieldPresentation>(this);
			base.Save<FieldPresentation>();
		}

		public void Delete()
		{
			CRUDFunctions.Delete(this);
		}

		public static List<FieldPresentation> Summary(string ConnectionString = null)
		{
			Collection<FieldPresentation> collection = new Collection<FieldPresentation>();
			if (!String.IsNullOrWhiteSpace(ConnectionString))
				collection.ConnectionString = ConnectionString;
			CRUDActions.Retrieve<FieldPresentation>(collection);

			return collection.ToList();
		}

	}

	#region "Element Index"

	public static class FieldPresentationElementIndex
	{
		public readonly static ElementBase DefaultValue = new ElementBase()
		{
			Name = "DefaultValue",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 255
		};

		public readonly static ElementBase DisplayName = new ElementBase()
		{
			Name = "DisplayName",
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 255
		};

		public readonly static ElementBase HideOnAdd = new ElementBase()
		{
			Name = "HideOnAdd",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit,
			SqlSize = 50
		};

		public readonly static ElementBase HideOnEdit = new ElementBase()
		{
			Name = "HideOnEdit",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit,
			SqlSize = 50
		};

		public readonly static ElementBase HideOnSummary = new ElementBase()
		{
			Name = "HideOnSummary",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit,
			SqlSize = 50
		};

		public readonly static ElementBase IsDateOnly = new ElementBase()
		{
			Name = "IsDateOnly",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit,
			SqlSize = 50
		};

		public readonly static ElementBase IsTimeOnly = new ElementBase()
		{
			Name = "IsTimeOnly",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit,
			SqlSize = 50
		};

		public readonly static ElementBase Redact = new ElementBase()
		{
			Name = "Redact",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit,
			SqlSize = 50
		};

		public readonly static ElementBase Confirm = new ElementBase()
		{
			Name = "Confirm",
			TypeName = "bool",
			SqlDataType = SqlDbType.Bit,
			SqlSize = 50
		};

		public readonly static ElementBase FieldContainerGuid = new ElementBase()
		{
			Name = "FieldContainerGuid",
			TypeName = "Guid",
			SqlDataType = SqlDbType.UniqueIdentifier,
			SqlSize = 50
		};

	}

	#endregion
}

