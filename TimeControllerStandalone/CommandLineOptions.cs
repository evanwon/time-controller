// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandLineOptions.cs" company="Apricity Software LLC">
//   Copyright © Apricity Software LLC
//   All Rights Reserved
// </copyright>
// <summary>
//   Defines the CommandLineOptions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using CommandLine;
using CommandLine.Text;

namespace TimeController
{
    class CommandLineOptions
    {
        [Option('s', "set",
          HelpText = "Sets the system time to this value. This value should be provided in UTC. Requires Administrator privileges.")]
        public DateTime? DateTimeToSet { get; set; }

        [Option('g', "get",
            HelpText = "Gets the current system time in UTC.")]
        public bool GetDateTime { get; set; }

        [Option('v', "verbose", DefaultValue = true,
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
