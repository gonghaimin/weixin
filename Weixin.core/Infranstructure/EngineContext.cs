using System;
using System.Collections.Generic;
using System.Text;

namespace Rns.Core.Infranstructure
{
    public class EngineContext
    {
        private static IEngine _engine;

        public static void Create(IServiceProvider serviceProvider)
        {
            _engine = new AppEngine(serviceProvider);
        }


        public static IEngine Current
        {
            get {
                return _engine;
            }
        }
    }
}
