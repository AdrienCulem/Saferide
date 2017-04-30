using System;
using Saferide.Models;

//using Plugin.Geolocator.Abstractions;

namespace Saferide.Helpers
{
    public class PositionConverter
    {
        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="degrees">
        /// The degrees to convert
        /// </param>
        /// <returns>
        /// The degrees in radians
        /// </returns>
        public static double ConvertDegreesToRadians(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }
        /// <summary>
        /// Convert radians to degree
        /// </summary>
        /// <param name="radian">
        /// The radians to convert
        /// </param>
        /// <returns>
        /// The radians in degrees
        /// </returns>
        public static double ConvertRadiansToDegrees(double radian)
        {
            return radian * (180.0 / Math.PI);
        }


        private double radLat; // latitude in radians
        private double radLon; // longitude in radians

        private double degLat; // latitude in degrees
        private double degLon; // longitude in degrees

        private static double MIN_LAT = ConvertDegreesToRadians(-90d); // -PI/2
        private static double MAX_LAT = ConvertDegreesToRadians(90d); //  PI/2
        private static double MIN_LON = ConvertDegreesToRadians(-180d); // -PI
        private static double MAX_LON = ConvertDegreesToRadians(180d); //  PI

        private const double earthRadius = 6371.01;

        public PositionConverter(Position pos)
        {
            radLat = ConvertDegreesToRadians(pos.Latitude);
            radLon = ConvertDegreesToRadians(pos.Longitude);
            degLat = pos.Latitude;
            degLon = pos.Longitude;

        }

        public Position[] BoundingCoordinates(double distance)
        {
            if (distance < 0d)
                throw new Exception("Distance cannot be less than 0");

            // angular distance in radians on a great circle
            double radDist = distance / earthRadius;

            double minLat = radLat - radDist;
            double maxLat = radLat + radDist;

            double minLon, maxLon;
            if (minLat > MIN_LAT && maxLat < MAX_LAT)
            {
                double deltaLon = Math.Asin(Math.Sin(radDist) /
                                            Math.Cos(radLat));
                minLon = radLon - deltaLon;
                if (minLon < MIN_LON) minLon += 2d * Math.PI;
                maxLon = radLon + deltaLon;
                if (maxLon > MAX_LON) maxLon -= 2d * Math.PI;
            }
            else
            {
                // a pole is within the distance
                minLat = Math.Max(minLat, MIN_LAT);
                maxLat = Math.Min(maxLat, MAX_LAT);
                minLon = MIN_LON;
                maxLon = MAX_LON;
            }

            Position[] array = new Position[2];
            array[0] = new Position()
            {
                Latitude = ConvertRadiansToDegrees(minLat),
                Longitude = ConvertRadiansToDegrees(minLon)
            };
            array[1] = new Position()
            {
                Latitude = ConvertRadiansToDegrees(maxLat),
                Longitude = ConvertRadiansToDegrees(maxLon)
            };
            return array;
        }
    }
}
