using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Product : IEntity
    {
        [Key]
        public int ProductId { get; set; }

        // Ürünün adı
        public string Name { get; set; }

        // Ürünün açıklaması
        public string Description { get; set; }

        // Ürünün birim fiyatı
        public double Price { get; set; }

 
        // Ürünün SKU (Stock Keeping Unit) numarası
        public string SKU { get; set; }

        // Ürünün üretim tarihi
        public DateTime ManufactureDate { get; set; }

        // Ürünün garanti süresi (gün cinsinden)
        public int WarrantyPeriod { get; set; }

        // Ürün puanı (ortalama kullanıcı puanı)
        public double Rating { get; set; }

        // Ürünün oluşturulma tarihi
        public DateTime CreatedDate { get; set; }

        // Ürünün güncellenme tarihi
        public DateTime UpdatedDate { get; set; }

        // Ürünün kategorisi
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Ürünün ait olduğu alt kategori
        public int SubCategoryId { get; set; } // Alt Kategori ID'si
        public SubCategories SubCategories { get; set; } // Alt Kategori ile ilişki


        // İlgili ürün detayları
        public ICollection<ProductImages> ProductImages { get; set; } // Sipariş Fotoğrafları
        public ICollection<ProductReview> Reviews { get; set; } // Ürün incelemeleri

        public ICollection<CouponProduct> CouponProducts { get; set; }

        public Product()
        {
            Reviews = new HashSet<ProductReview>();
            ProductImages = new HashSet<ProductImages>();
        }
    }
}
