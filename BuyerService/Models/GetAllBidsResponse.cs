using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerService.Models
{
    public class GetAllBidsResponse
    {
        private List<BidDetails> bids = new List<BidDetails>();
        public string ProductId { get; set; }
        public List<BidDetails> Bids 
        {
            set { bids = value; }
            get { return bids; }
        }
    }
}
