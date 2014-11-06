using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Utils
{
    public class DownloadFileActionResult : ActionResult
    {
        public GridView ExcelGridView { get; set; }
        public string fileName { get; set; }


        public DownloadFileActionResult(GridView gv, string pFileName)
        {
            ExcelGridView = gv;
            fileName = pFileName;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            //Create a response stream to create and write the Excel file
            //HttpContext curContext = HttpContext.Current;
            //curContext.Response.Clear();
            //curContext.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            //curContext.Response.Charset = "";
            //curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //curContext.Response.ContentType = "application/vnd.ms-excel";

            //Convert the rendering of the gridview to a string representation 
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            ExcelGridView.RenderControl(htw);

            //Open a memory stream that you can use to write back to the response
            byte[] byteArray = Encoding.UTF8.GetBytes(sw.ToString());
            //   MemoryStream s = new MemoryStream(byteArray);
            //    StreamReader sr = new StreamReader(s, Encoding.UTF8);

            //  Write the stream back to the response
            //  curContext.Response.Write(sr.ReadToEnd());
            // curContext.Response.End();

            string path = System.Web.HttpContext.Current.Server.MapPath("~/StorageReport");

            FileStream file = File.Create(path + "/" + String.Format("{0}.xls", Guid.NewGuid()));
            file.Write(byteArray, 0, byteArray.Length);

            byteArray = Encoding.UTF8.GetBytes(sw.ToString());
            file.Write(byteArray, 0, byteArray.Length);

            byteArray = Encoding.UTF8.GetBytes(sw.ToString());
            file.Write(byteArray, 0, byteArray.Length);

            file.Close();
        }
    }
}
