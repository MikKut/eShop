using MVC.ViewModels.Models;

namespace MVC.ViewModels.BasketViewModels
{
    public class BasketIndexViewModel
    {
        public IEnumerable<CatalogBasketItem> CatalogBasketItems { get; set; }
        public decimal Total { get; set; }
    }
}
