using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saferide.Web.Models.Poco
{
    public class Position
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public double RadLatitude { get; set; }
        public double RadLongitude { get; set; }
    }
}
