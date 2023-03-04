using Basket.Host.Models.Dtos;
using Basket.Host.Models.Items;
using MVC.Models.Dto;

namespace Basket.Host.Services.Interfaces
{
    public interface IKeyGeneratorService
    {
        string GenerateKey(UserDto catalogItem);
    }
}