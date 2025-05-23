using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductReview : IEntity
    {
        [Key]
        public int ProductReviewId { get; set; } // İnceleme ID'si
        public int ProductId { get; set; } // Ürün ID'si
        public int UserId { get; set; } // Kullanıcı ID'si
        public int Rating { get; set; } // Yıldız puanı
        public string ReviewText { get; set; } // İnceleme metni
        public DateTime ReviewDate { get; set; } // İnceleme metni

        public Product Product { get; set; } // Ürün
        public ApUser User { get; set; } // Kullanıcı


    }
}
