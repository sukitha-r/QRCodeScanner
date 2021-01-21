using QRReader.Interfaces;

namespace QRReader.Models
{
    public class TextToQRCode : ITextToQRCode
    {
        public string Text { get; set; }
    }
}