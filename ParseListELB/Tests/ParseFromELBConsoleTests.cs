// -----------------------------------------------------------------------
// <copyright file="ParseFromELBConsoleTests.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ParseListELB
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using ParseListELB.DOM;

    /// <summary>
    /// Unit Test Fixture for the <see cref="ParseFromELBConsole"/> class.
    /// </summary>
    [TestFixture]
    public class ParseFromELBConsoleTests
    {
        [TestCaseSource(typeof(ParseELBPath_ValidInput_Tests))]
        public void ParseELBPath_ValidInput(string input, string expected)
        {
            string actual = ParseFromELBConsole.ParseELBPath(input);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(InternalParseMethodSubroutineFunction_ValidInput_Tests))]
        public void InternalParseMethodSubroutineFunction_ValidInput(string input, MethodSubroutineFunction expected)
        {
            MethodSubroutineFunction actual = ParseFromELBConsole.InternalParseMethodSubroutineFunction(input);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(ParseMethodSubroutineFunction_ValidInput_Tests))]
        public void ParseMethodSubroutineFunction_ValidInput(string input, Tuple<SectionType, MethodSubroutineFunction> expected)
        {
            Tuple<SectionType, MethodSubroutineFunction> actual = ParseFromELBConsole.ParseMethodSubroutineFunction(input);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(ParseExternalReference_ValidInput_Tests))]
        public void ParseExternalReference_ValidInput(string input, ExternalReference expected)
        {
            ExternalReference actual = ParseFromELBConsole.ParseExternalReference(input);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(ParseGlobalSymbol_ValidInput_Tests))]
        public void ParseGlobalSymbol_ValidInput(string input, GlobalSymbol expected)
        {
            GlobalSymbol actual = ParseFromELBConsole.ParseGlobalSymbol(input);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="ParseFromELBConsoleTests.ParseELBPath_ValidInput"/> test.
    /// </summary>
    internal class ParseELBPath_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(@"[C:\Users\aqa\Desktop\ELBs\oenew.elb]", @"C:\Users\aqa\Desktop\ELBs\oenew.elb");
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="ParseFromELBConsoleTests.InternalParseMethodSubroutineFunction_ValidInput"/> test.
    /// </summary>
    internal class InternalParseMethodSubroutineFunction_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(@"20COMPUTERSUNLIMITED12ORDERENTRY17OEDEVICEMANAGER11IPADDRESS4GET_A  size   304, pos 119375, ext refs  0, gnamsiz  0", this.Generate("20COMPUTERSUNLIMITED12ORDERENTRY17OEDEVICEMANAGER11IPADDRESS4GET_A", "304", "119375", "0", "0"));
            yield return new TestCaseData(@"CM_OEDISPATCHERREGION  size   608, pos 221665, ext refs 119, gnamsiz 40", this.Generate("CM_OEDISPATCHERREGION", "608", "221665", "119", "40"));
        }

        internal MethodSubroutineFunction Generate(string name, string size, string position, string externalReferenceCount, string globalNameSize)
        {
            return new MethodSubroutineFunction()
            {
                Name = name,
                Size = size,
                Position = position,
                ExternalReferenceCount = externalReferenceCount,
                GlobalNameSize = globalNameSize
            };
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="ParseFromELBConsoleTests.ParseMethodSubroutineFunction_ValidInput"/> test.
    /// </summary>
    internal class ParseMethodSubroutineFunction_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(@"20COMPUTERSUNLIMITED12ORDERENTRY17OEDEVICEMANAGER11IPADDRESS4GET_A  size   304, pos 119375, ext refs  0, gnamsiz  0", this.Generate(SectionType.Method, "20COMPUTERSUNLIMITED12ORDERENTRY17OEDEVICEMANAGER11IPADDRESS4GET_A", "304", "119375", "0", "0"));
            yield return new TestCaseData(@"CM_OEDISPATCHERREGION  size   608, pos 221665, ext refs 119, gnamsiz 40", this.Generate(SectionType.SubroutineFunction, "CM_OEDISPATCHERREGION", "608", "221665", "119", "40"));
        }

        internal Tuple<SectionType, MethodSubroutineFunction> Generate(SectionType sectionType, string name, string size, string position, string externalReferenceCount, string globalNameSize)
        {
            MethodSubroutineFunction msf =
                new MethodSubroutineFunction()
                {
                    Name = name,
                    Size = size,
                    Position = position,
                    ExternalReferenceCount = externalReferenceCount,
                    GlobalNameSize = globalNameSize
                };

            return new Tuple<SectionType, MethodSubroutineFunction>(sectionType, msf);
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="ParseFromELBConsoleTests.ParseExternalReference_ValidInput"/> test.
    /// </summary>
    internal class ParseExternalReference_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(@"        LDXREF 20COMPUTERSUNLIMITED12ORDERENTRY9OEGLOBAL17MEMBERWISECLONE_O", this.Generate(@"20COMPUTERSUNLIMITED12ORDERENTRY9OEGLOBAL17MEMBERWISECLONE_O"));
            yield return new TestCaseData(@"        LDXREF OE_ITEM     ", this.Generate(@"OE_ITEM"));
        }

        internal ExternalReference Generate(string name)
        {
            return new ExternalReference() { Name = name };
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="ParseFromELBConsoleTests.ParseGlobalSymbol_ValidInput"/> test.
    /// </summary>
    internal class ParseGlobalSymbol_ValidInput_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData("  $SR_20COMPUTERSUNLIMIT79FFDB7E   GLOBAL, \"<0><0><0><0><0><0><0><0><0><0><0><0><0><0><0><0>\"", this.Generate("$SR_20COMPUTERSUNLIMIT79FFDB7E", "GLOBAL", "\"<0><0><0><0><0><0><0><0><0><0><0><0><0><0><0><0>\""));
            yield return new TestCaseData("  $SR_APPLY_TERMS0A71B7F4   GLOBAL, \"<1><0><0><0>      <0>,\"", this.Generate("$SR_APPLY_TERMS0A71B7F4", "GLOBAL", "\"<1><0><0><0>      <0>,\""));
        }

        internal GlobalSymbol Generate(string name, string type, string initialValues)
        {
            return new GlobalSymbol() { Name = name, Type = type, InitialValues = initialValues };
        }
    }
}
