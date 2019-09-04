using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Tool.Utility
{
    public class SignModel
    {
        public string signature { get; set; }
        public string timestamp { get; set; }
        public string nonce { get; set; }
        public string echostr { get; set; }
        public string encrypt_type { get; set; }
        public string msg_signature { get; set; }
    }
}
