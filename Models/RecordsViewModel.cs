using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VehicleTracker.Models
{
    public class RecordsViewModel
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Date { get; set; }
        public int? Mileage { get; set; }

        public string? Name { get; set; }

        public string? Notes { get; set; }

        public double? Cost { get; set; }

        public int? CategoryId { get; set; }

        public List<SelectListItem>? Categories { get; set; }

        public IFormFile? coverImage { get; set; }
    }
}
