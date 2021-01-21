# QRCodeScanner

#Steps to run:

1. Download the project and open the solution "QRReader.sln" in Visual Studio
2. Run the project, it will open Swagger UI.
3. TextToQRCode has an endpoint "/api/TextToQRCode" - Click on that - Select "Try it out".
4. Enter a string that you would like to be converted to a QRCode.
5. Click on "Execute"
6. You should have a "Download File" link appearing - click on that and save the image in your local machine in your desired location

7. QRCodeToText has an endpoint "/getQRCodeText" - Click on that - Select "Try it out".
8. In the request body please enter the image location that you just downloaded, along with your QRCode image name. Please use the below format for the body of this POST request
{
  "image": "<<Your image location>> \\ <<Image name with extension""
}
9. Please use \\ in the file path above - for exampls: C:\\Users\\<<User>>\\Documents\\Dev
10. Click on "Execute" and you would see a JSON result that has the value of the QRCode
  
#Steps to run Unit Test Case:

1. Download the project and open the solution "QRReader.sln" in Visual Studio
2. Right click on "QRScannerUnitTest" and select "Run Tests".
