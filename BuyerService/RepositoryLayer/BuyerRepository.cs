using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyerService.Models;
using BuyerService.RepositoryLayer.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BuyerService.RepositoryLayer
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly ILogger<BuyerRepository> _logger;
        private readonly IOptions<EAuctionDatabaseSettings> _dbSettings;
        private readonly IMongoCollection<BidAndBuyer> _bidCollections;
        public BuyerRepository(ILogger<BuyerRepository> logger, IOptions<EAuctionDatabaseSettings> DBSettings)
        {
            _logger = logger;
            _dbSettings = DBSettings;

            var mongoClient = new MongoClient(_dbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(_dbSettings.Value.DatabaseName);

            _bidCollections = mongoDatabase.GetCollection<BidAndBuyer>
                (_dbSettings.Value.BuyerCollectionName);
        }
        public async Task AddBid(BidAndBuyer bidDetails)
        {
            try
            {
                await _bidCollections.InsertOneAsync(bidDetails);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task UpdateBid(string productId, string buyerEmailId, double bidAmount)
        {
            try
            {
                var earlierBidDetails = await _bidCollections.Find(x => x.ProductId == productId && x.Email == buyerEmailId).FirstOrDefaultAsync();
                earlierBidDetails.BidAmount = bidAmount;
                await _bidCollections.ReplaceOneAsync(y => y.ProductId == productId && y.Email == buyerEmailId, earlierBidDetails);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<BidAndBuyer> GetBidDetails(string productId, string bidderEmailId = null)
        {
            try
            {
                BidAndBuyer bidDetails;
                if (bidderEmailId == null)
                {
                    bidDetails = _bidCollections.Find(x => x.ProductId == productId).FirstOrDefault();
                }
                else
                {
                    bidDetails = _bidCollections.Find(x => x.Email == bidderEmailId && x.ProductId == productId).FirstOrDefault();
                }
                return bidDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<BidAndBuyer> GetAllBidsForProductId(string productId)
        {
            try
            {
                List<BidAndBuyer> allBids;
                allBids = _bidCollections.Find(X => X.ProductId == productId).SortByDescending(Y => Y.BidAmount).ToList();
                return allBids;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public BidAndBuyer GetBidDetailsByBidId(string bidId)
        {
            try
            {
                return _bidCollections.Find(x => x.Id == bidId).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
