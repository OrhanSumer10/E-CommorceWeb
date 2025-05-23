using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class WishlistItem : IEntity
    {
        [Key]
        public int WishlistItemId { get; set; } // Wishlist item ID
        public int ApplicationUserId { get; set; } // Kullanıcı ID'si
        public int ProductId { get; set; } // Ürün ID'si
        public DateTime AddedDate { get; set; } // Ürünün ne zaman eklendiği
        // Navigation properties
        public ApUser ApplicationUser { get; set; } // Kullanıcı
        public Product Product { get; set; } // Ürün
    }

}
