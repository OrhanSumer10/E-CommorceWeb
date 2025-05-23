using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class SubCategories : IEntity
    {
        [Key]
        public int SubCategoryId { get; set; }

        public string SCName { get; set; }

        public int ParentCategoryId { get; set; }

        // Alt kategorinin ait olduğu ana kategori
        public Category Category { get; set; } // Ana Kategori ile ilişki

        // Alt kategorinin birden fazla ürüne sahip olabileceği durum
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
