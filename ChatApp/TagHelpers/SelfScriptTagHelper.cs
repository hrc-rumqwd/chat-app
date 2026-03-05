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

        override public void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "script";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("type", "text/javascript");

            StringBuilder sb = new StringBuilder();
            var filePath = ViewContext.ExecutingFilePath;
            sb.Append(filePath);
            sb.Append(".js");
            output.Attributes.SetAttribute("src", sb.ToString());
        }
    }
}
