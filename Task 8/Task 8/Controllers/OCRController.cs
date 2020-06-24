using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cloudmersive.APIClient.NETCore.OCR.Api;
using Cloudmersive.APIClient.NETCore.OCR.Client;
using Cloudmersive.APIClient.NETCore.OCR.Model;
using System.IO;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Microsoft.AspNetCore.Hosting;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Task_8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OCRController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OCRController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            Configuration.Default.AddApiKey("Apikey", "");
            var apiInstance = new ImageOcrApi();

            var client = new ClarifaiClient("");

            if (Request.Form.Files.GetFile("image_file") == null)
            {
                return BadRequest(new { message = "No file was found in the request." });
            }


            try
            {

                var image = Request.Form.Files.GetFile("image_file");

                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + ".jpg");

                var stream = new FileStream(fileName, FileMode.Create);
                await image.CopyToAsync(stream);
                stream.Close();

                var imageFile = new FileStream(fileName, FileMode.Open);

                // Recognize a photo of a receipt, extract key business information
                ReceiptRecognitionResult cloudmersiveResult = apiInstance.ImageOcrPhotoRecognizeReceipt(imageFile);
                imageFile.Close();

                var clarifaiResult = await client.PublicModels.GeneralModel.Predict(new ClarifaiFileImage(System.IO.File.ReadAllBytes(fileName))).ExecuteAsync();

                System.IO.File.Delete(fileName);

                var result = new
                {
                    address = cloudmersiveResult.AddressString,
                    businessName = cloudmersiveResult.BusinessName,
                    businessWebsite = cloudmersiveResult.BusinessWebsite,
                    phoneNumber = cloudmersiveResult.PhoneNumber,
                    receiptItems = cloudmersiveResult.ReceiptItems,
                    receiptSubtotal = cloudmersiveResult.ReceiptSubTotal,
                    receiptTotal = cloudmersiveResult.ReceiptTotal,
                    clarifaiTags = clarifaiResult.Get().Data
                };
                return Ok(result);

            }
            catch (Exception e)
            {

                return BadRequest(new { message = "An exception occurred while trying to tag and recognise receipt content. The error is: " + e.Message });
            }
        }
        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
