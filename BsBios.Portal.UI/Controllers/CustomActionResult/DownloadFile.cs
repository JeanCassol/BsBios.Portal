using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BsBios.Portal.UI.Controllers.CustomActionResult
{
    public class DownloadFileActionResult : ActionResult
    {
        public string ExcelGridView { get; set; }
        public string FileName { get; set; }

        public DownloadFileActionResult(string gridView, string fileName)
        {
            ExcelGridView = gridView;
            FileName = fileName;

        }

        public override void ExecuteResult(ControllerContext context)
        {

            //Create a response stream to create and write the Excel file
            HttpContext curContext = HttpContext.Current;
            curContext.Response.Clear();
            curContext.Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
            curContext.Response.Charset = "";
            curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            curContext.Response.ContentType = "application/ms-excel";
            //curContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //Open a memory stream that you can use to write back to the response
            byte[] byteArray = Encoding.ASCII.GetBytes(ExcelGridView);
            MemoryStream s = new MemoryStream(byteArray);
            StreamReader sr = new StreamReader(s, Encoding.Unicode);

            //FileStream fileStream = new FileStream("c:\\tmp\\RelatorioSalvo.xls",FileMode.Create);
            //fileStream.Write(byteArray, 0, byteArray.Length);

            //Write the stream back to the response
            curContext.Response.ContentEncoding = System.Text.Encoding.Unicode;
            curContext.Response.Write(sr.ReadToEnd());

            //fileStream.Close();
            curContext.Response.End();

        }
    }
}