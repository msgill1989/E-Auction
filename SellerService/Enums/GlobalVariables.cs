using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerService.Enums
{
    public class GlobalVariables
    {
        public enum ProductCategory
        { 
            Painting,
            Sculptor,
            Ornament
        }

        public const string AddProductSuccessMsg = "Product has been added successfully.";
    }
}
