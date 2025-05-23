using Core.Entities;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Entities.Concrete
{
    public class CreditCard : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int userId { get; set; }
        public string cartNumber { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int cvv { get; set; }
        public bool IsDefaultCard{ get; set; } // Varsayılan gönderim cart mi?

        public ApUser ApUser { get; set; }
    }
}
