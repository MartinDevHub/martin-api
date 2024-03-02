using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marcoff_API.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Detail { get; set; }

        public double Fee { get; set; }

        public int BedsOcuppied { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
