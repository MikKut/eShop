using Catalog.Host.Data.Entities.Interfaces;

namespace Catalog.Host.Extensions
{
    public static class CatalogBrandExtensions
    {
        public static bool Equal(this ICatalogBrand firstObj, ICatalogBrand secondObj)
        {
            if (firstObj == null || secondObj == null)
            {
                return false;
            }

            return firstObj.Id == secondObj.Id
                && firstObj.Brand == secondObj.Brand;
        }
    }
}
