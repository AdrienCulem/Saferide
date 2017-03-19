using System;
using Saferide.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saferide.Data
{
    public interface IRestService
    {
        Task<String> Authenticate(User user);
        Task<String> NewIncident(Incident incident);
    }
}