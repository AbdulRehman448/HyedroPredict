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
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            try
            {
                return await _context.Bookings
                    .OrderByDescending(b => b.RecordCreated)
                    .ToListAsync();
            }
            catch
            {
                return new List<Booking>();
            }
        }

        public async Task<List<User>> GetAvailableDriversAsync()
        {
            try
            {
                return await _context.Users
                    .Where(u => u.AccessRole == AccessRole.Driver)
                    .ToListAsync();
            }
            catch
            {
                return new List<User>();
            }
        }

        public async Task<bool> AssignDriverAsync(int bookingId, int driverId)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null) return false;

                booking.AssignedDriverId = driverId;
                booking.ManifestStatus = BookingStatus.ApprovedAndAssigned;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBookingStatusAsync(int bookingId, BookingStatus status)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null) return false;

                booking.ManifestStatus = status;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}