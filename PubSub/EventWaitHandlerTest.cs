using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PubSub
{
    /// <summary>
    /// 自动事件调用Set()只会向一个阻塞线程发出信号恢复执行，然后自动调用Reset()，而手动事件调用Set()后会向所有阻塞线程发出信号然后都恢复执行
    /// </summary>
    public class EventWaitHandlerTest
    {
        private volatile static int num = 0;
        private static EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        /// <summary>
        /// 多线程顺序打印0到100
        /// 两种实现方式：手动事件和自旋锁
        /// </summary>
        public static  void Print()
        {
            for(var i = 0; i < 100;i++)
            {
                var m = i;
                Task.Run(() => {
                    while (m != num)
                    {
                        eventWaitHandle.WaitOne();
                    }
                    //SpinWait.SpinUntil(() => m == num);
                    Console.WriteLine(num);
                    Interlocked.Increment(ref num);
                    eventWaitHandle.Set();//手动事件，发射信号后，所有阻塞线程都可以解除阻塞
                });
            }
            Console.ReadLine();
        }
    }
}
