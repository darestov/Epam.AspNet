using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.AspNet.Module1.Helpers
{
    public static class ImageHelpers
    {
        public static IHtmlContent NorthwindImageLink(this IHtmlHelper html, int imageId, string linkText)
        {
            return html.ActionLink(linkText, "Image", "Categories", new { id = imageId });
        }
    }
}
