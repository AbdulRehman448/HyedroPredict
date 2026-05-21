using System;
using System.ComponentModel.DataAnnotations;

namespace HydroPredict.Models
{
    public enum AccessRole { Admin, Driver, Consumer }

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string SecurityEmail { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public AccessRole AccessRole { get; set; } = AccessRole.Consumer;
        public int? DailyCapacityLiters { get; set; }
        public DateTime RecordCreated { get; set; } = DateTime.UtcNow;
    }
}