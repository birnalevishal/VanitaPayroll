using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
    public class CountryModel
    {
        public int CountryCd { get; set; }
        public string Country { get; set; }
        public string IsActive { get; set; }
    }
}