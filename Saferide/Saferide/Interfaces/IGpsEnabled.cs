namespace Saferide.Interfaces
{
    public interface IGpsEnabled
    {
        /// <summary>
        /// Checks if the location is enabled
        /// </summary>
        /// <returns>a boolean</returns>
        bool IsGpsEnabled();
    }
}
