using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Policy
    {
        public string id { get; set; }
        public string startDateOfPolicy { get; set; }
        public string generalMessage { get; set; }
        public string policyResult { get; set; }
        public double policyCost { get; set; }
        public List<Driver> drivers { get; set; }
        public List<Occupation> occupations { get; set; }
    }
    public class Driver
    {
        public string id { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string occupation { get; set; }
        public List<Claim> claims { get; set; }
    }

    public class Claim
    {
        public string id { get; set; }
        public string claimdate { get; set; }
    }

    public class Occupation
    {
        public string name { get; set; }
        public string id { get; set; }
    }

 
}
