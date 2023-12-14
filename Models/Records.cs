using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VehicleTracker.Models
{
    public class Records
    {
        [Key]
        public int Id { get; set; }

        public int VehicleId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Date { get; set; } = DateTime.Now;

        [Column(TypeName = "nvarchar(10)")]
        [DisplayFormat(DataFormatString = "{0:0,000}", ApplyFormatInEditMode = true)]
        public int? Mileage { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? Notes { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double? Cost { get; set; }

        public int? CategoryId { get; set; }
        public Category? Caterogy { get; set; }
    }
}
