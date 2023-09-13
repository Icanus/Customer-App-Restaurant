using FoodApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodApp.Utilities
{
    public class CountryManager
    {
        private List<CountryModel> countryCodesWithPrefix = new List<CountryModel>
        {
            new CountryModel
            {
                CountryCode = "AF",
                CountryName = "Afganisthan",
                FlagUrl = "AF"
            },
             new CountryModel
            {
                CountryCode = "AL",
                CountryName = "Afganisthan",
                FlagUrl = "AL"
            },
             new CountryModel
            {
                CountryCode = "DZ",
                CountryName = "Afganisthan",
                FlagUrl = "DZ"
            },
             new CountryModel
            {
                CountryCode = "AD",
                CountryName = "Afganisthan",
                FlagUrl = "AD"
            },
             new CountryModel
            {
                CountryCode = "AO",
                CountryName = "Afganisthan",
                FlagUrl = "AO"
            },
             new CountryModel
            {
                CountryCode = "AR",
                CountryName = "Afganisthan",
                FlagUrl = "AR"
            },

             new CountryModel
            {
                CountryCode = "FJ",
                CountryName = "Fiji",
                FlagUrl = "FJ"
            },
             new CountryModel
            {
                CountryCode = "PH",
                CountryName = "Philippines",
                FlagUrl = "PH"
            },
             
            };

            public CountryModel GetFlagByCountryCode(string countryCode)
            {
                return countryCodesWithPrefix.Where(x=>x.CountryCode == countryCode).FirstOrDefault();
            }
    }

}
