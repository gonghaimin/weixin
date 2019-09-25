using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSub
{
    internal class MemoryPubSub : IPubSub
    {
        private static readonly ConcurrentDictionary<string, MessageNotifier> Cache = new ConcurrentDictionary<string, MessageNotifier>();

        public void BroadCastPublish(string toChannel, string message)
        {
            if (!Cache.ContainsKey(toChannel))
            {
                return;
            }

            var notifier = Cache[toChannel];
            if (notifier != null)
                notifier.RaiseMessageEvent(toChannel, message);
        }

        public IDisposable Subscribe(string channel, Action<string, string> onMessage)
        {
            var notifier = Cache.GetOrAdd(channel, s => new MessageNotifier());
            var sub = new MemorySubscribe(channel, onMessage);
            notifier.MessageArrived += sub.RaiseMessaging;
            return sub;
        }

        public Task BroadCastPublishAsync(string toChannel, string message)
        {
            BroadCastPublish(toChannel, message);
            return Task.FromResult(1);
        }

        internal class MemorySubscribe : IDisposable
        {
            private readonly string _channel;

            public event Action<string, string> OnMessage;

            public MemorySubscribe(string channel, Action<string, string> onMessage)
            {
                _channel = channel;
                if (onMessage != null)
                {
                    OnMessage += onMessage;
                }
            }

            protected virtual void OnMessaging(string channel, string message)
            {
                var handler = OnMessage;
                if (handler != null)
                {
                    handler(channel, message);
                }
            }

            public void RaiseMessaging(string channel, string message)
            {
                OnMessaging(channel, message);
            }

            public void Dispose()
            {
                Cache[_channel].MessageArrived -= RaiseMessaging;
                if (Cache[_channel].MessageArrived == null)
                {
                    ((IDictionary<string, MessageNotifier>)Cache).Remove(_channel);
                }
            }
        }

        internal class MessageNotifier
        {
            public Action<string, string> MessageArrived;

            public void RaiseMessageEvent(string channel, string message)
            {
                Action<string, string> handler = MessageArrived;
                if (handler != null)
                {
                    handler(channel, message);
                }
            }
        }
    }
}
