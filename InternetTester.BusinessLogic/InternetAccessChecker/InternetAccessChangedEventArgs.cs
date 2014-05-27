using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternetTester.BusinessLogic.InternetAccessChecker
{
    public class InternetAccessChangedEventArgs : EventArgs
    {
        public bool IsInternetAvailable { get; set; }
    }
}
