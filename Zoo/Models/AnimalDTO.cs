using Zoo.Enums;

namespace Zoo.Models
{
    public class AnimalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public CustomSize Size { get; set; }
        public DietaryClass DietaryClass { get; set; }
        public ActivityPattern ActivityPattern { get; set; }
        public string Prey { get; set; }
        public int EnclosureId { get; set; }
        public string EnclosureName { get; set; }
        public double SpaceRequirement { get; set; }
        public SecurityLevel SecurityRequirement { get; set; }
    }
}
