using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerService.Models
{
    public class GetAllBidDetailsResponse
    {
        public string ProductId { get; set; }
        public List<BidDetails> Bids { get; set; }
    }
}
