using Catalog.Host.Data.Entities;

namespace Catalog.Host.Extensions
{
    public static class CatalogTypeExtensions
    {
        public static bool Equal(this CatalogType firstObj, CatalogType secondObj)
        {
            return firstObj != null && secondObj != null
                && firstObj.Id == secondObj.Id
                && firstObj.Type == secondObj.Type;
        }
    }
}
