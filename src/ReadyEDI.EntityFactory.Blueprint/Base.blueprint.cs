using ReadyEDI.EntityFactory.Data;
using ReadyEDI.EntityFactory.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Blueprint
{
	[DataContract]
	public partial class Base : EntityAbstract, Interfaces.IBase
	{
		#region "Enumerators"

		public enum BaseFields
		{
			Comment = 0,
			InternalNote = 1,
			BaseGuid = 2,
			DirtyFlag = 3,
			Sequence = 4,
			Name = 5
		}

		public enum BaseActions
		{

		}

		#endregion

		#region "Constructors"

		public Base()
		{
			Initialize();
		}

		public Base(DataProxyCollection data)
		{
			Initialize();

			Load(data);
		}

		#endregion

		#region "Initialize"

		private void Initialize()
		{
			__EntityName = "Base";
			RegisterActions(typeof(BaseActions));

			ElementBaseData ebdComment = new ElementBaseData(BaseElementIndex.Comment)
			{
				ID = (int)BaseFields.Comment,
				Data = String.Empty
			};
			ElementBaseData ebdInternalNote = new ElementBaseData(BaseElementIndex.InternalNote)
			{
				ID = (int)BaseFields.InternalNote,
				Data = String.Empty
			};
			ElementBaseData ebdBaseGuid = new ElementBaseData(BaseElementIndex.BaseGuid)
			{
				ID = (int)BaseFields.BaseGuid,
				Data = Guid.Empty
			};
			ElementBaseData ebdDirtyFlag = new ElementBaseData(BaseElementIndex.DirtyFlag)
			{
				ID = (int)BaseFields.DirtyFlag,
				Data = false
			};
			ElementBaseData ebdSequence = new ElementBaseData(BaseElementIndex.Sequence)
			{
				ID = (int)BaseFields.Sequence,
				Data = 0
			};
			ElementBaseData ebdName = new ElementBaseData(BaseElementIndex.Name)
			{
				ID = (int)BaseFields.Name,
				Data = String.Empty
			};

			__Elements.Add(ebdComment);
			__Elements.Add(ebdInternalNote);
			__Elements.Add(ebdBaseGuid);
			__Elements.Add(ebdDirtyFlag);
			__Elements.Add(ebdSequence);
			__Elements.Add(ebdName);
		}

		#endregion

		#region "Properties"

		[DataMember]
        [Browsable(false)]
		public string Comment
		{
			get { return __Elements[(int)BaseFields.Comment].Data.ToString(); }
			set { __Elements[(int)BaseFields.Comment].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public string InternalNote
		{
			get { return __Elements[(int)BaseFields.InternalNote].Data.ToString(); }
			set { __Elements[(int)BaseFields.InternalNote].Data = value; }
		}
		[DisplayName("Guid")]
		[DataMember]
        [Browsable(false)]
		public Guid BaseGuid
		{
			get { return Guid.Parse(__Elements[(int)BaseFields.BaseGuid].Data.ToString()); }
			set { __Elements[(int)BaseFields.BaseGuid].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public bool DirtyFlag
		{
			get
			{
				if (__Elements[(int)BaseFields.DirtyFlag].Data.ToString().Equals("0") || __Elements[(int)BaseFields.DirtyFlag].Data.Equals(String.Empty))
					return false;
				if (__Elements[(int)BaseFields.DirtyFlag].Data.ToString().Equals("1"))
					return true;
				return bool.Parse(__Elements[(int)BaseFields.DirtyFlag].Data.ToString());
			}
			set { __Elements[(int)BaseFields.DirtyFlag].Data = value; }
		}
		[DataMember]
        [Browsable(false)]
		public int Sequence
		{
			get { return int.Parse(__Elements[(int)BaseFields.Sequence].Data.ToString()); }
			set { __Elements[(int)BaseFields.Sequence].Data = value; }
		}
		[DataMember]
        [Category("Naming")]
		public string Name
		{
			get { return __Elements[(int)BaseFields.Name].Data.ToString(); }
			set { __Elements[(int)BaseFields.Name].Data = value; }
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
			CRUDFunctions.Load<Base>(this);
			__Memento = GetData();
		}

		public virtual void Save()
		{
			CRUDFunctions.Save<Base>(this);
			base.Save<Base>();
		}

		public virtual void Merge()
		{
			CRUDFunctions.Merge<Base>(this);
			base.Save<Base>();
		}

		public void Delete()
		{
			CRUDActions.Delete(this);
		}

		#endregion

		#region "Static Methods"
		public static Collection<Base> GetCollection(List<CriteriaParameter> Parameters, string ConnectionString, string CollectionName = "Bases")
		{
			Collection<Base> collection = new Collection<Base>();
			collection.CollectionName = CollectionName;
			collection.ConnectionString = ConnectionString;
			collection.Parameters = Parameters;
			CRUDActions.Retrieve<Base>(collection);
			return collection;
		}

		public static Base Get(string ConnectionString)
		{
			Base entity = new Base();
			entity.ConnectionString = ConnectionString;

			entity.Load();
			if (entity.Notifications.Count > 0 || entity.Exceptions.Count > 0) { entity = null; }
			return entity;
		}
		#endregion

	}

	#region "Element Index"

	public static class BaseElementIndex
	{
		public readonly static ElementBase Comment = new ElementBase()
		{
			Name = "Comment",
			Bitwise = 1 << 0 /* 1 */,
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 8000
		};

		public readonly static ElementBase InternalNote = new ElementBase()
		{
			Name = "InternalNote",
			Bitwise = 1 << 1 /* 2 */,
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 8000
		};

		public readonly static ElementBase BaseGuid = new ElementBase()
		{
			Name = "BaseGuid",
			Bitwise = 1 << 2 /* 4 */,
			TypeName = "Guid",
			UseForMatching = true,
			SqlDbIsPrimaryKey = true,
			SqlDataType = SqlDbType.UniqueIdentifier
		};

		public readonly static ElementBase DirtyFlag = new ElementBase()
		{
			Name = "DirtyFlag",
			Bitwise = 1 << 3 /* 8 */,
			TypeName = "bool",
			DoNotPersist = true,
			SqlDataType = SqlDbType.Bit
		};

		public readonly static ElementBase Sequence = new ElementBase()
		{
			Name = "Sequence",
			Bitwise = 1 << 4 /* 16 */,
			TypeName = "int",
			SqlDataType = SqlDbType.Int
		};

		public readonly static ElementBase Name = new ElementBase()
		{
			Name = "Name",
			Bitwise = 1 << 5 /* 32 */,
			TypeName = "string",
			SqlDataType = SqlDbType.VarChar,
			SqlSize = 255
		};

	}

	#endregion
}

