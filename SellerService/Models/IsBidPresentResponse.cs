using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerService.Models
{
    public class IsBidPresentResponse
    {
        public int ProductId { get; set; }
        public bool IsBidPresent { get; set; }
    }
}
