using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class PolicyResult
    {
        public string id { get; set; }
        public string generalMessage { get; set; }
        public bool policyResult { get; set; }
    }
}
