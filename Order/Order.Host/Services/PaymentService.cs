using Order.Host.Models.Responses;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<SuccessfulResultResponse> CommitTrasactionForTheUser(int ID, decimal amount)
        {
            SuccessfulResultResponse result = new() { IsCompletedSuccessfully = true };
            return Task.FromResult(result);
        }
        public Task<SuccessfulResultResponse> CheckTrasactionForAvailabilityForUser(int ID, decimal amount)
        {
            SuccessfulResultResponse result = new() { IsCompletedSuccessfully = true };
            return Task.FromResult(result);
        }
    }
}
