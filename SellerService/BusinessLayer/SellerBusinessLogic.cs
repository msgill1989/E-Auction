using SellerService.BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SellerService.RepositoryLayer.Interfaces;
using SellerService.RepositoryLayer;
using Microsoft.Extensions.Logging;

namespace SellerService.BusinessLayer
{
    public class SellerBusinessLogic : ISellerBusinessLogic
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly ILogger<SellerBusinessLogic> _logger;

        public SellerBusinessLogic(ISellerRepository sellerRepository, ILogger<SellerBusinessLogic> logger)
        {
            _sellerRepository = sellerRepository;
            _logger = logger;
        }
        public async Task AddProductBLayerAsync(ProductAndSeller productObj)
        {
            await _sellerRepository.AddProductAsync(productObj);
        }

        public async Task DeleteProductBLayerAsync(string productId)
        {
            throw new NotImplementedException();
        }
    }
}
