
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PubSub
{
    /// <summary>
    /// 生产-消费-消息，支持线程安全
    /// </summary>
    public class Producer_Consumer_Queue
    {
        public ILogger<Producer_Consumer_Queue> _logger;

        public Producer_Consumer_Queue()
        {

        }
     
        private volatile static ConcurrentQueue<WorkItem> _queue = new ConcurrentQueue<WorkItem>();
        private volatile static int flag = 0;
        private static SemaphoreSlim _semaphore = null;
        private TaskCreationOptions taskOption
        {
            get
            {
                return this.IsLongRunning ? TaskCreationOptions.LongRunning : TaskCreationOptions.None;
            }
        }

        /// <summary>
        /// 是否启用长任务来消息消息
        /// </summary>
        public bool IsLongRunning { get; set; } = false;
        private int _limt = Environment.ProcessorCount;
        /// <summary>
        /// 最大任务数量
        /// </summary>
        public int Limt
        {
            get
            {
                return _limt;
            }
            set
            {
                _limt = value;
                _semaphore = new SemaphoreSlim(value);
            }
        }
       
        public async Task Enqueue(Action action)
        {
            WorkItem workItem = new WorkItem();
            workItem.Execute += action;
            workItem.WorkId = DateTime.Now.Ticks;
           await ProducerHandler(workItem);
        }
        /// <summary>
        /// 生产消息
        /// </summary>
        /// <param name="work"></param>
        private async Task ProducerHandler(WorkItem work)
        {
            _queue.Enqueue(work);
            if (Interlocked.CompareExchange(ref flag, 1, 0) == 0)
            {
                if (_semaphore == null)
                {
                    _semaphore = new SemaphoreSlim(this.Limt);
                }
               await ConsumerHandler();
            }
        }
        /// <summary>
        /// 消费者
        /// </summary>
        /// <returns></returns>
        private async Task ConsumerHandler()
        {
           await Task.Factory.StartNew((arg) =>
            {
                while (!_queue.IsEmpty)
                {
                    _semaphore.Wait();
                    WorkItem work;
                    if (_queue.TryDequeue(out work))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                work.Execute.Invoke();
                            }
                            catch (Exception)
                            {
                            }
                          
                        }).ContinueWith(t =>
                        {
                            _semaphore.Release();
                        });
                    }
                    else
                    {
                        _semaphore.Release();
                    }
                }
            }, taskOption).ContinueWith(t =>
            {
                Interlocked.CompareExchange(ref flag, 0, 1);
            });
        }

        public static void log(string msg)
        {
            var dir = System.IO.Directory.GetCurrentDirectory();
            var path = Path.Combine(dir, "log.txt");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine(msg);
                }
            }
        }

        public static void test()
        {
            Producer_Consumer_Queue wt = new Producer_Consumer_Queue();
            int i = 0;
            while (true)
            {
                var action= new Action(() =>
                {
                    Console.WriteLine("消费消息");
                });
                wt.Enqueue(action).Wait();
                if (i > 1000)
                {
                    break;
                }
            }
            while (true)
            {
                var input = Console.ReadLine();
                var action = new Action(() =>
                {
                    Console.WriteLine("消费消息:" + input);
                });
                wt.Enqueue(action).Wait();
            }
        }

    }
    internal class WorkItem
    {
        public long WorkId { get; set; }
        public Action Execute { get; set; }
        public override string ToString()
        {
            return WorkId.ToString();
        }
    }
}
