using ReadyEDI.EntityFactory.Elements;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReadyEDI.EntityFactory
{
    public class Transaction
    {
        private List<TransactionStep> _steps = new List<TransactionStep>();
        private string _connectionString = String.Empty;
        List<EntityException> _exceptions = new List<EntityException>();
        List<Notification> _notifications = new List<Notification>();
        List<Tuple<IEntity, DataProxy, object>> memento = new List<Tuple<IEntity, DataProxy, object>>();

        public Transaction(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<TransactionStep> Steps
        {
            get { return _steps; }
            set { _steps = value; }
        }

        public List<EntityException> Exceptions
        {
            get { return _exceptions; }
            set { _exceptions = value; }
        }

        public List<Notification> Notifications
        {
            get { return _notifications; }
            set { _notifications = value; }
        }

        public void Run()
        {
            _exceptions.Clear();
            _notifications.Clear();
            Process();
        }

        private void Process()
	    {
            using (SqlConnection conn = new SqlConnection(_connectionString))
		    {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
              
                _steps.ForEach(s => ProcessStep(conn, tran, s));

                if (_exceptions.Count == 0)
                    tran.Commit();
                else
                    tran.Rollback();

                conn.Close();
		    }
	    }

        private void ProcessPropertyChange(IEntity entity, TransactionStep step)
        {
            ElementBaseData ebd = entity.__Elements.Find(e => e.Name.Equals(step.MemberName));
            
            if (ebd != null)
            {
                if (step.PropertyValueEntity != null)
                {
                    ElementBaseData ebdpv = step.PropertyValueEntity.__Elements.Find(e => e.Name.Equals(step.PropertyValueElementName));
                    if (ebd != null)
                    {
                        memento.Add(new Tuple<IEntity, DataProxy, object>(entity, new DataProxy((int)ebd.ID, ebd.Data), ebdpv.Data));
                        ebd.Data = ebdpv.Data;
                    }
                    else
                    {
                        //set notification
                    }
                }
                else
                {
                    memento.Add(new Tuple<IEntity, DataProxy, object>(entity, new DataProxy((int)ebd.ID, ebd.Data), step.PropertyValue));
                    ebd.Data = step.PropertyValue;
                }
            }
            else
            {
                //set notification
            }
        }

        private void ProcessMethodCall(IEntity entity, TransactionStep step, SqlConnection conn, SqlTransaction tran)
        {
            Type type = entity.GetType();
            //ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            //object classObject = constructor.Invoke(new object[] { });
            PropertyInfo propConn = type.GetProperty("SqlConnection", BindingFlags.Public | BindingFlags.Instance);
            propConn.SetValue(entity, conn, null);
            PropertyInfo propTran = type.GetProperty("SqlTransaction", BindingFlags.Public | BindingFlags.Instance);
            propTran.SetValue(entity, tran, null);
            MethodInfo method = type.GetMethod(step.MemberName);
            if (method != null)
            {
                method.Invoke(entity, new object[] { });
            }
            else
            {
                //set notification
            }
            propConn.SetValue(entity, null, null);
            propTran.SetValue(entity, null, null);
            PropertyInfo exceptions = type.GetProperty("Exceptions", BindingFlags.Public | BindingFlags.Instance);
            _exceptions.AddRange(exceptions.GetValue(entity, null) as List<EntityException>);
            PropertyInfo notifications = type.GetProperty("Notifications", BindingFlags.Public | BindingFlags.Instance);
            _notifications.AddRange(notifications.GetValue(entity, null) as List<Notification>);
        }

        private void ProcessStep(SqlConnection conn, SqlTransaction tran, TransactionStep step)
        {
            if (step.Collection == null && step.Entity == null)
            {
                //set notification
                return;
            }

            switch (step.Type)
            {
                case StepType.PropertyChange:
                    if (step.Collection == null)
                        ProcessPropertyChange(step.Entity, step);
                    else
                        step.Collection.ToList().ForEach(e => ProcessPropertyChange(e, step));
                    break;
                case StepType.MethodCall:
                    if (step.Collection == null)
                        ProcessMethodCall(step.Entity, step, conn, tran);
                    else
                        step.Collection.ToList().ForEach(e => ProcessMethodCall(e, step, conn, tran));
                    break;
            }

            return;
        }

    }
}
