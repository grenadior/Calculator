using System.IO;
using System.Web.Mvc;

namespace ATI.Web.Site.MVC.Helpers
{
    public static class RenderHelperExtension
    {
        public static string RenderPartialViewToString(this Controller controller, string viewName = null, object model = null)
        {
            return RenderPartialViewToString(controller.ControllerContext, controller.ViewData, controller.TempData, viewName, model);
        }

        private static string RenderPartialViewToString(
            ControllerContext context,
            ViewDataDictionary viewData,
            TempDataDictionary tempData,
            string viewName = null,
            object model = null)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            viewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}