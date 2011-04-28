﻿using System.Web.Mvc;

namespace WebUI
{
    public static class InputHelpers
    {
        public static MvcHtmlString Example(this HtmlHelper helper, string message)
        {
            return MvcHtmlString.Create(@"<div class='example'>" + message + "</div>");
        }
    }
}