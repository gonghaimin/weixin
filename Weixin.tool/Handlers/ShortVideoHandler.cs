﻿using System;
using System.Collections.Generic;
using System.Text;
using Weixin.Tool.Handlers.Base;
using Weixin.Tool.Models;
using Weixin.Tool.Utility;

namespace Weixin.Tool.Handlers
{
    public class ShortVideoHandler : IHandler
    {
        public ShortVideoHandler(string requestXml) : base(requestXml)
        {
        }

        public ShortVideoHandler(string requestXml, SignModel signModel) : base(requestXml, signModel)
        {
        }

        public override string HandleRequest()
        {
            throw new NotImplementedException();
        }
    }
}
