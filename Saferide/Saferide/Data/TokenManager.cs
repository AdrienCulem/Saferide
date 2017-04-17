using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saferide.Data
{
    public class TokenManager
    {
        IRestService _restService;
        public TokenManager(IRestService service)
        {
            _restService = service;
        }

        public async Task<bool> IsTokenValid()
        {
            return await _restService.IsTokenValid();
        }
    }
}
