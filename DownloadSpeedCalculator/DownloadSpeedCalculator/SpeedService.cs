using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
            int serviceMinutes;
            int.TryParse(ConfigurationManager.AppSettings["serviceMinutes"], out serviceMinutes);


            _timer = new Timer((60 * serviceMinutes) * 1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            return true;
        }

        public bool Stop()
        {
            _timer.Dispose();

            return true;
        }

        public bool Pause()
        {
            _timer.Enabled = false;

            return true;
        }

        public bool Continue()
        {
            _timer.Enabled = true;

            return true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            var newFileName = DateTime.Now.Date.ToString(@"MMddyyyy");
            LogManager.Configuration.Variables["currentDate"] = newFileName;

            _log.InfoFormat("*****Calculation STARTED*****");
            _log.InfoFormat("Downloading 100MB file from: {0}", @"http://ipv4.download.thinkbroadband.com/100MB.zip");

            Stopwatch stopWatch = new Stopwatch();
            WebClient webClient = new WebClient();
            Uri URL = new Uri(@"http://ipv4.download.thinkbroadband.com/100MB.zip");

            stopWatch.Start();
            webClient.DownloadFile(URL, @"C:\temp\downloadSpeedTest.zip");
            stopWatch.Stop();

            double seconds = Math.Floor((double)(stopWatch.ElapsedMilliseconds / 1000));
            double kbSec = Math.Round((1024 * 1024) / seconds);
            double mbSec = Math.Round(kbSec / 1000);
            _log.InfoFormat("The download took {0} seconds", seconds);
            _log.InfoFormat("Download speed is {0} mb/sec", mbSec);

            File.Delete(@"C:\temp\downloadSpeedTest.zip");

            _log.InfoFormat("*****Calculation ENDED*****");
        }
    }
}
