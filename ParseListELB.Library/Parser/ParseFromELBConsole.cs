// -----------------------------------------------------------------------
// <copyright file="ParseFromELBConsole.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ParseListELB.Library
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ParseListELB.Library.DOM;

    // This whole thing is a giant hack, we should really get proper
    // support to extract the contents of this file
    public static class ParseFromELBConsole
    {
        public static ELB Parse(string filePath)
        {
            ELB result = new ELB();

            using (TextReader tr = new StreamReader(filePath))
            {
                // The First Line contains the full path to the ELB
                result.Path = ParseELBPath(tr.ReadLine());

                // We can derive the name from the path
                result.Name = Path.GetFileName(result.Path);

                // The Second line will contain the structure version of the ELB
                result.StructureVersion = ParseELBStructureVersion(tr.ReadLine());

                // We're now in the "Methods and Structure/Functions" section,
                // we're in this section until we're no longer indented by '  '.
                string currentLine = tr.ReadLine();
                Tuple<SectionType, MethodSubroutineFunction> currentMethodSubroutineFunction = null;
                while (currentLine != null && currentLine.StartsWith("  "))
                {
                    // We need to determine if we're being given a method/
                    // subroutine/function or if we're in a section that
                    // indicates the external routine being called, we do
                    // this by assuming that if we're indented 4 times we're
                    // in that section, otherwise we're being given a method
                    // name.
                    if (currentLine.StartsWith("      "))
                    {
                        ExternalReference extRef = ParseExternalReference(currentLine);
                        currentMethodSubroutineFunction.Item2.AddExternalReference(extRef);
                    }
                    else
                    {
                        // We assume that we're being given a method name
                        // If we previously had a method that means that we
                        // need to add the old method and change current
                        if (currentMethodSubroutineFunction != null)
                        {
                            AddMethodSubroutineFunctionToResult(result, currentMethodSubroutineFunction);
                        }

                        currentMethodSubroutineFunction = ParseMethodSubroutineFunction(currentLine);
                    }

                    currentLine = tr.ReadLine();
                }

                // Make sure to get the last method added to the ELB Listing!
                if (currentMethodSubroutineFunction != null)
                {
                    AddMethodSubroutineFunctionToResult(result, currentMethodSubroutineFunction);
                }

                // Next up it could be the global symbols section
                if (currentLine.EndsWith("global symbol definitions"))
                {
                    currentLine = tr.ReadLine();

                    while (currentLine != null && currentLine.StartsWith("  "))
                    {
                        GlobalSymbol gs = ParseGlobalSymbol(currentLine);
                        result.AddGlobalSymbol(gs);
                        currentLine = tr.ReadLine();
                    }
                }

                // Finally it could be the linked ELB section
                if (currentLine.EndsWith("linked ELBs"))
                {
                    currentLine = tr.ReadLine();

                    while (currentLine != null && currentLine.StartsWith("  "))
                    {
                        LinkedELB linkedElb = ParseLinkedELB(currentLine);
                        result.AddLinkedELB(linkedElb);
                        currentLine = tr.ReadLine();
                    }
                }
            }

            return result;
        }

        internal static void AddMethodSubroutineFunctionToResult(ELB result, Tuple<SectionType, MethodSubroutineFunction> currentMethodSubroutineFunction)
        {
            switch (currentMethodSubroutineFunction.Item1)
            {
                case SectionType.Method:
                    {
                        result.AddMethod(currentMethodSubroutineFunction.Item2);
                        break;
                    }
                case SectionType.SubroutineFunction:
                    {
                        result.AddSubroutineFunction(currentMethodSubroutineFunction.Item2);
                        break;
                    }
                default:
                    {
                        throw new NotSupportedException("Unexpected SectionType Encountered");
                    }
            }
        }

        internal static Tuple<SectionType, MethodSubroutineFunction> ParseMethodSubroutineFunction(string input)
        {
            SectionType sectionType = SectionType.SubroutineFunction;

            // We need to determine if this entry is a method
            // or if its a subroutine/function, according to
            // support if it starts with a number it is a method,
            // otherwise its a Subroutine/Function
            string trimmed = input.Trim();

            if (char.IsNumber(trimmed.First()))
            {
                sectionType = SectionType.Method;
            }

            // Now Parse the Method
            MethodSubroutineFunction msf = InternalParseMethodSubroutineFunction(trimmed);

            return new Tuple<SectionType, MethodSubroutineFunction>(sectionType, msf);
        }

        internal static MethodSubroutineFunction InternalParseMethodSubroutineFunction(string input)
        {
            MethodSubroutineFunction msf = new MethodSubroutineFunction();

            // The first part of the string will be the name of the function until the first break
            int functionNameEnd = input.IndexOf(' ');
            msf.Name = input.Substring(0, functionNameEnd);

            // The remainder of the information is stored in the string, first just trim to the remaining info
            string remainingInfo = input.Substring(functionNameEnd);

            // Split this on comma
            string[] allRemainingInfo = remainingInfo.Split(',');

            // Sanity check, if we don't have 4 elements something has probably changed in the format of the output of listelb
            if (allRemainingInfo.Length != 4)
            {
                throw new InvalidOperationException("Unexpected remaining info length; did the format of listelb change?");
            }

            // MAGIC NUMBERS
            msf.Size = allRemainingInfo[0].RemoveNonNumeric();
            msf.Position = allRemainingInfo[1].RemoveNonNumeric();
            msf.ExternalReferenceCount = allRemainingInfo[2].RemoveNonNumeric();
            msf.GlobalNameSize = allRemainingInfo[3].RemoveNonNumeric();

            return msf;
        }

        internal static ExternalReference ParseExternalReference(string input)
        {
            // As it stands right now it appears that all we
            // get is the name of the method strip it out!
            string externalReference = input.Replace("LDXREF", string.Empty);

            // Trim extraneous whitespace
            string trimmed = externalReference.Trim();

            return new ExternalReference() { Name = trimmed };
        }

        internal static string ParseELBStructureVersion(string input)
        {
            // TODO: make this actually parse the result
            return "8.3";
        }

        internal static string ParseELBPath(string elbNameString)
        {
            ISet<char> replaceChars = new HashSet<char> { '[', ']' };
            return elbNameString.Remove(replaceChars);
        }

        internal static GlobalSymbol ParseGlobalSymbol(string input)
        {
            string sanitizedInput = input.Trim();
            GlobalSymbol gs = new GlobalSymbol();

            // The first part of the string will be the name of the symbol until the first break
            int globalNameEnd = sanitizedInput.IndexOf(' ');
            gs.Name = sanitizedInput.Substring(0, globalNameEnd);

            // The remainder of the information is stored in the string, first just trim to the remaining info
            string remainingInfo = sanitizedInput.Substring(globalNameEnd);

            // Split the elements on comma
            string[] allRemainingInfo = remainingInfo.Split(new char[] { ',' }, 2);

            // MAGIC NUMBERS
            gs.Type = allRemainingInfo[0].Trim();
            gs.InitialValues = allRemainingInfo[1].Trim();

            return gs;
        }

        internal static LinkedELB ParseLinkedELB(string input)
        {
            return new LinkedELB() { Name = input.Trim() };
        }
    }
}
