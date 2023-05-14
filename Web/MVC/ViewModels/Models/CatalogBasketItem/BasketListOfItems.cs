using MVC.Models.Dto;

namespace MVC.ViewModels.Models.CatalogBasketItem
{
    public class BasketListOfItems
    {
        public Dictionary<CatalogItemDto, int> CatalogItems { get; set; }
    }
}
