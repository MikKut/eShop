using Basket.Host.Models.Dtos;
using Basket.Host.Models.Items;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services
{
    public class KeyGeneratorService : IKeyGeneratorService
    {
        public string GenerateKey<T>(OrderDto<T> catalogItem)
        {
            return catalogItem.UserName + catalogItem.UserId;
        }
    }
}
