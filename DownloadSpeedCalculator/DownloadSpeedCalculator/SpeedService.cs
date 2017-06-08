using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf.Logging;

namespace DownloadSpeedCalculator
{
    public class SpeedService
    {
        private Timer _timer;
        private static readonly LogWriter _log = HostLogger.Get<SpeedService>();

        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;
        }

        public bool Pause()
        {
            return true;
        }

        public bool Continue()
        {
            return true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {

        }
    }
}
