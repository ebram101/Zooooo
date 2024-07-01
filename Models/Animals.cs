using Zoo.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoo.Models
{
    public class Animals
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Species { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        public CustomSize Size { get; set; }

        [Required]
        public DietaryClass DietaryClass { get; set; }

        [Required]
        public ActivityPattern ActivityPattern { get; set; }

        public string Prey { get; set; }

        public int EnclosureId { get; set; }

        [ForeignKey("EnclosureId")]
        public Enclosure Enclosure { get; set; }

        [Required]
        public double SpaceRequirement { get; set; }

        [Required]
        public SecurityLevel SecurityRequirement { get; set; }
    }
}
