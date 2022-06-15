using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SellerService.CustomValidators
{
    public class BidDateValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (Convert.ToDateTime(value) <= DateTime.Now)
            {
                return false;
            }
            else
                return true;
        }
    }
}
