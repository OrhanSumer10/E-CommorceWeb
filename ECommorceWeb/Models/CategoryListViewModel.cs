using Entities.Concrete;

namespace ECommorceWeb.Models
{
    public class CategoryListViewModel
    {

        public Category category { get; set; }
        public List<Category> categories { get; set; } = new List<Category>();
    }
}
