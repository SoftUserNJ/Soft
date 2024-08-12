using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using Microsoft.AspNetCore.Mvc;

namespace SoftaxeERP_API
{
    [Produces("application/json")]
    [Route("DXXRDV")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomWebDocumentViewerController : WebDocumentViewerController
    {
        public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService) { }

        public override Task<IActionResult> Invoke()
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return base.Invoke();
        }
    }
}