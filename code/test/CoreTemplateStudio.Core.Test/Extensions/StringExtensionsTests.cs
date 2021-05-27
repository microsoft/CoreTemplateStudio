// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Xunit;

namespace Microsoft.Templates.Core.Test.Extensions
{
    [Trait("ExecutionSet", "Minimum")]
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetMultiValue_EmptyOrNull_ShouldReturnEmptyArray(string value)
        {
            var expected = Array.Empty<string>();

            var actual = value.GetMultiValue();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("One|", new string[] { "One" })]
        [InlineData("One|Two", new string[] { "One", "Two" })]
        [InlineData("One|Two||Three", new string[] { "One", "Two", "Three" })]
        [InlineData("||", new string[] { })]
        public void GetMultiValue_Multivalue_ShouldReturnExpected(string value, string[] expected)
        {
            var actual = value.GetMultiValue();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("One|Two")]
        [InlineData("|One|Two|")]
        [InlineData("One||Two|Three")]
        [InlineData("One|  | Two|Three")]
        public void IsMultiValue_SeveralValues_ShouldReturnTrue(string value)
        {
            var actual = value.IsMultiValue();

            Assert.True(actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("||")]
        [InlineData("One")]
        [InlineData("One|")]
        public void IsMultiValue_Empty_NoValues_OneValue_ShouldReturnFalse(string value)
        {
            var actual = value.IsMultiValue();

            Assert.False(actual);
        }

        [Theory]
        [InlineData("hello this is a sentence", 0)]
        [InlineData(" hello this is a sentence", 1)]
        [InlineData("   hello this is a sentence", 3)]
        public void GetLeadingTrivia_ShouldCountInitialWhitespacesCorrectly(string value, int expected)
        {
            var actual = value.GetLeadingTrivia();

            Assert.Equal(expected, actual);
        }
    }
}
