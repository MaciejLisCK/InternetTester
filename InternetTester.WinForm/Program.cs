using InternetTester.BusinessLogic.DesktopDrawer;
using InternetTester.BusinessLogic.InternetAccessChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetTester.WinForm
{
    static class Program
    {
        static InternetAccessCheckerAgent _internetAccessAgent;
        static InternetInformationDesktopDrawerAgent _internetInformationDesktopDrawerAgent;

        [STAThread]
        static void Main()
        {
            const string googleHostName = "google.com";
            const int amountOfPingsPerCheck = 3;
            const int checkDelay = 500;

            var googleIps = Dns.GetHostAddresses(googleHostName);
            var pingInternetAccessChecker = new PingInternetAccessChecker(googleIps.First(), amountOfPingsPerCheck);
            _internetAccessAgent = new InternetAccessCheckerAgent(pingInternetAccessChecker, checkDelay);
            _internetAccessAgent.Changed += internetAccessAgent_Changed;
            _internetInformationDesktopDrawerAgent = new InternetInformationDesktopDrawerAgent();

            _internetAccessAgent.Start();
            _internetInformationDesktopDrawerAgent.Start();

            Application.Run();
        }

        static void internetAccessAgent_Changed(object sender, InternetAccessChangedEventArgs e)
        {
            _internetInformationDesktopDrawerAgent.SetInternetAvailability(e.IsInternetAvailable);
        }
    }
}
