#pragma checksum "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9b18af10e6de197c56b3eb12a91ee6ef6469daca"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
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
#line 2 "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml"
using Microsoft.AspNetCore.Authentication;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9b18af10e6de197c56b3eb12a91ee6ef6469daca", @"/Views/Home/Index.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<string>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(59, 9, true);
            WriteLiteral("\r\n<div>\r\n");
            EndContext();
#line 5 "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml"
     if (User?.Identity?.IsAuthenticated ?? false)
    {

#line default
#line hidden
            BeginContext(127, 21, true);
            WriteLiteral("        <h1>Welcome, ");
            EndContext();
            BeginContext(149, 18, false);
#line 7 "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml"
                Write(User.Identity.Name);

#line default
#line hidden
            EndContext();
            BeginContext(167, 7, true);
            WriteLiteral("</h1>\r\n");
            EndContext();
            BeginContext(176, 25, true);
            WriteLiteral("        <p>Access Token: ");
            EndContext();
            BeginContext(202, 43, false);
#line 9 "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml"
                    Write(await Context.GetTokenAsync("access_token"));

#line default
#line hidden
            EndContext();
            BeginContext(245, 19, true);
            WriteLiteral("</p>\r\n        <p>\r\n");
            EndContext();
#line 11 "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml"
             foreach (var claim in Context.User.Claims)
            {

#line default
#line hidden
            BeginContext(336, 21, true);
            WriteLiteral("                <div>");
            EndContext();
            BeginContext(358, 10, false);
#line 13 "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml"
                Write(claim.Type);

#line default
#line hidden
            EndContext();
            BeginContext(368, 5, true);
            WriteLiteral(": <b>");
            EndContext();
            BeginContext(374, 11, false);
#line 13 "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml"
                                Write(claim.Value);

#line default
#line hidden
            EndContext();
            BeginContext(385, 12, true);
            WriteLiteral("</b></div>\r\n");
            EndContext();
#line 14 "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml"
            }

#line default
#line hidden
            BeginContext(412, 14, true);
            WriteLiteral("        </p>\r\n");
            EndContext();
            BeginContext(428, 85, true);
            WriteLiteral("        <a class=\"btn btn-lg btn-danger\" href=\"/signout?returnUrl=%2F\">Sign out</a>\r\n");
            EndContext();
#line 18 "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml"
    }

    else
    {

#line default
#line hidden
            BeginContext(539, 121, true);
            WriteLiteral("        <h1>Welcome, anonymous</h1>\r\n        <a class=\"btn btn-lg btn-success\" href=\"/signin?returnUrl=%2F\">Sign in</a>\r\n");
            EndContext();
#line 24 "C:\Users\lucap\Desktop\layw-web\layw-appwebvc-2018\LaywAuthentication\Views\Home\Index.cshtml"
    }

#line default
#line hidden
            BeginContext(667, 6, true);
            WriteLiteral("</div>");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<string> Html { get; private set; }
    }
}
#pragma warning restore 1591
