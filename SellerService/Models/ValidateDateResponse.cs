using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerService.Models
{
    public class ValidateDateResponse
    {
        public string ProductId { get; set; }
        public bool IsValid { get; set; }
        public string Operation { get; set; }
    }
}
