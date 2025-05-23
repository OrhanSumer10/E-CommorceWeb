using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Payment : IEntity
    {
        [Key]
        public int PaymentId { get; set; } // Ödeme ID'si
        public int OrderId { get; set; } // Order ID'si

        public decimal Amount { get; set; } // Ödeme tutarı
        public DateTime PaymentDate { get; set; } // Ödeme tarihi
        public Order Order { get; set; } // Sipariş
    }
}
