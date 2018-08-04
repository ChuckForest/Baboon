using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory.DataLayer
{
    public static class Persist
    {
        public static void Edit<T>(IEntity entity) where T : IEntity, new()
        {
            string queuePath = Utility.GetQueuePath(entity);
            QueueManager.CreateQueue(queuePath);
            QueueManager.SendMessage(queuePath, entity);
        }

        public static bool Save<T>(IEntity entity, bool bypass) where T : IEntity, new()
        {
            bool ret = CRUDFunctions.Save<T>(entity, bypass);
            if (ret)
                entity.Save<T>();

            return ret;
        }

        public static IEntity Get<T>(string queuePath) where T : IEntity, new()
        {
            return QueueManager.Rebuild<T>(queuePath) as IEntity;
        }

    }
}
