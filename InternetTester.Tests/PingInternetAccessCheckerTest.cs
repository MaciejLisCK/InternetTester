using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InternetTester.BusinessLogic.InternetAccessChecker;
using System.Net;

namespace InternetTester.Tests
{
    [TestClass]
    public class PingInternetAccessCheckerTest
    {
        [TestMethod]
        public void ShouldReturnTrueForValidIp()
        {
            var ipAddress = Dns.GetHostAddresses("google.com")[0];
            var amountOfPingsToSend = 3;
            var checker = new PingInternetAccessChecker(ipAddress, amountOfPingsToSend);

            var result = checker.IsInternetAvailable();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ShouldReturnFalseForNotValidIp()
        {
            var ipAddress = IPAddress.Parse("12.12.12.12");
            var amountOfPingsToSend = 3;
            var checker = new PingInternetAccessChecker(ipAddress, amountOfPingsToSend);

            var result = checker.IsInternetAvailable();

            Assert.IsFalse(result);
        }
    }
}
