using Dierentuin.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dierentuin.Models
{
    public class Animals
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int? CategoryId { get; set; }
        public CustomSize Size { get; set; }
        public DietaryClass DietaryClass { get; set; }
        public ActivityPattern ActivityPattern { get; set; }
        public string Prey { get; set; }
        public int EnclosureId { get; set; }
        public Enclosure Enclosure { get; set; }
        public double SpaceRequirement { get; set; }
        public SecurityLevel SecurityRequirement { get; set; }

        public Category Category { get; set; }
    }
}
