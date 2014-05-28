using InternetTester.BusinessLogic.DesktopDrawer;
using InternetTester.BusinessLogic.InternetAccessChecker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace InternetTester.WindowsService
{
    public partial class InternetTesterService : ServiceBase
    {
        InternetAccessCheckerAgent _internetAccessAgent;
        InternetInformationDesktopDrawerAgent _internetInformationDesktopDrawerAgent;


        public InternetTesterService()
        {
            InitializeComponent();
            const string googleHostName = "google.com";
            const int amountOfPingsPerCheck = 3;
            const int checkDelay = 500;

            var googleIps = Dns.GetHostAddresses(googleHostName);
            var pingInternetAccessChecker = new PingInternetAccessChecker(googleIps.First(), amountOfPingsPerCheck);
            _internetAccessAgent = new InternetAccessCheckerAgent(pingInternetAccessChecker, checkDelay);
            _internetAccessAgent.Changed += internetAccessAgent_Changed;
            _internetInformationDesktopDrawerAgent = new InternetInformationDesktopDrawerAgent();
        }

        protected override void OnStart(string[] args)
        {
            _internetAccessAgent.Start();
            _internetInformationDesktopDrawerAgent.Start();
        }

        protected override void OnStop()
        {
            _internetAccessAgent.Stop();
            _internetInformationDesktopDrawerAgent.Stop();
        }

        void internetAccessAgent_Changed(object sender, InternetAccessChangedEventArgs e)
        {
            _internetInformationDesktopDrawerAgent.SetInternetAvailability(e.IsInternetAvailable);
        }
    }
}
