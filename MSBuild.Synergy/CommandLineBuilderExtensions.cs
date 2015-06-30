// -----------------------------------------------------------------------
// <copyright file="CommandLineBuilderExtensions.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MSBuild.Synergy
{
    using System;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Extension Method Class for <see cref="CommandLineBuilder"/>
    /// </summary>
    public static class CommandLineBuilderExtensions
    {
        /// <summary>
        /// Extension method to append a switch if the given condition is true.
        /// </summary>
        /// <param name="target">The <see cref="CommandLineBuilder"/> to modify.</param>
        /// <param name="appendIfTrue">Should the switch be appended?</param>
        /// <param name="switchName">The name of the switch to append to the command line.</param>
        public static void AppendSwitchIfTrue(this CommandLineBuilder target, bool appendIfTrue, string switchName)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (switchName == null)
            {
                throw new ArgumentNullException("switchName");
            }

            if (appendIfTrue)
            {
                target.AppendSwitch(switchName);
            }
        }
    }
}
