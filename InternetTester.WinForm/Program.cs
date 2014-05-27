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

        [STAThread]
        static void Main()
        {
            const string googleHostName = "google.com";
            const int amountOfPingsPerCheck = 3;
            const int checkDelay = 500;

            var googleIps = Dns.GetHostAddresses(googleHostName);
            var pingInternetAccessChecker = new PingInternetAccessChecker(googleIps.First(), amountOfPingsPerCheck);
            _internetAccessAgent = new InternetAccessCheckerAgent(pingInternetAccessChecker, checkDelay);

            _internetAccessAgent.Start();

            _internetAccessAgent.Changed += internetAccessAgent_Changed;
            Application.Run();
        }

        static void internetAccessAgent_Changed(object sender, EventArgs e)
        {
            Console.WriteLine(_internetAccessAgent.IsInternetAvailable);
        }
    }
}
