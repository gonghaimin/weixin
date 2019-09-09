using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    public class VoiceHandler : IHandler
    {
        public VoiceHandler(string requestXml) : base(requestXml)
        {
        }

        public VoiceHandler(string requestXml, SignModel signModel) : base(requestXml, signModel)
        {
        }

        public override string HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
