using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVCHomeWork.Infrastructure.ActionResults {

    /// <summary>
    /// 透過繼承ActionResult實做一個會出EXCLE的方法
    /// </summary>
    /// <remarks>
    /// 搭配 ClosedXML 做EXCEL匯出功能
    /// </remarks>
    public class ExportExcelResult : ActionResult {

        /// <summary>
        /// 頁籤名稱
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 轉出EXCEL的資料來源（Datatable）
        /// </summary>
        public DataTable ExportData { get; set; }

        public ExportExcelResult() {

        }

        public override void ExecuteResult(ControllerContext context) {
            if (ExportData == null) {
                throw new InvalidDataException("ExportData");
            }
            if (string.IsNullOrWhiteSpace(this.SheetName)) {
                this.SheetName = "Sheet1";
            }
            if (string.IsNullOrWhiteSpace(this.FileName)) {
                this.FileName = string.Concat(
                    "ExportData_",
                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ".xlsx");
            }

            this.ExportExcelEventHandler(context);
        }

        /// <summary>
        /// Exports the excel event handler.
        /// </summary>
        /// <param name="context">The context.</param>
        private void ExportExcelEventHandler(ControllerContext context) {
            try {
                
                // 建立工作簿
                var workbook = new XLWorkbook();

                if (this.ExportData != null) {
                    context.HttpContext.Response.Clear();

                    // 編碼
                    context.HttpContext.Response.ContentEncoding = Encoding.UTF8;

                    // 設定網頁ContentType
                    context.HttpContext.Response.ContentType =
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    // 匯出檔名
                    var browser = context.HttpContext.Request.Browser.Browser;

                    var exportFileName = browser.Equals("Firefox", StringComparison.OrdinalIgnoreCase)
                        ? this.FileName
                        : HttpUtility.UrlEncode(this.FileName, Encoding.UTF8);  //針對 瀏覽器判斷 做匯出檔名處理

                    context.HttpContext.Response.AddHeader(
                        "Content-Disposition",
                        string.Format("attachment;filename={0}", exportFileName));

                    // Add all DataTables in the DataSet as a worksheets
                    workbook.Worksheets.Add(this.ExportData, this.SheetName);

                    using (var memoryStream = new MemoryStream()) {
                        workbook.SaveAs(memoryStream);
                        memoryStream.WriteTo(context.HttpContext.Response.OutputStream);
                        memoryStream.Close();
                    }
                }
                workbook.Dispose();
            } catch (Exception ex) {
                throw ex;
            }
        }
    }
}