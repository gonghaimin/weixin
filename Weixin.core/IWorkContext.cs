using Weixin.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Core
{
    public interface IWorkContext
    {
        User CurrentUser { get; }
    }
}
