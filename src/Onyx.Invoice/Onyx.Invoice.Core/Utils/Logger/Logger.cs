using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Invoice.Core.Utils.Logger
{

    public class Logger
    {
        private readonly StreamWriter _writer;
        private readonly ITimeProvider _timeProvider;

        public Logger(StreamWriter writer, ITimeProvider timeProvider)
        {
            _writer = writer;
            _timeProvider = timeProvider;

            Log("Logger initialized");
        }

        public void Log(string str)
        {
            var timestamp = _timeProvider.GetCurrentTime();
            _writer.WriteLine($"[{timestamp:dd.MM.yy HH:mm:ss}] {str}");
        }
    }
}
