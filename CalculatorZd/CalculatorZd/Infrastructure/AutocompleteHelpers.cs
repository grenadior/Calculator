using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace AutoCompleteSample.Infrastructure
{
    public static class AutocompleteHelpers
    {
        public static MvcHtmlString AutocompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string actionName, string controllerName)
        {
            string autocompleteUrl = UrlHelper.GenerateUrl(null, actionName, controllerName,
                                                           null,
                                                           html.RouteCollection,
                                                           html.ViewContext.RequestContext,
                                                           includeImplicitMvcValues: true);
            return html.TextBoxFor(expression, new { data_autocomplete_url = autocompleteUrl });
        }
        private const string AutoCompleteControllerKey = "AutoCompleteController";
        private const string AutoCompleteActionKey = "AutoCompleteAction";
        public static void SetAutoComplete(this ModelMetadata metadata, string controller, string action)
        {
            metadata.AdditionalValues[AutoCompleteControllerKey] = controller;
            metadata.AdditionalValues[AutoCompleteActionKey] = action;
        }
        public static string GetAutoCompleteUrl(this HtmlHelper html, ModelMetadata metadata)
        {
            string controller = metadata.AdditionalValues.GetString(AutoCompleteControllerKey);
            string action = metadata.AdditionalValues.GetString(AutoCompleteActionKey);
            if (string.IsNullOrEmpty(controller)
                || string.IsNullOrEmpty(action))
            {
                return null;
            }
            return UrlHelper.GenerateUrl(null, action, controller, null, html.RouteCollection, html.ViewContext.RequestContext, true);
        }
        private static string GetString(this IDictionary<string, object> dictionary, string key)
        {
            object value;
            dictionary.TryGetValue(key, out value);
            return (string)value;
        }
    }
}