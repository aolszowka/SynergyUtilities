// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ParseListELB
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using ParseListELB.DOM;

    class Program
    {
        static void Main(string[] args)
        {
            string targetDirectory = @"R:\ELBListing";
            IEnumerable<string> listElbOutputs = Directory.EnumerateFiles(targetDirectory, "*.txt");

            ////Parallel.ForEach(listElbOutputs, listElbOutput =>
            foreach (string listElbOutput in listElbOutputs)
            {
                string outputFile = Path.ChangeExtension(listElbOutput, "xml");
                var parsedELBDOM = ParseFromELBConsole.Parse(listElbOutput);

                XmlSerializer serializer = new XmlSerializer(typeof(ELB));
                using (TextWriter writer = new StreamWriter(outputFile))
                {
                    serializer.Serialize(writer, parsedELBDOM);
                }
            }
            ////);
        }
    }
}
