using System.Threading.Tasks;
using HydroPredict.Models;

namespace HydroPredict.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, User? User, string ErrorMessage)> LoginAsync(string email, string password);
        Task<(bool Success, string ErrorMessage)> RegisterConsumerAsync(string name, string email, string password);
        Task<string> GetCurrentUserAsync(); // Added to route real-time persistent profile identity titles
    }
}