using System.Web.Mvc;

namespace CalculatorZd.Models
{
    public class BinaryContentResult : ActionResult
    {
        public BinaryContentResult()
        {
        }

        // Properties for encapsulating http headers. 
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }

        // The code sets the http headers and outputs the file content. 
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ClearContent();
            context.HttpContext.Response.ContentType = ContentType;

            context.HttpContext.Response.AddHeader("content-disposition",
                "attachment; filename=" + FileName);

            context.HttpContext.Response.BinaryWrite(Content);
            context.HttpContext.Response.End();
        }
    } 
}