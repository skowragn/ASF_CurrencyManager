using System.Linq;
using AutoMapper;
using CurrencyManagerWeb.Models;
using CurrencyManagerWeb.Dtos;
using System.Collections.Generic;

namespace CurrencyManagerWeb.Mappers
{
    public class CountriesMappingProfile : Profile
        {
            public CountriesMappingProfile()
            {
                _ =  CreateMap<CountryDto, CountryViewModel>()
                    .ForMember(item => item.Name, opt => opt.MapFrom(item => item.Name.Common))
                    .ForMember(item => item.Capital, opt => opt.MapFrom(item => item.Capital.FirstOrDefault()))
                    .ForMember(item => item.Region, opt => opt.MapFrom(item => item.Region))
                    .ForMember(item => item.Currency, opt => opt.MapFrom((src, dest) =>
                    {
                        return dest.Currency = src.Currencies!=null ? src.Currencies.Values : new List<CurrenciesType>() {new CurrenciesType() {Name = string.Empty, Symbol = string.Empty} };
                    }))
                    .ForMember(item => item.Flag, opt => opt.MapFrom((src, dest) =>
                    {
                        return dest.Flag = src.FlagPath != null ? src.FlagPath["png"] : string.Empty;
                    }));


            }
        }
    }