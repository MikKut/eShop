using Basket.Host.Models.Dtos;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services
{
    public class KeyGeneratorService : IKeyGeneratorService
    {
        public string GenerateKey(UserDto user)
        {
            return user.UserName.Replace(" ", string.Empty) + user.UserId;
        }
    }
}
