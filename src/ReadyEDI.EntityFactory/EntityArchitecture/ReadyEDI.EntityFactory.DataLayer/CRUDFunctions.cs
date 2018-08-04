using ReadyEDI.EntityFactory.DataAccess;
using ReadyEDI.EntityFactory.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory.DataLayer
{
    public static class CRUDFunctions
    {
        public static void Merge<T>(IEntity entity, bool bypassConflicts = false) where T : IEntity
        {

        }

        public static void Load<T>(IEntity entity) where T : IEntity
        {
            //getNLogger().Info(String.Format("Calling CRUDActions.Retrieve<T>({0})", entity.EntityName));
            entity.Populated = true;
            CRUDActions.Retrieve<T>(entity);

            for (int i=0;i<entity.__Elements.Count(); i++)
            {
                entity.__Memento.Add(new DataProxy() { ElementId = i, Data = entity.__Elements[i].Data });
            }
        }

        public static void Load<T>(Collection<T> entities) where T : IEntity
        {
            //getNLogger().Info(String.Format("Calling CRUDActions.Retrieve<T>({0})", entities.GetType()));
            CRUDActions.Retrieve<T>(entities);
        }

        public static bool Save<T>(IEntity entity) where T : IEntity, new()
        {
            //getNLogger().Info(String.Format("Calling Save<T>({0}, false)", entity.GetType()));
            //getNLogger().Info("Calling Save<T>(entity, false)");
            return Save<T>(entity, false);
        }

        public static bool Save<T>(IEntity entity, bool bypassConflicts) where T : IEntity, new()
        {
            bool ret = true;
            try
            {
                IEntity copy = new T();
                copy.ConnectionString = entity.ConnectionString;
                entity.__Elements.Where(ebd => ebd.UseForMatching).ToList().ForEach(ebd => copy.__Elements.Find(c => c.Name.Equals(ebd.Name)).Data = ebd.Data);
                //copy.Elements[0].Data = entity.Elements[0].Data;

                CRUDActions.Retrieve<T>(copy);

                if (copy.Notifications.Count == 0)
                {
                    if (entity.__Memento.Count == 0)
                    {
                        long fieldsToSave = entity.Compare(copy);
                        CRUDActions.Update<T>(entity, fieldsToSave);
                    }
                    else
                    {
                        long fieldsToSave = entity.Compare();
                        if (fieldsToSave == 0)
                            return false;

                        Utility.SetWarnings(entity, copy);

                        if (!bypassConflicts)
                        {
                            Utility.SetConflicts(entity, copy);
                            ret = entity.CRUDConflict.Fields.Count == 0;
                        }

                        if (ret)
                        {
                            entity.__Elements.ForEach(e => UpdateField(e, copy, fieldsToSave));
                            CRUDActions.Update<T>(copy, fieldsToSave);

                            entity.__Elements.Where(e => !e.IsCollection).ToList().ForEach(e => SaveCollection(e, copy));
                        }
                    }
                }
                else
                {
                    //copy.Notifications.ForEach(n => getNLogger().Info(n.Message));
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

        public static int Add<T>(IEntity entity, Collection<T> entities) where T : IEntity
        {
            //getNLogger().Info("Calling Add<T>({0}, {1}, false)", entity.EntityName, entities.GetType());

            return Add<T>(entity, entities, false);
        }

        public static int Add<T>(IEntity entity, Collection<T> entities, bool bypassConflicts) where T : IEntity
        {
            int ret = 0;

            //Collection<T> newEntities = new Collection<T>();
            Collection<T> copies = new Collection<T>();
            Load<T>(copies/*, entity.EntityName*/);
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

                //CRUDConflict ex = new CRUDConflict()
                //{
                //Message = "A matching profile was added by another user during your current session.",
                //    Options = options
                //};
                //entity.CRUDConflicts.Add(ex);
            }
            else
            {
                if (newEntities.Count > 0)
                {
                    entities.AddRange(newEntities);
                    ////ret = newEntities.ToList().Last().Id + 1;
                }
                else
                    ////ret = entities.ToList().Last().Id + 1;

                ////entity.Id = ret;
                //getNLogger().Info(String.Format("Calling CRUDActions.Create({0})", entity.EntityName));
                CRUDActions.Create<T>(entity);
            }

            return ret;
        }

        private static void UpdateField(IBaseData e, IEntity copy, long fieldsToSave)
        {
            long bw = e.Bitwise ?? default(long);

            if ((fieldsToSave & bw) == bw)
            {
                copy.__Elements.Find(c => c.ID == e.ID).Data = e.Data.ToString();
            }
        }

        public static bool Create<T>(IEntity entity) where T : IEntity
        {
            bool ret = true;
            try
            {
                //getNLogger().Info(String.Format("Calling CRUDActions.Create<T>({0})", entity.EntityName));
                CRUDActions.Create<T>(entity);
            }
            catch (SystemException ex)
            {
                entity.Exceptions.Add(new EntityException(1, ex.Message, ex));
                ret = false;
            }
            return ret;
        }

        public static bool Update<T>(IEntity entity, bool bypassConflicts = false) where T : IEntity, new()
        {
            bool ret = true;
            try
            {
                IEntity copy = new T();
                copy.ConnectionString = entity.ConnectionString;
                copy.__Elements[0].Data = entity.__Elements[0].Data;

                //getNLogger().Info(String.Format("Calling CRUDActions.Retrieve<T>({0}) for copy", entity.EntityName));
                CRUDActions.Retrieve<T>(copy);

                if (copy.Notifications.Count == 0)
                {
                    long fieldsToSave = entity.Compare();
                    //getNLogger().Info(String.Format("fieldsToSave {0}", fieldsToSave));
                    if (fieldsToSave == 0)
                        return false;

                    Utility.SetWarnings(entity, copy);

                    if (!bypassConflicts)
                    {
                        Utility.SetConflicts(entity, copy);
                        ret = entity.CRUDConflict.Fields.Count == 0;
                    }

                    if (ret)
                    {
                        entity.__Elements.ForEach(e => UpdateField(e, copy, fieldsToSave));
                        //getNLogger().Info(String.Format("Calling CRUDActions.Update({0}, {1})", entity.EntityName, fieldsToSave.ToString()));
                        CRUDActions.Update<T>(copy, fieldsToSave);
                    }
                }
                else
                {
                    throw new System.Exception("CRUDActions.Retrieve failed during Update");
                }
            }
            catch (System.Exception ex)
            {
                entity.Exceptions.Add(new EntityException(1, ex.Message, ex));
                ret = false;
            }
            return ret;
        }


    }
}
