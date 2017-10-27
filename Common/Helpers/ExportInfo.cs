using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.XlsIO;

namespace Common.Helpers
{
    public class ExportInfo
    {
        private Dictionary<string, ColumnInfo> _columns = new Dictionary<string, ColumnInfo>();

        public string FromSql { get; set; }
        public string DataFile { get; set; }
        public bool HasHeader { get; set; }

        /// <summary>
        /// 不一定是SQL结果集全部列，只有设置过的列才存在。
        /// </summary>
        public Dictionary<string, ColumnInfo> Columns
        {
            get { return _columns; }
        }

        public void SetFormat(string field, string value)
        {
            GetColumn(field).Format = value;
        }
        
        public void SetComment(string field, string value)
        {
            GetColumn(field).Comment = value;
        }

        public void SetWidth(string field, int width)
        {
            GetColumn(field).Width = width;
        }

        public void SetAlign(string field, ColumnInfo.AlignMode align)
        {
            GetColumn(field).Align = align;
        }

        public void SetAllowInput(string field)
        {
            GetColumn(field).AllowInput = true;
        }

        public void SetFiledType(string field, ExcelDataType filedType)
        {
            GetColumn(field).InputType = filedType;
        }

        private ColumnInfo GetColumn(string key)
        {
            ColumnInfo result = null;

            if (!_columns.ContainsKey(key))
            {
                result = new ColumnInfo(key);
                _columns.Add(key, result);
            }

            return _columns[key];
        }
    }
}
