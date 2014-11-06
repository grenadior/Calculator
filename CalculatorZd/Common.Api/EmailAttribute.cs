using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Api.Validation
{
    /// <summary>
    /// Email Validation Attribute
    /// </summary>
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base("^[a-zA-Z0-9_\\+-]+(\\.[a-zA-Z0-9_\\+-]+)*@[a-zA-Z0-9-]+(\\.[a-zA-Z0-9-]+)*\\.([a-zA-Z]{2,4})$") { }
    }
}