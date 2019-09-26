
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
        public Producer_Consumer_Queue()
        {

        }

        private volatile static ConcurrentQueue<WorkItem> _queue = new ConcurrentQueue<WorkItem>();
        private volatile int _flag = 0;
        private SemaphoreSlim _semaphore = null;
        private EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private TaskCreationOptions taskOption
        {
            get
            {
                return this.IsLongRunning ? TaskCreationOptions.LongRunning : TaskCreationOptions.None;
            }
        }

        /// <summary>
        /// 是否启用长任务来处理消息
        /// </summary>
        public bool IsLongRunning { get; set; } = false;
        /// <summary>
        /// 发生异常时最大重试次数
        /// </summary>
        public int MaxRetryCount { get; set; } = 10;
        /// <summary>
        /// 任务重试次数
        /// </summary>
        public int? WorkRetryCount { get; set; }
        /// <summary>
        /// 最大任务数量
        /// </summary>
        public int Limt { get; set; } = Environment.ProcessorCount;
        /// <summary>
        /// 是否是密集型生产消息
        /// </summary>
        public bool IsIntensive { get; set; } = false;
        public void Enqueue(Action action)
        {
            WorkItem workItem = new WorkItem();
            workItem.Execute += action;
            workItem.WorkId = DateTime.Now.Ticks;
            if (WorkRetryCount.HasValue)
            {
                workItem.RetryCount = WorkRetryCount.Value;
            }
            
            ProducerHandler(workItem);
        }
        /// <summary>
        /// 生产消息
        /// </summary>
        /// <param name="work"></param>
        private void ProducerHandler(WorkItem work)
        {
            if (IsIntensive)
            {
                _handle.Set();
            }
            _queue.Enqueue(work);
            if (Interlocked.CompareExchange(ref _flag, 1, 0) == 0)
            {
                WriteLog("启动一个task提取消息并创建任务");
                if (_semaphore == null)
                {
                    _semaphore = new SemaphoreSlim(this.Limt);
                }
                ConsumerHandler();
            }
        }
        /// <summary>
        /// 消费者
        /// </summary>
        /// <returns></returns>
        private void ConsumerHandler()
        {
            Task.Factory.StartNew((arg) =>
            {
                while (true)
                {
                    _semaphore.Wait();
                    WriteLog("队列消息个数：" + _queue.Count);
                    WorkItem work;
                    if (_queue.TryDequeue(out work))
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                work.Execute.Invoke();
                                WriteLog("消息消息：" + work.WorkId + "---线程ID：" + Thread.CurrentThread.ManagedThreadId);
                            }
                            catch (Exception e)
                            {
                                if (work.RetryCount < MaxRetryCount)
                                {
                                    work.RetryCount += 1;
                                    this.ProducerHandler(work);
                                }
                                else
                                {
                                    WriteLog(e.Message);
                                }
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
                    if (_queue.IsEmpty)
                    {
                        WriteLog("消息处理完成");
                        if (IsIntensive)
                        {
                            _handle.WaitOne();//没有消息可处理时，阻塞线程，释放cpu
                        }
                        else
                        {
                            SpinWait.SpinUntil(() => !_queue.IsEmpty, 10000);//休眠10秒，如果仍然没有消息，则释放线程
                            if (_queue.IsEmpty)
                            {
                                break;
                            }
                        }
                    }
                }
            }, taskOption).ContinueWith(t =>
            {
                Interlocked.CompareExchange(ref _flag, 0, 1);
                WriteLog("注销任务");
            });
        }

        public static void WriteLog(string msg)
        {
            var dir = System.IO.Directory.GetCurrentDirectory();
            var path = Path.Combine(dir, "Log.txt");
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
            wt.Limt = 10;
            wt.MaxRetryCount = 3;
            wt.IsLongRunning = false;
            wt.IsIntensive = true;
            int i = 0;
            while (true)
            {
                i++;
                Thread.Sleep(500);
                var num = i;
                var action = new Action(() =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("消费消息:" + num);
                });
                wt.Enqueue(action);
                if (i > 20)
                {
                    break;
                }
            }
            while (true)
            {
                var input = Console.ReadLine();
                var action = new Action(() =>
                {
                    Thread.Sleep(2000);
                    Console.WriteLine("消费消息:" + input);
                });
                wt.Enqueue(action);
            }
        }

    }
    internal class WorkItem
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; }
        /// <summary>
        /// 任务id
        /// </summary>
        public long WorkId { get; set; }
        /// <summary>
        /// 任务委托
        /// </summary>
        public Action Execute { get; set; }
        public override string ToString()
        {
            return WorkId.ToString();
        }
    }
}
