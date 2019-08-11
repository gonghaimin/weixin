using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weixin.Core.Options
{
    /// <summary>
    /// jwt配置
    /// </summary>
    public class JwtOption
    {
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
        public string Issuer {get;set;}
        public int Expiration { get; set; }

    }
}
