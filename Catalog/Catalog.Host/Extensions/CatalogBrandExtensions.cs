using Catalog.Host.Data.Entities.Interfaces;

namespace Catalog.Host.Extensions
{
    public static class CatalogBrandExtensions
    {
        public static bool Equal(this ICatalogBrand firstObj, ICatalogBrand secondObj)
        {
            return firstObj != null && secondObj != null
                && firstObj.Id == secondObj.Id
                && firstObj.Brand == secondObj.Brand;
        }
    }
}
