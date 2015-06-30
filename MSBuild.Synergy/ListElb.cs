// -----------------------------------------------------------------------
// <copyright file="ListElb.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MSBuild.Synergy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// MSBuild wrapper around the ListElb Utility.
    /// </summary>
    public class ListElb : ToolTask
    {
        private StreamWriter outputStreamWriter;

        /// <summary>
        ///     Gets or sets a value indicating whether or not to display
        /// the list of ELB names only.
        /// </summary>
        public bool ListELBNamesOnly
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to include
        /// information for each of the ELBs linked to the specified ELB.
        /// </summary>
        public bool IncludeLinkedELBs
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to give verbose
        /// output or not.
        /// </summary>
        public bool Verbose
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to print
        /// the version of the linker used to create the ELB.
        /// </summary>
        public bool ListVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ELB that should be inspected.
        /// </summary>
        [Required]
        public string ELB
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the Output File.
        /// </summary>
        [Required]
        public string OutputFile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the ListElb tool.
        /// </summary>
        protected override string ToolName
        {
            get
            {
                return "listelb.exe";
            }
        }

        /// <summary>
        /// Generate the Full Path to the ListElb program.
        /// </summary>
        /// <returns>The full path to the ListElb program</returns>
        protected override string GenerateFullPathToTool()
        {
            // TODO: We should probably add in some default probing logic
            //       to search commonly used Synergy Installation Directories
            // TODO: If you implement the above we should probably have some
            //       way to specify x64 vs x86. For now we're deferring to
            //       the end user to make this call

            if (string.IsNullOrEmpty(this.ToolPath))
            {
                return this.ToolName;
            }

            return System.IO.Path.Combine(this.ToolPath, this.ToolName);
        }

        /// <summary>
        /// Logs events from the output of ListElb.
        /// </summary>
        /// <param name="singleLine">A single line of output from ListElb.</param>
        /// <param name="messageImportance">The importance assigned to this message.</param>
        protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
        {
            this.outputStreamWriter.WriteLine(singleLine);
            base.LogEventsFromTextOutput(singleLine, messageImportance);
        }

        /// <summary>
        /// Generates the command line to send to ListElb.
        /// </summary>
        /// <returns>The command line to send to list elb</returns>
        protected override string GenerateCommandLineCommands()
        {
            CommandLineBuilder clb = new CommandLineBuilder();

            clb.AppendSwitchIfTrue(this.ListELBNamesOnly, "-e ");
            clb.AppendSwitchIfTrue(this.IncludeLinkedELBs, "-l ");
            clb.AppendSwitchIfTrue(this.Verbose, "-v ");
            clb.AppendSwitchIfTrue(this.ListVersion, "-i ");
            clb.AppendFileNameIfNotNull(this.ELB);

            return clb.ToString();
        }

        public override bool Execute()
        {
            using (this.outputStreamWriter = new StreamWriter(this.OutputFile))
            {
                bool success = base.Execute();
                return success;
            }
        }
    }
}
