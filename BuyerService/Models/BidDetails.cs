using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerService.Models
{
    public class BidDetails
    {
        public Double BidAmount { get; set; }
        public string BuyerFName { get; set; }
        public string Email { get; set; }
        public int Mobile { get; set; }
    }
}
