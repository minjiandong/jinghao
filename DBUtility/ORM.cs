using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
/*
*/

namespace DBUtility
{
    public class ORM
    {
        public static bool Add(object entity)
        {
            int num = 0;
            int intFlag = 0;
            return Add(entity, out num, intFlag);
        }
        public static bool Add(object entity, IDbTransaction Trans)
        {
            int intMaxID = 0;
            int intFlag = 0;
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> identityMapping = Mapping.GetIdentityMapping(entity);
            Dictionary<string, string> baseFieldMapping = Mapping.GetBaseFieldMapping(entity);
            Dictionary<string, string> valueMapping = Model.GetValueMapping(entity, baseFieldMapping);
            return SQL.ExecInsert(entity, tableName, identityMapping, baseFieldMapping, valueMapping,Trans, intFlag, out intMaxID);
        }
        public static bool Add(object entity, out int maxID)
        {
            maxID = 0;
            int intFlag = 1;
            return Add(entity, out maxID, intFlag);
        }
        private static bool Add(object entity, out int intMaxID, int intFlag)
        {
            intMaxID = 0;
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> identityMapping = Mapping.GetIdentityMapping(entity);
            Dictionary<string, string> baseFieldMapping = Mapping.GetBaseFieldMapping(entity);
            Dictionary<string, string> valueMapping = Model.GetValueMapping(entity, baseFieldMapping);
            return SQL.ExecInsert(entity, tableName, identityMapping, baseFieldMapping, valueMapping, intFlag, out intMaxID);
        }
        public static bool Save(object entity)
        {
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> identityMapping = Mapping.GetIdentityMapping(entity);
            Dictionary<string, string> baseFieldMapping = Mapping.GetBaseFieldMapping(entity);
            Dictionary<string, string> valueMapping = Model.GetValueMapping(entity, baseFieldMapping);
            return SQL.ExecUpdate(entity, tableName, identityMapping, baseFieldMapping, valueMapping);
        }

        public static bool Save(object entity,IDbTransaction Trans)
        {
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> identityMapping = Mapping.GetIdentityMapping(entity);
            Dictionary<string, string> baseFieldMapping = Mapping.GetBaseFieldMapping(entity);
            Dictionary<string, string> valueMapping = Model.GetValueMapping(entity, baseFieldMapping);
            return SQL.ExecUpdate(entity, tableName, identityMapping, baseFieldMapping, valueMapping,Trans);
        }

        public static object Get(object entity)
        {
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> identityMapping = Mapping.GetIdentityMapping(entity);
            Dictionary<string, string> allFieldMapping = Mapping.GetAllFieldMapping(entity);
            return SQL.ExecSelect(entity, tableName, identityMapping, allFieldMapping);
        }
        public static object Get(object entity, string where)
        {
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> allFieldMapping = Mapping.GetAllFieldMapping(entity);
            return SQL.ExecSelect(entity, tableName, where, allFieldMapping);
        }
        public static bool Remove(object entity)
        {
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> identityMapping = Mapping.GetIdentityMapping(entity);
            return SQL.ExecDelete(entity, tableName, identityMapping);
        }
        public static bool Remove(object entity,IDbTransaction Trans)
        {
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> identityMapping = Mapping.GetIdentityMapping(entity);
            return SQL.ExecDelete(entity, tableName, identityMapping,Trans);
        }
        public static bool RemoveList(List<object> listEntity, object entity)
        {
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> identityMapping = Mapping.GetIdentityMapping(entity);
            return SQL.ExecDeleteList(listEntity, tableName, identityMapping);
        }
        public static bool RemoveList(List<object> listEntity, object entity,IDbTransaction Trans)
        {
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> identityMapping = Mapping.GetIdentityMapping(entity);
            return SQL.ExecDeleteList(listEntity, tableName, identityMapping,Trans);
        }
        public static System.Data.DataTable GetDataTable(object entity)
        {
            string where = " 1 = 1 ";
            return GetDataTable(entity, where);
        }

        public static System.Data.DataTable GetDataTable(object entity, string where)
        {
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> allFieldMapping = Mapping.GetAllFieldMapping(entity);
            string strSQL = SQL.CreateSelectList(entity, tableName, allFieldMapping, where);
            return SQL.ExecSelectList(strSQL);
        }

        public static List<object> GetList(object entity)
        {
            string where = " 1 = 1 ";
            return GetList(entity, where);
        }
       
        public static List<object> GetList(object entity, string where)
        {
            string tableName = Mapping.GetTableName(entity);
            Dictionary<string, string> allFieldMapping = Mapping.GetAllFieldMapping(entity);
            string strSQL = SQL.CreateSelectList(entity, tableName, allFieldMapping, where);
            return SQL.ExecSelectList(strSQL, entity, allFieldMapping);
        }
        public static List<object> GetList(object entity, int pageSize, int currentIndex, out int totalCount)
        {
            string where = " 1 = 1 ";
            return GetList(entity, pageSize, currentIndex, out totalCount, where);
        }
        public static List<object> GetList(object entity, int pageSize, int currentIndex, out int totalCount, string where)
        {
            List<object> list = GetList(entity, where);
            totalCount = list.Count;
            return list.Skip(pageSize * (currentIndex - 1)).Take(pageSize).ToList();
        }
        public static List<object> ChangeDateSetToList(object entity, DataSet ds)
        {
            Dictionary<string, string> allFieldMapping = Mapping.GetAllFieldMapping(entity);
            return SQL.ChangeDateSetToList(entity, allFieldMapping, ds);
        }
        public static List<object> GetListPage(int pageSize, int currentIndex, out int totalCount, string strsql)
        {
            List<object> DSlist = new List<object>();
            DataSet DS = DBUtility.DALHelper.dbHelper.ExecuteDataset(CommandType.Text, strsql);
            for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                for (int f = 0; f < DS.Tables[0].Columns.Count; f++)
                {
                    dictionary.Add(DS.Tables[0].Columns[f].ColumnName.ToUpper(), DS.Tables[0].Rows[i][DS.Tables[0].Columns[f].ColumnName].ToString());
                }
                DSlist.Add(dictionary);
            }
            List<object> list = DSlist.Skip(pageSize * (currentIndex - 1)).Take(pageSize).ToList();
            totalCount = DSlist.Count;
            return list;
        }
        /// <summary>
        /// 无分页获取数据
        /// </summary>
        /// <param name="totalCount">返回数据条数</param>
        /// <param name="strsql">输入的SQL语句</param>
        /// <returns></returns>
        public static List<object> GetListNoPage(out int totalCount,string strsql)
        {
            List<object> DSlist = new List<object>();
            DataSet DS = DBUtility.DALHelper.dbHelper.ExecuteDataset(CommandType.Text, strsql);
            for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                for (int f = 0; f < DS.Tables[0].Columns.Count; f++)
                {
                    dictionary.Add(DS.Tables[0].Columns[f].ColumnName, DS.Tables[0].Rows[i][DS.Tables[0].Columns[f].ColumnName].ToString());
                }
                DSlist.Add(dictionary);
            }
            List<object> list = DSlist.ToList();
            totalCount = DSlist.Count;
            return list;
        }
    }
}
