

using Entities.Concrete;

namespace ECommorceWeb.Models
{
    public class CartListViewModel
    {
        public List<CartItem>  CartItems { get; set; }
        public List<ApUser>  ApplicationUsers { get; set; }
        public List<ProductImages>  ProductImages { get; set; }
    }
}
