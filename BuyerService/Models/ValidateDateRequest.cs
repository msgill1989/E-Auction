using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerService.Models
{
    public class ValidateDateRequest
    {
        public string ProductId { get; set; }
        public DateTime BidDate { get; set; }
        public string Operation { get; set; }
    }
}
