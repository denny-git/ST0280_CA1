using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Antlr.Runtime;
using Newtonsoft.Json;

namespace csc_Task4_finale.Controllers
{
    public class UploadController : ApiController
    {   
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static readonly string bucketName = "csctask5";
        static readonly IAmazonS3 s3Client = new AmazonS3Client(bucketRegion);
        string authToken = "";
        [Route("api/upload")]
        
        private static async Task<PutObjectResponse> UploadFileAsync(string filePath, string keyName)
        {
                var putRequest = new PutObjectRequest
                            {
                                BucketName = bucketName,
                                Key = keyName,
                                FilePath = filePath,
                                CannedACL = S3CannedACL.PublicRead
                            };
                PutObjectResponse response = await s3Client.PutObjectAsync(putRequest);
                return response;
        }

        // GET: api/Upload
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Upload/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Upload
        public async Task<IHttpActionResult> Post()
        {
            string s3Url = "";
            string keyName = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                if (authToken.Equals(""))
                {   try
                    {
                        HttpResponseMessage authTokenResponse = await AuthToken();
                        if (!authTokenResponse.IsSuccessStatusCode)
                        {
                            return Content(HttpStatusCode.BadRequest, new { message = "Unable to get bit.ly authentication token" });
                        } else
                        {
                            authToken = await authTokenResponse.Content.ReadAsStringAsync();
                        }
                    } catch(Exception e)
                    {
                        return Content(HttpStatusCode.BadRequest, new { message = "An error occurred while attempting to get the bit.ly authentication token. The error is: " + e.Message });
                    }   
                }

                foreach (string file_Name in httpRequest.Files.Keys)
                {
                    var file = httpRequest.Files[file_Name];
                    if(file.ContentLength == 0)
                    {
                        return Content(HttpStatusCode.BadRequest, new { message = "Length of received file is 0. Either you submitted an empty file, or no file was selected before submission." });
                    }
                    var file_Path = HttpContext.Current.Server.MapPath("~/image.jpg");
                    file.SaveAs(file_Path);
                    try
                    {
                        PutObjectResponse uploadResponse = await UploadFileAsync(file_Path, keyName);

                        if(uploadResponse.HttpStatusCode == HttpStatusCode.OK)
                        {
                            s3Url = "https://" + bucketName + ".s3.amazonaws.com/" + keyName;
                        } else
                        {
                            return Content(HttpStatusCode.BadRequest, new { message = "An error occurred while attempting to upload the file to Amazon S3. AWS returned the following HTTP status code: " + uploadResponse.HttpStatusCode });
                        }
                    }
                    catch (Exception e)
                    {
                        return Content(HttpStatusCode.BadRequest, new { message = "An error occurred while attempting to upload the file to Amazon S3. The error is: " + e.Message });
                    }
                }
                
                try
                {
                    var shortenLinkResult = await ShortenLink(authToken, s3Url);

                    if (shortenLinkResult.IsSuccessStatusCode)
                    {
                        var shortenLinkContent = await shortenLinkResult.Content.ReadAsStringAsync();

                        dynamic shortenLinkObj = JsonConvert.DeserializeObject<object>(shortenLinkContent);

                        return Ok(new { message = "Successfully uploaded image to Amazon S3! The link to the image is <a href=\"" + shortenLinkObj.link + "\">" + shortenLinkObj.link + "</a>." });
                    } else
                    {
                        return Content(HttpStatusCode.BadRequest, new { message = "Image was uploaded to S3, but the link could not be shortened. You may shorten the <a href=\"" + s3Url + "\">image link</a> manually instead." });
                    }
                } catch (Exception e)
                {
                    return Content(HttpStatusCode.BadRequest, new { message = "The image was uploaded to S3, but an exception occurred while trying to shorten the link. The error is: " + e.Message + ". You may shorten the <a href=\"" + s3Url + "\">image link</a> manually instead." });
                }
            } else
            {
                return Content(HttpStatusCode.BadRequest, new { message = "No file was specified" });
            }
        }

        static async Task<HttpResponseMessage> AuthToken()
        {
            var userName = "";
            var passwd = "";
            var url = "https://api-ssl.bitly.com/oauth/access_token";

            var client = new HttpClient();


            var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(authToken));

            var result = await client.PostAsync(url, null);

            return result;
        }

        static async Task<HttpResponseMessage> ShortenLink(string authToken, string urlToShorten)
        {
            var client = new HttpClient();
            var url = "https://api-ssl.bitly.com/v4/bitlinks";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    authToken);
            string json = "{\"long_url\":\"" + urlToShorten + "\"}"; 

            var content = new StringContent(
                              json,
                              System.Text.Encoding.UTF8,
                              "application/json"
                              );
            var result = await client.PostAsync(url, content);
            return result;
        }

        // DELETE: api/Upload/5
        public void Delete(int id)
        {
        }
    }
}
