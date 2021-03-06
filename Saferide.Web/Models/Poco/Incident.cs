﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saferide.Web.Models.Poco
{
    public class Incident
    {
        [Key]
        public int IncidentId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string IncidentType { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string UserId { get; set; }
        public int? Trust { get; set; }

        [NotMapped]
        public bool Confirmed { get; set; }
        public ApplicationUser User { get; set; }
    }
}