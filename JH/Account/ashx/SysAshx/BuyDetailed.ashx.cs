using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using Common.Helpers;
using Syncfusion.XlsIO;
using System.IO;

namespace JH.Account.ashx.SysAshx
{
    /// <summary>
    /// BuyDetailed 的摘要说明
    /// </summary>
    public class BuyDetailed : IHttpHandler
    {
        Models.jsoninfo json = new Models.jsoninfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string mt = context.Request["p"] ?? "";
            try
            {
                switch (mt) 
                {
                    case "list":
                        List(context);
                        break;
                    case "download":
                        download(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                 
            }
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
        private void download(HttpContext c)
        {
            string paymentType = c.Request["paymentType"] ?? "";
            string nickname = c.Request.Params["nickname"] ?? "";
            string timej = c.Request["timej"] ?? "";
            string timek = c.Request["timek"] ?? "";
            StringBuilder tr = new StringBuilder();
            if (paymentType != "-1")
            {
                tr.AppendFormat(" and a.paymentType='{0}' ", paymentType);
            }
            if (!string.IsNullOrWhiteSpace(nickname))
            {
                tr.AppendFormat(" and  b.MapName like '%{0}%'", nickname);
            }
            if (!string.IsNullOrWhiteSpace(timek) && !string.IsNullOrWhiteSpace(timej))
            {
                tr.AppendFormat(" and CONVERT(varchar(10),a.consumptionDate,120)>='{0}' and CONVERT(varchar(10),a.consumptionDate,120)<='{1}'", timek, timej);
            }
            Model.JH_SYS_USER user = SetCookie.GetUserInfo();
            if (user.UserType == "1")
            {
                tr.Append(" and exists(select id from JH_ScenicList as b where b.Scenicid=Scenic_id)");
            }

            DataTable db = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text, string.Format("select  b.MapName,a.money,a.consumptionDate,case when a.paymentType='0' then '激活码支付' when  a.paymentType='1' then '微信支付' else '余额支付' end as paymentType from JH_Activation as a left join JH_ScenicSpot as b on b.id=a.Scenic_id where 1=1 {0} order by a.id desc", tr.ToString()));

            DataTable dt = new DataTable();
            dt.Columns.Add("MapName", typeof(string));
            dt.Columns.Add("money", typeof(string));
            dt.Columns.Add("consumptionDate", typeof(string));
            dt.Columns.Add("paymentType", typeof(string));

            DataRow dr = dt.NewRow();
            dr["MapName"] = "景区名称";
            dr["money"] = "消费金额";
            dr["consumptionDate"] = "消费时间";
            dr["paymentType"] = "支付类型";
            dt.Rows.Add(dr);  
            for (int i = 0; i < db.Rows.Count; i++)
            {
                DataRow _dr = dt.NewRow();
                _dr["MapName"] = db.Rows[i]["MapName"].ToString();
                _dr["money"] = db.Rows[i]["money"].ToString();
                _dr["consumptionDate"] = db.Rows[i]["consumptionDate"].ToString();
                _dr["paymentType"] = db.Rows[i]["paymentType"].ToString();
                dt.Rows.Add(_dr);  
            }
            DataView dv = new DataView(dt); 
            DataTable dtv = dv.ToTable(); 
            string filename = Common.Utility.GetDataRandom() + ".xls";
            export_excel(HttpContext.Current.Server.MapPath("/excelFile/" + filename), dtv, false);
            json.info = "/excelFile/" + filename + "";
            json.type = "ok";
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        private void List(HttpContext c)
        {
            string strWhere = string.Empty;
            string sortname = c.Request["sortname"] ?? "id";//排序的字段
            string sortorder = c.Request["sortorder"] ?? "desc";//排序的方式
            int page = Convert.ToInt32(c.Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(c.Request["pagesize"] ?? "10");
            int totalCount = 0;
            string paymentType = c.Request["paymentType"] ?? "";
            string nickname = c.Request.Params["nickname"] ?? "";
            string timej = c.Request["timej"] ?? "";
            string timek = c.Request["timek"] ?? "";
            StringBuilder tr = new StringBuilder();
            if (paymentType != "-1")
            {
                tr.AppendFormat(" and paymentType='{0}' ", paymentType);
            }
            if (!string.IsNullOrWhiteSpace(nickname))
            {
                tr.AppendFormat(" and exists(select id from JH_ScenicSpot where id=Scenic_id and MapName like '%{0}%')", nickname);
            }
            if (!string.IsNullOrWhiteSpace(timek) && !string.IsNullOrWhiteSpace(timej))
            {
                tr.AppendFormat(" and CONVERT(varchar(10),consumptionDate,120)>='{0}' and CONVERT(varchar(10),consumptionDate,120)<='{1}'", timek, timej);
            }
            Model.JH_SYS_USER user = SetCookie.GetUserInfo();
            if (user.UserType == "1")
            {
                tr.Append(" and exists(select id from JH_ScenicList as b where b.Scenicid=Scenic_id and User_id=" + user.UserID + ")");
            }

            Model.JH_Activation model = new Model.JH_Activation();
            List<object> m = DBUtility.ORM.GetList(model, pagesize, page, out totalCount, string.Format("1=1 {0} order by {1} {2}", tr.ToString(), sortname, sortorder));
            List<object> _m = new List<object>();
            foreach (Model.JH_Activation item in m)
            {
                var rowslist = new
                {
                    id = item.id,
                    openid = item.openid,
                    ScenicSpotName = ScenicSpotName(item.Scenic_id),
                    money = item.money,
                    consumptionDate = item.consumptionDate,
                    paymentType = item.paymentType
                };
                _m.Add(rowslist);
            }
            var griddata = new { Rows = _m, Total = totalCount };
            c.Response.Write(JsonConvert.SerializeObject(griddata));
        }
        string ScenicSpotName(int id)
        {
            Model.JH_ScenicSpot model = Repository.BaseBll<Model.JH_ScenicSpot>.Get(new Model.JH_ScenicSpot() { id = id });
            if (model != null)
            {
                return model.MapName;
            }
            else
            {
                return string.Empty;
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}