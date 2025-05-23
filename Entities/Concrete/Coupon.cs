using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Coupon :IEntity
    {
        [Key]
        public int CouponId { get; set; } // Kupon ID'si
        public string Code { get; set; } // Kupon kodu
        public decimal Discount { get; set; } // İndirim oranı

        // Navigation Properties
        public ICollection<CouponProduct> CouponProducts { get; set; }
        public ICollection<CouponUser> CouponUsers { get; set; }

        public Coupon()
        {
            CouponProducts = new HashSet<CouponProduct>();
            CouponUsers = new HashSet<CouponUser>();
        }
    }
}
