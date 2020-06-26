using csc_Task4_finale.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VoiceRSS_SDK;

namespace csc_Task4_finale.Controllers
{
    public class TTSController : ApiController
    {
        //[Route("api/TTS")]
        // GET api/<controller>/5
        public HttpResponseMessage Get()
        {
            byte[] ImageByte = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "voice.mp3"));
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream oMemoryStream = new MemoryStream(ImageByte);
            result.Content = new StreamContent(oMemoryStream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/mp3");

            return result;
        }

        // POST api/<controller>
        public IHttpActionResult Post(TextToConvert text)
        {
            
            var apiKey = "cc5df482d9c54f3c89d4bf742e7de7d8";
            var isSSL = false;
            var lang = Languages.English_UnitedStates;
            try
            {
                 var voiceParams = new VoiceParameters(text.Text, lang)
                            {
                                AudioCodec = AudioCodec.MP3,
                                AudioFormat = AudioFormat.Format_44KHZ.AF_44khz_16bit_stereo,
                                IsBase64 = false,
                                IsSsml = false,
                                SpeedRate = 0
                            };
                var voiceProvider = new VoiceProvider(apiKey, isSSL);
                var voice = voiceProvider.Speech<byte[]>(voiceParams);
                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "voice.mp3");
                File.WriteAllBytes(fileName, voice);
                return Ok();
            } catch(Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, new { message = ex.Message });
            }
        }
    }
}