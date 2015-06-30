// -----------------------------------------------------------------------
// <copyright file="StringExtensionMethodsTests.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ParseListELB
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;
    using ParseListELB.Library;

    /// <summary>
    /// Unit tests for the <see cref="StringExtensionMethods"/> extension method class.
    /// </summary>
    [TestFixture]
    public class StringExtensionMethodsTests
    {
        /// <summary>
        /// Validate that giving valid input to the Remove function behaves as expected.
        /// </summary>
        /// <param name="targetString">The string to perform the remove on.</param>
        /// <param name="removeCharacters">The characters to remove.</param>
        /// <param name="expected">The expected string after removal.</param>
        [TestCaseSource(typeof(StringExtensionMethodsTests_Remove_Tests))]
        public void Remove(string targetString, IEnumerable<char> removeCharacters, string expected)
        {
            string actual = StringExtensionMethods.Remove(targetString, removeCharacters);

            Assert.That(actual, Is.EqualTo(expected));
        }

        /// <summary>
        /// Validate that arguments are validated for <c>null</c>; throwing <see cref="ArgumentNullException"/> when appropriate.
        /// </summary>
        /// <param name="targetString">The string to perform the remove on.</param>
        /// <param name="removeCharacters">The characters to remove.</param>
        [TestCaseSource(typeof(StringExtensionMethodsTests_Remove_ParameterValidation_Tests))]
        public void Remove_ParameterValidation(string targetString, IEnumerable<char> removeCharacters)
        {
            Assert.Throws<ArgumentNullException>(() => StringExtensionMethods.Remove(targetString, removeCharacters));
        }
    }

    /// <summary>
    /// Unit Tests for the <see cref="StringExtensionMethodsTests.Remove"/> test.
    /// </summary>
    internal class StringExtensionMethodsTests_Remove_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData("My name @is ,Wan.;'; Wan", new char[] { '@', ',', '.', ';', '\'' }, "My name is Wan Wan").SetName("StringUsingCharArray");
            yield return new TestCaseData("My name @is ,Wan.;'; Wan", new HashSet<char> { '@', ',', '.', ';', '\'' }, "My name is Wan Wan").SetName("StringUsingISetCollection");
            yield return new TestCaseData(string.Empty, new char[1], string.Empty).SetName("EmptyStringNoReplacementCharactersYieldsEmptyString");
            yield return new TestCaseData(string.Empty, new char[] { 'A', 'B', 'C' }, string.Empty).SetName("EmptyStringReplacementCharsYieldsEmptyString");
            yield return new TestCaseData("No replacement characters", new char[1], "No replacement characters").SetName("StringNoReplacementCharactersYieldsString");
            yield return new TestCaseData("No characters will be replaced", new char[] { 'Z' }, "No characters will be replaced").SetName("StringNonExistantReplacementCharactersYieldsString");
            yield return new TestCaseData("AaBbCc", new char[] { 'a', 'C' }, "ABbc").SetName("CaseSensitivityReplacements");
            yield return new TestCaseData("ABC", new char[] { 'A', 'B', 'C' }, string.Empty).SetName("AllCharactersRemoved");
            yield return new TestCaseData("AABBBBBBCC", new char[] { 'A', 'B', 'C' }, string.Empty).SetName("AllCharactersRemovedMultiple");
            yield return new TestCaseData("Test That They Didn't Attempt To Use .Except() which returns distinct characters", new char[] { '(', ')' }, "Test That They Didn't Attempt To Use .Except which returns distinct characters").SetName("ValidateTheStringIsNotJustDistinctCharacters");
        }
    }

    /// <summary>
    /// Unit tests for the <see cref="StringExtensionMethodsTests.Remove_ParameterValidation"/> test.
    /// </summary>
    internal class StringExtensionMethodsTests_Remove_ParameterValidation_Tests : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestCaseData(null, null);
            yield return new TestCaseData("valid string", null);
            yield return new TestCaseData(null, new char[1]);
        }
    }

}
