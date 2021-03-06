﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PubSub.LRU
{
    public delegate void RemoveKV<Tkey, TValue>(string cacheName, RemoveEntity<Tkey, TValue> entity);
    public class LRUCache<TKey, TValue>
    {
        const int DEFAULT_CAPACITY = int.MaxValue;

        private int _capacity;
        private ReaderWriterLockSlim locker;
        private IDictionary<TKey, TValue> dictionary;//数据
        private LinkedList<TKey> linkedList;//控制Key的数据
        private Dictionary<TKey, LinkedListNode<TKey>> dicLinkIndex = null;//控制删除
        private Dictionary<TKey, long> dicRefresh = null;//使用时间
        private volatile bool isCheckTime = false;//设置是否监测缓存的时间长度
        private long cacheTime = 600;//10分钟
        private volatile bool isCheckThread = false;//检查线程启动
        private long checkTicks = 0;//换算后的时间
        private BlockingCollection<RemoveEntity<TKey, TValue>> removeEntities = null;
        public event RemoveKV<TKey, TValue> RemoveEntitiesEvent = null;//移除通知
        private DateTime checkTime = DateTime.Now;//结束监测的时间


        /// <summary>
        /// 设置缓存的时间长度，当前按照秒算
        /// 设置时间自动设置属性IsCacheCheckTime=true
        /// </summary>
        public long CacheTime
        {
            get
            {
                return cacheTime;
            }
            set
            {
                cacheTime = value;
                isCheckTime = true;
                countTime();
            }
        }

        /// <summary>
        /// 是否监测时间
        /// </summary>
        public bool IsCacheCheckTime
        {
            get
            { 
                return isCheckTime; 
            }
            set
            {
                isCheckTime = value; 
                countTime();
            }
        }

        /// <summary>
        /// cache名称
        /// </summary>
        public string CacheName { get; set; }

        public LRUCache() : this(DEFAULT_CAPACITY) { }

        public LRUCache(int capacity)
        {
            locker = new ReaderWriterLockSlim();
            _capacity = capacity > 0 ? capacity : DEFAULT_CAPACITY;
            dictionary = new Dictionary<TKey, TValue>();
            linkedList = new LinkedList<TKey>();
            dicLinkIndex = new Dictionary<TKey, LinkedListNode<TKey>>();
            dicRefresh = new Dictionary<TKey, long>();
            removeEntities = new BlockingCollection<RemoveEntity<TKey, TValue>>(1000);

            countTime();
            RemoveNotice();
        }

        /// <summary>
        /// 换算时间;
        /// 将秒转换成ticks个数
        /// </summary>
        private void countTime()
        {

            checkTicks = 10000000 * cacheTime;
        }



        /// <summary>
        /// 更新时间
        /// </summary>
        /// <param name="key"></param>
        private void Refresh(TKey key)
        {
            dicRefresh[key] = DateTime.Now.Ticks;
            if (!isCheckTime)
            {
                return;
            }
            if (!isCheckThread)
            {
                isCheckThread = true;
                Task.Factory.StartNew(() =>
                {

                    double wait = (DateTime.Now - checkTime).TotalSeconds;
                    if (wait < cacheTime)
                    {
                        //如果上次监测到本次监测还未到设置的保持时间，
                        //则等待该时间差后再检查
                        double sleep = ((double)cacheTime - wait) * 1000 + 1;
                        Thread.Sleep((int)sleep);
                    }
                    locker.EnterWriteLock();
                    try
                    {
                        LinkedListNode<TKey> last = null;
                        long tick;
                        long curTick = DateTime.Now.Ticks;
                        last = linkedList.Last;//重后往前找
                        while (last != null)
                        {
                            if (dicRefresh.TryGetValue(last.Value, out tick))
                            {
                                if ((curTick - tick) > checkTicks)
                                {
                                    dicLinkIndex.Remove(last.Value);
                                    dicRefresh.Remove(last.Value);
                                    linkedList.RemoveLast();
                                    RemoveEntity<TKey, TValue> entity = new RemoveEntity<TKey, TValue>() { Key = last.Value, Value = dictionary[last.Value], RemoveType = RemoveType.TimeOut };
                                    removeEntities.Add(entity);
                                    dictionary.Remove(last.Value);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            last = linkedList.Last;
                        }
                    }
                    finally { locker.ExitWriteLock(); }
                    isCheckThread = false;
                    checkTime = DateTime.Now;
                });
            }
        }

        private void RemoveNotice()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {

                    RemoveEntity<TKey, TValue> item = null;
                    if (removeEntities.TryTake(out item, 500))
                    {
                        if (this.RemoveEntitiesEvent != null)
                        {
                            RemoveEntitiesEvent(CacheName, item);
                        }
                    }
                }
            });
        }

        public void Set(TKey key, TValue value)
        {
            locker.EnterWriteLock();
            try
            {
                dictionary[key] = value;
                LinkedListNode<TKey> item = null;
                if (dicLinkIndex.TryGetValue(key, out item))
                {
                    linkedList.Remove(item);
                }
                dicLinkIndex[key] = linkedList.AddFirst(key);

                if (linkedList.Count > _capacity)
                {
                    dictionary.Remove(linkedList.Last.Value);
                    dicLinkIndex.Remove(linkedList.Last.Value);
                    linkedList.RemoveLast();
                    dicRefresh.Remove(linkedList.Last.Value);
                    RemoveEntity<TKey, TValue> entity = new RemoveEntity<TKey, TValue>() { Key = linkedList.Last.Value, Value = dictionary[linkedList.Last.Value], RemoveType = RemoveType.Capacity };
                    removeEntities.Add(entity);
                    dictionary.Remove(linkedList.Last.Value);
                }
                Refresh(key);
            }
            finally { locker.ExitWriteLock(); }

        }

        public bool TryGet(TKey key, out TValue value)
        {
            locker.EnterUpgradeableReadLock();
            try
            {
                bool b = dictionary.TryGetValue(key, out value);
                if (b)
                {
                    locker.EnterWriteLock();
                    try
                    {
                        linkedList.Remove(key);
                        linkedList.AddFirst(key);
                    }
                    finally { locker.ExitWriteLock(); }

                }
                Refresh(key);
                return b;
            }
            catch { throw; }
            finally { locker.ExitUpgradeableReadLock(); }
        }
        public void Clear()
        {
            locker.EnterWriteLock();
            try
            {
                dictionary.Clear();
                linkedList.Clear();
                dicRefresh.Clear();
                dicLinkIndex.Clear();
                dicRefresh.Clear();
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
        public bool Remove(TKey key)
        {
            bool isSucess = false;
            locker.EnterWriteLock();
            try
            {
                isSucess = dictionary.Remove(key);
                dicRefresh.Remove(key);
                LinkedListNode<TKey> item = null;
                if (dicLinkIndex.TryGetValue(key, out item))
                {
                    linkedList.Remove(item);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
            return isSucess;
        }
        public bool ContainsKey(TKey key)
        {
            locker.EnterReadLock();
            try
            {
                return dictionary.ContainsKey(key);
            }
            finally { locker.ExitReadLock(); }
        }

        public int Count
        {
            get
            {
                locker.EnterReadLock();
                try
                {
                    return dictionary.Count;
                }
                finally { locker.ExitReadLock(); }
            }
        }

        public int Capacity
        {
            get
            {
                locker.EnterReadLock();
                try
                {
                    return _capacity;
                }
                finally { locker.ExitReadLock(); }
            }
            set
            {
                locker.EnterUpgradeableReadLock();
                try
                {
                    if (value > 0 && _capacity != value)
                    {
                        locker.EnterWriteLock();
                        try
                        {
                            _capacity = value;
                            while (linkedList.Count > _capacity)
                            {
                                linkedList.RemoveLast();
                            }
                        }
                        finally { locker.ExitWriteLock(); }
                    }
                }
                finally { locker.ExitUpgradeableReadLock(); }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                locker.EnterReadLock();
                try
                {
                    return dictionary.Keys;
                }
                finally { locker.ExitReadLock(); }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                locker.EnterReadLock();
                try
                {
                    return dictionary.Values;
                }
                finally { locker.ExitReadLock(); }
            }
        }
    }

    //最后在定一个异常的实体

    public class RemoveEntity<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public RemoveType RemoveType { get; set; }
    }
    public enum RemoveType
    {
        [Description("超时移除")]
        TimeOut,
        [Description("超量移除")]
        Capacity
    }
}
