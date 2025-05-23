using Core.Utilities.Results;
using Entities.Concrete;

namespace ECommorceWeb.Models
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();

        public Product Product { get; set; } = new Product(); // Artık tek bir Product tutuyoruz      public List<Product> Products { get; set; } = new List<Product>();

        public ProductImages productImage { get; set; } = new ProductImages(); // Artık tek bir Product tutuyoruz
        public List<ProductImages> productImages { get; set; } = new List<ProductImages>();
        public Dictionary<int, List<ImageViewModel>> ProductImages { get; set; } // Ürün ID'lerine göre resim yolları listesi
        public List<Category> Categories { get; set; } = new List<Category>();
        public Category Category { get; set; } = new Category();
        public List<SubCategories> SubCategories { get; set; } = new List<SubCategories>();
        public List<ProductOption> ProductOptions { get; set; } = new List<ProductOption>();
        public SubCategories SubCategory { get; set; } = new SubCategories();


        public List<Address> addresses { get; set; } = new List<Address>();
        public Address address { get; set; } = new Address();

        public List<CreditCard> creditCards { get; set; } = new List<CreditCard>();
        public CreditCard CreditCard { get; set; } = new CreditCard();

        public List<CartItem> cartItems { get; set; } = new List<CartItem>();
        public CartItem cartItem { get; set; } = new CartItem();

        public List<ApUser> apUsers { get; set; } = new List<ApUser>();
        public ApUser apUser { get; set; } = new ApUser();


        public List<Order> orders { get; set; } = new List<Order>();
        public Order order { get; set; } = new Order();


        public List<Coupon> coupons { get; set; } = new List<Coupon>();
        public Coupon coupon { get; set; } = new Coupon();  
        public List<CouponProduct> couponProducts { get; set; } = new List<CouponProduct>();
        public CouponProduct couponProduct { get; set; } = new CouponProduct();
        public List<CouponUser> couponUsers { get; set; } = new List<CouponUser>();
        public CouponUser couponUser { get; set; } = new CouponUser();

        public List<Notification> notifications { get; set; } = new List<Notification>();
        public Notification notification { get; set; } = new Notification();


        public List<ProductReview> reviews { get; set; } = new List<ProductReview>();
        public ProductReview review { get; set; } = new ProductReview();

        public List<WishlistItem> wishlistItems { get; set; }
        public List<ApUser> ApplicationUsers { get; set; }


        public class ImageViewModel
        {
            public int pImgId { get; set; }
            public string ImageBase64 { get; set; }
        }

    }
}
