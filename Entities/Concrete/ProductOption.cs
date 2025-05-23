using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductOption : IEntity
    {
        [Key]
        public int ProductOptionId { get; set; } // Ürün seçeneği ID'si
        public int ProductId { get; set; } // İlgili ürünün ID'si
        public string OptionName { get; set; } // Seçeneğin adı (örn. Renk, Beden)
        public string OptionValue { get; set; } // Seçeneğin değeri (örn. Kırmızı, M)
        public decimal? AdditionalPrice { get; set; } // Bu seçeneğin ek maliyeti (opsiyonel)
        public int? StockQuantity { get; set; } // Bu seçeneğin stok miktarı (opsiyonel)
        public Product Product { get; set; } // İlgili ürün


      
    }
}
