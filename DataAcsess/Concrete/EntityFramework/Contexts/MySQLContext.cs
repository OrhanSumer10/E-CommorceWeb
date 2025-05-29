using Entities.Concrete;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsess.Concrete.EntityFramework.Contexts
{
    public class MySQLContext : DbContext
    {


        private static IConfiguration _configuration; // Statik alan

        static MySQLContext()
        {
            // Statik yapılandırıcı içinde yapılandırma oluştur
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }


        string connectionString = _configuration.GetConnectionString("MyDatabaseConnection");



        //static readonly string connectionString = "server=45.84.189.34\\localhost; database=bilgegul_netcorelaw; user=bilgegul_er; password=Gara4ever*";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<ApUser> ApplicationUsers { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategories> subCategories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponProduct> CouponProducts { get; set; }
        public DbSet<CouponUser> CouponUsers { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<CreditCard> creditCards { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<ProductOption>()
                            .HasOne(o => o.Product)
                            .WithMany(p => p.Options)
                            .HasForeignKey(o => o.ProductId)
                            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductReview>()
                            .HasOne(o => o.Product)
                            .WithMany(p => p.Reviews)
                            .HasForeignKey(o => o.ProductId)
                            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductImages>()
                            .HasOne(p => p.product)
                            .WithMany(o => o.ProductImages)
                            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                                 .HasOne(o => o.Product)
                                 .WithMany(p => p.cartItems)
                                 .HasForeignKey(o => o.ProductId)
                                 .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<CartItem>()
                                    .HasOne(o => o.Order)
                                    .WithMany(p => p.CartItems)
                                    .HasForeignKey(o => o.OrderId)
                                    .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<SubCategories>()
                .HasOne(c => c.Category)
                .WithMany(p => p.SubCategories)
                .HasForeignKey(f => f.ParentCategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CartItem>()
        .HasKey(c => c.CartItemId);
            // OrderDetail için bileşik anahtar tanımlaması
            modelBuilder.Entity<CouponProduct>(entity =>
            {
                entity.HasKey(cp => cp.ID);
                entity.Property(cp => cp.ID).ValueGeneratedOnAdd();

                entity.HasOne(cp => cp.Coupon)
                    .WithMany(c => c.CouponProducts)
                    .HasForeignKey(cp => cp.CouponId)
                    .OnDelete(DeleteBehavior.Restrict); // veya Cascade, ClientSetNull gibi
                modelBuilder.Entity<CouponProduct>()
             .HasOne(cu => cu.Product)
             .WithMany(u => u.CouponProducts)
             .HasForeignKey(cu => cu.ProductId)
             .OnDelete(DeleteBehavior.Restrict); // User silinmesin
            });
            modelBuilder.Entity<CouponUser>(entity =>
            {
                entity.HasKey(cu => cu.ID);
                entity.Property(cu => cu.ID).ValueGeneratedOnAdd();

                modelBuilder.Entity<CouponUser>()
          .HasOne(cu => cu.Coupon)
          .WithMany(c => c.CouponUsers)
          .HasForeignKey(cu => cu.CouponId)
          .OnDelete(DeleteBehavior.Restrict); // veya NoAction

                modelBuilder.Entity<CouponUser>()
                    .HasOne(cu => cu.User)
                    .WithMany(u => u.CouponUsers)
                    .HasForeignKey(cu => cu.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // User silinmesin
            });



            // Order ve diğer ilişkiler
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User) //Her sipariş (Order), bir kullanıcıya (User) bağlıdır.
                .WithMany(u => u.Orders) //Bir kullanıcı birden fazla sipariş verebilir (WithMany).
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<BrandCategories>()
        .HasKey(bc => new { bc.BrandId, bc.CategoryId });

            modelBuilder.Entity<BrandCategories>()
                .HasOne(bc => bc.Brand)
                .WithMany(b => b.BrandCategories)
                .HasForeignKey(bc => bc.BrandId);




            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(pr => pr.ProductId);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.User)
                .WithMany(u => u.ProductReviews)
                .HasForeignKey(pr => pr.UserId);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);


            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<int>(); // Enum'u int olarak sakla

            // WishlistItem ile ApplicationUser ilişkisini yapılandırır
            modelBuilder.Entity<WishlistItem>()
                .HasOne(w => w.ApplicationUser)  // Her WishlistItem bir ApplicationUser'a sahiptir
                .WithMany(u => u.WishlistItems)  // Her ApplicationUser'un birçok WishlistItem'ı olabilir
                .HasForeignKey(w => w.ApplicationUserId)  // WishlistItem'daki ApplicationUserId, ApplicationUser tablosundaki ID'ye karşılık gelir
                .OnDelete(DeleteBehavior.Cascade);  // ApplicationUser silindiğinde ilişkili WishlistItem'lar da silinir

            modelBuilder.Entity<WishlistItem>()
                .HasOne(w => w.Product)  // Her WishlistItem bir Product'a sahiptir
                .WithMany()  // Product modelinde WishlistItems koleksiyonu olmayabilir
                .HasForeignKey(w => w.ProductId)  // WishlistItem'daki ProductId, Product tablosundaki ID'ye karşılık gelir
                .OnDelete(DeleteBehavior.Cascade);  // Product silindiğinde, WishlistItem'lar etkilenmez

            // CartItem ile ApplicationUser ilişkisini yapılandırır
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.ApplicationUser)  // Her CartItem bir ApplicationUser'a sahiptir
                .WithMany(u => u.CartItems)  // Her ApplicationUser'un birçok CartItem'ı olabilir
                .HasForeignKey(c => c.ApplicationUserId)  // CartItem'daki ApplicationUserId, ApplicationUser tablosundaki ID'ye karşılık gelir
                .OnDelete(DeleteBehavior.Cascade);  // ApplicationUser silindiğinde ilişkili CartItem'lar da silinir

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Product)  // Her CartItem bir Product'a sahiptir
                .WithMany()  // Product modelinde CartItems koleksiyonu olmayabilir
                .HasForeignKey(c => c.ProductId)  // CartItem'daki ProductId, Product tablosundaki ID'ye karşılık gelir
                .OnDelete(DeleteBehavior.Cascade);  // Product silindiğinde, CartItem'lar etkilenmez



        }
    }
}
