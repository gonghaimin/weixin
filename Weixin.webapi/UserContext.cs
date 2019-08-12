using AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weixin.Services;

namespace Weixin.WebApi
{
    public class UserContext: IAuthContext
    {
        

        public override bool IsAllowAnonymous(string path)
        {
            return true;
        }

        public override void TryInit(string json)
        {

        }

        public override bool Authorize(string path)
        {
            return true;
        }
    }
}
