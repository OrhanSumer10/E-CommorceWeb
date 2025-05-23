using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete
{
    public class CartItem : IEntity
    {
        [Key]
        public int CartItemId { get; set; } // Cart item ID
        public int ApplicationUserId { get; set; } // Kullanıcı ID'si
        public int ProductId { get; set; } // Ürün ID'si    
        public string SelectedValue { get; set; }  // Seçilen seçenek
        public decimal SelectedPrice { get; set; } // Seçilen fiyat
        public int SelectedQuantity { get; set; }  // Seçilen miktar
        public bool isPaid { get; set; }  // Ödendi mi
        public DateTime AddedDate { get; set; } // Ürünün ne zaman eklendiği

        // Navigation properties
        public ApUser ApplicationUser { get; set; } // Kullanıcı
        public Product Product { get; set; } // Ürün
        public int? OrderId { get; set; } // Sipariş ID'si
        public Order Order { get; set; } // Siparişle ilişkilendirilen öğe
    }

}
