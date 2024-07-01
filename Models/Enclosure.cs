namespace Dierentuin.Models
{
    public class Enclosure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Animals> Animals { get; set; }
        public string Climate { get; set; }
        public string HabitatType { get; set; }
        public string SecurityLevel { get; set; }
        public double Size { get; set; }
    }
}
