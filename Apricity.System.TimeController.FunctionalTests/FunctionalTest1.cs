using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apricity.OS.FunctionalTests
{
    /// <summary>
    /// Used to perform functional testing on the library.
    /// NOTE! This will affect your system time settings if executed!
    /// </summary>
    [TestClass]
    public class FunctionalTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tc = new TimeController();

            Debug.WriteLine("NOW: " + DateTime.Now);

            tc.SetTime(DateTime.Now.AddHours(1));

            Thread.Sleep(1000);

            Debug.WriteLine("ADDED 1 HOUR: " + DateTime.Now);

            Debug.WriteLine(DateTime.Now);

            tc.SetTime(DateTime.Now.AddHours(-1));

            Thread.Sleep(1000);

            Debug.WriteLine("SUBTRACTED 1 HOUR: " + DateTime.Now);

            //tc.ResyncTimeWithDomainTimeController();

            //Thread.Sleep(1000);

            //Debug.WriteLine("RESYNC: " + DateTime.Now);

            //Debug.WriteLine(DateTime.Now);
        }

        [TestMethod]
        public void TestResync()
        {
            var tc = new TimeController();
            tc.ResyncTimeWithDomainTimeController();
        }
    }
}
