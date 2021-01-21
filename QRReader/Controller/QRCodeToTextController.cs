using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QRReader.ViewModels;
using AutoMapper;
using QRReader.Models;
using QRReader.Helper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace QRReader.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRCodeToTextController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly QRCodeHelper _helper;
        private readonly ILogger<QRCodeToTextController> _logger;

        public QRCodeToTextController(IMapper mapper, 
            QRCodeHelper helper, 
            ILogger<QRCodeToTextController> logger)
        {
            _mapper = mapper;
            _helper = helper;
            _logger = logger;
        }

        [HttpPost("/getQRCodeText")]
        public async Task<IActionResult> GetQRCodeText(VMQRCodeToText qrCode)
        {
            _logger.LogInformation("Received "+ qrCode + " to be scanned");

            //Converting the view model to model using automapper
            var qrCodeDTO = _mapper.Map<QRCodeToText>(qrCode);
            var jsonResult = await _helper.QRCodeToTextConvertor(qrCodeDTO);
           
            return Ok(jsonResult.Value);
        }

    }
}
