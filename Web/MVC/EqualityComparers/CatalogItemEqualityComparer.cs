using MVC.Models.Dto;
using System.Diagnostics.CodeAnalysis;

namespace MVC.EqualityComparers;

public class CatalogItemEqualityComparer : IEqualityComparer<CatalogItemDto>
{
    public bool Equals(CatalogItemDto? x, CatalogItemDto? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null  || y is null)
        {
            return false;
        }

        return x.Id == y.Id;
    }

    public int GetHashCode([DisallowNull] CatalogItemDto obj)
    {
        return obj.Id.GetHashCode();
    }
}