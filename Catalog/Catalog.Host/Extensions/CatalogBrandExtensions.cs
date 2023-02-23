using Catalog.Host.Data.Entities;

namespace Catalog.Host.Extensions
{
    public static class CatalogBrandExtensions
    {
        public static bool Equal(this CatalogBrand firstObj, CatalogBrand secondObj)
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
