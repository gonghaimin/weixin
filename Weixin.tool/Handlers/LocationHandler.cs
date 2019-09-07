using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Messages;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    public class LocationHandler : IHandler
    {
        public LocationHandler(string requestXml) : base(requestXml)
        {
        }

        public LocationHandler(string requestXml, SignModel signModel) : base(requestXml, signModel)
        {
        }

        public override string HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
