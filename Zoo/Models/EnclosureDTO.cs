using Zoo.Enums;

namespace Zoo.Models
{
    public class EnclosureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Climate Climate { get; set; }
        public HabitatType HabitatType { get; set; }
        public SecurityLevel SecurityLevel { get; set; }
        public double Size { get; set; }
        public List<ForeignAnimalDTO> Animals { get; set; }
    }
}
