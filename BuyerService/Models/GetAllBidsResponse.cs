using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerService.Models
{
    public class GetAllBidsResponse
    {
        public string ProductId { get; set; }
        public List<BidDetails> Bids { get; set; }
    }
}
