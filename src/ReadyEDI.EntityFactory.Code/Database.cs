using ReadyEDI.EntityFactory.Blueprint;
using ReadyEDI.EntityFactory.Blueprint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory.Code
{
    public enum DatabaseType
    {
        SQLServer = 0,
        Mongo = 1
    }

    public static class Database
    {
        private static List<string> _intrinsicList = new List<string> { "bool", "byte", "sbyte", "char", "decimal", "double", "float", "int", "uint", "long", "ulong", "object", "short", "ushort", "string", "datetime", "Guid" };

        public static void CreateTable(Entity entity)
        {
            var builder = new StringBuilder();

            GenerateDatabaseStubHeader(builder);
            builder.AppendLine("SET ANSI_PADDING ON");
            builder.AppendLine("GO");
            builder.AppendLine(string.Format("CREATE TABLE [{0}].[{1}](", entity.SchemaName, entity.EntityName));

            List<string> TableFields = new List<string>();
            List<Constraint> constraints = new List<Constraint>();

            if (entity.DeletionType.Equals(Entity.DeletionTypeOption.Deactivate))
            {
                TableFields.Add("\t[__Active] [Bit] NOT NULL");
                constraints.Add(new Constraint() { ParentTableName = entity.Name, FieldName = "__Active", DefaultValue = "1" });
            }

            var foreignKeys = new List<ForeignKey>();
            var uniqueKeys = new List<UniqueKey>();
            GenerateTableFields(entity, TableFields, foreignKeys, uniqueKeys);



            builder.Append(String.Join(String.Format(",{0}", Environment.NewLine), TableFields));
            builder.AppendLine(String.Empty);
            List<Field> keyFields = new List<Field>();
            //entity.Interfaces.ToList()
            //    .ForEach(
            //        i =>
            //        {
            //            GenerateKeysForInterface(i, keyFields);
            //        }
            //    );
            keyFields.AddRange(entity.Fields.Where(f => !f.FieldType.DoNotPersist && (bool)f.DataType.IsPrimaryKey).ToList());
            if (keyFields.Count() > 0)
            {
                builder.AppendLine(String.Format("\tCONSTRAINT [PK_{0}s] PRIMARY KEY CLUSTERED", entity.Name));
                builder.AppendLine("\t(");
                List<string> keyFieldLines = new List<string>();
                keyFields.ForEach(f => keyFieldLines.Add(String.Format("\t\t[{0}] ASC", f.Name)));
                builder.Append(String.Join(String.Format(",{0}", Environment.NewLine), keyFieldLines));
                builder.AppendLine(String.Empty);
                builder.AppendLine(
                    "\t)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            }

            builder.AppendLine(") ON [PRIMARY]");
            builder.AppendLine("GO");

            builder.AppendLine("SET ANSI_PADDING OFF");

    //        GenerateConstraints(constraints, builder);

            var keyBuilder = new StringBuilder();

    //        GenerateForeignKeys(foreignKeys, keyBuilder, true);

            //entity.Interfaces.ToList()
            //    .ForEach(
            //        i =>
            //        {
            //            BluePrintORM.Library.BluePrint.ReadyEDI.BluePrintORM.BluePrint bluePrintTemplate = _bluePrints.Find(bp => bp.BluePrintGuid.Equals(i.BluePrintGuid));
            //            Interface interfaceTemplate = bluePrintTemplate.Interfaces.ToList().Find(bpi => bpi.Name.Equals(i.Name));
            //            Field field = interfaceTemplate.Fields.ToList().Find(f => !f.FieldType.DoNotPersist && (bool)f.DataType.IsIdentifier);
            //            if (field != null)
            //            {
            //                keyBuilder.AppendLine(String.Format("ALTER TABLE [{0}].[{1}] ADD  CONSTRAINT [DF_{1}_{2}]  DEFAULT (newsequentialid()) FOR [{2}]", bluePrint.Name, entity.Name, field.Name));
            //                keyBuilder.AppendLine("GO");
            //            }
            //        }
            //    );

    //        GenerateDatabaseStubFooter(builder);
        }

        private static void GenerateConstraints(string schemaName, List<Constraint> constraints, StringBuilder builder)
        {
            constraints.ForEach(f =>
            {
                builder.AppendLine(String.Format("ALTER TABLE [{0}].[{1}] ADD CONSTRAINT [DF_{1}_{2}] DEFAULT({3}) FOR [{2}]", schemaName, f.ParentTableName, f.FieldName, f.DefaultValue));
                builder.AppendLine("GO");
            });
        }

        private static string GetSqlColumnDataTypeConstraints(Field field)
        {
            string ret = String.Empty;

            if (field.FieldType.FieldTypeName.ToLower().Equals("string") && !field.DataType.ColumnType.Equals("Text"))
                ret = String.Format("({0})", field.DataType.Size);
            else if (field.FieldType.FieldTypeName.ToLower().Equals("decimal"))
                ret = String.Format("({0},{1})", field.DataType.Precision, field.DataType.Scale);

            return ret;
        }

        private static string GetSqlColumnDefinition(string fieldName, Field field, bool ignoreId = false, bool forceNull = false)
        {
            return String.Format("\t[{0}] [{1}]{2}{3}{4} NULL",
                fieldName, //0
                field.DataType.ColumnType, //1
                GetSqlColumnDataTypeConstraints(field), //2
                !ignoreId && field.DataType.IsIdentity ? " IDENTITY(1,1)" : !ignoreId && (bool)field.DataType.IsIdentifier ? " ROWGUIDCOL" : String.Empty, //3
                forceNull || field.FieldType.IsNullable ? String.Empty : " NOT" //4
            );
        }

        private static void GenerateTableFields(Blueprint.Interfaces.IFieldsContainer fieldContainer, List<string> tableFields, List<ForeignKey> foreignKeys, List<UniqueKey> uniqueKeys)
        {
            List<string> uniqueKeyColumns = new List<string>();

            fieldContainer.Fields.Where(f => 
                !f.FieldType.DoNotPersist
                && (!f.FieldType.IsCollection || _intrinsicList.Contains(f.FieldType.FieldTypeName, StringComparer.CurrentCultureIgnoreCase))
                && !f.IsEntity).ToList().ForEach(f =>
            {
                if (f.UseForMatching)
                    uniqueKeyColumns.Add(f.Name);

                tableFields.Add(GetSqlColumnDefinition(f.Name, f));

                //if (f.FieldType.IsCollection && !_intrinsicList.Contains(f.FieldType.FieldTypeName, StringComparer.CurrentCultureIgnoreCase))
                //{
                //    //BluePrintORM.Library.BluePrint.ReadyEDI.BluePrintORM.BluePrint collectionBluePrint = _bluePrints.Find(bp => bp.BluePrintGuid.Equals(f.FieldType.BluePrintGuid));
                //    //BluePrintORM.Library.BluePrint.ReadyEDI.BluePrintORM.Entity collectionEntity = collectionBluePrint.Entities.ToList().Find(e => e.Name.Equals(f.FieldType.FieldTypeName));

                //    ////GenerateMappingTable(fieldContainer, f.Name, collectionEntity, stubs, bluePrint);
                //    //GenerateMappingTable2(parentEntity, f.Name, collectionEntity, interfacesInExtendedEntities, stubs, bluePrint);
                //}
                //else if (f.IsEntity)
                //{
                //    //BluePrintORM.Library.BluePrint.ReadyEDI.BluePrintORM.BluePrint entityBluePrint = _bluePrints.Find(bp => bp.BluePrintGuid.Equals(f.FieldType.BluePrintGuid));
                //    //BluePrintORM.Library.BluePrint.ReadyEDI.BluePrintORM.Entity entityEntity = entityBluePrint.Entities.ToList().Find(e => e.Name.Equals(f.FieldType.FieldTypeName));

                //    //GenerateTableFieldsForEntityField(entityEntity, entityEntity, null, parentEntity.Name, f.Name, tableFields, mappedForeignKeys, interfacesInExtendedEntities);
                //}
                //else
                //{
                //    if (f.UseForMatching)
                //        uniqueKeyColumns.Add(f.Name);

                //    tableFields.Add(GetSqlColumnDefinition(f.Name, f));
                //}
            });

            if (uniqueKeyColumns.Count > 0)
                uniqueKeys.Add(new UniqueKey { TableName = fieldContainer.Name, ColumnNames = uniqueKeyColumns, BluePrintName = fieldContainer.SchemaName });

        }

        private static void GenerateDatabaseStubHeader(StringBuilder builder)
        {
            builder.AppendLine("SET ANSI_NULLS ON");
            builder.AppendLine("GO");
            builder.AppendLine("SET QUOTED_IDENTIFIER ON");
            builder.AppendLine("GO");
        }

    }
}
