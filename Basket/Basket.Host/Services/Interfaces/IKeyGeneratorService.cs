using Basket.Host.Models.Dtos;

namespace Basket.Host.Services.Interfaces
{
    public interface IKeyGeneratorService
    {
        string GenerateKey(UserDto catalogItem);
    }
}