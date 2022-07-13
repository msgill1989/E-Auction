using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerService.Models
{
    public class ValidateDateResponse
    {
        public string productId { get; set; }
        public bool IsValid { get; set; }
        public string Operation { get; set; }
    }
}
