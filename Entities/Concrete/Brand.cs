using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Brand :IEntity
    {
        [Key]
        public int brandId { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }      

      public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEnumerable<BrandCategories> BrandCategories { get; set; } = Enumerable.Empty<BrandCategories>();
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
