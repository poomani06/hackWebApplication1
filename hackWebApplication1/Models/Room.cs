
using System.ComponentModel.DataAnnotations;

namespace hackWebApplication1.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Single|Shared)$",
            ErrorMessage = "Room type must be Single or Shared.")]
        public string RoomType { get; set; } = string.Empty;

        [Range(1, int.MaxValue,
            ErrorMessage = "Capacity must be greater than zero.")]
        public int Capacity { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "Occupied beds cannot be negative.")]
        public int OccupiedBeds { get; set; }

        [Range(typeof(decimal), "0", "9999999999999999",
            ErrorMessage = "Monthly fee must be zero or greater.")]
        public decimal MonthlyFee { get; set; }
    }
}