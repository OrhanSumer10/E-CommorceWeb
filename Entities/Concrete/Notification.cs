using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Notification : IEntity 
    {
        [Key]
        public int NotificationId { get; set; } // Bildirim ID'si
        public string Content { get; set; } // Bildirim içeriği
        public DateTime CreatedAt { get; set; } // Bildirimin oluşturulma tarihi
        public NotificationStatus status { get; set; } // Bildirimin okunup okunmadığını belirten flag



        public enum NotificationStatus
        {
            Info,           // Genel bilgi amaçlı (önemsiz değil ama acil değil)
            Important,      // Kullanıcının dikkat etmesi gereken önemli bildirim
            Promotion,      // İndirim veya kampanya bildirimi
            Holiday,        // Özel gün, tatil, bayram gibi bildiriler
            Warning,        // Uyarı içeren kritik bildiriler (hesap kapanması, güvenlik vb.)
            System,         // Sistem kaynaklı otomatik bildirimler
            Other           // Diğer bildiriler
        }
    }

}
