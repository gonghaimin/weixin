using System;
using System.Collections.Generic;
using System.Text;

namespace Weixin.Core.Infranstructure
{
    public class EngineContext
    {
        private static IEngine _engine;

        public static void Create(IServiceProvider serviceProvider)
        {
            _engine = new DefaultEngine(serviceProvider);
        }


        public static IEngine Current
        {
            get {
                return _engine;
            }
        }
    }
}
