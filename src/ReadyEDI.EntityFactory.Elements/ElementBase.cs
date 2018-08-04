using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.Serialization;

namespace ReadyEDI.EntityFactory.Elements
{
    [Serializable]
    public class ElementBase : BaseHash, IBase
    {
        public enum ElementBaseFields : int
        {
            ID,
            Name,
            Bitwise,
            UseForMatching,
            //MaxLength,
            //MinLength,
            Control,
            ParameterIn,
            ParameterOut,
            DbType,
            DbSize,
            DbPrecision,
            DbScale,
            SqlDbType,
            SqlDbSize,
            SqlDbPrecision,
            SqlDbScale,
            OracleDbType,
            OracleDbSize,
            OracleDbPrecision,
            OracleDbScale,
            IsCollection,
            //Match,
            EDIElementNumber,
            ParameterInOut,
            MultipleResultSetIndex,
            DisplayName,
            PresentationOnly,
            IsEntity,
            EntityType,
            LazyLoad,
            ForeignKey,
            DefaultValue,
            DbIsPrimaryKey,
            DbIsIdentity,
            SqlDbIsPrimaryKey,
            SqlDbIsIdentity,
            OracleDbIsPrimaryKey,
            OracleDbIsIdentity,
            IsNodeAttribute,
            IsNodeValue,
            IsDateOnly,
            IsTimeOnly,
            IsDateTime,
            TypeName,
            IsFlag,
            ActionParameters,
            DoNotPersist//,
            //IsRequired
        }

        public enum ControlType
        {
            None,
            Textbox
        }

        public ElementBase()
        {
            InitializeProperties(typeof(ElementBaseFields));
        }

        public ElementBase(ElementBase elementBase)
        {
            Hash = elementBase.Hash;
        }

        public int? ID
        {
            get
            {
                if (Hash[(int)ElementBaseFields.ID] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.ID].ToString());
            }
            set { Hash[(int)ElementBaseFields.ID] = value; }
        }

        public string Name
        {
            get
            {
                if (Hash[(int)ElementBaseFields.Name] == null)
                    return null;
                return Hash[(int)ElementBaseFields.Name].ToString();
            }
            set { Hash[(int)ElementBaseFields.Name] = value; }
        }

        public long? Bitwise
        {
            get
            {
                if (Hash[(int)ElementBaseFields.Bitwise] == null)
                    return null;
                return long.Parse(Hash[(int)ElementBaseFields.Bitwise].ToString());
            }
            set { Hash[(int)ElementBaseFields.Bitwise] = value; }
        }

        public bool UseForMatching
        {
            get
            {
                if (Hash[(int)ElementBaseFields.UseForMatching] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.UseForMatching].ToString());
            }
            set { Hash[(int)ElementBaseFields.UseForMatching] = value; }
        }

        //public int? MaxLength
        //{
        //    get
        //    {
        //        if (Hash[(int)ElementBaseFields.MaxLength] == null)
        //            return null;
        //        return int.Parse(Hash[(int)ElementBaseFields.MaxLength].ToString());
        //    }
        //    set { Hash[(int)ElementBaseFields.MaxLength] = value; }
        //}

        //public int? MinLength
        //{
        //    get
        //    {
        //        if (Hash[(int)ElementBaseFields.MinLength] == null)
        //            return null;
        //        return int.Parse(Hash[(int)ElementBaseFields.MinLength].ToString());
        //    }
        //    set { Hash[(int)ElementBaseFields.MinLength] = value; }
        //}

        public ControlType Control
        {
            get
            {
                if (Hash[(int)ElementBaseFields.Control] == null)
                    return ControlType.None;
                return (ControlType)Enum.Parse(typeof(ControlType), Hash[(int)ElementBaseFields.Control].ToString());
            }
            set { Hash[(int)ElementBaseFields.Control] = value; }
        }

        public long? ParameterIn
        {
            get
            {
                if (Hash[(int)ElementBaseFields.ParameterIn] == null)
                    return null;
                return long.Parse(Hash[(int)ElementBaseFields.ParameterIn].ToString());
            }
            set { Hash[(int)ElementBaseFields.ParameterIn] = value; }
        }

        public long? ParameterOut
        {
            get
            {
                if (Hash[(int)ElementBaseFields.ParameterOut] == null)
                    return null;
                return long.Parse(Hash[(int)ElementBaseFields.ParameterOut].ToString());
            }
            set { Hash[(int)ElementBaseFields.ParameterOut] = value; }
        }

        public System.Data.SqlDbType? SqlDataType
        {
            get
            {
                if (Hash[(int)ElementBaseFields.SqlDbType] == null)
                    return null;
                return (SqlDbType)Hash[(int)ElementBaseFields.SqlDbType];
            }
            set { Hash[(int)ElementBaseFields.SqlDbType] = value; }
        }

        public int? SqlSize
        {
            get
            {
                if (Hash[(int)ElementBaseFields.SqlDbSize] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.SqlDbSize].ToString());
            }
            set { Hash[(int)ElementBaseFields.SqlDbSize] = value; }
        }

        public int? SqlPrecision
        {
            get
            {
                if (Hash[(int)ElementBaseFields.SqlDbPrecision] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.SqlDbPrecision].ToString());
            }
            set { Hash[(int)ElementBaseFields.SqlDbPrecision] = value; }
        }

        public int? SqlScale
        {
            get
            {
                if (Hash[(int)ElementBaseFields.SqlDbScale] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.SqlDbScale].ToString());
            }
            set { Hash[(int)ElementBaseFields.SqlDbScale] = value; }
        }

        public System.Data.OracleClient.OracleType? OracleDataType
        {
            get
            {
                if (Hash[(int)ElementBaseFields.OracleDbType] == null)
                    return null;
                return (System.Data.OracleClient.OracleType)Hash[(int)ElementBaseFields.OracleDbType];
            }
            set { Hash[(int)ElementBaseFields.OracleDbType] = value; }
        }

        public int? OracleSize
        {
            get
            {
                if (Hash[(int)ElementBaseFields.OracleDbSize] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.OracleDbSize].ToString());
            }
            set { Hash[(int)ElementBaseFields.OracleDbSize] = value; }
        }

        public int? OraclePrecision
        {
            get
            {
                if (Hash[(int)ElementBaseFields.OracleDbPrecision] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.OracleDbPrecision].ToString());
            }
            set { Hash[(int)ElementBaseFields.OracleDbPrecision] = value; }
        }

        public int? OracleScale
        {
            get
            {
                if (Hash[(int)ElementBaseFields.OracleDbScale] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.OracleDbScale].ToString());
            }
            set { Hash[(int)ElementBaseFields.OracleDbScale] = value; }
        }

        public System.Data.DbType? DbDataType
        {
            get
            {
                if (Hash[(int)ElementBaseFields.DbType] == null)
                    return null;
                return (System.Data.DbType)Hash[(int)ElementBaseFields.DbType];
            }
            set { Hash[(int)ElementBaseFields.DbType] = value; }
        }

        public int? DbSize
        {
            get
            {
                if (Hash[(int)ElementBaseFields.DbSize] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.DbSize].ToString());
            }
            set { Hash[(int)ElementBaseFields.DbSize] = value; }
        }

        public int? DbPrecision
        {
            get
            {
                if (Hash[(int)ElementBaseFields.DbPrecision] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.DbPrecision].ToString());
            }
            set { Hash[(int)ElementBaseFields.DbPrecision] = value; }
        }

        public int? DbScale
        {
            get
            {
                if (Hash[(int)ElementBaseFields.DbScale] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.DbScale].ToString());
            }
            set { Hash[(int)ElementBaseFields.DbScale] = value; }
        }

        public bool IsCollection
        {
            get
            {
                if (Hash[(int)ElementBaseFields.IsCollection] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.IsCollection].ToString());
            }
            set { Hash[(int)ElementBaseFields.IsCollection] = value; }
        }

        //public string Match
        //{
        //    get
        //    {
        //        if (Hash[(int)ElementBaseFields.Match] == null) return null;
        //        return Hash[(int)ElementBaseFields.Match].ToString();
        //    }
        //    set { Hash[(int)ElementBaseFields.Match] = value; }
        //}

        public int? EDIElementNumber
        {
            get
            {
                if (Hash[(int)ElementBaseFields.EDIElementNumber] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.EDIElementNumber].ToString());
            }
            set { Hash[(int)ElementBaseFields.EDIElementNumber] = value; }
        }

        public long? ParameterInOut
        {
            get
            {
                if (Hash[(int)ElementBaseFields.ParameterInOut] == null)
                    return null;
                return long.Parse(Hash[(int)ElementBaseFields.ParameterInOut].ToString());
            }
            set { Hash[(int)ElementBaseFields.ParameterInOut] = value; }
        }

         public int? MultipleResultSetIndex
        {
            get
            {
                if (Hash[(int)ElementBaseFields.MultipleResultSetIndex] == null)
                    return null;
                return int.Parse(Hash[(int)ElementBaseFields.MultipleResultSetIndex].ToString());
            }
            set { Hash[(int)ElementBaseFields.MultipleResultSetIndex] = value; }
        }

        public string DisplayName
        {
            get
            {
                if (Hash[(int)ElementBaseFields.DisplayName] == null) return null;
                return Hash[(int)ElementBaseFields.DisplayName].ToString();
            }
            set { Hash[(int)ElementBaseFields.DisplayName] = value; }
        }

        public bool IsEntity
        {
            get
            {
                if (Hash[(int)ElementBaseFields.IsEntity] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.IsEntity].ToString());
            }
            set { Hash[(int)ElementBaseFields.IsEntity] = value; }
        }

        public bool PresentationOnly
        {
            get
            {
                if (Hash[(int)ElementBaseFields.PresentationOnly] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.PresentationOnly].ToString());
            }
            set { Hash[(int)ElementBaseFields.PresentationOnly] = value; }
        }

        public string EntityType
        {
            get
            {
                if (Hash[(int)ElementBaseFields.EntityType] == null) return null;
                return Hash[(int)ElementBaseFields.EntityType].ToString();
            }
            set { Hash[(int)ElementBaseFields.EntityType] = value; }
        }

        public bool LazyLoad
        {
            get
            {
                if (Hash[(int)ElementBaseFields.LazyLoad] == null) return false;
                return bool.Parse(Hash[(int)ElementBaseFields.LazyLoad].ToString());
            }
            set { Hash[(int)ElementBaseFields.LazyLoad] = value; }
        }

        public string ForeignKey
        {
            get
            {
                if (Hash[(int)ElementBaseFields.ForeignKey] == null) return null;
                return Hash[(int)ElementBaseFields.ForeignKey].ToString();
            }
            set { Hash[(int)ElementBaseFields.ForeignKey] = value; }
        }
        
        public string DefaultValue
        {
            get
            {
                if (Hash[(int)ElementBaseFields.DefaultValue] == null) return null;
                return Hash[(int)ElementBaseFields.DefaultValue].ToString();
            }
            set { Hash[(int)ElementBaseFields.DefaultValue] = value; }
        }

        public bool DbIsPrimaryKey
        {
            get
            {
                if (Hash[(int)ElementBaseFields.DbIsPrimaryKey] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.DbIsPrimaryKey].ToString());
            }
            set { Hash[(int)ElementBaseFields.DbIsPrimaryKey] = value; }
        }

        public bool DbIsIdentity
        {
            get
            {
                if (Hash[(int)ElementBaseFields.DbIsIdentity] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.DbIsIdentity].ToString());
            }
            set { Hash[(int)ElementBaseFields.DbIsIdentity] = value; }
        }

        public bool SqlDbIsPrimaryKey
        {
            get
            {
                if (Hash[(int)ElementBaseFields.SqlDbIsPrimaryKey] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.SqlDbIsPrimaryKey].ToString());
            }
            set { Hash[(int)ElementBaseFields.SqlDbIsPrimaryKey] = value; }
        }

        public bool SqlDbIsIdentity
        {
            get
            {
                if (Hash[(int)ElementBaseFields.SqlDbIsIdentity] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.SqlDbIsIdentity].ToString());
            }
            set { Hash[(int)ElementBaseFields.SqlDbIsIdentity] = value; }
        }

        public bool OracleDbIsPrimaryKey
        {
            get
            {
                if (Hash[(int)ElementBaseFields.OracleDbIsPrimaryKey] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.OracleDbIsPrimaryKey].ToString());
            }
            set { Hash[(int)ElementBaseFields.OracleDbIsPrimaryKey] = value; }
        }

        public bool OracleDbIsIdentity
        {
            get
            {
                if (Hash[(int)ElementBaseFields.OracleDbIsIdentity] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.OracleDbIsIdentity].ToString());
            }
            set { Hash[(int)ElementBaseFields.OracleDbIsIdentity] = value; }
        }



        public bool IsNodeAttribute
        {
            get
            {
                if (Hash[(int)ElementBaseFields.IsNodeAttribute] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.IsNodeAttribute].ToString());
            }
            set { Hash[(int)ElementBaseFields.IsNodeAttribute] = value; }
        }

        public bool IsNodeValue
        {
            get
            {
                if (Hash[(int)ElementBaseFields.IsNodeValue] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.IsNodeValue].ToString());
            }
            set { Hash[(int)ElementBaseFields.IsNodeValue] = value; }
        }

        public bool IsDateOnly
        {
            get
            {
                if (Hash[(int)ElementBaseFields.IsDateOnly] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.IsDateOnly].ToString());
            }
            set { Hash[(int)ElementBaseFields.IsDateOnly] = value; }
        }

        public bool IsTimeOnly
        {
            get
            {
                if (Hash[(int)ElementBaseFields.IsTimeOnly] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.IsTimeOnly].ToString());
            }
            set { Hash[(int)ElementBaseFields.IsTimeOnly] = value; }
        }

        public bool IsDateTime
        {
            get
            {
                if (Hash[(int)ElementBaseFields.IsDateTime] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.IsDateTime].ToString());
            }
            set { Hash[(int)ElementBaseFields.IsDateTime] = value; }
        }

        public string TypeName
        {
            get
            {
                if (Hash[(int)ElementBaseFields.TypeName] == null) return null;
                return Hash[(int)ElementBaseFields.TypeName].ToString();
            }
            set { Hash[(int)ElementBaseFields.TypeName] = value; }
        }

        public bool IsFlag
        {
            get
            {
                if (Hash[(int)ElementBaseFields.IsFlag] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.IsFlag].ToString());
            }
            set { Hash[(int)ElementBaseFields.IsFlag] = value; }
        }

        public List<ActionParameter> ActionParameters
        {
            get
            {
                if (Hash[(int)ElementBaseFields.ActionParameters] == null)
                    return new List<ActionParameter>();
                return Hash[(int)ElementBaseFields.ActionParameters] as List<ActionParameter>;
            }
            set { Hash[(int)ElementBaseFields.ActionParameters] = value; }
        }

        public bool DoNotPersist
        {
            get
            {
                if (Hash[(int)ElementBaseFields.DoNotPersist] == null)
                    return false;
                return bool.Parse(Hash[(int)ElementBaseFields.DoNotPersist].ToString());
            }
            set { Hash[(int)ElementBaseFields.DoNotPersist] = value; }
        }

        //public bool IsRequired
        //{
        //    get
        //    {
        //        if (Hash[(int)ElementBaseFields.IsRequired] == null)
        //            return false;
        //        return bool.Parse(Hash[(int)ElementBaseFields.IsRequired].ToString());
        //    }
        //    set { Hash[(int)ElementBaseFields.IsRequired] = value; }
        //}

    }
}
