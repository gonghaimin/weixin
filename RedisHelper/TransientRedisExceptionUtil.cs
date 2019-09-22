using Polly;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedisHelper
{
    public static class TransientRedisExceptionUtil
    {
        private static bool IsTransient(this Exception exception)
        {
            if (exception.InnerException is TimeoutException)
            {
                return true;
            }

            var redisConnectionException = exception.InnerException as RedisConnectionException;
            if (redisConnectionException == null)
            {
                return false;
            }
            return redisConnectionException.FailureType == ConnectionFailureType.UnableToResolvePhysicalConnection;
        }


        private const int MaxRetryCount = 10;

        private static int _resetLock = 0;

        public static readonly Policy RetryOnTransientRedisExceptionPolicy =
            Policy.Handle<TimeoutException>()
                .Or<RedisConnectionException>(
                    exception => exception.FailureType == ConnectionFailureType.UnableToResolvePhysicalConnection)
                .Or<RedisConnectionException>(exception => exception.FailureType == ConnectionFailureType.SocketFailure)
                .WaitAndRetry(MaxRetryCount, retry =>
                {
                    if (retry == MaxRetryCount)
                    {
                        if (Interlocked.CompareExchange(ref _resetLock, 1, 0) == 0)
                        {
                            //写个程序重启服务

                        }
                    }
                    return TimeSpan.FromMilliseconds(100 * retry);
                });

        public static readonly AsyncPolicy RetryAsyncOnTransientRedisExceptionPolicy =
            Policy.Handle<TimeoutException>()
                .Or<RedisConnectionException>(
                    exception => exception.FailureType == ConnectionFailureType.UnableToResolvePhysicalConnection)
                .Or<RedisConnectionException>(exception => exception.FailureType == ConnectionFailureType.SocketFailure)
                .WaitAndRetryAsync(MaxRetryCount,  retry =>
                {
                    if (retry == MaxRetryCount)
                    {
                        if (Interlocked.CompareExchange(ref _resetLock, 1, 0) == 0)
                        {
                            //写个程序重启服务
                           
                        }
                    }
                    return TimeSpan.FromMilliseconds(100 * retry);
                });
    }
}
