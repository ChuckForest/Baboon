using ReadyEDI.EntityFactory.Elements;
using ReadyEDI.EntityFactory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace ReadyEDI.EntityFactory.DataAccess
{
    public static partial class CRUDActions
    {
        //private static NLog.Logger nLogger;
        //private static NLog.Logger getLogger()
        //{
        //    if (nLogger == null)
        //    {
        //        nLogger = NLog.LogManager.GetLogger("CRUDActions");
        //    }
        //    return nLogger;
        //}

        private static void ProcessNotification(IEntity entity, System.Exception ex, Notification.NoticeType severity)
        {
            Notification notification = new Notification() { Severity = severity, Message = ex.Message };
            //CRUDActions.getLogger().Info(notification.ToString());
            entity.Notifications.Add(notification);
        }

        private static void ProcessNotification<T>(Collection<T> entities, System.Exception ex, Notification.NoticeType severity) where T : IEntity
        {
            Notification notification = new Notification() { Severity = severity, Message = ex.Message };
            //CRUDActions.getLogger().Info(notification.ToString());
            //CRUDActions.getLogger().Info(ex);
            entities.Notifications.Add(notification);
        }

        private static void ProcessException(EntityExemptionType exType, IEntity entity, System.Exception ex)
        {
            EntityException entityEx = new EntityException((int)exType, String.Format("{0} generated a system failure.", entity.GetType()), ex);
            List<string> variables = new List<string>();
            entity.Exceptions.Add(entityEx);
            //logException(entityEx.Print(entity), ex);
        }

        private static void ProcessException<T>(EntityExemptionType exType, Collection<T> entities, System.Exception ex) where T : IEntity
        {
            EntityException entityEx = new EntityException((int)exType, String.Format("{0} generated a system failure.", entities.GetType()), ex);
            List<string> variables = new List<string>();
            entities.Exceptions.Add(entityEx);
            //logException("", ex);
        }

        private static void logException(string html, Exception ex)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("ExceptionFilePath"))
            {
                WriteToFile(html, ConfigurationManager.AppSettings.Get("ExceptionFilePath"));
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("ExceptionQueuePath"))
            {
                WriteToQueue(html, ConfigurationManager.AppSettings.Get("ExceptionQueuePath"));
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("IsNLogEnabled") && Boolean.Parse(ConfigurationManager.AppSettings.Get("IsNLogEnabled")) == true)
            {
                //getLogger().ErrorException(ex.Message, ex);
                //logCallStack(new StackTrace(), getLogger());
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("IsLog4NetEnabled") && Boolean.Parse(ConfigurationManager.AppSettings.Get("IsLog4NetEnabled")) == true)
            {
                //getLogger().Error(ex.Message, ex);
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("ExceptionEmail"))
            {
                //TODO variables.Add(html.ToString());
                //TOTO Utilities.Notification.Send(new Utilities.Entities.Email() { EmailType = (int)Utilities.Notification.EmailType.SystemFailure, To = ConfigurationManager.AppSettings.Get("ExceptionQueuePath"), Variables = variables });
            }

        }

        private static void WriteToQueue(string html, string path)
        {
            MessageQueue myQueue = new MessageQueue(path);
            myQueue.Send(html);
        }

        private static void WriteToFile(string html, string path)
        {
            try
            { 
                using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(String.Format(@"{0}\ex_{1}.html", path, DateTime.Now.ToString("yyyy-MM-dd-HH-mm"))))
                {
                    outfile.Write(html);
                }
            } 
            catch (System.Exception ex)
            {
                ex.Data.Add("0", "lets eat this exception here");
            }
        }

        private static void logActions(SqlCommand cmd)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("IsNLogEnabled") && Boolean.Parse(ConfigurationManager.AppSettings.Get("IsNLogEnabled")) == true)
            {
                //#if DEBUG
                //logSqlCall(cmd);
                //#endif
                //CRUDActions.getLogger().Info("------------------------------------------------------------------------------------");
                //CRUDActions.getLogger().Info(cmd.Connection.ConnectionString);
                //CRUDActions.getLogger().Info(cmd.CommandText);
                int index = 0;
                //cmd.Parameters.Cast<SqlParameter>().ToList().ForEach((p) => CRUDActions.getLogger().Info(String.Format("{0} @{1} = '{2}' -- Direction:{3} SqlDbType:{4} Size:{5}"
                //    , ++index
                //    , p.ParameterName ?? "null"
                //    , p.Value ?? "null"
                //    , p.Direction
                //    , p.SqlDbType
                //    , p.Size
                //    )));
                //CRUDActions.getLogger().Info("------------------------------------------------------------------------------------");
            }
        }

        private static void logActions(String message)
        {
            //if (ConfigurationManager.AppSettings.AllKeys.Contains("IsNLogEnabled") && Boolean.Parse(ConfigurationManager.AppSettings.Get("IsNLogEnabled")) == true)
            //    CRUDActions.getLogger().Info(message);
        }

        //private static void logCallStack(StackTrace stackTrace, NLog.Logger logger)
        //{
        //    //#if DEBUG
        //    StackFrame[] stackFrames = stackTrace.GetFrames();
        //    foreach (StackFrame frame in stackFrames)
        //    {
        //        logger.Info(String.Format("Call Stack {0} {1} {2}", frame.GetType(), frame.GetMethod().Name, frame.GetFileLineNumber().ToString()));
        //    }
        //    //#endif
        //}

        //private static void logSqlCall(SqlCommand cmd)
        //{
        //    if (!CRUDActions.getLogger().IsTraceEnabled) return;

        //    StringBuilder sb0 = new StringBuilder("-------------- SQL Script to call " + cmd.CommandText + " Start--------------------------\n");
        //    StringBuilder sb1 = new StringBuilder();
        //    StringBuilder sb2 = new StringBuilder();

        //    sb1.AppendFormat("exec {0} ", cmd.CommandText);
        //    sb2.Append("Select ");
        //    CRUDActions.getLogger().Trace("-------------- SQL Script to call " + cmd.CommandText + " Start--------------------------");
        //    try
        //    {
        //        cmd.Parameters.Cast<SqlParameter>().ToList().Where(param => (param.Direction == ParameterDirection.Input)).ToList<SqlParameter>().ForEach(param =>
        //        {
        //            sb0.AppendFormat(
        //                String.Format("declare @{0} {1} {2} = {3} -- Input \n"
        //                    , param.ParameterName, param.SqlDbType
        //                    , param.Size > 0 ? string.Format("({0}{1})", param.Size, param.Precision > 0 ? string.Format(",{0}", param.Precision) : "") : ""
        //                    , (param.SqlDbType.ToString().Contains("Binary") | param.SqlDbType.ToString().Contains("Char") | param.SqlDbType.ToString().Contains("Date")
        //                        | param.SqlDbType.ToString().Contains("Image") | param.SqlDbType.ToString().Contains("Date") | param.SqlDbType.ToString().Contains("Text")
        //                        | param.SqlDbType.ToString().Contains("Time") | param.SqlDbType.ToString().Contains("UniqueIdentifier") | param.SqlDbType.ToString().Contains("Xml")
        //                        ) ? string.Format("'{0}'", param.SqlValue) : param.SqlValue
        //                )
        //            );

        //            sb1.AppendFormat(" @{0} = {1}, ", param.ParameterName, (param.SqlDbType.ToString().Contains("Binary") | param.SqlDbType.ToString().Contains("Char") | param.SqlDbType.ToString().Contains("Date")
        //                  | param.SqlDbType.ToString().Contains("Image") | param.SqlDbType.ToString().Contains("Date") | param.SqlDbType.ToString().Contains("Text")
        //                  | param.SqlDbType.ToString().Contains("Time") | param.SqlDbType.ToString().Contains("UniqueIdentifier") | param.SqlDbType.ToString().Contains("Xml")
        //                  ) ? string.Format("'{0}'", param.SqlValue) : param.SqlValue
        //            );
        //        }
        //        );
        //        int index = 0;

        //        cmd.Parameters.Cast<SqlParameter>().ToList().Where(param => param.Direction == ParameterDirection.InputOutput).ToList<SqlParameter>().ForEach(param => GetParameterFormated(param, ++index, sb0, sb1, sb2));
        //        cmd.Parameters.Cast<SqlParameter>().ToList().Where(param => param.Direction == ParameterDirection.Output).ToList<SqlParameter>().ForEach(param => GetParameterFormated(param, ++index, sb0, sb1, sb2));
        //        cmd.Parameters.Cast<SqlParameter>().ToList().Where(param => param.Direction == ParameterDirection.ReturnValue).ToList<SqlParameter>().ForEach(param => GetParameterFormated(param, ++index, sb0, sb1, sb2));
        //    }
        //    catch (Exception ex)
        //    {
        //        ex = null;
        //        ex.Data.Add("0", "lets eat this exception here");
        //    }

        //    sb0.AppendFormat(sb1.ToString().Substring(0, sb1.Length - 2));
        //    sb0.Append("\n");
        //    sb0.AppendFormat(sb2.ToString().Substring(0, sb2.Length - 2));
        //    CRUDActions.getLogger().Trace(sb0.ToString());
        //    CRUDActions.getLogger().Trace("-------------- SQL CAll Script end--------------------------");
        //    sb0 = sb1 = sb2 = null;
        //}

        private static void GetParameterFormated(SqlParameter param, int index, StringBuilder sb0, StringBuilder sb1, StringBuilder sb2)
        {
            //(param.SqlValue.Equals(null) ? "0" : param.SqlValue
            sb0.AppendFormat(
                String.Format("declare @p{0} {1} {2} = {3} -- {4} {5} \n"
                , ++index, param.SqlDbType
                , param.Size > 0 ? string.Format("({0}{1})", param.Size, param.Precision > 0 ? string.Format(",{0}", param.Precision) : "") : ""
                , (param.SqlDbType.ToString().Contains("Binary") | param.SqlDbType.ToString().Contains("Char") | param.SqlDbType.ToString().Contains("Date")
                    | param.SqlDbType.ToString().Contains("Image") | param.SqlDbType.ToString().Contains("Date") | param.SqlDbType.ToString().Contains("Text")
                    | param.SqlDbType.ToString().Contains("Time") | param.SqlDbType.ToString().Contains("UniqueIdentifier") | param.SqlDbType.ToString().Contains("Xml")
                  ) ? string.Format("'{0}'", param.SqlValue) : (param.SqlValue == null ? "0" : param.SqlValue)
                , param.ParameterName
                , param.Direction.ToString()
                )
            );
            sb1.AppendFormat(" @{0} = @p{1} output, ", param.ParameterName, index);
            sb2.AppendFormat(" @p{0}, ", index);

        }
    }
}
