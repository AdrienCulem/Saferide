namespace Saferide.GPS
{
    public static class UserPosition
    {
        /// <summary>
        /// User's latitude
        /// </summary>
        public static double Latitude = 0;
        /// <summary>
        /// User's longitude
        /// </summary>
        public static double Longitude = 0;
        /// <summary>
        /// User's current direction
        /// </summary>
        public static double Heading;
        /// <summary>
        /// User's current speed
        /// </summary>
        public static double Speed;
        /// <summary>
        /// The street in which the user is
        /// </summary>
        public static string Address;
    }
}
