using System.ComponentModel.DataAnnotations;
using Zoo.Enums;

namespace Zoo.Models
{
    public class Enclosure
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Climate Climate { get; set; }

        [Required]
        public HabitatType HabitatType { get; set; }

        [Required]
        public SecurityLevel SecurityLevel { get; set; }

        [Required]
        public double Size { get; set; }

        public ICollection<Animals> Animals { get; set; }
    }
}
