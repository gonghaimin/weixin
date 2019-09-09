using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    public class VideoHandler : IHandler
    {
        public VideoHandler(string requestXml) : base(requestXml)
        {
        }

        public VideoHandler(string requestXml, SignModel signModel) : base(requestXml, signModel)
        {
        }

        public override string HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
