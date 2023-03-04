using Basket.Host.Models.Dtos;
using Basket.Host.Models.Items;
using Basket.Host.Services.Interfaces;
using MVC.Models.Dto;

namespace Basket.Host.Services
{
    public class KeyGeneratorService : IKeyGeneratorService
    {
        public string GenerateKey(UserDto user)
        {
            return user.UserName + user.UserId;
        }
    }
}
