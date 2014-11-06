using System;
using System.Web.Mvc;

namespace AutoCompleteSample.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoCompleteAttribute : Attribute, IMetadataAware
    {
        private readonly string _controller;
        private readonly string _action;

        public AutoCompleteAttribute(string controller, string action)
        {
            _controller = controller;
            _action = action;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.SetAutoComplete(_controller, _action);
        }
    }
}