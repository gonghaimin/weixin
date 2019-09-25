using RedisHelper;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSub
{
    internal class RedisPubSub : IPubSub
    {
        private static ConnectionMultiplexer _multiplexer = ConnectionMultiplexer.Connect("172.0.0.1:7963");
        public IDatabase Database = _multiplexer.GetDatabase();
        public IDatabase Client { get { return _multiplexer.GetDatabase(); } }
        public ISubscriber Subscriber { get { return _multiplexer.GetSubscriber(); } }
        public void BroadCastPublish(string toChannel, string message)
        {
            try
            {
                TransientRedisExceptionUtil.RetryOnTransientRedisExceptionPolicy.Execute(
                      () => Subscriber.Publish(toChannel, message, CommandFlags.HighPriority));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task BroadCastPublishAsync(string toChannel, string message)
        {
            try
            {
                await Subscriber.PublishAsync(toChannel, message, CommandFlags.HighPriority);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IDisposable Subscribe(string channel, Action<string, string> onMessage)
        {
            return new RedisSubscriber(delegate
            {
                return _multiplexer.GetSubscriber();
            }, channel, onMessage);
        }
    }

    internal class RedisSubscriber : IDisposable
    {
        private static Producer_Consumer_Queue _queue = new Producer_Consumer_Queue();
        private ISubscriber _client;
        private volatile bool _isDisposed;
        private readonly Func<ISubscriber> _clientFactory;
        private static readonly TimeSpan RecoverConnectionInterval = TimeSpan.FromSeconds(30);

        internal Action<RedisChannel, RedisValue> OnMessage { private get; set; }
        private readonly string _channel;

        internal RedisSubscriber(Func<ISubscriber> clientFactory, string channel, Action<string, string> onMessage)
        {
            if (onMessage == null)
            {
                throw new ArgumentNullException("onMessage");
            }
            _client = null;
            _isDisposed = false;
            _clientFactory = clientFactory;
            OnMessage = (redisChannel, value) =>
            {
                _queue.Enqueue(()=> {
                    onMessage(redisChannel, value);
                });
            };

            _channel = channel;

            DoSubscribe(channel).ConfigureAwait(false);
        }

        private async Task DoSubscribe(string channel)
        {
            while (!_isDisposed)
            {
                try
                {
                    _client = _clientFactory();

                    await _client.PingAsync();

                    await _client.SubscribeAsync(channel, OnMessage, CommandFlags.HighPriority);

                    break;
                }
                catch
                {

                }
                await Task.Delay(RecoverConnectionInterval).ConfigureAwait(false);
            }
        }

        private void UnSubscribe()
        {
            try
            {
                _client.Unsubscribe(_channel, flags: CommandFlags.FireAndForget);
            }
            catch
            { }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            UnSubscribe();
        }
    }
}
