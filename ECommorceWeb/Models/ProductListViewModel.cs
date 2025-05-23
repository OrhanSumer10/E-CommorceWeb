using Core.Utilities.Results;
using Entities.Concrete;

namespace ECommorceWeb.Models
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; } 

        public Product Product { get; set; }  // Artık tek bir Product tutuyoruz      public List<Product> Products { get; set; } = new List<Product>();

        public ProductImages productImage { get; set; } // Artık tek bir Product tutuyoruz
        public List<ProductImages> productImages { get; set; }
        public Dictionary<int, List<ImageViewModel>> ProductImages { get; set; } // Ürün ID'lerine göre resim yolları listesi
        public List<Category> Categories { get; set; } 
        public Category Category { get; set; } 
        public List<SubCategories> SubCategories { get; set; } 
        public List<ProductOption> ProductOptions { get; set; }
        public SubCategories SubCategory { get; set; } 


        public List<Address> addresses { get; set; } 
        public Address address { get; set; } 

        public List<CreditCard> creditCards { get; set; }
        public CreditCard CreditCard { get; set; } 

        public List<CartItem> cartItems { get; set; } 
        public CartItem cartItem { get; set; } 

        public List<ApUser> apUsers { get; set; } 
        public ApUser apUser { get; set; } 


        public List<Order> orders { get; set; } 
        public Order order { get; set; }


        public List<Coupon> coupons { get; set; }
        public Coupon coupon { get; set; } 
        public List<CouponProduct> couponProducts { get; set; } 
        public CouponProduct couponProduct { get; set; } 
        public List<CouponUser> couponUsers { get; set; }
        public CouponUser couponUser { get; set; } 

        public List<Notification> notifications { get; set; } 
        public Notification notification { get; set; }


        public List<ProductReview> reviews { get; set; } 
        public ProductReview review { get; set; }

        public List<WishlistItem> wishlistItems { get; set; }
        public List<ApUser> ApplicationUsers { get; set; }


        public class ImageViewModel
        {
            public int pImgId { get; set; }
            public string ImageBase64 { get; set; }
        }

    }
}
