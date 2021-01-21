using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QRReader.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace QRReader.Helper
{
    public class QRCodeHelper
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<QRCodeHelper> _logger;

        public QRCodeHelper(IHttpClientFactory clientFactory, 
            IConfiguration config, 
            ILogger<QRCodeHelper> logger)
        {
            //dependency injection from startup
            _clientFactory = clientFactory;
            _config = config;
            _logger = logger;
        }

        public async Task<JsonResult> QRCodeToTextConvertor(QRCodeToText qrCode)
        {

            try
            {
                _logger.LogInformation("Starting the QR scan process");

                //Reading the endpoint data from appsettings.json
                var readEndpoint = _config.GetSection("GOQrEndPoints").GetSection("Read_EndPoint").Value;

                //Reading the bytes from image
                byte[] imageBytes = File.ReadAllBytes(qrCode.Image);

                var form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(imageBytes);

                //Adding the content type as "multipart/form-data" as suggested in the API
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "file", qrCode.Image);

                //Creating httpclient instance
                using (var client = _clientFactory.CreateClient())
                {
                    _logger.LogInformation("Sending the request to: " + readEndpoint);

                    var response = await client.PostAsync(readEndpoint, form);
                    var responseJson = await response.Content.ReadAsStringAsync();

                    //check if the request failed and return an error response code
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError(responseJson);
                        throw new Exception("Failed to receive scanned data from endpoint: " + responseJson);
                    }

                    //return the value back as json
                    return new JsonResult(responseJson);
                }                   

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        public async Task<Stream> TextToQRConvertor(string text)
        {
            try
            {
                _logger.LogInformation("Starting the QR scan process");

                //read endpoint information from appsettings.json
                var createEndPoint = _config.GetSection("GOQrEndPoints").GetSection("Create_EndPoint").Value;

                //Concatenate query string to the endpoint with the text passed
                var uri = QueryHelpers.AddQueryString(createEndPoint, "data", text);

                //create a httpclient instance
                using (var client = _clientFactory.CreateClient())
                {
                    var response = await client.GetAsync(uri);

                    if(!response.IsSuccessStatusCode)
                    {
                        var errMessage = "Failed to receive QRCode from endpoint: " + createEndPoint;
                        _logger.LogError(errMessage);
                        throw new Exception(errMessage);
                    }

                    //return the stream back to be downloaded as a file
                    Stream content = await response.Content.ReadAsStreamAsync();
                    return content;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

    }
}
