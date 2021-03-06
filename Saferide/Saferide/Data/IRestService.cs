﻿using System;
using Saferide.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saferide.Data
{
    public interface IRestService
    {
        Task<String> Authenticate(LoginUser user);
        Task<String> NewIncident(Incident incident);
        Task<String> ConfirmIncident(Incident incident);
        Task<List<Incident>> GetIncidents(Position pos);
    }
}