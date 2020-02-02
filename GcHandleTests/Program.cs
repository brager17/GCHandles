using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace GcHandleTests
{
    class Program
    {
        private static object[] _ints;
        private static object _o;

        static void Main(string[] args)
        {
            _o = new Object().GCWatch("My Object created at " + DateTime.Now);
            GC.Collect(0);
            GC.KeepAlive(_o);
            _o = null;
            GC.Collect(0);
            Console.ReadLine();
        }
    }

    internal static class GCWatcher
    {
        private static readonly ConditionalWeakTable<Object,
            NotifyWhenGCd<String>> s_cwt =
            new ConditionalWeakTable<Object, NotifyWhenGCd<String>>();

        private sealed class NotifyWhenGCd<T>
        {
            private readonly T m_value;

            internal NotifyWhenGCd(T value)
            {
                m_value = value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }

            ~NotifyWhenGCd()
            {
                Console.WriteLine("GC'd: " + m_value);
            }
        }

        public static T GCWatch<T>(this T @object, String tag) where T : class
        {
            s_cwt.Add(@object, new NotifyWhenGCd<String>(tag));
            return @object;
        }
    }

    public delegate bool CallBack(int handle, IntPtr param);

    internal static class NativeMethods
    {
        [DllImport("Dll2.dll")]
        public static extern ulong fibonacci_current();

        [DllImport("Dll2.dll")]
        public static extern void fibonacci_init(ulong a, ulong b);

        [DllImport("Dll2.dll")]
        public static extern bool fibonacci_next();

        [DllImport("Dll2.dll")]
        public static extern unsafe int* initialArray(int* arr, int length);
    }
}