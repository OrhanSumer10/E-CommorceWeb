using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductOptionTemplate :IEntity
    {
        [Key]
        public int TemplateId { get; set; } // Şablon ID
        public string OptionName { get; set; } // Seçenek adı (örn. Numara, Beden)
        public string OptionValue { get; set; } // Değer (örn. 36, 37, M, L)
    }
}
