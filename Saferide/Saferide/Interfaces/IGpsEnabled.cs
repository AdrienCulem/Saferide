using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
