using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerService.Models
{
    public class ShowBidsResponse
    {
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string DetailedDescription { get; set; }
        public string Category { get; set; }
        public Double StartingPrice { get; set; }
        public DateTime BidEndDate { get; set; }
        public List<BidDetails> Bids { get; set; }
    }
}
