// -----------------------------------------------------------------------
// <copyright file="StringExtensionMethods.cs" company="Ace Olszowka">
// Copyright (c) Ace Olszowka 2015. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ParseListELB.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensionMethods
    {
        /// <summary>
        /// Returns a string with the specified characters removed.
        /// </summary>
        /// <param name="source">The string to filter.</param>
        /// <param name="removeCharacters">The characters to remove.</param>
        /// <returns>A new <see cref="System.String"/> with the specified characters removed.</returns>
        public static string Remove(this string source, IEnumerable<char> removeCharacters)
        {
            if (source == null)
            {
                throw new  ArgumentNullException("source");
            }

            if (removeCharacters == null)
            {
                throw new ArgumentNullException("removeCharacters");
            }

            // First see if we were given a collection that supports ISet
            ISet<char> replaceChars = removeCharacters as ISet<char>;

            if (replaceChars == null)
            {
                replaceChars = new HashSet<char>(removeCharacters);
            }

            IEnumerable<char> filtered = source.Where(currentChar => !replaceChars.Contains(currentChar));

            return new string(filtered.ToArray());
        }

        /// <summary>
        /// Returns a string with all non-numeric characters removed.
        /// </summary>
        /// <param name="source">The string to filter.</param>
        /// <returns>A new <see cref="System.String"/> with all non-numeric characters removed.</returns>
        public static string RemoveNonNumeric(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            IEnumerable<char> filtered = source.Where(currentChar => char.IsNumber(currentChar));
            return new string(filtered.ToArray());
        }
    }
}
