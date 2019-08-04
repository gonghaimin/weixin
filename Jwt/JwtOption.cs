using System;
using System.Collections.Generic;
using System.Text;

namespace Jwt
{
    public class JwtOption
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string IssuerSigningKey { get; set; }
        public Double Expiration { get; set; }
    }
}
