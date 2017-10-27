using System;
using System.Configuration;
/*
*/

namespace DBUtility
{
    public class DALHelper
    {
        public static DbHelper dbHelper = GetHelper("DB");
        public static DbHelper myHelper = GetHelper("MYSQL");

        /// <summary>
        /// 从Web.config从读取数据库的连接以及数据库类型
        /// </summary>
        public static DbHelper GetHelper(string connectionStringName)
        {
            ConnectionStringSettings connectionSetting = ConfigurationManager.ConnectionStrings[connectionStringName];
            string className = connectionSetting.ProviderName;
            DbHelper db = DbHelper.Create(className);
            string ConStringEncrypt = System.Configuration.ConfigurationManager.AppSettings["ConStringEncrypt"];
            if (ConStringEncrypt == "true")
            {
                db.ConnectionString = DESEncrypt.Decrypt(connectionSetting.ConnectionString);
            }
            else
            {
                db.ConnectionString = connectionSetting.ConnectionString;
            }
            return db;
        }
    }
}

