using Moq;
using System;
using System.IO;
using Xunit;

namespace Onyx.Invoice.Core.Utils.Logger.Tests
{
    public class LoggerTests
    {
        private readonly DateTime _fixedDateTime;
        private readonly Mock<ITimeProvider> _mockTimeProvider;

        public LoggerTests()
        {
            _fixedDateTime = new DateTime(2023, 12, 3, 14, 30, 0);
            _mockTimeProvider = new Mock<ITimeProvider>();
            _mockTimeProvider.Setup(tp => tp.GetCurrentTime()).Returns(_fixedDateTime);
        }

        [Fact]
        public void Log_ShouldPrefixWithTimestamp()
        {
            // Arrange
            var logMessage = "Logger initialized";
            var expectedMessage = $"[03.12.23 14:30:00] {logMessage}";

            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                var logger = new Logger(streamWriter, _mockTimeProvider.Object);

                // Act
                logger.Log(logMessage);
                streamWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                var output = new StreamReader(memoryStream).ReadToEnd().Trim();

                // Assert
                Assert.Contains(expectedMessage, output);
            }
        }
    }
}
