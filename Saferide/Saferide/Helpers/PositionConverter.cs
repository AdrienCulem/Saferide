using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
