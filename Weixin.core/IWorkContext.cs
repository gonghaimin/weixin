using Rns.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rns.Core
{
    public interface IWorkContext
    {
        User CurrentUser { get; }
    }
}
