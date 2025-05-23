using Entities.Concrete;
using System.Diagnostics.Contracts;

namespace ECommorceWeb.Models
{
    public class POTViewModel
    {
        public ProductOption ProductOption { get; set; }
        public List<ProductOption> ProductOptions { get; set; }
    }
}
