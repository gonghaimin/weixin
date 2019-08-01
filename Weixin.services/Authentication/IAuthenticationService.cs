using Weixin.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Services.Authentication
{
    public interface IAuthenticationService
    {
        void SignIn(User user);

        void SignOut();

        User GetCurrentUser();
    }
}
