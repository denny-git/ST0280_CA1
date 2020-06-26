using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Extra_feature__Quan_Feng_.Controllers
{
    public class DownloadController : ApiController
    {
        public HttpResponseMessage Get()
        {
            byte[] file = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "document.pdf"));
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream oMemoryStream = new MemoryStream(file);
            result.Content = new StreamContent(oMemoryStream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return result;
        }
    }
}
