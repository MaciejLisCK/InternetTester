﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace InternetTester.BusinessLogic.InternetAccessChecker
{
    public class PingInternetAccessChecker : IInternetAccessChecker
    {
        int _amountOfPingsToSend;
        IPAddress _ipAddressToTest;

        public PingInternetAccessChecker(IPAddress ipAddressToTest, int amountOfPingsToSend)
        {
            _ipAddressToTest = ipAddressToTest;
            _amountOfPingsToSend = amountOfPingsToSend;
        }

        public bool IsInternetAvailable()
        {
            var pingReplies = new List<PingReply>();
            var exceptions = new List<Exception>();
            for (int i = 0; i < _amountOfPingsToSend; i++)
            {
                try
                {
                    Ping pingService = new Ping();
                    var result = pingService.SendPingAsync(_ipAddressToTest);
                }
                catch(PingException e)
                {
                    exceptions.Add(e);
                }
            }

            var doesPingRepliesSuccess = pingReplies.All(pr => pr.Status == IPStatus.Success);
            var doesExceptionOccures = exceptions.Any();

            var isInternetAvailable = doesPingRepliesSuccess && !doesExceptionOccures;

            return isInternetAvailable;
        }
    }
}
