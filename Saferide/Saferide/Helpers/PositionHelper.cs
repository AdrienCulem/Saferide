using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.Models;
using Saferide.Ressources;

namespace Saferide.Helpers
{
    public static class PositionHelper
    {
        const int Radiusoftheearth = 6371;
        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        /// <param name="a">First geocode</param>
        /// <param name="b">Second Geocode</param>
        /// <returns>A distance in kilometers</returns>
        public static double DistanceBetweenPoints(Position a, Position b)
        {
            var dLat = PositionConverter.ConvertDegreesToRadians(a.Latitude - b.Latitude);
            var dLon = PositionConverter.ConvertDegreesToRadians(a.Longitude - b.Longitude);
            var d =
                    Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(PositionConverter.ConvertDegreesToRadians(b.Latitude)) * Math.Cos(PositionConverter.ConvertDegreesToRadians(a.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
                ;
            var c = 2 * Math.Atan2(Math.Sqrt(d), Math.Sqrt(1 - d));
            //Distance between current position and that incident
            var distance = Radiusoftheearth * c;
            return distance;
        }
        /// <summary>
        /// Calculate the direction relative to the north that the user should take from his current position
        /// </summary>
        /// <param name="point">Where to go</param>
        /// <param name="current">Current position</param>
        /// <returns>The direction in degrees relative to the north</returns>
        public static double DirectionFromPosition(Position point, Position current)
        {
            var dLat = PositionConverter.ConvertDegreesToRadians(point.Latitude - current.Latitude);
            var dLon = PositionConverter.ConvertDegreesToRadians(point.Longitude - current.Longitude);
            var direction = Math.Atan2(dLon, dLat);
            return Math.Abs(PositionConverter.ConvertRadiansToDegrees(direction));
        }
        /// <summary>
        /// Convert a direction in degress to its cardinal description
        /// </summary>
        /// <param name="heading">The direction to convert</param>
        /// <returns>A string that represents the direction</returns>
        public static string ConvertHeadingToDirection(double heading)
        {
            string[] caridnals = { AppTexts.N, AppTexts.NNE, AppTexts.NE, AppTexts.ENE, AppTexts.E, AppTexts.ESE
                    , AppTexts.SE, AppTexts.SSE, AppTexts.S, AppTexts.SSW, AppTexts.SW, AppTexts.WSW, AppTexts.W
                    , AppTexts.WNW, AppTexts.NW, AppTexts.NNW, AppTexts.N };
            return caridnals[(int)Math.Round(((double)heading * 10 % 3600) / 225)];
        }
    }
}
