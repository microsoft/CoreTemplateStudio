// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Xunit;

namespace Microsoft.Templates.Core.Test.Extensions
{
    [Trait("ExecutionSet", "Minimum")]
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("string")]
        [InlineData("string23TEST")]
        [InlineData(" string23TEST")]
        [InlineData("string23TEST ")]
        [InlineData("str ing23TEST")]
        public void Obfuscate_ShouldNotBeEmpty(string value)
        {
            var actual = value.Obfuscate();

            Assert.NotEmpty(actual);
        }

        [Theory]
        [InlineData("string")]
        [InlineData("string23TEST")]
        [InlineData(" string23TEST")]
        [InlineData("string23TEST ")]
        [InlineData("str ing23TEST")]
        public void Obfuscate_ShouldNotBeEqualsToTheValue(string value)
        {
            var actual = value.Obfuscate();

            Assert.False(value.Equals(actual, StringComparison.Ordinal));
        }

        [Theory]
        [InlineData("string")]
        [InlineData("string23TEST")]
        [InlineData(" string23TEST")]
        [InlineData("string23TEST ")]
        [InlineData("str ing23TEST")]
        public void Obfuscate_ShouldHaveBeenConvertedToUppercase(string value)
        {
            var actual = value.Obfuscate();

            Assert.DoesNotContain(actual, c => char.IsLower(c));
        }

        [Theory(Skip = "Control null and empty scenarios")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Obfuscate_Null_ShouldBeControlled(string value)
        {
            var actual = value.Obfuscate();

            // control these scenarios.
            // used from PackagePackage > VerifyAllowedPublicKey
            // used from PackagePackage > GetCertsInfo
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("||")]
        [InlineData("|")]
        public void GetMultiValue_NoValue_ShouldReturnEmptyArray(string value)
        {
            var expected = Array.Empty<string>();

            var actual = value.GetMultiValue();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("One|", new string[] { "One" })]
        [InlineData("One|Two", new string[] { "One", "Two" })]
        [InlineData("One Two|", new string[] { "One Two" })]
        [InlineData("One|Two Three", new string[] { "One", "Two Three" })]
        [InlineData("One Two|Three", new string[] { "One Two", "Three" })]
        [InlineData("One|Two||Three", new string[] { "One", "Two", "Three" })]
        public void GetMultiValue_HasValue_ShouldReturnExpected(string value, string[] expected)
        {
            var actual = value.GetMultiValue();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("| ")]
        [InlineData("One| ")]
        [InlineData("One |Two")]
        [InlineData(" One|Two")]
        [InlineData("  One|Two")]
        [InlineData("One  |Two")]
        [InlineData(" One Two|")]
        [InlineData("One Two |")]
        [InlineData("One|Two|| Three")]
        [InlineData("One|Two| |Three")]
        [InlineData("One|Two||Three ")]
        [InlineData("One| One Two||Three")]
        [InlineData("One|One Two ||Three")]
        [InlineData(" One Two|Three||Four")]
        [InlineData("One Two |Three||Four")]
        [InlineData("One|Two|| Three Four")]
        [InlineData("One|Two||Three Four ")]
        public void GetMultiValue_ValueContainsTrailingSpaces_ShouldThrowInvalidDataException(string value)
        {
            Assert.Throws<InvalidDataException>(() => { value.GetMultiValue(); });
        }


        [Theory]
        [InlineData("One|Two")]
        [InlineData("|One|Two|")]
        [InlineData("One||Two|Three")]
        [InlineData("One||Two Three|Four")]
        [InlineData("One||Two Three")]
        [InlineData("One Two||Two Three")]
        public void IsMultiValue_IfSeveralValues_ShouldReturnTrue(string value)
        {
            var actual = value.IsMultiValue();

            Assert.True(actual);
        }

        [Theory]
        [InlineData(" One")]
        [InlineData("One ")]
        [InlineData(" One|")]
        [InlineData("One |")]
        [InlineData("One| ")]
        [InlineData("One| Two")]
        [InlineData("One|Two ")]
        [InlineData(" |One|Two|")]
        [InlineData(" |One|Two Three|")]
        [InlineData(" |One|Two Three|Four")]
        [InlineData("|One|Two Three |Four")]
        [InlineData("|One|Two Three|Four ")]
        [InlineData("One|  | Two|Three")]
        public void IsMultiValue_ValueContainsTrailingSpaces_ShouldThrowInvalidDataException(string value)
        {
            Assert.Throws<InvalidDataException>(() => { value.IsMultiValue(); });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("||")]
        [InlineData("One")]
        [InlineData("One|")]
        public void IsMultiValue_SingleOrNoValue_ShouldReturnFalse(string value)
        {
            var actual = value.IsMultiValue();

            Assert.False(actual);
        }

        [Theory]
        [InlineData("hello this is a statement", 0)]
        [InlineData(" hello this is a statement", 1)]
        [InlineData("   hello this is a statement", 3)]
        public void GetLeadingTrivia_ShouldCountInitialWhitespacesCorrectly(string value, int expected)
        {
            var actual = value.GetLeadingTrivia();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("hello this is a statement", 0, "hello this is a statement")]
        [InlineData("hello this is a statement", 1, " hello this is a statement")]
        [InlineData("hello this is a statement", 3, "   hello this is a statement")]
        public void WithLeadingTrivia_Should(string value, int triviaCount, string expected)
        {
            var actual = value.WithLeadingTrivia(triviaCount);

            Assert.Equal(expected, actual);
        }
    }
}
