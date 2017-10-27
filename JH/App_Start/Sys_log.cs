using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace log
{
    public enum DbLogType
    {
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4
    }
    /// <summary>
    /// 系统日志类
    /// </summary>
    public sealed class Sys_log
    {
        public static void Debug(string message)
        {
            WriteLog(DbLogType.Debug, message,"");
        }
        public static void Info(string message)
        {
            WriteLog(DbLogType.Info, message,"");
        }
        public static void Warn(string message)
        {
            WriteLog(DbLogType.Warn, message,"");
        }
        public static void Error(string message)
        {
            WriteLog(DbLogType.Error, message,"");
        }
        public static void Error(string message, string ViewID)
        {
            WriteLog(DbLogType.Error, message, ViewID);
        }
        /// <summary>
        /// 日志写入到数据库
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="sqlText"></param>
        static void WriteLog(DbLogType logType, string message,string ViewID)
        {
            Model.JH_SYS_LOG m = new Model.JH_SYS_LOG() {
                OPERATIONTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                LOGCONTENT = message,
                LOGIP = Common.Utility.GetIP(),
                LOGID = Common.Utility.GetMd5HashCode(Common.Utility.GetDataRandom()),
                LOGTYPE = logType.ToString(),
                USERID = SetCookie.USER_ID.ToString(),
                VIEWID = ViewID 
            };
            Repository.BaseBll<Model.JH_SYS_LOG>.Add(m);
        }
    }
}