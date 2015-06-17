// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace FindDuplicateMethods
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <summary>
    /// Toy program to parse multiple "ListELB XML" documents to find duplicates.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            string targetDirectory = @"R:\ELBListing";
            IEnumerable<string> elbInformationFiles = Directory.EnumerateFiles(targetDirectory, "*.xml");

            IDictionary<string, string> distinctMSF = new Dictionary<string, string>();

            foreach (string elbInformationFile in elbInformationFiles)
            {
                var methodSubroutineFunctions = ParseELBForMethodSubroutinesFunctions(elbInformationFile);

                foreach (var methodSubroutineFunction in methodSubroutineFunctions)
                {
                    // Check for dupes
                    if (distinctMSF.ContainsKey(methodSubroutineFunction.Item2))
                    {
                        var duplicatedValue = distinctMSF[methodSubroutineFunction.Item2];
                        Console.WriteLine("DUPLICATE FOUND! {0} was Duplicated in {1} and {2}", methodSubroutineFunction.Item2, duplicatedValue, methodSubroutineFunction.Item1);
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
