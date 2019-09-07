using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    public class LinkHandler : IHandler
    {
        public LinkHandler(string requestXml) : base(requestXml)
        {
        }

        public LinkHandler(string requestXml, SignModel signModel) : base(requestXml, signModel)
        {
        }

        public override string HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
