using HydroPredict.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HydroPredict.Services.Interfaces
{
    public interface IBookingService
    {
        Task<(bool Success, string ErrorMessage)> CreateBookingAsync(Booking booking);
        Task<List<Booking>> GetUserBookingsAsync(int userId);
    }
}