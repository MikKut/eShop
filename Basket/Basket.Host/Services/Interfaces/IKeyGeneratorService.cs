using Basket.Host.Models.Dtos;
using MVC.Models.Dto;

namespace Basket.Host.Services.Interfaces
{
    public interface IKeyGeneratorService
    {
        string GenerateKey(UserDto catalogItem);
    }
}