using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Result { get; set; }

        public string QuoteNumber { get; set; }
        public bool IsComplete { get; set; }
    }
}
