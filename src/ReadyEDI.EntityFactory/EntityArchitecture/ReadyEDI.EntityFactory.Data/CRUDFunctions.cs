using ReadyEDI.EntityFactory.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReadyEDI.EntityFactory.Data
{
    public static class CRUDFunctions
    {
        public static int Add<T>(IEntity entity, Collection<T> entities) where T : IEntity
        {
            return Add<T>(entity, entities, false);
        }

        public static int Add<T>(IEntity entity, Collection<T> entities, bool bypassConflicts) where T : IEntity
        {
            int ret = 0;

            Collection<T> copies = new Collection<T>();
            Load<T>(copies);
            Collection<T> newEntities = copies.Except(entities);

            if (!bypassConflicts && newEntities.Count > 0 &&
                newEntities.ToList().Find(p => p.__Elements.Where(e => e.UseForMatching).Equals(entity.__Elements.Where(e => e.UseForMatching))) != null)
            {
                List<CRUDOption> options = new List<CRUDOption>();
                options.Add(new CRUDOption()
                {
                    Message = "Add Anyway",
                    OptionType = CRUDOption.CRUDOptionType.SaveAll
                });
                options.Add(new CRUDOption()
                {
                    Message = "Cancel Add",
                    OptionType = CRUDOption.CRUDOptionType.Cancel
                });
            }
            else
            {
                if (newEntities.Count > 0)
                    entities.AddRange(newEntities);
                else
                    CRUDActions.Create<T>(entity);
            }

            return ret;
        }

        public static void Load<T>(IEntity entity) where T : IEntity
        {
            entity.Populated = true;
            CRUDActions.Retrieve<T>(entity);

            for (int i = 0; i < entity.__Elements.Count(); i++)
            {
                entity.__Memento.Add(new DataProxy() { ElementId = i, Data = entity.__Elements[i].Data });
            }
        }

        public static void Load<T>(Collection<T> entities) where T : IEntity
        {
            CRUDActions.Retrieve<T>(entities);
        }

        private static void ProcessEntityException(IEntity entity, List<EntityException> exceptions)
        {
            exceptions.AddRange(entity.Exceptions);

            entity.__Elements.Where(el => el.IsEntity).ToList().ForEach(el =>
            {
                if (el.Data != null)
                    ProcessEntityException(el.Data as IEntity, exceptions);
            });

            entity.__Elements.Where(el => el.IsCollection).ToList().ForEach(el =>
            {
                List<IEntity> entities = el.Data as List<IEntity>;
                entities.ForEach(en => ProcessEntityException(en, exceptions));
            });
        }

        private static bool ProcessExceptions(IEntity entity)
        {
            List<EntityException> exceptions = new List<EntityException>();
            ProcessEntityException(entity, exceptions);

            if (exceptions.Count > 0)
            {
                string message = exceptions[0].Print(entity);
                File.WriteAllText(@"D:\BluePrint\BluePrint.root\BluePrint\BluePrint.Entity.Constructor\bin\Debug\ExceptionMessenger\Exception.html", message);
                System.Diagnostics.Process.Start(@"D:\BluePrint\BluePrint.root\BluePrint\BluePrint.Entity.Constructor\bin\Debug\ExceptionMessenger\Exception.html");
            }

            return exceptions.Count == 0;
        }

        public static bool Save<T>(IEntity entity) where T : IEntity, new()
        {
            //return Save<T>(entity, false);
            //bool ret = true;
            try
            {
                bool isNew = false;
                entity.__Elements.Where(ebd => ebd.SqlDbIsPrimaryKey).ToList().ForEach(ebd =>
                {
                    if ((ebd.TypeName.Equals("int") && int.Parse(ebd.Data.ToString()) == 0) || (ebd.TypeName.Equals("Guid") && Guid.Parse(ebd.Data.ToString()).Equals(Guid.Empty)))
                        isNew = true;
                });

                if (isNew)
                    CRUDActions.Create<T>(entity);
                else
                    CRUDActions.Update<T>(entity);

                ProcessExceptions(entity);
                //if (entity.Exceptions.Count > 0)
                //{
                //    string message = entity.Exceptions[0].Print(entity);
                //    File.WriteAllText(@"D:\BluePrint\BluePrint.root\BluePrint\BluePrint.Entity.Constructor\bin\Debug\ExceptionMessenger\Exception.html", message);
                //    System.Diagnostics.Process.Start(@"D:\BluePrint\BluePrint.root\BluePrint\BluePrint.Entity.Constructor\bin\Debug\ExceptionMessenger\Exception.html");
                //}

                return true;
            }
            catch (System.Exception ex)
            {
                entity.Exceptions.Add(new EntityException(1, ex.Message, ex));
                return false;
            }
        }

        /*public static bool Save<T>(IEntity entity, bool bypassConflicts) where T : IEntity, new()
        {
            bool ret = true;
            try
            {
                bool isNew = false;
                entity.__Elements.Where(ebd => ebd.UseForMatching).ToList().ForEach(ebd =>
                {
                    if ((ebd.TypeName.Equals("int") && int.Parse(ebd.Data.ToString()) == 0) || (ebd.TypeName.Equals("Guid") && Guid.Parse(ebd.Data.ToString()).Equals(Guid.Empty)))
                        isNew = true;
                });

                if (isNew)
                {
                    CRUDActions.Create<T>(entity);
                    return true;
                }

                IEntity copy = new T();
                copy.ConnectionString = entity.ConnectionString;
                entity.__Elements.Where(ebd => ebd.UseForMatching).ToList().ForEach(ebd => copy.__Elements.Find(c => c.Name.Equals(ebd.Name)).Data = ebd.Data);

                CRUDActions.Retrieve<T>(copy);

                if (copy.Notifications.Count == 0)
                {
                    if (entity.__Memento.Count == 0)
                    {
                        long fieldsToSave = entity.Compare(copy);
                        CRUDActions.Merge<T>(entity, fieldsToSave);
                    }
                    else
                    {
                        long fieldsToSave = entity.Compare();
                        if (fieldsToSave == 0)
                            return false;

                        SetWarnings(entity, copy);

                        if (!bypassConflicts)
                        {
                            SetConflicts(entity, copy);
                            ret = entity.CRUDConflict.Fields.Count == 0;
                        }

                        if (ret)
                        {
                            entity.__Elements.ForEach(e => UpdateField(e, copy, fieldsToSave));
                            CRUDActions.Merge<T>(copy, fieldsToSave);

                            entity.__Elements.Where(e => !e.IsCollection).ToList().ForEach(e => SaveCollection(e, copy));
                        }
                    }
                }
                else
                {
                    CRUDActions.Create<T>(entity);
                }
            }
            catch (System.Exception ex)
            {
                entity.Exceptions.Add(new EntityException(1, ex.Message, ex));
            }
            return ret;
        }*/

        public static void Delete(IEntity entity)
        {
            CRUDActions.Delete(entity);

            if (entity.Exceptions.Count > 0)
            {
                string message = entity.Exceptions[0].Print(entity);
                File.WriteAllText(@"D:\BluePrint\BluePrint.root\BluePrint\BluePrint.Entity.Constructor\bin\Debug\ExceptionMessenger\Exception.html", message);
                System.Diagnostics.Process.Start(@"D:\BluePrint\BluePrint.root\BluePrint\BluePrint.Entity.Constructor\bin\Debug\ExceptionMessenger\Exception.html");
            }

            return;
        }

        public static bool Merge<T>(IEntity entity) where T : IEntity, new()
        {
            return Merge<T>(entity, false);
        }

        public static bool Merge<T>(IEntity entity, bool bypassConflicts = false) where T : IEntity, new()
        {
            bool ret = true;
            try
            {
                bool isNew = true;
                foreach (ElementBaseData ebd in entity.__Elements.Where(ebd => ebd.SqlDbIsPrimaryKey).ToList())
                {
                    if ((ebd.TypeName.Equals("int") && int.Parse(ebd.Data.ToString()) != 0) || (ebd.TypeName.Equals("Guid") && !Guid.Parse(ebd.Data.ToString()).Equals(Guid.Empty)))
                    {
                        isNew = false;
                        break;
                    }
                }

                if (isNew)
                {
                    CRUDActions.Create<T>(entity);
                    return true;
                }

                IEntity copy = new T();
                copy.ConnectionString = entity.ConnectionString;
                entity.__Elements.Where(ebd => ebd.SqlDbIsPrimaryKey).ToList().ForEach(ebd => copy.__Elements.Find(c => c.Name.Equals(ebd.Name)).Data = ebd.Data);

                CRUDActions.Retrieve<T>(copy);

                if (copy.Notifications.Count == 0)
                {
                    if (entity.__Memento.Count == 0)
                    {
                        long fieldsToSave = entity.Compare(copy);
                        CRUDActions.Merge<T>(entity, fieldsToSave);
                    }
                    else
                    {
                        long fieldsToSave = entity.Compare();
                        if (fieldsToSave == 0)
                            return false;

                        SetWarnings(entity, copy);

                        if (!bypassConflicts)
                        {
                            SetConflicts(entity, copy);
                            ret = entity.CRUDConflict.Fields.Count == 0;
                        }

                        if (ret)
                        {
                            entity.__Elements.ForEach(e => UpdateField(e, copy, fieldsToSave));
                            CRUDActions.Merge<T>(copy, fieldsToSave);

                            entity.__Elements.Where(e => !e.IsCollection).ToList().ForEach(e => SaveCollection(e, copy));
                        }
                    }
                }
                else
                {
                    CRUDActions.Create<T>(entity);
                }
            }
            catch (System.Exception ex)
            {
                entity.Exceptions.Add(new EntityException(1, ex.Message, ex));
            }
            return ret;
        }

        private static void SaveCollection(ElementBaseData ebd, IEntity copy)
        {
            //(ebdData as List<T>)
        }

        private static void UpdateField(IBaseData e, IEntity copy, long fieldsToSave)
        {
            long bw = e.Bitwise ?? default(long);

            if ((fieldsToSave & bw) == bw)
            {
                copy.__Elements.Find(c => c.ID == e.ID).Data = e.Data.ToString();
            }
        }

        private static bool SetWarnings(IEntity entity, IEntity copy)
        {
            bool ret = false;

            entity.Notifications.RemoveAll(n => n.Severity == Notification.NoticeType.Warning);

            long fieldsThatHaveChanged = copy.Compare(entity.__Memento);
            if (fieldsThatHaveChanged > 0)
            {
                long fieldsToSave = entity.Compare();
                long otherFieldsChanged = Math.Abs(fieldsThatHaveChanged - (fieldsThatHaveChanged & fieldsToSave));
                if (otherFieldsChanged > 0)
                {
                    entity.__Elements.ForEach(e => GetOtherFieldUpdates(entity, e, otherFieldsChanged));
                    ret = true;
                }
            }

            return ret;
        }

        private static bool SetConflicts(IEntity entity, IEntity copy)
        {
            bool ret = false;

            entity.CRUDConflict.Clear();

            long fieldsToSave = entity.Compare();
            if (fieldsToSave > 0)
            {
                long fieldsThatHaveChanged = copy.Compare(entity.__Memento);
                long otherFieldsChanged = Math.Abs(fieldsToSave - (fieldsToSave & fieldsThatHaveChanged));
                if ((fieldsThatHaveChanged & fieldsToSave) > 0)
                {
                    List<string> conflictMessages = new List<string>();
                    conflictMessages.Add("Other users have altered some of the same data during your session.");
                    long conflictedFields = fieldsThatHaveChanged & fieldsToSave;
                    List<int> fields = new List<int>();
                    long bw = 1;
                    for (int i = 0; i < 64; i++)
                    {
                        if ((conflictedFields & bw) == bw)
                        {
                            int elementId = i;
                            fields.Add(elementId);
                            ElementBaseData element = entity.__Elements[i];
                            conflictMessages.Add(String.Format("Field '{0}' has been changed from '{1}' to '{2}' by another user.", element.Name, entity.__Memento.Find(m => m.ElementId == elementId).Data.ToString(), copy.__Elements[elementId].Data.ToString()));
                        }
                        bw = bw + bw;
                    }

                    List<CRUDOption> options = new List<CRUDOption>();
                    options.Add(new CRUDOption()
                    {
                        Message = "Save all my changes",
                        OptionType = CRUDOption.CRUDOptionType.SaveAll
                    });
                    if (otherFieldsChanged > 0)
                    {
                        options.Add(new CRUDOption()
                        {
                            Message = "Save my changes that don't conflict",
                            OptionType = CRUDOption.CRUDOptionType.SaveNonConflicts
                        });
                    }
                    options.Add(new CRUDOption()
                    {
                        Message = "Cancel Save",
                        OptionType = CRUDOption.CRUDOptionType.Cancel
                    });

                    entity.CRUDConflict.Fields = fields;
                    entity.CRUDConflict.Messages = conflictMessages;
                    entity.CRUDConflict.Options = options;
                }
            }

            return ret;
        }

        private static void GetOtherFieldUpdates(IEntity entity, IBaseData element, long otherFieldsChanged)
        {
            if ((otherFieldsChanged & element.Bitwise) == element.Bitwise)
            {
                Notification notification = new Notification()
                {
                    ElementId = element.ID ?? default(int),
                    Severity = Notification.NoticeType.Warning,
                    Message = String.Format("This field was changed from {0}, by another user.", element.Data.ToString())
                };

                entity.Notifications.Add(notification);
            }
        }

    }
}
