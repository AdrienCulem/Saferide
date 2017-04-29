namespace Saferide.Models
{
    public class Incident
    {
        public int IncidentId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string IncidentType { get; set; }

        public string Description { get; set; }
        public string UserId { get; set; }
    }
}
