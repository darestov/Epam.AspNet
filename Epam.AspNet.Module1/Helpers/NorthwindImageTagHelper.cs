using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.AspNet.Module1.Helpers
{
    [HtmlTargetElement(Attributes = "northwind-id")]
    public class NorthwindImageTagHelper: TagHelper
    {
        public int NorthwindId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("href", "/Categories/Image/" + NorthwindId);
        }
    }
}
