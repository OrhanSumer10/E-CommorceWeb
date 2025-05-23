using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductImages : IEntity
    {
        [Key]
        public int pImgId { get; set; }
        // Ürünün resim URL'si
        public int productId { get; set; }
        public byte[] ImageData { get; set; }  // 🔹 Görselin kendisi
        public string ContentType { get; set; }  // 🔹 image/jpeg vs.
  
      
        public Product product { get; set; }
    }
}
