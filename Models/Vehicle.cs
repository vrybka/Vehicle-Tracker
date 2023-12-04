using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VehicleTracker.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Make { get; set; }
        [Column(TypeName = "nvarchar(50)")]

        [DisplayName("Model")]
        public string? CarModel { get; set; }
        public int? Year { get; set; }

        public string? Notes { get; set; }

        // establishing one-to-many relationship between vehicles and records 
        public ICollection<Records>? Records { get; set; }

        public string? Image { get; set; }
    }
}
