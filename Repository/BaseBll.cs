using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;

namespace Repository
{
    public class BaseBll<T> where T : class, new()
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        public static bool Add(T model)
        {
            return ORM.Add(model);
        }

        /// <summary>
        /// 添加实体-返回最大ID
        /// </summary>
        public static bool Add(T model, out int intMaxID)
        {
            return ORM.Add(model, out intMaxID);
        }
        /// <summary>
        /// 添加实体-通过事务的方式提交
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="Trans">事务对象</param>
        /// <returns></returns>
        public static bool Add(T model, IDbTransaction Trans)
        {
            return ORM.Add(model, Trans);
        }
        /// <summary>
        /// 修改实体
        /// </summary>
        public static bool Save(T model)
        {
            return ORM.Save(model);
        }
        /// <summary>
        /// 修改实体-通过事务的方式
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="Trans">事务对象</param>
        /// <returns></returns>
        public static bool Save(T model, IDbTransaction Trans)
        {
            return ORM.Save(model, Trans);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        public static bool Remove(T model)
        {
            return ORM.Remove(model);
        }
        /// <summary>
        /// 删除实体-通过事务的方式
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="Trans">事务对象</param>
        /// <returns></returns>
        public static bool Remove(T model, IDbTransaction Trans)
        {
            return ORM.Remove(model, Trans);
        }
        /// <summary>
        /// 删除所有
        /// </summary>
        public static bool RemoveList()
        {
            return ORM.RemoveList(ORM.GetList(new T()), new T());
        }

        /// <summary>
        /// 删除所有-条件
        /// </summary>
        public static bool RemoveList(string where)
        {
            return ORM.RemoveList(ORM.GetList(new T(), where), new T());
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        public static T Get(T model)
        {
            return (T)ORM.Get(model);
        }

        /// <summary>
        /// 获取实体-条件
        /// </summary>
        public static T Get(T model, string where)
        {
            return (T)ORM.Get(model, where);
        }

        /// <summary>
        /// GetDataTable
        /// </summary>
        public static System.Data.DataTable GetDataTable()
        {
            return ORM.GetDataTable(new T());
        }

        /// <summary>
        /// GetDataTable-条件
        /// </summary>
        public static System.Data.DataTable GetDataTable(string where)
        {
            return ORM.GetDataTable(new T(), where);
        }

        /// <summary>
        /// 泛型集合
        /// </summary>
        public static List<T> GetList()
        {
            return GetList(ORM.GetList(new T()));
        }

        /// <summary>
        /// 泛型集合-条件
        /// </summary>
        public static List<T> GetList(string where)
        {
            return GetList(ORM.GetList(new T(), where));
        }

        /// <summary>
        /// 泛型分页
        /// </summary>
        public static List<T> GetList(int pageSize, int currentIndex, out int totalCount)
        {
            return GetList(ORM.GetList(new T(), pageSize, currentIndex, out totalCount));
        }

        /// <summary>
        /// 泛型分页-条件
        /// </summary>
        public static List<T> GetList(int pageSize, int currentIndex, out int totalCount, string where)
        {
            return GetList(ORM.GetList(new T(), pageSize, currentIndex, out totalCount, where));
        }

        /// <summary>
        /// 将对象类型转换成为实体类型
        /// </summary>
        private static List<T> GetList(List<object> objList)
        {
            List<T> list = new List<T>();
            foreach (object obj in objList)
            {
                list.Add((T)obj);
            }
            return list;
        }
        /// <summary>
        /// 将对象类型转换成为实体类型
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public static List<T> List(List<object> objList)
        {
            List<T> list = new List<T>();
            foreach (object obj in objList)
            {
                list.Add((T)obj);
            }
            return list;
        }
    }
}
