using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.XlsIO;

namespace Common.Helpers
{
    public class ColumnInfo
    {
        public ColumnInfo(string field)
        {
            Field = field;
            Width = 20;
            Align = AlignMode.None;
            AllowInput = false;
        }

        public enum AlignMode
        {
            None,
            Left,
            Right,
            Center
        }

        public string Field { get; private set; }
        public string Comment { get; set; }
        public string Format { get; set; }
        public ExcelDataType InputType { get; set; }
        public bool AllowInput { get; set; }
        public int Width { get; set; }
        public AlignMode Align { get; set; }
    }
}
