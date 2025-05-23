﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Category : IEntity
    {
        [Key]
        public int CategoryId { get; set; } // Kategori ID'si
        public string Name { get; set; } // Kategori adı
        public string Description { get; set; } // Kategori açıklaması

        public IEnumerable<BrandCategories> BrandCategories { get; set; } = Enumerable.Empty<BrandCategories>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
