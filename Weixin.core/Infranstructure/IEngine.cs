using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Core.Infranstructure
{
    public interface IEngine
    {
        T Resolve<T>() where T : class;
    }
}
