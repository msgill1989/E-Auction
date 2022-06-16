using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellerService.BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SellerService.Controllers
{
    public class SellerController : Controller
    {
        private readonly ISellerBusinessLogic _sellerBusinessLogic;
        private readonly ILogger<SellerController> _logger;
        public SellerController(ISellerBusinessLogic sellerBusinessLogic, ILogger<SellerController> logger)
        {
            _sellerBusinessLogic = sellerBusinessLogic;
            _logger = logger;
        }

        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddProduct([FromBody] ProductAndSeller productToAdd)
        {
            try
            {
                _logger.LogError("This is error.");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SellerController/Delete/5
        [HttpDelete("DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteProduct(string productId)
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
