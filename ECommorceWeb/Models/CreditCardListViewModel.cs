using Entities.Concrete;

namespace ECommorceWeb.Models
{
    public class CreditCardListViewModel
    {
        public CreditCard CreditCard { get; set; }
        public ApUser apUser { get; set; }
        public List<CreditCard> CreditCards { get; set; }
    }
}
