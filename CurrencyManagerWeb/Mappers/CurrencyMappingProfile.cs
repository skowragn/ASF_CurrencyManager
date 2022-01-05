using AutoMapper;
using CurrencyManagerWeb.Models;
using System.Collections.Generic;

namespace CurrencyManagerWeb.Mappers
{
    public class CurrencyMappingProfile :  Profile
    {
        public CurrencyMappingProfile()
        {
            _ = CreateMap<KeyValuePair<string, int>, CurrencyViewModel>()
                .ForMember(item => item.CurrencyName, opt => opt.MapFrom(item => item.Key))
                .ForMember(item => item.CurrencyQuantity, opt => opt.MapFrom(item => item.Value));
        }
    }
}