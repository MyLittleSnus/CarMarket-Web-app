namespace Razor_Test.Models
{
    public class CompleteCarModel
    {
        public CarViewModel CarViewModel { get; set; }

        public int MinSeats { get; set; }
        public int MaxSeats { get; set; }
        public List<string> EngineVolumes { get; set; }
        public List<string> LightTypes { get; set; }
        public List<string> AvailableGearBoxes { get; set; }
    }
}