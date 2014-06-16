// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Apricity Software LLC">
//   Copyright © Apricity Software LLC
//   All Rights Reserved
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace TimeController
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                // No args found - jump out
                return;
            }

            if (options.DateTimeToSet.HasValue)
            {
                SetDateTime(options.DateTimeToSet.Value);
            }

            if (options.GetDateTime)
            {
                Console.WriteLine(GetDateTime());
            }
        }

        static void SetDateTime(DateTime time)
        {
            var systemTime = ConvertDateTimeToSystemTime(time);

            try
            {
                SetSystemTime(ref systemTime);
            }
            catch (Exception ex)
            {
                // todo: log
                throw;
            }
        }

        static DateTime GetDateTime()
        {
            var systemTime = new SYSTEMTIME();

            GetSystemTime(ref systemTime);

            var dateTime = ConvertSystemTimeToDateTime(systemTime);

            return dateTime;
        }

        static SYSTEMTIME ConvertDateTimeToSystemTime(DateTime dateTime)
        {
            var systemTime = new SYSTEMTIME
            {
                wYear = (ushort) dateTime.Year,
                wMonth = (ushort) dateTime.Month,
                wDayOfWeek = (ushort) dateTime.DayOfWeek,
                wDay = (ushort) dateTime.Day,
                wHour = (ushort) dateTime.Hour,
                wMinute = (ushort) dateTime.Minute,
                wSecond = (ushort) dateTime.Second,
                wMilliseconds = (ushort) dateTime.Second
            };

            return systemTime;
        }

        static DateTime ConvertSystemTimeToDateTime(SYSTEMTIME systemTime)
        {
            var dateTime = new DateTime(systemTime.wYear, systemTime.wMonth, systemTime.wDay, systemTime.wHour,
                systemTime.wMinute, systemTime.wSecond, systemTime.wMilliseconds);

            return dateTime;
        }

        // System calls and struct provided by: http://msdn.microsoft.com/en-us/library/ms172517(v=vs.90).aspx

        [DllImport("kernel32.dll")]
        private extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);

        [DllImport("kernel32.dll")]
        private extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }
    }
}
