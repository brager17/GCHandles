using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Xunit;
using Xunit.Abstractions;

namespace MemoryTests
{
    public class ICallForTest1Tests
    {
        private HandleCollector _handleCollector;

        public ICallForTest1Tests()
        {
            _handleCollector = new HandleCollector("call", 1, 10);
        }

        public void A() => _handleCollector.Add();
        public void B() => _handleCollector.Remove();
    }

    public class NotepadWriter
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private Process _process;

        private static HandleCollector _handleCollector
            = new HandleCollector("NotepadWriter", 1, 10);

        public NotepadWriter(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _handleCollector.Add();
            _process = Process.Start(@"C:\\Windows\\System32\\notepad.exe");
        }

        ~NotepadWriter()
        {
            _handleCollector.Remove();
            _testOutputHelper.WriteLine($"GC, collect");
            _process.Kill();
        }
    }

  


    public class UnitTest1
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest1(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test1()
        {
        }
    }
}