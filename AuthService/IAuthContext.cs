using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService
{
    public abstract class IAuthContext
    {
       private static readonly List<string> allowAnonymousPath;
       public abstract bool IsAllowAnonymous(string path);
       public virtual void TryInit(string json)
       {

       }
       public abstract bool Authorize(string path);
    }
}
