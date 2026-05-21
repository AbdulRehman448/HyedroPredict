using System;
using System.ComponentModel.DataAnnotations;

namespace HydroPredict.Models
{
    public enum BookingStatus { PendingApproval, ApprovedAndAssigned, InTransit, Delivered, Cancelled }

    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TargetVolumeLiters { get; set; }
        public decimal DynamicTariffCalculated { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public DateTime TargetDeliveryWindow { get; set; }
        public BookingStatus ManifestStatus { get; set; } = BookingStatus.PendingApproval;
        public int? AssignedDriverId { get; set; }
        public DateTime RecordCreated { get; set; } = DateTime.UtcNow;
    }
}

