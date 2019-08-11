using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Core.Domain;

namespace AuthService
{
    public interface IAuthService
    {
        void SignIn(User user);

        void SignOut();

        User GetCurrentUser();
    }
}
