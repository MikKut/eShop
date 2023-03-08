using Order.Host.Models.Responses;

namespace Order.Host.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<SuccessfulResultResponse> CommitTrasactionForTheUser(int ID, decimal amount);
        Task<SuccessfulResultResponse> CheckTrasactionForAvailabilityForUser(int ID, decimal amount);
    }
}