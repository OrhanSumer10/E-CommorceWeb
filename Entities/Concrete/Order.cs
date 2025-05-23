using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Order : IEntity
    {
        [Key]
        public int OrderId { get; set; } // Sipariş ID'si
        public int UserId { get; set; } // Kullanıcı ID'si
     
        public DateTime OrderDate { get; set; } // Sipariş tarihi
        public OrderStatus Status { get; set; } // Sipariş durumu
        public ApUser User { get; set; } // Siparişin ait olduğu kullanıcı
        public PaymentType Payment { get; set; } // Ödeme bilgileri
        public int cartitemId { get; set; } // Kullanıcı ID'si
        public ICollection<CartItem> CartItems { get; set; } // Sepet öğeleri ile ilişkisi

        public Order()
        {
            CartItems = new List<CartItem>();
        }

        public enum OrderStatus
        {
            Pending,        // Sipariş alındı, ancak henüz işleme alınmadı
            Processing,     // Sipariş işleniyor
            Shipped,        // Sipariş gönderildi
            Delivered,      // Sipariş teslim edildi
            Cancelled,      // Sipariş iptal edildi
            Returned        // Sipariş iade edildi
        }

        public enum PaymentType
        {
            Online,         // Online ödeme
            CashOnDelivery  // Kapıda ödeme
        }
    }
}
