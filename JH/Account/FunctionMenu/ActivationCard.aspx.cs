using Common.Helpers;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH.Account.FunctionMenu
{
    public partial class ActivationCard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable db = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text, "select Code,faceValue from JH_ActivationCard where isRelease='1' and isUse='0'");
            string filename = "ActivationCard.xls";
            export_excel(HttpContext.Current.Server.MapPath("/excelFile/"+ filename), db, false);
            Literal1.Text = "<a href=\"/excelFile/"+ filename + "\">下载文件</a>";
        }
        private static ExportInfo _exportInfo;
        /// <summary>
        /// 导出EXCEL和CSV文件
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="table"></param>
        public static void export_excel(string dataFile, DataTable table, bool hasHead)
        {
            _exportInfo = new ExportInfo();
            _exportInfo.DataFile = dataFile;
            //_exportInfo.DataFile = Path.Combine(DataPath, dataFile);
            using (ExcelEngine engine = new ExcelEngine())
            {
                IWorkbook workbook = engine.Excel.Workbooks.Create(1);
                workbook.BuiltInDocumentProperties.Author = "";
                workbook.BuiltInDocumentProperties.Company = "";
                workbook.BuiltInDocumentProperties.CreationDate = DateTime.Now;

                IWorksheet sheet = workbook.Worksheets[0];
                sheet.ImportDataTable(table, hasHead, 1, 1);
                sheet.UsedRange.CellStyle.Font.FontName = "宋体";
                sheet.UsedRange.CellStyle.Font.Size = 9;
                sheet.UsedRange.CellStyle.Locked = true;

                if (hasHead)
                    SetHeaderStyle(sheet.Rows[0], _exportInfo.Columns);

                SetColumnStyle(table, sheet, _exportInfo.Columns);

                sheet.AutoFilters.FilterRange = sheet.UsedRange;
                sheet.Protect(Path.GetRandomFileName(), ExcelSheetProtection.All);

                workbook.SaveAs(_exportInfo.DataFile);
                engine.ThrowNotSavedOnDestroy = false;
            }
        }
        private static void SetHeaderStyle(IRange header, Dictionary<string, ColumnInfo> columns)
        {
            header.WrapText = true;
            header.CellStyle.Font.Bold = true;
            header.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
            header.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            header.CellStyle.Color = System.Drawing.Color.FromArgb(0xFFFF99);
            header.CellStyle.Borders.LineStyle = ExcelLineStyle.Thin;
            header.CellStyle.Borders[ExcelBordersIndex.DiagonalDown].LineStyle = ExcelLineStyle.None;
            header.CellStyle.Locked = true;

            foreach (IRange cell in header.Cells)
            {
                if (columns.ContainsKey(cell.Text) && columns[cell.Text].Comment != null)
                {
                    ICommentShape comment = cell.AddComment();
                    comment.Text = columns[cell.Text].Comment;
                }
            }
        }
        private static void SetColumnStyle(DataTable table, IWorksheet sheet, Dictionary<string, ColumnInfo> columns)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                IRange range = sheet.Columns[i];

                range.ColumnWidth = 20;
                range.CellStyle.Locked = true;

                string field = table.Columns[i].ColumnName;

                if (columns.ContainsKey(field))
                {
                    ColumnInfo info = columns[field];
                    range.ColumnWidth = info.Width;
                    range.CellStyle.Locked = !info.AllowInput;

                    if (info.AllowInput)
                    {
                        IDataValidation validation = range.DataValidation;
                        validation.AllowType = info.InputType;
                        validation.CompareOperator = ExcelDataValidationComparisonOperator.Between;
                        validation.FirstFormula = "-9999999999";
                        validation.SecondFormula = "9999999999";
                        validation.ShowErrorBox = true;
                        validation.ErrorBoxText = "此单元格只能输入数字";
                        validation.ErrorBoxTitle = "系统提示";
                    }

                    switch (info.Align)
                    {
                        case ColumnInfo.AlignMode.Center:
                            range.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            break;
                        case ColumnInfo.AlignMode.Left:
                            range.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                            break;
                        case ColumnInfo.AlignMode.Right:
                            range.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    range.CellStyle.Color = System.Drawing.Color.FromArgb(0xFFFF99);
                    range.CellStyle.Borders.LineStyle = ExcelLineStyle.Thin;
                    range.CellStyle.Borders[ExcelBordersIndex.DiagonalDown].LineStyle = ExcelLineStyle.None;
                }
            }
        }
    }
}