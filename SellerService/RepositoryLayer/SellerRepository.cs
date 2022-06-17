using SellerService.RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SellerService.Models;

namespace SellerService.RepositoryLayer
{
    public class SellerRepository : ISellerRepository
    {
        private readonly ILogger<SellerRepository> _logger;
        private readonly IOptions<EAuctionDatabaseSettings> _dbSettings;
        public SellerRepository(ILogger<SellerRepository> logger, IOptions<EAuctionDatabaseSettings> DBSettings)
        {
            _logger = logger;
            _dbSettings = DBSettings;
        }
        public async Task AddProductAsync(ProductAndSeller productObj)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProductAsync(string productId)
        {
            throw new NotImplementedException();
        }
        public async Task GetProductAsync(string productId)
        {
            throw new NotImplementedException();
        }
    }
}
