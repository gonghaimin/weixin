using System;
using System.Collections.Generic;
using System.Text;

namespace Rns.Core.Infranstructure
{
    public interface IEngine
    {
        T Resolve<T>() where T : class;
    }
}
