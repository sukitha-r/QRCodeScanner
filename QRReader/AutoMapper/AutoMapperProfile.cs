using AutoMapper;
using QRReader.Models;
using QRReader.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRReader.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VMQRCodeToText, QRCodeToText>();

            CreateMap<VMTextToQRCode, TextToQRCode>();
        }
       
    }
}
