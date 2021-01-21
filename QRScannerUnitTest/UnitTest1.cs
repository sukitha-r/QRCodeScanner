using Xunit;
using QRReader.Helper;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Moq;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using QRReader.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QRScannerUnitTest
{
    public class UnitTest1
    {
        
        [Fact]
        public void Test1()
        {
            var jsonString = "'{'type':'qrcode','symbol':[{'seq':0,'data':'Example','error':null}]}'";
            var logger = new NullLogger<QRCodeHelper>();
            var myConfiguration = new Dictionary<string, string>
            {
                {"GOQrEndPoints:Create_EndPoint", "http://api.qrserver.com/v1/create-qr-code"},
                {"GOQrEndPoints:Read_EndPoint", "https://api.qrserver.com/v1/read-qr-code/"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            var mockFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonString),
                });
          
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            QRCodeHelper scanner = new QRCodeHelper(mockFactory.Object, configuration, logger);
            QRCodeToText qrCode = new QRCodeToText();
            
            qrCode.Image = "../../../image.png";
            var result = scanner.QRCodeToTextConvertor(qrCode);

            Assert.NotNull(result.Result.Value);
            Assert.Contains("Example", JsonConvert.SerializeObject(result.Result.Value));

        }
    }
}
