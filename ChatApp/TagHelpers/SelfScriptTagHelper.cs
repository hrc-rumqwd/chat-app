using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace ChatApp.Web.TagHelpers
{
    [HtmlTargetElement("self-script")]
    public class SelfScriptTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "script";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("type", "text/javascript");

            StringBuilder sb = new();
            string filePath = ViewContext.View.Path;
            _ = sb.Append(filePath);
            _ = sb.Append(".js");
            output.Attributes.SetAttribute("src", sb.ToString());
        }
    }
}
