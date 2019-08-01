using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Rns.Core.Infranstructure
{
    public class AppEngine : IEngine
    {
        private readonly IServiceProvider _serviceProvider;
        public AppEngine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Resolve<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
