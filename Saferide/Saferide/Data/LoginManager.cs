using System;
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

        public async Task<string> Authenticate(LoginUser user)
        {
            return await _restService.Authenticate(user);
        }
    }
}
