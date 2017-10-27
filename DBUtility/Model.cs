using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
/*
*/

namespace DBUtility
{
    public class Model
    {
        public static Dictionary<string, string> GetValueMapping(object classObject, Dictionary<string, string> FieldMapping)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string current in FieldMapping.Keys)
            {
                object obj = classObject.GetType().InvokeMember(current, BindingFlags.GetProperty, null, classObject, null);
                if (obj != null)
                {
                    dictionary.Add(FieldMapping[current], obj.ToString());
                }
            }
            return dictionary;
        }
        public static  DbType GetDB2Type(Type typeStr)
        {
            switch (typeStr.Name) 
            {
                case "AnsiString":
                   return DbType.AnsiString;
                case "Binary":
                   return DbType.Binary;
                case "Byte":
                   return DbType.Byte;
                case "Boolean":
                   return DbType.Boolean;
                case "Currency":
                   return DbType.Currency;
                case "Date":
                   return DbType.Date;
                case "DateTime":
                   return DbType.DateTime;
                case "Decimal":
                   return DbType.Decimal;
                case "Double":
                   return DbType.Double;
                case "Guid":
                   return DbType.Guid;
                case "Int16":
                   return DbType.Int16;
                case "Int32":
                   return DbType.Int32;
                case "Int64":
                   return DbType.Int64;
                case "Object":
                   return DbType.Object;
                case "SByte":
                   return DbType.SByte;
                case "Single":
                   return DbType.Single;
                case "String":
                   return DbType.String;
                case "Time":
                   return DbType.Time;
                case "UInt16":
                   return DbType.UInt16;
                case "UInt32":
                   return DbType.UInt32;
                case "UInt64":
                   return DbType.UInt64;
                case "VarNumeric":
                   return DbType.VarNumeric;
                case "AnsiStringFixedLength":
                   return DbType.AnsiStringFixedLength;
                case "StringFixedLength":
                   return DbType.StringFixedLength;
                case "Xml":
                   return DbType.Xml;
                case "DateTime2":
                   return DbType.DateTime2;
                case "DateTimeOffset":
                    return DbType.DateTimeOffset;
                default:
                   return DbType.String;
            }
        }
        internal static object[] GetProType(Type type, object objectValue)
        {
            object[] result;
            if (objectValue == null)
            {
                result = null;
            }
            else if (type == typeof(bool))
            {
                result = new object[] { DbValue.GetBool(objectValue) };
            }
            else if (type == typeof(byte))
            {
                result = new object[] { DbValue.GetByte(objectValue) };
            }
            else if (type == typeof(byte[]))
            {
                result = new object[] { DbValue.GetBinary(objectValue) };
            }
            else if (type == typeof(char))
            {
                result = new object[] { DbValue.GetChar(objectValue) };
            }
            else if (type == typeof(DateTime))
            {
                result = new object[] { DbValue.GetDateTime(objectValue) };
            }
            else if (type == typeof(decimal))
            {
                result = new object[] { DbValue.GetDecimal(objectValue) };
            }
            else if (type == typeof(double))
            {
                result = new object[] { DbValue.GetDouble(objectValue) };
            }
            else if (type == typeof(Guid))
            {
                result = new object[] { DbValue.GetGuid(objectValue) };
            }
            else if (type == typeof(short))
            {
                result = new object[] { DbValue.GetInt16(objectValue) };
            }
            else if (type == typeof(int))
            {
                result = new object[] { DbValue.GetInt(objectValue) };
            }
            else if (type == typeof(long))
            {
                result = new object[] { DbValue.GetLong(objectValue) };
            }
            else if (type == typeof(sbyte))
            {
                result = new object[] { DbValue.GetSByte(objectValue) };
            }
            else if (type == typeof(float))
            {
                result = new object[] { DbValue.GetFloat(objectValue) };
            }
            else if (type == typeof(string))
            {
                result = new object[] { DbValue.GetString(objectValue) };
            }
            else if (type == typeof(ushort))
            {
                result = new object[] { DbValue.GetUInt16(objectValue) };
            }
            else if (type == typeof(uint))
            {
                result = new object[] { DbValue.GetUInt(objectValue) };
            }
            else if (type == typeof(ulong))
            {
                result = new object[] { DbValue.GetULong(objectValue) };
            }
            else
            {
                result = new object[] { DbValue.GetString(objectValue) };
            }
            return result;
        }

        /// <summary>
        /// 由Object取值
        /// </summary>
        public sealed class DbValue
        {
            /// <summary>
            /// 取得Int16值
            /// </summary>
            public static Int16? GetInt16(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    short result;
                    if (Int16.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得UInt16值
            /// </summary>
            public static UInt16? GetUInt16(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    ushort result;
                    if (UInt16.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            public static int GetInt(string obj)
            {
                if (!string.IsNullOrWhiteSpace(obj))
                {
                    int result;
                    if (int.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
            /// <summary>
            /// 取得Int值
            /// </summary>
            public static int? GetInt(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    int result;
                    if (int.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得UInt值
            /// </summary>
            public static uint? GetUInt(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    uint result;
                    if (uint.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得UInt64值
            /// </summary>
            public static ulong? GetULong(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    ulong result;
                    if (ulong.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得byte值
            /// </summary>
            public static byte? GetByte(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    byte result;
                    if (byte.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得sbyte值
            /// </summary>
            public static sbyte? GetSByte(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    sbyte result;
                    if (sbyte.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 获得Long值
            /// </summary>
            public static long? GetLong(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    long result;
                    if (long.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得Decimal值
            /// </summary>
            public static decimal? GetDecimal(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    decimal result;
                    if (decimal.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得Decimal值
            /// </summary>
            public static decimal GetDecimal(string obj)
            {
                if (!string.IsNullOrWhiteSpace(obj))
                {
                    decimal result;
                    if (decimal.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return 0.00m;
                }
                else
                {
                    return 0.00m;
                }
            }

            /// <summary>
            /// 取得float值
            /// </summary>
            public static float? GetFloat(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    float result;
                    if (float.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null; ;
                }
            }

            /// <summary>
            /// 取得double值
            /// </summary>
            public static double? GetDouble(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    double result;
                    if (double.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            /// <summary>
            /// 取得double值
            /// </summary>
            public static double GetDouble(string obj)
            {
                if (!string.IsNullOrWhiteSpace(obj))
                {
                    double result;
                    if (double.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return 0.00;
                }
                else
                {
                    return 0.00;
                }
            }
            /// <summary>
            /// 取得Guid值
            /// </summary>
            public static Guid? GetGuid(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    try
                    {
                        Guid result = new Guid(obj.ToString());
                        return result;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得DateTime值
            /// </summary>
            public static DateTime? GetDateTime(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    DateTime result;
                    if (DateTime.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得bool值
            /// </summary>
            public static bool? GetBool(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    bool result;
                    if (bool.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得byte[]
            /// </summary>
            public static byte[] GetBinary(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    return (byte[])obj;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得string值
            /// </summary>
            public static string GetString(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    return obj.ToString();
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// 取得char值
            /// </summary>
            public static char GetChar(object obj)
            {
                if (obj != null && obj != DBNull.Value)
                {
                    char result;
                    if (char.TryParse(obj.ToString(), out result))
                        return result;
                    else
                        return Convert.ToChar(obj);
                }
                else
                {
                    return Convert.ToChar(obj);
                }
            }
        }
    }
}
