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
using Microsoft.AspNetCore.Http;
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

            FileStream stream = null, imageFile = null;

            if (Request.Form.Files.GetFile("image_file") == null)
            {
                return BadRequest(new { message = "No file was found in the request." });
            }

            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + ".jpg");
            try
            {

                var image = Request.Form.Files.GetFile("image_file");


                stream = new FileStream(fileName, FileMode.Create);
                await image.CopyToAsync(stream);
                stream.Close();

                imageFile = new FileStream(fileName, FileMode.Open);

                // Recognize a photo of a receipt, extract key business information
                ReceiptRecognitionResult cloudmersiveResult = apiInstance.ImageOcrPhotoRecognizeReceipt(imageFile);
                imageFile.Close();

                //get tags of image using Clarifai public general model
                var clarifaiTagsResult = await client.PublicModels.GeneralModel.Predict(new ClarifaiFileImage(System.IO.File.ReadAllBytes(fileName))).ExecuteAsync();

                //get probability that image is a receipt using custom trained model
                var clarifaiReceiptResult = await client.Predict<Concept>("Receipt_Recognition", new ClarifaiFileImage(System.IO.File.ReadAllBytes(fileName))).ExecuteAsync();

                System.IO.File.Delete(fileName);

                var clarifaiReceiptProbability = clarifaiReceiptResult.Get().Data;

                var clarifaiTags = clarifaiTagsResult.Get().Data;

                var result = new
                {
                    clarifaiTags = clarifaiTags,
                    address = cloudmersiveResult.AddressString,
                    businessName = cloudmersiveResult.BusinessName,
                    businessWebsite = cloudmersiveResult.BusinessWebsite,
                    phoneNumber = cloudmersiveResult.PhoneNumber,
                    receiptItems = cloudmersiveResult.ReceiptItems,
                    receiptSubtotal = cloudmersiveResult.ReceiptSubTotal,
                    receiptTotal = cloudmersiveResult.ReceiptTotal,
                    receiptProbability = clarifaiReceiptProbability
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                System.IO.File.Delete(fileName);
                stream.Close();
                imageFile.Close();
                return BadRequest(new { message = "An exception occurred while trying to tag and recognise receipt content. The error is: " + e.Message });
            }
        }
    }
}
