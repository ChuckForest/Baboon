using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ReadyEDI.EntityFactory.DataLayer
{
    public static class QueueManager
    {
        public static void CreateQueue(string queuePath)
        {
            //try
            //{
                if (!MessageQueue.Exists(queuePath))
                    MessageQueue.Create(queuePath);
            //}
            //catch (MessageQueueException e)
            //{

            //}

        }

        private static void logException(System.Exception ex)
        {

        }

        public static void DeleteQueue(string queuePath)
        {
            MessageQueue.Delete(queuePath);
        }

        public static IEntity GetMemento<T>(string queuePath) where T : IEntity, new()
        {
            IEntity memento = new T();

            MessageQueue myQueue = new MessageQueue(queuePath);
            myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(DataProxyCollection) });
            Message message = myQueue.Peek();
            memento.Load(message.Body as DataProxyCollection);

            return memento;
        }

        public static IEntity Rebuild<T>(string queuePath) where T : IEntity, new()
        {
            IEntity memento = new T();

            MessageQueue myQueue = new MessageQueue(queuePath, QueueAccessMode.Peek);
            myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(DataProxyCollection) });

            List<Message> messages = myQueue.GetAllMessages().ToList();
            try
            {
                memento.Load(messages[0].Body as DataProxyCollection);
            }
            catch (System.Exception ex)
            {
                
                logException(ex);
            }

            messages.RemoveAt(0);

            try
            {
                messages.ForEach(m => (m.Body as DataProxyCollection).ForEach(d => memento.UpdateFieldValue(d)));
            }
            catch (System.Exception ex)
            {
                QueueManager.logException(ex);
            }

            return memento;
        }

        public static void SendMessage(string queuePath, IEntity message)
        {
            CreateQueue(queuePath);
            MessageQueue myQueue = new MessageQueue(queuePath);
            myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(DataProxyCollection) });
            // Create a new message.
            //Message myMessage = new Message("Original Message");

            //myMessage.AdministrationQueue = new MessageQueue(".\\myAdministrationQueue");
            //myMessage.AcknowledgeType = AcknowledgeTypes.PositiveReceive | AcknowledgeTypes.PositiveArrival;

            // Send the Order to the queue.
            try
            {
                if (message.QueueId.Equals(Guid.Empty))
                {
                    myQueue.Send(message.GetData());
                    message.QueueId = myQueue.Id;
                }
                else
                {
                    Message queue = myQueue.PeekById(message.QueueId.ToString());
                    queue.Body = message.GetData();
                }
            }
            catch (System.Exception ex)
            {
                logException(ex);
            }

            return;
        }

        public static void SendMessage(string queuePath, DataProxy message)
        {
            CreateQueue(queuePath);
            MessageQueue myQueue = new MessageQueue(queuePath);
            myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(DataProxyCollection) });
            // Create a new message.
            //Message myMessage = new Message("Original Message");

            //myMessage.AdministrationQueue = new MessageQueue(".\\myAdministrationQueue");
            //myMessage.AcknowledgeType = AcknowledgeTypes.PositiveReceive | AcknowledgeTypes.PositiveArrival;

            // Send the Order to the queue.
            //KeyValuePair<int, object> kvp = new KeyValuePair<int, object>(elementId, value);
            DataProxyCollection proxyList = new DataProxyCollection();
            proxyList.Add(message);
            myQueue.Send(proxyList);

            return;
        }

        public static List<DataProxy> PeekQueue(string queuePath)
        {
            if (!MessageQueue.Exists(queuePath))
                return null;

            MessageQueue myQueue = new MessageQueue(queuePath);
            myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(DataProxyCollection) });

            return myQueue.Peek().Body as DataProxyCollection;
        }

        public static IEntity Restore<T>(string queuePath, IEntity copy) where T : IEntity, new()
        {
            if (!MessageQueue.Exists(queuePath))
                return copy;

            MessageQueue myQueue = new MessageQueue(queuePath);
            myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(DataProxyCollection) });

            IEntity entity = new T();
            entity.Load(myQueue.Receive().Body as DataProxyCollection);

            //IEntity entity = (T)memento.Clone();
            myQueue.GetAllMessages().ToList().ForEach(m => entity.UpdateFieldValue(m.Body as DataProxy));
            DeleteQueue(queuePath);

            //Utility.SetNotificationsAndConflicts(entity, copy);

            return entity;
        }

        public static int QueueLength(string queuePath)
        {
            if (!MessageQueue.Exists(queuePath))
                return -1;

            //PerformanceCounter queueCounter = new PerformanceCounter(
            //    "MSMQ Queue",
            //    "Messages in Queue",
            //    queuePath);

            //return int.Parse(queueCounter.NextValue().ToString());

            MessageQueue queue = new MessageQueue(queuePath);
            return queue.GetAllMessages().Count();
        }
        /*private static void logException(Exception ex)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("IsNLogEnabled") && Boolean.Parse(ConfigurationManager.AppSettings.Get("IsNLogEnabled")) == true)
            {
                getNLogger().ErrorException(ex.Message, ex);
            }
        }
        private static NLog.Logger nLogger;
        private static NLog.Logger getNLogger()
        {
            if (nLogger == null)
            {
                nLogger = NLog.LogManager.GetLogger("CRUDActions");
            }
            return nLogger;
        }*/
    }
}
