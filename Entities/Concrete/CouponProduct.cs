using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class CouponProduct :IEntity
    {
        [Key]
        public int ID { get; set; }
        public int CouponId { get; set; }
        public Coupon Coupon { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public bool isUsed { get; set; }
    }
}
