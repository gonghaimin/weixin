using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSub
{
    public interface IPubSub
    {
        void BroadCastPublish(string toChannel, string message);
        IDisposable Subscribe(string channel, Action<string, string> onMessage);
        Task BroadCastPublishAsync(string toChannel, string message);
    }
}
