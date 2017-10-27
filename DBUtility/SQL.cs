using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace DBUtility
{
    public class SQL : DALHelper
    {
        internal static bool ExecInsert(object classObject, string strTableName, Dictionary<string, string> IdentityMapping, Dictionary<string, string> FieldMapping, Dictionary<string, string> FieldValueMapping, int intFlag, out int intMaxID)
        {
            Type type = classObject.GetType();
            List<string> listField = new List<string>();
            List<string> listFieldValue = new List<string>();
            List<string> listValue = new List<string>();
            foreach (string current in FieldMapping.Keys)
            {
                if (FieldValueMapping.ContainsKey(FieldMapping[current]))
                {
                    listField.Add(FieldMapping[current]);
                    listFieldValue.Add("@" + FieldMapping[current]);
                    listValue.Add(FieldValueMapping[FieldMapping[current]]);
                }
            }
            string field = string.Join(",", listField);
            string fieldValue = string.Join(",", listFieldValue);
            string commandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", strTableName, field, fieldValue);
            IDbDataParameter[] parms = DbParameterCache.GetCachedParameterSet(dbHelper.ConnectionString, commandText);
            parms = new IDbDataParameter[listFieldValue.Count];
            for (int i = 0; i < listFieldValue.Count; i++)
            {
                type = classObject.GetType().GetProperty(listField[i].ToString()).PropertyType;
                parms[i] = dbHelper.CreateDbParameter(listFieldValue[i], Model.GetDB2Type(type), ParameterDirection.Input, listValue[i]);
            }
            DbParameterCache.CacheParameterSet(dbHelper.ConnectionString, commandText, parms);
            if (intFlag != 0)
            {
                string strCommandText = commandText;
                commandText = string.Format("{0} SELECT MAX({1}) FROM {2}", strCommandText, SQL.GetIdentity(IdentityMapping), strTableName);
            }
            intMaxID = 0;
            bool result;
            if (intFlag == 0)
            {
                dbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms);
                result = true;
            }
            else
            {
                intMaxID = Convert.ToInt32(dbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
                result = true;
            }
            return result;
        }
        internal static bool ExecInsert(object classObject, string strTableName, Dictionary<string, string> IdentityMapping, Dictionary<string, string> FieldMapping, Dictionary<string, string> FieldValueMapping,IDbTransaction Trans, int intFlag, out int intMaxID)
        {
            Type type = classObject.GetType();
            List<string> listField = new List<string>();
            List<string> listFieldValue = new List<string>();
            List<string> listValue = new List<string>();
            foreach (string current in FieldMapping.Keys)
            {
                if (FieldValueMapping.ContainsKey(FieldMapping[current]))
                {
                    listField.Add(FieldMapping[current]);
                    listFieldValue.Add("@" + FieldMapping[current]);
                    listValue.Add(FieldValueMapping[FieldMapping[current]]);
                }
            }
            string field = string.Join(",", listField);
            string fieldValue = string.Join(",", listFieldValue);
            string commandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", strTableName, field, fieldValue);
            IDbDataParameter[] parms = DbParameterCache.GetCachedParameterSet(dbHelper.ConnectionString, commandText);
            parms = new IDbDataParameter[listFieldValue.Count];
            for (int i = 0; i < listFieldValue.Count; i++)
            {
                type = classObject.GetType().GetProperty(listField[i].ToString()).PropertyType;
                parms[i] = dbHelper.CreateDbParameter(listFieldValue[i], Model.GetDB2Type(type), ParameterDirection.Input, listValue[i]);
            }
            DbParameterCache.CacheParameterSet(dbHelper.ConnectionString, commandText, parms);
            if (intFlag != 0)
            {
                string strCommandText = commandText;
                commandText = string.Format("{0} SELECT MAX({1}) FROM {2}", strCommandText, SQL.GetIdentity(IdentityMapping), strTableName);
            }
            intMaxID = 0;
            bool result;
            if (intFlag == 0)
            {
                dbHelper.ExecuteNonQuery(Trans,CommandType.Text, commandText, parms);
                result = true;
            }
            else
            {
                intMaxID = Convert.ToInt32(dbHelper.ExecuteScalar(CommandType.Text, commandText, parms));
                result = true;
            }
            return result;
        }

        internal static bool ExecUpdate(object classObject, string strTableName, Dictionary<string, string> IdentityMapping, Dictionary<string, string> FieldMapping, Dictionary<string, string> FieldValueMapping)
        {
            Type type = classObject.GetType();
            List<string> listField = new List<string>();
            List<string> Field = new List<string>();
            foreach (string current in FieldMapping.Keys)
            {
                if (SQL.GetIdentity(IdentityMapping) != FieldMapping[current])
                {
                    listField.Add(FieldMapping[current] + "=@" + FieldMapping[current]);
                    Field.Add(FieldMapping[current]);
                }
            }
            List<string> listFieldValue = new List<string>();
            List<string> listValue = new List<string>();
            foreach (string current in FieldMapping.Keys)
            {
                if (SQL.GetIdentity(IdentityMapping) != FieldMapping[current])
                {
                    if (FieldValueMapping.ContainsKey(FieldMapping[current]))
                    {
                        listFieldValue.Add("@" + FieldMapping[current]);
                        listValue.Add(FieldValueMapping[FieldMapping[current]]);
                    }
                }
            }
            string field = string.Join(",", listField);
            string commandText = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", strTableName, field, SQL.GetIdentity(IdentityMapping));
            IDbDataParameter[] parms = DbParameterCache.GetCachedParameterSet(dbHelper.ConnectionString, commandText);
            parms = new IDbDataParameter[listFieldValue.Count + 1];
            for (int i = 0; i < listFieldValue.Count + 1; i++)
            {
                if (i == listFieldValue.Count)
                {
                    type = classObject.GetType().GetProperty(SQL.GetIdentity(IdentityMapping)).PropertyType;
                    parms[i] = dbHelper.CreateDbParameter("@" + SQL.GetIdentity(IdentityMapping),Model.GetDB2Type(type), ParameterDirection.Input, SQL.GetIndetityValue(classObject, IdentityMapping));
                }
                else
                {
                    type = classObject.GetType().GetProperty(Field[i].ToString()).PropertyType;
                    parms[i] = dbHelper.CreateDbParameter(listFieldValue[i],Model.GetDB2Type(type), ParameterDirection.Input, listValue[i]);
                }
            }
            DbParameterCache.CacheParameterSet(dbHelper.ConnectionString, commandText, parms);
            int num = Convert.ToInt32(dbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms));
            return num > 0;
        }

        internal static bool ExecUpdate(object classObject, string strTableName, Dictionary<string, string> IdentityMapping, Dictionary<string, string> FieldMapping, Dictionary<string, string> FieldValueMapping,IDbTransaction Trans)
        {
            Type type = classObject.GetType();
            List<string> listField = new List<string>();
            List<string> Field = new List<string>();
            foreach (string current in FieldMapping.Keys)
            {
                if (SQL.GetIdentity(IdentityMapping) != FieldMapping[current])
                {
                    listField.Add(FieldMapping[current] + "=@" + FieldMapping[current]);
                    Field.Add(FieldMapping[current]);
                }
                    
            }
            List<string> listFieldValue = new List<string>();
            List<string> listValue = new List<string>();
            foreach (string current in FieldMapping.Keys)
            {
                if (SQL.GetIdentity(IdentityMapping) != FieldMapping[current])
                {
                    if (FieldValueMapping.ContainsKey(FieldMapping[current]))
                    {
                        listFieldValue.Add("@" + FieldMapping[current]);
                        listValue.Add(FieldValueMapping[FieldMapping[current]]);
                    }
                }
            }
            string field = string.Join(",", listField);
            string commandText = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", strTableName, field, SQL.GetIdentity(IdentityMapping));
            IDbDataParameter[] parms = DbParameterCache.GetCachedParameterSet(dbHelper.ConnectionString, commandText);
            parms = new IDbDataParameter[listFieldValue.Count + 1];
            for (int i = 0; i < listFieldValue.Count + 1; i++)
            {
                if (i == listFieldValue.Count)
                {
                    type = classObject.GetType().GetProperty(SQL.GetIdentity(IdentityMapping)).PropertyType;
                    parms[i] = dbHelper.CreateDbParameter("@" + SQL.GetIdentity(IdentityMapping),Model.GetDB2Type(type), ParameterDirection.Input, SQL.GetIndetityValue(classObject, IdentityMapping));
                }
                else
                {
                    type = classObject.GetType().GetProperty(Field[i].ToString()).PropertyType;
                    parms[i] = dbHelper.CreateDbParameter(listFieldValue[i], Model.GetDB2Type(type), ParameterDirection.Input, listValue[i]);
                }
            }
            DbParameterCache.CacheParameterSet(dbHelper.ConnectionString, commandText, parms);
            int num = Convert.ToInt32(dbHelper.ExecuteNonQuery(Trans, CommandType.Text, commandText, parms));
            return num > 0;
        }
        internal static object ExecSelect(object classObject, string strTableName, Dictionary<string, string> IdentityMapping, Dictionary<string, string> FieldMapping)
        {
            Type type = classObject.GetType();
            List<string> listField = new List<string>();
            foreach (string current in FieldMapping.Keys)
            {
                listField.Add(FieldMapping[current]);
            }
            string field = string.Join(",", listField);
            string commandText = string.Format("SELECT {0} FROM {1} WHERE {2}=@{2}", field, strTableName, SQL.GetIdentity(IdentityMapping));
            IDbDataParameter[] parms = DbParameterCache.GetCachedParameterSet(dbHelper.ConnectionString, commandText);
            parms = new IDbDataParameter[IdentityMapping.Count];
            for (int i = 0; i < IdentityMapping.Count; i++)
            {
                type = classObject.GetType().GetProperty(listField[i].ToString()).PropertyType;
                parms[i] = dbHelper.CreateDbParameter("@" + SQL.GetIdentity(IdentityMapping), Model.GetDB2Type(type), ParameterDirection.Input, SQL.GetIndetityValue(classObject, IdentityMapping));
            }
            DbParameterCache.CacheParameterSet(dbHelper.ConnectionString, commandText, parms);
            DataSet ds = dbHelper.ExecuteDataset(CommandType.Text, commandText, strTableName, parms);
            List<object> list2 = SQL.ChangeDateSetToList(classObject, FieldMapping, ds);
            object result;
            if (list2.Count > 0)
            {
                result = list2[0];
            }
            else
            {
                result = null;
            }
            return result;
        }
        internal static object ExecSelect(object classObject, string strTableName, string strWHERE, Dictionary<string, string> FieldMapping)
        {
            List<string> listField = new List<string>();
            foreach (string current in FieldMapping.Keys)
            {
                listField.Add(FieldMapping[current]);
            }
            string field = string.Join(",", listField);
            string commandText = string.Format("SELECT {0} FROM {1} WHERE 1=1 AND {2}", field, strTableName, strWHERE);
            DataSet ds = dbHelper.ExecuteDataset(CommandType.Text, commandText, strTableName);
            List<object> list2 = SQL.ChangeDateSetToList(classObject, FieldMapping, ds);
            object result;
            if (list2.Count > 0)
            {
                result = list2[0];
            }
            else
            {
                result = null;
            }
            return result;
        }
        internal static bool ExecDelete(object classObject, string strTableName, Dictionary<string, string> IdentityMapping)
        {
            string commandText = string.Format("DELETE FROM {0} WHERE {1}=@{1}", strTableName, SQL.GetIdentity(IdentityMapping));
            IDbDataParameter[] parms = DbParameterCache.GetCachedParameterSet(dbHelper.ConnectionString, commandText);
            parms = new IDbDataParameter[IdentityMapping.Count];
            for (int i = 0; i < IdentityMapping.Count; i++)
            {
                parms[i] = dbHelper.CreateDbParameter("@" + SQL.GetIdentity(IdentityMapping), ParameterDirection.Input, SQL.GetIndetityValue(classObject, IdentityMapping));
            }
            DbParameterCache.CacheParameterSet(dbHelper.ConnectionString, commandText, parms);
            int num = Convert.ToInt32(dbHelper.ExecuteNonQuery(CommandType.Text, commandText, parms));
            return num > 0;
        }
        internal static bool ExecDelete(object classObject, string strTableName, Dictionary<string, string> IdentityMapping,IDbTransaction Trans)
        {
            string commandText = string.Format("DELETE FROM {0} WHERE {1}=@{1}", strTableName, SQL.GetIdentity(IdentityMapping));
            IDbDataParameter[] parms = DbParameterCache.GetCachedParameterSet(dbHelper.ConnectionString, commandText);
            parms = new IDbDataParameter[IdentityMapping.Count];
            for (int i = 0; i < IdentityMapping.Count; i++)
            {
                parms[i] = dbHelper.CreateDbParameter("@" + SQL.GetIdentity(IdentityMapping), ParameterDirection.Input, SQL.GetIndetityValue(classObject, IdentityMapping));
            }
            DbParameterCache.CacheParameterSet(dbHelper.ConnectionString, commandText, parms);
            int num = Convert.ToInt32(dbHelper.ExecuteNonQuery(Trans, CommandType.Text, commandText, parms));
            return num > 0;
        }
        internal static bool ExecDeleteList(List<object> listClassObject, string strTableName, Dictionary<string, string> IdentityMapping)
        {
            StringBuilder stringBuilder = new StringBuilder();
            IDbDataParameter[] parms = new IDbDataParameter[listClassObject.Count];
            for (int i = 0; i < listClassObject.Count; i++)
            {
                stringBuilder.Append(string.Format("DELETE FROM {0} WHERE {1}=@{1}{2}; ", strTableName, SQL.GetIdentity(IdentityMapping), i.ToString()));
                parms[i] = dbHelper.CreateDbParameter("@" + SQL.GetIdentity(IdentityMapping) + i.ToString(), ParameterDirection.Input, SQL.GetIndetityValue(listClassObject[i], IdentityMapping));
            }
            DbParameterCache.CacheParameterSet(dbHelper.ConnectionString, stringBuilder.ToString(), parms);
            int num = Convert.ToInt32(dbHelper.ExecuteNonQuery(CommandType.Text, stringBuilder.ToString(), parms));
            return num > 0;
        }
        internal static bool ExecDeleteList(List<object> listClassObject, string strTableName, Dictionary<string, string> IdentityMapping,IDbTransaction Trans)
        {
            StringBuilder stringBuilder = new StringBuilder();
            IDbDataParameter[] parms = new IDbDataParameter[listClassObject.Count];
            for (int i = 0; i < listClassObject.Count; i++)
            {
                stringBuilder.Append(string.Format("DELETE FROM {0} WHERE {1}=@{1}{2} ", strTableName, SQL.GetIdentity(IdentityMapping), i.ToString()));
                parms[i] = dbHelper.CreateDbParameter("@" + SQL.GetIdentity(IdentityMapping) + i.ToString(), ParameterDirection.Input, SQL.GetIndetityValue(listClassObject[i], IdentityMapping));
            }
            DbParameterCache.CacheParameterSet(dbHelper.ConnectionString, stringBuilder.ToString(), parms);
            int num = Convert.ToInt32(dbHelper.ExecuteNonQuery(Trans, CommandType.Text, stringBuilder.ToString(), parms));
            return num > 0;
        }
        internal static string CreateSelectList(object classObject, string strTableName, Dictionary<string, string> FieldMapping, string strWHERE)
        {
            List<string> listField = new List<string>();
            foreach (string current in FieldMapping.Keys)
            {
                listField.Add(FieldMapping[current]);
            }
            string field = string.Join(",", listField);
            return string.Format("SELECT {0} FROM {1} WHERE 1=1 AND {2}", field, strTableName, strWHERE);
        }
        internal static List<object> ExecSelectList(string strSQL, object classObject, Dictionary<string, string> FieldMapping)
        {
            DataSet ds = dbHelper.ExecuteDataset(CommandType.Text, strSQL);
            return SQL.ChangeDateSetToList(classObject, FieldMapping, ds);
        }
        internal static System.Data.DataTable ExecSelectList(string strSQL)
        {
            DataTable dt = dbHelper.ExecuteDataTable(CommandType.Text, strSQL);
            return dt;
        }
        internal static string GetIdentity(Dictionary<string, string> IdentityMapping)
        {
            string result = string.Empty;
            foreach (string current in IdentityMapping.Keys)
            {
                result = IdentityMapping[current];
            }
            return result;
        }
        internal static string GetIndetityValue(object classObject, Dictionary<string, string> IdentityMapping)
        {
            Type type = classObject.GetType();
            string text = string.Empty;
            string result = string.Empty;
            foreach (string current in IdentityMapping.Keys)
            {
                text = IdentityMapping[current];
                result = type.InvokeMember(current, BindingFlags.GetProperty, null, classObject, null).ToString();
            }
            return result;
        }
        internal static List<object> ChangeDateSetToList(object classObject, Dictionary<string, string> FieldMapping, DataSet ds)
        {
            Type type = classObject.GetType();
            List<object> list = new List<object>();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                PropertyInfo[] properties = type.GetProperties();
                Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
                PropertyInfo[] array = properties;
                for (int i = 0; i < array.Length; i++)
                {
                    PropertyInfo propertyInfo = array[i];
                    dictionary.Add(propertyInfo.Name, propertyInfo.PropertyType);
                }
                int count = ds.Tables[0].Rows.Count;
                for (int j = 0; j < count; j++)
                {
                    object obj = type.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
                    foreach (string current in FieldMapping.Keys)
                    {
                        object[] proType = Model.GetProType(classObject.GetType().GetProperty(current).PropertyType, ds.Tables[0].Rows[j][FieldMapping[current]].ToString());
                        type.InvokeMember(current, BindingFlags.SetProperty, null, obj, proType);
                    }
                    list.Add(obj);
                    obj = null;
                }
            }
            return list;
        }
    }
}
