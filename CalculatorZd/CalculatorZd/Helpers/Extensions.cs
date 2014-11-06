using System.Web.Mvc;

namespace ATI.Web.Site.MVC.Helpers
{
    public static class Extensions
    {
        public static MvcHtmlString ResolveUrl(this HtmlHelper htmlHelper, string url)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            return MvcHtmlString.Create(urlHelper.Content(url));
        }
    }
}