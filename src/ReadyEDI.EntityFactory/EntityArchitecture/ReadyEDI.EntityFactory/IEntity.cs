using ReadyEDI.EntityFactory.Elements;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory
{
    public interface IEntity
    {
        string __EntityName { get; set; }
        string EntityName { get; set; }
        List<ElementBaseData> __Elements { get; set; }
        long CurrentAction { get; set; }
        //string ActionName { get; set; }
        List<Action> __Actions { get; set; }
        List<Notification> Notifications { get; set; }
        CRUDConflict CRUDConflict { get; set; }
        //int Id { get; set; }
        //bool IsDirty { get; set; }
        DataProxyCollection __Memento { get; set; }
        List<EntityException> Exceptions { get; set; }
        string ConnectionString { get; set; }
        Guid QueueId { get; set; }
        long LazyLoad { get; set; }
        bool Populated { get; set; }
        Dictionary<string, string> Parameters { get; set; }
        string SchemaName { get; set; }
        string DBName { get; set; }
        SqlConnection SqlConnection { get; set; }
        SqlTransaction SqlTransaction { get; set; }
        List<ElementBaseData> AttachedCollections { get; set; }
        bool MarkForDeletion { get; set; }

        string GetDisplayName(string FieldName);

        void LoadFormData(System.Collections.Specialized.NameValueCollection nvc);

        object Clone();
        long Compare();
        long Compare(DataProxyCollection data);
        long Compare(IEntity to);
        string GetActionName();
        DataProxyCollection GetData();
        List<string> GetMatchDataValues();
        void Load();
        void Load(DataProxyCollection data);
        void UpdateFieldValue(DataProxy proxy);
        void Save<T>() where T : IEntity;
        //void SaveChanges();
        void RefreshCollections();
        string JsonStringify();
        void LoadFromJson(string jsonData);
        bool IsDirty();
        //bool Save(bool bypassConflicts);
    }
}
