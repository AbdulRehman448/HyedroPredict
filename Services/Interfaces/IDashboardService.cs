using HydroPredict.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HydroPredict.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<List<Booking>> GetAllBookingsAsync();
        Task<List<User>> GetAvailableDriversAsync();
        Task<bool> AssignDriverAsync(int bookingId, int driverId);
        Task<bool> UpdateBookingStatusAsync(int bookingId, BookingStatus status);
    }
}