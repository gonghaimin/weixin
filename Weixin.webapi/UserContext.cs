using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weixin.Services;

namespace Weixin.WebApi
{
    public class UserContext: IContext
    {
        

        public bool IsAllowAnonymous(string path)
        {
            return true;
        }

        public void TryInit(string json)
        {

        }

        public bool Authorize(string path)
        {
            return true;
        }
    }
}
