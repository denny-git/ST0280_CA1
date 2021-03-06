#pragma checksum "D:\CSC\CA1\code\Task 8\Task 8\Views\Home\OCR.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f17f5ad78ed2ef0476ca47f9e1d1d86869f8dcfb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_OCR), @"mvc.1.0.view", @"/Views/Home/OCR.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\CSC\CA1\code\Task 8\Task 8\Views\_ViewImports.cshtml"
using Task_8;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\CSC\CA1\code\Task 8\Task 8\Views\_ViewImports.cshtml"
using Task_8.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f17f5ad78ed2ef0476ca47f9e1d1d86869f8dcfb", @"/Views/Home/OCR.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"49a34992453d840fcf022b33114117a99216fcf2", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_OCR : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("formElem"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "D:\CSC\CA1\code\Task 8\Task 8\Views\Home\OCR.cshtml"
  
    ViewData["Title"] = "Receipt OCR using Clarifai and Cloudmersive";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Receipt OCR using Clarifai and Cloudmersive</h1>\r\n<br />\r\n<h2>Upload your receipt image here for image recognition</h2>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f17f5ad78ed2ef0476ca47f9e1d1d86869f8dcfb4201", async() => {
                WriteLiteral("\r\n    <input type=\"file\" name=\"image_file\" accept=\".jpg,.jpeg,.png\">\r\n    <input type=\"submit\">\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
<br />
<h2>Receipt image info</h2>
<br />
<div id=""div"">

</div>
<script>
    function NullReplacer(valueToCheck) {
        if (valueToCheck === null) {
            return '-';
        } else {
            return valueToCheck;
        }
    }
    async function sendImage() {
        var networkError = false;
        let res = await fetch('/api/OCR', {
            method: 'POST',
            body: new FormData(formElem)

        }).catch(error => {
            networkError = true;
            document.getElementById(""div"").innerHTML = ""<img src=\""/ajax-loader.gif\""> Encountered an error, retrying..."";
            setTimeout(sendImage, 3000);
        })
        if (!networkError) {
            if (res.status === 200) {
                var respObj = await res.json();

                html = """";
                html += ""<h4>Probability that image is a receipt (Clarifai)</h4>""
                if (respObj.receiptProbability !== null) {
                    respObj.receiptProbability");
            WriteLiteral(@".forEach(function (item, index) {
                        html += item.name + "": "" + item.value + ""<br /></br>""
                    });
                } else {
                    html += ""Clarifai did not return a receipt probability""
                }
                html += ""<h4>Business info (Cloudmersive)</h4>""
                html += ""<p>Address: "" + NullReplacer(respObj.address) + ""</p>""
                html += ""<p>Business name: "" + NullReplacer(respObj.businessName) + ""</p>""
                html += ""<p>Business website: "" + NullReplacer(respObj.businessWebsite) + ""</p>""
                html += ""<p>Phone: "" + NullReplacer(respObj.phoneNumber) + ""</p>""
                html += ""<h4>Receipt items (Cloudmersive)</h4>""
                if (respObj.receiptItems !== null) {
                    respObj.receiptItems.forEach(function (item, index) {
                        html += item.itemDescription + "": $"" + NullReplacer(item.itemPrice) + ""<br /></br>""
                    });
                ");
            WriteLiteral(@"} else {
                    html += ""No items were found in the receipt""
                }
                
                html += ""<p>Subtotal: $"" + NullReplacer(respObj.receiptSubtotal) + ""</p>""
                html += ""<p>Total: $"" + NullReplacer(respObj.receiptTotal) + ""<p>""
                html += ""<h4>Tags and probabilities (Clarifai)</h4>""
                if (respObj.clarifaiTags !== null) {
                    respObj.clarifaiTags.forEach(function (item, index) {
                        html += item.name + "": "" + item.value + ""<br />""
                    });
                } else {
                    html += ""No concepts returned by Clarifai""
                }
                
                document.getElementById(""div"").innerHTML = html;
            } else {
                var respObj = await res.json();

                document.getElementById(""div"").innerHTML = ""<font color=\""red\"">"" + await respObj.message + ""</font>"";
            }
        }

        return res;
   ");
            WriteLiteral(" }\r\n    formElem.onsubmit = async (e) => {\r\n        e.preventDefault();\r\n        document.getElementById(\"div\").innerHTML = \"<img src=\\\"/ajax-loader.gif\\\"> Processing... This will take a while\";\r\n        sendImage();\r\n    };\r\n</script>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
