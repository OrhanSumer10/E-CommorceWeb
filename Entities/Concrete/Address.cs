using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Address : IEntity
    {
        [Key]
        public int AddressId { get; set; } // Adres ID'si
        public int UserId { get; set; } // Kullanıcı ID'si
        public string Street { get; set; } // Sokak adı ve numarası
        public string City { get; set; } // Şehir
        public string State { get; set; } // Eyalet / Bölge
        public string Neighbourhood { get; set; } //Mahalle
        public string AddressTitle { get; set; } // Başlık
        public bool IsDefaultShipping { get; set; } // Varsayılan gönderim adresi mi?
        public bool IsDefaultBilling { get; set; } // Varsayılan fatura adresi mi?
        public ApUser User { get; set; } // Bu adresin ait olduğu kullanıcı
    }
}
