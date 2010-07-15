using System;
using System.Diagnostics;
using System.Threading;
using XenoGears.Assertions;
using XenoGears.Exceptions;
using XenoGears.Reflection.Attributes;

namespace XenoGears.Threading
{
    [DebuggerNonUserCode]
    public class WorkerThread
    {
        private WorkerThreadAttribute _attr;
        private String CustomName { get { return _attr.Name; } }
        private bool IsAffined { get { return _attr.IsAffined; } }

        protected virtual Func<T> Wrap<T>(Func<T> task) { return task; }
        protected virtual Action Wrap(Action task) { return task; }
        protected virtual void Initialize() { /* do nothing by default */ }
        protected virtual Exception Wrap(Exception exn)
        {
            var wrapped = exn as DeferredException;
            if (wrapped != null) return wrapped;
            return new DeferredException(exn);
        }

        private readonly Thread _thread;
        private int _nativeThreadId;
        private readonly Mutex _taskReservation = new Mutex(false);
        private readonly AutoResetEvent _taskArrived = new AutoResetEvent(false);
        private readonly AutoResetEvent _taskCompleted = new AutoResetEvent(false);
        private Func<Object> _task = null;
        private Object _ret = null;
        private Exception _exn = null;

        public WorkerThread()
        {
            _thread = new Thread(() =>
            {
                _nativeThreadId = NativeThread.Id;

                while (true)
                {
                    _taskArrived.WaitOne();
                    (_task != null).AssertTrue();
                    (_ret == null).AssertTrue();
                    (_exn == null).AssertTrue();

                    try
                    {
                        if (IsAffined)
                        {
                            using (NativeThread.Affinitize(_nativeThreadId))
                            {
                                EnsureInitialized();
                                _ret = _task();
                                _exn = null;
                            }
                        }
                        else
                        {
                            EnsureInitialized();
                            _ret = _task();
                            _exn = null;
                        }
                    }
                    catch (Exception exn)
                    {
                        _ret = null;
                        _exn = Wrap(exn);
                    }

                    _task = null;
                    _taskCompleted.Set();
                }
            });

            _attr = this.GetType().Attr<WorkerThreadAttribute>();
            _thread.Start();
        }

        private Object _initializationLock = new Object();
        private bool _hasBeenInitialized = false;
        private Exception _initializationException = null;
        private void EnsureInitialized()
        {
            if (!_hasBeenInitialized)
            {
                lock (_initializationLock)
                {
                    if (!_hasBeenInitialized)
                    {
                        try
                        {
                            Initialize();
                        }
                        catch(Exception exn)
                        {
                            _initializationException = exn;
                            throw new DeferredException(exn);
                        }
                        finally
                        {
                            _hasBeenInitialized = true;
                        }
                    }
                }
            }
            else
            {
                if (_initializationException != null)
                {
                    throw new DeferredException(_initializationException);
                }
                else
                {
                    return;
                }
            }
        }

        public void Invoke(Action task)
        {
            task.AssertNotNull();
            task = Wrap(task);

            if (Thread.CurrentThread == _thread)
            {
                task();
            }
            else
            {
                try
                {
                    _taskReservation.WaitOne();
                    (_task == null).AssertTrue();
                    var wrapped = Wrap(task);
                    _task = () => { wrapped(); return null; };
                    _taskArrived.Set();
                    _taskCompleted.WaitOne();

                    var ret = _ret.Fluent(_ => _ret = null);
                    var exn = _exn.Fluent(_ => _exn = null);
                    if (exn != null) throw exn;
                    (ret == null).AssertTrue();
                }
                finally
                {
                    _taskReservation.ReleaseMutex();
                }
            }
        }

        public T Invoke<T>(Func<T> task)
        {
            task.AssertNotNull();
            task = Wrap(task);

            if (Thread.CurrentThread == _thread)
            {
                return task();
            }
            else
            {
                try
                {
                    _taskReservation.WaitOne();
                    (_task == null).AssertTrue();
                    var wrapped = Wrap(task);
                    _task = () => wrapped();
                    _taskArrived.Set();
                    _taskCompleted.WaitOne();

                    var ret = _ret.Fluent(_ => _ret = null);
                    var exn = _exn.Fluent(_ => _exn = null);
                    if (exn != null) throw exn;
                    return (T)ret;
                }
                finally
                {
                    _taskReservation.ReleaseMutex();
                }
            }
        }
    }
}