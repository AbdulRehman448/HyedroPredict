using HydroPredict.Data;
using HydroPredict.Models;
using HydroPredict.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HydroPredict.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string ErrorMessage)> CreateBookingAsync(Booking booking)
        {
            try
            {
                booking.RecordCreated = DateTime.UtcNow;
                booking.ManifestStatus = BookingStatus.PendingApproval;

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Booking failed to save: {ex.Message}");
            }
        }

        public async Task<List<Booking>> GetUserBookingsAsync(int userId)
        {
            try
            {
                return await _context.Bookings
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.RecordCreated)
                    .ToListAsync();
            }
            catch
            {
                return new List<Booking>();
            }
        }
    }
}