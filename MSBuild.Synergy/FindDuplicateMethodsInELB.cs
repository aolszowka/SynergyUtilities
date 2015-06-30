// -----------------------------------------------------------------------
// <copyright file="FindDuplicateMethodsInELB.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MSBuild.Synergy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    ///     Task for scanning the given ListELB XML Descriptions for duplicate
    /// methods, subroutines, or functions.
    /// </summary>
    public class FindDuplicateMethodsInELB : Task
    {
        /// <summary>
        /// Gets or sets the set of ListElb XML Descriptions to scan for duplicates.
        /// </summary>
        [Required]
        public string[] ListElbXmlDescriptions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to error on duplicates.
        /// </summary>
        public bool ErrorOnDuplicates
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of Duplicate methods, subroutines, and functions.
        /// </summary>
        [Output]
        public string[] Duplicates
        {
            get;
            set;
        }

        /// <summary>
        /// Scans the given ListELB XML Description files for duplicates.
        /// </summary>
        /// <returns><c>true</c> if a scan was performed. <c>false</c> if duplicates were found AND ErrorOnDuplicates was <c>true</c>.</returns>
        public override bool Execute()
        {
            bool success = true;
            this.Duplicates = _FindDuplicateMethodsInElb(this.ListElbXmlDescriptions).ToArray();

            if (this.ErrorOnDuplicates && this.Duplicates.Any())
            {
                foreach (string duplicate in this.Duplicates)
                {
                    Log.LogError(duplicate);
                }

                success = false;
            }

            return success;
        }

        /// <summary>
        /// Scans the given ListELB XML Description files for duplicate method, subroutines, functions.
        /// </summary>
        /// <param name="listElbXmlDescriptions">An Enumerable of file paths to ListELB XML Description files.</param>
        /// <returns>An Enumerable of messages indicating if there are any duplicates.</returns>
        internal static IEnumerable<string> _FindDuplicateMethodsInElb(IEnumerable<string> listElbXmlDescriptions)
        {
            IDictionary<string, string> distinctMSF = new Dictionary<string, string>();

            foreach (string listElbXmlDescription in listElbXmlDescriptions)
            {
                var methodSubroutineFunctions = ParseELBForMethodSubroutinesFunctions(listElbXmlDescription);

                foreach (var methodSubroutineFunction in methodSubroutineFunctions)
                {
                    // Check for dupes
                    if (distinctMSF.ContainsKey(methodSubroutineFunction.Item2))
                    {
                        var duplicatedValue = distinctMSF[methodSubroutineFunction.Item2];
                        var duplicateMessage =
                            string.Format(
                            "{0} was Duplicated in {1} and {2}",
                            methodSubroutineFunction.Item2,
                            duplicatedValue,
                            methodSubroutineFunction.Item1);

                        yield return duplicateMessage;
                    }
                    else
                    {
                        // No Dupe yet, add to our list of known methods/sub/functions
                        distinctMSF.Add(methodSubroutineFunction.Item2, methodSubroutineFunction.Item1);
                    }
                }
            }
        }

        /// <summary>
        ///     Parses a "ListELB" XML File and extracts the Method/
        /// Subroutines/Functions from the XML File returning a Tuple where
        /// the first item is the name of the ELB and the second item is the
        /// Method/Subroutine/Function.
        /// </summary>
        /// <param name="xmlFilePath">The "ListELB" xml file to load.</param>
        /// <returns>An Enumerable of Tuple that contains the ELB name and Function.</returns>
        internal static IEnumerable<Tuple<string, string>> ParseELBForMethodSubroutinesFunctions(string xmlFilePath)
        {
            // Load up the xml file into an XDocument
            XDocument elbDescription = XDocument.Load(xmlFilePath);

            // Grab the name of this ELB
            string elbName = elbDescription.XPathSelectElement("/ELB").Attribute("Name").Value;

            // Grab all MethodSubroutinesFunctions
            var methodSubroutineFunctionElements = elbDescription.XPathSelectElements("//MethodSubroutineFunction");

            foreach (var methodSubroutineFunctionElement in methodSubroutineFunctionElements)
            {
                yield return new Tuple<string, string>(elbName, methodSubroutineFunctionElement.Attribute("Name").Value);
            }
        }
    }
}
