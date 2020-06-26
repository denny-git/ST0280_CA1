using ConvertApiDotNet;
using ConvertApiDotNet.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Extra_feature__Quan_Feng_.Controllers
{
    public class UploadController : ApiController
    {
        //create api instance
        ConvertApi convertApi = new ConvertApi("");
        // POST: api/Upload
        public async Task<IHttpActionResult> Post()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0) //check if request contains file
            {
                //get file from the request
                foreach (string file_Name in httpRequest.Files.Keys)
                {
                    var file = httpRequest.Files[file_Name];
                    if (file == null || file.ContentLength == 0) //check if file is bad
                    {
                        return Content(HttpStatusCode.BadRequest, new { message = "Length of received file is 0. Either you submitted an empty file, or no file was selected before submission." });
                    }

                    //file paths to store the uploaded and converted files in
                    var file_Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "document.docx");
                    var converted_file_Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "document.pdf");

                    file.SaveAs(file_Path);

                    try
                    {
                        //ConvertAPI client code to convert docx to pdf

                        ConvertApiResponse result = await convertApi.ConvertAsync("docx", "pdf", new[] {

                            new ConvertApiFileParam(File.OpenRead(file_Path), "document.docx")

                        });
                        File.Delete(converted_file_Path);
                        // save to file
                        var fileInfo = await result.SaveFileAsync(converted_file_Path);

                        return Ok();
                    }
                    catch (Exception e)
                    {
                        return Content(HttpStatusCode.BadRequest, new { message = "An error occurred while attempting to convert the file to PDF. The error is: " + e.Message });
                    }
                }
            }
            
            return Content(HttpStatusCode.BadRequest, new { message = "No file was found in the request." });
        }

    }
}
