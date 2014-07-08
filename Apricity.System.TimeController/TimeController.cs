// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeController.cs" company="Apricity Software LLC">
//   Copyright © Apricity Software LLC
//   All Rights Reserved
// </copyright>
// <summary>
//   Defines the TimeController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DependencySharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Apricity.OS
{

    /// <summary>
    /// Provides methods to set the system date and time.
    /// </summary>
    public class TimeController
    {
        private const int DefaultTimeout = 10;

        private TimeSpan? _timeout;

        /// <summary>
        /// The amount of time to wait before stopping the procedure.
        /// Default: 10 seconds
        /// </summary>
        public TimeSpan Timeout
        {
            get
            {
                if (!_timeout.HasValue)
                {
                    _timeout = TimeSpan.FromSeconds(DefaultTimeout);
                }

                return _timeout.Value;
            }
            set
            {
                _timeout = value;
            }
        }

        /// <summary>
        /// Sets the system's time to the provided time.
        /// </summary>
        /// <param name="dateTime">The system time will be set to this value.</param>
        public void SetTime(DateTime dateTime)
        {
            SetSystemTime(dateTime);
        }

        private void SetSystemTime(DateTime dateTime)
        {
            CheckDependenciesAndCreateIfMissing();

            var startInfo = new ProcessStartInfo
            {
                FileName = "TimeController.exe", 
                Arguments = string.Format("--set \"{0}\"", dateTime.ToUniversalTime()),
                // todo: subscribe to stdout and sterr
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas"                                  // Run the process as Administrator
            };
            
            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            process.WaitForExit(Timeout.Milliseconds);
        }

        /// <summary>
        /// Attempts to re-synchronize the local system time with the network time controller in an Active Directory 
        /// domain.
        /// </summary>
        public void ResyncTimeWithDomainTimeController()
        {
            var startInfo = new ProcessStartInfo
                                {
                                    FileName = "w32tm.exe",
                                    Arguments = "/resync",
                                    // todo: subscribe to stdout and sterr
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    UseShellExecute = false,
                                    CreateNoWindow = true,
                                    Verb = "runas"              // Run the process as Administrator
                                };

            var process = new Process
                              {
                                  StartInfo = startInfo
                              };

            process.Start();

            process.ErrorDataReceived += (sender, args) =>
                {
                    throw new TimeControllerException("Unable to resync time with network controller.");                    
                };

            process.WaitForExit(Timeout.Milliseconds);
        }

        private static void CheckDependenciesAndCreateIfMissing()
        {
            var expectedPath = AssemblyUtilities.ExecutingAssemblyPath;

            var dependencyList = new List<UnmanagedDependency>()
                                     {
                                         new UnmanagedDependency(
                                             Path.Combine(expectedPath, "CommandLine.dll"),
                                             Properties.Resources.CommandLine_dll),
                                         new UnmanagedDependency(
                                             Path.Combine(
                                                 expectedPath,
                                                 "TimeController.exe"),
                                             Properties.Resources.TimeController_exe,
                                             new Version(1, 0, 0, 0)),
                                     };

            var dependencyManager = new DependencyManager();
            dependencyManager.VerifyDependenciesAndExtractIfMissing(dependencyList);
        }
    }
}
