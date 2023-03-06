using Order.Host.Models.Responses;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<SuccessfulResultResponse> CommitTrasactionForTheUser(int ID, decimal amount)
        {
            var result = new SuccessfulResultResponse() { IsCompletedSuccessfully = true };
            return result;
        }
        public async Task<SuccessfulResultResponse> CheckTrasactionForAvailabilityForUser(int ID, decimal amount)
        {
            var result = new SuccessfulResultResponse() { IsCompletedSuccessfully = true };
            return result;
        }
    }
}
