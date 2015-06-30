// -----------------------------------------------------------------------
// <copyright file="ConvertListElbOutputToXML.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MSBuild.Synergy
{
    using System.IO;
    using System.Xml.Serialization;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using ParseListELB.Library;
    using ParseListELB.Library.DOM;

    /// <summary>
    /// Task to convert the text output of ListELB to an XML Version.
    /// </summary>
    public class ConvertListElbOutputToXML : Task
    {
        /// <summary>
        /// Gets or sets the ListELB output file.
        /// </summary>
        [Required]
        public string ListElbOutputFile
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Output XML File.
        /// </summary>
        [Required]
        public string OutputFile
        {
            get;
            set;
        }

        /// <summary>
        ///     Attempts to convert the file specified by <paramref name="ListElbOutputFile"/>
        /// into an XML file specified by <paramref name="OutputFile"/>.
        /// </summary>
        /// <returns><c>true</c> always.</returns>
        public override bool Execute()
        {
            ELB parsedELBDOM = ParseFromELBConsole.Parse(this.ListElbOutputFile);

            XmlSerializer serializer = new XmlSerializer(typeof(ELB));
            using (TextWriter writer = new StreamWriter(this.OutputFile))
            {
                serializer.Serialize(writer, parsedELBDOM);
            }

            return true;
        }
    }
}
