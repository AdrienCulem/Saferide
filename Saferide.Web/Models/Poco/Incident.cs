using System.ComponentModel.DataAnnotations;

namespace Saferide.Web.Models.Poco
{
    public class Incident
    {
        [Key]
        public int IncidentId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string IncidentType { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}