
using Eval3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public static class Utility
    {
        public class SymmetricEncryptor
        {
            private byte[] _key;
            private byte[] _vector;

            public SymmetricEncryptor(string key, string vector)
            {
                InitBuffer(ref _key, key);
                InitBuffer(ref _vector, vector);
            }

            private void InitBuffer(ref byte[] buffer, string value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    buffer = Convert.FromBase64String(value);
                }
                else
                {
                    buffer = new byte[8];
                    new Random().NextBytes(buffer);
                }
            }

            private bool IsEncrypted(string value)
            {
                try
                {
                    Convert.FromBase64String(value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public string Encrypt(string value)
            {
                if (IsEncrypted(value))
                    return value;

                SymmetricAlgorithm sa = new DESCryptoServiceProvider();
                ICryptoTransform ct = sa.CreateEncryptor(_key, _vector);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(value);
                        cs.Write(buffer, 0, buffer.Length);
                        cs.FlushFinalBlock();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }

            public string Decrypt(string value)
            {
                if (!IsEncrypted(value))
                    return value;

                try
                {
                    SymmetricAlgorithm sa = new DESCryptoServiceProvider();
                    ICryptoTransform ct = sa.CreateDecryptor(_key, _vector);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                        {
                            byte[] buffer = Convert.FromBase64String(value);
                            cs.Write(buffer, 0, buffer.Length);
                            cs.FlushFinalBlock();
                        }

                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
                catch (Exception e)
                {
                    e.ToString(); // disable CS0168
                    return value;
                }
            }
        }

        /// <summary>
        /// 获取系统名称
        /// </summary>
        public static string SystemName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["systemName"].ToString();
            }
        }
        /// <summary>
        /// 应用ID
        /// </summary>
        public static string AppID
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AppID"].ToString();
            }
        }
        /// <summary>
        /// 商户ID
        /// </summary>
        public static string MCHID
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["MCHID"].ToString();
            }
        }
        /// <summary>
        /// 网站域名
        /// </summary>
        public static string url
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["url"].ToString();
            }
        }
        /// <summary>
        /// 应用密匙
        /// </summary>
        public static string AppSecret
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AppSecret"].ToString();
            }
        }
        public static string key
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["key"].ToString();
            }
        }
        /// <summary>
        /// sToken
        /// </summary>
        public static string sToken
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["sToken"].ToString();
            }
        }
        /// <summary>
        /// 虚拟站点名称
        /// </summary>
        //public static string Page_Path
        //{
        //    get
        //    {
        //        return System.Configuration.ConfigurationManager.AppSettings["Page_Path"].ToString();
        //    }
        //}

        /// <summary>
        /// 上传图片路径
        /// </summary>
        //public static string Upload_Path
        //{
        //    get
        //    {
        //        return System.Configuration.ConfigurationManager.AppSettings["uploadPath"].ToString();
        //    }
        //}

        //public static string Page_Path_JB
        //{
        //    get
        //    {
        //        return System.Configuration.ConfigurationManager.AppSettings["Page_Path_JB"].ToString();
        //    }
        //}
        public static string Encrypt(string text)
        {
            char[] data = text.ToCharArray();
            byte[] buffer = new byte[data.Length];
            StringBuilder sb = new StringBuilder();
            HashAlgorithm sha = new SHA1CryptoServiceProvider();

            for (int i = 0; i < data.Length; ++i)
                buffer[i] = (byte)data[i];

            buffer = sha.ComputeHash(buffer);
            foreach (byte b in buffer)
                sb.AppendFormat("{0:X2}", b);

            return sb.ToString();
        }
        //log4net日志专用
        //public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        //public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="info"></param>
        //public static void Error(string info,Exception ex)
        //{
        //    if (logerror.IsErrorEnabled)
        //    {
        //        logerror.Error(info,ex);
        //    }
        //}
        /// <summary>
        /// 获取当前选中地市的ID号
        /// </summary>
        public static string CityID
        {
            get
            {
                if (HttpContext.Current.Session["CityId"] != null)
                    return HttpContext.Current.Session["CityId"].ToString();
                else
                    return string.Empty;
            }
        }
        //log4net日志专用
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="info"></param>
        public static void Error(string info)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info);
            }
        }
        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void Info(string info, Exception ex)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info, ex);
            }
        }
        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void Info(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void WriteLog(string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, se);
            }
        }
        /// <summary>
        /// 普通的文件记录日志
        /// </summary>
        /// <param name="info"></param>
        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }
        /// <summary>
        /// 判断页面请求的URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isurl"></param>
        /// <returns></returns>
        public static bool RequestUrl(string url)
        {
            string isurl = System.Configuration.ConfigurationManager.AppSettings["RequestUrl"].ToString();
            if (RootDomain(url) == isurl)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取当前域名的根域
        /// </summary>
        /// <param name="url">域名地址</param>
        /// <returns></returns>
        public static string RootDomain(string url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }

            var uri = new Uri(url);
            string rootDomain;
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    {
                        if (uri.IsLoopback)
                        {
                            rootDomain = uri.Host;
                        }
                        else
                        {
                            string host = uri.Host;
                            var hosts = host.Split('.');
                            rootDomain = hosts.Length == 2 ? host : string.Format("{0}.{1}", hosts[1], hosts[2]);
                        }
                    }
                    break;
                default:
                    rootDomain = uri.Host;
                    break;
            }
            return rootDomain;
        }
        /// <summary>
        /// 获取本机的IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string str = string.Empty;
            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostByName(hostname);
            IPAddress localIPAddr = localhost.AddressList[0];
            return localIPAddr.ToString();
        }
        /// <summary>
        /// 获取电脑名称
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }
        /// <summary>
        /// 获取本地MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetMac()
        {
            string str = string.Empty;

            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in nis)
            {
                PhysicalAddress pa = ni.GetPhysicalAddress();
                str = pa.ToString();
                break;
            }
            string mac_dest = string.Empty;
            for (int i = 0; i < 11; i++)
            {
                if (0 == (i % 2))
                {
                    if (i == 10)
                    {
                        mac_dest = mac_dest.Insert(0, str.Substring(i, 2));
                    }
                    else
                    {
                        mac_dest = "-" + mac_dest.Insert(0, str.Substring(i, 2));
                    }
                }
            }
            return mac_dest;
        }
        /// <summary>
        /// 普通的文件记录日志
        /// </summary>
        /// <param name="info"></param>


        public static string GetMd5HashCode(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                result.AppendFormat("{0:X2}", data[i]);
            }

            return result.ToString();
        }

        public static int GetStringHashCode(string text)
        {
            return (text == null ? 0 : text.GetHashCode());
        }

        /// <summary>
        /// 生成随机字符串 30位
        /// </summary>
        public static string NetxtString()
        {
            string str = string.Empty;
            Hashtable hashtable = new Hashtable();
            Random rm = new Random();
            int RmNum = 15;
            for (int i = 0; hashtable.Count < RmNum; i++)
            {
                int nValue = rm.Next(100);
                if (!hashtable.ContainsValue(nValue) && nValue != 0)
                {
                    hashtable.Add(nValue, nValue);
                    str += nValue.ToString();
                }
            }
            return str;
        }
        /// <summary>
        /// 随机种子数
        /// </summary>
        /// <returns></returns>
        public static string GetDataRandom()
        {
            string text = DateTime.Now.ToString("ffff");
            Random random = new Random();
            return (text + random.Next(902300));
        }
        /// <summary>
        /// Summary description for EvalFunctions.
        /// </summary>
        public class EvalFunctions : iEvalFunctions, iVariableBag
        {
            public int aNumber = 5;

            public string[] anArray
            {
                get
                {
                    return "How I want a drink alcoholic of course after the heavy lectures involving quantum mechanics".Split(' ');
                }
            }

            public string Description
            {
                get
                {
                    return "This module contains all the common functions";
                }
            }

            public EvalType EvalType
            {
                get
                {
                    return EvalType.Object;
                }
            }

            public string Name
            {
                get
                {
                    return "EvalFunctions";
                }
            }

            public System.Type systemType
            {
                get
                {
                    return this.GetType();
                }
            }

            public object Value
            {
                get
                {
                    return this;
                }
            }

            public double Sin(double v)
            {
                return Math.Sin(v);
            }

            public double Cos(double v)
            {
                return Math.Cos(v);
            }

            public DateTime Now()
            {
                return System.DateTime.Now;
            }

            public string Trim(string str)
            {
                return str.Trim();
            }

            public string LeftTrim(string str)
            {
                return str.TrimStart();
            }

            public string RightTrim(string str)
            {
                return str.TrimEnd();
            }

            public string PadLeft(string str, int wantedlen, string addedchar)
            {
                while ((str.Length < wantedlen))
                {
                    str = (addedchar + str);
                    // Warning!!! Optional parameters not supported
                }
                return str;
            }

            public double Mod(double x, double y)
            {
                return (x % y);
            }

            public object If(bool cond, object TrueValue, object FalseValue)
            {
                if (cond)
                {
                    return TrueValue;
                }
                else
                {
                    return FalseValue;
                }
            }

            public string Lower(string value)
            {
                return value.ToLower();
            }

            public string Upper(string value)
            {
                return value.ToUpper();
            }

            public string WCase(string value)
            {
                if ((value.Length == 0))
                    return string.Empty;

                return (value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower());
            }

            public DateTime Date(int year, int month, int day)
            {
                return new DateTime(year, month, day);
            }

            public int Year(DateTime d)
            {
                return d.Year;
            }

            public int Month(DateTime d)
            {
                return d.Month;
            }

            public int Day(DateTime d)
            {
                return d.Day;
            }

            string Replace(string Base, string search, string repl)
            {
                return Base.Replace(search, repl);
            }

            public string Substr(string s, int from, int len)
            {
                if ((s == null))
                {
                    return String.Empty;
                }
                // Warning!!! Optional parameters not supported
                from--;
                if ((from < 1))
                {
                    from = 0;
                }
                if ((from >= s.Length))
                {
                    from = s.Length;
                }
                if ((from + len) > s.Length)
                {
                    len = (s.Length - from);
                }
                return s.Substring(from, len);
            }

            public int Len(string str)
            {
                return str.Length;
            }

            public double Abs(double val)
            {
                if ((val < 0))
                {
                    return (val * -1);
                }
                else
                {
                    return val;
                }
            }

            public int Int(object value)
            {
                return (int)(value);
            }

            public int Trunc(double value, int prec)
            {
                value = (value - (0.5 / Math.Pow(10, prec)));
                // Warning!!! Optional parameters not supported
                return (int)(Math.Round(value, prec));
            }

            public double Dec(object value)
            {
                return (double)(value);
            }

            public double Round(object value)
            {
                return Math.Round((double)(value));
            }

            public string Chr(int c)
            {
                return string.Empty + (char)(c);
            }

            public string ChCR()
            {
                return "\r";
            }

            public string ChLF()
            {
                return "\n";
            }

            public string ChCRLF()
            {
                return "\r\n";
            }

            public double Exp(double Base, double pexp)
            {
                return Math.Pow(Base, pexp);
            }

            public string[] Split(string s, string delimiter)
            {
                return s.Split(delimiter[0]);
                // Warning!!! Optional parameters not supported
            }

            System.DBNull DbNull()
            {
                return System.DBNull.Value;
            }

            public double Sqrt(double v)
            {
                return Math.Sqrt(v);
            }

            public double Power(double v, double e)
            {
                return Math.Pow(v, e);
            }

            public iEvalFunctions InheritedFunctions()
            {
                return null;
            }

            public System.Type SystemType
            {
                get
                {
                    return this.GetType();
                }
            }

            public iEvalTypedValue GetVariable(string varname)
            {
                switch (varname.ToLower())
                {
                    case "pa":
                        return new Eval3.EvalVariable("pa", 12.0, "PA is the age of the programmer", typeof(double));
                    case "vat":
                        return new Eval3.EvalVariable("vat", 12.5, "V.A.T.", typeof(double));
                }
                return null;
            }
        }


        private static char[] constant ={ '0','1','2','3','4','5','6','7','8','9',
             'a','b','c','d','e','f','g','h','j','k','m','n','p','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','J','K','M','N','P','R','S','T','U','V','W','X','Y','Z'};
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="length">随机数字符串长度</param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            Random random = new Random();
            str.Append(constant.Skip(32).Take(22).ToArray()[random.Next(22)]);
            for (int i = 0; i < length - 1; i++)
            {
                str.Append(constant.Take(32).ToArray()[random.Next(32)]);
            }
            return str.ToString();
        }

        public static string GenerateRandomVerifyCode(int length)
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            Random random = new Random();
            str.Append(constant.Skip(10).Take(22).ToArray()[random.Next(22)]);
            for (int i = 0; i < length - 1; i++)
            {
                str.Append(constant.Take(10).ToArray()[random.Next(10)]);
            }
            return str.ToString();
        }
        /// <summary>
        /// 生成充值流水号格式：8位日期加8位顺序号，如2010030200000056。
        /// </summary>
        public static string GetSerialNumber(string serialNumber)
        {
            if (serialNumber != "0")
            {
                string headDate = serialNumber.Substring(0, 4);
                int lastNumber = int.Parse(serialNumber.Substring(6));
                //如果数据库最大值流水号中日期和生成日期在同一天，则顺序号加1
                if (headDate == DateTime.Now.ToString("yyyy"))
                {
                    lastNumber++;
                    return headDate + lastNumber.ToString("000000");
                }
            }
            return DateTime.Now.ToString("yyyy") + "000001";
        }

        /// <summary>
        /// 获取Session中CityId值
        /// </summary>
        /// <returns>CityId</returns>
        public static string GetSessionCityID()
        {
            string City_Id = "";
            if (HttpContext.Current.Session["CityId"] != null)
            {
                City_Id = HttpContext.Current.Session["CityId"].ToString();
            }
            return City_Id;
        }


    }
}
