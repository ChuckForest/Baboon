using ReadyEDI.EntityFactory.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory.DataLayer
{
    public static class Utility
    {
        public static bool SetWarnings(IEntity entity, IEntity copy)
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

        public static bool SetConflicts(IEntity entity, IEntity copy)
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

        /*public static void SetNotificationsAndConflicts(IEntity entity, IEntity copy)
        {
            long fieldsToSave = entity.Compare();
            long fieldsThatHaveChanged = copy.Compare(entity.Memento);

            if (fieldsToSave == 0 && fieldsThatHaveChanged == 0)
                return;

            if (fieldsThatHaveChanged > 0)
            {
                long otherFieldsChanged = Math.Abs(fieldsThatHaveChanged - (fieldsThatHaveChanged & fieldsToSave));
                if (otherFieldsChanged > 0)
                {
                    entity.Elements.ForEach(e => GetOtherFieldUpdates(entity, e, otherFieldsChanged));
                }

                if ((fieldsThatHaveChanged & fieldsToSave) > 0)
                {
                    List<CRUDOption> options = new List<CRUDOption>();
                    options.Add(new CRUDOption()
                    {
                        Message = "Save all my changes",
                        ActionType = CRUDOption.CRUDOptionActionType.SaveAll
                    });
                    if (otherFieldsChanged > 0)
                    {
                        options.Add(new CRUDOption()
                        {
                            Message = "Save my changes that don't conflict",
                            ActionType = CRUDOption.CRUDOptionActionType.SaveNonConflicts
                        });
                    }
                    options.Add(new CRUDOption()
                    {
                        Message = "Cancel Save",
                        ActionType = CRUDOption.CRUDOptionActionType.Cancel
                    });

                    CRUDConflict ex = new CRUDConflict()
                    {
                        Message = "Another user has altered some of the same data during your session.",
                        Options = options
                    };

                    entity.CRUDConflicts.Add(ex);
                }
            }
        }*/

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

        public static string GetQueuePath(IEntity entity)
        {
            return String.Format(@".\private$\MementoQueue_{0}_{1}", entity.EntityName, String.Join("_", entity.GetMatchDataValues()));
        }

    }
}
