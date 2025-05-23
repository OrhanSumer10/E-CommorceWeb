using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ApUser :  IEntity
    {
        [Key]
        public int ApplicationUserId { get; set; } // Kullanıcı ID'si
        public string UserName { get; set; } // Kullanıcı adı
        public string? Name { get; set; } // Kullanıcı adı
        public string? LastName { get; set; } // Kullanıcı soy adı
        public string Email { get; set; } // E-posta adresi
        public string PasswordHash { get; set; } // Şifre hash'lenmiş hali
        public string? IsAdmin { get; set; } // Admin olup olmadığını belirten flag
        public bool? IsActive { get; set; } // Hesabın aktif olup olmadığını belirten flag
        public string? PhoneNumber { get; set; } // Kullanıcının telefon numarası
        public string? Gender { get; set; } // Kullanıcının cinsiyeti
        public DateTime? DateOfBirth { get; set; } // Kullanıcının doğum tarihi
        public DateTime? CreatedAt { get; set; } // Hesabın oluşturulma tarihi
        public DateTime? LastLogin { get; set; } // Kullanıcının son giriş tarihi
        public string? ProfilePictureUrl { get; set; } // Kullanıcının profil fotoğrafı URL'si
        public ICollection<ProductReview> ProductReviews { get; set; } // Kullanıcının yazdığı incelemeler
        public ICollection<Order> Orders { get; set; } // Kullanıcının verdiği siparişler
        public ICollection<Address> Addresses { get; set; } // Kullanıcının adresleri
        public ICollection<WishlistItem> WishlistItems { get; set; } // Kullanıcının favori ürünleri
        public ICollection<CartItem> CartItems { get; set; } // Kullanıcının alışveriş sepeti
        public ICollection<Notification> Notifications { get; set; } // Kullanıcının bildirimleri

        public ICollection<CouponUser> CouponUsers { get; set; }

        public ApUser()
        {
            ProductReviews = new HashSet<ProductReview>();
            Orders = new HashSet<Order>();
            Addresses = new HashSet<Address>();
            WishlistItems = new HashSet<WishlistItem>();
            CartItems = new HashSet<CartItem>();
            Notifications = new HashSet<Notification>();

        }
    }
}
