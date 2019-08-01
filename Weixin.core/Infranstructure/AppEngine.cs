using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Weixin.Core.Infranstructure
{
    public class DefaultEngine : IEngine
    {
        private readonly IServiceProvider _serviceProvider;
        public DefaultEngine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Resolve<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
