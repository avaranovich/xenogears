using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using XenoGears.Assertions;
using XenoGears.Traits.Disposable;

namespace XenoGears.Unsafe
{
    [Finalizable]
    [DebuggerNonUserCode]
    public unsafe class PointerToPinnedObject<T> : Disposable
        where T : class
    {
        private readonly GCHandle _handle;
        public T Target { get { IsDisposed.AssertFalse(); return (T)_handle.Target; } }

        public PointerToPinnedObject(T obj)
        {
            obj.AssertNotNull();
            // note. please, do not try to make this work with structures
            (obj.GetType().IsClass || obj.GetType().IsArray).AssertTrue();
            _handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
        }

        protected override void DisposeUnmanagedResources()
        {
            _handle.Free();
        }

        public static implicit operator void*(PointerToPinnedObject<T> ppo)
        {
            return (void*)(IntPtr)ppo;
        }

        public static implicit operator IntPtr(PointerToPinnedObject<T> ppo)
        {
            // note. do NOT use GCHandle.ToIntPtr(_handle) here
            // it won't return the address of an object but rather object's handle in GC's internal table
            // (i.e. you won't be able to cast it to void* and do something meaningful with the pointer)
            return ppo._handle.AddrOfPinnedObject();
        }
    }
}