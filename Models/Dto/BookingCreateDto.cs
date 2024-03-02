using System.ComponentModel.DataAnnotations;

namespace Marcoff_API.Models.Dto
{
    public class BookingCreateDto
    {
      
        public string Name { get; set; }

        public string Detail { get; set; }

        public double Fee { get; set; }

        public int BedsOcuppied { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
