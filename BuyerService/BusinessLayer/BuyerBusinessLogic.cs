using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyerService.BusinessLayer.Interfaces;
using BuyerService.Models;
using BuyerService.RepositoryLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace BuyerService.BusinessLayer
{
    public class BuyerBusinessLogic : IBuyerBusinessLogic
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly ILogger<BuyerBusinessLogic> _logger;
        public BuyerBusinessLogic(IBuyerRepository buyerRepository, ILogger<BuyerBusinessLogic> logger)
        {
            _buyerRepository = buyerRepository;
            _logger = logger;
        }

        public async Task AddBid(BidAndBuyer bidDetails)
        {
            try
            {
                //If bid is placed after the bid date. throw a exception. -----------To be implemented--------------------

                //Check if the same user has already placed a bid
                BidAndBuyer existingBid = await _buyerRepository.GetBidDetails(bidDetails.ProductId, bidDetails.Email);

                if (existingBid != null)
                    throw new KeyNotFoundException("This buyer has already placed bid for this product.");

                await _buyerRepository.AddBid(bidDetails);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public async Task UpdateBid(string bidId, double bidAmount)
        {
            await _buyerRepository.UpdateBid(bidId, bidAmount);
        }
    }
}
