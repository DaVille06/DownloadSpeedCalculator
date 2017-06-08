using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace DownloadSpeedCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.Configuration.Variables["currentDate"] = DateTime.Now.Date.ToString(@"MMddyyyy");

            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.UseNLog();

                serviceConfig.Service<SpeedService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(
                        () => new SpeedService());

                    serviceInstance.WhenStarted(
                        execute => execute.Start());

                    serviceInstance.WhenStopped(
                        execute => execute.Stop());

                    serviceInstance.WhenPaused(
                        execute => execute.Pause());

                    serviceInstance.WhenContinued(
                        execute => execute.Continue());
                });

                serviceConfig.EnableServiceRecovery(recoveryOption =>
                {
                    // wait 5 minutes and restart the service
                    recoveryOption.RestartService(5);
                });

                serviceConfig.EnablePauseAndContinue();

                serviceConfig.SetServiceName("DownloadSpeedCalculator");
                serviceConfig.SetDisplayName("Download Speed Calculator");
                serviceConfig.SetDescription(string.Format("Calculates the networks download speed every {0} minutes.", ConfigurationManager.AppSettings["serviceMinutes"].ToString()));

                serviceConfig.StartAutomatically();
            });
        }
    }
}
