using FoodApp.Models;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FoodApp.Utilities
{
    public static class CountryUtils
    {
        /// <summary>
        /// Gets the list of countries based on ISO 3166-1
        /// </summary>
        /// <returns>Returns the list of countries based on ISO 3166-1</returns>
        public static List<CountryModel> GetCountriesByIso3166()
        {
            //var countries = new List<RegionInfo>();
            //foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            //{
            //    var info = new RegionInfo(culture.LCID);
            //    if (countries.All(p => p.Name != info.Name))
            //        countries.Add(info);
            //}
            //return countries.OrderBy(p => p.EnglishName).ToList();
            return GetCountry();
        }

        /// <summary>
        /// Get Country Model by Country Name
        /// </summary>
        /// <param name="countryName">English Name of Country</param>
        /// <returns>Complete Country Model with Region, Flag, Name and Code</returns>
        public static CountryModel GetCountryModelByName(string countryName)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var isoCountries = GetCountriesByIso3166();
            var regionInfo = isoCountries.FirstOrDefault(c => c.CountryName == countryName);
            return regionInfo = new CountryModel
            {
                CountryCode = phoneNumberUtil.GetCountryCodeForRegion(regionInfo.Code).ToString(),
                CountryName = regionInfo.CountryName,
                FlagUrl = $"https://hatscripts.github.io/circle-flags/flags/{regionInfo.Code.ToLower()}.svg",
            };
        }

        public static List<CountryModel> GetCountry()
        {
            var countryList = new List<CountryModel>()
            {
                new CountryModel{
                    CountryName= "Afghanistan",
                    CountryCode= "+93",
                    FlagUrl= "🇦🇫",
                    Code= "AF"
                },
                new CountryModel{
                    CountryName= "Aland Islands",
                    CountryCode= "+358",
                    FlagUrl= "🇦🇽",
                    Code= "AX"
                },
                new CountryModel{
                    CountryName= "Albania",
                    CountryCode= "+355",
                    FlagUrl= "🇦🇱",
                    Code= "AL"
                },
                new CountryModel{
                    CountryName= "Algeria",
                    CountryCode= "+213",
                    FlagUrl= "🇩🇿",
                    Code= "DZ"
                },
                new CountryModel{
                    CountryName= "AmericanSamoa",
                    CountryCode= "+1684",
                    FlagUrl= "🇦🇸",
                    Code= "AS"
                },
                new CountryModel{
                    CountryName= "Andorra",
                    CountryCode= "+376",
                    FlagUrl= "🇦🇩",
                    Code= "AD"
                },
                new CountryModel{
                    CountryName= "Angola",
                    CountryCode= "+244",
                    FlagUrl= "🇦🇴",
                    Code= "AO"
                },
                new CountryModel{
                    CountryName= "Anguilla",
                    CountryCode= "+1264",
                    FlagUrl= "🇦🇮",
                    Code= "AI"
                },
                new CountryModel{
                    CountryName= "Antarctica",
                    CountryCode= "+672",
                    FlagUrl= "🇦🇶",
                    Code= "AQ"
                },
                new CountryModel{
                    CountryName= "Antigua and Barbuda",
                    CountryCode= "+1268",
                    FlagUrl= "🇦🇬",
                    Code= "AG"
                },
                new CountryModel{
                    CountryName= "Argentina",
                    CountryCode= "+54",
                    FlagUrl= "🇦🇷",
                    Code= "AR"
                },
                new CountryModel{
                    CountryName= "Armenia",
                    CountryCode= "+374",
                    FlagUrl= "🇦🇲",
                    Code= "AM"
                },
                new CountryModel{
                    CountryName= "Aruba",
                    CountryCode= "+297",
                    FlagUrl= "🇦🇼",
                    Code= "AW"
                },
                new CountryModel{
                    CountryName= "Australia",
                    CountryCode= "+61",
                    FlagUrl= "🇦🇺",
                    Code= "AU"
                },
                new CountryModel{
                    CountryName= "Austria",
                    CountryCode= "+43",
                    FlagUrl= "🇦🇹",
                    Code= "AT"
                },
                new CountryModel{
                    CountryName= "Azerbaijan",
                    CountryCode= "+994",
                    FlagUrl= "🇦🇿",
                    Code= "AZ"
                },
                new CountryModel{
                    CountryName= "Bahamas",
                    CountryCode= "+1242",
                    FlagUrl= "🇧🇸",
                    Code= "BS"
                },
                new CountryModel{
                    CountryName= "Bahrain",
                    CountryCode= "+973",
                    FlagUrl= "🇧🇭",
                    Code= "BH"
                },
                new CountryModel{
                    CountryName= "Bangladesh",
                    CountryCode= "+880",
                    FlagUrl= "🇧🇩",
                    Code= "BD"
                },
                new CountryModel{
                    CountryName= "Barbados",
                    CountryCode= "+1246",
                    FlagUrl= "🇧🇧",
                    Code= "BB"
                },
                new CountryModel{
                    CountryName= "Belarus",
                    CountryCode= "+375",
                    FlagUrl= "🇧🇾",
                    Code= "BY"
                },
                new CountryModel{
                    CountryName= "Belgium",
                    CountryCode= "+32",
                    FlagUrl= "🇧🇪",
                    Code= "BE"
                },
                new CountryModel{
                    CountryName= "Belize",
                    CountryCode= "+501",
                    FlagUrl= "🇧🇿",
                    Code= "BZ"
                },
                new CountryModel{
                    CountryName= "Benin",
                    CountryCode= "+229",
                    FlagUrl= "🇧🇯",
                    Code= "BJ"
                },
                new CountryModel{
                    CountryName= "Bermuda",
                    CountryCode= "+1441",
                    FlagUrl= "🇧🇲",
                    Code= "BM"
                },
                new CountryModel{
                    CountryName= "Bhutan",
                    CountryCode= "+975",
                    FlagUrl= "🇧🇹",
                    Code= "BT"
                },
                new CountryModel{
                    CountryName= "Bolivia, Plurinational State of",
                    CountryCode= "+591",
                    FlagUrl= "🇧🇴",
                    Code= "BO"
                },
                new CountryModel{
                    CountryName= "Bosnia and Herzegovina",
                    CountryCode= "+387",
                    FlagUrl= "🇧🇦",
                    Code= "BA"
                },
                new CountryModel{
                    CountryName= "Botswana",
                    CountryCode= "+267",
                    FlagUrl= "🇧🇼",
                    Code= "BW"
                },
                new CountryModel{
                    CountryName= "Brazil",
                    CountryCode= "+55",
                    FlagUrl= "🇧🇷",
                    Code= "BR"
                },
                new CountryModel{
                    CountryName= "British Indian Ocean Territory",
                    CountryCode= "+246",
                    FlagUrl= "🇮🇴",
                    Code= "IO"
                },
                new CountryModel{
                    CountryName= "Brunei Darussalam",
                    CountryCode= "+673",
                    FlagUrl= "🇧🇳",
                    Code= "BN"
                },
                new CountryModel{
                    CountryName= "Bulgaria",
                    CountryCode= "+359",
                    FlagUrl= "🇧🇬",
                    Code= "BG"
                },
                new CountryModel{
                    CountryName= "Burkina Faso",
                    CountryCode= "+226",
                    FlagUrl= "🇧🇫",
                    Code= "BF"
                },
                new CountryModel{
                    CountryName= "Burundi",
                    CountryCode= "+257",
                    FlagUrl= "🇧🇮",
                    Code= "BI"
                },
                new CountryModel{
                    CountryName= "Cambodia",
                    CountryCode= "+855",
                    FlagUrl= "🇰🇭",
                    Code= "KH"
                },
                new CountryModel{
                    CountryName= "Cameroon",
                    CountryCode= "+237",
                    FlagUrl= "🇨🇲",
                    Code= "CM"
                },
                new CountryModel{
                    CountryName= "Canada",
                    CountryCode= "+1",
                    FlagUrl= "🇨🇦",
                    Code= "CA"
                },
                new CountryModel{
                    CountryName= "Cape Verde",
                    CountryCode= "+238",
                    FlagUrl= "🇨🇻",
                    Code= "CV"
                },
                new CountryModel{
                    CountryName= "Cayman Islands",
                    CountryCode= "+345",
                    FlagUrl= "🇰🇾",
                    Code= "KY"
                },
                new CountryModel{
                    CountryName= "Central African Republic",
                    CountryCode= "+236",
                    FlagUrl= "🇨🇫",
                    Code= "CF"
                },
                new CountryModel{
                    CountryName= "Chad",
                    CountryCode= "+235",
                    FlagUrl= "🇹🇩",
                    Code= "TD"
                },
                new CountryModel{
                    CountryName= "Chile",
                    CountryCode= "+56",
                    FlagUrl= "🇨🇱",
                    Code= "CL"
                },
                new CountryModel{
                    CountryName= "China",
                    CountryCode= "+86",
                    FlagUrl= "🇨🇳",
                    Code= "CN"
                },
                new CountryModel{
                    CountryName= "Christmas Island",
                    CountryCode= "+61",
                    FlagUrl= "🇨🇽",
                    Code= "CX"
                },
                new CountryModel{
                    CountryName= "Cocos (Keeling) Islands",
                    CountryCode= "+61",
                    FlagUrl= "🇨🇨",
                    Code= "CC"
                },
                new CountryModel{
                    CountryName= "Colombia",
                    CountryCode= "+57",
                    FlagUrl= "🇨🇴",
                    Code= "CO"
                },
                new CountryModel{
                    CountryName= "Comoros",
                    CountryCode= "+269",
                    FlagUrl= "🇰🇲",
                    Code= "KM"
                },
                new CountryModel{
                    CountryName= "Congo",
                    CountryCode= "+242",
                    FlagUrl= "🇨🇬",
                    Code= "CG"
                },
                new CountryModel{
                    CountryName= "Congo, The Democratic Republic of the Congo",
                    CountryCode= "+243",
                    FlagUrl= "🇨🇩",
                    Code= "CD"
                },
                new CountryModel{
                    CountryName= "Cook Islands",
                    CountryCode= "+682",
                    FlagUrl= "🇨🇰",
                    Code= "CK"
                },
                new CountryModel{
                    CountryName= "Costa Rica",
                    CountryCode= "+506",
                    FlagUrl= "🇨🇷",
                    Code= "CR"
                },
                new CountryModel{
                    CountryName= "Cote d'Ivoire",
                    CountryCode= "+225",
                    FlagUrl= "🇨🇮",
                    Code= "CI"
                },
                new CountryModel{
                    CountryName= "Croatia",
                    CountryCode= "+385",
                    FlagUrl= "🇭🇷",
                    Code= "HR"
                },
                new CountryModel{
                    CountryName= "Cuba",
                    CountryCode= "+53",
                    FlagUrl= "🇨🇺",
                    Code= "CU"
                },
                new CountryModel{
                    CountryName= "Cyprus",
                    CountryCode= "+357",
                    FlagUrl= "🇨🇾",
                    Code= "CY"
                },
                new CountryModel{
                    CountryName= "Czech Republic",
                    CountryCode= "+420",
                    FlagUrl= "🇨🇿",
                    Code= "CZ"
                },
                new CountryModel{
                    CountryName= "Denmark",
                    CountryCode= "+45",
                    FlagUrl= "🇩🇰",
                    Code= "DK"
                },
                new CountryModel{
                    CountryName= "Djibouti",
                    CountryCode= "+253",
                    FlagUrl= "🇩🇯",
                    Code= "DJ"
                },
                new CountryModel{
                    CountryName= "Dominica",
                    CountryCode= "+1767",
                    FlagUrl= "🇩🇲",
                    Code= "DM"
                },
                new CountryModel{
                    CountryName= "Dominican Republic",
                    CountryCode= "+1849",
                    FlagUrl= "🇩🇴",
                    Code= "DO"
                },
                new CountryModel{
                    CountryName= "Ecuador",
                    CountryCode= "+593",
                    FlagUrl= "🇪🇨",
                    Code= "EC"
                },
                new CountryModel{
                    CountryName= "Egypt",
                    CountryCode= "+20",
                    FlagUrl= "🇪🇬",
                    Code= "EG"
                },
                new CountryModel{
                    CountryName= "El Salvador",
                    CountryCode= "+503",
                    FlagUrl= "🇸🇻",
                    Code= "SV"
                },
                new CountryModel{
                    CountryName= "Equatorial Guinea",
                    CountryCode= "+240",
                    FlagUrl= "🇬🇶",
                    Code= "GQ"
                },
                new CountryModel{
                    CountryName= "Eritrea",
                    CountryCode= "+291",
                    FlagUrl= "🇪🇷",
                    Code= "ER"
                },
                new CountryModel{
                    CountryName= "Estonia",
                    CountryCode= "+372",
                    FlagUrl= "🇪🇪",
                    Code= "EE"
                },
                new CountryModel{
                    CountryName= "Ethiopia",
                    CountryCode= "+251",
                    FlagUrl= "🇪🇹",
                    Code= "ET"
                },
                new CountryModel{
                    CountryName= "Falkland Islands (Malvinas)",
                    CountryCode= "+500",
                    FlagUrl= "🇫🇰",
                    Code= "FK"
                },
                new CountryModel{
                    CountryName= "Faroe Islands",
                    CountryCode= "+298",
                    FlagUrl= "🇫🇴",
                    Code= "FO"
                },
                new CountryModel{
                    CountryName= "Fiji",
                    CountryCode= "+679",
                    FlagUrl= "🇫🇯",
                    Code= "FJ"
                },
                new CountryModel{
                    CountryName= "Finland",
                    CountryCode= "+358",
                    FlagUrl= "🇫🇮",
                    Code= "FI"
                },
                new CountryModel{
                    CountryName= "France",
                    CountryCode= "+33",
                    FlagUrl= "🇫🇷",
                    Code= "FR"
                },
                new CountryModel{
                    CountryName= "French Guiana",
                    CountryCode= "+594",
                    FlagUrl= "🇬🇫",
                    Code= "GF"
                },
                new CountryModel{
                    CountryName= "French Polynesia",
                    CountryCode= "+689",
                    FlagUrl= "🇵🇫",
                    Code= "PF"
                },
                new CountryModel{
                    CountryName= "Gabon",
                    CountryCode= "+241",
                    FlagUrl= "🇬🇦",
                    Code= "GA"
                },
                new CountryModel{
                    CountryName= "Gambia",
                    CountryCode= "+220",
                    FlagUrl= "🇬🇲",
                    Code= "GM"
                },
                new CountryModel{
                    CountryName= "Georgia",
                    CountryCode= "+995",
                    FlagUrl= "🇬🇪",
                    Code= "GE"
                },
                new CountryModel{
                    CountryName= "Germany",
                    CountryCode= "+49",
                    FlagUrl= "🇩🇪",
                    Code= "DE"
                },
                new CountryModel{
                    CountryName= "Ghana",
                    CountryCode= "+233",
                    FlagUrl= "🇬🇭",
                    Code= "GH"
                },
                new CountryModel{
                    CountryName= "Gibraltar",
                    CountryCode= "+350",
                    FlagUrl= "🇬🇮",
                    Code= "GI"
                },
                new CountryModel{
                    CountryName= "Greece",
                    CountryCode= "+30",
                    FlagUrl= "🇬🇷",
                    Code= "GR"
                },
                new CountryModel{
                    CountryName= "Greenland",
                    CountryCode= "+299",
                    FlagUrl= "🇬🇱",
                    Code= "GL"
                },
                new CountryModel{
                    CountryName= "Grenada",
                    CountryCode= "+1473",
                    FlagUrl= "🇬🇩",
                    Code= "GD"
                },
                new CountryModel{
                    CountryName= "Guadeloupe",
                    CountryCode= "+590",
                    FlagUrl= "🇬🇵",
                    Code= "GP"
                },
                new CountryModel{
                    CountryName= "Guam",
                    CountryCode= "+1671",
                    FlagUrl= "🇬🇺",
                    Code= "GU"
                },
                new CountryModel{
                    CountryName= "Guatemala",
                    CountryCode= "+502",
                    FlagUrl= "🇬🇹",
                    Code= "GT"
                },
                new CountryModel{
                    CountryName= "Guernsey",
                    CountryCode= "+44",
                    FlagUrl= "🇬🇬",
                    Code= "GG"
                },
                new CountryModel{
                    CountryName= "Guinea",
                    CountryCode= "+224",
                    FlagUrl= "🇬🇳",
                    Code= "GN"
                },
                new CountryModel{
                    CountryName= "Guinea-Bissau",
                    CountryCode= "+245",
                    FlagUrl= "🇬🇼",
                    Code= "GW"
                },
                new CountryModel{
                    CountryName= "Guyana",
                    CountryCode= "+595",
                    FlagUrl= "🇬🇾",
                    Code= "GY"
                },
                new CountryModel{
                    CountryName= "Haiti",
                    CountryCode= "+509",
                    FlagUrl= "🇭🇹",
                    Code= "HT"
                },
                new CountryModel{
                    CountryName= "Holy See (Vatican City State)",
                    CountryCode= "+379",
                    FlagUrl= "🇻🇦",
                    Code= "VA"
                },
                new CountryModel{
                    CountryName= "Honduras",
                    CountryCode= "+504",
                    FlagUrl= "🇭🇳",
                    Code= "HN"
                },
                new CountryModel{
                    CountryName= "Hong Kong",
                    CountryCode= "+852",
                    FlagUrl= "🇭🇰",
                    Code= "HK"
                },
                new CountryModel{
                    CountryName= "Hungary",
                    CountryCode= "+36",
                    FlagUrl= "🇭🇺",
                    Code= "HU"
                },
                new CountryModel{
                    CountryName= "Iceland",
                    CountryCode= "+354",
                    FlagUrl= "🇮🇸",
                    Code= "IS"
                },
                new CountryModel{
                    CountryName= "India",
                    CountryCode= "+91",
                    FlagUrl= "🇮🇳",
                    Code= "IN"
                },
                new CountryModel{
                    CountryName= "Indonesia",
                    CountryCode= "+62",
                    FlagUrl= "🇮🇩",
                    Code= "ID"
                },
                new CountryModel{
                    CountryName= "Iran, Islamic Republic of Persian Gulf",
                    CountryCode= "+98",
                    FlagUrl= "🇮🇷",
                    Code= "IR"
                },
                new CountryModel{
                    CountryName= "Iraq",
                    CountryCode= "+964",
                    FlagUrl= "🇮🇷",
                    Code= "IQ"
                },
                new CountryModel{
                    CountryName= "Ireland",
                    CountryCode= "+353",
                    FlagUrl= "🇮🇪",
                    Code= "IE"
                },
                new CountryModel{
                    CountryName= "Isle of Man",
                    CountryCode= "+44",
                    FlagUrl= "🇮🇲",
                    Code= "IM"
                },
                new CountryModel{
                    CountryName= "Israel",
                    CountryCode= "+972",
                    FlagUrl= "🇮🇱",
                    Code= "IL"
                },
                new CountryModel{
                    CountryName= "Italy",
                    CountryCode= "+39",
                    FlagUrl= "🇮🇹",
                    Code= "IT"
                },
                new CountryModel{
                    CountryName= "Jamaica",
                    CountryCode= "+1876",
                    FlagUrl= "🇯🇲",
                    Code= "JM"
                },
                new CountryModel{
                    CountryName= "Japan",
                    CountryCode= "+81",
                    FlagUrl= "🇯🇵",
                    Code= "JP"
                },
                new CountryModel{
                    CountryName= "Jersey",
                    CountryCode= "+44",
                    FlagUrl= "🇯🇪",
                    Code= "JE"
                },
                new CountryModel{
                    CountryName= "Jordan",
                    CountryCode= "+962",
                    FlagUrl= "🇯🇴",
                    Code= "JO"
                },
                new CountryModel{
                    CountryName= "Kazakhstan",
                    CountryCode= "+77",
                    FlagUrl= "🇰🇿",
                    Code= "KZ"
                },
                new CountryModel{
                    CountryName= "Kenya",
                    CountryCode= "+254",
                    FlagUrl= "🇰🇪",
                    Code= "KE"
                },
                new CountryModel{
                    CountryName= "Kiribati",
                    CountryCode= "+686",
                    FlagUrl= "🇰🇮",
                    Code= "KI"
                },
                new CountryModel{
                    CountryName= "Korea, Democratic People's Republic of Korea",
                    CountryCode= "+850",
                    FlagUrl= "🇰🇵",
                    Code= "KP"
                },
                new CountryModel{
                    CountryName= "Korea, Republic of South Korea",
                    CountryCode= "+82",
                    FlagUrl= "🇰🇷",
                    Code= "KR"
                },
                new CountryModel{
                    CountryName= "Kuwait",
                    CountryCode= "+965",
                    FlagUrl= "🇰🇼",
                    Code= "KW"
                },
                new CountryModel{
                    CountryName= "Kyrgyzstan",
                    CountryCode= "+996",
                    FlagUrl= "🇰🇬",
                    Code= "KG"
                },
                new CountryModel{
                    CountryName= "Laos",
                    CountryCode= "+856",
                    FlagUrl= "🇱🇦",
                    Code= "LA"
                },
                new CountryModel{
                    CountryName= "Latvia",
                    CountryCode= "+371",
                    FlagUrl= "🇱🇻",
                    Code= "LV"
                },
                new CountryModel{
                    CountryName= "Lebanon",
                    CountryCode= "+961",
                    FlagUrl= "🇱🇧",
                    Code= "LB"
                },
                new CountryModel{
                    CountryName= "Lesotho",
                    CountryCode= "+266",
                    FlagUrl= "🇱🇸",
                    Code= "LS"
                },
                new CountryModel{
                    CountryName= "Liberia",
                    CountryCode= "+231",
                    FlagUrl= "🇱🇷",
                    Code= "LR"
                },
                new CountryModel{
                    CountryName= "Libyan Arab Jamahiriya",
                    CountryCode= "+218",
                    FlagUrl= "🇱🇾",
                    Code= "LY"
                },
                new CountryModel{
                    CountryName= "Liechtenstein",
                    CountryCode= "+423",
                    FlagUrl= "🇱🇮",
                    Code= "LI"
                },
                new CountryModel{
                    CountryName= "Lithuania",
                    CountryCode= "+370",
                    FlagUrl= "🇱🇹",
                    Code= "LT"
                },
                new CountryModel{
                    CountryName= "Luxembourg",
                    CountryCode= "+352",
                    FlagUrl= "🇱🇺",
                    Code= "LU"
                },
                new CountryModel{
                    CountryName= "Macao",
                    CountryCode= "+853",
                    FlagUrl= "🇲🇴",
                    Code= "MO"
                },
                new CountryModel{
                    CountryName= "Macedonia",
                    CountryCode= "+389",
                    FlagUrl= "🇲🇰",
                    Code= "MK"
                },
                new CountryModel{
                    CountryName= "Madagascar",
                    CountryCode= "+261",
                    FlagUrl= "🇲🇬",
                    Code= "MG"
                },
                new CountryModel{
                    CountryName= "Malawi",
                    CountryCode= "+265",
                    FlagUrl= "🇲🇼",
                    Code= "MW"
                },
                new CountryModel{
                    CountryName= "Malaysia",
                    CountryCode= "+60",
                    FlagUrl= "🇲🇾",
                    Code= "MY"
                },
                new CountryModel{
                    CountryName= "Maldives",
                    CountryCode= "+960",
                    FlagUrl= "🇲🇻",
                    Code= "MV"
                },
                new CountryModel{
                    CountryName= "Mali",
                    CountryCode= "+223",
                    FlagUrl= "🇲🇱",
                    Code= "ML"
                },
                new CountryModel{
                    CountryName= "Malta",
                    CountryCode= "+356",
                    FlagUrl= "🇲🇹",
                    Code= "MT"
                },
                new CountryModel{
                    CountryName= "Marshall Islands",
                    CountryCode= "+692",
                    FlagUrl= "🇲🇭",
                    Code= "MH"
                },
                new CountryModel{
                    CountryName= "Martinique",
                    CountryCode= "+596",
                    FlagUrl= "🇲🇶",
                    Code= "MQ"
                },
                new CountryModel{
                    CountryName= "Mauritania",
                    CountryCode= "+222",
                    FlagUrl= "🇲🇷",
                    Code= "MR"
                },
                new CountryModel{
                    CountryName= "Mauritius",
                    CountryCode= "+230",
                    FlagUrl= "🇲🇺",
                    Code= "MU"
                },
                new CountryModel{
                    CountryName= "Mayotte",
                    CountryCode= "+262",
                    FlagUrl= "🇾🇹",
                    Code= "YT"
                },
                new CountryModel{
                    CountryName= "Mexico",
                    CountryCode= "+52",
                    FlagUrl= "🇲🇽",
                    Code= "MX"
                },
                new CountryModel{
                    CountryName= "Micronesia, Federated States of Micronesia",
                    CountryCode= "+691",
                    FlagUrl= "🇫🇲",
                    Code= "FM"
                },
                new CountryModel{
                    CountryName= "Moldova",
                    CountryCode= "+373",
                    FlagUrl= "🇲🇩",
                    Code= "MD"
                },
                new CountryModel{
                    CountryName= "Monaco",
                    CountryCode= "+377",
                    FlagUrl= "🇲🇨",
                    Code= "MC"
                },
                new CountryModel{
                    CountryName= "Mongolia",
                    CountryCode= "+976",
                    FlagUrl= "🇲🇳",
                    Code= "MN"
                },
                new CountryModel{
                    CountryName= "Montenegro",
                    CountryCode= "+382",
                    FlagUrl= "🇲🇪",
                    Code= "ME"
                },
                new CountryModel{
                    CountryName= "Montserrat",
                    CountryCode= "+1664",
                    FlagUrl= "🇲🇸",
                    Code= "MS"
                },
                new CountryModel{
                    CountryName= "Morocco",
                    CountryCode= "+212",
                    FlagUrl= "🇲🇦",
                    Code= "MA"
                },
                new CountryModel{
                    CountryName= "Mozambique",
                    CountryCode= "+258",
                    FlagUrl= "🇲🇿",
                    Code= "MZ"
                },
                new CountryModel{
                    CountryName= "Myanmar",
                    CountryCode= "+95",
                    FlagUrl= "🇲🇲",
                    Code= "MM"
                },
                new CountryModel{
                    CountryName= "Namibia",
                    FlagUrl= "🇳🇦",
                    CountryCode= "+264",
                    Code= "NA"
                },
                new CountryModel{
                    CountryName= "Nauru",
                    CountryCode= "+674",
                    FlagUrl= "🇳🇷",
                    Code= "NR"
                },
                new CountryModel{
                    CountryName= "Nepal",
                    CountryCode= "+977",
                    FlagUrl= "🇳🇵",
                    Code= "NP"
                },
                new CountryModel{
                    CountryName= "Netherlands",
                    CountryCode= "+31",
                    FlagUrl= "🇳🇱",
                    Code= "NL"
                },
                new CountryModel{
                    CountryName= "Netherlands Antilles",
                    CountryCode= "+599",
                    FlagUrl= "🇧🇶",
                    Code= "AN"
                },
                new CountryModel{
                    CountryName= "New Caledonia",
                    CountryCode= "+687",
                    FlagUrl= "🇳🇨",
                    Code= "NC"
                },
                new CountryModel{
                    CountryName= "New Zealand",
                    CountryCode= "+64",
                    FlagUrl= "🇳🇿",
                    Code= "NZ"
                },
                new CountryModel{
                    CountryName= "Nicaragua",
                    CountryCode= "+505",
                    FlagUrl= "🇳🇮",
                    Code= "NI"
                },
                new CountryModel{
                    CountryName= "Niger",
                    CountryCode= "+227",
                    FlagUrl= "🇳🇪",
                    Code= "NE"
                },
                new CountryModel{
                    CountryName= "Nigeria",
                    CountryCode= "+234",
                    FlagUrl= "🇳🇬",
                    Code= "NG"
                },
                new CountryModel{
                    CountryName= "Niue",
                    CountryCode= "+683",
                    FlagUrl= "🇳🇺",
                    Code= "NU"
                },
                new CountryModel{
                    CountryName= "Norfolk Island",
                    CountryCode= "+672",
                    FlagUrl= "🇳🇫",
                    Code= "NF"
                },
                new CountryModel{
                    CountryName= "Northern Mariana Islands",
                    CountryCode= "+1670",
                    FlagUrl= "🇲🇵",
                    Code= "MP"
                },
                new CountryModel{
                    CountryName= "Norway",
                    CountryCode= "+47",
                    FlagUrl= "🇳🇴",
                    Code= "NO"
                },
                new CountryModel{
                    CountryName= "Oman",
                    CountryCode= "+968",
                    FlagUrl= "🇴🇲",
                    Code= "OM"
                },
                new CountryModel{
                    CountryName= "Pakistan",
                    CountryCode= "+92",
                    FlagUrl= "🇵🇰",
                    Code= "PK"
                },
                new CountryModel{
                    CountryName= "Palau",
                    CountryCode= "+680",
                    FlagUrl= "🇵🇼",
                    Code= "PW"
                },
                new CountryModel{
                    CountryName= "Palestinian Territory, Occupied",
                    CountryCode= "+970",
                    FlagUrl= "🇵🇸",
                    Code= "PS"
                },
                new CountryModel{
                    CountryName= "Panama",
                    CountryCode= "+507",
                    FlagUrl= "🇵🇦",
                    Code= "PA"
                },
                new CountryModel{
                    CountryName= "Papua New Guinea",
                    CountryCode= "+675",
                    FlagUrl= "🇵🇬",
                    Code= "PG"
                },
                new CountryModel{
                    CountryName= "Paraguay",
                    CountryCode= "+595",
                    FlagUrl= "🇵🇾",
                    Code= "PY"
                },
                new CountryModel{
                    CountryName= "Peru",
                    CountryCode= "+51",
                    FlagUrl= "🇵🇪",
                    Code= "PE"
                },
                new CountryModel{
                    CountryName= "Philippines",
                    CountryCode= "+63",
                    FlagUrl= "🇵🇭",
                    Code= "PH"
                },
                new CountryModel{
                    CountryName= "Pitcairn",
                    CountryCode= "+872",
                    FlagUrl= "🇵🇳",
                    Code= "PN"
                },
                new CountryModel{
                    CountryName= "Poland",
                    CountryCode= "+48",
                    FlagUrl= "🇵🇱",
                    Code= "PL"
                },
                new CountryModel{
                    CountryName= "Portugal",
                    CountryCode= "+351",
                    FlagUrl= "🇵🇹",
                    Code= "PT"
                },
                new CountryModel{
                    CountryName= "Puerto Rico",
                    CountryCode= "+1939",
                    FlagUrl= "🇵🇷",
                    Code= "PR"
                },
                new CountryModel{
                    CountryName= "Qatar",
                    CountryCode= "+974",
                    FlagUrl= "🇶🇦",
                    Code= "QA"
                },
                new CountryModel{
                    CountryName= "Romania",
                    CountryCode= "+40",
                    FlagUrl= "🇷🇴",
                    Code= "RO"
                },
                new CountryModel{
                    CountryName= "Russia",
                    CountryCode= "+7",
                    FlagUrl= "🇷🇺",
                    Code= "RU"
                },
                new CountryModel{
                    CountryName= "Rwanda",
                    CountryCode= "+250",
                    FlagUrl= "🇷🇼",
                    Code= "RW"
                },
                new CountryModel{
                    CountryName= "Reunion",
                    CountryCode= "+262",
                    FlagUrl= "🇷🇪",
                    Code= "RE"
                },
                new CountryModel{
                    CountryName= "Saint Barthelemy",
                    CountryCode= "+590",
                    FlagUrl= "🇧🇱",
                    Code= "BL"
                },
                new CountryModel{
                    CountryName= "Saint Helena, Ascension and Tristan Da Cunha",
                    CountryCode= "+290",
                    FlagUrl= "🇸🇭",
                    Code= "SH"
                },
                new CountryModel{
                    CountryName= "Saint Kitts and Nevis",
                    CountryCode= "+1869",
                    FlagUrl= "🇰🇳",
                    Code= "KN"
                },
                new CountryModel{
                    CountryName= "Saint Lucia",
                    CountryCode= "+1758",
                    FlagUrl= "🇱🇨",
                    Code= "LC"
                },
                new CountryModel{
                    CountryName= "Saint Martin",
                    CountryCode= "+590",
                    FlagUrl= "🇲🇫",
                    Code= "MF"
                },
                new CountryModel{
                    CountryName= "Saint Pierre and Miquelon",
                    CountryCode= "+508",
                    FlagUrl= "🇵🇲",
                    Code= "PM"
                },
                new CountryModel{
                    CountryName= "Saint Vincent and the Grenadines",
                    CountryCode= "+1784",
                    FlagUrl= "🇻🇨",
                    Code= "VC"
                },
                new CountryModel{
                    CountryName= "Samoa",
                    CountryCode= "+685",
                    FlagUrl= "🇼🇸",
                    Code= "WS"
                },
                new CountryModel{
                    CountryName= "San Marino",
                    CountryCode= "+378",
                    FlagUrl= "🇸🇲",
                    Code= "SM"
                },
                new CountryModel{
                    CountryName= "Sao Tome and Principe",
                    CountryCode= "+239",
                    FlagUrl= "🇸🇹",
                    Code= "ST"
                },
                new CountryModel{
                    CountryName= "Saudi Arabia",
                    CountryCode= "+966",
                    FlagUrl= "🇸🇦",
                    Code= "SA"
                },
                new CountryModel{
                    CountryName= "Senegal",
                    CountryCode= "+221",
                    FlagUrl= "🇸🇳",
                    Code= "SN"
                },
                new CountryModel{
                    CountryName= "Serbia",
                    CountryCode= "+381",
                    FlagUrl= "🇷🇸",
                    Code= "RS"
                },
                new CountryModel{
                    CountryName= "Seychelles",
                    CountryCode= "+248",
                    FlagUrl= "🇸🇨",
                    Code= "SC"
                },
                new CountryModel{
                    CountryName= "Sierra Leone",
                    CountryCode= "+232",
                    FlagUrl= "🇸🇱",
                    Code= "SL"
                },
                new CountryModel{
                    CountryName= "Singapore",
                    CountryCode= "+65",
                    FlagUrl= "🇸🇬",
                    Code= "SG"
                },
                new CountryModel{
                    CountryName= "Slovakia",
                    CountryCode= "+421",
                    FlagUrl= "🇸🇰",
                    Code= "SK"
                },
                new CountryModel{
                    CountryName= "Slovenia",
                    CountryCode= "+386",
                    FlagUrl= "🇸🇮",
                    Code= "SI"
                },
                new CountryModel{
                    CountryName= "Solomon Islands",
                    CountryCode= "+677",
                    FlagUrl= "🇸🇧",
                    Code= "SB"
                },
                new CountryModel{
                    CountryName= "Somalia",
                    CountryCode= "+252",
                    FlagUrl= "🇸🇴",
                    Code= "SO"
                },
                new CountryModel{
                    CountryName= "South Africa",
                    CountryCode= "+27",
                    FlagUrl= "🇿🇦",
                    Code= "ZA"
                },
                new CountryModel{
                    CountryName= "South Sudan",
                    CountryCode= "+211",
                    FlagUrl= "🇸🇸",
                    Code= "SS"
                },
                new CountryModel{
                    CountryName= "South Georgia and the South Sandwich Islands",
                    CountryCode= "+500",
                    FlagUrl= "🇬🇸",
                    Code= "GS"
                },
                new CountryModel{
                    CountryName= "Spain",
                    CountryCode= "+34",
                    FlagUrl= "🇪🇸",
                    Code= "ES"
                },
                new CountryModel{
                    CountryName= "Sri Lanka",
                    CountryCode= "+94",
                    FlagUrl= "🇱🇰",
                    Code= "LK"
                },
                new CountryModel{
                    CountryName= "Sudan",
                    CountryCode= "+249",
                    FlagUrl= "🇸🇩",
                    Code= "SD"
                },
                new CountryModel{
                    CountryName= "SuriCountryName",
                    CountryCode= "+597",
                    FlagUrl= "🇸🇷",
                    Code= "SR"
                },
                new CountryModel{
                    CountryName= "Svalbard and Jan Mayen",
                    CountryCode= "+47",
                    FlagUrl= "🇸🇯",
                    Code= "SJ"
                },
                new CountryModel{
                    CountryName= "Swaziland",
                    CountryCode= "+268",
                    FlagUrl= "🇸🇿",
                    Code= "SZ"
                },
                new CountryModel{
                    CountryName= "Sweden",
                    CountryCode= "+46",
                    FlagUrl= "🇸🇪",
                    Code= "SE"
                },
                new CountryModel{
                    CountryName= "Switzerland",
                    CountryCode= "+41",
                    FlagUrl= "🇨🇭",
                    Code= "CH"
                },
                new CountryModel{
                    CountryName= "Syrian Arab Republic",
                    CountryCode= "+963",
                    FlagUrl= "🇸🇾",
                    Code= "SY"
                },
                new CountryModel{
                    CountryName= "Taiwan",
                    CountryCode= "+886",
                    FlagUrl= "🇹🇼",
                    Code= "TW"
                },
                new CountryModel{
                    CountryName= "Tajikistan",
                    CountryCode= "+992",
                    FlagUrl= "🇹🇯",
                    Code= "TJ"
                },
                new CountryModel{
                    CountryName= "Tanzania, United Republic of Tanzania",
                    CountryCode= "+255",
                    FlagUrl= "🇹🇿",
                    Code= "TZ"
                },
                new CountryModel{
                    CountryName= "Thailand",
                    CountryCode= "+66",
                    FlagUrl= "🇹🇭",
                    Code= "TH"
                },
                new CountryModel{
                    CountryName= "Timor-Leste",
                    CountryCode= "+670",
                    FlagUrl= "🇹🇱",
                    Code= "TL"
                },
                new CountryModel{
                    CountryName= "Togo",
                    CountryCode= "+228",
                    FlagUrl= "🇹🇬",
                    Code= "TG"
                },
                new CountryModel{
                    CountryName= "Tokelau",
                    CountryCode= "+690",
                    FlagUrl= "🇹🇰",
                    Code= "TK"
                },
                new CountryModel{
                    CountryName= "Tonga",
                    CountryCode= "+676",
                    FlagUrl= "🇹🇴",
                    Code= "TO"
                },
                new CountryModel{
                    CountryName= "Trinidad and Tobago",
                    CountryCode= "+1868",
                    FlagUrl= "🇹🇹",
                    Code= "TT"
                },
                new CountryModel{
                    CountryName= "Tunisia",
                    CountryCode= "+216",
                    FlagUrl= "🇹🇳",
                    Code= "TN"
                },
                new CountryModel{
                    CountryName= "Turkey",
                    CountryCode= "+90",
                    FlagUrl= "🇹🇷",
                    Code= "TR"
                },
                new CountryModel{
                    CountryName= "Turkmenistan",
                    CountryCode= "+993",
                    FlagUrl= "🇹🇲",
                    Code= "TM"
                },
                new CountryModel{
                    CountryName= "Turks and Caicos Islands",
                    CountryCode= "+1649",
                    FlagUrl= "🇹🇨",
                    Code= "TC"
                },
                new CountryModel{
                    CountryName= "Tuvalu",
                    CountryCode= "+688",
                    FlagUrl= "🇹🇻",
                    Code= "TV"
                },
                new CountryModel{
                    CountryName= "Uganda",
                    CountryCode= "+256",
                    FlagUrl= "🇺🇬",
                    Code= "UG"
                },
                new CountryModel{
                    CountryName= "Ukraine",
                    CountryCode= "+380",
                    FlagUrl= "🇺🇦",
                    Code= "UA"
                },
                new CountryModel{
                    CountryName= "United Arab Emirates",
                    CountryCode= "+971",
                    FlagUrl= "🇦🇪",
                    Code= "AE"
                },
                new CountryModel{
                    CountryName= "United Kingdom",
                    CountryCode= "+44",
                    FlagUrl= "🇬🇧",
                    Code= "GB"
                },
                new CountryModel{
                    CountryName= "United States",
                    CountryCode= "+1",
                    FlagUrl= "🇺🇸",
                    Code= "US"
                },
                new CountryModel{
                    CountryName= "Uruguay",
                    CountryCode= "+598",
                    FlagUrl= "🇺🇾",
                    Code= "UY"
                },
                new CountryModel{
                    CountryName= "Uzbekistan",
                    CountryCode= "+998",
                    FlagUrl= "🇺🇿",
                    Code= "UZ"
                },
                new CountryModel{
                    CountryName= "Vanuatu",
                    CountryCode= "+678",
                    FlagUrl= "🇻🇺",
                    Code= "VU"
                },
                new CountryModel{
                    CountryName= "Venezuela, Bolivarian Republic of Venezuela",
                    CountryCode= "+58",
                    FlagUrl= "🇻🇪",
                    Code= "VE"
                },
                new CountryModel{
                    CountryName= "Vietnam",
                    CountryCode= "+84",
                    FlagUrl= "🇻🇳",
                    Code= "VN"
                },
                new CountryModel{
                    CountryName= "Virgin Islands, British",
                    CountryCode= "+1284",
                    FlagUrl= "🇻🇬",
                    Code= "VG"
                },
                new CountryModel{
                    CountryName= "Virgin Islands, U.S.",
                    CountryCode= "+1340",
                    FlagUrl= "🇻🇮",
                    Code= "VI"
                },
                new CountryModel{
                    CountryName= "Wallis and Futuna",
                    CountryCode= "+681",
                    FlagUrl= "🇼🇫",
                    Code= "WF"
                },
                new CountryModel{
                    CountryName= "Yemen",
                    CountryCode= "+967",
                    FlagUrl= "🇾🇪",
                    Code= "YE"
                },
                new CountryModel{
                    CountryName= "Zambia",
                    CountryCode= "+260",
                    FlagUrl= "🇿🇲",
                    Code= "ZM"
                },
                new CountryModel{
                    CountryName= "Zimbabwe",
                    CountryCode= "+263",
                    FlagUrl= "🇿🇼",
                    Code= "ZW"
                }

            };
            return countryList;

        }
    }
}
