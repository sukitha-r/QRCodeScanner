using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRReader.Helper;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace QRReader.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextToQRCodecontroller : ControllerBase
    {
        private readonly QRCodeHelper _helper;
        private readonly ILogger<TextToQRCodecontroller> _logger;

        public TextToQRCodecontroller(IMapper mapper, 
            QRCodeHelper helper, 
            ILogger<TextToQRCodecontroller> logger)
        {
            _helper = helper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetQRCode(string keyword)
        {
            _logger.LogInformation("Received " + keyword + " to be converted to QRCode");
            var stream = await _helper.TextToQRConvertor(keyword);

            //convert stream to image
            Image img = Image.FromStream(stream);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            //download the image
            return File(ms.ToArray(), "image/png", "qrcode.png");
        }
    }
}
