﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saferide.Models;

namespace Saferide.Data
{
    public class LoginManager
    {
        private IRestService _restService;

        public LoginManager(IRestService service)
        {
            _restService = service;
        }
        /// <summary>
        /// Authenticate the user on the sever
        /// </summary>
        /// <param name="user">
        /// The user to log
        /// </param>
        /// <returns>
        /// A string that can be "Success", "Invalid" if password isn't right and "Error" if and exception has been thrown
        /// </returns>
        public async Task<string> Authenticate(LoginUser user)
        {
            return await _restService.Authenticate(user);
        }
    }
}
