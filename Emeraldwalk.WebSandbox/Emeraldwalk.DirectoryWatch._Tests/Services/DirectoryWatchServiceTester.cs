using Emeraldwalk.DirectoryWatch.Services.Abstract;
using Emeraldwalk.DirectoryWatch.Services.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Emeraldwalk.DirectoryWatch._Tests.Services
{
    [TestClass]
    public class DirectoryWatchServiceTester
    {
        private DirectoryWatchService Instance { get; set; }

        [TestMethod]
        public void Constructor_Sets_Properties()
        {
            //{
            //    WatchDirectory = "watchDirectory",
            //    Filter = "*.mock",
            //    ProcessFileMode = ProcessFileMode.Changed,
            //    ExecutablePath = "executablePath",
            //    ExecutableArgs = new string[] { }
            //};

            DirectoryWatchConfig config = new DirectoryWatchConfig();
            Mock<ICommandArgsService> commandArgsService = new Mock<ICommandArgsService>();
            Mock<IDirectoryWatcher> directoryWatcher = new Mock<IDirectoryWatcher>();
                       
            this.Instance = new DirectoryWatchService(
                config,
                commandArgsService.Object,
                directoryWatcher.Object);

            Assert.AreEqual(config, this.Instance.Config);
        }
    }
}
