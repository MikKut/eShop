using Catalog.Host.Data.Entities.Interfaces;

namespace Catalog.Host.Extensions
{
    public static class CatalogItemExtensions
    {
        public static bool Equal(this ICatalogItem firstObj, ICatalogItem secondObj)
        {
            return firstObj != null && secondObj != null &&
                firstObj.Name == secondObj.Name &&
                firstObj.Id == secondObj.Id &&
                firstObj.Description == secondObj.Description &&
                firstObj.Price == secondObj.Price &&
                secondObj.PictureFileName == firstObj.PictureFileName &&
                secondObj.CatalogBrandId == firstObj.CatalogBrandId &&
                secondObj.CatalogTypeId == firstObj.CatalogTypeId &&
                secondObj.AvailableStock == firstObj.AvailableStock;
        }
    }
}
